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
public partial class CrewCourseEnrollment : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();

		toolbar.AddButton("List", "LIST");
		toolbar.AddButton("Nomination List", "NOMINATIONLIST");
		toolbar.AddButton("Participant List", "PARTICIPANTLIST");
		toolbar.AddButton("Wait List", "WAITLIST");
		MenuCourseEnrollment.AccessRights = this.ViewState;
		MenuCourseEnrollment.MenuList = toolbar.Show();
		if (!IsPostBack)
		{
			
			if (Session["COURSEID"] != null && Request.QueryString["callfrom"]!=null)
			{
				if (Request.QueryString["callfrom"].ToUpper() == "BATCHDETAILS")
				{
				}

			}
			else
			{
				Session["COURSEID"] = null;
				MenuCourseEnrollment.SelectedMenuIndex = 0;
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type=3";
			}
		}

	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
	
	}

	protected void CourseEnrollment_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		UserControlTabs ucTabs = (UserControlTabs)sender;

		if (Session["COURSEID"] == null)
		{

			if (dce.CommandName.ToUpper().Equals("LIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type=1";
				return;
			}

			ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type=3";

			ShowError();
		}
		else
		{
			if (dce.CommandName.ToUpper().Equals("NOMINATIONLIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseNominationList.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("CONFIRMEDLIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseConfirmedList.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("WAITLIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseWaitList.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("PARTICIPANTLIST"))
			{
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseParticipantList.aspx?calledfrom=enrollment";
			}
			else
				ifMoreInfo.Attributes["src"] = "../Crew/CrewCourseList.aspx?type=3";

		}
	}

	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Please select a Course and then Navigate to other Tabs";
		ucError.Visible = true;
		MenuCourseEnrollment.SelectedMenuIndex = 0;
	}
}
