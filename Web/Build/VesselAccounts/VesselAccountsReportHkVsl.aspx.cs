using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportHkVsl : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportHkVsl.AccessRights = this.ViewState;
            MenuReportHkVsl.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                ddlYear.SelectedYear = DateTime.Now.Year;
                ddlMonth.SelectedMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidFilter(string month, string year)
    {
        if (month.Trim() == string.Empty)
            ucError.ErrorMessage = "Please Select Month";
        if (year.Trim() == string.Empty)
            ucError.ErrorMessage = "Please Select Year";
        return (!ucError.IsError);
    }
    protected void MenuReportHkVsl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlMonth.SelectedMonth, ddlYear.SelectedYear.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=HKVSL&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() +
                        "&month=" + ddlMonth.SelectedMonth + "&year=" + ddlYear.SelectedYear;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
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
