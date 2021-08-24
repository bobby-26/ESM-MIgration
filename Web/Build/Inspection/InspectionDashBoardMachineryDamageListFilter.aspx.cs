using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionDashBoardMachineryDamageListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuIncidentFilter.AccessRights = this.ViewState;
        MenuIncidentFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");

            }
            Filter.CurrentVesselConfiguration = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();

            BindVesselFleetList();
            BindVesselList();
            BindControls();

        }
    }



    protected void MenuIncidentFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            string fleetList = GetCsvValue(ddlFleet);
            string VesselList = GetCsvValue(ddlVessel);
            criteria.Clear();

            criteria.Add("FleetList", fleetList);
            criteria.Add("Owner", ucOwner.SelectedAddress);
            criteria.Add("VesselTypeList", ucVesselType.SelectedVesseltype);
            criteria.Add("VesselList", VesselList);
            criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            criteria.Add("ddlSubCategory", ddlSubCategory.SelectedValue);
            criteria.Add("ddlProcessSubHazardId", ddlProcessLoss.SelectedValue);
            criteria.Add("ddlPropertySubHazardId", ddlCost.SelectedValue);
            criteria.Add("txtRefno", txtRefNo.Text.Trim());
            criteria.Add("txtTitle", txtTitle.Text.Trim());
            criteria.Add("ddlConsequenceCategory", ucConsequenceCategory.SelectedHard);
            criteria.Add("ucIncidentFrom", ucIncidentFrom.Text);
            criteria.Add("ucIncidentTo", ucIncidentTo.Text);
            criteria.Add("ucReportedFrom", ucReportedFrom.Text);
            criteria.Add("ucReportedTo", ucReportedTo.Text);
            criteria.Add("ucClosedFrom", ucClosedFrom.Text);
            criteria.Add("ucClosedTo", ucClosedTo.Text);

            InspectionFilter.CurrentMachineryDamageDashboardFilter = criteria;            
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }

    protected void BindControls()
    {
        // Process Loss

        ddlProcessLoss.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, 1, null);
        ddlProcessLoss.DataTextField = "FLDNAME";
        ddlProcessLoss.DataValueField = "FLDSUBHAZARDID";
        ddlProcessLoss.DataBind();
        ddlProcessLoss.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        // Cost

        ddlCost.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, null, 1);
        ddlCost.DataTextField = "FLDNAME";
        ddlCost.DataValueField = "FLDSUBHAZARDID";
        ddlCost.DataBind();
        ddlCost.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));


        ddlCategory.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(4);
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.DataSource = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDNAME";
        ddlSubCategory.DataValueField = "FLDINCIDENTNEARMISSSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindVesselList()
    {
        DataSet ds = PhoenixDashboardOption.DashboarVessellist();
        ddlVessel.DataSource = ds;
        ddlVessel.DataBind();
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        ddlFleet.DataSource = ds;
        ddlFleet.DataBind();
    }
    protected void ddlFleet_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        RadComboBox radComboBox = (RadComboBox)sender;
        string strfleetlist = GetCsvValue(radComboBox);
        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist);

        foreach (RadComboBoxItem item in ddlVessel.Items)
            item.Checked = false;

        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = ddlVessel.FindItemByValue(dr["FLDVESSELID"].ToString());
                if (item != null) item.Checked = true;
            }
        }
    }
    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }
}
