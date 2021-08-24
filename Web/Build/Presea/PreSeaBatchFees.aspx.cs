using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchFees : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaBatchFees.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaFees')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../PreSea/PreSeaBatchFees.aspx", "Find", "search.png", "FIND");
                MenuPreSeaFees.AccessRights = this.ViewState;
                MenuPreSeaFees.MenuList = toolbar.Show();

                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch Contact", "CONTACT");
                Maintoolbar.AddButton("Fees", "FEES");
                Maintoolbar.AddButton("Semester", "SEMESTER");
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");
                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();


                MenuCourseMaster.SelectedMenuIndex = 4;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;


                ucBatch.SelectedBatch = Filter.CurrentPreSeaCourseMasterBatchSelection;
                ucBatch.Enabled = false;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {  "FLDFEESNAME", "FLDAMOUNT", "FLDQUICKNAME" };
        string[] alCaptions = {  "Fees Name", "Amount", "Pay term" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaBatchFees.PreSeaBatchFeesSearch(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection) 
            , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection)
            , txtFeesName.Text
            , General.GetNullableInteger(ucPayterm.SelectedQuick)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=BatchFeesDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Batch Fees</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPRESEABATCHNAME", "FLDFEESNAME", "FLDAMOUNT", "FLDPAYTERM" };
        string[] alCaptions = { "Batch Name", "Fees Name", "Amount", "Pay term" };

        DataSet ds = new DataSet();

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixPreSeaBatchFees.PreSeaBatchFeesSearch(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
            , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection), txtFeesName.Text
            , General.GetNullableInteger(ucPayterm.SelectedQuick)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        General.SetPrintOptions("gvPreSeaFees", "Pre Sea Batch Fees", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaFees.DataSource = ds;
            gvPreSeaFees.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaFees);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            {
                Response.Redirect("../PreSea/PreSeaBatchEligiblityDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaBatchFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionExam.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void PreSeaFees_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaFees.EditIndex = -1;
                gvPreSeaFees.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
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

    private void InsertPreSeaBatchFees(int feesid,int CourseId, int Batchid, decimal? amount, int? payterm, decimal? waiveramount)
    {
        PhoenixPreSeaBatchFees.InsertPreSeaBatchFees(PhoenixSecurityContext.CurrentSecurityContext.UserCode, feesid,CourseId, Batchid, amount, payterm, waiveramount);
    }

    private void UpdatePreSeaBatchFees(int Batchfeeid, int feesid,int CourseId, int Batchid, decimal? amount, int? payterm, decimal? waiveramount)
    {
        PhoenixPreSeaBatchFees.UpdatePreSeaBatchFees(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Batchfeeid, feesid,CourseId, Batchid, amount, payterm, waiveramount);
    }

    private void deletePreSeaFees(int Batchfeeid)
    {
        PhoenixPreSeaBatchFees.DeletePreSeaBatchFees(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Batchfeeid);
    }

    private bool IsValidPreSeaBatchfees(int? feesid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!feesid.HasValue)
        {
            ucError.ErrorMessage = "Fees name is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaFees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPreSeaFees_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaBatchfees(General.GetNullableInteger((((UserControlPreSeaFees)_gridView.FooterRow.FindControl("ddlFeesAdd")).SelectedFees))))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaBatchFees(
                     int.Parse(((UserControlPreSeaFees)_gridView.FooterRow.FindControl("ddlFeesAdd")).SelectedFees),
                     int.Parse(Filter.CurrentPreSeaCourseMasterSelection),
                     int.Parse(ucBatch.SelectedBatch),
                     General.GetNullableDecimal(((UserControlNumber)_gridView.FooterRow.FindControl("ucAmountAdd")).Text),
                     General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucPaytermAdd")).SelectedQuick),
                     General.GetNullableDecimal(((UserControlNumber)_gridView.FooterRow.FindControl("ucWaiverAmountAdd")).Text)
                 );
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaBatchfees(General.GetNullableInteger((((UserControlPreSeaFees)_gridView.Rows[nCurrentRow].FindControl("ddlFeesEdit")).SelectedFees))))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaBatchFees(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("LblBatchFeesIdEdit")).Text),
                    int.Parse(((UserControlPreSeaFees)_gridView.Rows[nCurrentRow].FindControl("ddlFeesEdit")).SelectedFees),
                    int.Parse(Filter.CurrentPreSeaCourseMasterSelection),
                    int.Parse(ucBatch.SelectedBatch),
                    General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucAmountEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucPaytermEdit")).SelectedQuick),
                    General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucWaiverAmountEdit")).Text)
                 );
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deletePreSeaFees(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("LblBatchFeesId")).Text)
                    );
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

    protected void gvPreSeaFees_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

            UserControlPreSeaFees ddlFeesEdit = (UserControlPreSeaFees)e.Row.FindControl("ddlFeesEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ddlFeesEdit != null)
            {
                ddlFeesEdit.SelectedFees = drv["FLDFEESID"].ToString();
            }

            UserControlQuick ucQuickEdit = (UserControlQuick)e.Row.FindControl("ucPaytermEdit");
            if (ucQuickEdit != null) ucQuickEdit.SelectedQuick = drv["FLDQUICKCODE"].ToString();

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

    protected void gvPreSeaFees_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvPreSeaFees_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvPreSeaFees_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaFees.EditIndex = -1;
        gvPreSeaFees.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaFees.EditIndex = -1;
        gvPreSeaFees.SelectedIndex = -1;
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
        gvPreSeaFees.SelectedIndex = -1;
        gvPreSeaFees.EditIndex = -1;
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
}
