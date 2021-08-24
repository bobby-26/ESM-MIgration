using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersActivityFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuActivityFilterMain.AccessRights = this.ViewState;
        MenuActivityFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            txtActivityCode.Focus();
            BindHard();
        }
    }

    protected void ActivityFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtActivityCode", txtActivityCode.Text);
            criteria.Add("txtActivityName", txtActivityName.Text);
            criteria.Add("ucGroup", ucGroup.SelectedHard);
            criteria.Add("ucLeave", ucLeave.SelectedHard);
            criteria.Add("txtPayRollHeader", txtPayRollHeader.Text);
            criteria.Add("txtLevel", txtLevel.Text);
            Filter.CurrentAddressFilterCriteria = criteria;
        }
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }

    protected void BindHard()
    {
        ucGroup.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.ACTIVITYGROUP).ToString();
        ucLeave.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.LEAVE).ToString();
    }
}
