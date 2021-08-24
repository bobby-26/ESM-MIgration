using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class AdministrationITSupportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageLink("javascript:Openpopup('code1','','AdministrationITSupportAdd.aspx'); return false;", "Add", "add.png", "ADDDEFECT");
            toolbarbuglist.AddImageButton("../Administration/AdministrationITSupportList.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../Administration/AdministrationITSupportList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../Administration/AdministrationITSupportList.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuITSupportTracker.AccessRights = this.ViewState;
            MenuITSupportTracker.MenuList = toolbarbuglist.Show();



            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

    protected void cmdThisUser_Click(object sender, EventArgs e)
    {
        txtLoggedBySearch.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString();
        BindData();
    }

   

    protected void MenuITSupportTracker_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        try
        {            
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        ucSUPDepartment.SelectedValue = "";
        ucITCategory.SelectedValue = "";
        ucITStatus.SelectedValue = "";
        txtIDSearch.Text = "";
        txtCallTypeSearch.Text = "";
        ucFromDate.Text = "";
        ucToDate.Text = "";
        txtnopage.Text = "";
        txtActionTakenSearch.Text = "";
        txtSystemNameSearch.Text = "";
        ddlITTeam.SelectedIndex = -1;
        txtLoggedBySearch.Text = "";
        ViewState["PAGENUMBER"] = 1;       
        BindData();
        SetPageNavigator();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        BindData();
        SetPageNavigator();        
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUGID", "FLDDATE", "FLDLOGGEDBY", "FLDSYSTEMNAME", "FLDDEPARTMENTNAME", "FLDNAME", "FLDCALLTYPE", "FLDACTIONTAKEN", "FLDSTATUSNAME", "FLDCLOSEDON", "FLDREMARKS" };
        string[] alCaptions = { "S.No", "Call Rec.Date", "Request User Name", "System Name", "Department", "Category", "Call Type", "Action Taken", "Status", "Closed On", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
      
        DataSet ds = PhoenixAdministrationITSupport.SearchITSupportList  (
            General.GetNullableInteger(txtIDSearch .Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            General.GetNullableString(txtLoggedBySearch.Text),
            General.GetNullableString(txtSystemNameSearch.Text ),
            General.GetNullableInteger(ddlITTeam.SelectedITTeam),
            General.GetNullableString(ucSUPDepartment .SelectedValue),
            General.GetNullableString(ucITCategory.SelectedValue),
            General.GetNullableString(ucITStatus.SelectedValue),
            General.GetNullableString(txtCallTypeSearch.Text),
            General.GetNullableString(txtActionTakenSearch.Text ),
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ITSupportSearchGrid.DataSource = ds;
            ITSupportSearchGrid.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, ITSupportSearchGrid);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    private bool IsVaildDate(string date)
    {
        if (Convert.ToDateTime(date)!=null && Convert.ToDateTime(date) > DateTime.Now)
        {
           // ucError.ErrorMessage = "Logged From date can't be greater than Today";
           // return (!ucError.IsError);
        }
        return (ucError.IsError);
    }
    private bool IsValidToDate(DateTime date)
    {
        if (date != null && date > DateTime.Now)
        {
            ucError.ErrorMessage = "Logged To Date can't be greater than Today.";
            //return (!ucError.IsError);
        }
        return (ucError.IsError);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string[] alColumns = { "FLDBUGID", "FLDDATE", "FLDLOGGEDBY", "FLDSYSTEMNAME", "FLDDEPARTMENTNAME", "FLDNAME", "FLDCALLTYPE", "FLDACTIONTAKEN", "FLDSTATUSNAME", "FLDCLOSEDON", "FLDREMARKS" };
        string[] alCaptions = { "S.No", "Call Rec.Date", "Request User Name", "System Name", "Department", "Category", "Call Type", "Action Taken", "Status", "Closed On", "Remarks" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAdministrationITSupport.SearchITSupportList(
            General.GetNullableInteger(txtIDSearch.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            General.GetNullableString(txtLoggedBySearch.Text),
            General.GetNullableString(txtSystemNameSearch.Text),  
            General.GetNullableInteger(ddlITTeam.SelectedITTeam),                           
            General.GetNullableString(ucSUPDepartment.SelectedValue),
            General.GetNullableString(ucITCategory.SelectedValue),
            General.GetNullableString(ucITStatus.SelectedValue),
            General.GetNullableString(txtCallTypeSearch.Text),
            General.GetNullableString(txtActionTakenSearch.Text),
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=ITSupportList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ITSupport List</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void ITSupportSearchGrid_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
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
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {     
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AdministrationITSupportEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }
          
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkCallType");
            if (lb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AdministrationITSupportEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkBugId");
            if (lbE != null)
            {
                Label lblE = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AdministrationITSupportEdit.aspx?dtkey=" + lblE.Text + "'); return false;");
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            ImageButton cd = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cd != null) cd.Visible = SessionUtil.CanAccess(this.ViewState, cd.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucCallType");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

            Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
            UserControlToolTip ucRemarkstooltip = (UserControlToolTip)e.Row.FindControl("ucRemarksTooltip");
            lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucRemarkstooltip.ToolTip + "', 'visible');");
            lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucRemarkstooltip.ToolTip + "', 'hidden');");

            DataRowView drv = (DataRowView)e.Row.DataItem;
           
        }

    }
    protected void ITSupportSearchGrid_Sorting(object sender, GridViewSortEventArgs se)
    {
        ITSupportSearchGrid.EditIndex = -1;
        ITSupportSearchGrid.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        ITSupportSearchGrid.EditIndex = -1;
        ITSupportSearchGrid.SelectedIndex = -1;
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
        ITSupportSearchGrid.SelectedIndex = -1;
        ITSupportSearchGrid.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;        

        BindData();
        SetPageNavigator();
    }
    protected void ITSupportSearchGrid_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void ITSupportSearchGrid_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        //PhoenixAdminITSupport.DeleteITSupport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(lbldtkey.Text));
        BindData();
        SetPageNavigator();
    }
   
    protected void ITSupportSearchGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Label lbldtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID");
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAdministrationITSupport.DeleteITSupport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(lbldtkey .Text ));
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
