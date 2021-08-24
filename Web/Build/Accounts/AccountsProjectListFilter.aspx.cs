using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
            ProjectCodeList.AccessRights = this.ViewState;
            ProjectCodeList.MenuList = toolbar.Show();
        }
    }

    protected void ProjectCodeList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtTitle", txtTitle.Text.Trim());
            criteria.Add("txtProjectCode", txtProjectCode.Text.Trim());
            criteria.Add("ddltype", ddltype.SelectedValue);
        
            Filter.ProjectCodeListFilter = criteria;
        }

        String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
    }
}