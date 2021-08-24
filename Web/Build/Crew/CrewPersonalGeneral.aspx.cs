using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class CrewPersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
         //   Title1.ShowMenu = false;
        }

        if (!IsPostBack)
        {
            ViewState["ACCESS"] = "0";
            if (General.GetNullableInteger(Filter.CurrentCrewSelection).HasValue)
                ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Document", "DOCUMENT", CreateDocumentSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Training", "TRAINING", CreateTrainingSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Experience", "HISTORY", CreateHistorySubTab(string.Empty, 0, string.Empty));

        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT")
        {
            toolbarmain.AddButton("Evaluation", "EVALUATION", CreateEvaluationSubTab(string.Empty, 0, string.Empty));

            toolbarmain.AddButton("Correspondence", "CORRESPONDENCE", CreateCorrespondenceSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Remarks", "NOTES", CreateNotesSubTab(string.Empty, 0, string.Empty));
        }
        if (Request.QueryString["back"] != null)
        {
            toolbarmain.AddButton("Back", "LIST", ToolBarDirection.Right);
        }
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        { 
            if (Request.QueryString["showimportantremarks"] != null)
            {
                CreateNotesSubTab("../Crew/CrewImportantRemarks.aspx", 1, "IMPORTANTREMARKS");
                return;
            }
            else if (Request.QueryString["email"] != null)
            {
                CreateCorrespondenceSubTab("../Crew/CrewCorrespondenceEmail.aspx?cid=" + Request.QueryString["cid"], 1, "EMAIL");
                return;
            }
            else if (Request.QueryString["remarks"] != null)
            {
                CreateNotesSubTab("../Crew/CrewRemarks.aspx", 0, "GENERALREMARKS");
                return;
            }
            else if (Request.QueryString["licence"] != null)
            {

                CreateDocumentSubTab("../Crew/CrewLicence.aspx?cid=" + Request.QueryString["cid"], 2, "LICENCE");

                return;
            }
            else if (Request.QueryString["med"] != null)
            {
                CreateDocumentSubTab("../Crew/CrewMedicalDocument.aspx", 1, "MEDICAL");
            }
            else if (Request.QueryString["medreq"] != null && Request.QueryString["medreq"] != "" && Request.QueryString["familyid"].ToString() != null && Request.QueryString["familyid"].ToString() != "")
            {
                CreatePersonalSubTab("../Crew/CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], 2, "MEDICAL");
            }
            else if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], 2, "FAMILY/NOK");
            }
            else if (Request.QueryString["insurance"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewInsurance.aspx", 4, "INSURANCE");
            }
            else if (Request.QueryString["evaluation"] != null)
            {
                CreateEvaluationSubTab("CrewCandidateEvaluationList.aspx", 0, "ASSESSMENT");
            }
            else if (Request.QueryString["evaluationevent"] != null)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1)
                    CreateEvaluationSubTab("../Crew/CrewInspectionSupdtConcernsList.aspx", 5, "EVAPERSONAL");
                else
                    CreateEvaluationSubTab("../Crew/CrewEmployeeSuperintendentAssessment.aspx", 5, "EVAPERSONAL");
            }
            else if (Request.QueryString["documents"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewTravelDocument.aspx", 0, "TRAVELDOCUMENTS");
            }
            else if (Request.QueryString["Otherdocuments"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewOtherDocument.aspx", 3, "OTHERDOCUMENTS");
            }
            else if (Request.QueryString["appraisal"] != null && Request.QueryString["tabindex"] != null)
            {
                CreateEvaluationSubTab("../Crew/CrewAppraisal.aspx", int.Parse(Request.QueryString["tabindex"].ToString()), "APPRISAL");
            }
            else if (Request.QueryString["appraisal"] != null)
            {
                CreateEvaluationSubTab("../Crew/CrewAppraisal.aspx?empid=" + Request.QueryString["empid"], 5, "APPRISAL");

                return;
            }
            
            else if (Request.QueryString["recommendedcourse"] != null)
            {
                CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");
            }
            else if (Request.QueryString["course"] != null)
            {
                CreateTrainingSubTab("../Crew/CrewCourseAndCertificate.aspx", 0, "TRAINING");
            }
            else
            {
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
            }
        }        
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentCrewSelection == null || CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (Filter.CurrentCrewSelection == null && Request.QueryString["t"] == null)
            {
                ucError.ErrorMessage = "  Please Select a Employee .";
                ucError.Visible = true;
                CrewMenu.SelectedMenuIndex = 0;
            }
            CreatePersonalSubTab("CrewPersonal.aspx", 0, "SUBPERSONAL");
        }        
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("CrewQueryActivity.aspx?p=" + Request.QueryString["p"], false);
        }       
        else
        {
            CrewMenuGeneral_TabStripCommand(sender, e);
        }

    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if ((Filter.CurrentCrewSelection == null && Request.QueryString["t"] == null) || (Filter.CurrentCrewSelection == null && CommandName.ToUpper() != "PERSONAL"))
        {
            ucError.ErrorMessage = " Please Select a Employee ";
            ucError.Visible = true;
            CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("SUBPERSONAL"))
        {
            CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("ADDRESS"))
        {
            CreatePersonalSubTab("../Crew/CrewAddress.aspx", 1, "ADDRESS");
        }
        else if (CommandName.ToUpper().Equals("WORKINGGEAR"))
        {
            string employeeid = Filter.CurrentCrewSelection;
            CreatePersonalSubTab("../Crew/CrewWorkingGear.aspx?empid=" + int.Parse(employeeid), 2, "WORKINGGEAR");
        }
        else if (CommandName.ToUpper().Equals("FAMILY/NOK"))
        {
            string employeeid = Filter.CurrentCrewSelection;
            string familyid = null;
            CreatePersonalSubTab("../Crew/CrewFamilyNok.aspx?familyid=" + familyid + "&empid=" + int.Parse(employeeid), 3, "FAMILY/NOK");
        }

        else if (CommandName.ToUpper().Equals("INSURANCE"))
        {
            CreatePersonalSubTab("../Crew/CrewInsurance.aspx", 4, "INSURANCE");
        }
        else if (CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
        {
            CreateDocumentSubTab("../Crew/CrewTravelDocument.aspx", 4, "TRAVELDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("TRAVELPLAN"))
        {
            CreateDocumentSubTab("../Crew/CrewTravelPlanDocument.aspx", 4, "TRAVELPLAN");
        }
        else if (CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
        {
            CreateDocumentSubTab("../Crew/CrewOtherDocument.aspx", 3, "OTHERDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            CreateDocumentSubTab("../Crew/CrewMedicalDocument.aspx", 1, "MEDICAL");
        }
        else if (CommandName.ToUpper().Equals("MEDICALHISTORY"))
        {
            CreatePersonalSubTab("../Crew/CrewMedical.aspx", 7, "MEDICALHISTORY");
        }
        else if (CommandName.ToUpper().Equals("STATUS"))
        {
            CreatePersonalSubTab("../Crew/CrewStatus.aspx", 2, "STATUS");
        }
        else if (CommandName.ToUpper().Equals("BANKACCT"))
        {
            CreatePersonalSubTab("../Crew/CrewBankAccount.aspx", 3, "BANKACCT");
        }
        else if (CommandName.ToUpper().Equals("SUITABILITY"))
        {
            CreatePersonalSubTab("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?personalmaster=1&crewplanid=" + ViewState["crewplanid"], 10, "SUITABILITY");
        }        
        else if (CommandName.ToUpper().Equals("ASSESSMENT"))
        {
            CreateEvaluationSubTab("../Crew/CrewCandidateEvaluationList.aspx", 0, "ASSESSMENT");
        }
        else if (CommandName.ToUpper().Equals("APPRISAL"))
        {
            CreateEvaluationSubTab("../Crew/CrewAppraisal.aspx", 1, "APPRAISAL");
        }
        else if (CommandName.ToUpper().Equals("ABOUT US BY"))
        {
            CreateEvaluationSubTab("../Crew/CrewAboutUsBy.aspx", 1, "ABOUT US BY");
        }
        else if (CommandName.ToUpper().Equals("REFRENCES"))
        {
            CreateEvaluationSubTab("../Crew/CrewReference.aspx", 2, "REFRENCES");
        }
        else if (CommandName.ToUpper().Equals("UNALLOCATEDVESSELEXP"))
        {
            string employeeid = Filter.CurrentCrewSelection;
            CreateEvaluationSubTab("../Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + int.Parse(employeeid), 2, "UNALLOCATEDVESSELEXP");
        }
        else if (CommandName.ToUpper().Equals("EVAPERSONAL"))
        {
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1)
                CreateEvaluationSubTab("../Crew/CrewInspectionSupdtConcernsList.aspx", 2, "EVAPERSONAL");
            else
                CreateEvaluationSubTab("../Crew/CrewEmployeeSuperintendentAssessment.aspx", 2, "EVAPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("FEEDBACK"))
        {
            CreateEvaluationSubTab("../Crew/CrewSignoffFeedbackList.aspx?personalmaster=true", 5, "FEEDBACK");
        }
        else if (CommandName.ToUpper().Equals("ACADEMICS"))
        {
            CreateDocumentSubTab("../Crew/CrewAcademicQualification.aspx", 4, "ACADEMICS");
        }
        else if (CommandName.ToUpper().Equals("COURSEANDCERTIFICATES"))
        {
            CreateTrainingSubTab("../Crew/CrewCourseAndCertificate.aspx", 0, "COURSEANDCERTIFICATES");
        }
        else if (CommandName.ToUpper().Equals("CBTCOURSES"))
        {
            CreateTrainingSubTab("../Crew/CrewCBTCourses.aspx", 1, "CBTCOURSES");
        }
        else if (CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
        {
            CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");
        }
        else if (CommandName.ToUpper().Equals("ONBOARDTRAINING"))
        {
            CreateTrainingSubTab("../Crew/CrewOnboardTraining.aspx", 4, "ONBOARDTRAINING");
        }
        else if (CommandName.ToUpper().Equals("BRIEFING/DEBRIEFING"))
        {
            CreateTrainingSubTab("../Crew/CrewBriefingDebriefing.aspx", 5, "BRIEFING/DEBRIEFING");
        }
        else if (CommandName.ToUpper().Equals("EPSS"))
        {
            CreateTrainingSubTab("../Crew/CrewTrainingEpss.aspx", 3, "EPSS");
        }
        else if (CommandName.ToUpper().Equals("SUMMARY"))
        {
            CreateHistorySubTab("../Crew/CrewSummary.aspx?personalmaster=true", 0, "SUMMARY");
        }
        else if (CommandName.ToUpper().Equals("EXPERIENCE IN OTHERS"))
        {
            CreateHistorySubTab("../Crew/CrewOtherExperience.aspx", 2, "EXPERIENCE IN OTHERS");
        }
        else if (CommandName.ToUpper().Equals("EXPERIENCEINCOMPANY"))
        {
            CreateHistorySubTab("../Crew/CrewCompanyExperience.aspx", 1, "EXPERIENCEINCOMPANY");
        }
        else if (CommandName.ToUpper().Equals("LICENCE"))
        {
            CreateDocumentSubTab("../Crew/CrewLicence.aspx", 1, "LICENCE");
        }
        else if (CommandName.ToUpper().Equals("TRAINING"))
        {
            CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");
        }
        else if (CommandName.ToUpper().Equals("SUBACTIVITYLOG"))
        {
            CreateHistorySubTab("../Crew/CrewActivityLog.aspx", 3, "SUBACTIVITYLOG");
        }      
        else if (CommandName.ToUpper().Equals("GENERALREMARKS"))
        {
            CreateNotesSubTab("CrewRemarks.aspx", 0, "GENERALREMARKS");
        }
        else if (CommandName.ToUpper().Equals("IMPORTANTREMARKS"))
        {
            CreateNotesSubTab("CrewImportantRemarks.aspx", 1, "IMPORTANTREMARKS");
        }
        else if (CommandName.ToUpper().Equals("EMAIL"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondenceEmail.aspx", 1, "EMAIL");
        }
        else if (CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondence.aspx", 0, "SUBCORRESPONDENCE");
        }
    }
    private PhoenixToolbar CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
  
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        if (ViewState["ACCESS"] != null && ViewState["ACCESS"].ToString() == "1")
        {
            toolbarsub.AddButton("Address ", "ADDRESS");
        }
        toolbarsub.AddButton("Family NOK", "FAMILY/NOK");
        toolbarsub.AddButton("Working Gear", "WORKINGGEAR");
        toolbarsub.AddButton("Status", "STATUS");
        toolbarsub.AddButton("Bank", "BANKACCT");
        toolbarsub.AddButton("Insurance", "INSURANCE");

        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    
    private PhoenixToolbar CreateNotesSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        
        toolbarsub.AddButton("General", "GENERALREMARKS");
        toolbarsub.AddButton("Important", "IMPORTANTREMARKS");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();

        PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
        objdiscussion.dtkey = GetCurrentEmployeeDTkey();
        objdiscussion.userid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;
        objdiscussion.type = PhoenixCrewConstants.REMARKS;
        PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateDocumentSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Travel", "TRAVELDOCUMENTS");    
        if (ViewState["ACCESS"] !=null && ViewState["ACCESS"].ToString() == "1")
        {
            toolbarsub.AddButton("Medical", "MEDICAL");
        }
        toolbarsub.AddButton("Licence", "LICENCE");
        toolbarsub.AddButton("Academics", "ACADEMICS");
        toolbarsub.AddButton("Other", "OTHERDOCUMENTS");
        toolbarsub.AddButton("Travel Plan", "TRAVELPLAN");

        if (!SessionUtil.CanAccess(this.ViewState, "TRAVELDOCUMENTS"))
        {            
            if (SessionUtil.CanAccess(this.ViewState, "MEDICAL"))
                Page = "../Crew/CrewMedicalDocument.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "LICENCE"))
                Page = "../Crew/CrewLicence.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ACADEMICS"))
                Page = "../Crew/CrewAcademicQualification.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "OTHERDOCUMENTS"))
                Page = "../Crew/CrewOtherDocument.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "TRAVELPLAN"))
                Page = "../Crew/CrewTravelPlanDocument.aspx";
        }

        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
    
        DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDRANK"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        toolbarsub.AddButton("Summary", "SUMMARY");
        toolbarsub.AddButton("Company", "EXPERIENCEINCOMPANY");
        toolbarsub.AddButton("Other", "EXPERIENCE IN OTHERS");
        toolbarsub.AddButton("Activity Log", "SUBACTIVITYLOG");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        if (!SessionUtil.CanAccess(this.ViewState, "SUMMARY"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "EXPERIENCEINCOMPANY"))
                Page = "../Crew/CrewCompanyExperience.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "EXPERIENCE IN OTHERS"))
                Page = "../Crew/CrewOtherExperience.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "SUBACTIVITYLOG"))
                Page = "../Crew/CrewActivityLog.aspx";           
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateTrainingSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Course", "COURSEANDCERTIFICATES");
        toolbarsub.AddButton("CBT Courses", "CBTCOURSES");
        if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            toolbarsub.AddButton("Recommended Training", "RECOMMENDEDCOURSES");
        else
            toolbarsub.AddButton("Recommended Courses", "RECOMMENDEDCOURSES");
        // toolbarsub.AddButton("Training", "TRAINING");
        toolbarsub.AddButton("EPSS", "EPSS");
        toolbarsub.AddButton("On Board", "ONBOARDTRAINING");
        toolbarsub.AddButton("Briefing", "BRIEFING/DEBRIEFING");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        if (!SessionUtil.CanAccess(this.ViewState, "COURSEANDCERTIFICATES"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "CBTCOURSES"))
                Page = "../Crew/CrewCBTCourses.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "RECOMMENDEDCOURSES"))
                Page = "../Crew/CrewRecommendedCourses.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "EPSS"))
                Page = "../Crew/CrewTrainingEpss.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ONBOARDTRAINING"))
            {
                Page = "../Crew/CrewOnboardTraining.aspx";
            }
            else if (SessionUtil.CanAccess(this.ViewState, "BRIEFING/DEBRIEFING"))
                Page = "../Crew/CrewBriefingDebriefing.aspx";
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateEvaluationSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Assessment", "ASSESSMENT");
        
        string canview = "1";
        DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDRANK"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
        }
        if (canview.Equals("1"))
        {
            toolbarsub.AddButton("Appraisal", "APPRISAL");
        }
        toolbarsub.AddButton("About Us By", "ABOUT US BY");
        toolbarsub.AddButton("Reference", "REFRENCES");
        toolbarsub.AddButton("Unallocated Exp", "UNALLOCATEDVESSELEXP");
        toolbarsub.AddButton("Events", "EVAPERSONAL");
        toolbarsub.AddButton("FeedBack", "FEEDBACK");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        if (!SessionUtil.CanAccess(this.ViewState, "ASSESSMENT"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "APPRISAL"))
                Page = "../Crew/CrewAppraisal.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ABOUT US BY"))
                Page = "../Crew/CrewAboutUsBy.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "UNALLOCATEDVESSELEXP"))
                Page = "../Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + int.Parse(Filter.CurrentCrewSelection);
            else if (SessionUtil.CanAccess(this.ViewState, "EVAPERSONAL"))
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1)
                    Page = "../Crew/CrewInspectionSupdtConcernsList.aspx";
                else
                    Page = "../Crew/CrewEmployeeSuperintendentAssessment.aspx";
            }
            else if (SessionUtil.CanAccess(this.ViewState, "FEEDBACK"))
                Page = "../Crew/CrewSignoffFeedbackList.aspx?personalmaster=true";
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateCorrespondenceSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Correspondence", "SUBCORRESPONDENCE");
        toolbarsub.AddButton("Email", "EMAIL");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {

            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }
}
