using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsReportsVesselTrailBalance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuVessel.AccessRights = this.ViewState;
            MenuVessel.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                BindVesselAccount();
                ddlMonth.DataBind();
                ddlMonth.Items.Insert(0, "--Select--");
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=null&month=null&year=null&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindVesselAccount()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , ""
                , ""
                , null
                , null
                , null
                , 1
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
        ddlVesselAccount.DataValueField = "FLDACCOUNTID";
        ddlVesselAccount.DataSource = ds;

        ddlVesselAccount.DataBind();
        ddlVesselAccount.Items.Insert(0, "--Select--");
    }
    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(ddlVesselAccount.SelectedValue, ddlMonth.SelectedValue, ucFinancialYear.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + ddlVesselAccount.SelectedValue + "&month=" + ddlMonth.SelectedValue + "&year=" + ucFinancialYear.SelectedQuick + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string vessel, string month, string year)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Trim().Equals("--Select--"))
            ucError.ErrorMessage = "Vessel is required.";

        if (month.Trim().Equals("--Select--"))
            ucError.ErrorMessage = "Month is required.";

        if (year.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Year is required.";

        return (!ucError.IsError);
    }
}
