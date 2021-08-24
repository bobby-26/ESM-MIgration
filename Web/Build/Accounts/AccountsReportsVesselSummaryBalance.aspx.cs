using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class AccountsReportsVesselSummaryBalance : PhoenixBasePage
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
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=null&month=null&year=null&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(ddlVessel.SelectedVessel, ucFinancialYear.SelectedQuick, ddlMonth.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + ddlVessel.SelectedVessel + "&month="+ddlMonth.SelectedValue+"&year="+ucFinancialYear.SelectedQuick+"&showmenu=0";
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

        if (vessel.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        if (month.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Month is required.";

        if (year.Trim().Equals(""))
            ucError.ErrorMessage = "Year is required.";

        return (!ucError.IsError);
    }
}
