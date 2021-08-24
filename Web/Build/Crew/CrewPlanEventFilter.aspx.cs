using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewPlanEventFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        FilterMain.AccessRights = this.ViewState;
        FilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();           
                ddlVessel.Enabled = false;
            }
        }
    }

    protected void FilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Add("ddlvessel", ddlVessel.SelectedVessel);          
            criteria.Add("txtEventFrom", txtEventFrom.Text);
            criteria.Add("txtEventTo", txtEventTo.Text);
            criteria.Add("ucport", ucport.SelectedValue);       
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);

            Filter.CurrentCrewPlanEventFilter = criteria;

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);

        }
        
    }

}