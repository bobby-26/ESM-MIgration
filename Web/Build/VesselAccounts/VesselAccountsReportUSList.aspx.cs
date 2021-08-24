using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountingReportUSList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            rblFormats.SelectedIndex = 0;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=7&reportcode=USCREWLIST&vessel=null&seaport=null&date=null&showexcel=no&showword=no&showmenu=0";

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
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString(), ucSeaPort.SelectedSeaport.ToString(), ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=7&reportcode=USCREWLIST&vessel=" + ucVessel.SelectedVessel.ToString() + "&seaport=" + ucSeaPort.SelectedSeaport.ToString() + "&date=" + ucDate.Text + "&showmenu=0";
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
    public bool IsValidFilter(string vessel, string seaport, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (seaport.Equals("") || seaport.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Seaport";
        }
        if (string.IsNullOrEmpty(date))
        {
            ucError.ErrorMessage = "From Date is required";
        }

        return (!ucError.IsError);
    }
    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFormats.SelectedIndex == 1)
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportUSList.aspx");
        }
        else
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportUSListPage2.aspx");
        }
    }
}
