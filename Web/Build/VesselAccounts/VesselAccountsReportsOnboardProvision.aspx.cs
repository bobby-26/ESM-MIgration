using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Web;
public partial class VesselAccountsReportsOnboardProvision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../VesselAccounts/VesselAccountsReportsOnboardProvision.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvProvision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../VesselAccounts/VesselAccountsReportsOnboardProvision.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvProvision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                BindYear();
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
        gvProvision.SelectedIndexes.Clear();
        gvProvision.EditIndexes.Clear();
        gvProvision.DataSource = null;
        gvProvision.Rebind();
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDMONTHYEAR", "FLDBROUGHTFORWARD", "FLDPURCHASE", "FLDTOTAL", "FLDCONSUMPTION", "FLDCARRIEDFORWARD", "FLDMANDAYS", "FLDVICTRATE" };
        string[] alCaptions = { "Vessel Name", "Month/Year", "Brought Forward", "Purchase", "Total", "Consumption", "Carried Forward", "Mandays", "Rate" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixVesselAccountsOnboardProvision.SearchVesselOnboardProvision((ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
            , (ucVesselList.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVesselList.SelectedVessel.ToString())
            , null
            , General.GetNullableInteger(ucPrincipal.SelectedAddress)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ddlYear.SelectedValue)
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvProvision.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvProvision", "Vessel Onboard Provision", alCaptions, alColumns, ds);

        gvProvision.DataSource = ds;
        gvProvision.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ucVesselList.SelectedVessel = "";
                ucVesselType.SelectedVesseltype = "";
                ucPrincipal.SelectedAddress = "";
                ddlMonth.SelectedValue = "DUMMY";
                ucNavigationArea.SelectedQuick = "";

                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDMONTHYEAR", "FLDBROUGHTFORWARD", "FLDPURCHASE", "FLDTOTAL", "FLDCONSUMPTION", "FLDCARRIEDFORWARD", "FLDMANDAYS", "FLDVICTRATE" };
        string[] alCaptions = { "Vessel Name", "Month/Year", "Brought Forward", "Purchase", "Total", "Consumption", "Carried Forward", "Mandays", "Rate" };

        //string[] FilterColumns = { "FLDSELECTEDVESSEL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDMANAGER", "FLDSELECTEDRANK", "FLDSELECTEDPOOL", "FLDSELECTEDZONE", "FLDSELECTEDSTATUS", "FLDFROMDATE", "FLDTODATE" };
        //string[] FilterCaptions = { "Vessel List", "Vessel Type", "Manager", "Rank", "Pool", "Zone", "Feedback Status", "FromDate", "ToDate" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselAccountsOnboardProvision.SearchVesselOnboardProvision((ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
          , (ucVesselList.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVesselList.SelectedVessel.ToString())
          , null
          , General.GetNullableInteger(ucPrincipal.SelectedAddress)
          , General.GetNullableInteger(ddlMonth.SelectedValue)
          , General.GetNullableInteger(ddlYear.SelectedValue)
          , sortexpression, sortdirection
          , Int32.Parse(ViewState["PAGENUMBER"].ToString())
          , General.ShowRecords(null)
          , ref iRowCount
          , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=VesselOnboardProvision.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Vessel Onboard Provision</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }





    protected void gvProvision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }
    protected void gvProvision_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvProvision_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvProvision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProvision.CurrentPageIndex + 1;

        ShowReport();
    }
    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (General.GetNullableString(ucVesselList.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel required";
        }
        if (General.GetNullableString(ddlMonth.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Month required";
        }
        return (!ucError.IsError);

    }
}


