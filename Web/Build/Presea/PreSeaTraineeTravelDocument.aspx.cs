using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTraineeTravelDocument : PhoenixBasePage
{
	
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{

			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			if (!IsPostBack)
			{
				DataSet ds = PhoenixRegistersThirdPartyLinks.EditThirdPartyLinks(1);
				if (ds.Tables[0].Rows.Count > 0)
				{
					cdcChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
				}
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ucECNR.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
				ucBlankPages.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
				SetEmployeePrimaryDetails();
				SetEmployeePassportDetails();
				SetEmployeeSeamanBookDetails();
				SetEmployeeUSVisaDetails();
				SetEmployeeMCVDetails();
				imgPPClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				  + PhoenixModule.PRESEA + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PRESEAPASSPORTUPLOAD'); return false;";
				imgCCClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				 + PhoenixModule.PRESEA + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=PRESEASEAMANBOOKUPLOAD'); return false;";
				imgUSVisaClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				 + PhoenixModule.PRESEA + "&type=" + PhoenixCrewAttachmentType.USVISA + "&cmdname=PRESEAVISAUPLOAD'); return false;";
				imgMCVClip.Attributes["onclick"] = "javascript:parent.Openpopup('MCV','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				 + PhoenixModule.PRESEA + "&type=" + PhoenixCrewAttachmentType.MCVAUSTRALIA + "&cmdname=PRESEAMCVUPLOAD'); return false;";
			}

			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("Save", "SAVE");
			toolbarmain.AddButton("Send Mail", "SENDMAIL");
			CrewPassPort.MenuList = toolbarmain.Show();
		
			DateTime? d = General.GetNullableDateTime(ucDateOfExpiry.Text);
			if (d.HasValue)
			{
				TimeSpan t = d.Value - DateTime.Now;
				if (t.Days >= 0 && t.Days < 120)
				{
					imgPPFlag.Visible = true;
					imgPPFlag.ImageUrl = Session["images"] + "/yellow.png";
				}
				else if (t.Days < 0)
				{
					imgPPFlag.Visible = true;
					imgPPFlag.ImageUrl = Session["images"] + "/red.png";
				}
				else
					imgPPFlag.Visible = false;
			}
			d = General.GetNullableDateTime(ucSeamanDateOfExpiry.Text);
			if (d.HasValue)
			{
				TimeSpan t = d.Value - DateTime.Now;
				if (t.Days >= 0 && t.Days < 120)
				{
					imgCCFlag.Visible = true;
					imgCCFlag.ImageUrl = Session["images"] + "/yellow.png";
				}
				else if (t.Days < 0)
				{
					imgCCFlag.Visible = true;
					imgCCFlag.ImageUrl = Session["images"] + "/red.png";
				}
				else
					imgCCFlag.Visible = false;
			}
			d = General.GetNullableDateTime(txtUSDateofExpiry.Text);
			if (d.HasValue)
			{
				TimeSpan t = d.Value - DateTime.Now;
				if (t.Days >= 0 && t.Days < 120)
				{
					imgUSVisa.Visible = true;
					imgUSVisa.ImageUrl = Session["images"] + "/yellow.png";
				}
				else if (t.Days < 0)
				{
					imgUSVisa.Visible = true;
					imgUSVisa.ImageUrl = Session["images"] + "/red.png";
				}
				else
					imgUSVisa.Visible = false;
			}
			d = General.GetNullableDateTime(txtMCVDateofExpiry.Text);
			if (d.HasValue)
			{
				TimeSpan t = d.Value - DateTime.Now;
				if (t.Days >= 0 && t.Days < 120)
				{
					imgMCV.Visible = true;
					imgMCV.ImageUrl = Session["images"] + "/yellow.png";
				}
				else if (t.Days < 0)
				{
					imgMCV.Visible = true;
					imgMCV.ImageUrl = Session["images"] + "/red.png";
				}
				else
					imgMCV.Visible = false;
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		SetEmployeePassportDetails();
		SetEmployeeSeamanBookDetails();
		SetEmployeeUSVisaDetails();
		SetEmployeeMCVDetails();
	}
	public void SetEmployeePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
			if (dt.Rows.Count > 0)
			{
				txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CrewPassPort_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidPassport())
				{
					ucError.Visible = true;
					return;
				}
				UpdateEmployeePassport();
                if (!IsValidSeamanBook() || !IsValidUSVisa())
                {
                    ucError.Visible = true;
                    return;
                }
				UpdateEmployeeSeamanBook();
				UpdateEmployeeUSVisa();
				UpdateEmployeeMCV();
				ucStatus.Text = "Passport, Seaman's Book & Visa information updated";
			}
			if (dce.CommandName.ToUpper() == "SENDMAIL")
			{
				try
				{
					PhoenixPreSeaCommon.TraineeDocsSendMail(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection.ToString()), 1);
					ucStatus.Text = "Mail sent successfully";
				}
				catch (Exception ex)
				{
					ucError.ErrorMessage = ex.Message;
					ucError.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	public void UpdateEmployeePassport()
	{


		PhoenixPreSeaTraineeTravelDocument.UpdateTraineePassport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
															, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
															, Convert.ToDateTime(ucDateOfIssue.Text)
															, txtPlaceOfIssue.Text
															, Convert.ToDateTime(ucDateOfExpiry.Text)
															, General.GetNullableInteger(ucECNR.SelectedHard)
															, General.GetNullableInteger(ucBlankPages.SelectedHard)
															, General.GetNullableString(txtPassportnumber.Text));
	
		SetEmployeePassportDetails();
	}

	public static bool IsValidTextBox(string text)
	{

		string regex = "^[0-9a-zA-Z ]+$";
		Regex re = new Regex(regex);
		if (!re.IsMatch(text))
			return (false);

		return true;
	}

	public bool IsValidPassport()
	{

		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		DateTime resultdate;

		if (txtPassportnumber.Text == "")
			ucError.ErrorMessage = "Passport Number is required";

		else if (!IsValidTextBox(txtPassportnumber.Text.Trim()))
			ucError.ErrorMessage = "Invalid Passport Number";

		if (ucDateOfIssue.Text == null)
			ucError.ErrorMessage = "Passport Date of Issue is required";
		else if (DateTime.TryParse(ucDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Passport Date of Issue should be earlier than current date";
		}

		if (ucDateOfExpiry.Text == null)
			ucError.ErrorMessage = "Passport Date of Expiry is required";
		else if (!string.IsNullOrEmpty(ucDateOfIssue.Text)
			&& DateTime.TryParse(ucDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucDateOfIssue.Text)) < 0)
		{
			ucError.ErrorMessage = "Passport Date of Expiry should be later than 'Date of Issue'";
		}

		if (txtPlaceOfIssue.Text == "")
			ucError.ErrorMessage = "Passport Place of Issue is required";

		if (ucECNR.SelectedHard.Equals("") || !Int16.TryParse(ucECNR.SelectedHard, out result))
			ucError.ErrorMessage = "Passport ECNR  is required";

		if (ucBlankPages.SelectedHard.Equals("") || !Int16.TryParse(ucBlankPages.SelectedHard, out result))
			ucError.ErrorMessage = " Passport Minimum 4 Blank pages  is required";

		return (!ucError.IsError);

	}

	public void UpdateEmployeeSeamanBook()
	{

		try
		{
			PhoenixPreSeaTraineeTravelDocument.UpdateTraineeSeamanBook(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																, Convert.ToInt32(ucSeamanCountry.SelectedFlag)
																, Convert.ToDateTime(ucSeamanDateOfIssue.Text)
																, txtSeamanPlaceOfIssue.Text
																, Convert.ToDateTime(ucSeamanDateOfExpiry.Text)
																, Convert.ToInt16(chkVerifiedYN.Checked ? 1 : 0)
																, General.GetNullableString(txtSeamanBookNumber.Text)
																);
			SetEmployeeSeamanBookDetails();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	public void UpdateEmployeeUSVisa()
	{

		try
		{

			
			PhoenixPreSeaTraineeTravelDocument.UpdateTraineeUSVisa(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																, General.GetNullableString(txtUSVisaType.Text)
																, General.GetNullableString(txtUSVisaNumber.Text)
																, General.GetNullableDateTime(txtUSVisaIssuedOn.Text)
																, General.GetNullableString(txtUSPlaceOfIssue.Text)
																, General.GetNullableDateTime(txtUSDateofExpiry.Text)
																);
			
			SetEmployeeUSVisaDetails();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	public void UpdateEmployeeMCV()
	{

		try
		{
			PhoenixPreSeaTraineeTravelDocument.UpdateTraineeMCVAustralia(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																, General.GetNullableString(txtMCVNumber.Text)
																, General.GetNullableDateTime(txtMCVIssuedOn.Text)
																, General.GetNullableDateTime(txtMCVDateofExpiry.Text)
																, General.GetNullableString(txtMCVRemarks.Text)
																);

			SetEmployeeMCVDetails();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	public bool IsValidSeamanBook()
	{

		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		DateTime resultdate;

		if (txtSeamanBookNumber.Text == "")
			ucError.ErrorMessage = "Seaman's Book Number is required";

		else if (!IsValidTextBox(txtSeamanBookNumber.Text.Trim()))
			ucError.ErrorMessage = "Invalid Seaman Book Number";

		if (ucSeamanDateOfIssue.Text == null)
			ucError.ErrorMessage = "Seaman's Book Date of Issue is required";
		else if (DateTime.TryParse(ucSeamanDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Seaman's Book Date of Issue should be earlier than current date";
		}

		if (ucSeamanDateOfExpiry.Text == null)
			ucError.ErrorMessage = "Seaman's Book Date of Expiry is required";
		else if (!string.IsNullOrEmpty(ucSeamanDateOfIssue.Text)
			&& DateTime.TryParse(ucSeamanDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucSeamanDateOfIssue.Text)) < 0)
		{
			ucError.ErrorMessage = "Seaman's Book Date of Expiry should be later than 'Date of Issue'";
		}

		if (txtSeamanPlaceOfIssue.Text == "")
			ucError.ErrorMessage = "Seaman's Book Place of Issue is required";

		if (ucSeamanCountry.SelectedFlag.Equals("") || !Int16.TryParse(ucSeamanCountry.SelectedFlag, out result))
			ucError.ErrorMessage = "Seaman's Book Flag  is required";

		return (!ucError.IsError);

	}
	public bool IsValidUSVisa()
	{

		ucError.HeaderMessage = "Please provide the following required information";

		DateTime resultdate;
		if (!string.IsNullOrEmpty(txtUSVisaType.Text) || !string.IsNullOrEmpty(txtUSVisaNumber.Text) || !string.IsNullOrEmpty(txtUSVisaIssuedOn.Text)
		   || !string.IsNullOrEmpty(txtUSDateofExpiry.Text) || !string.IsNullOrEmpty(txtUSPlaceOfIssue.Text))
		{
			if (txtUSVisaType.Text == "")
				ucError.ErrorMessage = "US Visa Type is required";

			if (txtUSVisaNumber.Text == "")
				ucError.ErrorMessage = "US Visa Number is required";

			if (!DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate))
				ucError.ErrorMessage = "US Visa Issued On is required";
			else if (DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
			{
				ucError.ErrorMessage = "US Visa Issued On should be earlier than current date";
			}

			if (!DateTime.TryParse(txtUSDateofExpiry.Text, out resultdate))
				ucError.ErrorMessage = "US Visa Date of Expiry is required";
			else if (DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate)
				&& DateTime.TryParse(txtUSDateofExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(txtUSVisaIssuedOn.Text)) < 0)
			{
				ucError.ErrorMessage = "US Visa Date of Expiry should be later than 'Date of Issue'";
			}

			if (txtUSPlaceOfIssue.Text == "")
				ucError.ErrorMessage = "US Visa Place of Issue is required";
		}
		return (!ucError.IsError);

	}


	protected void SetEmployeePassportDetails()
	{
		DataTable dt = PhoenixPreSeaTraineeTravelDocument.ListTraineePassport(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));

		if (dt.Rows.Count > 0)
		{
			txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
			ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
			txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOFISSUE"].ToString();
			ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
			ucECNR.SelectedHard = dt.Rows[0]["FLDECNRYESNO"].ToString();
			ucBlankPages.SelectedHard = dt.Rows[0]["FLDMINIMUMPAGE"].ToString();
			if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
				imgPPClip.ImageUrl = Session["images"] + "/no-attachment.png";
			else
				imgPPClip.ImageUrl = Session["images"] + "/attachment.png";
		}

	}

	protected void SetEmployeeSeamanBookDetails()
	{
		DataTable dt = PhoenixPreSeaTraineeTravelDocument.ListTraineeSeamanBook(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));

		if (dt.Rows.Count > 0)
		{
			txtSeamanBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
			ucSeamanDateOfIssue.Text = dt.Rows[0]["FLDSDATEOFISSUE"].ToString();
			txtSeamanPlaceOfIssue.Text = dt.Rows[0]["FLDSPLACEOFISSUE"].ToString();
			ucSeamanDateOfExpiry.Text = dt.Rows[0]["FLDSDATEOFEXPIRY"].ToString();
			ucSeamanCountry.SelectedFlag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();
			chkVerifiedYN.Checked = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? true : false;
			chkVerifiedYN.Enabled = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? false : true;
			txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBYNAME"].ToString();
			txtVerifiedOn.Text = dt.Rows[0]["FLDVERIFIEDON"].ToString();
			if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
				imgCCClip.ImageUrl = Session["images"] + "/no-attachment.png";
			else
				imgCCClip.ImageUrl = Session["images"] + "/attachment.png";
		}
	}
	public void SetEmployeeUSVisaDetails()
	{
		DataTable dt = PhoenixPreSeaTraineeTravelDocument.ListTraineeUSVisa(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));

		if (dt.Rows.Count > 0)
		{
			txtUSVisaType.Text = dt.Rows[0]["FLDUSVISATYPE"].ToString();
			txtUSVisaNumber.Text = dt.Rows[0]["FLDUSVISANUMBER"].ToString();
			txtUSVisaIssuedOn.Text = dt.Rows[0]["FLDUSVISADATEOFISSUE"].ToString();
			txtUSPlaceOfIssue.Text = dt.Rows[0]["FLDUSVISAPLACEOFISSUE"].ToString();
			txtUSDateofExpiry.Text = dt.Rows[0]["FLDUSVISADATEOFEXPIRY"].ToString();
			if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
				imgUSVisaClip.ImageUrl = Session["images"] + "/no-attachment.png";
			else
				imgUSVisaClip.ImageUrl = Session["images"] + "/attachment.png";
		}
	}
	public void SetEmployeeMCVDetails()
	{
		DataTable dt = PhoenixPreSeaTraineeTravelDocument.ListTraineeMCVAustralia(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));

		if (dt.Rows.Count > 0)
		{
			txtMCVNumber.Text = dt.Rows[0]["FLDMCVAUSTRALIATXNUMBER"].ToString();
			txtMCVIssuedOn.Text = dt.Rows[0]["FLDMCVAUSTRALIAISSUEDATE"].ToString();
			txtMCVDateofExpiry.Text = dt.Rows[0]["FLDMCVAUSTRALIAEXPIRYDATE"].ToString();
			txtMCVRemarks.Text = dt.Rows[0]["FLDMCVAUSTRALIAREMARKS"].ToString();
			if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
				imgMCVClip.ImageUrl = Session["images"] + "/no-attachment.png";
			else
				imgMCVClip.ImageUrl = Session["images"] + "/attachment.png";
		}
	}

}
