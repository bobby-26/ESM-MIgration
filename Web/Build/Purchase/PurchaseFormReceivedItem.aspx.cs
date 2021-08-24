using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PurchaseFormReceivedItem : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                PhoenixToolbar toolSave = new PhoenixToolbar();
                toolSave.AddButton("File PO", "FILE", ToolBarDirection.Right);
                toolSave.AddButton("Save", "BULKSAVE", ToolBarDirection.Right);
                toolSave.AddButton("Copy Order qty", "COPY", ToolBarDirection.Right);
                menuSaveDetails.AccessRights = this.ViewState;
                menuSaveDetails.MenuList = toolSave.Show();

                ViewState["RECEIPT"] = null;
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Vessel Form", "FORM", ToolBarDirection.Left);
                toolbarmain.AddButton("Received", "RECEIVE",ToolBarDirection.Left);
                MenuQuotationLineItem.AccessRights = this.ViewState;
                MenuQuotationLineItem.MenuList = toolbarmain.Show();
                MenuQuotationLineItem.SelectedMenuIndex = 1;
                

                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();

                if (Request.QueryString["pageno"] != null)
                    ViewState["pageno"] = Request.QueryString["pageno"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ITEMID"] = 1;
                txtRecivedDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());

                //ddlSeaPort.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                //ddlSeaPort.DataBind();

                BindHard();
                
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
            DataSet ds = PhoenixPurchaseOrderForm.EditOrderForm(
                new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);


            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FLDFORMSTATUS"].ToString().Equals("55"))
                    {
                        ucError.ErrorMessage = "PO is already 'Filed' and you cannot copy it .";
                        ucError.Visible = true;
                        return;
                    }
                }

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                {
                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        string orderqty = ((RadLabel)item["QUANTITY"].FindControl("lblOrderQuantity")).Text;
                        UserControlDecimal receivedqty = ((UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtStoreQuantityEdit"));
                        receivedqty.Text = orderqty;

                    }
                }
                else
                {
                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        string orderqty = ((RadLabel)item["QUANTITY"].FindControl("lblOrderQuantity")).Text;
                        UserControlDecimal receivedqty = ((UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtSpareQuantityEdit"));
                        receivedqty.Text = orderqty.Replace(".00", "");
                        
                    }
                }
                ucStatus.Text = "Order quantities copied to the received quantity column.";
                ucStatus.Visible = true;
            }

            if (CommandName.ToUpper().Equals("BULKSAVE"))
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FLDFORMSTATUS"].ToString().Equals("55"))
                    {
                        ucError.ErrorMessage = "PO is already 'Filed' and you cannot modify it again.";
                        ucError.Visible = true;
                        return;
                    }
                }
                if (General.GetNullableDateTime(txtRecivedDate.Text) == null)
                {
                    ucError.ErrorMessage = "Received date is required.";
                    ucError.Visible = true;
                    return;
                }
                if(General.GetNullableInteger(rblGoodsStatus.SelectedValue)==null)
                {
                    ucError.ErrorMessage = "Goods Status is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableString(txtRemarks.Text) == null)
                {
                    if (rblGoodsStatus.SelectedValue == "1447")
                    {
                        ucError.ErrorMessage = "Remarks is required.";
                        ucError.Visible = true;
                        return;

                    }
                }

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                {
                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        string orderlineid = item.GetDataKeyValue("FLDORDERLINEID").ToString();
                        string component = item.GetDataKeyValue("FLDFORCOMPID").ToString();
                        string receivedqty = ((UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtStoreQuantityEdit")).Text;
                        string cancelqty;
                        string price = ((RadLabel)item["QUANTITY"].FindControl("lblPrice")).Text;
                        string partid = item.GetDataKeyValue("FLDPARTID").ToString();
                        
                        string unitid = ((UserControlPurchaseUnit)item["UNIT"].FindControl("ucUnit")).SelectedUnit;
                        string quantity = ((RadLabel)item["RECEIVEDQTY"].FindControl("lblQuantityEdit")).Text;
                        string orderqty = ((RadLabel)item["QUANTITY"].FindControl("lblOrderQuantity")).Text;

                        decimal? cqty = (General.GetNullableDecimal(orderqty) == null ? 0 : General.GetNullableDecimal(orderqty))
                            - (General.GetNullableDecimal(receivedqty) == null ? 0 : General.GetNullableDecimal(receivedqty));

                        cancelqty = cqty.ToString();

                        UpdateQuotationLineItem(
                            orderlineid, component, receivedqty,
                            cancelqty, 
                            price, partid,
                            "", unitid, quantity);
                        
                    }
                    PhoenixPurchaseFormReceivedLine.UpdateOrderReceiptStatus(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["orderid"].ToString()),
                            Filter.CurrentPurchaseVesselSelection,
                            General.GetNullableDateTime(txtRecivedDate.Text),
                            0);
                }
                else
                {
                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        string orderlineid = item.GetDataKeyValue("FLDORDERLINEID").ToString();
                        string component = item.GetDataKeyValue("FLDFORCOMPID").ToString();
                        string receivedqty = ((UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtSpareQuantityEdit")).Text;
                        string cancelqty;
                        string price = ((RadLabel)item["QUANTITY"].FindControl("lblPrice")).Text;
                        string partid = item.GetDataKeyValue("FLDPARTID").ToString();
                        string unitid = ((UserControlPurchaseUnit)item["UNIT"].FindControl("ucUnit")).SelectedUnit;
                        string quantity = ((RadLabel)item["RECEIVEDQTY"].FindControl("lblQuantityEdit")).Text;
                        string orderqty = ((RadLabel)item["QUANTITY"].FindControl("lblOrderQuantity")).Text;
                        
                        decimal? cqty = (General.GetNullableDecimal(orderqty) == null ? 0 : General.GetNullableDecimal(orderqty))
                            - (General.GetNullableDecimal(receivedqty) == null ? 0 : General.GetNullableDecimal(receivedqty));

                        cancelqty = cqty.ToString();

                            UpdateQuotationLineItem(
                                orderlineid, component, receivedqty,
                                cancelqty,
                                price, partid,
                                "", unitid, quantity);
                       
                    }
                    PhoenixPurchaseFormReceivedLine.UpdateOrderReceiptStatus(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["orderid"].ToString()),
                            Filter.CurrentPurchaseVesselSelection,
                            General.GetNullableDateTime(txtRecivedDate.Text),
                            0);
                }
                ucStatus.Text = "Receipt of line items updated.";
                ucStatus.Visible = true;

                rgvLine.Rebind();
            }
            if (CommandName.ToUpper().Equals("FILE"))
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FLDFORMSTATUS"].ToString().Equals("55"))
                    {
                        ucError.ErrorMessage = "PO is already 'Filed' and you cannot receive it again.";
                        ucError.Visible = true;
                        return;
                    }
                }
                if (General.GetNullableDateTime(txtRecivedDate.Text) == null)
                {
                    ucError.ErrorMessage = "Received date is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableInteger(rblGoodsStatus.SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Goods Status is required.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseFormReceivedLine.OrderlineItemFilePO(new Guid(ViewState["orderid"].ToString()));

                ucStatus.Text = "PO is Filed successfully.";
                ucStatus.Visible = true;
                rgvLine.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    //{
    //    RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //    try
    //    {
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    //        if (CommandName.ToUpper().Equals("FIND"))
    //        {
    //            ViewState["PAGENUMBER"] = 1;
    //            rgvLine.Rebind();
    //        }
    //        if (CommandName.ToUpper().Equals("EXCEL"))
    //        {
    //            ShowExcel();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNUMBER", "FLDPARTNUMBER", "FLDNAME", "FLDORDEREDQUANTITY", "FLDUNITNAME", "FLDRECEIVEDQUANTITY", "FLDQUICKNAME" };
        string[] alCaptions = { "S.No", "Part Number", "Part Name", "On Order", "Unit", "Receiving", "Receipt Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseFormReceivedLine.RecivedLineSearch(General.GetNullableGuid(ViewState["orderid"].ToString()), sortexpression, sortdirection, 1,
           rgvLine.VirtualItemCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=ReciveItem - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        //Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Receive Item List</h3></td>");
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
    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDPARTNUMBER", "FLDNAME", "FLDORDEREDQUANTITY", "FLDUNITNAME", "FLDRECEIVEDQUANTITY", "FLDQUICKNAME" };
        string[] alCaptions = { "S.No", "Part Number", "Part Name", "On Order", "Unit", "Receiving", "Receipt Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseFormReceivedLine.RecivedLineSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()),
            sortexpression, sortdirection, rgvLine.CurrentPageIndex+1,
            rgvLine.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDGOODSSTATUS"].ToString()) != null)
                rblGoodsStatus.SelectedValue = ds.Tables[0].Rows[0]["FLDGOODSSTATUS"].ToString();
            
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDGOODSSTATUSREMARKS"].ToString();
            txtRecivedDate.Text = ds.Tables[0].Rows[0]["FLDVENDORDELIVERYDATE"].ToString();
            ucPortMulti.SelectedValue = ds.Tables[0].Rows[0]["FLDRECEIVEDPORT"].ToString();
            ucPortMulti.Text = ds.Tables[0].Rows[0]["FLDRECEIVEDPORTNAME"].ToString();

            if (ViewState["orderlineid"] == null)
            {
                ViewState["orderlineid"] = ds.Tables[0].Rows[0]["FLDORDERLINEID"].ToString();
                ViewState["ITEMID"] = ds.Tables[0].Rows[0]["FLDPARTID"].ToString();
            }

        }

        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvLine", "Recived items - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
    }

    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        rgvLine.SelectedIndexes.Clear();
        
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            if (Filter.CurrentPurchaseStockType == "STORE")
            {
                RadLabel lblComponentNo = (RadLabel)item["COMPONENT"].FindControl("lblComponentNo");
                if (lblComponentNo != null)
                {
                    lblComponentNo.Visible = false;
                }
            }

            DataRowView drv = (DataRowView)item.DataItem;
            
            LinkButton img = (LinkButton)item["REMARKS"].FindControl("imgReceiptRemarks");
            img.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','Purchase/PurchaseFormItemReceiptRemarks.aspx?orderlineid=" + item.GetDataKeyValue("FLDORDERLINEID").ToString() + "','large'); return false;");

            //UserControlQuick ucQuick = (UserControlQuick)e.Row.FindControl("ucReciptStatusEdit");
            //if (ucQuick != null) ucQuick.SelectedQuick = drvQuick["FLDRECEIPTSTATUSID"].ToString();

            UserControlPurchaseUnit unit = (UserControlPurchaseUnit)item["UNIT"].FindControl("ucUnit");

            if (unit != null)
            {
                unit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(
                    item.GetDataKeyValue("FLDPARTID").ToString(), Filter.CurrentPurchaseStockType
                    , Filter.CurrentPurchaseVesselSelection);

                unit.SelectedUnit = drv["FLDUNITID"].ToString();
            }

            RadLabel lblQuantityEdit = (RadLabel)item["RECEIVEDQTY"].FindControl("lblQuantityEdit");
            RadLabel lblOrderQuantity = (RadLabel)item["QUANTITY"].FindControl("lblOrderQuantity");

            if (lblQuantityEdit != null && lblOrderQuantity != null)
            {
                if (General.GetNullableDecimal(lblQuantityEdit.Text) != null)
                {
                    if (General.GetNullableDecimal(lblOrderQuantity.Text) != General.GetNullableDecimal(lblQuantityEdit.Text))
                    {
                        //e.Row.ForeColor = System.Drawing.Color.Red;
                        item.ForeColor = System.Drawing.Color.Red;
                    }
                    //else if (General.GetNullableDecimal(lblOrderQuantity.Text) < General.GetNullableDecimal(lblQuantityEdit.Text))
                    //{
                    //    //e.Row.ForeColor = System.Drawing.Color.Red;
                    //    item.ForeColor = System.Drawing.Color.Red;
                    //}
                }
            }

            UserControlDecimal Storeqnd = (UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtStoreQuantityEdit");
            UserControlDecimal Spareqnd = (UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtSpareQuantityEdit");
            Storeqnd.Visible = Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE") ? true : false;
            Spareqnd.Visible= Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE") ? false : true;

            ImageButton cmdMisMaching = (ImageButton)e.Item.FindControl("cmdMisMaching");
            if (cmdMisMaching != null)
            {
                if ((drv["FLDRECEIVEDQUANTITY"].ToString() != drv["FLDSHIPPEDQUANTITY"].ToString()) && SessionUtil.CanAccess(this.ViewState, cmdMisMaching.CommandName))
                    cmdMisMaching.Visible = true;
            }
        }
    }
    protected void rgvLine_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = rgvLine.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        {
            headerItem["MAKERREF"].Text = "Product Code";
            rgvLine.MasterTableView.GetColumn("COMPONENT").Display = false;
        }
            
    }
    private void UpdateQuotationLineItem(string orderlineid, string component, string quantity
                                        , string cancelquantity, string rate, string partid
                                        , string receiptstatusid, string unit, string prevrecdqty)
    {
        
            PhoenixPurchaseFormReceivedLine.UpdateReceivedLine(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(orderlineid),
                General.GetNullableDecimal(cancelquantity) == null ? 0 : General.GetNullableDecimal(cancelquantity),
                General.GetNullableDecimal(quantity) == null ? 0 : General.GetNullableDecimal(quantity),
                General.GetNullableDateTime(txtRecivedDate.Text),
                General.GetNullableInteger(receiptstatusid),
                int.Parse(unit),
                General.GetNullableInteger(rblGoodsStatus.SelectedValue),
                General.GetNullableString(txtRemarks.Text),
                General.GetNullableInteger(ucPortMulti.SelectedValue));


            if (ViewState["LINEID"] == null)
            {
                ViewState["LINEID"] = orderlineid;
            }
        
    }

    private void UpdateInventory(
        string orderlineid, string component, string quantity
        , string cancelquantity, string rate, string partid
        , string receiptstatusid, string unit, string prevrecdqty)
    {
        Decimal? prevqty = General.GetNullableDecimal(prevrecdqty) == null ? 0 : General.GetNullableDecimal(prevrecdqty);
        string iMessageText = "";
        string DisPositionHeaderId = "";
        Guid iDisPositionHeaderId = new Guid();
        int iMessageCode = 0;

        if (General.GetNullableDecimal(quantity) != null)
        {
            Decimal? dispqty = General.GetNullableDecimal(quantity);

            if (dispqty > 0)
            {
                /* TRANSACTION TYPE : USED =7  LOST =9  PURCHASED =6  FOUND =8 */

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    if (ViewState["HEADER"] == null || ViewState["LINEID"].ToString() == orderlineid)
                    {
                        PhoenixInventorySpareItemDisposition.InsertSpareItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                   6, Convert.ToDateTime(txtRecivedDate.Text),
                                  new Guid(ViewState["orderid"].ToString()), component,
                                  Filter.CurrentPurchaseVesselSelection, "", ref iDisPositionHeaderId);

                        //DisPositionHeaderId = iDisPositionHeaderId.ToString();
                        ViewState["HEADER"] = iDisPositionHeaderId.ToString();
                    }
                    DisPositionHeaderId = ViewState["HEADER"].ToString();

                    PhoenixInventorySpareItemDisposition.InsertSpearItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(DisPositionHeaderId),
                        new Guid(partid),
                        dispqty, General.GetNullableDecimal(rate), ViewState["orderid"].ToString(),
                         Filter.CurrentPurchaseVesselSelection, null, 0, int.Parse(unit), ref iMessageCode, ref iMessageText
                        );
                }
                else
                {
                    if (ViewState["HEADER"] == null || ViewState["LINEID"].ToString() == orderlineid)
                    {
                        PhoenixInventoryStoreItemDisposition.InsertStoreItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                  6, Convert.ToDateTime(txtRecivedDate.Text),
                                   new Guid(ViewState["orderid"].ToString()), null,
                                   Filter.CurrentPurchaseVesselSelection, "", ref iDisPositionHeaderId);
                        //DisPositionHeaderId = iDisPositionHeaderId.ToString();

                        ViewState["HEADER"] = iDisPositionHeaderId.ToString();
                    }

                    DisPositionHeaderId = ViewState["HEADER"].ToString();

                    PhoenixInventoryStoreItemDisposition.InsertStoreItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(DisPositionHeaderId),
                        new Guid(partid),
                        dispqty, General.GetNullableDecimal(rate), ViewState["orderid"].ToString(),
                        Filter.CurrentPurchaseVesselSelection, null, 0, int.Parse(unit), ref iMessageCode, ref iMessageText
                        );
                }
            }
        }
    }

    //private void InsertOrderFormHistory()
    //{
    //    PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    //}

    protected void gvRecivedItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && 
            (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null);", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }

    

    
    private bool IsValidRemark()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ViewState["quotationlineid"]==null || ViewState["quotationlineid"].ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Line item selection is required. ";
       
        return (!ucError.IsError);
    }

    protected void BindHard()
    {
        rblGoodsStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 245);
        rblGoodsStatus.DataBind();
    }
}
