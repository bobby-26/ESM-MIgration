using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Web.UI;

public partial class Purchase_PurchaseSpareItemDeliveryAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSpare')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            toolbar.AddImageButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvSpare')", "Print Grid", "icon_print.png", "PRINT");

            SpareItemMenu.MenuList = toolbar.Show();
            SpareItemMenu.AccessRights = this.ViewState;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            //toolbar1.AddButton("Show Report", "SHOWREPORT");
            toolbar1.AddButton("Visual", "SHOWVISUAL",ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar1.Show();
            txtComponentID.Attributes.Add("style", "visibility:hidden");

         //   pageTitle.Text = "Spare Item Delivery Analysis";
            if (!IsPostBack)

            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                gvSpare.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    // tab strip method
    protected void SpareItem_TabStripCommand(object sender, EventArgs e)
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
                ShowReport();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtPartName.Text = "";
                txtPartNumber.Text = "";
                txtComponent.Text = "";
                txtComponentName.Text = "";
                txtComponentID.Text = "";

                ClearFilter();
                ShowReport();
                gvSpare.Rebind();
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
                Response.Redirect("../Purchase/PurchaseSpareItemDeliveryAnalysisVisual.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // grid Events
    protected void gvSpare_RowDataBound(object sender, GridItemEventArgs e)
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

    protected void gvSpare_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if(e.CommandName=="Page")
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

    protected void gvSpare_Sorting(object sender, GridSortCommandEventArgs e)
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

        DataSet ds = new DataSet();
        ds = GetData();

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        Response.AddHeader("Content-Disposition", "attachment; filename=Spare Item Delivery Analysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Spare Item Delivery Analysis</center></h5></td></tr>");
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

    private void ShowReport()
    {
        DataSet ds = new DataSet();
        ds = GetData();


            gvSpare.DataSource = ds;
          //  gvSpare.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
     
           ViewState["ROWSINGRIDVIEW"] = 0;
    }

    private DataSet GetData()
    {
        ViewState["SHOWREPORT"] = 1;
       // divPage.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter != null)
        {

            NameValueCollection filter = filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter;
            txtPartName.Text = filter.Get("txtPartName");
            txtPartNumber.Text = filter.Get("txtPartNumber");
            txtComponentID.Text = filter.Get("txtComponentID");
            txtComponent.Text = filter.Get("txtComponent");
            txtComponentName.Text = filter.Get("txtComponentName");

            ds = PhoenixPurchaseAnalytics.PurchaseSpareItemDeliveryAnalysis(
               General.GetNullableGuid(filter.Get("txtComponentID")),
               General.GetNullableString(filter.Get("txtPartName")),
               General.GetNullableString(filter.Get("txtPartNumber")),
               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvSpare.PageSize,
               ref iRowCount,
               ref iTotalPageCount
           );
        }
        else
        {
            ds = PhoenixPurchaseAnalytics.PurchaseSpareItemDeliveryAnalysis(
                      General.GetNullableGuid(txtComponentID.Text),
                      General.GetNullableString(txtPartName.Text),
                      General.GetNullableString(txtPartNumber.Text),
                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                      gvSpare.PageSize,
                      ref iRowCount,
                      ref iTotalPageCount
                  );

            // assign filter value
            AssignDefaultFilter(ds);
        }

        General.SetPrintOptions("gvSpare", "Crew Contract", alCaptions, alColumns, ds);
        gvSpare.DataSource = ds;
        gvSpare.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        return ds;
    }

    private string[] getColumns()
    {
        string[] alColumns = { "FLDITEMNAME", "FLDITEMNUMBER", "FLDCOMPONENTNAME", "FLDVENDORNAME", "FLDPORTNAME", "FLDQUANTITY", "FLDUNITNAME", "FLDPRICE", "FLDTOTALPRICE", "FLDORDERDDATE", "FLDDELIVERYDATE", "FLDDURATION" };
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "Item Name", "ItemNo", "Component Name", "Vendor", "Port", "Quantity", "Unit", "Price(USD)", "Total Price(USD)", "Purchase", "Received", "Delivered" };
        return alCaptions;
    }

    // popup method
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ShowReport();
    }

    private DataRow GetDefaultData(DataSet ds)
    {
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0];
        }

        return null;
    }

    private void ClearFilter()
    {
        filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter = null;
    }

    private void AssignDefaultFilter(DataSet ds)
    {

        // read from session filter and bind the data
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        if (String.IsNullOrEmpty(txtComponentID.Text) && String.IsNullOrEmpty(txtPartName.Text) && String.IsNullOrEmpty(txtPartNumber.Text))
        {
            DataRow row = GetDefaultData(ds);
            if (row != null)
            {
                criteria.Add("txtComponentID", null);
                criteria.Add("txtPartName", row["FLDITEMNAME"].ToString());
                criteria.Add("txtPartNumber", row["FLDITEMNUMBER"].ToString());
                filterPurchase.DefaultPurchaseSpareItemDeliveryAnalysisFilter = criteria;
            }
        }
        else
        {
            criteria.Add("txtComponentID", txtComponentID.Text);
            criteria.Add("txtPartName", txtPartName.Text);
            criteria.Add("txtPartNumber", txtPartNumber.Text);
            criteria.Add("txtComponent", txtComponent.Text);
            criteria.Add("txtComponentName", txtComponentName.Text);

            filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter = criteria;
        }
    }

    public void PopulateSP()
    {
        PhoenixPurchaseAnalytics.PurchaseSpareItemDeliveryPopulate();
    }


    protected void gvSpare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSpare.CurrentPageIndex + 1;
            GetData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}