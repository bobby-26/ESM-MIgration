using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CommonPickListVesselAndAccountCombined : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAccount.MenuList = toolbarmain.Show();
        //MenuAccount.SetTrigger(pnlAccount);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        BindData();
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
                BindData();
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


            ds = PhoenixRegistersAccount.VesselAndMappedAccountSearch(
                txtAccountCodeSearch.Text.Trim(), txtAccountDescSearch.Text.Trim()
                , txtVesselName.Text
                , sortexpression, sortdirection,
                gvAccount.CurrentPageIndex + 1,
                gvAccount.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    //gvAccount.DataBind();
            ////}
            ////else
            ////{
            ////    DataTable dt = ds.Tables[0];
            ////    ShowNoRecordsFound(dt, gvAccount);
            ////}

            //ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            gvAccount.DataSource = ds;
            gvAccount.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //if (e.CommandName.ToUpper().Equals("SORT"))
            //    return;
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;
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
                    RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                    nvc.Set(lblVesselId.ID, lblVesselId.Text.ToString());
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
                    RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                    nvc.Set(nvc.GetKey(4), lblVesselId.Text.ToString());
                }

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }

            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                gvAccount.Rebind();
            }
        }
    }

    protected void gvAccount_RowEditing(object sender, GridViewEditEventArgs e)
    {
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

    protected void gvAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        if (e.Item is GridHeaderItem)
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

    protected void gvAccount_RowDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        if (e.Item is GridEditableItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
            }
        }
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
