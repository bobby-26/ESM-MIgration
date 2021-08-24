using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;
using System.Collections.Specialized;


public partial class PlannedMaintenanceVesselSurveyAuditPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarmain.AddButton("Plan", "PLAN");
                toolbarmain.AddButton("Audit", "AUDIT");
                toolbarsub.AddButton("Save", "SAVE");
                MenuPlanAudit.AccessRights = this.ViewState;
                MenuGeneral.AccessRights = this.ViewState;
                MenuPlanAudit.MenuList = toolbarmain.Show();
                MenuGeneral.MenuList = toolbarsub.Show();
                VesselConfiguration();
                ucVessel.Enabled = true;
                ViewState["COMPANYID"] = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
                ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["CertificateList"] = Request.QueryString["CertificateList"] != null ? Request.QueryString["CertificateList"] : "";
                ViewState["ucPlanDate"] = Request.QueryString["ucPlanDate"].ToString()!= null ? Request.QueryString["ucPlanDate"] : "";
                ViewState["ucPort"] = Request.QueryString["ucPort"].ToString() != null ? Request.QueryString["ucPort"] : null;
         

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.bind();
                    ucVessel.SelectedVessel = ViewState["VesselId"].ToString();
                    ucVessel.Enabled = false;
                    ucAuditCategory.SelectedHard = "711";
                }
                Bind_UserControls(sender, new EventArgs());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");

        string externalaudit = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");

        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSchedule(ucVessel.SelectedVessel, ddlAudit.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertAuditPlanner(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    ,PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    ,ViewState["CertificateList"].ToString()
                                                                    ,General.GetNullableDateTime(ViewState["ucPlanDate"].ToString())
                                                                    ,General.GetNullableInteger(ViewState["ucPort"].ToString())
                                                                    ,int.Parse(ViewState["COMPANYID"].ToString())
                                                                    ,new Guid(ddlAudit.SelectedValue));

                ucStatus.Text = "Audit created successfully.";

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPlanAudit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("PLAN"))
            {

                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyPlanner.aspx?VesselId=" + ViewState["VesselId"].ToString()
                    + "&Certificateid=" + ViewState["CertificateList"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSchedule(string vesselid, string inspectionid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(inspectionid) == null)
            ucError.ErrorMessage = "Audit/Inspection is required.";

        return (!ucError.IsError);
    }

    protected void txtLastDoneDate_Changed(object sender, EventArgs e)
    {
        int frequency = 0;
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(ddlAudit.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
                frequency = int.Parse(ds.Tables[0].Rows[0]["FLDAUDITFREQUENCYINMONTHS"].ToString());
        }

    }

    protected void ddlAudit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "";
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(ddlAudit.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
                type = ds.Tables[0].Rows[0]["FLDINSPECTIONCATEGORYID"].ToString();
        }

    }
}
