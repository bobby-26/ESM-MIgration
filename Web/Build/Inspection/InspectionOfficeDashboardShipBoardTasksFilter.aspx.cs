using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionOfficeDashboardShipBoardTasksFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new  PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            ViewState["TASK"] = "";

            if (Request.QueryString["TASK"] != null && Request.QueryString["TASK"].ToString() != string.Empty)
                ViewState["TASK"] = Request.QueryString["TASK"].ToString();

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            if(ViewState["TASK"].ToString() =="VIR")
            {
                rowinspection.Visible = false;
                
            }

            BindVIRChapter();
            BindVesselFleetList();
            BindVesselList();
            BindInspection();
            BindChapter();
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
            criteria.Add("ucInspectionType", ucInspectionType.SelectedHard);
            criteria.Add("ucInspection", ddlInspection.SelectedValue);
            criteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucNonConformanceCategory", ucNonConformanceCategory.SelectedQuick);
            criteria.Add("ucChapter", ddlChapter.SelectedValue);
            criteria.Add("ddlSourceType", ddlSourceType.SelectedValue == "0" ? null : ddlSourceType.SelectedValue);
            criteria.Add("txtSourceRefNo", txtSourceRefNo.Text);
            criteria.Add("ucFrom", ucFrom.Text);
            criteria.Add("ucTo", ucTo.Text);

            InspectionFilter.CurrentShipTaskDashboardFilter = criteria;

        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
   
    protected void ucInspectionType_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }  

    protected void BindInspection()
    {

        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , null
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindVIRChapter()
    {
        if (ViewState["TASK"].ToString() == "VIR")
        {
            ddlChapter.DataSource = PhoenixInspectionChapter.ListVIRInspectionChapter();
            ddlChapter.DataBind();
        }
    }

    protected void BindChapter ()
    {
        if (ViewState["TASK"].ToString() != "VIR")
        {
            ddlChapter.DataSource = PhoenixInspectionChapter.ListInspectionChapter(null,
                                                                                  null,
                                                                                  General.GetNullableGuid(ddlInspection.SelectedValue));
            ddlChapter.DataBind();
        }
    }
    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        BindChapter();
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