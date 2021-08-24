using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsOwnerOfficeSingleDepartment : PhoenixBasePage
{
    public string strCurrency = string.Empty;
    public string strAmountTotal = string.Empty;
    public decimal totalAmount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Repost", "REPOST", ToolBarDirection.Right);
            MenuSingleDepartment.AccessRights = this.ViewState;
            MenuSingleDepartment.MenuList = toolbar.Show();

            PhoenixToolbar toolbarPost = new PhoenixToolbar();
            toolbarPost.AddButton("Post", "POST", ToolBarDirection.Right);
            MenuPost.AccessRights = this.ViewState;
            MenuPost.MenuList = toolbarPost.Show();

            if (!IsPostBack)
            {
                ViewState["OwnerOfficeFundId"] = "";
                if (Request.QueryString["OwnerOfficeFundId"] != null)
                    ViewState["OwnerOfficeFundId"] = Request.QueryString["OwnerOfficeFundId"];
                DataSet ds = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 0);
                ddlBankAccount.DataSource = ds;
                ddlBankAccount.DataTextField = "FLDBANKACCOUNTNUMBER";
                ddlBankAccount.DataValueField = "FLDSUBACCOUNTID";
                ddlBankAccount.DataBind();
                ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ViewState["PAGENUMBER"] = 1;
                FundReceivedVoucherEdit();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerOfficeSingleDepartment.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundAllocate')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerOfficeSingleDepartment.aspx", "Add", "add.png", "ADD");
            //toolbargrid.AddImageLink("javascript:OpenPopup('codehelp1','','../Accounts/AccountsOwnerOfficeSingleDepartmentAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "');", "Add", "add.png", "ADD");
            MenuSingleDepartmentGrid.AccessRights = this.ViewState;
            MenuSingleDepartmentGrid.MenuList = toolbargrid.Show();

            gvFundAllocate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSingleDepartment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            Guid strOwnerOfficeFundID = new Guid();
            if (CommandName.ToUpper().Equals("REPOST"))
            {
                if (ViewState["OwnerOfficeFundId"].ToString() != string.Empty && ViewState["OwnerOfficeFundId"] != null)
                {
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherRepost(new Guid(ViewState["OwnerOfficeFundId"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    FundReceivedVoucherEdit();
                    ResetAll();
                    Rebind();
                    ucStatus.Text = "Bank receipt voucher Reposted.";
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

                }

            }
            if (CommandName.ToUpper().Equals("NEW"))
            {

                ResetAll();
                DataSet ds = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 0);
                ddlBankAccount.DataSource = ds;
                ddlBankAccount.DataTextField = "FLDBANKACCOUNTNUMBER";
                ddlBankAccount.DataValueField = "FLDSUBACCOUNTID";
                ddlBankAccount.DataBind();
                Rebind();
                ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["OwnerOfficeFundId"].ToString() != string.Empty && ViewState["OwnerOfficeFundId"] != null)
                {
                    if (!IsValidData(ddlBankAccount.SelectedValue, ddlSource.SelectedValue, ucDate.Text, ucAmount.Text, txtReference.Text, txtLongDescription.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                         , new Guid(ViewState["OwnerOfficeFundId"].ToString())
                                                                                         , General.GetNullableDateTime(ucDate.Text)
                                                                                         , General.GetNullableDecimal(ucAmount.Text)
                                                                                         , txtReference.Text
                                                                                         , txtLongDescription.Text
                                                                                         , int.Parse(ddlSource.SelectedValue)
                                                                                         , int.Parse(ddlBankAccount.SelectedValue));
                    ucStatus.Text = "Updated successfully.";
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    //   Session["New"] = "Y";
                }
                else
                {
                    if (!IsValidData(ddlBankAccount.SelectedValue, ddlSource.SelectedValue, ucDate.Text, ucAmount.Text, txtReference.Text, txtLongDescription.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                         , int.Parse(ddlBankAccount.SelectedValue)
                                                                                         , int.Parse(ddlSource.SelectedValue)
                                                                                         , General.GetNullableDateTime(ucDate.Text)
                                                                                         , General.GetNullableDecimal(ucAmount.Text)
                                                                                         , txtReference.Text
                                                                                         , txtLongDescription.Text
                                                                                         , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                         , ref strOwnerOfficeFundID);
                    ucStatus.Text = "Inserted successfully.";
                    String scriptupdate = String.Format("var cmdbutton = parent.document.getElementById('hiddenkey'); if (cmdbutton != null) cmdbutton.value = '" + strOwnerOfficeFundID.ToString() + "';");
                    scriptupdate = scriptupdate + String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }

            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(ddlBankAccount.SelectedValue, ddlSource.SelectedValue, ucDate.Text, ucAmount.Text, txtReference.Text, txtLongDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                     , int.Parse(ddlBankAccount.SelectedValue)
                                                                                     , int.Parse(ddlSource.SelectedValue)
                                                                                     , General.GetNullableDateTime(ucDate.Text)
                                                                                     , General.GetNullableDecimal(ucAmount.Text)
                                                                                     , txtReference.Text
                                                                                     , txtLongDescription.Text
                                                                                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                     , ref strOwnerOfficeFundID);
                ucStatus.Text = "Inserted successfully.";
                String scriptupdate = String.Format("var cmdbutton = parent.document.getElementById('hiddenkey'); if (cmdbutton != null) cmdbutton.value = '" + strOwnerOfficeFundID.ToString() + "';");
                scriptupdate = scriptupdate + String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                Session["New"] = "Y";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSingleDepartmentGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ViewState["PAGENUMBER"] = 1;
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                //Response.Redirect("../Accounts/AccountsOwnerOfficeSingleDepartmentAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString());
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "&Ispopup=');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidPost(ucbankchargeamtusd.Text, txtVoucherStatus.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableDecimal(ucbankchargeamtusd.Text) > 50)
                {
                    if (txtCurrency.Text == "USD")
                    {
                        RadWindowManager1.RadConfirm("Bank Charges incurred is USD" + " " + ucBankCharges.Text + " " +". Please confirm to proceed.", "DeleteRecord", 320, 150, null, "Confirm");
                        return;
                    }
                    else
                    {
                      
                        RadWindowManager1.RadConfirm("Bank Charges incurred is " + " " + txtCurrency.Text + " " + ucBankCharges.Text +" "+ "equivalent of USD" + " "+ ucbankchargeamtusd.Text + " " +". Please confirm to proceed.", "DeleteRecord", 320, 150, null, "Confirm");
                        return;
                    }
                }
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentPost.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void FundReceivedVoucherEdit()
    {
        try
        {
            if (ViewState["OwnerOfficeFundId"].ToString() != string.Empty && ViewState["OwnerOfficeFundId"] != null)
            {
                DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherEdit(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    DataSet dsBankAccount = new DataSet();
                    //ddlBankAccount.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null,General.GetNullableInteger(dr["FLDCOMPANYID"].ToString()));
                    if (General.GetNullableInteger(dr["FLDISBANKACCOUNTACTIVE"].ToString()) == 0)
                    {
                        dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDCOMPANYID"].ToString()), 1);
                    }
                    else
                    {
                        dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDCOMPANYID"].ToString()), 0);
                    }
                    ddlBankAccount.DataSource = dsBankAccount;
                    ddlBankAccount.DataTextField = "FLDBANKACCOUNTNUMBER";
                    ddlBankAccount.DataValueField = "FLDSUBACCOUNTID";
                    ddlBankAccount.DataBind();
                    ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                    ddlBankAccount.SelectedValue = dr["FLDBANKACCOUNTID"].ToString();
                    ddlSource.SelectedValue = dr["FLDSOURCE"].ToString();
                    ucDate.Text = dr["FLDDATE"].ToString();
                    txtVoucherStatus.Text = dr["FLDVOUCHERSTATUS"].ToString();
                    txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                    strCurrency = dr["FLDCURRENCYCODE"].ToString();
                    ucAmount.Text = dr["FLDAMOUNT"].ToString();
                    txtReference.Text = dr["FLDREFERENCE"].ToString();
                    ucBankCharges.Text = dr["FLDBANKCHARGES"].ToString();
                    txtLongDescription.Text = dr["FLDDESCRIPTION"].ToString();
                    ViewState["OwnerOfficeFundId"] = dr["FLDOWNEROFFICEFUNDID"].ToString();
                    ucbankchargeamtusd.Text = dr["FLDBANKCHARGEAMOUNTUSD"].ToString();


                    if (dr["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 56, "Drft"))
                    {
                        ddlSource.Enabled = true;
                        ddlBankAccount.Enabled = true;
                    }
                    else
                    {
                        ddlSource.Enabled = false;
                        ddlBankAccount.Enabled = false;
                    }
                }
            }
            else
            {
                ResetAll();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPost(string ucbankchargeamtusd, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableDecimal(ucbankchargeamtusd) < 0 || General.GetNullableDecimal(ucbankchargeamtusd) > 50)
        //  ucError.ErrorMessage = "Bank Charge should be between 0 and 50.";
        if (status != "Draft")
            ucError.ErrorMessage = "Voucher should be Draft status.";
        return (!ucError.IsError);
    }
    protected void ucConfirmmsg_Click(object sender, EventArgs e)
    {
        try
        {
            String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentPost.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidData(string BankAccount, string Source, string Date, string Amount, string Reference, string LongDescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (BankAccount.Trim().Equals(""))
            ucError.ErrorMessage = "Bank account is required.";
        if (Source.Trim().Equals(""))
            ucError.ErrorMessage = "Source is required.";
        if (General.GetNullableDateTime(Date) == null)
            ucError.ErrorMessage = "Date is required.";
        if (Amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (Reference.Trim().Equals(""))
            ucError.ErrorMessage = "Reference is required.";
        if (LongDescription.Trim().Equals(""))
            ucError.ErrorMessage = "Long Description is required.";
        return (!ucError.IsError);
    }

    private bool IsValidLineItem(string description, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Allocated amount is required.";

        return (!ucError.IsError);
    }
    public void ResetAll()
    {
        try
        {
            ddlSource.SelectedValue = string.Empty;
            ucDate.Text = string.Empty;
            txtVoucherStatus.Text = string.Empty;
            txtCurrency.Text = string.Empty;
            ucAmount.Text = string.Empty;
            txtReference.Text = string.Empty;
            ucBankCharges.Text = string.Empty;
            txtLongDescription.Text = string.Empty;
            ViewState["OwnerOfficeFundId"] = "";
            ddlBankAccount.Enabled = true;
            ddlSource.Enabled = true;
            totalAmount = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvFundAllocate_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
    }

    protected void gvFundAllocate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
        }
        if (e.Item is GridDataItem)
        {
            RadLabel lblCurrencyCode = (RadLabel)e.Item.FindControl("lblCurrencyCode");
            RadLabel lblOwnerOfficeAllocateId = (RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId");
            ImageButton cmdConvert = (ImageButton)e.Item.FindControl("cmdConvert");
            RadLabel lblAllocatedAmountBankCur = (RadLabel)e.Item.FindControl("lblAllocatedAmountBankCur");
            RadLabel lblAllocatedAmount = (RadLabel)e.Item.FindControl("lblAllocatedAmount");

            if (cmdConvert != null)
            {
                cmdConvert.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentConvertCurrency.aspx?OwnerOfficeAllocateId=" + lblOwnerOfficeAllocateId.Text +
                    "&BankCurrency=" + txtCurrency.Text + "&Currency=" + lblCurrencyCode.Text + "&AllocatedAmount=" + lblAllocatedAmount.Text + "&convertedAmount=" + lblAllocatedAmountBankCur.Text + "'); return true;");
            }

            if (txtCurrency.Text != lblCurrencyCode.Text && txtVoucherStatus.Text == "Draft")
            {
                if (cmdConvert != null) cmdConvert.Visible = true;
            }
            else
            {
                if (cmdConvert != null) cmdConvert.Visible = false;
            }
            totalAmount = totalAmount + Convert.ToDecimal(lblAllocatedAmountBankCur.Text == "" ? "0" : lblAllocatedAmountBankCur.Text);
            strAmountTotal = totalAmount.ToString();
        }
    }
    protected void gvFundAllocate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFundAllocate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFundAllocate_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        Rebind();
    }

    protected void gvFundAllocate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            //   int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("lblDescriptionEdit")).Text
                                        , ((UserControlMaskNumber)e.Item.FindControl("ucAllocatedAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(((RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId")).Text)
                     , ((RadTextBox)e.Item.FindControl("lblDescriptionEdit")).Text
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAllocatedAmountEdit")).Text));
                ucStatus.Text = "Credit/Debit note line item updated";

                //String script = "javascript:fnReloadList('codehelp1',null,'true');";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                Rebind();
                FundReceivedVoucherEdit();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateDelete(new Guid(((RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //String script = "javascript:fnReloadList('codehelp1',null,'true');";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                totalAmount = 0;
                Rebind();
                ucStatus.Text = "Deleted Successfully";
                FundReceivedVoucherEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDLONGDESCRIPTION", "FLDCURRENCY", "FLDALLOCATIONAMOUNT", "FLDALLOCATIONAMOUNTBANKCUR" };
            string[] alCaptions = { "Reference No", "Description", "Currency", "Allocated Amount", "Allocated Amount(" + txtCurrency.Text + ")" };

            DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                        gvFundAllocate.CurrentPageIndex + 1,
                                                                                                        gvFundAllocate.PageSize,
                                                                                                        ref iRowCount,
                                                                                                        ref iTotalPageCount);

            General.SetPrintOptions("gvFundAllocate", "Allocate List", alCaptions, alColumns, ds);

            gvFundAllocate.DataSource = ds;
            gvFundAllocate.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                totalAmount = 0;
            }
            else
            {

                DataTable dt = ds.Tables[0];
                //  ShowNoRecordsFound(dt, gvFundAllocate);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvFundAllocate.EditIndex = -1;
    //    gvFundAllocate.SelectedIndex = -1;
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvFundAllocate.EditIndex = -1;
    //    gvFundAllocate.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvFundAllocate.SelectedIndex = -1;
    //    gvFundAllocate.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDLONGDESCRIPTION", "FLDCURRENCY", "FLDALLOCATIONAMOUNT", "FLDALLOCATIONAMOUNTBANKCUR" };
        string[] alCaptions = { "Reference No", "Description", "Currency", "Allocated Amount", "Allocated Amount(" + txtCurrency.Text + ")" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                    (int)ViewState["PAGENUMBER"],
                                                                                                    PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                                    ref iRowCount,
                                                                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AllocateList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allocate List</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void Rebind()
    {
        gvFundAllocate.SelectedIndexes.Clear();
        gvFundAllocate.EditIndexes.Clear();
        gvFundAllocate.DataSource = null;
        gvFundAllocate.Rebind();
    }

    protected void gvFundAllocate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFundAllocate.CurrentPageIndex + 1;
        BindData();
    }
}
