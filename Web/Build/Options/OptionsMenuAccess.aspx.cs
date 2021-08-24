using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsMenuAccess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("DB Measure", "MEASURE", ToolBarDirection.Right);
            toolbar.AddButton("DB Module", "MODULE", ToolBarDirection.Right);
            toolbar.AddButton("Access", "FUNCTION", ToolBarDirection.Right);
            toolbar.AddButton("Menus", "MENU", ToolBarDirection.Right);
            toolbar.AddButton("Application", "APPLICATION", ToolBarDirection.Right);
            toolbar.AddButton("User Groups", "USERGROUP", ToolBarDirection.Right);

            MenuSecurityAccessRights.MenuList = toolbar.Show();
            MenuSecurityAccessRights.SelectedMenuIndex = 3;

            if (!IsPostBack)
            {

                ucUserGroup.UserGroupList = SessionUtil.UserGroupList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ddlAdministratorMenuList.DataSource = SessionUtil.AdministratorMenuList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ddlAdministratorMenuList.DataBind();
            }
            if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
            {
                gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucUserGroup.SelectedUserGroup),
                    General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
                gvMenu.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void ddlAdministratorMenuList_TextChanged(object sender, EventArgs e)
    {
        gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ucUserGroup.SelectedUserGroup),
                        General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
        gvMenu.DataBind();
    }

    protected void ucUserGroup_TextChanged(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {
            gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ucUserGroup.SelectedUserGroup),
                General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
            gvMenu.DataBind();
        }
    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.Equals("SAVE"))
            {

            }
            else if (CommandName.Equals("USERGROUP"))
                Response.Redirect("OptionsUserGroups.aspx");
            else if (CommandName.Equals("APPLICATION"))
                Response.Redirect("OptionsApplication.aspx");
            else if (CommandName.Equals("MENU"))
                Response.Redirect("OptionsMenuAccess.aspx");
            else if (CommandName.Equals("FUNCTION"))
                Response.Redirect("OptionsFunctionAccess.aspx");
            else if (CommandName.Equals("MODULE"))
                Response.Redirect("OptionsModule.aspx");
            else if (CommandName.Equals("MEASURE"))
                Response.Redirect("OptionsMeasure.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    //protected void CheckBoxClicked(object sender, EventArgs e)
    //{
    //    RadCheckBox cb = (RadCheckBox)sender;

    //    string menucode = ((RadLabel)e.Item.FindControl("lblMenuCode")).Text;
    //    SessionUtil.MenuAccessAssign(int.Parse(ucUserGroup.SelectedUserGroup), Int32.Parse(menucode));
    //    gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //        int.Parse(ucUserGroup.SelectedUserGroup),
    //        General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
    //    gvMenu.DataBind();

    //}

    protected void Rebind()
    {
        gvMenu.SelectedIndexes.Clear();
        gvMenu.EditIndexes.Clear();
        gvMenu.DataSource = null;
        gvMenu.Rebind();
    }


    protected void gvMenu_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //int GroupRights = ((RadCheckBox)e.Item.FindControl("chkGroupRights")).Checked == true ? 1 : 0;

                string menucode = ((RadLabel)e.Item.FindControl("lblMenuCode")).Text;
                SessionUtil.MenuAccessAssign(int.Parse(ucUserGroup.SelectedUserGroup), Int32.Parse(menucode));

                //if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
                //{
                    gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ucUserGroup.SelectedUserGroup),
                General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
                    gvMenu.DataBind();
                //}
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                {
                    string menucode = ((RadLabel)e.Item.FindControl("lblMenuCode")).Text;
                    SessionUtil.MenuAccessAssign(int.Parse(ucUserGroup.SelectedUserGroup), Int32.Parse(menucode));
                }
                gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(ucUserGroup.SelectedUserGroup),
            General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
                gvMenu.DataBind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvMenu_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkMenuRights");
            cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;

            string _jsDouble = ClientScript.GetPostBackClientHyperlink(cb, drv["FLDMENUCODE"].ToString());

            string js = "javascript:openNewWindow('codehelpactivity', 'OptionsMenuToUserGroups', '" + Session["sitepath"] + "/Options/OptionsMenuToUserGroups.aspx?menucode=" + drv["FLDMENUCODE"].ToString() + "'); return false;";
            LinkButton ib = (LinkButton)e.Item.FindControl("cmdUserGroups");
            if (ib != null)
            {
                ib.Attributes.Clear();
                ib.Attributes.Add("onclick", js);
            }

            LinkButton prb = (LinkButton)e.Item.FindControl("cmdPageRights");
            if (prb != null)
            {
                if (drv["FLDRIGHTS"].ToString().Equals("0") || drv["FLDURL"].ToString().Equals(""))
                    prb.Visible = false;
                else

                    prb.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Options', '" + Session["sitepath"] + "/Options/OptionsFunctionPageAccess.aspx?groupcode=" + ucUserGroup.SelectedUserGroup + "&menucode=" + drv["FLDMENUCODE"].ToString() + "'); return false;");
            }

        }
    }

    protected void gvMenu_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMenu.CurrentPageIndex + 1;

        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {
            gvMenu.DataSource = SessionUtil.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ucUserGroup.SelectedUserGroup),
                General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue));
            gvMenu.DataBind();
        }
    }
}
