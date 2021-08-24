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

public partial class CrewReportsRecruitmentStatisticsReportDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsRecruitmentStatisticsReportDetails.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ShowReport();


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
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBATCH", "FLDNATIONALITY", "FLDSTATUS" };
        string[] alCaptions = { "File No", "Name", "Rank", "Batch", "Nationality", "Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewStatisticsReport.CrewStatisticsReportDetails(General.GetNullableString(Request.QueryString["principal"].ToString()),
                                                            General.GetNullableString(Request.QueryString["vesseltype"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["batch"].ToString()),
                                                            General.GetNullableString(Request.QueryString["vessel"].ToString()),
                                                            General.GetNullableString(Request.QueryString["pool"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["selzone"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["selrank"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                            1,
                                                            iRowCount,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            General.GetNullableInteger(Request.QueryString["refby"].ToString()));


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewRecruitmentStatistics_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Recruitment Statistics Report as on " + date + "</center></h5></td></tr>");
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
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixCrewStatisticsReport.CrewStatisticsReportDetails(General.GetNullableString(Request.QueryString["principal"].ToString()),
                                                            General.GetNullableString(Request.QueryString["vesseltype"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["Batch"].ToString()),
                                                            General.GetNullableString(Request.QueryString["vessel"].ToString()),
                                                            General.GetNullableString(Request.QueryString["pool"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["selzone"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["selrank"].ToString()),
                                                            General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvCrew.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            General.GetNullableInteger(Request.QueryString["refby"].ToString()));

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkFirstName");
            lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage5','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}
