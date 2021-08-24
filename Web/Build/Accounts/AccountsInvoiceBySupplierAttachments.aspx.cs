using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsInvoiceBySupplierAttachments : PhoenixBasePage
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

            //if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
            //{
            //    ViewState["ReceivedFrom"] = Request.QueryString["qfrom"];
            //}

            if (Request.QueryString["attachmenttype"] != null && Request.QueryString["attachmenttype"] != string.Empty)
            {
                ViewState["ATTCHMENTTYPE"] = Request.QueryString["attachmenttype"].ToString();
                rblAttachmentType.Enabled = false;
            }
            else
                ViewState["ATTCHMENTTYPE"] = "Invoice";
            BindHard();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Invoice", "INVOICE");
        toolbar.AddButton("Attachments", "ATTACHMENTS");

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.MenuList = toolbar.Show();
       // MenuInvoice1.SetTrigger(pnlInvoice);

        if (ViewState["ISAPPROVED"] != null && ViewState["ISAPPROVED"].ToString().Equals("0"))
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=";
        }
    }

    protected void BindHard()
    {
        rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 93, 0, ",INV,CDT,");

       // rblAttachmentType.DataTextField = "FLDHARDNAME";
       // rblAttachmentType.DataValueField = "FLDHARDCODE";
        rblAttachmentType.DataBind();

        if (ViewState["ATTCHMENTTYPE"].ToString() == "Credit Notes")
            rblAttachmentType.SelectedIndex = 0;
        else
            rblAttachmentType.SelectedIndex = 1;
    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex != -1)
            ViewState["ATTCHMENTTYPE"] = rblAttachmentType.Items[rblAttachmentType.SelectedIndex].Text.Trim();

        if (ViewState["ISAPPROVED"] != null && ViewState["ISAPPROVED"].ToString().Equals("0"))
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString() + "&U=1";
        }
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("INVOICE"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceBySupplierMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
        }
    }

    protected void InvoiceEdit()
    {
        if (ViewState["invoicecode"] != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceBySupplierEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
               // ttlInvoice.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                ViewState["FLDINVOICESTATUS"] = drInvoice["FLDINVOICESTATUS"].ToString();
                ViewState["DTKey"] = drInvoice["FLDDTKEY"].ToString();

                ViewState["ISAPPROVED"] = drInvoice["FLDINVOICESTATUS"].ToString().ToUpper().Equals("APPROVED") ? "1" : "0";
            }
        }
    }
}
