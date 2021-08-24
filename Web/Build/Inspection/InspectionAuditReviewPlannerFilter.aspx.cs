using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditReviewPlannerFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionReviewPlanner.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            VesselConfiguration();
            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                fleetrow.Visible = false;
                vesselrow.Visible = false;
            }
            if (Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration != "" && int.Parse(Filter.CurrentVesselConfiguration) > 0)
            {
                ddlPlanned.SelectedValue = "1";
                ddlPlanned.Enabled = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                office.Visible = false;
            }
            Bind_UserControls(sender, new EventArgs());
            BindInternalInspector();
            Bind_OfficeUserControls(sender, new EventArgs());
            BindOfficeInternalInspector();
        }
    }

    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucAuditCategory", ucAuditCategory.SelectedHard);
            criteria.Add("ucAudit", ddlAudit.SelectedValue);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucFromPort", ucFromPort.SelectedValue);
            criteria.Add("ucToPort", ucToPort.SelectedValue);
            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ddlPlanned", (ddlPlanned.SelectedValue == "2" ? null : ddlPlanned.SelectedValue));
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucCharterer", ucCharterer.SelectedAddress);
            criteria.Add("txtExternalInspector", txtExternalInspector.Text);
            criteria.Add("txtExternalOrganization", txtExternalOrganization.Text);
            criteria.Add("ddlInspectorName", ddlInspectorName.SelectedValue);
            criteria.Add("txtPlannedFrom", txtPlannedFrom.Text);
            criteria.Add("txtPlannedTo", txtPlannedTo.Text);
            criteria.Add("chkAtSea", (chkAtSea.Checked == true ? "1" : "0"));

            Filter.CurrentReviewPlannerFilter = criteria;

            NameValueCollection Officecriteria = new NameValueCollection();

            Officecriteria.Clear();
            Officecriteria.Add("ucCompanySelect", ucOfficeCompanySelect.SelectedCompany);
            Officecriteria.Add("ucAuditCategory", ucOfficeAuditCategory.SelectedHard);
            Officecriteria.Add("ucAudit", ddlOfficeAudit.SelectedValue);
            Officecriteria.Add("txtFrom", txtOfficeFrom.Text);
            Officecriteria.Add("txtTo", txtOfficeTo.Text);
            Officecriteria.Add("ddlPlanned", (ddlOfficePlanned.SelectedValue == "2" ? null : ddlOfficePlanned.SelectedValue));
            Officecriteria.Add("txtExternalInspector", txtOfficeExternalInspector.Text);
            Officecriteria.Add("txtExternalOrganization", txtOfficeExternalOrganization.Text);
            Officecriteria.Add("ddlInspectorName", ddlOfficeInspectorName.SelectedValue);
            Officecriteria.Add("txtPlannedFrom", txtOfficePlannedFrom.Text);
            Officecriteria.Add("txtPlannedTo", txtOfficePlannedTo.Text);

            Filter.CurrentReviewOfficePlannerFilter = Officecriteria;

            Response.Redirect("../Inspection/InspectionReviewPlanner.aspx", false);
        }
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void BindInternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlInspectorName.DataSource = ds.Tables[0];
        ddlInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlInspectorName.DataValueField = "FLDUSERCODE";
        ddlInspectorName.DataBind();
        ddlInspectorName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void Bind_OfficeUserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlOfficeAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlOfficeAudit.DataTextField = "FLDSHORTCODE";
        ddlOfficeAudit.DataValueField = "FLDINSPECTIONID";
        ddlOfficeAudit.DataBind();
        ddlOfficeAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void BindOfficeInternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlOfficeInspectorName.DataSource = ds.Tables[0];
        ddlOfficeInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlOfficeInspectorName.DataValueField = "FLDUSERCODE";
        ddlOfficeInspectorName.DataBind();
        ddlOfficeInspectorName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}

