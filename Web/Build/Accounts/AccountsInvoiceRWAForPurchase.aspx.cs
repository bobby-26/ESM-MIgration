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

public partial class AccountsInvoiceRWAForPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //txtVendorCode.Attributes.Add("onkeydown", "return false;");
        //txtVenderName.Attributes.Add("onkeydown", "return false;");
        cmdHiddenSubmit.Attributes.Add("style", "Display:None;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("New", "NEW");
        toolbar.AddButton("Notify", "NOTIFY", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.Title = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")  ";
        MenuInvoice1.MenuList = toolbar.Show();
        //MenuInvoice1.SetTrigger(pnlInvoice);

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

            DataTable dt = PhoenixRegistersVessel.ListAllVessel(1).Tables[0];
            chkVesselList.DataSource = dt;
            chkVesselList.DataBind();

            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
                //InvoiceEdit();
            }

        }

        if (ViewState["INVOICECODE"] != null)
        {
            // ttlInvoice.Text = "Invoice      (" + PhoenixAccountsVoucher.InvoiceNumber + ")     ";
        }
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        // ddlInvoiceType.SelectedHard = "239";
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
        //ttlInvoice.Text = "Invoice      ()";
        ddlPhysicalLocation.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
        ddlEarmarkedCompany.SelectedCompany = "";
        txtVesselNameList.Text = "";
        ucBankInfoPageNumber.Text = "";
        ddlLiabilitycompany.SelectedCompany = "";
        txtDispatchstatus.Text = "";
        txtPurchaseInvoiceVoucherNumber.Text = "";
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
        //decimal deNonDiscountableAmount = 0;
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

            foreach (ListItem item in chkVesselList.Items)
            {
                if (item.Selected)
                {
                    strvessellist = strvessellist + item.Value + ",";
                }
            }

            if (!IsValidInvoice(strvessellist))
            {
                ucError.Visible = true;
                return;
            }
            int PriorityInvoice = 0;
            if (chkPriorityInv.Checked == true)
            {
                PriorityInvoice = 1;
            }

            if (ViewState["INVOICECODE"] != null)
            {
                try
                {
                    PhoenixAccountsInvoice.InvoiceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), txtSupplierRefEdit.Text, int.Parse(ddlCurrencyCode.SelectedCurrency), decimal.Parse(ucInvoiceAmoutEdit.Text),
                        0, null, decimal.Parse(txtExchangeRateEdit.Text),
                        DateTime.Parse(txtInvoiceDateEdit.Text), DateTime.Parse(txtInvoiceReceivedDateEdit.Text),
                       General.GetNullableInteger(txtVendorId.Text), int.Parse(ddlInvoiceType.SelectedHard),
                        txtRemarks.Text, General.GetNullableInteger(ddlPhysicalLocation.SelectedCompany), null, string.Empty, General.GetNullableInteger(ddlEarmarkedCompany.SelectedCompany), General.GetNullableDecimal(txtInvoiceAdjustmentAmount.Text), string.Empty, General.GetNullableInteger(ucBankInfoPageNumber.Text), General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), PriorityInvoice
                        , strvessellist, General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), txtPort.Text);
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
            //Response.Redirect("../Accounts/AccountsInvoiceNotify.aspx?INVOICENUMBER=" + txtInvoiceNumber.Text + "&DTKEY=" + txtDTKey.Text + "&INVOICECODE=" + ViewState["INVOICECODE"].ToString());
            SendNotificationMail();
        }
    }

    public bool IsValidInvoice(String vessellist)
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

        //if (ddlInvoiceType.SelectedHard.Equals("1340"))
        //{
        //    if (txtETA.Text == null)
        //        ucError.ErrorMessage = "ETA is required.";
        //    if (txtETD.Text == null)
        //        ucError.ErrorMessage = "ETD is required.";
        //    if (General.GetNullableInteger(ucPort.SelectedSeaport) == null)
        //        ucError.ErrorMessage = "Port is required.";
        //}


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

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
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
                txtPurchaseInvoiceVoucherNumber.Text = drInvoice["FLDPURCHASEINVOICEVOUCHERNUMBER"].ToString();
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
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131,967', true); ");
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
                if (drInvoice["FLDPRIORITYINVOICE"].ToString() == "1")
                {
                    chkPriorityInv.Checked = true;
                }
                ucToolTipStatus.Text = drInvoice["FLDINVOICESTATUSCHANGEINFO"].ToString();
                if (txtStatus != null && drInvoice["FLDINVOICESTATUS"].ToString() == "242")
                {
                    txtStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'visible');");
                    txtStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'hidden');");
                }

                txtETA.Text = General.GetDateTimeToString(drInvoice["FLDETA"].ToString());
                txtETD.Text = General.GetDateTimeToString(drInvoice["FLDETD"].ToString());
                txtPort.Text = drInvoice["FLDPORTLIST"].ToString();

                if (chkVesselList != null)
                {
                    foreach (ListItem item in chkVesselList.Items)
                    {
                        item.Selected = false;
                        if (!string.IsNullOrEmpty(drInvoice["FLDVESSELLIST"].ToString()) && ("," + drInvoice["FLDVESSELLIST"].ToString() + ",").Contains("," + item.Value.ToString() + ","))
                            item.Selected = true;
                    }
                }
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
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
            }

            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "QTY"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=183', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
            }
            else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
            {
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true); ");
                txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                ddlLiabilitycompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.ToString();
            }
            else
            {
                ImgSupplierPickList.Attributes["onclick"] = null;
            }

        }
    }
    private void SendNotificationMail()
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
        DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
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
                    PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid);
                    ucStatus.Text = "Email sent successfully";
                    PhoenixAccountsInvoice.InvoiceNotifyDetailsUpdate(new Guid(ViewState["INVOICECODE"].ToString()), "", vessellist, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                }
            }
        }
        InvoiceEdit();
    }
}
