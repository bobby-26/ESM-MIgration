using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsOfficeAccrualAllotmentRequestDraftView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ddlBankAccount.EmployeeId = Filter.CurrentOfficeAccrualCrew.Get("empid");
                txtAmount.Text = Filter.CurrentOfficeAccrualCrew.Get("amt");
                BindPortageBill();
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void BindPortageBill()
    {
        NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
        int vslid = int.Parse(nvc.Get("vslid"));
        DataTable dt = PhoenixAccountsOfficeAccrual.ListOfficeAccrualPortageBill(vslid);     
        ddlPortageBill.DataTextFormatString = "{0:dd/MMM/yyyy}";
        ddlPortageBill.DataSource = dt;
        ddlPortageBill.DataBind();
        ddlPortageBill.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAllotment(ddlBankAccount.SelectedBankAccount, txtAmount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
                int employeeid = int.Parse(nvc.Get("empid"));
                Guid componentid = new Guid(nvc.Get("cid"));
                string componentname = txtComponent.Text;
                decimal amt = decimal.Parse(txtAmount.Text);
                int acctid = int.Parse(nvc.Get("aid"));
                int vslid = int.Parse(nvc.Get("vslid"));
                Guid bankacctid = new Guid(ddlBankAccount.SelectedBankAccount);
                int sgid = int.Parse(nvc.Get("sgid"));

                PhoenixAccountsOfficeAccrual.InsertOfficeAccrualAllotmentRequest(employeeid, componentid, componentname, amt, acctid, vslid, bankacctid, 1, null, null, General.GetNullableGuid(ddlPortageBill.SelectedValue), sgid);
                BindData();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('codehelp1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
            DataSet ds = PhoenixAccountsOfficeAccrual.ListOfficeAccrualAllotmentDraftView(int.Parse(nvc.Get("empid"))
                                               , new Guid(nvc.Get("cid")), int.Parse(nvc.Get("aid")), int.Parse(nvc.Get("vslid")), 1
                                               , General.GetNullableDecimal(txtAmount.Text).HasValue ? decimal.Parse(txtAmount.Text) : decimal.Parse(nvc.Get("amt")));
            if (ds.Tables[0].Rows.Count > 0)
                SetPrimaryData(ds.Tables[1]);
            gvPB.DataSource = ds;
            gvPB.VirtualItemCount = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPrimaryData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtFileNo.Text = dr["FLDFILENO"].ToString();
            txtFileNo.ToolTip = dr["FLDFILENO"].ToString();
            txtVesselAccount.Text = dr["FLDDESCRIPTION"].ToString();
            txtVesselAccount.ToolTip = dr["FLDDESCRIPTION"].ToString();
            txtRank.Text = dr["FLDEMPLOYEENAME"].ToString();
            txtRank.ToolTip = dr["FLDEMPLOYEENAME"].ToString();
            txtComponent.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponent.ToolTip = dr["FLDCOMPONENTNAME"].ToString();
        }
    }
    private bool IsValidAllotment(string bankaccountid, string amount)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableGuid(bankaccountid).HasValue)
            ucError.ErrorMessage = "Bank Account is required.";
        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";

        if (General.GetNullableDecimal(amount).HasValue && General.GetNullableDecimal(amount).Value <= 0)
            ucError.ErrorMessage = "Amount should be greater than zero.";

        if (!General.GetNullableGuid(ddlPortageBill.SelectedValue).HasValue)
            ucError.ErrorMessage = "Portage Bill is required.";

        return (!ucError.IsError);
    }
    protected void chkPBPosting_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbk = (CheckBox)sender;
        if (!cbk.Checked)
        {
            if (General.GetNullableDecimal(txtAmount.Text).HasValue && General.GetNullableDecimal(txtAmount.Text).Value > 0)
                Filter.CurrentOfficeAccrualCrew["amt"] = txtAmount.Text;
            Response.Redirect("AccountsOfficeAccrualAllotmentRequestDraftViewNotinPB.aspx", true);
        }
    }
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

}
