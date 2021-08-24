using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class RegistersCompany : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["CompanyId"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["CompanyId"] = Request.QueryString["CompanyId"].ToString();
                CompanyEdit(Int32.Parse(Request.QueryString["CompanyId"].ToString()));
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuCompanyList.AccessRights = this.ViewState;
            MenuCompanyList.MenuList = toolbar.Show();
            BindAccountCode();
            BindSupplierCode();
        }
    }

    public void BindAccountCode()
    {
        try
        {
            DataSet ds = PhoenixRegistersCompany.SourceAccountCodelist("", "", 74);
            ddlInterCompanyAccount.DataSource = ds;
            ddlInterCompanyAccount.DataTextField = "FLDDESCRIPTION";
            ddlInterCompanyAccount.DataValueField = "FLDACCOUNTID";
            ddlInterCompanyAccount.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindSupplierCode()
    {
        try
        {
            DataSet ds = PhoenixRegistersCompany.SupplierCodelist(null, "");

            ddlInterCompanySupplier.DataSource = ds;
            ddlInterCompanySupplier.DataTextField = "FLDDESCRIPTION";
            ddlInterCompanySupplier.DataValueField = "FLDADDRESSCODE";
            ddlInterCompanySupplier.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CompanyList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidCompany())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["CompanyId"] != null)
                {
                    PhoenixRegistersCompany.UpdateCompany(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Int32.Parse(ViewState["CompanyId"].ToString()),
                        txtCompanyPrefix.Text.Trim(),
                        txtCompanyName.Text.Trim(),
                        txtRegNo.Text.Trim(),
                        ucAddress.Address1,
                        txtPlaceOfIncorp.Text.Trim(),
                        Int16.Parse(ucBaseCurrency.SelectedCurrency.ToString()),
                        Int16.Parse(ucReportingCurrency.SelectedCurrency.ToString()),
                        null,
                        null,
                        null,
                        null,
                        txtRemarks.Text,
                        chkActive.Checked == true ? 1 : 0,
                        txtShortCode.Text,
                        ucAddress.Address2,
                        ucAddress.Address3,
                        ucAddress.Address4,
                        General.GetNullableInteger(ucAddress.City),
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.Country),
                        ucAddress.PostalCode,
                        txtTelephoneNo.Text,
                        txtFaxNo.Text,
                        txtEmail.Text,
                        General.GetNullableInteger(ucCompanyPIC.SelectedUser),
                        null,
                        chkUseSupplierConfigYN.Checked == true ? 1 : 0,
                        int.Parse(ddlInterCompanyAccount.SelectedValue),
                        int.Parse(ddlInterCompanySupplier.SelectedValue),
                        chkAllowVesselEntries.Checked == true ? 1 : 0,
                        txtcompanugstno.Text,
                        General.GetNullableInteger(ucdefaultcurrecy.SelectedCurrency));
                }
                else
                {
                    PhoenixRegistersCompany.InsertCompany(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        txtCompanyPrefix.Text.Trim(),
                        txtCompanyName.Text.Trim(),
                        txtRegNo.Text.Trim(),
                        ucAddress.Address1,
                        txtPlaceOfIncorp.Text.Trim(),
                        Int16.Parse(ucBaseCurrency.SelectedCurrency.ToString()),
                        Int16.Parse(ucReportingCurrency.SelectedCurrency.ToString()),
                        null,
                        null,
                        null,
                        null,
                        txtRemarks.Text,
                        chkActive.Checked == true ? 1 : 0,
                        txtShortCode.Text,
                        ucAddress.Address2,
                        ucAddress.Address3,
                        ucAddress.Address4,
                         General.GetNullableInteger(ucAddress.City),
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.Country),
                        ucAddress.PostalCode,
                        txtTelephoneNo.Text,
                        txtFaxNo.Text,
                        txtEmail.Text,
                        General.GetNullableInteger(ucCompanyPIC.SelectedUser),
                        null,
                        chkUseSupplierConfigYN.Checked == true ? 1 : 0,
                        int.Parse(ddlInterCompanyAccount.SelectedValue),
                        int.Parse(ddlInterCompanySupplier.SelectedValue),
                        chkAllowVesselEntries.Checked == true ? 1 : 0,
                        txtcompanugstno.Text,
                        General.GetNullableInteger(ucdefaultcurrecy.SelectedCurrency));
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private bool IsValidCompany()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtCompanyPrefix.Text.Equals(""))
            ucError.ErrorMessage = "Company Prefix is required.";

        if (txtCompanyName.Text.Equals(""))
            ucError.ErrorMessage = "Company Name is required.";

        if (txtRegNo.Text.Equals(""))
            ucError.ErrorMessage = "Company Reg. No. is required.";

        if (txtShortCode.Text.Equals(""))
            ucError.ErrorMessage = "Short Code is required.";

        if (ucAddress.Address1.Equals(""))
            ucError.ErrorMessage = "Address1 is required.";

        if (txtPlaceOfIncorp.Text.Equals(""))
            ucError.ErrorMessage = "Place of Incorporation is required.";

        if (ucBaseCurrency.Equals(""))
            ucError.ErrorMessage = "Base Currency is required.";

        if (ucReportingCurrency.SelectedCurrency.Equals(""))
            ucError.ErrorMessage = "Reporting Currency is required.";

        if (General.GetNullableInteger(ucAddress.Country) == null)
            ucError.ErrorMessage = "Country is required.";

        if (txtEmail.Text.Trim() != "" && !General.IsvalidEmail(txtEmail.Text))
            ucError.ErrorMessage = "Enter valid E-Mail";

        //if (ucAddress.City.Equals(""))
        //    ucError.ErrorMessage = "City is required.";

        if (chkUseSupplierConfigYN.Checked == true)
        {
            if (General.GetNullableInteger(ucCompanyPIC.SelectedUser) == null)
                ucError.ErrorMessage = "Person In Charge is required when 'Invoice Posting By Supplier Configuration Only' checkbox is checked.";
        }

        if (ddlInterCompanyAccount.SelectedValue == "")
            ucError.ErrorMessage = "Inter company account code is required.";

        if (ddlInterCompanySupplier.SelectedValue == "")
            ucError.ErrorMessage = "Inter company supplier code is required.";

        if (ucAddress.Address1.Length > 40)
        {
            ucError.ErrorMessage = "Address1 can have a maximum of 40 characters only.";
        }
        if (ucAddress.Address2.Length > 40)
        {
            ucError.ErrorMessage = "Address2 can have a maximum of 40 characters only.";
        }
        if (ucAddress.Address3.Length > 40)
        {
            ucError.ErrorMessage = "Address3 can have a maximum of 40 characters only.";
        }
        if (ucAddress.Address4.Length > 40)
        {
            ucError.ErrorMessage = "Address4 can have a maximum of 40 characters only.";
        }
        return (!ucError.IsError);
    }

    private void CompanyEdit(int companyid)
    {
        try
        {
            DataSet ds = PhoenixRegistersCompany.EditCompany(companyid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtCompanyPrefix.Text = dr["FLDCOMPANYPREFIX"].ToString();
                txtCompanyName.Text = dr["FLDCOMPANYNAME"].ToString();
                txtRegNo.Text = dr["FLDCOMPANYREGNO"].ToString();
                ucAddress.Address1 = dr["FLDCOMPANYADDRESS"].ToString();
                txtPlaceOfIncorp.Text = dr["FLDPLACEOFINCORPORATION"].ToString();
                ucBaseCurrency.SelectedCurrency = dr["FLDBASECURRENCY"].ToString();
                ucReportingCurrency.SelectedCurrency = dr["FLDREPORTINGCURRENCY"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtcompanugstno.Text = dr["FLDCOMPANYGSTNO"].ToString();
                chkActive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;
                txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
                ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
                ucAddress.Address4 = dr["FLDADDRESS4"].ToString();
                ucAddress.Country = dr["FLDCOUNTRY"].ToString();
                ucAddress.State = dr["FLDSTATE"].ToString();
                ucAddress.City = dr["FLDCITY"].ToString();
                ucAddress.PostalCode = dr["FLDPOSTALCODE"].ToString();
                txtEmail.Text = dr["FLDEMAILID"].ToString();
                txtTelephoneNo.Text = dr["FLDTELEPHONENO"].ToString();
                txtFaxNo.Text = dr["FLDFAXNO"].ToString();
                ucCompanyPIC.SelectedUser = dr["FLDCOMPANYPIC"].ToString();
                ucdefaultcurrecy.SelectedCurrency = dr["FLDDEFAULTCURRENCY"].ToString();
                if (dr["FLDUSESUPPLIERCONFIGYN"] != null && dr["FLDUSESUPPLIERCONFIGYN"].ToString() != "" && dr["FLDUSESUPPLIERCONFIGYN"].ToString() == "1")
                    chkUseSupplierConfigYN.Checked = true;
                else
                    chkUseSupplierConfigYN.Checked = false;
                if (dr["FLDINTERCOMPANYACCOUNT"] != null && dr["FLDINTERCOMPANYACCOUNT"].ToString() != "")
                    ddlInterCompanyAccount.SelectedValue = dr["FLDINTERCOMPANYACCOUNT"].ToString();
                if (dr["FLDINTERCOMPANYSUPPLIER"] != null && dr["FLDINTERCOMPANYSUPPLIER"].ToString() != "")
                    ddlInterCompanySupplier.SelectedValue = dr["FLDINTERCOMPANYSUPPLIER"].ToString();
                chkAllowVesselEntries.Checked = dr["FLDALLOWVESSELENTRIES"].ToString() == "1" ? true : false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
