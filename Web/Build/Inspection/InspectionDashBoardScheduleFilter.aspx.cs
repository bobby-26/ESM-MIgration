using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;

public partial class InspectionDashBoardScheduleFilter : PhoenixBasePage
{
     protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionScheduleMaster.aspx", false);
        }
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            ViewState["Filter"] = null;

            VesselConfiguration();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            BindVesselFleetList();
            BindVesselList();
            BindAttendingSupt();
            
            GetInspectionCompanyList();
            Bind_UserControls(sender, new EventArgs());
        }
    }

    protected void ucAddrOwner_Changed(object sender, EventArgs e)
    {
        
    }
    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

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
            criteria.Add("ucInspectionType", PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS"));
            criteria.Add("ucInspectionCategory", ucInspectionCategory.SelectedHard);
            criteria.Add("ucInspection", ddlInspection.SelectedValue);
            criteria.Add("ucPort", ucPort.SelectedValue);
            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ddlCompany", ddlCompany.SelectedValue);
            criteria.Add("txtInspector", txtInspector.Text);
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("ddlAttendingSupt", ddlAttendingSupt.SelectedValue);
            criteria.Add("ucChapter", ucChapter.SelectedChapter);
            criteria.Add("Isrejected", chkRejection.Checked == true ? "1" : "0");


            InspectionFilter.CurrentVettingSIRELOGDashboardFilter = criteria;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
    protected void GetInspectionCompanyList()
    {
        DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindInspection()
    {
        ucInspection.InspectionType = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ucInspection.InspectionList = PhoenixInspection.ListInspection(
                                       General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS"))
                                       , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                       , null
                                       );
        ucInspection.DataBind();
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ddlInspection_Changed(object sender, EventArgs e)
    {
        BindChapter();
    }

    protected void BindChapter()
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "INS")),
            General.GetNullableInteger(ucInspectionCategory.SelectedHard),
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }

    protected void BindAttendingSupt()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlAttendingSupt.DataSource = ds;
        ddlAttendingSupt.DataTextField = "FLDDESIGNATIONNAME";
        ddlAttendingSupt.DataValueField = "FLDUSERCODE";
        ddlAttendingSupt.DataBind();
        ddlAttendingSupt.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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