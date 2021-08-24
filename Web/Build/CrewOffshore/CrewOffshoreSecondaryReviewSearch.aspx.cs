using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshoreSecondaryReviewSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
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

    private void Clear()
    {
        txtName.Text = "";
        txtFileNo.Text = "";
        ddlRank.SelectedRank = "";
    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("txtName", txtName.Text);
            nvc.Add("txtFileNo", txtFileNo.Text);
            nvc.Add("ddlRank", ddlRank.SelectedRank);
            Filter.CurrentOffshoreSecondaryReviewSearch = nvc;
            Response.Redirect("CrewOffshoreSecondaryReview.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Clear();
        }
    }
}
