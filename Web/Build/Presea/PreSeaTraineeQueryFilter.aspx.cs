using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class PreSeaTraineeQueryFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		if (!IsPostBack)
		{
			if (Request.QueryString["batchid"] != null)
			{
				ucBatch.SelectedBatch = Request.QueryString["batchid"];
				ucBatch.Enabled = false;
			}
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Next", "GO");

			MenuActivityFilterMain.AccessRights = this.ViewState;
			MenuActivityFilterMain.MenuList = toolbar.Show();

			ddlSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
			txtName.Focus();
		}
	}
	
	protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		string Script = "";
		Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
		Script += "fnReloadList();";
		Script += "</script>" + "\n";

		if (dce.CommandName.ToUpper().Equals("GO"))
		{
			NameValueCollection criteria = new NameValueCollection();

			criteria.Clear();

			criteria.Add("txtName", txtName.Text);
			criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
			criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
			criteria.Add("txtDOBStartDate", txtDOBStartDate.Text);
			criteria.Add("txtDOBEndDate", txtDOBEndDate.Text);
			criteria.Add("lstNationality", lstNationality.SelectedList);
			criteria.Add("ddlSex", ddlSex.SelectedHard);
			criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
			criteria.Add("ddlState", ddlState.SelectedState);
			criteria.Add("ddlCity", ddlCity.SelectedCity);
			criteria.Add("ucBatch", ucBatch.SelectedBatch);
			criteria.Add("ddlQualificaiton", ddlQualificaiton.SelectedQualification);
			criteria.Add("ucPreSeaCourse", ucPreSeaCourse.SelectedCourse);
			criteria.Add("ucExamVenue1", ucExamVenue1.SelectedExamVenue);
			criteria.Add("ucExamVenue2", ucExamVenue2.SelectedExamVenue);
			Filter.CurrentPreSeaNewApplicantFilterCriteria = criteria;

			Response.Redirect("../PreSea/PreSeaTraineeList.aspx", false);
		}

		//Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	}
	protected void ddlCountry_TextChanged(object sender, EventArgs e)
	{
		ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
		ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), null);
	}
	protected void ddlState_TextChanged(object sender, EventArgs e)
	{
		ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));
	}
}
