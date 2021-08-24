using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportUSList2 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            rblFormats.SelectedIndex = 1;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=USCREWLIST1&showmenu=0";

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
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=USCREWLIST1&showmenu=0";
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
            Response.Redirect("../Crew/CrewReportUSList.aspx");
        }
        else
        {
            Response.Redirect("../Crew/CrewReportUSList2.aspx");
        }
    }
}
