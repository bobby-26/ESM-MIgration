using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectLineItemConfirmationFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        ProjectCodeList.AccessRights = this.ViewState;
        ProjectCodeList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PROJECTID"] = null;
            ViewState["ACCOUNTID"] = "";

            if (Request.QueryString["id"] != null)
            {
                ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
            }
            if (Request.QueryString["accountid"] != null)
            {
                ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
            }

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
        }
    }

    protected void ProjectCodeList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtRequisitionNo", txtRequisitionNo.Text.Trim());
            criteria.Add("ddlType", ddlType.SelectedValue);
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());
          
            Filter.ProjectCodeConfirmationListFilter = criteria;
        }

        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();

            Filter.ProjectCodeConfirmationListFilter = criteria;
        }

        Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), false);
    }
}