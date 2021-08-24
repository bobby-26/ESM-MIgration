using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class AccountsInvoiceLedgerPostingConfirmation : PhoenixBasePage
{
    public string strCreditorAccountDetails;
    public string strSupplierCode;
    public string strRebateAccountDetails;
    public string strIncomeAccountDetails;
    public string strGSTClaimAccountDetails;
    public string strInvoicePayableAmount = "0.00";
    public string strPayableAmount = "0.00";
    public string strCreditorsdiscountAmount = "0.00";
    public string strGstAmount = "0.00";
    public string strGstClaim;
    public string strGSTonDiscount;
    public string strGSTForVessel;
    public string strRebateReceivableAmount = "0.00";
    public string strDiscountrebateincome;
    public string strTotalIncomeAmount = "0.00";
    public string strTDSPayable = "0.00";
    public string strServiceTaxPayable = "0.00";
    public string strWCTPayable = "0.00";
    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;
    public double basedebittotal = 0.00;
    public double basecredittotal = 0.00;
    public double reportdebittotal = 0.00;
    public double reportcredittotal = 0.00;
    public string strBaseDebitTotal = string.Empty;
    public string strBaseCrebitTotal = string.Empty;
    public string strReportDebitTotal = string.Empty;
    public string strReportCrebitTotal = string.Empty;
    public decimal dPayableamount;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Post", "POST", ToolBarDirection.Right);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            MenuLineItem.Title = "MenuLineItem";
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                txtInvoicePostdate.Text = General.GetDateTimeToString(DateTime.Now.ToString());

                if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
                gvOrderLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx");
            }
            else if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidPost())
                {
                    ucError.Visible = true;
                    MenuLineItem.SelectedMenuIndex = 1;
                    return;
                }
                PhoenixAccountsInvoice.PostInvoiceWithBaseAndReportCurrencyExchangerate(new Guid(ViewState["invoicecode"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, DateTime.Parse(txtInvoicePostdate.Text), decimal.Parse(txtBaseexchangerate.Text), decimal.Parse(txtReportexchangerate.Text));
                //PhoenixAccountsInvoice.PostInvoice(new Guid(ViewState["invoicecode"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode,DateTime.Parse(txtInvoicePostdate.Text));
                ucStatus.Text = "Invoice Posted Successfully.";
                InvoiceEdit();
                RebindOrder();
                Rebind();

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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            string[] alColumns = {"FLDNUMBER", "FLDNAME","FLDUNITNAME","FLDCURRENCY","FLDVENDORNOTES",
                                 "FLDQUANTITY","FLDQUOTEDPRICE","FLDDISCOUNT","FLDDELIVERYTIME" };
            string[] alCaptions = {"Partnumber", "Item Name","Unit","Currency","Vendor Notes",
                                 "Quantity","Price","Discount", "Delivery Time" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            //int? sortdirection = null;
            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                DataSet ds = PhoenixAccountsPOStaging.InvoiceStagingSearch(new Guid(ViewState["invoicecode"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drInvoice = ds.Tables[0].Rows[0];
                    strCreditorAccountDetails = drInvoice["FLDCREDITORSACCOUNTDETAILS"].ToString();
                    strSupplierCode = drInvoice["FLDSUPPLIERCODE"].ToString();
                    strRebateAccountDetails = drInvoice["FLDREBATEACCOUNTDETAILS"].ToString();
                    strIncomeAccountDetails = drInvoice["FLDINCOMEACCOUNTDETAILS"].ToString();
                    strGSTClaimAccountDetails = drInvoice["FLDGSTCLAIMACCOUNTDETAILS"].ToString();

                    strTDSPayable = drInvoice["FLDTDSPAYABLE"].ToString();
                    strServiceTaxPayable = drInvoice["FLDSERVICETAXPAYABLE"].ToString();
                    strWCTPayable = drInvoice["FLDWCTPAYABLE"].ToString();

                    if (decimal.Parse(drInvoice["FLDINVOICEPAYABLEAMOUNT"].ToString()) > 0)
                    {
                        strInvoicePayableAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEPAYABLEAMOUNT"]));
                    }
                    else
                    {
                        strInvoicePayableAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDINVOICEPAYABLEAMOUNT"]));
                    }


                    strPayableAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDPAYABLEAMOUNT"]));
                    dPayableamount = decimal.Parse(drInvoice["FLDPAYABLEAMOUNT"].ToString());


                    if (decimal.Parse(drInvoice["FLDCREDITORSDISCOUNTAMOUNT"].ToString()) > 0)
                    {
                        strCreditorsdiscountAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDCREDITORSDISCOUNTAMOUNT"]));
                    }
                    else
                    {
                        strCreditorsdiscountAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDCREDITORSDISCOUNTAMOUNT"]));
                    }
                    if (decimal.Parse(drInvoice["FLDGSTONDISCOUNT"].ToString()) > 0)
                    {
                        strGSTonDiscount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTONDISCOUNT"]));
                    }
                    else
                    {
                        strGSTonDiscount = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTONDISCOUNT"]));
                    }
                    if (decimal.Parse(drInvoice["FLDGSTCLAIM"].ToString()) > 0)
                    {
                        strGSTForVessel = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTFORVESSEL"]));
                        strTotalIncomeAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTFORVESSEL"]));
                    }
                    else
                    {
                        strGSTForVessel = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTFORVESSEL"]));
                    }

                    if (decimal.Parse(drInvoice["FLDREBATERECEIVABLEAMOUNT"].ToString()) > 0)
                    {
                        strRebateReceivableAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDREBATERECEIVABLEAMOUNT"]));
                    }
                    else
                    {
                        strRebateReceivableAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDREBATERECEIVABLEAMOUNT"]));
                    }

                    if (decimal.Parse(drInvoice["FLDGSTAMOUNT"].ToString()) > 0)
                    {
                        strGstAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTCLAIM"]));
                    }
                    else
                    {
                        strGstAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTCLAIM"]));
                    }

                    if (decimal.Parse(drInvoice["FLDGSTCLAIM"].ToString()) > 0)
                    {
                        strGstClaim = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTCLAIM"]));
                    }
                    else
                    {
                        strGstClaim = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTCLAIM"]));
                    }

                    if (decimal.Parse(drInvoice["FLDDISCOUNTREBATEINCOME"].ToString()) > 0)
                    {
                        strDiscountrebateincome = string.Format(String.Format("{0:#####.00}", drInvoice["FLDDISCOUNTREBATEINCOME"]));
                        strTotalIncomeAmount = string.Format(String.Format("{0:#####.00}", (decimal.Parse(strTotalIncomeAmount) + decimal.Parse(drInvoice["FLDDISCOUNTREBATEINCOME"].ToString()))));
                    }
                    else
                    {
                        strDiscountrebateincome = string.Format(String.Format("{0:0.00}", drInvoice["FLDDISCOUNTREBATEINCOME"]));
                    }
                }

                else
                {

                }
                gvOrderLine.DataSource = ds;
                gvOrderLine.VirtualItemCount = iRowCount;
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                General.SetPrintOptions("gvOrderLine", "Vendor Item List", alCaptions, alColumns, ds);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void InvoiceEdit()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet dsAttachment = new DataSet();
        if (ViewState["invoicecode"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtPurchaseInvoiceVoucherNumber.Text = drInvoice["FLDPURCHASEINVOICEVOUCHERNUMBER"].ToString();
                txtBaseexchangerate.Text = drInvoice["FLDBASEEXCHANGERATE"].ToString();
                txtReportexchangerate.Text = drInvoice["FLDREPORTEXCHANGERATE"].ToString();
                txtCurrency.Text = drInvoice["FLDCURRENCYCODE"].ToString();
                if (General.GetNullableInteger(drInvoice["FLDVOUCHERID"].ToString()) != null)
                    ViewState["VOUCHERID"] = drInvoice["FLDVOUCHERID"].ToString();
                if (General.GetNullableInteger(drInvoice["FLDINVOICESTATUS"].ToString()) != null)
                    ViewState["INVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"].ToString();
                ViewState["PURCHASEINVOICEVOUCHERNUMBER"] = drInvoice["FLDPURCHASEINVOICEVOUCHERNUMBER"].ToString();

                string dtKey = drInvoice["FLDDTKEY"].ToString();
                if (drInvoice["FLDVENDORINVOICENUMBERALREADYEXISTS"].ToString() == "1")
                {
                    HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsVendorInvoiceDuplicateList.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
                    HlinkRefDuplicate.Visible = true;
                }
                dsAttachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtKey), null, "invoice", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                if (dsAttachment != null && dsAttachment.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISATTACHMENTAVAILABLE"] = 1;

                }
                else
                {
                    ViewState["ISATTACHMENTAVAILABLE"] = 0;
                }
            }
        }
    }

    private void BindLineitemData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        string voucherid = (ViewState["VOUCHERID"] == null) ? null : (ViewState["VOUCHERID"].ToString());
        if (ViewState["VOUCHERID"] != null && General.GetNullableInteger(ViewState["VOUCHERID"].ToString()) != null)
        {
            divTable.Visible = false;
            gvOrderLine.Visible = false;
            txtCommondescription.Visible = true;
            lblCommondescription.Visible = true;
            Imgrefresh.Visible = true;
            // tblpager.Visible = true;
        }

        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["VOUCHERID"] != null && General.GetNullableInteger(ViewState["VOUCHERID"].ToString()) != null)
        {
            ds = PhoenixAccountsVoucher.VoucherLineItemSearch(
                                                                     int.Parse(voucherid)
                                                                    , null
                                                                    , null
                                                                    , string.Empty
                                                                    , string.Empty
                                                                    , null
                                                                    , null
                                                                    , sortdirection
                                                                    , sortexpression
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvOrderLine.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , ref TransactionAmountTotal
                                                                    , ref BaseAmountTotal
                                                                    , ref ReportAmountTotal
                                                               );



            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                DataTable dt1 = ds.Tables[0];
                foreach (DataRow row in dt1.Rows)
                {
                    row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
                    row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
                    row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
                }
            }
            else
            {
                //DataTable dt = ds.Tables[0];
                //ShowNoRecordsFound(dt, gvLineItem);
                TransactionAmountTotal = 0;
                BaseAmountTotal = 0;
                ReportAmountTotal = 0;
            }
            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDOWNERACCOUNT","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
            string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Owners Budget Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
            General.SetPrintOptions("gvLineItem", "Voucher Line Item", alCaptions, alColumns, ds);

            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);

        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? VoucherLineId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text);
                string Description = (((RadTextBox)e.Item.FindControl("txtDescription")).Text);
                PhoenixAccountsVoucher.InvoiceDraftPostUpdate(null
                                                            , (((RadTextBox)e.Item.FindControl("txtDescription")).Text)
                                                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text));

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvLineItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            basedebittotal = 0.00;
            basecredittotal = 0.00;
            reportdebittotal = 0.00;
            reportcredittotal = 0.00;
        }
        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            RadLabel lblBaseAmount = (RadLabel)e.Item.FindControl("lblBaseAmount");
            RadLabel lblReportAmount = (RadLabel)e.Item.FindControl("lblReportAmount");
            if (lblBaseAmount != null && lblBaseAmount.Text != "")
            {
                if (Convert.ToDouble(lblBaseAmount.Text) <= 0)
                {
                    basecredittotal = basecredittotal + Convert.ToDouble(lblBaseAmount.Text);
                    strBaseCrebitTotal = String.Format("{0:n}", basecredittotal);
                    ViewState["basecredittotal"] = basecredittotal;
                }
                else
                {
                    basedebittotal = Math.Abs(basedebittotal) + Math.Abs(Convert.ToDouble(lblBaseAmount.Text));
                    strBaseDebitTotal = String.Format("{0:n}", basedebittotal);
                    ViewState["basedebittotal"] = basedebittotal;
                }
            }
            if (lblReportAmount != null && lblReportAmount.Text != "")
            {
                if (Convert.ToDouble(lblReportAmount.Text) <= 0)
                {
                    reportcredittotal = reportcredittotal + Convert.ToDouble(lblReportAmount.Text);
                    strReportCrebitTotal = String.Format("{0:n}", reportcredittotal);
                    ViewState["basecredittotal"] = reportcredittotal;
                }
                else
                {
                    reportdebittotal = Math.Abs(reportdebittotal) + Math.Abs(Convert.ToDouble(lblReportAmount.Text));
                    strReportDebitTotal = String.Format("{0:n}", reportdebittotal);
                    ViewState["basedebittotal"] = reportdebittotal;
                }
            }
            if (basedebittotal == 0.00 || strBaseDebitTotal == string.Empty)
                strBaseDebitTotal = "0.00";
            if (basecredittotal == 0.00 || strBaseCrebitTotal == string.Empty)
                strBaseCrebitTotal = "0.00";
            if (reportcredittotal == 0.00 || strReportCrebitTotal == string.Empty)
                strReportCrebitTotal = "0.00";
            if (reportdebittotal == 0.00 || strReportDebitTotal == string.Empty)
                strReportDebitTotal = "0.00";
        }

        if (e.Item is GridEditableItem)
        {

            RadLabel lblVoucherLineId1 = (RadLabel)e.Item.FindControl("lblVoucherLineId");
            RadLabel lnkVoucherLineItemNo1 = (RadLabel)e.Item.FindControl("lnkVoucherLineItemNo");
            if (lblVoucherLineId1 != null && lnkVoucherLineItemNo1 != null)
            {
                if (lblVoucherLineId1.Text != "" && lnkVoucherLineItemNo1.Text != null)
                {
                    if (ViewState["LASTADDEDROWNO"] != null)
                    {
                        if (Convert.ToInt32(ViewState["LASTADDEDROWNO"].ToString()) < Convert.ToInt32(lnkVoucherLineItemNo1.Text))
                        {
                            ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo1.Text;
                            ViewState["LASTADDEDLINEITEMID"] = lblVoucherLineId1.Text;
                        }
                    }
                    else
                    {
                        ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo1.Text;
                        ViewState["LASTADDEDLINEITEMID"] = lblVoucherLineId1.Text;
                    }
                }
            }

            RadLabel lblbudgetid = (RadLabel)e.Item.FindControl("lblBudgetId");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit");
            ImageButton ib2 = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            if (ib2 != null)
            {
                // ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + lblvesselid.Text + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

            }
            RadLabel lblAccountUsage = (RadLabel)e.Item.FindControl("lblAccountUsage");
            if (lblAccountUsage.Text.ToUpper().Trim() != "VESSEL")
                if (lblAccountUsage.Text.ToUpper().Trim() != "VESSEL")
                {

                    if (ib2 != null) ib2.Attributes.Add("style", "visibility:hidden");
                    if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                }
        }


    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            RebindOrder();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }
    protected bool IsValidPost()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtInvoicePostdate.Text == null)
            ucError.ErrorMessage = "Post date is required";

        if (ViewState["ISATTACHMENTAVAILABLE"] == null || ViewState["ISATTACHMENTAVAILABLE"].ToString() == "0")
        {
            ucError.ErrorMessage = "Attachment is required of invoice type to post the invoice";
        }

        return (!ucError.IsError);
    }
    public void GetExchangeRate(object sender, EventArgs e)
    {
        DataSet ds = PhoenixAccountsInvoice.GetBaseAndReportCurrencyExchangerate(new Guid(ViewState["invoicecode"].ToString()), DateTime.Parse(txtInvoicePostdate.Text));
        if (ds.Tables.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtBaseexchangerate.Text = dr["FLDBASEEXCHANGERATE"].ToString();
            txtReportexchangerate.Text = dr["FLDREPORTEXCHANGERATE"].ToString();
        }
    }
    public void CommondescriptionUpdate(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsVoucher.InvoiceDraftPostUpdate(txtPurchaseInvoiceVoucherNumber.Text, txtCommondescription.Text, null);
            RebindOrder();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindOrder()
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
            if (ViewState["PURCHASEINVOICEVOUCHERNUMBER"].ToString() != "" && ViewState["INVOICESTATUS"].ToString() == "243")//243 Account checking
            {
                BindData();
            }
            else if (ViewState["PURCHASEINVOICEVOUCHERNUMBER"].ToString() != "")
            {
                BindLineitemData();
            }
            else
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            if (ViewState["PURCHASEINVOICEVOUCHERNUMBER"].ToString() != "" && ViewState["INVOICESTATUS"].ToString() != "243")
            {
                BindLineitemData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
