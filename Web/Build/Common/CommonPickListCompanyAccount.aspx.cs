using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class CommonPickListCompanyAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuAccount.MenuList = toolbarmain.Show();
        //MenuAccount.SetTrigger(pnlAccount);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ACCOUNTCODE"] = null;
            ViewState["ACCOUNTSOURCE"] =null;
            gvAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (Request.QueryString["ACCOUNTSOURCE"] != null)
            {
                ViewState["ACCOUNTSOURCE"] = Request.QueryString["ACCOUNTSOURCE"].ToString();
            }
            else
            {
                ViewState["ACCOUNTSOURCE"] = "";
            }
        }
    }

   
    protected void MenuAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Session["sAccountCode"] = txtAccountcode.Text;
                ViewState["PAGENUMBER"] = 1;
                gvAccount.CurrentPageIndex = 0;
                BindData();
                gvAccount.Rebind();
                //SetPageNavigator();
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

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string accountcode = (Session["sAccountCode"] == null) ? null : (Session["sAccountCode"].ToString());


            ds = PhoenixRegistersAccount.CompanyAccountSearch(accountcode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null
                , sortexpression, sortdirection,
                gvAccount.CurrentPageIndex+1,
                gvAccount.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(ViewState["ACCOUNTSOURCE"].ToString()));
           
    
                gvAccount.DataSource = ds;
            gvAccount.VirtualItemCount = iRowCount;
          

            ViewState["ROWCOUNT"] = iRowCount;
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

      
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("ACCOUNT"))
        {
            if (Request.QueryString["mode"] == "custom")
            {


                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();

                LinkButton lblAccountCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                nvc.Add(lblAccountCode.ID, lblAccountCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                nvc.Add(lnkAccountDescription.ID, lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                nvc.Add(lblAccountId.ID, lblAccountId.Text);

            }
            else
            {

               
                nvc = Filter.CurrentPickListSelection;

                LinkButton lblAccountCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                //string dd = lblAccountCode.Text;
                nvc.Set(nvc.GetKey(1), lblAccountCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                nvc.Set(nvc.GetKey(2), lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                nvc.Set(nvc.GetKey(3), lblAccountId.Text.ToString());

                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }
                
            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);

        }


    }
    protected void CloseWindow(object sender, EventArgs e)
    {
        string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
            Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
        else
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
        Script += "</script>" + "\n";
        RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
    }

    //protected void gvAccountRebind()
    //{
    //    gvAccount.SelectedIndexes.Clear();
    //    gvAccount.EditIndexes.Clear();
    //    gvAccount.DataSource = null;
    //    gvAccount.Rebind();
    //}
    protected void gvAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvAccount_SortCommand(object sender, GridSortCommandEventArgs e)
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

 
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAccount.CurrentPageIndex + 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
