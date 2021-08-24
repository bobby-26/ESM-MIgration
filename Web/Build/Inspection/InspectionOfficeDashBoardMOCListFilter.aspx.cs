using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionOfficeDashBoardMOCListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuMocFilter.AccessRights = this.ViewState;
        MenuMocFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            ViewState["OFFICEYN"] = "";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            if (Request.QueryString["OFFICEYN"] != null && Request.QueryString["OFFICEYN"].ToString() != string.Empty)
                ViewState["OFFICEYN"] = Request.QueryString["OFFICEYN"].ToString();

            if(ViewState["OFFICEYN"].ToString()=="1")
            {
                trCompany.Visible = false;
                trVessel.Visible = false;
            }
            BindVesselFleetList();
            BindVesselList();
            Category();
            SubCategory();
        }
       
    }

    protected void MenuMocFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            string fleetList = GetCsvValue(ddlFleet);
            string VesselList = GetCsvValue(ddlVessel);

            criteria.Clear();

            criteria.Add("FleetList", fleetList);
            criteria.Add("Owner", ucOwner.SelectedAddress);
            criteria.Add("VesselTypeList", ucVesselType.SelectedVesseltype);
            criteria.Add("VesselList", VesselList);
            criteria.Add("uccategory", ddlCategory.SelectedValue);
            criteria.Add("ucsubcategory", ddlSubCategory.SelectedValue);
            criteria.Add("ucnatureofchange", ddlNatureofChange.SelectedValue);
            criteria.Add("ucreferenceno", txtRefNO.Text.Trim());
            criteria.Add("ucFrom", ucFrom.Text);
            criteria.Add("ucTo", ucTo.Text);

            InspectionFilter.CurrentMOCDashboardFilter = criteria;
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'MOC');", true);
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SubCategory();
    }
    protected void SubCategory()
    {

        ddlSubCategory.DataSource = PhoenixInspectionMOCCategory.MOCSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));

        ddlSubCategory.DataTextField = "FLDMOCSUBCATEGORYNAME";
        ddlSubCategory.DataValueField = "FLDMOCSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void Category()
    {
        ddlCategory.DataSource = PhoenixInspectionMOCCategory.MOCCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlCategory.DataTextField = "FLDMOCCATEGORYNAME";
        ddlCategory.DataValueField = "FLDMOCCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
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