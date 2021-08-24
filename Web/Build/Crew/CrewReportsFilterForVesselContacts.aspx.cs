using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class ReportsFilterForVesselContacts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
               
             }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=VESSELCONTACTLIST&principal=null&vessel=null&vesseltype=null&showmenu=0";
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PrincipalSelection(object sender, EventArgs e)
    {
        
    }
    protected void VesselSelection(object sender, EventArgs e)
    {
       
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {                
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=VESSELCONTACTLIST&principal=" + ucPrincipal.SelectedAddress.ToString() + "&vessel=" + ucVessel.SelectedVessel.ToString()+"&vesseltype="+ucVesselType.SelectedVesseltype.ToString() + "&showmenu=0";
            }           
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
