using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsReportTrialBalanceReportingCurrency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (ddlType.SelectedValue == "1")
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=TBBASECURRENCY&date=" + ucDate.Text + "&currency=" + ddlType.SelectedValue + "&showmenu=0";
                    else
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=TBREPORTINGCURRENCY&date=" + ucDate.Text + "&currency=" + ddlType.SelectedValue + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }
        return (!ucError.IsError);
    }
}
