using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersTaxAndChargesAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
                MenuTaxAndCharges.AccessRights = this.ViewState;
                MenuTaxAndCharges.Title = "Tax and Charges";
                MenuTaxAndCharges.MenuList = toolbarmain.Show();
                
                ViewState["ID"] = Request.QueryString["id"];
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1);
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                    Edit(new Guid(ViewState["ID"].ToString()));
                toolbarmain = new PhoenixToolbar();
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuTaxAndCharges1.AccessRights = this.ViewState;
                MenuTaxAndCharges1.MenuList = toolbarmain.Show();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Edit(Guid Dtkey)
    {
        DataSet ds = PhoenixRegistersTaxAndCharges.EditTaxAndCharges(Dtkey);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlPaymentmode.SelectedHard = ds.Tables[0].Rows[0]["FLDPAYMENTMODE"].ToString();

            if (ds.Tables[0].Rows[0]["FLDISBANKACCOUNTACTIVE"].ToString() == "0")
            {
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1);
            }
            else
            {
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 0);
            }
            ddlBankAccount.SelectedBankAccount = ds.Tables[0].Rows[0]["FLDBANKLIST"].ToString();
            txtRangeFrom.Text = ds.Tables[0].Rows[0]["FLDRANGEFROM"].ToString();
            txtRangeTo.Text = ds.Tables[0].Rows[0]["FLDRANGETO"].ToString();
            txtTaxPercent.Text = ds.Tables[0].Rows[0]["FLDTAXPERCENTAGE"].ToString();
            if (ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1")
                chkActiveYN.Checked = true;
            txtMaxGrossAmount.Text = ds.Tables[0].Rows[0]["FLDMAXGROSSAMOUNT"].ToString();
        }
    }
    protected void MenuTaxAndCharges_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegistersTaxAndCharges.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        //{
        //    DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DataRow dr = ds.Tables[0].Rows[0];
        //        txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
        //        //txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
        //        txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
        //        txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
        //    }
        //}
    }
    protected void MenuTaxAndCharges1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTaxAndCharges())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                {
                    PhoenixRegistersTaxAndCharges.UpdateTaxAndCharges(
                      General.GetNullableGuid(ViewState["ID"].ToString())
                       , int.Parse(ddlBankAccount.SelectedBankAccount)
                       , decimal.Parse(txtRangeFrom.Text.Trim())
                       , decimal.Parse(txtRangeTo.Text.Trim())
                       , decimal.Parse(txtTaxPercent.Text.Trim())
                       , decimal.Parse(txtMaxGrossAmount.Text.Trim())
                       , chkActiveYN.Checked ? 1 : 0);

                }
                else
                {
                    PhoenixRegistersTaxAndCharges.InsertTaxAndCharges(int.Parse(ddlPaymentmode.SelectedHard), int.Parse(ddlBankAccount.SelectedBankAccount)
                       , decimal.Parse(txtRangeFrom.Text.Trim())
                       , decimal.Parse(txtRangeTo.Text.Trim())
                       , decimal.Parse(txtTaxPercent.Text.Trim())
                       , decimal.Parse(txtMaxGrossAmount.Text.Trim())
                       , chkActiveYN.Checked ? 1 : 0
                     );

                }
                Response.Redirect("../Registers/RegistersTaxAndCharges.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTaxAndCharges()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlPaymentmode.SelectedHard == "Dummy")
        {
            ucError.ErrorMessage = "Payment mode is  required.";
        }
        if (ddlBankAccount.SelectedBankAccount == "Dummy")
        {
            ucError.ErrorMessage = "Bank Account is required.";
        }
        if (txtRangeFrom.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Range From is required.";
        }
        if (txtRangeTo.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Range To is required.";
        }
        if (txtTaxPercent.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Tax % is  required.";
        }
        //else if (int.Parse(txtTaxPercent.Text.Trim()) < 0 || int.Parse(txtTaxPercent.Text.Trim()) >100)
        //{
        //    ucError.ErrorMessage = "Tax %  between 0 to 100.";
        //}
        if (txtMaxGrossAmount.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Max Gross Amount is  required.";
        }
        //else if (int.Parse(txtMaxGrossAmount.Text.Trim()) <= 0)
        //{
        //    ucError.ErrorMessage = "Max Gross Amount Digits is greater than or equal to Zero.";
        //}
        return (!ucError.IsError);
    }

}
