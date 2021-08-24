using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewHRTravelRequestApprovalFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        travelrequestfilter.MenuList = toolbar.Show();

    }

    protected void travelrequestfilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Add("txtTravelRequestNo", txtTravelRequestNo.Text);
            criteria.Add("uctravelstatus", uctravelstatus.SelectedHard);
            criteria.Add("txtStartDate", txtStartDate.Text);
            criteria.Add("txtEndDate", txtEndDate.Text);
            criteria.Add("txtOrigin", txtOrigin.SelectedValue);
            criteria.Add("txtDestination", txtDestination.SelectedValue);
            criteria.Add("chkApprovedYN", chkApprovedYN.Checked== true ? "1" : string.Empty);

            Filter.CurrentHRTravelRequestApprovalFilter = criteria;

        }

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
}