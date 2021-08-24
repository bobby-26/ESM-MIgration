using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardFavorites : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
            Response.Redirect("PhoenixLogout.aspx");

        cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

        gvFavorites.DataSource = PhoenixRegistersAlerts.GetMenuFavorites(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvFavorites.DataBind();

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFavorites.DataSource = PhoenixRegistersAlerts.GetMenuFavorites(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvFavorites.DataBind();
    }

    protected void gvFavorites_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView _gv = (GridView)sender;
            DataRowView drv = (DataRowView)e.Row.DataItem;
            LinkButton lb = (LinkButton)e.Row.FindControl("lbDescription");
            if (lb != null) lb.Attributes["onclick"] = "javascript:parent.OpenSearchPage('" + drv["FLDURL"].ToString() + "'," + drv["FLDMENUCODE"].ToString() + ");";
        }
    }
}
