using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditOfficeVesselScheduleFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionAuditRecordList.aspx", false);
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
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            ucOfficeStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            txtDoneDateTo.Text = DateTime.Now.ToShortDateString();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                ChkCredited.Visible = false;
                lblCredited.Visible = false;
                ucStatus.Enabled = true;
                ucOfficeStatus.Enabled = true;
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
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                Office.Visible = false;
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

            criteria.Add("ucAuditType", ucAuditType.SelectedHard);
            criteria.Add("ucAuditCategory", ucAuditCategory.SelectedHard);
            criteria.Add("ucAudit", ddlAudit.SelectedValue);
            criteria.Add("ucPort", ucPort.SelectedValue);
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucCharterer", ucCharterer.SelectedAddress);
            criteria.Add("txtExternalInspector", txtExternalInspector.Text);
            criteria.Add("txtExternalOrganization", txtExternalOrganization.Text);
            criteria.Add("ddlInspectorName", ddlInspectorName.SelectedValue);
            criteria.Add("ucPortTo", ucPortTo.SelectedValue);
            criteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucChapter", ucChapter.SelectedChapter);
            criteria.Add("txtKey", txtKey.Text);
            criteria.Add("chkAtSea", (chkAtSea.Checked == true ? "1" : "0"));
            criteria.Add("ChkCredited", (ChkCredited.Checked == true ? "1" : "0"));
            Filter.CurrentAuditScheduleFilterCriteria = criteria;

            NameValueCollection Officecriteria = new NameValueCollection();

            Officecriteria.Clear();

            Officecriteria.Add("ucAuditType", ucOfficeAuditType.SelectedHard);
            Officecriteria.Add("ucAuditCategory", ucOfficeAuditCategory.SelectedHard);
            Officecriteria.Add("ucAudit", ddlOfficeAudit.SelectedValue);
            Officecriteria.Add("ucStatus", ucOfficeStatus.SelectedHard);
            Officecriteria.Add("txtDoneFrom", txtOfficeDoneDateFrom.Text);
            Officecriteria.Add("txtDoneTo", txtOfficeDoneDateTo.Text);
            Officecriteria.Add("txtRefNo", txtOfficeRefNo.Text);
            Officecriteria.Add("txtExternalInspector", txtOfficeExternalInspector.Text);
            Officecriteria.Add("txtExternalOrganization", txtOfficeExternalOrganization.Text);
            Officecriteria.Add("ddlInspectorName", ddlOfficeInspectorName.SelectedValue);
            Officecriteria.Add("ddlDefType", ddlOfficeNCType.SelectedValue == "0" ? null : ddlOfficeNCType.SelectedValue);
            Officecriteria.Add("ucChapter", ucOfficeChapter.SelectedChapter);
            Officecriteria.Add("txtKey", txtOfficeKey.Text);

            Filter.CurrentAuditOfficeScheduleFilterCriteria = Officecriteria;

            Response.Redirect("../Inspection/InspectionAuditRecordList.aspx?callfrom=record&menu=y", false);
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

    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlAudit.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(ucAuditType.SelectedHard),
            General.GetNullableInteger(ucAuditCategory.SelectedHard),
            General.GetNullableGuid(ddlAudit.SelectedValue));
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
    protected void ucOfficeInspection_Changed(object sender, EventArgs e)
    {
        ucOfficeChapter.InspectionId = ddlOfficeAudit.SelectedValue;
        ucOfficeChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(ucOfficeAuditType.SelectedHard),
            General.GetNullableInteger(ucOfficeAuditCategory.SelectedHard),
            General.GetNullableGuid(ddlOfficeAudit.SelectedValue));
    }
}
