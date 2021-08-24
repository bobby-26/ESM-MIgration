using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsReconcilationStagingInvoiceDiff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsReconcilationStagingInvoiceDiff.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderLine')", "Print Grid", "icon_print.png", "PRINT");

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Accounts/AccountsReconcilationStagingInvoiceDiff.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvTax')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();
            AdditionalChargeItem.AccessRights = this.ViewState;
            AdditionalChargeItem.MenuList = toolgrid.Show();
            PhoenixToolbar Title = new PhoenixToolbar();
            TabStripTitle.AccessRights = this.ViewState;
            TabStripTitle.Title = "Invoice Matching";
            TabStripTitle.MenuList = Title.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["ORDERID"] != null)
                {
                    ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
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
                gvOrderLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvTax.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindVendorInfo();
                BindData();
            }
            // BindDataTax();
            // BindData();
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

            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["EDITMODE"] = dr["FLDEDITMODE"].ToString();
            lblBalanceAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDBALANCEAMOUNTWITHINVOICEDIFF"]));
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDPARTNAME", "FLDPRICE","FLDRECEIVEDQUANTITY","FLDPOSUBACCOUNT","FLDOWNERBUDGETCODE","FLDSUBACCOUNT"
                                     ,"FLDNEWOWNERBUDGETCODE","FLDOWNERDISCOUNT","FLDAMOUNT","FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
        string[] alCaptions = {"Line Item ", "PO Unit Price ", "Recived Quantity ","Original Budget Code ","Original Owner Code","New Budget Code"
                                      ,"New Owner Code","Discount","Amount","Original Cr. Note Discount","New Cr. Note Discount","Amount After Discount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderLineStagingSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
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

    protected void ShowExcelAdditionalChargeItem()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDDISCRIPTION", "FLDVALUE","FLDSUBACCOUNT","FLDSUBACCOUNT","FLDAMOUNT",
                                 "FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
        string[] alCaptions = {"Charge Description", "Value ","Actual Budget Code","New Budget Code","Amount",
                                 "Actual Discount","New Discount","Amount After Discount " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["TAXROWCOUNT"] == null || Int32.Parse(ViewState["TAXROWCOUNT"].ToString()) == 0)
            iTaxRowCount = 10;
        else
            iTaxRowCount = Int32.Parse(ViewState["TAXROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=AdditionalChargeItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");

        Response.Write("<td><h3>ADDITIONAL CHARGE ITEM</h3></td>");
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
            string[] alColumns = {"FLDPARTNAME", "FLDPRICE","FLDRECEIVEDQUANTITY","FLDPOSUBACCOUNT","FLDOWNERBUDGETCODE","FLDSUBACCOUNT"
                                     ,"FLDNEWOWNERBUDGETCODE","FLDOWNERDISCOUNT","FLDAMOUNT","FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
            string[] alCaptions = {"Line Item ", "PO Unit Price ", "Recived Quantity ","Original Budget Code ","Original Owner Code","New Budget Code"
                                      ,"New Owner Code","Discount","Amount","Original Cr. Note Discount","New Cr. Note Discount","Amount After Discount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixAccountsPOStaging.OrderLineStagingInvoiceDifferenceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            gvOrderLine.DataSource = ds;
            gvOrderLine.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        //  gvOrderLine.SelectedIndex = e.NewSelectedIndex;
        //  if (gvOrderLine.EditIndex > -1)
        //      gvOrderLine.UpdateRow(gvOrderLine.EditIndex, false);
        //  gvOrderLine.EditIndex = -1;
        //  BindData();
    }

    private void BindDataTax()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        string[] alColumns = {"FLDDISCRIPTION", "FLDVALUE","FLDSUBACCOUNT","FLDSUBACCOUNT","FLDOLDAMOUNT","FLDAMOUNT",
                                 "FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
        string[] alCaptions = {"Charge Description", "Value ","Original Budget Code","New Budget Code","Original Amount","New Amount",
                                 "Original Discount","New Discount","Amount After Discount " };
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingInvoiceDifferenceSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);


        gvTax.DataSource = ds;
        gvTax.VirtualItemCount = iTaxRowCount;

        ViewState["TAXROWCOUNT"] = iTaxRowCount;
        ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        General.SetPrintOptions("gvTax", "ADDITIONAL CHARGE ITEM", alCaptions, alColumns, ds);
    }

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            return;
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidTax(
              ((UserControlDecimal)e.Item.FindControl("txtTaxInvoiceEdit")).Text.ToString().Trim(),
              ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text.ToString()
                ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateOrderTaxStagingInvoiceDifference(((RadLabel)e.Item.FindControl("lblOrderTaxCode")).Text,
                                                    ((UserControlDecimal)e.Item.FindControl("txtTaxInvoiceEdit")).Text.ToString().Trim(),
                                                    ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text.ToString());

            gvTax.Rebind();
            BindVendorInfo();

        }

        TaxRebind();
    }

    protected void gvTax_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            RadLabel lbl1 = (RadLabel)e.Item.FindControl("lblIsGst");

            if (db != null)
            {
                if (ViewState["EDITMODE"] != null && ViewState["EDITMODE"].ToString() == "0")
                    db.Visible = true;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                if (lbl1.Text == "1")
                    db.Visible = false;
            }
        }

    }

    protected void gvOrderLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                // _gridView.EditIndex = -1;
                return;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidOrderLine(
             ((UserControlDecimal)e.Item.FindControl("txtInvoiceEdit")).Text.ToString().Trim(),
             ((RadTextBox)e.Item.FindControl("txtOrderLineRemarksEdit")).Text.ToString()
               ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateOrderLineStagingInvoiceDifference(((RadLabel)e.Item.FindControl("lblOrderLineId")).Text,
                        ((UserControlDecimal)e.Item.FindControl("txtInvoiceEdit")).Text.Replace("_", "0"),
                        ((RadTextBox)e.Item.FindControl("txtOrderLineRemarksEdit")).Text.ToString());
                gvOrderLine.Rebind();
                gvTax.Rebind();
                BindVendorInfo();

            }
            Rebind();
            TaxRebind();
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
        }
        SetKeyDownScroll(sender, e);
    }

    protected void gvOrderLine_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected bool IsValidTax(string strdiscount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strdiscount.Trim() == string.Empty)
            ucError.ErrorMessage = "Discount is required.";

        return (!ucError.IsError);
    }

    protected void UpdateOrderTaxStagingInvoiceDifference(string ordertaxcode, string invoicedifference, string remarks)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderTaxStagingInvoiceDifferenceUpdate(new Guid(ordertaxcode), General.GetNullableDecimal(invoicedifference), remarks);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('invoicediff',null,'keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void UpdateOrderLineStagingInvoiceDifference(string orderlineid, string InvoiceDifference, string remarks)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderLineStagingInvoiceDifferenceUpdate(new Guid(orderlineid), General.GetNullableDecimal(InvoiceDifference), remarks);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('invoicediff',null,'keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    private bool IsValidForm(string Discount, string quantity)
    {
        if (Discount.Trim().Equals("") || Discount == "0")
            ucError.ErrorMessage = "Discount is required.";

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindVendorInfo();
        BindDataTax();
        BindData();
    }

    protected bool IsValidOrderLine(string strinvoicediffamt, string strinvoicediffremark)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strinvoicediffamt.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice difference amount is required.";
        if (strinvoicediffremark.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice difference remark is required.";

        return (!ucError.IsError);
    }

    protected bool IsValidTax(string strinvoicediffamt, string strinvoicediffremark)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strinvoicediffamt.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice difference amount is required.";
        if (strinvoicediffremark.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice difference remark is required.";

        return (!ucError.IsError);
    }
    protected void TaxRebind()
    {
        gvTax.SelectedIndexes.Clear();
        gvTax.EditIndexes.Clear();
        gvTax.DataSource = null;
        gvTax.Rebind();
    }
    protected void Rebind()
    {
        gvOrderLine.SelectedIndexes.Clear();
        gvOrderLine.EditIndexes.Clear();
        gvOrderLine.DataSource = null;
        gvOrderLine.Rebind();
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

}
