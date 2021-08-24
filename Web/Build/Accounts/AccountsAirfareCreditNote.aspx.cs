using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAirfareCreditNote : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtSupplierCode.Attributes.Add("onkeydown", "return false;");
            txtSupplierName.Attributes.Add("onkeydown", "return false;");
            txtSupplierId.Attributes.Add("onkeydown", "return false;");
            txtSupplierId.Attributes.Add("style", "display:none;");            


            if (!IsPostBack)
            {
                ddlBillToCompany.SelectedCompany = "16";
                ViewState["STATUS"] = null;
                ViewState["POSTEDFROM"] = null;
                if (Request.QueryString["AirfareCreditNoteId"] != string.Empty)
                {
                    ViewState["AirfareCreditNoteId"] = Request.QueryString["AirfareCreditNoteId"];
                    CreditNoteEdit();
                }
            }

            PhoenixToolbar toolbar1 = new PhoenixToolbar();         
 
            if (ViewState["STATUS"] != null && ViewState["STATUS"].ToString() == "1379" && ViewState["POSTEDFROM"] != null && ViewState["POSTEDFROM"].ToString() == "1")
                toolbar1.AddButton("Repost", "REPOST",ToolBarDirection.Right);
            else if (ViewState["STATUS"] != null && ViewState["STATUS"].ToString() == "1378" && ViewState["POSTEDFROM"] != null && ViewState["POSTEDFROM"].ToString() == "1")
                toolbar1.AddButton("Post", "POST");

            toolbar1.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbar1.AddButton("New", "NEW",ToolBarDirection.Right);
            MenuCreditNote.AccessRights = this.ViewState;

            MenuCreditNote.MenuList = toolbar1.Show();
            //    MenuCreditNote.SetTrigger(pnlCreditNote);

            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131&framename=ifMoreInfo', true); ");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCreditNote())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["AirfareCreditNoteId"] == null)
                {
                    PhoenixAccountsAirfareCreditNote.InsertAirfareCreditNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableDateTime(ucReceivedDate.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(txtSupplierId.Text)
                        , txtVendorCreditNoteNo.Text
                        , decimal.Parse(txtAmount.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , txtRemarks.Text
                        , int.Parse(ddlBillToCompany.SelectedCompany));

                    ucStatus.Text = "Airfare Credit Note added";
                    //Reset();
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    PhoenixAccountsAirfareCreditNote.UpdateAirfareCreditNote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["AirfareCreditNoteId"].ToString())
                        , General.GetNullableDateTime(ucReceivedDate.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(txtSupplierId.Text)
                        , txtVendorCreditNoteNo.Text
                        , decimal.Parse(txtAmount.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , txtRemarks.Text
                        , int.Parse(ddlBillToCompany.SelectedCompany));

                    ucStatus.Text = "Airfare Credit Note updated";
                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsAirfareCreditNote.AirfareCreditNotePosting(new Guid(ViewState["AirfareCreditNoteId"].ToString()));
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            if (CommandName.ToUpper().Equals("REPOST"))
            {
                PhoenixAccountsAirfareCreditNote.AirfareCreditNoteRePosting(new Guid(ViewState["AirfareCreditNoteId"].ToString()));
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            CreditNoteEdit();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
             if (ViewState["STATUS"] != null && ViewState["STATUS"].ToString() == "1379" && ViewState["POSTEDFROM"] != null && ViewState["POSTEDFROM"].ToString() == "1")
                toolbar1.AddButton("Repost", "REPOST",ToolBarDirection.Right);
            else if (ViewState["STATUS"] != null && ViewState["STATUS"].ToString() == "1378" && ViewState["POSTEDFROM"] != null && ViewState["POSTEDFROM"].ToString() == "1")
                toolbar1.AddButton("Post", "POST", ToolBarDirection.Right);

            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuCreditNote.AccessRights = this.ViewState;
            MenuCreditNote.MenuList = toolbar1.Show();
          //  MenuCreditNote.SetTrigger(pnlCreditNote);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void Reset()
    {
        ViewState["AirfareCreditNoteId"] = null;
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
    }

    protected void CreditNoteEdit()
    {
        if (ViewState["AirfareCreditNoteId"] != null)
        {
            DataSet ds = PhoenixAccountsAirfareCreditNote.EditCreditDebitNote(new Guid(ViewState["AirfareCreditNoteId"].ToString()));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtRegisterNo.Text = dr["FLDCNREGISTERNO"].ToString();
                    txtAmount.Text = (dr["FLDAMOUNT"].ToString());
                    ucReceivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
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

                    ViewState["STATUS"] = dr["FLDSTATUS"].ToString();
                    ViewState["POSTEDFROM"] = dr["FLDPOSTEDFROM"].ToString();
                }
            }
        }
    }

    protected bool IsValidCreditNote()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucReceivedDate.Text) == null)
            ucError.ErrorMessage = "Received date is required";
        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Date is required";
        if (txtAmount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required";
        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required";
        if (txtSupplierId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier is required";
        if (txtVendorCreditNoteNo.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor credit note number is required";
        if (ddlBillToCompany.SelectedCompany == "Dummy")
            ucError.ErrorMessage = "Paying Company is required";

        return (!ucError.IsError);
    }

    protected void btnPickSupplier_Click(object sender, ImageClickEventArgs e)
    {
        //btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");
    }    
}
