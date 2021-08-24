using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsPostInvoiceStaging : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuQuotationLineItem.AccessRights = this.ViewState;
            MenuQuotationLineItem.MenuList = toolbarmain.Show();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsPostInvoiceStaging.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderLine')", "Print Grid", "icon_print.png", "PRINT");

            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Accounts/AccountsPostInvoiceStaging.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvTax')", "Print Grid", "icon_print.png", "PRINT");

            AdditionalChargeItem.AccessRights = this.ViewState;
            AdditionalChargeItem.MenuList = toolgrid.Show();

            PhoenixToolbar toolmain = new PhoenixToolbar();
            toolmain.AddButton("PO", "PO", ToolBarDirection.Right);
            toolmain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.Title = "Post Invoice";
            MenuLineItem.MenuList = toolmain.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["ORDERID"] != null)
                {
                    ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
                    imgShowVessel.Attributes.Add("onclick", "return showPickList('spnPickListVessel', 'codehelp1', '', '../Common/CommonPickListVessel.aspx', true); ");
                }
                else
                {
                    ViewState["ORDERID"] = "";
                }
                if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                    ViewState["callfrom"] = Request.QueryString["qcallfrom"];
                if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
                {
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"];
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELACCOUNT"] = string.Empty;

                BindVendorInfo();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindVendorInfo()
    {
        DataSet dsVendor = PhoenixAccountsPOStaging.POStagingEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (dsVendor.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVendor.Tables[0].Rows[0];
            txtPONumber.Text = dr["FLDFORMNO"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
            txtReceivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
            txtVenderName.Text = dr["FLDNAME"].ToString();
            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtVendorCode.Text = dr["FLDCODE"].ToString();
            txtVendorId.Text = dr["FLDVENDORID"].ToString();
            chkGSTForVessel.Checked = dr["FLDGSTOFFSET"].ToString() == "1" ? true : false;
            txtVesselDiscount.Text = String.Format("{0:##.00}", dr["FLDVESSELDISCOUNT"]);
            txtPortName.Text = dr["FLDSEAPORTNAME"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["Ownerid"] = dr["FLDPRINCIPALID"].ToString();
            ViewState["VESSELACCOUNT"] = dr["FLDACCOUNTID"].ToString();
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblGSTForVessel.Visible = (showcreditnotedisc == 1) ? true : false;
        chkGSTForVessel.Visible = (showcreditnotedisc == 1) ? true : false;
        lblDiscountForOwner.Visible = (showcreditnotedisc == 1) ? true : false;
        txtVesselDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
        lblPercentage.Visible = (showcreditnotedisc == 1) ? true : false;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDPARTNAME","FLDPRICE","FLDRECEIVEDQUANTITY","FLDSUBACCOUNT","FLDOWNERBUDGETCODE","FLDOWNERDISCOUNT",
                                 "FLDAMOUNT","FLDDISCOUNT","FLDTOTALAMOUNT","FLDVESSELDISCOUNT","FLDTOTALVESSELAMOUNT"};

        string[] alCaptions = {"Line Item ","PO Unit Price ","Received Quantity","Budget Code","Owner Code","Discount","Amount",
                                 "Cr. Note Discount","Payble Amount After Discount ","Discount For Vessel","Chargeable Amount After Discount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderLineStagingInvoiceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=LineItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>LINE ITEMS</h3></td>");
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
    protected void ShowExcelAdditionalChargeItem()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDDISCRIPTION", "FLDVALUE","FLDSUBACCOUNT","FLDAMOUNT",
                                 "FLDDISCOUNT","FLDTOTALAMOUNT","FLDVESSELDISCOUNT","FLDTOTALVESSELAMOUNT"};
        string[] alCaptions = {"Charge Description", "Value ","Budget Code","Amount",
                                 "Discount","Payable Amount After Discount ","Discount For Vessel","Chargeable Amount After Discount" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["TAXROWCOUNT"] == null || Int32.Parse(ViewState["TAXROWCOUNT"].ToString()) == 0)
            iTaxRowCount = 10;
        else
            iTaxRowCount = Int32.Parse(ViewState["TAXROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingInvoiceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=AdditionalChargeItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");

        Response.Write("<td><h3>Additional Charge Item</h3></td>");
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
        if (ds.Tables[0].Rows.Count > 0)
        {
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
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            string[] alColumns = {"FLDPARTNAME","FLDPRICE","FLDRECEIVEDQUANTITY","FLDSUBACCOUNT","FLDOWNERBUDGETCODE","FLDOWNERDISCOUNT",
                                 "FLDAMOUNT","FLDDISCOUNT","FLDTOTALAMOUNT","FLDVESSELDISCOUNT","FLDTOTALVESSELAMOUNT"};

            string[] alCaptions = {"Line Item ","PO Unit Price ","Received Quantity","Budget Code","Owner Code","Discount","Amount",
                                 "Cr. Note Discount","Payble Amount After Discount ","Discount For Vessel","Chargeable Amount After Discount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixAccountsPOStaging.OrderLineStagingInvoiceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                gvOrderLine.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
            BindFooter();

            gvOrderLine.DataSource = ds;
            gvOrderLine.VirtualItemCount = iRowCount;
            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            gvOrderLine.Columns[9].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[10].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[11].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[12].Visible = (showcreditnotedisc == 1) ? true : false;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvOrderLine", "Line Items", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFooter()
    {
        DataSet dsTotal = PhoenixAccountsPOStaging.OrderLineStagingInvoiceTotal(new Guid(ViewState["ORDERID"].ToString()));
        if (dsTotal.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsTotal.Tables[0].Rows[0];
            ViewState["FLDSUMAMOUNT"] = dr["FLDSUMAMOUNT"].ToString();
            ViewState["FLDSUMDISCOUN"] = dr["FLDSUMDISCOUN"].ToString();
            ViewState["FLDSUMTOTALAMOUNT"] = dr["FLDSUMTOTALAMOUNT"].ToString();
            ViewState["FLDSUMVESSELDISCOUNT"] = dr["FLDSUMVESSELDISCOUNT"].ToString();
            ViewState["FLDSUMTOTALVESSELAMOUNT"] = dr["FLDSUMTOTALVESSELAMOUNT"].ToString();
        }
        else
        {
            ViewState["FLDSUMAMOUNT"] = 0;
            ViewState["FLDSUMDISCOUN"] = 0;
            ViewState["FLDSUMTOTALAMOUNT"] = 0;
            ViewState["FLDSUMVESSELDISCOUNT"] = 0;
            ViewState["FLDSUMTOTALVESSELAMOUNT"] = 0;
        }
    }

    private void BindTotalFooter()
    {
        DataSet dsTotal = PhoenixAccountsPOStaging.OrderFormStagingInvoiceTotalPOAmount(new Guid(ViewState["ORDERID"].ToString()));
        if (dsTotal.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsTotal.Tables[0].Rows[0];
            ViewState["FLDTOTALAMOUNT"] = dr["FLDTOTALAMOUNT"].ToString();
            ViewState["FLDTOTALDISCOUNT"] = dr["FLDTOTALDISCOUNT"].ToString();
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"] = dr["FLDTOTALAFTERDISCOUNTAMOUNT"].ToString();
            ViewState["FLDTOTALVESSELDISCOUNT"] = dr["FLDTOTALVESSELDISCOUNT"].ToString();
            ViewState["FLDTOTALVESSELAMOUNT"] = dr["FLDTOTALVESSELAMOUNT"].ToString();
        }
        else
        {
            ViewState["FLDTOTALAMOUNT"] = 0;
            ViewState["FLDTOTALDISCOUNT"] = 0;
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"] = 0;
            ViewState["FLDTOTALVESSELDISCOUNT"] = 0;
            ViewState["FLDTOTALVESSELAMOUNT"] = 0;
        }
    }

    private void BindAdditionalDiscount()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;
        //
        string[] alColumns = { "FLDOLDAMOUNT", "FLDAMOUNT", "FLDTOTALVESSELAMOUNT", "FLDFORMBUDGETCODE", "FLDOWNERBUDGETCODE" };
        string[] alCaptions = { "Original Additional Discount", "New Additional Discount", "Discount Amount For Vessel", "Budget Code", "Owner Code" };

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingInvoiceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1,
            10, ref iTaxRowCount, ref iTaxTotalPageCount);

        gvAdditionalDiscount.DataSource = ds;
        gvAdditionalDiscount.VirtualItemCount = iTaxRowCount;

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvAdditionalDiscount.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;
    }

    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=PR");
            }
            else if (CommandName.ToUpper().Equals("INVOICE") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                if (ViewState["callfrom"].ToString() == "PR")
                {
                    string s = ViewState["invoicecode"].ToString();
                    Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=PR");
                }
                else if (ViewState["callfrom"].ToString() == "invoice")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "AD")
                {
                    //  string str = Request.QueryString["qinvoicecode"].ToString();
                    Response.Redirect("../Accounts/AccountsInvoiceAdjustmentMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());

                }
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
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();

            }
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
    protected void gvOrderLine_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvOrderLine_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //gvOrderLine.SelectedIndex = e.NewSelectedIndex;
        //ViewState["orderlineid"] = ((RadLabel)gvOrderLine.Rows[e.NewSelectedIndex].FindControl("lblLineid")).Text;
        //if (gvOrderLine.EditIndex > -1)
        //    gvOrderLine.UpdateRow(gvOrderLine.EditIndex, false);
        //gvOrderLine.EditIndex = -1;
        //BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            Rebind();
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

        string[] alColumns = {"FLDDISCRIPTION", "FLDVALUE","FLDSUBACCOUNT","FLDAMOUNT",
                                 "FLDDISCOUNT","FLDTOTALAMOUNT","FLDVESSELDISCOUNT","FLDTOTALVESSELAMOUNT"};
        string[] alCaptions = {"Charge Description", "Value ","Budget Code","Amount",
                                 "Discount","Payable Amount After Discount ","Discount For Vessel","Chargeable Amount After Discount" };
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingInvoiceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1,
            10, ref iTaxRowCount, ref iTaxTotalPageCount);
        BindTotalFooter();

        gvTax.DataSource = ds;
        gvTax.VirtualItemCount = iTaxRowCount;

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvTax.Columns[7].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[8].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[9].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[10].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["TAXROWCOUNT"] = iTaxRowCount;
        ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        General.SetPrintOptions("gvTax", "Additional Charge Item", alCaptions, alColumns, ds);
    }

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTax(
                    ((RadTextBox)e.Item.FindControl("txtVesselDiscountEdit")).Text.ToString().Trim(),
                    ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                RadLabel lblDiscount = ((RadLabel)e.Item.FindControl("lblDiscount"));
                UpdateInvoiceTax(ViewState["ORDERID"].ToString()
                                    , ((RadTextBox)e.Item.FindControl("txtTaxMapCode")).Text.ToString()
                                                                , lblDiscount != null ? lblDiscount.Text.ToString() : "0"
                                    , ((RadTextBox)e.Item.FindControl("txtVesselDiscountEdit")).Text.ToString().Trim()
                                    , ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.ToString()
                                    , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text
                                    , ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode
                                    );
                TaxRebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno;
                iRowno = e.Item.ItemIndex;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTax_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

            //if (lblAdditionalDiscountYN != null && lblAdditionalDiscountYN.Text == "1")
            //{
            //    GridViewRow gvr = e.Row;
            //    gvr.Visible = false;
            //}

            RadLabel lblAdditionalDiscountYN = (RadLabel)e.Item.FindControl("lblAdditionalDiscountYN");
            ViewState["ADDITIONALDISCOUNT"] = lblAdditionalDiscountYN.Text;

            GridDataItem item = (GridDataItem)e.Item;
            if (ViewState["ADDITIONALDISCOUNT"] != null && ViewState["ADDITIONALDISCOUNT"].ToString() == "1")
            {
                item.Display = false;
            }
        }

        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudget");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            if (ibtnShowOwnerBudgetEdit != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString()), BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }

        }

        if (e.Item is GridFooterItem)
        {
            RadLabel lb1 = (RadLabel)e.Item.FindControl("lblTaxAmountFooter");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblTaxDiscountFooter");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTaxTotalFooter");
            RadLabel lb4 = (RadLabel)e.Item.FindControl("lbltaxVesselDiscountfooter");
            RadLabel lb5 = (RadLabel)e.Item.FindControl("lbltaxVesselAmountfooter");
            if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDTOTALAMOUNT"].ToString()).ToString("########0.00");
            if (lb2 != null) lb2.Text = decimal.Parse(ViewState["FLDTOTALDISCOUNT"].ToString()).ToString("########0.00");
            if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"].ToString()).ToString("########0.00");
            if (lb4 != null) lb4.Text = decimal.Parse(ViewState["FLDTOTALVESSELDISCOUNT"].ToString()).ToString("########0.00");
            if (lb5 != null) lb5.Text = decimal.Parse(ViewState["FLDTOTALVESSELAMOUNT"].ToString()).ToString("########0.00");
        }
    }

    protected void gvOrderLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno;
                iRowno = e.Item.ItemIndex;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                if (!IsValidForm(((UserControlDecimal)e.Item.FindControl("txtVesselDiscountEdit")).Text, ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateInvoiceItemVesselDiscount(((RadLabel)e.Item.FindControl("lblOrderId")).Text,
                        ((RadLabel)e.Item.FindControl("lblOrderLineId")).Text,
                        ((UserControlDecimal)e.Item.FindControl("txtVesselDiscountEdit")).Text.Replace("_", "0"),
                        ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text,
                        ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode
                        );

                Rebind();
               // gvOrderLine.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvOrderLine_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
            //{

            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOrderLine, "Edit$" + e.Row.RowIndex.ToString(), false);
            //}
        }
        SetKeyDownScroll(sender, e);
    }

    protected void gvOrderLine_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDiscountUpdateForAll = (ImageButton)e.Item.FindControl("cmdDiscountUpdateForAll");
            if (cmdDiscountUpdateForAll != null)
            {//cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");

                cmdDiscountUpdateForAll.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Accounts/AccountsReconcilationStagingDiscountUpdatePopUp.aspx?ORDERID=" + ViewState["ORDERID"] + "'); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, cmdDiscountUpdateForAll.CommandName)) cmdDiscountUpdateForAll.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");

            if (ibtnShowOwnerBudgetEdit != null && ViewState["VESSELID"] != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString()), BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            ImageButton cmdDiscountUpdateForAll = (ImageButton)e.Item.FindControl("cmdDiscountUpdateForAll");
            if (cmdDiscountUpdateForAll != null)
                cmdDiscountUpdateForAll.Visible = (showcreditnotedisc == 1) ? true : false;
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lb1 = (RadLabel)e.Item.FindControl("lblAmountfooter");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblDiscountfooter");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTotalAmountfooter");
            RadLabel lb4 = (RadLabel)e.Item.FindControl("lblVesselDiscountfooter");
            RadLabel lb5 = (RadLabel)e.Item.FindControl("lblTotalVesselAmountfooter");
            if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDSUMAMOUNT"].ToString()).ToString("######0.00");
            if (lb2 != null) lb2.Text = decimal.Parse(ViewState["FLDSUMDISCOUN"].ToString()).ToString("######0.00");
            if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDSUMTOTALAMOUNT"].ToString()).ToString("######0.00");
            if (lb4 != null) lb4.Text = decimal.Parse(ViewState["FLDSUMVESSELDISCOUNT"].ToString()).ToString("######0.00");
            if (lb5 != null) lb5.Text = decimal.Parse(ViewState["FLDSUMTOTALVESSELAMOUNT"].ToString()).ToString("######0.00");
        }
    }

    protected bool IsValidTax(string strdiscount, string budgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strdiscount.Trim() == string.Empty)
            ucError.ErrorMessage = "Discount is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";
        return (!ucError.IsError);
    }


    protected void gvAdditionalDiscount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");
                RadLabel lblOrderTaxCode = (RadLabel)e.Item.FindControl("lblOrderTaxCode");
                UpdateTaxAdditionalDiscount(ViewState["ORDERID"].ToString()
                                            , lblOrderTaxCode.Text.Trim()
                                            , ((UserControlDecimal)e.Item.FindControl("ucAdditionalAmountEdit")).Text.ToString().Trim()
                                            , ((UserControlDecimal)e.Item.FindControl("ucVesselAmountEdit")).Text.ToString().Trim()
                                            , ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text
                                            , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text
                                            , ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode
                                            );

               // gvAdditionalDiscount.Rebind();
                AddDisRebind();
               
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno;
                iRowno = e.Item.ItemIndex;
            }
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAdditionalDiscount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

        }

        if (e.Item is GridEditableItem)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }

            //if (lblAdditionalDiscountYN != null && lblAdditionalDiscountYN.Text == "0")
            //{
            //    GridViewRow gvr = e.Item;
            //    gvr.Visible = false;
            //}
            RadLabel lblAdditionalDiscountYN = (RadLabel)e.Item.FindControl("lblAdditionalDiscountYN");
            ViewState["ADDITIONALDISCOUNT"] = lblAdditionalDiscountYN.Text;

            GridDataItem item = (GridDataItem)e.Item;
            if (ViewState["ADDITIONALDISCOUNT"] != null && ViewState["ADDITIONALDISCOUNT"].ToString() == "0")
            {
                item.Display = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");

            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");

            //RadLabel lblBudgetId = (RadLabel)e.Row.FindControl("lblBudgetId");
            //RadLabel lblPOBudgetId = (RadLabel)e.Row.FindControl("lblPOBudgetId");
            //if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");


            if (ib1 != null)
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");

            if (ibtnShowOwnerBudgetEdit != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString()), BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }

        }

    }


    protected void UpdateInvoiceTax(string orderid, string taxmapcode, string discount, string vesseldiscount, string budgetcodeid, string ownerbudgetcode, string Projectcode)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderTaxStagingUpdateVesselDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid), new Guid(taxmapcode), General.GetNullableDecimal(discount), General.GetNullableDecimal(vesseldiscount), General.GetNullableInteger(budgetcodeid), General.GetNullableGuid(ownerbudgetcode), General.GetNullableInteger(Projectcode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void UpdateInvoiceItemVesselDiscount(string orderid, string orderlineid, string vesseldiscount, string budgetid, string ownerbudgetcode, string Projectcode)
    {
        PhoenixAccountsPOStaging.OrderLineStagingUpdateVesselDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid), new Guid(orderlineid), General.GetNullableDecimal(vesseldiscount), General.GetNullableInteger(budgetid), General.GetNullableGuid(ownerbudgetcode), General.GetNullableInteger(Projectcode));
    }

    private bool IsValidForm(string Discount, string quantity)
    {
        if (Discount.Trim().Equals("") || Discount == "0")
            ucError.ErrorMessage = "Discount is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateOrderFormStaging();
                ucStatus.Text = "Quotation  updated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void UpdateOrderFormStaging()
    {
        PhoenixAccountsPOStaging.OrderFormStagingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , new Guid(ViewState["ORDERID"].ToString())
                 , chkGSTForVessel.Checked ? "1" : "0"
                 , General.GetNullableDecimal(txtVesselDiscount.Text));

        PhoenixAccountsPOStaging.OrderLineStagingCalculateVesselDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
       , new Guid(ViewState["ORDERID"].ToString())
       , General.GetNullableDecimal(txtVesselDiscount.Text));

        PhoenixAccountsPOStaging.OrderTaxStagingUpdateVesselAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
       , new Guid(ViewState["ORDERID"].ToString())
       , General.GetNullableDecimal(txtVesselDiscount.Text));

        Rebind();
        TaxRebind();
        AddDisRebind();

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

    protected void btnVesselDiscount_Click(object sender, EventArgs e)
    {
        PhoenixAccountsPOStaging.OrderLineStagingCalculateVesselDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , new Guid(ViewState["ORDERID"].ToString())
               , General.GetNullableDecimal(txtVesselDiscount.Text));

        Rebind();
        gvTax.Rebind();
    }

    protected void UpdateTaxAdditionalDiscount(string orderid, string taxmapcode, string additionaldiscount, string vesseladditionaldiscount, string budgetcodeid, string newownerbudgetid, string Projectcode)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderTaxStagingAdditionalDiscountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(orderid)
                                                                            , new Guid(taxmapcode)
                                                                            , General.GetNullableDecimal(additionaldiscount)
                                                                            , General.GetNullableDecimal(vesseladditionaldiscount)
                                                                            , General.GetNullableInteger(budgetcodeid)
                                                                            , General.GetNullableGuid(newownerbudgetid)
                                                                            , General.GetNullableInteger(Projectcode)
                                                                            );
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        TaxRebind();
        AddDisRebind();
        BindVendorInfo();
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
    protected void gvAdditionalDiscount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTax.CurrentPageIndex + 1;
            BindAdditionalDiscount();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void AddDisRebind()
    {
        gvAdditionalDiscount.SelectedIndexes.Clear();
        gvAdditionalDiscount.EditIndexes.Clear();
        gvAdditionalDiscount.DataSource = null;
        gvAdditionalDiscount.Rebind();
    }

    protected void VesselUpdate(object sender, ImageClickEventArgs arg)
    {
        if (ViewState["ORDERID"] != null)
        {
            if (txtVesselId.Text != string.Empty)
            {
                try
                {
                    PhoenixAccountsPOStaging.InvoiceReconciliationStagingVesselUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVesselId.Text));
                    Response.Redirect("AccountsPostInvoiceStaging.aspx?ORDERID=" + ViewState["ORDERID"].ToString() + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString());
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
    }

    protected void AdditionalBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

        GridDataItem Item = (GridDataItem)gvAdditionalDiscount.MasterTableView.GetItems(GridItemType.EditItem)[0];
        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        UserControls_UserControlProjectCode ProjectCodeEdit = (UserControls_UserControlProjectCode)Item.FindControl("ucProjectcodeEdit");
        int? BudgetEditid = General.GetNullableInteger(((RadTextBox)Item.FindControl("txtBudgetIdEdit")).Text);

        if (ib1 != null)
        {
            ProjectCodeEdit.bind(VesselAccountId, BudgetEditid);
        }
    }

    protected void TaxBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
         int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

        GridDataItem Item = (GridDataItem)gvTax.MasterTableView.GetItems(GridItemType.EditItem)[0];
        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        UserControls_UserControlProjectCode ProjectCodeEdit = (UserControls_UserControlProjectCode)Item.FindControl("ucProjectcodeEdit");
        int? BudgetEditid = General.GetNullableInteger(((RadTextBox)Item.FindControl("txtBudgetIdEdit")).Text);

        if (ib1 != null)
        {
            ProjectCodeEdit.bind(VesselAccountId, BudgetEditid);
        }
    }

    protected void OrderlineBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

        GridDataItem Item = (GridDataItem)gvOrderLine.MasterTableView.GetItems(GridItemType.EditItem)[0];
        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        UserControls_UserControlProjectCode ProjectCodeEdit = (UserControls_UserControlProjectCode)Item.FindControl("ucProjectcodeEdit");
        int? BudgetEditid = General.GetNullableInteger(((RadTextBox)Item.FindControl("txtBudgetIdEdit")).Text);

        if (ib1 != null)
        {
            ProjectCodeEdit.bind(VesselAccountId, BudgetEditid);
        }
    }
}
