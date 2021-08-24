using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaExaminationResults : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Report", "REPORT");
        MenuPreSea.MenuList = toolbar.Show();
        try
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();            
            toolbarsub.AddImageLink("../PreSea/PreSeaExaminationResults.aspx", "Filter", "search.png", "FIND");
            toolbarsub.AddImageButton("../PreSea/PreSeaExaminationResults.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            PreExamResultsMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COURSEID"] = "";
                ViewState["BATCHID"] = "";
                ViewState["SECTIONID"] = "";
                BindStudentDetails();
                SetPreSeaNewApplicantPrimaryDetails();
            }
            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindStudentDetails()
    {
        DataTable dt = new DataTable();

        dt = PhoenixPreSeaExamResults.TraineeDetails(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));

        if (dt.Rows.Count > 0)
        {
            //ucBatch.SelectedBatch = dt.Rows[0]["FLDBATCHID"].ToString();
            hdnBatchId.Value= dt.Rows[0]["FLDBATCHID"].ToString();
            ucCourse.SelectedCourse = dt.Rows[0]["FLDCOURSEID"].ToString();
            ddlSemester.Course = dt.Rows[0]["FLDCOURSEID"].ToString();
            //ucSemester.Course = dt.Rows[0]["FLDCOURSEID"].ToString();
            //ucSemester.Batch = dt.Rows[0]["FLDBATCHID"].ToString();
            //ucSemester.SemesterList = PhoenixPreSeaBatchAdmissionSemester.ListPreSeaBatchAdmissionSemester(General.GetNullableInteger(ucCourse.SelectedCourse), General.GetNullableInteger(ucBatch.SelectedBatch), null);

            ViewState["COURSEID"] = dt.Rows[0]["FLDCOURSEID"].ToString();
            ViewState["BATCHID"] = dt.Rows[0]["FLDBATCHID"].ToString();
            ViewState["SECTIONID"] = dt.Rows[0]["FLDSECTIONID"].ToString();
        }
    }

    protected void MenuPreSea_OnTabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("REPORT"))
        {
            String scriptpopup = String.Format(
                     "javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsView.aspx?applicationcode=10&reportcode=INDIVIDUALREPORT&studentid=" + Filter.CurrentPreSeaTraineeSelection + "&examscheduleid=" + ucBatchExam.SelectedBatchExam + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

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
                , General.GetNullableGuid(ucBatchExam.SelectedBatchExam)
                , null
                , General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection)
                , General.GetNullableInteger(ddlsubject.SelectedValue)
                , General.GetNullableString(ddlsubjectCode.SelectedValue)                
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                gvPreSeaExam.DataSource = ds.Tables[0];
                gvPreSeaExam.DataBind();

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                }
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvPreSeaExam);
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
                    bh.Attributes.Add("onclick", "javascript:parent.Openpopup('PreSea','','PreSeaBatchExamResultsHistory.aspx?traineeexamid=" + drv["FLDTRAINEEEXAMID"].ToString() + "'); return false;");                    
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
                        , int.Parse(Filter.CurrentPreSeaTraineeSelection)
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

    protected void ucSemester_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlSemester.SelectedSemester) != null)
        {
            ucBatchExam.Enabled = true;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            dt = PhoenixPreSeaBatchExamSchedule.ListPreSeaBatchExamSchedule(General.GetNullableInteger(ViewState["BATCHID"].ToString())
                    , General.GetNullableInteger(ddlSemester.SelectedSemester)
                    , null
                    , General.GetNullableInteger(ViewState["SECTIONID"].ToString())
                    , null
                    );

            ds.Tables.Add(dt.Copy());
            ucBatchExam.BatchExamList = ds;
        }
    }

    private bool IsValidExamResult(string maxmarks, string passmarks, string examdate, string absentyn, string marks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

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
    public void SetPreSeaNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PreExamResultsMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;            
            string course = string.Empty;
            string batch = string.Empty;
            
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();               
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlSemester.SelectedSemester = "";
                ucBatchExam.SelectedBatchExam = "";
                ddlsubject.SelectedIndex = 0;
                ddlsubjectCode.SelectedIndex = 0;
                BindData();              
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucBatchExam_TextChangedEvent(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucBatchExam.SelectedBatchExam) != null)
        {
            DataTable dt = new DataTable();

            dt = PhoenixPreSeaBatchExamSchedule.ListPreSeaBatchExamSubjects( null               
                    , General.GetNullableGuid(ucBatchExam.SelectedBatchExam).Value);

            ddlsubjectCode.DataSource = dt;
            ddlsubjectCode.DataTextField = "FLDSUBJECTCODE";
            ddlsubjectCode.DataValueField = "FLDSUBJECTCODE";
            ddlsubjectCode.DataBind();
            ddlsubjectCode.Items.Insert(0, new ListItem("--Select--", "Dummy"));

            ddlsubject.DataSource = dt;
            ddlsubject.DataTextField = "FLDSUBJECTNAME";
            ddlsubject.DataValueField = "FLDSUBJECTID";
            ddlsubject.DataBind();
            ddlsubject.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
    }
}
