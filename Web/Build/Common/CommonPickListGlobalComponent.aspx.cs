using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class CommonPickListGlobalComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                ViewState["PAGENUMBER"] = 1;
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

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixPlannedMaintenanceGlobalComponent.ComponentSearch(General.GetNullableString(txtNumberSearch.Text),
                                                           General.GetNullableString(txtComponentNameSearch.Text),
                                                           sortexpression, sortdirection,
                                                           gvComponent.CurrentPageIndex + 1,
                                                           gvComponent.PageSize, ref iRowCount, ref iTotalPageCount);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblComponentNumber");
                nvc.Add(lblComponentNumber.ID, lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)e.Item.FindControl("lnkComponentName");
                nvc.Add(lbComponentName.ID, lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
                nvc.Add(lblComponentId.ID, lblComponentId.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblComponentNumber");
                nvc.Set(nvc.GetKey(1), lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)e.Item.FindControl("lnkComponentName");
                nvc.Set(nvc.GetKey(2), lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
                nvc.Set(nvc.GetKey(3), lblComponentId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponent.CurrentPageIndex + 1;
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