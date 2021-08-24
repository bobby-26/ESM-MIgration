using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFormLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            MenuMain.AccessRights = this.ViewState;
            MenuMain.Title = "Line Items    (  " + Request.QueryString["refno"] + "     )";
            MenuMain.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                //if (Request.QueryString["orderid"] != null)
                //{
                //    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                //}
                //else
                //{
                //    ViewState["orderid"] = "0";
                //}

                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = {"FLDPARTNUMBER", "FLDNAME","FLDMAKERREF","FLDUNITNAME",
                                 "FLDREQUESTEDQUANTITY","FLDPRICE","FLDSTATUSNAME","FLDRECEIVEDQUANTITY", "FLDORDEREDQUANTITY"};
        string[] alCaptions = {"Part Number", "Part Name","Makerref","Unit",
                                 "Quantity","Price", "Status","Received Quantity","Order Quantity"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPurchaseOrderForm.PurchaseOrderGetStockType(General.GetNullableGuid(Request.QueryString["orderid"].ToString()));
        string curstocktype = Filter.CurrentPurchaseStockType;
        DataSet ds = new DataSet();
        if (dt.Rows.Count > 0)
        {
            Filter.CurrentPurchaseStockType = dt.Rows[0]["FLDSTOCKTYPE"].ToString();
        }
        ds = PhoenixPurchaseOrderLine.OrderLineSearch(General.GetNullableGuid(Request.QueryString["orderid"].ToString()), General.GetNullableInteger(Request.QueryString["vesselid"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);

        Filter.CurrentPurchaseStockType = curstocktype;

        gvLineItem.DataSource = ds;
        gvLineItem.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvLineItem_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
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

}
