using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsPostInvoice : PhoenixBasePage
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
    public decimal dPayableamount;


    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtVendorCode.Attributes.Add("onkeydown", "return false;");
        txtVenderName.Attributes.Add("onkeydown", "return false;");
        txtAccountNo.Attributes.Add("onkeydown", "return false;");
        txtBankID.Attributes.Add("style", "visibility:hidden;");
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbar.AddButton("Notify", "NOTIFY");   

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.Title = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        MenuInvoice1.MenuList = toolbar.Show();
        if (ViewState["BankInformationAvailable"] == null)
            ViewState["BankInformationAvailable"] = 0;
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsPostInvoice.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','Add','AccountsInvoiceLineItem.aspx?qinvoicecode=" + Request.QueryString["QINVOICECODE"] + "'); return false;", "Add", "Add.png", "ADD");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["INVOICECURRENCY"] = null;

            gvOrderLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindServiceTax();
            BindTDS();
            BindWCT();
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
            }
            else
            {
                lblBankInformation.Visible = false;
                lnkBankInformation.Visible = false;
            }

            if (ViewState["FLDINVOICESTATUS"] != null)
            {
                if (int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) < 371)
                {
                    imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                    + PhoenixModule.ACCOUNTS + "&type=Adjustment" + "'); return false;");
                }
                else
                {
                    imgAdjustmentAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + txtDTKey.Text + "&mod="
                    + PhoenixModule.ACCOUNTS + "&type=Adjustment" + "&U=1" + "'); return false;");
                }
            }

        }
        if (ViewState["INVOICECODE"] != null)
        {
            //ttlInvoice.Text = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        }
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        //        ddlInvoiceType.SelectedHard = "239";
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
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                DataSet ds = PhoenixAccountsPOStaging.InvoiceStagingSearch(new Guid(ViewState["INVOICECODE"].ToString()));
                gvOrderLine.DataSource = ds;
                gvOrderLine.VirtualItemCount = iRowCount;
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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

                    // if (decimal.Parse(drInvoice["FLDPAYABLEAMOUNT"].ToString()) > 0)  -- 27704
                    {
                        strPayableAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDPAYABLEAMOUNT"]));
                        dPayableamount = decimal.Parse(drInvoice["FLDPAYABLEAMOUNT"].ToString());
                    }
                    //else
                    //{
                    //    strPayableAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDINVOICEPAYABLEAMOUNT"]));
                    //}

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
                    if (decimal.Parse(drInvoice["FLDGSTFORVESSEL"].ToString()) > 0)
                    {
                        strGSTForVessel = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTFORVESSEL"]));
                        strTotalIncomeAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTFORVESSEL"]));
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
                        strGstAmount = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTAMOUNT"]));
                    }
                    else
                    {
                        strGstAmount = string.Format(String.Format("{0:0.00}", drInvoice["FLDGSTAMOUNT"]));
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
                    if (drInvoice["FLDVENDORINVOICENUMBERALREADYEXISTS"] != null && drInvoice["FLDVENDORINVOICENUMBERALREADYEXISTS"].ToString() == "1")
                    {
                        HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsVendorInvoiceDuplicateList.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString();
                        HlinkRefDuplicate.Visible = true;
                    }
                }
                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                gvOrderLine.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;
                gvOrderLine.Columns[5].Visible = (showcreditnotedisc == 1) ? true : false;
                gvOrderLine.Columns[6].Visible = (showcreditnotedisc == 1) ? true : false;


                General.SetPrintOptions("gvOrderLine", "Vendor Item List", alCaptions, alColumns, ds);
            }
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

    private void Reset()
    {
        ViewState["INVOICECODE"] = null;
        txtInvoiceNumber.Text = "";
        txtSupplierRefEdit.Text = "";
        txtVendorId.Text = "";
        txtVendorCode.Text = "";
        txtVenderName.Text = "";
        txtInvoiceAmoutEdit.Text = "";
        txtRemarks.Text = "";
        // txtTotalDiscountAmount.Text = "";
        //ttlInvoice.Text = "Invoice      ()";
    }


    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {

        string strInvoiceCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

            if (ViewState["INVOICECODE"] == null)
            {

            }
            else
            {
                try
                {
                    PhoenixAccountsInvoice.InvoicePostUpdate(new Guid(ViewState["INVOICECODE"].ToString())
                        , txtRemarks.Text
                        , General.GetNullableInteger(ucBankInfoPageNumber.Text)
                        , General.GetNullableInteger(txtBankID.Text)
                        , Convert.ToInt32(ViewState["BankInformationAvailable"])
                        , PriorityInvoice
                        , General.GetNullableInteger(chkDelayedUtilization.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkServiceTax.Checked == true ? "1" : "0")
                        , General.GetNullableGuid(ddlServiceTaxType.SelectedValue)
                        , General.GetNullableDecimal(txtServiTax.Text)
                        , General.GetNullableInteger(chkTDS.Checked == true ? "1" : "0")
                        , General.GetNullableGuid(ddlTDSType.SelectedValue)
                        , General.GetNullableDecimal(txtTDSRate.Text)
                        , General.GetNullableGuid(ddlWCTType.SelectedValue)
                        , General.GetNullableDecimal(txtWCTRate.Text)
                        , General.GetNullableInteger(txtVendorId.Text)
                        );

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
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString());
        }
    }
    protected void Rebind()
    {
        gvOrderLine.SelectedIndexes.Clear();
        gvOrderLine.EditIndexes.Clear();
        gvOrderLine.DataSource = null;
        gvOrderLine.Rebind();
    }
    public bool IsValidInvoice()
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (txtAccountNo.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Bank information is required.";
        if (txtVendorId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier is required.";

        if (chkBankInformationAvailable.Checked == true)
            ViewState["BankInformationAvailable"] = 2;

        if (ViewState["BankInformationAvailable"].ToString() == "0" && ViewState["ISATTACHMENTAVAILABLE"].ToString() == "0")
            ucError.ErrorMessage = "Attachment is required otherwise select bank information not available option";

        if (chkBankInformationAvailable.Checked == false && ucBankInfoPageNumber.Text == "")
            ucError.ErrorMessage = "Bank Information Page is required otherwise select bank information not available option";

        return (!ucError.IsError);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void InvoiceEdit()
    {
        //ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet dsattachment = new DataSet();
        if (ViewState["INVOICECODE"] != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoicePostEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
                ViewState["VendorId"] = drInvoice["FLDADDRESSCODE"].ToString();
                txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
                txtVenderName.Text = drInvoice["FLDNAME"].ToString();
                ViewState["INVOICECURRENCY"] = drInvoice["FLDCURRENCY"].ToString();
                txtCurrencyCode.Text = drInvoice["FLDCURRENCYCODE"].ToString();

                txtInvoiceAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]));
                txtAdjustmentAmount.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEADJUSTMENTAMOUNT"]));

                txtRemarks.Text = drInvoice["FLDREMARKS"].ToString();

                txtDTKey.Text = drInvoice["FLDDTKEY"].ToString();
                ucBankInfoPageNumber.Text = drInvoice["FLDINVOICEBANKINFOPAGENUMBER"].ToString();
                txtBankID.Text = drInvoice["FLDBANKID"].ToString();
                txtBankName.Text = drInvoice["FLDBANKNAME"].ToString();
                txtAccountNo.Text = drInvoice["FLDACCOUNTNUMBER"].ToString();
                txtGST.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDGSTPERCENTAGE"]));
                imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtVendorId.Text + "&currency=" + ViewState["INVOICECURRENCY"] + "', true);");
                ViewState["FLDINVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"].ToString();
                if (drInvoice["FLDISBANKINFORMATIONAVAILABLE"] != null && drInvoice["FLDISBANKINFORMATIONAVAILABLE"].ToString() != string.Empty)
                {
                    chkBankInformationAvailable.Checked = drInvoice["FLDISBANKINFORMATIONAVAILABLE"].ToString() == "2" ? true : false;
                }
                else
                    ViewState["BankInformationAvailable"] = 0;
                ViewState["ISATTACHMENTAVAILABLE"] = 0;
                dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(txtDTKey.Text), null, "invoice", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                if (dsattachment.Tables[0].Rows.Count > 0)
                {
                    DataRow drattachment = dsattachment.Tables[0].Rows[0];
                    string filepath = drattachment["FLDFILEPATH"].ToString();
                    lnkBankInformation.NavigateUrl = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();//Session["sitepath"] + "/attachments/" + filepath + "#page=" + ucBankInfoPageNumber.Text;
                    //lnkBankInformation.NavigateUrl = Session["sitepath"] + "/attachments/" + filepath + "#page=" + ucBankInfoPageNumber.Text;
                    lblBankInformation.Visible = true;
                    lnkBankInformation.Visible = true;
                    ViewState["BankInformationAvailable"] = 1;
                    ViewState["ISATTACHMENTAVAILABLE"] = 1;
                    //chkBankInformationAvailable.Visible = false;
                }
                if (drInvoice["FLDPRIORITYINVOICE"].ToString() == "1")
                {
                    chkPriorityInv.Checked = true;
                }

                if (drInvoice["FLDISDELAYEDUTILIZATION"].ToString() == "1")
                {
                    chkDelayedUtilization.Checked = true;
                }
                chkServiceTax.Checked = drInvoice["FLDSERVICETAXYN"].ToString() == "1" ? true : false;
                ddlServiceTaxType.SelectedValue = drInvoice["FLDSERVICETAXTYPE"].ToString();
                ddlServiceTaxType.Enabled = drInvoice["FLDSERVICETAXYN"].ToString() == "1" ? true : false;

                txtServiTax.Text = drInvoice["FLDSERVICETAXRATE"].ToString();
                chkTDS.Checked = drInvoice["FLDTDSYN"].ToString() == "1" ? true : false;
                ddlTDSType.SelectedValue = drInvoice["FLDTDSTYPE"].ToString();
                ddlTDSType.Enabled = drInvoice["FLDTDSYN"].ToString() == "1" ? true : false;
                txtTDSRate.Text = drInvoice["FLDTDSRATE"].ToString();
                ddlWCTType.SelectedValue = drInvoice["FLDWCTTYPE"].ToString();
                txtWCTRate.Text = drInvoice["FLDWCTRATE"].ToString();
                ddlWCTType.Enabled = ddlTDSType.SelectedItem.Text.ToUpper() == "194C" ? true : false;
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?framename=ifMoreInfo', true); ");
            }
        }
        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblDelayedUtilization.Visible = (showcreditnotedisc == 1) ? true : false;
        chkDelayedUtilization.Visible = (showcreditnotedisc == 1) ? true : false;
        spnCreditNoteDisc.Visible = (showcreditnotedisc == 1) ? true : false;
        Table1.Visible = (showcreditnotedisc == 1) ? true : false;
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void InvoiceClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMCODE"] = e.CommandArgument;
    }

    protected void chkBankInformationAvailable_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkBankInformationAvailable.Checked == true)
        //    ViewState["BankInformationAvailable"] = 2;     
    }
    protected void chkServiceTax_Changed(object sender, EventArgs e)
    {
        if (chkServiceTax.Checked)
            ddlServiceTaxType.Enabled = true;
        else
            ddlServiceTaxType.Enabled = false;

        ddlServiceTaxType.SelectedValue = "Dummy";
        txtServiTax.Text = "";

    }
    protected void chkTDS_Changed(object sender, EventArgs e)
    {
        if (chkTDS.Checked)
            ddlTDSType.Enabled = true;
        else
            ddlTDSType.Enabled = false;

        ddlTDSType.SelectedValue = "Dummy";
        txtTDSRate.Text = "";
        ddlWCTType.SelectedValue = "Dummy";
        txtWCTRate.Text = "";
        ddlWCTType.Enabled = false;
    }
    protected void BindServiceTax()
    {
        int irowcount = 0;
        int itotalpagecount = 0;

        ddlServiceTaxType.DataTextField = "FLDSERVICETAXPAYMENTTYPE";
        ddlServiceTaxType.DataValueField = "FLDSERVICETAXID";

        ddlServiceTaxType.DataSource = PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterList(1, 1000, ref irowcount, ref itotalpagecount);
        ddlServiceTaxType.DataBind();
        ddlServiceTaxType.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }
    protected void BindTDS()
    {
        int irowcount = 0;
        int itotalpagecount = 0;

        ddlTDSType.DataTextField = "FLDSECTIONCODE";
        ddlTDSType.DataValueField = "FLDTDSPAYMENTID";

        ddlTDSType.DataSource = PhoenixAccountsTDSRegister.TDSRegisterList(1, 1000, ref irowcount, ref itotalpagecount);
        ddlTDSType.DataBind();
        ddlTDSType.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }
    protected void BindWCT()
    {
        int irowcount = 0;
        int itotalpagecount = 0;

        ddlWCTType.DataTextField = "FLDPAYMENTTYPE";
        ddlWCTType.DataValueField = "FLDWCTPAYMENTID";

        ddlWCTType.DataSource = PhoenixAccountsWCTRegister.WCTRegisterList(1, 1000, ref irowcount, ref itotalpagecount);
        ddlWCTType.DataBind();
        ddlWCTType.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }
    protected void ddlTDSType_TextChanged(object sender, EventArgs e)
    {

        if (ddlTDSType.SelectedItem.Text.ToUpper() == "194C")
            ddlWCTType.Enabled = true;
        else
            ddlWCTType.Enabled = false;

        DataTable dt = PhoenixAccountsTDSCalculations.GetTDSRate(General.GetNullableGuid(ddlTDSType.SelectedValue), General.GetNullableInteger(txtVendorId.Text));
        if (dt.Rows.Count > 0)
        {
            txtTDSRate.Text = dt.Rows[0]["FLDTDSRATE"].ToString();
        }
        else
            txtTDSRate.Text = "";

        ddlWCTType.SelectedValue = "Dummy";
        txtWCTRate.Text = "";
    }
    protected void ddlServiceTaxType_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = PhoenixAccountsTDSCalculations.GetServiceTax(General.GetNullableGuid(ddlServiceTaxType.SelectedValue));
        if (dt.Rows.Count > 0)
        {
            txtServiTax.Text = dt.Rows[0]["FLDSERVICETAX"].ToString();
        }
        else
            txtServiTax.Text = "";
    }
    protected void ddlWCTType_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = PhoenixAccountsTDSCalculations.GetWCTRate(General.GetNullableGuid(ddlWCTType.SelectedValue), General.GetNullableInteger(txtVendorId.Text));
        if (dt.Rows.Count > 0)
        {
            txtWCTRate.Text = dt.Rows[0]["FLDWCTRATE"].ToString();
        }
        else
            txtWCTRate.Text = "";
    }

    public void txtVendorCodeChanged()
    {

        if (Convert.ToString(ViewState["VendorId"]) != txtVendorId.Text)
        {
            ucBankInfoPageNumber.Text = "";
            txtAccountNo.Text = "";
            txtBankName.Text = "";
            txtBankID.Text = "";
            chkBankInformationAvailable.Checked = false;

        }
        imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtVendorId.Text + "&currency=" + ViewState["INVOICECURRENCY"] + "', true);");
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        txtVendorCodeChanged();
        ViewState["VendorId"] = txtVendorId.Text;


    }
}
