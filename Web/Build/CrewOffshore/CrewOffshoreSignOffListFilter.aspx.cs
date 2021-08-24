using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOffshoreSignOffListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
       
        MenuSignOffFilterMain.AccessRights = this.ViewState;
        MenuSignOffFilterMain.MenuList = toolbar.Show();
        MenuSignOffFilterMain.Title = "Sign-Off List Filter";
        
        if (!IsPostBack)
        {
            ucVessel.Enabled = true;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
        }
    }

    private void Clear()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            ucVessel.SelectedVessel = "";
        }
        ucRank.SelectedRank = "";
        txtName.Text = "";
        txtFileNo.Text = "";
        txtSignonFromDate.Text = "";
        txtSignonToDate.Text = "";
        txtReliefDueFromDate.Text = "";
        txtReliefDueToDate.Text = "";
        
    }

    protected void MenuSignOffFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucRank", ucRank.SelectedRank);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);
            criteria.Add("txtSignonFromDate", txtSignonFromDate.Text);
            criteria.Add("txtSignonToDate", txtSignonToDate.Text);
            criteria.Add("txtReliefDueFromDate", txtReliefDueFromDate.Text);
            criteria.Add("txtReliefDueToDate", txtReliefDueToDate.Text);
            Filter.CurrentOffshoreSignOffCriteria = criteria;
            Response.Redirect("../CrewOffshore/CrewOffshoreSignOffList.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Clear();
        }
    }
}
