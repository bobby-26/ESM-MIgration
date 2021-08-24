using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Registers_RegisterAccountsFleet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Registers/RegisterAccountsFleet.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAccountsFleet')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuAccountsFleetList.AccessRights = this.ViewState;
            MenuAccountsFleetList.MenuList = toolbargrid.Show();

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["fid"] != null)
                {
                    ViewState["fid"] = Request.QueryString["fid"].ToString();
                }
                if (Request.QueryString["FleetName"] != null)
                {
                    ViewState["FleetName"] = Request.QueryString["FleetName"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvAccountsFleet.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuGeneral.Title = "Accounts Fleet -  " + ViewState["FleetName"] + " ";
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAccountsFleetList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDPRINCIPALNAME", "FLDCOUNTRYNAME", "FLDACCOUNTADMINNAME", "FLDACCOUNTSEXECUTIVENAME" };
                string[] alCaptions = { "Vessel", "Vessel Type ", "Principal", "Flag", "Account Administrator", "Accounts Executive" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string fleetid;
                fleetid = (ViewState["fid"] == null) ? null : (ViewState["fid"].ToString());

                NameValueCollection nvc = Filter.CurrentFleetFilter;

                DataSet ds = PhoenixRegistersFleet.AccountFleetSearch(General.GetNullableInteger(fleetid)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucFlag"] : string.Empty)
                                                                    , sortexpression, sortdirection,
                                                                    1,
                                                                    gvAccountsFleet.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel(MenuGeneral.Title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDPRINCIPALNAME", "FLDCOUNTRYNAME", "FLDACCOUNTADMINNAME", "FLDACCOUNTSEXECUTIVENAME" };
        string[] alCaptions = { "Vessel", "Vessel Type ", "Principal", "Flag", "Account Administrator", "Accounts Executive" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        string fleetid;
        fleetid = (ViewState["fid"] == null) ? null : (ViewState["fid"].ToString());

        NameValueCollection nvc = Filter.CurrentFleetFilter;

        DataSet ds = PhoenixRegistersFleet.AccountFleetSearch(General.GetNullableInteger(fleetid)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucFlag"] : string.Empty)
                                                            , sortexpression, sortdirection,
                                                            int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvAccountsFleet.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);

        General.SetPrintOptions("gvAccountsFleet", MenuGeneral.Title, alCaptions, alColumns, ds);

        gvAccountsFleet.DataSource = ds;
        gvAccountsFleet.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvAccountsFleet.SelectedIndexes.Clear();
        gvAccountsFleet.EditIndexes.Clear();
        gvAccountsFleet.DataSource = null;
        gvAccountsFleet.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvAccountsFleet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = gvAccountsFleet.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAccountsFleet_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
}