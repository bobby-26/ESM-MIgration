using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PurchaseBulkPOGeneral : PhoenixBasePage
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
            ViewState["POTYPE"] = "";
            BindPOType();

            if (Request.QueryString["POTYPE"] != null && Request.QueryString["POTYPE"].ToString() != null)
            {
                ViewState["POTYPE"] = Request.QueryString["POTYPE"].ToString();
            }

            

            if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != string.Empty)
            {
                ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
                BulkPOEdit();
            }
            else
                ViewState["ORDERID"] = "";
            cmdPicPartPaid.Attributes.Add("onclick", "return showPickList('spnPicPartPaid', 'codehelp1', '', '../Purchase/PurchaseBulkPOPartPaid.aspx?ORDERID=" + ViewState["ORDERID"] + "&FORMNO=" + txtBulkPORefNo.Text + "', true); ");
           
        }
        ddlPOType.SelectedValue = ViewState["POTYPE"].ToString();
        if (ViewState["POTYPE"].ToString() == "2")
        {
            lblStockType.Visible = false;
            ddlStockType.Visible = false;
            ddlStockClass.Visible = false;
            ddlPOType.Visible = false;
        }
    }

    protected void BulkPOEdit()
    {
        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtInvoiceRefNo.Text = dr["FLDINVOICEREFERENCENUMBER"].ToString();
            txtInvoiceDate.Text = dr["FLDINVOICEDATE"].ToString();
            if (dr["FLDBULKPOTYPE"].ToString() == "2")
            {
                ddlInvoiceType.SelectedHard = dr["FLDINVOICETYPE"].ToString() != "" ? dr["FLDINVOICETYPE"].ToString() : "1339";
            }
            else
            {
                ddlInvoiceType.SelectedHard = dr["FLDINVOICETYPE"].ToString() != "" ? dr["FLDINVOICETYPE"].ToString() : "239";
            }
            ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtVendor.Text = dr["FLDVENDORID"].ToString();
            txtVenderName.Text = dr["FLDVENDORNAME"].ToString();
            txtVendorNumber.Text = dr["FLDVENDORCODE"].ToString();
            txtInvoiceReceivedDate.Text = dr["FLDINVOICERECEIVEDDATE"].ToString();
            txtBulkPORefNo.Text = dr["FLDFORMNUMBER"].ToString();
            txtFormTitle.Text = dr["FLDFORMTITLE"].ToString();
            txtInvoiceNumber.Text = dr["FLDINVOICENUMBER"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETNAME"].ToString();
            txtBudgetgroupId.Text = dr["FLDBUDGETGROUP"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            ddlStockClass.SelectedHard = dr["FLDSTOCKCLASSID"].ToString();
            ddlPOType.SelectedValue = dr["FLDBULKPOTYPE"].ToString();
            ddlPOType.Enabled = false;

            txtPartPaid.Text = dr["FLDPARTPAYMENT"].ToString();
            if (dr["FLDAPPROVEDYN"] != null && dr["FLDAPPROVEDYN"].ToString() == "0")
            {
                cmdPicPartPaid.Visible = false;
                lblPartPaid.Visible = false;
                txtPartPaid.Visible = false;
            }
            if (dr["FLDSTOCKTYPE"] != null && dr["FLDSTOCKTYPE"].ToString() == "STORE")
            {
                lblStockType.Text = "Stock Type & Store Type";
                if (ViewState["POTYPE"].ToString() != "2")
                    ddlStockClass.Visible = true;
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

            if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) == null)
            {
                try
                {
                    PhoenixPurchaseBulkPurchase.BulkPOInsert(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableString(txtInvoiceRefNo.Text.Trim())
                                                                , General.GetNullableDateTime(txtInvoiceDate.Text)
                                                                , General.GetNullableInteger(ddlInvoiceType.SelectedHard)
                                                                , General.GetNullableInteger(txtVendor.Text.Trim())
                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                , General.GetNullableDateTime(txtInvoiceReceivedDate.Text)
                                                                , General.GetNullableString(txtBulkPORefNo.Text.Trim())
                                                                , General.GetNullableString(txtFormTitle.Text.Trim())
                                                                , General.GetNullableInteger(txtBudgetId.Text)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , General.GetNullableInteger(ddlStockClass.SelectedHard)
                                                                ,General.GetNullableInteger(ddlPOType.SelectedValue)); 

                    ucStatus.Text = "Bulk PO information is added.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                Reset();
                Filter.CurrentSelectedBulkOrderId = null;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);               
            }

            else
            {
                try
                {
                    PhoenixPurchaseBulkPurchase.BulkPOUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["ORDERID"].ToString())
                                                                , General.GetNullableString(txtInvoiceRefNo.Text.Trim())
                                                                , General.GetNullableDateTime(txtInvoiceDate.Text)
                                                                , General.GetNullableInteger(ddlInvoiceType.SelectedHard)
                                                                , General.GetNullableInteger(txtVendor.Text.Trim())
                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                , General.GetNullableDateTime(txtInvoiceReceivedDate.Text)
                                                                , General.GetNullableString(txtBulkPORefNo.Text.Trim())
                                                                , General.GetNullableString(txtFormTitle.Text.Trim())
                                                                , General.GetNullableInteger(txtBudgetId.Text)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , General.GetNullableInteger(ddlStockClass.SelectedHard)); 

                    ucStatus.Text = "Bulk PO information is updated.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                String scriptupdate = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }        
    }
    private void Reset()
    {
        ViewState["ORDERID"] = "";
        txtInvoiceDate.Text = txtInvoiceReceivedDate.Text = txtInvoiceRefNo.Text = txtVendor.Text = txtVendorNumber.Text = txtVenderName.Text = "";
        txtBudgetId.Text = txtBudgetName.Text = txtBudgetgroupId.Text = txtBudgetCode.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        ddlCurrencyCode.SelectedCurrency = "10"; 
        txtBulkPORefNo.Text = "";
        txtFormTitle.Text = "";
        txtInvoiceNumber.Text = "";
        if (ViewState["POTYPE"].ToString() == "2")
        {
            ddlInvoiceType.SelectedHard = "1339";
        }
        else
        {
            ddlInvoiceType.SelectedHard = "239";
        }
       
        ddlStockClass.Visible = false;
        ddlStockClass.CssClass = "input";
        lblStockType.Text = "Stock Type";
        ddlStockType.SelectedIndex = 0;
        ddlStockClass.SelectedHard = "";
        //ddlPOType.SelectedIndex = 0;
        //ddlPOType.Enabled = true;
        lblPartPaid.Visible = false;
        txtPartPaid.Text = "";
        txtPartPaid.Visible = false;
        cmdPicPartPaid.Visible = false;

        ddlPOType.SelectedValue = ViewState["POTYPE"].ToString();
    }
    public bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //DateTime? dtinvoicedate = null, dtreceiveddate = null;

        //if (General.GetNullableString(txtBulkPORefNo.Text.Trim()) == null)
        //    ucError.ErrorMessage = "Bulk Purchase Reference Number is required.";

        if (General.GetNullableString(txtFormTitle.Text.Trim()) == null)
            ucError.ErrorMessage = "Form Title is required.";

        if (General.GetNullableInteger(txtBudgetId.Text) == null) 
            ucError.ErrorMessage = "Budget Code is required.";

        if (General.GetNullableString(txtVendor.Text) == null)
            ucError.ErrorMessage = "Vendor is required.";

        if (General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required.";

        if (ddlStockType.SelectedValue != null && ddlStockType.SelectedValue == "STORE")
        {
            if (General.GetNullableInteger(ddlStockClass.SelectedHard) == null)
                ucError.ErrorMessage = "Store Type is required.";
        }

        if (General.GetNullableInteger(ddlPOType.SelectedValue) == null)
            ucError.ErrorMessage = "PO Type is required.";

        //if (General.GetNullableString(txtInvoiceRefNo.Text.Trim()) == null)
        //    ucError.ErrorMessage = "Invoice Reference Number is Required.";

        //if (General.GetNullableDateTime(txtInvoiceDate.Text) == null)
        //    ucError.ErrorMessage = "Invoice Date is Required.";
        //else
        //    dtinvoicedate = DateTime.Parse(txtInvoiceDate.Text);

        //if (General.GetNullableDateTime(txtInvoiceReceivedDate.Text) == null)
        //    ucError.ErrorMessage = "Invoice Received Date is Required.";
        //else
        //    dtreceiveddate = DateTime.Parse(txtInvoiceReceivedDate.Text);

        //if (dtinvoicedate > DateTime.Today)
        //    ucError.ErrorMessage = "Invoice Date should not be the future date.";

        //if (dtreceiveddate > DateTime.Today)
        //    ucError.ErrorMessage = "Invoice Received Date should not be the future date.";

        //if (dtreceiveddate < dtinvoicedate)
        //    ucError.ErrorMessage = "Invoice Received date should be later than Invoice date.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    protected void BindPOType()
    {
        ddlPOType.DataTextField = "FLDBULKPOTYPENAME";
        ddlPOType.DataValueField = "FLDBULKPOTYPEID";

        ddlPOType.DataSource = PhoenixPurchaseBulkPurchase.BulkPOTypeList();
        ddlPOType.DataBind();
        ddlPOType.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ddlStockType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        if (ddlStockType.SelectedValue == "SERVICE")
        {
            ddlStockClass.Visible = false;
            ddlStockClass.CssClass = "input";
            lblStockType.Text = "Stock type";
        }
        else
        {
            ddlStockClass.Visible = true;
            ddlStockClass.CssClass = "dropdown_mandatory";
            lblStockType.Text = "Stock type & Store type";
        }
    }
}
