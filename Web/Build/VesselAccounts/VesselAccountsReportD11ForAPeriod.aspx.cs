using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportD11ForAPeriod : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportD11.AccessRights = this.ViewState;
            MenuReportD11.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                PhoenixVesselAccountsOrderForm.OrderFormFinalAmountBulkUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null, null);
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
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

    protected void MenuReportD11_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (IsValidDates(txtFromDate.Text, txtToDate.Text))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=D11FORAPERIOD&showmenu=false&showexcel=no&showword=no&VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text;
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

        else if (DateTime.TryParse(FromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ToDate)) > 0)
            ucError.ErrorMessage = "FromDate should be earlier or Equal to Todate.";

        return (!ucError.IsError);
    }
}
