using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewReports;
using Telerik.Web.UI;
public partial class Crew_CrewReportReasonwiseIllnessAndInjuryDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportReasonwiseIllnessAndInjuryDetails.aspx?Reason=" + Request.QueryString["Reason"] + "&port=" + Request.QueryString["port"] + "&pool=" + Request.QueryString["pool"] + "&zone=" + Request.QueryString["zone"] + "&fromdate=" + Request.QueryString["fromdate"] + "&todate=" + Request.QueryString["todate"] + "&RankList=" + Request.QueryString["RankList"] + "&Principal=" + Request.QueryString["Principal"] + "&VesselType=" + Request.QueryString["VesselType"] + "&InjuryType=" + Request.QueryString["InjuryType"] + "&MedicalCase=" + Request.QueryString["MedicalCase"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

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
        string fromdate = Request.QueryString["fromdate"].ToString();
        string todate = Request.QueryString["todate"].ToString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME", "FLDRANK", "FLDDATEOFILLNESS", "FLDTYPESOFINJURY", "FLDPNIMEDICALCASEID", "FLDPORT", "FLDVESSELNAME", "FLDZONE", "FLDPOOL" };
        string[] alCaptions = { "Seafarer Name", "Rank", "Date of Illness/Injury", "Reason", "Case No", "Port", "Vessel", "Zone", "Pool" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjuryDetails(
                                                            General.GetNullableString(Request.QueryString["port"].ToString()),
                                                            General.GetNullableString(Request.QueryString["pool"].ToString()),
                                                            General.GetNullableString(Request.QueryString["zone"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                                                            General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                                                            1,
                                                            iRowCount,
                                                            ref iRowCount,
                                                            ref iTotalPageCount
                                                            , General.GetNullableString(Request.QueryString["RankList"].ToString())
                                                            , General.GetNullableString(Request.QueryString["Principal"].ToString())
                                                            , General.GetNullableString(Request.QueryString["VesselType"].ToString())
                                                            , General.GetNullableInteger(Request.QueryString["reason"].ToString())
                                                            , General.GetNullableInteger(Request.QueryString["MedicalCase"].ToString()));
        Response.AddHeader("Content-Disposition", "attachment; filename=Head_Count_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Reasonwise Illness/Injury Detailed Report </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From: " + fromdate + " To: " + todate + " </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>As of Date:" + date + "</td></tr>");
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

        ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjuryDetails(
                                                          General.GetNullableString(Request.QueryString["port"].ToString()),
                                                          General.GetNullableString(Request.QueryString["pool"].ToString()),
                                                          General.GetNullableString(Request.QueryString["zone"].ToString()),
                                                          General.GetNullableDateTime(Request.QueryString["fromdate"].ToString()),
                                                          General.GetNullableDateTime(Request.QueryString["todate"].ToString()),
                                                          Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                        gvCrew.PageSize,
                                                          ref iRowCount,
                                                          ref iTotalPageCount
                                                          , General.GetNullableString(Request.QueryString["RankList"].ToString())
                                                          , General.GetNullableString(Request.QueryString["Principal"].ToString())
                                                          , General.GetNullableString(Request.QueryString["VesselType"].ToString())
                                                          , General.GetNullableInteger(Request.QueryString["reason"].ToString())
                                                          , General.GetNullableInteger(Request.QueryString["MedicalCase"].ToString()));

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
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage5','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            RadLabel lblCaseDesc = (RadLabel)e.Item.FindControl("lblCaseDesc");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblSubject");
            if (lblCaseDesc != null)
            {
                lblCaseDesc.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblCaseDesc.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblCaseDesc.ClientID;
            }
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