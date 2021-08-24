using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsCreditDebitNote : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtSupplierCode.Attributes.Add("onkeydown", "return false;");
        txtSupplierName.Attributes.Add("onkeydown", "return false;");
        txtSupplierId.Attributes.Add("onkeydown", "return false;");
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        txtSupplierId.Attributes.Add("style", "display: none;");

        //PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        //toolbar1.AddButton("Repost", "REPOST", ToolBarDirection.Right);
        //toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);

        //MenuCreditNote.AccessRights = this.ViewState;
        //MenuCreditNote.MenuList = toolbar1.Show();
       // MenuCreditNote.SetTrigger(pnlCreditNote);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ddlBillToCompany.SelectedCompany = "16";

            if (Request.QueryString["creditdebitnoteid"] != string.Empty)
            {
                ViewState["CREDITDEBITNOTEID"] = Request.QueryString["creditdebitnoteid"];
                CreditNoteEdit();
            }
        }
        //CreditNoteEdit();
        btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=131&framename=ifMoreInfo', true); ");
    }

    protected void MenuCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
            ShowCancelRepostButton();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidCreditNote())
                {
                    ucError.Visible = true;
                    return;
                }

                string txtExchangeRate = "0";
                string txt2ndExchangeRate = "0";

                DataSet ds = PhoenixRegistersCompany.EditCompany(int.Parse(ddlBillToCompany.SelectedCompany));
                if (ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));

                    DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
                    DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];

                    decimal dTransactionExchangerate = 0;

                    DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ucCurrency.SelectedCurrency), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    if (dsInvoice.Tables.Count > 0)
                    {
                        DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                        dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                    }


                    txtExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
                    txt2ndExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));

                }

                if (ViewState["CREDITDEBITNOTEID"] == null)
                {
                    PhoenixAccountsCreditDebitNote.InsertCreditDebitNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableDateTime(ucReceivedDate.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(txtSupplierId.Text)
                        , txtVendorCreditNoteNo.Text
                        , decimal.Parse(txtAmount.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , txtRemarks.Text
                        , int.Parse(ddlBillToCompany.SelectedCompany)
                        , decimal.Parse(txtExchangeRate)
                        , decimal.Parse(txt2ndExchangeRate)
                        , General.GetNullableDateTime(UCPostDate.Text));

                    ucStatus.Text = "Credit Note information added";
                    Reset();
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    PhoenixAccountsCreditDebitNote.UpdateCreditDebitNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["CREDITDEBITNOTEID"].ToString())
                        , General.GetNullableDateTime(ucReceivedDate.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(txtSupplierId.Text)
                        , txtVendorCreditNoteNo.Text
                        , decimal.Parse(txtAmount.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , txtRemarks.Text
                        , int.Parse(ddlBillToCompany.SelectedCompany)
                        , decimal.Parse(txtExchangeRate)
                        , decimal.Parse(txt2ndExchangeRate)
                        , General.GetNullableDateTime(UCPostDate.Text));

                    ucStatus.Text = "Credit Note information updated";
                }
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
        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            try
            {
                if (ViewState["CREDITDEBITNOTEID"] != null)
                {
                    PhoenixAccountsCreditDebitNote.CancelUpdateCreditDebitNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["CREDITDEBITNOTEID"].ToString()));

                    ucStatus.Text = "Credit Note Canceled";
                }
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
        if (CommandName.ToUpper().Equals("REPOST"))
        {
            try
            {
                if (ViewState["CREDITDEBITNOTEID"] != null)
                {
                    PhoenixAccountsCreditDebitNote.RepostUpdateCreditDebitNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["CREDITDEBITNOTEID"].ToString()));

                    ucStatus.Text = "Credit Note Reposted";
                }
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
        CreditNoteEdit();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void CreditNote_SetExchangeRate(object sender, EventArgs e)
    {
        if (ucCurrency.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ucCurrency.SelectedCurrency));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                //txtExchangeRate.Text = string.Format(String.Format("{0:#####.000000}", drInvoice["FLDEXCHANGERATE"]));
            }
        }
        else
        {
            //txtExchangeRate.Text = "";
        }
    }

    protected void Reset()
    {
        ViewState["CREDITDEBITNOTEID"] = null;
        txtRegisterNo.Text = "";
        txtAmount.Text = "";
        ucReceivedDate.Text = "";
        ucCurrency.SelectedCurrency = string.Empty;
        ucDate.Text = "";
        txtRemarks.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierId.Text = "";
        txtSupplierName.Text = "";
        txtStatus.Text = "";
        txtVendorCreditNoteNo.Text = "";
        txtCreditNoteVoucherNo.Text = "";
        txtPaymentVoucherNo.Text = "";
        ddlBillToCompany.SelectedCompany = "";
        UCPostDate.Text = "";
        UCPostDate.Enabled = true;

    }

    protected void CreditNoteEdit()
    {
        if (ViewState["CREDITDEBITNOTEID"] != null)
        {
            DataSet ds = PhoenixAccountsCreditDebitNote.EditCreditDebitNote(new Guid(ViewState["CREDITDEBITNOTEID"].ToString()));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtRegisterNo.Text = dr["FLDCNREGISTERNO"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:##,###,###.000000}", dr["FLDAMOUNT"].ToString()));
                    ucReceivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                    ucDate.Text = General.GetDateTimeToString(dr["FLDDOCUMENTDATE"].ToString());
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    txtSupplierCode.Text = dr["FLDCODE"].ToString();
                    txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
                    txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                    txtVendorCreditNoteNo.Text = dr["FLDREFERENCENO"].ToString();
                    txtCreditNoteVoucherNo.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtPaymentVoucherNo.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    ddlBillToCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                    UCPostDate.Text = Convert.ToString(dr["FLDPOSTDATE"]);
                    if (Convert.ToString(dr["FLDVOUCHERNUMBER"]) != "")
                    {
                        UCPostDate.Enabled = false;
                    }
                    else
                    {
                        UCPostDate.Enabled = true;
                    }


                    if (dr["FLDVENDORCREDITNOTENUMBERALREADYEXISTS"].ToString() == "1")
                    {
                        HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsCreditDebitNoteDuplicateList.aspx?creditdebitnoteid=" + ViewState["CREDITDEBITNOTEID"].ToString() + "&suppliercode=" + dr["FLDSUPPLIERCODE"].ToString() + "&referenceno=" + dr["FLDREFERENCENO"].ToString();
                        HlinkRefDuplicate.Visible = true;
                    }
                    if (dr["FLDISFROMDEPOSIT"].ToString() == "1" || dr["FLDISPOCANCELLED"].ToString() == "1" || dr["FLDPOSTEDFROM"].ToString() == "3")
                    {
                        HideCancelRepostButton();
                    }
                    else
                    {
                        ShowCancelRepostButton();
                    }
                }
            }
        }
    }

    private void HideCancelRepostButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("New", "NEW",ToolBarDirection.Right);
        //toolbar1.AddButton("Save", "SAVE");   //issue id - 26964 

        MenuCreditNote.AccessRights = this.ViewState;
        MenuCreditNote.MenuList = toolbar1.Show();
       // MenuCreditNote.SetTrigger(pnlCreditNote);
    }
    private void ShowCancelRepostButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar1.AddButton("Repost", "REPOST", ToolBarDirection.Right);
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuCreditNote.AccessRights = this.ViewState;
        MenuCreditNote.MenuList = toolbar1.Show();
    }

    protected bool IsValidCreditNote()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucReceivedDate.Text) == null)
            ucError.ErrorMessage = "Received date is required";
        if (txtAmount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required";
        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required";
        if (txtSupplierId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier is required";
        if (txtVendorCreditNoteNo.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor credit note number is required";

        if (ddlBillToCompany.SelectedCompany == "Dummy")
            ucError.ErrorMessage = "Bill to company is required";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

   // protected void btnPickSupplier_Click(object sender, ImageClickEventArgs e)
   // {
   //     btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=131&framename=ifMoreInfo', true); ");
   // }
}
