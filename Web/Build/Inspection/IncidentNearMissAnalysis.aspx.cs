using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Web.UI;


public partial class Inspection_IncidentNearMissAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();
            MenuReportsFilter.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Inspection/IncidentNearMissAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar1.AddFontAwesomeButton("../Inspection/IncidentNearMissAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                BindYear();
                sessionFilterValues();
            }
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // bind filter criteria if any
        BindFilterCriteria();
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
                ddlYear.SelectedValue = System.DateTime.Today.Year.ToString();
                lstQuarter.SelectedValue = null;
                ucVessel.SelectedVesselValue = "";
                ucFleet.SelectedFleetValue = "";
                ucVesselType.SelectedVesseltype = "";
                ucPrincipal.SelectedList = "";
                ddlEventType.SelectedValue = "Incident";

                InspectionFilter.CurrentInspectionIncidentNearMissFilter = null;
                gvCrew.Rebind();
                ShowReport();
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
                InspectionFilter.CurrentInspectionIncidentNearMissFilter = null;
                sessionFilterValues();
            }

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                sessionFilterValues();
                Response.Redirect("../Inspection/IncidentNearMissAnalysisVisual.aspx");
            }

            ViewState["PAGENUMBER"] = 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public NameValueCollection sessionFilterValues()
    {
        NameValueCollection nvc = new NameValueCollection();

        if (IsPostBack)
        {
            nvc.Clear();
            nvc.Add("ddlYear", YearList());
            nvc.Add("Quarter", selectedQuarterList());
            nvc.Add("ucFleet", ucFleet.SelectedList.ToString());
            nvc.Add("ucVessel", ucVessel.SelectedVessel.ToString());
            nvc.Add("ucVesselType", ucVesselType.SelectedVesseltype.ToString());
            nvc.Add("ucPrincipal", ucPrincipal.SelectedList.ToString());
            nvc.Add("ddlEventType", ddlEventType.SelectedValue);

            InspectionFilter.CurrentInspectionIncidentNearMissFilter = nvc;
        }
        return InspectionFilter.CurrentInspectionIncidentNearMissFilter;
    }
    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = InspectionFilter.CurrentInspectionIncidentNearMissFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }

        ddlYear.SelectedValue = CheckIsNull(nvc.Get("ddlYear")) == "" ? System.DateTime.Today.Year.ToString() : CheckIsNull(nvc.Get("ddlYear"));
        ucPrincipal.SelectedList = CheckIsNull(nvc.Get("ucPrincipal"));
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucVesselType.SelectedVesseltype = CheckIsNull(nvc.Get("ucVesselType"));
        ddlEventType.SelectedValue = CheckIsNull(nvc.Get("ddlEventType"));
        //ucFleet.SelectedFleetValue = CheckIsNull(nvc.Get("ucFleet"));
        //string Fleet = CheckIsNull(nvc.Get("ucFleet"));
        //CheckUncheck(Fleet, "Fleet");
        string Quarter = CheckIsNull(nvc.Get("Quarter"));
        CheckUncheck(Quarter, "Quarter");
        ShowReport();
    }
    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }

    protected void CheckUncheck(string Values, string ID)
    {
        if (Values != "" && Values != null)
        {
            string[] values = Values.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                if (ID == "Quarter")
                    lstQuarter.Items.FindByValue(values[i]).Selected = true;
                else if (ID == "Fleet")
                    ucFleet.SelectedList = values[i];
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEVENTDATE", "FLDEVENTREFNO", "FLDEVENTTYPE", "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDEVENTSTATUS", "FLDEVENTCONSEQUENCECATEGORY", "FLDEVENTCATEGORY", "FLDEVENTSUBCATEGORY" };
        string[] alCaptions = { "Event Date", "Ref. No", "Event Type", "Vessel", "Vessel Type", "Status", "Consequence Category", "Category", "Sub. Category" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionIncidentNearMissAnalysis.QualityIncidentNearMissAnalysis((ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                                      , (ucVesselType.SelectedVesseltype.ToString() == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()))
                                                                                      , (ucPrincipal.SelectedList.ToString() == "," ? null : General.GetNullableString(ucPrincipal.SelectedList.ToString()))
                                                                                      , Convert.ToInt16(ddlYear.SelectedValue)
                                                                                      , selectedQuarterList() == "" ? null : selectedQuarterList()
                                                                                      , (ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                                      , null
                                                                                      , null
                                                                                      , ddlEventType.SelectedValue
                                                                                      , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                      , gvCrew.PageSize
                                                                                      , ref iRowCount
                                                                                      , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=" + ucTitle.Text + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + ucTitle.Text + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        if (ViewState["SUMMARY"].ToString() == "1")
        {
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            Response.Write("<tr>");
            //for (int i = 0; i < alCaptions1.Length; i++)
            //{
            //    Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            //    Response.Write("<b>" + alCaptions1[i] + "</b>");
            //    Response.Write("</td>");
            //}
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
    }

    private void ShowReport()
    {

        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVENDORNAME", "FLDSTOCKTYPE", "FLDFORMNO", "FLDVESSELNAME", "FLDREQUESTSENTDATE", "FLDREQUESTRESPONDDATE", "FLDPOISSUEDDATE", "FLDVENDORDELIVEREDDATE", "FLDDELIVERYTIME", "FLDPOAMOUNTTOTAL" };
        string[] alCaptions = { "Vendor", "Stock Type", "Form No.", "Vessel", "RFQ's Sent On", "RFQ’s Responded On", "PO Issued On", "Delivered On", "Delivery Time(Days)", "PO Total(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        StringBuilder strlist = new StringBuilder();
        foreach (RadComboBoxItem item in ddlYear.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }

        //if (PurchaseFilter.CurrentVendorPerformanceFilter != null)
        //{
        //    NameValueCollection Filter = PurchaseFilter.CurrentVendorPerformanceFilter;

        //    ds = PhoenixInspectionIncidentAnalysis.QualityIncidentAnalysis(General.GetNullableString(Filter.Get("ddlYear").ToString())
        //                                                                  , General.GetNullableString(Filter.Get("Quarter").ToString())
        //                                                                  , General.GetNullableString(Filter.Get("Month").ToString())
        //                                                                  , General.GetNullableString(Filter.Get("txtVendorID").ToString())
        //                                                                  , (General.GetNullableString(Filter.Get("ucFleet").ToString()))
        //                                                                  , (General.GetNullableString(Filter.Get("ucVessel").ToString()))
        //                                                                  , General.GetNullableString(Filter.Get("ddlType"))
        //                                                                  , sortdirection
        //                                                                  , Int32.Parse(ViewState["PAGENUMBER"].ToString())
        //                                                                  , General.ShowRecords(null)
        //                                                                  , ref iRowCount
        //                                                                  , ref iTotalPageCount);
        //}
        //else
        //{
        ds = PhoenixInspectionIncidentNearMissAnalysis.QualityIncidentNearMissAnalysis((ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                                       , (ucVesselType.SelectedVesseltype.ToString() == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()))
                                                                                       , (ucPrincipal.SelectedList.ToString() == "," ? null : General.GetNullableString(ucPrincipal.SelectedList.ToString()))
                                                                                       , Convert.ToInt16(ddlYear.SelectedValue)
                                                                                       , selectedQuarterList() == "" ? null : selectedQuarterList()
                                                                                       , (ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                                       , null
                                                                                       , null
                                                                                       , ddlEventType.SelectedValue
                                                                                       , gvCrew.CurrentPageIndex + 1
                                                                                       , gvCrew.PageSize
                                                                                       , ref iRowCount
                                                                                       , ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Incident Analysis", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvCrew.EditIndex = -1;
    //    gvCrew.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    ShowReport();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrew.SelectedIndex = -1;
    //    gvCrew.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    ShowReport();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}
    //protected void gvCrew_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //}
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReport();
    }
    //protected void gvCrew_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    ShowReport();
    //}
    //protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year); i >= 2010; i--)
        {
            RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    private string YearList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadComboBoxItem item in ddlYear.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);

        }
        return strlist.ToString();
    }

    private string selectedQuarterList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstQuarter.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }


    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ShowReport();
    }
}
