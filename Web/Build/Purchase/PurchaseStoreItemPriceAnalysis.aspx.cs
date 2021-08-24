using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Web.UI;


public partial class Purchase_PurchaseStoreItemPriceAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvStore')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            toolbar.AddImageButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseStoreItemPriceAnalysis.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvStore')", "Print Grid", "icon_print.png", "PRINT");


            StoreItemMenu.MenuList = toolbar.Show();
            StoreItemMenu.AccessRights = this.ViewState;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar1.Show();

            //   pageTitle.Text = "Store Item";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                ddlStoreTypeClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                gvStore.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //ShowReport();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // tab strip method
    protected void StoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                var hard = ddlStoreTypeClass.SelectedHard;

                //filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter = null;
                gvStore.DataSource = null;
                gvStore.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtPartName.Text = "";
                txtPartNumber.Text = "";
                ddlStoreTypeClass.SelectedHard = "";
                filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter = null;
                //ShowReport();
                gvStore.DataSource = null;
                gvStore.Rebind();
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

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                //sessionFilterValues();
                Response.Redirect("../Purchase/PurchaseStoreItemPriceVisual.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // grid Events
    protected void gvStore_RowDataBound(object sender, GridItemEventArgs e)
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

    protected void gvStore_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName == "Page")
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

    protected void gvStore_Sorting(object sender, GridSortCommandEventArgs e)
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


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixPurchaseStoreItemPriceAnalysis.PurchaseStoreItemPriceAnalysis(
               null,
               null,
               null,
               General.GetNullableString(txtPartName.Text),
               General.GetNullableString(txtPartNumber.Text),
               General.GetNullableInteger(ddlStoreTypeClass.SelectedHard),
               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvStore.PageSize,
               ref iRowCount,
               ref iTotalPageCount
           );

        Response.AddHeader("Content-Disposition", "attachment; filename=Store Item.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Store Item</center></h5></td></tr>");
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

    //private void ShowReport()
    //{
    //    DataSet ds = new DataSet();
    //    ds = GetData();


    //        gvStore.DataSource = ds;
    //        //gvStore.DataBind();
    //        ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;

    //       ViewState["ROWSINGRIDVIEW"] = 0;

    //}

    private void GetData()
    {
        ViewState["SHOWREPORT"] = 1;
        //  divPage.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        var storeTypeId = ddlStoreTypeClass.SelectedHard == "Dummy" ? "" : ddlStoreTypeClass.SelectedHard;

        if (filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter != null)
            filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter = null;

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtPartName", txtPartName.Text);
        criteria.Add("txtPartNumber", txtPartNumber.Text);
        criteria.Add("txtStoreTypeId", storeTypeId);
        // read from session filter and bind the data
        filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter = criteria;

        NameValueCollection filter = filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter;

        ds = PhoenixPurchaseStoreItemPriceAnalysis.PurchaseStoreItemPriceAnalysis(
               null,
               null,
               null,
               General.GetNullableString(filter.Get("txtPartName")),
               General.GetNullableString(filter.Get("txtPartNumber")),
               General.GetNullableInteger(filter.Get("txtStoreTypeId")),
               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvStore.PageSize,
               ref iRowCount,
               ref iTotalPageCount
           );

        if (txtPartName.Text == "" && txtPartNumber.Text == "" && storeTypeId == "")
        {
            criteria.Clear();
            criteria.Add("txtPartName", txtPartName.Text);
            criteria.Add("txtPartNumber", getFirstStoreItem(ds));
            criteria.Add("txtStoreTypeId", storeTypeId);
            filterPurchase.DefaultPurchaseStoreItemAnalysisPriceFilter = criteria;
        }
        else
        {
            criteria.Clear();
            criteria.Add("txtPartName", txtPartName.Text);
            criteria.Add("txtPartNumber", txtPartNumber.Text);
            criteria.Add("txtStoreTypeId", storeTypeId);
            filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter = criteria;
        }


        General.SetPrintOptions("gvStore", "Store Price Analysis", alCaptions, alColumns, ds);
        gvStore.DataSource = ds;
        gvStore.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private string[] getColumns()
    {
        string[] alColumns = { "FLDITEMNUMBER", "FLDITEMNAME", "FLDVENDORNAME", "FLDPORTNAME", "FLDQUANTITY", "FLDUNITNAME", "FLDSINGLEUNITPRICE", "FLDPRICE", "FLDDATEOFPURCHASE" };
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "Number", "Name", "Vendor", "Port", "Quantity", "Unit", "Price (USD)", "Total Price (USD)", "Date of Purchase" };
        return alCaptions;
    }

    // popup method
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvStore.DataSource = null;
        gvStore.Rebind();
    }


    private string getFirstStoreItem(DataSet ds)
    {
        string id = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            id = row["FLDITEMNUMBER"].ToString();
        }

        return id;
    }


    protected void gvStore_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStore.CurrentPageIndex + 1;
            GetData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
