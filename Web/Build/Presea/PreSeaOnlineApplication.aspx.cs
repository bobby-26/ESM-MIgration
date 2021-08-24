using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class PreSeaOnlineApplication : PhoenixBasePage
{

    #region :   Page Events     :

    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            ShowTab("divBasic");
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {

                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

                ucError.Visible = false;
                ucCountry.Visible = false;
                ViewState["FAMILYID"] = "";
                ViewState["INTERVIEWIDYN"] = "0";
                ViewState["PERSONALMAIL"] = "";
                rdoPayMode.SelectedValue = "1";
                rdoESMFamilyRelation.SelectedValue = "0";
                ddlEyeSight.SelectedValue = "0";
                ddlIllnessYN.SelectedValue = "0";

                TextBox txtPostal = (TextBox)PostalAddress.FindControl("txtPinCode");
                txtPostal.CssClass = "input_mandatory";

                TextBox txtPermanent = (TextBox)PermanentAddress.FindControl("txtPinCode");
                txtPermanent.CssClass = "input_mandatory";

                ucGender.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();

                ucGender.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , Convert.ToInt32(PhoenixHardTypeCode.SEX)
                                                                            , "M");

                DDInfo.Visible = false;
                NetPayInfo.Visible = true;
                OtherPayInfo.Visible = false;

                lnkAlready.Visible = true;
                lnkSignOut.Visible = false;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

                ucNationality.SelectedNationality = "97";

                BindCourse();

                if (!String.IsNullOrEmpty(Filter.CurrentPreSeaNewApplicantSelection))
                {
                    CheckApplicationCompletion();

                    FillPreSeaApplicantInformation();
                    CheckInterviewedYN();
                    LoadAddressPanel();
                    LoadPanelOtherDetails();
                    BindFamily();
                    FillPaymentDetails();
                    ListPreSeaOthers();
                    BindAwardCertificate();
                }
                ddlColourBlindness.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are not eligible for admission as you are colour blind, Are you sure you want to continue'); return false;");
            }

            if (ddlColourBlindness.SelectedItem.Value == "1")
                ddlColourBlindness.Attributes.Clear();
            else
            {
                ddlColourBlindness.Attributes.Clear();
                ddlColourBlindness.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are not eligible for admission as you are colour blind, Are you sure you want to continue'); return false;");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Primary Details Method :

    protected void CheckInterviewedYN()
    {
        if (!String.IsNullOrEmpty(Filter.CurrentPreSeaNewApplicantSelection))
        {
            int interviewedyn;
            interviewedyn = 0;
            PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantInterviewedCheckYN(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection), ref interviewedyn);

            if (interviewedyn == 1)
            {
                ViewState["INTERVIEWIDYN"] = "1";
                divCompletion.Visible = false;
                HideSaveButtons();
            }
            else
            {
                ViewState["INTERVIEWIDYN"] = "0";
            }
        }
        else
        {
            ViewState["INTERVIEWIDYN"] = "0";
        }
    }
    protected void HideExtraDetails()
    {
        trFamilyAddress.Visible = false;
        trIncome.Visible = false;
    }
    protected void HideSaveButtons()
    {
        btnBasicSave.Visible = false;
        btnOtherSave.Visible = false;
        btnPaymentSave.Visible = false;
    }

    protected void BindCourse()
    {
        DataTable dt = PhoenixPreSeaCourse.EditPreSeaCourse(null);
        ddlCourse.DataSource = dt;
        ddlCourse.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlCourse.Items.Insert(0, li);

        ddlBatch.Items.Insert(0, li);
        ddlBatch.DataBind();
    }

    protected void BindBatch(string course)
    {
        ddlBatch.DataSource = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), null, null);
        ddlBatch.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlBatch.Items.Insert(0, li);
    }

    public static bool IsValidName(string text)
    {

        string regex = "^[a-zA-Z. ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }

    private bool IsValidPrimaryDetails()
    {        
        
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        Int32 resultInt;

        if (divBasic.Visible == true)
        {
            if (int.TryParse(ddlCourse.SelectedValue, out resultInt) == false)
                ucError.ErrorMessage = "Course is required";

            if (int.TryParse(ddlBatch.SelectedValue, out resultInt) == false)
                ucError.ErrorMessage = "Batch is required";

            if (txtFirstname.Text.Trim() == "")
                ucError.ErrorMessage = "First Name is required";

            if (!IsValidName(txtFirstname.Text.Trim()) && !String.IsNullOrEmpty(txtFirstname.Text.Trim()))
                ucError.ErrorMessage = "Firstname should contain alphabets only";

            if (!IsValidName(txtMiddlename.Text.Trim()) && !String.IsNullOrEmpty(txtMiddlename.Text.Trim()))
                ucError.ErrorMessage = "Middlename should contain alphabets only";

            if (!IsValidName(txtLastname.Text.Trim()) && !String.IsNullOrEmpty(txtLastname.Text.Trim()))
                ucError.ErrorMessage = "Lastname should contain alphabets only";

            if (!DateTime.TryParse(txtDateofBirth.Text, out resultDate))
            {
                ucError.ErrorMessage = "Date Of Birth is required";
            }
            else if (General.GetNullableInteger(ddlBatch.SelectedValue) != null && General.GetNullableDateTime(txtDateofBirth.Text) != null)
            {
                DataTable dt = PhoenixPreSeaBatch.PreSeaApplicantAgeValidationCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(ddlBatch.SelectedValue)
                , General.GetNullableDateTime(txtDateofBirth.Text));
                if (dt.Rows.Count > 0)
                {
                    string minagelimit = "";
                    string maxagelimit = "";
                    string minagevalidyn = "";
                    string maxagevalidyn = "";

                    DataRow dr = dt.Rows[0];
                    minagelimit = dr["FLDMINAGELIMIT"].ToString();
                    maxagelimit = dr["FLDMAXAGELIMIT"].ToString();
                    minagevalidyn = dr["FLDMINAGEVALIDYN"].ToString();
                    maxagevalidyn = dr["FLDMAXAGEVALIDYN"].ToString();

                    if (minagevalidyn != "1")
                    {
                        ucError.ErrorMessage = "Minimum age should be " + minagelimit + " and above.";
                    }
                    else if (maxagevalidyn != "1")
                    {
                        ucError.ErrorMessage = "Age should not exceed more than " + maxagelimit.ToString() + "years for this course.";
                    }
                }
            }
            if (Int32.TryParse(ucNationality.SelectedNationality, out resultInt) == false)
                ucError.ErrorMessage = "Nationality is required";

            if (Int32.TryParse(ucHighestQualificaiton.SelectedQualification, out resultInt) == false)
                ucError.ErrorMessage = "Your Highest Qualification is required";

            if (General.GetNullableString(txtEmail.Text) != null)
            {
                if (!General.IsvalidEmail(txtEmail.Text))
                    ucError.ErrorMessage = "Valid email is required";
            }
            else
                ucError.ErrorMessage = "Email is required";

            if (txtContact.Text.Trim() != "")
            {
                if (!General.IsValidPhoneNumber(txtContact.Text))
                {
                    ucError.ErrorMessage = "Valid contact no. is required";
                }
            }
            else
                ucError.ErrorMessage = "Contact No. is required";

            if (ddlIllnessYN.SelectedItem.Value == "1")
            {
                if (General.GetNullableString(txtIllnessDesc.Text) == null)
                    ucError.ErrorMessage = "Illness/operations description is required";
            }
            if (ddlColourBlindness.SelectedItem.Value == "1")
            {
                ucError.ErrorMessage = "Candidtes with Colour Blindness are not eligible for Admission.";
            }

        }
        if (divPersonal.Visible == true)
        {
            if (txtHeight.Text.Trim() == "")
                ucError.ErrorMessage = "Height is required";

            if (txtWeight.Text.Trim() == "")
                ucError.ErrorMessage = "Weight is required";
        }

        return (!ucError.IsError);
    }

    public void FillPreSeaApplicantInformation()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                lnkAlready.Visible = false;
                lnkSignOut.Visible = true;
                lblWelcome.Text = "Welcome, " + dt.Rows[0]["FLDNAME"].ToString();
                txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                ucNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
                if (dt.Rows[0]["FLDHEIGHT"].ToString() != "0.00" && dt.Rows[0]["FLDWEIGHT"].ToString() != "0.00"
                        && dt.Rows[0]["FLDHEIGHT"].ToString() != "" && dt.Rows[0]["FLDWEIGHT"].ToString() != "")
                {
                    txtHeight.Text = dt.Rows[0]["FLDHEIGHT"].ToString();
                    txtHeight.Text = txtHeight.Text.Substring(0, txtHeight.Text.IndexOf('.'));
                    txtWeight.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
                    txtWeight.Text = txtWeight.Text.Substring(0, txtWeight.Text.IndexOf('.'));
                }
                ddlEyeSight.SelectedValue = dt.Rows[0]["FLDEYESIGHT"].ToString();
                if (General.GetNullableInteger(dt.Rows[0]["FLDEYECOLORBLINDNESS"].ToString()) != null)
                    ddlColourBlindness.SelectedValue = dt.Rows[0]["FLDEYECOLORBLINDNESS"].ToString();
                txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();
                ddlIllnessYN.SelectedValue = dt.Rows[0]["FLDILLNESSYN"].ToString();

                if (dt.Rows[0]["FLDILLNESSYN"].ToString() == "1")
                    txtIllnessDesc.CssClass = "input_mandatory";

                txtIllnessDesc.Text = dt.Rows[0]["FLDILLNESSDESC"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDPERSONALMAIL"].ToString();
                txtContact.Text = dt.Rows[0]["FLDCONTACTNO"].ToString();
                ucHighestQualificaiton.SelectedQualification = dt.Rows[0]["FLDQUALIFICATIONID"].ToString();

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "PRESEAAPPLICANTPHOTO");
                ViewState["dtkey"] = string.Empty;
                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhoto.ImageUrl = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + dta.Rows[0]["FLDFILEPATH"].ToString();
                    aPreSeaImg.HRef = "#";
                    aPreSeaImg.Attributes["onclick"] = "javascript:parent.Openpopup('codehelp1', '', '" + imgPhoto.ImageUrl + "');";
                    if (General.GetNullableGuid(ViewState["dtkey"].ToString()) != null)
                    {
                        btnPhotoSave.Visible = false;
                        txtFileUpload.Visible = false;
                    }
                }

                ddlCourse.SelectedValue = dt.Rows[0]["FLDCOURSEID"].ToString();
                BindBatch(dt.Rows[0]["FLDCOURSEID"].ToString());
                ddlBatch.SelectedValue = dt.Rows[0]["FLDAPPLIEDBATCH"].ToString();
                ucGender.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();


                if (dt.Rows[0]["FLDISDECLARATIONCHECKED"].ToString().Equals("1"))
                {
                    chkDeclaration.Checked = true;
                    chkDeclaration.Enabled = false;
                }
                else
                {
                    chkDeclaration.Checked = false;
                    chkDeclaration.Enabled = true;
                }

            }
            else
            {
                lnkAlready.Visible = true;
                lblWelcome.Text = "Welcome";
                lnkSignOut.Visible = false;
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
            if (!IsValidPrimaryDetails())
            {
                ucError.Visible = true;
                return;
            }
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaNewApplicantSelection))
            {
                int iEmployeeId = 0;
                Guid iDtkey = new Guid();
                PhoenixPreSeaNewApplicantPersonal.InsertPreSeaNewApplicantPersonal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , txtFirstname.Text.Trim()
                                                                            , txtLastname.Text.Trim()
                                                                            , txtMiddlename.Text.Trim()
                                                                            , Convert.ToInt32(ucNationality.SelectedNationality)
                                                                            , Convert.ToDateTime(txtDateofBirth.Text)
                                                                            , ""
                                                                            , Convert.ToInt32(ucGender.SelectedHard)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableDecimal(txtHeight.Text)
                                                                            , General.GetNullableDecimal(txtWeight.Text)
                                                                            , null //appno
                                                                            , null //doj                              
                                                                            , General.GetNullableInteger(ddlEyeSight.SelectedValue)
                                                                            , General.GetNullableInteger(ddlColourBlindness.SelectedValue)
                                                                            , txtDistinguishMark.Text
                                                                            , General.GetNullableInteger(ddlIllnessYN.SelectedValue)
                                                                            , txtIllnessDesc.Text
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableString("")
                                                                            , General.GetNullableInteger(ddlBatch.SelectedValue)
                                                                            , General.GetNullableInteger(ddlCourse.SelectedValue)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableInteger("")
                                                                            , txtEmail.Text.Trim()
                                                                            , txtContact.Text.Trim()
                                                                            , General.GetNullableInteger(ucHighestQualificaiton.SelectedQualification)
                                                                            , ref iEmployeeId
                                                                            , ref iDtkey);

                if (iEmployeeId > 0)
                {

                    Filter.CurrentPreSeaNewApplicantSelection = iEmployeeId.ToString();
                    //ucConfirm.Visible = true;
                    //ucConfirm.Text = "A mail will send to candidate to fill additional information in Online Application. Do you want to send";
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, iDtkey, PhoenixModule.PRESEA, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                    ViewState["attachmentcode"] = iDtkey.ToString();
                    Response.Redirect(Request.RawUrl);
                    //confirm message

                    return;
                }
            }
            else
            {
                PhoenixPreSeaNewApplicantPersonal.UpdatePreSeaNewApplicantPersonal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                            , General.GetNullableString("")
                                                                            , General.GetNullableString("")
                                                                            , txtFirstname.Text.Trim()
                                                                            , txtLastname.Text.Trim()
                                                                            , txtMiddlename.Text.Trim()
                                                                            , Convert.ToInt32(ucNationality.SelectedNationality)
                                                                            , Convert.ToDateTime(txtDateofBirth.Text)
                                                                            , ""
                                                                            , Convert.ToInt32(ucGender.SelectedHard)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableDecimal(txtHeight.Text)
                                                                            , General.GetNullableDecimal(txtWeight.Text)
                                                                            , null //appno
                                                                            , null //doj                              
                                                                            , General.GetNullableInteger(ddlEyeSight.SelectedValue)
                                                                            , General.GetNullableInteger(ddlColourBlindness.SelectedValue)
                                                                            , txtDistinguishMark.Text
                                                                            , General.GetNullableInteger(ddlIllnessYN.SelectedValue)
                                                                            , txtIllnessDesc.Text
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableString("")
                                                                            , General.GetNullableInteger(ddlBatch.SelectedValue)
                                                                            , General.GetNullableInteger(ddlCourse.SelectedValue)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableInteger("")
                                                                            , txtEmail.Text.Trim()
                                                                            , txtContact.Text.Trim()
                                                                            , General.GetNullableInteger(ucHighestQualificaiton.SelectedQualification)
                                                                            );
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        FillPreSeaApplicantInformation();
    }

    public void CheckApplicationCompletion()
    {
        decimal percentage;
        percentage = 0;

        if (!String.IsNullOrEmpty(Filter.CurrentPreSeaNewApplicantSelection))
        {
            PhoenixPreSeaNewApplicantPersonal.PreseaOnlineApplicationCompletion(PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection.ToString())
             , ref percentage);
        }

        lblPercentageCompletion.Text = "<b>" + percentage.ToString() + "</b>";
    }


    #endregion

    #region :   Other Panel Methods    :

    protected void LoadPanelOtherDetails()
    {
        BindCheckBoxValues();
        ListPreSeaExamVenue();
        ucCountry_TextChanged("97", null);
        ddlState_TextChanged(null, null);

    }

    public void ListPreSeaExamVenue()
    {
        DataSet ds = PhoenixPreSeaExamVenue.SearchBatchExamVenueList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            rblExamVenueFirst.DataSource = ds.Tables[0];
            rblExamVenueFirst.DataBind();
            rblExamVenueSecond.DataSource = ds.Tables[0];
            rblExamVenueSecond.DataBind();
        }
    }

    public void ListPreSeaOthers()
    {
        DataTable dt = PhoenixPreSeaNewApplicantOthers.PreSeaNewApplicantOtherDetailsList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
        if (dt.Rows.Count > 0)
        {
            ucNewspaperMagazine.SelectedQuick = dt.Rows[0]["FLDNEWSPAPERMAGAZINE"].ToString();
            ucSchoolCollage.SelectedQuick = dt.Rows[0]["FLDSCHOOLCOLLEGE"].ToString();

            ucState.SelectedState = dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString();

            ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger("97")
            , General.GetNullableInteger(dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString()) == null ? 0 : General.GetNullableInteger(dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString()));

            ddlPlace.SelectedCity = dt.Rows[0]["FLDEDUCATIONJOBFAIRCITY"].ToString();
            txtInternet.Text = dt.Rows[0]["FLDINTERNET"].ToString();
            ucDirectContact.SelectedQuick = dt.Rows[0]["FLDDIRECTCONTACT"].ToString();
            txtOthers.Text = dt.Rows[0]["FLDOTHERS"].ToString();
            ViewState["OTHERSID"] = dt.Rows[0]["FLDOTHERSID"].ToString();

            rblExamVenueFirst.SelectedValue = dt.Rows[0]["FLDEXAMVENUE1"].ToString();
            if (General.GetNullableInteger(dt.Rows[0]["FLDEXAMVENUE2"].ToString()) != null)
                rblExamVenueSecond.SelectedValue = dt.Rows[0]["FLDEXAMVENUE2"].ToString();
            txtAboutYourselfRemarks.Text = dt.Rows[0]["FLDABOUTYOURSELFREMARKS"].ToString();

            string[] knowntype = dt.Rows[0]["FLDINSTITUTEKNOWNTYPE"].ToString().Split(',');

            foreach (string item in knowntype)
            {
                if (item != "")
                {
                    if (chkNewspaperMagazine.InputAttributes["value"] == item.ToString())
                        chkNewspaperMagazine.Checked = true;
                    if (chkFamilyRelativeFriends.InputAttributes["value"] == item.ToString())
                        chkFamilyRelativeFriends.Checked = true;
                    if (chkSchoolCollege.InputAttributes["value"] == item.ToString())
                        chkSchoolCollege.Checked = true;
                    if (chkEducationJoFfair.InputAttributes["value"] == item.ToString())
                        chkEducationJoFfair.Checked = true;
                    if (chkEmailBySims.InputAttributes["value"] == item.ToString())
                        chkEmailBySims.Checked = true;
                    if (chkShiksha.InputAttributes["value"] == item.ToString())
                        chkShiksha.Checked = true;
                    if (chkInternet.InputAttributes["value"] == item.ToString())
                        chkInternet.Checked = true;
                    if (chkFlyers.InputAttributes["value"] == item.ToString())
                        chkFlyers.Checked = true;
                    if (chkDirectContact.InputAttributes["value"] == item.ToString())
                        chkDirectContact.Checked = true;
                    if (chkOthers.InputAttributes["value"] == item.ToString())
                        chkOthers.Checked = true;
                }
            }
        }

    }

    public void BindCheckBoxValues()
    {
        chkNewspaperMagazine.InputAttributes.Add("value", "1217");
        chkFamilyRelativeFriends.InputAttributes.Add("value", "1218");
        chkSchoolCollege.InputAttributes.Add("value", "1219");
        chkEducationJoFfair.InputAttributes.Add("value", "1220");
        chkEmailBySims.InputAttributes.Add("value", "1221");
        chkShiksha.InputAttributes.Add("value", "1222");
        chkInternet.InputAttributes.Add("value", "1223");
        chkFlyers.InputAttributes.Add("value", "1224");
        chkDirectContact.InputAttributes.Add("value", "1225");
        chkOthers.InputAttributes.Add("value", "1226");
    }

    protected void SaveOtherDetails()
    {
        if (!IsValidOtherDetails())
        {
            ucError.Visible = true;
            return;
        }
        else
        {
            StringBuilder strknowntype = new StringBuilder();

            if (chkNewspaperMagazine.Checked == true || General.GetNullableInteger(ucNewspaperMagazine.SelectedValue) != null)
            {
                strknowntype = strknowntype.Append(chkNewspaperMagazine.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkFamilyRelativeFriends.Checked == true)
            {
                strknowntype = strknowntype.Append(chkFamilyRelativeFriends.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkSchoolCollege.Checked == true)
            {

                strknowntype = strknowntype.Append(chkSchoolCollege.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkEducationJoFfair.Checked == true)
            {
                strknowntype = strknowntype.Append(chkEducationJoFfair.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkEmailBySims.Checked == true)
            {
                strknowntype = strknowntype.Append(chkEmailBySims.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkShiksha.Checked == true)
            {
                strknowntype = strknowntype.Append(chkShiksha.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkInternet.Checked == true || (!string.IsNullOrEmpty(txtInternet.Text)))
            {
                strknowntype = strknowntype.Append(chkInternet.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkFlyers.Checked == true)
            {
                strknowntype = strknowntype.Append(chkFlyers.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkDirectContact.Checked == true)
            {
                strknowntype = strknowntype.Append(chkDirectContact.InputAttributes["value"]);
                strknowntype.Append(",");
            }
            if (chkOthers.Checked == true || (!string.IsNullOrEmpty(txtOthers.Text)))
            {
                strknowntype = strknowntype.Append(chkOthers.InputAttributes["value"]);
                strknowntype.Append(",");
            }

            string secondchoice = rblExamVenueSecond.SelectedValue;

            if (ViewState["OTHERSID"] != null)
            {
                PhoenixPreSeaNewApplicantOthers.UpdatePreSeaNewApplicantOtherDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , General.GetNullableInteger(ucNewspaperMagazine.SelectedQuick)
                                                         , General.GetNullableInteger(ucSchoolCollage.SelectedQuick)
                                                         , General.GetNullableInteger(ucState.SelectedState)
                                                         , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                         , txtInternet.Text
                                                         , General.GetNullableInteger(ucDirectContact.SelectedQuick)
                                                         , txtOthers.Text
                                                         , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                         , General.GetNullableInteger(ViewState["OTHERSID"].ToString())
                                                         , General.GetNullableInteger(rblExamVenueFirst.SelectedValue.ToString())
                                                         , General.GetNullableInteger(secondchoice)
                                                         , General.GetNullableString(txtAboutYourselfRemarks.Text)
                                                         , strknowntype.ToString());
                ListPreSeaOthers();
            }
            else
            {
                PhoenixPreSeaNewApplicantOthers.InertPreSeaNewApplicantOtherDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , General.GetNullableInteger(ucNewspaperMagazine.SelectedQuick)
                                                         , General.GetNullableInteger(ucSchoolCollage.SelectedQuick)
                                                         , General.GetNullableInteger(ucState.SelectedState)
                                                         , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                         , txtInternet.Text
                                                         , General.GetNullableInteger(ucDirectContact.SelectedQuick)
                                                         , txtOthers.Text
                                                         , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                         , General.GetNullableInteger(rblExamVenueFirst.SelectedValue.ToString())
                                                         , General.GetNullableInteger(secondchoice)
                                                         , General.GetNullableString(txtAboutYourselfRemarks.Text)
                                                         , strknowntype.ToString());
                ListPreSeaOthers();
            }

        }
    }

    private bool IsValidOtherDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(rblExamVenueFirst.SelectedValue))
            ucError.ErrorMessage = "First Choice exam venue is mandatory.";

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Address Panel Methods    :

    protected void LoadAddressPanel()
    {
        PostalAddress.Country = "97";
        PermanentAddress.Country = "97";
        SetPreSeaNewApplicantAddress();
    }

    protected void chkCopyAddress_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        UserControlCommonAddress suc = (chk.ID == "chkPostalCopyAddress" ? PermanentAddress : PostalAddress);
        UserControlCommonAddress tuc = (chk.ID == "chkPostalCopyAddress" ? PostalAddress : PermanentAddress);
        int i = chk.ID == "chkPostalCopyAddress" ? 0 : 1;
        try
        {
            if (chk.Checked == true)
            {

                tuc.Address1 = suc.Address1;
                tuc.Address2 = suc.Address2;
                tuc.Address3 = suc.Address3;
                tuc.Address4 = suc.Address4;
                if (suc.Country == "Dummy")
                {
                    tuc.Country = "0";
                }
                else
                {
                    tuc.Country = suc.Country;
                }
                if (suc.State == "Dummy")
                {
                    tuc.State = "0";
                }
                else
                {
                    tuc.State = suc.State;
                }
                if (suc.City == "Dummy")
                {
                    tuc.City = "0";
                }
                else
                {
                    tuc.City = suc.City;
                }
                tuc.PostalCode = suc.PostalCode;

            }
            else
            {
                tuc.Address1 = "";
                tuc.Address2 = "";
                tuc.Address3 = "";
                tuc.Address4 = "";
                tuc.Country = "";
                tuc.State = "";
                tuc.City = "";
                tuc.PostalCode = "";

                DataTable dt = PhoenixPreSeaNewApplicantPersonalAddress.ListPreSeaNewApplicantAddress(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
                if (dt.Rows.Count > 0)
                {
                    tuc.Address1 = dt.Rows[i]["FLDADDRESS1"].ToString();
                    tuc.Address2 = dt.Rows[i]["FLDADDRESS2"].ToString();
                    tuc.Address3 = dt.Rows[i]["FLDADDRESS3"].ToString();
                    tuc.Address4 = dt.Rows[i]["FLDADDRESS4"].ToString();
                    tuc.Country = dt.Rows[i]["FLDCOUNTRY"].ToString();
                    tuc.State = dt.Rows[i]["FLDSTATE"].ToString();
                    tuc.City = dt.Rows[i]["FLDCITY"].ToString();
                    tuc.PostalCode = dt.Rows[i]["FLDPOSTALCODE"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SetPreSeaNewApplicantAddress()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonalAddress.ListPreSeaNewApplicantAddress(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {

                PermanentAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                PermanentAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                PermanentAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                PermanentAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();

                PermanentAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
                PermanentAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
                PermanentAddress.City = dt.Rows[0]["FLDCITY"].ToString();
                PermanentAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();

                ViewState["EMPLOYEEPERMANENTADDRESSID"] = dt.Rows[0]["FLDEMPLOYEEADDRESSID"].ToString();

                PostalAddress.Address1 = dt.Rows[1]["FLDADDRESS1"].ToString();
                PostalAddress.Address2 = dt.Rows[1]["FLDADDRESS2"].ToString();
                PostalAddress.Address3 = dt.Rows[1]["FLDADDRESS3"].ToString();
                PostalAddress.Address4 = dt.Rows[1]["FLDADDRESS4"].ToString();

                PostalAddress.Country = dt.Rows[1]["FLDCOUNTRY"].ToString();
                PostalAddress.State = dt.Rows[1]["FLDSTATE"].ToString();
                PostalAddress.City = dt.Rows[1]["FLDCITY"].ToString();
                PostalAddress.PostalCode = dt.Rows[1]["FLDPOSTALCODE"].ToString();

                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SaveCommunicationDetails()
    {
        if (ViewState["EMPLOYEEPERMANENTADDRESSID"] == null)
        {
            SavePreSeaNewApplicantAddress();
        }
        else
        {
            UpdatePreSeaNewApplicantAddress();
        }

        UpdatePreSeaMainPersonalInformation();
    }

    public void SavePreSeaNewApplicantAddress()
    {
        try
        {

            if (!IsValidate(PostalAddress.Address1, PostalAddress.City, PostalAddress.State, PostalAddress.Country,
                     txtEmail.Text, PostalAddress.PostalCode, "Postal"))
            {
                ucError.Visible = true;
                return;
            }
            if (!IsValidate(PermanentAddress.Address1, PermanentAddress.City, PermanentAddress.State, PermanentAddress.Country,
                        txtEmail.Text, PermanentAddress.PostalCode, "Permanent"))
            {
                ucError.Visible = true;
                return;
            }
            int localaddressid = PhoenixPreSeaNewApplicantPersonalAddress.InsertPreSeaNewApplicantAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                      , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                                      , Convert.ToInt32(PhoenixPreSeaConstants.POSTALADDRESS)
                                                                                                      , PostalAddress.Address1
                                                                                                      , General.GetNullableString(PostalAddress.Address2)
                                                                                                      , General.GetNullableString(PostalAddress.Address3)
                                                                                                      , General.GetNullableString(PostalAddress.Address4)
                                                                                                      , PostalAddress.City
                                                                                                      , General.GetNullableInteger(PostalAddress.State)
                                                                                                      , Convert.ToInt32(PostalAddress.Country)
                                                                                                      , PostalAddress.PostalCode
                                                                                                      , General.GetNullableString(string.Empty)
                                                                                                      , ""
                                                                                                      , ""
                                                                                                      , txtEmail.Text
                                                                                                      , ""
                                                                                                      , ""
                                                                                                      , ""
                                                                                                      , null
                                                                                                      , General.GetNullableInteger("")
                                                                                                      , General.GetNullableInteger("")
                                                                                                      , General.GetNullableInteger("")
                                                                                                      );

            int Permanentaddressid = PhoenixPreSeaNewApplicantPersonalAddress.InsertPreSeaNewApplicantAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                        , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                                        , Convert.ToInt32(PhoenixPreSeaConstants.PERMANENTADDRESS)
                                                                                                        , PermanentAddress.Address1
                                                                                                        , General.GetNullableString(PermanentAddress.Address2)
                                                                                                        , General.GetNullableString(PermanentAddress.Address3)
                                                                                                        , General.GetNullableString(PermanentAddress.Address4)
                                                                                                        , PermanentAddress.City
                                                                                                        , General.GetNullableInteger(PermanentAddress.State)
                                                                                                        , Convert.ToInt32(PermanentAddress.Country)
                                                                                                        , PermanentAddress.PostalCode
                                                                                                        , General.GetNullableString(string.Empty)
                                                                                                        , ""
                                                                                                        , ""
                                                                                                        , txtEmail.Text
                                                                                                        , ""
                                                                                                        , ""
                                                                                                        , ""
                                                                                                        , null
                                                                                                        , General.GetNullableInteger("")
                                                                                                        , General.GetNullableInteger("")
                                                                                                        , General.GetNullableInteger("")
                                                                                                        );




            SetPreSeaNewApplicantAddress();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void UpdatePreSeaNewApplicantAddress()
    {
        try
        {
            if (!IsValidate(PermanentAddress.Address1, PermanentAddress.City, PermanentAddress.State, PermanentAddress.Country,
                        txtEmail.Text, PermanentAddress.PostalCode, "Permanent"))
            {
                ucError.Visible = true;
                return;
            }
            if (!IsValidate(PostalAddress.Address1, PostalAddress.City, PostalAddress.State, PostalAddress.Country,
                     txtEmail.Text, PostalAddress.PostalCode, "Postal"))
            {
                ucError.Visible = true;
                return;
            }


            int localaddressid = PhoenixPreSeaNewApplicantPersonalAddress.UpdatePreSeaNewApplicantAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                            , Convert.ToInt32(ViewState["EMPLOYEELOCALADDRESSID"].ToString())
                                                                                            , Convert.ToInt32(PhoenixPreSeaConstants.POSTALADDRESS)
                                                                                            , PostalAddress.Address1
                                                                                            , General.GetNullableString(PostalAddress.Address2)
                                                                                            , General.GetNullableString(PostalAddress.Address3)
                                                                                            , General.GetNullableString(PostalAddress.Address4)
                                                                                            , PostalAddress.City
                                                                                            , General.GetNullableInteger(PostalAddress.State)
                                                                                            , Convert.ToInt32(PostalAddress.Country)
                                                                                            , PostalAddress.PostalCode
                                                                                            , General.GetNullableString(string.Empty)
                                                                                            , null
                                                                                            , null
                                                                                            , txtEmail.Text
                                                                                            , null
                                                                                            , null
                                                                                            , null
                                                                                            , null
                                                                                            , General.GetNullableInteger("")
                                                                                            , General.GetNullableInteger("")
                                                                                            , General.GetNullableInteger("")
                                                                                            );

            int Permanentaddressid = PhoenixPreSeaNewApplicantPersonalAddress.UpdatePreSeaNewApplicantAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                            , Convert.ToInt32(ViewState["EMPLOYEEPERMANENTADDRESSID"].ToString())
                                                                                            , Convert.ToInt32(PhoenixPreSeaConstants.PERMANENTADDRESS)
                                                                                            , PermanentAddress.Address1
                                                                                            , General.GetNullableString(PermanentAddress.Address2)
                                                                                            , General.GetNullableString(PermanentAddress.Address3)
                                                                                            , General.GetNullableString(PermanentAddress.Address4)
                                                                                            , PermanentAddress.City
                                                                                            , General.GetNullableInteger(PermanentAddress.State)
                                                                                            , Convert.ToInt32(PermanentAddress.Country)
                                                                                            , PermanentAddress.PostalCode
                                                                                            , General.GetNullableString(string.Empty)
                                                                                            , null
                                                                                            , null
                                                                                            , txtEmail.Text
                                                                                            , null
                                                                                            , null
                                                                                            , null
                                                                                            , null
                                                                                            , General.GetNullableInteger("")
                                                                                            , General.GetNullableInteger("")
                                                                                            , General.GetNullableInteger("")
                                                                                            );

            SetPreSeaNewApplicantAddress();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidate(string address1, string city, string state, string county
                           , string email, string postal, string prefix)
    {
        int employeecountry;
        ucError.HeaderMessage = "Please provide the following required information";

        if (address1.Trim() == "")
            ucError.ErrorMessage = prefix + " Address1 is required";

        if (Int32.TryParse(city, out employeecountry) == false)
            ucError.ErrorMessage = prefix + " City is required";

        //if (state.Trim() == "")
        //    ucError.ErrorMessage = "State is required";

        if (Int32.TryParse(county, out employeecountry) == false)
            ucError.ErrorMessage = prefix + " Country is required";

        if (email.Trim() == "")
            ucError.ErrorMessage = "E-Mail is required";
        else if (!General.IsvalidEmail(txtEmail.Text))
        {
            ucError.ErrorMessage = "Please enter valid E-Mail";
        }

        if (postal.Trim() == "")
            ucError.ErrorMessage = prefix + " Postal Code is required";

        return (!ucError.IsError);
    }

    #endregion

    #region :   Extra Panel Methods :

    private void BindAwardCertificate()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixPreSeaAwardAndCertificate.PreSeaAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                     , null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAwardAndCertificate.DataSource = ds.Tables[0];
                gvAwardAndCertificate.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvAwardAndCertificate);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCertificate(string certificate, string issuedate, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(certificate) == null)
            ucError.ErrorMessage = "Award/Certificate is required";

        if (General.GetNullableDateTime(issuedate) == null)
            ucError.ErrorMessage = "Issue Date is required";

        if (remarks.Trim() == "")
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }

    #endregion

    #region :   Family Panel Methods :

    private void BindFamily()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;


            DataSet ds = PhoenixPreSeaNewApplicantFamilyNok.SearchPreSeaNewApplicantFamily(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                     , null, General.GetNullableString(""), General.GetNullableString(""), null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFamily.DataSource = ds.Tables[0];
                gvFamily.DataBind();

                if (General.GetNullableInteger(ViewState["FAMILYID"].ToString()) == null)
                {
                    ViewState["FAMILYID"] = ds.Tables[0].Rows[0]["FLDFAMILYID"].ToString();
                    gvFamily.SelectedIndex = 0;
                }
                FillFamilyCommunicationDetails(General.GetNullableInteger(ViewState["FAMILYID"].ToString()));
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvFamily);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidFamilyDetails(string name, string relation, string gender)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Name is required";

        if (General.GetNullableInteger(relation) == null)
            ucError.ErrorMessage = "Relation is required";

        if (General.GetNullableInteger(gender) == null)
            ucError.ErrorMessage = "Gender is required";

        return (!ucError.IsError);
    }

    private bool IsValidFamilyAddressDetails(string Address1, string country, string city, string familyemail)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["INTERVIEWIDYN"].ToString() == "1")
        {
            if (String.IsNullOrEmpty(Address1))
                ucError.ErrorMessage = "Address 1 is required";

            if (General.GetNullableInteger(country) == null)
                ucError.ErrorMessage = "Country is required";

            if (General.GetNullableInteger(city) == null)
                ucError.ErrorMessage = "City is required";
        }
        if (General.GetNullableString(familyemail) != null)
        {
            if (!General.IsvalidEmail(familyemail))
            {
                ucError.ErrorMessage = "Invalid Email";
            }
        }
        return (!ucError.IsError);
    }

    private void ClearFamilyCommunicationDetails()
    {
        ucFamilyAddress.Address1 = "";
        ucFamilyAddress.Address2 = "";
        ucFamilyAddress.Address3 = "";
        ucFamilyAddress.Address4 = "";
        ucFamilyAddress.City = "";
        ucFamilyAddress.State = "";
        ucFamilyAddress.Country = "";
        ucFamilyAddress.PostalCode = "";
        txtFamilyOfficeNo.Text = "";
        txtFamilyEmail.Text = "";
        ddlAnualIncome.SelectedValue = "0";
    }

    private void FillFamilyCommunicationDetails(int? FamilyId)
    {
        if (FamilyId != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixPreSeaNewApplicantFamilyNok.ListPreSeaNewApplicantFamily(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection), FamilyId);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ucFamilyAddress.Address1 = dr["FLDADDRESS1"].ToString();
                ucFamilyAddress.Address2 = dr["FLDADDRESS2"].ToString();
                ucFamilyAddress.Address3 = dr["FLDADDRESS3"].ToString();
                ucFamilyAddress.Address4 = dr["FLDADDRESS4"].ToString();
                ucFamilyAddress.Country = dr["FLDCOUNTRY"].ToString();
                ucFamilyAddress.State = dr["FLDSTATE"].ToString();
                ucFamilyAddress.City = dr["FLDCITY"].ToString();
                ucFamilyAddress.PostalCode = dr["FLDPOSTALCODE"].ToString();
                txtFamilyOfficeNo.Text = dr["FLDPHONENUMBER"].ToString();
                txtFamilyEmail.Text = dr["FLDEMAIL"].ToString();
                ddlAnualIncome.SelectedValue = dr["FLDPARENTSANUALINCOME"].ToString();
                if (General.GetNullableInteger(dr["FLDFAMILYRELATESTOESMYN"].ToString()) != null)
                    rdoESMFamilyRelation.SelectedValue = dr["FLDFAMILYRELATESTOESMYN"].ToString();
                txtESMFamilyNames.Text = dr["FLDFAMILYRELATESTOESM"].ToString();
            }
        }
        else
        {
            ClearFamilyCommunicationDetails();
        }
    }

    private void SaveFamilyCommunicationDetails()
    {
        try
        {

            if (!String.IsNullOrEmpty(ViewState["FAMILYID"].ToString()))
            {
                if (!IsValidFamilyAddressDetails(ucFamilyAddress.Address1, ucFamilyAddress.Country, ucFamilyAddress.City, txtFamilyEmail.Text))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPreSeaNewApplicantFamilyNok.UpdatePreSeaApplicantFamilyAddress(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                            , Convert.ToInt32(ViewState["FAMILYID"].ToString())
                            , ucFamilyAddress.Address1
                            , ucFamilyAddress.Address2
                            , ucFamilyAddress.Address3
                            , ucFamilyAddress.Address4
                            , ucFamilyAddress.City
                            , General.GetNullableInteger(ucFamilyAddress.State)
                            , Convert.ToInt32(ucFamilyAddress.Country)
                            , ucFamilyAddress.PostalCode
                            , txtFamilyOfficeNo.Text.Trim()
                            , General.GetNullableString(txtFamilyEmail.Text)
                            , General.GetNullableInteger(ddlAnualIncome.SelectedValue)
                            , General.GetNullableInteger(rdoESMFamilyRelation.SelectedValue)
                            , txtESMFamilyNames.Text.Trim()
                            );
                BindFamily();
            }
            else
            {
                ucError.Text = "Please add family name in the grid and click the 'Name' then enter correspondence details.";
                ucError.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Photo Panel Methods :

    private void UploadApplicantPhoto()
    {
        if (Request.Files["txtFileUpload"] != null && Request.Files["txtFileUpload"].ContentLength > 0)
        {
            if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
            {
                PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.PRESEA, null, ".jpg,.png,.gif", string.Empty, "PRESEAAPPLICANTPHOTO");
            }
            else
            {
                PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PRESEA, ".jpg,.png,.gif");
            }
            FillPreSeaApplicantInformation();
        }
    }

    #endregion

    #region :   Payment Details Methods :

    private void SavePaymentDetails()
    {
        if (!IsValidPaymentDetails())
        {
            ucError.Visible = true;
            return;
        }

        string ddacno = "", paydetais = "";

        switch (rdoPayMode.SelectedValue)
        {
            case "2":
                ddacno = txtDDNumber.Text.Trim();
                paydetais = txtDDDeatils.Text.Trim();
                break;
            case "3":
                paydetais = txtOtherPayDeatail.Text.Trim();
                break;
            default:
                ddacno = txtNETrfrACno.Text.Trim();
                paydetais = txtNETTrfrDetails.Text.Trim();
                break;
        }

        PhoenixPreSeaFees.RegistrationFeesDetailInsert(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection), byte.Parse(rdoPayMode.SelectedValue), ddacno, paydetais);
    }

    private void FillPaymentDetails()
    {
        DataTable dt = PhoenixPreSeaFees.RegistrationFeesDetailList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection), General.GetNullableInteger("0"));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            rdoPayMode.SelectedValue = dr["FLDPAYMODE"].ToString();
            PayMode_Changed(null, null);

            switch (dr["FLDPAYMODE"].ToString())
            {
                case "2":
                    txtDDNumber.Text = dr["FLDDDNO"].ToString();
                    txtDDDeatils.Text = dr["FLDDDDETAILS"].ToString();
                    break;
                case "3":
                    txtOtherPayDeatail.Text = dr["FLDOTHERTRFR"].ToString();
                    break;
                default:
                    txtNETrfrACno.Text = dr["FLDNETTRFRACNO"].ToString();
                    txtNETTrfrDetails.Text = dr["FLDNETTRFRDETAILS"].ToString();
                    break;
            }

        }
    }

    private bool IsValidPaymentDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rdoPayMode.SelectedIndex < 0)
            ucError.ErrorMessage = "Pay Mode is required";
        else
        {
            switch (rdoPayMode.SelectedValue)
            {
                case "2":
                    if (txtDDNumber.Text.Trim() == "")
                        ucError.ErrorMessage = "DD Number is required";
                    if (txtDDDeatils.Text.Trim() == "")
                        ucError.ErrorMessage = "DD Details is required";
                    break;
                case "3":
                    if (txtOtherPayDeatail.Text.Trim() == "")
                        ucError.ErrorMessage = "Payment Details is required";
                    break;
                default:
                    if (txtNETrfrACno.Text.Trim() == "")
                        ucError.ErrorMessage = "Transfer From(A/C No) is required";
                    if (txtNETTrfrDetails.Text.Trim() == "")
                        ucError.ErrorMessage = "Transaction Details is required";
                    break;
            }
        }

        return (!ucError.IsError);
    }

    #endregion

    #region :   Common Events   :

    protected void lnk_serverclick(object sender, EventArgs e)
    {
        HtmlAnchor a = (HtmlAnchor)sender;
        if (Filter.CurrentPreSeaNewApplicantSelection == null && !a.ID.Equals("lnkBasic"))
        {
            ShowTab("divBasic");
            ucError.ErrorMessage = "Please fill Basic Details first";
            ucError.Visible = true;
            return;
        }
        ShowTab(a.ID.Replace("lnk", "div"));


    }

    protected void Application_Save(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;

            switch (btn.CommandName)
            {
                case "Basic":
                    ShowTab("divBasic");
                    UpdatePreSeaMainPersonalInformation();
                    CheckApplicationCompletion();
                    break;
                case "Personal":
                    SaveCommunicationDetails();
                    CheckApplicationCompletion();
                    break;
                case "Family":
                    SaveFamilyCommunicationDetails();
                    CheckApplicationCompletion();
                    break;
                case "Other":
                    SaveOtherDetails();
                    CheckApplicationCompletion();
                    break;
                case "Photo":
                    UploadApplicantPhoto();
                    CheckApplicationCompletion();
                    break;
                case "Payment":
                    SavePaymentDetails();
                    CheckApplicationCompletion();
                    break;
                default:
                    break;
            }
            //ShowTab("divBasic");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["SUBMITCLICKED"] == null)
            Response.Redirect(Request.RawUrl);
        else
        {
            if (Session["SUBMITCLICKED"].ToString() == "1")
            {
                Session["SUBMITCLICKED"] = null;
                Response.Redirect("~/Presea/PreSeaOnlineApplicationComplete.aspx");
            }
        }
    }

    protected void lnkSignOut_Click(object sender, EventArgs e)
    {
        Filter.CurrentPreSeaNewApplicantSelection = null;
        Response.Redirect("~/Presea/PreSeaRegisterdCandidatesLogin.aspx");
    }

    protected void lnkAlready_Click(object sender, EventArgs e)
    {
        Filter.CurrentPreSeaNewApplicantSelection = null;
        Response.Redirect("~/Presea/PreSeaRegisterdCandidatesLogin.aspx");
    }

    #endregion

    #region :   Common Methods  :

    private void ShowTab(string tabname)
    {
        int familyadditionalyn = 0;
        divApplication.Visible = false;
        divBasic.Visible = false;
        divPersonal.Visible = false;
        divSSLC.Visible = false;
        divHSC.Visible = false;
        divGraduation.Visible = false;
        divFamily.Visible = false;
        divExtra.Visible = false;
        divOther.Visible = false;
        divVenue.Visible = false;
        divTerms.Visible = false;
        divDeclaration.Visible = false;
        divPhoto.Visible = false;
        divPayment.Visible = false;

        switch (tabname)
        {
            case "divPersonal":
                SetPreSeaNewApplicantAddress();
                FillPreSeaApplicantInformation();
                break;
            case "divFamily":
                tabname = "divFamily";
                BindFamily();
                TextBox txtName = (TextBox)gvFamily.FooterRow.FindControl("txtFamilyNameAdd");
                txtName.Focus();
                break;
            case "divOther":
                ListPreSeaOthers();
                rblExamVenueFirst.Focus();
                break;
            case "divPayment":
                FillPaymentDetails();
                rdoPayMode.Focus();
                break;
            case "divDeclaration":
                chkDeclaration.Focus();
                break;
            default:
                break;
        }

        if (ViewState["INTERVIEWIDYN"] != null)
        {

            if (ViewState["INTERVIEWIDYN"].ToString() == "0")
            {
                lnkPersonal.Visible = false;
                lnkExtra.Visible = false;
                divAdditionalInfo.Visible = false;
                HideExtraDetails();
                lnkFamily1.Visible = false;
            }
            if (ViewState["INTERVIEWIDYN"].ToString() == "1")
            {
                lnkFamily.Visible = false;
            }
            HtmlGenericControl div = (HtmlGenericControl)Page.FindControl(tabname.ToString());
            if (div != null)
                div.Visible = true;
            string lnkname = tabname.Replace("div", "lnk");

            lnkBasic.Style.Remove("background-image");
            lnkPersonal.Style.Remove("background-image");
            lnkSSLC.Style.Remove("background-image");
            lnkHSC.Style.Remove("background-image");
            lnkGraduation.Style.Remove("background-image");
            lnkFamily.Style.Remove("background-image");
            lnkFamily1.Style.Remove("background-image");
            lnkExtra.Style.Remove("background-image");
            lnkOther.Style.Remove("background-image");
            lnkVenue.Style.Remove("background-image");
            lnkTerms.Style.Remove("background-image");
            lnkDeclaration.Style.Remove("background-image");
            lnkPhoto.Style.Remove("background-image");
            lnkPayment.Style.Remove("background-image");

            lnkBasic.Attributes.Add("class", "notselected");
            lnkPersonal.Attributes.Add("class", "notselected");
            lnkSSLC.Attributes.Add("class", "notselected");
            lnkHSC.Attributes.Add("class", "notselected");
            lnkGraduation.Attributes.Add("class", "notselected");
            lnkFamily.Attributes.Add("class", "notselected");
            lnkFamily1.Attributes.Add("class", "notselected");
            lnkExtra.Attributes.Add("class", "notselected");
            lnkOther.Attributes.Add("class", "notselected");
            lnkVenue.Attributes.Add("class", "notselected");
            lnkTerms.Attributes.Add("class", "notselected");
            lnkDeclaration.Attributes.Add("class", "notselected");
            lnkPhoto.Attributes.Add("class", "notselected");
            lnkPayment.Attributes.Add("class", "notselected");

            HtmlAnchor anc;
            if (familyadditionalyn == 1)
                anc = (HtmlAnchor)Page.FindControl("lnkFamily1");
            else
                anc = (HtmlAnchor)Page.FindControl(lnkname);
            if (anc != null)
            {
                anc.Style.Add("background-image", "none");
                anc.Attributes.Add("class", "navPanellabel");
            }
        }

        else
        {
            divAdditionalInfo.Visible = false;
            lnkPersonal.Visible = false;
            lnkExtra.Visible = false;
            lnkFamily1.Visible = false;
        }
    }
    #endregion

    #region :   Primary Details Events :

    protected void Course_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlCourse.SelectedValue).HasValue)
        {
            ShowTab("divBasic");
            BindBatch(ddlCourse.SelectedValue);
        }
    }

    protected void ddlIllnessYN_Changed(object sender, EventArgs e)
    {
        ShowTab("divBasic");
        if (ddlIllnessYN.SelectedItem.Value == "1")
            txtIllnessDesc.CssClass = "input_mandatory";
        else
            txtIllnessDesc.CssClass = "input";
    }
    protected void ddlColourBlindness_Changed(object sender, EventArgs e)
    {
        ShowTab("divBasic");

    }
    #endregion

    #region :   Other Panel Events  :

    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.Country = "97";
        ucState.StateList = PhoenixRegistersState.ListState(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(ucCountry.SelectedCountry));
        if (IsPostBack)
            ((DropDownList)ucCountry.FindControl("ddlCountry")).Focus();
    }

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlPlace.State = ucState.SelectedState;
        ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger("97")
            , General.GetNullableInteger(ucState.SelectedState) == null ? 0 : General.GetNullableInteger(ucState.SelectedState));
        if (IsPostBack)
            ((DropDownList)ucState.FindControl("ddlState")).Focus();
    }

    #endregion

    #region :   Extra curicular Events  :

    protected void gvAwardAndCertificate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdXDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                ImageButton cme = (ImageButton)e.Row.FindControl("cmdXEdit");
                if (cme != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            }
            DropDownList ddlcertificate = (DropDownList)e.Row.FindControl("ddlCertificateEdit");
            DataRowView drvCertificate = (DataRowView)e.Row.DataItem;
            if (ddlcertificate != null) ddlcertificate.SelectedValue = drvCertificate["FLDCERTIFICATE"].ToString();
        }

    }
    protected void gvAwardAndCertificate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {

                _gridView.EditIndex = -1;
                string certificate = ((DropDownList)_gridView.FooterRow.FindControl("ddlCertificateAdd")).SelectedValue;
                string dateofissue = ((UserControlDate)_gridView.FooterRow.FindControl("txtIssueDateAdd")).Text;
                string remarks = ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text;

                if (!IsValidCertificate(certificate, dateofissue, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaAwardAndCertificate.InsertPreSeaAwardAndCertificate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(certificate)
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableString(remarks)
                    , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                    );

                BindAwardCertificate();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvAwardAndCertificate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string awardid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAwardIdEdit")).Text;
            string certificate = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCertificateEdit")).SelectedValue;
            string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
            string remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text;

            if (!IsValidCertificate(certificate, dateofissue, remarks))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPreSeaAwardAndCertificate.UpdatePreSeaAwardAndCertificate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(awardid)
                    , Convert.ToInt32(certificate)
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableString(remarks)
                    , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                    );

            _gridView.EditIndex = -1;
            BindAwardCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvAwardAndCertificate_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;

            BindAwardCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAwardAndCertificate_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string awardid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAwardId")).Text;

            PhoenixPreSeaAwardAndCertificate.DeletePreSeaAwardAndCertificate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    Convert.ToInt32(awardid));
            BindAwardCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvAwardAndCertificate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindAwardCertificate();
    }

    #endregion

    #region :   Family Panel Events  :

    protected void gvFamily_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdXDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    }

                    ImageButton cme = (ImageButton)e.Row.FindControl("cmdXEdit");
                    if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                }

                UserControlQuick ucRelation = (UserControlQuick)e.Row.FindControl("ucRelationEdit");
                if (ucRelation != null)
                {
                    ucRelation.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION).ToString();
                    ucRelation.SelectedQuick = drv["FLDRELATIONSHIP"].ToString();
                }

                UserControlHard ucGenderEdit = (UserControlHard)e.Row.FindControl("ucGenderEdit");
                if (ucGenderEdit != null)
                {
                    ucGenderEdit.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                    ucGenderEdit.SelectedHard = drv["FLDSEX"].ToString();
                }

                DropDownList ddlOccupation = (DropDownList)e.Row.FindControl("ddlOccupationEdit");
                if (ddlOccupation != null)
                    ddlOccupation.SelectedValue = drv["FLDOCCUPATION"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                UserControlQuick ucRelation = (UserControlQuick)e.Row.FindControl("ucRelationAdd");
                if (ucRelation != null)
                    ucRelation.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION).ToString();

                UserControlHard ucGenderAdd = (UserControlHard)e.Row.FindControl("ucGenderAdd");
                if (ucGenderAdd != null)
                    ucGenderAdd.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFamily_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string familyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFamilyId")).Text;
                ViewState["FAMILYID"] = familyid;
                FillFamilyCommunicationDetails(General.GetNullableInteger(familyid));
            }
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {

                _gridView.EditIndex = -1;
                string Name = ((TextBox)_gridView.FooterRow.FindControl("txtFamilyNameAdd")).Text;
                string relation = ((UserControlQuick)_gridView.FooterRow.FindControl("ucRelationAdd")).SelectedQuick;
                string gender = ((UserControlHard)_gridView.FooterRow.FindControl("ucGenderAdd")).SelectedHard;
                string contact = ((UserControlPhoneNumber)_gridView.FooterRow.FindControl("txtFamilyContactNoAdd")).Text;
                string occupation = ((DropDownList)_gridView.FooterRow.FindControl("ddlOccupationAdd")).SelectedValue;

                if (!IsValidFamilyDetails(Name, relation, gender))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaNewApplicantFamilyNok.InsertPreSeaNewApplicantFamily(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                    , Name
                    , General.GetNullableString("")
                    , General.GetNullableString("")
                    , Convert.ToInt32(gender)
                    , Convert.ToInt32(relation)
                    , null
                    , ""
                    , ""
                    , ""
                    , ""
                    , ""
                    , null
                    , 97
                    , ""
                    , ""
                    , ""
                    , General.GetNullableString(contact)
                    , ""
                    , null
                    , null
                    , ""
                    , ""
                    , ""
                    , ""
                    , ""
                    , ""
                    , ""
                    , null
                    , null
                    , null
                    , ""
                    , null
                    , occupation
                    , null);

                gvFamily.SelectedIndex = 0;
                ViewState["FAMILYID"] = "";

                BindFamily();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFamily_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string FamilyId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFamilyIdEdit")).Text;
            string Name = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFamilyNameEdit")).Text;
            string relation = ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucRelationEdit")).SelectedQuick;
            string gender = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucGenderEdit")).SelectedHard;
            string contact = ((UserControlPhoneNumber)_gridView.Rows[nCurrentRow].FindControl("txtFamilyContactNoEdit")).Text;
            string occupation = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlOccupationEdit")).SelectedValue;

            if (!IsValidFamilyDetails(Name, relation, gender))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPreSeaNewApplicantFamilyNok.UpdatePreSeaOnlineApplicantFamily(
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                , Convert.ToInt32(FamilyId)
                , Name
                , Convert.ToInt32(gender)
                , Convert.ToInt32(relation)
                , General.GetNullableString(contact));

            _gridView.EditIndex = -1;
            BindFamily();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFamily_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;

            BindFamily();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFamily_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string familyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFamilyId")).Text;

            PhoenixPreSeaNewApplicantFamilyNok.DeletePreSeaNewApplicantFamily(General.GetNullableInteger(familyid).Value);

            BindFamily();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFamily_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindFamily();
    }

    #endregion

    #region :   Payment Details Events :

    protected void PayMode_Changed(object sender, EventArgs e)
    {

        if (General.GetNullableInteger(rdoPayMode.SelectedValue).HasValue)
        {
            switch (rdoPayMode.SelectedValue)
            {
                case "2":
                    DDInfo.Visible = true;
                    NetPayInfo.Visible = false;
                    OtherPayInfo.Visible = false;
                    txtDDNumber.FindControl("txtNumber").Focus();
                    break;
                case "3":
                    DDInfo.Visible = false;
                    NetPayInfo.Visible = false;
                    OtherPayInfo.Visible = true;
                    txtOtherPayDeatail.Focus();
                    break;
                default:
                    DDInfo.Visible = false;
                    NetPayInfo.Visible = true;
                    OtherPayInfo.Visible = false;
                    txtNETrfrACno.FindControl("txtNumber").Focus();
                    break;
            }
        }
    }

    #endregion

    #region :   Declaration Panel Events :

    protected void chkDeclaration_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            string chk = chkDeclaration.Checked ? "1" : "0";
            if (chkDeclaration.Checked)
            {
                PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantPersonalDeclarationCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                            , General.GetNullableByte(chk));
                divBasic.Visible = false;
                divPersonal.Visible = false;
                divSSLC.Visible = false;
                divHSC.Visible = false;
                divGraduation.Visible = false;
                divFamily.Visible = false;
                divExtra.Visible = false;
                divOther.Visible = false;
                divVenue.Visible = false;
                divTerms.Visible = false;
                divDeclaration.Visible = false;
                divPhoto.Visible = false;
                divPayment.Visible = false;
                divApplication.Visible = true;
                ifMoreApplication.Attributes["src"] = "../Presea/PreSeaOnlineApplicaitonReport.aspx";
            }

        }
        catch (Exception ex)
        {
            chkDeclaration.Checked = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
        FillPreSeaApplicantInformation();
    }

    #endregion


}

