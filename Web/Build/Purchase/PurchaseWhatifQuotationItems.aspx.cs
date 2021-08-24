using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class PurchaseWhatifQuotationItems : PhoenixBasePage
{

   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            MenuQuotationLineItem.Title = "Quotation Details [" + PhoenixPurchaseOrderForm.FormNumber + "]";
            toolbarmain.AddButton("Confirm", "CONFIRM",ToolBarDirection.Right);
            MenuQuotationLineItem.AccessRights = this.ViewState;  
            MenuQuotationLineItem.MenuList = toolbarmain.Show();
            
          
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseWhatifQuotationItems.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVendorItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "");

            MenuRegistersStockItem.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                gvVendorItem.PageSize = General.ShowRecords(null);
                UCDeliveryTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();

                if (Request.QueryString["STOCKTYPE"] != null)
                {
                    Filter.CurrentPurchaseStockType = Request.QueryString["STOCKTYPE"].ToString();
                }
                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                BindVendorInfo();
                EnableFalse();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private void EnableFalse()
    {
            txtExpirationDate.Enabled = false;           
            txtVendorName.Enabled = false;
            txtVendorAddress.Enabled = false;
            txtOrderDate.Enabled = false;
            txtVenderReference.Enabled = false;
            txtPriority.Enabled = false;            
            UCDeliveryTerms.Enabled = false;
            UCPaymentTerms.Enabled = false;
            //txtVendorRemarks.Enabled = false;
            ucVendorRemarks.Enabled = false;
    }

     private void BindVendorInfo()
    {
        DataSet dsVendor = PhoenixPurchaseQuotation.QuotationHeader(new Guid(ViewState["quotationid"].ToString()));
        if (dsVendor.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVendor.Tables[0].Rows[0];

            txtExpirationDate.Text = General.GetDateTimeToString(dr["FLDEXPIRYDATE"].ToString());
            
            txtVendorName.Text = dr["FLDNAME"].ToString();
            txtVendorAddress.Text = dr["FLDVENDORADDRESS"].ToString();
            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDERBEFOREDELIVERYDATE"].ToString());
            txtVenderReference.Text = dr["FLDVENDORQUOTATIONREF"].ToString();
            txtPriority.Text = dr["FLDORDERPRIORITY"].ToString();
            UCDeliveryTerms.SelectedQuick = dr["FLDVENDORDELIVERYTERMID"].ToString();
            UCPaymentTerms.SelectedQuick = dr["FLDVENDORPAYMENTTERMID"].ToString();
            //txtVendorRemarks.Text = dr["FLDVENDORREMARKS"].ToString();
            ucVendorRemarks.Text = dr["FLDVENDORREMARKS"].ToString();
          }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDROWNUMBER","FLDNUMBER", "FLDNAME","FLDUNITNAME","FLDORDEREDQUANTITY","FLDQUANTITY",
                                 "FLDQUOTEDPRICE","TOTALQUOTEDPRICE","FLDWHATIFQUANTITY","TOTALREVICEDPRICE" };
        string[] alCaptions = {"S.No","Part Number", "Part Name","Unit","Order Qty","Quoted Qty",
                                 "Unit Price","Amount","What If Qty", "Review Amount" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseQuotationLine.WhatIfQuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");

        Response.Write("<td><h3>StockItem Register</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void BindDataFooterAmont(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            ViewState["TOTALAMOUNT"] = dt.Rows[0]["TOTALAMOUNT"].ToString();
            ViewState["TOTALREVIEWAMOUNT"] = dt.Rows[0]["TOTALREVIEWAMOUNT"].ToString();
        }
        else
        {
            ViewState["TOTALAMOUNT"]        =   0;
            ViewState["TOTALREVIEWAMOUNT"]  =   0;
        }
    }

   
    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvVendorItem.Rebind();
            }
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




    private void UpdateQuotationLineItem(string quotaitionlineid, string quantity)
    {
      //new Guid(ViewState["quotationid"].ToString()),
        PhoenixPurchaseQuotationLine.UpdateQuotationLineWhatIfQuantity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(quotaitionlineid),  General.GetNullableDecimal(quantity));

    }

    private bool IsValidForm( string quantity)
    {
        if (quantity.Trim().Equals(""))
            ucError.ErrorMessage = "Quantity  is required.";
        if (General.GetNullableGuid(ViewState["quotationid"].ToString()) == null)
            ucError.ErrorMessage = "Quotationid is required.";
        return (!ucError.IsError);
    }

  
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                UpdateQuotation();
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
             }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateQuotation()
    {
       PhoenixPurchaseQuotation.QuotationUpadeWhatifConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()));
    }
    

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    protected void gvVendorItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = {"FLDROWNUMBER","FLDNUMBER", "FLDNAME","FLDUNITNAME","FLDORDEREDQUANTITY","FLDQUANTITY",
                                 "FLDQUOTEDPRICE","TOTALQUOTEDPRICE","FLDWHATIFQUANTITY","TOTALREVICEDPRICE" };
        string[] alCaptions = {"S.No","Part Number", "Part Name","Unit","Order Qty","Quoted Qty",
                                 "Unit Price","Amount","What If Qty", "Review Amount" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixPurchaseQuotationLine.WhatIfQuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, gvVendorItem.CurrentPageIndex+1,
            gvVendorItem.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        gvVendorItem.DataSource = ds;
        gvVendorItem.VirtualItemCount = iRowCount;
        BindDataFooterAmont(ds.Tables[1]);
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvVendorItem", "Vendor Item List", alCaptions, alColumns, ds);
    }

    protected void gvVendorItem_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        if (!IsValidForm(((UserControlDecimal)item.FindControl("txtWhatIfQtyEdit")).Text))
        {
            ucError.Visible = true;
            return;
        }

        UpdateQuotationLineItem(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString(),
                ((UserControlDecimal)item.FindControl("txtWhatIfQtyEdit")).Text);
    }

    protected void gvVendorItem_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblComponentName = (RadLabel)item.FindControl("lblComponentName");
            RadLabel lblvesselid = (RadLabel)item.FindControl("lblVesselid");
            RadLabel lblcomponentid = (RadLabel)item.FindControl("lblComponentId");
            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
            }


            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    Label ReviewTotalAmount = (Label)e.Row.FindControl("lblReviewTotalAmount");
            //    Label TotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
            //    if (ReviewTotalAmount != null)
            //        ReviewTotalAmount.Text = decimal.Parse(ViewState["TOTALREVIEWAMOUNT"].ToString()).ToString("######0.00");
            //    if (TotalAmount != null)
            //        TotalAmount.Text = decimal.Parse(ViewState["TOTALAMOUNT"].ToString()).ToString("######0.00");
            //}
        }
    }
}
