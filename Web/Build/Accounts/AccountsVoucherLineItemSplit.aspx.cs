using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVoucherLineItemSplit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVoucherLineItemSplit.aspx", "Find", "search.png", "FIND");

            Menu.AccessRights = this.ViewState;
            Menu.MenuList = toolbargrid.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["qLineItemId"] != null && Request.QueryString["qLineItemId"] != string.Empty)
                {
                    ViewState["LineItemId"] = Request.QueryString["qLineItemId"];
                    BindHeader();
                }
                //if (Request.QueryString["qRowno"] != null && Request.QueryString["qRowno"] != string.Empty) ;
                //   Title1.Text = "Row Number (" + Request.QueryString["qRowno"] + ")";

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.Title = "Row Number (" + Request.QueryString["qRowno"] + ")";
                MenuOrderFormMain.MenuList = toolbar.Show();
            }



            gvLineItem.Rebind();
            gvAllocation.Rebind();
            //  BindAllocationData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                // BindData();
                gvLineItem.Rebind();
                //  gvAllocation.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeader()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsVoucher.AllocatedVoucherLineItemEdit(General.GetNullableGuid(ViewState["LineItemId"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVoucherRow.Text = dr["FLDVOUCHERNUMBER"].ToString() + "-" + dr["FLDVOUCHERLINEITEMNO"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString() + "/" + dr["FLDAMOUNT"].ToString();
            txtAmount.Text = dr["FLDBALANCE"].ToString();
        }
    }

    private void BindData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsVoucher.AllotedVoucherSearch(General.GetNullableGuid(ViewState["LineItemId"].ToString()));

        gvLineItem.DataSource = ds;

        gvAllocation.Rebind();
    }

    private void BindAllocationData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixAccountsVoucher.VoucherAllotmentSearch(General.GetNullableGuid(ViewState["LineItemId"].ToString())
                                                                   , txtvoucherno.Text.Trim()
                                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , gvAllocation.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount);

        gvAllocation.DataSource = ds;
        gvAllocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllocation_RowCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                RadLabel lblAllocatingVOucherId = (RadLabel)e.Item.FindControl("lblVoucherLineItemId");
                RadLabel lblAmountRemaining = (RadLabel)e.Item.FindControl("lblAmountUnAllocatedEdit");

                UserControlMaskNumber txtAllocatingAmount = (UserControlMaskNumber)e.Item.FindControl("txtAmount");

                if (decimal.Parse(txtAllocatingAmount.Text) <= 0)
                {
                    ucError.ErrorMessage = "Amount entered should be greater than zero.";
                    ucError.Visible = true;
                    return;
                }

                if (decimal.Parse(txtAllocatingAmount.Text) > Math.Abs(decimal.Parse(lblAmountRemaining.Text)))
                {
                    ucError.ErrorMessage = "Amount entered should be less than remaining balance.";
                    ucError.Visible = true;
                    return;
                }

                if (Math.Abs(decimal.Parse(txtAmount.Text)) < Math.Abs(decimal.Parse(txtAllocatingAmount.Text)))
                {
                    ucError.ErrorMessage = "Amount entered should be less than available balance.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVoucher.VoucherAllotmentInsert(General.GetNullableGuid(ViewState["LineItemId"].ToString()), General.GetNullableGuid(lblAllocatingVOucherId.Text), decimal.Parse(txtAllocatingAmount.Text));
                BindData();
                gvLineItem.Rebind();
                BindHeader();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvAllocation_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllocation_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }

    protected void gvLineItem_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("UPDATELINEITEM"))
            {
                RadLabel lblVoucherAllotmentIdEdit = (RadLabel)e.Item.FindControl("lblVoucherAllotmentIdEdit");

                RadLabel lblTotalAmountEdit = (RadLabel)e.Item.FindControl("lblTotalAmount");

                RadLabel lblAmountRemaining = (RadLabel)e.Item.FindControl("lblAmountRemaining");

                UserControlMaskNumber txtAmountAllocating = (UserControlMaskNumber)e.Item.FindControl("txtAmountAllocated");

                if (decimal.Parse(txtAmountAllocating.Text) <= 0)
                {
                    ucError.ErrorMessage = "Amount entered should be greater than zero.";
                    ucError.Visible = true;
                    return;
                }

                if (Math.Abs(decimal.Parse(lblTotalAmountEdit.Text)) < decimal.Parse(txtAmountAllocating.Text))
                {
                    ucError.ErrorMessage = "Amount entered should be less than remaining balance.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVoucher.VoucherAllotementUpdate(General.GetNullableGuid(lblVoucherAllotmentIdEdit.Text), decimal.Parse(txtAmountAllocating.Text));

                BindHeader();
                BindData();
                gvLineItem.Rebind();
                gvAllocation.Rebind();

            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblVoucherAllotmentId = (RadLabel)e.Item.FindControl("lblVoucherAllotmentId");

                PhoenixAccountsVoucher.VoucherAllotmentDelete(General.GetNullableGuid(lblVoucherAllotmentId.Text));

                BindHeader();
                gvLineItem.Rebind();
                BindAllocationData();
                gvAllocation.Rebind();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvLineItem_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            gvLineItem.Rebind();
            gvAllocation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strallocatingvoucherlineid = new StringBuilder();

                RadGrid gv = (RadGrid)gvAllocation;

                foreach (GridDataItem row in gv.Items)
                {

                    RadCheckBox chk1 = (RadCheckBox)row.FindControl("chkAllocate");

                    if (chk1 != null && chk1.Checked == true)
                    {
                        string strtemp;
                        strtemp = ((RadLabel)row.FindControl("lblVoucherLineItemIdItem")).Text.ToString();
                        strallocatingvoucherlineid.Append(((RadLabel)row.FindControl("lblVoucherLineItemIdItem")).Text.ToString());
                        strallocatingvoucherlineid.Append(",");

                    }
                }
                if (strallocatingvoucherlineid.Length > 1)
                {
                    PhoenixAccountsVoucher.VoucherAllotmentInsertBulk(General.GetNullableGuid(ViewState["LineItemId"].ToString()), General.GetNullableString(strallocatingvoucherlineid.ToString()));
                }
                BindHeader();
                BindData();
                gvLineItem.Rebind();
                BindAllocationData();
                gvAllocation.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllocation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllocation.CurrentPageIndex + 1;
            BindAllocationData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
