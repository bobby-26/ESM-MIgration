using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionInternalAuditReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                VesselConfiguration();
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                
                MenuReportsFilter.AccessRights = this.ViewState;
                if (Request.QueryString["alert"] == null)
                    MenuReportsFilter.MenuList = toolbar.Show();                
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {                
                ucAuditSchedule.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucAuditSchedule.AuditScheduleList = PhoenixInspectionAuditSchedule.ListAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, General.GetNullableInteger(ucAuditSchedule.VesselId), null);
                ucAuditSchedule.DataBind();
                ucDate.Text = DateTime.Now.ToShortDateString();
            }
            if (Request.QueryString["alert"] != null)
            {                
                ucAuditSchedule.VesselId = Request.QueryString["vesselid"].ToString();
                ucAuditSchedule.AuditScheduleList = PhoenixInspectionAuditSchedule.ListAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, General.GetNullableInteger(ucAuditSchedule.VesselId), null);
                ucAuditSchedule.DataBind();
                ucAuditSchedule.SelectedAuditSchedule = Request.QueryString["reviewscheduleid"].ToString();                
                ucDate.Enabled = false;
                ucAuditSchedule.Enabled = false;
                ucDate.Text = Request.QueryString["date"].ToString();
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=INTERNALAUDIT&reviewscheduleid=" + new Guid(ucAuditSchedule.SelectedAuditSchedule) + "&reportdate=" + Convert.ToDateTime(ucDate.Text) + "&showmenu=0&showexcel=no";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=INTERNALAUDIT&reviewscheduleid=" + new Guid(ucAuditSchedule.SelectedAuditSchedule) + "&reportdate=" + Convert.ToDateTime(ucDate.Text) + "&showmenu=0&showexcel=no";
                }
            }
            if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionAuditSchedule.AuditReportGenerationUpdate(new Guid(ucAuditSchedule.SelectedAuditSchedule));
                ucStatus.Text = "Report is confirmed.";
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
        
        if (ucAuditSchedule.SelectedAuditSchedule.ToString() == "" || ucAuditSchedule.SelectedAuditSchedule.ToString() == "Dummy")
        {
            ucError.ErrorMessage = "Audit is Required";
        }

        return (!ucError.IsError);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
}
