using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;
public partial class Dashboard_DashboardOfficeV2Filter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ddlOwner.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();
            ViewState["type"] = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["d"]))
            {
                ViewState["type"] = Request.QueryString["d"];
            }

            BindVesselFleetList();
            BindVesselTypeList();
            BindVesselList();
            BindGroupRankList();
            BindZoneList();
            BindFilterVessel();

            if (ViewState["type"].ToString() == "2")
            {
                lblGroupRank.Visible = true;
                ddlGroupRank.Visible = true;
                lblzone.Visible = true;
                ddlzone.Visible = true;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("OWNER"))
            {
                owner.Visible = false;
            }
            else
            {
                owner.Visible = true;
            }
            if (chkViewAll.Checked == true)
            {
                ddlOwner.Enabled = false;
                ddlFleet.Enabled = false;
                ddlVeselType.Enabled = false;
                ddlVessel.Enabled = false;
                ddlGroupRank.Enabled = false;
            }
            else
            {
                ddlOwner.Enabled = true;
                ddlFleet.Enabled = true;
                ddlVeselType.Enabled = true;
                ddlVessel.Enabled = true;
                ddlGroupRank.Enabled = true;
            }

        }
    }
    private void BindZoneList()
    {
        DataSet ds = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
        ddlzone.DataSource = ds;
        ddlzone.DataBind();
    }
    private void BindGroupRankList()
    {
        DataSet ds = PhoenixDashboardOption.DashboarGroupRanklist();
        ddlGroupRank.DataSource = ds;
        ddlGroupRank.DataBind();
    }

    private void BindVesselList()
    {
        if (ViewState["type"].ToString() == "2")//Crew
        {
            DataSet ds = PhoenixDashboardOption.DashboarVesselList(4);
            ddlVessel.DataSource = ds;
            ddlVessel.DataBind();
        }
        else
        {
            DataSet ds = PhoenixDashboardOption.DashboarVesselList();
            ddlVessel.DataSource = ds;
            ddlVessel.DataBind();
        }
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        ddlFleet.DataSource = ds;
        ddlFleet.DataBind();
    }
    private void BindVesselTypeList()
    {
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(1);
        ddlVeselType.DataSource = ds;
        ddlVeselType.DataBind();
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        if (CommandName.ToUpper().Equals("GO"))
        {
            string vesseList = GetCsvValue(ddlVessel);
            string flletList = GetCsvValue(ddlFleet);
            string grouprankList = GetCsvValue(ddlGroupRank);
            string vesselTypeList = GetCsvValue(ddlVeselType);
            byte viewall = byte.Parse(chkViewAll.Checked == true ? "1" : "0");
            string ZoneList = GetCsvValue(ddlzone);
            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            nvc["VesselList"] = vesseList;
            nvc["FleetList"] = flletList;
            nvc["Owner"] = ddlOwner.SelectedValue;
            nvc["VesselTypeList"] = vesselTypeList;
            nvc["GroupRankList"] = grouprankList;
            nvc["ZoneList"] = ZoneList;
            FilterDashboard.OfficeDashboardFilterCriteria = nvc;
            PhoenixDashboardOption.UservesselListFilterUpdate(General.GetNullableString(vesseList), General.GetNullableString(flletList), ddlOwner.SelectedValue, vesselTypeList, General.GetNullableString(grouprankList), byte.Parse(ViewState["type"].ToString()), viewall, General.GetNullableString(ZoneList));

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "closeTelerikWindow('filter', null, true);";
            Script += "</script>" + "\n";
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "closeTelerikWindow('filter');";
            Script += "</script>" + "\n";
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            nvc["VesselList"] = string.Empty;
            nvc["FleetList"] = string.Empty;
            nvc["Owner"] = string.Empty;
            nvc["VesselTypeList"] = string.Empty;
            nvc["GroupRankList"] = string.Empty;
            nvc["ZoneList"] = string.Empty;
            chkViewAll.Checked = false;
            FilterDashboard.OfficeDashboardFilterCriteria = nvc;
            PhoenixDashboardOption.UservesselListFilterUpdate(null, null, null, null, null, byte.Parse(ViewState["type"].ToString()), 0, null);
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "closeTelerikWindow('filter', null, true);";
            Script += "</script>" + "\n";
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    private void BindFilterVessel()
    {
        DataTable dt = PhoenixDashboardOption.UservesselListFilterSearch(byte.Parse(ViewState["type"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddlOwner.SelectedValue = dr["FLDOWNERLIST"].ToString().Trim(',');
            if (!string.IsNullOrEmpty(ddlOwner.SelectedValue))
            {
                DataSet ds = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ddlOwner.SelectedValue));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlOwner.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                }
            }
            SetCsvValue(ddlVessel, dr["FLDVESSELLIST"].ToString());
            SetCsvValue(ddlFleet, dr["FLDFLEETLIST"].ToString());
            SetCsvValue(ddlGroupRank, dr["FLDGROUPRANKLIST"].ToString());
            SetCsvValue(ddlVeselType, dr["FLDVESSELTYPELIST"].ToString());
            chkViewAll.Checked = dr["FLDVIEWALL"].ToString().Equals("0") ? false : true;
            SetCsvValue(ddlzone, dr["FLDZONELIST"].ToString());
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

    protected void ddlFleet_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        RadComboBox radComboBox = (RadComboBox)sender;
        string strfleetlist = GetCsvValue(radComboBox);
        SetFilter(null, strfleetlist, null);
        radComboBox.OpenDropDownOnLoad = true;
        ddlVeselType.OpenDropDownOnLoad = false;
    }

    protected void ddlOwner_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlMultiColumnAddress address = (UserControlMultiColumnAddress)sender;
        SetFilter(address.SelectedValue, null, null);
    }

    protected void ddlVeselType_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        RadComboBox radComboBox = (RadComboBox)sender;
        string csvList = GetCsvValue(radComboBox);
        SetFilter(null, null, csvList);
        ddlFleet.OpenDropDownOnLoad = false;
        radComboBox.OpenDropDownOnLoad = true;
    }
    private void SetFilter(string owner, string fleet, string vesseltype)
    {
        string techfleet = string.Empty;
        string crewfleet = string.Empty;
        string accountfleet = string.Empty;
        if (ViewState["type"].ToString().Equals("1"))
        {
            techfleet = fleet;
        }
        if (ViewState["type"].ToString().Equals("2"))
        {
            crewfleet = fleet;
        }
        if (ViewState["type"].ToString().Equals("3"))
        {
            accountfleet = fleet;
        }
        DataSet ds = PhoenixDashboardOption.DashboarVesselList(techfleet, crewfleet, accountfleet, vesseltype, owner);

        foreach (RadComboBoxItem item in ddlVessel.Items)
            item.Checked = false;

        foreach (RadComboBoxItem item in ddlFleet.Items)
            item.Checked = false;

        foreach (RadComboBoxItem item in ddlVeselType.Items)
            item.Checked = false;

        if ((fleet != null && fleet.Trim().Equals("")) || (owner != null && owner.Trim().Equals(""))
            || (vesseltype != null && vesseltype.Trim().Equals(""))) return;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                RadComboBoxItem item = ddlVessel.FindItemByValue(dr["FLDVESSELID"].ToString());
                if (item != null) item.Checked = true;
            }
            DataView view = new DataView(dt);
            DataTable vslType = view.ToTable(true, "FLDTYPE");
            foreach (DataRow dr in vslType.Rows)
            {
                RadComboBoxItem item = ddlVeselType.FindItemByValue(dr["FLDTYPE"].ToString());
                if (item != null) item.Checked = true;
            }
            view = new DataView(dt);
            string field = "FLDTECHFLEET";

            if (ViewState["type"].ToString().Equals("2"))
                field = "FLDFLEET";
            else if (ViewState["type"].ToString().Equals("3"))
                field = "FLDACCOUNTFLEET";
            DataTable flt = view.ToTable(true, field);
            foreach (DataRow dr in flt.Rows)
            {
                RadComboBoxItem item = ddlFleet.FindItemByValue(dr[field].ToString());
                if (item != null) item.Checked = true;
            }
        }
    }

    protected void chkViewAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkViewAll.Checked == true)
        {
            ddlOwner.Enabled = false;
            ddlFleet.Enabled = false;
            ddlVeselType.Enabled = false;
            ddlVessel.Enabled = false;
            ddlGroupRank.Enabled = false;
        }
        else
        {
            ddlOwner.Enabled = true;
            ddlFleet.Enabled = true;
            ddlVeselType.Enabled = true;
            ddlVessel.Enabled = true;
            ddlGroupRank.Enabled = true;
        }
    }
}



