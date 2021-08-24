using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PurchaseBulkPOLineItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + "&budgetdate=" + DateTime.Now.Date + "', true); ");
        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
        

        MenuBulkPurchase.AccessRights = this.ViewState;
        MenuBulkPurchase.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ORDERID"] = "";
            cmdShowItem.Visible = false;
            ibtnRecord.Visible = false;
            ibtnInventoryUpdate.Visible = false;

            if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != string.Empty)
            {
                ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
                BulkPOEdit();
            }
            else
                ViewState["ORDERID"] = "";
            if (Request.QueryString["LINEITEMID"] != null && Request.QueryString["LINEITEMID"].ToString() != string.Empty)
            {
                ViewState["ORDERLINEITEMID"] = Request.QueryString["LINEITEMID"].ToString();
                BulkPOLineItemEdit();
            }
            else
                ViewState["ORDERLINEITEMID"] = ""; 
                    
            cmdShowItem.OnClientClick = "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?storetype=" + ViewState["STORETYPE"] + "&vesselid=0&txtnumber='+ document.getElementById('txtItemNumber').value+'&txtname='+ document.getElementById('txtPartName').value, true);";
            ibtnRecord.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseBulkPOLineItemSealNumberGeneral.aspx?LINEITEMID=" + ViewState["ORDERLINEITEMID"] + "&STOREITEMID=" + ViewState["STOREITEMID"] + "'); return false;");
            ibtnInventoryUpdate.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseBulkPOLineItemSealLocationUpdate.aspx?LINEITEMID=" + ViewState["ORDERLINEITEMID"] + "&STOREITEMID=" + ViewState["STOREITEMID"] + "','medium'); return false;");
            ibtnMoreSeals.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseBulkPOLineItemAdditionalSealNumber.aspx?LINEITEMID=" + ViewState["ORDERLINEITEMID"] + "&STOREITEMID=" + ViewState["STOREITEMID"] + "'); return false;");
        }
    }

    protected void BulkPOEdit()
    {
        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["FLDSTOCKTYPE"] = dr["FLDSTOCKTYPE"].ToString();
            ViewState["STORETYPE"] = dr["FLDSTOCKCLASSID"].ToString();
            
            if (dr["FLDSTOCKTYPE"] != null && dr["FLDSTOCKTYPE"].ToString() != "" && dr["FLDSTOCKTYPE"].ToString() == "SERVICE")
            {
                cmdShowItem.Visible = false;
                ibtnRecord.Visible = false;
                ibtnInventoryUpdate.Visible = false;
                ibtnMoreSeals.Visible = false;
            }
            else
            {
                if (ViewState["STORETYPE"] != null && ViewState["STORETYPE"].ToString() == "1153")
                {                   
                    cmdShowItem.Visible = true;                    
                }                
            }
        }
    }

    protected void BulkPOLineItemEdit()
    {
        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemEdit(new Guid(ViewState["ORDERLINEITEMID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtPartName.Text = dr["FLDLINEITEMNAME"].ToString();
            txtItemNumber.Text = dr["FLDLINEITEMNUMBER"].ToString();
            txtPartId.Text = dr["FLDSTOREITEMID"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETCODENAME"].ToString();
            txtBudgetgroupId.Text = dr["FLDBUDGETGROUPID"].ToString();
            //ucOrderQty.Text = dr["FLDORDERQUANTITY"].ToString();
            //ucTotalReceivedQty.Text = dr["FLDRECEIVEDQUANTITY"].ToString();
            ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            ucPrice.Text = dr["FLDPRICE"].ToString();
            ucTotal.Text = dr["FLDTOTALAMOUNT"].ToString();
            ucTotalReceivedQty.Text = dr["FLDRECEIVEDQUANTITY"].ToString();
            ucTotalOrderQty.Text = dr["FLDORDERQUANTITY"].ToString();
            
            ViewState["STOREITEMID"] = dr["FLDSTOREITEMID"].ToString();

            if (ViewState["FLDSTOCKTYPE"] != null && ViewState["FLDSTOCKTYPE"].ToString() == "STORE")
            {
                if (ViewState["STORETYPE"] != null && ViewState["STORETYPE"].ToString() == "1153")
                {
                    if (General.GetNullableInteger(dr["FLDORDERQUANTITY"].ToString()) != null && int.Parse(dr["FLDORDERQUANTITY"].ToString()) > 0)
                    {
                        ibtnRecord.Visible = true;
                        ibtnInventoryUpdate.Visible = true;
                    }
                    if (dr["FLDISLOCATIONUPDATED"].ToString().Equals("1"))
                        ibtnMoreSeals.Visible = true;                       
                }
            }                            
        }
    }

    protected void MenuBulkPurchase_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
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
                    PhoenixPurchaseBulkPurchase.BulkPOLineItemInsert(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["ORDERID"].ToString())
                                                                , General.GetNullableString(txtPartName.Text.Trim())
                                                                , General.GetNullableString(string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals)
                                                                , General.GetNullableInteger(txtBudgetId.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(ucTotalReceivedQty.Text)
                                                                , General.GetNullableInteger(ucUnit.SelectedUnit)
                                                                , General.GetNullableDecimal(ucPrice.Text)
                                                                , General.GetNullableGuid(txtPartId.Text)
                                                                );
                    
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
                    PhoenixPurchaseBulkPurchase.BulkPOLineItemUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["ORDERID"].ToString())
                                                                , new Guid(ViewState["ORDERLINEITEMID"].ToString())
                                                                , General.GetNullableString(txtPartName.Text.Trim())
                                                                , General.GetNullableString(string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals)
                                                                , General.GetNullableInteger(txtBudgetId.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(ucTotalReceivedQty.Text)
                                                                , General.GetNullableInteger(ucUnit.SelectedUnit)
                                                                , General.GetNullableDecimal(ucPrice.Text)
                                                                , General.GetNullableGuid(txtPartId.Text)
                                                                );

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
        txtPartName.Text = txtItemNumber.Text = ucTotalReceivedQty.Text = ucPrice.Text = ucUnit.SelectedUnit = ucTotal.Text = "";
        txtBudgetCode.Text = txtBudgetgroupId.Text = txtBudgetName.Text = txtBudgetId.Text = "";
        ucTotalReceivedQty.Text = "";
        txtPartId.Text = "";
        ucTotalOrderQty.Text = "";
        ibtnRecord.Visible = false;
        ibtnInventoryUpdate.Visible = false;
        ibtnMoreSeals.Visible = false;
    }
    public bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) == null)
            ucError.ErrorMessage = "Bulk PO details should be recorded before adding the Line Items.";

        if (General.GetNullableString(txtPartName.Text.Trim()) == null)
            ucError.ErrorMessage = "Line Item Name is Required.";

        //if (General.GetNullableInteger(ucOrderQty.Text) == null)
        //    ucError.ErrorMessage = "Ordered Qty Qty is Required.";

        //if (General.GetNullableInteger(ucTotalReceivedQty.Text) == null)
        //    ucError.ErrorMessage = "Received Qty is Required.";
        //else
        //{
        //    if (General.GetNullableInteger(ucTotalReceivedQty.Text) == 0)
        //        ucError.ErrorMessage = "Received Qty should be greater than zero.";
        //}

        if (General.GetNullableDecimal(ucPrice.Text) == null) 
            ucError.ErrorMessage = "Price is Required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
