using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class Accounts_AccountsInvoicePaymentVoucherCreditNoteView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["creditnoteid"] != null && Request.QueryString["creditnoteid"] != string.Empty)
                ViewState["creditnoteid"] = Request.QueryString["creditnoteid"];
            else
                ViewState["creditnoteid"] = "";

            if (Request.QueryString["paymentvoucherid"] != null && Request.QueryString["paymentvoucherid"] != string.Empty)
                ViewState["paymentvoucherid"] = Request.QueryString["paymentvoucherid"];
            else
                ViewState["paymentvoucherid"] = "";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindCreditNote();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCreditNote()
    {
        if (ViewState["creditnoteid"] != null)
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsInvoicePaymentVoucher.CreditDebitNoteVoucherSearch(General.GetNullableGuid(ViewState["creditnoteid"].ToString()), General.GetNullableGuid(ViewState["paymentvoucherid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCreditNotes.DataSource = ds;
                gvCreditNotes.DataBind();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCreditNotes);
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                lblCnoRegisterNo.Text = ds.Tables[1].Rows[0]["FLDCNREGISTERNO"].ToString();
                lblVendorCreditNote.Text = ds.Tables[1].Rows[0]["FLDREFERENCENO"].ToString();
                lblSupplier.Text = ds.Tables[1].Rows[0]["FLDSUPPLIERNAME"].ToString();
                lblCurrency.Text = ds.Tables[1].Rows[0]["FLDCURRENCYCODE"].ToString();
                lblAmount.Text = ds.Tables[1].Rows[0]["FLDAMOUNT"].ToString();
            }

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void gvCreditNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblCreditNoteMappingid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCreditMappingId");

                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherCreditMappingDelete(General.GetNullableGuid(lblCreditNoteMappingid.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
