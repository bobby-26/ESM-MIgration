using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class RegistersBankAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuOfficeMain.Title = "Bank Information";
            MenuOfficeMain.AccessRights = this.ViewState;
            MenuOfficeMain.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                //Session["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
                ViewState["subaccountid"] = Request.QueryString["subaccountid"].ToString();

                EditBankInformation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditBankInformation()
    {
        DataSet ds = PhoenixRegistersBankInformation.EditBankInformation(
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
            , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
            , General.GetNullableInteger(ViewState["subaccountid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtBankCode.Text = dr["FLDBANKCODE"].ToString();
            txtBankName.Text = dr["FLDBANKNAME"].ToString();
            txtBranchcode.Text = dr["FLDBRANCHCODE"].ToString();
            txtIBANnumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtSwiftCode.Text = dr["FLDSWIFTCODE"].ToString();

            ucAddress.Address1 = dr["FLDADDRESS1"].ToString();
            ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
            ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
            ucAddress.Address4 = dr["FLDADDRESS4"].ToString();
            ucAddress.City = dr["FLDCITY"].ToString();
            ucAddress.Country = dr["FLDCOUNTRY"].ToString();
            ucAddress.State = dr["FLDSTATE"].ToString();
            ucAddress.PostalCode = dr["FLDPOSTALCODE"].ToString();

            //txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            //txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtBankChargesofACH.Text = dr["FLDBANKCHARGEOFACH"].ToString();
            txtBankChargesofTT.Text = dr["FLDBANKCHARGEOFTT"].ToString();
            txtIntermediaryAccountNumber.Text = dr["FLDIRACCOUNTNO"].ToString();
            txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
            txtIntermediaryBankAdd1.Text = dr["FLDIRBANKADD1"].ToString();
            txtIntermediaryBankAdd2.Text = dr["FLDIRBANKADD2"].ToString();
            txtIntermediarySwiftCode.Text = dr["FLDIRSWIFTCODE"].ToString();
            txtIntermediaryBankName.Text = dr["FLDIRBANKNAME"].ToString();


            ViewState["bankid"] = dr["FLDBANKID"].ToString();
        }
        txtBankName.Focus();
    }

    protected void OfficeMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        String script = String.Format("javascript:CloseWindow('codehelp1');");
                
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidCompanyBank())
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                string AccountCode = "", CurrencyCode = "";
                if (Session["ACCOUNTID"] != null)
                {
                    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        AccountCode = dr["FLDACCOUNTCODE"].ToString();
                        CurrencyCode = dr["FLDCURRENCYCODE"].ToString();
                    }
                }

                if (ViewState["bankid"] == null)
                {
                    try
                    {

                        PhoenixRegistersBankInformation.InsertBankInformation(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , txtBankName.Text
                            , txtBankCode.Text
                            , txtBranchcode.Text, txtSwiftCode.Text, txtIBANnumber.Text
                            , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                            , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                            , General.GetNullableInteger(ViewState["subaccountid"].ToString())
                            , ucAddress.Address1
                            , ucAddress.Address2
                            , ucAddress.Address3
                            , ucAddress.Address4
                            , ucAddress.City
                            , General.GetNullableInteger(ucAddress.State)
                            , General.GetNullableInteger(ucAddress.Country)
                            , ucAddress.PostalCode
                            , txtAccountNumber.Text
                            , CurrencyCode
                            , AccountCode
                            , General.GetNullableDecimal(txtBankChargesofTT.Text)
                            , General.GetNullableDecimal(txtBankChargesofACH.Text)
                            , txtBeneficiaryName.Text
                            , txtIntermediaryBankName.Text
                            , txtIntermediaryBankAdd1.Text
                            , txtIntermediaryBankAdd2.Text
                            , txtIntermediarySwiftCode.Text
                            , txtIntermediaryAccountNumber.Text
                            );
                    }
                    catch (Exception ex)
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                    

                else
                {
                    try
                    {
                        PhoenixRegistersBankInformation.UpdateBankInformation(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , int.Parse(ViewState["bankid"].ToString())
                            , txtBankName.Text
                            , txtBankCode.Text
                            , txtBranchcode.Text, txtSwiftCode.Text, txtIBANnumber.Text
                            , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                            , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                            , General.GetNullableInteger(ViewState["subaccountid"].ToString())
                            , ucAddress.Address1
                            , ucAddress.Address2
                            , ucAddress.Address3
                            , ucAddress.Address4
                            , ucAddress.City
                            , General.GetNullableInteger(ucAddress.State)
                            , General.GetNullableInteger(ucAddress.Country)
                            , ucAddress.PostalCode
                            , txtAccountNumber.Text
                            , CurrencyCode
                            , AccountCode
                            , General.GetNullableDecimal(txtBankChargesofTT.Text)
                            , General.GetNullableDecimal(txtBankChargesofACH.Text)
                            , txtBeneficiaryName.Text
                            , txtIntermediaryBankName.Text
                            , txtIntermediaryBankAdd1.Text
                            , txtIntermediaryBankAdd2.Text
                            , txtIntermediarySwiftCode.Text
                            , txtIntermediaryAccountNumber.Text
                            )                            
                            ;
                    }
                    catch (Exception ex)
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                        return;
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidCompanyBank()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtBankName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Bank Name is required";

        if (txtSwiftCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "SWIFT Code is required";

        if (ucAddress.Address1.Trim().Equals(""))
            ucError.ErrorMessage = "Address1 is required.";

        if (General.GetNullableInteger(ucAddress.Country) == null)
            ucError.ErrorMessage = "Country is required.";

        if (General.GetNullableInteger(ucAddress.City)==null )
            ucError.ErrorMessage = "City is required.";

        //if (txtCurrencyCode.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Voucher Prefix Currency Code is required";

        //if (txtAccountCode.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = " Voucher Prefix Account Code is required";

        //if (txtBranchcode.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Branch Code is required";

        //if (txtIBANnumber.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "IBAN number is required";

        //if (txtSwiftCode.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "SWIFT Code is required";

        return (!ucError.IsError);
    }
}
