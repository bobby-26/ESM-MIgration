using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;

public partial class PreSeaTraineeFamilyNok : PhoenixBasePage
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
			if (!IsPostBack)
			{
				HookOnFocus(this.Page as Control);
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
				ucRelation.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION).ToString();
				if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
				{
					ViewState["EMPLOYEEFAMILYID"] = Request.QueryString["familyid"];
				}

				SetPreSeaTraineePrimaryDetails();
				ucAddress.Country = "97";
				SetPreSeaTraineeFamilyDetails();
				SetPreSeaTraineeSiblings();
                RadTextBox txtBankAddress1 = (RadTextBox)(ucBankAddress.FindControl("txtAddressLine1"));
				UserControlCountry ddlCountry = (UserControlCountry)(ucBankAddress.FindControl("ddlCountry"));
				UserControlCity ddlcity = (UserControlCity)(ucBankAddress.FindControl("ddlCity"));
				txtBankAddress1.CssClass = "input";
				ddlCountry.CssClass = "input";
				ddlcity.CssClass = "input";
				Page.ClientScript.RegisterStartupScript(
				typeof(PreSeaTraineeFamilyNok),
				"ScriptDoFocus",
				SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
				true);
			}
			CreateTabs();
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
	private void CreateTabs()
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("New", "NEW");
		toolbarmain.AddButton("Save", "SAVE");

		if (ViewState["EMPLOYEEFAMILYID"] != null)
		{
			toolbarmain.AddImageLink("javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				+ PhoenixModule.PRESEA + "&type=" + PhoenixPreSeaAttachmentType.FAMILY + "&cmdname=FAMILYNOKUPLOAD'); return false;", "Attachment", "", "ATTACHMENT");
			toolbarmain.AddImageLink("javascript: return fnConfirmDelete(event);", "Delete", string.Empty, "DELETE");
		}
		PreSeaFamily.AccessRights = this.ViewState;
		PreSeaFamily.MenuList = toolbarmain.Show();

		toolbarmain = new PhoenixToolbar();
	}
	protected void PreSeaFamily_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidate(txtFamilyFirstName.Text, ucSex.SelectedHard, ucRelation.SelectedQuick
									, ucAddress.Address1, ucAddress.Country, txtEmail.Text, ucMobileNumber.Text))
				{
					ucError.Visible = true;
					return;
				}
				if (!ucPhoneNumber.IsValidPhoneNumber())
				{
					ucError.ErrorMessage = "Enter area code for phone number";
					ucError.Visible = true;
					return;
				}

				if (ViewState["EMPLOYEEFAMILYID"] == null)
				{
					SavePreSeaTraineeFamilyDetails();

				}
				else
				{
					UpdateTraineeFamilyDetails();
				}
			}
			else if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				ResetFormControlValues(this);
				SetPreSeaTraineePrimaryDetails();
				ViewState["EMPLOYEEFAMILYID"] = null;
				imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
			}
			else if (dce.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixPreSeaTraineeFamilyNok.DeletePreSeaTraineeFamily(General.GetNullableInteger(ViewState["EMPLOYEEFAMILYID"].ToString()).Value);
				ViewState["EMPLOYEEFAMILYID"] = null;
				ResetFormControlValues(this);
				SetPreSeaTraineePrimaryDetails();
				SetPreSeaTraineeFamilyDetails();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	public void SetPreSeaTraineePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
			if (dt.Rows.Count > 0)
			{
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
			}
			dt = PhoenixPreSeaTraineeFamilyNok.ListPreSeaTraineeFamily(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), null);
			lstFamily.Items.Clear();
			ListItem items = new ListItem();
			lstFamily.DataSource = dt;
			lstFamily.DataTextField = "FLDFIRSTNAME";
			lstFamily.DataValueField = "FLDFAMILYID";
			lstFamily.DataBind();

			if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
			{
				lstFamily.SelectedValue = Request.QueryString["familyid"];
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void SavePreSeaTraineeFamilyDetails()
	{
		try
		{

			DataTable family = PhoenixPreSeaTraineeFamilyNok.InsertPreSeaTraineeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																								, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																								, txtFamilyFirstName.Text
																								, txtFamilyMiddleName.Text
																								, txtFamilyLastName.Text
																								, Convert.ToInt32(ucSex.SelectedHard)
																								, Convert.ToInt32(ucRelation.SelectedQuick)
																								, General.GetNullableDateTime(ucDateOfBirth.Text)
																								, ucAddress.Address1
																								, ucAddress.Address2
																								, ucAddress.Address3
																								, ucAddress.Address4
																								, ucAddress.City
																								, General.GetNullableInteger(ucAddress.State)
																								, Convert.ToInt32(ucAddress.Country)
																								, ucAddress.PostalCode
																								, General.GetNullableString(string.Empty)
																								, ucPhoneNumber.Text
																								, ucMobileNumber.Text
																								, txtEmail.Text
																								, General.GetNullableDateTime(ucAnniversaryDate.Text)
																								, chkNOK.Checked ? byte.Parse("1") : byte.Parse("0")
																								, txtBankName.Text
																								, txtAccountNumber.Text
																								, txtBranch.Text
																								, ucBankAddress.Address1
																								, ucBankAddress.Address2
																								, ucBankAddress.Address3
																								, ucBankAddress.Address4
																								, General.GetNullableInteger(ucBankAddress.City)
																								, General.GetNullableInteger(ucBankAddress.State)
																								, General.GetNullableInteger(ucBankAddress.Country)
																								, ucBankAddress.PostalCode
																								, General.GetNullableInteger(ucNatioanlity.SelectedNationality)
																								, General.GetNullableString(txtOccupation.Text)
																								, General.GetNullableInteger(ddlAnualIncome.SelectedValue)
																								);

			ViewState["EMPLOYEEFAMILYID"] = family.Rows[0][0].ToString();
			ViewState["attachmentcode"] = family.Rows[0][1].ToString();
			if (txtFileUpload.HasFile)
			{
				if (Request.Files["txtFileUpload"].ContentLength > 0)
				{
					if (ViewState["dtkey"] == null || ViewState["dtkey"].ToString() == string.Empty)
					{
						PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.PRESEA, null, ".jpg,.png,.gif", string.Empty, "PRESEAIMAGE");
					}
					else
					{
						PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PRESEA, ".jpg,.png,.gif");
					}
				}
			}
			DataTable dt = PhoenixPreSeaTraineeFamilyNok.ListPreSeaTraineeFamily(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																	, General.GetNullableInteger(ViewState["EMPLOYEEFAMILYID"].ToString()));
			lstFamily.DataSource = dt;
			lstFamily.DataTextField = "FLDFIRSTNAME";
			lstFamily.DataValueField = "FLDFAMILYID";
			lstFamily.DataBind();
			SetPreSeaTraineeFamilyDetails();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void SetPreSeaTraineeFamilyDetails()
	{
		try
		{
			string familyid = null;
			if (ViewState["EMPLOYEEFAMILYID"] != null)
			{
				familyid = ViewState["EMPLOYEEFAMILYID"].ToString();
			}

			DataTable dt = PhoenixPreSeaTraineeFamilyNok.ListPreSeaTraineeFamily(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), General.GetNullableInteger(familyid));

			if (dt.Rows.Count > 0)
			{

				txtFamilyFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtFamilyMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtFamilyLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				ucSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
				ucNatioanlity.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
				ucRelation.SelectedQuick = dt.Rows[0]["FLDRELATIONSHIP"].ToString();
				ucDateOfBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
				ucAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
				ucAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
				ucAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
				ucAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();
				ucAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
				ucAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
				ucAddress.City = dt.Rows[0]["FLDCITY"].ToString();
				ucAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();
				//txtSTDCode.Text = dt.Rows[0]["FLDSTDCODE"].ToString();
				ucPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
				ucPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
				txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
				ucMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
				ucMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
				ucAnniversaryDate.Text = dt.Rows[0]["FLDANNIVERSARYDATE"].ToString();
				ViewState["EMPLOYEEFAMILYID"] = dt.Rows[0]["FLDFAMILYID"].ToString();
				chkNOK.Checked = dt.Rows[0]["FLDNOK"].ToString() == "1" ? true : false;
				txtBankName.Text = dt.Rows[0]["FLDBANKNAME"].ToString();
				txtAccountNumber.Text = dt.Rows[0]["FLDACCOUNTNUMBER"].ToString();
				txtBranch.Text = dt.Rows[0]["FLDBRANCH"].ToString();
				ucBankAddress.Address1 = dt.Rows[0]["FLDBANKADDRESS1"].ToString();
				ucBankAddress.Address2 = dt.Rows[0]["FLDBANKADDRESS2"].ToString();
				ucBankAddress.Address3 = dt.Rows[0]["FLDBANKADDRESS3"].ToString();
				ucBankAddress.Address4 = dt.Rows[0]["FLDBANKADDRESS4"].ToString();
				ucBankAddress.Country = dt.Rows[0]["FLDBANKCOUNTRY"].ToString();
				ucBankAddress.State = dt.Rows[0]["FLDBANKSTATE"].ToString();
				ucBankAddress.City = dt.Rows[0]["FLDBANKCITY"].ToString();
				ucBankAddress.PostalCode = dt.Rows[0]["FLDBANKPOSTALCODE"].ToString();

				txtOccupation.Text = dt.Rows[0]["FLDOCCUPATION"].ToString();

				ddlAnualIncome.SelectedValue = dt.Rows[0]["FLDPARENTSANUALINCOME"].ToString();
				lstFamily.SelectedValue = ViewState["EMPLOYEEFAMILYID"].ToString();

				ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
				DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "PRESEAIMAGE");
				ViewState["dtkey"] = string.Empty;
				if (dta.Rows.Count > 0)
				{
					ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
					imgPhoto.ImageUrl = HttpContext.Current.Session["sitepath"] + "/attachments/" + dta.Rows[0]["FLDFILEPATH"].ToString();
					aPreSeaFamilyImg.HRef = "#";
					aPreSeaFamilyImg.Attributes["onclick"] = "javascript:parent.Openpopup('codehelp1', '', '" + imgPhoto.ImageUrl + "');";
				}
				else
				{
					imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void UpdateTraineeFamilyDetails()
	{
		try
		{
			if (txtFileUpload.HasFile)
			{
				if (Request.Files["txtFileUpload"].ContentLength > 0)
				{
					if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
					{
						PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.PRESEA, null, ".jpg,.png,.gif", string.Empty, "PRESEAIMAGE");
					}
					else
					{
						PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PRESEA, ".jpg,.png,.gif");
					}
				}
			}

			PhoenixPreSeaTraineeFamilyNok.UpdatePreSeaTraineeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																	, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
																	, Convert.ToInt32(ViewState["EMPLOYEEFAMILYID"].ToString())
																	, txtFamilyFirstName.Text
																	, txtFamilyMiddleName.Text
																	, txtFamilyLastName.Text
																	, Convert.ToInt32(ucSex.SelectedHard)
																	, Convert.ToInt32(ucRelation.SelectedQuick)
																	, General.GetNullableDateTime(ucDateOfBirth.Text)
																	, ucAddress.Address1
																	, ucAddress.Address2
																	, ucAddress.Address3
																	, ucAddress.Address4
																	, ucAddress.City
																	, General.GetNullableInteger(ucAddress.State)
																	, Convert.ToInt32(ucAddress.Country)
																	, ucAddress.PostalCode
																	, General.GetNullableString(string.Empty)
																	, ucPhoneNumber.Text
																	, ucMobileNumber.Text
																	, txtEmail.Text
																	, General.GetNullableDateTime(ucAnniversaryDate.Text)
																	, chkNOK.Checked ? byte.Parse("1") : byte.Parse("0")
																	, txtBankName.Text
																	, txtAccountNumber.Text
																	, txtBranch.Text
																	, ucBankAddress.Address1
																	, ucBankAddress.Address2
																	, ucBankAddress.Address3
																	, ucBankAddress.Address4
																	, General.GetNullableInteger(ucBankAddress.City)
																	, General.GetNullableInteger(ucBankAddress.State)
																	, General.GetNullableInteger(ucBankAddress.Country)
																	, ucBankAddress.PostalCode
																	, General.GetNullableInteger(ucNatioanlity.SelectedNationality)
																	, General.GetNullableString(txtOccupation.Text)
																	, General.GetNullableInteger(ddlAnualIncome.SelectedValue)
																	);
			ucStatus.Text = "Family Member information Updated.";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		SetPreSeaTraineeFamilyDetails();
	}
	private bool IsValidate(string firstname, string sex, string relation, string address1,
							string country, string email, string mobilenumber)
	{

		ucError.HeaderMessage = "Please provide the following required information";
		Int16 resultint;
		DateTime resultdate;

		if (firstname.Trim() == "")
			ucError.ErrorMessage = "First Name is required";

		if (sex.Equals("") || !Int16.TryParse(sex, out resultint))
			ucError.ErrorMessage = "Sex  is required";

		if (relation.Equals("") || !Int16.TryParse(relation, out resultint))
			ucError.ErrorMessage = "Relationship  is required";

		if (address1.Trim() == "")
			ucError.ErrorMessage = "Address1 is required";

		if (country.Equals("") || !Int16.TryParse(country, out resultint))
			ucError.ErrorMessage = "Country  is required";
		if (ucAddress.City.Equals("") || !Int16.TryParse(ucAddress.City, out resultint))
			ucError.ErrorMessage = "City  is required";
		if (txtEmail.Text != string.Empty && !General.IsvalidEmail(txtEmail.Text))
		{
			ucError.ErrorMessage = "E-Mail is not valid";
		}

		if (!string.IsNullOrEmpty(ucDateOfBirth.Text) && DateTime.TryParse(ucDateOfBirth.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Date of Birth should be earlier than current date";
		}

		if (!string.IsNullOrEmpty(ucAnniversaryDate.Text) && DateTime.TryParse(ucAnniversaryDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Anniversary Date should be earlier than current date";
		}
		//if (mobilenumber == null)
		//    ucError.ErrorMessage = "Mobile Number is required";

		return (!ucError.IsError);


	}
	public StateBag ReturnViewState()
	{
		return ViewState;
	}
	private void ResetFormControlValues(Control parent)
	{

		try
		{
			foreach (Control c in parent.Controls)
			{
				if (c.Controls.Count > 0)
				{
					ResetFormControlValues(c);
				}
				else
				{
					switch (c.GetType().ToString())
					{
						case "System.Web.UI.WebControls.TextBox":
							((TextBox)c).Text = "";
							break;
						case "System.Web.UI.WebControls.CheckBox":
							((CheckBox)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.RadioButton":
							((RadioButton)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.DropDownList":
							((DropDownList)c).SelectedIndex = 0;
							break;
						case "System.Web.UI.WebControls.ListBox":
							((ListBox)c).SelectedIndex = -1;
							break;

					}
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void lstFamily_SelectedIndexChanged(object sender, EventArgs e)
	{
		ViewState["EMPLOYEEFAMILYID"] = lstFamily.SelectedValue.ToString();

		SetPreSeaTraineeFamilyDetails();
	}
	public void SetPreSeaTraineeSiblings()
	{
		//DataTable dt = PhoenixPreSeaTraineePersonal.PreSeaTraineeSiblingsList(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
		//if (dt.Rows.Count > 0)
		//{
		//    txtSiblings.Text = dt.Rows[0]["FLDSIBLINGS"].ToString();
		//}

	}
	protected void CopyAddress_Click(object sender, EventArgs e)
	{
		LinkButton lnk = sender as LinkButton;
		int i = lnk.ID == "lnkCPA" ? 0 : 1;
		DataTable dt = PhoenixPreSeaTraineePersonal.ListPreSeaTraineeAddress(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection));
		if (dt.Rows.Count > 0)
		{
			ucAddress.Address1 = dt.Rows[i]["FLDADDRESS1"].ToString();
			ucAddress.Address2 = dt.Rows[i]["FLDADDRESS2"].ToString();
			ucAddress.Address3 = dt.Rows[i]["FLDADDRESS3"].ToString();
			ucAddress.Address4 = dt.Rows[i]["FLDADDRESS4"].ToString();
			ucAddress.Country = dt.Rows[i]["FLDCOUNTRY"].ToString();
			ucAddress.State = dt.Rows[i]["FLDSTATE"].ToString();
			ucAddress.City = dt.Rows[i]["FLDCITY"].ToString();
			ucAddress.PostalCode = dt.Rows[i]["FLDPOSTALCODE"].ToString();
		}
	}
	protected void AnniversaryDetails(object sender, EventArgs e)
	{
		if (ucRelation.SelectedQuick == "237")
		{
			ucAnniversaryDate.CssClass = "input";
			ucAnniversaryDate.Enabled = true;
		}
		else
		{
			ucAnniversaryDate.CssClass = "readonlytextbox";
			ucAnniversaryDate.Enabled = false;

		}
	}

}
