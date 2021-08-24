using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewOtherExperienceList : PhoenixBasePage
{
	string empid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        SessionUtil.PageAccessRights(this.ViewState);

		empid = Request.QueryString["empid"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewOtherExperienceList.AccessRights = this.ViewState;
        MenuCrewOtherExperienceList.MenuList = toolbar.Show();

        if (!IsPostBack)
		{
			if (Request.QueryString["type"] == "p")
			{
				chkPromtedyn.Visible = false;
				lblpromotion.Visible = false;
				
			}
            ucNatlyOfficers.SelectedNationality = "97";
            ucNatlyRatings.SelectedNationality = "97";
            
            ddlUMS.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();

			if (Request.QueryString["CREWOTHEREXPERIENCEID"] != null)
			{
				EditCrewOtherExperience(int.Parse(Request.QueryString["CREWOTHEREXPERIENCEID"].ToString()));
			}
			txtManagingCompany.Focus();
		}
		RadComboBox dl = (RadComboBox)ddlRank.FindControl("ddlRank");
		dl.DataTextField = "FLDRANKCODE";
	}

	protected void EditCrewOtherExperience(int? Otherexperienceid)
	{
		DataTable dt = PhoenixNewApplicantOtherExperience.ListEmployeeOtherExperience(Convert.ToInt32(empid), Otherexperienceid);

		if (dt.Rows.Count > 0)
		{
			txtManagingCompany.Text = dt.Rows[0]["FLDMANAGINGCOMPANY"].ToString();
			ddlManningCompany.SelectedOtherCompany = dt.Rows[0]["FLDMANNINGCOMPANY"].ToString();
			ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
			txtFrom.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
			txtTo.Text = dt.Rows[0]["FLDTODATE"].ToString();
			txtVessel.Text = dt.Rows[0]["FLDVESSEL"].ToString();
			ddlVesselType.SelectedVesseltype = dt.Rows[0]["FLDVESSELTYPE"].ToString();
			ddlEngineType.SelectedEngineName = dt.Rows[0]["FLDENGINETYPE"].ToString();
			txtDWT.Text = dt.Rows[0]["FLDVESSELDWT"].ToString();
			txtGt.Text = dt.Rows[0]["FLDVESSELGT"].ToString();
			txtKWT.Text = dt.Rows[0]["FLDVESSELKW"].ToString();
            txtTEU.Text = dt.Rows[0]["FLDVESSELTEU"].ToString();
            txtBHP.Text = dt.Rows[0]["FLDVESSELBHP"].ToString();
			ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASON"].ToString();
			ddlSignonReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASON"].ToString();
			ddlUMS.SelectedHard = dt.Rows[0]["FLDVESSELUMS"].ToString();
			txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
			ucNatlyRatings.SelectedNationality = dt.Rows[0]["FLDRATINGNATIONALITY"].ToString();
			ucNatlyOfficers.SelectedNationality = dt.Rows[0]["FLDOFFICERSNATIONALITY"].ToString();
			txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
			txtGap.Text = dt.Rows[0]["FLDGAP"].ToString();
			if (dt.Rows[0]["FLDFRAMOEXP"].ToString() == "1")
			{
				chkFramo.Checked = true;
			}
			else
			{
				chkFramo.Checked = false;
			}
            if (dt.Rows[0]["FLDPROMOTEDONBOARDYN"].ToString() == "1")
            {
                chkPromtedyn.Checked = true;
            }
            else
            {
                chkPromtedyn.Checked = false;
            }
            
			ucFlag.SelectedFlag = dt.Rows[0]["FLDFLAG"].ToString();
            ddlIceClass.SelectedValue = dt.Rows[0]["FLDICECLASSYN"].ToString();
            txtEngineModel.Text = dt.Rows[0]["FLDENGINEMODEL"].ToString();
		}
	}

	protected void CalculateBHP(object sender, EventArgs e)
	{
		if (txtKWT.Text != "")
		{
			Double bhp = (Convert.ToDouble(txtKWT.Text)) * 1.341;  
			txtBHP.Text = Convert.ToString(Math.Ceiling(bhp));
		}
	}
	protected void CalculateKwt(object sender, EventArgs e)
	{
		if (txtBHP.Text != "")
		{
			Double kwt = (Convert.ToDouble(txtBHP.Text)) / 1.341;  
			txtKWT.Text = Convert.ToString(Math.Ceiling(kwt));
		}
	}
	
	protected void ResetCrewOtherExperience()
	{
		txtManagingCompany.Text = "";
		ddlManningCompany.SelectedOtherCompany = "";
		ddlRank.SelectedRank = "";
		txtFrom.Text = "";
		txtTo.Text = "";
		txtVessel.Text = "";
		ddlVesselType.SelectedVesseltype = "";
		ddlEngineType.SelectedEngineName = "";
		txtDWT.Text = "";
		txtGt.Text = "";
		txtKWT.Text = "";
		txtBHP.Text = "";
        txtTEU.Text = "";
        ddlSignonReason.SelectedSignOnReason = "";
		ddlSignOffReason.SelectedSignOffReason = "";
		ddlUMS.SelectedHard = "";
		txtRemarks.Text = "";
		ucNatlyRatings.SelectedNationality = "";
		ucNatlyOfficers.SelectedNationality = "";
		chkPromtedyn.Checked = false;
		ucFlag.SelectedFlag = "";
        txtEngineModel.Text = ""; 
        chkFramo.Checked = false;
        ddlIceClass.ClearSelection();
        ddlIceClass.Text = string.Empty;
      
    }

	protected void CrewOtherExperienceList_TabStripCommand(object sender, EventArgs e)
	{
		String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
		String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
		string type=Request.QueryString["type"].ToString();

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
		{
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (IsValidOtherExperience())
				{


					if (Request.QueryString["CREWOTHEREXPERIENCEID"] != null)
					{
						if (type == "n")
						{

							PhoenixNewApplicantOtherExperience.UpdateEmployeeOtherExperience(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																	, Convert.ToInt32(empid)
																	, txtManagingCompany.Text
																	, General.GetNullableInteger(ddlManningCompany.SelectedOtherCompany)
																	, General.GetNullableInteger(ddlRank.SelectedRank)
																	, General.GetNullableDateTime(txtFrom.Text)
																	, General.GetNullableDateTime(txtTo.Text)
																	, txtVessel.Text
																	, Convert.ToInt32(ddlVesselType.SelectedVesseltype)
																	, General.GetNullableInteger(ddlEngineType.SelectedEngineName)
																	, General.GetNullableDecimal(txtDWT.Text)
																	, General.GetNullableDecimal(txtGt.Text)
																	, General.GetNullableDecimal(txtKWT.Text)
																	, General.GetNullableDecimal(txtBHP.Text)
																	, General.GetNullableString(string.Empty)
																	, General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
																	, General.GetNullableInteger(ddlUMS.SelectedHard)
																	, txtRemarks.Text
																	, Convert.ToInt32(Request.QueryString["CREWOTHEREXPERIENCEID"])
																	, General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
																	, General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
																    , Convert.ToByte(chkFramo.Checked == true ? 1 : 0)
																	, General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
																	, chkPromtedyn.Checked == true ? 1 : 0
																	, General.GetNullableInteger(ucFlag.SelectedFlag)
                                                                    , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                                    , General.GetNullableString(txtEngineModel.Text)
                                                                    , General.GetNullableInteger(txtTEU.Text)
                                                                    );

							
						}
						else
						{
							PhoenixCrewOtherExperience.UpdateEmployeeOtherExperience(PhoenixSecurityContext.CurrentSecurityContext.UserCode
															 , Convert.ToInt32(empid)
															 , txtManagingCompany.Text
															 , int.Parse(ddlManningCompany.SelectedOtherCompany)
															 , ddlRank.SelectedValue
															 , Convert.ToDateTime(txtFrom.Text)
															 , Convert.ToDateTime(txtTo.Text)
															 , txtVessel.Text
															 , Convert.ToInt32(ddlVesselType.SelectedVesseltype)
															 , General.GetNullableInteger(ddlEngineType.SelectedEngineName)
															 , General.GetNullableDecimal(txtDWT.Text)
															 , General.GetNullableDecimal(txtGt.Text)
															 , General.GetNullableDecimal(txtKWT.Text)
															 , General.GetNullableDecimal(txtBHP.Text)
															 , General.GetNullableString(string.Empty)
															 , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
															 , General.GetNullableInteger(ddlUMS.SelectedHard)
															 , txtRemarks.Text
															 , Convert.ToInt32(Request.QueryString["CREWOTHEREXPERIENCEID"])
															 , General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
															 , General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
															 , Convert.ToByte(chkFramo.Checked== true ? 1 : 0)
															 , General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
															 , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                             , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                             , General.GetNullableString(txtEngineModel.Text)
                                                             , General.GetNullableInteger(txtTEU.Text)
                                                             );
						}
                        
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
					else
					{
						if (type == "n")
						{
							DataTable dt = PhoenixNewApplicantOtherExperience.InsertEmployeeOtherExperience(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																   , Convert.ToInt32(empid)
																   , General.GetNullableString(txtManagingCompany.Text)
																   , General.GetNullableInteger(ddlManningCompany.SelectedOtherCompany)
																   , General.GetNullableInteger(ddlRank.SelectedRank)
																   , General.GetNullableDateTime(txtFrom.Text)
																   , General.GetNullableDateTime(txtTo.Text)
																   , txtVessel.Text
																   , Convert.ToInt32(ddlVesselType.SelectedVesseltype)
																   , General.GetNullableInteger(ddlEngineType.SelectedEngineName)
																   , General.GetNullableDecimal(txtDWT.Text)
																   , General.GetNullableDecimal(txtGt.Text)
																   , General.GetNullableDecimal(txtKWT.Text)
																   , General.GetNullableDecimal(txtBHP.Text)
																   , General.GetNullableString(string.Empty)
																   , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
																   , General.GetNullableInteger(ddlUMS.SelectedHard)
																   , General.GetNullableString(txtRemarks.Text)
																   , General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
																   , General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
																   , Convert.ToByte(chkFramo.Checked==true ? 1 : 0)
																   , General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
																   , chkPromtedyn.Checked == true ? 1 : 0
																   , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                                   , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                                   , General.GetNullableString(txtEngineModel.Text)
                                                                   , General.GetNullableInteger(txtTEU.Text)
                                                                   );
						}
						else
						{
							DataTable dt = PhoenixCrewOtherExperience.InsertEmployeeOtherExperience(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
															  , Convert.ToInt32(empid)
															  , General.GetNullableString(txtManagingCompany.Text)
															  , int.Parse(ddlManningCompany.SelectedOtherCompany)
															  , Convert.ToInt32(ddlRank.SelectedRank)
															  , Convert.ToDateTime(txtFrom.Text)
															  , Convert.ToDateTime(txtTo.Text)
															  , txtVessel.Text
															  , Convert.ToInt32(ddlVesselType.SelectedVesseltype)
															  , General.GetNullableInteger(ddlEngineType.SelectedEngineName)
															  , General.GetNullableDecimal(txtDWT.Text)
															  , General.GetNullableDecimal(txtGt.Text)
															  , General.GetNullableDecimal(txtKWT.Text)
															  , General.GetNullableDecimal(txtBHP.Text)
															  , General.GetNullableString(string.Empty)
															  , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
															  , General.GetNullableInteger(ddlUMS.SelectedHard)
															  , General.GetNullableString(txtRemarks.Text)
															  , General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
															  , General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
															  , Convert.ToByte(chkFramo.Checked==true? 1:0)
															  , General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
															  , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                              , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                              , General.GetNullableString(txtEngineModel.Text)
                                                              , General.GetNullableInteger(txtTEU.Text)
                                                              );
						}
                        
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                        ResetCrewOtherExperience();

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

	private bool IsValidOtherExperience()
	{

		ucError.HeaderMessage = "Please provide the following required information";
		int resultint;
		DateTime resultdate;

		if (Request.QueryString["type"] == "p")
		{
			if (!int.TryParse(ddlRank.SelectedRank, out resultint))
				ucError.ErrorMessage = "Rank is required";

			if (txtVessel.Text.Trim() == "")
				ucError.ErrorMessage = "Vessel is required";

			if (!int.TryParse(ddlManningCompany.SelectedOtherCompany, out resultint))
			{
				ucError.ErrorMessage = "Manning Company is required";
			}
			if (string.IsNullOrEmpty(txtFrom.Text))
			{
				ucError.ErrorMessage = "From Date can not be blank";
			}
			else if (DateTime.TryParse(txtFrom.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
			{
				ucError.ErrorMessage = "From Date should be later than current date";
			}

			if (string.IsNullOrEmpty(txtTo.Text))
			{
				ucError.ErrorMessage = "To Date can not be blank";
			}
			else if (!string.IsNullOrEmpty(txtTo.Text) && !string.IsNullOrEmpty(txtFrom.Text)
				&& DateTime.TryParse(txtTo.Text, out resultdate) && DateTime.Compare(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text)) > 0)
			{
				ucError.ErrorMessage = "To Date should be greater than 'From Date'";
			}            
		}
		if (!int.TryParse(ddlVesselType.SelectedVesseltype, out resultint))
		{
			ucError.ErrorMessage = "Vessel Type is required";
		}
		if (Request.QueryString["type"] == "p")
		{
			if ((DateTime.TryParse(txtFrom.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0) && General.GetNullableDateTime(txtFrom.Text) != null)
			{
				ucError.ErrorMessage = "From Date should be later than current date";
			}
			if (string.IsNullOrEmpty(txtTo.Text) && General.GetNullableDateTime(txtFrom.Text) != null)
			{
				ucError.ErrorMessage = "To Date can not be blank";
			}
			if (!string.IsNullOrEmpty(txtTo.Text) && !string.IsNullOrEmpty(txtFrom.Text)
					&& DateTime.TryParse(txtTo.Text, out resultdate) && DateTime.Compare(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text)) > 0)
			{
				ucError.ErrorMessage = "To Date should be greater than 'From Date'";
			}
		}
		return (!ucError.IsError);
        
	}
}
