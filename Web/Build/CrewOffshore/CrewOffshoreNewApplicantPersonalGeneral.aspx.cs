using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewOffshoreNewApplicantPersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["t"] != null)
            {
                Response.Redirect("../Crew/CrewNewApplicantRegister.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentNewApplicantSelection = Request.QueryString["empid"];
                //Title1.ShowMenu = "false";
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Documents", "DOCUMENTS", CreateDocumentSubTab(string.Empty, 0, string.Empty));
            if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT")
            {
                toolbarmain.AddButton("Evaluation", "EVALUATION", CreateEvaluationSubTab(string.Empty, 0, string.Empty));
            }
            toolbarmain.AddButton("Training ", "RECOMMENDEDCOURSES", CreateRecommendCoursesSubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("History ", "HISTORY", CreateHistorySubTab(string.Empty, 0, string.Empty));
            toolbarmain.AddButton("Experience", "OPERATIONALEXPERIENCE", CreateOperationExperienceSubTab(string.Empty, 0, string.Empty));
            if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT")
            {
                toolbarmain.AddButton("Activity", "ACTIVITY", CreateActivitySubTab(string.Empty, 0, string.Empty));
                toolbarmain.AddButton("Correspondence", "CORRESPONDENCE", CreateCorrespondenceSubTab(string.Empty, 0, string.Empty));
                toolbarmain.AddButton("Remarks", "REMARKS", CreateRemarkSubTab(string.Empty, 0, string.Empty));
            }
            if (Request.QueryString["back"] != null)
            {
                toolbarmain.AddButton("Back", "LIST",ToolBarDirection.Right);
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


                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "selectTab('Main', 'Personal');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

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
                    CreateRecommendCoursesSubTab("../Crew/CrewNewApplicantCourseAndCertificate.aspx", 1, "COURSEANDCERTIFICATES");
                }
                else if (Request.QueryString["remarks"] != null)
                {
                    CreateRemarkSubTab("../Crew/CrewNewApplicantRemarks.aspx", 1, "GENERALREMARKS");
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
                    ucError.ErrorMessage = " Please Select a New Applicant.";
                    ucError.Visible = true;
                }
                //toolbarsub.AddButton("Personal", "SUBPERSONAL");
                //toolbarsub.AddButton("Address ", "ADDRESS");
                //toolbarsub.AddButton("NOK", "FAMILY/NOK");
                //toolbarsub.AddButton("Working Gear", "WORKINGGEAR");
                //CrewMenuGeneral.AccessRights = this.ViewState;
                //CrewMenuGeneral.MenuList = toolbarsub.Show();
                //CrewMenuGeneral.SelectedMenuIndex = 0;
                //ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
                CreatePersonalSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBPERSONAL");
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() != "" && ViewState["crewsearch"].ToString() == "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantList.aspx?p=" + Request.QueryString["p"] + "&launchedfrom=" + ViewState["launchedfrom"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                else if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() == "" && ViewState["crewsearch"].ToString() == "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreQueryActivity.aspx?p=" + Request.QueryString["p"] + "&launchedfrom=" + ViewState["launchedfrom"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                else if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString() != "" && ViewState["NewApplicant"].ToString() == "" && ViewState["crewsearch"].ToString() != "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreEmployee.aspx?sid=" + ViewState["crewsearch"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&launchedfrom=" + ViewState["launchedfrom"].ToString(), true);
                else
                    Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantList.aspx?p=" + Request.QueryString["p"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                //toolbarsub.AddButton("Documents", "ALLDOCUMENTS");
                //toolbarsub.AddButton("Travel", "TRAVELDOCUMENTS");
                //toolbarsub.AddButton("License", "LICENCE");
                //toolbarsub.AddButton("Medical", "MEDICAL");
                //toolbarsub.AddButton("Course", "COURSEANDCERTIFICATES");
                //toolbarsub.AddButton("CBT", "CBTCOURSES");
                //toolbarsub.AddButton("Academic", "ACADEMICS");
                //toolbarsub.AddButton("Award", "AWARDS");
                //toolbarsub.AddButton("Other", "OTHERDOCUMENTS");
                //CrewMenuGeneral.AccessRights = this.ViewState;
                //CrewMenuGeneral.MenuList = toolbarsub.Show();
                //CrewMenuGeneral.SelectedMenuIndex = 0;
                //ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentNewApplicantSelection.ToString();
                CreateDocumentSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBDOCUMENTS");
            }
            else if (CommandName.ToUpper().Equals("EVALUATION"))
            {
                //string canview = "1";
                //DataTable dt = PhoenixCrewManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
                //DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDRANK"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
                //}
                //if (canview.Equals("1"))
                //{
                //    toolbarsub.AddButton("Appraisal", "APPRISAL");
                //}
                //toolbarsub.AddButton("Briefing", "BRIEFING/DEBRIEFING");
                //toolbarsub.AddButton("Assessment", "ASSESSMENT");
                ////toolbarsub.AddButton("Training", "RECOMMENDEDCOURSES");
                //toolbarsub.AddButton("Reference", "REFRENCES");
                //CrewMenuGeneral.AccessRights = this.ViewState;
                //CrewMenuGeneral.MenuList = toolbarsub.Show();
                //CrewMenuGeneral.SelectedMenuIndex = 0;
                //if (canview.Equals("1"))
                //    ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantAppraisal.aspx";
                //else
                //    ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantBriefingDebriefing.aspx";
                CreateEvaluationSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBEVALUATION");
            }
            else if (CommandName.ToUpper().Equals("RECOMMENDEDCOURSES"))
            {
                CreateRecommendCoursesSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBRECOMMENDEDCOURSES");
              
                //toolbarsub.AddButton("Pending", "PENDINGTRAINING");
                //toolbarsub.AddButton("Completed ", "COMPLETEDTRAINING");
                //CrewMenuGeneral.AccessRights = this.ViewState;
                //CrewMenuGeneral.MenuList = toolbarsub.Show();
                //CrewMenuGeneral.SelectedMenuIndex = 0;
                //ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                CreateHistorySubTab("CrewNewApplicantPersonal.aspx", 0, "SUBHISTORY");
               
            }
            //OPERATIONAL EXPERIANCE
            else if (CommandName.ToUpper().Equals("OPERATIONALEXPERIENCE"))
            {
                CreateOperationExperienceSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBOPERATIONALEXPERIENCE");
               
            }
            else if (CommandName.ToUpper().Equals("ACTIVITY"))
            {
                CreateActivitySubTab("CrewNewApplicantPersonal.aspx", 0, "SUBACTIVITY");
              
            }
            else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
            {
                CreateCorrespondenceSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBCORRESPONDENCE");
              
            }
            else if (CommandName.ToUpper().Equals("REMARKS"))
            {
                CreateRemarkSubTab("CrewNewApplicantPersonal.aspx", 0, "SUBREMARKS");

               
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
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //PERSONAL
            if (CommandName.ToUpper().Equals("SUBPERSONAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
            }
            else if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantAddress.aspx";
            }
            else if (CommandName.ToUpper().Equals("WORKINGGEAR"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewWorkingGear.aspx?empid=" + Filter.CurrentNewApplicantSelection.ToString();
            }
            else if (CommandName.ToUpper().Equals("FAMILY/NOK"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantFamilyNok.aspx";
            }

            //DOCUMENTS
            else if (CommandName.ToUpper().Equals("ALLDOCUMENTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentNewApplicantSelection.ToString();
            }
            else if (CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantTravelDocument.aspx";
            }
            else if (CommandName.ToUpper().Equals("LICENCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantLicence.aspx";
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantMedicalDocument.aspx";
            }
            else if (CommandName.ToUpper().Equals("COURSEANDCERTIFICATES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantCourseAndCertificate.aspx";
            }
            else if (CommandName.ToUpper().Equals("CBTCOURSES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantCBTCourses.aspx";
            }
            else if (CommandName.ToUpper().Equals("ACADEMICS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantAcademicQualification.aspx";
            }
            else if (CommandName.ToUpper().Equals("AWARDS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreAwards.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantOtherDocument.aspx";
            }

            //EVALUATION
            else if (CommandName.ToUpper().Equals("APPRISAL"))
            {
                Filter.CurrentAppraisalSelection = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantAppraisal.aspx";
            }
            else if (CommandName.ToUpper().Equals("BRIEFING/DEBRIEFING"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantBriefingDebriefing.aspx";
            }
            else if (CommandName.ToUpper().Equals("ASSESSMENT"))
            {
                Filter.CurrentCrewLaunchedFrom = Request.QueryString["launchedfrom"].ToString();
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreEvaluationList.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&newapp=true";
            }
            else if (CommandName.ToUpper().Equals("REFRENCES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantReference.aspx";
            }

            //TRAINING
            //else if (dce.CommandName.ToUpper().Equals("TRAINING"))
            //{
            //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantRecommendedCourses.aspx";
            //}
            else if (CommandName.ToUpper().Equals("PENDINGTRAINING"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }
            else if (CommandName.ToUpper().Equals("COMPLETEDTRAINING"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreEmployeeCompletedTrainingNeeds.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }

            //HISTORY
            else if (CommandName.ToUpper().Equals("EXPERIENCEINCOMPANY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantCompanyExperience.aspx";
            }
            else if (CommandName.ToUpper().Equals("EXPERIENCEINOTHERS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantOtherExperience.aspx";
            }
            else if (CommandName.ToUpper().Equals("COMBINEDEXPERIENCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreEmployeeExperience.aspx?newapplicant=true";
            }
            else if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewSummary.aspx?newapplicant=true";
            }
            else if (CommandName.ToUpper().Equals("APPOINTMENTHISTORY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreEmployeeAppointmentHistory.aspx?newapplicant=1";
            }
            else if (CommandName.ToUpper().Equals("MEDICALHISTORY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantMedical.aspx";
            }
            //ACTIVITY
            else if (CommandName.ToUpper().Equals("SUITABILITY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?newapplicant=1&crewplanid=" + ViewState["crewplanid"];
            }
            //OPERATIONSAL EXPERIANCE
            else if (CommandName.ToUpper().Equals("OPERATIONALCOEXPERIENCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crewoffshore/CrewoffshoreOperationalCompanyExp.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }
            else if (CommandName.ToUpper().Equals("OPERATIONALEXPERIENCEINOTHERS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crewoffshore/CrewoffshoreOpreationalOtherCompanyExp.aspx?empid=" + Filter.CurrentNewApplicantSelection;
            }
            else if (CommandName.ToUpper().Equals("AVAILABILITY"))
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
            else if (CommandName.ToUpper().Equals("PROPOSAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreProposalHistory.aspx?newapplicant=1";
            }

            //CORRESPONDENCE
            else if (CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantCorrespondence.aspx";
            }
            //REMARKS
            else if (CommandName.ToUpper().Equals("IMPORTANTREMARKS"))
            {

                Session["USERID"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
                objdiscussion.dtkey = GetCurrentEmployeeDTkey();
                objdiscussion.userid = Convert.ToInt32(Session["USERID"]);
                objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;

                objdiscussion.type = PhoenixCrewConstants.REMARKS;
                PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantImportantRemarks.aspx";
            }
            else if (CommandName.ToUpper().Equals("SUBREMARKS"))
            {

                Session["USERID"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
                objdiscussion.dtkey = GetCurrentEmployeeDTkey();
                objdiscussion.userid = Convert.ToInt32(Session["USERID"]);
                objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;

                objdiscussion.type = PhoenixCrewConstants.REMARKS;
                PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewNewApplicantRemarks.aspx";
            }


            if (Filter.CurrentNewApplicantSelection == null)
            {
                ucError.ErrorMessage = " Please Select a New Applicant.";
                ucError.Visible = true;
                ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
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

     
        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        toolbarsub.AddButton("Address ", "ADDRESS");
        toolbarsub.AddButton("Family NOK ", "FAMILY/NOK");
        toolbarsub.AddButton("Working Gear", "WORKINGGEAR");

        //toolbarsub.AddButton("Personal", "SUBPERSONAL");
        //toolbarsub.AddButton("Address ", "ADDRESS");
        //toolbarsub.AddButton("NOK", "FAMILY/NOK");
        //toolbarsub.AddButton("Working Gear", "WORKINGGEAR");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = 0;
        //ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantPersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty);
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateDocumentSubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Documents", "ALLDOCUMENTS");
        toolbarsub.AddButton("Travel", "TRAVELDOCUMENTS");
        toolbarsub.AddButton("License", "LICENCE");
        toolbarsub.AddButton("Medical", "MEDICAL");
        toolbarsub.AddButton("Course", "COURSEANDCERTIFICATES");
        toolbarsub.AddButton("CBT", "CBTCOURSES");
        toolbarsub.AddButton("Academic", "ACADEMICS");
        toolbarsub.AddButton("Award", "AWARDS");
        toolbarsub.AddButton("Other", "OTHERDOCUMENTS");
      
        ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentNewApplicantSelection.ToString();
        return toolbarsub;
    }
    private PhoenixToolbar CreateEvaluationSubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        string canview = "1";
        DataTable dt = PhoenixCrewManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
        DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDRANK"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));
        if (ds.Tables[0].Rows.Count > 0)
        {
            canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
        }
        if (canview.Equals("1"))
        {
            toolbarsub.AddButton("Appraisal", "APPRISAL");
        }
        toolbarsub.AddButton("Briefing", "BRIEFING/DEBRIEFING");
        toolbarsub.AddButton("Assessment", "ASSESSMENT");
        //toolbarsub.AddButton("Training", "RECOMMENDEDCOURSES");
        toolbarsub.AddButton("Reference", "REFRENCES");
       
        if (canview.Equals("1"))
            ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantAppraisal.aspx";
        else
            ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantBriefingDebriefing.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateRecommendCoursesSubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Pending", "PENDINGTRAINING");
        toolbarsub.AddButton("Completed ", "COMPLETEDTRAINING");
       
        ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?empid=" + Filter.CurrentNewApplicantSelection;
        return toolbarsub;
    }
    private PhoenixToolbar CreateHistorySubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Company", "EXPERIENCEINCOMPANY");
        toolbarsub.AddButton("Other ", "EXPERIENCEINOTHERS");
        toolbarsub.AddButton("Experience ", "COMBINEDEXPERIENCE");
        toolbarsub.AddButton("Summary", "SUMMARY");
        toolbarsub.AddButton("Appointment", "APPOINTMENTHISTORY");
        toolbarsub.AddButton("Medical", "MEDICALHISTORY");
    
        ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantCompanyExperience.aspx";
        return toolbarsub;
    }
    private PhoenixToolbar CreateOperationExperienceSubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Company", "OPERATIONALCOEXPERIENCE");
        toolbarsub.AddButton("Other", "OPERATIONALEXPERIENCEINOTHERS");

        ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewoffshoreOperationalCompanyExp.aspx?empid=" + Filter.CurrentNewApplicantSelection.ToString();
        return toolbarsub;
    }
    private PhoenixToolbar CreateActivitySubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        if (Filter.CurrentNewApplicantSelection != null && Filter.CurrentNewApplicantSelection != "")
        {
           
            toolbarsub.AddButton("Suitability", "SUITABILITY");
            toolbarsub.AddButton("Activities", "AVAILABILITY");
            toolbarsub.AddButton("Proposals", "PROPOSAL");
          
            ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?newapplicant=1&crewplanid=" + ViewState["crewplanid"];
            
        }
        return toolbarsub;
    }
    private PhoenixToolbar CreateCorrespondenceSubTab(string page, int MenuIndex, string Cmd)
    {

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Correspondence", "SUBCORRESPONDENCE");
       
        ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantCorrespondence.aspx";
        return toolbarsub;
    }

    private PhoenixToolbar CreateRemarkSubTab(string page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Remarks", "SUBREMARKS");
        toolbarsub.AddButton("Important", "IMPORTANTREMARKS");
       
        PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
        objdiscussion.dtkey = GetCurrentEmployeeDTkey();
        objdiscussion.userid = Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;
        objdiscussion.type = PhoenixCrewConstants.GENERALREMARKS;
        PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

        ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantRemarks.aspx";
        return toolbarsub;
    }
}
