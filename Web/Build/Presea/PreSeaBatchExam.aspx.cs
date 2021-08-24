using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchExam : PhoenixBasePage
{
    string batchid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            batchid = Filter.CurrentPreSeaBatchManagerSelection;
            if (!IsPostBack)
            {

                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaBatchExam.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
                MenuPreSeaExam.AccessRights = this.ViewState;
                MenuPreSeaExam.MenuList = toolbar.Show();


                PhoenixToolbar MainToolbar = new PhoenixToolbar();
                PhoenixToolbar ToolbarSub = new PhoenixToolbar();

                MainToolbar.AddButton("Batch", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
                MainToolbar.AddButton("Events", "EVENT");
                MainToolbar.AddButton("Semester", "SEMESTER");
                MainToolbar.AddButton("Subjects", "SUBJECTS");
                MainToolbar.AddButton("Exam", "EXAM");
                MainToolbar.AddButton("Contact", "CONTACT");

                MenuBatchMaster.AccessRights = this.ViewState;
                MenuBatchMaster.MenuList = MainToolbar.Show();

                MenuBatchMaster.SelectedMenuIndex = 5;

                ToolbarSub.AddButton("Exam Details", "EXAMDETAILS");
                ToolbarSub.AddButton("Back", "BACK");
                MenuPreSeaExamSub.AccessRights = this.ViewState;
                MenuPreSeaExamSub.MenuList = ToolbarSub.Show();
                MenuPreSeaExamSub.SelectedMenuIndex = 0;

                ViewState["EXAMSCHEDULEID"] = "";
                ViewState["SEMESTERID"] = "";

                if (Request.QueryString["examscheduleid"] != null)
                    ViewState["EXAMSCHEDULEID"] = Request.QueryString["examscheduleid"].ToString();

                if (Request.QueryString["semesterid"] != null)
                    ViewState["SEMESTERID"] = Request.QueryString["semesterid"].ToString();

                if (Request.QueryString["examname"] != null)
                    TitleSub.Text = Request.QueryString["examname"].ToString();

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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEXAMNAME", "FLDSEMESTERNAME", "FLDEXAMDATE", "FLDSTARTTIME", "FLDENDTIME", "FLDSUBJECTNAME", "FLDSUBTYPENAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDINVIGILATORSNAME", "FLDACTIVEYNNAME" };
        string[] alCaptions = { "Exam Name", "Semester", "Exam Date", "Start Time", "Enad Time", "Subject", "Type", "Max. marks", "Pass Marks","Invigilators", "Active YN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaBatchExam.PreSeaBatchExamSearch(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            , null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaExam.DataSource = ds;
            gvPreSeaExam.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaExam);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvPreSeaExam", "Presea Batch Exam", alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();


        string[] alColumns = { "FLDEXAMNAME", "FLDSEMESTERNAME", "FLDEXAMDATE", "FLDSTARTTIME", "FLDENDTIME", "FLDSUBJECTNAME", "FLDSUBTYPENAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDINVIGILATORSNAME", "FLDACTIVEYNNAME" };
        string[] alCaptions = { "Exam Name", "Semester", "Exam Date", "Start Time", "Enad Time", "Subject", "Type", "Max. marks", "Pass Marks", "Invigilators", "Active YN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaBatchExam.PreSeaBatchExamSearch(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            , null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );


        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaBatchExam.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Batch Exam</h3></td>");
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

    protected void BatchMaster_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManageDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EVENT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchEvents.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaBatchExam.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchContact.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PreSeaExam_TabStripCommand(object sender, EventArgs e)
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

    protected void MenuPreSeaExam_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx?examscheduleid=" + ViewState["EXAMSCHEDULEID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton bi = (ImageButton)e.Row.FindControl("cmdInvigilator");
            if (bi != null) bi.Visible = SessionUtil.CanAccess(this.ViewState, bi.CommandName);

            if (bi != null)
            {
                bi.Attributes.Add("onclick", "javascript:return Openpopup('PreSea','','PreSeaBatchExamInvigilator.aspx?batchexamid=" + drv["FLDBATCHEXAMID"].ToString() + "'); return false;");
            }

            HtmlImage img = (HtmlImage)e.Row.FindControl("imgInvList");
            img.Attributes.Add("onclick", "showMoreInformation(ev, 'PreSeaMoreInfoExamInvigilator.aspx?batchexamid=" + drv["FLDBATCHEXAMID"].ToString() + "')");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlBatchSubjectAdd");

            if (ddlSubject != null)
            {

                DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                    , General.GetNullableInteger(ViewState["SEMESTERID"].ToString()));

                ddlSubject.DataSource = ds;
                ddlSubject.DataBind();
            }
        }
    }    

    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string examdate = ((UserControlDate)_gridView.FooterRow.FindControl("txtExamDateAdd")).Text;
                string starttime = ((TextBox)_gridView.FooterRow.FindControl("txtStartTimeAdd")).Text;
                string endtime = ((TextBox)_gridView.FooterRow.FindControl("txtEndTimeAdd")).Text;
                string subjectid = ((DropDownList)_gridView.FooterRow.FindControl("ddlBatchSubjectAdd")).SelectedValue;
                string maxmark = ((UserControlNumber)_gridView.FooterRow.FindControl("txtMaxMarkAdd")).Text;
                string passmark = ((UserControlNumber)_gridView.FooterRow.FindControl("txtPassMarkAdd")).Text;
                string activeyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAciveAdd")).Checked ? "1" : "0");


                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;


                if (!IsValidPreSeaBatchExam(examdate
                    , examstarttime
                    , examendtime
                    , subjectid
                    , maxmark
                    , passmark
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaExam(new Guid(ViewState["EXAMSCHEDULEID"].ToString())
                , int.Parse(subjectid)                
                , decimal.Parse(maxmark)
                , decimal.Parse(passmark)
                , int.Parse(activeyn)
                , DateTime.Parse(examdate)
                , DateTime.Parse(examdate + " " + starttime)
                , DateTime.Parse(examdate + " " + examendtime));                

                BindData();
                SetPageNavigator();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                
                string batchexamid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchExamIdEdit")).Text;
                string examdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExamDateEdit")).Text;
                string starttime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text;
                string endtime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text;
                string subjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                string maxmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text;
                string passmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text;         
                string activeyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAciveEdit")).Checked ? "1" : "0");

                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;

                if (!IsValidPreSeaBatchExam(examdate
                  , examstarttime
                  , examendtime
                  , subjectid
                  , maxmark
                  , passmark
                  ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaExam(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
                    , int.Parse(batchexamid)
                    , decimal.Parse(maxmark)
                    , decimal.Parse(passmark)
                    , int.Parse(activeyn)
                    , DateTime.Parse(examdate)
                    , DateTime.Parse(examdate + " " + starttime)
                    , DateTime.Parse(examdate + " " + examendtime));

                _gridView.EditIndex = -1;
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaExam(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchExamId")).Text));
                _gridView.EditIndex = -1;
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

    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvPreSeaExam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    private bool IsValidPreSeaBatchExam(string examdate, string starttime, string endtime, string sujectid, string maxmark, string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(examdate) == null)
            ucError.ErrorMessage = "Exam date is required.";
        if (General.GetNullableString(starttime) == null)
            ucError.ErrorMessage = "Start time is required";
        else if (General.GetNullableString(endtime) == null)
            ucError.ErrorMessage = "End time is required";
        else
        {
            DateTime resultdate;
            if (General.GetNullableDateTime(examdate + " " + starttime) == null)
                ucError.ErrorMessage = "Start time is not valid time.";
            else if (General.GetNullableDateTime(examdate + " " + starttime) == null)
                ucError.ErrorMessage = "Start time is not valid time.";
            if (DateTime.TryParse(examdate + " " + starttime, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(examdate + " " + endtime)) > 0)
                ucError.ErrorMessage = "Start time should be earlier than end time";
        }
        if (General.GetNullableInteger(sujectid) == null)
        {
            ucError.ErrorMessage = "Subject is required.";
        }
        if (General.GetNullableDecimal(maxmark) == null)
        {
            ucError.ErrorMessage = "Max mark is required.";
        }
        if (General.GetNullableDecimal(passmark) == null)
        {
            ucError.ErrorMessage = "Pass mark is required.";
        }
        else if (General.GetNullableDecimal(maxmark) < General.GetNullableDecimal(passmark))
        {
            ucError.ErrorMessage = "Pass mark should be less than or equal to max mark.";
        }

        return (!ucError.IsError);
    }

    private void InsertPreSeaExam(Guid scheduleid, int subjectid, decimal maxmarks, decimal passmarks, int activeyn, DateTime examdate, DateTime starttime, DateTime endtime)
    {
        PhoenixPreSeaBatchExam.InsertPreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , scheduleid
            , subjectid
            , null
            , maxmarks
            , passmarks
            , activeyn
            , examdate
            , starttime
            , endtime
            );
    }

    private void UpdatePreSeaExam(Guid? scheduleid, int batchexamid, decimal maxmarks, decimal passmarks, int activeyn, DateTime examdate, DateTime starttime, DateTime endtime)
    {
        PhoenixPreSeaBatchExam.UpdatePreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
          , scheduleid
          , batchexamid
          , maxmarks
          , passmarks
          , activeyn
          , examdate
          , starttime
          , endtime
          );
    }

    private void DeletePreSeaExam(int batchidexamid)
    {
        PhoenixPreSeaBatchExam.DeletePreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode, batchidexamid);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaExam.EditIndex = -1;
        gvPreSeaExam.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaExam.EditIndex = -1;
        gvPreSeaExam.SelectedIndex = -1;
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
        gvPreSeaExam.SelectedIndex = -1;
        gvPreSeaExam.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
}

