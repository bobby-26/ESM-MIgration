using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsCreditPurchaseAdvancePayment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetName.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Commitment", "COMMITMENT", ToolBarDirection.Right);
            toolbar.AddButton("Credit Purchase", "CREDITPURCHASE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            MenuGeneral.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbarsub.AddButton("Commit PO", "COMMIT", ToolBarDirection.Right);
            toolbarsub.AddButton("Supplier Mapping", "SUPPLIERMAPPING", ToolBarDirection.Right);
            MenuGeneralSub.AccessRights = this.ViewState;
            MenuGeneralSub.MenuList = toolbarsub.Show();

            MenuGeneralSub.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarsave.Show();

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
            }
            else
                ViewState["vesselid"] = "";

            if (Request.QueryString["vesselsupplierid"] != null && !string.IsNullOrEmpty(Request.QueryString["vesselsupplierid"].ToString()))
            {
                ViewState["VESSELSUPPLIERID"] = Request.QueryString["vesselsupplierid"];
            }
            else
                ViewState["VESSELSUPPLIERID"] = "";

            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
            }
            else
                ViewState["orderid"] = "";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                SetBudgetDetails();
                BindProjectCode();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CREDITPURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                try
                {
                    PhoenixAccountsVesselAccounting.VesselSupplierMapConfirm(int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["VESSELSUPPLIERID"].ToString()));
                    ucStatus.Text = "Mapped Supplier is confirmed.";
                }
                catch (Exception ee)
                {
                    ucError.ErrorMessage = ee.Message;
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindProjectCode()
    {
        ucProjectcode.bind(General.GetNullableInteger(ddlAccountDetails.SelectedValue), General.GetNullableInteger(txtBudgetId.Text));

    }

    protected void MenuSave_TabStripCommand(Object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidBudget(txtBudgetId.Text, ddlCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                string podescription = "";
                DataTable dt = PhoenixAccountsVesselAccounting.EditBondProvisionOrderForm(new Guid(Request.QueryString["OrderId"]));
                if (dt.Rows.Count > 0)
                {
                    podescription = dt.Rows[0]["FLDSTOCKTYPENAME"].ToString() + " received at " + dt.Rows[0]["FLDRECEIVEDPORT"] + " on " + General.GetDateTimeToString(dt.Rows[0]["FLDRECEIVEDDATE"].ToString());
                }

                PhoenixAccountsVesselAccounting.CreditPurchaseMappingInsert(new Guid(Request.QueryString["OrderId"])
                                        , General.GetNullableInteger(txtBudgetId.Text), General.GetNullableInteger(ddlCompany.SelectedCompany)
                                        , General.GetNullableGuid(txtOwnerBudgetId.Text)
                                        , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                        , General.GetNullableInteger(lblVesselid.Text)
                                        , General.GetNullableInteger(ucProjectcode.SelectedProjectCode));

                PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , int.Parse(lblVesselid.Text)
                                        , int.Parse(txtBudgetId.Text)
                                        , DateTime.Parse(DateTime.Now.ToString())
                                        , decimal.Parse(lblAmountBudgetCommit.Text)
                                        , 0                      //Update FLDCOMMITTEDAMOUNT
                                        , 'D'                    //Debit
                                        , lblRefNo.Text
                                        , lblOrderId.Text
                                        , General.GetNullableInteger(lblCurrency.Text)
                                        , decimal.Parse(lblAmountInUSDBudgetCommit.Text)
                                        , General.GetNullableDateTime(DateTime.Now.ToString())
                                        , null
                                        , null
                                        , null
                                        , null
                                        , General.GetNullableInteger(lblAddressCode.Text)
                                        , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                        , null, null, null, null
                                        , podescription
                                        , General.GetNullableGuid(txtOwnerBudgetId.Text)
                                        , General.GetNullableInteger(ucProjectcode.SelectedProjectCode));

                //implemented to resolve the bug - 8459
                PhoenixAccountsVesselAccounting.IncorrectBondPrvPOInsert();

                ucStatus.Text = "PO is committed successfully.";
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisionsCommitted.aspx", false);
            }
        }
        catch (Exception es)
        {
            ucError.ErrorMessage = es.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneralSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SUPPLIERMAPPING"))
            {
                Response.Redirect("../Accounts/AccountsVesselSupplierMapping.aspx?vesselid=" + ViewState["vesselid"] + "&vesselsupplierid=" + ViewState["vesselsupplierid"] + "&orderid=" + ViewState["orderid"].ToString());
            }
            if (CommandName.ToUpper().Equals("COMMITTPO"))
            {
                try
                {
                    if (!IsValidBudget(txtBudgetId.Text, ddlCompany.SelectedCompany))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixAccountsVesselAccounting.CreditPurchaseMappingInsert(new Guid(Request.QueryString["OrderId"])
                                            , General.GetNullableInteger(txtBudgetId.Text), General.GetNullableInteger(ddlCompany.SelectedCompany)
                                            , General.GetNullableGuid(txtOwnerBudgetId.Text)
                                            , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                            , General.GetNullableInteger(lblVesselid.Text)
                                            , General.GetNullableInteger(ucProjectcode.SelectedProjectCode));

                    PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , int.Parse(lblVesselid.Text)
                                            , int.Parse(txtBudgetId.Text)
                                            , DateTime.Parse(DateTime.Now.ToString())
                                            , decimal.Parse(lblAmountBudgetCommit.Text)
                                            , 0                      //Update FLDCOMMITTEDAMOUNT
                                            , 'D'                    //Debit
                                            , lblRefNo.Text
                                            , lblOrderId.Text
                                            , General.GetNullableInteger(lblCurrency.Text)
                                            , decimal.Parse(lblAmountInUSDBudgetCommit.Text)
                                            , General.GetNullableDateTime(DateTime.Now.ToString())
                                            , null
                                            , null
                                            , null
                                            , null
                                            , General.GetNullableInteger(lblAddressCode.Text)
                                            , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                            
                                           );

                    //implemented to resolve the bug - 8459
                    PhoenixAccountsVesselAccounting.IncorrectBondPrvPOInsert();

                    ucStatus.Text = "PO is committed successfully.";
                    Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisionsCommitted.aspx", false);
                }
                catch (Exception es)
                {
                    ucError.ErrorMessage = es.Message;
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

    private bool IsValidBudget(string budgetid, string billtocomp)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (string.IsNullOrEmpty(budgetid))
            ucError.ErrorMessage = "Budget code is required.";
        //if (string.IsNullOrEmpty(ownerbudgetid))
        //    ucError.ErrorMessage = "Owner Budget Code is required.";
        if (string.IsNullOrEmpty(billtocomp) || billtocomp.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Bill to company is required.";

        if (lblIsSupplierConfirmed.Text != "1")
            ucError.ErrorMessage = "Supplier needs to be mapped and confirmed before committing PO.";

        if (string.IsNullOrEmpty(lblCurrency.Text))
            ucError.ErrorMessage = "Invoice currency should not be empty for committing PO.";

        if (string.IsNullOrEmpty(lblAmountInUSD.Text) || lblAmountInUSD.Text.Equals("0.00"))
            ucError.ErrorMessage = "Amount in USD should not be empty for committing PO.";

        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Please select a vessel account.";

        return (!ucError.IsError);
    }

    private void SetBudgetDetails()
    {
        DataTable dt = PhoenixAccountsVesselAccounting.CreditPurchaseMappingEdit(General.GetNullableGuid(Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : string.Empty));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETNAME"].ToString();
            txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            txtOwnerBudgetCode.Text = dr["FLDOWNERACCOUNT"].ToString();
            lblBudgetDate.Text = dr["FLDORDERDATE"].ToString();
            lblAmountInUSD.Text = dr["FLDTOTALAMOUNTUSD"].ToString();
            lblRefNo.Text = dr["FLDREFERENCENO"].ToString();
            lblOrderId.Text = dr["FLDORDERID"].ToString();
            lblCurrency.Text = dr["FLDCURRENCYID"].ToString();
            lblAmount.Text = dr["FLDTOTALAMOUNT"].ToString();
            lblAddressCode.Text = dr["FLDADDRESSCODE"].ToString();
            lblIsSupplierConfirmed.Text = dr["FLDSTATUS"].ToString();
            lblVesselid.Text = dr["FLDVESSELID"].ToString();
            lblOrderDate.Text = dr["FLDORDERDATE"].ToString();

            ////////////////////////////////
            lblAmountInUSDBudgetCommit.Text = dr["FLDEXCHANGEPRICE"].ToString();
            lblAmountBudgetCommit.Text = dr["FLDTOTALAMOUNTLOCAL"].ToString();
            ////////////////////////////////

            ddlAccountDetails.Visible = true;
            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
            ddlAccountDetails.DataBind();

            ddlAccountDetails.SelectedValue = dr["FLDACCOUNTID"].ToString();

            if (dr["FLDBILLTOCOMPANY"] != null && dr["FLDBILLTOCOMPANY"].ToString().Trim() != "")
                ddlCompany.SelectedCompany = dr["FLDBILLTOCOMPANY"].ToString();

            btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?iframignore=true&budgetgroup=119&hardtypecode=30&vesselid=" + dr["FLDVESSELID"].ToString() + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
			 if(!txtBudgetId.Text.Equals(""))
                btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + dr["FLDVESSELID"].ToString() + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "&budgetid=" + dr["FLDBUDGETID"].ToString() + "', true); ");
            else
            	btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + dr["FLDVESSELID"].ToString() + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
        }

    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {

        btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + lblVesselid.Text + "&budgetdate=" + lblOrderDate.Text + "&budgetid=" + txtBudgetId.Text + "', true); ");

        BindProjectCode();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlAccountDetails_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        try
        {
            BindProjectCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
