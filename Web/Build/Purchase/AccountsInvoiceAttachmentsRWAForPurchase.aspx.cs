using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsInvoiceAttachmentsRWAForPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["invoicecode"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
            }
            if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
            {
                ViewState["ReceivedFrom"] = Request.QueryString["qfrom"];
            }
            if (Request.QueryString["attachmenttype"] != null && Request.QueryString["attachmenttype"] != string.Empty)
            {
                ViewState["ATTCHMENTTYPE"] = Request.QueryString["attachmenttype"].ToString();
                rblAttachmentType.Enabled = false;
            }
            else
                ViewState["ATTCHMENTTYPE"] = "Invoice";
            BindHard();
            RemarksEdit();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ReceivedFrom"].ToString() == "PR")
        {
            toolbar.AddButton("Invoice", "POSTINVOICE");
            toolbar.AddButton("PO", "POPR");
        }
        else if (ViewState["ReceivedFrom"].ToString().ToUpper() == "INVOICE")
        {
            toolbar.AddButton("Invoice", "INVOICE");
            toolbar.AddButton("PO", "PO");
           // toolbar.AddButton("Direct PO", "DPO");
        }

        else if (ViewState["ReceivedFrom"].ToString().ToUpper() == "INVOICEFORPURCHASE")
        {
            toolbar.AddButton("Invoice", "INVOICEFORPURCHASE");
            toolbar.AddButton("PO", "PO");
            toolbar.AddButton("Direct PO", "DPO");
        }

        else if (ViewState["ReceivedFrom"].ToString() == "AD")
        {
            toolbar.AddButton("Invoice", "INVOICEAD");
            toolbar.AddButton("PO", "POAD");
        }

        else if (ViewState["ReceivedFrom"].ToString().ToUpper() == "PURCHASEINVOICE")
        {
            toolbar.AddButton("Voucher", "VOUCHER");
            toolbar.AddButton("Line Items", "VLINEITEM");
        }
        toolbar.AddButton("Attachments", "ATTACHMENTS");

        if (!ViewState["ReceivedFrom"].ToString().ToUpper().Equals("PURCHASE"))
        {
            toolbar.AddButton("History", "HISTORY");
        }

        MenuInvoice1.AccessRights = this.ViewState;
        if (ViewState["ReceivedFrom"].ToString() != "PV")
        {
            MenuInvoice1.MenuList = toolbar.Show();
        }

        if (ViewState["ReceivedFrom"].ToString() == "PV")
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=ACCOUNTSPV" + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1";
        }

        if (int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) < 371 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 632 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 872 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 939 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 1586)
        {
            if (ViewState["PURCHASEINVOICEDTKEY"].ToString() != "" && ViewState["ReceivedFrom"].ToString().ToUpper() == "PURCHASEINVOICE")
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&pvdtkey=" + ViewState["PURCHASEINVOICEDTKEY"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
            else
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&mimetype=.pdf";
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1&mimetype=.pdf";
        }

        if (ViewState["ReceivedFrom"].ToString() == "PURCHASE")
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1";
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMain.MenuList = toolbarmain.Show();

    }

    protected void BindHard()
    {
        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        if (showcreditnotedisc == 1)
            rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 93, 0, "INV,CDT,MST,OWA,SPD,ADT");
        else
            rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 93, 0, "INV,MST,OWA,SPD,ADT"); //to hide credit note disc 12601
                                                                                                                                                                // rblAttachmentType.DataTextField = "FLDHARDNAME";
                                                                                                                                                                // rblAttachmentType.DataValueField = "FLDHARDCODE";
        rblAttachmentType.DataBind();
        if (showcreditnotedisc == 1)
        {
            if (ViewState["ATTCHMENTTYPE"].ToString() == "Credit Notes")
                rblAttachmentType.SelectedIndex = 1;
            else
                rblAttachmentType.SelectedIndex = 2;
        }
        else
            rblAttachmentType.SelectedIndex = 1;


    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex != -1)
            ViewState["ATTCHMENTTYPE"] = rblAttachmentType.Items[rblAttachmentType.SelectedIndex].Text.Trim();


        if (ViewState["ReceivedFrom"].ToString() == "PV")
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=ACCOUNTSPV" + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1";
        }
        if (int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) < 371 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 632 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 872 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 939 || int.Parse(ViewState["FLDINVOICESTATUS"].ToString()) == 1586)
        {
            if (rblAttachmentType.SelectedValue == "540")
            {
                if (ViewState["adjustmentamount"].ToString() != ".00")
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&adjustmentamount=" + ViewState["adjustmentamount"] + "&U=D";
                else
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();

            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&mimetype=.pdf";
            }
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1&mimetype=.pdf";
        }

        if (ViewState["ReceivedFrom"].ToString() == "PURCHASE")
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1";
        }
        //if (ViewState["ReceivedFrom"].ToString() != "PV")
        //{
        //    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=ACCOUNTSPV" + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
        //}
        //ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString(); 

        RemarksEdit();
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
        {
            Response.Redirect("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["ReceivedFrom"]);
        }
        if (CommandName.ToUpper().Equals("DPO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
        {
            Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["ReceivedFrom"]);
        }
        if (CommandName.ToUpper().Equals("PO"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
        else if (CommandName.ToUpper().Equals("INVOICE"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
        else if (CommandName.ToUpper().Equals("INVOICEFORPURCHASE"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceMasterForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
        else if (CommandName.ToUpper().Equals("POSTINVOICE"))
        {
            Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
        if (CommandName.ToUpper().Equals("POPR"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["ReceivedFrom"]);
        }
        else if (CommandName.ToUpper().Equals("INVOICEAD"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
        else if (CommandName.ToUpper().Equals("POAD"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["ReceivedFrom"]);
        }
        else if (CommandName.ToUpper().Equals("VOUCHER"))
        {
            Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherMaster.aspx?voucherid=" + ViewState["VOUCHERID"]);
        }
        else if (CommandName.ToUpper().Equals("VLINEITEM"))
        {
            Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["VOUCHERID"]);
        }
        else if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceAttachmentsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["ReceivedFrom"]);
        }
        else if (CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../Purchase/AccountsInvoiceHistoryRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["ReceivedFrom"], false);
        }
    }

    protected void InvoiceEdit()
    {
        if (ViewState["invoicecode"] != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                ViewState["FLDINVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"].ToString();
                ViewState["DTKey"] = drInvoice["FLDDTKEY"].ToString();
                ViewState["VOUCHERID"] = drInvoice["FLDVOUCHERID"].ToString();
                ViewState["PURCHASEINVOICEDTKEY"] = drInvoice["FLDPURCHASEINVOICEDTKEY"].ToString();
                ViewState["adjustmentamount"] = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEADJUSTMENTAMOUNT"]));
            }
        }
    }
    protected void RemarksEdit()
    {
        if (ViewState["invoicecode"] != null)
        {
            DataSet dsRemarks = PhoenixAccountsInvoice.AttachmentRemarksEdit(new Guid(ViewState["invoicecode"].ToString()), ViewState["ATTCHMENTTYPE"].ToString());
            if (dsRemarks.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsRemarks.Tables[0].Rows[0];
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
            }
            else
                txtRemarks.Text = string.Empty;
        }
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
        {
            PhoenixAccountsInvoice.AttachmentRemarksInsert(new Guid(ViewState["invoicecode"].ToString())
                                        , ViewState["ATTCHMENTTYPE"].ToString()
                                        , txtRemarks.Text);
        }
        RemarksEdit();
    }

}
