using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsLeaveBTBTransfer : PhoenixBasePage
{
    string fileno = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            fileno = Request.QueryString["fileno"];
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Transfer", "TRANSFER",ToolBarDirection.Right);
                BTBTransfer.AccessRights = this.ViewState;
                BTBTransfer.MenuList = toolbarmain.Show();
                SetEmployeePrimaryDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BTBTransfer_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("TRANSFER"))
        {
            string empid = ViewState["EMPID"].ToString();
            if (!IsValidTransfer(empid,txtNoofDays.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixAccountsLeave.UpdateBTBTransfer(int.Parse(empid), decimal.Parse(txtNoofDays.Text));
            ucStatus.Text = "BTB Transfer Successful.";
            txtNoofDays.Text = "";
        }
    }
    private bool IsValidTransfer(string empid,string noofdays)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(noofdays).HasValue)
            ucError.ErrorMessage = "No of days is required";
        if (!General.GetNullableInteger(empid).HasValue)
            ucError.ErrorMessage = "Employee is required";
        return (!ucError.IsError);
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixAccountsLeave.EmployeeListByFileNo(fileno);
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["EMPID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
