using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;

public partial class CrewOffshoreEmployeeGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
            Title1.ShowMenu = "false";
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL");
        toolbarmain.AddButton("History ", "HISTORY");
        toolbarmain.AddButton("Training", "TRAINING");
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT")
        {
            toolbarmain.AddButton("Evaluation", "EVALUATION");
            toolbarmain.AddButton("Notes", "NOTES");
            toolbarmain.AddButton("Corres.", "CORRESPONDENCE");
            toolbarmain.AddButton("Activity Log", "ACTIVITYLOG");
        }
        if (Request.QueryString["back"] != null)
        {
            toolbarmain.AddButton("Back", "LIST");
        }
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["ACCESS"] = "0";
            if (General.GetNullableInteger(Filter.CurrentCrewSelection).HasValue)
                ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();

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

                CreateHistorySubTab("../Crew/CrewLicence.aspx?cid=" + Request.QueryString["cid"], 1, "LICENCE");               

                return;
            }
            else if (Request.QueryString["med"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewMedicalDocument.aspx", 5, "MEDICAL");                
            }
            else if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], 2, "FAMILY/NOK");
            }
            else if (Request.QueryString["evaluation"] != null)
            {
                CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEvaluationList.aspx", 0, "ASSESSMENT");               
            }
            else if (Request.QueryString["documents"] != null)
            {
                CreatePersonalSubTab("../Crew/CrewTravelDocument.aspx", 3, "TRAVELDOCUMENTS");
            }
            else if (Request.QueryString["appraisal"] != null)
            {
                CreateHistorySubTab("../Crew/CrewAppraisal.aspx?empid=" + Request.QueryString["empid"], 4, "APPRISAL");                

                return;
            }
			else if (Request.QueryString["recommendedcourse"] != null)
			{
				CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");
			}
            else
            {
                CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
            }
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (Filter.CurrentCrewSelection == null || dce.CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (Filter.CurrentCrewSelection == null && Request.QueryString["t"] == null)
            {
                ucError.ErrorMessage = "  Please Select a Employee .";
                ucError.Visible = true;
                CrewMenu.SelectedMenuIndex = 0;
            }
            CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");            
        }
        //if (Filter.CurrentCrewSelection == null)
        //{
        //    ucError.ErrorMessage = " Please Select a Employee ";
        //    ucError.Visible = true;
        //    Response.Redirect("../Crew/CrewPersonalGeneral.aspx");
        //}
        else if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Crew/CrewQueryActivity.aspx?p=" + Request.QueryString["p"], false);
        }
        else if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            CreateHistorySubTab("../Crew/CrewAcademicQualification.aspx", 0, "ACADEMICS");            
        }
        else if (dce.CommandName.ToUpper().Equals("EVALUATION"))
        {
            CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEvaluationList.aspx", 0, "ASSESSMENT");
        }
        else if (dce.CommandName.ToUpper().Equals("TRAINING"))
        {
            CreateTrainingSubTab("../Crew/CrewCourseAndCertificate.aspx", 0, "COURSEANDCERTIFICATES");            
        }
        else if (dce.CommandName.ToUpper().Equals("NOTES"))
        {
            CreateNotesSubTab("../Crew/CrewRemarks.aspx", 0, "GENERALREMARKS");            
        }
        else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondence.aspx", 0, "SUBCORRESPONDENCE");           
        }
        else if (dce.CommandName.ToUpper().Equals("ACTIVITYLOG"))
        {
            CreateActivityLogSubTab("../Crew/CrewActivityLog.aspx", 0, "SUBACTIVITYLOG");
        }
		else if (dce.CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
		{
			CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");
		}
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if ((Filter.CurrentCrewSelection == null && Request.QueryString["t"] == null) || (Filter.CurrentCrewSelection == null && dce.CommandName.ToUpper() != "PERSONAL"))
        {
            ucError.ErrorMessage = " Please Select a Employee ";
            ucError.Visible = true;
            CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (dce.CommandName.ToUpper().Equals("SUBPERSONAL"))
        {
            CreatePersonalSubTab("../Crew/CrewPersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            CreatePersonalSubTab("../Crew/CrewAddress.aspx", 1, "ADDRESS");            
        }
        else if (dce.CommandName.ToUpper().Equals("FAMILY/NOK"))
        {
            CreatePersonalSubTab("../Crew/CrewFamilyNok.aspx", 2, "FAMILY/NOK");            
        }
        else if (dce.CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
        {
            CreatePersonalSubTab("../Crew/CrewTravelDocument.aspx", 3, "TRAVELDOCUMENTS");            
        }
        else if (dce.CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
        {
            CreatePersonalSubTab("../Crew/CrewOtherDocument.aspx", 4, "OTHERDOCUMENTS");            
        }
        else if (dce.CommandName.ToUpper().Equals("MEDICAL"))
        {
            CreatePersonalSubTab("../Crew/CrewMedicalDocument.aspx", 5, "MEDICAL");            
        }
        else if (dce.CommandName.ToUpper().Equals("MEDICALHISTORY"))
        {
            CreatePersonalSubTab("../Crew/CrewMedical.aspx", 6, "MEDICALHISTORY");            
        }
        else if (dce.CommandName.ToUpper().Equals("STATUS"))
        {
            CreatePersonalSubTab("../Crew/CrewStatus.aspx", 7, "STATUS");            
        }
        else if (dce.CommandName.ToUpper().Equals("BANKACCT"))
        {
            CreatePersonalSubTab("../Crew/CrewBankAccount.aspx", 8, "BANKACCT");            
        }
        else if (dce.CommandName.ToUpper().Equals("ASSESSMENT"))
        {
            CreateEvaluationSubTab("../CrewOffshore/CrewOffshoreEvaluationList.aspx", 0, "ASSESSMENT");            
        }
        else if (dce.CommandName.ToUpper().Equals("ABOUT US BY"))
        {
            CreateEvaluationSubTab("../Crew/CrewAboutUsBy.aspx", 1, "ABOUT US BY");            
        }
        else if (dce.CommandName.ToUpper().Equals("REFRENCES"))
        {
            CreateEvaluationSubTab("../Crew/CrewReference.aspx", 2, "REFRENCES");            
        }
        else if (dce.CommandName.ToUpper().Equals("UNALLOCATEDVESSELEXP"))
        {
            string employeeid = Filter.CurrentCrewSelection;
            CreateEvaluationSubTab("../Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + int.Parse(employeeid), 2, "UNALLOCATEDVESSELEXP");
        }
        else if (dce.CommandName.ToUpper().Equals("ACADEMICS"))
        {
            CreateHistorySubTab("../Crew/CrewAcademicQualification.aspx", 0, "ACADEMICS");            
        }        
        else if (dce.CommandName.ToUpper().Equals("COURSEANDCERTIFICATES"))
        {
            CreateTrainingSubTab("../Crew/CrewCourseAndCertificate.aspx", 0, "COURSEANDCERTIFICATES");            
        }
        else if (dce.CommandName.ToUpper().Equals("CBTCOURSES"))
        {
            CreateTrainingSubTab("../Crew/CrewCBTCourses.aspx", 1, "CBTCOURSES");            
        }
        else if (dce.CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
        {
            CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");            
        }
        else if (dce.CommandName.ToUpper().Equals("ONBOARDTRAINING"))
        {
            CreateTrainingSubTab("../Crew/CrewOnboardTraining.aspx", 3, "ONBOARDTRAINING");            
        }
        else if (dce.CommandName.ToUpper().Equals("BRIEFING/DEBRIEFING"))
        {
            CreateTrainingSubTab("../Crew/CrewBriefingDebriefing.aspx", 4, "BRIEFING/DEBRIEFING");            
        }
        else if (dce.CommandName.ToUpper().Equals("EXPERIENCE IN OTHERS"))
        {
            CreateHistorySubTab("../Crew/CrewOtherExperience.aspx", 2, "EXPERIENCE IN OTHERS");            
        }
        else if (dce.CommandName.ToUpper().Equals("EXPERIENCEINCOMPANY"))
        {
            CreateHistorySubTab("../Crew/CrewCompanyExperience.aspx", 3, "EXPERIENCEINCOMPANY");            
        }
        else if (dce.CommandName.ToUpper().Equals("APPRISAL"))
        {
            Filter.CurrentAppraisalSelection = null;
            CreateHistorySubTab("../Crew/CrewAppraisal.aspx", 4, "APPRISAL");            
        }
        else if (dce.CommandName.ToUpper().Equals("SUMMARY"))
        {
            CreateHistorySubTab("../Crew/CrewSummary.aspx?personalmaster=true", 5, "SUMMARY");            
        }                    
        else if (dce.CommandName.ToUpper().Equals("LICENCE"))
        {
            CreateHistorySubTab("../Crew/CrewLicence.aspx", 1, "LICENCE");            
        }
		else if (dce.CommandName.ToUpper().Equals("TRAINING"))
		{
			CreateTrainingSubTab("../Crew/CrewRecommendedCourses.aspx", 2, "RECOMMENDEDCOURSES");   
		}
		else if (dce.CommandName.ToUpper().Equals("SUBACTIVITYLOG"))
        {
            CreateActivityLogSubTab("../Crew/CrewActivityLog.aspx", 0, "SUBACTIVITYLOG");            
        }                
        else if (dce.CommandName.ToUpper().Equals("GENERALREMARKS"))
        {
            CreateNotesSubTab("../Crew/CrewRemarks.aspx", 0, "GENERALREMARKS");            
        }
        else if (dce.CommandName.ToUpper().Equals("IMPORTANTREMARKS"))
        {
            CreateNotesSubTab("../Crew/CrewImportantRemarks.aspx", 1, "IMPORTANTREMARKS");            
        }              
        else if (dce.CommandName.ToUpper().Equals("EMAIL"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondenceEmail.aspx", 1, "EMAIL");            
        }
        else if (dce.CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../Crew/CrewCorrespondence.aspx", 0, "SUBCORRESPONDENCE");            
        }
        //ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
    }
    private void CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 0;       
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        if (ViewState["ACCESS"].ToString() == "1")
        {
            toolbarsub.AddButton("Address ", "ADDRESS");
            toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
        }
        toolbarsub.AddButton("Documents", "TRAVELDOCUMENTS");
        toolbarsub.AddButton("Othr. Documents", "OTHERDOCUMENTS");
        toolbarsub.AddButton("Medical", "MEDICAL");
        toolbarsub.AddButton("Med History", "MEDICALHISTORY");
        toolbarsub.AddButton("Status", "STATUS");
        toolbarsub.AddButton("Bank Acct.", "BANKACCT");
        CrewMenuGeneral.TabStrip = "true";
        CrewMenuGeneral.AccessRights = this.ViewState;        
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    private void CreateNotesSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 4;

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        //toolbarsub.AddButton("Scratch Pad ", "SCRATCHPAD");
        //toolbarsub.AddButton("CRM", "CRM");
        toolbarsub.AddButton("General Remarks", "GENERALREMARKS");
        toolbarsub.AddButton("Important Remarks", "IMPORTANTREMARKS");
        //toolbarsub.AddButton("Electrical", "ELECTRICAL");
        //toolbarsub.AddButton("Summary", "SUMMARY");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();

        PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
        objdiscussion.dtkey = GetCurrentEmployeeDTkey();
        objdiscussion.userid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;
        objdiscussion.type = PhoenixCrewConstants.REMARKS;
        PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    private void CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 1;

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Academics", "ACADEMICS");
        toolbarsub.AddButton("Licence", "LICENCE");
        // toolbarsub.AddButton("Course", "COURSEANDCERTIFICATES");
        toolbarsub.AddButton("Other Exp ", "EXPERIENCE IN OTHERS");
        toolbarsub.AddButton("Company Exp", "EXPERIENCEINCOMPANY");

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
        toolbarsub.AddButton("Summary", "SUMMARY");
        //toolbarsub.AddButton("TravelRequired", "TRAVELREQUIRED");
        //toolbarsub.AddButton("Working History", "CONTRACTANDRANKHISTORY");                       
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;

        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        
    }
    private void CreateTrainingSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 2;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Course", "COURSEANDCERTIFICATES");
        toolbarsub.AddButton("CBT Courses", "CBTCOURSES");
        toolbarsub.AddButton("Recommended Courses", "RECOMMENDEDCOURSES");
        // toolbarsub.AddButton("Training", "TRAINING");
        toolbarsub.AddButton("On Board", "ONBOARDTRAINING");
        toolbarsub.AddButton("Briefing", "BRIEFING/DEBRIEFING");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    private void CreateEvaluationSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 3;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Assessment", "ASSESSMENT");
        toolbarsub.AddButton("About Us By", "ABOUT US BY");
        toolbarsub.AddButton("Reference", "REFRENCES");
        toolbarsub.AddButton("Unallocated Vsl.Exp", "UNALLOCATEDVESSELEXP");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    private void CreateCorrespondenceSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 5;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Corres.", "SUBCORRESPONDENCE");
        toolbarsub.AddButton("Email", "EMAIL");
        //toolbarsub.AddButton("Inbox ", "INBOX");
        //toolbarsub.AddButton("Sent Mails", "SENT MAILS");
        //toolbarsub.AddButton("Drafts", "DRAFTS");
        //toolbarsub.AddButton("Trash", "DELETEMAIL");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
    }
    private void CreateActivityLogSubTab(string Page, int MenuIndex, string Cmd)
    {
        CrewMenu.SelectedMenuIndex = 6;
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Activity Log", "SUBACTIVITYLOG");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();
        CrewMenuGeneral.SelectedMenuIndex = MenuIndex;
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
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
