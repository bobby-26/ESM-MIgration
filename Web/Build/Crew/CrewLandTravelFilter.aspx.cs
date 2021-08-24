using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLandTravelFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuLandTravelFilter.AccessRights = this.ViewState;
        MenuLandTravelFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {          
            txtReferenceNo.Focus();
        }
    }

    protected void MenuLandTravelFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtReferenceNo", txtReferenceNo.Text.Trim());
            criteria.Add("txtcityid", txtcityid.Text);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("ucReqFromDate", ucReqFromDate.Text);
            criteria.Add("ucReqToDate", ucReqToDate.Text);
            criteria.Add("ucTypeOfDuty", ucTypeOfDuty.SelectedHard);
            criteria.Add("ddlStatus", ddlStatus.SelectedHard);
            Filter.CurrentLandTravelFilterCriteria = criteria;
        }
        Response.Redirect("../Crew/CrewLandTravelRequest.aspx", false);
    }
}
