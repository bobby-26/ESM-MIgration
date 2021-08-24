using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportLeaveWagesAndPerformanceBonus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportLeaveWagesPerformanceBonus.AccessRights = this.ViewState;
            MenuReportLeaveWagesPerformanceBonus.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";
                if (string.IsNullOrEmpty(txtToDate.Text))
                    txtToDate.Text = DateTime.Now.ToLongDateString();
                ListPortageBill();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReportLeaveWagesPerformanceBonus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (IsValidDates(txtFromDate.Text, txtToDate.Text))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=LEAVEWAGESANDPERFORMANCEBONUS&showmenu=false&showexcel=no&showword=no&VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text;

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

        if (!DateTime.TryParse(txtToDate.Text, out resultdate))
            ucError.ErrorMessage = "To Date is not a valid date format.";

        return (!ucError.IsError);
    }

    private void ListPortageBill()
    {
        DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortageBillHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlHistory.DataSource = dt;
        ddlHistory.DataBind();
        ddlHistory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        if (txtToDate.Text != null && txtToDate.Text != string.Empty)
        {
            foreach (RadComboBoxItem li in ddlHistory.Items)
            {
                if (li.Value == string.Empty) continue;
                if (DateTime.Parse(txtToDate.Text) == DateTime.Parse(li.Value))
                {
                    li.Selected = true;
                    break;
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            txtFromDate.Text = General.GetNullableDateTime(dt.Rows[0]["FLDPBCLOSGINDATE"].ToString()).Value.AddDays(1).ToString();
        }
    }

    protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                txtToDate.Text = DateTime.Now.ToShortDateString();

            DateTime d = DateTime.Now;
            DataSet ds = new DataSet();
            if (General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                d = General.GetNullableDateTime(ddlHistory.SelectedValue).Value;
            else if (General.GetNullableDateTime(txtToDate.Text).HasValue)
                d = General.GetNullableDateTime(txtToDate.Text).Value;

            ds = PhoenixVesselAccountsPortageBill.ListVesselPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, d);
            txtToDate.Text = d.ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
            }
            else
            {
                txtFromDate.Text = "01/" + d.Month.ToString() + "/" + d.Year.ToString();
            }

            if (IsValidDates(txtFromDate.Text, txtToDate.Text))
            {
                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=LEAVEWAGESANDPERFORMANCEBONUS&showmenu=false&VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text;

            }
            else
            {
                ucError.Visible = true;
                ifMoreInfo.Attributes["src"] = "";
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }
}
