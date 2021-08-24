using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsProjectBillingItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        ProjectBilling.AccessRights = this.ViewState;
        ProjectBilling.Title = "Project Billing Item";
        ProjectBilling.MenuList = toolbar.Show();
        if (!IsPostBack)
        {


            if (Request.QueryString["projectbillingid"] != string.Empty)
            {
                ViewState["projectbillingid"] = Request.QueryString["projectbillingid"];
            }
            ProjectBillingEdit();

            txtBudgetCodeId.Attributes.Add("style", "visibility:hidden");
            txtCreditAccountId.Attributes.Add("style", "visibility:hidden");
            txtInventoryAccountId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            txtServiceNumber.Attributes.Add("style", "visibility:hidden");
            txtStoreItemId.Attributes.Add("style", "visibility:hidden");

            cmdShowItem.Attributes.Add("onclick", "return showPickList('spnPickItem', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStoreItem.aspx?iframignore=true&storetype=417&vesselid=0', true); ");
            imgShowBudgetCode.Attributes.Add("onclick", "return showPickList('spnPickListBudgetCode', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?framename=ifMoreInfo', true);");
            imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx',true); ");
            img1.Attributes.Add("onclick", "return showPickList('spnPickListInventoryAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx',true); ");
        }
    }
    protected void ProjectBillingEdit()
    {
        if (ViewState["projectbillingid"] != null)
        {

            DataSet ds = PhoenixAccountsProjectBilling.ProjectBillingEdit(new Guid(ViewState["projectbillingid"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    txtprojectBillingName.Text = dr["FLDPROJECTBILLINGNAME"].ToString();
                    txtBillingDescription.Text = dr["FLDBILLINGITEMDESCRIPTION"].ToString();
                    ucBillingUnit.SelectedUnit = dr["FLDBILLINGUNIT"].ToString();
                    ucCurrencyCode.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    ucProjectBillingGroups.SelectedQuick = dr["FLDPROJECTBILLINGGROUP"].ToString();
                    ucDefaultPrice.Text = dr["FLDAMOUNT"].ToString();
                    txtBudgetCodeId.Text = dr["FLDVESSELBUDGETID"].ToString();
                    txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
                    txtBudgetCodeDescription.Text = dr["FLDBUDGETCODEDESCRIPTION"].ToString();
                    txtCreditAccountId.Text = dr["FLDCREDITACCOUNT"].ToString();
                    txtCreditAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                    txtInventoryAccountId.Text = dr["FLDINVENTORYACCOUNT"].ToString();
                    txtInventoryAccountCode.Text = dr["FLDINVENTORYACCOUNTCODE"].ToString();
                    txtCreditAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
                    txtInventoryAccountDescription.Text = dr["FLDINVENTORYACCOUNTDESCRIPTION"].ToString();
                    txtItemNumber.Text = dr["FLDNUMBER"].ToString();
                    txtStoreItemId.Text = dr["FLDSTOREITEMID"].ToString();

                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void Reset()
    {
        txtprojectBillingName.Text = "";
        txtBillingDescription.Text = "";
        ucBillingUnit.SelectedUnit = "";
        ucCurrencyCode.SelectedCurrency = "";
        ucProjectBillingGroups.SelectedQuick = "";
        ucDefaultPrice.Text = "";
        txtBudgetCodeId.Text = "";
        txtCreditAccountId.Text = "";
        txtBudgetCode.Text = "";
        txtBudgetCodeDescription.Text = "";
        txtCreditAccountCode.Text = "";
        txtCreditAccountDescription.Text = "";
        ViewState["projectbillingid"] = null;
        txtStoreItemId.Text = "";
        txtItemNumber.Text = "";


    }
    protected void ProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidProjectBilling())
                {
                    if (ViewState["projectbillingid"] != null && ViewState["projectbillingid"].ToString() != "")
                    {
                        PhoenixAccountsProjectBilling.ProjectBillingUpdate(new Guid(ViewState["projectbillingid"].ToString())
                                                                               , txtprojectBillingName.Text
                                                                               , txtBillingDescription.Text
                                                                               , int.Parse(ucBillingUnit.SelectedUnit)
                                                                               , int.Parse(ucProjectBillingGroups.SelectedQuick)
                                                                               , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                               , decimal.Parse(ucDefaultPrice.Text)
                                                                               , int.Parse(txtBudgetCodeId.Text)
                                                                               , int.Parse(txtCreditAccountId.Text)
                                                                               , new Guid(txtStoreItemId.Text.ToString())
                                                                               , int.Parse(txtInventoryAccountId.Text));

                    }
                    else
                    {
                        PhoenixAccountsProjectBilling.ProjectBillingInsert(txtprojectBillingName.Text
                                                                               , txtBillingDescription.Text
                                                                               , int.Parse(ucBillingUnit.SelectedUnit)
                                                                               , int.Parse(ucProjectBillingGroups.SelectedQuick)
                                                                               , int.Parse(ucCurrencyCode.SelectedCurrency)
                                                                               , decimal.Parse(ucDefaultPrice.Text)
                                                                               , int.Parse(txtBudgetCodeId.Text)
                                                                               , int.Parse(txtCreditAccountId.Text)
                                                                               , new Guid(txtStoreItemId.Text.ToString())
                                                                               , int.Parse(txtInventoryAccountId.Text));

                    }
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            else if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["projectbillingid"] != null && ViewState["projectbillingid"].ToString() != "")
                {
                    PhoenixAccountsProjectBilling.ProjectBillingItemDelete(new Guid(ViewState["projectbillingid"].ToString()));

                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidProjectBilling()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtprojectBillingName.Text.Equals(""))
            ucError.ErrorMessage = "Billing name is required.";

        if (txtBillingDescription.Text.Equals(""))
            ucError.ErrorMessage = "Billing description is required.";

        if (General.GetNullableInteger(ucBillingUnit.SelectedUnit) == null)
            ucError.ErrorMessage = "Billing unit is required.";

        if (General.GetNullableInteger(ucCurrencyCode.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required.";

        if (General.GetNullableInteger(ucProjectBillingGroups.SelectedQuick) == null)
            ucError.ErrorMessage = "Project billing group is required.";

        if (ucDefaultPrice.Text.Equals(""))
            ucError.ErrorMessage = "Default price is required.";

        if (txtBudgetCodeId.Text.Equals(""))
            ucError.ErrorMessage = "Vessel budget code is required.";

        if (txtCreditAccountId.Text.Equals(""))
            ucError.ErrorMessage = "Credit account is required.";

        if (txtStoreItemId.Text.Equals(""))
            ucError.ErrorMessage = "Inventory is required.";


        return (!ucError.IsError);

    }

}
