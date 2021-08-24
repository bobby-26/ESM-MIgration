using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewReimbursementDeduction : PhoenixBasePage
{
    string rembid = string.Empty;
    public string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            rembid = Request.QueryString["rembid"].ToString();
            if (!IsPostBack)
            {
                ViewState["CRR"] = PhoenixRegistersHard.EditHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "CRR").Tables[0].Rows[0]["FLDHARDCODE"].ToString();
                SetReimbursement();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDeduction.AccessRights = this.ViewState;
            MenuDeduction.MenuList = toolbar.Show();
            gvRem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                if (!IsValidDeduction(txtNoofInstallment.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.UpdateCrewReimbursementDeduction(new Guid(rembid), byte.Parse(txtNoofInstallment.Text), byte.Parse(chk_CurrentContract.Checked ? "1" : "0"));
                ucStatus.Text = "Deduction Information Details Saved";
                BindData();
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
            // ucTitle.Text = dr["FLDEARNINGDEDUCTION"].ToString() == "-2" ? "Recovery (Monthly / OneTime)" : "Reimbursement (Monthly / OneTime)";
            txtId.Text = dr["FLDREFERENCENO"].ToString();
            txtDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dr["FLDCREATEDDATE"].ToString()));
            txtFileNo.Text = dr["FLDFILENO"].ToString();
            txtEmployee.Text = dr["FLDEMPLOYEENAME"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYNAME"].ToString();
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
            txtDesc.Text = dr["FLDDESCRIPTION"].ToString();
            txtNoofInstallment.Text = dr["FLDNOOFINSTALLMENT"].ToString();
            chk_CurrentContract.Checked = (dr["FLDCURRENTCONTRACTYN"].ToString() == "1" ? true : false);
            //if (ViewState["CRR"].ToString() == dr["FLDHARDCODE"].ToString())
            //    MenuDeduction.Visible = false;
            if (dr["FLDACTIVEYN"].ToString() == "0")
            {
                MenuDeduction.Visible = false;
            }
        }
    }
    private void BindData()
    {

        DataTable dt = PhoenixCrewReimbursement.ListReimbursementPayment(new Guid(rembid));
        gvRem.DataSource = dt;
        gvRem.VirtualItemCount = dt.Rows.Count;

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
                string paymentmode = ((RadLabel)e.Item.FindControl("lblPaymentModeid")).Text.Trim();
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text;
                if (!IsValidateReimbursement(paymentmode, amt))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.InsertCrewReimbursementPayment(new Guid(rembid), int.Parse(paymentmode), decimal.Parse(amt));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblpaymentid")).Text.Trim();
                string paymentmode = ((RadLabel)e.Item.FindControl("lblPaymentModeid")).Text.Trim();
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;

                if (!IsValidateReimbursement(paymentmode, amt))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReimbursement.UpdateCrewReimbursementPayment(new Guid(id), int.Parse(paymentmode), decimal.Parse(amt));
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
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) de.Visible = false;
            else if (de != null) { de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName); de.Attributes.Add("onclick", "return fnConfirmDelete();"); }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) ed.Visible = false;
            else if (ed != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");

            if (ad != null) ad.Visible = false;
        }
    }


    private bool IsValidateReimbursement(string paymentmode, string amt)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(paymentmode).HasValue)
            ucError.ErrorMessage = "Recovery Mode is required";
        if (!General.GetNullableDecimal(amt).HasValue)
            ucError.ErrorMessage = "Recovery Amount is required";
        if (General.GetNullableDecimal(amt).HasValue && General.GetNullableDecimal(amt).Value <= 0)
            ucError.ErrorMessage = "Recovery Amount should be greater than Zero.";
        return (!ucError.IsError);
    }
    private bool IsValidDeduction(string noofinstallment)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(noofinstallment).HasValue)
            ucError.ErrorMessage = "No of Installment is required";
        if (General.GetNullableDecimal(noofinstallment).HasValue && General.GetNullableDecimal(noofinstallment).Value <= 0)
            ucError.ErrorMessage = "No of Installment should be greater than Zero.";
        return (!ucError.IsError);
    }

}
