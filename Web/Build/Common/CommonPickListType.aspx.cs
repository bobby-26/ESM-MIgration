using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CommonPickListType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            gvStockClass.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds;
            string hardtypecode = "94";
            if (Request.QueryString["stocktypecode"] != null)
                hardtypecode = Request.QueryString["stocktypecode"].ToString();
      
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCommonRegisters.HardSearch(
              1
              , hardtypecode, null, null
              , sortexpression, sortdirection,
              int.Parse(ViewState["PAGENUMBER"].ToString()),
              gvStockClass.PageSize,
              ref iRowCount,
              ref iTotalPageCount);

            gvStockClass.DataSource = ds;

            gvStockClass.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStockClass_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockClass.CurrentPageIndex + 1;
        BindData();
    }
    protected void gvStockClass_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                if (ViewState["framename"] != null)
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockClassId");
                nvc.Add(lbl.ID, lbl.Text);
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockClass");
                nvc.Add(lb.ID, lb.Text.ToString());
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblShortNameName");
                nvc.Set(nvc.GetKey(1), lbl.Text);
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockClass");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());
                RadLabel lb2 = (RadLabel)e.Item.FindControl("lblStockClassId");
                nvc.Set(nvc.GetKey(3), lb2.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvStockClass_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void gvStockClass_SortCommand(object sender, GridSortCommandEventArgs e)
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
