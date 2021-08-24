using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class Accounts_AccountAirfareMarkupRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuAirfare.Title = "Direct Billing";
            MenuAirfare.AccessRights = this.ViewState;
            MenuAirfare.MenuList = toolbar.Show();

            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            toolbarMain.AddButton("Account", "ACCOUNT", ToolBarDirection.Right);
            toolbarMain.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);

            MenuAirfareMain.Title = "Airfare";
            MenuAirfareMain.AccessRights = this.ViewState;
            MenuAirfareMain.MenuList = toolbarMain.Show();
            MenuAirfareMain.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarMainn = new PhoenixToolbar();
            MenuOrderFormMain.Title = "Mark Up & Charged Out";
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarMainn.Show();
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERSUP"] = 1;
                ViewState["FROMAMOUNT"] = 0;
                BindQuick();
                MainData();
                gvSupplier.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvAirfare.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            // BindData();

            // BindSupplierData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvSupplier.SelectedIndexes.Clear();
        gvSupplier.EditIndexes.Clear();
        gvSupplier.DataSource = null;
        gvSupplier.Rebind();
    }
    protected void Rebind1()
    {
        gvAirfare.SelectedIndexes.Clear();
        gvAirfare.EditIndexes.Clear();
        gvAirfare.DataSource = null;
        gvAirfare.Rebind();
    }
    protected void MenuAirfareMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                Response.Redirect("../Accounts/AccountAirfareMarkupRegister.aspx");
            }
            if (CommandName.ToUpper().Equals("ACCOUNT"))
            {
                Response.Redirect("../Accounts/AccountAirfareNonVesselRegister.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuAirfare_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMain(ddlCurrencyCode.SelectedCurrency,
                     txtAmount.Text, txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateMain(int.Parse(ddlCurrencyCode.SelectedCurrency),
                        txtAmount.Text, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany));

                Rebind1();


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    //protected void gvAirfare_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
    //        TableCell tb1 = new TableCell();
    //        TableCell tb2 = new TableCell();
    //        TableCell tb3 = new TableCell();

    //        tb1.ColumnSpan = 2;
    //        tb2.ColumnSpan = 1;
    //        tb3.ColumnSpan = 1;

    //        tb1.Text = "Price Range(USD)";
    //        tb2.Text = "";
    //        tb3.Text = "";

    //        tb1.Attributes.Add("style", "text-align:center");
    //        tb2.Attributes.Add("style", "text-align:center");
    //        tb3.Attributes.Add("style", "text-align:center");

    //        gv.Cells.Add(tb1);
    //        gv.Cells.Add(tb2);
    //        gv.Cells.Add(tb3);
    //        gvAirfare.Controls[0].Controls.AddAt(0, gv);
    //    }

    //}
    protected void gvAirfare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAirfare.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAirfare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidMain(ddlCurrencyCode.SelectedCurrency,
                     txtAmount.Text, txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertAirfare(int.Parse(ddlCurrencyCode.SelectedCurrency),
                    txtAmount.Text, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany),
                    ((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text
                );
                ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Focus();

                UpdateMain(int.Parse(ddlCurrencyCode.SelectedCurrency),
                        txtAmount.Text, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany));

                Rebind1();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                         ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                         ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateAirfare(((RadLabel)e.Item.FindControl("lblMarkupIdEdit")).Text,
                        ((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text
                    );


                Rebind1();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateAirfare(((RadLabel)e.Item.FindControl("lblMarkupIdEdit")).Text,
                        ((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text
                    );


                Rebind1();


            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                GroupAirfare(((RadLabel)e.Item.FindControl("lblMarkupId")).Text);
                Rebind1();
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvAirfare_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidMain(ddlCurrencyCode.SelectedCurrency,
    //                txtAmount.Text, txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            if (!IsValidAirfare(((Label)_gridView.FooterRow.FindControl("lblFromAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtMarkupAmountAdd")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            InsertAirfare(int.Parse(ddlCurrencyCode.SelectedCurrency),
    //                txtAmount.Text, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany),
    //                ((Label)_gridView.FooterRow.FindControl("lblFromAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtMarkupAmountAdd")).Text
    //            );
    //            ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Focus();

    //            UpdateMain(int.Parse(ddlCurrencyCode.SelectedCurrency),
    //                    txtAmount.Text, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany));

    //            BindData();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {

    //            if (!IsValidAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            UpdateAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupIdEdit")).Text,
    //                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text
    //                );

    //            _gridView.EditIndex = -1;
    //            BindData();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            GroupAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupId")).Text);
    //        }
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvAirfare_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}

    //protected void gvAirfare_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator(); 
    //}

    //protected void gvAirfare_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;


    //        if (!IsValidAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        UpdateAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupIdEdit")).Text,
    //                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text
    //            );

    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvAirfare_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindData();
    //    ((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtToAmountEdit")).Focus();
    //    SetPageNavigator();
    //}

    protected void gvAirfare_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
            }
            if (e.Item is GridFooterItem)
            {
                ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                        cmdAdd.Visible = false;
                }
                RadLabel lblFromAmountAdd = (RadLabel)e.Item.FindControl("lblFromAmountAdd");
                if (lblFromAmountAdd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                        cmdAdd.Visible = false;

                    if (ViewState["FROMAMOUNT"].ToString() == "0")
                    {
                        lblFromAmountAdd.Text = "0.00";
                    }
                    else
                    {
                        lblFromAmountAdd.Text = ViewState["FROMAMOUNT"].ToString();
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAirfare_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

    //        ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

    //        ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton cmdAdd = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (cmdAdd != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
    //                cmdAdd.Visible = false;
    //        }
    //    }
    //}

    private void MainData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iNextFromAmount = 0;
        DataSet ds = PhoenixAccountAirfareMarkupRegister.SearchAirfare((int)ViewState["PAGENUMBER"],
                                                        General.ShowRecords(null),
                                                        ref iRowCount,
                                                        ref iTotalPageCount,
                                                        ref iNextFromAmount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ddlCurrencyCode.SelectedCurrency = dr["FLDMARKUPCANCELLATIONCURRENCYID"].ToString(); ;
            txtAmount.Text = string.Format(String.Format("{0:###,###,###.00}", dr["FLDMARKUPCANCELLATIONAMOUNT"]));
            txtMaxPrice.Text = string.Format(String.Format("{0:###,###,###.00}", dr["FLDMAXPRICEINUSD"]));
            ddlBillToCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
            ddlPayingCompany.SelectedCompany = dr["FLDPAYINGCOMPANY"].ToString();
            ucBillToCompanySetting.SelectedValue = dr["FLDBILLTOCOMPANYSETTING"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iNextFromAmount = 0;
        DataSet ds = PhoenixAccountAirfareMarkupRegister.SearchAirfare((int)ViewState["PAGENUMBER"],
                                                         gvAirfare.PageSize,
                                                        ref iRowCount,
                                                        ref iTotalPageCount,
                                                        ref iNextFromAmount);
        gvAirfare.DataSource = ds;
        gvAirfare.VirtualItemCount = iRowCount;

        //GridFooterItem footerItem = (GridFooterItem)gvAirfare.MasterTableView.GetItems(GridItemType.Footer)[0];
        //RadLabel lblFromAmountAdd = (RadLabel)gvAirfare.FindControl ("lblFromAmountAdd");
        if (iNextFromAmount == 0)
        {
            ViewState["FROMAMOUNT"] = 0;
            //lblFromAmountAdd.Text = "0.00";
        }
        else
        {
            ViewState["FROMAMOUNT"] = (iNextFromAmount + Convert.ToDecimal(0.01)).ToString();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void InsertAirfare(int currencyId, string amount, string maxAmount, int companyId, string fromAmount, string toAmount, string markupAmount)
    {
        PhoenixAccountAirfareMarkupRegister.InsertAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            currencyId, Convert.ToDecimal(amount), Convert.ToDecimal(maxAmount), companyId,
            Convert.ToDecimal(fromAmount), Convert.ToDecimal(toAmount), Convert.ToDecimal(markupAmount), int.Parse(ddlPayingCompany.SelectedCompany));
    }

    private void UpdateAirfare(string markupRangeId, string fromAmount, string toAmount, string markupAmount)
    {
        PhoenixAccountAirfareMarkupRegister.UpdateAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
           new Guid(markupRangeId), Convert.ToDecimal(fromAmount), Convert.ToDecimal(toAmount), Convert.ToDecimal(markupAmount));
        ucStatus.Text = "Airfare information updated";
    }

    private void UpdateMain(int currencyId, string amount, string maxAmount, int companyId)
    {
        PhoenixAccountAirfareMarkupRegister.UpdateMain(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            currencyId, Convert.ToDecimal(amount), Convert.ToDecimal(maxAmount), companyId, int.Parse(ddlPayingCompany.SelectedCompany), int.Parse(ucBillToCompanySetting.SelectedValue));
        ucStatus.Text = "Airfare information updated";
    }

    private void GroupAirfare(string markupRangeId)
    {
        PhoenixAccountAirfareMarkupRegister.GroupAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(markupRangeId));
        ucStatus.Text = "Airfare information grouped";
    }



    private bool IsValidMain(string currencyCode, string Amount, string maxAmount, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucBillToCompanySetting.SelectedValue) == null)
            ucError.ErrorMessage = "Bill To Company Setting is required.";

        if (currencyCode.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency code is required.";

        if (Amount.Trim().Equals(""))
            ucError.ErrorMessage = "Invoice amount is required.";

        if (maxAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Max markup price is required.";

        if (company.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableInteger(ddlPayingCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Paying Company is required.";

        return (!ucError.IsError);

    }

    private bool IsValidAirfare(string fromAmount, string toAmount, string markupAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (fromAmount.Trim().Equals(""))
            ucError.ErrorMessage = "From amount is required.";

        if (toAmount.Trim().Equals(""))
            ucError.ErrorMessage = "To amount is required.";

        if (markupAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Markup amount is required.";
        if (fromAmount != "" && toAmount != "")
        {
            if (Convert.ToDecimal(fromAmount) >= Convert.ToDecimal(toAmount))
                ucError.ErrorMessage = "From Amount should be less than To Amount";
        }
        return (!ucError.IsError);
    }

    private bool IsValidAirfare(string fromAmount, string toAmount, string markupAmount, string nextFromAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (fromAmount.Trim().Equals(""))
            ucError.ErrorMessage = "From amount is required.";

        if (toAmount.Trim().Equals(""))
            ucError.ErrorMessage = "To amount is required.";

        if (markupAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Markup amount is required.";

        if (Convert.ToDecimal(fromAmount) >= Convert.ToDecimal(toAmount))
            ucError.ErrorMessage = "From Amount should be less than To Amount";

        if (Convert.ToDecimal(nextFromAmount) <= Convert.ToDecimal(toAmount))
            ucError.ErrorMessage = "Invalid to amount";
        return (!ucError.IsError);
    }

    protected void Invoice_SetExchangeRate(object sender, EventArgs e)
    {
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ddlCurrencyCode.SelectedCurrency));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
            }
        }
    }
    protected void gvSupplier_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

                DataRowView drv = (DataRowView)e.Item.DataItem;
                UserControlCompany ddlCompanyEdit = (UserControlCompany)e.Item.FindControl("ddlCompanyEdit");
                if (ddlCompanyEdit != null) ddlCompanyEdit.SelectedCompany = drv["FLDCOMPANYID"].ToString();

                RadTextBox tbe = (RadTextBox)e.Item.FindControl("txtSupplierNameEdit");
                if (tbe != null) tbe.Attributes.Add("style", "visibility:hidden;");
                tbe = (RadTextBox)e.Item.FindControl("txtSupplierIdEdit");
                if (tbe != null) tbe.Attributes.Add("style", "visibility:hidden;");

                ImageButton btnPickSupplierEdit = (ImageButton)e.Item.FindControl("btnPickSupplierEdit");
                if (btnPickSupplierEdit != null)
                    btnPickSupplierEdit.Attributes.Add("onclick", "return showPickList('spnPickListSupplierEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");

            }
            if (e.Item is GridFooterItem)
            {
                ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                        cmdAdd.Visible = false;
                }
                RadTextBox tba = (RadTextBox)e.Item.FindControl("txtSupplierNameAdd");
                if (tba != null) tba.Attributes.Add("style", "visibility:hidden;");
                tba = (RadTextBox)e.Item.FindControl("txtSupplierIdAdd");
                if (tba != null) tba.Attributes.Add("style", "visibility:hidden;");

                ImageButton btnPickSupplierAdd = (ImageButton)e.Item.FindControl("btnPickSupplierAdd");
                if (btnPickSupplierAdd != null)
                    btnPickSupplierAdd.Attributes.Add("onclick", "return showPickList('spnPickListSupplierAdd', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    //protected void gvSupplier_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
    //        if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

    //        ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

    //        ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        UserControlCompany ddlCompanyEdit = (UserControlCompany)e.Row.FindControl("ddlCompanyEdit");
    //        if (ddlCompanyEdit != null) ddlCompanyEdit.SelectedCompany = drv["FLDCOMPANYID"].ToString();

    //        TextBox tbe = (TextBox)e.Row.FindControl("txtSupplierNameEdit");
    //        if(tbe != null) tbe.Attributes.Add("style", "visibility:hidden;");
    //        tbe = (TextBox)e.Row.FindControl("txtSupplierIdEdit");
    //        if (tbe != null) tbe.Attributes.Add("style", "visibility:hidden;");

    //        ImageButton btnPickSupplierEdit = (ImageButton)e.Row.FindControl("btnPickSupplierEdit");
    //        if (btnPickSupplierEdit != null)
    //            btnPickSupplierEdit.Attributes.Add("onclick", "return showPickList('spnPickListSupplierEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton cmdAdd = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (cmdAdd != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
    //                cmdAdd.Visible = false;
    //        }
    //        TextBox tba = (TextBox)e.Row.FindControl("txtSupplierNameAdd");
    //        if (tba != null) tba.Attributes.Add("style", "visibility:hidden;");
    //        tba = (TextBox)e.Row.FindControl("txtSupplierIdAdd");
    //        if (tba != null) tba.Attributes.Add("style", "visibility:hidden;");

    //        ImageButton btnPickSupplierAdd = (ImageButton)e.Row.FindControl("btnPickSupplierAdd");
    //        if (btnPickSupplierAdd != null) 
    //            btnPickSupplierAdd.Attributes.Add("onclick", "return showPickList('spnPickListSupplierAdd', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");
    //    }
    //}

    //protected void gvSupplier_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = -1;
    //        BindSupplierData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvSupplier_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindSupplierData();
    //    SetPageNavigatorSup();
    //}

    //protected void gvSupplier_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        if (!IsValidSupplier(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierCodeEdit")).Text,
    //            ((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyEdit")).SelectedCompany,
    //            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucIndiaDomesticSectorEdit")).Text,
    //            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucInternationalSectorEdit")).Text,
    //            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucCancellationTicketEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixAccountAirfareMarkupRegister.AirfareSupplierUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , new Guid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblAirfareSupplierIdEdit")).Text)
    //                , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierIdEdit")).Text)
    //                , int.Parse(((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyEdit")).SelectedCompany)
    //                , Convert.ToDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucIndiaDomesticSectorEdit")).Text)
    //                , Convert.ToDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucInternationalSectorEdit")).Text)
    //                , Convert.ToDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucCancellationTicketEdit")).Text));

    //        BindSupplierData();
    //        SetPageNavigatorSup();

    //        _gridView.EditIndex = -1;
    //        BindSupplierData();
    //        SetPageNavigatorSup();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvSupplier_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindSupplierData();
    //    SetPageNavigatorSup();
    //}
    protected void gvSupplier_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidSupplier(((RadTextBox)e.Item.FindControl("txtSupplierCodeAdd")).Text,
                    ((UserControlCompany)e.Item.FindControl("ddlCompanyAdd")).SelectedCompany,
                    ((UserControlNumber)e.Item.FindControl("ucIndiaDomesticSectorAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucInternationalSectorAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucCancellationTicketAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountAirfareMarkupRegister.AirfareSupplierInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(((RadTextBox)e.Item.FindControl("txtSupplierIdAdd")).Text)
                        , int.Parse(((UserControlCompany)e.Item.FindControl("ddlCompanyAdd")).SelectedCompany)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucIndiaDomesticSectorAdd")).Text)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucInternationalSectorAdd")).Text)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucCancellationTicketAdd")).Text));

                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidSupplier(((RadTextBox)e.Item.FindControl("txtSupplierCodeEdit")).Text,
                ((UserControlCompany)e.Item.FindControl("ddlCompanyEdit")).SelectedCompany,
                ((UserControlNumber)e.Item.FindControl("ucIndiaDomesticSectorEdit")).Text,
                ((UserControlNumber)e.Item.FindControl("ucInternationalSectorEdit")).Text,
                ((UserControlNumber)e.Item.FindControl("ucCancellationTicketEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountAirfareMarkupRegister.AirfareSupplierUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((RadLabel)e.Item.FindControl("lblAirfareSupplierIdEdit")).Text)
                        , int.Parse(((RadTextBox)e.Item.FindControl("txtSupplierIdEdit")).Text)
                        , int.Parse(((UserControlCompany)e.Item.FindControl("ddlCompanyEdit")).SelectedCompany)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucIndiaDomesticSectorEdit")).Text)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucInternationalSectorEdit")).Text)
                        , Convert.ToDecimal(((UserControlNumber)e.Item.FindControl("ucCancellationTicketEdit")).Text));

                Rebind();



                //   BindSupplierData();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixAccountAirfareMarkupRegister.AirfareSupplierDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((RadLabel)e.Item.FindControl("lblAirfareSupplierId")).Text));

                Rebind();
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvSupplier_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidSupplier(((TextBox)_gridView.FooterRow.FindControl("txtSupplierCodeAdd")).Text,
    //                ((UserControlCompany)_gridView.FooterRow.FindControl("ddlCompanyAdd")).SelectedCompany,
    //                ((TextBox)_gridView.FooterRow.FindControl("ucIndiaDomesticSectorAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("ucInternationalSectorAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("ucCancellationTicketAdd")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixAccountAirfareMarkupRegister.AirfareSupplierInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                    , int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtSupplierIdAdd")).Text)
    //                    , int.Parse(((UserControlCompany)_gridView.FooterRow.FindControl("ddlCompanyAdd")).SelectedCompany)
    //                    , Convert.ToDecimal(((TextBox)_gridView.FooterRow.FindControl("ucIndiaDomesticSectorAdd")).Text)
    //                    , Convert.ToDecimal(((TextBox)_gridView.FooterRow.FindControl("ucInternationalSectorAdd")).Text)
    //                    , Convert.ToDecimal(((TextBox)_gridView.FooterRow.FindControl("ucCancellationTicketAdd")).Text));

    //            BindSupplierData();
    //            SetPageNavigatorSup();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixAccountAirfareMarkupRegister.AirfareSupplierDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                    , new Guid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblAirfareSupplierId")).Text));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvSupplier_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvSupplier_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplier.CurrentPageIndex + 1;
            BindSupplierData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindSupplierData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixAccountAirfareMarkupRegister.AirfareSupplierList(
                                                        gvSupplier.CurrentPageIndex + 1,
                                                       gvSupplier.PageSize,
                                                        ref iRowCount,
                                                        ref iTotalPageCount);

        gvSupplier.DataSource = ds;
        gvSupplier.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNTSUP"] = iRowCount;
        ViewState["TOTALPAGECOUNTSUP"] = iTotalPageCount;
    }


    private bool IsValidSupplier(string supplierId, string companyId, string indiaDomesticSector, string internationalSector, string cancellationticket)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supplierId.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier code is required.";
        if (General.GetNullableInteger(companyId) == null)
            ucError.ErrorMessage = "Company is required.";
        if (General.GetNullableDecimal(indiaDomesticSector) == null)
            ucError.ErrorMessage = "India domestic sector is required.";
        if (General.GetNullableDecimal(internationalSector) == null)
            ucError.ErrorMessage = "International sector is required.";
        if (General.GetNullableDecimal(cancellationticket) == null)
            ucError.ErrorMessage = "Cancellation ticket is required.";
        return (!ucError.IsError);
    }
    protected void BindQuick()
    {


        ucBillToCompanySetting.DataSource = PhoenixAccountAirfareMarkupRegister.BilltoCompanySettingList(151, "SPL,VSL");
        ucBillToCompanySetting.DataBind();
        ucBillToCompanySetting.AppendDataBoundItems = true;
        ucBillToCompanySetting.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

}
