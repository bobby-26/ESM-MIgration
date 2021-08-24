using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportBondOwnersCharterers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState); PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportBond.AccessRights = this.ViewState;
            MenuReportBond.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";

                if (string.IsNullOrEmpty(txtFromDate.Text))
                    txtFromDate.Text = "01/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
                if (string.IsNullOrEmpty(txtToDate.Text))
                    txtToDate.Text = DateTime.Now.ToLongDateString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReportBond_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (IsValidDates(txtFromDate.Text, txtToDate.Text))
                {
                    if (ddlReportFor.SelectedValue.ToString() == "-1")
                        ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=BONDFOROWNERS&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&EmployeeId=" + ddlReportFor.SelectedValue;
                    else if (ddlReportFor.SelectedValue.ToString() == "1")
                        ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=BONDFORSTAFF&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&EmployeeId=" + ddlReportFor.SelectedValue;
                    else
                        ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=BONDFORCHARTERERS&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&EmployeeId=" + ddlReportFor.SelectedValue;
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
    private bool IsValidDates(string FromDate, string ToDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        if (!DateTime.TryParse(FromDate, out resultdate))
            ucError.ErrorMessage = "From Date is not a valid date format.";

        if (!DateTime.TryParse(ToDate, out resultdate))
            ucError.ErrorMessage = "To Date is not a valid date format.";

        else if (DateTime.TryParse(FromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "From Date Should be earlier or Equal to Today";

        else if (DateTime.TryParse(ToDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "To Date Should be earlier or Equal to Today";

        else if (DateTime.TryParse(FromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ToDate)) > 0)
            ucError.ErrorMessage = "FromDate should be earlier or Equal to Todate.";

        if ((Convert.ToDateTime(FromDate)).Month.ToString() != (Convert.ToDateTime(ToDate)).Month.ToString())
            ucError.ErrorMessage = "Selected Period Should Be Within a Month.";

        else if ((Convert.ToDateTime(FromDate)).Year.ToString() != (Convert.ToDateTime(ToDate)).Year.ToString())
            ucError.ErrorMessage = "Selected Period Should Be Within a Month.";

        return (!ucError.IsError);
    }
}
