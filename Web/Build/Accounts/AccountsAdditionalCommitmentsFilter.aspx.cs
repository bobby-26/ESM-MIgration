using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Accounts_AccountsAdditionalCommitmentsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
          
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuOfficeFilterMain.Title = "Additional Commitments";
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
        txtVendorId.Attributes.Add("style", "visibility:hidden");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.AdditionalCommitments;
        if (nvc != null && !IsPostBack)
        {
            ddlVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
            txtPOFrom.Text = (nvc.Get("txtPOFrom") == null) ? "" : nvc.Get("txtPOFrom").ToString();
            txtPOTo.Text = (nvc.Get("txtPOTo") == null) ? "" : nvc.Get("txtPOTo").ToString();
            txtRvFrom.Text = (nvc.Get("txtRvFrom") == null) ? "" : nvc.Get("txtRvFrom").ToString();
            txtRvTo.Text = (nvc.Get("txtRvTo") == null) ? "" : nvc.Get("txtRvTo").ToString();
            txtVendorId.Text = (nvc.Get("txtVendorId") == null) ? "" : nvc.Get("txtVendorId").ToString();
            txtPONumber.Text = (nvc.Get("txtPONumber") == null) ? "" : nvc.Get("txtPONumber").ToString();
            txtVenderName.Text = (nvc.Get("txtVenderName") == null) ? "" : nvc.Get("txtVenderName").ToString();
            txtVendorCode.Text = (nvc.Get("txtVendorCode") == null) ? "" : nvc.Get("txtVendorCode").ToString();
            //txtVendorId.Text = (nvc.Get("txtVendorId") == null) ? "" : nvc.Get("txtVendorId").ToString();

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
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("txtPOFrom", txtPOFrom.Text);
            criteria.Add("txtPOTo", txtPOTo.Text);
            criteria.Add("txtRvFrom", txtRvFrom.Text);
            criteria.Add("txtRvTo", txtRvTo.Text);
            criteria.Add("txtVendorId", txtVendorId.Text);
            criteria.Add("txtPONumber", txtPONumber.Text);
            criteria.Add("txtVendorCode", txtVendorCode.Text);
            criteria.Add("txtVenderName", txtVenderName.Text);
            Filter.AdditionalCommitments = criteria;
            Response.Redirect("../Accounts/AccountsAdditionalCommitments.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            Filter.AdditionalCommitments = null;
            Response.Redirect("../Accounts/AccountsAdditionalCommitments.aspx", false);
        }
    }
}
