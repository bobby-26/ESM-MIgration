using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;


public partial class OwnerReportTechnicalJobPlanned : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Owners/OwnerReportTechnicalJobPlanned.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
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
            //int iRowCount = 0;
            //int iTotalPageCount = 0;
            //string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            //string[] alCaptions = { "Work Order No", "Category", "Planned Date", "Duration", "Assigned To", "Status" };
            //string sortexpression;
            //int? sortdirection = null;

            //sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            //    iRowCount = 10;
            //else
            //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            //DataTable dt = new DataTable();

            //General.ShowExcel("Work Orders", dt, alColumns, alCaptions, sortdirection, sortexpression);

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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration", "Assigned To", "Status" };

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

            DataTable dt = PhoenixOwnerReportPMS.PMSJobPlannedSearch(ViewState["CODE"].ToString()
                                        , sortexpression, sortdirection
                                        , gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize
                                        , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
                                        , General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

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
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 10, 0, "DFT,ISS,POP,IPG");
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDHARDCODE";

        status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
}