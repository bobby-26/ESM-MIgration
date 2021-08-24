using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Web;

public partial class VesselPositionEUMRVProcedureDetailHistoryList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvProcedure.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvProcedure.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Procedure", "PROCEDURE");
            toolbarmain.AddButton("Company Procedure", "PROCEDUREDETAIL");
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();
            //MenuProcedureDetailList.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVProcedureDetailHistoryList.aspx?DetailID=" + Request.QueryString["DetailID"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvProcedure')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVProcedureDetail.aspx", "Add", "add.png", "Add");
            
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

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

    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
           
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
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailList.aspx");
        }     
    }
    private bool IsValidProcedure(string Procedure)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((Procedure.Trim() == null) || (Procedure.Trim() == ""))
            ucError.ErrorMessage = "Procudure is required.";

        return (!ucError.IsError);
    }

    protected void gvProcedure_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMRV.DeleteEUMRVProcedureDetail(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcedureIdadd")).Text));
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
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
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                }
                ImageButton cmdView = (ImageButton)e.Row.FindControl("cmdView");
                if (cmdView != null)
                {
                        cmdView.Visible = SessionUtil.CanAccess(this.ViewState, cmdView.CommandName);
                        cmdView.Attributes.Add("onclick"
                        , "parent.Openpopup('codehelp1', '', '../VesselPosition/VesselPositionEUMRVProcedureDetailView.aspx?DetailID="
                        + drv["FLDPROCEDUREDETAILID"].ToString() + "&lanchfrom=History');return false;");
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

    protected void gvProcedure_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvProcedure_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvProcedure, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvProcedure_RowEditing(object sender, GridViewEditEventArgs de)
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

    protected void gvProcedure_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
       
    }

    protected void gvProcedure_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvProcedure_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvProcedure.EditIndex = -1;
        gvProcedure.SelectedIndex = -1;
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
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
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
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
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
        gvProcedure.SelectedIndex = -1;
        gvProcedure.EditIndex = -1;
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDUREGUIDANCE", "FLDVERSION" };
        string[] alCaptions = { "Table", "Procedure Name", "Version" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionEUMRV.EUMRVProcedureDetailHistorySearch(
            "", sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(Request.QueryString["DetailID"].ToString()));

        General.SetPrintOptions("gvProcedure", "Company Procedure History", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvProcedure.DataSource = ds;
            gvProcedure.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvProcedure);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDUREGUIDANCE", "FLDVERSION" };
        string[] alCaptions = { "Table", "Procedure Name", "Version" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixVesselPositionEUMRV.EUMRVProcedureDetailHistorySearch(
            "", sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(Request.QueryString["DetailID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=\"CompanyProcedureHistory.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Company Procedure History</h3></td>");
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
