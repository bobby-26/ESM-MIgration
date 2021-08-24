using System;
using System.Data;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOfficeDebitCreditNoteGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuDebitCreditNote.AccessRights = this.ViewState;
            MenuDebitCreditNote.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["OfficeDebitCreditNoteId"] = null;
                if (Request.QueryString["OfficeDebitCreditNoteId"] != null)
                    ViewState["OfficeDebitCreditNoteId"] = Request.QueryString["OfficeDebitCreditNoteId"];
                ddlType.DataBind();
                BindCheckBoxList();
                BindBankAccount();
                ViewState["PAGENUMBERLINE"] = 1;
                ViewState["AccountCode"] = "";
                ViewState["VesselId"] = "";
                ViewState["BudgetCode"] = "";
                ViewState["OwnerBudgetCode"] = "";
                ViewState["SubAccountId"] = "";
                OfficeDebitCreditNoteEdit();

                BindProjectCode();
            }
            //BindLineItem();
            BindOwnerBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindProjectCode()
    {
        ucProjectcode.bind(General.GetNullableInteger(rblAccount.SelectedValue),
            General.GetNullableInteger(rblBudgetCode.SelectedValue));
    }

    protected void MenuDebitCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(txtSubject.Text, ddlType.SelectedValue, ddlBillToCompany.SelectedCompany, rblAccount.SelectedValue,
                    ddlBank.SelectedValue, rblBudgetCode.SelectedValue, ucOwnerBudgetCode.SelectedValue, ucDate.Text,
                    ucCurrencyCode.SelectedCurrency, txtReferenceNo.Text, txtAddressee.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["OfficeDebitCreditNoteId"] == null || ViewState["OfficeDebitCreditNoteId"].ToString() == string.Empty)
                {
                    Guid iOfficeDebitCreditId = new Guid();
                    PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , txtSubject.Text
                                                                                        , int.Parse(ddlType.SelectedValue)
                                                                                        , int.Parse(ddlBillToCompany.SelectedCompany)
                                                                                        , int.Parse(ddlBank.SelectedValue)
                                                                                        , int.Parse(rblAccount.SelectedValue)
                                                                                        , General.GetNullableInteger(rblBudgetCode.SelectedValue)
                                                                                        , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue)
                                                                                        , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                                        , Convert.ToDateTime(ucDate.Text)
                                                                                        , txtReferenceNo.Text
                                                                                        , txtAddressee.Text
                                                                                        , ref iOfficeDebitCreditId
                                                                                        , txtVoucherLineItemDesc.Text
                                                                                        , General.GetNullableInteger(ucProjectcode.SelectedProjectCode));
                    ViewState["OfficeDebitCreditNoteId"] = iOfficeDebitCreditId;
                    ucStatus.Text = "Office Debit/Credit note saved.";

                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["OfficeDebitCreditNoteId"].ToString())
                                                                                        , txtSubject.Text
                                                                                        , int.Parse(ddlType.SelectedValue)
                                                                                        , int.Parse(ddlBillToCompany.SelectedCompany)
                                                                                        , int.Parse(ddlBank.SelectedValue)
                                                                                        , int.Parse(rblAccount.SelectedValue)
                                                                                        , General.GetNullableInteger(rblBudgetCode.SelectedValue)
                                                                                        , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue)
                                                                                        , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                                        , Convert.ToDateTime(ucDate.Text)
                                                                                        , txtReferenceNo.Text
                                                                                        , txtAddressee.Text
                                                                                        , txtVoucherLineItemDesc.Text
                                                                                        ,General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                                                                                        );
                    ucStatus.Text = "Office Debit/Credit note updated.";

                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void OfficeDebitCreditNoteEdit()
    {
        try
        {
            if (ViewState["OfficeDebitCreditNoteId"] != null && ViewState["OfficeDebitCreditNoteId"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteEdit(new Guid(ViewState["OfficeDebitCreditNoteId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];
                DataSet dsBankAccount = new DataSet();
               
                if (General.GetNullableInteger(dr["FLDACTIVEYN"].ToString()) != 0)
                {
                     dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDBILLTOCOMPANY"].ToString()),0);
                }
                else
                {
                     dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDBILLTOCOMPANY"].ToString()),1);
                }
                
                ddlBank.DataSource = dsBankAccount;
                ddlBank.DataBind();
                ddlBank.DataTextField = "FLDBANKACCOUNTNUMBER";
                ddlBank.DataValueField = "FLDSUBACCOUNTID";
                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                string temp = dr["FLDBUDGETID"].ToString().Equals("") ? "Dummy" : dr["FLDBUDGETID"].ToString();

                ddlType.SelectedValue = dr["FLDTYPE"].ToString();
                txtSubject.Text = dr["FLDSUBJECT"].ToString();
                ddlBillToCompany.SelectedCompany = dr["FLDBILLTOCOMPANY"].ToString();
                rblAccount.SelectedValue = dr["FLDACCOUNTID"].ToString();
                ViewState["ACCOUNTID"] = dr["FLDACCOUNTID"].ToString();
                txtAccountSearch.Text = rblAccount.SelectedItem.ToString();
                ddlBank.SelectedValue = dr["FLDBANKID"].ToString();
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucDate.Text = dr["FLDDATE"].ToString();
                ucCurrencyCode.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                txtReferenceNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtAddressee.Text = dr["FLDADDRESSEE"].ToString();
                txtVoucherLineItemDesc.Text = dr["FLDVOUCHERLONGDESCRIPTION"].ToString();
                ucProjectcode.SelectedProjectCode = dr["FLDPROJECTID"].ToString();
                rblBudgetCode.Items.Clear();
                DataSet ds1 = PhoenixAccountsOfficeDebitCreditNoteGenerate.SubAccountSearchList(General.GetNullableInteger(ViewState["ACCOUNTID"].ToString()), General.GetNullableString(txtBudgetCodeSearch.Text));
                bindbudgetcode(ds1);

                if (ds1.Tables[0].Rows.Count > 0 && General.GetNullableInteger(dr["FLDBUDGETID"].ToString()) != null)
                {
                    rblBudgetCode.SelectedValue = dr["FLDBUDGETID"].ToString();
                    txtBudgetCodeSearch.Text = rblBudgetCode.SelectedItem.Text;
                }
                ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
                ViewState["BudgetCode"] = dr["FLDBUDGETID"].ToString().Equals("") ? "" : dr["FLDBUDGETID"].ToString();
                Getprincipal(Convert.ToInt32(dr["FLDACCOUNTID"]));
                if (dr["FLDOWNERBUDGETCODEID"].ToString() != "")
                {
                    ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                    ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
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

    private bool IsValidData(string subject, string type, string billingCompany, string account, string bank, string budgetId, string ownerBudgetCode,string date, string currency, string referenceNo, string addressee)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (subject.Trim().Equals(""))
            ucError.ErrorMessage = "Subject is required.";
        if (type.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (billingCompany.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Billing company is required.";
        if (account.Trim().Equals(""))
            ucError.ErrorMessage = "Account is required.";
        if (bank.Trim().Equals(""))
            ucError.ErrorMessage = "Bank receiving funds is required.";
        //if (rblBudgetCode.Items.Count> 0)
        //{
        //if (General.GetNullableInteger(budgetId) == null)
        //    ucError.ErrorMessage = "Sub Account is required.";
        //}
        //if (ViewState["OwnerBudgetCode"].ToString() != "")
        //{
        //    if (ownerBudgetCode.Trim().Equals(""))
        //        ucError.ErrorMessage = "Owner budget code is required.";
        //}
        //if (ownerBudgetCode.Trim().Equals(""))
        //    ucError.ErrorMessage = "Owner budget code is required.";
        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        if (referenceNo.Trim().Equals(""))
            ucError.ErrorMessage = "Reference no. is required.";
        if (addressee.Trim().Equals(""))
            ucError.ErrorMessage = "Addressee is required.";

        return (!ucError.IsError);
    }

    private bool IsValidLineItem(string description, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }

    protected void ddlBillToCompany_Changed(object sender, EventArgs e)
    {
        //ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlBillToCompany.SelectedCompany));
        //ddlBank.DataBind();
        DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlBillToCompany.SelectedCompany),0);
        ddlBank.DataSource = dsBankAccount;
        ddlBank.DataTextField = "FLDBANKACCOUNTNUMBER";
        ddlBank.DataValueField = "FLDSUBACCOUNTID";
        ddlBank.DataBind();
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

    }
    protected void BindBankAccount()
    {
        DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlBillToCompany.SelectedCompany),0);
        ddlBank.DataSource = dsBankAccount;
        ddlBank.DataTextField = "FLDBANKACCOUNTNUMBER";
        ddlBank.DataValueField = "FLDSUBACCOUNTID";
        ddlBank.DataBind();
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public void ResetAll()
    {
        try
        {
            txtSubject.Text = string.Empty;
            ddlType.SelectedValue = string.Empty;
            ddlBillToCompany.SelectedCompany = string.Empty;
            rblAccount.SelectedValue = string.Empty;
            ddlBank.SelectedValue = string.Empty;
            rblBudgetCode.SelectedValue = string.Empty;
            ucOwnerBudgetCode.SelectedValue = string.Empty;
            ucDate.Text = string.Empty;
            ucCurrencyCode.SelectedCurrency = string.Empty;
            txtReferenceNo.Text = string.Empty;
            txtAddressee.Text = string.Empty;
            ucProjectcode.SelectedProjectCode = string.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlType_DataBound(object sender, EventArgs e)
    {
        ddlType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
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

            if (ViewState["OfficeDebitCreditNoteId"] != null && ViewState["OfficeDebitCreditNoteId"].ToString() != string.Empty)
            {
                if (cmdEdit != null) cmdEdit.Visible = true;
                if (cmdDelete != null) cmdDelete.Visible = true;
            }
            else
            {
                if (cmdEdit != null) cmdEdit.Visible = false;
                if (cmdDelete != null) cmdDelete.Visible = false;
            }
        }

        if (e.Item is GridFooterItem)
        {
            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);

            if (ViewState["OfficeDebitCreditNoteId"] != null && ViewState["OfficeDebitCreditNoteId"].ToString() != "")
            {
                if (cmdAdd != null) cmdAdd.Visible = true;
            }
            else
            {
                if (cmdAdd != null) cmdAdd.Visible = false;
            }
        }
    }
    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteLineItemInsert(new Guid(ViewState["OfficeDebitCreditNoteId"].ToString())
                     , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Line item inserted";

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteLineItemUpdate(new Guid(((RadLabel)e.Item.FindControl("lblLineItemIdEdit")).Text)
                     , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Travel claim line item updated";

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteLineItemDelete(new Guid(((RadLabel)e.Item.FindControl("lblLineItemId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ucStatus.Text = "Deleted Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteLineItemList(General.GetNullableGuid(ViewState["OfficeDebitCreditNoteId"].ToString()),
                                                                                                        (int)ViewState["PAGENUMBERLINE"],
                                                                                                        gvLineItem.PageSize,
                                                                                                        ref iRowCount,
                                                                                                        ref iTotalPageCount);

                gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTLINE"] = iRowCount;
            ViewState["TOTALPAGECOUNTLINE"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetPageNavigator()
    //{
    //    cmdPreviousLine.Enabled = IsPreviousEnabledLine();
    //    cmdNextLine.Enabled = IsNextEnabledLine();
    //    lblPagenumberLine.Text = "Page " + ViewState["PAGENUMBERLINE"].ToString();
    //    lblPagesLine.Text = " of " + ViewState["TOTALPAGECOUNTLINE"].ToString() + " Pages. ";
    //    lblRecordsLine.Text = "(" + ViewState["ROWCOUNTLINE"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabledLine()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERLINE"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTLINE"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledLine()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERLINE"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTLINE"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGoLine_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopageLine.Text, out result))
    //    {
    //        ViewState["PAGENUMBERLINE"] = Int32.Parse(txtnopageLine.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTLINE"] < Int32.Parse(txtnopageLine.Text))
    //            ViewState["PAGENUMBERLINE"] = ViewState["TOTALPAGECOUNTLINE"];


    //        if (0 >= Int32.Parse(txtnopageLine.Text))
    //            ViewState["PAGENUMBERLINE"] = 1;

    //        if ((int)ViewState["PAGENUMBERLINE"] == 0)
    //            ViewState["PAGENUMBERLINE"] = 1;

    //        txtnopageLine.Text = ViewState["PAGENUMBERLINE"].ToString();
    //    }
    //    BindLineItem();
    //}

    //protected void PagerButtonClickLine(object sender, CommandEventArgs ce)
    //{
    //    gvLineItem.SelectedIndex = -1;
    //    gvLineItem.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBERLINE"] = (int)ViewState["PAGENUMBERLINE"] - 1;
    //    else
    //        ViewState["PAGENUMBERLINE"] = (int)ViewState["PAGENUMBERLINE"] + 1;
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



    private void BindOwnerBudgetCode()
    {
        if (Convert.ToString(ViewState["PRINCIPALID"]) != "" && ViewState["BudgetCode"].ToString() != "")
        {
            //ucOwnerBudgetCode.VesselId = ViewState["VesselId"].ToString();
            ucOwnerBudgetCode.OwnerId = ViewState["PRINCIPALID"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(Convert.ToString(ViewState["PRINCIPALID"]))
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["BudgetCode"].ToString())
                                                                                     , null, null
                                                                                     , 1
                                                                                     , General.ShowRecords(null)
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , ref iownerid);

            if (ds1.Tables[0].Rows.Count > 0)
                ViewState["OwnerBudgetCode"] = "1";
            else
                ViewState["OwnerBudgetCode"] = "";
            if (ds1.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }
        }
    }



    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        BindCheckBoxList();
        ViewState["ACCOUNTID"] = rblAccount.SelectedValue.ToString();
        budgetcodeseach();
    }

    protected void BindCheckBoxList()
    {
        DataSet ds = new DataSet();

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            ds = PhoenixRegistersAccount.AccountListforReport(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , txtAccountSearch.Text
                , null
                , null
                , null
                , null
                , 1
                , null, null,
                1,
                1000,
                ref iRowCount,
                ref iTotalPageCount);

            ds.Tables[0].Columns.Add("FLDaccoandept");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDaccoandept"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];

            }

            rblAccount.DataTextField = "FLDaccoandept";
            rblAccount.DataValueField = "FLDACCOUNTID";
            rblAccount.DataSource = ds;
            rblAccount.DataBind();
            if (ViewState["ACCOUNTID"] != null && ViewState["ACCOUNTID"].ToString() != string.Empty)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    if ((dr["FLDACCOUNTID"].ToString().Trim()) == (ViewState["ACCOUNTID"].ToString().Trim()))
                    {
                        rblAccount.SelectedValue = ViewState["ACCOUNTID"].ToString();
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



    protected void SelectAccount()
    {
        string str = "," + ViewState["ACCOUNT"].ToString();

        foreach (ListItem item in rblAccount.Items)
        {
            if (str.Contains("," + item.Value.ToString() + ","))
            {
                item.Selected = true;
            }
        }
    }

    private void bindbudgetcode(DataSet ds)
    {
        rblBudgetCode.DataTextField = "FLDSUBACCOUNT";
        rblBudgetCode.DataValueField = "FLDBUDGETID";
        rblBudgetCode.DataSource = ds;
        rblBudgetCode.DataBind();
    }
    private void budgetcodeseach()
    {
        DataSet ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.SubAccountSearchList(General.GetNullableInteger(rblAccount.SelectedValue), General.GetNullableString(txtBudgetCodeSearch.Text));
        bindbudgetcode(ds);
    }
    protected void cmdSearchSubAccount_Click(object sender, EventArgs e)
    {
        budgetcodeseach();
    }

    protected void SubAccountSelection(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ucProjectcode.SelectedProjectCode = "";
        ucProjectcode.SelectedText = "";
        ViewState["BudgetCode"] = rblBudgetCode.SelectedValue;
        txtBudgetCodeSearch.Text = rblBudgetCode.SelectedItem.Text;
        try
        {
            
            BindOwnerBudgetCode();
            BindProjectCode();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPALID"] = Convert.ToString(dr["FLDPRINCIPALID"]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AccountSelection(object sender, EventArgs e)
    {
       

        try
        {
            rblBudgetCode.Items.Clear();

            ViewState["AccountCode"] = rblAccount.SelectedItem.ToString().Substring(0, 7);
            txtAccountSearch.Text = rblAccount.SelectedItem.ToString();

            budgetcodeseach();

            ViewState["BudgetCode"] = rblBudgetCode.SelectedValue;
            Getprincipal(Convert.ToInt32(rblAccount.SelectedValue));
            BindProjectCode();

           // BindOwnerBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
        BindLineItem();
    }
}
