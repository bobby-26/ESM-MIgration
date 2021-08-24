using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //txtVendorCode.Attributes.Add("onkeydown", "return false;");
        //txtVenderName.Attributes.Add("onkeydown", "return false;");
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        //toolbar.AddButton("Notify", "NOTIFY", ToolBarDirection.Right);

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.Title="Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        MenuInvoice1.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("RePost", "REPOST", ToolBarDirection.Right);
        toolbar1.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        toolbar1.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
        toolbar1.AddButton("Lock", "LOCK", ToolBarDirection.Right);

        MenuInvoice2.AccessRights = this.ViewState;
        MenuInvoice2.MenuList = toolbar1.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoice.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','Add','AccountsInvoiceLineItem.aspx?qinvoicecode=" + Request.QueryString["QINVOICECODE"] + "'); return false;", "Add", "Add.png", "ADD");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            txtInvoiceReceivedDateEdit.Text = DateTime.Now.ToString();

            DataSet ds = PhoenixRegistersVessel.ListAllVessel(1);
            DataTable dt = ds.Tables[0];
            chkVesselList.DataSource = dt;
            chkVesselList.DataBind();
            ViewState["INVOICESTATUS"] = "";
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
                Filter.CurrentInvoiceCodeSelection = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
            }
        }

        //60              241         Awaiting PO Details                                                                                                                                                                                
        //60              242         Reconciliation of PO                                                                                                                                                                                 
        //60              243         Accounts Checking
        //60              632         Reconciled waiting for Approval                                                                                                                                                                    
        //60              872         Invoice Cancelled                                                                                                                                                                                    
        //60              939         Awaiting Scanned Invoice           
        //bug id 18412        
        if (ViewState["INVOICESTATUS"].ToString() != "")
        {
            if (ViewState["INVOICESTATUS"].ToString() == "241" || ViewState["INVOICESTATUS"].ToString() == "242" || ViewState["INVOICESTATUS"].ToString() == "243" || ViewState["INVOICESTATUS"].ToString() == "939" || ViewState["INVOICESTATUS"].ToString() == "872" || ViewState["INVOICESTATUS"].ToString() == "632")
            {
                toolbar.AddButton("Notify", "NOTIFY", ToolBarDirection.Right);
                MenuInvoice1.AccessRights = this.ViewState;
                MenuInvoice1.MenuList = toolbar.Show();
            }
        }
        //
        if (ViewState["INVOICECODE"] != null)
        {
            // ttlInvoice.Text = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        }
        txtVendorId.Attributes.Add("style", "visibility:hidden");

    }

    private void Reset()
    {
        ViewState["INVOICECODE"] = null;
        txtInvoiceNumber.Text = "";
        txtInvoiceDateEdit.Text = "";
        txtSupplierRefEdit.Text = "";
        txtInvoiceReceivedDateEdit.Text = "";
        ddlInvoiceType.SelectedHard = "";
        ddlCurrencyCode.SelectedCurrency = "";
        txtVendorId.Text = "";
        txtVendorCode.Text = "";
        txtVenderName.Text = "";
        txtExchangeRateEdit.Text = "";
        ucInvoiceAmoutEdit.Text = "";
        txtStatus.Text = "";
        txtRemarks.Text = "";
        imgAttachment.Visible = false;
        // ttlInvoice.Text = "Invoice      ()";
        ddlPhysicalLocation.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
        ddlEarmarkedCompany.SelectedCompany = "";
        txtVesselNameList.Text = "";
        ucBankInfoPageNumber.Text = "";
        ddlLiabilitycompany.SelectedCompany = "";
        txtDispatchstatus.Text = "";
        txtPurchaseInvoiceVoucherNumber.Text = "";
        txtPONumber.Text = "";
        chkPriorityInv.Checked = false;
        if (chkVesselList != null)
        {
            foreach (ListItem item in chkVesselList.Items)
            {
                item.Selected = false;
            }
        }

        ucPortMulti.SelectedValue = string.Empty;
        ucPortMulti.Text = string.Empty;
        txtETA.Text = "";
        txtETD.Text = "";
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?framename=ifMoreInfo', true); ");
    }

    protected void Invoice_SetExchangeRate(object sender, EventArgs e)
    {
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ddlCurrencyCode.SelectedCurrency));
            if (dsInvoice.Tables[0].Rows.Count > 0)
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
        decimal deNonDiscountableAmount = 0;
        string strInvoiceCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();

        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceList.aspx");
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string strvessellist = "";
            string strportlist = "";

            foreach (ListItem item in chkVesselList.Items)
            {
                if (item.Selected)
                {
                    strvessellist = strvessellist + item.Value + ",";
                }
            }
            strportlist = General.GetNullableInteger(ucPortMulti.SelectedValue).ToString();

            if (!IsValidInvoice(strvessellist, strportlist))
            {
                ucError.Visible = true;
                return;
            }

            int? PriorityInvoice = null;
            if (chkPriorityInv.Checked == true)
            {
                PriorityInvoice = 1;
            }

            if (ViewState["INVOICECODE"] == null)
            {
                try
                {
                    PhoenixAccountsInvoice.InvoiceInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSupplierRefEdit.Text, int.Parse(ddlCurrencyCode.SelectedCurrency), decimal.Parse(ucInvoiceAmoutEdit.Text),
                        deNonDiscountableAmount, null, decimal.Parse(txtExchangeRateEdit.Text),
                        DateTime.Parse(txtInvoiceDateEdit.Text), DateTime.Parse(txtInvoiceReceivedDateEdit.Text),
                        General.GetNullableInteger(txtVendorId.Text), int.Parse(ddlInvoiceType.SelectedHard), 939,
                        txtRemarks.Text, General.GetNullableInteger(ucBankInfoPageNumber.Text), strvessellist, General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), ref strInvoiceCode,
                        General.GetNullableInteger(ddlPhysicalLocation.SelectedCompany), PriorityInvoice, General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), strportlist);

                    ucStatus.Text = "Invoice information is added";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                Reset();
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                Session["New"] = "Y";
            }

            else
            {
                try
                {

                    PhoenixAccountsInvoice.InvoiceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), txtSupplierRefEdit.Text, int.Parse(ddlCurrencyCode.SelectedCurrency), decimal.Parse(ucInvoiceAmoutEdit.Text),
                        0, null, decimal.Parse(txtExchangeRateEdit.Text),
                        DateTime.Parse(txtInvoiceDateEdit.Text), DateTime.Parse(txtInvoiceReceivedDateEdit.Text),
                       General.GetNullableInteger(txtVendorId.Text), int.Parse(ddlInvoiceType.SelectedHard),
                        txtRemarks.Text, General.GetNullableInteger(ddlPhysicalLocation.SelectedCompany), General.GetNullableInteger(txtReasonHolding.Text), string.Empty, General.GetNullableInteger(ddlEarmarkedCompany.SelectedCompany), General.GetNullableDecimal(txtInvoiceAdjustmentAmount.Text), string.Empty, General.GetNullableInteger(ucBankInfoPageNumber.Text), General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), PriorityInvoice
                        , strvessellist, General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), strportlist);
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
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }

        if (CommandName.ToUpper().Equals("LOCK"))
        {
            PhoenixAccountsInvoice.InvoiceStatusReset(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()));
            ucStatus.Text = "Invoice information is updated";
            strInvoiceCode = ViewState["INVOICECODE"].ToString();
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        if (CommandName.ToUpper().Equals("UNLOCK"))
        {
            PhoenixAccountsInvoice.InvoiceChangetoAccountschecking(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()));
            ucStatus.Text = "Invoice information is updated";
            strInvoiceCode = ViewState["INVOICECODE"].ToString();
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        if (CommandName.ToUpper().Equals("CONFIRM"))
        {
            PhoenixAccountsInvoice.InvoiceUnlockstatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()));
            ucStatus.Text = "Invoice information is updated";
            strInvoiceCode = ViewState["INVOICECODE"].ToString();
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        if (CommandName.ToUpper().Equals("REPOST"))
        {
            try
            {

                PhoenixAccountsInvoice.GrantInvoiceToRepost(new Guid(ViewState["INVOICECODE"].ToString()));
                ucStatus.Text = "Invoice information is updated";
                strInvoiceCode = ViewState["INVOICECODE"].ToString();
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "qfrom=INVOICE");
        }
        if (CommandName.ToUpper().Equals("NOTIFY"))
        {
            //Response.Redirect("../Accounts/AccountsInvoiceNotify.aspx?INVOICENUMBER=" + txtInvoiceNumber.Text + "&DTKEY=" + txtDTKey.Text + "&INVOICECODE=" + ViewState["INVOICECODE"]);
            SendNotificationMail();
        }
    }

    public bool IsValidInvoice(String vessellist, String strportlist)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtinvoicedate = null, dtreceiveddate = null;
        if (txtSupplierRefEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice reference is required";

        if (ddlCurrencyCode.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";

        if (ucInvoiceAmoutEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice amount is required.";

        //if (txtExchangeRateEdit.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Exchange rate is required.";

        if (txtInvoiceDateEdit.Text == null)
            ucError.ErrorMessage = "Invoice date is required.";
        else
            dtinvoicedate = DateTime.Parse(txtInvoiceDateEdit.Text);

        if (txtInvoiceReceivedDateEdit.Text == null)
            ucError.ErrorMessage = "Invoice received date is required.";
        else
            dtreceiveddate = DateTime.Parse(txtInvoiceReceivedDateEdit.Text);

        if (ddlInvoiceType.SelectedHard.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Invoice type is required.";

        if (ddlInvoiceType.SelectedHard.Equals("1340"))
        {
            if (txtETA.Text == null)
                ucError.ErrorMessage = "ETA is required.";
            if (txtETD.Text == null)
                ucError.ErrorMessage = "ETD is required.";
            if (strportlist == "," || strportlist.ToUpper().Equals(""))
                ucError.ErrorMessage = "Port is required.";
            //if (General.GetNullableInteger(ucPort.SelectedSeaport)== null)
            //    ucError.ErrorMessage = "Port is required.";        
        }

        if (dtreceiveddate < dtinvoicedate)
            ucError.ErrorMessage = "Invoice received date should be later than invoice date.";

        if (ddlPhysicalLocation.SelectedCompany.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Invoice physical location is required.";

        if (dtinvoicedate > DateTime.Today)
            ucError.ErrorMessage = "Invoice date should not be the future date.";

        if (dtreceiveddate > DateTime.Today)
            ucError.ErrorMessage = "Invoice received date should not be the future date.";

        if (vessellist == "" || vessellist == null)
        {
            ucError.ErrorMessage = "Vessels involved is required";
        }

        return (!ucError.IsError);
    }


    public bool IsValidInvoiceLineItem(string strAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strAmount == string.Empty)
            ucError.ErrorMessage = "Invoice payable amount is required";
        return (!ucError.IsError);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void InvoiceEdit()
    {
        if (ViewState["INVOICECODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceRegisterEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {

                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                PhoenixAccountsVoucher.InvoiceNumber = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString());
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
                txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
                txtVenderName.Text = drInvoice["FLDNAME"].ToString();
                txtInvoiceReceivedDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICERECEIVEDDATE"].ToString());
                ddlInvoiceType.SelectedHard = drInvoice["FLDINVOICETYPE"].ToString();
                ddlCurrencyCode.SelectedCurrency = drInvoice["FLDCURRENCY"].ToString();
                txtExchangeRateEdit.Text = drInvoice["FLDEXCHANGERATE"].ToString();
                ucInvoiceAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]));
                txtVesselNameList.Text = drInvoice["FLDVESSELNAMELIST"].ToString();
                ddlPhysicalLocation.SelectedCompany = drInvoice["FLDPHYSICALLOCATION"].ToString();
                ddlEarmarkedCompany.SelectedCompany = drInvoice["FLDEARMARKEDCOMPANY"].ToString();
                txtRemarks.Text = drInvoice["FLDREMARKS"].ToString();
                txtStatus.Text = drInvoice["FLDINVOICESTATUSNAME"].ToString();
                ucBankInfoPageNumber.Text = drInvoice["FLDINVOICEBANKINFOPAGENUMBER"].ToString();
                txtDTKey.Text = drInvoice["FLDDTKEY"].ToString();
                ddlLiabilitycompany.SelectedCompany = drInvoice["FLDLIABILITYCOMPANY"].ToString();
                txtDispatchstatus.Text = drInvoice["FLDDISPATCHSTATUS"].ToString();
                txtInvoiceAdjustmentAmount.Text = drInvoice["FLDINVOICEADJUSTMENTAMOUNT"].ToString();
                txtPurchaseInvoiceVoucherNumber.Text = drInvoice["FLDACCOUNTVOUCHERNUMBERS"].ToString();
                lblBillToCompanyName.Text = drInvoice["FLDLIABILITYCOMPANYNAME"].ToString();
                txtReasonHolding.Text = drInvoice["FLDREASONSFORHOLDING"].ToString();
                ViewState["INVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"];
                if (chkVesselList != null)
                {
                    foreach (ListItem item in chkVesselList.Items)
                    {
                        item.Selected = false;
                        if (!string.IsNullOrEmpty(drInvoice["FLDVESSELLIST"].ToString()) && ("," + drInvoice["FLDVESSELLIST"].ToString() + ",").Contains("," + item.Value.ToString() + ","))
                            item.Selected = true;
                    }
                }

                //btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=131&framename=ifMoreInfo', true); ");

                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?framename=ifMoreInfo', true); ");


                ucPortMulti.SelectedValue = drInvoice["FLDPORTLIST"].ToString();
                ucPortMulti.Text = drInvoice["FLDSEAPORTNAME"].ToString();

                //if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "SPI"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131', true); ");
                //}
                //else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "QTY"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131', true); ");
                //}
                //else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx?addresstype=183', true); ");
                //}
                //else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD")
                //|| drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx?addresstype=132', true); ");
                //}
                //else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx?addresstype=135', true); ");
                //}
                //else if (drInvoice["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "BNP"))
                //{
                //    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListBondAndProvisionAddress.aspx', true); ");
                //}
                //else
                //{
                //    ImgSupplierPickList.Attributes["onclick"] = null;
                //}

                if (drInvoice["FLDMISMATCHYN"].ToString() == "1")
                {
                    lblMismatch.Visible = true;
                }
                if (drInvoice["FLDVENDORINVOICENUMBERALREADYEXISTS"].ToString() == "1")
                {
                    HlinkRefDuplicate.NavigateUrl = "#";
                    HlinkRefDuplicate.Text = "Possible Duplicate Invoices exist for this Supplier. Click here to view the Invoice List";
                    HlinkRefDuplicate.Visible = true;
                    HlinkRefDuplicate.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsVendorInvoiceDuplicateList.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "');return false;");
                }

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
                txtETA.Text = General.GetDateTimeToString(drInvoice["FLDETA"].ToString());
                txtETD.Text = General.GetDateTimeToString(drInvoice["FLDETD"].ToString());
                ucPortMulti.SelectedValue = drInvoice["FLDPORTLIST"].ToString();
                ucPortMulti.Text = drInvoice["FLDSEAPORTNAME"].ToString();
                //if (chkPortList != null)
                //{
                //    foreach (ListItem item in chkPortList.Items)
                //    {
                //        item.Selected = false;
                //        if (!string.IsNullOrEmpty(drInvoice["FLDPORTLIST"].ToString()) && ("," + drInvoice["FLDPORTLIST"].ToString() + ",").Contains("," + item.Value.ToString() + ","))
                //            item.Selected = true;
                //    }
                //}   



            }
        }
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void InvoiceClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMCODE"] = e.CommandArgument;
    }

    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoiceType.SelectedHard.ToUpper() != "DUMMY")
        {

            DataSet ds1 = PhoenixAccountsInvoice.InvoiceTypeEdit(int.Parse(ddlInvoiceType.SelectedHard.ToUpper()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = ds1.Tables[0].Rows[0];
                ddlLiabilitycompany.SelectedCompany = drInvoice["FLDCOMPANYID"].ToString();
            }
            else
            {
                ddlLiabilitycompany.SelectedCompany = "";
            }

            if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "SPI"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ucInvoiceAmoutEdit.Focus();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }

            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "QTY"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ucInvoiceAmoutEdit.Focus();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=183&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ucInvoiceAmoutEdit.Focus();
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                {
                    DataSet dsCurrency = PhoenixRegistersCurrency.ListCurrency(1, "INR");
                    if (dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"] != null && dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString() != "")
                    {
                        ddlCurrencyCode.SelectedCurrency = dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                        {
                            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString()));
                            if (dsInvoice.Tables.Count > 0)
                            {
                                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                                txtExchangeRateEdit.Text = string.Format(String.Format("{0:#####.000000}", drInvoice["FLDEXCHANGERATE"]));
                                ucInvoiceAmoutEdit.Focus();
                            }
                        }
                    }
                }
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=132&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=132&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=135&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "BNP"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListBondAndProvisionAddress.aspx?framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "CRW"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                //ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AGY"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=130,131&framename=ifMoreInfo', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                //ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
                if (ViewState["INVOICECODE"] == null)
                    ddlCurrencyCode.SelectedCurrency = "";
            }
            else
            {
                ImgSupplierPickList.Attributes["onclick"] = null;
            }

        }
    }

    private void SendNotificationMail()
    {
        try
        {
            string to, cc, bcc, mailsubject, mailbody, sessionid = string.Empty;
            bool htmlbody = true;
            MailPriority priority = MailPriority.Normal;
            string vessellist = "";
            string vesselnamelist = "";
            foreach (ListItem item in chkVesselList.Items)
            {
                if (item.Selected)
                {
                    vessellist = vessellist + item.Value + ",";
                    vesselnamelist = vesselnamelist + item.Text + ",";
                }
            }
            vesselnamelist = vesselnamelist.TrimEnd(',');
            if (vessellist.ToString() == "")
            {
                ucError.ErrorMessage = "Please select atleast one vessel";
                ucError.Visible = true;
                return;
            }
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceRegisterEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                mailbody = "Dear Sir/Madam, " + "<br /><br />";
                mailbody = mailbody + "<table><tr><td>Invoice Number </td><td>: </td><td> " + drInvoice["FLDINVOICENUMBER"].ToString()
                    + "</td><td> Invoice Status</td><td>: </td><td>" + drInvoice["FLDINVOICESTATUSNAME"].ToString()
                    + "</td></tr><tr><td>Supplier </td><td>: </td><td> " + drInvoice["FLDCODE"].ToString() + "-" + drInvoice["FLDNAME"].ToString()
                    + "</td><td> Vendor Invoice Number</td><td>: </td><td>" + drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString()
                    + "</td></tr><tr><td>Invoice Date  </td><td>: </td><td> " + General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString())
                    + "</td><td> Date Received </td><td>: </td><td>" + General.GetDateTimeToString(drInvoice["FLDINVOICERECEIVEDDATE"].ToString())
                    + "</td></tr><tr><td>Amount </td><td>: </td><td> " + string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]))
                    + "</td><td> Remarks</td><td>: </td><td>" + drInvoice["FLDREMARKS"].ToString()
                    + "</td></tr></table>";

                mailbody = mailbody + "<br />";
                mailbody = mailbody + "<table><tr><td> Vessels Involved : </td><td><tr><td>" + vesselnamelist + "</td></tr></table>";
                mailbody = mailbody + "<br /><br />";
                mailbody = mailbody + "<table><tr><td>Regards</td></tr><tr><td>" + PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString() + "</td></tr></table>";

                DataTable dte = PhoenixAccountsInvoice.VesselEmailForInvoice(General.GetNullableString(vessellist), General.GetNullableGuid(ViewState["INVOICECODE"].ToString()));
                if (dte.Rows.Count > 0)
                {
                    if (dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"] != null && !string.IsNullOrEmpty(dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"].ToString()))
                    {
                        to = dte.Rows[0]["FLDPURCHASERANDSUPDTEMAIL"].ToString();
                        cc = dte.Rows[0]["FLDTECHSUPDTANDFLEETMGREMAIL"] != null ? dte.Rows[0]["FLDTECHSUPDTANDFLEETMGREMAIL"].ToString() : "";
                        bcc = "";
                        mailsubject = "Notification for the Invoice received without PO Number and registered same.";
                        PhoenixAccountsInvoice.InvoiceNotifyDetailsUpdate(new Guid(ViewState["INVOICECODE"].ToString()), "", vessellist, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid);
                        ucStatus.Text = "Email sent successfully";
                    }
                }
            }
            InvoiceEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    //private void BindPortList()
    //{
    //    DataSet ds = PhoenixRegistersSeaport.ListSeaport();
    //    chkPortList.Items.Add("select");
    //    chkPortList.DataSource = ds;
    //    chkPortList.DataTextField = "FLDSEAPORTNAME";
    //    chkPortList.DataValueField = "FLDSEAPORTID";
    //    chkPortList.DataBind();
    //    chkPortList.Items.Insert(0, new ListItem("--Select--", ""));
    //}
}
