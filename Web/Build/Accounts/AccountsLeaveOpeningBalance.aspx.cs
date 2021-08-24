using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class AccountsLeaveOpeningBalance : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);

       if (!IsPostBack)
        {
            ViewState["EMPID"] = null;
            trSeniority.Visible = false;
            ddlSeniority.Items.Add(new DropDownListItem("--Select--", ""));
            for (int i = 12; i <= (12 * 15); i = i + 12)
            {
                ddlSeniority.Items.Add(new DropDownListItem((i / 12).ToString(), i.ToString()));
            }
            gvLVP.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Gen. Opening Balance", "SEARCH", ToolBarDirection.Right);
            MenuLeaveOpeningBalance.AccessRights = this.ViewState;
            MenuLeaveOpeningBalance.MenuList = toolbar.Show();
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixAccountsLeave.EmployeeListByFileNo(txtFileNo.Text.Trim());
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

    protected void MenuLeaveOpeningBalance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                if (!IsValidOpeningBalance(txtFirstName.Text, ddlVesselSailed.SelectedValue, txtLeaveUnPaid.Text, txtBTBUnPaid.Text, ddlSeniority.SelectedValue, txtMonthlyWages.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("You will not be able to make any changes for the selected Sailing. Are you sure you want to Lock Opening Balance?", "ConfirmLock", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidOpeningBalance(txtFirstName.Text, ddlVesselSailed.SelectedValue, txtLeaveUnPaid.Text, txtBTBUnPaid.Text, "1", "1.00"))
                {
                    ucError.Visible = true;
                    return;
                }
                string[] csv = ddlVesselSailed.SelectedValue.Split(',');
                string empid = csv[0];
                string vslid = csv[1];
                string rnkid = csv[2];
                string btod = csv[3];
                string fromdate = csv[4];
                string todate = csv[5];
                string etod = csv[6];
                DataTable dt = PhoenixAccountsLeaveAllotment.LeaveBalanceOpeningList(int.Parse(empid), int.Parse(vslid), int.Parse(rnkid), DateTime.Parse(btod)
                    , DateTime.Parse(fromdate), DateTime.Parse(todate), DateTime.Parse(etod), decimal.Parse(txtLeaveUnPaid.Text), decimal.Parse(txtBTBUnPaid.Text));
                if (dt.Rows.Count > 0)
                {
                    gvLVP.DataSource = dt;
                    gvLVP.DataBind();
                    trSeniority.Visible = true;
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Gen. Opening Balance", "SEARCH", ToolBarDirection.Right);
                    toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                    MenuLeaveOpeningBalance.AccessRights = this.ViewState;
                    MenuLeaveOpeningBalance.MenuList = toolbar.Show();
                }
                else
                {
                    trSeniority.Visible = false;
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Gen. Opening Balance", "SEARCH", ToolBarDirection.Right);                    
                    MenuLeaveOpeningBalance.AccessRights = this.ViewState;
                    MenuLeaveOpeningBalance.MenuList = toolbar.Show();
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

    private bool IsValidRevesalRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ddlVesselSailed.SelectedValue))
            ucError.ErrorMessage = "Vessel is required.";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File number is required";

        return (!ucError.IsError);
    }

    private bool IsValidOpeningBalance(string Name, string VesselSailed, string LeaveUnPaid, string BTBUnPaid, string Seniority, string MonthlyWages)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(Name.Trim()))
        {
            ucError.ErrorMessage = "Please verify the entered file number is right before confirm <br/>  by clicking search icon next to File number textbox";
        }

        if (string.IsNullOrEmpty(VesselSailed))
            ucError.ErrorMessage = "Vessel sailed is required.";

        if (!General.GetNullableDecimal(LeaveUnPaid).HasValue)
            ucError.ErrorMessage = "Leave unpaid is required";

        if (!General.GetNullableDecimal(BTBUnPaid).HasValue)
            ucError.ErrorMessage = "BTB unpaid is required";

        if (!General.GetNullableInteger(Seniority).HasValue)
            ucError.ErrorMessage = "Seniority year is required";

        if (!General.GetNullableDecimal(MonthlyWages).HasValue)
            ucError.ErrorMessage = "Monthly wages is required";

        return (!ucError.IsError);
    }

    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File number is required";

        return (!ucError.IsError);
    }

    protected void ImgBtnValidFileno_Click(object sender, ImageClickEventArgs e)
    {
        if (IsValidFileNoCheck())
        {
            SetEmployeePrimaryDetails();
            ddlVesselSailed.Items.Clear();
            ddlVesselSailed.DataSource = PhoenixAccountsLeave.EmployeeSailedVesseList(int.Parse(ViewState["EMPID"].ToString()));
            ddlVesselSailed.Items.Insert(0, new DropDownListItem("--Select--", ""));
            ddlVesselSailed.DataBind();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ddlVesselSailed_ItemDataBound(object sender, DropDownListItemEventArgs e)
    {
        
    }

    protected void ucConfirmLock_Click(object sender, EventArgs e)
    {
        string[] csv = ddlVesselSailed.SelectedValue.Split(',');
        string empid = csv[0];
        string vslid = csv[1];
        string rnkid = csv[2];
        string btod = csv[3];
        string fromdate = csv[4];
        string todate = csv[5];
        string etod = csv[6];

        PhoenixAccountsLeaveAllotment.InsertBalanceOpening(int.Parse(empid), int.Parse(vslid), int.Parse(rnkid), DateTime.Parse(btod)
            , DateTime.Parse(fromdate), DateTime.Parse(todate), DateTime.Parse(etod), decimal.Parse(txtLeaveUnPaid.Text), decimal.Parse(txtBTBUnPaid.Text)
            , int.Parse(ddlSeniority.SelectedValue), decimal.Parse(txtMonthlyWages.Text));
        //ucStatus.Text = "Opening Balance Created.";
        RadWindowManager1.RadAlert("Opening Balance Created.", 320, 150, null, "");
    }
}
