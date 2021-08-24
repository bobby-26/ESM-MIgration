using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsCashOutRequestGeneral : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtSupplierId.Attributes.Add("style", "visibility:hidden");
            txtAccountId.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            // toolbarmain.AddButton("Save", "SAVE");
            //toolbarmain.AddButton("Post", "POST");
            //toolbarmain.AddButton("Repost", "REPOST");
            // toolbarmain.AddButton("Change to Remittance", "REMITTANCE");
            MenuSave.AccessRights = this.ViewState;
            // MenuSave.MenuList = toolbarmain.Show();
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            if (!IsPostBack)
            {
                ViewState["cashpaymentid"] = "";
                if ((Request.QueryString["cashpaymentid"] != null) && (Request.QueryString["cashpaymentid"] != ""))
                {
                    ViewState["cashpaymentid"] = Request.QueryString["cashpaymentid"].ToString();
                    BindHeader(ViewState["cashpaymentid"].ToString());
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

            if (Convert.ToString(ViewState["CASHPAYMENTSTATUS"]) != "Posted")
            {
                toolbarmain.AddButton("Change to Remittance", "REMITTANCE", ToolBarDirection.Right);
                toolbarmain.AddButton("Post", "POST", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuSave.MenuList = toolbarmain.Show();
            }
            else
            {
                toolbarmain.AddButton("Change to Remittance", "REMITTANCE", ToolBarDirection.Right);
                toolbarmain.AddButton("Repost", "REPOST", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuSave.MenuList = toolbarmain.Show();

            }

            if (ViewState["cashpaymentid"] != null)
            {
                // Title1.Text = "Cash Out      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            }
            MenuSave.AccessRights = this.ViewState;
            MenuSave.Title = "Cash Out      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            MenuSave.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindHeader(string cashpaymentid)
    {
        DataSet ds = PhoenixAccountsCashOut.CashOutEdit(cashpaymentid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
            txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDCASHPAYMENTAMOUNT"]));
            txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
            txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
            txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
            txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
            lblCashPaymentId.Text = dr["FLDCASHPAYMENTID"].ToString();
            ViewState["CASHPAYMENTSTATUS"] = dr["FLDCASHPAYMENTSTATUS"].ToString();

            imgBankPicklist.Attributes.Add("onclick", "showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=', true); return false;");
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCashOut())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsCashOut.CashOutAssignedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, lblCashPaymentId.Text, int.Parse(txtAccountId.Text));

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }

            if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidCashOut())
                {
                    ucError.Visible = true;
                    return;
                }
                // toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsInvoiceLineItem.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "'); return false;", "Add", "Add.png", "ADD");


                String scriptpopup = String.Format(
                    "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsCashOutPaidDate.aspx?cashpaymentid=" + lblCashPaymentId.Text + "&editable=true');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                //PhoenixAccountsCashOut.CashOutAssignedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, lblCashPaymentId.Text, int.Parse(txtAccountId.Text));
                //dce.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','AccountsCashOutPaidDate.aspx?cashpaymentid=" + lblCashPaymentId.Text + "&r=1');");
                //String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }

            if (CommandName.ToUpper().Equals("REPOST"))
            {
                try
                {
                    PhoenixAccountsCashOut.CashOutRepost(PhoenixSecurityContext.CurrentSecurityContext.UserCode, lblCashPaymentId.Text, int.Parse(txtAccountId.Text));
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }



            if (CommandName.ToUpper().Equals("REMITTANCE"))
            {
                if (!IsValidRemittance())
                {
                    ucError.Visible = true;
                    return;
                }

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

    void Reset()
    {
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtSupplierId.Text = "";
        txtCurrencyCode.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtSupplierId.Text = "";
        txtAmount.Text = "";
        txtVoucherNumber.Text = "";
        ViewState["cashpaymentid"] = null;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCashOut()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtAccountId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Cash account is required";
        if (lblCashPaymentId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Cash payment is required";

        return (!ucError.IsError);
    }

    private bool IsValidRemittance()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtBankID.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Back account detail is required";

        return (!ucError.IsError);
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {


    }

    protected void txtAccountId_Changed(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsCashOut.CheckInsufficientCash(Convert.ToInt32(txtAccountId.Text), Convert.ToInt32(txtCurrencyId.Text), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, Convert.ToDecimal(txtAmount.Text));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
}
