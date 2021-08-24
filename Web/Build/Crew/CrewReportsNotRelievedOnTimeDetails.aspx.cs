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
public partial class CrewReportsNotRelievedOnTimeDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsNotRelievedOnTimeDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["RANKID"] = "";
                ViewState["FROM"] = "";
                ViewState["TYPE"] = "";
                ViewState["DATE"] = "";

                if (Request.QueryString["rank"] != null && Request.QueryString["rank"] != "")
                    ViewState["RANKID"] = Request.QueryString["rank"].ToString();

                if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                    ViewState["TYPE"] = Request.QueryString["type"].ToString();

                if (Request.QueryString["from"] != null && Request.QueryString["from"] != "")
                    ViewState["FROM"] = Request.QueryString["from"].ToString();

                if (Request.QueryString["date"] != null && Request.QueryString["date"] != "")
                    ViewState["DATE"] = Request.QueryString["date"].ToString();

                if (Request.QueryString["title"] != null && Request.QueryString["title"] != "")
                    ucTitle.Text = ucTitle.Text + " - " + Request.QueryString["title"].ToString();
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
        string from = "";

        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("0"))
            from = "OnLeave";
        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("1"))
            from = "OnBoard";

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDBATCH", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDDELAY" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Vessel", "Trip Length Date", "SignOff Date", "Delay (No.of days)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("0"))
        {
            NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnleave;

            ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnLeaveDetailsSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucVesselType"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucFromDate"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucToDate"] : null)
                                                                    , int.Parse(ViewState["RANKID"].ToString())
                                                                    , int.Parse(ViewState["TYPE"].ToString())
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);
        }

        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("1"))
        {
            NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnboard;

            ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnBoardDetailsSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucAsOnDate"] : ViewState["DATE"].ToString())
                                                                    , int.Parse(ViewState["RANKID"].ToString())
                                                                    , int.Parse(ViewState["TYPE"].ToString())
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);
        }
        string heading = "Relief Delayed( " + from + " )";
        General.ShowExcel(heading, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDNAME", "FLDRANKNAME", "FLDFILENO", "FLDBATCH", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDDELAY" };
        string[] alCaptions = { "S.No", "Name", "Rank", "File No", "Batch", "Vessel", "Trip Length Date", "S/off Date", "Delay (No.of days)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("0"))
        {
            NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnleave;

            ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnLeaveDetailsSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucVesselType"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucFromDate"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucToDate"] : null)
                                                                    , int.Parse(ViewState["RANKID"].ToString())
                                                                    , int.Parse(ViewState["TYPE"].ToString())
                                                                    , sortexpression, sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCrew.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);
        }

        if (!string.IsNullOrEmpty(ViewState["FROM"].ToString()) && ViewState["FROM"].ToString().Equals("1"))
        {
            NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnboard;

            ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnBoardDetailsSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                    , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucAsOnDate"] : ViewState["DATE"].ToString())
                                                                    , int.Parse(ViewState["RANKID"].ToString())
                                                                    , int.Parse(ViewState["TYPE"].ToString())
                                                                    , sortexpression, sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCrew.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);
        }

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

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
