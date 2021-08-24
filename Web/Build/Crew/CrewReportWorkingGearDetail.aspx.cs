using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewReportWorkingGearDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbar.AddButton("Detail", "DETAIL");
            toolbar.AddButton("Summary", "SUMMARY");
            MenuWorkingGear.AccessRights = this.ViewState;
            MenuWorkingGear.MenuList = toolbar.Show();
            MenuWorkingGear.SelectedMenuIndex = 0;


            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
            toolbar1.AddButton("Lock", "LOCK", ToolBarDirection.Right);
            CrewWorkGearLock.AccessRights = this.ViewState;
            CrewWorkGearLock.MenuList = toolbar1.Show();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGear')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearDetail.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearDetail.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuWorkingGearItem.AccessRights = this.ViewState;
            MenuWorkingGearItem.MenuList = toolbargrid.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                for (int i = 2010; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                if (Request.QueryString["zoneid"] != null && Request.QueryString["zoneid"] != "" && Request.QueryString["zoneid"] != "Dummy")
                {
                    ucZone.SelectedZone = Request.QueryString["zoneid"];
                }

                if (Request.QueryString["year"] != null && Request.QueryString["year"] != "")
                {
                    ddlYear.SelectedValue = Request.QueryString["year"];
                }

                if (Request.QueryString["month"] != null && Request.QueryString["month"] != "")
                {
                    ddlMonth.SelectedValue = Request.QueryString["month"];
                }

                gvWorkingGear.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void MenuWorkingGear_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SUMMARY"))
        {
            Response.Redirect("../Crew/CrewReportWorkingGearSummary.aspx?zoneid=" + ucZone.SelectedZone + "&year=" + ddlYear.SelectedValue + "&month=" + ddlMonth.SelectedValue);
        }
        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindReport();
            gvWorkingGear.Rebind();

        }
    }
    
    protected void CrewWorkGearLock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LOCK"))
            {
                PhoenixCrewWorkingGearStockMove.WorkingGearSummaryLock(General.GetNullableInteger(ucZone.SelectedZone)
                , General.GetNullableInteger(ddlMonth.SelectedValue)
                , General.GetNullableInteger(ddlYear.SelectedValue)

                );
                ucStatus.Text = "Working Gear transaction Locked.";
                ucStatus.Visible = true;
                BindReport();
                gvWorkingGear.Rebind();
            }
            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                PhoenixCrewWorkingGearStockMove.WorkingGearSummaryUnLock(General.GetNullableInteger(ucZone.SelectedZone)
                , General.GetNullableInteger(ddlMonth.SelectedValue)
                , General.GetNullableInteger(ddlYear.SelectedValue)

                );
                ucStatus.Text = "Working Gear transaction Unlocked.";
                ucStatus.Visible = true;
                BindReport();
                gvWorkingGear.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDRANKCODE", "FLDWORKINGGEARITEMNAME", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
                string[] alCaptions = { "File No.", "Employee", "Vessel", "Rank", "Working Gear Item", "Issued Quantity", "Issue Date" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewWorkingGearStockMove.SelectWorkingGearSummaryDetail(General.GetNullableInteger(Request.QueryString["zoneid"] == null ? ucZone.SelectedZone : Request.QueryString["zoneid"])
               , General.GetNullableInteger(ddlMonth.SelectedValue)
               , General.GetNullableInteger(ddlYear.SelectedValue)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvWorkingGear.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );


                General.ShowExcel("Working Gear Detail", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucZone.SelectedZone = "";
                ddlMonth.SelectedValue = "";
                ddlMonth.Text = "";
                ddlYear.SelectedValue = "";
                ddlYear.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvWorkingGear.CurrentPageIndex = 0;
                BindReport();
                gvWorkingGear.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindReport();
                gvWorkingGear.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkingGear_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGear.CurrentPageIndex + 1;

        BindReport();
    }


    protected void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDRANKCODE", "FLDWORKINGGEARITEMNAME", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "File No.", "Employee", "Vessel", "Rank", "Working Gear Item", "Issued Quantity", "Issue Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearStockMove.SelectWorkingGearSummaryDetail(General.GetNullableInteger(Request.QueryString["zoneid"] == null ? ucZone.SelectedZone : Request.QueryString["zoneid"])
                                                                                        , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                                        , General.GetNullableInteger(ddlYear.SelectedValue)
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , (int)ViewState["PAGENUMBER"]
                                                                                        , gvWorkingGear.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount
                                                                                        );

            General.SetPrintOptions("gvWorkingGear", "Working Gear Detail", alCaptions, alColumns, ds);

            gvWorkingGear.DataSource = ds.Tables[0];
            gvWorkingGear.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWorkingGear_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvWorkingGear_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {

    }
    
}