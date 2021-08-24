using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportD11 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportD11.AccessRights = this.ViewState;
            MenuReportD11.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                PhoenixVesselAccountsOrderForm.OrderFormFinalAmountBulkUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null, null);
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ddlYear.SelectedYear = DateTime.Today.Year;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 

    protected void MenuReportD11_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                string selecteddate = DateTime.Today.Day.ToString() + "/" + ddlMonth.SelectedMonth.ToString() + "/" + ddlYear.SelectedYear.ToString();

                if (IsValidDates(selecteddate))
                {

                    PhoenixVesselAccountsProvision.UpdateBulkProvisionreport(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), int.Parse(ddlMonth.SelectedMonth.ToString()), int.Parse(ddlYear.SelectedYear.ToString()));
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=D11&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&month=" + ddlMonth.SelectedMonth + "&year=" + ddlYear.SelectedYear.ToString();

                }
                else
                {
                    ucError.Visible = true;
                    ifMoreInfo.Attributes["src"] = "";
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDates(string selecteddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;


        if (DateTime.TryParse(selecteddate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
        {
            ucError.ErrorMessage = "Month should be earlier or Equal to Current Month";
        }
        return (!ucError.IsError);
    }
}
