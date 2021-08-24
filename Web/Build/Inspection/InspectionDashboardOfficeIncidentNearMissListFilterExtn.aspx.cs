using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class InspectionDashboardOfficeIncidentNearMissListFilterExtn : PhoenixBasePage
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
            VesselConfiguration();
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            BindVesselFleetList();
            BindVesselList();
            BindSubCategory();
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 168, "S1"); 
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
            criteria.Add("ddlIncidentNearmiss", ddlIncidentNearmiss.SelectedValue);
            criteria.Add("ucActivity", ucActivity.SelectedHard);
            criteria.Add("ucCategory", ucCategory.SelectedCategory);
            criteria.Add("ucSubCategory", ucSubcategory.SelectedSubCategory);
            criteria.Add("txtRefNo", txtRefNo.Text.Trim());
            criteria.Add("txtTitle", txtTitle.Text.Trim());
            criteria.Add("ucConsequenceCategory", ucConsequenceCategory.SelectedHard);
            criteria.Add("ucIncidentFrom", ucIncidentFrom.Text);
            criteria.Add("ucIncidentTo", ucIncidentTo.Text);
            criteria.Add("ucReportedFrom", ucReportedFrom.Text);
            criteria.Add("ucReportedTo", ucReportedTo.Text);
            criteria.Add("ucStatus", ucStatus.SelectedHard);

            InspectionFilter.CurrentIncidentnearmissDashboardFilter = criteria;
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfoQuality',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    protected void ddlIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindCategory();
        BindSubCategory();
    }
    protected void BindCategory()
    {
        ucCategory.TypeId = ddlIncidentNearmiss.SelectedValue;
        ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue == "0" ? null : ddlIncidentNearmiss.SelectedValue));
        ucCategory.DataBind();
    }

    protected void BindSubCategory()
    {
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
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
