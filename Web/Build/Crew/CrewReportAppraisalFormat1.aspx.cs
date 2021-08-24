using System;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewReportAppraisalFormat1 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportAppraisalFormat1.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportAppraisalFormat1.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Analysis Format2", "FORMAT2", ToolBarDirection.Right);
            toolbar1.AddButton("Analysis Format1", "FORMAT1", ToolBarDirection.Right);

            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar1.Show();
            MenuReport.SelectedMenuIndex = 1;

            toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        gvAQ.SelectedIndexes.Clear();
        gvAQ.EditIndexes.Clear();
        gvAQ.DataSource = null;
        gvAQ.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDAPPRAISALDATE", "FLDRECOMANDPROMOTION", "FLDNOOFDOCTORVISIT", "FLDRECOMMENDEDCOURSES", "FLDFITFORREEMPLOYMENTYN", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT", "FLDSEAMANCOMMENT" };
        string[] alCaptions = { "S.No", "File No.", "Name", "Rank", "Vessel", "Date of Report", "Promotion Recommended(Y/N)", "Medical Visit", "Courses Recommended", "Fit for Employment(y/n/with warning)", "HOD Remarks", "Master Remarks", "Seafarer Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewAppraisal.ReportAppraisalFormat1Search(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                                    , lstRank.selectedlist
                                    , ucPool.SelectedPool
                                    , ucBatch.SelectedList
                                    , ucVesselType.SelectedVesseltype
                                    , ucManager.SelectedList
                                    , 1
                                    , iRowCount
                                    , ref iRowCount
                                    , ref iTotalPageCount);
        General.ShowExcel("Appraisal Analysis", dt, alColumns, alColumns, null, null);
    }
    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("CrewReportAppraisalFormat2.aspx", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e)
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
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPool = "";
                ucBatch.SelectedList = "";
                lstRank.selectedlist = "";
                ucManager.SelectedList = "";

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
                if (!IsValidFilter(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblHODComment");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipHODComment");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            lbtn = (RadLabel)e.Item.FindControl("lblMasterComment");
            uct = (UserControlToolTip)e.Item.FindControl("ucToolTipMasterComment");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            lbtn = (RadLabel)e.Item.FindControl("lblSeafarerComment");
            uct = (UserControlToolTip)e.Item.FindControl("ucToolTipSeafarerComment");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            lbtn = (RadLabel)e.Item.FindControl("lblRecommendedCourses");
            uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRecommendedCourses");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }


    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDAPPRAISALDATE", "FLDRECOMANDPROMOTION", "FLDNOOFDOCTORVISIT", "FLDRECOMMENDEDCOURSES", "FLDFITFORREEMPLOYMENTYN", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT", "FLDSEAMANCOMMENT" };
        string[] alCaptions = { "S.No", "File No.", "Name", "Rank", "Vessel", "Date of Report", "Promotion Recommended(Y/N)", "Medical Visit", "Courses Recommended", "Fit for Employment(y/n/with warning)", "HOD Remarks", "Master Remarks", "Seafarer Remarks" };
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string sortexpression;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        try
        {

            DataTable dt = PhoenixCrewAppraisal.ReportAppraisalFormat1Search(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                                    , lstRank.selectedlist
                                    , ucPool.SelectedPool
                                    , ucBatch.SelectedList
                                    , ucVesselType.SelectedVesseltype
                                    , ucManager.SelectedList
                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                    , gvAQ.PageSize
                                    , ref iRowCount
                                    , ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvAQ", "Crew Appraisal", alCaptions, alColumns, ds);

            gvAQ.DataSource = ds;
            gvAQ.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private bool IsValidFilter(string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following filter criteria";
        DateTime resultdate;

        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'To Date' should be later than 'From Date'";
        }
        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'From date' should not be future date";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date";
        }
        return (!ucError.IsError);
    }

    protected void gvAQ_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;

        BindData();
    }
}
