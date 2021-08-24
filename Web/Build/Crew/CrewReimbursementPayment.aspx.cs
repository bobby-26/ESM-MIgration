using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewReimbursementPayment : PhoenixBasePage
{
    string rembid = string.Empty;
    public string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            rembid = Request.QueryString["rembid"].ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post", "POST", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDeduction.AccessRights = this.ViewState;
            MenuDeduction.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["ACTIVEYN"] = null;
                ddlBankAccount.EmployeeId = "0";
                SetReimbursement();
                gvRem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDeduction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPaymentMode(ddlPaymentMode.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.UpdateReimbursementPaymentMode(new Guid(rembid), int.Parse(ddlPaymentMode.SelectedHard));
                ucStatus.Text = "Payment Mode Updated.";
            }
            else if (CommandName.ToUpper().Equals("POST"))
            {
                PhoenixCrewReimbursement.PostReimbursementCashPayment(new Guid(rembid));
                ucStatus.Text = "Cash Payment Posted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetReimbursement()
    {
        DataTable dt = PhoenixCrewReimbursement.EditReimbursement(new Guid(rembid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtId.Text = dr["FLDREFERENCENO"].ToString();
            txtDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dr["FLDCREATEDDATE"].ToString()));
            txtFileNo.Text = dr["FLDFILENO"].ToString();
            txtEmployee.Text = dr["FLDEMPLOYEENAME"].ToString();
            lblClaimAmountUSD.Text = "Claim Amount (" + dr["FLDREIMBURCURRENCYCODE"].ToString() + ")";
            txtCurrency.Text = dr["FLDCURRENCYNAME"].ToString();
            ddlBankAccount.EmployeeId = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
            ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(0, General.GetNullableInteger(dt.Rows[0]["FLDEMPLOYEEID"].ToString()));

            txtAmount.Text = dr["FLDAMOUNT"].ToString();
            txtApprovedAmount.Text = dr["FLDAPPROVEDAMOUNT"].ToString();
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtClaimAmount.Text = dr["FLDUSDAMOUNT"].ToString();
            txtDesc.Text = dr["FLDDESCRIPTION"].ToString();
            txtAccountOff.Text = dr["FLDACCOUNTOFNAME"].ToString();

            ddlPaymentMode.SelectedHard = dr["FLDPAYMENTMODE"].ToString();
            ddlBankAccount.CssClass = dr["FLDPAYMENTMODE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT") ? "input_mandatory" : "input";
            ViewState["ACTIVEYN"] = dr["FLDACTIVEYN"].ToString();
            ddlBankAccount.SelectedBankAccount = dt.Rows[0]["FLDBANKACCOUNTID"].ToString();
        }
    }
    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataTable dt = PhoenixCrewReimbursement.ListReimbursementPayment(new Guid(rembid));
        gvRem.DataSource = dt;
        gvRem.VirtualItemCount = dt.Rows.Count;
    }

    protected void Rebind()
    {
        gvRem.SelectedIndexes.Clear();
        gvRem.EditIndexes.Clear();
        gvRem.DataSource = null;
        gvRem.Rebind();
    }
    protected void gvRem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string paymentmode = string.Empty;
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text;
                if (!IsValidateReimbursement(paymentmode, amt))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.InsertCrewReimbursementPayment(new Guid(rembid), null, decimal.Parse(amt));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblpaymentid")).Text.Trim();
                string paymentmode = string.Empty;// ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlPaymentMode")).SelectedHard;
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                if (!IsValidateReimbursement(paymentmode, amt))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.UpdateCrewReimbursementPayment(new Guid(id), null, decimal.Parse(amt));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblpaymentid")).Text.Trim();
                PhoenixCrewReimbursement.DleteCrewReimbursementPayment(new Guid(id));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvRem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) de.Visible = false;
            else if (de != null) { de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName); de.Attributes.Add("onclick", "return fnConfirmDelete();"); }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) ed.Visible = false;
            else if (ed != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            if (ViewState["ACTIVEYN"].ToString() == "0") { ed.Visible = false; de.Visible = false; }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) ad.Visible = false;
            else if (ad != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);

            if (ViewState["ACTIVEYN"].ToString() == "0") ad.Visible = false;
        }
    }
    protected void ddlPaymentModeAdd_TextChangedEvent(object sender, EventArgs e)
    {
        if (ddlPaymentMode.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT"))
        {

            ddlBankAccount.Enabled = true;
            ddlBankAccount.CssClass = "input_mandatory";
        }
        else
        {
            ddlBankAccount.SelectedBankAccount = "";
            ddlBankAccount.Enabled = false;
            ddlBankAccount.CssClass = "input";
        }

    }

    private bool IsValidateReimbursement(string paymentmode, string amt)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //if (!General.GetNullableInteger(paymentmode).HasValue)
        //    ucError.ErrorMessage = "Payment Mode is required";
        if (!General.GetNullableDecimal(amt).HasValue)
            ucError.ErrorMessage = "Payment Amount is required";
        if (General.GetNullableDecimal(amt).HasValue && General.GetNullableDecimal(amt).Value <= 0)
            ucError.ErrorMessage = "Payment Amount should be greater than Zero.";
        return (!ucError.IsError);
    }
    private bool IsValidPaymentMode(string paymentmode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(paymentmode).HasValue)
            ucError.ErrorMessage = "Payment Mode is required";
        return (!ucError.IsError);
    }

}
