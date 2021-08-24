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
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CommonPickListUserName : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);

        MenuUserNameList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvusername.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void Rebind()
    {
        gvusername.SelectedIndexes.Clear();
        gvusername.EditIndexes.Clear();
        gvusername.DataSource = null;
        gvusername.Rebind();
    }

    protected void MenuUserNameList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtUserName.Text = "";
                Rebind();
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
        int iTotalPageCount = 10;

        DataSet ds;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 0;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixUser.PickUserList(
            null, 172, 2
            , txtUserName.Text.Trim()
            , sortexpression
            , sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvusername.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            );

        gvusername.DataSource = ds;
        gvusername.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvusername_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string Script = "";
                NameValueCollection nvc;
                if (Request.QueryString["mode"] == "custom")
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = new NameValueCollection();

                    LinkButton lbl = (LinkButton)e.Item.FindControl("lnkusercode");
                    nvc.Add(lbl.ID, lbl.Text);
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkUserName");
                    nvc.Add(lb.ID, lb.Text.ToString());
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = Filter.CurrentPickListSelection;
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkusercode");
                    nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                    LinkButton lb2 = (LinkButton)e.Item.FindControl("lnkUserName");
                    nvc.Set(nvc.GetKey(2), lb2.Text);

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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvusername_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvusername.CurrentPageIndex + 1;
        BindData();
    }
}
