using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Purchase;

public partial class AccountsInvoiceBySupplierLineItemDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLineItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvLineItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    public decimal InvoicePayableAmount = 0;
    public string TotalPOPayableAmount = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice", "INVOICE");
            toolbarmain.AddButton("PO", "PO");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");


            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            MenuLineItem.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ucConfirm.Visible = false;


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                    ViewState["invoicelineitemcode"] = Request.QueryString["qvoucherlineitemcode"];
                if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
                    ViewState["callfrom"] = Request.QueryString["qfrom"];

                DataSet DsActualInvoiceCode = PhoenixAccountsInvoice.InvoiceBySupplierEdit(new Guid(Request.QueryString["qinvoicecode"].ToString()));
                ViewState["invoicecode"] = DsActualInvoiceCode.Tables[0].Rows[0]["FLDACTUALINVOICECODE"];

                ViewState["qinvoicecode"] = Request.QueryString["qinvoicecode"];

                Title1.Text = "Items    (  " + PhoenixAccountsVoucher.InvoiceNumber + "     )";
                InvoiceEdit();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() != "AVN")
                toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsInvoiceLineItem.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
            //toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx", "Post Voucher", "pr.png", "CREATEPR");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
            MenuOrderLineItem.SetTrigger(pnlStockItemEntry);

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InvoiceEdit()
    {
        if (ViewState["invoicecode"] != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                ViewState["SHORTNAME"] = drInvoice["FLDSHORTNAME"].ToString();
            }
        }
    }
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PO"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx?qinvoicecode=" + ViewState["qinvoicecode"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceBySupplierMaster.aspx?qinvoicecode=" + ViewState["qinvoicecode"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceBySupplierAttachments.aspx?qinvoicecode=" + ViewState["qinvoicecode"].ToString() + "&qfrom=" + ViewState["callfrom"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("CREATEPR"))
            {
                ReconcileInvoiceWithPurchaseOrder(ViewState["invoicecode"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ReconcileInvoiceWithPurchaseOrder(string strInvoiceCode)
    {
        try
        {
            Response.Redirect("../Accounts/AccountsInvoiceLedgerPostingConfirmation.aspx?qinvoicecode=" + strInvoiceCode);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        if (ViewState["callfrom"].ToString() == "invoice" || ViewState["callfrom"].ToString() == "invoiceforpurchase")
        {
            string[] alCaptions = { 
                                "PO Number",                               
                                "Vessel Name",
                                "PO Currency",
                                "PO Additional Charges",
                                "PO Payable Amount",
                                "Invoice Payable Amount",
                                "Receipt Last Updated By"                                               
                              };

            string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNT",                                
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
        else if (ViewState["callfrom"].ToString() == "AD")
        {
            string[] alCaptions = { 
                                    "PO Number",                               
                                    "Vessel Name",
                                    "PO Currency",
                                    "PO Committed Amount",
                                    "PO Short Delivery Amount",
                                    "PO Additional Charges",
                                    "PO Payable Amount",                              
                                    "Advance Payment",
                                    "Invoice Payable Amount",
                                    "Receipt Last Updated By"
                              };

            string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDCOMMITTEDAMOUNT",
                                "FLDSHORTDELIVERYAMOUNT",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",                             
                                "FLDPAYMENT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
        else if (ViewState["callfrom"].ToString() == "PR")
        {
            string[] alCaptions = { 
                                "PO Number",                               
                                "Vessel Name",
                                "PO Currency",	
                                "PO Payable Amount.",
 	                            "Advance Payment",
                                "GST On PO Payable", 	
                                "GST On Discount", 	
                                "Total Discount Amount.", 	
                                "Vessel Allocated Discount ",
                                "Service Charge", 	
                                "Invoice Payable Amount.",
                                "Receipt Last Updated By"
                                //"PO Payable Amount",                               
                                //"Total Payable Amount"                                                      
                              };

            string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDPAYMENT",
                                "FLDGSTAMOUNT",
                                "FLDGSTDISCOUNTAMOUNT",
                                "FLDORDERFORMTOTALDISCOUNTAMOUNT",
                                "FLDVESSELALLOCATEDDISCOUNTAMOUNT",
                                "FLDSERVICECHARGE",                             
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
        if (ViewState["invoicecode"] != null)
        {
            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                           new Guid(ViewState["invoicecode"].ToString())
                                                           , null
                                                           , string.Empty
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                       );


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds;
                gvLineItem.DataBind();

                TotalPOPayableAmount = string.Format(String.Format("{0:#####.00}", ds.Tables[0].Rows[0]["FLDTOTALPURCHASEPAYABLEAMOUNT"]));
                if (ViewState["invoicelineitemcode"] == null)
                {
                    ViewState["invoicelineitemcode"] = ds.Tables[0].Rows[0]["FLDINVOICELINEITEMCODE"].ToString();
                    gvLineItem.SelectedIndex = 0;
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/AccountsInvoiceLineItem.aspx?qinvoicecode=";
                }
                {
                    if (ViewState["invoicelineitemcode"] != null)
                    {
                        string strRowno = string.Empty;
                    }
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvLineItem);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ViewState["callfrom"].ToString() == "invoice" || ViewState["callfrom"].ToString() == "invoiceforpurchase")
            {
                string[] alCaptions = { 
                                "PO Number",                               
                                "Vessel Name",
                                "PO Currency",
                                "PO Additional Charges",
                                "PO Payable Amount",
                                "Invoice Payable Amount",
                                "Receipt Last Updated By"                                                                              
                              };

                string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNT",                                
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);
            }
            else if (ViewState["callfrom"].ToString() == "AD")
            {
                string[] alCaptions = { 
                                    "PO Number",                               
                                    "Vessel Name",
                                    "PO Currency",
                                    "PO Committed Amount",
                                    "PO Short Delivery Amount",
                                    "PO Additional Charges",
                                    "PO Payable Amount", 
                                    "Advance Payment",
                                    "Invoice Payable Amount",
                                    "Receipt Last Updated By"                  
                              };

                string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDCOMMITTEDAMOUNT",
                                "FLDSHORTDELIVERYAMOUNT",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",
                                "FLDPAYMENT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);
            }
            else if (ViewState["callfrom"].ToString() == "PR")
            {
                string[] alCaptions = { 
                                "PO Number",                               
                                "Vessel Name",
                                "PO Currency",	
                                "PO Payable Amount.",
 	                            "Advance Payment",
                                "GST On PO Payable", 	
                                "GST On Discount", 	
                                "Total Discount Amount.", 	
                                "Vessel Allocated Discount ",
                                "Service Charge", 	
                                "Invoice Payable Amount.",
                                "Receipt Last Updated By"
                                //"PO Payable Amount",                               
                                //"Total Payable Amount"                                                      
                              };

                string[] alColumns = { 
                                "FLDPURCHASEORDERNUMBER",                                
                                "FLDVESSELNAME", 
                                "FLDPOCURRENCYCODE",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",
                                "FLDPAYMENT",
                                "FLDGSTAMOUNT",
                                "FLDGSTDISCOUNTAMOUNT",
                                "FLDORDERFORMTOTALDISCOUNTAMOUNT",
                                "FLDVESSELALLOCATEDDISCOUNTAMOUNT",
                                "FLDSERVICECHARGE",                             
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY" };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);

            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');
        int nCurrentRow = Int32.Parse(strValues[0]);
        try
        {
            if (e.CommandName.ToUpper().Equals("OFFSET"))
            {
                return;
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                try
                {
                    PhoenixAccountsInvoice.InvoiceLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInvoiceLineItemCode")).Text.ToString()));
                    PhoenixAccountsPOStaging.OrderFormStagingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text));
                    _gridView.EditIndex = -1;
                    BindData();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("PORECONSILATIONSTAGING"))
            {
                Filter.CurrentPurchaseStockType = ((Label)gvLineItem.Rows[nCurrentRow].FindControl("lblStockType")).Text;
                string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
                Response.Redirect("AccountsReconcilationStaging.aspx?ORDERID=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("POPOSTINVOICESTAGING"))
            {
                Filter.CurrentPurchaseStockType = ((Label)gvLineItem.Rows[nCurrentRow].FindControl("lblStockType")).Text;
                string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
                Response.Redirect("AccountsPostInvoiceStaging.aspx?ORDERID=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=PR");
            }
            if (e.CommandName.ToUpper().Equals("ORDER"))
            {
                Filter.CurrentPurchaseStockType = ((Label)gvLineItem.Rows[nCurrentRow].FindControl("lblStockType")).Text;
                string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
                if (Filter.CurrentPurchaseStockType == "MEDICAL")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=MEDICALSLIP&emailyn=1&medicalinvoiceyn=1&reqid=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"] + "&showactual=0", false);
                }
                else if (Filter.CurrentPurchaseStockType == "TRAVEL")
                {
                    return;
                }
                else
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=ACCOUNTSORDERFORM&quotationid=&orderid=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"] + "&showactual=0");
                }
            }
            if (e.CommandName.ToUpper().Equals("POAPPROVE"))
            {
                string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
                UpdateApprovalStatus(strOrderId);
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("POREFRESH"))
            {
                string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
                Label lblPurchaseOrderNumber = (Label)_gridView.Rows[nCurrentRow].FindControl("lnkPurchaseOrderNumber");
                ViewState["orderid"] = strOrderId;
                ucConfirm.Visible = true;
                ucConfirm.Text = lblPurchaseOrderNumber.Text + " - " + "All the Reconciliation changes will be reset when the PO gets refreshed. Do you want to continue.?";
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void UpdateInvoicePayable(Guid gInvoiceLineItemCode, decimal dInvoicePayableAmount)
    //{
    //  PhoenixAccountsInvoice.InvoiceLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, gInvoiceLineItemCode, dInvoicePayableAmount);
    //}

    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    public decimal POPayableAmount = 0.00m;
    protected void gvLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdOrder = (ImageButton)e.Row.FindControl("cmdOrder");
            if (cmdOrder != null)
            {
                cmdOrder.Visible = SessionUtil.CanAccess(this.ViewState, cmdOrder.CommandName);
                if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                    cmdOrder.Visible = false;
            }

            ImageButton cmdReceipt = (ImageButton)e.Row.FindControl("cmdReceipt");
            if (cmdReceipt != null)
            {
                cmdReceipt.Visible = SessionUtil.CanAccess(this.ViewState, cmdReceipt.CommandName);
                if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                    cmdReceipt.Visible = false;
            }

            Label lblOrdertype = (Label)e.Row.FindControl("lblOrdertype");
            if (lblOrdertype != null && lblOrdertype.Text != "1")
            {
                cmdOrder.Visible = false;
                cmdReceipt.Visible = false;
            }
            Label lblPOReceived = (Label)e.Row.FindControl("lblPOReceived");
            if (lblOrdertype != null && lblOrdertype.Text == "1")
            {
                if (lblPOReceived != null && lblPOReceived.Text == "1")
                    cmdReceipt.Visible = true;
                else
                    cmdReceipt.Visible = false;
            }
            TextBox txtOrderId = (TextBox)e.Row.FindControl("txtOrderId");
            if (cmdReceipt != null && txtOrderId != null)
            {
                cmdReceipt.Attributes.Add("onclick", "Openpopup('VesselReceipt', '', '../Reports/ReportsView.aspx?applicationcode=3&reportcode=VESSELRECEIPT&orderid=" + txtOrderId.Text + "&showmenu=0&showword=no&showexcel=no');return false;");
            }

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                    cmdDelete.Visible = false;
            }

            ImageButton cmdPOPostInvoiceStaging = (ImageButton)e.Row.FindControl("cmdPOPostInvoiceStaging");
            if (cmdPOPostInvoiceStaging != null)
                cmdPOPostInvoiceStaging.Visible = SessionUtil.CanAccess(this.ViewState, cmdPOPostInvoiceStaging.CommandName);

            ImageButton cmdRefresh = (ImageButton)e.Row.FindControl("cmdRefresh");
            if (cmdRefresh != null)
                if (lblOrdertype != null && (lblOrdertype.Text == "2" || lblOrdertype.Text == "3"))// for the bugid 9132 refresh is not for direct po and amos po
                    cmdRefresh.Visible = false;
                else
                    cmdRefresh.Visible = SessionUtil.CanAccess(this.ViewState, cmdRefresh.CommandName);

            //ImageButton cmdPOApprove = (ImageButton)e.Row.FindControl("cmdPOApprove");
            //if (cmdPOApprove != null)
            //    cmdPOApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName);
        }

        if (ViewState["callfrom"].ToString() == "invoice")
        {
            gvLineItem.Columns[0].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = true;
            gvLineItem.Columns[8].Visible = true;
            gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
            TextBox txtInvPayableAmountEdit = (TextBox)e.Row.FindControl("txtInvPayableAmountEdit");
            if (txtInvPayableAmountEdit != null) txtInvPayableAmountEdit.ReadOnly = true;
        }
        else if (ViewState["callfrom"].ToString() == "invoiceforpurchase")
        {
            gvLineItem.Columns[0].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = true;
            gvLineItem.Columns[8].Visible = true;
            gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
            TextBox txtInvPayableAmountEdit = (TextBox)e.Row.FindControl("txtInvPayableAmountEdit");
            if (txtInvPayableAmountEdit != null) txtInvPayableAmountEdit.ReadOnly = true;
        }

        else if (ViewState["callfrom"].ToString() == "AD")
        {
            gvLineItem.Columns[0].Visible = false;
            //gvLineItem.Columns[8].Visible = false;
            //gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
        }
        else if (ViewState["callfrom"].ToString() == "PR")
        {
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            POPayableAmount = 0.00m;
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            ImageButton db1 = (ImageButton)e.Row.FindControl("cmdPOReconsilationStaging");
            ImageButton db2 = (ImageButton)e.Row.FindControl("cmdPOPostInvoiceStaging");
            Label lbl = (Label)e.Row.FindControl("lblApproval");
            ImageButton db3 = (ImageButton)e.Row.FindControl("cmdApproved");
            ImageButton db4 = (ImageButton)e.Row.FindControl("cmdAwaitingForApproval");
            //ImageButton db5 = (ImageButton)e.Row.FindControl("cmdPOApprove");
            //db5.Attributes.Add("onclick", "parent.Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDINVOICELINEITEMCODE"].ToString() + "&mod=PURCHASE"
            //        + "&type=" + drv["FLDINVOICEAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString()
            //        + "&v=invoice&vslid=" + drv["FLDVESSELID"].ToString() + "');return false;");
            Label lblPOPayableAmount = (Label)e.Row.FindControl("lblPOPayableAmount");
            if (lblPOPayableAmount != null && lblPOPayableAmount.Text != "")
            {
                POPayableAmount = POPayableAmount + Convert.ToDecimal(lblPOPayableAmount.Text);
                String.Format("{0:0.00}", POPayableAmount);
            }
            if (db1 != null && ViewState["callfrom"].ToString() == "invoice" && (db2 != null))
            {
                // db1.Attributes.Add("style", "visibility:hidden");
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db1 != null && ViewState["callfrom"].ToString() == "invoiceforpurchase" && (db2 != null))
            {
                // db1.Attributes.Add("style", "visibility:hidden");
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db2 != null && ViewState["callfrom"].ToString() == "AD")
            {
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db1 != null && ViewState["callfrom"].ToString() == "PR")
            {
                db1.Attributes.Add("style", "visibility:hidden");
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsInvoiceLineItem.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
                toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx", "Post Voucher", "pr.png", "CREATEPR");

                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
                MenuOrderLineItem.SetTrigger(pnlStockItemEntry);
            }
            if (lbl != null && lbl.Text != "" && db3 != null && db4 != null)
            {
                int i = Convert.ToInt32(lbl.Text);
                if (i == 0)
                {
                    db3.Attributes.Add("style", "visibility:hidden");
                    //db5.Enabled = true;
                    //db5.Visible = true;
                }
                else if (i == 1)
                    db4.Attributes.Add("style", "visibility:hidden");
                else
                {
                    db3.Attributes.Add("style", "visibility:hidden");
                    db4.Attributes.Add("style", "visibility:hidden");
                }
            }
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkAdvanceAmount");
            Label lblPayment = (Label)e.Row.FindControl("lblAdvPayment");
            if (lb != null && lb.Text != "" && lblPayment != null)
            {
                if (Convert.ToDouble(lb.Text) == 0.00)
                {
                    lb.Visible = false;
                    lblPayment.Visible = true;
                }
            }
            //if (db5 != null && ViewState["callfrom"].ToString() == "PR")
            //{
            //    db5.Attributes.Add("style", "visibility:hidden");
            //}
            //if (db5 != null && ViewState["callfrom"].ToString() == "invoice")
            //{
            //    db5.Attributes.Add("style", "visibility:hidden");
            //}

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOffset = (Label)e.Row.FindControl("lblOffset");
            ImageButton db = (ImageButton)e.Row.FindControl("cmdOffset");

            Label lblInvoiceCurrencyId = (Label)e.Row.FindControl("lblInvoiceCurrencyId");
            Label lblPOCurrencyId = (Label)e.Row.FindControl("lblPOCurrencyId");
            Label lblPOCurrency = (Label)e.Row.FindControl("lblPOCurrency");
            Label lblPurPayableAmount = (Label)e.Row.FindControl("lblPurPayableAmount");
            Label lblInvoicePayableAmount = (Label)e.Row.FindControl("lblInvoicePayableAmount");

            if (lblInvoicePayableAmount.Text != string.Empty)
            {
                InvoicePayableAmount = decimal.Parse(lblInvoicePayableAmount.Text);
            }

            if (lblInvoiceCurrencyId.Text != lblPOCurrencyId.Text)
            {
                lblPOCurrency.ForeColor = System.Drawing.Color.Red;
            }

            if (lblOffset.Text != "1")
            {
                db.Visible = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = ((LinkButton)e.Row.FindControl("lnkAdvanceAmount"));

            TextBox txtOrderId = ((TextBox)e.Row.FindControl("txtOrderId"));
            Label lbl = ((Label)e.Row.FindControl("lnkPurchaseOrderNumber"));
            ViewState["ponumber"] = lbl.Text;

            Label lblVesselName = ((Label)e.Row.FindControl("lblVesselName"));
            Label lblStockType = ((Label)e.Row.FindControl("lblStockType"));
            Label lblVesselId = ((Label)e.Row.FindControl("lblVesselId"));

            if (General.GetNullableInteger(lblVesselId.Text) != null)
            {
                NameValueCollection criteria = new NameValueCollection();

                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(lblVesselId.Text);
                Filter.CurrentPurchaseVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = lblVesselName.Text;

                criteria.Clear();
                criteria.Add("ucVessel", lblVesselId.Text);
                criteria.Add("ddlStockType", lblStockType.Text);
                criteria.Add("txtNumber", lbl.Text);
                criteria.Add("txtTitle", "");
                criteria.Add("txtVendorid", "");
                criteria.Add("txtDeliveryLocationId", "");
                criteria.Add("txtBudgetId", "");
                criteria.Add("txtBudgetgroupId", "");
                criteria.Add("ucFinacialYear", "");
                criteria.Add("ucFormState", "");
                criteria.Add("ucApproval", "");
                criteria.Add("UCrecieptCondition", "");
                criteria.Add("UCPeority", "");
                criteria.Add("ucFormStatus", "");
                criteria.Add("ucFormType", "");
                criteria.Add("ucComponentclass", "");
                criteria.Add("txtMakerReference", "");
                criteria.Add("txtOrderedDate", "");
                criteria.Add("txtOrderedToDate", "");
                criteria.Add("txtCreatedDate", "");
                criteria.Add("txtCreatedToDate", "");
                criteria.Add("txtApprovedDate", "");
                criteria.Add("txtApprovedToDate", "");
                Filter.CurrentOrderFormFilterCriteria = criteria;

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);

                ImageButton cmdViewPOForm = (ImageButton)e.Row.FindControl("cmdViewPOForm");
                if (cmdViewPOForm != null)
                {
                    cmdViewPOForm.Visible = SessionUtil.CanAccess(this.ViewState, cmdViewPOForm.CommandName);
                    if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                        cmdViewPOForm.Visible = false;

                    cmdViewPOForm.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Purchase/PurchaseForm.aspx?launchedfrom=ACCOUNTS'); return false;");

                    if (drv["FLDORDERFORMTYPE"].ToString() == "1")
                    {
                        cmdViewPOForm.Visible = true;
                    }
                    else
                        cmdViewPOForm.Visible = false;
                }

                ImageButton cmdPurAttachment = (ImageButton)e.Row.FindControl("cmdPurAttachment");
                if (cmdPurAttachment != null)
                {
                    cmdPurAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdPurAttachment.CommandName);
                    if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                        cmdPurAttachment.Visible = false;

                    cmdPurAttachment.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Purchase/PurchaseAttachments.aspx?orderid=" + txtOrderId.Text + "&copydtkey=" + drv["FLDDTKEY"].ToString() + "&launchedfrom=ACCOUNTS&MOD=PURCHASE'); return false;");

                    if (drv["FLDORDERFORMTYPE"].ToString() == "1")
                    {
                        cmdPurAttachment.Visible = true;
                    }
                    else
                        cmdPurAttachment.Visible = false;
                }
            }

            lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsAdvancePaymentDetails.aspx?orderid=" + txtOrderId.Text + "&ponumber=" + lbl.Text + "');return false;");

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    private void UpdateApprovalStatus(string orderid)
    {
        PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(orderid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }
    private void PORefresh(string orderid)
    {
        PhoenixAccountsInvoice.InvoicePORefresh(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid));
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["orderid"] != null)
                    PORefresh(ViewState["orderid"].ToString());
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
