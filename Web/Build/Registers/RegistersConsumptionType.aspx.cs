using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersConsumptionType : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvConsumptionType.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvConsumptionType.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersConsumptionType.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvConsumptionType')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersConsumptionType.AccessRights = this.ViewState;
            MenuRegistersConsumptionType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindData();
               
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersConsumptionType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvConsumptionType.EditIndex = -1;
                gvConsumptionType.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumptionType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                    if (!checkvalue((((TextBox)_gridView.FooterRow.FindControl("txtConsumptionTypeNameAdd")).Text),((TextBox)_gridView.FooterRow.FindControl("txtConsumptionShortNameAdd")).Text))
                        return;

                   PhoenixRegistersConsumptionType.InsertConsumptionType((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                                (((TextBox)_gridView.FooterRow.FindControl("txtConsumptionTypeNameAdd")).Text),
                                                                (((TextBox)_gridView.FooterRow.FindControl("txtConsumptionShortNameAdd")).Text)                                    
                                                                );    
                   BindData();
               
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersConsumptionType.DeleteConsumptionType((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblConsumptionTypeCode")).Text));
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool checkvalue(string type,string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((type == null) || (type == ""))
            ucError.ErrorMessage = "Consumption Type is required.";

        if ((type == null) || (type == ""))
            ucError.ErrorMessage = "Consumption Shortname is required.";

        if(ucError.ErrorMessage!="")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTNAME", "FLDCONSUMPTIONTYPENAME" };
        string[] alCaptions = { "Shortname", "Consumption Type" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersConsumptionType.ConsumptionTypeSearch("","",sortexpression,sortdirection, (int)ViewState["PAGENUMBER"],
                                                    General.ShowRecords(null),ref iRowCount,ref iTotalPageCount
                                                   );

        General.SetPrintOptions("gvConsumptionType", "ConsumptionType Registers", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvConsumptionType.DataSource = ds;
            gvConsumptionType.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvConsumptionType);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvConsumptionType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

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
            
                Label lb = (Label)e.Row.FindControl("lblShowYN");
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (lb != null)
                    lb.Text = drv["FLDSHOWYESNO"].ToString().Equals("1") ? "Yes" : "No";

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    CheckBox cb = (CheckBox)e.Row.FindControl("chkShowYN");                    
                    if (cb != null)
                        cb.Checked = drv["FLDSHOWYESNO"].ToString().Equals("1") ? true : false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvConsumptionType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvConsumptionType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if(e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvConsumptionType, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    protected void gvConsumptionType_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {     
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumptionType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
           
            PhoenixRegistersConsumptionType.UpdateConsumptionType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                   General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblConsumptionTypeCodeEdit")).Text),
                                                   General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtConsumptionTypeNameEdit")).Text),
                                                   (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtConsumptionShortNameEdit")).Text));   
                                                
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumptionType_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvConsumptionType_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvConsumptionType.EditIndex = -1;
        gvConsumptionType.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvConsumptionType.SelectedIndex = -1;
        gvConsumptionType.EditIndex = -1;
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
        gvConsumptionType.SelectedIndex = -1;
        gvConsumptionType.EditIndex = -1;
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
        {
            return true;
        }

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

    protected void cmdSearch_Click(object sender, EventArgs e)
    { 
        gvConsumptionType.SelectedIndex = -1;
        gvConsumptionType.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
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
        DataSet ds = new DataSet();
      
        string[] alColumns = { "FLDSHORTNAME", "FLDCONSUMPTIONTYPENAME" };
        string[] alCaptions = { "Shortname", "Consumption Type" };        

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersConsumptionType.ConsumptionTypeSearch("", "", sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                    General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                                                   );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ConsumptionType.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ConsumptionType Registers</h3></td>");
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
}
