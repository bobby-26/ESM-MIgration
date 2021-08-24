using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class CrewEmployeeGrowthList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewEmployeeGrowthList.aspx?type=" + (Request.QueryString["type"]!=null ? Request.QueryString["type"] : ViewState["type"]) + "&year=" + (Request.QueryString["year"] != null ? Request.QueryString["year"] : ViewState["year"]) + "&t=" + (Request.QueryString["t"] != null ? Request.QueryString["t"] : ViewState["t"]), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            MenuEmployeeGrowthList.AccessRights = this.ViewState;
            MenuEmployeeGrowthList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                if (Request.QueryString["year"] != null)
                {
                    ViewState["year"] = Request.QueryString["year"];
                }
                if (Request.QueryString["type"] != null)
                {
                    ViewState["type"] = Request.QueryString["type"];
                }
                if (Request.QueryString["t"] != null)
                {
                    ViewState["t"] = Request.QueryString["t"];
                }

                ucFromDate.Text = Request.QueryString["fromdate"];
                ucToDate.Text = Request.QueryString["todate"];
            }
            gvEmployeeGrowthList.PageSize = 10000;
            //  BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int irowcount = 0;
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDAPPLIEDRANK", "FLDPOSTEDRANK", "FLDBATCH", "FLDDATEOFJOINING", "FLDNATIONALITY", "FLDSTATUS", };
        string[] alCaptions = { "File No", "Name", "Recruited Rank", " Current Rank", "Batch", "1st join date", "Nationality", "Status" };


        NameValueCollection nvc = Filter.CurrentEmployeeGrowthReportFilter;

        DataSet ds = new DataSet();

        if (Request.QueryString["type"] == "1")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISRecruitedByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , General.GetNullableInteger(Request.QueryString["year"])
                                    , (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("asondate")) : null)
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")) : null)
                                    );

            General.SetPrintOptions("gvEmployeeGrowthList", "Recruited by Year List", alCaptions, alColumns, ds);

            gvEmployeeGrowthList.DataSource = ds.Tables[1];
            gvEmployeeGrowthList.VirtualItemCount = irowcount;
        }
        else if (Request.QueryString["type"] == "2")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISActivityByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , Request.QueryString["empstatus"] != null ? General.GetNullableInteger(Request.QueryString["empstatus"]) : null
                                    , General.GetNullableInteger(Request.QueryString["t"])
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("asondate")) : null)
                                    );

            General.SetPrintOptions("gvEmployeeGrowthList", "Active/Inactive List", alCaptions, alColumns, ds);

            gvEmployeeGrowthList.DataSource = ds.Tables[2];
            gvEmployeeGrowthList.VirtualItemCount = irowcount;

        }
        else if (Request.QueryString["type"] == "3")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISStatisticsByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , General.GetNullableInteger(Request.QueryString["t"])
                                    , (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
                                    , 1
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")) : null)
                                    );

            General.SetPrintOptions("gvEmployeeGrowthList", "% Statistics List", alCaptions, alColumns, ds);

            gvEmployeeGrowthList.DataSource = ds;
            gvEmployeeGrowthList.VirtualItemCount = irowcount;
        }

    }
    private void ShowExcel()
    {
        int irowcount = 0;

        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDAPPLIEDRANK", "FLDPOSTEDRANK", "FLDBATCH", "FLDDATEOFJOINING", "FLDNATIONALITY", "FLDSTATUS", };
        string[] alCaptions = { "File No", "Name", "Recruited Rank", " Current Rank", "Batch", "1st join date", "Nationality", "Status" };

        NameValueCollection nvc = Filter.CurrentEmployeeGrowthReportFilter;

        DataSet ds = new DataSet();

        if (Request.QueryString["type"] == "1")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISRecruitedByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , General.GetNullableInteger(Request.QueryString["year"])
                                    , (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("asondate")) : null)
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")) : null)
                                    );

            General.SetPrintOptions("gvEmployeeGrowthList", "Recruited by Year List", alCaptions, alColumns, ds);

            gvEmployeeGrowthList.DataSource = ds.Tables[1];
            gvEmployeeGrowthList.VirtualItemCount = irowcount;
        }
        else if (Request.QueryString["type"] == "2")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISActivityByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , Request.QueryString["empstatus"] != null ? General.GetNullableInteger(Request.QueryString["empstatus"]) : null
                                    , General.GetNullableInteger(Request.QueryString["t"])
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("asondate")) : null)
                                    );

        }
        else if (Request.QueryString["type"] == "3")
        {

            ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISStatisticsByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                    , General.GetNullableInteger(Request.QueryString["t"])
                                    , (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
                                    , 1
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")) : null)
                                    );


        }
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewEmployeeGrowthReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Employee Growth Recruitment Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Recruited From: " + ucFromDate.Text + " To: " + ucToDate.Text + " </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>As of Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
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
        if (Request.QueryString["type"] == "1")
        {
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
        }
        else if (Request.QueryString["type"] == "2")
        {
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
        }
        else if (Request.QueryString["type"] == "3")
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
        }

        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvEmployeeGrowthList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEmployeeGrowthList_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblRelieveeId = (RadLabel)e.Item.FindControl("lblEmpid");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblRelieveeId.Text + "'); return false;");            
        }
    }
    protected void EmployeeGrowthList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvEmployeeGrowthList.CurrentPageIndex = 0;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvEmployeeGrowthList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}