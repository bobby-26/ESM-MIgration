using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportMISRecruitmentAnalysisSeafarers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
           
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRecruitmentAnalysisSeafarers.aspx?fromdate=" + Request.QueryString["fromdate"] + "&todate=" + Request.QueryString["todate"] + "&zonelist=" + Request.QueryString["zonelist"] + "&poollist=" + Request.QueryString["poollist"] + "&fleetlist=" + Request.QueryString["fleetlist"] + "&rankid=" + Request.QueryString["rankid"] + "&principal=" + Request.QueryString["principal"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            DateTime from = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
            DateTime to = Convert.ToDateTime(Request.QueryString["todate"].ToString());
            ucTitle.Text = "Seafarers Joined as " + Request.QueryString["rankname"].ToString() +" From " + from.ToShortDateString() + " To " + to.ToShortDateString();
          
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
        string[] alColumns = { "FLDROW", "FLDBATCH", "FLDJOINEDRANK", "FLDLASTNAME", "FLDMIDDLENAME", "FLDFIRSTNAME", "FLDNATIONALITY", "FLDJOINEDDATE", "FLDCURRENTRANK", "FLDCURRENTVESSEL", "FLDSIGNONDATE", "FLDCOMPANIES" };
        string[] alCaptions = { "Sr.No", "Batch", "Joined Rank", "Surname", "Middle Name", "First Name", "Nationality", "Joined Date", "Current Rank", "Current Vessel", "Signed On", "No Of<br/>Company<br/>In 3 Yrs" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISRecruitmentSeafarer(
                General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                (Request.QueryString["zonelist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["zonelist"].ToString()),
                (Request.QueryString["poollist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["poollist"].ToString()),
                (Request.QueryString["fleetlist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["fleetlist"].ToString()),
                int.Parse(Request.QueryString["rankid"].ToString()),
                General.GetNullableInteger(Request.QueryString["principal"].ToString()),
                sortexpression, sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MISRecruitmentAnalysisSeafarersReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISRecruitmentSeafarer(
                General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                (Request.QueryString["zonelist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["zonelist"].ToString()),
                (Request.QueryString["poollist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["poollist"].ToString()),
                (Request.QueryString["fleetlist"].ToString()) == "Dummy" ? null : General.GetNullableString(Request.QueryString["fleetlist"].ToString()),
                int.Parse(Request.QueryString["rankid"].ToString()),
                General.GetNullableInteger(Request.QueryString["principal"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        //General.SetPrintOptions("gvCrew", "Crew Senior Officer Analysis", alCaptions, alColumns, ds);
        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkFirstName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage5','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
        }
    }
    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
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
