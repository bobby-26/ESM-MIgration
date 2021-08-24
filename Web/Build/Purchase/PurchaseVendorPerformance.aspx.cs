using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Purchase_PurchaseVendorPerformance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            toolbar.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Purchase/PurchaseVendorPerformance.aspx", "Find", "search.png", "FIND");
            toolbar1.AddImageButton("../Purchase/PurchaseVendorPerformance.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageButton("../Purchase/PurchaseVendorPerformance.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            //gvCrew.PageSize = General.ShowRecords(gvCrew.PageSize);

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            BindYear();
            sessionFilterValues();
            // ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    // bind filter criteria if any
    //    BindFilterCriteria();
    //}

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
                //  ucTitle.Text = "Vendor Performance";
                ucVessel.SelectedVesselValue = "";
                ucFleet.SelectedFleetValue = "";
                ddlYear.SelectedValue = System.DateTime.Today.Year.ToString();
                ddlType.SelectedValue = "";
                txtVendorId.Text = "";
                txtVendorCode.Text = "";
                txtVendorName.Text = "";

                filterPurchase.CurrentVendorPerformanceFilter = null;
                Rebind();

            }
            if (CommandName.ToUpper().Equals("FIND"))
            {

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
                filterPurchase.CurrentVendorPerformanceFilter = null;
                sessionFilterValues();
            }

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                sessionFilterValues();
                Response.Redirect("../Purchase/PurchaseVendorperformanceDataVisual.aspx");
            }

            ViewState["PAGENUMBER"] = 1;
            gvCrew.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = filterPurchase.CurrentVendorPerformanceFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }

        string Year = CheckIsNull(nvc.Get("ddlYear"));
        CheckUncheck(Year, "ddlYear");
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ddlType.SelectedValue = CheckIsNull(nvc.Get("ddlType"));
        string Quarter = CheckIsNull(nvc.Get("Quarter"));
        CheckUncheck(Quarter, "Quarter");
        txtVendorId.Text = CheckIsNull(nvc.Get("txtVendorID"));
        txtVendorName.Text = CheckIsNull(nvc.Get("txtVendorName"));
        txtVendorCode.Text = CheckIsNull(nvc.Get("txtVendorCode"));
        //gvCrew.Rebind();
    }

    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }

    public NameValueCollection sessionFilterValues()
    {
        NameValueCollection nvc = new NameValueCollection();

        if (IsPostBack)
        {
            nvc.Clear();
            nvc.Add("ddlYear", YearList());
            nvc.Add("ucFleet", ucFleet.SelectedFleetValue.ToString());
            nvc.Add("ucVessel", ucVessel.SelectedVessel.ToString());
            nvc.Add("txtVendorID", txtVendorId.Text);
            nvc.Add("txtVendorName", txtVendorName.Text);
            nvc.Add("txtVendorCode", txtVendorCode.Text);
            nvc.Add("ddlType", ddlType.SelectedValue);
            nvc.Add("ddlTypeName", ddlType.SelectedItem.ToString());
            nvc.Add("Quarter", "");
            nvc.Add("Month", "");
            filterPurchase.CurrentVendorPerformanceFilter = nvc;

        }
        return filterPurchase.CurrentVendorPerformanceFilter;
    }

    protected void CheckUncheck(string Values, string ID)
    {
        string[] values = Values.Split(',');
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].Trim();
            if (ID == "ddlYear")
            {
                if (values[i] == "--Select--" || values[i] == "")
                    return;
                //ddlYear.Items.FindByValue(values[i]).Selected = true;
            }
            //else if (ID == "ucFleet")
            //    ucFleet.SelectedFleetValue = Values[i].ToString();

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVENDORNAME", "FLDSTOCKTYPE", "FLDFORMNO", "FLDVESSELNAME", "FLDREQUESTSENTDATE", "FLDREQUESTRESPONDDATE", "FLDPOISSUEDDATE", "FLDVENDORDELIVEREDDATE", "FLDDELIVERYTIME", "FLDPOAMOUNTTOTAL" };
        string[] alCaptions = { "Vendor", "Stock Type", "Form No.", "Vessel", "RFQ's Sent On", "RFQ's Responded On", "PO Issued On", "Delivered On", "Delivery Time(Days)", "PO Total(USD)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (filterPurchase.CurrentVendorPerformanceFilter != null)
        {
            NameValueCollection Filter = filterPurchase.CurrentVendorPerformanceFilter;

            ds = PhoenixPurchaseVendorPerformance.VendorPerformance(YearList()
                                                                  , null
                                                                  , null
                                                                  , General.GetNullableString(Filter.Get("txtVendorID").ToString())
                                                                  , (General.GetNullableString(Filter.Get("ucFleet").ToString()))
                                                                  , (General.GetNullableString(Filter.Get("ucVessel").ToString()))
                                                                  , General.GetNullableString(Filter.Get("ddlType"))
                                                                  , sortdirection
                                                                  , 1
                                                                  , iRowCount
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseVendorPerformance.VendorPerformance(YearList()
                                                                      , null
                                                                      , null
                                                                      , txtVendorId.Text == "" ? null : txtVendorId.Text
                                                                      , (ucFleet.SelectedFleetValue.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedFleetValue.ToString())
                                                                      , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                      , General.GetNullableString(ddlType.SelectedValue)
                                                                      , sortdirection
                                                                      , 1
                                                                      , iRowCount
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Vendor Performance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Vendor Performance</center></h5></td></tr>");
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
        //divPage.Visible = true;
        //divTab1.Visible = true;

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
        foreach (RadListBoxItem item in ddlYear.Items)
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

        if (filterPurchase.CurrentVendorPerformanceFilter != null)
        {
            NameValueCollection Filter = filterPurchase.CurrentVendorPerformanceFilter;

            ds = PhoenixPurchaseVendorPerformance.VendorPerformance(General.GetNullableString(Filter.Get("ddlYear").ToString())
                                                                  , General.GetNullableString(Filter.Get("Quarter").ToString())
                                                                  , General.GetNullableString(Filter.Get("Month").ToString())
                                                                  , General.GetNullableString(Filter.Get("txtVendorID").ToString())
                                                                  , (General.GetNullableString(Filter.Get("ucFleet").ToString()))
                                                                  , (General.GetNullableString(Filter.Get("ucVessel").ToString()))
                                                                  , General.GetNullableString(Filter.Get("ddlType"))
                                                                  , sortdirection
                                                                  , gvCrew.CurrentPageIndex+1
                                                                  , gvCrew.PageSize
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseVendorPerformance.VendorPerformance(YearList()
                                                                   , null
                                                                   , null
                                                                   , txtVendorId.Text == "" ? null : txtVendorId.Text
                                                                   , (ucFleet.SelectedFleetValue.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedFleetValue.ToString())
                                                                   , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                   , General.GetNullableString(ddlType.SelectedValue)
                                                                   , sortdirection
                                                                   , gvCrew.CurrentPageIndex+1
                                                                   , gvCrew.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount);
        }

        General.SetPrintOptions("gvCrew", "Vendor Performance", alCaptions, alColumns, ds);


        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


        // gvCrew.DataBind();
      //  ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;

     //   ViewState["ROWSINGRIDVIEW"] = 0;

        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;
        switch (se.NewSortOrder)
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
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

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

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtVendorCode.Text = "";
        txtVendorId.Text = "";
        txtVendorName.Text = "";
    }

    protected void BindYear()
    {
        //for (int i = 2010; i <= (DateTime.Today.Year); i++)
        //{
        //    ListItem li = new ListItem(i.ToString(), i.ToString());
        //    ddlYear.Items.Add(li);
        //}
        //ddlYear.DataBind();
        //ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        int index = 1;
        ddlYear.Items.Insert(0, new RadListBoxItem("--Select--", ""));
        for (int i = (DateTime.Today.Year); i >= 2010; i--)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Insert(index, new RadListBoxItem(li.ToString(), li.ToString()));
            index++;
        }

        if (filterPurchase.CurrentVendorPerformanceFilter == null)
        {
            if (ddlYear.SelectedValue == "")
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }
    }

    private string YearList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in ddlYear.Items)
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

    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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

