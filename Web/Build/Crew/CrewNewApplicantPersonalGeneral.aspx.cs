using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantPersonalGeneral : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["t"] != null)
            {
                Response.Redirect("CrewNewApplicantRegister.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentNewApplicantSelection = Request.QueryString["empid"];
                //Title1.ShowMenu = "false";
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Document", "OTHERDOCUMENTS", CreateDocumentSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Training", "TRAINING", CreateTrainingSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Experience", "HISTORY", CreateHistorySubTab(string.Empty, 0, string.Empty));

            if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT")
            {
                toolbarmain.AddButton("Evaluation", "EVALUATION", CreateEvaluationSubTab(string.Empty, 0, string.Empty));
                toolbarmain.AddButton("Correspondence", "CORRESPONDENCE", CreateCorrespondenceSubTab(string.Empty, 0, string.Empty));
                toolbarmain.AddButton("Remarks", "REMARKS", CreateNotesSubTab(string.Empty, 0, string.Empty));
            }
            if (Request.QueryString["back"] != null)
            {
                toolbarmain.AddButton("Back", "LIST", ToolBarDirection.Right);
            }
            Main.AccessRights = this.ViewState;
            Main.MenuList = toolbarmain.Show();

            if (!Page.IsPostBack)
            {
                ViewState["launchedfrom"] = "";
                ViewState["pl"] = "";
                ViewState["NewApplicant"] = "";
                ViewState["crewsearch"] = "";
                ViewState["crewplanid"] = "";

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                {
                    ViewState["launchedfrom"] = Request.QueryString["launchedfrom"].ToString();
                    Filter.CurrentCrewLaunchedFrom = Request.QueryString["launchedfrom"].ToString();
                }
                else
                    Filter.CurrentCrewLaunchedFrom = null;

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                if (Request.QueryString["Newapp"] != null && Request.QueryString["Newapp"].ToString() != "")
                    ViewState["NewApplicant"] = Request.QueryString["Newapp"].ToString();

                if (Request.QueryString["crewsearch"] != null && Request.QueryString["crewsearch"].ToString() != "")
                    ViewState["crewsearch"] = Request.QueryString["crewsearch"].ToString();

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                if (Request.QueryString["medreq"] != null && Request.QueryString["medreq"] != "" && Request.QueryString["familyid"].ToString() != null && Request.QueryString["familyid"].ToString() != "")
                {
                    CreatePersonalSubTab("../Crew/CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], 1, "MEDICAL");
                }
                else if (Request.QueryString["med"] != null)
                {
                    CreateDocumentSubTab("../Crew/CrewNewApplicantMedicalDocument.aspx", 2, "MEDICAL");
                }
                else if (Request.QueryString["email"] != null)
                {
                    CreateCorrespondenceSubTab("../Crew/CrewNewApplicantCorrespondenceEmail.aspx?cid=" + Request.QueryString["cid"], 1, "SUBCORRESPONDENCE");
                }
                else if (Request.QueryString["evaluation"] != null)
                {
                    CreateEvaluationSubTab("../Crew/CrewNewApplicantCandidateEvaluationList.aspx", 1, "ASSESSMENT");
                }
                else if (Request.QueryString["recommendedcourse"] != null)
                {
                    CreateTrainingSubTab("../Crew/CrewNewApplicantCourseAndCertificate.aspx", 1, "COURSEANDCERTIFICATES");
                }
                else if (Request.QueryString["remarks"] != null)
                {
                    CreateNotesSubTab("../Crew/CrewNewApplicantRemarks.aspx", 1, "GENERALREMARKS");
                }
                else
                {
                    CreatePersonalSubTab("../Crew/CrewNewApplicantPersonal.aspx", 0, "SUBPERSONAL");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Main_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (Filter.CurrentNewApplicantSelection == null || CommandName.ToUpper().Equals("PERSONAL"))
            {
                if (Filter.CurrentNewApplicantSelection == null && Request.QueryString["t"] == null)
                {
                    ucError.ErrorMessage = "Please Select a New Applicant.";
                    ucError.Visible = true;
                    //CrewMenu.SelectedMenuIndex = 0;
                }
                CreatePersonalSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBPERSONAL");
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() != "" && ViewState["crewsearch"].ToString() == "")
                    Response.Redirect("../Crew/CrewNewApplicantQueryActivity.aspx?p=" + Request.QueryString["p"] + "&launchedfrom=" + ViewState["launchedfrom"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                else if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() == "" && ViewState["crewsearch"].ToString() == "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreQueryActivity.aspx?p=" + Request.QueryString["p"] + "&launchedfrom=" + ViewState["launchedfrom"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                else if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() == "" && ViewState["crewsearch"].ToString() != "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreEmployee.aspx?sid=" + ViewState["crewsearch"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&launchedfrom=" + ViewState["launchedfrom"].ToString(), true);
                else
                    Response.Redirect("../Crew/CrewNewApplicantQueryActivity.aspx?p=" + Request.QueryString["p"], false);
            }
            if (CommandName.ToUpper().Equals("SUITABILITY"))
            {
                toolbarsub.AddButton("Suitability Check", "SUBSUITABILITY");
                //CrewMenuGeneral.AccessRights = this.ViewState;
                //CrewMenuGeneral.MenuList = toolbarsub.Show();
                //CrewMenuGeneral.SelectedMenuIndex = 0;
                ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?newapplicant=1&crewplanid=" + ViewState["crewplanid"];
            }
            else if (CommandName.ToUpper().Equals("ACTIVITIES"))
            {

                if (Filter.CurrentNewApplicantSelection != null && Filter.CurrentNewApplicantSelection != "")
                {
                    toolbarsub.AddButton("Update Availability", "SUBACTIVITIES");
                    //CrewMenuGeneral.AccessRights = this.ViewState;
                    //CrewMenuGeneral.MenuList = toolbarsub.Show();
                    //CrewMenuGeneral.SelectedMenuIndex = 0;

                    DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentNewApplicantSelection));
                    if (dt.Rows[0]["FLDNEWAPP"] != null && dt.Rows[0]["FLDNEWAPP"].ToString().Equals("1") && ViewState["launchedfrom"].ToString() == "")
                    {
                        ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + Filter.CurrentNewApplicantSelection;
                    }
                    else
                        ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&launchedfrom=" + ViewState["launchedfrom"].ToString();
                }
            }

            else if (CommandName.ToUpper().Equals("ACTIVITYLOG"))
            {
                CreateActivityLogSubTab("../Crew/CrewNewApplicantActiveLog.aspx", 0, "SUBACTIVITYLOG");
            }
            else
            {
                CrewMenuGeneral_TabStripCommand(sender, e);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateActivityLogSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Activity Log", "SUBACTIVITYLOG");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUBPERSONAL"))
            {
                CreatePersonalSubTab("../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty), 0, "SUBPERSONAL");
                //ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
            }
            else if (CommandName.ToUpper().Equals("ADDRESS"))
            {   
                CreatePersonalSubTab("../Crew/CrewNewApplicantAddress.aspx", 1, "ADDRESS");
            }
            else if (CommandName.ToUpper().Equals("WORKINGGEAR"))
            {
                string employeeid = Filter.CurrentNewApplicantSelection;
                CreatePersonalSubTab("../Crew/CrewWorkingGear.aspx?empid=" + int.Parse(employeeid), 2, "WORKINGGEAR");                
            }
            else if (CommandName.ToUpper().Equals("FAMILY/NOK"))
            {
                //ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantFamilyNok.aspx";
                string employeeid = Filter.CurrentCrewSelection;
                //string familyid = null;
                //CreatePersonalSubTab("../Crew/CrewNewApplicantFamilyNok.aspx?familyid=" + familyid + "&empid=" + int.Parse(employeeid), 3, "FAMILY/NOK");
                CreatePersonalSubTab("../Crew/CrewNewApplicantFamilyNok.aspx", 3, "FAMILY/NOK");
            }
            else if (CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantTravelDocument.aspx", 1, "TRAVELDOCUMENTS");                
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantMedicalDocument.aspx", 2, "MEDICAL");                
            }
            else if (CommandName.ToUpper().Equals("MEDICALHISTORY"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantMedical.aspx", 7, "MEDICALHISTORY");               
            }
            else if (CommandName.ToUpper().Equals("SUITABILITY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?newapplicant=1&crewplanid=" + ViewState["crewplanid"];
            }
            else if (CommandName.ToUpper().Equals("ACTIVITIES"))
            {
                if (Filter.CurrentNewApplicantSelection != null && Filter.CurrentNewApplicantSelection != "")
                {
                    
                    DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentNewApplicantSelection));
                    if (dt.Rows[0]["FLDNEWAPP"] != null && dt.Rows[0]["FLDNEWAPP"].ToString().Equals("1") && ViewState["launchedfrom"].ToString() == "")
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + Filter.CurrentNewApplicantSelection;
                    else
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&launchedfrom=" + ViewState["launchedfrom"].ToString();
                }
            }
            else if (CommandName.ToUpper().Equals("COURSEANDCERTIFICATES"))
            {
                CreateTrainingSubTab("../Crew/CrewNewApplicantCourseAndCertificate.aspx", 1, "MEDICALHISTORY");               
            }
            else if (CommandName.ToUpper().Equals("CBTCOURSES"))
            {
                CreateTrainingSubTab("../Crew/CrewNewApplicantCBTCourses.aspx", 2, "CBTCOURSES");               
            }
            else if (CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
            {
                CreateTrainingSubTab("../Crew/CrewNewApplicantRecommendedCourses.aspx", 3, "RECOMMENDEDCOURSES");                
            }
            else if (CommandName.ToUpper().Equals("ONBOARDTRAINING"))
            {
                CreateTrainingSubTab("../Crew/CrewNewApplicantOnboardTraining.aspx", 4, "ONBOARDTRAINING");                
            }
            else if (CommandName.ToUpper().Equals("BRIEFING/DEBRIEFING"))
            {
                CreateTrainingSubTab("../Crew/CrewNewApplicantBriefingDebriefing.aspx", 5, "BRIEFING/DEBRIEFING");                
            }
            else if (CommandName.ToUpper().Equals("EXPERIENCE IN OTHERS"))
            {
                CreateHistorySubTab("../Crew/CrewNewApplicantOtherExperience.aspx", 3, "EXPERIENCE IN OTHERS");                
            }
            else if (CommandName.ToUpper().Equals("EXPERIENCEINCOMPANY"))
            {
                CreateHistorySubTab("../Crew/CrewNewApplicantCompanyExperience.aspx", 2, "EXPERIENCEINCOMPANY");                
            }
            else if (CommandName.ToUpper().Equals("ACADEMICS"))
            {                
                CreateDocumentSubTab("../Crew/CrewNewApplicantAcademicQualification.aspx", 5, "ACADEMICS");
            }
            else if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                CreateHistorySubTab("../Crew/CrewSummary.aspx?newapplicant=true", 1, "SUMMARY");                
            }
            else if (CommandName.ToUpper().Equals("ACTIVITYLOG"))
            {
                CreateHistorySubTab("../Crew/CrewNewApplicantActiveLog.aspx", 4, "ACTIVITYLOG");               
            }
            else if (CommandName.ToUpper().Equals("LICENCE"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantLicence.aspx", 3, "LICENCE");               
            }
            else if (CommandName.ToUpper().Equals("ASSESSMENT"))
            {
                CreateEvaluationSubTab("../Crew/CrewNewApplicantCandidateEvaluationList.aspx", 1, "ASSESSMENT");                
            }
            else if (CommandName.ToUpper().Equals("ABOUTUSBY"))
            {
                CreateEvaluationSubTab("../Crew/CrewNewApplicantAboutUsBy.aspx", 3, "ABOUTUSBY");                
            }
            else if (CommandName.ToUpper().Equals("REFRENCES"))
            {
                CreateEvaluationSubTab("../Crew/CrewNewApplicantReference.aspx", 4, "REFRENCES");                
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantOtherDocument.aspx", 6, "OTHERDOCUMENTS");                
            }
            else if (CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                CreateDocumentSubTab("../Crew/CrewNewApplicantTravelPlanDocument.aspx", 6, "TRAVELPLAN");
            }

            else if (CommandName.ToUpper().Equals("EMAIL"))
			{
                CreateCorrespondenceSubTab("../Crew/CrewNewApplicantCorrespondenceEmail.aspx", 2, "EMAIL");                
			}
			else if (CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
			{
                CreateCorrespondenceSubTab("../Crew/CrewNewApplicantCorrespondence.aspx", 1, "SUBCORRESPONDENCE");                
			}
            else if (CommandName.ToUpper().Equals("IMPORTANTREMARKS"))
            {
                CreateCorrespondenceSubTab("../Crew/CrewNewApplicantImportantRemarks.aspx", 1, "IMPORTANTREMARKS");               
            }
            else if (CommandName.ToUpper().Equals("SUBREMARKS"))
            {
                CreateCorrespondenceSubTab("../Crew/CrewNewApplicantRemarks.aspx", 2, "SUBREMARKS");            
            }
			else if (CommandName.ToUpper().Equals("APPRISAL"))
			{
                Filter.CurrentAppraisalSelection = null;
                CreateEvaluationSubTab("../Crew/CrewNewApplicantAppraisal.aspx", 2, "APPRISAL");                
            }
            else if (CommandName.ToUpper().Equals("SUBSUITABILITY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?newapplicant=1&crewplanid=" + ViewState["crewplanid"];
            }
            if (Filter.CurrentNewApplicantSelection == null)
            {
                ucError.ErrorMessage = " Please Select a New Applicant.";
                ucError.Visible = true;
                ifMoreInfo.Attributes["src"] = "CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
            }
            //else
            //{
            //    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {

            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
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

    private PhoenixToolbar CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        if (Filter.CurrentCrewLaunchedFrom != null)
        {
            toolbarsub.AddButton("Suitability", "SUITABILITY");
            toolbarsub.AddButton("Availability", "ACTIVITIES");
        }
        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        toolbarsub.AddButton("Address ", "ADDRESS");
        toolbarsub.AddButton("Family NOK ", "FAMILY/NOK");
        toolbarsub.AddButton("Working Gear", "WORKINGGEAR");

        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;      
    }
    private PhoenixToolbar CreateDocumentSubTab(string Page, int MenuIndex, string Cmd)
    {  
        if (Filter.CurrentNewApplicantSelection == null && Request.QueryString["t"] == null)
        {
            ucError.ErrorMessage = "Please Select a New Applicant.";
            ucError.Visible = true;
        }

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Travel", "TRAVELDOCUMENTS");

        toolbarsub.AddButton("Medical", "MEDICAL");

        toolbarsub.AddButton("Licence", "LICENCE");
        toolbarsub.AddButton("Academics", "ACADEMICS");
        toolbarsub.AddButton("Other", "OTHERDOCUMENTS");
        toolbarsub.AddButton("Travel Plan", "TRAVELPLAN");

        if (!SessionUtil.CanAccess(this.ViewState, "TRAVELDOCUMENTS"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "MEDICAL"))
                Page = "../Crew/CrewNewApplicantMedicalDocument.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "LICENCE"))
                Page = "../Crew/CrewNewApplicantLicence.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ACADEMICS"))
                Page = "../Crew/CrewNewApplicantAcademicQualification.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "OTHERDOCUMENTS"))
                Page = "../Crew/CrewNewApplicantOtherDocument.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "TRAVELPLAN"))
                Page = "../Crew/CrewNewApplicantTravelPlanDocument.aspx";
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
        toolbarsub.AddButton("On Board", "ONBOARDTRAINING");
        toolbarsub.AddButton("Briefing", "BRIEFING/DEBRIEFING");
      
        if (!SessionUtil.CanAccess(this.ViewState, "COURSEANDCERTIFICATES"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "CBTCOURSES"))
                Page = "../Crew/CrewNewApplicantCBTCourses.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "RECOMMENDEDCOURSES"))
                Page = "../Crew/CrewNewApplicantRecommendedCourses.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "EPSS"))
                Page = "../Crew/CrewTrainingEpss.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ONBOARDTRAINING"))
            {
                Page = "../Crew/CrewNewApplicantOnboardTraining.aspx";
            }
            else if (SessionUtil.CanAccess(this.ViewState, "BRIEFING/DEBRIEFING"))
                Page = "../Crew/CrewNewApplicantBriefingDebriefing.aspx";
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        //DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        //DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDRANK"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        toolbarsub.AddButton("Summary", "SUMMARY");
        toolbarsub.AddButton("Company", "EXPERIENCEINCOMPANY");
        toolbarsub.AddButton("Other", "EXPERIENCE IN OTHERS");
        toolbarsub.AddButton("Activity Log", "ACTIVITYLOG");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        if (!SessionUtil.CanAccess(this.ViewState, "SUMMARY"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "EXPERIENCEINCOMPANY"))
                Page = "../Crew/CrewNewApplicantCompanyExperience.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "EXPERIENCE IN OTHERS"))
                Page = "../Crew/CrewNewApplicantOtherExperience.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ACTIVITYLOG"))
                Page = "../Crew/CrewNewApplicantActiveLog.aspx";
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateEvaluationSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
       
        toolbarsub.AddButton("Assessment", "ASSESSMENT");
        toolbarsub.AddButton("Appraisal", "APPRISAL");
        toolbarsub.AddButton("About Us By", "ABOUTUSBY");
        toolbarsub.AddButton("Reference", "REFRENCES");

      
        if (!SessionUtil.CanAccess(this.ViewState, "ASSESSMENT"))
        {
            if (SessionUtil.CanAccess(this.ViewState, "APPRISAL"))
                Page = "../Crew/CrewNewApplicantAppraisal.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "ABOUTUSBY"))
                Page = "../Crew/CrewNewApplicantAboutUsBy.aspx";
            else if (SessionUtil.CanAccess(this.ViewState, "REFRENCES"))
                Page = "../Crew/CrewNewApplicantReference.aspx";
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
    private PhoenixToolbar CreateNotesSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Remarks", "SUBREMARKS");
        toolbarsub.AddButton("Important Remarks", "IMPORTANTREMARKS");
      
        Session["USERID"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
        objdiscussion.dtkey = GetCurrentEmployeeDTkey();
        objdiscussion.userid = Convert.ToInt32(Session["USERID"]);
        objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;
        objdiscussion.type = PhoenixCrewConstants.REMARKS;
        PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
}
