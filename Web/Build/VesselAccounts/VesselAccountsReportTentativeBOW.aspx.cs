using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsReportTentativeBOW : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportTentativeBow.AccessRights = this.ViewState;
            MenuReportTentativeBow.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidFilter(string Employee, string date)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(Employee) || Employee == "DUMMY")
            ucError.ErrorMessage = "Select Employee from list";

        if (String.IsNullOrEmpty(date))
        {
            ucError.ErrorMessage = "Date is required";
        }
        else if (DateTime.TryParse(date, out resultdate) && resultdate < DateTime.Today)
        {
            ucError.ErrorMessage = "Date should not be past date";
        }

        return (!ucError.IsError);

    }

    protected void MenuReportTentativeBow_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (IsValidFilter(ddlEmployee.SelectedEmployee, txtDate.Text.Trim()))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=TENTATIVEBOW&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&employee=" + ddlEmployee.SelectedEmployee + "&date=" + txtDate.Text.Trim();
                }
                else
                {
                    ucError.Visible = true;
                    ifMoreInfo.Attributes["src"] = "";
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
