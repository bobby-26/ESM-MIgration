using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsSundryPurchaseGeneral : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {                
                txtSupplierId.Attributes.Add("style", "display:none");
                ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                rblPaymentTerm.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125);
                rblPaymentTerm.DataBind();
                ViewState["ACTIVE"] = "1";
                if (ViewState["ORDERID"] != null)
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
            }
            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewBond_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
           
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOrder(ucVessel.SelectedVessel, ddlStock.SelectedHard, txtSupplierId.Text, txtOrderDate.Text, txtRoundOff.Text, txtDiscount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] == null)
                {
                    PhoenixAccountsSundryPurchase.InsertSundryPurchase (int.Parse(ucVessel.SelectedVessel), int.Parse(ddlStock.SelectedHard)
                        , DateTime.Parse(txtOrderDate.Text), new Guid(txtSupplierId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtExchangeRate.Text), General.GetNullableDecimal(txtTotalAmount.Text), General.GetNullableInteger(rblPaymentTerm.SelectedValue)
                        , General.GetNullableDecimal(txtDiscount.Text));
                }
                else
                {
                    PhoenixAccountsSundryPurchase.UpdateSundryPurchase(new Guid(ViewState["ORDERID"].ToString()), int.Parse(ddlStock.SelectedHard)
                        , DateTime.Parse(txtOrderDate.Text), new Guid(txtSupplierId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtExchangeRate.Text), General.GetNullableDecimal(txtTotalAmount.Text), General.GetNullableInteger(rblPaymentTerm.SelectedValue)
                        , General.GetNullableDecimal(txtDiscount.Text));
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ucVessel.Enabled = true;
                ViewState["ORDERID"] = null;
                ViewState["ACTIVE"] = "1";
                MainMenu();                
                ddlStock.SelectedHard = string.Empty;
                ddlStock.Enabled = true;
                txtOrderDate.Text = string.Empty;
                txtSupplierId.Text = string.Empty;
                ddlCurrency.SelectedCurrency = string.Empty;
                txtExchangeRate.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                txtSupplierId.Text = string.Empty;
                txtSupplierName.Text = string.Empty;
                txtSupplierCode.Text = string.Empty;
                txtDiscount.Text = string.Empty;
                txtRoundOff.Text = string.Empty;
                txtRefNo.Text = string.Empty;
                foreach (ListItem item in rblPaymentTerm.Items)
                    item.Selected = false;
            }
            else if (CommandName.ToUpper().Equals("SAVEREC"))
            {
                if (!IsValidReceivedDetails(txtReceivedDate.Text, ddlSeaPort.SelectedSeaport))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsSundryPurchase.ConfirmSundryPurchase (new Guid(ViewState["ORDERID"].ToString()), DateTime.Parse(txtReceivedDate.Text), int.Parse(ddlSeaPort.SelectedSeaport));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditBondProvision(Guid gOrderId)
    {
        ucVessel.Enabled = false;
        DataTable dt = PhoenixAccountsSundryPurchase.EditSundryPurchase (gOrderId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucVessel.SelectedVessel= dr["FLDVESSELID"].ToString();
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();
            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
            ddlStock.SelectedHard = dr["FLDSTOCKTYPE"].ToString();
            ddlStock.Enabled = false;
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtTotalAmount.Text = dr["FLDAMOUNTPAID"].ToString();
            txtDiscount.Text = dr["FLDDISCOUNT"].ToString();
            txtRoundOff.Text = dr["FLDROUNDOFFAMOUNT"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            ListItem item = rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString());
            if (item != null)
                rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString()).Selected = true;
            MainMenu();

        }
    }
    private bool IsValidOrder(string vessel,string stocktype, string supplier, string orderdate, string roundoff, string discount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order date should be earlier than current date";
        }
        if (!General.GetNullableInteger(stocktype).HasValue)
        {
            ucError.ErrorMessage = "Stock type is required.";
        }
        if (!General.GetNullableInteger(vessel).HasValue)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (!General.GetNullableGuid(supplier).HasValue)
        {
            ucError.ErrorMessage = "Vendor is required.";
        }
        //if (General.GetNullableDecimal(roundoff).HasValue && General.GetNullableDecimal(roundoff).Value > 10)
        //{
        //    ucError.ErrorMessage = "Round off Amount should be between 0$ and 10$";
        //}
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        return (!ucError.IsError);
    }
    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["r"] == null)
        {
            divMain.Visible = true;
            divSub.Visible = false;
          //  toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
            if (ViewState["ACTIVEYN"] == null || ViewState["ACTIVEYN"].ToString() == "1")
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            }
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        }
        else
        {
            MenuCrewBond.Title = "Update Received Details";
          //  Title1.Text = "Update Received Details";
            divMain.Visible = false;
            divSub.Visible = true;
            toolbar.AddButton("Save", "SAVEREC", ToolBarDirection.Right);
        }
        MenuCrewBond.AccessRights = this.ViewState;
        MenuCrewBond.MenuList = toolbar.Show();
    }
    private bool IsValidReceivedDetails(string date, string Seaport)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Received date is required.";
        }
        if (!General.GetNullableInteger(Seaport).HasValue)
        {
            ucError.ErrorMessage = "Received Sea port is required.";
        }

        return (!ucError.IsError);
    }
}

