using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class VesselAccountsOtherCrewFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuSignOnOffeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
            txtName.Focus();
    }

    protected void MenuSignOnOffeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtSignOnStartDate", txtSignOnStartDate.Text);
            criteria.Add("txtSignOnEndDate", txtSignOnEndDate.Text);
            criteria.Add("txtSignOffStartDate", txtSignOffStartDate.Text);
            criteria.Add("txtSignOffEndDate", txtSignOffEndDate.Text);
            criteria.Add("ddlSignOnPort", ddlSignOnPort.SelectedValue);
            criteria.Add("ddlSignOffPort", ddlSignoffport.SelectedValue);
            criteria.Add("ddlAccountType", ddlAccountType.SelectedValue);
            criteria.Add("chkSignedOff", chkSignedOff.Checked == true ? "0" : "1");
            Filter.CurrentVesselAccountsOtherCrewFilter = criteria;
        }
        Response.Redirect("VesselAccountsOtherCrew.aspx", false);
    }
}
