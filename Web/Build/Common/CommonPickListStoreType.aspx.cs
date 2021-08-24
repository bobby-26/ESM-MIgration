using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CommonPickListStoreType : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
		MenuComponent.MenuList = toolbarmain.Show();

		if (!IsPostBack)
		{
            gvStockType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAGENUMBER"] = 1;
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
                gvStockType.Rebind();
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

            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixCommonInventory.StoreTypeSearch(vesselid,
                txtStoreTypeNumberSearch.Text, txtStoreTypeNameSearch.Text, null, null, null, null, null, null,
                sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvStockType.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvStockType.DataSource = ds;
            gvStockType.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
	}

    protected void gvStockType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockType.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvStockType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

                RadLabel lblStoreTypeNumber = (RadLabel)e.Item.FindControl("lblStoreTypeNumber");
                nvc.Add(lblStoreTypeNumber.ID, lblStoreTypeNumber.Text.ToString());
                LinkButton lblStoreTypeName = (LinkButton)e.Item.FindControl("lnkStoreTypeName");
                nvc.Add(lblStoreTypeName.ID, lblStoreTypeName.Text.ToString());
                RadLabel lblStoreTypeId = (RadLabel)e.Item.FindControl("lblStoreTypeId");
                nvc.Add(lblStoreTypeId.ID, lblStoreTypeId.Text);

            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblStoreTypeNumber = (RadLabel)e.Item.FindControl("lblStoreTypeNumber");
                nvc.Set(nvc.GetKey(1), lblStoreTypeNumber.Text.ToString());
                LinkButton lblStoreTypeName = (LinkButton)e.Item.FindControl("lnkStoreTypeName");
                nvc.Set(nvc.GetKey(2), lblStoreTypeName.Text.ToString());
                RadLabel lblStoreTypeId = (RadLabel)e.Item.FindControl("lblStoreTypeId");
                nvc.Set(nvc.GetKey(3), lblStoreTypeId.Text.ToString());


            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvStockType_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
