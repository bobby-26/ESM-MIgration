using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLeaveRecordFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
         
            MenuPD.AccessRights = this.ViewState;
            MenuPD.MenuList = toolbar.Show();           
        }
    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);
            Filter.CrewLeaveRecordFilter = criteria;
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
        "BokMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
    }	
}
