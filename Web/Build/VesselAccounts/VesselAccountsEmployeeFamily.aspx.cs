using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeFamily :  PhoenixBasePage
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
            MenuActivityFilterMain.AccessRights = this.ViewState;
            MenuActivityFilterMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                HookOnFocus(this.Page as Control);                
                ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                ucRelation.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION).ToString();
                if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
                {
                    ViewState["EMPLOYEEFAMILYID"] = Request.QueryString["familyid"];
                }

                SetEmployeePrimaryDetails();

                SetEmployeeFamilyDetails();
                ucAddress.Country = "97";

                RadTextBox txtBankAddress1 = (RadTextBox)(ucBankAddress.FindControl("txtAddressLine1"));
                UserControlCountry ddlCountry = (UserControlCountry)(ucBankAddress.FindControl("ddlCountry"));
                UserControlCity ddlcity = (UserControlCity)(ucBankAddress.FindControl("ddlCity"));
                txtBankAddress1.CssClass = "input";
                ddlCountry.CssClass = "input";
                ddlcity.CssClass = "input";
                Page.ClientScript.RegisterStartupScript(
                typeof(VesselAccountsEmployeeFamily),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
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

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToInt32(Filter.CurrentVesselCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
            dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentVesselCrewSelection), null);
            lstFamily.Items.Clear();
            ListItem items = new ListItem();
            lstFamily.DataSource = dt;
            lstFamily.DataTextField = "FLDFIRSTNAME";
            lstFamily.DataValueField = "FLDFAMILYID";
            lstFamily.DataBind();

            //ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();

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

    private void SetEmployeeFamilyDetails()
    {
        try
        {
            string familyid = null;
            if (ViewState["EMPLOYEEFAMILYID"] != null)
            {
                familyid = ViewState["EMPLOYEEFAMILYID"].ToString();
            }

            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid));

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

                lstFamily.SelectedValue = ViewState["EMPLOYEEFAMILYID"].ToString();

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                ViewState["dtkey"] = string.Empty;
                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhoto.ImageUrl = HttpContext.Current.Session["sitepath"] + "/attachments/" + dta.Rows[0]["FLDFILEPATH"].ToString();
                    aCrewFamilyImg.HRef = "#";
                    aCrewFamilyImg.Attributes["onclick"] = "javascript:parent.Openpopup('codehelp1', '', '" + imgPhoto.ImageUrl + "');";
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void lstFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EMPLOYEEFAMILYID"] = lstFamily.SelectedValue.ToString();

        SetEmployeeFamilyDetails();
    }
}
