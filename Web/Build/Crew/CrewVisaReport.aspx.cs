using System;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

public partial class Crew_CrewVisaReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewVisaReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewVisaReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewVisaReport.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();


            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Onleave", "ONLEAVE", ToolBarDirection.Right);
            toolbar2.AddButton("Onboard", "ONBOARD", ToolBarDirection.Right);
            

            MenuReportsFilterVisa.AccessRights = this.ViewState;
            MenuReportsFilterVisa.MenuList = toolbar2.Show();
            MenuReportsFilterVisa.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvCrewVisaReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        gvCrewVisaReport.SelectedIndexes.Clear();
        gvCrewVisaReport.EditIndexes.Clear();
        gvCrewVisaReport.DataSource = null;
        gvCrewVisaReport.Rebind();
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
                ucRank.selectedlist = "";
                ucZone.SelectedZoneValue = "";
                ucManager.SelectedList = "";
                ucVessel.SelectedVessel = "";
                ddlNoofentryAdd.SelectedIndex = 0;
                ucCountry.SelectedCountry = "";
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
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString(), ucCountry.SelectedCountry.ToString()))
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

    protected void ReportsVisaFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ONLEAVE"))
            {
                Response.Redirect("../Crew/CrewVisaReportOnleavewise.aspx", true);
                MenuReportsFilterVisa.SelectedMenuIndex = 0;
            }

            ViewState["PAGENUMBER"] = 1;
            Rebind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidFilter(string vessel, string visa)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Vessel is Required";
        }
        if (visa.Equals("") || visa.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Visa is Required";
        }
        return (!ucError.IsError);

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDCOUNTRYNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDRELIEFDUEDATE", "FLDNOOFENTRYNAME", "FLDZONE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Vessel", "Visa", "Date of Issue", "Date of Expiry", "Relief Due", "No. of Entries", "Zone" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        //ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;

        ds = PhoenixCrewVisaReport.CrewVisaReportSearch(
               General.GetNullableString(ucVessel.SelectedVessel)
            , General.GetNullableString(ucRank.selectedlist)
            , General.GetNullableString(ucZone.selectedlist)
            , General.GetNullableInteger(ucManager.SelectedValue)
            , General.GetNullableString(ucCountry.SelectedCountry)
            , General.GetNullableInteger(ddlNoofentryAdd.SelectedValue)
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Visa_OnboardReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Visa Report</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
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
                Response.Write("<td align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.Write(string.IsNullOrEmpty(ConfigurationManager.AppSettings["softwarename"]) ? "" : ConfigurationManager.AppSettings["softwarename"]);
        Response.End();
    }

    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDCOUNTRYNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDRELIEFDUEDATE", "FLDNOOFENTRYNAME", "FLDZONE" };
            string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Vessel", "Visa", "Date of Issue", "Date of Expiry", "Relief Due", "No. of Entries", "Zone" };


            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
            //ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;

            DataSet ds = new DataSet();

            if (!IsPostBack)
            {
                ds = PhoenixCrewVisaReport.CrewVisaReportSearch(null, null, null, null, null, null
                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                 , gvCrewVisaReport.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount);

            }
            else
            {
                if (ucVessel.SelectedVessel.ToString().Equals(""))
                {
                    ds = PhoenixCrewVisaReport.CrewVisaReportSearch(null, null, null, null, null, null
                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                     , gvCrewVisaReport.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount);
                }
                else
                {
                    ds = PhoenixCrewVisaReport.CrewVisaReportSearch(
                        General.GetNullableString(ucVessel.SelectedVessel)
                     , General.GetNullableString(ucRank.selectedlist)
                     , General.GetNullableString(ucZone.selectedlist)
                     , General.GetNullableInteger(ucManager.SelectedValue)
                     , General.GetNullableString(ucCountry.SelectedCountry)
                     , General.GetNullableInteger(ddlNoofentryAdd.SelectedValue)
                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                     , gvCrewVisaReport.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount);
                }
            }

            General.SetPrintOptions("gvCrewVisaReport", "Visa Report", alCaptions, alColumns, ds);

            gvCrewVisaReport.DataSource = ds;
            gvCrewVisaReport.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewVisaReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblEmployeeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
                LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
                if (lblEmployeeId != null && lnkEmployeeName != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmployeeId.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewVisaReport_SortCommand(object sender, GridSortCommandEventArgs e)
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

    
    protected void gvCrewVisaReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewVisaReport.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvCrewVisaReport_ItemCommand(object sender, GridCommandEventArgs e)
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