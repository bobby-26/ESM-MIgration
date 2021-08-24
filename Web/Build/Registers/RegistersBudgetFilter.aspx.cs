using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersBudgetFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbar.Show();

            txtAccountCodeSearch.Focus();
        }
        ucGroupSearch.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.BUDGETGROUP).ToString();
       
    }

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', 'ifMoreInfo');";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            criteria.Clear();
            criteria.Add("txtAccountCodeSearch", txtAccountCodeSearch.Text.Trim());
            criteria.Add("txtDescription", txtDescription.Text.Trim()); 
            criteria.Add("ucGroupSearch", ucGroupSearch.SelectedHard);
            criteria.Add("IsActive", Convert.ToString(chkactive.Checked==true ? 1 : 0));
            Filter.CurrentBudgetFilterSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentBudgetFilterSelection = criteria;
        }

        //     Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

    }
}



