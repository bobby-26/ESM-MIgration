using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewWorkingGearIssuedPerMonth : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearIssuedPerMonth.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGearIssuedPerMonth')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearIssuedPerMonth.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearIssuedPerMonth.aspx", "Clear", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuWorkingGearIssuedPerMonth.AccessRights = this.ViewState;
            MenuWorkingGearIssuedPerMonth.MenuList = toolbar.Show();
            

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Working Gear Detail", "WORKINGGEARDETAIL", ToolBarDirection.Right);
            toolbar.AddButton("Receipt", "RECEIPT", ToolBarDirection.Right);
            toolbar.AddButton("Monthly Stock", "MONTHLYSTOCK", ToolBarDirection.Right);
            MenuCrewWorkingGear.AccessRights = this.ViewState;
            MenuCrewWorkingGear.MenuList = toolbar.Show();
            MenuCrewWorkingGear.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkingGearIssuedPerMonth.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvWorkingGearIssuedPerMonth.SelectedIndexes.Clear();
        gvWorkingGearIssuedPerMonth.EditIndexes.Clear();
        gvWorkingGearIssuedPerMonth.DataSource = null;
        gvWorkingGearIssuedPerMonth.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDQUANTITY", "FLDISSUEDATE" };
        string[] alCaptions = { "Name","Rank","Item Name", "Size", "Quantity Issued", "Issued Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearReports.WorkingGearIssuedPerMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableInteger(ddlMonth.SelectedValue),
                                                                                General.GetNullableInteger(txtYear.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Issued Per Month", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
    protected void WorkingGearIssuedPerMonth_TabStripCommand(object sender, EventArgs e)
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDWORKINGGEARITEMNAME", "FLDSIZENAME", "FLDQUANTITY", "FLDISSUEDATE" };
        string[] alCaptions = { "Name", "Rank", "Item Name", "Size", "Quantity Issued", "Issued Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewWorkingGearReports.WorkingGearIssuedPerMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableInteger(ddlMonth.SelectedValue),
                                                                                General.GetNullableInteger(txtYear.Text),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvWorkingGearIssuedPerMonth.PageSize,
                                                                                ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvWorkingGearIssuedPerMonth", "Working Gear Issued Per Month", alCaptions, alColumns, ds);

        gvWorkingGearIssuedPerMonth.DataSource = ds;
        gvWorkingGearIssuedPerMonth.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvWorkingGearIssuedPerMonth_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
        }
        
    }
    protected void gvWorkingGearIssuedPerMonth_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvWorkingGearIssuedPerMonth_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGearIssuedPerMonth.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvWorkingGearIssuedPerMonth_ItemCommand(object sender, GridCommandEventArgs e)
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
