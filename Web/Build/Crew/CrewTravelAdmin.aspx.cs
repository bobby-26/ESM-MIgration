using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewTravelAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
                ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Travel", "TRAVEL");
            toolbar.AddButton("Travel Request", "TRAVELREQUEST");

            MenuTravelAdmin.AccessRights = this.ViewState;
            MenuTravelAdmin.MenuList = toolbar.Show();
            MenuTravelAdmin.SelectedMenuIndex = 0;

            ifMoreInfo.Attributes["src"] = "../Crew/CrewTravel.aspx";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                ViewState["CURRENTTAB"] = "../Crew/CrewTravel.aspx";
            }
            else if (CommandName.ToUpper().Equals("TRAVELREQUEST"))
            {
                ViewState["CURRENTTAB"] = "../Crew/CrewTravelRequestAdmin.aspx";
            }            
            ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }   
}
