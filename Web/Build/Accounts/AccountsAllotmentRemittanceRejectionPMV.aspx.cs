using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class AccountsAllotmentRemittanceRejectionPMV : PhoenixBasePage
{
    string RemittanceRejectionId = string.Empty;
    string RemittanceId = string.Empty;
    string ReasonId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            RemittanceRejectionId = Request.QueryString["RemittanceRejectionId"] == null ? string.Empty : Request.QueryString["RemittanceRejectionId"].ToString();
            RemittanceId = Request.QueryString["remittanceid"] == null ? string.Empty : Request.QueryString["remittanceid"].ToString();
            ReasonId = Request.QueryString["reasonid"] == null ? string.Empty : Request.QueryString["reasonid"].ToString();

            if (!IsPostBack)
            {

            }
            BindData();
            BindAllotmentData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixAccountsAllotmentRemittanceRejection.EditRemittanceRejectionList(RemittanceRejectionId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            txtFileNo.Text = dr["FLDEMPLOYEECODE"].ToString();
            txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
            txtEmployeeId.Text = dr["FLDEMPLOYEEID"].ToString();
            txtRank.Text = dr["FLDRANK"].ToString();

            gvVoucherDetails.DataSource = ds;
            gvVoucherDetails.DataBind();
            gvVoucherDetails.SelectedIndex = 0;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVoucherDetails);
        }
    }

    private void BindAllotmentData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsAllotmentRemittanceRejection.RemittanceAllotmentLineItemSearch(RemittanceId, sortexpression, sortdirection
                                            , 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDADVANCEAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdvanceVoucherDetails.DataSource = ds;
            gvAdvanceVoucherDetails.DataBind();
            gvAdvanceVoucherDetails.SelectedIndex = 0;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAdvanceVoucherDetails);
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

    protected void gvVoucherDetails_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        UserControlEmployeeBankAccount ddlBankAccount = (UserControlEmployeeBankAccount)e.Row.FindControl("ddlBankAccountEdit");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ddlBankAccount != null) ddlBankAccount.SelectedBankAccount = drv["FLDNEWCREWBANKID"].ToString();
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {

                ImageButton db = (ImageButton)e.Row.FindControl("cmdApprove");
                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");

                string quickcode = "";
                DataSet ds = PhoenixRegistersQuick.GetQuickCode( 162, "PRJ");

                quickcode = ds.Tables[0].Rows[0]["FLDQUICKCODE"].ToString();


                if (ReasonId.ToString().Equals(quickcode))
                {
                    if (SessionUtil.CanAccess(this.ViewState, "cmdEdit"))
                    {
                        if (edit != null) edit.Attributes.Add("style", "visibility:hidden");
                    }

                    if (db != null) db.Attributes.Add("style", "visibility:hidden");
                }


                if (SessionUtil.CanAccess(this.ViewState, "cmdApprove"))
                {
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to Approve the remittance?'); return false;");
                }
                else
                {
                    if (db != null) db.Attributes.Add("style", "visibility:hidden");
                }

            }
        }
    }

    protected void gvVoucherDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.ToString().ToUpper() == "EDIT")
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            string pmvrejectionId = (_gridView.DataKeys[nCurrentRow].Value).ToString();
            string pmvId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtVoucherId")).Text;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string bank = ((UserControlEmployeeBankAccount)_gridView.Rows[nCurrentRow].FindControl("ddlBankAccountEdit")).SelectedBankAccount;

                if(General.GetNullableGuid(bank)==null || bank==string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";

                    ucError.ErrorMessage = "Bank account is required.";
                }

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAllotmentRemittanceRejection.PVRemittanceDetailsUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(pmvrejectionId)
                    , new Guid(pmvId)
                    , new Guid(bank));

                ucStatus.Text = "PMV Details Updated Successfully";
                _gridView.EditIndex = -1;
                BindData();
            }
            if(e.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsAllotmentRemittanceRejection.PVRemittanceDetailsApprove(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(pmvrejectionId)
                    , new Guid(pmvId));

                ucStatus.Text = "Approved Successfully.";
                BindData();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoucherDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;

            BindData();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoucherDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvVoucherDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType== DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            //HeaderCell = new TableCell();
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderCell.ColumnSpan = 1;
            //HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Bank Account";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 1;
            HeaderGridRow.Cells.Add(HeaderCell);
            gvVoucherDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);

            GridViewRow row1 = ((GridViewRow)gvVoucherDetails.Controls[0].Controls[0]);
            row1.Attributes.Add("style", "position:static");
        }
    }
}   