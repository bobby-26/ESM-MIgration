using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Owners;

public partial class OwnersVesselMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("List", "LIST");
            //toolbar.AddButton("Particulars", "PARTICULARS");
            toolbar.AddButton("Communication", "COMMUNICATIONDETAILS");
            toolbar.AddButton("Admin", "ADMIN");
            toolbar.AddButton("Certificates", "CERTIFICATES");
            MenuVesselMaster.AccessRights = this.ViewState;
            MenuVesselMaster.MenuList = toolbar.Show();

            if (Filter.CurrentOwnerVesselMasterFilter != null && Request.QueryString["certificatesalert"] != null)
            {
                MenuVesselMaster.SelectedMenuIndex = 2;
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselCertificates.aspx";
            }
            else
            {
                Filter.CurrentOwnerVesselMasterFilter = null;
                MenuVesselMaster.SelectedMenuIndex = 0;
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselList.aspx";
            }
        }

        if (Filter.CurrentOwnerVesselMasterFilter == null)
            MenuVesselMaster.SelectedMenuIndex = 0;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }

    protected void VesselMaster_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;

        if (Filter.CurrentOwnerVesselMasterFilter == null)
        {
            if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
            {               
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVessel.aspx";
                return;            
            }            
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselList.aspx";
                return;
            }
            if (dce.CommandName.ToUpper().Equals("ADMIN"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselAdmin.aspx";
                return;
            }            
            ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselList.aspx";

            ShowError();
        }
        else
        {            
            if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselCertificates.aspx";                
            }
            else if (dce.CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselCommunicationDetails.aspx";             
            }
            else if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVessel.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("ADMIN"))
            {
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselAdmin.aspx";
            }
            else
                ifMoreInfo.Attributes["src"] = "../Owners/OwnersVesselList.aspx";   
        }       
    }

    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Vessel and then Navigate to other Tabs";
        ucError.Visible = true;
        MenuVesselMaster.SelectedMenuIndex = 0;
    }
}
