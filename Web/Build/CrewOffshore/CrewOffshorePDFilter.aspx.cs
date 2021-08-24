using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOffshorePDFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();

    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
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

            criteria.Clear();

            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ddlUser", ddlUser.SelectedUser);
            Filter.CrewOffshorePDFilterSelection = criteria;
        }

        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                        "BookMarkScript", "parent.fnReloadList('Filter', 'ifMoreInfo', null);", true);
    }

    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
        ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableInteger(ucPrincipal.SelectedAddress), 0);
    }
}
