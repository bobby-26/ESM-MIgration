using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Telerik.Web.UI;
using System.Web;

public partial class PurchaseQuotationVendorItems : PhoenixBasePage
{ 
    public string strTotalAmount = string.Empty;
    protected void Page_Prerender(object sender, EventArgs e)
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
       
            if (!IsPostBack)
            {
                rgvLine.PageSize = General.ShowRecords(null);
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Save", "SAVE");
                toolbarmain.AddButton("Vessel Form", "FORM",ToolBarDirection.Left);
                toolbarmain.AddButton("Vendor Quotations", "QTNDETAILS",ToolBarDirection.Left);
                toolbarmain.AddButton("Line Items", "LINEITEM",ToolBarDirection.Left);
                toolbarmain.AddButton("Split Form", "SPLIT",ToolBarDirection.Left);
                MenuQuotationLineItem.AccessRights = this.ViewState;  
                MenuQuotationLineItem.MenuList = toolbarmain.Show();
                MenuQuotationLineItem.SelectedMenuIndex = 2;
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                
                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                ViewState["vendorname"] = "";

                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["pageno"] = Request.QueryString["pageno"].ToString(); 
                BindVendorInfo();

            }
            PhoenixToolbar toolSave = new PhoenixToolbar();
            menuSaveDetails.Title = "Quotation Lineitem [ " + PhoenixPurchaseOrderForm.FormNumber + " Vendor : " + ViewState["vendorname"].ToString() + " ]";
            if (Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "STORE")
            {
                toolSave.AddButton("Contract Price", "CONTRACTPRICE", ToolBarDirection.Right);
            }
            toolSave.AddButton("Save", "SAVE", ToolBarDirection.Right);
            menuSaveDetails.AccessRights = this.ViewState;
            menuSaveDetails.MenuList = toolSave.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorItems.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvLine')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Report','','Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=VENDORQUOTATION&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "'); return false;", "RFQ", "<i class=\"fas fa-receipt\"></i>", "ORDER");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Purchase/PurchaseQuotationLineItemBulkSave.aspx?quotationid="
                + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&VendorName="+ ViewState["vendorname"].ToString() + "','true')", "Save", "<i class=\"fa fa-database\"></i>", "BULKSAVE");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Split','','Purchase/PurchaseSplitByQuantity.aspx?quotationid="
                + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() +"','true')", "Split", "<i class=\"fa fa-sitemap\"></i>", "SPLIT");
            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();
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
            alColumns = new string[16] {"FLDROWNUMBER","FLDNUMBER", "FLDMAKERNAME", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE","FLDITEMTYPENAME", "FLDDELIVERYTIME", "FLDNOTES", "FLDVENDORNOTES" };
            alCaptions = new string[16] {"S.No","Part Number", "Maker","Part Name", "Maker Reference","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price","Item Type", "Del. Time (Days)", "Vessel Details", "Vendor Remarks" };
        }
        else
        {
            alColumns = new string[16] {"FLDROWNUMBER","FLDNUMBER", "FLDMAKERNAME", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE","FLDITEMTYPENAME", "FLDDELIVERYTIME", "FLDNOTES", "FLDVENDORNOTES" };
            alCaptions = new string[16] {"S.No","Part Number", "Maker", "Part Name", "Product Code","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price","Item Type", "Del. Time (Days)", "Vessel Details", "Vendor Remarks" };
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

        ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorLineItem - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        //Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td colspan='8'><h4>Quotation Lineitem [" + PhoenixPurchaseOrderForm.FormNumber + "   Vendor :   " + PhoenixPurchaseQuotation.VendorName + "     ]</h4></td>");
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
                Response.Write((alColumns[i] == "FLDNOTES" || alColumns[i] == "FLDVENDORNOTES") ? RemoveHTMLTags(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    /// <summary>
    /// Remove HTML tags from string.
    /// </summary>
    public static string RemoveHTMLTags(string source)
    {
        HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
        htmlDiv.InnerHtml = source;
        String plainText = htmlDiv.InnerText;

        return plainText;
    }

    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FORM"))
            {
                if (ViewState["orderid"] != null)
                    Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
            }
           
            if (CommandName.ToUpper().Equals("QTNDETAILS"))
            {
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + ViewState["pageno"].ToString());
                }
                //if (ViewState["orderid"] != null)
                //    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                //else
                //    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + ViewState["pageno"].ToString());
            }
            else if (CommandName.ToUpper().Equals("SPLIT"))
            {
                GetSelectedCheckbox();

                List<String> SelectedPvs = new List<String>();

                string selectedline = ",";

                if (ViewState["SelectedLineItems"] != null)
                {
                    SelectedPvs = (List<String>)ViewState["SelectedLineItems"];

                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (string line in SelectedPvs)
                        {
                            selectedline = selectedline + line + ","; 
                        }
                    }
                }


                if (SelectedPvs.Count == rgvLine.VirtualItemCount)
                {
                    ucError.ErrorMessage = "You have selected all items, Can not split all items.";
                    ucError.Visible = true;
                    MenuQuotationLineItem.SelectedMenuIndex = 1;
                    return;
                }

                if (selectedline.Length > 1)
                    Response.Redirect("../Purchase/PurchaseFormSplit.aspx?orderid=" + ViewState["orderid"].ToString() + "&orderline=" + selectedline + "&quotationid=" + ViewState["quotationid"].ToString());
                else
                {
                    ucError.ErrorMessage = "You have not selected any items, Please select item for split.";
                    ucError.Visible = true;
                    MenuQuotationLineItem.SelectedMenuIndex = 1;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRemark())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateVendorNotes();
                rgvLine.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CONTRACTPRICE"))
            {
                DataSet ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    string port = dr["FLDPORT"].ToString();

                    String scriptpopup = String.Format(
                        "javascript:parent.Openpopup('codehelp1', '', 'PurchaseContractPriceList.aspx?quotationid=" + ViewState["quotationid"].ToString() +
                        "&vendorid=" + ViewState["vendorid"].ToString() + "&portid=" + port + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[13] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE","FLDITEMTYPENAME", "FLDDELIVERYTIME" };
            alCaptions = new string[13] {"S.No","Part Number", "Part Name", "Maker Reference","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price","Ïtem Type", "Del. Time (Days)" };
        }
        else
        {
            alColumns = new string[13] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE","FLDITEMTYPENAME", "FLDDELIVERYTIME" };
            alCaptions = new string[13] {"S.No","Part Number", "Part Name", "Product Code","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price","Ïtem Type", "Del. Time (Days)" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, rgvLine.CurrentPageIndex+1,
            rgvLine.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["quotationlineid"] == null)
            {
                ViewState["quotationlineid"] = ds.Tables[0].Rows[0]["FLDQUOTATIONLINEID"].ToString();
                bindQuotationLine();
            }
            strTotalAmount = String.Format("{0:0.00}", ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString());

        }
       
        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("rgvLine", "Order Line Item - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
        if (!SessionUtil.CanAccess(this.ViewState, "CONTRACTEXISTS"))
        {
            imgContractExists.Visible = false;
            Label1.Visible = false;
        }
    }
    private void BindVendorInfo()
    {
        if (ViewState["quotationid"].ToString() != "")
        {
            DataSet dsVendor = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));
            if (dsVendor.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVendor.Tables[0].Rows[0];
                ucCurrency.SelectedCurrency = dr["FLDQUOTEDCURRENCYID"].ToString();
                ViewState["vendorid"] = dr["FLDVENDORID"].ToString();
                ViewState["vendorname"] = dr["FLDNAME"].ToString();
            }
        }
    }
    private void UpdateQuotationLineItem(string quotaitionlineid, string quantity, string rate, string discount, string deliveryitem,string unitid,string itemtype)
    {
        try
        {
            PhoenixPurchaseQuotationLine.UpdateQuotationLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(quotaitionlineid), new Guid(ViewState["quotationid"].ToString()), General.GetNullableDecimal(quantity), General.GetNullableDecimal(rate)
                , General.GetNullableDecimal(discount), General.GetNullableDecimal(deliveryitem)
                ,General.GetNullableInteger(unitid)
                , General.GetNullableInteger(itemtype));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                PhoenixPurchaseQuotationLine.DeleteQuotationLine(1, new Guid(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString()));
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                rgvLine.SelectedIndexes.Clear();
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                ViewState["quotationlineid"] = item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString();
                rgvLine.SelectedIndexes.Add(item.ItemIndex);
                bindQuotationLine();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRate(string rate, string quantity, string discount)
    {

        if (rate.Trim().Equals("") || rate == "0")
            ucError.ErrorMessage = "Item Rate is required.";
        if (quantity.Trim().Equals("") || quantity == "0")
            ucError.ErrorMessage = "Quantity  is required.";
        if (General.GetNullableGuid(ViewState["quotationid"].ToString()) == null)
            ucError.ErrorMessage = "Quotation is required.";
        if (General.GetNullableDecimal(quantity) < 0)
            ucError.ErrorMessage = "Quoted Qty cannot be negative value.";
        if (General.GetNullableDecimal(rate) < 0)
            ucError.ErrorMessage = "Price cannot be negative value.";
        if (General.GetNullableDecimal(discount) < 0)
            ucError.ErrorMessage = "Discount cannot be negative value.";
            return (!ucError.IsError);
    }
    protected void rgvLine_InsertCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void rgvLine_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        rgvLine.Rebind();
    }
    protected void rgvLine_EditCommand(object sender, GridCommandEventArgs e)
    {
        rgvLine.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;
        ViewState["quotationlineid"] = item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString();
        rgvLine.SelectedIndexes.Add(e.Item.ItemIndex);
    }
    protected void rgvLine_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem item = (GridEditableItem)e.Item;

        if (!IsValidRate(((UserControlDecimal)item["PRICE"].FindControl("txtQuotedPriceEdit")).Text, ((UserControlDecimal)item["QUOTEDQTY"].FindControl("txtQuantityEdit")).Text, ((UserControlDecimal)item["Discount"].FindControl("txtDiscountEdit")).Text))
        {
            ucError.Visible = true;
            return;
        }
        UpdateQuotationLineItem(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString(),
                ((UserControlDecimal)item["QUOTEDQTY"].FindControl("txtQuantityEdit")).Text.Replace("_", "0"),
                ((UserControlDecimal)item["PRICE"].FindControl("txtQuotedPriceEdit")).Text.Replace("_", "0")
                , ((UserControlDecimal)item["Discount"].FindControl("txtDiscountEdit")).Text.Replace("_", "0")
                , ((UserControlDecimal)item["DELTIME"].FindControl("txtDeliveryTimeEdit")).Text
                , ((UserControlPurchaseUnit)item["UNIT"].FindControl("ucUnit")).SelectedUnit
                , ((UserControlHard)item["ITEMTYPE"].FindControl("ddlType")).SelectedHard
                );

        rgvLine.Rebind();
    }
    protected void rgvLine_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void rgvLine_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = rgvLine.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
            headerItem["MAKERREF"].Text = "Product Code";
    }
    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;
            if (item["TOTAL"].HasControls())
            {
                LinkButton lButton = item["TOTAL"].Controls[0] as LinkButton;
                if (lButton != null)
                    lButton.Text = "Total (" + strTotalAmount + ")";
            }

            
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            LinkButton db = (LinkButton)item["Action"].FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cmdEdit = (LinkButton)item["Action"].FindControl("cmdEdit");
            if (cmdEdit != null)
            {
                 cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            }

            LinkButton cmdUpdate = (LinkButton)item["Action"].FindControl("cmdUpdate");
            if (cmdUpdate != null)
            {
                cmdUpdate.Visible = SessionUtil.CanAccess(this.ViewState, cmdUpdate.CommandName);
            }

            RadLabel lblComponentName = (RadLabel)item["Name"].FindControl("lblComponentName");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
            }
            RadLabel lblIsdefault = (RadLabel)item["FLAG"].FindControl("lblIsNotes");
            ImageButton imgFlag = (ImageButton)item["FLAG"].FindControl("imgFlag");
            imgFlag.Visible = lblIsdefault.Text.ToUpper().Equals("1") ? true : false;

            RadLabel lblIsContract = (RadLabel)item["FLAG"].FindControl("lblIsContract");
            ImageButton imgContractFlag = (ImageButton)item["FLAG"].FindControl("imgContractFlag");
            if (imgContractFlag != null)
            {
                imgContractFlag.Visible = lblIsContract.Text.ToUpper().Equals("1") ? SessionUtil.CanAccess(this.ViewState, imgContractFlag.CommandName) : false;
            }

            UserControlPurchaseUnit unit = (UserControlPurchaseUnit)item["UNIT"].FindControl("ucUnit");
            DataRowView drv = (DataRowView)item.DataItem;
            if (unit != null)
            {                
                unit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(item.GetDataKeyValue("FLDPARTID").ToString(), Filter.CurrentPurchaseStockType
                                                                     , Filter.CurrentPurchaseVesselSelection );
                unit.SelectedUnit = drv["FLDUNITID"].ToString();
            }
            LinkButton cmdAudit = (LinkButton)item["ACTION"].FindControl("cmdAudit");
            if (cmdAudit != null)
            {
                cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                    cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDQUOTATIONLINEID"].ToString() + "&shortcode=PUR-QTNSTORE-LINE');return false;");
                else
                    cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDQUOTATIONLINEID"].ToString() + "&shortcode=PUR-QTN-LINE');return false;");
            }
            LinkButton cmdVendor = (LinkButton)item["ACTION"].FindControl("cmdVendor");
            if(cmdVendor != null)
            {
                if (drv["FLDPARTID"].ToString() != "99999999-9999-9999-9999-999999999999" && SessionUtil.CanAccess(this.ViewState, cmdVendor.CommandName))
                {
                    cmdVendor.Visible = true;
                    if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                        cmdVendor.Attributes.Add("onclick", "openNewWindow('Vendor', '', 'Inventory/InventoryStoreItemVendor.aspx?STOREITEMID=" + drv["FLDPARTID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return false;");
                    else
                        cmdVendor.Attributes.Add("onclick", "openNewWindow('Vendor', '', 'Inventory/InventorySpareItemVendor.aspx?SPAREITEMID=" + drv["FLDPARTID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return false;");

                }
                else
                {
                    cmdVendor.Visible = false;

                }
            }
                
            
            if (General.GetNullableDecimal(drv["FLDORDEREDQUANTITY"].ToString()) != General.GetNullableDecimal(drv["FLDQUANTITY"].ToString()))
            {
                item["QUOTEDQTY"].BackColor = System.Drawing.Color.Red;
            }

            UserControlHard itemtype = (UserControlHard)item["ITEMTYPE"].FindControl("ddlType");
            if(itemtype !=null)
            {
                itemtype.bind();
            }
        }	


    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        rgvLine.Rebind();
    }
       private bool IsValidRemark()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ViewState["quotationlineid"] == null || ViewState["quotationlineid"].ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Line item selection is required. ";
        //if (txtRemarks.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Vendor Remarks is required.";
        return (!ucError.IsError);
    }
    private void UpdateVendorNotes()
    {
        PhoenixPurchaseQuotationLine.UpdateQuotationLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationlineid"].ToString()),0, txtRemarks.Content);

    }
    private void bindQuotationLine()
    {
        DataSet quotationlinedataset = new DataSet();
        quotationlinedataset = PhoenixPurchaseQuotationLine.EditQuotationLine(new Guid(ViewState["quotationlineid"].ToString()));
        if (quotationlinedataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationlinedataset.Tables[0].Rows[0];
            txtPartName.Text = dr["FLDNAME"].ToString();
            txtPartNumber.Text = dr["FLDNUMBER"].ToString();
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            txtContent.Text = "Goods";
            txtStatus.Text = "Active";
            txtRemarks.Content = HttpUtility.HtmlDecode(dr["FLDVENDORNOTES"].ToString());
            txtPosition.Text = dr["FLDPOSITION"].ToString();
            txtDrawingNo.Text = dr["FLDDRAWINGNUMBER"].ToString();
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                lblMakerRef.Text = "Product Code";
            else
                lblMakerRef.Text = "Maker Ref";
        }

    }

    private void GetSelectedCheckbox()
    {
        List<String> SelectedPvs = new List<string>(); ;
        string lineid;

        if (ViewState["SelectedLineItems"] != null)
            SelectedPvs = (List<String>)ViewState["SelectedLineItems"];

        foreach (GridDataItem item in rgvLine.Items)
        {
            if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
            {
                lineid = item.GetDataKeyValue("FLDORDERLINEID").ToString();
                if (!SelectedPvs.Contains(lineid))
                    SelectedPvs.Add(lineid);
            }
            else
            {
                lineid = item.GetDataKeyValue("FLDORDERLINEID").ToString();
                if (SelectedPvs.Contains(lineid))
                    SelectedPvs.Remove(lineid);
            }
            
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            ViewState["SelectedLineItems"] = SelectedPvs;
    }

}
