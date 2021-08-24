using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewReportWorkingGearSummary : PhoenixBasePage
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
            MenuSummary.AccessRights = this.ViewState;
            MenuSummary.MenuList = toolbar.Show();
            MenuSummary.SelectedMenuIndex = 1;

            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGearSummary')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearSummary.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportWorkingGearSummary.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuSummaryItems.AccessRights = this.ViewState;
            MenuSummaryItems.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
             
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                for (int i = 2005; i <= DateTime.Now.Year; i++)
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

                gvWorkingGearSummary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSummary_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SUMMARY"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindReport();
            gvWorkingGearSummary.Rebind();   
        }
        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            Response.Redirect("../Crew/CrewReportWorkingGearDetail.aspx?zoneid=" + ucZone.SelectedZone + "&year=" + ddlYear.SelectedValue + "&month=" + ddlMonth.SelectedValue);
        }
    }

    protected void MenuSummaryItems_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDOPENINGBALANCE", "FLDRECEIPTS", "FLDISSUED", "FLDCLOSINGSTOCK" };
                string[] alCaptions = { "Working Gear Item", "Opening Stock", "Receipts", "Issued", "Closing Stock" };

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

                DataSet ds = PhoenixCrewWorkingGearStockMove.SelectWorkingGearSummary(General.GetNullableInteger(Request.QueryString["zoneid"] == null ? ucZone.SelectedZone : Request.QueryString["zoneid"])
                                                                                     , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                                     , General.GetNullableInteger(ddlYear.SelectedValue)
                                                                                     , sortexpression
                                                                                     , sortdirection
                                                                                     , (int)ViewState["PAGENUMBER"]
                                                                                     , gvWorkingGearSummary.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     );


                General.ShowExcel("Working Gear Summary", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucZone.SelectedZone = "";
                ddlMonth.SelectedValue = "";
                ddlMonth.Text = "";
                ddlYear.SelectedValue = "";
                ddlYear.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvWorkingGearSummary.CurrentPageIndex = 0;
                BindReport();
                gvWorkingGearSummary.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindReport();
                gvWorkingGearSummary.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWorkingGearSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGearSummary.CurrentPageIndex + 1;

        BindReport();
    }

    protected void gvWorkingGearSummary_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDOPENINGBALANCE", "FLDRECEIPTS", "FLDISSUED", "FLDCLOSINGSTOCK" };
            string[] alCaptions = { "Working Gear Item", "Opening Stock", "Receipts", "Issued", "Closing Stock" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearStockMove.SelectWorkingGearSummary(General.GetNullableInteger(Request.QueryString["zoneid"] == null ? ucZone.SelectedZone : Request.QueryString["zoneid"])
                , General.GetNullableInteger(ddlMonth.SelectedValue)
                , General.GetNullableInteger(ddlYear.SelectedValue)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvWorkingGearSummary.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                );

            General.SetPrintOptions("gvWorkingGearSummary", "Working Gear Summary", alCaptions, alColumns, ds);

            gvWorkingGearSummary.DataSource = ds.Tables[0];
            gvWorkingGearSummary.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}