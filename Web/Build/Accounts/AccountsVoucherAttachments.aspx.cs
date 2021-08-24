using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsVoucherAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {

            if (Request.QueryString["Status"] != null)
            {
                ViewState["Status"] = Request.QueryString["Status"].ToString();
            }
            else
            {
                ViewState["Status"] = "";
            }

            if (Request.QueryString["qvoucherlineid"] != null && Request.QueryString["qvoucherlineid"] != string.Empty)
            {
                ViewState["VOUCHERLINEITEMID"] = Request.QueryString["qvoucherlineid"];
                VoucherLineEdit();
            }
            if (Request.QueryString["dtkey"] != null && Request.QueryString["dtkey"] != string.Empty)
            {
                ViewState["DTKey"] = Request.QueryString["dtkey"].ToString();
            }
            else
                ViewState["DTKey"] = "";

            if (Request.QueryString["SOAStatus"] != null && Request.QueryString["SOAStatus"] != string.Empty)
            {
                ViewState["SOAStatus"] = Request.QueryString["SOAStatus"].ToString();
            }

            if (ViewState["VOUCHERLINEITEMID"] != null && ViewState["VOUCHERDTKEY"] != null)
            {
                if (ViewState["SOAStatus"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOAGenerationVoucherLevelFileAttachments.aspx?dtkey="
                + ViewState["VOUCHERDTKEY"].ToString() + "&MOD=ACCOUNTS&SOAStatus=" + ViewState["SOAStatus"] + "&VOUCHERLINEITEMNO=" + ViewState["VOUCHERLINEITEMNO"] + "&mimetype=.pdf";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                    + ViewState["VOUCHERDTKEY"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&U=1&mimetype=.pdf";
                }
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                + ViewState["DTKey"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&mimetype=.pdf";
            }

            if (Request.QueryString["voucherdetailid"] != null && Request.QueryString["voucherdetailid"] != string.Empty)
            {
                ViewState["voucherdetailid"] = Request.QueryString["voucherdetailid"];                
            }

        }
        VoucherNumberEdit();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Line Item Level", "LINEITEM", ToolBarDirection.Right);
        toolbar.AddButton("Voucher Level", "VOUCHER", ToolBarDirection.Right);


        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.MenuList = toolbar.Show();
        //MenuInvoice1.SetTrigger(pnlInvoice);
        if (!IsPostBack)
        MenuInvoice1.SelectedMenuIndex = 1;
    }

    protected void VoucherNumberEdit()
    {
        if (ViewState["voucherdetailid"] != null)
        {
            DataSet ds = PhoenixAccountsERMVoucherDetail.ErmVoucherEdit(new Guid(ViewState["voucherdetailid"].ToString()));

            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuInvoice1.Title = "Attachments - " + dr["FLDVOUCHERNUMBER"].ToString();
            }
        }
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VOUCHER"))
        {

            if (ViewState["VOUCHERLINEITEMID"] != null && ViewState["VOUCHERDTKEY"] != null)
            {
                if (ViewState["SOAStatus"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOAGenerationVoucherLevelFileAttachments.aspx?dtkey="
                    + ViewState["VOUCHERDTKEY"].ToString() + "&MOD=ACCOUNTS&SOAStatus=" + ViewState["SOAStatus"] + "&VOUCHERLINEITEMNO=" + ViewState["VOUCHERLINEITEMNO"] + "&mimetype=.pdf";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                    + ViewState["VOUCHERDTKEY"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&U=1&mimetype=.pdf";
                }
                MenuInvoice1.SelectedMenuIndex = 1;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                + ViewState["DTKey"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&mimetype=.pdf";
            }
        }
        else if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            if (ViewState["VOUCHERLINEITEMID"] != null)
            {
                if (ViewState["SOAStatus"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOAGenerationVoucherLevelFileAttachments.aspx?dtkey="
                    + ViewState["DTKey"].ToString() + "&MOD=ACCOUNTS&SOAStatus=" + ViewState["SOAStatus"] + "&LineYN=Y&mimetype=.pdf";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                    + ViewState["DTKey"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&U=1&mimetype=.pdf";
                }
                MenuInvoice1.SelectedMenuIndex = 0;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSOACheckingVouchersFileAttachments.aspx?dtkey="
                + ViewState["DTKey"].ToString() + "&Status=" + ViewState["Status"] + "&MOD=ACCOUNTS&mimetype=.pdf";
            }
        }
    }

    protected void VoucherLineEdit()
    {
        if (ViewState["VOUCHERLINEITEMID"] != null)
        {
            DataSet ds = PhoenixAccountsVoucher.VoucherLineItemEdit(new Guid(ViewState["VOUCHERLINEITEMID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //ucTitle.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                ViewState["VOUCHERID"] = dr["FLDVOUCHERID"].ToString();
                ViewState["DTKey"] = dr["FLDDTKEY"].ToString();
                ViewState["VOUCHERDTKEY"] = dr["FLDVOUCHERDTKEY"].ToString();
                ViewState["VOUCHERLINEITEMNO"] = dr["FLDVOUCHERLINEITEMNO"].ToString();

            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
    }
}
