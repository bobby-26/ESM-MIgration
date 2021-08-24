using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class PurchaseSchedulePOLineItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        txtBudgetId.Attributes.Add("style", "visibility:hidden;");
        txtBudgetgroupId.Attributes.Add("style", "visibility:hidden;");
        btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + "&budgetdate=" + DateTime.Now.Date + "', true); ");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("New", "NEW");
        toolbar.AddButton("Save", "SAVE");

        MenuBulkPurchase.AccessRights = this.ViewState;
        MenuBulkPurchase.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SCHEDULEPOID"] = "";

            if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != string.Empty)
                ViewState["SCHEDULEPOID"] = Filter.CurrentSelectedSchedulePO.ToString();
            else
                ViewState["SCHEDULEPOID"] = "";
            if (Request.QueryString["LINEITEMID"] != null && Request.QueryString["LINEITEMID"].ToString() != string.Empty)
            {
                ViewState["ORDERLINEITEMID"] = Request.QueryString["LINEITEMID"].ToString();
                BulkPOLineItemEdit();
            }
            else
                ViewState["ORDERLINEITEMID"] = "";
        }
    }

    protected void BulkPOLineItemEdit()
    {
        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemEdit(new Guid(ViewState["ORDERLINEITEMID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtItemName.Text = dr["FLDLINEITEMNAME"].ToString();
            //txtItemNumber.Text = dr["FLDLINEITEMNUMBER"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETCODENAME"].ToString();
            txtBudgetgroupId.Text = dr["FLDBUDGETGROUPID"].ToString();
            ucRequestedQty.Text = dr["FLDREQUESTEDQUANTITY"].ToString();           
            ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            ucPrice.Text = dr["FLDPRICE"].ToString();
            
            //ucPrice.Text = dr["FLDPRICE"].ToString();
            //ucTotal.Text = dr["FLDTOTALAMOUNT"].ToString();
        }
    }

    protected void MenuBulkPurchase_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {

            if (!IsValidDetails())
            {
                ucError.Visible = true;
                return;
            }

            if (General.GetNullableGuid(ViewState["ORDERLINEITEMID"].ToString()) == null)
            {
                try
                {
                    PhoenixPurchaseSchedulePO.SchedulePOLineItemInsert(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["SCHEDULEPOID"].ToString())
                                                                , General.GetNullableString(txtItemName.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(txtBudgetId.Text.Trim())
                                                                , General.GetNullableInteger(ucRequestedQty.Text)                                                               
                                                                , General.GetNullableInteger(ucUnit.SelectedUnit) 
                                                                , decimal.Parse(ucPrice.Text));
                    
                    ucStatus.Text = "Line Item details added.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                Session["NEWBULKLINEITEM"] = "Y";
                Reset();                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }

            else
            {
                try
                {
                    PhoenixPurchaseSchedulePO.SchedulePOLineItemUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["SCHEDULEPOID"].ToString())
                                                                , new Guid(ViewState["ORDERLINEITEMID"].ToString())
                                                                , General.GetNullableString(txtItemName.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(txtBudgetId.Text.Trim())
                                                                , General.GetNullableInteger(ucRequestedQty.Text)
                                                                , General.GetNullableInteger(ucUnit.SelectedUnit)
                                                                , decimal.Parse(ucPrice.Text));

                    ucStatus.Text = "Line Item details updated.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
    }
    private void Reset()
    {
        ViewState["ORDERLINEITEMID"] = "";
        txtItemName.Text = ucRequestedQty.Text = ucUnit.SelectedUnit = "";
        txtBudgetCode.Text = txtBudgetgroupId.Text = txtBudgetName.Text = txtBudgetId.Text = "";
        ucPrice.Text = "";
    }
    public bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString()) == null)
            ucError.ErrorMessage = "Schedule PO details should be recorded before adding the Line Items.";

        if (General.GetNullableString(txtItemName.Text.Trim()) == null)
            ucError.ErrorMessage = "Line Item Name is required.";

        //if (General.GetNullableString(txtBudgetId.Text.Trim()) == null) 
        //    ucError.ErrorMessage = "Budget code is Required.";

        if (General.GetNullableDecimal(ucPrice.Text) == null)
            ucError.ErrorMessage = "Unit Price is required.";

        if (General.GetNullableInteger(ucRequestedQty.Text) == null)
            ucError.ErrorMessage = "Requested Quantity is required.";            

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
