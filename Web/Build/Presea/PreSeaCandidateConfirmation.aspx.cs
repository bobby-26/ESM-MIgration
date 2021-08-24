using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;

public partial class PreSeaCandidateConfirmation : PhoenixBasePage
{
    string strCandidatesId, strBatchId, strCourseId;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            strCandidatesId = Request.QueryString["candidateId"];
            strBatchId = Request.QueryString["batch"];
            strCourseId = Request.QueryString["course"];
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["INTERVIEWID"] = String.Empty;
                ViewState["COURSEID"] = String.Empty;
                ViewState["APPLIEDBATCH"] = String.Empty;

                if (!string.IsNullOrEmpty(Request.QueryString["interviewid"]))
                    ViewState["INTERVIEWID"] = Request.QueryString["interviewid"];

                ddlSatus.DataSource = PhoenixPreSeaNewApplicantManagement.ListPreSeaApplicantNextStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(strCandidatesId));
                ddlSatus.DataBind();

                SetPrimaryCandidatesDetails();
                BindBatch(ViewState["COURSEID"].ToString());
                SetInterviewDeatils();               
            }
            BindBatchFillingInformation(strCourseId, strBatchId);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string Script = "";

                Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
                Script += "fnReloadList();";
                Script += "</script>" + "\n";

                if (!IsValidDetails())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    
                    PhoenixPreSeaCandidateConfirmation.PreSeaCandidateConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(ViewState["INTERVIEWID"].ToString())
                        , int.Parse(strBatchId)
                        , int.Parse(ddlSatus.SelectedValue)
                        , General.GetNullableInteger(ddlBatch.SelectedValue)
                        , General.GetNullableByte(rdoIMUQulify.SelectedValue)
                        , General.GetNullableInteger(ddlRejectReason.SelectedQuick));

                    ucStatus.Text = "Interview result updated and confirmed successfully.";
                    BindBatchFillingInformation(ViewState["COURSEID"].ToString(), ddlBatch.SelectedValue);
                    GetPresentStatus();
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetInterviewDeatils()
    {
        try
        {
            if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
            {
                DataTable dt = PhoenixPreSeaEntranceExam.EditPreSeaEntranceInterviewSummary(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()));

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    ucExamdate.Text = dr["FLDINTERVIEWDATE"].ToString();
                    txtInterviewBy.Text = dr["FLDINTERVIEDBY"].ToString();
                    ucExamVenue.SelectedExamVenue = dr["FLDINTERVIEWVENUE"].ToString();

                    ucExamdate.ReadOnly = true;
                    txtInterviewBy.ReadOnly = true;
                    ucExamVenue.Enabled = false;
                    txtRollNo.ReadOnly = true;

                    if (General.GetNullableInteger(dr["FLDSELECTIONSTATUS"].ToString()) != null)
                        ddlSatus.SelectedValue = dr["FLDSELECTIONSTATUS"].ToString();

                    ddlBatch.SelectedValue = dr["FLDTRAININGBATCH"].ToString();

                    if (General.GetNullableInteger(dr["FLDISIMUQUALIFY"].ToString()) != null)
                        rdoIMUQulify.SelectedValue = dr["FLDISIMUQUALIFY"].ToString();

                    ddlRejectReason.SelectedQuick = dr["FLDREJECTIONEMARKS"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPrimaryCandidatesDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(strCandidatesId));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtBatch.Text = dr["FLDBATCHNAME"].ToString();
                txtCourse.Text = dr["FLDCOURSENAME"].ToString();
                ucExamVenue.SelectedExamVenue = dr["FLDCALLEDVENUE"].ToString();
                ViewState["COURSEID"] = dr["FLDCOURSEID"].ToString();
                txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtRollNo.Text = dr["FLDENTRANCEROLLNO"].ToString();
                ViewState["APPLIEDBATCH"] = dr["FLDAPPLIEDBATCH"].ToString();

                txtFirstName.ReadOnly = true;
                txtMiddleName.ReadOnly = true;
                txtLastName.ReadOnly = true;
                txtBatch.ReadOnly = true;
                txtCourse.ReadOnly = true;
                ucExamVenue.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidConfirmationDeatils()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(ddlSatus.SelectedValue))
            ucError.ErrorMessage = "Interview Status is Required.";
        else
        {
            string confirmed = "";
            string rejected = "";

            confirmed = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , 255, "CON");
            rejected = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , 255, "REJ");
            if (ddlSatus.SelectedValue == confirmed)
            {
                if (General.GetNullableInteger(ddlBatch.SelectedValue) == null)
                    ucError.ErrorMessage = "Confirmed Batch(Posted) is required.";
            }
            if (ddlSatus.SelectedValue == rejected)
            {
                if (General.GetNullableInteger(ddlRejectReason.SelectedQuick) == null)
                    ucError.ErrorMessage = "Rejection reason is required.";
            }
        }
        if (String.IsNullOrEmpty(rdoIMUQulify.SelectedValue))
            ucError.ErrorMessage = "Mention IMU Qualification";

        return (!ucError.IsError);
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ddlSatus.SelectedValue))
        {
            if (ddlSatus.SelectedItem.Text.Contains("Reje"))
            {
                ddlBatch.CssClass = "dropdown_mandatory";
                ddlRejectReason.CssClass = "dropdown_mandatory";
            }
            if (ddlSatus.SelectedItem.Text.Contains("Recom"))
            {
                ddlBatch.SelectedValue = "DUMMY";
                ddlBatch.CssClass = "input";
                ddlRejectReason.CssClass = "input";
                ddlRejectReason.SelectedQuick = "";
            }
            if (ddlSatus.SelectedItem.Text.Contains("Con"))
            {
                ddlRejectReason.CssClass = "input";
                ddlBatch.CssClass = "dropdown_mandatory";
                ddlRejectReason.SelectedQuick = "";
            }
        }
    }

    private void BindBatchFillingInformation(string strCourseId, string strBatchId)
    {
        if (General.GetNullableInteger(strBatchId) == null)
            strBatchId = ViewState["APPLIEDBATCH"].ToString();

            DataTable dt = PhoenixPreSeaBatch.ListPreSeaBatchRecruitementStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(strCourseId)
                , int.Parse(strBatchId)
                );
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                txtMaxLimit.Text = dr["FLDMAXLIMIT"].ToString();
                txtConfirmed.Text = dr["FLDCONFIRMED"].ToString();
                txtWaitListed.Text = dr["FLDWAITLISTED"].ToString();
                //txtRejected.Text   = dr["FLDREJECTED"].ToString(); 
            }
        
    }
    protected void ddlBatch_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlBatch.SelectedValue) != null)
        {
            BindBatchFillingInformation("", ddlBatch.SelectedValue);
        }
        if (General.GetNullableInteger(ddlBatch.SelectedValue) == null)
        {
            txtMaxLimit.Text = "";
            txtWaitListed.Text = "";
            txtConfirmed.Text = "";
        }

    }
    private void GetPresentStatus()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(strCandidatesId));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindBatch(string course)
    {
        ddlBatch.DataSource = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), null, null);
        ddlBatch.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlBatch.Items.Insert(0, li);
    }

    private bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlSatus.SelectedItem.Text.Contains("Confirmed"))
        {
            if (ddlBatch.SelectedValue == "DUMMY")
                ucError.ErrorMessage = "Batch is required";
        }
        if (ddlSatus.SelectedItem.Text.Contains("Rejected"))
        {
            if (ddlBatch.SelectedValue == "DUMMY")
                ucError.ErrorMessage = "Rejected batch is required";
            if (General.GetNullableInteger(ddlRejectReason.SelectedQuick) == null)
                ucError.ErrorMessage = "Rejected reason is required";
        }

        return (!ucError.IsError);
    }
}
