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

public partial class Accounts_AccountsProjectCodeDebitCreditNoteGenerateShipOwners : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDebitCreditNote.AccessRights = this.ViewState;
            MenuDebitCreditNote.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["OwnerDebitCreditNoteId"] = null;
                ViewState["PROJECTID"] = null;

                if (Request.QueryString["OwnerDebitCreditNoteId"] != null)
                {
                    ViewState["OwnerDebitCreditNoteId"] = Request.QueryString["OwnerDebitCreditNoteId"];
                }
                if (Request.QueryString["ID"] != null)
                    ViewState["PROJECTID"] = Request.QueryString["ID"];

                ddlType.DataBind();
                ddlSubtype.DataBind();
                ddlVessel.DataBind();
                ViewState["PAGENUMBERLINE"] = 1;
                ViewState["VesselId"] = "";
                ViewState["PRINCIPAL"] = "";
                ViewState["BudgetCode"] = "Dummy";
                OwnerDebitCreditNoteEdit();

                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindLineItem();
            BindOwnerBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDebitCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ddlType.SelectedValue, ddlVessel.SelectedValue, ddlBillToCompany.SelectedCompany,
                    ucDate.Text, ddlBank.SelectedValue, txtReferenceNo.Text, ucBudgetCode.SelectedBudgetCode,
                    ucOwnerBudgetCode.SelectedValue, ddlOwner.SelectedAddress, ucCurrencyCode.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["OwnerDebitCreditNoteId"] == null || ViewState["OwnerDebitCreditNoteId"].ToString() == string.Empty)
                {
                    Guid iOwnerDebitCreditId = new Guid();
                    PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteInsert(int.Parse(ddlType.SelectedValue)
                                                                                        , int.Parse(ViewState["VesselId"].ToString())
                                                                                        , int.Parse(ddlBillToCompany.SelectedCompany)
                                                                                        , Convert.ToDateTime(ucDate.Text)
                                                                                        , int.Parse(ddlBank.SelectedValue)
                                                                                        , txtReferenceNo.Text
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , int.Parse(ucBudgetCode.SelectedBudgetCode)
                                                                                        , new Guid(ucOwnerBudgetCode.SelectedValue)
                                                                                        , int.Parse(ddlOwner.SelectedAddress)
                                                                                        , ref iOwnerDebitCreditId
                                                                                        , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                                        , General.GetNullableInteger(ddlSubtype.SelectedValue)
                                                                                        , txtVoucherLineItemDesc.Text, int.Parse(ddlVessel.SelectedValue)
                                                                                        ,General.GetNullableInteger(ViewState["PROJECTID"].ToString()));
                    ViewState["OwnerDebitCreditNoteId"] = iOwnerDebitCreditId;
                    ucStatus.Text = "Debit/Credit note saved.";

                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteUpdate(new Guid(ViewState["OwnerDebitCreditNoteId"].ToString())
                                                                                        , int.Parse(ddlType.SelectedValue)
                                                                                        , int.Parse(ViewState["VesselId"].ToString())
                                                                                        , int.Parse(ddlBillToCompany.SelectedCompany)
                                                                                        , Convert.ToDateTime(ucDate.Text)
                                                                                        , int.Parse(ddlBank.SelectedValue)
                                                                                        , txtReferenceNo.Text
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , int.Parse(ucBudgetCode.SelectedBudgetCode)
                                                                                        , new Guid(ucOwnerBudgetCode.SelectedValue)
                                                                                        , int.Parse(ddlOwner.SelectedAddress)
                                                                                        , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                                        , txtVoucherLineItemDesc.Text, int.Parse(ddlVessel.SelectedValue));
                    ucStatus.Text = "Debit/Credit note updated.";

                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }

                gvLineItem.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void OwnerDebitCreditNoteEdit()
    {
        try
        {
            if (ViewState["OwnerDebitCreditNoteId"] != null && ViewState["OwnerDebitCreditNoteId"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteEdit(new Guid(ViewState["OwnerDebitCreditNoteId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];


                if (ds.Tables[0].Rows[0]["FLDISACTIVEBANKACCOUNT"].ToString() == "0")
                {
                    ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDBILLTOCOMPANY"].ToString()), 1);
                }
                else
                {
                    ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dr["FLDBILLTOCOMPANY"].ToString()), 0);
                }

                ddlBank.DataBind();

                string temp = dr["FLDBUDGETID"].ToString().Equals("") ? "Dummy" : dr["FLDBUDGETID"].ToString();

                ddlType.SelectedValue = dr["FLDTYPE"].ToString();
                ddlSubtype.SelectedValue = dr["FLDSUBTYPE"].ToString();
                ddlVessel.SelectedValue = dr["FLDVESSELACCOUNTMAPID"].ToString();
                ddlBillToCompany.SelectedCompany = dr["FLDBILLTOCOMPANY"].ToString();
                ucDate.Text = dr["FLDDATE"].ToString();
                ddlBank.SelectedValue = dr["FLDBANKID"].ToString();
                txtReferenceNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtVoucherLineItemDesc.Text = dr["FLDVOUCHERLONGDESCRIPTION"].ToString();

                if (dr["FLDBUDGETID"].ToString() != "")
                    ucBudgetCode.SelectedBudgetCode = dr["FLDBUDGETID"].ToString();
                if (dr["FLDOWNER"].ToString() != "")
                    ddlOwner.SelectedAddress = dr["FLDOWNER"].ToString();
                ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
                ViewState["BudgetCode"] = dr["FLDBUDGETID"].ToString().Equals("") ? "Dummy" : dr["FLDBUDGETID"].ToString();
                if (dr["FLDOWNERBUDGETCODEID"].ToString() != "")
                {
                    ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                    ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
                }
                ucCurrencyCode.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                DataSet ds1 = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VesselId"].ToString()));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ViewState["OWNERID"] = ds1.Tables[0].Rows[0]["FLDOWNER"].ToString();
                    ddlOwner.SelectedAddress = ViewState["OWNERID"].ToString();

                }
                DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(ViewState["VesselId"])), 1);
                if (dsaccount.Tables[0].Rows.Count > 0)
                {
                    Getprincipal(Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]));
                }
                FreezAll();
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

    private bool IsValidData(string type, string vessel, string billingCompany, string date, string bank, string referenceNo,
        string budgetId, string ownerBudgetCode, string ownerId, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (type.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (vessel.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel is required.";
        if (billingCompany.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Billing company is required.";
        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";
        if (bank.Trim().Equals(""))
            ucError.ErrorMessage = "Bank receiving funds is required.";
        if (budgetId.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Budget code is required.";
        if (ownerBudgetCode.Trim().Equals(""))
            ucError.ErrorMessage = "Owner budget code is required.";
        if (ownerId.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Owner is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        return (!ucError.IsError);
    }

    private bool IsValidLineItem(string description, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        else if (General.GetNullableDecimal(amount) == null)
        {
            ucError.ErrorMessage = "Amount is required.";
        }

        return (!ucError.IsError);
    }

    protected void ddlBillToCompany_Changed(object sender, EventArgs e)
    {
        ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlBillToCompany.SelectedCompany));
        ddlBank.DataBind();
    }
    public void ResetAll()
    {
        try
        {
            ddlType.SelectedValue = string.Empty;
            ddlVessel.SelectedValue = string.Empty;
            ddlBillToCompany.SelectedCompany = string.Empty;
            ucDate.Text = string.Empty;
            ddlBank.SelectedValue = string.Empty;
            txtReferenceNo.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void FreezAll()
    {
        try
        {
            ddlType.Enabled = false;
            ddlVessel.Enabled = false;
            ucDate.Enabled = false;
            ddlSubtype.Enabled = false;
            ddlBillToCompany.Enabled = false;
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

    protected void ddlSubtype_DataBound(object sender, EventArgs e)
    {
        ddlSubtype.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlVessel_DataBound(object sender, EventArgs e)
    {
        ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    public void BindLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            if (General.GetNullableGuid(Convert.ToString(ViewState["OwnerDebitCreditNoteId"])) != null)
            {
                FreezAll();
                //OwnerDebitCreditNoteEdit();
            }

            DataSet ds = PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteLineItemList(General.GetNullableGuid(ViewState["OwnerDebitCreditNoteId"].ToString()),
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

    protected void ucBudgetCode_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ViewState["BudgetCode"] = ucBudgetCode.SelectedBudgetCode;
        BindOwnerBudgetCode();
    }

    private void BindOwnerBudgetCode()
    {
        if (Convert.ToString(ViewState["PRINCIPAL"]) != "" && ViewState["BudgetCode"].ToString() != "Dummy")
        {
            //ucOwnerBudgetCode.VesselId = ViewState["VesselId"].ToString();
            ucOwnerBudgetCode.OwnerId = ViewState["PRINCIPAL"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(Convert.ToString(ViewState["PRINCIPAL"]))
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

    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ViewState["VesselAccountMapid"] = ddlVessel.SelectedValue;


        DataSet ds1 = PhoenixRegistersVesselAccount.EditVesselAccount(int.Parse(ViewState["VesselAccountMapid"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ViewState["VesselId"] = ds1.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VesselId"].ToString()));
            ViewState["OWNERID"] = ds.Tables[0].Rows[0]["FLDOWNER"].ToString();
            // ViewState["PRINCIPAL"] = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();
            ddlOwner.SelectedAddress = ViewState["OWNERID"].ToString();
            Getprincipal(Convert.ToInt32(ds1.Tables[0].Rows[0]["FLDACCOUNTID"]));
            Gettemplate(Convert.ToInt32(ds1.Tables[0].Rows[0]["FLDACCOUNTID"]));

        }
        BindOwnerBudgetCode();
    }

    public void Gettemplate(int accountid)
    {
        try
        {
            ddlBillToCompany.SelectedCompany = string.Empty;
            ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, null);
            ddlBank.DataBind();
            ddlBillToCompany.SelectedCompany = string.Empty;
            //txtReferenceNo.Text = "";
            DataSet dstemplate = null;
            DataSet dss = new DataSet();
            dstemplate = PhoenixAccountsOwnerFundRequestRegister.OwnerFundRequestRegisterEdit(accountid);
            if (dstemplate.Tables[0].Rows.Count > 0)
            {
                //txtReferenceNo.Text = dstemplate.Tables[0].Rows[0]["FLDREFERENCENUMBER"].ToString();
                ucCurrencyCode.SelectedCurrency = dstemplate.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                ddlBillToCompany.SelectedCompany = dstemplate.Tables[0].Rows[0]["FLDBILLTOCOMPANYID"].ToString();
                if (dstemplate.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() != "0")
                {
                    dss = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(dstemplate.Tables[0].Rows[0]["FLDBILLTOCOMPANYID"].ToString()), 0);


                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        ddlBank.DataSource = dss.Tables[0];

                        ddlBank.DataBind();
                        ddlBank.SelectedValue = dstemplate.Tables[0].Rows[0]["FLDSUBACCOUNTID"].ToString();
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
    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPAL"] = Convert.ToString(dr["FLDPRINCIPALID"]);

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
            ViewState["PAGENUMBERLINE"] = ViewState["PAGENUMBERLINE"] != null ? ViewState["PAGENUMBERLINE"] : gvLineItem.CurrentPageIndex + 1;
            BindLineItem();
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
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                                        , ((UserControlDecimal)e.Item.FindControl("txtAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteLineItemInsert(new Guid(ViewState["OwnerDebitCreditNoteId"].ToString())
                     , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                     , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtAmountAdd")).Text)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Line item inserted";

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindLineItem();
                gvLineItem.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                                        , ((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteLineItemUpdate(new Guid(((RadLabel)e.Item.FindControl("lblLineItemIdEdit")).Text)
                     , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                     , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Line item updated";

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindLineItem();
                gvLineItem.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteLineItemDelete(new Guid(((RadLabel)e.Item.FindControl("lblLineItemId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindLineItem();
                gvLineItem.Rebind();

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERLINE"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");

            if (ViewState["OwnerDebitCreditNoteId"] != null && ViewState["OwnerDebitCreditNoteId"].ToString() != string.Empty)
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

            if (ViewState["OwnerDebitCreditNoteId"] != null && ViewState["OwnerDebitCreditNoteId"].ToString() != "")
            {
                if (cmdAdd != null) cmdAdd.Visible = true;
            }
            else
            {
                if (cmdAdd != null) cmdAdd.Visible = false;
            }
        }
    }
}

