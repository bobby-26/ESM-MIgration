using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceRequestLineItem : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtEmployeeId.Attributes.Add("style", "display:none");
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            if (!IsPostBack)
            {
                ViewState["Remittanceid"] = "";
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 1);
                if ((Request.QueryString["REMITTANCEID"] != null) && (Request.QueryString["REMITTANCEID"] != ""))
                {
                    ViewState["Remittanceid"] = Request.QueryString["REMITTANCEID"].ToString();
                    ddlBankAccount.Enabled = false;
                    BindHeader(ViewState["Remittanceid"].ToString());
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                    ddlBankAccount.Enabled = true;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERPMV"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvAdvanceVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeader(string remittanceid)
    {
        DataSet ds = PhoenixAccountsAllotmentRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDREMITTANCECURRENCY"].ToString();
            txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();
            //txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtEmployeeName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtEmployeeId.Text = dr["FLDSUPPLIERID"].ToString();
        }
    }

    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoucherDetails.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixAccountsAllotmentRemittance.RemittancePaymentVoucherSearch(General.GetNullableString(ViewState["Remittanceid"].ToString())
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvVoucherDetails.PageSize
                                            , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);
        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
        //if (ds.Tables[0].Rows.Count > 0)
        //    gvVoucherDetails.Items[0].Selected = true;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindAdvancePaymentData()
    {
        ViewState["PAGENUMBERPMV"] = ViewState["PAGENUMBERPMV"] != null ? ViewState["PAGENUMBERPMV"] : gvAdvanceVoucherDetails.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.RemittanceAllotmentLineItemSearch(General.GetNullableString(ViewState["Remittanceid"].ToString()), sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBERPMV"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDADVANCEAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);
        gvAdvanceVoucherDetails.DataSource = ds;
        gvAdvanceVoucherDetails.VirtualItemCount = iRowCount;
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvAdvanceVoucherDetails.Items[0].Selected = true;
        //}

        ViewState["ADVROWCOUNT"] = iRowCount;
        ViewState["ADVTOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvVoucherDetails_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                PhoenixAccountsAllotmentRemittance.PaymentVoucherRemittanceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["Remittanceid"].ToString(), ((RadTextBox)e.Item.FindControl("txtVoucherId")).Text);
                Rebind();
                RebindAdvanceVoucherDetails();
                ucStatus.Text = "Payment voucher removed successfully from the Remittance.";
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        else if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            ViewState["PAGENUMBER"] = null;
            Rebind();         
        }

    }

    protected void gvAdvanceVoucherDetails_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            ViewState["PAGENUMBERPMV"] = null;
            RebindAdvanceVoucherDetails();
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }

        Rebind();
    }

    protected void gvVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (SessionUtil.CanAccess(this.ViewState, "cmdDelete"))
            {
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to delete?'); return false;");
            }
            else
            {
                if (db != null) db.Attributes.Add("style", "visibility:hidden");
            }



        }
    }

    protected void gvAdvanceVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (SessionUtil.CanAccess(this.ViewState, "cmdDelete"))
                {
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to delete?'); return false;");
                }
                else
                {
                    if (db != null) db.Attributes.Add("style", "visibility:hidden");
                }
        }
    }

    protected void gvVoucherDetails_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAdvanceVoucherDetails_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            RebindAdvanceVoucherDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
                txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
                Rebind();
            }
        }
    }
    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }

    protected void RebindAdvanceVoucherDetails()
    {
        gvAdvanceVoucherDetails.SelectedIndexes.Clear();
        gvAdvanceVoucherDetails.EditIndexes.Clear();
        gvAdvanceVoucherDetails.DataSource = null;
        gvAdvanceVoucherDetails.Rebind();
    }
    protected void gvVoucherDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAdvanceVoucherDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindAdvancePaymentData();
    }
}
