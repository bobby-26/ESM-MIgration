using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Portal;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PortalPurchaseVendorQuotationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Forms", "FORMS");
        toolbar.AddButton("Quotations", "QUOTATIONS");

        MenuMain.MenuList = toolbar.Show();
        MenuMain.SelectedMenuIndex = 0;

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Portal/PortalPurchaseVendorQuotationList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuFilter.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {          
            ucVessel.bind();
            ucVessel.DataBind();
            //ViewState["PAGENUMBER"] = 1;
            rgvLine.CurrentPageIndex = 0;
            rgvLine.PageSize = General.ShowRecords(null);

            BindFilter();
        }
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FORMS"))
            {
                //if (ViewState["orderid"] != null)
                //    ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx?orderid=" + ViewState["orderid"].ToString();
                //else
                //    ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx";

            }
            if (CommandName.ToUpper().Equals("QUOTATIONS"))
            {
                if (Filter.CurrentPurchaseSelectedQuotationId != null)
                    Response.Redirect("../Portal/PortalPurchaseQuotationRFQ.aspx?SESSIONID=" + Filter.CurrentPurchaseSelectedQuotationId.ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("txtFormNo", txtFormNO.Text);
        criteria.Add("txtTitle", txtTitle.Text);

        Filter.CurrentPortalQuotationFilter = criteria;

        rgvLine.Rebind();
    }
    protected void MenuFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("txtFormNo", txtFormNO.Text);
                criteria.Add("txtTitle", txtTitle.Text);
                
                Filter.CurrentPortalQuotationFilter = criteria;

                rgvLine.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindFilter()
    {
        if (Filter.CurrentPortalQuotationFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentPortalQuotationFilter;

            ucVessel.SelectedVessel = General.GetNullableInteger(nvc.Get("ucVessel").ToString()) != null ? nvc.Get("ucVessel").ToString() : "Dummy";
            txtFormNO.Text = nvc.Get("txtFormNo").ToString();
            txtTitle.Text = nvc.Get("txtTitle").ToString();
        }
    }
    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 10;
        int iTotalPageCount = 0;

        string deliveryid = ViewState["deliveryid"] != null ? ViewState["deliveryid"].ToString() : "";
        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentPortalQuotationFilter;

        ds = PhoenixPortalPurchaseVendorQuotations.VendorQuotationList(nvc != null ? General.GetNullableInteger(nvc.Get("ucVessel").ToString()) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtFormNo").ToString()) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtTitle").ToString()) : null
            , rgvLine.CurrentPageIndex + 1
            , rgvLine.PageSize,
            ref iRowCount, ref iTotalPageCount
            );

        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        rgvLine.SelectedIndexes.Clear();
        if (Filter.CurrentPurchaseOrderIdSelection != null)
        {
            foreach (GridDataItem item in rgvLine.Items)
            {
                if (item.GetDataKeyValue("FLDORDERID").ToString().Equals(Filter.CurrentPurchaseOrderIdSelection.ToString()))
                {
                    rgvLine.SelectedIndexes.Add(item.ItemIndex);
                    break;

                }
            }
        }
    }

    
    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            rgvLine.SelectedIndexes.Add(item.ItemIndex);
            Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
            Filter.CurrentPurchaseOrderIdSelection = ((RadLabel)item.FindControl("lblOrderId")).Text;
            Filter.CurrentPurchaseSelectedQuotationId = ((RadLabel)item.FindControl("lblQuotationId")).Text;

            Response.Redirect("../Portal/PortalPurchaseQuotationRFQ.aspx?SESSIONID=" + Filter.CurrentPurchaseSelectedQuotationId.ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString());

        }
    }
}