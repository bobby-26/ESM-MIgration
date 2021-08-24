using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceCurrencyConverterBatch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");

        if (!IsPostBack)
        {
        }
    }

    protected void BindLoad()
    {
        DataSet ds = PhoenixAccountsAllotmentRemittance.EditCurrencyConverterBatch();
        gvCurrencyConverter.DataSource = ds;
    }
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvCurrencyConverter.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCurrencyConverter_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "CONVERT")
            {
                string pvcurrency = ((RadLabel)e.Item.FindControl("lblPVCurrencyId")).Text;
                string exchangerate = ((TextBox)e.Item.FindControl("txtExchangeRate")).Text;
                UserControlCurrency rmcurrency = ((UserControlCurrency)e.Item.FindControl("ddlConversionCurrencyCode"));
                string pvamount = ((RadLabel)e.Item.FindControl("lblPVAmount")).Text;

                if (!IsValidExchange(exchangerate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAllotmentRemittance.CurrencyConverterBatch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(pvcurrency), General.GetNullableInteger(rmcurrency.SelectedCurrency), decimal.Parse(pvamount), decimal.Parse(exchangerate));

                ucStatus.Text = "Currency converted";

                gvCurrencyConverter.Rebind();

                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
                Script += "fnReloadList();";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
    protected void gvCurrencyConverter_RowDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdSave");
            ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to convert ?')");
            UserControlCurrency rmcurrency = ((UserControlCurrency)e.Item.FindControl("ddlConversionCurrencyCode"));
            if (rmcurrency != null) rmcurrency.SelectedCurrency = drv["FLDRMCURRENCY"].ToString();
        }
    }

    protected void gvCurrencyConverter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLoad();
    }
}
