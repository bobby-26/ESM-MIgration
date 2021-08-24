using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class InspectionReportCARStatusLog : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                VesselConfiguration();
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }    
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=CARSTATUSLOG&vesselid=" + ucVessel.SelectedVessel + "&showmenu=0&fromdate=null&todate=null&showexcel=no";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";        

        if (ucVessel.SelectedVessel.ToUpper().Equals("DUMMY") || ucVessel.SelectedVessel == "")
        {
            ucError.ErrorMessage = "Vessel is Required.";
        }        
        return (!ucError.IsError);
    }
}
