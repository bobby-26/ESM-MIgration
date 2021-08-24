using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportExtraMeals : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportExtraMeals.AccessRights = this.ViewState;
            MenuReportExtraMeals.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";
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
 
    protected void MenuReportExtraMeals_TabStripCommand(object sender, EventArgs e)
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
                    if (ddlReportFor.SelectedValue.ToString() == "-1")
                        ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=EXTRAMEALSFOROWNERS&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&month=" + ddlMonth.SelectedMonth + "&year=" + ddlYear.SelectedYear + "&ReferenceId=" + ddlReportFor.SelectedValue;
                    else
                        ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=EXTRAMEALSFORCHARTERERS&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&month=" + ddlMonth.SelectedMonth + "&year=" + ddlYear.SelectedYear + "&ReferenceId=" + ddlReportFor.SelectedValue;
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
