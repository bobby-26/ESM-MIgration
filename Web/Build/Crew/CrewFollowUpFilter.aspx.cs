using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewFollowUpFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ddlUser.Enabled = SessionUtil.CanAccess(this.ViewState, "FOLLOWUPUSER");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);      
        MenuCrewFollowUp.AccessRights = this.ViewState;
        MenuCrewFollowUp.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            
        }
    }

    protected void CrewFollowUp_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("lstRank", ddlRank.selectedlist);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("ucLastContactFromDate", ucLastContactFromDate.Text);
            criteria.Add("ucLastContactToDate", ucLastContactToDate.Text);
            criteria.Add("lstVesselType", ddlVesselType.SelectedVesseltype);
            criteria.Add("ddlUser", ddlUser.SelectedUser);
            Filter.CrewFollowUpFilterSelection = criteria;
        }
        
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }

    
}
