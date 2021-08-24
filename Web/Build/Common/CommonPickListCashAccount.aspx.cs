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

public partial class CommonPickListCashAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuAccount.MenuList = toolbarmain.Show();
     //   MenuAccount.SetTrigger(pnlAccount);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;


            gvAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
       // BindData();
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
    protected void MenuAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
               // BindData();
                Rebind();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAccount.SelectedIndexes.Clear();
        gvAccount.EditIndexes.Clear();
        gvAccount.DataSource = null;
        gvAccount.Rebind();
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


            ds = PhoenixRegistersAccount.CashAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,txtAccountCodeSearch.Text.Trim(), txtAccountDescSearch.Text.Trim() 
                , sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAccount.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvAccount.DataSource = ds;
            gvAccount.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    gvAccount.DataSource = ds;
            //    gvAccount.DataBind();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvAccount);
            //}

            //ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void gvAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

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

                    LinkButton lblAccountCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                    nvc.Add(lblAccountCode.ID, lblAccountCode.Text.ToString());
                    RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                    nvc.Add(lnkAccountDescription.ID, lnkAccountDescription.Text.ToString());
                    RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                    nvc.Add(lblAccountId.ID, lblAccountId.Text);
                }
                else
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = Filter.CurrentPickListSelection;
                    LinkButton lblAccountCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                    nvc.Set(nvc.GetKey(1), lblAccountCode.Text.ToString());
                    RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                    nvc.Set(nvc.GetKey(2), lnkAccountDescription.Text.ToString());
                    RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                    nvc.Set(nvc.GetKey(3), lblAccountId.Text.ToString());
                }

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
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
    //        Script += "fnReloadList('codehelp1','ifMoreInfo');";
    //        Script += "</script>" + "\n";

    //        nvc = new NameValueCollection();

    //        LinkButton lblAccountCode = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAccountCode");
    //        nvc.Add(lblAccountCode.ID, lblAccountCode.Text.ToString());
    //        Label lnkAccountDescription = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountDescription");
    //        nvc.Add(lnkAccountDescription.ID, lnkAccountDescription.Text.ToString());
    //        Label lblAccountId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId");
    //        nvc.Add(lblAccountId.ID, lblAccountId.Text);
    //    }
    //    else
    //    {

    //        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
    //        Script += "</script>" + "\n";

    //        nvc = Filter.CurrentPickListSelection;
    //        LinkButton lblAccountCode = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAccountCode");
    //        nvc.Set(nvc.GetKey(1), lblAccountCode.Text.ToString());
    //        Label lnkAccountDescription = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountDescription");
    //        nvc.Set(nvc.GetKey(2), lnkAccountDescription.Text.ToString());
    //        Label lblAccountId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId");
    //        nvc.Set(nvc.GetKey(3), lblAccountId.Text.ToString());
    //    }

    //    Filter.CurrentPickListSelection = nvc;
    //    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    //}

  

    //protected void gvAccount_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}

    //protected void gvAccount_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    //protected void gvAccount_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //        }
    //    }
    //}

  
    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;

    //    ViewState["SORTEXPRESSION"] = ib.CommandName;
    //    ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    //    BindData();
       
    //}
}
