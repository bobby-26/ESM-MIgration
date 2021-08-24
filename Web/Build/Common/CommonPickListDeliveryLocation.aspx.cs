using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common ;
using Telerik.Web.UI;
public partial class CommonPickListDeliveryLocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        
        MenuSeaportList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            rgvSeaport.PageSize = General.ShowRecords(null);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
     
    }
    

    protected void SeaportList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            rgvSeaport.Rebind();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtSeaportCodeSearch.Text = "";
            txtSeaportNameSearch.Text = "";
            rgvSeaport.Rebind();
        }
    }
    protected void rgvSeaport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 10;

        DataSet ds;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 0;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonRegisters.SeaportSearch(
            1
            , txtSeaportCodeSearch.Text
            , txtSeaportNameSearch.Text
            , null
            , null
            , sortexpression, sortdirection,
            Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        rgvSeaport.DataSource = ds;
        rgvSeaport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void rgvSeaport_PreRender(object sender, EventArgs e)
    {
    }
    protected void rgvSeaport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            if (Request.QueryString["mode"] == "custom")
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lbl = (RadLabel)item["CODE"].FindControl("lblID");
                nvc.Add(lbl.ID, item.GetDataKeyValue("FLDSEAPORTID").ToString());
                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkSeaportCode");
                nvc.Add(lb.ID, lb.Text.ToString());
            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;
                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkSeaportCode");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lb2 = (RadLabel)item["NAME"].FindControl("lblSeaportName");
                nvc.Set(nvc.GetKey(2), lb2.Text);
                
                nvc.Set(nvc.GetKey(3), item.GetDataKeyValue("FLDSEAPORTID").ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }


    }

}
