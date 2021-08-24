using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPickListMultipleComponents : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (Request.QueryString["spareitemid"] != null && Request.QueryString["spareitemid"].ToString() != "")
                ViewState["SPAREITEMID"] = Request.QueryString["spareitemid"].ToString();
            else
                ViewState["SPAREITEMID"] = null;
        }
       
    }

    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                gvComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;


            int vesselid ;
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixCommonInventory.ComponentSearch(vesselid, txtNumberSearch.Text, txtComponentNameSearch.Text,
                null, null, null, null, null,
                null,
                null, null, sortexpression, sortdirection,
                gvComponent.CurrentPageIndex + 1,
                gvComponent.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveCheckedComponentValues(Object sender, EventArgs e)
    {
        try
        {
            RadCheckBox obj = sender as RadCheckBox;
            GridDataItem gvr = (GridDataItem)obj.Parent.Parent;

            string componentid = ((RadLabel)gvr.FindControl("lblComponentId")).Text;

            if (obj.Checked == true)
            {
                PhoenixInventorySpareItemComponent.InsertSpareItemComponent
                     (
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , new Guid(componentid)
                          , new Guid(ViewState["SPAREITEMID"].ToString())
                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                     );
            }
            else
            {
                PhoenixInventorySpareItemComponent.DeleteSpareItemComponentMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SPAREITEMID"].ToString())
                                                                                , new Guid(componentid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            var Frame = string.IsNullOrEmpty(Request.QueryString["ifr"]) ? "null" : "'ifMoreInfo'";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1'," + Frame + ", true);", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvComponent_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
