using System;
using System.Data;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsRejectedInvoiceRemarks : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
            {
                PaymentVoucherEdit();
            }
        }
        base.Render(writer);
    }    

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);       

        if (!IsPostBack)
        {            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            toolbar.AddButton("Cancel", "CANCEL");
            MenuInvoiceRemarks.AccessRights = this.ViewState;
            MenuInvoiceRemarks.MenuList = toolbar.Show();

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            if (Request.QueryString["invoicecode"] != null && Request.QueryString["invoicecode"] != string.Empty)
                ViewState["invoicecode"] = Request.QueryString["invoicecode"];
            PaymentVoucherEdit();
            InvoiceEdit();
        }
    }

    private void PaymentVoucherEdit()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtPaymentVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();                    
                }
            }
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
                ViewState["DTKEY"] = drInvoice["FLDDTKEY"].ToString();
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();                    
            }
        }
    }

    protected void MenuInvoiceRemarks_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != "")
                {
                    if (!IsValidRemarks())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsInvoicePaymentVoucher.InvoiceRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["invoicecode"].ToString())
                        , txtRemarks.Text.Trim());                                        
                    PhoenixAccountsInvoicePaymentVoucher.DeleteInvoiceMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["invoicecode"].ToString());
                    SendNotificationMail();             
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
            }
            else if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemarks()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtRemarks.Text.Trim()) == null)
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }
    private void SendNotificationMail()
    {        
        string to, cc, bcc, mailsubject, mailbody, sessionid = string.Empty;
        string strbody = "";
        strbody = "<table><tr><td>Good Day,</td></tr><tr><td></td></tr>";        
        strbody = strbody + "<tr><td>The Invoice " + txtInvoiceNumber.Text + " has been deleted from the Payment Voucher " + txtPaymentVoucherNumber.Text + " by -" + PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString() + "</td></tr>";
        strbody = strbody + "<tr><td>due to the concern of  ' " + txtRemarks.Text.Trim() + "'</td></tr><tr><td></td></tr>";
        strbody = strbody + "<tr><td>Kindly do the needful at the earliest to process this Invoice again otherwise please ignore this.</td></tr></table>";

        bool htmlbody = true;
        MailPriority priority = MailPriority.High;       
        DataSet ds = PhoenixAccountsInvoicePaymentVoucher.RejectedInvoiceMailList(new Guid(ViewState["invoicecode"].ToString()));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDTOEMAILADDRESS"].ToString().Trim()))
            {
                to = ds.Tables[0].Rows[0]["FLDTOEMAILADDRESS"].ToString();
                cc = "";
                bcc = "";
                mailsubject = "Notification for the Deleted Invoice (" + txtInvoiceNumber.Text + ") from the Payment Voucher (" + txtPaymentVoucherNumber.Text + ").";                
                mailbody = strbody;                
                PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid);                
                ucStatus.Text = "Email sent successfully";
            }            
        }        
    }  
}
