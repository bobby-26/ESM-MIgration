using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewWarnListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        
        WarnListFilterMain.AccessRights = this.ViewState;
        WarnListFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           
        }
    }
    protected void WarnListFilterMain_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("txtPassportNo", txtPassportNo.Text);
            criteria.Add("txtCDCNo", txtCDCNo.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("ddlLastRank", ddlLastRank.SelectedRank);
            criteria.Add("ddlNationality", ddlNationality.SelectedNationality);
            criteria.Add("txtStatus", txtStatus.Text);

            Filter.WarnListFilter = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
