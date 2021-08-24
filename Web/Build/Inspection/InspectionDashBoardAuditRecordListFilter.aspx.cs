using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;


public partial class InspectionDashBoardAuditRecordListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            BindVesselFleetList();
            BindVesselList();
            Bind_UserControls(sender, new EventArgs());
        }        
    }

    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("ucAuditCategory", ucAuditCategory.SelectedHard);
            criteria.Add("ucAudit", ddlAudit.SelectedValue);
            criteria.Add("ucFromPort", ucPort.SelectedValue);
            criteria.Add("ucToPort", ucPortTo.SelectedValue);
            criteria.Add("ReferenceNo", txtRefNo.Text);
            criteria.Add("ucFrom", ucFrom.Text);
            criteria.Add("ucTo", ucTo.Text);

            InspectionFilter.CurrentAuditInspectionCategoryDashboardFilter = criteria;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAudit.DataSource = PhoenixInspection.ListAllInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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