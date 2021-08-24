using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class InspectionReportIncident : PhoenixBasePage
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
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }    
                BindIncidents();
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
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=INCIDENTNEARMISS&inspectionincidentid=" + ddlIncidents.SelectedValue + "&showmenu=0&showexcel=no";
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

        if (ddlIncidents.SelectedIndex == 0)
        {
            ucError.ErrorMessage = "Please select Incident.";
        }
        return (!ucError.IsError);
    }
    public void BindIncidents()
    {        
        string status = "";
        status = PhoenixCommonRegisters.GetHardCode(1, 168, "S2");
        status = status + "," + PhoenixCommonRegisters.GetHardCode(1, 168, "S3");
        status = status + "," + PhoenixCommonRegisters.GetHardCode(1, 168, "S4");        
        DataTable dt = PhoenixInspectionIncident.ListIncidents(General.GetNullableInteger(ucVessel.SelectedVessel)
            , General.GetNullableString(status));
        ddlIncidents.Items.Add("select");
        ddlIncidents.DataSource = dt;
        ddlIncidents.DataTextField = "FLDINCIDENTREFNO";
        ddlIncidents.DataValueField = "FLDINSPECTIONINCIDENTID";
        ddlIncidents.DataBind();
        ddlIncidents.Items.Insert(0, new ListItem("--Select--", "Dummy"));         
    }
    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        BindIncidents();        
        ifMoreInfo.Attributes["src"] = "";
    }
}
