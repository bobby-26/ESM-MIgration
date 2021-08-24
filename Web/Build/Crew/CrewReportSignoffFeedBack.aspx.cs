using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewReportSignoffFeedBack : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.AccessRights = this.ViewState;
        MenuReportsFilter.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewReportSignoffFeedBack.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvSignoffFB')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar1.AddFontAwesomeButton("../Crew/CrewReportSignoffFeedBack.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        //ShowReport();
        
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
                ucRank.selectedlist = "";
                ucZone.selectedlist = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucBatch.SelectedBatch = "";
                ucFBFromDate.Text = "";
                ucFBToDate.Text = "";
                UcSFFromDate.Text = "";
                UcSFToDate.Text = "";
                ucPool.SelectedPool = "";
                ucManager.SelectedAddress = "";
                ViewState["PAGENUMBER"] = 1;
                gvSignoffFB.CurrentPageIndex = 0;
                ShowReport();
                gvSignoffFB.Rebind();
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
                if (!IsValidFilter(ucFBFromDate.Text, ucFBToDate.Text, UcSFFromDate.Text, UcSFToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    gvSignoffFB.CurrentPageIndex = 0;
                    ShowReport();
                    gvSignoffFB.Rebind();
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
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDVESSELNAME", "FLDFEEDBACKSTATUS", "FLDFEEDBACKDATE" };
        string[] alCaptions = { "Emp Code", "Emp Name", "Rank", "Sign On Date", "Sign Off Date", "Vessel", "FeedBack Y/N", "FeedBack Date" };

        string[] FilterColumns = { "FLDSELECTEDRANK", "FLDSELECTEDPRINCIPAL", "FLDSELECTEDMANAGER", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDTRAININGBATCH" };
        string[] FilterCaptions = { "Rank", "Principal", "Manager", "Zone", "Pool", "Vessel Type", "Batch" };

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
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;


        ds = PhoenixCrewReportSignoffFeedBack.SignoffFeedBackSearch((ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                    (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                    (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                    (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                    (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                    (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                    (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                     General.GetNullableDateTime(ucFBFromDate.Text),
                    General.GetNullableDateTime(ucFBToDate.Text),
                     General.GetNullableDateTime(UcSFFromDate.Text),
                    General.GetNullableDateTime(UcSFToDate.Text),
                     sortexpression, sortdirection,
                     1,
                     iRowCount,
                     ref iRowCount,
                     ref iTotalPageCount);

        string FBfromdate = ucFBFromDate.Text;
        string FBtodate = ucFBToDate.Text;
        string SFfromdate = UcSFFromDate.Text;
        string SFtodate = UcSFToDate.Text;

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Sign_Off_FeedBack.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign Off FeedBack Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Feed Back From: " + FBfromdate + " Feed Back To:" + FBtodate + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign Off From: " + SFfromdate + " Sign Off To:" + SFtodate + "</center></h5></td></tr>");
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
                Response.Write("<td align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    public bool IsValidFilter(string FBfromdate, string FBtodate, string SFfromdate, string SFtodate)
    {
        DateTime resultdate;
        if (General.GetNullableDateTime(FBfromdate) == null && General.GetNullableDateTime(FBtodate) == null
            && General.GetNullableDateTime(SFfromdate) == null && General.GetNullableDateTime(SFtodate) == null)
        {
            ucError.ErrorMessage = "Any one Type of Date Filter is Required.";
        }
        else
        {
            int filterCount = 0;
            if (General.GetNullableDateTime(FBfromdate) != null || General.GetNullableDateTime(FBtodate) != null)
                filterCount++;
            if (General.GetNullableDateTime(SFfromdate) != null || General.GetNullableDateTime(SFtodate) != null)
                filterCount++;
            if (filterCount > 1)
            {
                ucError.ErrorMessage = "Please select Only one Type of Date Filter.";
            }
            else if (!string.IsNullOrEmpty(FBfromdate))
            {
                if (DateTime.TryParse(FBfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "FeedBack From Date  should be earlier than current date.";
                }
            }
            else if (!string.IsNullOrEmpty(SFfromdate))
            {
                if (DateTime.TryParse(SFfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "Sign Off From Date  should be earlier than current date.";
                }
            }
        }
        return (!ucError.IsError);

    }
    private void ShowReport()
    {
        try
        {
            DataSet ds = new DataSet();
            ViewState["SHOWREPORT"] = 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDVESSELNAME", "FLDFEEDBACKSTATUS", "FLDFEEDBACKDATE" };
            string[] alCaptions = { "Emp Code", "Emp Name", "Rank", "Sign On Date", "Sign Off Date", "Vessel", "FeedBack Y/N", "FeedBack Date" };

            string[] FilterColumns = { "FLDSELECTEDRANK", "FLDSELECTEDPRINCIPAL", "FLDSELECTEDMANAGER", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDTRAININGBATCH" };
            string[] FilterCaptions = { "Rank", "Principal", "Manager", "Zone", "Pool", "Vessel Type", "Batch" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
            ds = PhoenixCrewReportSignoffFeedBack.SignoffFeedBackSearch((ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                            (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                            (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                            (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                            (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                            (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                            (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                             General.GetNullableDateTime(ucFBFromDate.Text),
                            General.GetNullableDateTime(ucFBToDate.Text),
                             General.GetNullableDateTime(UcSFFromDate.Text),
                            General.GetNullableDateTime(UcSFToDate.Text),
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             gvSignoffFB.PageSize,
                             ref iRowCount,
                             ref iTotalPageCount);

            General.SetPrintOptions("gvSignoffFB", "Crew Sign Off Feed Back Report", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
            }
            else
                ViewState["ROWSINGRIDVIEW"] = 0;

            gvSignoffFB.DataSource = ds;
            gvSignoffFB.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
      protected void gvSignoffFB_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
        gvSignoffFB.Rebind();
    }
    protected void gvSignoffFB_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvSignoffFB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            RadLabel lblSignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffId");
            LinkButton cmdShowReport = (LinkButton)e.Item.FindControl("cmdShowReport");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblFBYN = (RadLabel)e.Item.FindControl("lblFBYN");

            if (lbr != null)
            {
                lbr.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewListForAPeriod','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }

            if (cmdShowReport != null)
            {
                cmdShowReport.Visible = lblFBYN.Text.Trim().Equals("1");
                cmdShowReport.Attributes.Add("onclick", "javascript:parent.openNewWindow('FeedBackReport','','" + Session["sitepath"] + "/Crew/CrewReportSignoffFeedBackAnalysis.aspx?empid=" + empid.Text + "&SignOnOffId=" + lblSignonoffId.Text.Trim() + "'); return false;");                
            }
        }
    }

    protected void gvSignoffFB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSignoffFB.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
