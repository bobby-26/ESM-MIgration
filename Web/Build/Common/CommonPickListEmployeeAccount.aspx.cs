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
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CommonPickListEmployeeAccount : PhoenixBasePage
{
    public DataSet dsaccount = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuBudget.MenuList = toolbarmain.Show();
     //   MenuBudget.SetTrigger(pnlBudget);
        if (!IsPostBack)
        {
            if (Request.QueryString["SelectedAccountId"] != null && Request.QueryString["SelectedAccountId"].ToString().Length > 0)
            {
                ViewState["SelectedAccountId"] = Request.QueryString["SelectedAccountId"].ToString();
                dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(ViewState["SelectedAccountId"].ToString()));
            }
            else
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;
                DataSet ds = PhoenixRegistersAccount.AccountFunctionMapSearch(
                                                   12
                                                 , null
                                                 , null, null
                                                 , 1, General.ShowRecords(null)
                                                 , ref iRowCount, ref iTotalPageCount
                                                 , 1);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewState["SelectedAccountId"] = ds.Tables[0].Rows[0]["FLDACCOUNTID"];
                dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(ViewState["SelectedAccountId"].ToString()));
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (dsaccount != null)
            {
                if (dsaccount.Tables[0].Rows.Count > 0)
                {
                    DataRow draccount = dsaccount.Tables[0].Rows[0];
                    ViewState["ACCOUNTUSAGE"] = draccount["FLDACCOUNTUSAGE"].ToString();
                }
            }
            gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
       // BindData();
    }

    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvBudget.Rebind();


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;
            string selectedaccid = null;
            string accusage = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["SelectedAccountId"] != null)
                selectedaccid = ViewState["SelectedAccountId"].ToString();
            if (ViewState["ACCOUNTUSAGE"] != null)
                accusage = ViewState["ACCOUNTUSAGE"].ToString();
            ds = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                  , General.GetNullableInteger(selectedaccid)
                  , General.GetNullableInteger(accusage)
                  , General.GetNullableString(txtSubAccountCodeSearch.Text)  //txtDescriptionNameSearch.Text
                  , General.GetNullableString(txtDescriptionSearch.Text)
                  , sortexpression, sortdirection,
                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvBudget.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount
                  );

            gvBudget.DataSource = ds;
            gvBudget.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    gvBudget.DataSource = ds;
            //    gvBudget.DataBind();
            //}
            //else
            //{
            //    ShowNoRecordsFound(ds.Tables[0], gvBudget);
            //}

           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    //protected void gvBudget_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    string Script = "";
    //    NameValueCollection nvc;
    //    nvc = Filter.CurrentPickListSelection;


    //    if (Request.QueryString["mode"] == "custom")
    //    {
    //        Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDescription");
    //        nvc.Set(nvc.GetKey(1), lbl.Text.ToString());
    //        LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkBudget");
    //        nvc.Set(nvc.GetKey(2), lb.Text);
    //        Label lblBudgetCodeId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetCodeId");
    //        nvc.Set(nvc.GetKey(3), lblBudgetCodeId.Text);
    //        Label lblBudgetGroupId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetGroupId");
    //        nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);
    //    }
    //    else
    //    {
    //        Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDescription");
    //        nvc.Set(nvc.GetKey(1), lbl.Text.ToString());

    //        LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkBudget");
    //        nvc.Set(nvc.GetKey(2), lb.Text);
    //        Label lblBudgetId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetCodeId");
    //        nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
    //        Label lblBudgetGroupId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetGroupId");
    //        nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);
    //    }

    //    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
    //    Script += "</script>" + "\n";

    //    Filter.CurrentPickListSelection = nvc;
    //    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    //}

    protected void CloseWindow(object sender, EventArgs e)
    {
        if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        {
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }


    //protected void gvBudget_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
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



                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                    nvc.Set(nvc.GetKey(1), lbl.Text.ToString());
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                    nvc.Set(nvc.GetKey(2), lb.Text);
                    RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");
                    nvc.Set(nvc.GetKey(3), lblBudgetCodeId.Text);
                    RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                    nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    nvc = Filter.CurrentPickListSelection;
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                    nvc.Set(nvc.GetKey(1), lbl.Text.ToString());

                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                    nvc.Set(nvc.GetKey(2), lb.Text);
                    RadLabel lblBudgetId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");
                    nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                    RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                    nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);
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

    protected void gvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudget.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudget_SortCommand(object sender, GridSortCommandEventArgs e)
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

  
    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;

    //    ViewState["SORTEXPRESSION"] = ib.CommandName;
    //    ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //    BindData();
    //    SetPageNavigator();
    //}
}
