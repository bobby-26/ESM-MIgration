using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalJobPlanned : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJobPlanned.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJobPlanned.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CODE"] = string.Empty;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["filterJobCategory"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                if (Request.QueryString["code"] != null)
                {
                    ViewState["CODE"] = Request.QueryString["code"];
                }
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Vessel", "Work Order No", "Title", "Category", "Planned Date", "Duration", "Assigned To", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc["VesselList"] = string.Empty;
                nvc["FleetList"] = string.Empty;
                nvc["Owner"] = string.Empty;
                nvc["VesselTypeList"] = string.Empty;
                nvc["RankList"] = string.Empty;
            }
            string vesselId = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataTable dt = PhoenixDashboardTechnical.DashboardPMSJobPlannedSearch(ViewState["CODE"].ToString()
                                        , General.GetNullableString(nvc["VesselList"])
                                        , General.GetNullableString(nvc["VesselTypeList"])
                                        , General.GetNullableString(nvc["FleetList"])
                                        , General.GetNullableInteger(nvc["Owner"])
                                        , General.GetNullableInteger(ViewState["filterDiscipline"].ToString())
                                        , General.GetNullableString(ViewState["filterStatus"].ToString())
                                        , ViewState["filterJobCategory"].ToString()
                                        , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                        , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                        , sortexpression, sortdirection
                                        , 1, iRowCount
                                        , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(vesselId));
            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Work Orders";
            }
            General.ShowExcel(heading, dt, alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Vessel", "Work Order No", "Title", "Category", "Planned Date", "Duration", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc["VesselList"] = string.Empty;
                nvc["FleetList"] = string.Empty;
                nvc["Owner"] = string.Empty;
                nvc["VesselTypeList"] = string.Empty;
                nvc["RankList"] = string.Empty;
            }
            string vesselId = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataTable dt = PhoenixDashboardTechnical.DashboardPMSJobPlannedSearch(ViewState["CODE"].ToString()
                                        , General.GetNullableString(nvc["VesselList"])
                                        , General.GetNullableString(nvc["VesselTypeList"])
                                        , General.GetNullableString(nvc["FleetList"])
                                        , General.GetNullableInteger(nvc["Owner"])
                                        , General.GetNullableInteger(ViewState["filterDiscipline"].ToString())
                                        , General.GetNullableString(ViewState["filterStatus"].ToString())
                                        , ViewState["filterJobCategory"].ToString()
                                        , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                        , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                        , sortexpression, sortdirection
                                        , gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize
                                        , ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(vesselId));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Work Orders";
            }
            General.SetPrintOptions("gvWorkOrder", heading, alCaptions, alColumns, ds);
            if(General.GetNullableInteger(vesselId).HasValue && General.GetNullableInteger(vesselId).Value > 0)
            {
                gvWorkOrder.Columns.FindByUniqueName("FLDVESSELNAME").Visible = false;
            }
            gvWorkOrder.DataSource = dt;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
           if (e.CommandName == RadGrid.FilterCommandName)
            {
                ViewState["PAGENUMBER"] = "1";
                ViewState["filterDiscipline"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["filterStatus"] = gvWorkOrder.MasterTableView.GetColumn("FLDSTATUSCODE").CurrentFilterValue;                
                ViewState["filterJobCategory"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue;
                string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue;
                string[] dates = daterange.Split('~');
                ViewState["FDATE"] = (dates.Length > 0 ? dates[0] : string.Empty);
                ViewState["TDATE"] = (dates.Length > 1 ? dates[1] : string.Empty);
                gvWorkOrder.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = "1";
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["filterJobCategory"] = string.Empty;
                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox discipline = sender as RadComboBox;
        discipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        discipline.DataTextField = "FLDDISCIPLINENAME";
        discipline.DataValueField = "FLDDISCIPLINEID";

        discipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        string code = "DFT,ISS,POP,IPG";
        if (ViewState["CODE"].ToString() == "TECH-PMS-WOVRQ")
            code = "SVP,CVR";
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 10, 0, code);
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDHARDCODE";

        status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "&vslid="+ drv["FLDVESSELID"] + "'); return false;");
                }
            }
        }
    }
}
