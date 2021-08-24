using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsCashOutRequestLineItem : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["popup"] == null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("LineItems", "LINEITEMS", ToolBarDirection.Right);
                toolbarmain.AddButton("Cash Request", "CASHREQ", ToolBarDirection.Right);

                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.MenuList = toolbarmain.Show();
                MenuOrderFormMain.SelectedMenuIndex = 0;
            }
            else
            {
                PhoenixToolbar toolbarApprove = new PhoenixToolbar();
                toolbarApprove.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                MenuApprove.AccessRights = this.ViewState;
                MenuApprove.MenuList = toolbarApprove.Show();
            }

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtSupplierId.Attributes.Add("style", "visibility:hidden");
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

                ViewState["batched"] = "";
                if ((Request.QueryString["batched"] != null))
                {
                    ViewState["batched"] = "1";
                    //  divHeaderDetails.Visible = false;
                    BindHeader(ViewState["cashpaymentid"].ToString());
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }
    private void BindHeader(string cashpaymentid)
    {
        DataSet ds = PhoenixAccountsCashOut.CashOutEdit(cashpaymentid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString() + " / " + dr["FLDCASHPAYMENTAMOUNT"].ToString();
            txtCashPaymentNumber.Text = dr["FLDCASHPAYMENTNUMBER"].ToString();
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
            txtCashAccountCurDescription.Text = dr["FLDDESCRIPTION"].ToString();
        }
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

        if (ViewState["batched"].ToString() == "1")
        {
            ds = PhoenixAccountsCashOut.CashOutBatchedPaymentVoucherSearch(General.GetNullableString(ViewState["cashpaymentid"].ToString()), "", null, null
                                                , string.Empty, string.Empty
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , gvVoucherDetails.PageSize
                                                , ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsCashOut.CashOutPaymentVoucherSearch(General.GetNullableString(ViewState["cashpaymentid"].ToString()), "", null, null
                                                , string.Empty, string.Empty
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , gvVoucherDetails.PageSize
                                                , ref iRowCount, ref iTotalPageCount);
        }


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {



        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CASHREQ"))
            {
                Response.Redirect("../Accounts/AccountsCashOutRequest.aspx?cashpaymentid=" + ViewState["cashpaymentid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuApprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (ViewState["batched"].ToString() == "1")
                    PhoenixAccountsCashOut.CashOutApprovedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "," + ViewState["cashpaymentid"].ToString() + ",", 0);
                else
                    PhoenixAccountsCashOut.CashOutApprovedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "," + ViewState["cashpaymentid"].ToString() + ",", 1);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoucherDetails_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
    protected void gvVoucherDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoucherDetails.CurrentPageIndex + 1;
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
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //  Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblpaymentvoucher")).Text);
                PhoenixAccountsCashOut.CashOutLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["cashpaymentid"].ToString()), id);
                Rebind();
                BindHeader(ViewState["cashpaymentid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


        }
    }
 


    //protected void gvVoucherDetails_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = de.RowIndex;
    //    try
    //    {
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        PhoenixAccountsCashOut.CashOutLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["cashpaymentid"].ToString()), id);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //    BindHeader(ViewState["cashpaymentid"].ToString());
    //}

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




}
