using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class OptionsUserMenuAccess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Command Permissions", "COMMANDPERMISSIONS", ToolBarDirection.Right);
        MenuSecurityAccessRights.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
        string usercode = Request.QueryString["usercode"].ToString();
        gvMenu.DataSource = FunctionPermissions.UserMenuPermissionsList(int.Parse(usercode));
        gvMenu.DataBind();
    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.Equals("COMMANDPERMISSIONS"))
        {
            Response.Redirect("OptionsUserCommandAccess.aspx?usercode=" + Request.QueryString["usercode"].ToString());
            Rebind();
        }
    }
    protected void Rebind()
    {
        gvMenu.SelectedIndexes.Clear();
        gvMenu.EditIndexes.Clear();
        gvMenu.DataSource = null;
        gvMenu.Rebind();
    }
    protected void CheckBoxClicked(object sender, EventArgs e)
    {


    }
    protected void gvMenu_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("USERGROUPRIGHTS"))

            {
                string usercode = Request.QueryString["usercode"].ToString();
                string menuvalue = ((RadLabel)e.Item.FindControl("lblMenuValue")).Text;

                RadGrid gv = (RadGrid)e.Item.FindControl("gvUserGroups");

                gv.DataSource = FunctionPermissions.UserMenuPermissionByUserGroup(int.Parse(usercode), menuvalue);
                gv.DataBind();


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
}
