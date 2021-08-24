using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsInvoicePaymentVoucherCreditNoteAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["supplier"] != null && Request.QueryString["supplier"] != string.Empty)
                ViewState["SuppCode"] = Request.QueryString["supplier"];
            else
                ViewState["SuppCode"] = "";

            if (Request.QueryString["currency"] != null && Request.QueryString["currency"] != string.Empty)
                ViewState["CurrencyCode"] = Request.QueryString["currency"];
            else
                ViewState["CurrencyCode"] = "";

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            else
                ViewState["voucherid"] = "";

            BindHeaderData();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
         //   BindCreditNote();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeaderData()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();

                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                    ViewState["CurrencyCode"] = dr["FLDCURRENCY"].ToString();
                    ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
                    ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUSCODE"].ToString();
                    ViewState["PVType"] = dr["FLDPAYMENTVOUCHERTYPE"].ToString();
                    ViewState["RemittanceId"] = dr["FLDREMITTANCEID"].ToString();
                    ViewState["TypeOfPV"] = dr["FLDSUBTYPE"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                }
            }
        }
    }

    private void BindCreditNote()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsInvoicePaymentVoucher.CreditNoteVoucherSearch(General.GetNullableInteger(ViewState["SuppCode"].ToString())
                                                        , General.GetNullableInteger(ViewState["CurrencyCode"].ToString())
                                                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                        , General.GetNullableGuid(ViewState["voucherid"].ToString()));

            gvCreditNotes.DataSource = ds;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCreditNotes.Rebind();
    }

    protected void gvCreditNotes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                //GridView _gridView = (GridView)sender;
                //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                RadTextBox txtAmountUtilized = (RadTextBox)e.Item.FindControl("txtAmountUtilized");
                RadLabel lblCreditNoteid = (RadLabel)e.Item.FindControl("lblCreditNoteId");
                RadLabel lblAvailableAmount = (RadLabel)e.Item.FindControl("lblBalance");

                if (General.GetNullableDecimal(txtAmountUtilized.Text) == 0)
                {
                    ucError.ErrorMessage = "Please enter amount";
                    ucError.Visible = true;
                    return;
                }

                if (decimal.Parse(lblAvailableAmount.Text) < decimal.Parse(txtAmountUtilized.Text))
                {
                    ucError.ErrorMessage = "Please enter lesser amount than the available balance";
                    ucError.Visible = true;
                    return;
                }

                if (decimal.Parse(txtAmount.Text) < decimal.Parse(txtAmountUtilized.Text))
                {
                    ucError.ErrorMessage = "Please enter lesser amount than the payable amount";
                    ucError.Visible = true;
                    return;
                }
                else
                    PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherCreditNoteMappingInsert(General.GetNullableGuid(ViewState["voucherid"].ToString()), General.GetNullableGuid(lblCreditNoteid.Text), decimal.Parse(txtAmountUtilized.Text));

           //     _gridView.EditIndex = -1;
                BindCreditNote();
                BindHeaderData();

                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNotes_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvCreditNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindCreditNote();
    }
}
