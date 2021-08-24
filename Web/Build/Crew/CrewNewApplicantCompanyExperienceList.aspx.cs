using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantCompanyExperienceList : PhoenixBasePage
{
	string empid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		empid = Request.QueryString["empid"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewCompanyExperienceList.AccessRights = this.ViewState;
        MenuCrewCompanyExperienceList.MenuList = toolbar.Show();
        if (!IsPostBack)
		{
			
			ViewState["rankid"] = "";
			ucNatlyOfficers.SelectedNationality = "97";
			ucNatlyRatings.SelectedNationality = "97";
			if (Request.QueryString["CrewCompanyExperienceId"] != null)
			{

				EditCrewCompanyExperience(int.Parse(Request.QueryString["CrewCompanyExperienceId"].ToString()));
			}
			else
			{
				ResetCrewCompanyExperience();
				txtVesselName.Visible = false;
				ucVessel.Visible = true;
				txtRankName.Visible = false;
				ucRank.Visible = true;
			}

            RadComboBox dl = (RadComboBox)ucRank.FindControl("ddlRank");
			dl.DataTextField = "FLDRANKCODE";

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                trBTET.Visible = false;
                trSignonoffdates.Visible = false;
                trSignonoffby.Visible = false;
                trTravelDates.Visible = false;
                trCOCEOC.Visible = false;
                lblFromDate.Text = "Sign On Date";
                lblToDate.Text = "Sign Off Date";
            }
		}
	}

	protected void EditCrewCompanyExperience(int companyexperienceid)
	{
		DataSet ds = PhoenixCrewCompanyExperience.EditCrewCompanyExperience(companyexperienceid);

		if (ds.Tables[0].Rows.Count > 0)
		{
			DataRow dr = ds.Tables[0].Rows[0];
			ViewState["rankid"] = dr["FLDRANK"].ToString();
			ucRank.SelectedRank = dr["FLDRANK"].ToString();
			ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
			ucSignOffReason.SelectedSignOffReason = dr["FLDSIGNOFFREASON"].ToString();
			ucSignOffPort.SelectedSeaport = dr["FLDSIGNOFFPORT"].ToString();
			ucSignOnReason.SelectedSignOnReason = dr["FLDSIGNONREASON"].ToString();
			ucSignOnPort.SelectedSeaport = dr["FLDSIGNONPORT"].ToString();

			txtFromDate.Text = dr["FLDFROMDATE"].ToString();
			txtToDate.Text = dr["FLDTODATE"].ToString();
			txtRemarks.Text = dr["FLDREMARKS"].ToString();
			txtBToD.Text = dr["FLDBTOD"].ToString();
			txtEToD.Text = dr["FLDETOD"].ToString();
			txtTravelToVessel.Text = dr["FLDTRAVELTOVESSEL"].ToString();
			txtOnLeave.Text = dr["FLDONLEAVE"].ToString();
			txtGap.Text = dr["FLDGAP"].ToString();
			txtDuration.Text = dr["FLDDURATION"].ToString();
			//txtXvct.Text = dr["FLDXVCT"].ToString();
			//txtEvct.Text = dr["FLDEVCT"].ToString();
			txtOriginalEOC.Text = dr["FLDORIGINALEOC"].ToString();
			txtOriginalCOC.Text = dr["FLDORIGINALCOC"].ToString();
			ucNatlyRatings.SelectedNationality = dr["FLDNATIONALITYRATINGS"].ToString();
			ucNatlyOfficers.SelectedNationality = dr["FLDNATIONALITYOFFICERS"].ToString();
			SetEditable(dr["FLDCANEDIT"].ToString().Equals("1"));
			ucVessel.Enabled = false;
			ucSignOnDate.Text = dr["FLDSIGNONDATE"].ToString();
			ucSignOffDate.Text = dr["FLDSIGNOFFDATE"].ToString();
			txtSignOnBy.Text = dr["FLDSIGNONBY"].ToString();
			txtSignOffBy.Text = dr["FLDSIGNOFFBY"].ToString();
			txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
			txtRankName.Text = dr["FLDRANKCODE"].ToString();
			ucVessel.Visible = false;
			txtVesselName.Visible = true;
			ucRank.Visible = false;

		}

	}

	private void SetEditable(bool canedit)
	{
		ucRank.Enabled = canedit;
		ucVessel.Enabled = canedit;
		ucSignOffReason.Enabled = canedit;
		ucSignOffPort.Enabled = canedit;
		ucSignOnReason.Enabled = canedit;
		ucSignOnPort.Enabled = canedit;
		txtFromDate.Enabled = canedit;
		txtToDate.Enabled = canedit;
		txtRemarks.Enabled = canedit;
		txtBToD.Enabled = canedit;
		txtEToD.Enabled = canedit;
		txtTravelToVessel.Enabled = canedit;
		txtOnLeave.Enabled = canedit;
		txtGap.Enabled = canedit;
		txtDuration.Enabled = canedit;
		txtOriginalEOC.Enabled = canedit;
		txtOriginalCOC.Enabled = canedit;
		((RadComboBox)ucNatlyRatings.FindControl("ddlNationality")).Enabled = canedit;
		((RadComboBox)ucNatlyOfficers.FindControl("ddlNationality")).Enabled = canedit;
	}

	protected void ResetCrewCompanyExperience()
	{
		ucRank.SelectedRank = "";
		ucVessel.SelectedVessel = "";
		ucSignOffReason.SelectedSignOffReason = "";
		ucSignOffPort.SelectedSeaport = "";
		ucSignOnReason.SelectedSignOnReason = "";
		ucSignOnPort.SelectedSeaport = "";
		ucNatlyRatings.SelectedNationality = "";
		ucNatlyOfficers.SelectedNationality = "";
		txtFromDate.Text = "";
		txtToDate.Text = "";
		txtRemarks.Text = "";
		txtBToD.Text = "";
		txtEToD.Text = "";
		txtTravelToVessel.Text = "";
		txtOnLeave.Text = "";
		txtGap.Text = "";
		txtDuration.Text = "";
		//txtXvct.Text = "";
		//txtEvct.Text = "";
		txtOriginalEOC.Text = "";
		txtOriginalCOC.Text = "";
		
	}

	protected void CrewCompanyExperienceList_TabStripCommand(object sender, EventArgs e)
	{
		String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
		String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
		{
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (IsValidCrewCompanyExperience())
				{
					TimeSpan ts = DateTime.Parse(txtToDate.Text) - DateTime.Parse(txtFromDate.Text);

					decimal duration = (decimal)ts.Days / (decimal)30.00;

					if (Request.QueryString["CrewCompanyExperienceId"] != null)
					{
						PhoenixCrewCompanyExperience.UpdateCrewCompanyExperience(
							PhoenixSecurityContext.CurrentSecurityContext.UserCode
							, Int32.Parse(Request.QueryString["CrewCompanyExperienceId"].ToString())
							, Int32.Parse(empid)
							, Int32.Parse(ViewState["rankid"].ToString())
							, Int32.Parse(ucVessel.SelectedVessel)
							, DateTime.Parse(txtFromDate.Text)
							, DateTime.Parse(txtToDate.Text)
							, General.GetNullableDateTime(txtBToD.Text)
							, General.GetNullableDateTime(txtEToD.Text)
							, General.GetNullableInteger(ucSignOnReason.SelectedSignOnReason)
							, General.GetNullableInteger(ucSignOffReason.SelectedSignOffReason)
							, txtRemarks.Text
							, General.GetNullableDateTime(txtTravelToVessel.Text)
							, General.GetNullableDateTime(txtOnLeave.Text)
							, General.GetNullableDecimal(txtGap.Text)
							, duration
							, null          //General.GetNullableDateTime(txtXvct.Text)
							, null          //General.GetNullableDateTime(txtEvct.Text)
							, General.GetNullableDateTime(txtOriginalEOC.Text)
							, General.GetNullableDateTime(txtOriginalCOC.Text)
							, General.GetNullableInteger(ucSignOnPort.SelectedSeaport)
							, General.GetNullableInteger(ucSignOffPort.SelectedSeaport)
							, General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
							, General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
							, 0
							);

						RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
					}
					else
					{
						
						PhoenixCrewCompanyExperience.InsertCrewCompanyExperience(
							PhoenixSecurityContext.CurrentSecurityContext.UserCode
							, Int32.Parse(empid)
							, Int32.Parse(ucRank.SelectedRank)
							, Int32.Parse(ucVessel.SelectedVessel)
							, DateTime.Parse(txtFromDate.Text)
							, DateTime.Parse(txtToDate.Text)
							, General.GetNullableDateTime(txtBToD.Text)
							, General.GetNullableDateTime(txtEToD.Text)
							, General.GetNullableInteger(ucSignOnReason.SelectedSignOnReason)
							, General.GetNullableInteger(ucSignOffReason.SelectedSignOffReason)
							, txtRemarks.Text
							, General.GetNullableDateTime(txtTravelToVessel.Text)
							, General.GetNullableDateTime(txtOnLeave.Text)
							, General.GetNullableDecimal(txtGap.Text)
							, duration
							, null          //General.GetNullableDateTime(txtXvct.Text)
							, null          //General.GetNullableDateTime(txtEvct.Text)
							, General.GetNullableDateTime(txtOriginalEOC.Text)
							, General.GetNullableDateTime(txtOriginalCOC.Text)
							, General.GetNullableInteger(ucSignOnPort.SelectedSeaport)
							, General.GetNullableInteger(ucSignOffPort.SelectedSeaport)
							, General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
							, General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
							, 0);

						RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
						ResetCrewCompanyExperience();
					}
				}
				else
				{
					ucError.Visible = true;
					return;
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidCrewCompanyExperience()
	{
		DateTime resultDate;
		Int32 resultInt;

		ucError.HeaderMessage = "Please provide the following required information";
		if (ucVessel.Visible == true)
		{
			if ((!Int32.TryParse(ucVessel.SelectedVessel, out resultInt)) || ucVessel.SelectedVessel == "0")
				ucError.ErrorMessage = "Vessel is required.";
		}
		if (ucRank.Visible == true)
		{
			if ((!Int32.TryParse(ucRank.SelectedRank, out resultInt)) || ucRank.SelectedRank == "0")
				ucError.ErrorMessage = "Rank is required.";
		}
		if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
			ucError.ErrorMessage = "Valid From Date is required.";
		if (!DateTime.TryParse(txtToDate.Text, out resultDate))
			ucError.ErrorMessage = "Valid To Date is required.";

		if (txtBToD.Text != null && !DateTime.TryParse(txtBToD.Text, out resultDate))
			ucError.ErrorMessage = "Valid BToD is required.";

		if (txtEToD.Text != null && !DateTime.TryParse(txtEToD.Text, out resultDate))
			ucError.ErrorMessage = "Valid EToD is required.";

		if (txtTravelToVessel.Text != null && !DateTime.TryParse(txtTravelToVessel.Text, out resultDate))
			ucError.ErrorMessage = "Valid  Date on Travel to Vessel is required.";
		if (txtOnLeave.Text != null && !DateTime.TryParse(txtOnLeave.Text, out resultDate))
			ucError.ErrorMessage = "Valid On Leave Date is required.";

		if (txtOriginalEOC.Text != null && !DateTime.TryParse(txtOriginalEOC.Text, out resultDate))
			ucError.ErrorMessage = "Valid Original EOC Date is required.";
		if (txtOriginalCOC.Text != null && !DateTime.TryParse(txtOriginalCOC.Text, out resultDate))
			ucError.ErrorMessage = "Valid Original COC Date is required.";

		if (txtFromDate.Text != null && txtToDate.Text != null)
		{
			if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
				if ((DateTime.Parse(txtFromDate.Text)) >= (DateTime.Parse(txtToDate.Text)))
					ucError.ErrorMessage = "'To Date' should be greater than 'From Date'";
		}

		if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtBToD.Text, out resultDate)))
			if ((DateTime.Parse(txtFromDate.Text)) < (DateTime.Parse(txtBToD.Text)))
				ucError.ErrorMessage = "'From Date' should be greater than 'BToD '";

		if ((DateTime.TryParse(txtEToD.Text, out resultDate)) && (DateTime.TryParse(txtFromDate.Text, out resultDate)))
			if ((DateTime.Parse(txtEToD.Text)) < (DateTime.Parse(txtToDate.Text)))
				ucError.ErrorMessage = "'EToD' should be greater than 'To Date '";

		if (ucSignOnPort.SelectedSeaport.ToUpper() == "DUMMY" || ucSignOnPort.SelectedSeaport == "")
			ucError.ErrorMessage = "Sign On Port is required.";
        if (ucSignOffPort.SelectedSeaport.ToUpper() == "DUMMY" || ucSignOffPort.SelectedSeaport == "")
            ucError.ErrorMessage = "Sign Off Port is required.";


        return (!ucError.IsError);
	}

	
}
