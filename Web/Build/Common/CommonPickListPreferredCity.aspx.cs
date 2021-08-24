using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Common_CommonPickListPreferredCity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        Menucity.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvCity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }    
    }

    protected void Menucity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCity.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCity.CitySearch(
                   txtSearch.Text,
                   General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                   General.GetNullableInteger(ucState.SelectedState),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvCity.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCity.DataSource = ds;
            gvCity.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCity.DataSource = "";
        }
    }
    protected void ddlCountry_Changed(object sender, EventArgs e)
    {
        ucState.Country = ddlcountrylist.SelectedCountry;
        ucState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(ddlcountrylist.SelectedCountry));        
        BindData();
        gvCity.Rebind();
    }

    protected void ucState_Changed(object sender, EventArgs e)
    {
        BindData();
        gvCity.Rebind();
    }

    //protected void gvCity_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    string Script = "";
    //    NameValueCollection nvc;

    //    if (Request.QueryString["mode"] == "custom")
    //    {
    //        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //        if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
    //            Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
    //        else
    //            Script += "fnReloadList('codehelp1','ifMoreInfo');";
    //        Script += "</script>" + "\n";

    //        nvc = new NameValueCollection();

    //        LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCityName");
    //        nvc.Add(lb.ID, lb.Text.ToString());
    //        Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCityid");
    //        nvc.Add(lbl.ID, lbl.Text);
    //    }
    //    else
    //    {

    //        nvc = Filter.CurrentPickListSelection;

    //        LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCityName");
    //        nvc.Set(nvc.GetKey(1), lb.Text.ToString());
    //        Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCityid");
    //        nvc.Set(nvc.GetKey(2), lbl.Text);


    //        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //        if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
    //            Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
    //        else
    //            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
    //        Script += "</script>" + "\n";
    //    }

    //    Filter.CurrentPickListSelection = nvc;
    //    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    //}

    //protected void gvCity_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //}
    
    protected void gvCity_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
   
    protected void gvCity_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
     
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCityName");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCityid");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCityName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCityid");
                nvc.Set(nvc.GetKey(2), lbl.Text);


                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
           // Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCity.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvCity_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
