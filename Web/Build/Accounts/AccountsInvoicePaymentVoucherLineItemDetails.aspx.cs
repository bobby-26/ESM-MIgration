using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucherLineItemDetails : PhoenixBasePage
{
    public string strReportCurrencyCode = string.Empty;
    public string strTranCurrencyCode = string.Empty;
    public decimal dTotalCommittedAmount = 0;
    public decimal dTotalChargedAmount = 0;
    public decimal dTotalVesselAmount = 0;
    public decimal dTotalPayableAmount = 0;
    public decimal dTotalGSTClaimAmount = 0;
    public decimal dTotalIncomeExpenseAmount = 0;
    public decimal dTotalRebatereceivableAmount = 0;
    public decimal dTotalPayingAmount = 0;
    public decimal dGrandVesselTotal = 0;
    public decimal dGrandPayableTotal = 0;
    public decimal dGrandGstTotal = 0;
    public decimal dGrandIncomeTotal = 0;
    public decimal dGrandRebateTotal = 0;
    public decimal dGrandPayingTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            ucConfirmMessage.Visible = false;
            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (showcreditnotedisc == 1)
                toolbarmain.AddButton("Report", "REPORT", ToolBarDirection.Right);

            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            // MenuOrderFormMain.SetTrigger(pnlStockItemEntry);

            PhoenixToolbar toolbarrevoke = new PhoenixToolbar();
            toolbarrevoke.AddButton("Revoke", "REVOKE", ToolBarDirection.Right);
            MenuRevoke.AccessRights = this.ViewState;
            MenuRevoke.MenuList = toolbarrevoke.Show();


            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                Title1.Text = "Invoice Payment Voucher Details";
                ViewState["callfrom"] = null;
                BindHeaderData();
            }

            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];
            if (ViewState["TypeOfPV"] != null)
            {
                if (ViewState["TypeOfPV"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "OFC"))
                {
                    gvInvoicePo.Columns[4].HeaderText = "Account Description";
                    gvInvoicePo.Columns[5].HeaderText = "Sub Account Code";
                    gvInvoicePo.Columns[9].HeaderText = "Amount (" + strTranCurrencyCode + ")";
                }
            }

            if (ViewState["PVStatuscode"].ToString() != "48")
            {
                string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
                //cmdApprove.Attributes.Add("onclick", "parent.openNewWindow('approval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            }
            else
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }

            PhoenixToolbar toolbargridCreditNotes = new PhoenixToolbar();
            toolbargridCreditNotes.AddImageLink("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListCreditNote.aspx?mode=custom&CURRENCYCODE=" + ViewState["CurrencyCode"].ToString() + "&SUPPCODE=" + ViewState["SuppCode"].ToString() + "', true);", "Credit Notes List", "add.png", "ADDCREDITNOTE");

            BindInvoicePOData();
            BindCreditNote();
            //BindPendingCreditNote();
            if (ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
                BindTotalFooter();


            lblLessCreditNotesOverpayment.Visible = (showcreditnotedisc == 1) ? true : false;
            MenuOrderAdd.Visible = (showcreditnotedisc == 1) ? true : false;
            gvCreditNotes.Visible = (showcreditnotedisc == 1) ? true : false;
            //lblListofCreditNoteOverpaymentPending.Visible = (showcreditnotedisc == 1) ? true : false;
            //gvCreditNotePending.Visible = (showcreditnotedisc == 1) ? true : false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "ZEROPV")
                    Response.Redirect("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }

            if (CommandName.ToUpper().Equals("REPORT"))
            {
                string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
                string paymentvoucherinvoiceid = (ViewState["PAYMENTVOUCHERINVOICEID"] == null) ? null : (ViewState["PAYMENTVOUCHERINVOICEID"].ToString());
                string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + paymentvoucherinvoiceid
                                            + "&voucherid=" + voucherid
                                            + "&invoicecode=" + invoiceid
                                            + "&orderid=" + null
                                            + "&vesselid=null&applicationcode=5&reportcode=INVOICEPAYMENTVOUCHER&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNotePending_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPendingCreditNote();
    }

    private void BindPendingCreditNote()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsInvoicePaymentVoucher.CreditNoteVoucherSearch(General.GetNullableInteger(ViewState["SuppCode"].ToString())
                                                            , General.GetNullableInteger(ViewState["CurrencyCode"].ToString())
                                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , General.GetNullableGuid(ViewState["voucherid"].ToString()));

        gvCreditNotePending.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbaradd = new PhoenixToolbar();
            toolbaradd.AddImageLink("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Accounts/AccountsInvoicePaymentVoucherCreditNoteAdd.aspx?supplier=" + ViewState["SuppCode"].ToString() + "&currency=" + ViewState["CurrencyCode"].ToString() + "&voucherid=" + ViewState["voucherid"].ToString() + "'); return false;", "Add", "add.png", "ADD");
            MenuOrderAdd.AccessRights = this.ViewState;
            MenuOrderAdd.MenuList = toolbaradd.Show();
        }
    }

    protected void gvCreditNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCreditNote();
    }

    private void BindCreditNote()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherCreditNoteSearch(General.GetNullableGuid(ViewState["voucherid"].ToString()));

            gvCreditNotes.DataSource = ds;
        }
    }

    private void BindHeaderData()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                    ViewState["CurrencyCode"] = dr["FLDCURRENCY"].ToString();
                    ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
                    ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUSCODE"].ToString();
                    ViewState["PVType"] = dr["FLDPAYMENTVOUCHERTYPE"].ToString();
                    ViewState["RemittanceId"] = dr["FLDREMITTANCEID"].ToString();
                    ViewState["TypeOfPV"] = dr["FLDSUBTYPE"].ToString();
                    strTranCurrencyCode = dr["FLDCURRENCYCODE"].ToString();
                    txtRevokeBy.Text = dr["FLDREVOKEDBY"].ToString();
                    if (General.GetNullableDateTime(dr["FLDREVOKEDDATE"].ToString()) != null)
                        txtRevokedDate.Text = dr["FLDREVOKEDDATE"].ToString();
                    txtRevokeRemarks.Text = dr["FLDREVOKEDREASON"].ToString();

                    if (ViewState["PVStatuscode"].ToString() == "48")
                    {
                        cmdApprove.Attributes.Add("style", "visibility:hidden");
                    }

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
                    txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
                    txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
                    lblBankdetails.Text = dr["FLDBANKINGDETAILS"].ToString();
                    txtBankdetails.Text = dr["FLDSWIFTCODE"].ToString();
                }
            }
        }
        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblPayableAmount.Visible = (showcreditnotedisc == 1) ? true : false;
        txtAmount.Visible = (showcreditnotedisc == 1) ? true : false;

    }
    private void BindTotalFooter()
    {
        DataSet ds = new DataSet();
        Guid frominvoicecode = Guid.Empty, toinvoicecode = Guid.Empty;
        Guid fromorderid = Guid.Empty, toorderid = Guid.Empty;
        ds = PhoenixAccountsInvoicePaymentVoucher.InvoicePaymentVoucherLineItemTotal(new Guid(ViewState["voucherid"].ToString()));
        dGrandVesselTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALVESSELAMOUNT"].ToString()));
        dGrandPayableTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALPAYABLEAMOUNT"].ToString()));
        dGrandGstTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALGSTCLAIMAMOUNT"].ToString()));
        dGrandIncomeTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALINCOMEEXPENSEAMOUNT"].ToString()));
        dGrandRebateTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALREBATERECEIVABLEAMOUNT"].ToString()));
        dGrandPayingTotal = (Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDTOTALPAYABLEAMOUNT"].ToString()));

        dGrandVesselTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandVesselTotal));
        dGrandPayableTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandPayableTotal));
        dGrandGstTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandGstTotal));
        dGrandIncomeTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandIncomeTotal));
        dGrandRebateTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandRebateTotal));
        dGrandPayingTotal = Convert.ToDecimal(string.Format("{0:0.00}", dGrandPayingTotal));
    }

    protected void gvInvoicePo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindInvoicePOData();
    }
    private void BindInvoicePOData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDBASEAMOUNT", "FLDREPORTAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Sub Account", "Transaction Currency", "Transaction Amount", "Base Amount", "Report Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
        {

            ds = PhoenixAccountsInvoicePaymentVoucher.InvoicePaymentVoucherPOSearch(ViewState["voucherid"].ToString());

            gvInvoicePo.DataSource = ds;
            gvInvoicePo.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                strReportCurrencyCode = ds.Tables[0].Rows[0]["FLDREPORTCURRENCYCODE"].ToString();
                if (ViewState["ORDERID"] == null)
                {
                    ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
                    // gvInvoicePo.SelectedIndex = 0;
                    ViewState["ORDERNUMBER"] = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
                    BindAdvancePaymentData();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //ShowNoRecordsFound(dt, gvInvoicePo);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvInvoice", "Voucher Line Item", alCaptions, alColumns, ds);
        }
        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvInvoicePo.Columns[10].Visible = (showcreditnotedisc == 1) ? true : false;
        gvInvoicePo.Columns[11].Visible = (showcreditnotedisc == 1) ? true : false;
        gvInvoicePo.Columns[12].Visible = (showcreditnotedisc == 1) ? true : false;
        gvInvoicePo.Columns[13].Visible = (showcreditnotedisc == 1) ? true : false;
    }


    protected void gvInvoicePo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["TypeOfPV"] != null)
            {
                if (ViewState["TypeOfPV"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "OFC"))
                {
                    gvInvoicePo.Columns[4].HeaderText = "Account Description";
                    gvInvoicePo.Columns[5].HeaderText = "Sub Account Code";
                    gvInvoicePo.Columns[9].HeaderText = "Amount (" + strTranCurrencyCode + ")";
                    gvInvoicePo.Columns[5].FooterText = "Total";
                }
            }
        }
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

        if (e.Item is GridDataItem)
        {
            if (!e.Item.ItemType.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.ItemType.Equals(DataControlRowState.Edit))
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblInvoicePoId");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblPoNumber");
                RadLabel lblInvoiceCode1 = (RadLabel)e.Item.FindControl("lblInvoiceCode");

                {
                    if (e.Item is GridDataItem)
                    {
                        string strLinks;
                        string strLinkids;
                        string strCurrentid = string.Empty;

                        strLinks = lb.Text;
                        strLinkids = l.Text;
                        string[] arrLinks;
                        string[] arrLinkids;

                        arrLinks = strLinks.Split(new Char[] { ',' });
                        arrLinkids = strLinkids.Split(new Char[] { ',' });

                        for (int i = 0; i < arrLinks.Length; i++)
                        {
                            LinkButton btnLink = new LinkButton();
                            btnLink.ID = "Id" + arrLinks[i] + i;
                            btnLink.Text = arrLinks[i] + "<br>";
                            strCurrentid = arrLinkids[i];
                            btnLink.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoicePaymentVoucherPODetails.aspx?orderid=" + strCurrentid + "+&voucherid=" + ViewState["voucherid"].ToString() + "');return false;");
                            e.Item.Cells[5].Controls.Add(btnLink);
                        }
                    }
                }


                LinkButton lnkInvoiceNumber = (LinkButton)e.Item.FindControl("lnkInvoiceNumber");
                if (lnkInvoiceNumber != null)
                {
                    lnkInvoiceNumber.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsFileAttachment.aspx?DTKEY=" + lnkInvoiceNumber.CommandArgument + "&MOD=" + PhoenixModule.ACCOUNTS + "&U=1" + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, lnkInvoiceNumber.CommandName)) lnkInvoiceNumber.Visible = false;
                }
            }

            RadLabel lblVendorinvoiceduplicateexists = (RadLabel)e.Item.FindControl("lblVendorinvoiceduplicateexists");
            RadLabel lblInvoiceCode = (RadLabel)e.Item.FindControl("lblInvoiceCode");
            HyperLink HlinkRefDuplicate = (HyperLink)e.Item.FindControl("HlinkRefDuplicate");
            if (lblVendorinvoiceduplicateexists.Text == "1")
            {
                HlinkRefDuplicate.NavigateUrl = "#";
                HlinkRefDuplicate.Visible = true;
                HlinkRefDuplicate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVendorInvoiceDuplicateList.aspx?qinvoicecode=" + lblInvoiceCode.Text + "');return false;");
            }

            RadLabel lblSupplierCurrencyMismatch = (RadLabel)e.Item.FindControl("lblSupplierCurrencyMismatch");
            ImageButton imbSupplierMismatch = (ImageButton)e.Item.FindControl("imbSupplierMismatch");
            ImageButton imbCurrencyMismatch = (ImageButton)e.Item.FindControl("imbCurrencyMismatch");

            if (lblSupplierCurrencyMismatch != null)
            {
                int mismatch = !string.IsNullOrEmpty(lblSupplierCurrencyMismatch.Text) ? int.Parse(lblSupplierCurrencyMismatch.Text) : 0;
                if (mismatch == 1 && imbCurrencyMismatch != null)
                    imbCurrencyMismatch.Visible = true;
                if (mismatch == 2 && imbSupplierMismatch != null)
                    imbSupplierMismatch.Visible = true;
                if (mismatch == 3)
                {
                    if (imbCurrencyMismatch != null)
                        imbCurrencyMismatch.Visible = true;
                    if (imbSupplierMismatch != null)
                        imbSupplierMismatch.Visible = true;
                }
            }
            if (imbCurrencyMismatch != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbCurrencyMismatch.CommandName)) imbCurrencyMismatch.Visible = false;
            if (imbSupplierMismatch != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbSupplierMismatch.CommandName)) imbSupplierMismatch.Visible = false;

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton imgpo = new ImageButton();
            imgpo = (ImageButton)e.Item.FindControl("imgReceivedBeforeInvoice");

            ImageButton cmdDelete = new ImageButton();
            cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");

            if (ViewState["RemittanceId"].ToString().Length > 0)
                cmdDelete.Visible = false;

            if (drv["FLDISINVOICEREGISTEREDBEFOREPO"].ToString() == "1" && imgpo != null)
                imgpo.Visible = true;
        }

        if (e.Item is GridFooterItem)
        {
            if (ViewState["TypeOfPV"] != null)
            {
                if (ViewState["TypeOfPV"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "OFC"))
                {
                    gvInvoicePo.Columns[6].Visible = false;
                    gvInvoicePo.Columns[7].Visible = false;
                    gvInvoicePo.Columns[8].Visible = false;
                }
            }
        }
        GridDecorator.MergeRows(gvInvoicePo);
    }

    protected void gvAdvancePayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindAdvancePaymentData();
    }

    private void BindAdvancePaymentData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        if (ViewState["ORDERNUMBER"] != null)
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.POAdvancePaymentSearch(ViewState["voucherid"].ToString());

            gvAdvancePayments.DataSource = ds;
            gvAdvancePayments.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }

    private void BindCreditNotesData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDBASEAMOUNT", "FLDREPORTAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Sub Account", "Transaction Currency", "Transaction Amount", "Base Amount", "Report Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
        string paymentvoucherinvoiceid = (ViewState["PAYMENTVOUCHERINVOICEID"] == null) ? null : (ViewState["PAYMENTVOUCHERINVOICEID"].ToString());
        string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());
        string orderid = (ViewState["ORDERID"] == null) ? null : (ViewState["ORDERID"].ToString());

        if (ViewState["voucherid"] != null)
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.InvoicePaymentVoucherLineItemSearch(paymentvoucherinvoiceid, voucherid, invoiceid, orderid,
                 null, sortexpression, sortdirection);

            gvCreditNotes.DataSource = ds;
            gvCreditNotes.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvInvoice", "Voucher Line Item", alCaptions, alColumns, ds);
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoicePo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            try
            {
                DeleteInvoiceMapping(((RadLabel)e.Item.FindControl("lblInvoiceCode")).Text);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            ViewState["INVOICEID"] = null;
            ViewState["ORDERID"] = null;

            dTotalCommittedAmount = 0;
            dTotalChargedAmount = 0;
            dTotalVesselAmount = 0;
            dTotalPayableAmount = 0;
            dTotalGSTClaimAmount = 0;
            dTotalIncomeExpenseAmount = 0;
            dTotalRebatereceivableAmount = 0;
            dTotalPayingAmount = 0;
            BindInvoicePOData();
        }
    }

    private void DeleteInvoiceMapping(string strInvoicecode)
    {
        PhoenixAccountsInvoicePaymentVoucher.DeleteInvoiceMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode, strInvoicecode);
    }

    private void DeleteCreditNotesMapping(string creditdebitnoteid, string orderid)
    {
        PhoenixAccountsInvoicePaymentVoucher.DeleteCreditNoteMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            creditdebitnoteid, orderid);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindHeaderData();
            BindInvoicePOData();
            BindCreditNote();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvCreditNotes_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblCreditNoteMappingid = (RadLabel)e.Item.FindControl("lblCreditMappingId");

                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherCreditMappingDelete(General.GetNullableGuid(lblCreditNoteMappingid.Text));

                BindHeaderData();
                BindInvoicePOData();
                BindCreditNote();
                //BindPendingCreditNote();
            }

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblCreditMappingIdEdit = (RadLabel)e.Item.FindControl("lblCreditMappingIdEdit");
                RadTextBox txtCreditAmount = (RadTextBox)e.Item.FindControl("txtCurrentUtilization");

                RadLabel lblOriginalAmount = (RadLabel)e.Item.FindControl("lblAmount");
                RadLabel lblAlreadyUtilised = (RadLabel)e.Item.FindControl("lblAlreadyUtilized");

                decimal balance = decimal.Parse(lblOriginalAmount.Text) - decimal.Parse(lblAlreadyUtilised.Text);

                if (General.GetNullableDecimal(txtCreditAmount.Text) == 0)
                {
                    ucError.ErrorMessage = "Amount to be entered";
                    ucError.Visible = true;
                    return;
                }

                if (!(balance >= decimal.Parse(txtCreditAmount.Text)))
                {
                    ucError.ErrorMessage = "Amount to be less than available amount";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherCreditMappingUpdate(General.GetNullableGuid(lblCreditMappingIdEdit.Text), decimal.Parse(txtCreditAmount.Text));

                    // _gridView.EditIndex = -1;

                    BindHeaderData();
                    BindInvoicePOData();
                    BindCreditNote();
                    //BindPendingCreditNote();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNotes_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblAlreadyUtilized = (RadLabel)e.Item.FindControl("lblAlreadyUtilized");
            LinkButton lnkAmountUtilized = (LinkButton)e.Item.FindControl("lnkAmountUtilized");
            RadLabel lblCreditNoteid = (RadLabel)e.Item.FindControl("lblCreditNoteId");
            RadLabel lblPaymentVOucherId = (RadLabel)e.Item.FindControl("lblPaymentVOucherId");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (drv != null)
            {
                if (General.GetNullableDecimal(drv["FLDALREADYUTILIZED"].ToString()) > 0)
                {
                    lnkAmountUtilized.Visible = true;
                    lnkAmountUtilized.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Accounts/AccountsInvoicePaymentVoucherCreditNoteView.aspx?creditnoteid=" + lblCreditNoteid.Text + "&paymentvoucherid=" + ViewState["voucherid"].ToString() + "'); return false;");
                }
                else
                    lblAlreadyUtilized.Visible = true;
            }

        }
    }


    protected void gvInvoicePOTotal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lblCommittedAmt = (RadLabel)e.Row.FindControl("lblCommittedAmt");
            RadLabel lblChargedAmt = (RadLabel)e.Row.FindControl("lblChargedAmt");
            RadLabel lblVesselAmt = (RadLabel)e.Row.FindControl("lblVesselAmt");
            RadLabel lblPayableAmt = (RadLabel)e.Row.FindControl("lblPayableAmt");
            RadLabel lblGSTClaimAmt = (RadLabel)e.Row.FindControl("lblGSTClaimAmt");
            RadLabel lblIncomeAmt = (RadLabel)e.Row.FindControl("lblIncomeAmt");
            RadLabel lblRebatesAmt = (RadLabel)e.Row.FindControl("lblRebatesAmt");
            RadLabel lblPayingAmt = (RadLabel)e.Row.FindControl("lblPayingAmt");
            if (lblCommittedAmt != null && lblCommittedAmt.Text != "")
                dTotalCommittedAmount = dTotalCommittedAmount + decimal.Parse(lblCommittedAmt.Text);
            if (lblChargedAmt != null && lblChargedAmt.Text != "")
                dTotalChargedAmount = dTotalChargedAmount + decimal.Parse(lblChargedAmt.Text);
            if (lblVesselAmt != null && lblVesselAmt.Text != "")
                dTotalVesselAmount = dTotalVesselAmount + decimal.Parse(lblVesselAmt.Text);
            if (lblPayableAmt != null && lblPayableAmt.Text != "")
                dTotalPayableAmount = dTotalPayableAmount + decimal.Parse(lblPayableAmt.Text);
            if (lblGSTClaimAmt != null && lblGSTClaimAmt.Text != "")
                dTotalGSTClaimAmount = dTotalGSTClaimAmount + decimal.Parse(lblGSTClaimAmt.Text);
            if (lblIncomeAmt != null && lblIncomeAmt.Text != "")
                dTotalIncomeExpenseAmount = dTotalIncomeExpenseAmount + decimal.Parse(lblIncomeAmt.Text);
            if (lblRebatesAmt != null && lblRebatesAmt.Text != "")
                dTotalRebatereceivableAmount = dTotalRebatereceivableAmount + decimal.Parse(lblRebatesAmt.Text);
            if (lblPayingAmt != null && lblPayingAmt.Text != "")
                dTotalPayingAmount = dTotalPayingAmount + decimal.Parse(lblPayingAmt.Text);
        }
    }

    protected void gvInvoicePo_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        // gvInvoicePo.SelectedIndex = e.NewSelectedIndex;
        ViewState["ORDERID"] = ((RadLabel)gvInvoicePo.Items[e.NewSelectedIndex].FindControl("lblInvoicePoId")).Text;
        ViewState["ORDERNUMBER"] = ((LinkButton)gvInvoicePo.Items[e.NewSelectedIndex].FindControl("lblPoNumber")).Text;
        BindCreditNotesData();
        BindAdvancePaymentData();
    }

    protected void gvInvoicePo_PreRender(object sender, EventArgs e)
    {
        //   GridDecorator.MergeRows(gvInvoicePo);
    }

    public class GridDecorator
    {
        //public static void MergeRows(GridView gridView)
        //{
        //    for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        //    {
        //        GridViewRow row = gridView.Rows[rowIndex];
        //        GridViewRow previousRow = gridView.Rows[rowIndex + 1];

        //        string strCurrentBudgetGroupCode = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblBudgetGroupId")).Text;
        //        string strPreviousBudgetGroupCode = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblBudgetGroupId")).Text;

        //        string strCurrentInvoicenumber = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblInvoiceNumber")).Text;
        //        string strPreviousInvoicenumber = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblInvoiceNumber")).Text;

        //        string strCurrentVesselName = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblVesselName")).Text;
        //        string strPreviousVesselName = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblVesselName")).Text;


        //        string strCurrentFormNo = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblPoNumber")).Text;
        //        string strPrviousFormNo = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblPoNumber")).Text;

        //        if (strCurrentInvoicenumber == strPreviousInvoicenumber)
        //        {
        //            row.Cells[2].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
        //                previousRow.Cells[2].RowSpan + 1;
        //            previousRow.Cells[2].Visible = false;

        //            if (strCurrentFormNo == strPrviousFormNo)
        //            {

        //                //row.Cells[4].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
        //                //previousRow.Cells[4].RowSpan + 1;
        //                //previousRow.Cells[4].Visible = false;

        //                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
        //                previousRow.Cells[3].RowSpan + 1;
        //                previousRow.Cells[3].Visible = false;
        //            }

        //        }

        //        if (strCurrentInvoicenumber == strPreviousInvoicenumber)
        //        {
        //            row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[0].RowSpan + 1;
        //            previousRow.Cells[0].Visible = false;

        //            row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[1].RowSpan + 1;
        //            previousRow.Cells[1].Visible = false;

        //            //row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[9].RowSpan + 1;
        //            //previousRow.Cells[9].Visible = false;

        //            row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
        //            previousRow.Cells[10].RowSpan + 1;
        //            previousRow.Cells[10].Visible = false;

        //            row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
        //            previousRow.Cells[11].RowSpan + 1;
        //            previousRow.Cells[11].Visible = false;

        //            row.Cells[12].RowSpan = previousRow.Cells[12].RowSpan < 2 ? 2 :
        //            previousRow.Cells[12].RowSpan + 1;
        //            previousRow.Cells[12].Visible = false;

        //            row.Cells[13].RowSpan = previousRow.Cells[13].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[13].RowSpan + 1;
        //            previousRow.Cells[13].Visible = false;

        //            //row.Cells[15].RowSpan = previousRow.Cells[15].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[15].RowSpan + 1;
        //            //previousRow.Cells[15].Visible = false;

        //            //row.Cells[16].RowSpan = previousRow.Cells[16].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[16].RowSpan + 1;
        //            //previousRow.Cells[16].Visible = false;

        //            //row.Cells[17].RowSpan = previousRow.Cells[17].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[17].RowSpan + 1;
        //            //previousRow.Cells[17].Visible = false;
        //        }

        //        if ((strCurrentBudgetGroupCode == strPreviousBudgetGroupCode) & (strCurrentInvoicenumber == strPreviousInvoicenumber))
        //        {

        //            //row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[4].RowSpan + 1;
        //            //previousRow.Cells[4].Visible = false;

        //            row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
        //            previousRow.Cells[5].RowSpan + 1;
        //            previousRow.Cells[5].Visible = false;

        //            //row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[6].RowSpan + 1;
        //            //previousRow.Cells[6].Visible = false;

        //            //row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
        //            //previousRow.Cells[7].RowSpan + 1;
        //            //previousRow.Cells[7].Visible = false;

        //            //row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
        //            //                       previousRow.Cells[8].RowSpan + 1;
        //            //previousRow.Cells[8].Visible = false;

        //            //row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
        //            //                       previousRow.Cells[9].RowSpan + 1;
        //            //previousRow.Cells[9].Visible = false;

        //            row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[10].RowSpan + 1;
        //            previousRow.Cells[10].Visible = false;

        //            row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[11].RowSpan + 1;
        //            previousRow.Cells[11].Visible = false;

        //            row.Cells[12].RowSpan = previousRow.Cells[12].RowSpan < 2 ? 2 :
        //                                   previousRow.Cells[12].RowSpan + 1;
        //            previousRow.Cells[12].Visible = false;

        //            row.Cells[13].RowSpan = previousRow.Cells[13].RowSpan < 2 ? 2 :
        //            previousRow.Cells[13].RowSpan + 1;
        //            previousRow.Cells[13].Visible = false;

        //            //row.Cells[14].RowSpan = previousRow.Cells[14].RowSpan < 2 ? 2 :
        //            //                       previousRow.Cells[14].RowSpan + 1;
        //            //previousRow.Cells[14].Visible = false;

        //            //row.Cells[15].RowSpan = previousRow.Cells[15].RowSpan < 2 ? 2 :
        //            //                       previousRow.Cells[15].RowSpan + 1;
        //            //previousRow.Cells[15].Visible = false;

        //        }
        //    }
        //}
        public static void MergeRows(RadGrid gridView)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string strCurrentBudgetGroupCode = ((RadLabel)gridView.Items[rowIndex].FindControl("lblBudgetGroupId")).Text;
                string strPreviousBudgetGroupCode = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblBudgetGroupId")).Text;

                string strCurrentInvoicenumber = ((RadLabel)gridView.Items[rowIndex].FindControl("lblInvoiceNumber")).Text;
                string strPreviousInvoicenumber = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblInvoiceNumber")).Text;

                string strCurrentVesselName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblVesselName")).Text;
                string strPreviousVesselName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblVesselName")).Text;

                string strCurrentPoNumber = ((RadLabel)gridView.Items[rowIndex].FindControl("lblPoNumber")).Text;
                string strPreviousPoNumber = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblPoNumber")).Text;


                if (strCurrentVesselName == strPreviousVesselName)
                {

                    //row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                    //previousRow.Cells[2].RowSpan + 1;
                    //previousRow.Cells[2].Visible = false;

                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                    previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }
                if (strCurrentPoNumber == strPreviousPoNumber)
                {

                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                    previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }


                if (strCurrentInvoicenumber == strPreviousInvoicenumber)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;

                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;

                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;

                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                          previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;

                    //row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                    //previousRow.Cells[9].RowSpan + 1;
                    //previousRow.Cells[9].Visible = false;

                    //row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                    //previousRow.Cells[10].RowSpan + 1;
                    //previousRow.Cells[10].Visible = false;

                    //row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
                    //previousRow.Cells[11].RowSpan + 1;
                    //previousRow.Cells[11].Visible = false;

                    row.Cells[12].RowSpan = previousRow.Cells[12].RowSpan < 2 ? 2 :
                    previousRow.Cells[12].RowSpan + 1;
                    previousRow.Cells[12].Visible = false;

                    row.Cells[13].RowSpan = previousRow.Cells[13].RowSpan < 2 ? 2 :
                                           previousRow.Cells[13].RowSpan + 1;
                    previousRow.Cells[13].Visible = false;

                    row.Cells[14].RowSpan = previousRow.Cells[14].RowSpan < 2 ? 2 :
                                          previousRow.Cells[14].RowSpan + 1;
                    previousRow.Cells[14].Visible = false;

                    row.Cells[15].RowSpan = previousRow.Cells[15].RowSpan < 2 ? 2 :
                    previousRow.Cells[15].RowSpan + 1;
                    previousRow.Cells[15].Visible = false;

                    //row.Cells[16].RowSpan = previousRow.Cells[16].RowSpan < 2 ? 2 :
                    //previousRow.Cells[16].RowSpan + 1;
                    //previousRow.Cells[16].Visible = false;

                    //row.Cells[17].RowSpan = previousRow.Cells[17].RowSpan < 2 ? 2 :
                    //previousRow.Cells[17].RowSpan + 1;
                    //previousRow.Cells[17].Visible = false;
                }


                if ((strCurrentBudgetGroupCode == strPreviousBudgetGroupCode) & (strCurrentInvoicenumber == strPreviousInvoicenumber))
                {

                    //row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                    //previousRow.Cells[4].RowSpan + 1;
                    //previousRow.Cells[4].Visible = false;

                    //row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                    //previousRow.Cells[5].RowSpan + 1;
                    //previousRow.Cells[5].Visible = false;

                    //row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                    //previousRow.Cells[6].RowSpan + 1;
                    //previousRow.Cells[6].Visible = false;

                    row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
                    previousRow.Cells[7].RowSpan + 1;
                    previousRow.Cells[7].Visible = false;

                    row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                           previousRow.Cells[8].RowSpan + 1;
                    previousRow.Cells[8].Visible = false;

                    row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                           previousRow.Cells[9].RowSpan + 1;
                    previousRow.Cells[9].Visible = false;

                    row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                           previousRow.Cells[10].RowSpan + 1;
                    previousRow.Cells[10].Visible = false;

                    //row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
                    //                       previousRow.Cells[11].RowSpan + 1;
                    //previousRow.Cells[11].Visible = false;

                    row.Cells[12].RowSpan = previousRow.Cells[12].RowSpan < 2 ? 2 :
                                           previousRow.Cells[12].RowSpan + 1;
                    previousRow.Cells[12].Visible = false;

                    row.Cells[13].RowSpan = previousRow.Cells[13].RowSpan < 2 ? 2 :
                    previousRow.Cells[13].RowSpan + 1;
                    previousRow.Cells[13].Visible = false;

                    //row.Cells[14].RowSpan = previousRow.Cells[14].RowSpan < 2 ? 2 :
                    //                       previousRow.Cells[14].RowSpan + 1;
                    //previousRow.Cells[14].Visible = false;

                    //row.Cells[15].RowSpan = previousRow.Cells[15].RowSpan < 2 ? 2 :
                    //                       previousRow.Cells[15].RowSpan + 1;
                    //previousRow.Cells[15].Visible = false;

                }
            }
        }

    }
    protected void MenuRevoke_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {


            if (CommandName.ToUpper().Equals("REVOKE"))
            {
                if (General.GetNullableString(txtRevokeRemarks.Text) == null)
                {
                    ucError.ErrorMessage = "Revoke Remarks is required.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRevoke(General.GetNullableGuid(ViewState["voucherid"].ToString()), txtRevokeRemarks.Text);
                BindHeaderData();

                if (ViewState["PVStatuscode"].ToString() != "48")
                {
                    string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
                    cmdApprove.Attributes.Add("style", "visibility:visible");
                    // cmdApprove.Attributes.Add("onclick", "parent.openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
                }
                else
                {
                    cmdApprove.Attributes.Add("style", "display:none");
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DirectApproval()
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(459, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), 459, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), 459, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), 459, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());
        ucStatus.Text = "Payment voucher Approved";

    }
    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval();
            BindHeaderData();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }
            //ucConfirmMessage.Visible = true;
            //ucConfirmMessage.Text = "Do you want Proceed?.";
            //return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OnAction_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            DirectApproval();
            BindHeaderData();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkCNOverPayPending_Click(object sender, EventArgs e)
    {
        try
        {
            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            lblListofCreditNoteOverpaymentPending.Visible = (showcreditnotedisc == 1) ? true : false;
            gvCreditNotePending.Visible = (showcreditnotedisc == 1) ? true : false;
            gvCreditNotePending.Rebind();
            //BindPendingCreditNote();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
