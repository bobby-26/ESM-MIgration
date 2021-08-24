using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PurchaseBulkPOLineItemByVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseBulkPOLineItemByVesselList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBulkPOLineItemByVessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseBulkPOLineItemByVesselGeneral.aspx?')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDVESSEL");

            MenuVessel.AccessRights = this.ViewState;
            MenuVessel.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseBulkPOLineItemByVesselList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBulkPOLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbar.Show();


            if (!IsPostBack)
            {

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Bulk PO", "BULKPO");
                toolbar.AddButton("Line Items", "LINEITEM");
                toolbar.AddButton("Vessels", "VESSEL");
                toolbar.AddButton("Attachments", "ATTACHMENTS");

                gvBulkPOLineItem.PageSize = General.ShowRecords(null);
                MenuBulkPO.AccessRights = this.ViewState;
                MenuBulkPO.MenuList = toolbar.Show();
                MenuBulkPO.SelectedMenuIndex = 2;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION1"] = null;
                ViewState["SORTDIRECTION1"] = null;
                ViewState["CURRENTINDEX1"] = 1;

                ViewState["LINEITEMID"] = "";
                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                    ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
                else
                    ViewState["ORDERID"] = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BULKPO"))
        {
            if (Filter.CurrentSelectedBulkPOType == "1")
                Response.Redirect("../Purchase/PurchaseBulkPOList.aspx", false);
            else
                Response.Redirect("../Purchase/PurchaseBulkDirectPOList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemList.aspx?", false);
        }
        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemByVesselList.aspx?");
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOAttachments.aspx");
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER1"] = 1;
            gvBulkPOLineItem.CurrentPageIndex = 0;
            gvBulkPOLineItem.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelLineItem();
        }
    }

    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            gvBulkPOLineItemByVessel.CurrentPageIndex = 0;
            gvBulkPOLineItemByVessel.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Total Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                                  General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , (int)ViewState["PAGENUMBER1"]
                                                                 , gvBulkPOLineItem.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItem", "Bulk PO LineItem", alCaptions, alColumns, ds);

        gvBulkPOLineItem.DataSource = ds;
        gvBulkPOLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["LINEITEMID"] == null || ViewState["LINEITEMID"].ToString() == "")
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                Filter.CurrentSelectedBulkOrderLineItemId = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
            }
            SetRowSelectionLineItem();
            gvBulkPOLineItemByVessel.Rebind();
        }

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

    protected void ShowExcelLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Total Received Quantity", "Unit Price", "Total" };

        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                                  General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , 1
                                                                 , gvBulkPOLineItem.VirtualItemCount == 0 ? 1 : gvBulkPOLineItem.VirtualItemCount
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BulkPOLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bulk PO LineItem</h3></td>");
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

    private void SetRowSelectionLineItem()
    {
        gvBulkPOLineItem.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvBulkPOLineItem.Items)
        {
            if (item.GetDataKeyValue("FLDLINEITEMID").ToString() == ViewState["LINEITEMID"].ToString())
            {
                gvBulkPOLineItem.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDBUDGETCODE", "FLDFORMNO", "FLDREQUESTEDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Form No", "Requested Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselSearch(
                                                                   General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , (int)ViewState["PAGENUMBER"]
                                                                 , gvBulkPOLineItemByVessel.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItemByVessel", "Line Item by Vessel", alCaptions, alColumns, ds);

        gvBulkPOLineItemByVessel.DataSource = ds;
        gvBulkPOLineItemByVessel.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["LINEITEMBYVESSELID"] == null)
            {
                ViewState["LINEITEMBYVESSELID"] = ds.Tables[0].Rows[0]["FLDLINEITEMBYVESSELID"].ToString();
            }
            SetRowSelection();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDBUDGETCODE", "FLDFORMNO", "FLDREQUESTEDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Form No", "Requested Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselSearch(
                                                            General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                             , sortexpression
                                                             , sortdirection
                                                             , 1
                                                             , gvBulkPOLineItemByVessel.VirtualItemCount == 0 ? 10 : gvBulkPOLineItemByVessel.VirtualItemCount
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LineItemByVessel.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Line Item by Vessel</h3></td>");
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
    //protected void gvBulkPOLineItemByVessel_RowEditing(object sender, GridViewEditEventArgs de)
    //{            
    //    try
    //    {            
    //         GridView _gridView = (GridView)sender;
    //         if (_gridView.EditIndex > -1)
    //             _gridView.UpdateRow(_gridView.EditIndex, false);

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;

    //        ViewState["LINEITEMBYVESSELID"] = ((Label)gvBulkPOLineItemByVessel.Rows[de.NewEditIndex].FindControl("lblLineItemByVesselId")).Text;

    //        BindData();
    //        SetPageNavigator();            

    //        ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucOrderQtyEdit")).SetFocus();
    //        ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucOrderQtyEdit")).Attributes.Add("onfocus", "this.select()");
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["LINEITEMBYVESSELID"] = null;
        gvBulkPOLineItemByVessel.Rebind();
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["ORDERID"] == null || General.GetNullableGuid(ViewState["ORDERID"].ToString()) == null)
            ucError.ErrorMessage = "Bulk PO details should be recorded first.";

        if (ViewState["LINEITEMID"] == null || General.GetNullableGuid(ViewState["LINEITEMID"].ToString()) == null)
            ucError.ErrorMessage = "Either Line Items are not available or No Line Item is not selected.";

        return (!ucError.IsError);
    }

    private void SetRowSelection()
    {
        gvBulkPOLineItemByVessel.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvBulkPOLineItemByVessel.Items)
        {
            if (item.GetDataKeyValue("FLDLINEITEMBYVESSELID").ToString() == ViewState["LINEITEMBYVESSELID"].ToString())
            {
                gvBulkPOLineItemByVessel.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void VesselLineItemInsert(Guid orderid, Guid lineitemid, string vessellist, int? recievedqty)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , orderid
                                                                , lineitemid
                                                                , General.GetNullableString(vessellist)
                                                                , null
                                                                , recievedqty
                                                                , null);
    }

    private void VesselLineItemUpdate(Guid orderid, Guid lineitemid, Guid lineitembyvesselid, int vesselid, string budgetid, string recievedqty, string price, string orderqty, int? accountid, Guid? ownerbudgetid)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , orderid
                                                                , lineitemid
                                                                , lineitembyvesselid
                                                                , vesselid
                                                                , General.GetNullableInteger(budgetid)
                                                                , General.GetNullableInteger(recievedqty)
                                                                , General.GetNullableDecimal(price)
                                                                , General.GetNullableInteger(orderqty)
                                                                , accountid
                                                                , ownerbudgetid
                                                                );
    }

    private void VesselLineItemDelete(Guid lineitembyvesselid)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , lineitembyvesselid);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvBulkPOLineItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Total Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                                  General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , gvBulkPOLineItem.CurrentPageIndex + 1
                                                                 , gvBulkPOLineItem.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItem", "Bulk PO LineItem", alCaptions, alColumns, ds);

        gvBulkPOLineItem.DataSource = ds;
        gvBulkPOLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["LINEITEMID"] == null || ViewState["LINEITEMID"].ToString() == "")
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                Filter.CurrentSelectedBulkOrderLineItemId = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
            }
            SetRowSelectionLineItem();
            //gvBulkPOLineItemByVessel.Rebind();
        }

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

    protected void gvBulkPOLineItem_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Label lblLineItemId = (Label)e.Item.FindControl("lblLineItemId");
                if (lblLineItemId != null)
                {
                    ViewState["LINEITEMID"] = lblLineItemId.Text;
                    Filter.CurrentSelectedBulkOrderLineItemId = lblLineItemId.Text;
                }
                gvBulkPOLineItemByVessel.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPOLineItem_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
    protected void gvBulkPOLineItemByVessel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDBUDGETCODE", "FLDFORMNO", "FLDREQUESTEDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Form No", "Requested Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselSearch(
                                                                   General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                 , General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , gvBulkPOLineItemByVessel.CurrentPageIndex + 1
                                                                 , gvBulkPOLineItemByVessel.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItemByVessel", "Line Item by Vessel", alCaptions, alColumns, ds);

        gvBulkPOLineItemByVessel.DataSource = ds;
        gvBulkPOLineItemByVessel.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["LINEITEMBYVESSELID"] == null)
            {
                ViewState["LINEITEMBYVESSELID"] = ds.Tables[0].Rows[0]["FLDLINEITEMBYVESSELID"].ToString();
            }
            SetRowSelection();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvBulkPOLineItemByVessel_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            TextBox tb1 = (TextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:done");

            TextBox txtBudgetIdEdit = (TextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (txtBudgetIdEdit != null) txtBudgetIdEdit.Attributes.Add("style", "display:done");

            tb1 = (TextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:done");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");

            Label lblVesselId = (Label)e.Item.FindControl("lblVesselId");

            if (ib1 != null && lblVesselId != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            UserControlMaskNumber ucReceivedQtyEdit = (UserControlMaskNumber)e.Item.FindControl("ucReceivedQtyEdit");
            if (ucReceivedQtyEdit != null) ucReceivedQtyEdit.Focus();

            DropDownList ddlAccountDetails = (DropDownList)e.Item.FindControl("ddlAccountDetails");
            if (ddlAccountDetails != null)
            {
                ddlAccountDetails.Visible = true;
                ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                    General.GetNullableInteger(drv["FLDVESSELID"].ToString()), 1);
                ddlAccountDetails.DataBind();
                ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));

                ddlAccountDetails.SelectedValue = drv["FLDACCOUNTID"].ToString();


                if (ddlAccountDetails.SelectedIndex > 0)
                {
                    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["Ownerid"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                        }
                    }
                }
            }

            tb1 = (TextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:done");
            tb1 = (TextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:done");
            tb1 = (TextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:done");
            ImageButton ib2 = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            if (ViewState["Ownerid"] != null && ib2 != null)
            {
                // ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
                ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");

            }
        }
    }

    protected void gvBulkPOLineItemByVessel_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Label lblLineItemByVesselId = (Label)e.Item.FindControl("lblLineItemByVesselId");
                if (lblLineItemByVesselId != null)
                    ViewState["LINEITEMBYVESSELID"] = lblLineItemByVesselId.Text;

            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblLineItemByVesselId = (Label)e.Item.FindControl("lblLineItemByVesselId");
                VesselLineItemDelete(new Guid(lblLineItemByVesselId.Text));
                ViewState["LINEITEMBYVESSELID"] = null;
                gvBulkPOLineItemByVessel.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPOLineItemByVessel_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void gvBulkPOLineItemByVessel_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidData())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["LINEITEMBYVESSELID"] != null && ViewState["LINEITEMBYVESSELID"].ToString() != "")
            {
                Label lblVesselId = (Label)e.Item.FindControl("lblVesselId");
                Label lblLineItemByVesselId = (Label)e.Item.FindControl("lblLineItemByVesselId");
                int vesselid = int.Parse(lblVesselId.Text);
                string LineItemByVesselId = lblLineItemByVesselId.Text;

                VesselLineItemUpdate(new Guid(ViewState["ORDERID"].ToString())
                                    , new Guid(ViewState["LINEITEMID"].ToString())
                                    , new Guid(LineItemByVesselId)
                                    , vesselid
                                    , ((TextBox)e.Item.FindControl("txtBudgetIdEdit")).Text
                                    , ((UserControlMaskNumber)(e.Item.FindControl("ucReceivedQtyEdit"))).Text.Replace(".", "")
                                    , ((UserControlMaskNumber)(e.Item.FindControl("ucPriceEdit"))).Text
                                    , ((UserControlMaskNumber)(e.Item.FindControl("ucOrderQtyEdit"))).Text.Replace(".", "")
                                    , General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlAccountDetails")).SelectedValue)
                                    , General.GetNullableGuid(((TextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text)
                                    );
            }
            gvBulkPOLineItemByVessel.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        GridDataItem Item = (GridDataItem)gvBulkPOLineItemByVessel.MasterTableView.GetItems(GridItemType.EditItem)[0];

        ((TextBox)Item.FindControl("txtOwnerBudgetCodeEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetNameEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetIdEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetgroupIdEdit")).Text = null;
    }

    protected void ddlAccountDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataItem Item = (GridDataItem)gvBulkPOLineItemByVessel.MasterTableView.GetItems(GridItemType.EditItem)[0];

        DropDownList ddlAccountDetails = (DropDownList)Item.FindControl("ddlAccountDetails");
        if (ddlAccountDetails.SelectedIndex > 0)
        {
            ViewState["Ownerid"] = null;
            DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Ownerid"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                }
            }
        }
        ((TextBox)Item.FindControl("txtOwnerBudgetCodeEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetNameEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetIdEdit")).Text = null;
        ((TextBox)Item.FindControl("txtOwnerBudgetgroupIdEdit")).Text = null;

        ImageButton ib2 = (ImageButton)Item.FindControl("btnShowOwnerBudgetEdit");
        TextBox txtBudgetIdEdit = (TextBox)Item.FindControl("txtBudgetIdEdit");
        if (ViewState["Ownerid"] != null && ib2 != null)
        {
            ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");

        }
    }

    protected void txtBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
        GridDataItem Item = (GridDataItem)gvBulkPOLineItemByVessel.MasterTableView.GetItems(GridItemType.EditItem)[0];

        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        ImageButton ib2 = (ImageButton)Item.FindControl("btnShowOwnerBudgetEdit");
        TextBox txtBudgetIdEdit = (TextBox)Item.FindControl("txtBudgetIdEdit");
        Label lblVesselId = (Label)Item.FindControl("lblVesselId");

        if (ViewState["Ownerid"] != null && ib2 != null)
        {
            ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");

        }

    }
    protected void cmdHiddenPick_Click1(object sender, EventArgs e)
    {

    }
}


