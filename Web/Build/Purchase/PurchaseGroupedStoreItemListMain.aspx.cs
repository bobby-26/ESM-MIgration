using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PurchaseGroupedStoreItemListMain : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
			//cmdHiddenSubmit.Attributes.Add("style", "display:none;");
			PhoenixToolbar storemenu = new PhoenixToolbar();
			storemenu.AddButton("Deck", "DECK");
			storemenu.AddButton("Engine", "ENGINE");
			storemenu.AddButton("Cabin", "CABIN");
            storemenu.AddButton("Safety", "SAFETY");
            MenuStores.MenuList = storemenu.Show();
			MenuStores.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
				Filter.GroupedStoreVesselSelected = null;
				ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseGroupedStoreItemList.aspx?StoreType=1";
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStores_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DECK"))
            {
				MenuStores.SelectedMenuIndex = 0;
                ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseGroupedStoreItemList.aspx?StoreType=1";
            }
            else if (CommandName.ToUpper().Equals("ENGINE"))
            {
				MenuStores.SelectedMenuIndex = 1;
                ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseGroupedStoreItemList.aspx?StoreType=2";
            }
			else if (CommandName.ToUpper().Equals("CABIN"))
			{
				MenuStores.SelectedMenuIndex = 2;
				ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseGroupedStoreItemList.aspx?StoreType=3";
			}
            else if (CommandName.ToUpper().Equals("SAFETY"))
            {
                MenuStores.SelectedMenuIndex = 3;
                ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseGroupedStoreItemList.aspx?StoreType=4";
            }
            RadScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
