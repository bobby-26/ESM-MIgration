using System;
using System.Data;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportMedicalCostEmployeeWise : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!string.IsNullOrEmpty(Request.QueryString["vesid"]))
            {
                Filter.CurrentVesselSelection = Request.QueryString["vesid"];

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMedicalCostEmployeeWise.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
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
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANK", "FLDLASTVESSEL", "FLDSIGNOFFDATE", "FLDONBOARD", "FLDSIGNONDATE", "FLDTYPEOFCASE", "FLDCOST" };
        string[] alCaptions = { "Sr.No", "File No", "Name", "Rank", "Last Vessel", "Sign Off Date", "OnBoard", "Sign On Date", "Type Of Case", "Cost" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.MedicalCostFilter;

        ds = PhoenixCrewMedicalCostEmployeewiseReport.CrewMedicalCostEmployeewiseReport(
            (nvc.Get("ucZone").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucZone").ToString())),
            (nvc.Get("ucPool").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucPool").ToString())),
            (nvc.Get("ucRank").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucRank").ToString())),
            (General.GetNullableInteger(Filter.CurrentVesselSelection).Value),
            (nvc.Get("ucVesseltype").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucVesseltype").ToString())),
            (nvc.Get("ucManager").Equals("Dummy") ? null : General.GetNullableInteger(nvc.Get("ucManager").ToString())),
            (nvc.Get("ucPrincipal").Equals("Dummy") ? null : General.GetNullableInteger(nvc.Get("ucPrincipal").ToString())),
            (nvc.Get("ucTestfromdate").Equals("") ? null : General.GetNullableDateTime(nvc.Get("ucTestfromdate").ToString())),
            (nvc.Get("ucTesttodate").Equals("") ? null : General.GetNullableDateTime(nvc.Get("ucTesttodate").ToString())),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);





        Response.AddHeader("Content-Disposition", "attachment; filename=Employeewise_Medical_Cost_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Employeewise Medical Cost Report</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + + "To:" + + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
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

    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANK", "FLDLASTVESSEL", "FLDSIGNOFFDATE", "FLDONBOARD", "FLDSIGNONDATE", "FLDTYPEOFCASE", "FLDCOST" };
        string[] alCaptions = { "Sr.No", "File No", "Name", "Rank", "Last Vessel", "Sign Off Date", "OnBoard", "Sign On Date", "Type Of Case", "Cost" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.MedicalCostFilter;
        string D = (nvc.Get("ucZone").ToString());

        ds = PhoenixCrewMedicalCostEmployeewiseReport.CrewMedicalCostEmployeewiseReport(
            (nvc.Get("ucZone").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucZone").ToString())),
            (nvc.Get("ucPool").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucPool").ToString())),
            (nvc.Get("ucRank").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucRank").ToString())),
            (General.GetNullableInteger(Filter.CurrentVesselSelection).Value),
            (nvc.Get("ucVesseltype").Equals("Dummy") ? null : General.GetNullableString(nvc.Get("ucVesseltype").ToString())),
            (nvc.Get("ucManager").Equals("Dummy") ? null : General.GetNullableInteger(nvc.Get("ucManager").ToString())),
            (nvc.Get("ucPrincipal").Equals("Dummy") ? null : General.GetNullableInteger(nvc.Get("ucPrincipal").ToString())),
            (nvc.Get("ucTestfromdate").Equals("") ? null : General.GetNullableDateTime(nvc.Get("ucTestfromdate").ToString())),
            (nvc.Get("ucTesttodate").Equals("") ? null : General.GetNullableDateTime(nvc.Get("ucTesttodate").ToString())),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCrew.PageSize,
            ref iRowCount,
            ref iTotalPageCount);



        General.SetPrintOptions("gvCrew", "Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

        }


    }



    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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


    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("CrewReportMedicalCost.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
