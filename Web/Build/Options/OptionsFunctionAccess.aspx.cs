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

public partial class OptionsFunctionAccess : PhoenixBasePage
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
            MenuSecurityAccessRights.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {

            }
            BindUserGroup();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void ucUserGroup_TextChanged(object sender, EventArgs e)
    {
        BindUserGroup();
    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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


    private void BindUserGroup()
    {
        gvUserGroup.DataSource = SessionUtil.MenuTree(1); //UserAccessRights.GroupFunctionRightsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
        gvUserGroup.DataBind();
    }

    protected void gvUserGroup_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        BindUserGroup();
    }

    protected void gvUserGroup_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lb = (RadLabel)e.Item.FindControl("lblMenuName");
            if (lb != null && drv["FLDEDITYN"].ToString().Equals("0"))
            {
                lb.Font.Bold = false;
                lb.ForeColor = System.Drawing.Color.DarkBlue;
            }

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblMenuCode");
            LinkButton prb = (LinkButton)e.Item.FindControl("cmdPageRights");
            if (prb != null)
            {
                if (drv["FLDEDITYN"].ToString().Equals("0"))
                    prb.Visible = false;
                else

                    prb.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FunctionPermission', '" + Session["sitepath"] + "/Options/OptionsFunctionPermission.aspx?menucode=" + lbl.Text + "'); return true;");
            }
        }
    }

    protected void gvUserGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUserGroup.CurrentPageIndex + 1;
        BindUserGroup();
    }
}
