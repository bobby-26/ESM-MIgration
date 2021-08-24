using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewHRTravelRequestListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        
        travelrequestfilter.AccessRights = this.ViewState;
        travelrequestfilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            txtStartDate.Text = General.GetDateTimeToString(DateTime.Now);
        }
    }


    protected void travelrequestfilter_TabStripCommand(object sender, EventArgs e)
    {
        //try
        //{

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();

            criteria.Add("txtTravelRequestNo", txtTravelRequestNo.Text);
            criteria.Add("uctravelstatus", string.Empty);
            criteria.Add("txtStartDate", txtStartDate.Text);
            criteria.Add("txtEndDate", txtEndDate.Text);
            criteria.Add("txtOrigin", txtOrigin.SelectedValue);
            criteria.Add("txtDestination", txtDestination.SelectedValue);
            criteria.Add("chkApprovedYN", ddlApproved.SelectedValue == "Dummy" ? string.Empty : ddlApproved.SelectedValue);
            Filter.CurrentHRTravelRequestFilter = criteria;

        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
       
    }
}