using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewNewApplicantAddress : PhoenixBasePage
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
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                toolbarmain.AddButton("Edit Details", "EMPSAVE", ToolBarDirection.Right);
            else
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            DataTable dt = PhoenixCrewPortalDetailsConfirmation.ListEmployeeAddressBySeafarer(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger("1"));
            if (dt.Rows.Count > 0 && Request.QueryString["portal"] == null)
                toolbarmain.AddButton("Approve Details", "APPROVE", ToolBarDirection.Right);
            CrewNewApplicantAddressMain.AccessRights = this.ViewState;
            CrewNewApplicantAddressMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                HookOnFocus(this.Page as Control);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = null;
                ViewState["EMPLOYEELOCALADDRESSID"] = null;
                if (Filter.CurrentNewApplicantSelection != null)
                {
                    SetEmployeePrimaryDetails();

                }
                else
                {
                    ucError.ErrorMessage = " Please Select a Employee ";
                    ucError.Visible = true;
                    return;
                }

                LocalAddress.Country = "97";
                PermanentAddress.Country = "97";
                SetEmployeeAddress();
                ddlCountry_TextChanged(null, null);
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewNewApplicantAddress),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            }
            UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
            RadComboBox ddl = (RadComboBox)cntry.FindControl("ddlCountry");
            ddl.TextChanged += new EventHandler(ddlCountry_TextChanged);

            txtPhoneNumber.Attributes.Add("onKeypress", "return isNumberKey(event)");
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
    private bool ValidPhoneNumber(UserControlPhoneNumber phone, string value)
    {
        if (Regex.IsMatch(value, @"^-?\d*\~+\d*$"))
            return true;
        return false;
    }
    private bool ValidPhoneNumber(UserControlMobileNumber mobile, string value)
    {
        if (Regex.IsMatch(value, @"^-?\d*\~+\d*$"))
            return true;
        return false;
    }

    protected void CrewNewApplicantAddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidate(PermanentAddress.Address1, PermanentAddress.City, PermanentAddress.State, PermanentAddress.Country,
                        txtEmail.Text, txtMobileNumber.Text, "Permanent"))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidate(LocalAddress.Address1, LocalAddress.City, LocalAddress.State, LocalAddress.Country,
                         txtEmail.Text, txtMobileNumber.Text, "Local"))
                {
                    ucError.Visible = true;
                    return;
                }
                if ((!txtPhoneNumber2.IsValidPhoneNumber() || !txtPhoneNumber.IsValidPhoneNumber() || !txtlocalPhoneNumber.IsValidPhoneNumber() || !txtLocalPhoneNumber2.IsValidPhoneNumber()))
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["EMPLOYEEPERMANENTADDRESSID"] == null)
                {
                    SaveEmployeeAddress();
                    Response.Redirect("CrewNewApplicantOtherExperience.aspx", false);
                }
                else
                {
                    UpdateEmployeeAddress();
                }

            }
            if (CommandName.ToUpper().Equals("EMPSAVE"))
            {
                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeAddressConfirmationEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentNewApplicantSelection) + "&portal=1');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if(CommandName.ToUpper().Equals("APPROVE"))
            {
                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeAddressConfirmationEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentNewApplicantSelection) + "');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
        RadCheckBox chk = sender as RadCheckBox;
        UserControlCommonAddress suc = (chk.ID == "chkLocalCopyAddress" ? LocalAddress : PermanentAddress);
        UserControlCommonAddress tuc = (chk.ID == "chkLocalCopyAddress" ? PermanentAddress : LocalAddress);
        int i = chk.ID == "chkLocalCopyAddress" ? 0 : 1;
        try
        {
            if (chk.Checked == true)
            {

                tuc.Address1 = suc.Address1;
                tuc.Address2 = suc.Address2;
                tuc.Address3 = suc.Address3;
                tuc.Address4 = suc.Address4;
                tuc.Country = suc.Country;
                tuc.State = suc.State;
                if (suc.City != "Dummy")
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
                DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentNewApplicantSelection));
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
                    if (chk.ID == "chkLocalCopyAddress")
                    {
                        txtPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
                    }
                    else
                    {
                        txtlocalPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
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

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SaveEmployeeAddress()
    {
        try
        {
            int Permanentaddressid = PhoenixNewApplicantEmployeeAddress.InsertEmployeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                                , Convert.ToInt32(PhoenixCrewConstants.PERMANENTADDRESS)
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
                                                                                , General.GetNullableInteger(ucAirport.SelectedAirport)
                                                                                , General.GetNullableInteger(ucPerRelation.SelectedQuick)
                                                                                , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport)
                                                                                );



            int localaddressid = PhoenixNewApplicantEmployeeAddress.InsertEmployeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                               , Convert.ToInt32(PhoenixCrewConstants.LOCALADDRESS)
                                                                               , LocalAddress.Address1
                                                                               , General.GetNullableString(LocalAddress.Address2)
                                                                               , General.GetNullableString(LocalAddress.Address3)
                                                                               , General.GetNullableString(LocalAddress.Address4)
                                                                               , LocalAddress.City
                                                                               , General.GetNullableInteger(LocalAddress.State)
                                                                               , Convert.ToInt32(LocalAddress.Country)
                                                                               , LocalAddress.PostalCode
                                                                               , General.GetNullableString(string.Empty)
                                                                               , txtlocalPhoneNumber.Text
                                                                               , txtLocalMobileNumber.Text
                                                                               , txtEmail.Text
                                                                               , txtLocalPhoneNumber2.Text
                                                                               , txtLocalMobileNumber2.Text
                                                                               , txtLocalMobileNumber3.Text
                                                                               , General.GetNullableInteger(ucAirport.SelectedAirport)
                                                                               , General.GetNullableInteger(ucLocRelation.SelectedQuick)
                                                                               , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport)
                                                                               );
            SetEmployeeAddress();
            ucStatus.Text = "Address Information Updated.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeeAddress()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentNewApplicantSelection));
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

                LocalAddress.Address1 = dt.Rows[1]["FLDADDRESS1"].ToString();
                LocalAddress.Address2 = dt.Rows[1]["FLDADDRESS2"].ToString();
                LocalAddress.Address3 = dt.Rows[1]["FLDADDRESS3"].ToString();
                LocalAddress.Address4 = dt.Rows[1]["FLDADDRESS4"].ToString();

                LocalAddress.Country = dt.Rows[1]["FLDCOUNTRY"].ToString();
                LocalAddress.State = dt.Rows[1]["FLDSTATE"].ToString();
                LocalAddress.City = dt.Rows[1]["FLDCITY"].ToString();
                LocalAddress.PostalCode = dt.Rows[1]["FLDPOSTALCODE"].ToString();
                txtlocalPhoneNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtlocalPhoneNumber.Text = dt.Rows[1]["FLDPHONENUMBER"].ToString();
                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();

                txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                //txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                // txtMobileNumber3.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                ucAirport.SelectedAirport = dt.Rows[0]["FLDNEARESTAIRPORT"].ToString();

                //txtLocalMobileNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber.Text = dt.Rows[1]["FLDMOBILENUMBER"].ToString();
                txtLocalPhoneNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalPhoneNumber2.Text = dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                //txtLocalMobileNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber2.Text = dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
                //txtLocalMobileNumber3.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber3.Text = dt.Rows[1]["FLDMOBILENUMBER3"].ToString();
                ucLocRelation.SelectedQuick = dt.Rows[1]["FLDRELATIONNO"].ToString();
                txtLastUpdatedBy.Text = dt.Rows[0]["FLDLASTMODIFIEDBY"].ToString();
                txtLastUpdateDate.Text = string.Format("{0:dd/MMM/yyy}", dt.Rows[0]["FLDMODIFIEDDATE"]);
                ddlPortofEngagement.SelectedSeaport = dt.Rows[0]["FLDPORTOFENGAGEMENT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void UpdateEmployeeAddress()
    {
        try
        {

            int Permanentaddressid = PhoenixNewApplicantEmployeeAddress.UpdateEmployeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                            , Convert.ToInt32(ViewState["EMPLOYEEPERMANENTADDRESSID"].ToString())
                                                            , Convert.ToInt32(PhoenixCrewConstants.PERMANENTADDRESS)
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
                                                            , General.GetNullableInteger(ucAirport.SelectedAirport)
                                                            , General.GetNullableInteger(ucPerRelation.SelectedQuick)
                                                            , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport)
                                                            );


            int localaddressid = PhoenixNewApplicantEmployeeAddress.UpdateEmployeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                            , Convert.ToInt32(ViewState["EMPLOYEELOCALADDRESSID"].ToString())
                                                            , Convert.ToInt32(PhoenixCrewConstants.LOCALADDRESS)
                                                            , LocalAddress.Address1
                                                            , General.GetNullableString(LocalAddress.Address2)
                                                            , General.GetNullableString(LocalAddress.Address3)
                                                            , General.GetNullableString(LocalAddress.Address4)
                                                            , LocalAddress.City
                                                            , General.GetNullableInteger(LocalAddress.State)
                                                            , Convert.ToInt32(LocalAddress.Country)
                                                            , LocalAddress.PostalCode
                                                            , General.GetNullableString(string.Empty)
                                                            , txtlocalPhoneNumber.Text
                                                            , txtLocalMobileNumber.Text
                                                            , txtEmail.Text
                                                           , txtLocalPhoneNumber2.Text
                                                           , txtLocalMobileNumber2.Text
                                                           , txtLocalMobileNumber3.Text
                                                           , General.GetNullableInteger(ucAirport.SelectedAirport)
                                                           , General.GetNullableInteger(ucLocRelation.SelectedQuick)
                                                           , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport));
            SetEmployeeAddress();
            ucStatus.Text = "Address Information Updated.";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidate(string address1, string city, string state, string county
                            , string email, string mobilenumber, string prefix)
    {
        int employeecountry;
        ucError.HeaderMessage = "Please provide the following required information";

        if (address1.Trim() == "")
            ucError.ErrorMessage = prefix + " Address1 is required";

        if (!int.TryParse(city, out employeecountry))
            ucError.ErrorMessage = prefix + " City is required";

        //if (state.Trim() == "")
        //    ucError.ErrorMessage = "State is required";

        if (Int32.TryParse(county, out employeecountry) == false)
            ucError.ErrorMessage = prefix + " Country is required";

        if (email.Trim() == "")
            ucError.ErrorMessage = "E-Mail is required";
        else if (!General.IsvalidEmail(txtEmail.Text) && txtEmail.Text.ToUpper() != "NA")
        {
            ucError.ErrorMessage = "Please enter valid E-Mail";
        }

        if ((string.IsNullOrEmpty(txtMobileNumber.Text) && string.IsNullOrEmpty(txtMobileNumber2.Text) && string.IsNullOrEmpty(txtMobileNumber3.Text))
               && (string.IsNullOrEmpty(txtPhoneNumber.Text) && string.IsNullOrEmpty(txtPhoneNumber2.Text)))
            ucError.ErrorMessage = "Mobile Number (or) Phone Number (Permanent) is required";

        return (!ucError.IsError);
    }

    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
        if (ucAirport.Country != cntry.SelectedCountry)
        {
            ucAirport.Country = cntry.SelectedCountry;
            ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(General.GetNullableInteger(cntry.SelectedCountry), null);
        }
    }
}
