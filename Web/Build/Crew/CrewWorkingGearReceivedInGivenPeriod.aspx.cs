using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewWorkingGearReceivedInGivenPeriod : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkingGearReceivedInGivenPeriod.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGearReceivedInGivenPeriod')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkingGearReceivedInGivenPeriod.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkingGearReceivedInGivenPeriod.aspx", "Clear", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuWorkingGearReceivedInGivenPeriod.AccessRights = this.ViewState;
            MenuWorkingGearReceivedInGivenPeriod.MenuList = toolbargrid.Show();


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Working Gear Detail", "WORKINGGEARDETAIL", ToolBarDirection.Right);
            toolbar.AddButton("Receipt", "RECEIPT", ToolBarDirection.Right);
            toolbar.AddButton("Monthly Stock", "MONTHLYSTOCK", ToolBarDirection.Right);


            MenuCrewWorkingGear.AccessRights = this.ViewState;
            MenuCrewWorkingGear.MenuList = toolbar.Show();
            MenuCrewWorkingGear.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkingGearReceivedInGivenPeriod.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void Rebind()
    {
        gvWorkingGearReceivedInGivenPeriod.SelectedIndexes.Clear();
        gvWorkingGearReceivedInGivenPeriod.EditIndexes.Clear();
        gvWorkingGearReceivedInGivenPeriod.DataSource = null;
        gvWorkingGearReceivedInGivenPeriod.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSUPPLIERNAME", "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDQUANTITY", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Supplier Name", "Item Name", "Size", "Quantity", "Received Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearReports.WorkingGearReceivedInGivenPeriod(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableDateTime(txtFromDate.Text),
                                                                                General.GetNullableDateTime(txtToDate.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Received In Given Period", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void WorkingGearReceivedInGivenPeriod_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewWorkingGear_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("MONTHLYSTOCK"))
            {
                Response.Redirect("CrewWorkingGearStockPerMonth.aspx", true);
                MenuCrewWorkingGear.SelectedMenuIndex = 2;
            }
            if (CommandName.ToUpper().Equals("RECEIPT"))
            {
                Response.Redirect("CrewWorkingGearReceivedInGivenPeriod.aspx", true);
                MenuCrewWorkingGear.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("WORKINGGEARDETAIL"))
            {
                Response.Redirect("CrewWorkingGearIssuedPerMonth.aspx", true);
                MenuCrewWorkingGear.SelectedMenuIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUPPLIERNAME", "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDQUANTITY", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Supplier Name", "Item Name", "Size", "Quantity", "Received Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewWorkingGearReports.WorkingGearReceivedInGivenPeriod(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableDateTime(txtFromDate.Text),
                                                                                General.GetNullableDateTime(txtToDate.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvWorkingGearReceivedInGivenPeriod.PageSize,
                                                                                ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvWorkingGearReceivedInGivenPeriod", "Working Gear Received In Given Period", alCaptions, alColumns, ds);

        gvWorkingGearReceivedInGivenPeriod.DataSource = ds;
        gvWorkingGearReceivedInGivenPeriod.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvWorkingGearReceivedInGivenPeriod_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
        }

    }

    protected void gvWorkingGearReceivedInGivenPeriod_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvWorkingGearReceivedInGivenPeriod_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGearReceivedInGivenPeriod.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvWorkingGearReceivedInGivenPeriod_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
