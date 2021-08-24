using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.PreSea;
using System.Web;

public partial class PreSeaTraineePersonal : PhoenixBasePage
{
	private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("Save", "SAVE");
			PreSeaMainPersonal.AccessRights = this.ViewState;
			PreSeaMainPersonal.MenuList = toolbarmain.Show();

			if (!IsPostBack)
			{
				HookOnFocus(this.Page as Control);
				ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
				if (Filter.CurrentPreSeaTraineeSelection != null)
				{
					ListPreSeaInformation();
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void HookOnFocus(Control CurrentControl)
	{
		if ((CurrentControl is TextBox) ||
			(CurrentControl is DropDownList))

			(CurrentControl as WebControl).Attributes.Add(
			   "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
		if (CurrentControl.HasControls())

			foreach (Control CurrentChildControl in CurrentControl.Controls)
				HookOnFocus(CurrentChildControl);
	}

	

	protected void PreSeaMainPersonal_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidate())
				{
					ucError.Visible = true;
					return;
				}
				UpdatePreSeaMainPersonalInformation();
			}
			else if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				ResetFormControlValues();
				Filter.CurrentPreSeaTraineeSelection = null;
				imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void UpdatePreSeaMainPersonalInformation()
	{
		try
		{
			if (Request.Files["txtFileUpload"].ContentLength > 0)
			{
				if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
				{
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.PRESEA, null, ".jpg,.png,.gif", string.Empty, "PRESEAAPPLICANTPHOTO");
				}
				else
				{
					PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PRESEA, ".jpg,.png,.gif");
				}
			}
			
			PhoenixPreSeaTraineePersonal.UpdatePreSeaTraineePersonal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																		, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																		, General.GetNullableString(txtPassport.Text.Trim().Replace(" ", string.Empty))
																		, General.GetNullableString(txtSeamenBookNumber.Text.Trim().Replace(" ", string.Empty))
																		, txtFirstname.Text.Trim()
                                                                        , txtLastname.Text.Trim()
                                                                        , txtMiddlename.Text.Trim()
																		, Convert.ToInt32(ucNationality.SelectedNationality)
																		, Convert.ToDateTime(txtDateofBirth.Text)
																		, txtPlaceofBirth.Text
																		, Convert.ToInt32(ucSex.SelectedHard)
																		, General.GetNullableInteger(ucMaritialStatus.SelectedMaritalStatus.ToString())
																		, General.GetNullableDecimal(txtHeight.Text)
																		, General.GetNullableDecimal(txtWeight.Text)
																		, null //appno
																		, null //doj                              
																		, Convert.ToInt32(ddlEyeSight.SelectedValue)
																		, Convert.ToInt32(ddlColourBlindness.SelectedValue)
																		, txtDistinguishMark.Text
																		, Convert.ToInt32(ddlIllnessYN.SelectedValue)
																		, txtIllnessDesc.Text
																		, Convert.ToInt32(ddlRelationToCompany.SelectedValue)
																		, txtRelationToCompany.Text
																		, General.GetNullableInteger(ucBatch.SelectedBatch)
																		, General.GetNullableInteger(ucPreSeaCourse.SelectedCourse)
																		, General.GetNullableInteger(rblTerritoryCode.SelectedValue)
																		, General.GetNullableInteger(ucCategory.SelectedQuick)
                                                                        , General.GetNullableString(txtBloodGroup.Text)                                                                       
                                                                        );

			ucStatus.Text = "Trainee Information Updated.";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		ListPreSeaInformation();
	}

	public void ListPreSeaInformation()
	{
		try
		{
			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));
			if (dt.Rows.Count > 0)
			{
				txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				txtRollno.Text = dt.Rows[0]["FLDBATCHROLLNUMBER"].ToString();
				ucSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
				txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
				txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
				txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
				txtPlaceofBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
				ucMaritialStatus.SelectedMaritalStatus = dt.Rows[0]["FLDMARITALSTATUS"].ToString();
				ucNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
				if (dt.Rows[0]["FLDHEIGHT"].ToString() != "0.00" && dt.Rows[0]["FLDWEIGHT"].ToString() != "0.00"
						&& dt.Rows[0]["FLDHEIGHT"].ToString() != "" && dt.Rows[0]["FLDWEIGHT"].ToString() != "")
				{
					txtHeight.Text = dt.Rows[0]["FLDHEIGHT"].ToString();
					txtHeight.Text = txtHeight.Text.Substring(0, txtHeight.Text.IndexOf('.'));
					txtWeight.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
					txtWeight.Text = txtWeight.Text.Substring(0, txtWeight.Text.IndexOf('.'));
				}
				//txtAppliedOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDAPPLIEDON"].ToString()));
				//txtDateofJoin.Text = dt.Rows[0]["FLDDATEOFJOINING"].ToString();
				ddlEyeSight.SelectedValue = dt.Rows[0]["FLDEYESIGHT"].ToString();
				ddlColourBlindness.SelectedValue = dt.Rows[0]["FLDEYECOLORBLINDNESS"].ToString();
				txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();
				ddlIllnessYN.SelectedValue = dt.Rows[0]["FLDILLNESSYN"].ToString();
				txtIllnessDesc.Text = dt.Rows[0]["FLDILLNESSDESC"].ToString();
                ddlRelationToCompany.Text = dt.Rows[0]["FLDFAMILYRELATESTOESMYN"].ToString();
                txtRelationToCompany.Text = dt.Rows[0]["FLDFAMILYRELATESTOESM"].ToString();
				txtCreatedBy.Text = dt.Rows[0]["FLDCREATEDBY"].ToString();
				txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();
                txtBloodGroup.Text = dt.Rows[0]["FLDBLOODGROUP"].ToString();
                txtIndosNo.Text = dt.Rows[0]["FLDINDOSNO"].ToString();

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "PRESEAAPPLICANTPHOTO");
				ViewState["dtkey"] = string.Empty;
				txtFileNo.Text = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
				ucBatch.SelectedBatch = dt.Rows[0]["FLDAPPLIEDBATCH"].ToString();
				ucPreSeaCourse.SelectedCourse = dt.Rows[0]["FLDCOURSEID"].ToString();
				ucCategory.SelectedQuick = dt.Rows[0]["FLDCATEGORY"].ToString();
				rblTerritoryCode.SelectedValue = dt.Rows[0]["FLDTERRITORY"].ToString();
				if (dta.Rows.Count > 0)
				{
					ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
					imgPhoto.ImageUrl = HttpContext.Current.Session["sitepath"] + "/attachments/" + dta.Rows[0]["FLDFILEPATH"].ToString();
					aPreSeaImg.HRef = "#";
					aPreSeaImg.Attributes["onclick"] = "javascript:parent.Openpopup('codehelp1', '', '" + imgPhoto.ImageUrl + "');";
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	public static bool IsValidTextBox(string text)
	{

		string regex = "^[0-9a-zA-Z ]+$";
		Regex re = new Regex(regex);
		if (!re.IsMatch(text))
			return (false);

		return true;
	}

	public static bool IsValidName(string text)
	{

		string regex = "^[a-zA-Z. ]+$";
		Regex re = new Regex(regex);
		if (!re.IsMatch(text))
			return (false);

		return true;
	}

	public static bool IsValidFileNo(string text)
	{

		string regex = "^[0-9a-zA-Z]+$";
		Regex re = new Regex(regex);
		if (!re.IsMatch(text))
			return (false);

		return true;
	}
	private bool IsValidate()
	{
		ucError.HeaderMessage = "Please provide the following required information";
		DateTime resultDate;
		Int32 resultInt;
		decimal resultDec;

		if (txtFirstname.Text.Trim() == "")
			ucError.ErrorMessage = "First Name is required";
		if (Int32.TryParse(ucSex.SelectedHard.Trim(), out resultInt) == false)
			ucError.ErrorMessage = "Gender is required";
		if (!DateTime.TryParse(txtDateofBirth.Text, out resultDate))
		{
			ucError.ErrorMessage = "Date Of Birth is required";
		}
		else if (DateTime.TryParse(txtDateofBirth.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now.AddYears(-16)) > 0)
		{
			ucError.ErrorMessage = "Must be above a minimum age of 16 years and above.";
		}
		if (Int32.TryParse(ucNationality.SelectedNationality, out resultInt) == false)
			ucError.ErrorMessage = "Nationality is required";
		if (!decimal.TryParse(txtHeight.Text, out resultDec))
		{
			ucError.ErrorMessage = "Height cannot be blank.";
		}
		if (!decimal.TryParse(txtWeight.Text, out resultDec))
		{
			ucError.ErrorMessage = "Weight cannot be blank.";
		}

		if (txtDistinguishMark.Text.Trim() == "")
			ucError.ErrorMessage = "Distinguish Mark is required";
      
        if (int.TryParse(ucBatch.SelectedBatch, out resultInt) == false)
			ucError.ErrorMessage = "Batch is required";

		if (int.TryParse(ucPreSeaCourse.SelectedCourse, out resultInt) == false)
			ucError.ErrorMessage = "Course is required";

		if (!IsValidName(txtFirstname.Text.Trim()) && !String.IsNullOrEmpty(txtFirstname.Text.Trim()))
			ucError.ErrorMessage = "Firstname should contain alphabets only";

		if (!IsValidName(txtMiddlename.Text.Trim()) && !String.IsNullOrEmpty(txtMiddlename.Text.Trim()))
			ucError.ErrorMessage = "Middlename should contain alphabets only";

		if (!IsValidName(txtLastname.Text.Trim()) && !String.IsNullOrEmpty(txtLastname.Text.Trim()))
			ucError.ErrorMessage = "Lastname should contain alphabets only";
       
        return (!ucError.IsError);
	}

	private void ResetFormControlValues()
	{
		try
		{
			Filter.CurrentPreSeaTraineeSelection = null;
			txtFirstname.Text = "";
			txtMiddlename.Text = "";
			txtLastname.Text = "";
			ucSex.SelectedHard = null;
			txtPassport.Text = "";
			txtSeamenBookNumber.Text = "";
			txtDateofBirth.Text = "";
			txtPlaceofBirth.Text = "";
            txtBloodGroup.Text = "";
            txtIndosNo.Text = "";
			ucNationality.SelectedNationality = null;

			txtWeight.Text = "";

			txtDateofJoin.Text = "";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
}
