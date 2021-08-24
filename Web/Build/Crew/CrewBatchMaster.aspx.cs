using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewBatchMaster : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Batch List", "BATCH");
        toolbar.AddButton("Batch Details", "BATCHDETAILS");
        //toolbar.AddButton("Registration", "REGISTRATION");
        toolbar.AddButton("Registration", "PARTICIPANTLIST");
        toolbar.AddButton("Attendance", "ATTENDANCE");
        toolbar.AddButton("Assessment", "ASSESSMENT");
        toolbar.AddButton("Certification", "CERTIFICATE");
        toolbar.AddButton("Feedback", "FEEDBACK");
        MenuBatchMaster.MenuList = toolbar.Show();
        if (!IsPostBack)
		{
            if (Request.QueryString["calledfrom"] != null)
			{
				if (Request.QueryString["calledfrom"].ToUpper() == "TRAINING")
				{

					MenuBatchMaster.SelectedMenuIndex = 1;
					ifMoreInfo.Attributes["src"] = "../Crew/CrewBatchDetail.aspx?batchid=" + Request.QueryString["batchid"];

				}
			}
			else
			{
				MenuBatchMaster.SelectedMenuIndex = 0;
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseBatchList.aspx?";
			}

		}
		if (Request.QueryString["batchid"] == null || Filter.CurrentCourseSelection == null)
			MenuBatchMaster.SelectedMenuIndex = 0;
		
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{

	}

	protected void BatchMaster_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		UserControlTabs ucTabs = (UserControlTabs)sender;

		if (Filter.CurrentCourseSelection == null)
		{

			if (dce.CommandName.ToUpper().Equals("BATCH"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseBatchList.aspx";
				return;
			}

			ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseBatchList.aspx";
			
			ShowError();
			MenuBatchMaster.SelectedMenuIndex = 0;

		}
		else
		{
			if (Request.QueryString["batchid"] == null || Filter.CurrentCourseSelection == null)
			{
				MenuBatchMaster.SelectedMenuIndex = 0;
				ShowError();
				return;
			}
			if (dce.CommandName.ToUpper().Equals("BATCH"))
			{
				ifMoreInfo.Attributes["src"]="../Crew/CrewCourseBatchList.aspx?callfrom=batchdetails&batchid=" + Request.QueryString["batchid"];
			}
			
			else if (dce.CommandName.ToUpper().Equals("REMARKS")) 
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewBatchRemarks.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("BATCHDETAILS")) 
			{
				
				ifMoreInfo.Attributes["src"] = "../Crew/CrewBatchDetail.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("REGISTRATION"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseRegistration.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("PARTICIPANTLIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseParticipantList.aspx?calledfrom=batchmaster&batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("ASSESSMENT"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseAssessmentList.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("CERTIFICATE"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseCertification.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("ATTENDANCE"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseAttendance.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else if (dce.CommandName.ToUpper().Equals("FEEDBACK"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseFeedback.aspx?batchid=" + Request.QueryString["batchid"];
			}
			else
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseBatchList.aspx";

		}
	}

	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Please select a Course and batch and then Navigate to other Tabs";
		ucError.Visible = true;
		MenuBatchMaster.SelectedMenuIndex = 0;
	}
}
