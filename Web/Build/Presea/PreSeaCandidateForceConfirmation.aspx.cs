using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaCandidateForceConfirmation : PhoenixBasePage
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
            ucCandidate.Batch = strBatchId;
            if (!IsPostBack)
            {
                ViewState["INTERVIEWID"] = String.Empty;
                
                if (!string.IsNullOrEmpty(Request.QueryString["interviewid"]))
                    ViewState["INTERVIEWID"] = Request.QueryString["interviewid"];

                SetPrimaryCandidatesDetails();
                SetInterviewDeatils();

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = toolbarmain.Show();

                
            }

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


                ucStatus.Text = "Force confirmation will be implement shortly.";
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
                    txtRollNo.Text = dr["FLDROLLNO"].ToString();

                    ucExamdate.ReadOnly = true;
                    txtInterviewBy.ReadOnly = true;
                    ucExamVenue.Enabled = false;
                    txtRollNo.ReadOnly = true;
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

}
