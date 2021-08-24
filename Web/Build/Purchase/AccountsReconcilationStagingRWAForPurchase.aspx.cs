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

public partial class AccountsReconcilationStagingRWAForPurchase : PhoenixBasePage
{
    PhoenixToolbar toolbarmain;
    PhoenixToolbar toolbar;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        // txtVendorId.Attributes.Add("style", "visibility:hidden");
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/AccountsReconcilationStagingRWAForPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderLine')", "Print Grid", "icon_print.png", "PRINT");

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Purchase/AccountsReconcilationStagingRWAForPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvTax')", "Print Grid", "icon_print.png", "PRINT");
            //toolgrid.AddImageLink("../Accounts/AccountsReconcilationStagingRWAForPurchase.aspx", "Add Gst", "add.png", "GST");

            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["nextIndex"] = -1;
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

                if (ViewState["ORDERID"].ToString() != "")
                {
                    cmdShowInvoiceDiff.Attributes.Add("onclick", "openNewWindow('invoicediff', '', '" + Session["sitepath"] + "/Accounts/AccountsReconcilationStagingInvoiceDiff.aspx?ORDERID=" + ViewState["ORDERID"].ToString() + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString() + "');return false;");
                    //cmdShowInvoiceDiff.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','AccountsReconcilationStagingInvoiceDiff.aspx?ORDERID=" + ViewState["ORDERID"].ToString() + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString() + "');");
                    imgShowVessel.Attributes.Add("onclick", "return showPickList('spnPickListVessel', 'codehelp1', '', '../Common/CommonPickListVessel.aspx', true); ");
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FORMTYPE"] = null;
                ViewState["INVOICESTATUS"] = null;
                ViewState["CURRENCYCONVERTEDYN"] = null;
                ViewState["CONFIRMATIONMESSAGE"] = null;
                ViewState["APPROVALSTATUS"] = null;
                ViewState["ADDITIONALDISCOUNT"] = null;
                ViewState["VESSELACCOUNT"] = null;
                ViewState["Ownerid"] = null;
                BindVendorInfo();
                //BindData();

                BindUsercontrol();
            }

            if ((ViewState["INVOICESTATUS"].ToString() == "632" || ViewState["INVOICESTATUS"].ToString() == "242") && ViewState["APPROVALSTATUS"].ToString() == "0")
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);

                MenuStaging.AccessRights = this.ViewState;
                MenuStaging.MenuList = toolbar.Show();
                MenuStaging.Visible = true;
            }

            if ((ViewState["INVOICESTATUS"].ToString() == "632" || ViewState["INVOICESTATUS"].ToString() == "242") && ViewState["APPROVALSTATUS"].ToString() != "0")
            {
                MenuStaging.Visible = false;
                tbldiv.Visible = false;
            }
            if (ViewState["FORMTYPE"].ToString() == "3" || ViewState["FORMTYPE"].ToString() == "4")
            {
                ucVesselAccount.Enabled = true;
                cmdVesselAccount.Visible = true;
            }
            else
            {
                ucVesselAccount.Enabled = false;
                cmdVesselAccount.Visible = false;
            }

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("PO", "PO", ToolBarDirection.Right);
            toolbarmain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);
            toolbarmain.AddButton("Next", "NEXT", ToolBarDirection.Right);
            toolbarmain.AddButton("Prev", "PREV", ToolBarDirection.Right);
            MenuLineItem.Title = "Invoice Matching";
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();


            //TaxRebind();
            //BindData();
            //AddDisRebind();

            //    toolgrid.AddImageLink("javascript:Openpopup('Filter','','AccountsInvoiceReconciliationGstAdd.aspx?orderid=" + ViewState["ORDERID"].ToString() + "'); return false;", "Add", "add.png", "GST");
            toolgrid.AddImageLink("javascript:openNewWindow('codehelpactivity','','Accounts/AccountsInvoiceReconciliationGstAdd.aspx?orderid=" + ViewState["ORDERID"].ToString() + "')", "Add", "add.png", "GST");

            AdditionalChargeItem.AccessRights = this.ViewState;
            AdditionalChargeItem.MenuList = toolgrid.Show();

            txtBulkBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetgroupId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden;");

            if (btnShowBulkBudget != null && ViewState["VESSELID"] != null)
            {
                btnShowBulkBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            }
            if (btnShowBulkOwnerBudget != null && ViewState["VESSELID"] != null)
            {
                btnShowBulkOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBulkBudgetId.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
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
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString() + '/' + dr["FLDINVOICECURRENCYCODE"].ToString();
            txtReceivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtVenderName.Text = dr["FLDNAME"].ToString();
            txtVendorCode.Text = dr["FLDCODE"].ToString();
            txtVendorId.Text = dr["FLDVENDORID"].ToString();
            txtPortName.Text = dr["FLDSEAPORTNAME"].ToString();
            txtInvoiceNumber.Text = dr["FLDINVOICENUMBER"].ToString();
            txtPOAmountAccordingRecievedGoods.Text = string.Format(String.Format("{0:#####.00}", dr["FLDPOPAYBLEAMONTACCOUNTSRECEIVEDGOODSWITHINVOICEDIFF"]));
            txtInvoiceAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDINVOICEAMOUNT"]));
            txtBalanceAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDBALANCEAMOUNTWITHINVOICEDIFF"]));
            txtPOAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDACTUALPOPAYBLEAMOUNT"]));
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["EDITMODE"] = dr["FLDEDITMODE"].ToString();
            ViewState["FORMTYPE"] = dr["FLDFORMTYPE"].ToString();
            ViewState["INVOICESTATUS"] = dr["FLDINVOICESTATUS"].ToString();
            ViewState["CURRENCYCONVERTEDYN"] = dr["FLDCURRENCYCONVERTEDYN"].ToString();
            ucVesselAccount.Text = dr["FLDACCOUNTCODE"].ToString();
            ucVesselAccount.SelectedValue = dr["FLDACCOUNTID"].ToString();
            ViewState["VESSELACCOUNT"] = dr["FLDACCOUNTID"].ToString();
            ViewState["Ownerid"] = dr["FLDPRINCIPALID"].ToString();
            decimal Diffamount = 0;
            if (ViewState["FLDSUMAMOUNT"] != null)
            {
                Diffamount = Convert.ToDecimal(ViewState["FLDSUMAMOUNT"]);
            }
            ViewState["CONFIRMATIONMESSAGE"] = dr["FLDINVOICECURRENCYCODE"].ToString() + string.Format(String.Format("{0:#####.00}", dr["FLDCURRENCYCONVERTEDAMOUNT"])) + " (" + string.Format(String.Format("{0:#####.00}", dr["FLDINVOICEAMOUNT"])) + "-"
                                        + string.Format(String.Format("{0:#####.00}", Diffamount)) + ")";

            if (dr["FLDCURRENCYCODE"].ToString() == dr["FLDINVOICECURRENCYCODE"].ToString())
            {
                txtExchangeRateEdit.Text = "1.000000";
                txtExchangeRateEdit.ReadOnly = true;
                cmdCurrencyConvert.Visible = false;
            }

            ViewState["APPROVALSTATUS"] = dr["FLDAPPROVALSTATUS"].ToString();

            if (dr["FLDAPPROVALSTATUS"] != null && dr["FLDAPPROVALSTATUS"].ToString() != "")
            {
                if (Convert.ToInt32(dr["FLDAPPROVALSTATUS"]) == 0 || (dr["FLDFORMTYPE"].ToString() == "3" && (ViewState["INVOICESTATUS"].ToString() == "632" || ViewState["INVOICESTATUS"].ToString() == "242")))
                {
                    txtAprooverStatus.Text = "Awaiting Approval";
                    tbldiv.Visible = true;
                    if (ViewState["INVOICESTATUS"].ToString() == "632" || ViewState["INVOICESTATUS"].ToString() == "242")
                    {
                        toolbar = new PhoenixToolbar();
                        toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);

                        MenuStaging.AccessRights = this.ViewState;
                        MenuStaging.MenuList = toolbar.Show();
                        MenuStaging.Visible = true;
                    }
                    else
                    {
                        tbldiv.Visible = false;
                        MenuStaging.Visible = false;
                    }
                }

                else
                {

                    txtAprooverStatus.Text = "Approved";
                    //    ViewState["INVOICESTATUS"].ToString() != "242" &&
                    if (ViewState["INVOICESTATUS"].ToString() != "632")
                        tbldiv.Visible = false;
                }
            }
            else
            {
                txtAprooverStatus.Text = "Approved";
                //  ViewState["INVOICESTATUS"].ToString() != "242" &&
                if (ViewState["INVOICESTATUS"].ToString() != "632")
                    tbldiv.Visible = false;
            }
        }
    }

    protected void BindUsercontrol()
    {
        int? accountid = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());
        int? budgetid = General.GetNullableInteger(txtBulkBudgetId.Text);
        ucProjectcode.bind(accountid, budgetid);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDPARTNAME", "FLDPRICE","FLDRECEIVEDQUANTITY","FLDPOSUBACCOUNT","FLDOWNERBUDGETCODE","FLDSUBACCOUNT"
                                     ,"FLDNEWOWNERBUDGETCODE","FLDPROJECTCODE","FLDOWNERDISCOUNT","FLDAMOUNT","FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
        string[] alCaptions = {"Line Item ", "PO Unit Price ", "Recived Quantity ","Original Budget Code ","Original Owner Code","New Budget Code"
                                      ,"New Owner Code","Project Code","Discount","Amount","Original Cr. Note Discount","New Cr. Note Discount","Amount After Discount" };

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
                                     ,"FLDNEWOWNERBUDGETCODE","FLDPROJECTCODE","FLDOWNERDISCOUNT","FLDAMOUNT","FLDPODISCOUNT","FLDDISCOUNT","FLDTOTALAMOUNT"};
            string[] alCaptions = {"Line Item ", "PO Unit Price ", "Recived Quantity ","Original Budget Code ","Original Owner Code","New Budget Code"
                                      ,"New Owner Code","Project Code","Discount","Amount","Original Cr. Note Discount","New Cr. Note Discount","Amount After Discount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixAccountsPOStaging.OrderLineStagingSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                gvOrderLine.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
            BindFooter();

            gvOrderLine.DataSource = ds;
            gvOrderLine.VirtualItemCount = iRowCount;


            txtAprooverStatus.Text = "";
            //AdditionalChargeItem.MenuList = "";

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            gvOrderLine.Columns[13].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[14].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[15].Visible = (showcreditnotedisc == 1) ? true : false;

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

    private void BindFooter()
    {
        DataSet dsTotal = PhoenixAccountsPOStaging.OrderLineStagingTotal(new Guid(ViewState["ORDERID"].ToString()));
        if (dsTotal.Tables[0].Rows.Count > 0)
        {

            DataRow dr = dsTotal.Tables[0].Rows[0];
            ViewState["FLDSUMAMOUNT"] = dr["FLDSUMAMOUNT"].ToString();
            ViewState["FLDSUMDISCOUN"] = dr["FLDSUMDISCOUN"].ToString();
            ViewState["FLDSUMTOTALAMOUNT"] = dr["FLDSUMTOTALAMOUNT"].ToString();
            ViewState["FLDSUMVESSELDISCOUNT"] = dr["FLDSUMVESSELDISCOUNT"].ToString();
            ViewState["FLDSUMTOTALVESSELAMOUNT"] = dr["FLDSUMTOTALVESSELAMOUNT"].ToString();

            ViewState["FLDSUMAMOUNTWITHINVOICEDIFFAMT"] = dr["FLDSUMAMOUNTWITHINVOICEDIFFAMT"].ToString();
            ViewState["FLDSUMTOTALAMOUNTWITHINVOICEDIFFAMT"] = dr["FLDSUMTOTALAMOUNTWITHINVOICEDIFFAMT"].ToString();
        }
        else
        {
            ViewState["FLDSUMAMOUNT"] = 0;
            ViewState["FLDSUMDISCOUN"] = 0;
            ViewState["FLDSUMTOTALAMOUNT"] = 0;
            ViewState["FLDSUMVESSELDISCOUNT"] = 0;
            ViewState["FLDSUMTOTALVESSELAMOUNT"] = 0;

            ViewState["FLDSUMAMOUNTWITHINVOICEDIFFAMT"] = 0;
            ViewState["FLDSUMTOTALAMOUNTWITHINVOICEDIFFAMT"] = 0;
        }
    }

    private void BindTotalFooter()
    {
        DataSet dsTotal = PhoenixAccountsPOStaging.OrderFormStagingTotalPOAmount(new Guid(ViewState["ORDERID"].ToString()));
        if (dsTotal.Tables[0].Rows.Count > 0)
        {

            DataRow dr = dsTotal.Tables[0].Rows[0];
            ViewState["FLDTOTALAMOUNT"] = dr["FLDTOTALAMOUNT"].ToString();
            ViewState["FLDTOTALDISCOUNT"] = dr["FLDTOTALDISCOUNT"].ToString();
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"] = dr["FLDTOTALAFTERDISCOUNTAMOUNT"].ToString();
            ViewState["FLDTOTALVESSELDISCOUNT"] = dr["FLDTOTALVESSELDISCOUNT"].ToString();
            ViewState["FLDTOTALVESSELAMOUNT"] = dr["FLDTOTALVESSELAMOUNT"].ToString();

            ViewState["FLDTOTALAMOUNTWITHINVOICEDIFFAMT"] = dr["FLDTOTALAMOUNTWITHINVOICEDIFFAMT"].ToString();
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNTWITHINVOICEDIFFAMT"] = dr["FLDTOTALAFTERDISCOUNTAMOUNTWITHINVOICEDIFFAMT"].ToString();
        }
        else
        {
            ViewState["FLDTOTALAMOUNT"] = 0;
            ViewState["FLDTOTALDISCOUNT"] = 0;
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"] = 0;
            ViewState["FLDTOTALVESSELDISCOUNT"] = 0;
            ViewState["FLDTOTALVESSELAMOUNT"] = 0;

            ViewState["FLDTOTALAMOUNTWITHINVOICEDIFFAMT"] = 0;
            ViewState["FLDTOTALAFTERDISCOUNTAMOUNTWITHINVOICEDIFFAMT"] = 0;
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
    protected void AdditionalChargeItem_TabStripCommand(object sender, EventArgs e)
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
                ShowExcelAdditionalChargeItem();
            }

            if (CommandName.ToUpper().Equals("GST"))
            {

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
                Response.Redirect("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"].ToString());
            }
            else if (CommandName.ToUpper().Equals("INVOICE"))
            {
                if (ViewState["callfrom"].ToString() == "PR")
                {
                    string s = ViewState["invoicecode"].ToString();
                    Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "invoice")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "AD")
                {
                    //  string str = Request.QueryString["qinvoicecode"].ToString();
                    Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());

                }
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Purchase/AccountsInvoiceAttachmentsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("NEXT"))
            {
                DataSet ds = PhoenixAccountsPOStaging.OrderNext(new Guid(ViewState["ORDERID"].ToString()),
                                                                new Guid(ViewState["invoicecode"].ToString()),
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //int nextIndex = int.Parse(ViewState["nextIndex"].ToString()) + 1;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["invoicecode"] = dr["FLDINVOICECODE"].ToString();
                    ViewState["ORDERID"] = dr["FLDORDERID"].ToString();

                    BindVendorInfo();
                    TaxRebind();
                    Rebind();
                    AddDisRebind();

                }
                else
                {
                    ucError.ErrorMessage = "No data found";
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("PREV"))
            {
                DataSet ds = PhoenixAccountsPOStaging.OrderPrev(new Guid(ViewState["ORDERID"].ToString()),
                                                                new Guid(ViewState["invoicecode"].ToString()),
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //int prevIndex = int.Parse(ViewState["nextIndex"].ToString()) - 1;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["invoicecode"] = dr["FLDINVOICECODE"].ToString();
                    ViewState["ORDERID"] = dr["FLDORDERID"].ToString();

                    BindVendorInfo();
                    TaxRebind();
                    Rebind();
                    AddDisRebind();
                }
                else
                {
                    ucError.ErrorMessage = "No data found";
                    ucError.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStaging_TabStripCommand(object sender, EventArgs e)
    {
        string strInvoiceCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (ViewState["CURRENCYCONVERTEDYN"].ToString() == "0")
                {
                    if (ViewState["ORDERID"] != null)
                    {
                        if (ViewState["FORMTYPE"].ToString() == "3")
                        {
                            PhoenixAccountsPOStaging.DirectPOInvoiceStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        }
                        else
                            PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        Rebind();
                        TaxRebind();
                        BindVendorInfo();

                        if (MenuStaging.Visible == true)
                        {
                            MenuStaging.Visible = false;
                            tbldiv.Visible = false;
                        }
                        ViewState["nextIndex"] = 0;
                    }
                }
                else
                {
                    RadWindowManager1.RadConfirm("<b>CURRENCY CONVERSION!!!</b><br><br>Invoice is more than PO by:<br>" + ViewState["CONFIRMATIONMESSAGE"].ToString() + "<br>Please re-confirm the additional charges are approved.", "confirm", 320, 180, null, "Confirm Message");
                    // ucConfirmSent.HeaderMessage = "CURRENCY CONVERSION!!!";
                    // ucConfirmSent.OKText = "Approve";
                    // ucConfirmSent.CancelText = "Cancel";
                    // ucConfirmSent.ErrorMessage = "Invoice is more than PO by:<br>" + ViewState["CONFIRMATIONMESSAGE"].ToString() + "<br>Please re-confirm the additional charges are approved.";
                    // ucConfirmSent.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Convertcurrency(object sender, ImageClickEventArgs arg)
    {
        if (ViewState["ORDERID"] != null)
        {
            if (txtExchangeRateEdit.Text != string.Empty)
            {
                try
                {
                    PhoenixAccountsPOStaging.OrderFormStagingConvertCurreny(PhoenixSecurityContext.CurrentSecurityContext.UserCode, decimal.Parse(txtExchangeRateEdit.Text), new Guid(ViewState["ORDERID"].ToString()));
                    Response.Redirect("AccountsReconcilationStagingRWAForPurchase.aspx?ORDERID=" + ViewState["ORDERID"].ToString() + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString());
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else
            {
                ucError.ErrorMessage = "Exchangerate is required.";
                ucError.Visible = true;
                return;
            }
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
        //ViewState["orderlineid"] = ((Label)gvOrderLine.Rows[e.NewSelectedIndex].FindControl("lblLineid")).Text;
        //if (gvOrderLine.EditIndex > -1)
        //    gvOrderLine.UpdateRow(gvOrderLine.EditIndex, false);
        //gvOrderLine.EditIndex = -1;
        //BindData();

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

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER1"], 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);
        BindTotalFooter();

        gvTax.DataSource = ds;
        gvTax.VirtualItemCount = iTaxRowCount;


        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvTax.Columns[11].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[12].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[13].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["TAXROWCOUNT"] = iTaxRowCount;
        ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        General.SetPrintOptions("gvTax", "ADDITIONAL CHARGE ITEM", alCaptions, alColumns, ds);
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

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearch(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);

        gvAdditionalDiscount.DataSource = ds;
        gvAdditionalDiscount.VirtualItemCount = iTaxRowCount;


        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvAdditionalDiscount.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;

        //ViewState["TAXROWCOUNT"] = iTaxRowCount;
        //ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        //General.SetPrintOptions("gvTax", "ADDITIONAL CHARGE ITEM", alCaptions, alColumns, ds);
    }
    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {

        // GridView _gridView = (GridView)sender;
        // int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            int status = 0;
            if (!IsValidTaxAdditonal(
                ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.ToString().Trim(),
                (((RadTextBox)e.Item.FindControl("txtBudgetCodeEdit")).Text.ToString().Trim()),
                (((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text.ToString().Trim()),
                ((UserControlDecimal)e.Item.FindControl("txtAmount")).Text.ToString().Trim()))
            {
                ucError.Visible = true;
                return;
            }
            InsertOrderTaxStaging(
                                  ViewState["ORDERID"].ToString()
                                , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.ToString().Trim()
                                , ((RadTextBox)e.Item.FindControl("txtBudgetCodeEdit")).Text.ToString().Trim()
                                , decimal.Parse(((UserControlDecimal)e.Item.FindControl("txtAmount")).Text)
                                , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text.ToString())
                                , ref status
                                , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd")).SelectedProjectCode));
            BindVendorInfo();
            TaxRebind();
            AddDisRebind();
            //String script = String.Format("javascript:parent.fnReloadList('code1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            //UpdateApprovalStatus(ViewState["ORDERID"].ToString());
            if (status == 1)
            {
                UpdateApprovalStatus(ViewState["ORDERID"].ToString());
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Approve", "APPROVE");
                MenuStaging.AccessRights = this.ViewState;
                MenuStaging.MenuList = toolbar.Show();
                MenuStaging.Visible = true;
                tbldiv.Visible = true;
            }
        }

        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            int status = 0;

            string amount = null;
            int isstagingaddeditem = 0;
            RadLabel lbl1 = (RadLabel)e.Item.FindControl("lblIsGst");
            UserControlDecimal txtValueEdit = (UserControlDecimal)e.Item.FindControl("txtValueEdit");

            if (lbl1.Text == "1" && txtValueEdit != null)
            {
                if (!IsValidTax(
                  ((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text))
                {
                    ucError.ErrorMessage = "";
                    ucError.ErrorMessage = "Value is required";
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoice.InvoiceDirectPOTaxUpdateStaging(new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapCode")).Text.ToString())
                    , ((RadLabel)e.Item.FindControl("lblDescriptionEdit")).Text
                    , 2
                    , decimal.Parse(txtValueEdit.Text)
                    , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.ToString())
                    , General.GetNullableDecimal(null)
                    , (byte?)General.GetNullableInteger(lbl1.Text)
                    , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text.ToString())
                     , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode)
                    );
            }
            else
            {

                string txtDiscountEdit = (General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text.ToString()) == null) ? "0.00" : ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text.ToString();


                if (((UserControlDecimal)e.Item.FindControl("ucAmountEdit")) != null)
                    amount = ((UserControlDecimal)e.Item.FindControl("ucAmountEdit")).Text.ToString();
                else if ((((RadLabel)e.Item.FindControl("lblAmountEdit")) != null))
                    amount = ((RadLabel)e.Item.FindControl("lblAmountEdit")).Text.ToString();
                if ((((RadLabel)e.Item.FindControl("lblIsStagingAddedItem")) != null))
                    isstagingaddeditem = int.Parse(((RadLabel)e.Item.FindControl("lblIsStagingAddedItem")).Text);

                if (!IsValidTaxAdditonal(((RadLabel)e.Item.FindControl("lblDescriptionEdit")).Text.ToString(),
                                            ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.ToString(),
                                            ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text, amount
                                            )
                    )
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotationTax(ViewState["ORDERID"].ToString()
                                    , ((RadTextBox)e.Item.FindControl("txtTaxMapCode")).Text.ToString()
                                    , txtDiscountEdit
                                    , ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.ToString()
                                    , amount
                                    , isstagingaddeditem
                                    , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text
                                    , ref status
                                    , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode)
                                    );
            }
            if (status == 1)
            {
                UpdateApprovalStatus(ViewState["ORDERID"].ToString());
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Approve", "APPROVE");
                MenuStaging.AccessRights = this.ViewState;
                MenuStaging.MenuList = toolbar.Show();
                MenuStaging.Visible = true;
                tbldiv.Visible = true;
            }
            BindVendorInfo();
            TaxRebind();
            AddDisRebind();
            //toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Approve", "APPROVE");
            //MenuStaging.AccessRights = this.ViewState;
            //MenuStaging.MenuList = toolbar.Show();
            //MenuStaging.Visible = true;
            //tbldiv.Visible = true;
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            // _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            TaxRebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                PhoenixAccountsPOStaging.OrderTaxStagingDelete(new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapCode")).Text.ToString())
                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                BindVendorInfo();
                TaxRebind();
                AddDisRebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        // TaxRebind();
    }

    protected void gvTax_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            RadLabel lbl1 = (RadLabel)e.Item.FindControl("lblIsGst");

            if (db != null)
            {
                if (ViewState["EDITMODE"] != null && ViewState["EDITMODE"].ToString() == "0")
                    db.Visible = true;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                //if (lbl1.Text == "1")
                //    db.Visible = false;

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
            ImageButton ib2 = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");

            RadLabel lbl1 = (RadLabel)e.Item.FindControl("lblIsGst");

            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (lbl1 != null && lbl1.Text == "1" && ib1 != null)
            {
                ib1.Visible = false;
                UserControlDecimal txtvalueEdit = (UserControlDecimal)e.Item.FindControl("txtValueEdit");
                RadLabel lblValueEdit = (RadLabel)e.Item.FindControl("lblValueEdit");

                if (ib2 != null)
                    ib2.Visible = false;

                if (txtvalueEdit != null)
                {
                    lblValueEdit.Visible = false;
                    txtvalueEdit.Visible = true;
                }

                RadLabel lblTaxAmountEdit = (RadLabel)e.Item.FindControl("lblTaxAmountEdit");
                UserControlDecimal ucAmountEdit = (UserControlDecimal)e.Item.FindControl("ucAmountEdit");
                if (lblTaxAmountEdit != null)
                {
                    lblTaxAmountEdit.Visible = true;
                    ucAmountEdit.Visible = false;
                }



                RadLabel lblDiscountEdit = (RadLabel)e.Item.FindControl("lblDiscountEdit");
                UserControlDecimal txtDiscountEdit = (UserControlDecimal)e.Item.FindControl("txtDiscountEdit");
                if (lblDiscountEdit != null)
                {
                    lblDiscountEdit.Visible = true;
                    txtDiscountEdit.Visible = false;
                }
            }
            // if (lblAdditionalDiscountYN != null && lblAdditionalDiscountYN.Text == "1")
            // {
            //     // GridViewRow gvr = e.Item;
            //     // gvr.Visible = false;
            //     e.Item.Visible = false;
            // }
            // if (lblAdditionalDiscountYN != null && lblAdditionalDiscountYN.Text == "2")
            // {
            //     // GridViewRow gvr = e.Item;
            //     // gvr.BackColor = System.Drawing.Color.Red;
            //     // gvr.ForeColor = System.Drawing.Color.White;
            //     // gvr.Visible = true;
            //     e.Item.BackColor = System.Drawing.Color.Red;
            //     e.Item.ForeColor = System.Drawing.Color.White;
            //     e.Item.Visible = true;
            // }

            RadLabel lblAdditionalDiscountYN = (RadLabel)e.Item.FindControl("lblAdditionalDiscountYN");
            ViewState["ADDITIONALDISCOUNT"] = lblAdditionalDiscountYN.Text;
            GridDataItem item = (GridDataItem)e.Item;

            if (ViewState["ADDITIONALDISCOUNT"] != null && ViewState["ADDITIONALDISCOUNT"].ToString() == "1")
            {
                item.Display = false;
            }

            if (ViewState["ADDITIONALDISCOUNT"] != null && ViewState["ADDITIONALDISCOUNT"].ToString() == "2")
            {
                item.BackColor = System.Drawing.Color.Red;
                item.ForeColor = System.Drawing.Color.White;
                item.Display = true;
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
            RadLabel lblPOBudgetId = (RadLabel)e.Item.FindControl("lblPOBudgetId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ibtnShowOwnerBudgetEdit != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(VesselAccountId, BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }


        }

        if (e.Item is GridEditableItem)
        {
            RadLabel lblIsStagingAdded = (RadLabel)e.Item.FindControl("lblIsStagingAdded");
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (lblIsStagingAdded != null && lblIsStagingAdded.Text != "1")
            {
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
            }
            else
                if (cmdDelete != null)
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

        }

        if (e.Item is GridEditableItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblTaxInvoiceDifferenceAmount");

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipTaxRemark");
                lbl.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lbl.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
        }


        if (e.Item is GridFooterItem)
        {
            RadLabel lb1 = (RadLabel)e.Item.FindControl("lblTaxAmountFooter");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblTaxDiscountFooter");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTaxTotalFooter");
            if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDTOTALAMOUNTWITHINVOICEDIFFAMT"].ToString()).ToString("########0.00");

            //if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDTOTALAMOUNT"].ToString()).ToString("########0.00");
            if (lb2 != null) lb2.Text = decimal.Parse(ViewState["FLDTOTALDISCOUNT"].ToString()).ToString("########0.00");
            //if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDTOTALAFTERDISCOUNTAMOUNT"].ToString()).ToString("########0.00");

            if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDTOTALAFTERDISCOUNTAMOUNTWITHINVOICEDIFFAMT"].ToString()).ToString("########0.00");
        }
        if (e.Item is GridFooterItem)
        {
            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            tb = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            ImageButton ib = (ImageButton)e.Item.FindControl("btnShowBudget");
            {
                if (ib != null) ib.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (ib != null)
                    if (!SessionUtil.CanAccess(this.ViewState, ib.CommandName)) ib.Visible = false;
            }

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton dbadd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (dbadd != null)
            {
                if (ViewState["EDITMODE"] != null && ViewState["EDITMODE"].ToString() == "0")
                    dbadd.Visible = true;
                else
                    dbadd.Enabled = false;
                if (!SessionUtil.CanAccess(this.ViewState, dbadd.CommandName)) dbadd.Visible = false;
            }

            ImageButton btnShowOwnerBudget = (ImageButton)e.Item.FindControl("btnShowOwnerBudget");
            if (btnShowOwnerBudget != null && txtBudgetIdEdit != null)
            {
                btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          
                if (!SessionUtil.CanAccess(this.ViewState, btnShowOwnerBudget.CommandName)) btnShowOwnerBudget.Visible = false;
            }

            UserControls_UserControlProjectCode Projectcode = ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd"));

            int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());
            int? Budget = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
            Projectcode.bind(VesselAccountId, Budget);
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

            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;

                int status = 0;

                if (!IsVaildOrderBudgetcode(((RadTextBox)(e.Item.FindControl("txtOwnerBudgetCodeEdit"))).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateQuotationLineItem(((RadLabel)e.Item.FindControl("lblOrderId")).Text,
                        ((RadLabel)e.Item.FindControl("lblOrderLineId")).Text,
                        ((UserControlDecimal)e.Item.FindControl("txtUnitPriceEdit")).Text.Replace("_", "0"),
                        ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text.Replace("_", "0"),
                        ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text,
                         ref status,
                        ((UserControlDecimal)e.Item.FindControl("txtOwnerDiscountEdit")).Text.Replace("_", "0"),
                         ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode
                        );
                if (txtAprooverStatus.Text == "Approved" && status == 1)
                {
                    UpdateApprovalStatus(((RadLabel)(e.Item.FindControl("lblOrderId"))).Text);
                    toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                    MenuStaging.AccessRights = this.ViewState;
                    MenuStaging.MenuList = toolbar.Show();
                    MenuStaging.Visible = true;
                    tbldiv.Visible = true;
                }
                // gvOrderLine.Rebind();
                Rebind();
                TaxRebind();
                BindVendorInfo();


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
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton cmdDiscountUpdateForAll = (ImageButton)e.Item.FindControl("cmdDiscountUpdateForAll");
            if (db != null)
            {
                if (ViewState["EDITMODE"] != null && ViewState["EDITMODE"].ToString() == "0")
                    db.Visible = true;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            }
            if (cmdDiscountUpdateForAll != null)
            {
                //  cmdDiscountUpdateForAll.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Accounts/AccountsReconcilationStagingDiscountUpdatePopUp.aspx?ORDERID=" + ViewState["ORDERID"] + "'); return false;");
                cmdDiscountUpdateForAll.Attributes.Add("onclick", "openNewWindow('NAFA','','Accounts/AccountsReconcilationStagingDiscountUpdatePopUp.aspx?ORDERID=" + ViewState["ORDERID"] + "');return true;");

                if (!SessionUtil.CanAccess(this.ViewState, cmdDiscountUpdateForAll.CommandName)) cmdDiscountUpdateForAll.Visible = false;
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
            RadLabel lblPOBudgetId = (RadLabel)e.Item.FindControl("lblPOBudgetId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ibtnShowOwnerBudgetEdit != null && ViewState["VESSELID"] != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? Account = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());
                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(Account, BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }


            if (e.Item is GridDataItem)
            {
                if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
                {
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblInvoiceDifference");
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemark");
                    lbl.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                    lbl.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
                }

                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                ImageButton cmdDiscountUupdateForAll = (ImageButton)e.Item.FindControl("cmdDiscountUpdateForAll");
                if (showcreditnotedisc != 1 && cmdDiscountUupdateForAll != null)
                    cmdDiscountUupdateForAll.Visible = false;
            }
            if (e.Item is GridFooterItem)
            {
                RadLabel lb1 = (RadLabel)e.Item.FindControl("lblAmountfooter");
                RadLabel lb2 = (RadLabel)e.Item.FindControl("lblDiscountfooter");
                RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTotalAmountfooter");
                //if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDSUMAMOUNT"].ToString()).ToString("######0.00");
                if (lb2 != null) lb2.Text = decimal.Parse(ViewState["FLDSUMDISCOUN"].ToString()).ToString("######0.00");
                //if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDSUMTOTALAMOUNT"].ToString()).ToString("######0.00");

                if (lb1 != null) lb1.Text = decimal.Parse(ViewState["FLDSUMAMOUNTWITHINVOICEDIFFAMT"].ToString()).ToString("######0.00");
                if (lb3 != null) lb3.Text = decimal.Parse(ViewState["FLDSUMTOTALAMOUNTWITHINVOICEDIFFAMT"].ToString()).ToString("######0.00");
            }
        }
    }

    protected bool IsValidTax(string strdiscount, string ownerBudgetCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strdiscount.Trim() == string.Empty)
            ucError.ErrorMessage = "Discount is required.";
        //if (ownerBudgetCode.Trim() == string.Empty)
        //    ucError.ErrorMessage = "Owner Code is required.";
        //if (budgetid.Trim() == string.Empty)
        //   ucError.ErrorMessage = "Budget Code is required.";     

        return (!ucError.IsError);
    }

    protected bool IsValidAdditionalDiscount(string strdiscount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strdiscount.Trim() == string.Empty)
            ucError.ErrorMessage = "New Additional Discount is required.";
        //if (budgetid.Trim() == string.Empty)
        //   ucError.ErrorMessage = "Budget Code is required.";     
        return (!ucError.IsError);
    }

    protected bool IsValidTaxAdditonal(string description, string budgetid, string ownerBudgetCode, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (description.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget Code is required.";
        //if (ownerBudgetCode.Trim() == string.Empty)
        //    ucError.ErrorMessage = "Owner code is required.";
        if (amount.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }


    protected bool IsVaildOrderBudgetcode(string budgetcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       // if (budgetcode == null)
       //     ucError.ErrorMessage = "New Owner Code is required.";
        // if ((General.GetNullableInteger(ProjectcodeEdit.SelectedProjectCode)) == null)
        //     ucError.ErrorMessage = "Project Code is required.";

        return (!ucError.IsError);
    }



    protected void UpdateQuotationTax(string orderid, string taxmapcode, string discount, string budgetcodeid, string amount, int isstagingaddeditem, string newownerbudgetid, ref int status,int? projectcode)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderTaxStagingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid), new Guid(taxmapcode), General.GetNullableDecimal(discount), General.GetNullableInteger(budgetcodeid), General.GetNullableDecimal(amount), isstagingaddeditem, General.GetNullableGuid(newownerbudgetid), ref status, projectcode);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void UpdateQuotationLineItem(string orderid, string orderlineid, string unitprice, string discount, string budgetid, string newownerbudgetid, ref int status, string ownerdiscount, string ProjectId)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderLineStagingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid), new Guid(orderlineid), General.GetNullableDecimal(unitprice), General.GetNullableDecimal(discount),
                General.GetNullableInteger(budgetid), General.GetNullableGuid(newownerbudgetid), ref status,
                General.GetNullableDecimal(ownerdiscount), General.GetNullableInteger(ProjectId));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private void UpdateApprovalStatus(string orderid)
    {
        PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(orderid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }

    private bool IsValidForm(string Discount, string quantity)
    {
        if (Discount.Trim().Equals("") || Discount == "0")
            ucError.ErrorMessage = "Discount is required.";

        return (!ucError.IsError);
    }

    protected string PrepareApprovalText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Quotation is Received from  " + dr["FLDNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");

        return sbemailbody.ToString();

    }
    private void UpdateQuotation()
    {
        if (ucCurrency.SelectedCurrency.ToString().Trim() == "" || ucCurrency.SelectedCurrency.ToString() == "0")
        {
            ucError.ErrorMessage = "Quoted Currency is Required.";
            ucError.Visible = true;
            return;
        }
        if (ViewState["quotationid"].ToString() == "")
        {
            ucError.ErrorMessage = " ";
            ucError.Visible = true;
            return;
        }
    }

    protected void onPurchaseQuotationLine(object sender, CommandEventArgs e)
    {
        ViewState["quotationlineid"] = e.CommandArgument.ToString();
        //bindQuotationLine();
    }

    protected void gvAdditionalDiscount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int nCurrentRow = e.Item.ItemIndex;
            RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");
            RadLabel lblOrderTaxCode = (RadLabel)e.Item.FindControl("lblOrderTaxCode");

            UpdateTaxAdditionalDiscount(lblOrderId.Text
                                        , lblOrderTaxCode.Text.Trim()
                                        , ((UserControlDecimal)e.Item.FindControl("ucAdditionalAmountEdit")).Text.ToString().Trim()
                                        , ((UserControlDecimal)e.Item.FindControl("ucVesselAmountEdit")).Text.ToString().Trim()
                                        , ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text
                                        , ((RadTextBox)(e.Item.FindControl("txtOwnerBudgetIdEdit"))).Text
                                        , ((UserControls_UserControlProjectCode)(e.Item.FindControl("ucProjectcodeEdit"))).SelectedProjectCode
                                        );
            gvAdditionalDiscount.Rebind();
            if (txtAprooverStatus.Text == "Approved")
            {
                UpdateApprovalStatus(ViewState["ORDERID"].ToString());
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Approve", "APPROVE");
                MenuStaging.AccessRights = this.ViewState;
                MenuStaging.MenuList = toolbar.Show();
                MenuStaging.Visible = true;
                tbldiv.Visible = true;
            }
            AddDisRebind();
            TaxRebind();
            BindVendorInfo();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            TaxRebind();
        }
        TaxRebind();
    }

    protected void gvAdditionalDiscount_ItemDataBound(Object sender, GridItemEventArgs e)
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

        if (e.Item is GridDataItem)
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
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }

            // if (lblAdditionalDiscountYN != null && (lblAdditionalDiscountYN.Text == "0" || lblAdditionalDiscountYN.Text == "2"))
            // {
            //     //GridViewRow gvr = e.Row;
            //     //gvr.Visible = false;
            //    // e.Item.Visible = false;
            //    
            // }

            RadLabel lblAdditionalDiscountYN = (RadLabel)e.Item.FindControl("lblAdditionalDiscountYN");
            ViewState["ADDITIONALDISCOUNT"] = lblAdditionalDiscountYN.Text;

            GridDataItem item = (GridDataItem)e.Item;
            if (ViewState["ADDITIONALDISCOUNT"] != null && (ViewState["ADDITIONALDISCOUNT"].ToString() == "0" || ViewState["ADDITIONALDISCOUNT"].ToString() == "2"))//set your condition for hiding the row
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
            RadLabel lblBudgetId = (RadLabel)e.Item.FindControl("lblBudgetId");
            RadLabel lblPOBudgetId = (RadLabel)e.Item.FindControl("lblPOBudgetId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ibtnShowOwnerBudgetEdit != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&Ownerid=" + ViewState["Ownerid"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + lblBudgetId.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
            if (ProjectCode != null)
            {
                int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(VesselAccountId, BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }
        }

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
    protected void InsertOrderTaxStaging(string orderid, string description, string newbudgetcode, decimal amount, Guid? newOwnerBudgetId, ref int status,int? Projectcode)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderTaxStagingInsert(new Guid(orderid), description, newbudgetcode, amount, PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.UserCode, newOwnerBudgetId, ref status,Projectcode);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateTaxAdditionalDiscount(string orderid, string taxmapcode, string additionaldiscount, string vesseladditionaldiscount, string budgetcodeid, string newownerbudgetid,string Projectid)
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
                                                                            , General.GetNullableInteger(Projectid)
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
        BindVendorInfo();
        BindData();
        BindDataTax();
        BindAdditionalDiscount();

        TaxRebind();
        Rebind();
        AddDisRebind();

    }

    protected void VesselUpdate(object sender, ImageClickEventArgs arg)
    {
        if (ViewState["ORDERID"] != null)
        {
            //if (txtVesselId.Text != string.Empty)
            //{
            try
            {
                PhoenixAccountsPOStaging.InvoiceReconciliationStagingVesselUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(txtVesselId.Text));
                Response.Redirect("AccountsReconcilationStagingRWAForPurchase.aspx?ORDERID=" + ViewState["ORDERID"].ToString() + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString());
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            //}
        }
    }
    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ucConfirmSent.confirmboxvalue == 1)
            {
                if (ViewState["ORDERID"] != null)
                {
                    if (ViewState["FORMTYPE"].ToString() == "3")
                    {
                        PhoenixAccountsPOStaging.DirectPOInvoiceStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    }
                    else
                        PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    Rebind();
                    TaxRebind();
                    BindVendorInfo();

                    if (MenuStaging.Visible == true)
                    {
                        MenuStaging.Visible = false;
                        //   tbldiv.Visible = false;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);



                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RefreshBudgetCode(object sender, ImageClickEventArgs arg)
    {
        if (ViewState["ORDERID"] != null)
        {
            try
            {
                PhoenixAccountsPOStaging.OrderFormStagingBudgetCodeUpdateBulk(new Guid(ViewState["ORDERID"].ToString()), General.GetNullableInteger(txtBulkBudgetId.Text), General.GetNullableGuid(txtBulkOwnerBudgetId.Text),
                    General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                    );

                ucStatus.Text = "Budget Codes updated Successfully.";

                BindData();
                BindDataTax();
                BindAdditionalDiscount();
                BindVendorInfo();

                String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);



            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    protected void VesselAccountUpdate(object sender, ImageClickEventArgs arg)
    {
        try
        {
            if (ViewState["ORDERID"] != null)
            {
                if (ucVesselAccount.Text != string.Empty)
                {
                    PhoenixAccountsPOStaging.OrderFormStagingVesselAccountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(ViewState["ORDERID"].ToString())
                                                                                    , int.Parse(ucVesselAccount.SelectedValue));
                    ucStatus.Text = "Vessel account updated Successfully.";
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
            }
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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAdditionalDiscount.CurrentPageIndex + 1;
            BindAdditionalDiscount();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["FORMTYPE"].ToString() == "3")
            {
                PhoenixAccountsPOStaging.DirectPOInvoiceStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            }
            else
                PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            Rebind();
            TaxRebind();
            BindVendorInfo();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            int? VesselAccountId = General.GetNullableInteger(ViewState["VESSELACCOUNT"].ToString());

            if (btnShowBulkBudget != null)
            {
                int? bulkBudget = General.GetNullableInteger(txtBulkBudgetId.Text);
                ucProjectcode.bind(VesselAccountId, bulkBudget);
            }

           
            GridFooterItem footerItem = (GridFooterItem)gvTax.MasterTableView.GetItems(GridItemType.Footer)[0];
            ImageButton btbudget = (ImageButton)footerItem.FindControl("btnShowBudget");
            UserControls_UserControlProjectCode ProjectCodeAdd = (UserControls_UserControlProjectCode)footerItem.FindControl("ucProjectcodeAdd");
            int? Budget = General.GetNullableInteger(((RadTextBox)footerItem.FindControl("txtBudgetIdEdit")).Text);

            if (btbudget != null)
                ProjectCodeAdd.bind(VesselAccountId, Budget);

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void txtBudgetIdEdit_TextChanged(object sender, EventArgs e)
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
}

