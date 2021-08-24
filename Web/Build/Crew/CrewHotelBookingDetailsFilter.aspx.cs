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
public partial class CrewHotelBookingDetailsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuHotelBookingFilter.AccessRights = this.ViewState;
        MenuHotelBookingFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           
            txtReferenceNo.Focus();
        }

    }
    protected void MenuHotelBookingFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtReferenceNo", txtReferenceNo.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("txtcityid", txtcityid.Text);
            criteria.Add("ucPurpose", ucPurpose.SelectedReason);
            criteria.Add("txtCheckinDateFrom", txtCheckinDateFrom.Text);
            criteria.Add("txtCheckinDateTo", txtCheckinDateTo.Text);
            criteria.Add("txtCheckOutDateFrom", txtCheckOutDateFrom.Text);
            criteria.Add("txtCheckOutDateTo", txtCheckOutDateTo.Text);
            criteria.Add("rblGuestStatus", rblGuestStatus.SelectedValue);
            criteria.Add("ddlBookingStatus", ddlBookingStatus.SelectedHard);

            Filter.CurrentHotelRequestDetailsFilter = criteria;
        }
        Response.Redirect("../Crew/CrewHotelBookingDetails.aspx", false);
    }
}
