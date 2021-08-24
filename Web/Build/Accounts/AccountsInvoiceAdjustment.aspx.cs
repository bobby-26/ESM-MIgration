using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsInvoiceAdjustment : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //txtVendorCode.Attributes.Add("onkeydown", "return false;");
        //txtVenderName.Attributes.Add("onkeydown", "return false;");

        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            toolbar.AddButton("Comments", "COMMENTS", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAdjustment.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','Add','AccountsInvoiceLineItem.aspx?qinvoicecode=" + Request.QueryString["QINVOICECODE"] + "'); return false;", "Add", "Add.png", "ADD");
        toolbargrid.AddImageLink("javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["CDTKEY"] + "&mod="
                   + PhoenixModule.CREW + "'); return false;", "Approved PD", "", "APPROVEDPD");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            txtInvoiceReceivedDateEdit.Text = DateTime.Now.ToString();

            short showcreditnote = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            lblDiscount.Visible = (showcreditnote == 1) ? true : false;
            txtTotalDiscountAmount.Visible = (showcreditnote == 1) ? true : false;

            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
            }

            if (ViewState["FLDISPOSTED"] != null)
            {
                if (int.Parse(ViewState["FLDISPOSTED"].ToString()) == 0)
                {
                    if (ucAdjustmentAmount.Text == "" || ucAdjustmentAmount.Text == "0.00")
                        imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                                + PhoenixModule.ACCOUNTS + "&type=Adjustment&adjustmentamount=" + ucAdjustmentAmount.Text + "'); return false;");
                    else
                        imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                                + PhoenixModule.ACCOUNTS + "&type=Adjustment&adjustmentamount=" + ucAdjustmentAmount.Text + "&U=D'); return false;");
                }
                else
                {
                    if (ucAdjustmentAmount.Text == "" || ucAdjustmentAmount.Text == "0.00")
                        imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                                + PhoenixModule.ACCOUNTS + "&type=Adjustment" + "&U=1&adjustmentamount=" + ucAdjustmentAmount.Text + "'); return false;");
                    else
                        imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                                + PhoenixModule.ACCOUNTS + "&type=Adjustment" + "&U=1&adjustmentamount=" + ucAdjustmentAmount.Text + "'); return false;");

                }
            }
        }

        //if (ViewState["INVOICECODE"] != null)
        //{
        //    ttlInvoice.Text = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        //}

        txtVendorId.Attributes.Add("style", "visibility:hidden");
        // ddlInvoiceType.SelectedHard = "239";
    }

    protected void Invoice_SetExchangeRate(object sender, EventArgs e)
    {
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ddlCurrencyCode.SelectedCurrency));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtExchangeRateEdit.Text = string.Format(String.Format("{0:#####.000000}", drInvoice["FLDEXCHANGERATE"]));
            }
        }
        else
        {
            txtExchangeRateEdit.Text = "";
        }
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        string strInvoiceCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceList.aspx");
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidInvoice())
            {
                ucError.Visible = true;
                return;
            }

            int PriorityInvoice = 0;
            if (chkPriorityInv.Checked == true)
            {
                PriorityInvoice = 1;
            }

            try
            {
                string strAdjustmentRemarks = "";
                if (ViewState["INVOICEADJUSTMENTREMARKS"] != null && ViewState["INVOICEADJUSTMENTREMARKS"].ToString() != string.Empty)
                {
                    strAdjustmentRemarks = ViewState["INVOICEADJUSTMENTREMARKS"].ToString();
                    if (txtAdjustmentRemarks.Text.Trim().Length > strAdjustmentRemarks.Trim().Length)
                        strAdjustmentRemarks += txtAdjustmentRemarks.Text.Substring(strAdjustmentRemarks.Trim().Length, (txtAdjustmentRemarks.Text.Trim().Length - strAdjustmentRemarks.Trim().Length));

                }
                else
                {
                    strAdjustmentRemarks = txtAdjustmentRemarks.Text.Trim();
                }
                PhoenixAccountsInvoice.InvoiceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), txtSupplierRefEdit.Text, int.Parse(ddlCurrencyCode.SelectedCurrency), decimal.Parse(ucInvoiceAmoutEdit.Text),
                    0, null, decimal.Parse(txtExchangeRateEdit.Text),
                    DateTime.Parse(txtInvoiceDateEdit.Text), DateTime.Parse(txtInvoiceReceivedDateEdit.Text),
                      General.GetNullableInteger (txtVendorId.Text), int.Parse(ddlInvoiceType.SelectedHard),
                    txtRemarks.Text, ViewState["FLDPHYSICALLOCATION"] != null ? General.GetNullableInteger(ViewState["FLDPHYSICALLOCATION"].ToString()) : null, General.GetNullableInteger(ddlReason.SelectedQuick), string.Empty, null, General.GetNullableDecimal(ucAdjustmentAmount.Text), strAdjustmentRemarks, null, General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), PriorityInvoice, null, null, null, null);
                ucStatus.Text = "Invoice information is updated";
            }

            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            strInvoiceCode = ViewState["INVOICECODE"].ToString();
        }
        String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString());
        }
        if (CommandName.ToUpper().Equals("COMMENTS"))
        {
            if (ViewState["INVOICECODE"] == null)
                Response.Redirect("../Accounts/AccountsInvoiceReconcilationComments.aspx?pageno=" + ViewState["PAGENUMBER"]);
            else
                Response.Redirect("../Accounts/AccountsInvoiceReconcilationComments.aspx?INVOICECODE=" + ViewState["INVOICECODE"].ToString() + "&pageno=" + ViewState["PAGENUMBER"]);
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void InvoiceEdit()
    {
        if (ViewState["INVOICECODE"] != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet dsattachment = new DataSet();
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceReconcileEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0 && dsInvoice != null && dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtTotalAmount.Text = drInvoice["FLDAMOUNT"].ToString();
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString());
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
                txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
                txtVenderName.Text = drInvoice["FLDNAME"].ToString();
                txtInvoiceReceivedDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICERECEIVEDDATE"].ToString());
                ddlInvoiceType.SelectedHard = drInvoice["FLDINVOICETYPE"].ToString();
                ddlCurrencyCode.SelectedCurrency = drInvoice["FLDCURRENCY"].ToString();
                txtCurrency.Text = drInvoice["FLDCURRENCYCODE"].ToString();
                txtExchangeRateEdit.Text = drInvoice["FLDEXCHANGERATE"].ToString();
                ucInvoiceAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]));
                txtTotalPayableAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", decimal.Parse(drInvoice["FLDINVOICEAMOUNT"].ToString()) - decimal.Parse(drInvoice["FLDINVOICEADJUSTMENTAMOUNT"].ToString())));
                txtTotalDiscountAmount.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDTOTALPODISCOUNT"]));
                txtPOPaybleAmount.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDTOTALPOPAYBLEAMOUNT"]));
                if (Convert.ToDouble(txtTotalDiscountAmount.Text) == 0.00)
                    txtTotalDiscountAmount.Text = "0.00";
                txtTotalGSTAmount.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDTOTALGSTAMOUNT"]));
                txtRemarks.Text = drInvoice["FLDREMARKS"].ToString();
                txtStatus.Text = drInvoice["FLDINVOICESTATUSNAME"].ToString();
                txtDTKey.Text = drInvoice["FLDDTKEY"].ToString();
                //ddlPhysicalLocation.SelectedCompany = drInvoice["FLDPHYSICALLOCATION"].ToString();
                ViewState["FLDPHYSICALLOCATION"] = drInvoice["FLDPHYSICALLOCATION"].ToString();
                ddlReason.SelectedQuick = drInvoice["FLDREASONSFORHOLDING"].ToString();
                //txtUserName.Text = drInvoice["FLDUSERNAME"].ToString();
                //ddlEarmarkedCompany.SelectedCompany = drInvoice["FLDEARMARKEDCOMPANY"].ToString();
                ucAdjustmentAmount.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEADJUSTMENTAMOUNT"]));
                txtAdjustmentRemarks.Text = drInvoice["FLDINVOICEADJUSTMENTREMARKS"].ToString() + "\n";
                ddlLiabilitycompany.SelectedCompany = drInvoice["FLDLIABILITYCOMPANY"].ToString();
                ViewState["FLDINVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"].ToString();
                ViewState["FLDISPOSTED"] = drInvoice["FLDISPOSTED"].ToString();
                dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(txtDTKey.Text), null, "invoice", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                ViewState["INVOICEADJUSTMENTREMARKS"] = drInvoice["FLDINVOICEADJUSTMENTREMARKS"].ToString();
                lblBillToCompanyName.Text = drInvoice["FLDLIABILITYCOMPANYNAME"].ToString();
                if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "SPI"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
                }
                else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "QTY"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
                }
                else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=183', true); ");
                }
                else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD")
                || drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132', true); ");
                }
                else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true); ");
                }
                else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "BNP"))
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListBondAndProvisionAddress.aspx', true); ");
                }
                else
                {
                    ImgSupplierPickList.Attributes["onclick"] = null;
                }

                if (drInvoice["FLDVENDORINVOICENUMBERALREADYEXISTS"].ToString() == "1")
                {
                    HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsVendorInvoiceDuplicateList.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString();
                    HlinkRefDuplicate.Visible = true;
                }

                if (drInvoice["FLDISCLASSGVENDOR"].ToString() == "1")
                    HlinkClassG.Visible = true;

                if (drInvoice["FLDPRIORITYINVOICE"].ToString() == "1")
                {
                    chkPriorityInv.Checked = true;
                }
                ucToolTipStatus.Text = drInvoice["FLDINVOICESTATUSCHANGEINFO"].ToString();
                if (txtStatus != null && (drInvoice["FLDINVOICESTATUS"].ToString() == "242" || drInvoice["FLDINVOICESTATUS"].ToString() == "372"))

                {
                    txtStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'visible');");
                    txtStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'hidden');");
                }

            }
        }
    }

    protected void InvoiceEditAterTaxUpdate()
    {
        if (ViewState["INVOICECODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceReconcileEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtTotalPayableAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDTOTALPAYABLEAMOUNT"]));
            }
        }
    }

    protected void InvoiceAdjustmentAmountChanged(object sender, EventArgs e)
    {
        txtTotalPayableAmoutEdit.Text = Convert.ToString(decimal.Parse(ucInvoiceAmoutEdit.Text) - decimal.Parse(ucAdjustmentAmount.Text));
    }

    public bool IsValidInvoice()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtinvoicedate = null, dtreceiveddate = null;
        if (txtSupplierRefEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice reference is required";

        if (ddlCurrencyCode.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";

        if (ucInvoiceAmoutEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice amount is required.";

        if (txtExchangeRateEdit.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange rate is required.";

        if (txtInvoiceDateEdit.Text == null)
            ucError.ErrorMessage = "Invoice date is required.";
        else
            dtinvoicedate = DateTime.Parse(txtInvoiceDateEdit.Text);

        if (txtInvoiceReceivedDateEdit.Text == null)
            ucError.ErrorMessage = "Invoice received date is required.";
        else
            dtreceiveddate = DateTime.Parse(txtInvoiceReceivedDateEdit.Text);

        //if (txtVendorId.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Supplier code is required.";

        if (ddlInvoiceType.SelectedHard.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Invoice type is required.";

        if (dtreceiveddate < dtinvoicedate)
            ucError.ErrorMessage = "Received date should be later than invoice date.";

        if (ucAdjustmentAmount.Text.Trim() != string.Empty)
        {
            if (General.GetNullableDecimal(ucAdjustmentAmount.Text.Trim()) != null)
            {
                if (General.GetNullableDecimal(ucAdjustmentAmount.Text.Trim()) > General.GetNullableDecimal("0.0"))
                {
                    DataSet ds = new DataSet();
                    int iRowCount = 0, iTotalPageCount = 0;
                    ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(txtDTKey.Text), null, "Adjustment", null, null,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                General.ShowRecords(null),
                                                                                ref iRowCount, ref iTotalPageCount);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    {
                        ucError.ErrorMessage = "Adjustment attachment is mandatory to add invoice adjustment amount.";
                    }
                }
            }
        }
        //if (ViewState["INVOICEADJUSTMENTREMARKS"] != null && ViewState["INVOICEADJUSTMENTREMARKS"].ToString() != string.Empty)
        //{            
        //    if (!txtAdjustmentRemarks.Text.Contains(ViewState["INVOICEADJUSTMENTREMARKS"].ToString()))
        //        ucError.ErrorMessage = "Existsing comments can not be editable";
        //}

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void InvoiceClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMCODE"] = e.CommandArgument;
    }

    protected void lnkBankInformation_click(object sender, EventArgs e)
    {


    }
}
