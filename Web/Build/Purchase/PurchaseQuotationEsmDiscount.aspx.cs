using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PurchaseQuotationEsmDiscount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationEsmDiscount.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvEsmdiscount')", "Print", "<i class=\"fas fa-print\"></i>", "Print");
            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();

            toolbargrid.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuBulkDiscount.AccessRights = this.ViewState;
            MenuBulkDiscount.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                lblAddLumpSumDisc.Visible = (showcreditnotedisc == 1) ? true : false;
                txtBulkDiscount.Visible = (showcreditnotedisc == 1) ? true : false;

                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    ViewState["discount"] = Request.QueryString["discount"].ToString();
                    UpdateQuotationLineDiscount();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }

                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                DataSet ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBulkDiscount.Text = ds.Tables[0].Rows[0]["FLDBULKDISCOUNT"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void BulkDiscount_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableDecimal(txtBulkDiscount.Text) == null)
                {
                    ucError.ErrorMessage = "Please give a valid discount";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseQuotation.UpdateQuotationBulkDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["quotationid"].ToString()),
                    General.GetNullableDecimal(txtBulkDiscount.Text));

                ucStatus.Visible = true;
                ucStatus.Text = "Additional Lump sum Disc saved successfully.";

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                ViewState["PAGENUMBER"] = 1;
                gvEsmdiscount.Rebind();
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[11] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[11] {"S.No","Part Number", "Part Name", "Maker Reference", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }
        else
        {
            alColumns = new string[11] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[11] {"S.No","Part Number", "Part Name", "Product Code", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseQuotationLineDiscount.QuotationLineDiscountSearch(ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        //Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Vendor List</h3></td>");
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
    
    private void UpdateQuotationLineDiscount()
    {
        PhoenixPurchaseQuotationLineDiscount.UpdateQuotationLineDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()),
                                                    General.GetNullableDecimal(ViewState["discount"].ToString()));

    }
    
    private void UpdateQuotationlineEsmDiscount(string quotaitionlineid, string discount)
    {
        try
        {
            PhoenixPurchaseQuotationLineDiscount.UpdateQuotationlineEsmDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(quotaitionlineid), new Guid(ViewState["quotationid"].ToString()), 
                General.GetNullableDecimal(discount));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEsmdiscount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[11] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[11] {"S.No","Part Number", "Part Name", "Maker Reference", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }
        else
        {
            alColumns = new string[11] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[11] {"S.No","Part Number", "Part Name", "Product Code", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixPurchaseQuotationLineDiscount.QuotationLineDiscountSearch(ViewState["quotationid"].ToString(), sortexpression, sortdirection, gvEsmdiscount.CurrentPageIndex+1,
            gvEsmdiscount.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        gvEsmdiscount.DataSource = ds;
        gvEsmdiscount.VirtualItemCount = iRowCount;
        

     

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvEsmdiscount", "Quotation Line item", alCaptions, alColumns, ds);
    }

    protected void gvEsmdiscount_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            if (General.GetNullableDecimal(((UserControlDecimal)item.FindControl("txtDiscountEdit")).Text) == null)
            {
                ucError.ErrorMessage = "Discount is required.";
                ucError.Visible = true;
                return;
            }
            UpdateQuotationlineEsmDiscount(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString(),
                    ((UserControlDecimal)item.FindControl("txtDiscountEdit")).Text
                    );
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvEsmdiscount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblComponentName = (RadLabel)item.FindControl("lblComponentName");
            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
            }
        }
    }

    protected void gvEsmdiscount_EditCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        UserControlDecimal txtDiscountEdit = (UserControlDecimal)item.FindControl("txtDiscountEdit");
        //if (txtBulkDiscount != null)
        //    txtDiscountEdit.Focus();
    }

    protected void gvEsmdiscount_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = gvEsmdiscount.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        {
            headerItem["MAKERREF"].Text = "Product Code";
        }
        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        
        gvEsmdiscount.MasterTableView.GetColumn("DISCOUNT").Visible= (showcreditnotedisc == 1) ? true : false;
        gvEsmdiscount.MasterTableView.GetColumn("ACTION").Visible = (showcreditnotedisc == 1) ? true : false;
    }
}
