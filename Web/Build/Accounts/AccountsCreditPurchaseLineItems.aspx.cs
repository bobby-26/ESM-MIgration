using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;


//using SouthNests.Phoenix.VesselAccounts;

public partial class AccountsCreditPurchaseLineItems : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsCreditPurchaseLineItems.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCreditPurchaseLineItems')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
          //  MenuOffice.SetTrigger(pnlLineItemsEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = null;
                ViewState["ORDERID"] = null;
                gvCreditPurchaseLineItems.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["orderid"] != null && !string.IsNullOrEmpty(Request.QueryString["orderid"].ToString()))
            {
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            }
            if (ViewState["ORDERID"] != null)
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
            
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixAccountsVesselAccounting.EditOrderForm(gOrderId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["EXCHANGERATE"] = dr["FLDEXCHANGERATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();            
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixIntegrationAccounts.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvCreditPurchaseLineItems", title, alCaptions, alColumns, ds);

            gvCreditPurchaseLineItems.DataSource = ds;
            gvCreditPurchaseLineItems.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditPurchaseLineItems_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

 
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixIntegrationAccounts.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
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

    protected void gvCreditPurchaseLineItems_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
    private decimal TotalPrice = 0, LocalPrice = 0, DiscountPrice = 0, ExchangePrice = 0;
    protected void gvCreditPurchaseLineItems_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      
        DataRowView drv = (DataRowView)e.Item.DataItem;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            if (e.Item is GridDataItem)
            {
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalPrice);
            decimal.TryParse(drv["FLDTOTALLOCALPRICE"].ToString(), out LocalPrice);
            decimal.TryParse(drv["FLDDISCOUNTLOCALPRICE"].ToString(), out DiscountPrice);
            decimal.TryParse(drv["FLDEXCHANGEPRICE"].ToString(), out ExchangePrice);
        }
        else if (e.Item is GridFooterItem)
        {
            e.Item.Cells[7].Text = LocalPrice.ToString() + "<br/>" + DiscountPrice.ToString("0.00") + "<br/>" + TotalPrice.ToString() + "<br/>" + ExchangePrice.ToString();
            e.Item.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Item.Font.Bold = true;
        }
    }

    protected void gvCreditPurchaseLineItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCreditPurchaseLineItems.CurrentPageIndex + 1;
        BindData();
    }
}
