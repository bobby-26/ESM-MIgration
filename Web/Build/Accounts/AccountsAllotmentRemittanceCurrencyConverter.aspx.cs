using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceCurrencyConverter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Convert", "CONVERT",ToolBarDirection.Right);
        MenuConverter.AccessRights = this.ViewState;
        MenuConverter.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["REMITTANCEID"] = "";

            ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
            ViewState["ACCESSEDFROM"] = Request.QueryString["ACCESSEDFROM"].ToString();
            BindLoad(ViewState["REMITTANCEID"].ToString());
        }

    }
    protected void BindLoad(string remittanceid)
    {
        DataSet ds = PhoenixAccountsAllotmentRemittance.EditCurrencyConverter(remittanceid, 0);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtPVCurrencyCode.Text = dr["FLDPAYMENTVOUCHERCURRENCY"].ToString();
            txtPVCurrencyId.Text = dr["FLDPVCURRRENCYID"].ToString();
            txtPaymentVoucherAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDPAYMENTVOUCHERAMOUNT"]));
            ddlConversionCurrencyCode.SelectedCurrency = dr["FLDREMITTANCECURRENCYID"].ToString();
            lblRemittanceId.Text = dr["FLDREMITTANCEID"].ToString();
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
        }
    }

    protected void MenuConverter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CONVERT"))
            {
                if (!IsValidExchange(txtExchangeRate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAllotmentRemittance.CurrencyConverter(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Request.QueryString["REMITTANCEID"].ToString(), int.Parse(ddlConversionCurrencyCode.SelectedCurrency), decimal.Parse(txtExchangeRate.Text));

                ucStatus.Text = "Currency converted";

                String scriptinsert = String.Format("javascript:parent.fnReloadList('codehelp1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidExchange(string strExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strExchangeRate.Trim().Equals(string.Empty) || strExchangeRate.Trim().Equals("0"))
        {
            ucError.ErrorMessage = "Please Enter valid Exchange Rate.";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
