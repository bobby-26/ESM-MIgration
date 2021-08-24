using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshorePortalPersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["launchedfrom"] = "";
            ViewState["pl"] = "";
            ViewState["crewsearch"] = "";
            ViewState["crewplanid"] = "";

           

            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                ViewState["pl"] = Request.QueryString["pl"].ToString();

            if (Request.QueryString["crewsearch"] != null && Request.QueryString["crewsearch"] != "")
                ViewState["crewsearch"] = Request.QueryString["crewsearch"].ToString();

            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

            ViewState["ACCESS"] = "0";
            if (General.GetNullableInteger(Filter.CurrentCrewSelection).HasValue)
                ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value);


        }
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
            // Title1.ShowMenu = "false";
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Documents", "DOCUMENTS", CreateDocumentSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Event", "EVENT", ToolBarDirection.Left);
     
        if (Request.QueryString["back"] != null)
        {
            toolbarmain.AddButton("Back", "LIST", ToolBarDirection.Right);
        }
        //CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx?portal=1", 0, "SUBPERSONAL");
          
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
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx?portal=1", 0, "SUBPERSONAL");
            else
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["crewsearch"].ToString() == "")
                Response.Redirect("../CrewOffshore/CrewOffshoreQueryActivity.aspx?p=" + Request.QueryString["p"] + "&launchedfrom=" + ViewState["launchedfrom"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
            else if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["crewsearch"].ToString() != "")
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployee.aspx?sid=" + ViewState["crewsearch"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&launchedfrom=" + ViewState["launchedfrom"].ToString(), false);
            else
                Response.Redirect("../Crew/CrewQueryActivity.aspx?p=" + Request.QueryString["p"], false);
        }
        else if (CommandName.ToUpper().Equals("DOCUMENTS"))
        {
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() != "")
                CreateDocumentSubTab("../CrewOffshore/CrewOffshorePortalDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection, 0, "ALLDOCUMENTS");
            else
                CreateDocumentSubTab("../CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection, 0, "ALLDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("EVALUATION"))
        {
            Filter.CurrentAppraisalSelection = null;
            CreateEvaluationSubTab("../Crew/CrewAppraisal.aspx", 0, "APPRISAL");
        }

        else if (CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
        {
            CreateTrainingSubTab("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?empid=" + Filter.CurrentCrewSelection, 0, "PENDINGTRAINING");
        }
        else if (CommandName.ToUpper().Equals("HISTORY"))
        {
            CreateHistorySubTab("../Crew/CrewCompanyExperience.aspx", 0, "EXPERIENCEINCOMPANY");
        }
        else if (CommandName.ToUpper().Equals("OPERATIONALEXPERIENCE"))
        {
            CreateOperationalSubTab("../CrewOffshore/CrewoffshoreOperationalCompanyExp.aspx?empid=" + Filter.CurrentCrewSelection, 0, "OPERATIONALEXPERIENCE");
        }
        else if (CommandName.ToUpper().Equals("ACTIVITY"))
        {
            CreateActivitySubTab("../CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?personalmaster=1&crewplanid=" + ViewState["crewplanid"], 0, "SUITABILITY");
        }
        else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondence.aspx", 0, "SUBCORRESPONDENCE");
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

        //PERSONAL

        if ((Filter.CurrentCrewSelection == null && Request.QueryString["t"] == null) || (Filter.CurrentCrewSelection == null && CommandName.ToUpper() != "PERSONAL"))
        {
            ucError.ErrorMessage = " Please Select a Employee ";
            ucError.Visible = true;
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx?portal=1", 0, "SUBPERSONAL");
            else
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("SUBPERSONAL"))
        {
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx?portal=1", 0, "SUBPERSONAL");
            else
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("ADDRESS"))
        {
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                CreatePersonalSubTab("../Crew/CrewAddress.aspx?portal=1", 0, "ADDRESS");
            else
                CreatePersonalSubTab("../Crew/CrewAddress.aspx", 1, "ADDRESS");
        }
        else if (CommandName.ToUpper().Equals("FAMILY/NOK"))
        {
            CreatePersonalSubTab("../Crew/CrewFamilyNok.aspx", 2, "FAMILY/NOK");
        }
        else if (CommandName.ToUpper().Equals("WORKINGGEAR"))
        {
            string employeeid = Filter.CurrentCrewSelection;
            CreatePersonalSubTab("../Crew/CrewWorkingGear.aspx?empid=" + int.Parse(employeeid), 3, "WORKINGGEAR");
        }
        else if (CommandName.ToUpper().Equals("BANKACCT"))
        {
            CreatePersonalSubTab("../Crew/CrewBankAccount.aspx", 4, "BANKACCT");
        }

        //DOCUMENTS

        else if (CommandName.ToUpper().Equals("ALLDOCUMENTS"))
        {
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() != "")
                CreateDocumentSubTab("../CrewOffshore/CrewOffshorePortalDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection, 0, "ALLDOCUMENTS");
            else
                CreateDocumentSubTab("../CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection, 0, "ALLDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
        {
            CreateDocumentSubTab("../Crew/CrewTravelDocument.aspx", 1, "TRAVELDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("TRAVELPLAN"))
        {
            CreateDocumentSubTab("../Crew/CrewTravelPlanDocument.aspx", 2, "TRAVELPLAN");
        }
        else if (CommandName.ToUpper().Equals("LICENCE"))
        {
            CreateDocumentSubTab("../Crew/CrewLicence.aspx", 3, "LICENCE");
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            CreateDocumentSubTab("../Crew/CrewMedicalDocument.aspx", 4, "MEDICAL");
        }
        else if (CommandName.ToUpper().Equals("COURSEANDCERTIFICATES"))
        {
            CreateDocumentSubTab("../Crew/CrewCourseAndCertificate.aspx", 5, "COURSEANDCERTIFICATES");
        }
        else if (CommandName.ToUpper().Equals("CBTCOURSES"))
        {
            CreateDocumentSubTab("../Crew/CrewCBTCourses.aspx", 6, "CBTCOURSES");
        }
        else if (CommandName.ToUpper().Equals("ACADEMICS"))
        {
            CreateDocumentSubTab("../Crew/CrewAcademicQualification.aspx", 7, "ACADEMICS");
        }
        else if (CommandName.ToUpper().Equals("AWARDS"))
        {
            CreateDocumentSubTab("../CrewOffshore/CrewOffshoreAwards.aspx?empid=" + Filter.CurrentCrewSelection, 8, "AWARDS");
        }
        else if (CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
        {
            CreateDocumentSubTab("../Crew/CrewOtherDocument.aspx", 9, "OTHERDOCUMENTS");
        }

        //EVALUATION
        else if (CommandName.ToUpper().Equals("APPRISAL"))
        {
            Filter.CurrentAppraisalSelection = null;
            CreateEvaluationSubTab("../Crew/CrewAppraisal.aspx", 0, "APPRISAL");
        }
        else if (CommandName.ToUpper().Equals("BRIEFING/DEBRIEFING"))
        {
            CreateEvaluationSubTab("../Crew/CrewBriefingDebriefing.aspx", 1, "BRIEFING/DEBRIEFING");
        }
        else if (CommandName.ToUpper().Equals("ASSESSMENT"))
        {
            CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEvaluationList.aspx?empid=" + Filter.CurrentCrewSelection, 2, "ASSESSMENT");
        }
        else if (CommandName.ToUpper().Equals("REFRENCES"))
        {
            CreateEvaluationSubTab("../Crew/CrewReference.aspx", 3, "REFRENCES");
        }
        else if (CommandName.ToUpper().Equals("PROMOTIONSUMMARY"))
        {
            CreateEvaluationSubTab("../Crew/CrewPromotionSummary.aspx", 4, "PROMOTIONSUMMARY");
        }
        else if (CommandName.ToUpper().Equals("SUPTEVAL"))
        {
            CreateEvaluationSubTab("../Crew/CrewEmployeeSuptEvalForm.aspx", 5, "SUPTEVAL");
        }
        else if (CommandName.ToUpper().Equals("EVAPERSONAL"))
        {
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1)
                CreateEvaluationSubTab("../Crew/CrewInspectionSupdtConcernsList.aspx", 2, "EVAPERSONAL");
            else
                CreateEvaluationSubTab("../Crew/CrewEmployeeSuperintendentAssessment.aspx", 2, "EVAPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("EVENT"))
        {
            Filter.CurrentAppraisalSelection = null;
            CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEmployeeProsperScore.aspx?empid=" + Filter.CurrentCrewSelection + "&portal=1", 0, "PROSPER");
        }
        else if (CommandName.ToUpper().Equals("INCIDENT"))
        {
            Filter.CurrentAppraisalSelection = null;
            CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEmployeeIncidentScore.aspx?empid=" + Filter.CurrentCrewSelection + "&portal=1", 0, "PROSPER");
        }
        
        //TRAINING

        else if (CommandName.ToUpper().Equals("SUBRECOMMENDEDCOURSES"))
        {
            CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 0, "SUBRECOMMENDEDCOURSES");
        }
        else if (CommandName.ToUpper().Equals("PENDINGTRAINING"))
        {
            CreateTrainingSubTab("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?empid=" + Filter.CurrentCrewSelection, 0, "PENDINGTRAINING");
        }
        else if (CommandName.ToUpper().Equals("COMPLETEDTRAINING"))
        {
            CreateTrainingSubTab("../CrewOffshore/CrewOffshoreEmployeeCompletedTrainingNeeds.aspx?empid=" + Filter.CurrentCrewSelection, 1, "COMPLETEDTRAINING");
        }

        //HISTORY
        else if (CommandName.ToUpper().Equals("EXPERIENCEINCOMPANY"))
        {
            CreateHistorySubTab("../Crew/CrewCompanyExperience.aspx", 0, "EXPERIENCEINCOMPANY");
        }
        else if (CommandName.ToUpper().Equals("EXPERIENCEINOTHERS"))
        {
            CreateHistorySubTab("../Crew/CrewOtherExperience.aspx", 1, "EXPERIENCEINOTHERS");
        }
        else if (CommandName.ToUpper().Equals("COMBINEDEXPERIENCE"))
        {
            CreateHistorySubTab("../CrewOffshore/CrewOffshoreEmployeeExperience.aspx", 2, "COMBINEDEXPERIENCE");
        }
        else if (CommandName.ToUpper().Equals("SUMMARY"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "Phoenix")
            {
                CreateHistorySubTab("../Crew/CrewSummary.aspx?personalmaster=true", 3, "SUMMARY");
            }
            else
            {
                CreateHistorySubTab("../CrewOffshore/CrewOffshoreSummary.aspx?personalmaster=true", 3, "SUMMARY");
            }
        }
        else if (CommandName.ToUpper().Equals("OFFERLETTERHISTORY"))
        {
            CreateHistorySubTab("../CrewOffshore/CrewOffshoreEmployeeOfferLetterHistory.aspx?personalmaster=true", 4, "OFFERLETTERHISTORY");
        }
        else if (CommandName.ToUpper().Equals("APPOINTMENTHISTORY"))
        {
            CreateHistorySubTab("../CrewOffshore/CrewOffshoreEmployeeAppointmentHistory.aspx?personalmaster=true", 5, "APPOINTMENTHISTORY");
        }
        else if (CommandName.ToUpper().Equals("MEDICALHISTORY"))
        {
            CreateHistorySubTab("../Crew/CrewMedical.aspx", 6, "MEDICALHISTORY");
        }
        //OPERATIONAL EXPERIENCE
        else if (CommandName.ToUpper().Equals("OPERATIONALEXPERIENCEINOTHERS"))
        {
            CreateOperationalSubTab("../Crewoffshore/CrewoffshoreOpreationalOtherCompanyExp.aspx?empid=" + Filter.CurrentCrewSelection, 0, "OPERATIONALEXPERIENCEINOTHERS");
        }
        else if (CommandName.ToUpper().Equals("OPERATIONALCOEXPERIENCE"))
        {
            CreateOperationalSubTab("../Crewoffshore/CrewoffshoreOperationalCompanyExp.aspx?empid=" + Filter.CurrentCrewSelection, 1, "OPERATIONALCOEXPERIENCE");
        }
        //ACTIVITY
        else if (CommandName.ToUpper().Equals("SUITABILITY"))
        {
            CreateActivitySubTab("../CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?personalmaster=1&crewplanid=" + ViewState["crewplanid"], 0, "SUITABILITY");
        }
        else if (CommandName.ToUpper().Equals("AVAILABILITY"))
        {
            if (Filter.CurrentCrewSelection != null && Filter.CurrentCrewSelection != "")
            {
                DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentCrewSelection));
                string status = dt.Rows[0]["FLDSTATUSNAME"].ToString();
                if (ViewState["launchedfrom"].ToString() != "")
                {
                    if (status != null && (status.Contains("ONB") || status.Contains("OBP")))
                        CreateActivitySubTab("../Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ntbr=0&ds=" + dt.Rows[0]["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore", 1, "AVAILABILITY");//
                    else
                        CreateActivitySubTab("../Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ds=" + dt.Rows[0]["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore", 1, "AVAILABILITY");
                }
                else
                {
                    if (status != null && (status.Contains("ONB") || status.Contains("OBP")))
                        CreateActivitySubTab("../Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ntbr=0&ds=" + dt.Rows[0]["FLDDIRECTSIGNON"].ToString(), 1, "AVAILABILITY");//
                    else
                        CreateActivitySubTab("../Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ds=" + dt.Rows[0]["FLDDIRECTSIGNON"].ToString(), 1, "AVAILABILITY");
                }
            }
        }
        else if (CommandName.ToUpper().Equals("SUBACTIVITYLOG"))
        {
            CreateActivitySubTab("../CrewOffshore/CrewOffshoreActivityLog.aspx", 2, "SUBACTIVITYLOG");
        }
        else if (CommandName.ToUpper().Equals("PROPOSAL"))
        {
            CreateActivitySubTab("../CrewOffshore/CrewOffshoreProposalHistory.aspx", 3, "PROPOSAL");
        }

        //CORRESPONDENCE
        else if (CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondence.aspx", 0, "SUBCORRESPONDENCE");
        }
        else if (CommandName.ToUpper().Equals("NOTES"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewRemarks.aspx", 1, "GENERALREMARKS");
        }

    }
    //private void CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    //{
    //    CrewMenu.SelectedMenuIndex = 0;
    //    PhoenixToolbar toolbarsub = new PhoenixToolbar();
    //    toolbarsub.AddButton("Personal", "SUBPERSONAL");
    //    if (ViewState["ACCESS"].ToString() == "1")
    //    {
    //        toolbarsub.AddButton("Address ", "ADDRESS");
    //        toolbarsub.AddButton("NOK", "FAMILY/NOK");
    //    }
    //    toolbarsub.AddButton("Working Gear", "WORKINGGEAR");
    //    toolbarsub.AddButton("Bank Account", "BANKACCT");
    //    CrewMenuGeneral.TabStrip = true;
    //    CrewMenuGeneral.AccessRights = this.ViewState;
    //    CrewMenuGeneral.MenuList = toolbarsub.Show();
    //    CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
    //    ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    //}
    private PhoenixToolbar CreateDocumentSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 1;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Documents", "ALLDOCUMENTS");       
        toolbarsub.AddButton("Travel Plan", "TRAVELPLAN");      
        toolbarsub.AddButton("Academic", "ACADEMICS");
        toolbarsub.AddButton("Award", "AWARDS");      
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;


        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateEvaluationSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 2;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
       
        toolbarsub.AddButton("Prosper", "PROSPER");
        toolbarsub.AddButton("Incident", "INCIDENT");
        toolbarsub.AddButton("Inspection", "INSPECTION");
        toolbarsub.AddButton("Vetting", "VETTING");


        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    protected PhoenixToolbar CreateTrainingSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 3;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Recommended Course", "SUBRECOMMENDEDCOURSES");
        toolbarsub.AddButton("Pending", "PENDINGTRAINING");
        toolbarsub.AddButton("Completed", "COMPLETEDTRAINING");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    protected PhoenixToolbar CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 4;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Company", "EXPERIENCEINCOMPANY");
        toolbarsub.AddButton("Other", "EXPERIENCEINOTHERS");
        toolbarsub.AddButton("Experience ", "COMBINEDEXPERIENCE");
        toolbarsub.AddButton("Summary", "SUMMARY");
        toolbarsub.AddButton("Offer Letter ", "OFFERLETTERHISTORY");
        toolbarsub.AddButton("App. Letter", "APPOINTMENTHISTORY");
        toolbarsub.AddButton("Medical", "MEDICALHISTORY");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    protected PhoenixToolbar CreateOperationalSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 5;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Company", "OPERATIONALCOEXPERIENCE");
        toolbarsub.AddButton("Other", "OPERATIONALEXPERIENCEINOTHERS");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateActivitySubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 6;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Suitability", "SUITABILITY");
        toolbarsub.AddButton("Activities", "AVAILABILITY");
        toolbarsub.AddButton("Activity Log", "SUBACTIVITYLOG");
        toolbarsub.AddButton("Proposals", "PROPOSAL");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateCorrespondenceSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 7;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Correspondence", "SUBCORRESPONDENCE");
        toolbarsub.AddButton("Notes", "NOTES");
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
    private PhoenixToolbar CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 0;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        if (ViewState["ACCESS"].ToString() == "1")
        {
            toolbarsub.AddButton("Address ", "ADDRESS");
           // toolbarsub.AddButton("NOK", "FAMILY/NOK");
        }
        toolbarsub.AddButton("Working Gear", "WORKINGGEAR");
        toolbarsub.AddButton("Bank Account", "BANKACCT");
        // CrewMenuGeneral.TabStrip = true;
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
}