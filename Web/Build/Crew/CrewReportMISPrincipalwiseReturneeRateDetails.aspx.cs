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

public partial class Crew_CrewReportMISPrincipalwiseReturneeRateDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISPrincipalwiseReturneeRateDetails.aspx?zone=" + Request.QueryString["zone"] + "&pool=" + Request.QueryString["pool"] + "&nationality=" + Request.QueryString["nationality"] + "&rank=" + Request.QueryString["rank"] + "&principal=" + Request.QueryString["principal"] + "&fromdate=" + Request.QueryString["fromdate"] + "&todate=" + Request.QueryString["todate"] + "&fleet=" + Request.QueryString["fleet"] + "&value=" + Request.QueryString["value"] + "&pmtype=" + Request.QueryString["pmtype"] + "&manager=" + Request.QueryString["manager"] + "&batch=" + Request.QueryString["batch"] + "&vsltype=" + Request.QueryString["vsltype"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                                    
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDNATIONALITY", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDMANAGER", "FLDJOINEDDATE", "FLDLASTVESSEL" };
        string[] alCaptions = { "File No", "Emp Name", "Rank", "Nationality", "Vessel", "Signed On", "Principal", "Date First Join", "Last Vessel" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISPrincipalwiseReturneeRateDetails(
                (Request.QueryString["zone"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["zone"]),
                (Request.QueryString["pool"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["pool"]),
                (Request.QueryString["nationality"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["nationality"]),
                (Request.QueryString["rank"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["rank"]),
                (Request.QueryString["principal"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["principal"]),
                General.GetNullableDateTime(Request.QueryString["fromdate"]),
                General.GetNullableDateTime(Request.QueryString["todate"]),
                (Request.QueryString["fleet"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["fleet"]),
                General.GetNullableInteger(Request.QueryString["value"]),
                General.GetNullableInteger(Request.QueryString["pmtype"]),
                General.GetNullableInteger(Request.QueryString["manager"]),
                General.GetNullableString(Request.QueryString["batch"]),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                (Request.QueryString["vsltype"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["vsltype"]));

        General.SetPrintOptions("gvCrew", "MIS Principalwise Returnee Rate", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (iRowCount > 0)
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        else
            ViewState["ROWSINGRIDVIEW"] = 0;
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
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDNATIONALITY", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDMANAGER", "FLDJOINEDDATE", "FLDLASTVESSEL" };
        string[] alCaptions = { "File No", "Emp Name", "Rank", "Nationality", "Vessel", "Signed On", "Principal", "Date First Join", "Last Vessel" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISPrincipalwiseReturneeRateDetails(
                (Request.QueryString["zone"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["zonelist"]),
                (Request.QueryString["pool"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["poollist"]),
                (Request.QueryString["nationality"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["nationality"]),
                (Request.QueryString["rank"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["rank"]),
                (Request.QueryString["principal"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["principal"]),
                General.GetNullableDateTime(Request.QueryString["fromdate"]),
                General.GetNullableDateTime(Request.QueryString["todate"]),
                (Request.QueryString["fleet"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["fleet"]),
                General.GetNullableInteger(Request.QueryString["value"]),
                General.GetNullableInteger(Request.QueryString["pmtype"]),
                General.GetNullableInteger(Request.QueryString["manager"]),
                General.GetNullableString(Request.QueryString["batch"]),
                sortexpression, sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount,
                (Request.QueryString["vsltype"]) == "Dummy" ? null : General.GetNullableString(Request.QueryString["vsltype"]));

        Response.AddHeader("Content-Disposition", "attachment; filename=MISPrincipalwiseReturneeRate.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Principalwise Returnee Rate</center></h5></td></tr>");
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
   
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {            
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
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
