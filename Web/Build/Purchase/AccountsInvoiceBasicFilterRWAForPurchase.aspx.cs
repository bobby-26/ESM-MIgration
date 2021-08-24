using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsInvoiceBasicFilterRWAForPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Invoice";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            if (Request.QueryString["qcalfrom"] != null && Request.QueryString["qcalfrom"] != string.Empty)
                ViewState["CALLFROM"] = Request.QueryString["qcalfrom"];
            if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                ViewState["CALFROM"] = Request.QueryString["qcallfrom"];

           
            txtInvoiceNumberSearch.Focus();
          //  bind();
          //  BindVesselFleetList();
          //  BindVesselList();
          //  BindVesselPurchaserList();
          //  BindVesselPurchaseSuptList();
            //BindPortList();
            ViewState["SelectedVesselList"] = "";
          
           
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
          
           
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
            if (!IsValidSearch())
            {
                ucError.Visible = true;
                return;
            }


            criteria.Add("ddlSuptList", RadMcUserSup.SelectedValue);

           
            criteria.Clear();
            criteria.Add("txtInvoiceNumberSearch", txtInvoiceNumberSearch.Text.Trim());
            criteria.Add("txtSupplierReferenceSearch", txtSupplierReferenceSearch.Text.Trim());
            criteria.Add("txtOrderNumber", txtOrderNumber.Text.Trim());
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());
            criteria.Add("ddlSuptList", RadMcUserSup.SelectedValue);
            criteria.Add("RadMcUserFM", RadMcUserFM.SelectedValue);
            criteria.Add("RadMcUserTD", RadMcUserTD.SelectedValue);
            Filter.CurrentInvoiceSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceSelection = criteria;
        }
        if (ViewState["CALLFROM"].ToString() == "POSTINVOICE")
            Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "INVOICE")
            Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "INVOICEFORPURCHASE")
            Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "ADJUSTMENT")
            Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx", false);
    }

    public bool IsValidSearch()
    {
        ucError.HeaderMessage = "Please provide the following required information";
      
        return (!ucError.IsError);
    }

    
}



