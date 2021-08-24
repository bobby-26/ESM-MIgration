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
public partial class CrewCourseMaster : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("Organization", "ORGANIZATION");
        toolbar.AddButton("Content", "CONTENT");
        toolbar.AddButton("Batch", "BATCH");
        toolbar.AddButton("Target Group", "TARGETGROUP");
        toolbar.AddButton("Course Contact", "COURSECONTACT");
        //toolbar.AddButton("Assessment Aspects", "ASSESSMENT");
        toolbar.AddButton("Approval Certificates", "CERTIFICATE");
        //toolbar.AddButton("Attachment", "ATTACHMENT");
        MenuCourseMaster.MenuList = toolbar.Show();
        if (!IsPostBack)
		{
			
			ViewState["type"] = "";
			if (Request.QueryString["type"] != null)
			{
				ViewState["type"] = Request.QueryString["type"];
			}
			if ((Session["COURSEID"] != null && Request.QueryString["callfrom"] != null) || Request.QueryString["courseid"]!=null)
			{
				if (Request.QueryString["callfrom"].ToUpper() == "BATCHDETAILS")
				{
					MenuCourseMaster.SelectedMenuIndex = 2;
					ifMoreInfo.Attributes["src"] = "../Crew/CrewBatchList.aspx";
				}
				if (Request.QueryString["callfrom"].ToUpper() == "ALERTS")
				{
					MenuCourseMaster.SelectedMenuIndex = 6;
					ifMoreInfo.Attributes["src"] = "../Registers/RegistersCourseCertificates.aspx?courseid="+Request.QueryString["courseid"];
				}
				
			}
			
			else
			{
				Session["COURSEID"] = null;
				MenuCourseMaster.SelectedMenuIndex = 0;
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type="+ViewState["type"];
			}
			
		}

	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{

	}

	protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		UserControlTabs ucTabs = (UserControlTabs)sender;

		if (Session["COURSEID"] == null)
		{
			
			if (dce.CommandName.ToUpper().Equals("LIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx";
				return;
			}

			ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx";

			ShowError();
		}
		else
		{
			
			if (dce.CommandName.ToUpper().Equals("CONTENT"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseContent.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("TARGETGROUP"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseTargetGroup.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("ORGANIZATION"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseOrganization.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("COURSECONTACT"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseContact.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("EVALUATION"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseEvaluation.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
			{
				if (Session["COURSEID"] != null)
				{
					ViewState["DTKEY"] = PhoenixRegistersDocumentCourse.EditDocumentCourse(Convert.ToInt32(Session["COURSEID"].ToString())).Tables[0].Rows[0]["FLDDTKEY"].ToString();

				}
				ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.CREW;
			}
			else if (dce.CommandName.ToUpper().Equals("BATCH"))
			{
				ifMoreInfo.Attributes["src"]="../Crew/CrewBatchList.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("CERTIFICATE"))
			{
				ifMoreInfo.Attributes["src"] = "../Registers/RegistersCourseCertificates.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("ASSESSMENT"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseAssessmentAspect.aspx";
			}
			else
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type=" + ViewState["type"];

		}
	}

	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Please select a Course and then Navigate to other Tabs";
		ucError.Visible = true;
		MenuCourseMaster.SelectedMenuIndex = 0;
	}
}
