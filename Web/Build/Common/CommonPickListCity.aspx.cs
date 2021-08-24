using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CommonPickListCity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuCity.MenuList = toolbarmain.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COUNTRYID"] = null;
            gvCity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["countryid"] != null && General.GetNullableInteger(Request.QueryString["countryid"].ToString()) != null)
            {
                ViewState["COUNTRYID"] = Request.QueryString["countryid"].ToString();
                ddlcountrylist.SelectedCountry = ViewState["COUNTRYID"].ToString();
                ddlcountrylist.Enabled = false;
                bindstate();
            }
            BindFrequestCities();
        }
    }
    private void bindstate()
    {
        ucState.Country = ViewState["COUNTRYID"] == null ? ddlcountrylist.SelectedCountry : ViewState["COUNTRYID"].ToString();
        ucState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(ViewState["COUNTRYID"] == null ? ddlcountrylist.SelectedCountry : ViewState["COUNTRYID"].ToString()));
        ucState.DataBind();
    }
    protected void MenuCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCity.SelectedIndexes.Clear();
                gvCity.EditIndexes.Clear();
                gvCity.DataSource = null;
                gvCity.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFrequestCities()
    {
        DataSet ds = PhoenixRegistersCity.FrequestCityList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOriginCity.DataSource = ds.Tables[0];
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            gvDestinationCity.DataSource = ds.Tables[1];
        }
    }
    protected void gvCity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = gvCity.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCity.CitySearch(
                   txtSearch.Text,
                   General.GetNullableInteger(ViewState["COUNTRYID"] == null ? ddlcountrylist.SelectedCountry : ViewState["COUNTRYID"].ToString()),
                   General.GetNullableInteger(ucState.SelectedState),
                   sortexpression, sortdirection,
                   int.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvCity.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);

        gvCity.DataSource = ds;
        gvCity.VirtualItemCount = iRowCount;
    }
    protected void RadDropDownCountry_TextChangedEvent(object sender, EventArgs e)
    {
        ucState.Country = ddlcountrylist.SelectedCountry;

        ucState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(ucState.Country == null ? "0" : ucState.Country));
        ucState.DataBind();

        gvCity.CurrentPageIndex = 0;

        gvCity.EditIndexes.Clear();
        gvCity.SelectedIndexes.Clear();

        gvCity.Rebind();
    }

    protected void RadDropDownState_TextChangedEvent(object sender, EventArgs e)
    {
        gvCity.EditIndexes.Clear();
        gvCity.SelectedIndexes.Clear();

        gvCity.CurrentPageIndex = 0;

        gvCity.Rebind();
    }
    protected void gvCity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PICKCITY"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCityName");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCityid");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {
                nvc = Filter.CurrentPickListSelection;
                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCityName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCityid");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            gvCity.Rebind();
        }
    }

    protected void gvCity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
    }
    protected void gvOriginCity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.ItemIndex;

        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnReloadList('codehelp1');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkorgCityName");
            nvc.Add(lb.ID, lb.Text.ToString());
            RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblorgCityid");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {
            nvc = Filter.CurrentPickListSelection;

            LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkorgCityName");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblorgCityid");
            nvc.Set(nvc.GetKey(2), lbl.Text);

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1');";
            Script += "</script>" + "\n";
        }

        Filter.CurrentPickListSelection = nvc;
        RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
    }
    protected void gvDestinationCity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.ItemIndex;

        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnReloadList('codehelp1');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkdestCityName");
            nvc.Add(lb.ID, lb.Text.ToString());
            RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lbldestCityid");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {
            nvc = Filter.CurrentPickListSelection;

            LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkdestCityName");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lbldestCityid");
            nvc.Set(nvc.GetKey(2), lbl.Text);

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1');";
            Script += "</script>" + "\n";
        }

        Filter.CurrentPickListSelection = nvc;
        RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
    }

    protected void gvCity_SortCommand(object sender, GridSortCommandEventArgs e)
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
