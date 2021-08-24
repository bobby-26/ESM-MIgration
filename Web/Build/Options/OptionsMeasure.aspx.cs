using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
public partial class OptionsMeasure : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("DB Measure", "MEASURE", ToolBarDirection.Right);
        toolbar.AddButton("DB Module", "MODULE", ToolBarDirection.Right);
        toolbar.AddButton("Access", "FUNCTION", ToolBarDirection.Right);
        toolbar.AddButton("Menus", "MENU", ToolBarDirection.Right);
        toolbar.AddButton("Application", "APPLICATION", ToolBarDirection.Right);
        toolbar.AddButton("User Groups", "USERGROUP", ToolBarDirection.Right);

        MenuTab.MenuList = toolbar.Show();
        MenuTab.SelectedMenuIndex = 0;
        
    }

    protected void Rebind()
    {
        gvMeasures.SelectedIndexes.Clear();
        gvMeasures.EditIndexes.Clear();
        gvMeasures.DataSource = null;
        gvMeasures.Rebind();
    }
    private void BindModuleList()
    {

        DataTable dt = PhoenixDashboardOption.UserGroupDashboardModuleRightsDropDownlist(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int32.Parse(ucUserGroup.SelectedUserGroup));

        ddlModulelist.Items.Clear();
        ddlModulelist.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlModulelist.DataSource = dt;
        ddlModulelist.DataBind();
    }

    protected void gvMeasures_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMeasures.CurrentPageIndex + 1;

        if (General.GetNullableInteger(ucUserGroup.SelectedUserGroup) != null)
        { 
            BindData();
        }    
    }

    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixDashboardOption.UserGroupDashboardMeasureRightslist
                (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(ucUserGroup.SelectedUserGroup),
                General.GetNullableInteger(ddlModulelist.SelectedValue)
                );

                gvMeasures.DataSource = dt;         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlModulelist_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Rebind();
    }


    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
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

    protected void gvMeasures_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string MeasureCode = General.GetNullableString(((RadLabel)e.Item.FindControl("lblMeasureCode")).Text);
                int Rights = ((RadCheckBox)e.Item.FindControl("chkMeasureRights")).Checked == true ? 1 : 0;

                PhoenixDashboardOption.UserGroupDashboardMeasureRightsInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     General.GetNullableInteger(ucUserGroup.SelectedUserGroup),
                MeasureCode,
                Rights
                    );

                PhoenixDashboardOption.UserGroupDashboardMeasureRightsUpdate(
                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   int.Parse(ucUserGroup.SelectedUserGroup)
                  );
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ucUserGroup_TextChangedEvent(object sender, EventArgs e)
    {
        ddlModulelist.ClearSelection();
        BindModuleList();
        gvMeasures.Rebind();
    }

    protected void gvMeasures_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkMeasureRights");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "UPDATE");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDRIGHTS").ToString().Equals("1") ? true : false;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}