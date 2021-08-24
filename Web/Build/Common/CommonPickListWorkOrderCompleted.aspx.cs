using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPickListWorkOrderCompleted : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuOrder.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvOrder.Rebind();
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


            ds = PhoenixCommonPlannedMaintenance.WorkOrderListSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , General.GetNullableString(txtWorkOrderNumber.Text)
                                                                    , General.GetNullableString(txtTitle.Text)
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , sortexpression, sortdirection
                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvOrder.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            gvOrder.DataSource = ds;
            gvOrder.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrder_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

                RadLabel lblOrderNumber = (RadLabel)e.Item.FindControl("lblWorkOrderNumber");
                nvc.Add(lblOrderNumber.ID, lblOrderNumber.Text.ToString());
                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                nvc.Add(lblTitle.ID, lblTitle.Text.ToString());
                RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");
                nvc.Add(lblOrderId.ID, lblOrderId.Text);
                RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblComponentNumber");
                nvc.Add(lblComponentNumber.ID, lblComponentNumber.Text.ToString());
                RadLabel lblComponent = (RadLabel)e.Item.FindControl("lblComponent");
                nvc.Add(lblComponent.ID, lblComponent.Text.ToString());
                RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
                nvc.Add(lblComponentId.ID, lblComponentId.Text);

            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblOrderNumber = (RadLabel)e.Item.FindControl("lblWorkOrderNumber");
                nvc.Set(nvc.GetKey(1), lblOrderNumber.Text.ToString());
                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                nvc.Set(nvc.GetKey(2), lblTitle.Text.ToString());
                RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");
                nvc.Set(nvc.GetKey(3), lblOrderId.Text.ToString());


            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrder.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOrder_SortCommand(object sender, GridSortCommandEventArgs e)
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