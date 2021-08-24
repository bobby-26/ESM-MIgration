using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceBySupplierFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Invoice";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {          
            if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                ViewState["CALLFROM"] = Request.QueryString["qcallfrom"];
          
            txtInvoiceNumberSearch.Focus();

            if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString().ToUpper().Equals("SUPPLIERPORTAL"))
            {
                ImgSupplierPickList.Attributes["onclick"] = null;
                txtVendorId.Attributes.Add("style", "visibility:hidden");
                txtVendorCode.Attributes.Add("style", "visibility:hidden");
                txtVenderName.Attributes.Add("style", "visibility:hidden");
                ImgSupplierPickList.Visible = false;
                
            }
            else
            {
                txtVendorId.Attributes.Add("style", "visibility:hidden");
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132,135,183', true); ");
            }
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            //criteria.Add("ddlInvoiceType", ddlInvoiceType.SelectedHard);
            criteria.Add("txtInvoiceNumberSearch", txtInvoiceNumberSearch.Text.Trim());
            criteria.Add("txtSupplierReferenceSearch", txtSupplierReferenceSearch.Text.Trim());
            //criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtInvoiceFromdateSearch", txtInvoiceFromdateSearch.Text);
            criteria.Add("txtInvoiceTodateSearch", txtInvoiceTodateSearch.Text);
            //criteria.Add("txtReceivedFromdateSearch", txtReceivedFromdateSearch.Text);
            //criteria.Add("txtReceivedTodateSearch", txtReceivedTodateSearch.Text);
            criteria.Add("ddlInvoiceStatus", ddlInvoiceStatus.SelectedValue);
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());

            Filter.CurrentInvoiceBySupplierSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceBySupplierSelection = criteria;
        }

        Response.Redirect("../Accounts/AccountsInvoiceBySupplierMaster.aspx", false);
    }
}
