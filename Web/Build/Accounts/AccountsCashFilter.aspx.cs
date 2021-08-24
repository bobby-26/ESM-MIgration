using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsCashFilter : PhoenixBasePage
{
    public int iCompanyCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtAccountId.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            txtRemittenceNumberSearch.Focus();
            iCompanyCode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ucRemittanceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.CASHSTATUS).ToString();
            //txtVendorId.Attributes.Add("style", "visibility:hidden");

            //if (Request.QueryString["source"] != null)
            //{
            //    ViewState["Source"] = Request.QueryString["source"];
            //    lblCaption.Text = "Cash Out Filter";
            //}

           // ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true); ");
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtCashNumberSearch", txtRemittenceNumberSearch.Text.Trim());
            criteria.Add("txtuserid", txtuserid.Text.Trim());
            criteria.Add("ucCashStatus", ucRemittanceStatus.SelectedHard);
            criteria.Add("txtSupplierCode", txtuserid.Text.Trim());
            criteria.Add("txtAccountId", txtAccountId.Text.Trim());
            criteria.Add("ddlCurrencyCode",Convert.ToString(ddlCurrencyCode.SelectedCurrency));
           

            Filter.CurrentCashOutSelection = criteria;
        }

      
               Response.Redirect("../Accounts/AccountsCashOutRequest.aspx", false);
     
    }
}



