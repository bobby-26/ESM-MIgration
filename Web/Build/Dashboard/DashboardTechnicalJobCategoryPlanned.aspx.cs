using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardTechnicalJobCategoryPlanned : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJobCategoryPlanned.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);
            MenuWorkOrder.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["NUMBER"] = string.Empty;
                ViewState["NAME"] = string.Empty;
                if (Request.QueryString["td"] != null)
                {
                    ViewState["TDATE"] = Request.QueryString["td"];
                }
                ViewState["JobCategoryFilter"] = string.Empty;
                ViewState["PLANID"] = string.Empty;
                if (Request.QueryString["p"] != null)
                {
                    ViewState["PLANID"] = Request.QueryString["p"];
                }
                ViewState["SELMODE"] = string.Empty;
                if(Request.QueryString["sm"] != null)
                {
                    ViewState["SELMODE"] = Request.QueryString["sm"];
                }
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (ViewState["SELMODE"].ToString().ToUpper() == "SINGLE")
                {
                    gvWorkOrder.AllowMultiRowSelection = false;
                }
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No", "Category", "Planned Date", "Duration", "Assigned To", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = PhoenixDashboardTechnical.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                        General.GetNullableInteger(ViewState["filterDiscipline"].ToString()),
                                        General.GetNullableString(ViewState["filterStatus"].ToString()),
                                        ViewState["JobCategoryFilter"].ToString(),
                                        General.GetNullableDateTime(ViewState["FDATE"].ToString()),
                                        General.GetNullableDateTime(ViewState["TDATE"].ToString()),
                                        sortexpression, sortdirection,
                                        1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount
                                        , ViewState["NUMBER"].ToString()
                                        , ViewState["NAME"].ToString());

            General.ShowExcel("Work Orders", dt, alColumns, alCaptions, sortdirection, sortexpression);

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

            DataTable dt = PhoenixDashboardTechnical.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                        General.GetNullableInteger(ViewState["filterDiscipline"].ToString()),
                                        General.GetNullableString(ViewState["filterStatus"].ToString()),
                                        ViewState["JobCategoryFilter"].ToString(),
                                        General.GetNullableDateTime(ViewState["FDATE"].ToString()),
                                        General.GetNullableDateTime(ViewState["TDATE"].ToString()),
                                        sortexpression, sortdirection,
                                         int.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount
                                         , ViewState["NUMBER"].ToString()
                                         , ViewState["NAME"].ToString());

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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
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
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                ViewState["PAGENUMBER"] = "1";
                ViewState["NUMBER"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue;
                ViewState["NAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["filterDiscipline"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["filterStatus"] = gvWorkOrder.MasterTableView.GetColumn("FLDSTATUSCODE").CurrentFilterValue;                
                ViewState["JobCategoryFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue;
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
    private string GetSelectedWorkOrderList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.SelectedItems)
            {
                if (ViewState["SELMODE"].ToString().ToUpper() != "SINGLE")
                    strlist.Append(gv.GetDataKeyValue("FLDWORKORDERGROUPID") + ", ");
                else
                {
                    string name = ((RadLabel)gv.FindControl("lblWorkOrderName")).Text;
                    strlist.Append(gv.GetDataKeyValue("FLDWORKORDERGROUPID") + "~" + name + ", ");
                }            
            }
        }
        return strlist.ToString();       
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                string script = string.Empty;
                string csvWorkOrder = GetSelectedWorkOrderList();
                if (csvWorkOrder.Trim().Equals(""))
                {
                    ucError.ErrorMessage = "Select atleast one job";
                    ucError.Visible = true;
                    return;
                }
                csvWorkOrder = csvWorkOrder.Trim().Trim(',');
                if (ViewState["SELMODE"].ToString().ToUpper() != "SINGLE")
                {               
                    PhoenixPlannedMaintenanceDailyWorkPlan.InsertWorkOrder(General.GetNullableGuid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, csvWorkOrder);
                    script = "function sd(){CloseModelWindow(); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                }
                else
                {
                    script = "function sd(){CloseModelWindow('"+ csvWorkOrder + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = "1";
                ViewState["NUMBER"] = string.Empty;
                ViewState["NAME"] = string.Empty;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["JobCategoryFilter"] = string.Empty;
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
