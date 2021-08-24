using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersPortAgentList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPortAgent.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$lnkDoubleClick");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersPortAgentList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPortAgent')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersPortAgentList.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersPortAgentList.aspx", "Clear Filter", "clear-filter.png", "RESET");
            toolbar.AddImageLink("javascript:Openpopup('codehelp1','','RegistersPortAgent.aspx')", "Add", "add.png", "ADDCOMPANY");
            MenuRegistersCompany.AccessRights = this.ViewState;
            MenuRegistersCompany.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    UcVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;              
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDVESSELNAME", "FLDADDRESSTYPENAME", "FLDCOUNTRYNAME", "FLDSTATE","FLDCITY","FLDEMAIL1" };
        string[] alCaptions = { "Name", "Vessel Name", "Address Type", "Country", "State", "City", "Email-1" };
        string sortexpression;
        int? sortdirection = null;
        
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strAgentSearch = (txtAgentName.Text == null) ? "" : txtAgentName.Text;

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersPortAgent.PortAgentSearch(
            txtAgentName.Text,
            General.GetNullableInteger(vesselid), 
            General.GetNullableInteger((ucCountry.SelectedCountry).ToString()), 
            null, null, null, 
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Address.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Address</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersCompany_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        txtAgentName.Text = "";
        UcVessel.SelectedVessel = "";
        ucCountry.SelectedCountry = "";
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string strAgentNameSearch = (txtAgentName.Text == null) ? "" : txtAgentName.Text;

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersPortAgent.PortAgentSearch(
            txtAgentName.Text,
            General.GetNullableInteger(vesselid),
            General.GetNullableInteger((ucCountry.SelectedCountry).ToString()),
            null, null, null,
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount );

        string[] alColumns = { "FLDNAME", "FLDVESSELNAME", "FLDADDRESSTYPENAME", "FLDCOUNTRYNAME", "FLDSTATE", "FLDCITY", "FLDEMAIL1" };
        string[] alCaptions = { "Name", "Vessel Name", "Address Type", "Country", "State", "City", "Email-1" };

        General.SetPrintOptions("gvPortAgent", "Address", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPortAgent.DataSource = ds;
            gvPortAgent.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPortAgent);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvPortAgent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvPortAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersPortAgent.DeletePortAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid   ((((Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode")).Text)));
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPortAgent_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvPortAgent_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvPortAgent_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }   

    protected void gvPortAgent_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            _gridView.Columns[1].Visible = false;
        }
       
        if (ViewState["SORTEXPRESSION"] != null)
        {
            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
            if (img != null)
            {
                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                    img.Src = Session["images"] + "/arrowUp.png";
                else
                    img.Src = Session["images"] + "/arrowDown.png";

                img.Visible = true;
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int nCount = 0; nCount < e.Row.Cells.Count; nCount++)
                e.Row.Cells[nCount].Attributes.Add("columnname", "colname" + nCount);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (!SessionUtil.CanAccess(this.ViewState, del.CommandName)) del.Visible = false;
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false; 

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");                    
                }

                Label l = (Label)e.Row.FindControl("lblAddressCode");

                LinkButton lb = (LinkButton)e.Row.FindControl("lnkAddressCode");
                lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersPortAgent.aspx?AddressCode=" + l.Text + "');return false;");

                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
                lb.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lb.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");               

                ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
                db1.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersPortAgent.aspx?AddressCode=" + l.Text + "');return false;");

            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvPortAgent.SelectedIndex = -1;
        gvPortAgent.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
