using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewTravelRequestFilter : PhoenixBasePage
{
   // string vesselid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            travelrequestfilter.MenuList = toolbar.Show();

            if (Request.QueryString["office"] != null)
            {
                ddlofficetravelyn.SelectedValue = "1";
                ddlofficetravelyn.Enabled = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();
        criteria.Add("ddlPICVesselList", ucVessel.SelectedVessel);
        criteria.Add("ucpurpose", ucpurpose.SelectedReason);
        criteria.Add("uctravelstatus", uctravelstatus.SelectedHard);
        criteria.Add("txtStartDate", txtStartDate.Text);
        criteria.Add("txtEndDate", txtEndDate.Text);
        criteria.Add("ddlofficetravelyn", ddlofficetravelyn.SelectedValue);
        criteria.Add("txtTravelRequestNo", txtTravelRequestNo.Text);
        criteria.Add("txtPassengerName", txtPassengerName.Text);
        criteria.Add("ucZone", ucZone.SelectedZone);
        criteria.Add("ucport", ucport.SelectedValue);

        Filter.CurrentTravelRequestFilter = criteria;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
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

            criteria.Add("vesselid", ucVessel.SelectedVessel);
            criteria.Add("ucpurpose", ucpurpose.SelectedReason);
            criteria.Add("uctravelstatus", uctravelstatus.SelectedHard);
            criteria.Add("txtStartDate", txtStartDate.Text);
            criteria.Add("txtEndDate", txtEndDate.Text);
            criteria.Add("txtOrigin", txtOrigin.SelectedValue);
            criteria.Add("txtDestination", txtDestination.SelectedValue);
            criteria.Add("ddlofficetravelyn", ddlofficetravelyn.SelectedValue);
            criteria.Add("txtTravelRequestNo", txtTravelRequestNo.Text);
            criteria.Add("txtPassengerName", txtPassengerName.Text);
            criteria.Add("ucZone", ucZone.SelectedZone);
            criteria.Add("ucport", ucport.SelectedValue);
            criteria.Add("txtFileNo", txtFileNo.Text);
           

            if (Request.QueryString["office"] != null && Request.QueryString["office"] != string.Empty)
            {
                Filter.CurrentOfficeTravelRequestFilter = criteria;
            }
            else
            {
                Filter.CurrentTravelRequestFilter = criteria;
            }
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }   
    
}
