
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

public partial class Crew_CrewReportMISAgeExperienceAnalysisDetails : PhoenixBasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucRank.selectedlist = General.GetNullableString(Request.QueryString["currentrank"]) == null ? "" : Request.QueryString["currentrank"];
        }
    }
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISAgeExperienceAnalysisDetails.aspx?zone=" + Request.QueryString["zone"] + "&pool=" + Request.QueryString["pool"] + "&batch=" + Request.QueryString["batch"] + "&nationality=" + Request.QueryString["nationality"] + "&status=" + Request.QueryString["status"] + "&fromdate=" + Request.QueryString["fromdate"] + "&todate=" + Request.QueryString["todate"] + "&GraterAge=" + Request.QueryString["GraterAge"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                //ucRank.selectedlist = General.GetNullableString(Request.QueryString["currentrank"]);
                //ucRank.RankList= PhoenixRegistersRank.ListRank();
                //ucRank.selectedlist = General.GetNullableString(Request.QueryString["currentrank"]) == null ? "" : Request.QueryString["currentrank"];
                ViewState["GRATERAGE"] = General.GetNullableString(Request.QueryString["GraterAge"]) == null ? "" : Request.QueryString["GraterAge"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Session["agefrom"] = null;
                Session["ageto"] = null;

                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //ShowReport();
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
                ViewState["PAGENUMBER"] = 1;
                gvCrew.CurrentPageIndex = 0;
                ShowReport();

                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();
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
        string[] alColumns = { "FLDRANKNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDAGE", "FLDRANKEXP", "FLDNATIONALITY", "FLDCURRENTVESSEL", "FLDSIGNONDATE", "FLDLASTVESSEL", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Rank", "Emp No", "Name", "Age", "Experience in Rank(Months)", "Nationality", "Current Vessel", "Signed On", "Last Vessel", "Signed Off" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISAgeExpAnalysisDetails(Request.QueryString["zone"].ToString() == "Dummy" ? null : General.GetNullableString(Request.QueryString["zone"].ToString()),
           Request.QueryString["pool"].ToString() == "Dummy" ? null : General.GetNullableString(Request.QueryString["pool"].ToString()),
           Request.QueryString["batch"].ToString() == "Dummy" ? null : General.GetNullableInteger(Request.QueryString["batch"].ToString()),
           Request.QueryString["nationality"].ToString() == "Dummy" ? null : General.GetNullableString(Request.QueryString["nationality"].ToString()),
           General.GetNullableString(General.GetNullableString(Request.QueryString["currentrank"]) == null ? "" : Request.QueryString["currentrank"]),
           General.GetNullableString(Request.QueryString["status"]),
           General.GetNullableDateTime(Request.QueryString["fromdate"]),
           General.GetNullableDateTime(Request.QueryString["todate"]),
           Convert.ToInt32(Session["agefrom"]),
           Convert.ToInt32(Session["ageto"]),
           sortexpression, sortdirection,
           1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount,
           General.GetNullableInteger(ViewState["GRATERAGE"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=MISAgeExperianceStasticsDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Age Experiance Experiance Details</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b><center>" + alCaptions[i] + "</center></b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write("<center>" + dr[alColumns[i]] + "</center>");
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISAgeExpAnalysisDetails(Request.QueryString["zone"] == "Dummy" ? null : General.GetNullableString(Request.QueryString["zone"]),
            Request.QueryString["pool"].ToString() == "Dummy" ? null : General.GetNullableString(Request.QueryString["pool"].ToString()),
            Request.QueryString["batch"].ToString() == "Dummy" ? null : General.GetNullableInteger(Request.QueryString["batch"].ToString()),
            Request.QueryString["nationality"].ToString() == "Dummy" ? null : General.GetNullableString(Request.QueryString["nationality"].ToString()),
            General.GetNullableString(General.GetNullableString(Request.QueryString["currentrank"]) == null ? "" : Request.QueryString["currentrank"]),
            General.GetNullableString(Request.QueryString["status"]),
            General.GetNullableDateTime(Request.QueryString["fromdate"]),
            General.GetNullableDateTime(Request.QueryString["todate"]),
            Convert.ToInt32(Session["agefrom"]),
            Convert.ToInt32(Session["ageto"]),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCrew.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ViewState["GRATERAGE"].ToString()));

        //General.SetPrintOptions("gvCrew", "Crew Senior Officer Analysis", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (iRowCount > 0)
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        else
            ViewState["ROWSINGRIDVIEW"] = 0;
    }
    public bool IsValidFilter(string rank, string age, string agerange)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (rank.Equals(""))
        {
            ucError.ErrorMessage = "Select Rank";
        }
        if (agerange.Equals("0") && General.GetNullableInteger(age) == null)
        {
            //if(agerange.Equals("0"))
            //    ucError.ErrorMessage = "Select Age Range";
            //if(General.GetNullableInteger(age)==null)
            ucError.ErrorMessage = "Select either Age greater than or Age range";
        }
        return (!ucError.IsError);
    }
    public void ddlSelectRange_SelectedChangeEvent(object sender, EventArgs e)
    {
        if (ddlAgeRange.SelectedIndex == 1)
        {
            Session["agefrom"] = 18;
            Session["ageto"] = 25;
        }
        else if (ddlAgeRange.SelectedIndex == 2)
        {
            Session["agefrom"] = 26;
            Session["ageto"] = 30;
        }
        else if (ddlAgeRange.SelectedIndex == 3)
        {
            Session["agefrom"] = 31;
            Session["ageto"] = 35;
        }
        else if (ddlAgeRange.SelectedIndex == 4)
        {
            Session["agefrom"] = 36;
            Session["ageto"] = 40;
        }
        else if (ddlAgeRange.SelectedIndex == 5)
        {
            Session["agefrom"] = 41;
            Session["ageto"] = 45;
        }
        else if (ddlAgeRange.SelectedIndex == 6)
        {
            Session["agefrom"] = 46;
            Session["ageto"] = 50;
        }
        else if (ddlAgeRange.SelectedIndex == 7)
        {
            Session["agefrom"] = 51;
            Session["ageto"] = 55;
        }
        else if (ddlAgeRange.SelectedIndex == 8)
        {
            Session["agefrom"] = 56;
            Session["ageto"] = 60;
        }
        else if (ddlAgeRange.SelectedIndex == 9)
        {
            Session["agefrom"] = 61;
            Session["ageto"] = 100;
        }
        else if (ddlAgeRange.SelectedIndex == 0)
        {
            ucError.ErrorMessage = "Select valid Age Range";
            ucError.Visible = true;
            ddlAgeRange.SelectedIndex = 1;
            Session["agefrom"] = 18;
            Session["ageto"] = 25;
        }
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {

                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
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
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
}