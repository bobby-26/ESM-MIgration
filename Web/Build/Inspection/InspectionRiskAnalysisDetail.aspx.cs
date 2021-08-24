using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Text;

public partial class InspectionRiskAnalysisDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("List", "LIST");
			toolbar.AddButton("Details", "DETAIL");
			MenuHeader.AccessRights = this.ViewState;
			MenuHeader.MenuList = toolbar.Show();
			MenuHeader.SelectedMenuIndex = 1;

			toolbar = new PhoenixToolbar();
			toolbar.AddButton("New", "NEW");
			toolbar.AddButton("Save", "SAVE");
			MenuRiskAnalysisDetail.AccessRights = this.ViewState;
			MenuRiskAnalysisDetail.MenuList = toolbar.Show();
			BindCheckBoxList();
			if (Request.QueryString["analysisid"] != null)
			{
				ViewState["ANALYSISID"]=Request.QueryString["analysisid"] ;
			}
			if (Request.QueryString["assessmentid"] != null)
			{
				ViewState["ASSESSMENTID"] = Request.QueryString["assessmentid"];
			}
		}
    }
	protected void BindSubCategory(object sender, EventArgs e)
	{
		ddlSubCategory.Items.Clear();
		ddlSubCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlSubCategory.DataSource = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(General.GetNullableInteger(ucCategory.SelectedCategory));
		ddlSubCategory.DataTextField = "FLDNAME";
		ddlSubCategory.DataValueField = "FLDACTIVITYID";
		ddlSubCategory.DataBind();
	
	
	
	}

	protected void BindCheckBoxList()
	{
		cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(1,null);
		cblReason.DataTextField = "FLDNAME";
		cblReason.DataValueField = "FLDTYPEID";
		cblReason.DataBind();

		rblHSActivityDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
		rblHSActivityDuration.DataTextField = "FLDNAME";
		rblHSActivityDuration.DataValueField = "FLDFREQUENCYID";
		rblHSActivityDuration.DataBind();

		rblHSNoofpeople.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(3);
		rblHSNoofpeople.DataTextField = "FLDNAME";
		rblHSNoofpeople.DataValueField = "FLDFREQUENCYID";
		rblHSNoofpeople.DataBind();

		rblHSControls.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
		rblHSControls.DataTextField = "FLDNAME";
		rblHSControls.DataValueField = "FLDFREQUENCYID";
		rblHSControls.DataBind();

		cblHSHazard.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(1,null);
		cblHSHazard.DataTextField = "FLDNAME";
		cblHSHazard.DataValueField = "FLDHAZARDID";
		cblHSHazard.DataBind();

		rblEnvActivityDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
		rblEnvActivityDuration.DataTextField = "FLDNAME";
		rblEnvActivityDuration.DataValueField = "FLDFREQUENCYID";
		rblEnvActivityDuration.DataBind();

		rblEnvActivityFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(2);
		rblEnvActivityFrequency.DataTextField = "FLDNAME";
		rblEnvActivityFrequency.DataValueField = "FLDFREQUENCYID";
		rblEnvActivityFrequency.DataBind();

		rblEnvControls.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
		rblEnvControls.DataTextField = "FLDNAME";
		rblEnvControls.DataValueField = "FLDFREQUENCYID";
		rblEnvControls.DataBind();

		cblEnvHazard.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(2,null);
		cblEnvHazard.DataTextField = "FLDNAME";
		cblEnvHazard.DataValueField = "FLDHAZARDID";
		cblEnvHazard.DataBind();

		cblOtherConsequences.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(4,null);
		cblOtherConsequences.DataTextField = "FLDNAME";
		cblOtherConsequences.DataValueField = "FLDHAZARDID";
		cblOtherConsequences.DataBind();

		rblOtherActivityDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
		rblOtherActivityDuration.DataTextField = "FLDNAME";
		rblOtherActivityDuration.DataValueField = "FLDFREQUENCYID";
		rblOtherActivityDuration.DataBind();

		rblOtherActivityFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(2);
		rblOtherActivityFrequency.DataTextField = "FLDNAME";
		rblOtherActivityFrequency.DataValueField = "FLDFREQUENCYID";
		rblOtherActivityFrequency.DataBind();

		rblOtherControls.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
		rblOtherControls.DataTextField = "FLDNAME";
		rblOtherControls.DataValueField = "FLDFREQUENCYID";
		rblOtherControls.DataBind();

	}

	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("LIST"))
		{
			Response.Redirect("../Inspection/InspectionRiskAnalysis.aspx", false);
		}
	
	}

	protected void RiskAnalysisDetail_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("SAVE"))
		{
			StringBuilder strhshazard = new StringBuilder();
			StringBuilder strenvhazard = new StringBuilder();
			StringBuilder strothhazard = new StringBuilder();

			foreach (ListItem item in cblHSHazard.Items)
			{
				if (item.Selected == true)
				{
					strhshazard.Append(item.Value.ToString());
					strhshazard.Append(",");
				}
			}

			if (strhshazard.Length > 1)
			{
				strhshazard.Remove(strhshazard.Length - 1, 1);
			}


			foreach (ListItem item in cblEnvHazard.Items)
			{
				if (item.Selected == true)
				{
					strenvhazard.Append(item.Value.ToString());
					strenvhazard.Append(",");
				}
			}

			if (strenvhazard.Length > 1)
			{
				strenvhazard.Remove(strenvhazard.Length - 1, 1);
			}

			foreach (ListItem item in cblOtherConsequences.Items)
			{
				if (item.Selected == true)
				{
					strothhazard.Append(item.Value.ToString());
					strothhazard.Append(",");
				}
			}

			if (strothhazard.Length > 1)
			{
				strothhazard.Remove(strhshazard.Length - 1, 1);
			}

			if (!IsValidRiskAnalysis(strhshazard.ToString(), strenvhazard.ToString(), strothhazard.ToString()))
			{
				ucError.Visible = true;
				return;
			}
		}
		else if (dce.CommandName.ToUpper().Equals("SAVE"))
		{
			Reset();

		}
	}

	private bool IsValidRiskAnalysis(string hshazard,string envhazard,string othhazard)
	{

		ucError.HeaderMessage = "Please provide the following required information";
		//Health and Safety
		if (General.GetNullableInteger(ucCategory.SelectedCategory) != null)
			ucError.ErrorMessage = "Category is required.";

		if (General.GetNullableInteger(ddlSubCategory.SelectedValue) != null)
			ucError.ErrorMessage = "Sub Category is required.";

		if (txtActivity.Text.Equals(""))
			ucError.ErrorMessage = "Activity is required.";

		if (txtHSRisk.Text.Equals(""))
			ucError.ErrorMessage = "Health and safety-Elements being assessed on  is required.";

		if ((General.GetNullableInteger(rblHSActivityDuration.SelectedValue)!=null))
			ucError.ErrorMessage = "Health and safety-Activity Duration is required.";

		if ((General.GetNullableInteger(rblHSNoofpeople.SelectedValue) != null))
			ucError.ErrorMessage = "Health and safety-No of people is required.";

		if ((General.GetNullableInteger(rblHSControls.SelectedValue) != null))
			ucError.ErrorMessage = "Health and safety-Controls is required.";

		if (hshazard.Trim().Length == 0)
			ucError.ErrorMessage = "Health and Safety-Select Atleast one Hazard.";

		//Environmental

		if (txtEnvRisk.Text.Equals(""))
			ucError.ErrorMessage = "Environmental-Elements being assessed on  is required.";

		if ((General.GetNullableInteger(rblEnvActivityDuration.SelectedValue) != null))
			ucError.ErrorMessage = "Environmental-Activity Duration is required.";

		if ((General.GetNullableInteger(rblEnvActivityFrequency.SelectedValue) != null))
			ucError.ErrorMessage = "Environmental-Activity Frequency is required.";

		if ((General.GetNullableInteger(rblEnvControls.SelectedValue) != null))
			ucError.ErrorMessage = "Environmental-Controls is required.";

		if (envhazard.Trim().Length == 0)
			ucError.ErrorMessage = "Environmental-Select Atleast one Impact.";

		//Others

		if (txtOthers.Text.Equals(""))
			ucError.ErrorMessage = "Others-Elements being assessed on  is required.";

		if ((General.GetNullableInteger(rblOtherActivityDuration.SelectedValue) != null))
			ucError.ErrorMessage = "Others-Activity Duration is required.";

		if ((General.GetNullableInteger(rblOtherActivityFrequency.SelectedValue) != null))
			ucError.ErrorMessage = "Others-Activity Frequency is required.";

		if ((General.GetNullableInteger(rblOtherControls.SelectedValue) != null))
			ucError.ErrorMessage = "Others-Controls is required.";

		if (othhazard.Trim().Length == 0)
			ucError.ErrorMessage = "Others-Select Atleast one Consequence.";

		return (!ucError.IsError);
	}

	protected void Reset()
	{
		ucCategory.SelectedCategory = "";
		ddlSubCategory.SelectedValue = "";
		txtActivity.Text = "";
		cblReason.SelectedValue = "";
		txtHSRisk.Text = "";
		rblHSActivityDuration.SelectedValue= "";
		rblHSNoofpeople.SelectedValue = "";
		rblHSControls.SelectedValue = "";
		cblHSHazard.SelectedValue = "";
		txtHSPOC.Text = "";
		txtHSLOH.Text = "";
		txtHSLOC.Text = "";
		txtHSLevelOfRisk.Text = "";
		txtEnvRisk.Text = "";
		rblEnvActivityDuration.SelectedValue = "";
		rblEnvActivityFrequency.SelectedValue = "";
		rblEnvControls.SelectedValue= "";
		cblEnvHazard.SelectedValue= "";
		txtEnvPOC.Text="";
		txtEnvLOH.Text="";
		txtEnvLOC.Text="";
		txtEnvLevelOfRisk.Text="";
		txtOthers.Text="";
		rblOtherActivityDuration.SelectedValue= "";
		rblOtherActivityFrequency.SelectedValue= "";
		rblOtherControls.SelectedValue= "";
		cblOtherConsequences.SelectedValue = "";
		txtOtherPOC.Text="";
		txtOtherLOH.Text="";
		txtLOC.Text="";
		txtOtherLevelofrisk.Text="";
		ViewState["ASSESSMENTID"] = "";
		ViewState["ANLAYSISID"] = "";
	}

}
