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
public partial class CrewTravelVisaFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtAgentId.Attributes.Add("style", "display:none");
            txtReferenceNo.Focus();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuTravelVisaFilter.MenuList = toolbar.Show();


    }
    protected void MenuTravelVisaFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtReferenceNo", txtReferenceNo.Text);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("txtAgentId", txtAgentId.Text);
            criteria.Add("ucCountry", ucCountry.SelectedCountry);
            Filter.CurrentTravelVisaFilterCriteria = criteria;
        }
        Response.Redirect("../Crew/CrewTravelVisaRequest.aspx", false);

    }
}
