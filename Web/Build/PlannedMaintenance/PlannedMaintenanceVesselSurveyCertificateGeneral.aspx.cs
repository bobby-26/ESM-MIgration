using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");

            if (!string.IsNullOrEmpty( Request.QueryString["Dtkey"]))
            {
                if ((string.IsNullOrEmpty(Request.QueryString["ShowOnlyEndorse"]) ? "" : Request.QueryString["ShowOnlyEndorse"]).Trim().Equals("1")
                    && !string.IsNullOrEmpty(Request.QueryString["ScheduleId"]))
                {
                    toolbarmain.AddButton("Endorse", "ENDORSE");
                }
                else if (string.IsNullOrEmpty(Request.QueryString["ScheduleId"]))
                    toolbarmain.AddButton("Renewal", "RENEWAL");
                else
                {
                    toolbarmain.AddButton("Renewal", "RENEWAL");
                    toolbarmain.AddButton("Endorse", "ENDORSE");
                }
            }
            else
                toolbarmain.AddButton("Issue", "RENEWAL");

            MenuSurveyCertificateHeader.AccessRights = this.ViewState;
            MenuSurveyCertificateHeader.MenuList = toolbarmain.Show();
            MenuSurveyCertificateHeader.SetTrigger(pnlSurvey);
            if (!IsPostBack)
            {
                ViewState["ScheduleId"] = Request.QueryString["ScheduleId"] != null ? Request.QueryString["ScheduleId"] : "";
                ViewState["CertificateId"] = Request.QueryString["CertificateId"] != null ? Request.QueryString["CertificateId"] : "";
                ViewState["DtKey"] = Request.QueryString["DtKey"] != null ? Request.QueryString["DtKey"] : "";
                ViewState["VesselId"] = Request.QueryString["VesselId"] != null ? Request.QueryString["VesselId"] : "";
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateDetails.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&CertificateId=" + ViewState["CertificateId"].ToString();
                ViewState["CURRENTINDEX"] = 0;
            }
            MenuSurveyCertificateHeader.SelectedMenuIndex = Int32.Parse(ViewState["CURRENTINDEX"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveyCertificateHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateDetails.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&CertificateId=" + ViewState["CertificateId"].ToString() + "&DtKey=" + ViewState["DtKey"].ToString();
                ViewState["CURRENTINDEX"] = 0;
            }
            else if (dce.CommandName.ToUpper().Equals("RENEWAL"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateRenewal.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&CertificateId=" + ViewState["CertificateId"].ToString() + "&DtKey=" + ViewState["DtKey"].ToString();
                ViewState["CURRENTINDEX"] = 1;
            }
            else if (dce.CommandName.ToUpper().Equals("ENDORSE"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateEndorse.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&CertificateId=" + ViewState["CertificateId"].ToString() + "&DtKey=" + ViewState["DtKey"].ToString();
                ViewState["CURRENTINDEX"] = 2;
            }
            else if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
