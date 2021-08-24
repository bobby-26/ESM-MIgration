using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceDirectPOLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceDirectPOLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderLine')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Accounts/AccountsInvoiceDirectPOLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvTax')", "Print Grid", "icon_print.png", "PRINT");
            AdditionalChargeItem.AccessRights = this.ViewState;
            AdditionalChargeItem.MenuList = toolgrid.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = Request.QueryString["vslid"];
                ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
                gvTax.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvOrderLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                EditOrder();

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERTax"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

        string[] alColumns = { "FLDPARTNAME", "FLDSUBACCOUNT", "FLDAMOUNT", "FLDPODISCOUNT", "FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };
        string[] alCaptions = { "Item Name", "Budget Code", "Amount", "Discount", "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderLineStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
           PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
           ref iRowCount,
           ref iTotalPageCount);
        General.ShowExcel("Line Items", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void ShowExcelAdditionalChargeItem()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDDISCRIPTION", "FLDTAXTYPENAME", "FLDSUBACCOUNT", "FLDVALUE", "FLDPODISCOUNT",
                                 "FLDAMOUNT","FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };

        string[] alCaptions = {"Charge Description", "Type ","Budget Code", "Value", "Discount", "Amount",
                                 "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["TAXROWCOUNT"] == null || Int32.Parse(ViewState["TAXROWCOUNT"].ToString()) == 0)
            iTaxRowCount = 10;
        else
            iTaxRowCount = Int32.Parse(ViewState["TAXROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);
        General.ShowExcel("Additional Charge Items", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void EditOrder()
    {
        if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));


            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (dt.Rows[0]["FLDAPPROVALSTATUS"].ToString() == "0")
            {
                toolbar.AddButton("Approve", "APPROVE",ToolBarDirection.Right);
                MenuDirectPO.AccessRights = this.ViewState;
                MenuDirectPO.Visible = true;
                MenuDirectPO.MenuList = toolbar.Show();
            }
            else
            {
                MenuDirectPO.Visible = false;
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            string[] alColumns = { "FLDPARTNAME", "FLDSUBACCOUNT", "FLDAMOUNT", "FLDPODISCOUNT", "FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };
            string[] alCaptions = { "Item Name", "Budget Code", "Amount", "Discount", "Total Amount", "Vessel Discount", "Total Vessel Amount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsPOStaging.OrderLineStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                gvOrderLine.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvOrderLine.DataSource = ds;
            gvOrderLine.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            gvOrderLine.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[5].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[6].Visible = (showcreditnotedisc == 1) ? true : false;

            General.SetPrintOptions("gvOrderLine", "LINE ITEMS", alCaptions, alColumns, ds);
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
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsPOStaging.DirectPOApprove(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                EditOrder();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AdditionalChargeItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAdditionalChargeItem();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataTax()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        string[] alColumns = {"FLDDISCRIPTION", "FLDTAXTYPENAME", "FLDSUBACCOUNT", "FLDVALUE", "FLDPODISCOUNT",
                                 "FLDAMOUNT","FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };

        string[] alCaptions = {"Charge Description", "Type ","Budget Code", "Value", "Discount", "Amount",
                                 "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);


        gvTax.DataSource = ds;
        gvTax.VirtualItemCount = iTaxRowCount;

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvTax.Columns[5].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[7].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[8].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[9].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["TAXROWCOUNT"] = iTaxRowCount;
        ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        General.SetPrintOptions("gvTax", "ADDITIONAL CHARGE ITEM", alCaptions, alColumns, ds);
    }

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERTax"] = null;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string amount = ((UserControlDecimal)e.Item.FindControl("txtValueAdd")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountAdd")).Text;
                if (!IsValidTaxAdditonal(description, budgetcode, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoice.InvoiceDirectPOTaxInsert(new Guid(ViewState["ORDERID"].ToString()), description, 1, decimal.Parse(amount), General.GetNullableInteger(budgetcode)
                    , General.GetNullableDecimal(discount), null);
                EditOrder();
                gvTax.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblordertaxcode")).Text);
                PhoenixAccountsInvoice.InvoiceDirectPOTaxDelete(id);
                EditOrder();
                gvTax.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblordertaxcode")).Text);
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                string value = ((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text;
                string taxtype = ((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType;
                string isgst = ((RadLabel)e.Item.FindControl("lblIsGST")).Text;
                if (!IsValidTaxAdditonal(description, (taxtype == "2" ? "1" : budgetcode), value))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoice.InvoiceDirectPOTaxUpdate(id, description, int.Parse(taxtype), decimal.Parse(value), General.GetNullableInteger(budgetcode)
                    , General.GetNullableDecimal(discount), (byte?)General.GetNullableInteger(isgst));
                EditOrder();
                gvTax.Rebind();
            }
            TaxRebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvTax_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton de = (ImageButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                if (drv["FLDTAXTYPE"].ToString() == "2") de.Visible = false;
            }
            UserControlDecimal number = (UserControlDecimal)e.Item.FindControl("txtAmountEdit");
            //if (number != null && drv["FLDTAXTYPE"].ToString() == "1")
            //{
            //    number.CssClass = "readonlytextbox";
            //    number.ReadOnly = true;
            //}
            //else if (number != null)
            //{
            //    number.CssClass = "input_mandatory";
            //    number.ReadOnly = false;
            //}
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
                if (drv["FLDTAXTYPE"].ToString() == "2")
                {
                    HtmlGenericControl spn = (HtmlGenericControl)e.Item.FindControl("spnPickListTaxBudgetEdit");
                    spn.Visible = false;
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
            ImageButton ib = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib != null)
            {
                ib.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }
        }
    }

    protected void gvOrderLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string amount = ((UserControlDecimal)e.Item.FindControl("txtAmountAdd")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountAdd")).Text;
                string partname = ((RadTextBox)e.Item.FindControl("txtPartNameAdd")).Text;
                if (!IsValidLineItem(budgetcode, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsPOStaging.OrderLineStagingInsert(new Guid(ViewState["ORDERID"].ToString()), partname, int.Parse(budgetcode), decimal.Parse(amount), General.GetNullableDecimal(discount));
                EditOrder();
                gvOrderLine.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblorderlineitem")).Text);
                PhoenixAccountsPOStaging.OrderLineStagingDelete(id, new Guid(ViewState["ORDERID"].ToString()));
                EditOrder();
                gvOrderLine.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblorderlineitem")).Text);
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                string amount = ((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text;
                string partname = ((RadTextBox)e.Item.FindControl("txtPartNameEdit")).Text;
                if (!IsValidLineItem(budgetcode, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsPOStaging.OrderLineStagingUpdate(id, new Guid(ViewState["ORDERID"].ToString()), partname, int.Parse(budgetcode), decimal.Parse(amount), General.GetNullableDecimal(discount));
                EditOrder();
                gvOrderLine.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrderLine_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton de = (ImageButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
            }
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);

            }
        }
    }
    protected void Rebind()
    {
        gvOrderLine.SelectedIndexes.Clear();
        gvOrderLine.EditIndexes.Clear();
        gvOrderLine.DataSource = null;
        gvOrderLine.Rebind();
    }
    protected void TaxRebind()
    {
        gvTax.SelectedIndexes.Clear();
        gvTax.EditIndexes.Clear();
        gvTax.DataSource = null;
        gvTax.Rebind();
    }
    protected void gvOrderLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrderLine.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTax.CurrentPageIndex + 1;
            BindDataTax();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidLineItem(string budgetid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (amount.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";

        return (!ucError.IsError);
    }
    protected bool IsValidTaxAdditonal(string description, string budgetid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (description.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";
        if (amount.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
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
}

