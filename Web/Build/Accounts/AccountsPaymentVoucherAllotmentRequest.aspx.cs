using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class AccountsPaymentVoucherAllotmentRequest : PhoenixBasePage
{
    public decimal dGrandTotal = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
            ViewState["voucherid"] = Request.QueryString["voucherid"];

        
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Voucher", "VOUCHER");
        MenuOrderFormMain.AccessRights = this.ViewState;
        MenuOrderFormMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["callfrom"] = null;
            
        }

        if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
            ViewState["callfrom"] = Request.QueryString["callfrom"];

        BindPVDetails();
        BindData();
        //if (ViewState["PVStatuscode"].ToString() != "48")
        //{
        //    cmdApprove.Attributes.Add("onclick", "parent.Openpopup('PaymentVoucherApproval', '', '../Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=1&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
        //}
        //else
        if (ViewState["PVStatuscode"].ToString() == "48")
        {
            cmdApprove.Attributes.Add("style", "visibility:hidden");
        }        
    }   
    private void BindPVDetails()
    {
        DataTable dt = new DataTable();
        dt = PhoenixAccountsAllotmentRequestPaymentVoucher.AllotmentRequestPaymentVoucherSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["voucherid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtVoucherNo.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
            txtVoucherDate.Text = dr["FLDVOUCHERDATE"].ToString();
            txtNameFileNO.Text = dr["FLDEMPLOYEENAME"].ToString();
            txtBankName.Text = dr["FLDBANKNAME"].ToString();
            txtBeneficiaryName.Text = dr["FLDACCOUNTNAME"].ToString();
            txtBankAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
            txtpaymentAmount.Text = dr["FLDPAYABLEAMOUNT"].ToString();
            ViewState["RemittanceId"] = dr["FLDREMITTANCEID"].ToString();
            ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
            ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
        }
    }
    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixAccountsAllotmentRequestPaymentVoucher.AllotmentRequestPaymentVoucherLISearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["voucherid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            dGrandTotal = (Convert.ToDecimal(dt.Rows[0]["FLDTOTALAMOUNT"].ToString()));
            gvPVGenerate.DataSource = dt;
            gvPVGenerate.DataBind();
            if(ViewState["RemittanceId"].ToString()!= string.Empty)
                gvPVGenerate.Columns[5].Visible = false;
        }
        else
        {
            dGrandTotal = 0;
            ShowNoRecordsFound(dt, gvPVGenerate);
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
    protected void gvPVGenerate_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "ZEROPV")
                    Response.Redirect("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPVGenerate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsAllotmentRequestPaymentVoucher.AllotmentRequestPaymentVoucherDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text));

                BindData();
                BindPVDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPVGenerate_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindPVDetails();
    }

    private void DirectApproval(int ApprovalType)
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), ApprovalType, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());

    }

    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "PAA")));
            BindPVDetails();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
