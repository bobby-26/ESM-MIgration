using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewSignOffPOEAFormat : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOffPOEAFormat.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOffPOEAFormat.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
                NameValueCollection nvc = Filter.CurrentSignOffFilter;
                if (nvc != null)
                {
                    ucZone.SelectedZoneValue = nvc.Get("ucZone");
                    if (nvc.Get("ucNationality").Equals("Dummy"))
                    {
                        ucNationality.SelectedNationalityValue = "";
                    }
                    else
                    {
                        ucNationality.SelectedNationalityValue = nvc.Get("ucNationality");
                    }
                    if (nvc.Get("ucPrincipal").Equals("Dummy"))
                    {
                        ucPrincipal.SelectedAddress = "";
                    }
                    else
                    {
                        ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal");
                    }
                    if (nvc.Get("ucVesselType").Equals("Dummy"))
                    {
                        ucVesselType.SelectedVesselTypeValue = "";
                    }
                    else
                    {
                        ucVesselType.SelectedVesselTypeValue = nvc.Get("ucVesselType");
                    }
                    if (nvc.Get("ucBatch").Equals("Dummy"))
                    {
                        ucBatch.SelectedBatch = "";
                    }
                    else
                    {
                        ucBatch.SelectedBatch = nvc.Get("ucBatch");
                    }
                    if (nvc.Get("ucEmpFleet").Equals("Dummy"))
                    {
                        ucEmpFleet.SelectedFleetValue = "";
                    }
                    else
                    {
                        ucEmpFleet.SelectedFleetValue = nvc.Get("ucEmpFleet").ToString();
                    }
                    if (nvc.Get("ucPool").Equals("Dummy"))
                    {
                        ucPool.SelectedPoolValue = "";
                    }
                    else
                    {
                        ucPool.SelectedPoolValue = nvc.Get("ucPool").ToString();
                    }
                }
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ddlMonthlist.SelectedHard = "";
                ddlYearlist.SelectedQuick = "";
                ucZone.SelectedZoneValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucBatch.SelectedBatch = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPoolValue = "";
                ucEmpFleet.SelectedFleetValue = "";
                Filter.CurrentSignOffFilter = null;

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
                if (!IsValidFilter(ddlMonthlist.SelectedHard.ToString(), ddlYearlist.SelectedQuick.ToString()))
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
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDBATCH", "FLDRANKCODE", "FLDMARITALSTATUS", "FLDVESSELNAME", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Name of Seaman", "Batch", "Rank", "Civil Status", "Vessel", "Disembark.dt" };
        string[] FilterColumns = { "FLDSELECTEDMONTH", "FLDSELECTEDYEAR", "FLDSELECTEDNATIONALITY", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDFLEET", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDTRAININGBATCH" };
        string[] FilterCaptions = { "Month", "Year", "Nationality", "Zone", "Pool", "Fleet", "Vessel Type", "Batch" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewSignOffReport.CrewSignOffReportPoeaFormat(
             (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
             (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
             (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
             (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
             (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
             (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
             (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
             General.GetNullableInteger(ddlMonthlist.SelectedHard),
             General.GetNullableInteger(ddlYearlist.SelectedQuick),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Sign_Off_Report_POEA_Format.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Sign Off POEA Format</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDBATCH", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Name of Seaman", "Batch", "Rank", "Vessel", "Disembark.dt" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewSignOffReport.CrewSignOffReportPoeaFormat(
                 (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                 (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                 (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                 (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                 (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                 (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                 (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                 General.GetNullableInteger(ddlMonthlist.SelectedHard),
                 General.GetNullableInteger(ddlYearlist.SelectedQuick),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "CrewReport", alCaptions, alColumns, ds);

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
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            if (ViewState["TTO"].ToString() != drv["FLDEMPLOYEESTATUS"].ToString())
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
            else
                lbr.Enabled = false;
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
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
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
    public bool IsValidFilter(string month, string year)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (month.Equals("") || month.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Month is Required";
        }
        if (year.Equals("") || month.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Year is required";
        }
        return (!ucError.IsError);

    }

}
