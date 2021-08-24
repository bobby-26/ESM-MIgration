using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsInvoiceLedgerPostingConfirmationcheck : PhoenixBasePage
{
    public string Message;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Check & Post", "POST", ToolBarDirection.Right);
            MenuLineItem.Title = "Check & Post Invoice";
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();

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

        DataSet dsAttachment = new DataSet();
        if (ViewState["invoicecode"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtckInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                string dtKey = drInvoice["FLDDTKEY"].ToString();
                ViewState["CURRENCY"] = drInvoice["FLDCURRENCY"].ToString();
                ViewState["INVOICEAMOUNT"] = drInvoice["FLDINVOICEAMOUNT"];
                BindAttachment(dtKey);
            }
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

                    return;
                }
                if (ViewState["invoicecode"] != null)
                {
                    DataSet dspo = new DataSet();
                    dspo = PhoenixAccountsInvoice.PoAdvanceStatusCheck(new Guid(ViewState["invoicecode"].ToString()));

                    if (dspo.Tables[0].Rows.Count > 0)
                    {
                        int n = dspo.Tables[0].Rows.Count;
                        while (n > 0)
                        {
                            DataRow dr = dspo.Tables[0].Rows[n - 1];
                            Message += dr["FLDADVANCENUMBER"].ToString() + " status is still " + dr["FLDSTATUS"].ToString() + ". To cancel, please contact Accounts Dept.<br>";
                            n--;
                        }
                        ucError.ErrorMessage = Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                if (ViewState["invoicecode"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = PhoenixAccountsInvoice.VoucherStatusCheck(new Guid(ViewState["invoicecode"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int n = ds.Tables[0].Rows.Count;
                        while (n > 0)

                        {
                            DataRow dr = ds.Tables[0].Rows[n - 1];
                            Message += dr["FLDVOUCHERNUMBER"].ToString() + "  for " + dr["FLDADVANCENUMBER"].ToString() + "  is not balanced. Please check  with Remittance Dept.<br>";
                            n--;
                        }
                        ucError.ErrorMessage = Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                if (ViewState["invoicecode"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = PhoenixAccountsInvoice.PoAdvancePaymentVoucherCheck(new Guid(ViewState["invoicecode"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int n = ds.Tables[0].Rows.Count;
                        while (n > 0)

                        {
                            DataRow dr = ds.Tables[0].Rows[n - 1];
                            Message += "No bank payment voucher is created for " + dr["FLDPONUMBER"].ToString() + "  . Please check  with Remittance Dept.<br>";
                            n--;
                        }
                        ucError.ErrorMessage = Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                if (ViewState["invoicecode"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = PhoenixAccountsInvoice.AdvancePaymentCompanyCheck(new Guid(ViewState["invoicecode"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int n = ds.Tables[0].Rows.Count;
                        while (n > 0)

                        {
                            DataRow dr = ds.Tables[0].Rows[n - 1];
                            Message += dr["FLDADVANCENUMBER"].ToString() + "  'Po number' was not paid under the same company. Please check 'bank payment voucher no'<br>";
                            n--;
                        }
                        ucError.ErrorMessage = Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                string strInvoiceCode = ViewState["invoicecode"].ToString();
                Response.Redirect("../Accounts/AccountsInvoiceLedgerPostingConfirmation.aspx?qinvoicecode=" + strInvoiceCode);
                ucStatus.Text = "Invoice Posted Successfully.";
                InvoiceEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidPost()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        Decimal? reginvoiceamount = General.GetNullableDecimal(ViewState["INVOICEAMOUNT"].ToString());
        Decimal? invoiceamount = General.GetNullableDecimal(ucInvoiceAmoutEdit.Text);
        if (ddlCurrencyCode.SelectedCurrency != ViewState["CURRENCY"].ToString())
            ucError.ErrorMessage = "Entered invoice currency is mismatching";
        if (reginvoiceamount != invoiceamount)
            ucError.ErrorMessage = "Entered invoice amount is mismatching";

        DataTable dt = PhoenixAccountsDebitNoteReferenceAttachments.InvoiceAttachmentTypeValidation(new Guid(ViewState["invoicecode"].ToString()));
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString() == "1")
            {
                ucError.ErrorMessage = "Non Standard PDF(s) are not allowed";
            }
        }
        return (!ucError.IsError);
    }
    protected void BindAttachment(string dtkey)
    {
        DataSet dsattachment = new DataSet();
        DataSet dsvoucherlevelattachment = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string Attachmenttype = "Invoice";
        dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtkey), null, Attachmenttype, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        if (dsattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drattachment = dsattachment.Tables[0].Rows[0];
            string filepath = drattachment["FLDFILEPATH"].ToString();
            ifMoreInfo.Attributes["src"] = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();//Session["sitepath"] + "/attachments/" + filepath;
                                                                                                                  //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
        else
        {

            ucError.ErrorMessage = "Invalid Invoice";
            ucError.Visible = true;
            //return;
        }
    }
}
