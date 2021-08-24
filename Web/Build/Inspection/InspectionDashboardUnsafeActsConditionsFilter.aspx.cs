using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionDashboardUnsafeActsConditionsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {      

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuOpenReportsFilter.AccessRights = this.ViewState;
            MenuOpenReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
              
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");                   
                }

                BindVesselFleetList();
                BindVesselList();
                BindSubCategory();                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }   
    protected void MenuOpenReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {          

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
                criteria.Add("ucCategory", ucCategory.SelectedHard);
                criteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
                criteria.Add("txtReferenceNumber", txtReferenceNumber.Text);
                criteria.Add("chkIncidentNearMissRaisedYN", chkIncidentNearMissRaisedYN.Checked == true ? "1" : "0");
                criteria.Add("ucFrom", ucFrom.Text);
                criteria.Add("ucTo", ucTo.Text);

                InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter = criteria;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    private void BindSubCategory()
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ucCategory.SelectedHard));
        ddlSubcategory.DataSource = dt;
        ddlSubcategory.DataTextField = "FLDIMMEDIATECAUSE";
        ddlSubcategory.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
