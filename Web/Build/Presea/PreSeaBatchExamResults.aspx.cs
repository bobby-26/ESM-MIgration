using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaBatchExamResults : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["STUDENTID"] = "";
                ViewState["EXAMSCHEDULEID"] = "";
                ViewState["RESULTDETAILID"] = "";
                ViewState["PRESEACOURSEID"] = "";
                ViewState["SECTIONID"] = "";

                ViewState["EXAMNAME"] = "";
                ViewState["BATCHID"] = "";
                ViewState["ADDYN"] = "";

                if (Request.QueryString["examscheduleid"] != null)
                    ViewState["EXAMSCHEDULEID"] = Request.QueryString["examscheduleid"].ToString();

                if (Request.QueryString["resultdetailid"] != null)
                {
                    ViewState["RESULTDETAILID"] = Request.QueryString["resultdetailid"].ToString();
                    btnShowTrainee.Visible = false;
                }

                if (Request.QueryString["studentid"] != null)
                {
                    ViewState["STUDENTID"] = Request.QueryString["studentid"].ToString();
                    txtStudentId.Text = ViewState["STUDENTID"].ToString();
                }
                if (Request.QueryString["batchid"] != null)
                {
                    ViewState["BATCHID"] = Request.QueryString["batchid"].ToString();
                }
                SessionUtil.PageAccessRights(this.ViewState);
                cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
                txtStudentId.Attributes.Add("style", "visibility:hidden");

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbarmain.AddButton("Back", "BACK");
                toolbargrid.AddImageButton("../PreSea/PreSeaBatchExamResults.aspx", "Export to Excel", "icon_xls.png", "Excel");
                //toolbargrid.AddImageLink("javascript:CallPrint('gvExamResult')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../PreSea/PreSeaBatchExamResults.aspx", "Filter", "search.png", "FIND");

                MenuBatchExam.AccessRights = this.ViewState;
                MenuBatchExam.MenuList = toolbarmain.Show();

                ucTitle.Text = "Exam Results";

                DataTable dt = PhoenixPreSeaBatchExamSchedule.ListPreSeaBatchExamSchedule(General.GetNullableInteger(ViewState["BATCHID"].ToString())
                    , null
                    , null
                    , null
                    , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString()));

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    ucBatch.SelectedBatch = dr["FLDBATCHID"].ToString();
                    ucSemester.Batch = dr["FLDBATCHID"].ToString();
                    ucSemester.SelectedSemester = dr["FLDSEMESTERID"].ToString();
                    ucSemester.bind();
                   

                    //ucBatchExam.Enabled = true;
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt.Copy());

                    ucBatchExam.BatchExamList = ds;
                    ucBatchExam.DataBind();

                    ucBatchExam.SelectedBatchExam = dr["FLDEXAMSCHEDULEID"].ToString();
                    ViewState["SECTIONID"] = dr["FLDSECTIONID"].ToString();
                }

                MenuPreSeaExamResult.MenuList = toolbargrid.Show();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBatchExam_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("PreSeaBatchExamResultDetails.aspx", false);
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
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaExam.EditIndex = -1;
                gvPreSeaExam.SelectedIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindData()
    {

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBJECTNAME", "FLDSUBJECTCODE", "FLDTYPENAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDMARKOBTAINED", "FLDEXAMDATE", "FLDPASSFAILNAME", "FLDABSENTYNNAME" };
        string[] alCaptions = { "Subject Name", "Subject Code", "Type", "Max. Marks", "Pass Marks", "Marks Obtained", "Exam date", "Result", "IsAbsent" };

        try
        {
            ds = PhoenixPreSeaBatchExamResults.PreSeaBatchExamResultsList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
                , General.GetNullableGuid(ViewState["RESULTDETAILID"].ToString())
                , General.GetNullableInteger(txtStudentId.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                gvPreSeaExam.Visible = true;
                gvPreSeaExam.DataSource = ds.Tables[0];
                gvPreSeaExam.DataBind();

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];

                    txtRollNo.Text = dr["FLDBATCHROLLNUMBER"].ToString();
                    txtName.Text = dr["FLDTRAINEENAME"].ToString();
                    txtStudentId.Text = dr["FLDSTUDENTID"].ToString();
                }
            }
            else
            {
                gvPreSeaExam.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        //General.SetPrintOptions("gvPreSeaExam", "Presea Batch Exam Results", alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBJECTNAME", "FLDSUBJECTCODE", "FLDTYPENAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDMARKOBTAINED", "FLDEXAMDATE", "FLDPASSFAILNAME", "FLDABSENTYNNAME" };
        string[] alCaptions = { "Subject Name", "Subject Code", "Type", "Max. Marks", "Pass Marks", "Marks Obtained", "Exam date", "Result", "IsAbsent" };

        string[] FilterColumns = { "FLDBATCHNAME", "FLDBATCHROLLNUMBER", "FLDTRAINEENAME", "FLDSEMESTERNAME", "FLDBATCHEXAMNAME" };
        string[] FilterCaptions = { "Batch Name", "Batch RollNo.", "Trainee Name", "Semester", "Exam" };

        ds = PhoenixPreSeaBatchExamResults.PreSeaBatchExamResultsList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
               , General.GetNullableGuid(ViewState["RESULTDETAILID"].ToString())
               , General.GetNullableInteger(txtStudentId.Text));

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSea Exam Results.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PreSea Exam Results </h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton bh = (ImageButton)e.Row.FindControl("cmdHistory");
            if (drv["FLDISSEMESTEREXAM"].ToString() == "1")
            {
                if (bh != null)
                {
                    bh.Visible = true;
                    bh.Attributes.Add("onclick", "javascript:return Openpopup('PreSea','','PreSeaBatchExamResultsHistory.aspx?traineeexamid=" + drv["FLDTRAINEEEXAMID"].ToString() + "'); return false;");
                }
            }
        }
    }
    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView gvr = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string traineeexamid = ((Label)gvr.Rows[nCurrentRow].FindControl("lblTraineeExamIdEdit")).Text;
                string resultdetailid = ((Label)gvr.Rows[nCurrentRow].FindControl("lblResultDetailId")).Text;
                string scheduleid = ((Label)gvr.Rows[nCurrentRow].FindControl("lblExamScheduleIdEdit")).Text;
                string subjectid = ((Label)gvr.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;

                string marks = ((UserControlNumber)gvr.Rows[nCurrentRow].FindControl("txtMarkObtainedEdit")).Text;
                string absentyn = ((CheckBox)gvr.Rows[nCurrentRow].FindControl("chkAbsentYNEdit")).Checked == true ? "1" : "0";
                string passmarks = ((Label)gvr.Rows[nCurrentRow].FindControl("lblPassMark")).Text;
                string maxmarks = ((Label)gvr.Rows[nCurrentRow].FindControl("lblMaxMark")).Text;
                string examdate = ((UserControlDate)gvr.Rows[nCurrentRow].FindControl("txtExamDateEdit")).Text;

                if (!IsValidExamResult(maxmarks, passmarks, examdate, absentyn, marks))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixPreSeaBatchExamResults.UpdatePreSeaBatchExamResults(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(scheduleid)
                        , General.GetNullableGuid(traineeexamid)
                        , General.GetNullableGuid(resultdetailid)
                        , int.Parse(txtStudentId.Text)
                        , int.Parse(subjectid)
                        , General.GetNullableDateTime(examdate)
                        , General.GetNullableDecimal(marks)
                        , int.Parse(absentyn) != 2 ? General.GetNullableInteger(absentyn) : null
                        );

                    ucStatus.Visible = true;
                    ucStatus.Text = "Marks Updated";
                    gvr.EditIndex = -1;
                    BindData();
                }
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

    private void UpdatePreSeaExamScheduleSubjects(Guid? examsubjectid, decimal? maxmarks, decimal? passmarks)
    {
        PhoenixPreSeaBatchExamSchedule.UpdatePreSeaBatchExamScheduleSubjects(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , examsubjectid
            , maxmarks
            , passmarks
            );
    }
    private void DeletePreSeaExamScheduleSubjects(Guid? examsubjectid)
    {
        //PhoenixPreSeaBatchExamSchedule.DeletePreSeaBatchExamScheduleSubjects(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //    , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
        //    , examsubjectid
        //    );
    }

    private bool IsValidExamResult(string maxmarks, string passmarks, string examdate, string absentyn, string marks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(txtStudentId.Text) == null)
            ucError.ErrorMessage = "Trainee Required";
        if (General.GetNullableDateTime(examdate) == null)
            ucError.ErrorMessage = "Exam date is required.";
        if (General.GetNullableInteger(absentyn) != 1)
        {
            if (General.GetNullableDecimal(marks) == null)
                ucError.ErrorMessage = "Mark is required.";
            else if (General.GetNullableDecimal(marks) > General.GetNullableDecimal(maxmarks))
                ucError.ErrorMessage = "marks is should be below or equal max marks.";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
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

