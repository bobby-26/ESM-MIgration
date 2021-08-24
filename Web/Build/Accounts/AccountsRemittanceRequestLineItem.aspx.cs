using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsRemittanceRequestLineItem : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            
            toolbarmain.AddButton("LineItems", "LINEITEMS",ToolBarDirection.Right);
            toolbarmain.AddButton("Remittance", "REMITTANCE", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtSupplierId.Attributes.Add("style", "display:none");
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            if (!IsPostBack)
            {
                ViewState["Remittenceid"] = "";
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 1);
                if ((Request.QueryString["REMITTENCEID"] != null) && (Request.QueryString["REMITTENCEID"] != ""))
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    ddlBankAccount.Enabled = false;
                    BindHeader(ViewState["Remittenceid"].ToString());
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                    ddlBankAccount.Enabled = true;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            //BindData();
            //BindAdvancePaymentData();
          //  SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeader(string remittanceid)
    {
        DataSet ds = PhoenixAccountsRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
        }
    }

    protected void gvVoucherDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixAccountsRemittance.RemittancePaymentVoucherSearch(General.GetNullableString(ViewState["Remittenceid"].ToString()), "", General.GetNullableInteger(txtSupplierId.Text), General.GetNullableInteger(txtCurrencyId.Text)
                                            , string.Empty, string.Empty
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null)
                                            , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindAdvancePaymentData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsRemittance.RemittanceAdvancePaymentVoucherSearch(General.GetNullableString(ViewState["Remittenceid"].ToString()), "", General.GetNullableInteger(txtSupplierId.Text), General.GetNullableInteger(txtCurrencyId.Text)
                                            , string.Empty, string.Empty
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null)
                                            , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDADVANCEAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        gvAdvanceVoucherDetails.DataSource = ds;
        gvAdvanceVoucherDetails.VirtualItemCount = iRowCount;

        ViewState["ADVROWCOUNT"] = iRowCount;
        ViewState["ADVTOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAdvanceVoucherDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindAdvancePaymentData();
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REMITTANCE"))
            {
                Response.Redirect("../Accounts/AccountsRemittanceMaster.aspx?REMITTENCEID=" + ViewState["Remittenceid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRemittanceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ViewState["Remittenceid"].ToString()
                    , ((RadTextBox)e.Item.FindControl("txtVoucherId")).Text);
                BindData();
                ucStatus.Text = "Payment voucher removed successfully from the Remittance.";
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void gvAdvanceVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETEA"))
        {
            try
            {
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRemittanceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ViewState["Remittenceid"].ToString()
                    , ((RadTextBox)e.Item.FindControl("txtVoucherId")).Text);
                BindData();
                ucStatus.Text = "Payment voucher removed successfully from the Remittance.";
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    try
    //    {
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvVoucherDetails.SelectedIndex = -1;
    //        gvVoucherDetails.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";

    //    cmdADVPrevious.Enabled = IsPreviousEnabled();
    //    cmdADVNext.Enabled = IsNextEnabled();
    //    lblADVPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblADVPages.Text = " of " + ViewState["ADVTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblADVRecords.Text = "(" + ViewState["ADVROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;
    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private Boolean IsADVPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["ADVPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["ADVTOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;
    //    return false;
    //}

    //private Boolean IsADVNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["ADVPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["ADVTOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    protected void gvVoucherDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
        }

    }


    protected void gvAdvanceVoucherDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {

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
                BindData();
             //   SetPageNavigator();
            }
        }
    }
}
