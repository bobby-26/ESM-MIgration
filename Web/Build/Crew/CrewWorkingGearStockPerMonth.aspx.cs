using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class CrewWorkingGearStockPerMonth : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)

    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearStockPerMonth.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGearItemStockPerMonth')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearStockPerMonth.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearStockPerMonth.aspx", "Clear", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuWorkingGearItemStock.AccessRights = this.ViewState;
            MenuWorkingGearItemStock.MenuList = toolbar.Show();


            toolbar = new PhoenixToolbar();

            toolbar.AddButton("Working Gear Detail", "WORKINGGEARDETAIL", ToolBarDirection.Right);
            toolbar.AddButton("Receipt", "RECEIPT", ToolBarDirection.Right);
            toolbar.AddButton("Monthly Stock", "MONTHLYSTOCK", ToolBarDirection.Right);

            MenuCrewWorkingGear.AccessRights = this.ViewState;
            MenuCrewWorkingGear.MenuList = toolbar.Show();
            MenuCrewWorkingGear.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkingGearItemStockPerMonth.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvWorkingGearItemStockPerMonth.SelectedIndexes.Clear();
        gvWorkingGearItemStockPerMonth.EditIndexes.Clear();
        gvWorkingGearItemStockPerMonth.DataSource = null;
        gvWorkingGearItemStockPerMonth.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDZONE", "FLDUNITNAME", "FLDOPENING", "FLDRECEIVED", "FLDISSUED", "FLDCLOSING" };
        string[] alCaptions = { "Item Name", "Item Size", "Zone", "Unit", "Opening Stock", "Received", "Issued", "Closing" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearReports.WorkingGearStockPerMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableInteger(ucZone.SelectedZone),
                                                                                General.GetNullableInteger(ddlMonth.SelectedValue),
                                                                                General.GetNullableInteger(txtYear.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Item Stock Per Month", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void WorkingGearItemStock_TabStripCommand(object sender, EventArgs e)
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
                ucZone.SelectedZone = "";
                ddlMonth.SelectedIndex = -1;
                txtYear.Text = "";
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

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDZONE", "FLDUNITNAME", "FLDOPENING", "FLDRECEIVED", "FLDISSUED", "FLDCLOSING" };
        string[] alCaptions = { "Item Name", "Item Size", "Zone", "Unit", "Opening Stock", "Received", "Issued", "Closing" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewWorkingGearReports.WorkingGearStockPerMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableInteger(ucZone.SelectedZone),
                                                                                General.GetNullableInteger(ddlMonth.SelectedValue),
                                                                                General.GetNullableInteger(txtYear.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvWorkingGearItemStockPerMonth.PageSize,
                                                                                ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvWorkingGearItemStockPerMonth", "Working Gear Item Stock Per Month", alCaptions, alColumns, ds);

        gvWorkingGearItemStockPerMonth.DataSource = ds;
        gvWorkingGearItemStockPerMonth.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvWorkingGearItemStockPerMonth_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
        }

    }

    protected void gvWorkingGearItemStockPerMonth_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvWorkingGearItemStockPerMonth_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGearItemStockPerMonth.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvWorkingGearItemStockPerMonth_ItemCommand(object sender, GridCommandEventArgs e)
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
