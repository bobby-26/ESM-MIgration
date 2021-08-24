using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseSchedulePOGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        txtVendorId.Attributes.Add("style", "visibility:hidden;");
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
            ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                        
            ucVessel.Enabled = true;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != string.Empty)
            {
                ViewState["SCHEDULEPOID"] = Filter.CurrentSelectedSchedulePO.ToString();
                BulkPOEdit();
            }
            else
                ViewState["SCHEDULEPOID"] = "";
        }
    }

    protected void BindFrequency()
    { 
        //ddlFrequencyType.Items.Add()
    }

    protected void BulkPOEdit()
    {
        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOEdit(new Guid(ViewState["SCHEDULEPOID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormTitle.Text = dr["FLDFORMTITLE"].ToString();
            txtFormNumber.Text = dr["FLDFORMNO"].ToString();
            ucStartDate.Text = dr["FLDSTARTDATE"].ToString();
            ucEndDate.Text = dr["FLDENDDATE"].ToString();                         
            ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtVendorId.Text = dr["FLDVENDORID"].ToString();
            txtVenderName.Text = dr["FLDVENDORNAME"].ToString();
            txtVendorCode.Text = dr["FLDVENDORCODE"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETCODEID"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETCODENAME"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetgroupId.Text = dr["FLDBUDGETGROUPID"].ToString();
            ddlFrequencyType.SelectedValue = dr["FLDFREQUENCYTYPE"].ToString();
            ucFrequencyValue.Text = dr["FLDFREQUENCY"].ToString();
            ucLastScheduleDoneDate.Text = dr["FLDLASTSCHEDULEDONEDATE"].ToString();
            ucNextScheduleDate.Text = dr["FLDNEXTSCHEDULEDATE"].ToString();
            ddlStockClassType.SelectedHard = dr["FLDSTORETYPE"].ToString();
            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucVessel.Enabled = false;
            //ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            if (dr["FLDACTIVEYN"] != null && dr["FLDACTIVEYN"].ToString() == "1")           
                rdoActive.Checked = true;
            else
                rdoInActive.Checked = true;

            if (dr["FLDAPPROVEDYN"].ToString() == "1")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("New", "NEW");

                MenuBulkPurchase.AccessRights = this.ViewState;
                MenuBulkPurchase.MenuList = toolbar.Show();
            }
            //if (dr["FLDSTOCKTYPE"] != null && dr["FLDSTOCKTYPE"].ToString() == "STORE")
            //{
            //    ddlStockClassType.Visible = true;
            //    lblStokType.Text = "Stock Type & Store Type";
            //}
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

            if (General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString()) == null)
            {
                try
                {
                    PhoenixPurchaseSchedulePO.SchedulePOInsert(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableString(txtFormTitle.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                , General.GetNullableInteger(txtBudgetId.Text)
                                                                , DateTime.Parse(ucStartDate.Text)
                                                                , DateTime.Parse(ucEndDate.Text)
                                                                , null
                                                                , null
                                                                , int.Parse(ddlFrequencyType.SelectedValue)
                                                                , int.Parse(ucFrequencyValue.Text)
                                                                , (rdoActive.Checked == true) ? 1 : 0
                                                                , General.GetNullableInteger(ddlStockClassType.SelectedHard)
                                                                , int.Parse(ucVessel.SelectedVessel));

                    ucStatus.Text = "Schedule PO is created";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                Reset();
                Filter.CurrentSelectedSchedulePO = null;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }

            else
            {
                try
                {
                    PhoenixPurchaseSchedulePO.SchedulePOUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["SCHEDULEPOID"].ToString())
                                                                , General.GetNullableString(txtFormTitle.Text.Trim())
                                                                , null
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                , General.GetNullableInteger(txtBudgetId.Text)
                                                                , DateTime.Parse(ucStartDate.Text)
                                                                , DateTime.Parse(ucEndDate.Text)
                                                                , null
                                                                , null
                                                                , int.Parse(ddlFrequencyType.SelectedValue)
                                                                , int.Parse(ucFrequencyValue.Text)
                                                                , (rdoActive.Checked == true) ? 1 : 0
                                                                , 0
                                                                , General.GetNullableInteger(ddlStockClassType.SelectedHard) 
                                                                , int.Parse(ucVessel.SelectedVessel)
                                                                ) ;                                                                
                    BulkPOEdit();
                    ucStatus.Text = "Schedule PO is updated";
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
        ViewState["SCHEDULEPOID"] = "";
        txtFormTitle.Text = txtFormNumber.Text = "";
        txtVendorId.Text = txtVendorCode.Text = txtVenderName.Text = "";
        txtBudgetCode.Text = txtBudgetgroupId.Text = txtBudgetName.Text = txtBudgetId.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        ucFrequencyValue.Text = "";
        ddlFrequencyType.SelectedValue = "0";
        ucStartDate.Text = ucEndDate.Text = "";
        ucLastScheduleDoneDate.Text = ucNextScheduleDate.Text = "";
        ddlStockClassType.SelectedHard = "";
        //ddlStockType.SelectedIndex = 0;
        rdoInActive.Checked = false;
        rdoActive.Checked = true;

        ddlStockClassType.Visible = false;
        lblStokType.Text = "Stock Type";
        ddlCurrencyCode.SelectedCurrency = "10";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedVessel = "";
            ucVessel.Enabled = true;
        }
    }
    public bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtStartDate = null, dtEndDate = null;

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null) 
            ucError.ErrorMessage = "Vessel is Required.";

        if (General.GetNullableString(txtFormTitle.Text.Trim()) == null) 
            ucError.ErrorMessage = "Form Title is Required.";       

        if (General.GetNullableString(txtVendorId.Text) == null)
            ucError.ErrorMessage = "Vendor is Required.";

        if (General.GetNullableInteger(txtBudgetId.Text) == null)
            ucError.ErrorMessage = "Budget Code is Required.";

        if (General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is Required.";

        //if (General.GetNullableString(ddlStockType.SelectedValue) == null)
        //    ucError.ErrorMessage = "Stock Type is Required.";
        //else
        //{
        //    if ( ddlStockType.SelectedValue == "STORE" && General.GetNullableInteger(ddlStockClassType.SelectedHard) == null)
        //        ucError.ErrorMessage = "Store Type is Required.";
        //}

        if (General.GetNullableDateTime(ucStartDate.Text) == null)
            ucError.ErrorMessage = "Start Date is Required.";
        else
            dtStartDate = DateTime.Parse(ucStartDate.Text);

        if (General.GetNullableDateTime(ucEndDate.Text) == null)
            ucError.ErrorMessage = "End Date is Required.";
        else
            dtEndDate = DateTime.Parse(ucEndDate.Text);
        
        if (dtEndDate < dtStartDate)
            ucError.ErrorMessage = "'End Date' should be later than 'Start date'.";

        if (General.GetNullableInteger(ddlFrequencyType.SelectedValue) == 0)
            ucError.ErrorMessage = "Frequency type is Required.";

        if(General.GetNullableInteger(ucFrequencyValue.Text.Trim()) == null)
            ucError.ErrorMessage = "Frequency is Required.";
                                                               
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void ddlStockType_Changed(object sender, EventArgs e)
    {
        if (ddlStockType.SelectedValue.ToString() == "STORE")
        {
            ddlStockClassType.Visible = true;
            lblStokType.Text = "Stock Type & Store Type";
        }
        else
        {
            ddlStockClassType.Visible = false;
            lblStokType.Text = "Stock Type";
        }
    }
}
