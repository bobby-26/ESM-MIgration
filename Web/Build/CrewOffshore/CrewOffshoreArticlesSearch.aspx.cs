using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshoreArticlesSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("txtFromdate", txtFromdate.Text);
            nvc.Add("txtTodate", txtTodate.Text);
            nvc.Add("chkInactive", chkInactive.Checked.Value ? "1" : "0");
            nvc.Add("ucArticalType", ucArticalType.SelectedQuick);
            nvc.Add("UcVessel",  UcVessel.SelectedVessel);
            Filter.Currentcrewarticle = nvc;

            Response.Redirect("CrewOffshoreArticles.aspx", true);
        }
    }
}
