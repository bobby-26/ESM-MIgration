using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class InspectionMedicalCaseFilter : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");            
            MenuIncidentFilter.AccessRights = this.ViewState;
            MenuIncidentFilter.MenuList = toolbar.Show();
            ucVessel.Enabled = true;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }          
        }
    }

    protected void MenuIncidentFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            
            criteria.Add("txtCaseNo", txtCaseNo.Text.Trim());
            criteria.Add("ucTypeOfCase", ucTypeOfCase.SelectedHard);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);            
            criteria.Add("ucVessel", ucVessel.SelectedVessel);

            Filter.CurrentMedicalCaseFilterCriteria = criteria;            
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            //criteria.Clear();
            //Filter.CurrentMedicalCaseFilterCriteria = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
