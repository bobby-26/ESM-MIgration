using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Accounts;

using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseShippedQuantity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseShippedQuantity.aspx", "Copy Order Qty to Shipped Qty", "copy.png", "COPYORDERQTY");
            toolbargrid.AddButton("Upload", "UPLOAD", ToolBarDirection.Right);
            toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["ORDERID"] = Request.QueryString["orderid"].ToString();
                }
                if (Request.QueryString["VesselId"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VesselId"].ToString();
                }

                if (Request.QueryString["StockType"] != null)
                {
                    ViewState["STOCKTYPE"] = Request.QueryString["StockType"].ToString();
                }
                if (Request.QueryString["Callfrom"] != null)
                {
                    ViewState["Callfrom"] = Request.QueryString["Callfrom"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSupplierlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                GetEditData();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetEditData()
    {
        if (ViewState["ORDERID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["ORDERID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["FLDDTKEY"] = dr["FLDDTKEY"].ToString();
                if (dr["FLDSHIPPEDDATE"].ToString() == "")
                {
                    ucShippeddate.Text = DateTime.Now.Date.ToString();
                }
                else
                {
                    ucShippeddate.Text = dr["FLDSHIPPEDDATE"].ToString();
                }
            }

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet dsattachment = new DataSet();

            dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(ViewState["FLDDTKEY"].ToString()), null, "Invoicecopy", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            if (dsattachment.Tables[0].Rows.Count > 0)
            {
                DataRow drattachment = dsattachment.Tables[0].Rows[0];
                string filepath = drattachment["FLDFILEPATH"].ToString();
                lnkfilename.NavigateUrl = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();
                lnkfilename.Visible = true;
                ViewState["ATTACHMENTDTKEY"] = drattachment["FLDDTKEY"].ToString();
             }
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("COPYORDERQTY"))
            {
                if (ViewState["STOCKTYPE"].ToString() == "SPARE" || ViewState["STOCKTYPE"].ToString() == "SERVICE")
                {

                    PhoenixPurchaseOrderLine.OrderlineshippedqtyBulkUpdate(
                General.GetNullableGuid(ViewState["ORDERID"].ToString())
                 );
                }
                if (ViewState["STOCKTYPE"].ToString() == "STORE")
                {
                    PhoenixPurchaseOrderLine.OrderlineStoreshippedqtyBulkUpdate(
                    General.GetNullableGuid(ViewState["ORDERID"].ToString())
                    );

                }
                Rebind();

            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["Callfrom"].ToString() == "ACCOUNTS")
                {

                    PhoenixPurchaseOrderLine.OrderFormShippeddateUpdate(
                   General.GetNullableGuid(ViewState["ORDERID"].ToString()), General.GetNullableDateTime(ucShippeddate.Text)
                   );
                    PhoenixAccountsInvoice.InvoicePORefresh(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ORDERID"].ToString()));
                }
                else
                {
                    PhoenixPurchaseOrderLine.OrderFormShippeddateUpdate(
                 General.GetNullableGuid(ViewState["ORDERID"].ToString()), General.GetNullableDateTime(ucShippeddate.Text)
                 );
                }
            }
            if (CommandName.ToUpper().Equals("UPLOAD"))
            {
                ViewState["TYPE"] = "Invoicecopy";
                if (ViewState["ATTACHMENTDTKEY"] == null)
                {

                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["FLDDTKEY"].ToString()), PhoenixModule.PURCHASE, null, string.Empty,
                         string.Empty, ViewState["TYPE"].ToString(), int.Parse(ViewState["VESSELID"].ToString()));
                }
            
                else
                {
                    PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["ATTACHMENTDTKEY"].ToString()), PhoenixModule.PURCHASE);
                }
                GetEditData();

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
        gvSupplierlist.SelectedIndexes.Clear();
        gvSupplierlist.EditIndexes.Clear();
        gvSupplierlist.DataSource = null;
        gvSupplierlist.Rebind();
    }
    protected void gvSupplierlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplierlist.CurrentPageIndex + 1;
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

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["STOCKTYPE"].ToString() == "SPARE" || ViewState["STOCKTYPE"].ToString() == "SERVICE")
        {

            ds = PhoenixPurchaseOrderLine.OrderLineSuppliedSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
           General.GetNullableGuid(ViewState["ORDERID"].ToString()),
           sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvSupplierlist.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        else if (ViewState["STOCKTYPE"].ToString() == "STORE")
        {

            ds = PhoenixPurchaseOrderLine.OrderLineStoreSuppliedSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
           General.GetNullableGuid(ViewState["ORDERID"].ToString()),
           sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvSupplierlist.PageSize, ref iRowCount, ref iTotalPageCount);

        }

        gvSupplierlist.DataSource = ds;
        gvSupplierlist.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



    }
    protected void gvSupplierlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;
            if (ViewState["STOCKTYPE"].ToString() == "STORE")
                item["MAKERREF"].Text = "Product Code";
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);


        }

    }
    protected void gvSupplierlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string lblorderlineid = ((RadLabel)e.Item.FindControl("lblorderlineid")).Text;
                string lblorderid = ((RadLabel)e.Item.FindControl("lblorderid")).Text;
                string txtshippedQtyEdit = ((UserControlNumber)e.Item.FindControl("txtshippedQtyEdit")).Text.Replace("_", "0");

                if (ViewState["STOCKTYPE"].ToString() == "SPARE" || ViewState["STOCKTYPE"].ToString() == "SERVICE")
                {

                    PhoenixPurchaseOrderLine.OrderlinesuppliedqtyUpdate(
                General.GetNullableGuid(lblorderlineid),
                General.GetNullableDecimal(txtshippedQtyEdit), General.GetNullableGuid(lblorderid)
                 );
                }
                if (ViewState["STOCKTYPE"].ToString() == "STORE")
                {
                    PhoenixPurchaseOrderLine.OrderlinestoresuppliedqtyUpdate(
                    General.GetNullableGuid(lblorderlineid),
                    General.GetNullableDecimal(txtshippedQtyEdit), General.GetNullableGuid(lblorderid)
                    );

                }
                Rebind();
            }
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

}