using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccounts_VesselAccountsVesselProvision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsVesselProvision.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVesselProvision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsVesselProvision.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsVesselProvision.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuVesselAccountsVesselProvision.AccessRights = this.ViewState;
            MenuVesselAccountsVesselProvision.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["YEAR"] = DateTime.Today.Year.ToString();
                ddlYear.SelectedYear = DateTime.Today.Year;
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ViewState["MONTH"] = DateTime.Today.Month.ToString();


                gvVesselProvision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselAccountsVesselProvision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDVESSELNAME", "FLDD11OPENING", "FLDPROVISIONPURCHASE", "FLDPROVISIONDELIVERY", "FLDD11CONSUMPTION", "FLDCLOSINGAMOUNT", "FLDVICTUALLINGRATE", "FLDINVOPENING", "FLDINVPROVISION", "FLDINVCONSUMPTION", "FLDINVCLOSING" };
                string[] alCaptions = { "Vessel", "D11 Opening", "D11 Purchase", "D11 Delivery", "D11 Consumption", "D11 Closing", "Victualling Rate", "Inventory Opening", "Inventory Purchase", "Inventory Consumption", "Inventory Closing" };

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

                DataSet ds = PhoenixVesselAccountsProvision.VesselProvisionSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
               , General.GetNullableInteger(ddlMonth.SelectedMonth)
               , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvVesselProvision.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );


                General.ShowExcel("Vessel Provision", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = "";
                ddlYear.SelectedYear = DateTime.Today.Year;
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ViewState["PAGENUMBER"] = 1;
                gvVesselProvision.CurrentPageIndex = 0;
                gvVesselProvision.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindReport();
                gvVesselProvision.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselProvision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselProvision.CurrentPageIndex + 1;

        BindReport();
    }
    protected void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDD11OPENING", "FLDPROVISIONPURCHASE", "FLDPROVISIONDELIVERY", "FLDD11CONSUMPTION", "FLDCLOSINGAMOUNT", "FLDVICTUALLINGRATE", "FLDINVOPENING", "FLDINVPROVISION", "FLDINVCONSUMPTION", "FLDINVCLOSING" };
            string[] alCaptions = { "Vessel", "D11 Opening", "D11 Purchase", "D11 Delivery", "D11 Consumption", "D11 Closing", "Victualling Rate", "Inventory Opening", "Inventory Purchase", "Inventory Consumption", "Inventory Closing" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsProvision.VesselProvisionSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                              , General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                                              , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                              , sortexpression
                                                                              , sortdirection
                                                                              , (int)ViewState["PAGENUMBER"]
                                                                              , gvVesselProvision.PageSize
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount
                                                                                        );

            General.SetPrintOptions("gvVesselProvision", "Vessel Provision", alCaptions, alColumns, ds);

            gvVesselProvision.DataSource = ds.Tables[0];
            gvVesselProvision.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselProvision_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {

            RadLabel Vessel = (RadLabel)e.Item.FindControl("lblVessel");

            if (Vessel != null)
            {
                if (drv["FLDD11CONSUMTIONYN"].ToString() == "0")
                {
                    Vessel.Attributes.Add("style", "color:red !important");
                }
            }
        }
    }//FLDD11CONSUMTIONYN

    protected void gvVesselProvision_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "REFRESH")
            {

                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
                PhoenixVesselAccountsProvision.UpdateBulkProvisionreport(int.Parse(lblvesselid.Text), int.Parse(ddlMonth.SelectedMonth), int.Parse(ddlYear.SelectedYear.ToString()));
                BindReport();
                gvVesselProvision.Rebind();
            }
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

    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedMonth;
        gvVesselProvision.Rebind();
    }

    protected void ddlYear_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["YEAR"] = ddlYear.SelectedYear.ToString();
        gvVesselProvision.Rebind();
    }
}