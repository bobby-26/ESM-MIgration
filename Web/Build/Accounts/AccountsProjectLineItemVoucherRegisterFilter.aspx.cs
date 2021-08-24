using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectLineItemVoucherRegisterFilter : PhoenixBasePage
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
            criteria.Add("txtVoucherNo", txtVoucherNo.Text.Trim());
            criteria.Add("txtCreatedDate", txtCreatedDate.Text);

            Filter.ProjectCodeVoucherListFilter = criteria;

            Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegister.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), false);
        }

        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.ProjectCodeVoucherListFilter = criteria;
            Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegister.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), false);
        }
    }
}