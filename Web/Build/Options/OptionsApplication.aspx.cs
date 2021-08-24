using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class OptionsApplication : PhoenixBasePage
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
         

            ApplicationSecurityAccessRights.MenuList = toolbar.Show();
            ApplicationSecurityAccessRights.SelectedMenuIndex = 4;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ucUserGroup.UserGroupList = SessionUtil.UserGroupList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            }

            if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
            {
                gvApplication.DataSource = SessionUtil.ApplicationAccessSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup));
                //gvApplication.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ucUserGroup_TextChanged(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {
            gvApplication.DataSource = SessionUtil.ApplicationAccessSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup));
            gvApplication.Rebind();
        }
    }
    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            //if (CommandName.Equals("SAVE"))
            //{

            //}
            if (CommandName.Equals("USERGROUP"))
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
    protected void Rebind()
    {
        gvApplication.SelectedIndexes.Clear();
        gvApplication.EditIndexes.Clear();
        gvApplication.DataSource = null;
        gvApplication.Rebind();
    }

    protected void gvApplication_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Applicationcode = (((RadLabel)e.Item.FindControl("lblApplicationCode")).Text);
                bool ApplicationRights = ((RadCheckBox)e.Item.FindControl("chkApplicationRights")).Checked == true ? true : false;
                SessionUtil.ApplicationAccessAssign(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup), Int32.Parse(Applicationcode), ApplicationRights);

                //   gvApplication.DataSource = SessionUtil.ApplicationAccessSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup));
                //  gvApplication.DataBind();

                if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
                {
                    gvApplication.DataSource = SessionUtil.ApplicationAccessSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup));
                    gvApplication.Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvApplication_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadCheckBox cb = ((RadCheckBox)e.Item.FindControl("chkApplicationRights"));

            // if (cb != null)
            cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;

            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(cb, drv["FLDAPPLICATIONCODE"].ToString());
            // Add this javascript to the onclick Attribute of the row
            cb.Attributes["onclick"] = e.Item.FindControl("lnkCheck").ClientID + ".click();";


        }
    }

    protected void gvApplication_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvApplication.CurrentPageIndex + 1;
        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {
            gvApplication.DataSource = SessionUtil.ApplicationAccessSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucUserGroup.SelectedUserGroup));
        }
    }
}
