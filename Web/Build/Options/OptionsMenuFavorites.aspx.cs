using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;


public partial class Options_OptionsMenuFavorites : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Ok", "OK");

        MenuFavorites.MenuList = toolbar.Show();

        gvMenu.DataSource = SessionUtil.MenuAccessFavorites();
        gvMenu.DataBind();
    }

    protected void MenuFavorites_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
            Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
        else
            Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
        Script += "</script>" + "\n";

        NameValueCollection nvc = new NameValueCollection();
        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string menucode = ((Label)_gridview.Rows[nCurrentRow].FindControl("lblMenuCode")).Text;
            SessionUtil.MenuAccessFavoritesAssign(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(menucode));
        }
        gvMenu.DataSource = SessionUtil.MenuAccessFavorites();
        gvMenu.DataBind();
    }

    protected void gvMenu_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }


    protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cb = (CheckBox)e.Row.FindControl("chkMenuRights");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;
            cb.Visible = drv["FLDENABLEYN"].ToString().Equals("0") ? false : true;
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int nCurrentRow = Int32.Parse(cb.Text);
        string menucode = ((Label)gvMenu.Rows[nCurrentRow].FindControl("lblMenuCode")).Text;
        SessionUtil.MenuAccessFavoritesAssign(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(menucode));
        gvMenu.DataSource = SessionUtil.MenuAccessFavorites();
        gvMenu.DataBind();

    }

}
