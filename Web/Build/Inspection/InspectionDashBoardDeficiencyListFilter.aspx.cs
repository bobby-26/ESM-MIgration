using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionDashBoardDeficiencyListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuDefeciencyFilter.AccessRights = this.ViewState;
        MenuDefeciencyFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            BindVesselFleetList();
            BindVesselList();
            BindInspection();
            
        }        
    }

    protected void MenuDefeciencyFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {

            string fleetList = GetCsvValue(ddlFleet);
            string VesselList = GetCsvValue(ddlVessel);
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("FleetList", fleetList);
            criteria.Add("Owner", ucOwner.SelectedAddress);
            criteria.Add("VesselTypeList", ucVesselType.SelectedVesseltype);
            criteria.Add("VesselList", VesselList);
            criteria.Add("uctype", ucInspectionType.SelectedHard );
            criteria.Add("uccategory", ucInspectionCategory.SelectedHard );
            criteria.Add("ucinspection", ddlInspection.SelectedValue );
            criteria.Add("ucchapter", ucChapter.SelectedChapter );
            criteria.Add("ucdefeciencytype", ddlNCType.SelectedValue == "0" ? null: ddlNCType.SelectedValue);
            criteria.Add("ucdefeciencycategory", ucNonConformanceCategory.SelectedQuick);
            criteria.Add("ucsource", ddlSource.SelectedValue == "0" ? null : ddlSource.SelectedValue);
            criteria.Add("ucsourceno", txtSourceRefNo.Text.Trim());
            criteria.Add("ucreferenceno", txtRefNo.Text.Trim());

            InspectionFilter.CurrentDeficiencyDashboardFilter = criteria;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(ucInspectionType.SelectedHard),
            General.GetNullableInteger(ucInspectionCategory.SelectedHard),
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }
    protected void ucInspectionType_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }

    protected void ucInspectionCategory_TextChangedEvent(object sender, EventArgs e)
    {
        BindInspection();
    }
    protected void BindInspection()
    {
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(ucInspectionType.SelectedHard)
                                       , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                       , null
                                       , 1
                                       , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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