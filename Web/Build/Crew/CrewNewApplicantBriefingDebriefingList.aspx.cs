using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewNewApplicantBriefingDebriefingList : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewBriefingDebriefingList.AccessRights = this.ViewState;
        MenuCrewBriefingDebriefingList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {           
			ddlUserAdd.ActiveYN = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 44, "Y"));
            if (Request.QueryString["CrewBriefingDebriefingId"] != null)
            {
                EditCrewBriefingDebriefing(Int16.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString()));
            }
            else
                ResetCrewBriefingDebriefing();
        }
    }

    protected void EditCrewBriefingDebriefing(int employeebriefingdebriefingid)
    {
        DataSet ds = PhoenixCrewBriefingDebriefing.EditBriefingDebriefing(employeebriefingdebriefingid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            //ucSubject.SelectedBriefingTopic = dr["FLDSUBJECTID"].ToString();
            txtSubject.Text = dr["FLDSUBJECT"].ToString(); 

            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            //txtRemarks.Text = dr["FLDREMARKS"].ToString();
            //txtInstructor.Text = dr["FLDINSTRUCTOR"].ToString();
            txtTopicsdiscussed.Text = dr["FLDTOPICSDISCUSSED"].ToString();
            txtCandidatesfeedback.Text = dr["FLDCANDIDATESFEEDBACK"].ToString();
            txtFieldofficefeedback.Text = dr["FLDFIELDOFFICEFEEDBACK"].ToString();
            txtConclusion.Text = dr["FLDCONCLUSION"].ToString();
            ddlUserAdd.SelectedUser = dr["FLDINSTRUCTORID"].ToString();
            ddlBriefingTopic.SelectedBriefingTopic = dr["FLDBRIEFINGTOPICID"].ToString();
            ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
            ddlBriefingMode.SelectedValue = dr["FLDBRIEFINGMODE"].ToString();
            ddlUserAdd_TextChanged(null, null);
        }
    }

    protected void ResetCrewBriefingDebriefing()
    {
        ucVessel.SelectedVessel = "";
        //ucSubject.SelectedBriefingTopic = "";
        txtSubject.Text = "";

        txtFromDate.Text = "";
        txtToDate.Text = "";
        //txtRemarks.Text = "";
        //txtInstructor.Text = "";
        txtTopicsdiscussed.Text = "";
        txtCandidatesfeedback.Text = "";
        txtFieldofficefeedback.Text = "";
        txtConclusion.Text = "";
        ddlUserAdd.SelectedUser = "";
        ddlBriefingTopic.SelectedBriefingTopic = "";
    }

    protected void CrewBriefingDebriefingList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidCrewBriefingDebriefing())
            {
                if (Request.QueryString["CrewBriefingDebriefingId"] != null)
                {
                    PhoenixCrewBriefingDebriefing.UpdateBriefingDebriefing(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Int32.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString())
                        , Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                        , Int32.Parse(ucVessel.SelectedVessel)
                        , DateTime.Parse(txtFromDate.Text)
                        , DateTime.Parse(txtToDate.Text)
                        , ""
                        , txtSubject.Text.Trim()
                        , txtTopicsdiscussed.Text.Trim()
                        , txtCandidatesfeedback.Text.Trim()
                        , txtFieldofficefeedback.Text.Trim()
                        , txtConclusion.Text.Trim()
                        , int.Parse(ddlUserAdd.SelectedUser)
                         , General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic)
                         , General.GetNullableInteger(ucCompany.SelectedCompany)
                        , General.GetNullableInteger(ddlBriefingMode.SelectedValue)
                        );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
                else
                {
                    PhoenixCrewBriefingDebriefing.InsertBriefingDebriefing(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                        , Int32.Parse(ucVessel.SelectedVessel)
                        , DateTime.Parse(txtFromDate.Text)
                        , DateTime.Parse(txtToDate.Text)
                        , ""
                        , txtSubject.Text.Trim()
                        , txtTopicsdiscussed.Text.Trim()
                        , txtCandidatesfeedback.Text.Trim()
                        , txtFieldofficefeedback.Text.Trim()
                        , txtConclusion.Text.Trim()
                        , int.Parse(ddlUserAdd.SelectedUser)
                        , General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic)
                        , General.GetNullableGuid(Request.QueryString["appraisalid"])
                        , General.GetNullableInteger(ucCompany.SelectedCompany)
                        , General.GetNullableInteger(ddlBriefingMode.SelectedValue)
                        );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    ResetCrewBriefingDebriefing();
                }
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }
    protected void ddlUserAdd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ucCompany.UserId = ddlUserAdd.SelectedUser;
            ucCompany.bind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCrewBriefingDebriefing()
    {
        DateTime resultDate;
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if ((!Int32.TryParse(ucVessel.SelectedVessel, out resultInt)) || ucVessel.SelectedVessel == "0")
            ucError.ErrorMessage = "Vessel is required.";
        //if ((!Int32.TryParse(ucSubject.SelectedBriefingTopic, out resultInt)) || ucSubject.SelectedBriefingTopic == "0")
        //    ucError.ErrorMessage = "Subject is required.";
        
        if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
            ucError.ErrorMessage = "Briefing From Date is required.";
        if (!DateTime.TryParse(txtToDate.Text, out resultDate))
            ucError.ErrorMessage = "Briefing To Date is required.";
        if (txtSubject.Text.Trim() == "")
            ucError.ErrorMessage = "Subject is required.";
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
                if ((DateTime.Parse(txtFromDate.Text)) > (DateTime.Parse(txtToDate.Text)))
                    ucError.ErrorMessage = "'Briefing To Date' should be greater than or equal to 'Briefing From Date'";
        }

        if (ddlUserAdd.SelectedUser == "Dummy")
        {
            ucError.ErrorMessage = "Instructor is required.";
        }

        if (!General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic).HasValue)
        {
            ucError.ErrorMessage = "Briefing Details is required.";
        }

        return (!ucError.IsError);

    }
}
