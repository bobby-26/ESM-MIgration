using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRemittanceInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
       
        toolbar1.AddButton("Details Verified", "DETAILSVERIFIED",ToolBarDirection.Right);
        toolbar1.AddButton("Next", "NEXT", ToolBarDirection.Right);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["REMITTANCEID"] != null && Request.QueryString["REMITTANCEID"] != string.Empty)
            {
                ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"];
                ViewState["INVOICEDTKEY"] = Request.QueryString["INVOICEDTKEY"];
                ViewState["INVOICEBANKINFOPAGENUMBER"] = Request.QueryString["INVOICEBANKINFOPAGENUMBER"];
                if (ViewState["INVOICEDTKEY"] != null && ViewState["INVOICEDTKEY"].ToString() != string.Empty)
                {
                    DataSet dsattachment = new DataSet();
                    int iRowCount = 0;
                    int iTotalPageCount = 0;
                    if (ViewState["INVOICEBANKINFOPAGENUMBER"].ToString() == "CTM") // Ctm
                        dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(ViewState["INVOICEDTKEY"].ToString()), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                    else // Invoice
                        dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(ViewState["INVOICEDTKEY"].ToString()), null, "invoice", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

                    if (dsattachment.Tables[0].Rows.Count > 0)
                    {
                        DataRow drattachment = dsattachment.Tables[0].Rows[0];
                        string filepath = drattachment["FLDFILEPATH"].ToString();
                        ifMoreInfo.Attributes["src"] = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString(); //Session["sitepath"] + "/attachments/" + filepath + "#page=" + ViewState["INVOICEBANKINFOPAGENUMBER"];
                        //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath + "#page=" + ViewState["INVOICEBANKINFOPAGENUMBER"];
                    }
                    //BindHeader(ViewState["REMITTANCEID"].ToString());
                }
                BindHeader(ViewState["REMITTANCEID"].ToString());
            }
            ViewState["Indexno"] = Request.QueryString["gvindex"];
        }

        if (ViewState["INVOICEDTKEY"] != null)
        {
            MenuVoucher.Title = "Invoice("+PhoenixAccountsInvoicePaymentVoucher.InvoiceVoucherNumber+")";
        }
        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DETAILSVERIFIED"))
            {
                if (ViewState["REMITTANCEID"] != null)
                {
                    PhoenixAccountsRemittance.ApproveRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["REMITTANCEID"].ToString());
                    ucStatus.Text = "Remittance Invoice details verified.";
                }
            }

            if (CommandName.ToUpper().Equals("NEXT"))
            {
                int index = 0;
                index = Convert.ToInt32(ViewState["Indexno"].ToString());
                index += 1;
                ViewState["Indexno"] = index;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("gvindexno", ViewState["Indexno"].ToString());
                Filter.CurrentRemittenceinvoicegvindex = criteria;
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void BindHeader(string remittanceid)
    {
        DataSet ds = PhoenixAccountsRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            ddlPaymentmode.SelectedHard = dr["FLDPAYMENTMODE"].ToString();
            txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
            txtBenficiaryBankSWIFTCode.Text = dr["FLDSWIFTCODE"].ToString();
            txtIntermediaryBankSWIFTCode.Text = dr["FLDISWIFTCODE"].ToString();
            txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
            txtBenficiaryBankCode.Text = dr["FLDBANKCODE"].ToString();
            txtIntermediaryBankName.Text = dr["FLDIBANKNAME"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtBenficiaryBranchCode.Text = dr["FLDBRANCHCODE"].ToString();
            txtIntermediaryBankAccountNumber.Text = dr["FLDIACCOUNTNUMBER"].ToString();
            txtIBANNumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtEmailAddress.Text = dr["FLDEMAILID"].ToString();
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
