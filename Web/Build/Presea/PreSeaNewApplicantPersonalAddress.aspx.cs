using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PreSeaNewApplicantPersonalAddress : PhoenixBasePage
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
            PreSeaPersonalAddressMain.AccessRights = this.ViewState;
            PreSeaPersonalAddressMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                HookOnFocus(this.Page as Control);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = null;
                ViewState["EMPLOYEELOCALADDRESSID"] = null;
                if (Filter.CurrentPreSeaNewApplicantSelection != null)
                {
                    SetPreSeaNewApplicantPrimaryDetails();
                }
                else
                {
                    ucError.ErrorMessage = " Please Select a Employee ";
                    ucError.Visible = true;
                    return;
                }
               
                PostalAddress.Country = "97";
                PermanentAddress.Country = "97";
                SetPreSeaNewApplicantAddress();
                ddlCountry_TextChanged(null, null);

                Page.ClientScript.RegisterStartupScript(
                typeof(PreSeaNewApplicantPersonalAddress),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            }
            UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
            RadComboBox ddl = (RadComboBox)cntry.FindControl("ddlCountry");
            ddl.TextChanged += new EventHandler(ddlCountry_TextChanged);
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
            (CurrentControl is DropDownList)) //|| (CurrentControl is RadComboBox))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void PreSeaPersonalAddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["EMPLOYEEPERMANENTADDRESSID"] == null)
                {
                    SavePreSeaNewApplicantAddress();
                }
                else
                {
                    UpdatePreSeaNewApplicantAddress();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                //ucPostalYears.Text = ucPermanentYears.Text;
                //ucPostalMonths.Text = ucPermanentMonths.Text;
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
                //ucPostalYears.Text = "";
                //ucPostalMonths.Text = "";
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
                    if (chk.ID == "chkPostalCopyAddress")
                    {
                        //txtStdCode.Text = dt.Rows[i]["FLDSTDCODE"].ToString();
                        txtPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
                    }
                    else
                    {
                        //txtlocalStdCode.Text = dt.Rows[i]["FLDSTDCODE"].ToString();
                        txtPostalPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
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
    public void SetPreSeaNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SavePreSeaNewApplicantAddress()
    {
        try
        {
            
            if (!IsValidate(PostalAddress.Address1, PostalAddress.City, PostalAddress.State, PostalAddress.Country,
                     txtEmail.Text, txtMobileNumber.Text, "Postal",ucPostalYears.Text,ucPermanentYears.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (!IsValidate(PermanentAddress.Address1, PermanentAddress.City, PermanentAddress.State, PermanentAddress.Country,
                        txtEmail.Text, txtMobileNumber.Text, "Permanent",ucPermanentYears.Text,ucPermanentMonths.Text))
            {
                ucError.Visible = true;
                return;
            }
            if ((!txtPhoneNumber2.IsValidPhoneNumber() || !txtPhoneNumber.IsValidPhoneNumber() || !txtPostalPhoneNumber.IsValidPhoneNumber() || !txtPostalPhoneNumber2.IsValidPhoneNumber()))
            {
                ucError.ErrorMessage = "Enter area code for phone number";
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
                                                                                                      , txtPostalPhoneNumber.Text
                                                                                                      , txtPostalMobileNumber.Text
                                                                                                      , txtEmail.Text
                                                                                                      , txtPostalPhoneNumber2.Text
                                                                                                      , txtPostalMobileNumber2.Text
                                                                                                      , txtPostalMobileNumber3.Text
                                                                                                      ,null
                                                                                                      , General.GetNullableInteger(ucLocRelation.SelectedQuick)
                                                                                                      , General.GetNullableInteger(ucPostalYears.Text)
                                                                                                      , General.GetNullableInteger(ucPostalMonths.Text)
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
                                                                                                        , txtPhoneNumber.Text
                                                                                                        , txtMobileNumber.Text
                                                                                                        , txtEmail.Text
                                                                                                        , txtPhoneNumber2.Text
                                                                                                        , txtMobileNumber2.Text
                                                                                                        , txtMobileNumber3.Text
                                                                                                        ,null
                                                                                                        , General.GetNullableInteger(ucPerRelation.SelectedQuick)
                                                                                                        , General.GetNullableInteger(ucPermanentYears.Text)
                                                                                                        , General.GetNullableInteger(ucPermanentMonths.Text)
                                                                                                        );




            SetPreSeaNewApplicantAddress();
            ucStatus.Text = "Address Information Updated."; 
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
                //txtStdCode.Text = dt.Rows[0]["FLDSTDCODE"].ToString();
                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                //txtMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = dt.Rows[0]["FLDEMPLOYEEADDRESSID"].ToString();
                ucPerRelation.SelectedQuick = dt.Rows[0]["FLDRELATIONNO"].ToString();

                PostalAddress.Address1 = dt.Rows[1]["FLDADDRESS1"].ToString();
                PostalAddress.Address2 = dt.Rows[1]["FLDADDRESS2"].ToString();
                PostalAddress.Address3 = dt.Rows[1]["FLDADDRESS3"].ToString();
                PostalAddress.Address4 = dt.Rows[1]["FLDADDRESS4"].ToString();

                PostalAddress.Country = dt.Rows[1]["FLDCOUNTRY"].ToString();
                PostalAddress.State = dt.Rows[1]["FLDSTATE"].ToString();
                PostalAddress.City = dt.Rows[1]["FLDCITY"].ToString();
                PostalAddress.PostalCode = dt.Rows[1]["FLDPOSTALCODE"].ToString();
                txtPostalPhoneNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtPostalPhoneNumber.Text = dt.Rows[1]["FLDPHONENUMBER"].ToString();
                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();

                txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                //txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                //txtMobileNumber3.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                //ucAirport.SelectedAirport = dt.Rows[0]["FLDNEARESTAIRPORT"].ToString();

                //txtPostalMobileNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtPostalMobileNumber.Text = dt.Rows[1]["FLDMOBILENUMBER"].ToString();
                txtPostalPhoneNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtPostalPhoneNumber2.Text = dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                //txtPostalMobileNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtPostalMobileNumber2.Text = dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
                txtPostalMobileNumber3.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtPostalMobileNumber3.Text = dt.Rows[1]["FLDMOBILENUMBER3"].ToString();
                ucLocRelation.SelectedQuick = dt.Rows[1]["FLDRELATIONNO"].ToString();
                txtLastUpdatedBy.Text = dt.Rows[0]["FLDLASTMODIFIEDBY"].ToString();
                txtLastUpdateDate.Text = string.Format("{0:dd/MMM/yyy}", dt.Rows[0]["FLDMODIFIEDDATE"]);

                ucPostalYears.Text = dt.Rows[1]["FLDVALIDYEARS"].ToString();
                ucPostalMonths.Text = dt.Rows[1]["FLDVALIDMONTHS"].ToString();

                ucPermanentYears.Text = dt.Rows[0]["FLDVALIDYEARS"].ToString();
                ucPermanentMonths.Text = dt.Rows[0]["FLDVALIDMONTHS"].ToString();
            }
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
                        txtEmail.Text, txtMobileNumber.Text, "Permanent",ucPermanentYears.Text,ucPermanentMonths.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (!IsValidate(PostalAddress.Address1, PostalAddress.City, PostalAddress.State, PostalAddress.Country,
                     txtEmail.Text, txtMobileNumber.Text, "Postal",ucPostalYears.Text,ucPostalMonths.Text))
            {
                ucError.Visible = true;
                return;
            }
            if ((!txtPhoneNumber2.IsValidPhoneNumber() || !txtPhoneNumber.IsValidPhoneNumber() || !txtPostalPhoneNumber.IsValidPhoneNumber() || !txtPostalPhoneNumber2.IsValidPhoneNumber()))
            {
                ucError.ErrorMessage = "Enter area code for phone number";
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
                                                                                            , txtPostalPhoneNumber.Text
                                                                                            , txtPostalMobileNumber.Text
                                                                                            , txtEmail.Text
                                                                                            , txtPostalPhoneNumber2.Text
                                                                                            , txtPostalMobileNumber2.Text
                                                                                            , txtPostalMobileNumber3.Text
                                                                                            , null
                                                                                            , General.GetNullableInteger(ucLocRelation.SelectedQuick)
                                                                                            , General.GetNullableInteger(ucPostalYears.Text)
                                                                                            , General.GetNullableInteger(ucPostalMonths.Text)
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
                                                                                            , txtPhoneNumber.Text
                                                                                            , txtMobileNumber.Text
                                                                                            , txtEmail.Text
                                                                                            , txtPhoneNumber2.Text
                                                                                            , txtMobileNumber2.Text
                                                                                            , txtMobileNumber3.Text
                                                                                            , null
                                                                                            , General.GetNullableInteger(ucPerRelation.SelectedQuick)
                                                                                            , General.GetNullableInteger(ucPermanentYears.Text)
                                                                                            , General.GetNullableInteger(ucPermanentMonths.Text)
                                                                                            );

            SetPreSeaNewApplicantAddress();
            ucStatus.Text = "Address Information Updated.";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidate(string address1, string city, string state, string county
                           , string email, string mobilenumber, string prefix,string years,string months)
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
        if (years.Trim() == "")
        {
            ucError.ErrorMessage = prefix + " Address valid years reruired";
        }
        if (months.Trim() == "")
        {
            ucError.ErrorMessage = prefix + " Address valid months reruired";
        }
        if ((string.IsNullOrEmpty(txtMobileNumber.Text) && string.IsNullOrEmpty(txtMobileNumber2.Text) && string.IsNullOrEmpty(txtMobileNumber3.Text))
               && (string.IsNullOrEmpty(txtPhoneNumber.Text) && string.IsNullOrEmpty(txtPhoneNumber2.Text)))
            ucError.ErrorMessage = "Mobile Number (or) Phone Number (Permanent) is required";

        return (!ucError.IsError);
    }
    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
    }
}
