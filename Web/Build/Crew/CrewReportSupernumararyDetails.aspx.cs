using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportSupernumararyDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Supernumarary Format2", "FORMAT2", ToolBarDirection.Right);
            toolbar1.AddButton("Supernumarary Format1", "FORMAT1", ToolBarDirection.Right);

            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar1.Show();
            MenuReport.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbarSub = new PhoenixToolbar();
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportSupernumararyDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarSub.AddFontAwesomeButton("javascript:CallPrint('gvSupernumarary')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportSupernumararyDetails.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbarSub.Show();

            if (!IsPostBack)
            {
                rblFormats.SelectedIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSupernumarary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvSupernumarary.SelectedIndexes.Clear();
        gvSupernumarary.EditIndexes.Clear();
        gvSupernumarary.DataSource = null;
        gvSupernumarary.Rebind();
    }
    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("CrewReportFamilyNOK.aspx", true);
                MenuReport.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
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
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucVessel.SelectedVessel = "";
                ucZone.selectedlist = "";
                ucfromdate.Text = "";
                uctodate.Text = "";
                ucPool.SelectedPoolValue = "";
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
                if (!IsValidFilter(ucfromdate.Text, uctodate.Text))
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
        string[] alColumns = { "FLDROWNUMBER", "FLDFAMILYNAME", "FLDFAMILYDOB", "FLDRELATION", "FLDFILENO", "FLDEMPLOYEEYNAME", "FLDRANKNAME", "FLDEMPDOB", "FLDVESSELNAME", "FLDFAMILYSIGNONDATE", "FLDFAMILYSIGNONPORT", "FLDFAMILYSIGNOFFDATE", "FLDFAMILYSIGNOFFPORT", "FLDSAILDURATION" };
        string[] alCaptions = { "S.No.", "Family's Name", "Family's DOB", "Relation", "File No", "Seafarer's Name", "Rank", "Seafarer's DOB", "Vessel", "Signon Date", "Sign-on Port", "Signoff Date", "Sign-off Port", "Sail Duration(Months)" };
        string[] filtercolumns = { "FLDSELECTEDPRICIPAL", "FLDSELECTEDVESTYPE", "FLDSELECTEDVESSEL", "FLDSELECTEDZONE", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };
        string[] filtercaptions = { "Principal", "Vessel Type", "Vessel", "Zone", "From Date", "To Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.SupernumararyDetailsReport(
              (ucPrincipal.SelectedAddress) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
              (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
              (ucVessel.SelectedVessel) == "" ? null : General.GetNullableString(ucVessel.SelectedVessel),
              (ucZone.selectedlist) == "" ? null : General.GetNullableString(ucZone.selectedlist),
              (ucPool.SelectedPool) == "" ? null : General.GetNullableString(ucPool.SelectedPool),
              General.GetNullableDateTime(ucfromdate.Text),
              General.GetNullableDateTime(uctodate.Text),
              sortexpression, sortdirection,
              1,
             iRowCount,
              ref iRowCount,
              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SupernumararyDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Supernumarary Details</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From: " + ds.Tables[1].Rows[0]["FLDSELECTEDFROMDATE"].ToString() + " - " + "To: " + ds.Tables[1].Rows[0]["FLDSELECTEDTODATE"].ToString() + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDFAMILYNAME", "FLDFAMILYDOB", "FLDRELATION", "FLDFILENO", "FLDEMPLOYEEYNAME", "FLDRANKNAME", "FLDEMPDOB", "FLDVESSELNAME", "FLDFAMILYSIGNONDATE", "FLDFAMILYSIGNONPORT", "FLDFAMILYSIGNOFFDATE", "FLDFAMILYSIGNOFFPORT", "FLDSAILDURATION" };
        string[] alCaptions = { "S.No.", "Family's Name", "Family's DOB", "Relation", "File No", "Seafarer's Name", "Rank", "Seafarer's DOB", "Vessel", "Signon Date", "Sign-on Port", "Signoff Date", "Sign-off Port", "Sail Duration(Months)" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.SupernumararyDetailsReport(
                (ucPrincipal.SelectedAddress) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
                (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                (ucVessel.SelectedVessel) == "" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                (ucZone.selectedlist) == "" ? null : General.GetNullableString(ucZone.selectedlist),
                (ucPool.SelectedPool) == "" ? null : General.GetNullableString(ucPool.SelectedPool),
                General.GetNullableDateTime(ucfromdate.Text),
                General.GetNullableDateTime(uctodate.Text),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvSupernumarary.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvSupernumarary", "Supernumarary Details", alCaptions, alColumns, ds);

        gvSupernumarary.DataSource = ds;
        gvSupernumarary.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSupernumarary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
        }


    }

    private bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }

    protected void gvSupernumarary_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvSupernumarary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupernumarary.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvSupernumarary_ItemCommand(object sender, GridCommandEventArgs e)
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
    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFormats.SelectedIndex == 0)
        {
            Response.Redirect("../Crew/CrewReportSupernumeraryDetails.aspx", true);
        }
        else
        {
            Response.Redirect("../Crew/CrewReportFamilyNOK.aspx", true);
        }
    }

}
