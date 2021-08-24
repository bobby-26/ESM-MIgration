using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsOwnerFundRequestRegister : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            RadCheckBox chkAddress1 = (RadCheckBox)chkSelect;
            if (chkAddress1 != null)
            {
                if (chkAddress1.Checked == true)
                    txtAddress.Enabled = true;
            }

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuOfficeFund.Title = "Owner Fund Request Template";
            MenuOfficeFund.AccessRights = this.ViewState;
            MenuOfficeFund.MenuList = toolbar1.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsOwnerFundRequestRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOwnerFund')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/Accountsownerfundrequestregisterfilter.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsOwnerFundRequestRegister.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuGrid.AccessRights = this.ViewState;
            MenuGrid.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvOwnerFund.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselAccount.aspx', true); ");
            }

            //    BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOwnerFund.SelectedIndexes.Clear();
        gvOwnerFund.EditIndexes.Clear();
        gvOwnerFund.DataSource = null;
        gvOwnerFund.Rebind();
    }
    protected void MenuOfficeFund_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("NEW"))
        {
            clear();
            BindData();

        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {

            if (txtownerfundrequesttemplateid.Text != "")
            {

                try
                {
                    RadCheckBox chkAddress1 = (RadCheckBox)chkSelect;
                    if (chkAddress1 != null)
                    {
                        if (!IsValidData(txtAddress.Text, chkAddress1.Checked == true ? 1 : 0))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegisterUpdate
                                        (
                                        General.GetNullableGuid(txtownerfundrequesttemplateid.Text)
                                        , General.GetNullableInteger(txtAccountId.Text)
                                        , General.GetNullableInteger(ddlCompany.SelectedCompany)
                                        , General.GetNullableInteger(ddlBankreceivingfunds.SelectedValue)
                                        , ""
                                        , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                                        , chkAddress1.Checked == true ? 1 : 0
                                        , chkAddress1.Checked == true ? txtAddress.Text : string.Empty
                                        );

                        ucStatus.Text = "Owner FundRequest Template Updated";
                        Rebind();
                    }
                    //  gvOwnerFund.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }
            else
            {
                try
                {
                    RadCheckBox chkAddress1 = (RadCheckBox)chkSelect;

                    if (chkAddress1 != null)
                    {
                        if (!IsValidData(txtAddress.Text, chkAddress1.Checked == true ? 1 : 0))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegisterinsert
                                    (
                                    General.GetNullableInteger(txtAccountId.Text)
                                    , General.GetNullableInteger(ddlCompany.SelectedCompany)
                                    , General.GetNullableInteger(ddlBankreceivingfunds.SelectedValue)
                                    , ""
                                    , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                                    , chkAddress1.Checked == true ? 1 : 0
                                    , chkAddress1.Checked == true ? txtAddress.Text : string.Empty
                                    );
                        Rebind();

                        ucStatus.Text = "Owner FundRequest Template Created";
                        clear();
                    }
                    //  gvOwnerFund.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }


            }
        }

    }
    protected void MenuGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.Ownerofficefund = null;
                ViewState["PAGENUMBER"] = 1;
                clear();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void clear()
    {
        txtownerfundrequesttemplateid.Text = "";
        txtAccountId.Text = "";
        txtAccountCode.Text = "";
        txtAccountDescription.Text = "";
        //txtDefaultReferenceno.Text = "";
        ddlBankreceivingfunds.SelectedValue = "Dummy";
        ucCurrency.SelectedCurrency = string.Empty;
        imgShowAccount.Enabled = true;
        // txtvesselshortcode.Text = "";
        ddlCompany.SelectedCompany = string.Empty;
        txtAddress.Text = "";
        chkSelect.Checked = false;
        txtAddress.Enabled = false;

    }
    protected void gvOwnerFund_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {


                ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }


            }
            if (e.Item is GridFooterItem)
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                //att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                //    + PhoenixModule.ACCOUNTS + "'); return false;");
                att.Attributes.Add("onclick", "openNewWindow('att','','Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                     + PhoenixModule.ACCOUNTS + "');return true;");


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOwnerFund_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnerFund.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvOwnerFund_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null)
    //            {
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
    //        }

    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                db.Visible = false;
    //        }

    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
    //    if (att != null)
    //    {
    //        att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
    //        if (drv["FLDISATTACHMENT"].ToString() == "0")
    //            att.ImageUrl = Session["images"] + "/no-attachment.png";
    //        att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
    //            + PhoenixModule.ACCOUNTS + "'); return false;");

    //    }
    //}

    protected void ddlCompany_selectedtextchange(object sender, EventArgs e)
    {
        DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlCompany.SelectedCompany), 0);
        ddlBankreceivingfunds.DataSource = dsBankAccount;
        ddlBankreceivingfunds.DataTextField = "FLDBANKACCOUNTNUMBER";
        ddlBankreceivingfunds.DataValueField = "FLDSUBACCOUNTID";
        ddlBankreceivingfunds.DataBind();
        ddlBankreceivingfunds.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    //protected void gvOwnerFund_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}
    protected void gvOwnerFund_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegisterDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblownerfundrequesttemplateid")).Text));
                ucStatus.Text = "Owner Fund Request Template deleted";
                Rebind();



            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                //  GridDataItem item = (GridDataItem)e.Item;
                //  SetRowSelection();

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    //protected void gvOwnerFund_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        int iRowno;
    //        iRowno = int.Parse(e.CommandArgument.ToString());

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {

    //        }
    //        else if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {

    //        }
    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegisterDelete(General.GetNullableGuid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblownerfundrequesttemplateid")).Text));
    //            ucStatus.Text = "Owner Fund Request Template deleted";
    //            clear();

    //        }

    //        else if (e.CommandName.ToUpper().Equals("Edit"))
    //        {

    //            //SetRowSelection();
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

    protected void gvOwnerFund_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gvOwnerFund_OnEditing(object sender, GridCommandEventArgs e)
    {
        // GridView _gridView = (GridView)sender;

        GridDataItem item = (GridDataItem)e.Item;


        RadLabel ownerfundrequesttemplateid = (RadLabel)e.Item.FindControl("lblownerfundrequesttemplateid");
        RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
        RadLabel lblAccountid = (RadLabel)e.Item.FindControl("lblAccountid");
        RadLabel lblCompanyid = (RadLabel)e.Item.FindControl("lblCompanyid");
        RadLabel lblDefaultBankreceivingfundsid = (RadLabel)e.Item.FindControl("lblDefaultBankreceivingfundsid");
        RadLabel lblDefaultReferenceno = (RadLabel)e.Item.FindControl("lblDefaultReferenceno");
        RadLabel lblCurrencyid = (RadLabel)e.Item.FindControl("lblCurrencyid");
        RadLabel lblvesselshortcode = (RadLabel)e.Item.FindControl("lblvesselshortcode");
        RadLabel lblIsActiveBankaccount = (RadLabel)e.Item.FindControl("lblIsActiveBankaccount");

        RadLabel lblChkAddress = (RadLabel)e.Item.FindControl("lblChkAddress");
        RadLabel lblDefAddress = (RadLabel)e.Item.FindControl("lblDefAddress");

        txtAddress.Text = lblDefAddress.Text;
        chkSelect.Checked = lblChkAddress.Text == "1" ? true : false;

        txtAccountId.Text = lblAccountid.Text;
        txtownerfundrequesttemplateid.Text = ownerfundrequesttemplateid.Text;
        txtAccountCode.Text = lblAccountCode.Text;
        ddlCompany.SelectedCompany = lblCompanyid.Text;
        ddlCompany_selectedtextchange(null, null);
        if (General.GetNullableInteger(lblDefaultBankreceivingfundsid.Text) != null)
        {
            if (General.GetNullableInteger(lblIsActiveBankaccount.Text) != 0)
            {
                DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlCompany.SelectedCompany), 0);
                ddlBankreceivingfunds.DataSource = dsBankAccount;
                ddlBankreceivingfunds.DataTextField = "FLDBANKACCOUNTNUMBER";
                ddlBankreceivingfunds.DataValueField = "FLDSUBACCOUNTID";
                ddlBankreceivingfunds.DataBind();
                ddlBankreceivingfunds.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            else
            {
                DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlCompany.SelectedCompany), 1);
                ddlBankreceivingfunds.DataSource = dsBankAccount;
                ddlBankreceivingfunds.DataTextField = "FLDBANKACCOUNTNUMBER";
                ddlBankreceivingfunds.DataValueField = "FLDSUBACCOUNTID";
                ddlBankreceivingfunds.DataBind();
                ddlBankreceivingfunds.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            ddlBankreceivingfunds.SelectedValue = lblDefaultBankreceivingfundsid.Text;
        }

        // txtDefaultReferenceno.Text = lblDefaultReferenceno.Text;
        ucCurrency.SelectedCurrency = lblCurrencyid.Text;
        //        txtvesselshortcode.Text = lblvesselshortcode.Text;
        imgShowAccount.Enabled = false;
        txtAccountCode.ReadOnly = true;
        txtAccountCode.Enabled = false;
        Rebind();

    }


    //protected void gvOwnerFund_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    int nCurrentRow = Int32.Parse(de.NewEditIndex.ToString());

    //    Literal ownerfundrequesttemplateid = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblownerfundrequesttemplateid");
    //    Literal lblAccountCode = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblAccountCode");
    //    Literal lblAccountid = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblAccountid");
    //    Literal lblCompanyid = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblCompanyid");
    //    Literal lblDefaultBankreceivingfundsid = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblDefaultBankreceivingfundsid");
    //    Label lblDefaultReferenceno = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDefaultReferenceno");
    //    Label lblCurrencyid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCurrencyid");
    //    Label lblvesselshortcode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblvesselshortcode");
    //    Literal lblIsActiveBankaccount = (Literal)_gridView.Rows[nCurrentRow].FindControl("lblIsActiveBankaccount");

    //    txtAccountId.Text = lblAccountid.Text;
    //    txtownerfundrequesttemplateid.Text = ownerfundrequesttemplateid.Text;
    //    txtAccountCode.Text = lblAccountCode.Text;
    //    ddlCompany.SelectedCompany = lblCompanyid.Text;
    //    ddlCompany_selectedtextchange(null, null);
    //    if (General.GetNullableInteger(lblDefaultBankreceivingfundsid.Text) != null)
    //    {
    //        if (General.GetNullableInteger(lblIsActiveBankaccount.Text) != 0)
    //        {
    //            DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlCompany.SelectedCompany), 0);
    //            ddlBankreceivingfunds.DataSource = dsBankAccount;
    //            ddlBankreceivingfunds.DataTextField = "FLDBANKACCOUNTNUMBER";
    //            ddlBankreceivingfunds.DataValueField = "FLDSUBACCOUNTID";
    //            ddlBankreceivingfunds.DataBind();
    //            ddlBankreceivingfunds.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //        }
    //        else
    //        {
    //            DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlCompany.SelectedCompany), 1);
    //            ddlBankreceivingfunds.DataSource = dsBankAccount;
    //            ddlBankreceivingfunds.DataTextField = "FLDBANKACCOUNTNUMBER";
    //            ddlBankreceivingfunds.DataValueField = "FLDSUBACCOUNTID";
    //            ddlBankreceivingfunds.DataBind();
    //            ddlBankreceivingfunds.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //        }
    //        ddlBankreceivingfunds.SelectedValue = lblDefaultBankreceivingfundsid.Text;
    //    }

    //    // txtDefaultReferenceno.Text = lblDefaultReferenceno.Text;
    //    ucCurrency.SelectedCurrency = lblCurrencyid.Text;
    //    //        txtvesselshortcode.Text = lblvesselshortcode.Text;
    //    imgShowAccount.Enabled = false;
    //    txtAccountCode.ReadOnly = true;
    //    txtAccountCode.Enabled = false;
    //    BindData();

    //}

    //protected void gvOwnerFund_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

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

    protected void gvOwnerFund_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvOwnerFund.SelectedIndexes.Add(e.NewSelectedIndex);
        SetRowSelection();
        //  BindPageURL(e.NewSelectedIndex);

        //BindPageURL(e.NewSelectedIndex);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCOMPANYNAME", "FLDBANKRECEIVINGFUNDSNAME", "FLDCURRENCYCODE" };
        string[] alCaptions = { "Account", "Description", "Bill To Company", "Default Bank receiving funds", "Default Currency" };

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.Ownerofficefund;
        ds = PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegistersearch(nvc != null ? General.GetNullableString(nvc.Get("txtAccountcode").ToString().Trim()) : null, (int)ViewState["PAGENUMBER"],
                                                         gvOwnerFund.PageSize,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);

        General.SetPrintOptions("gvOwnerFund", "Owner Fund Request Template", alCaptions, alColumns, ds);

        gvOwnerFund.DataSource = ds;
        gvOwnerFund.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindData();

    }


    private bool IsValidData(string address, int chkyn)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        
        if (chkyn == 1 && string.IsNullOrEmpty(address))
            ucError.ErrorMessage = "Address is required.";
        return (!ucError.IsError);
    }


    private void SetRowSelection()
    {
        gvOwnerFund.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvOwnerFund.Items)
        {
            if (item.GetDataKeyValue("FLDOWNERFUNDREQUESTTEMPLATEID").ToString().Equals(ViewState["lblownerfundrequesttemplateid"].ToString()))
            {
                gvOwnerFund.SelectedIndexes.Add(item.ItemIndex);
                gvOwnerFund_OnEditing(gvOwnerFund, null);
            }
        }
    }

    //private void SetRowSelection()
    //{
    //    gvOwnerFund.SelectedIndex = -1;
    //    int i;
    //    for (i = 0; i < gvOwnerFund.Rows.Count; i++)
    //    {
    //        if (gvOwnerFund.DataKeys[i].Value.ToString().Equals(ViewState["lblownerfundrequesttemplateid"].ToString()))
    //        {
    //            gvOwnerFund.SelectedIndex = i;
    //            gvOwnerFund_RowEditing(gvOwnerFund, null);
    //        }
    //    }
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCOMPANYNAME", "FLDBANKRECEIVINGFUNDSNAME", "FLDCURRENCYCODE" };
        string[] alCaptions = { "Account", "Description", "Bill To Company", "Default Bank receiving funds", "Default Currency" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegistersearch(General.GetNullableString(txtAccountCode.Text), (int)ViewState["PAGENUMBER"],
                                                          PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);



        Response.AddHeader("Content-Disposition", "attachment; filename=OwnerFundRequestTemplate.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Owner Fund Request Template</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {

                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

}

