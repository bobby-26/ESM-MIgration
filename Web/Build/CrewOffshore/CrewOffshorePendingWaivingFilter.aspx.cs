using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshorePendingWaivingFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        
        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
           

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.Enabled = false;
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
        }
    }
    private void Clear()
    {
        txtName.Text = "";
        txtFileNo.Text = "";
        ddlRank.SelectedRank = "";
        ucVessel.SelectedVessel = "";
        //ucCompletedFrom.Text = "";
        //ucCompletedTo.Text = "";
        //ddlStatus.SelectedValue = "1";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.Enabled = false;
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
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

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("txtName", txtName.Text);
            nvc.Add("txtFileNo", txtFileNo.Text);
            nvc.Add("ddlRank", ddlRank.SelectedRank);
            nvc.Add("ucVessel", ucVessel.SelectedVessel);
            //nvc.Add("ucCompletedFrom", ucCompletedFrom.Text);
            //nvc.Add("ucCompletedTo", ucCompletedTo.Text);
            nvc.Add("Status", ddlStatus.SelectedValue); 
            Filter.TrainingNeedsSearch = nvc;
            Response.Redirect("CrewOffshorePendingWaivingRequest.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Clear();
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    
    //protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    //{
    //    ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
    //    ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableInteger(ucPrincipal.SelectedAddress), 0);
    //}
}
