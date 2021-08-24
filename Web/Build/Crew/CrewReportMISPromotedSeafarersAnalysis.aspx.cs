using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportMISPromotedSeafarersAnalysis : PhoenixBasePage
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

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISPromotedSeafarersAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISPromotedSeafarersAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP").Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();

                lstStatus.DataBind();
                lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", ""));

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
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
                ucRankTo.SelectedRankValue = "";
                ucRankFrom.SelectedRankValue = "";
                ucPool.SelectedPoolValue = "";
                ucZone.SelectedZoneValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucBatchList.SelectedList = "";
                lstStatus.SelectedValue = "";
                ucDate.Text = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucBatchList.SelectedValue = "";

                ViewState["PAGENUMBER"] = 1;
                gvCrew.CurrentPageIndex = 0;
                ShowReport();
                gvCrew.Rebind();
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text, ucRankTo.selectedlist.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    gvCrew.CurrentPageIndex = 0;
                    ShowReport();
                    gvCrew.Rebind();
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
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDPROMOTIONDATE", "FLDFROMRANKNAME", "FLDTORANKNAME", "FLDCURRENTVESSELNAME", "FLDCURRENTSIGNONDATE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "ISPROMOTEDONBOARD" };
        string[] alCaptions = { "S.No.", "File No.", "Batch", "Name", "Promoted On", "From", "To", "Vessel On", "Sign-On Date", "Last Vessel", "Sign-Off Date", "Promoted OnBoard" };
        string[] FilterColumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDFROMRANK", "FLDSELECTEDTORANK", "FLDSELECTEDZONE", "FLDSELECTEDNATIONALITY", "FLDSELECTEDTRAININGBATCH", "FLDSELECTEDSTATUS", "FLDSELECTEDPOOL" };
        string[] FilterCaptions = { "From Date", "To Date", "Promoted From Rank", "Promoted To Rank", "Zone", "Nationality", "Batch", "Status", "Pool" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucRankFrom.selectedlist = ucRankFrom.selectedlist.ToString().Contains("Dummy,") ? ucRankFrom.selectedlist.ToString().Replace("Dummy,", "") : ucRankFrom.selectedlist;
        ucRankTo.selectedlist = ucRankTo.selectedlist.ToString().Contains("Dummy,") ? ucRankTo.selectedlist.ToString().Replace("Dummy,", "") : ucRankTo.selectedlist;
        ucBatchList.SelectedList = ucBatchList.SelectedList.ToString().Contains("Dummy,") ? ucBatchList.SelectedList.ToString().Replace("Dummy,", "") : ucBatchList.SelectedList;

        ds = PhoenixCrewReportPromotedSeafarers.SeafarersPromotedList(
                     (ucRankFrom.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRankFrom.selectedlist.ToString()),   
                     (ucRankTo.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRankTo.selectedlist.ToString()),
                     (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                     (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                     (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                     General.GetNullableDateTime(ucDate.Text),
                     General.GetNullableDateTime(ucDate1.Text),
                     (ucBatchList.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatchList.SelectedList.ToString()),
                     (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
                     (chkCompanyExp.Checked == true ? 1 : 0),
                     sortexpression, sortdirection,
                     1,
                     iRowCount,
                     ref iRowCount,
                     ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Promoted_Seafarers_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Promoted Seafarers Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
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
                string svalue = dr[alColumns[i]].GetType().ToString().ToUpper().Equals("SYSTEM.DATETIME") ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString();
                Response.Write("<td align='left'>");
                Response.Write(svalue);
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
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDPROMOTIONDATE", "FLDFROMRANKNAME", "FLDTORANKNAME", "FLDCURRENTVESSELNAME", "FLDCURRENTSIGNONDATE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "ISPROMOTEDONBOARD" };
        string[] alCaptions = { "S.No.", "File No.", "Batch", "Name", "Promoted On", "From", "To", "Vessel On", "Sign-On Date", "Last Vessel", "Sign-Off Date", "Promoted OnBoard" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        //ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        //ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        //ucRankFrom.selectedlist = ucRankFrom.selectedlist.ToString().Contains("Dummy,") ? ucRankFrom.selectedlist.ToString().Replace("Dummy,", "") : ucRankFrom.selectedlist;
        //ucRankTo.selectedlist = ucRankTo.selectedlist.ToString().Contains("Dummy,") ? ucRankTo.selectedlist.ToString().Replace("Dummy,", "") : ucRankTo.selectedlist;
        //ucBatchList.SelectedList = ucBatchList.SelectedList.ToString().Contains("Dummy,") ? ucBatchList.SelectedList.ToString().Replace("Dummy,", "") : ucBatchList.SelectedList;

        //if (ucRankTo.SelectedRankValue.Equals("") || ucRankTo.SelectedRankValue.Equals("Dummy"))
        //{
        //    ds = PhoenixCrewReportPromotedSeafarers.SeafarersPromotedList(null, null,
        //            null, null, null, null, null, null,null,
        //            (chkCompanyExp.Checked == true ? 1 : 0),
        //            sortexpression, sortdirection,
        //            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        //            General.ShowRecords(null), ref iRowCount,
        //            ref iTotalPageCount);
        //}
        //else
        //{
            ds = PhoenixCrewReportPromotedSeafarers.SeafarersPromotedList(
                    //(ucRankFrom.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRankFrom.selectedlist.ToString()),
                    //(ucRankTo.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRankTo.selectedlist.ToString()),
                    (ucRankFrom.selectedlist) == "," ? null : General.GetNullableString(ucRankFrom.selectedlist),
                    (ucRankTo.selectedlist) == "," ? null : General.GetNullableString(ucRankTo.selectedlist),
                    (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                    (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                    (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                    General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text),
                    (ucBatchList.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatchList.SelectedList.ToString()),
                    (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
                    (chkCompanyExp.Checked == true ? 1 : 0),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);
        //}
        General.SetPrintOptions("gvCrew", "Promoted Seafarers Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
             
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {  
        if (e.Item is GridDataItem)
        {
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");            
        }   
    }
    public bool IsValidFilter(string fromdate,string todate,string rank)
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
        if (rank.Equals("") || rank.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Rank is required";
        }
        return (!ucError.IsError);
    }
   
    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {       
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
        gvCrew.Rebind();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
