using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceDirectPO : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("New", "NEW");
        toolbar.AddButton("Save", "SAVE");
        MenuDirectPO.AccessRights = this.ViewState;
        MenuDirectPO.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            

            MenuDirectPO.Visible = false;
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
            ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
            ViewState["ORDERID"] = Request.QueryString["orderid"];            
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            EditOrder();
        }
    }
    protected void MenuDirectPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string vessel = ddlVessel.SelectedVessel;
                string ponumber = txtPONumber.Text;
                string vendor = txtVendorId.Text;
                string currency = ddlCurrency.SelectedCurrency;
                string podate = txtPoReceivedDate.Text;
                if (!IsValidInvoice(vessel, ponumber, vendor, currency, podate))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] == null)
                {
                    PhoenixAccountsInvoice.InvoiceDirectPOInsert(new Guid(ViewState["INVOICECODE"].ToString()), int.Parse(vessel), ponumber, int.Parse(vendor), int.Parse(currency)
                        , DateTime.Parse(podate), General.GetNullableDecimal(string.Empty), byte.Parse(chkGSTOffset.Checked ? "1" : "0"), General.GetNullableDecimal(txtAdvanceAmount.Text), chkIssues.Checked ? 1 : 0,txtDescription.Text  );
                }
                else
                {
                    PhoenixAccountsInvoice.InvoiceDirectPOUpdate(new Guid(ViewState["ORDERID"].ToString()), int.Parse(vessel), ponumber
                        , int.Parse(vendor), int.Parse(currency)
                        , DateTime.Parse(podate), General.GetNullableDecimal(string.Empty), byte.Parse(chkGSTOffset.Checked ? "1" : "0")
                        , General.GetNullableDecimal(txtAdvanceAmount.Text), chkIssues.Checked ? 1 : 0, txtDescription.Text
                        , General.GetNullableDecimal(txtDiscount.Text)
                        , null
                        ,null);
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["ORDERID"] = null;                
                ddlVessel.SelectedVessel = string.Empty;
                txtPONumber.Text = string.Empty;
                txtPoReceivedDate.Text = string.Empty;
                ddlCurrency.SelectedCurrency = string.Empty;
                txtVendorId.Text = string.Empty;
                txtVenderName.Text = string.Empty;
                txtVendorCode.Text = string.Empty;
                //txtVesselDiscount.Text = string.Empty;
                chkGSTOffset.Checked = false;
                txtAdvanceAmount.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditOrder()
    {
        if(ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                txtPONumber.Text = dr["FLDFORMNO"].ToString();
                txtPoReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
                ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                txtVendorId.Text = dr["FLDVENDORID"].ToString();
                txtVenderName.Text = dr["FLDNAME"].ToString();
                txtVendorCode.Text = dr["FLDCODE"].ToString();
                //txtVesselDiscount.Text = dr["FLDVESSELDISCOUNT"].ToString();
                if (dr["FLDGSTOFFSET"].ToString() == "1")
                    chkGSTOffset.Checked = true;
                //if (dr["FLDISGSTAPPLICABLE"].ToString() == "1")
                //    chkGSTOffset.Enabled = false;
                if (dr["FLDPOHAVINGISSUES"].ToString() == "1")
                    chkIssues.Checked = true;
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtAdvanceAmount.Text = dr["FLDPURCHASEADVANCEAMOUNT"].ToString();
                txtDiscount.Text = dr["FLDDEFAULTDISCOUNT"].ToString();

                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                lblDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
                txtDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
                lblPercentage.Visible = (showcreditnotedisc == 1) ? true : false;
            }
        }
    }
    private bool IsValidInvoice(string vessel, string ponumber, string vendor, string currency, string poreceiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (!General.GetNullableInteger(vessel).HasValue)
            ucError.ErrorMessage = "Vessel is required";

        //if (ponumber.Trim() == string.Empty)
        //    ucError.ErrorMessage = "PO Number is required.";

        if (!General.GetNullableInteger(vendor).HasValue)
            ucError.ErrorMessage = "Vendor is required.";

        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Currency is required.";

        if (!General.GetNullableDateTime(poreceiveddate).HasValue)
            ucError.ErrorMessage = "PO received date is required.";
        else if (General.GetNullableDateTime(poreceiveddate).Value > DateTime.Now)
            ucError.ErrorMessage = "PO received date should be later then current date.";
        if (chkIssues.Checked && txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required when PO having issues is ticked.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
