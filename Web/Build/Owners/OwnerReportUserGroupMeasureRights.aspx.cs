using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Owners;

public partial class OwnerReportUserGroupMeasureRights : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Measure", "MEASURE", ToolBarDirection.Right);
        toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);

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

        DataTable dt = PhoenixOwnerReport.OwnerReportModulelist(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode);

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
            DataTable dt = PhoenixOwnerReport.UserGroupDashboardMeasureRightslist
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
            if (CommandName.Equals("SECTION"))
                Response.Redirect("OwnerReportUserGroupSectionRights.aspx");
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

                PhoenixOwnerReport.UserGroupMeasureRightsInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     General.GetNullableInteger(ucUserGroup.SelectedUserGroup),
                new Guid(MeasureCode.ToString()),
                Rights
                    );

                PhoenixOwnerReport.UserMeasureRightsUpdate(
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