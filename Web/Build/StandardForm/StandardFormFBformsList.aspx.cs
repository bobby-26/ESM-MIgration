using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class StandardFormFBformsList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {
    //        foreach (GridViewRow r in gvFormList.Rows)
    //        {
    //            if (r.RowType == DataControlRowType.DataRow)
    //            {
    //                Page.ClientScript.RegisterForEventValidation(gvFormList.UniqueID, "Edit$" + r.RowIndex.ToString());
    //            }
    //        }
    //        //if (!IsPostBack && ViewState["COMPONENTID"] != null)
    //        //{
    //        //    tvwComponent.FindNodeByValue(tvwComponent.ThisTreeView.Nodes, ViewState["COMPONENTID"].ToString());
    //        //}
    //        base.Render(writer);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Forms", "FORMS");
            //toolbarmain.AddButton("Reports", "REPORTS");
            //toolbarmain.AddButton("Search", "SEARCH");
            //toolbarmain.AddButton("Close", "CLOSE");
            //MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            //MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();
            //string s= DateTime.Parse("2019-07-11T11:59:00 05:30").ToString();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../StandardForm/StandardFormFBformsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../StandardForm/StandardFormFBformsList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbargrid.AddImageButton("../StandardForm/StandardFormFBformsList.aspx", "Add", "add.png", "ADD");
            MenuGridItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["cat"]))
                    ViewState["CategoryId"] = Request.QueryString["cat"].ToString();
                else
                    ViewState["CategoryId"] = "";

                BindTreeData();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["FIRSTINITIALIZED"] = true;
                //if (Request.QueryString["COMPONENTID"] != null)
                //    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                //else
                //    ViewState["COMPONENTID"] = "";
                gvFormList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTreeData()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixFormBuilder.CategoryList(0);

            tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
            tvwCategory.DataValueField = "FLDFORMCATEGORYID";
            tvwCategory.DataFieldParentID = "FLDPARENTCATEGORYID";
            
            //tvwCategory.XPathField = "xpath";
            //tvwCategory.PopulateTree(ds);
            //TreeView tvw = tvwCategory.ThisTreeView;
            //((TreeNode)tvw.Nodes[0]).Expand();
            tvwCategory.SelectNodeByValue = ViewState["CategoryId"].ToString();
           
            tvwCategory.RootText = "Category";
            //tvwCategory.PopulateTree(ds);
            //TreeView tvw = tvwCategory.ThisTreeView;
            //((TreeNode)tvw.Nodes[0]).Expand();
            tvwCategory.PopulateTree(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        //try
        //{
        //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        //    if (dce.CommandName.ToUpper().Equals("CLOSE"))
        //    {
        //        string Script = "";
        //        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        //        Script += " fnReloadList();";
        //        Script += "</script>" + "\n";
        //        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        //    }

        //    if (dce.CommandName.ToUpper().Equals("SEARCH"))
        //    {
        //        ViewState["PAGENUMBER"] = 1;
        //        BindData();
        //        SetPageNavigator();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDFORMNAME", "FLDSTATUSNAME", "FLDPUBLISHEDDATE", "FLDACTIVEYESNO" };
            string[] alCaptions = { "Name", "Status", "Published", "Active(Yes/No)" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
                                                        , int.Parse(ddlActive.SelectedValue), sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, 0);

            General.ShowExcel("Form List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGridStoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvFormList.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
            int iRowCount = 10;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDFORMNAME", "FLDSTATUSNAME", "FLDPUBLISHEDDATE", "FLDACTIVEYESNO" };
            string[] alCaptions = { "Name", "Status", "Published", "Active(Yes/No)" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
                                                        , int.Parse(ddlActive.SelectedValue), sortexpression, sortdirection,
                                                        int.Parse(ViewState["PAGENUMBER"].ToString()), gvFormList.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, 0);


            General.SetPrintOptions("gvFormList", "Form List", alCaptions, alColumns, ds);


            //if (ds.Tables[0].Rows.Count > 0)

            gvFormList.DataSource = ds;
            gvFormList.VirtualItemCount = iRowCount;



            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridFooterItem footerItem = (GridFooterItem)gvFormList.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixFormBuilder.FormDelete(new Guid(((Label)e.Item.FindControl("lblFormId")).Text));

                BindData();
                gvFormList.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertForm(
                        ((TextBox)footerItem.FindControl("txtFormNameAdd")).Text);
                BindData();
                gvFormList.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CREATE"))
            {
                string formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                string formType = ((Label)e.Item.FindControl("lblFormType")).Text;
                Response.Redirect("StandardFormFBFormCreate.aspx?FormId=" + formId + "&FormType=" + formType + "&cat=" + ViewState["CategoryId"]);
            }
            if (e.CommandName.ToUpper().Equals("REVISION"))
            {
                String formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                PhoenixFormBuilder.FormCopy(new Guid(formId));
                BindData();
                gvFormList.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("PUBLISH"))
            {
                String formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                PhoenixFormBuilder.FormPublish(new Guid(formId));
                BindData();
                gvFormList.Rebind();
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

    protected void gvFormList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;
                string frmEditable = ((Label)e.Item.FindControl("lblEditable")).Text;
                string statusId = ((Label)e.Item.FindControl("lblStatusId")).Text;
                string formId = ((Label)e.Item.FindControl("lblFormId")).Text;

                //Label lnkformName = (Label)e.Row.FindControl("lnkFormName");
                //if (lnkformName != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, lnkformName.CommandName))
                //    {
                //        lnkformName.Visible = false;
                //    }

                //    lnkformName.Attributes.Add("onclick", "javascript:return Openpopup('codehelp1','','../StandardForm/StandardFormFBformView.aspx?FormId=" + formId + "&FormName=" + lnkformName.Text + "'); return false;");

                //}

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Attributes.Add("style", "display:none");
                }

                db = (LinkButton)e.Item.FindControl("cmdEdit");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Attributes.Add("style", "display:none");

                }
                db = (LinkButton)e.Item.FindControl("cmdPublish");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName) || statusId == "2")
                        db.Attributes.Add("style", "display:none");
                }

                db = (LinkButton)e.Item.FindControl("cmdCreate");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName) || frmEditable == "0")
                        db.Attributes.Add("style", "display:none");

                }
                db = (LinkButton)e.Item.FindControl("cmdRevision");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName) || statusId == "1")
                        db.Attributes.Add("style", "display:none");
                }

                db = (LinkButton)e.Item.FindControl("cmdMove");
                if (db != null)
                {
                    Label lblformId = (Label)e.Item.FindControl("lblFormId");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Attributes.Add("style", "display:none");
                    else
                        db.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] +"/StandardForm/StandardFormFbCategoryChange.aspx?formId=" + lblformId.Text + "')");


                }
                db = (LinkButton)e.Item.FindControl("cmdDistribute");
                if (db != null)
                {
                    Label lblformId = (Label)e.Item.FindControl("lblFormId");
                    Label lnkFormName = (Label)e.Item.FindControl("lnkFormName");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName) || statusId == "1")
                         db.Attributes.Add("style", "display:none");
                        //lnkFormName.Visible = false;
                    else
                        db.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/StandardForm/StandardFormDistributeVesselList.aspx?FormId=" + lblformId.Text + "&FormName=+" + lnkFormName.Text + "')");

                }


            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormList.CurrentPageIndex + 1;
        BindData();
    }


    protected void gvFormList_SortCommand(object sender, GridSortCommandEventArgs e)
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
    //    try
    //    {
    //        ImageButton ib = (ImageButton)sender;
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //        gvFormList.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    protected void gvFormList_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            int activeyn = ((CheckBox)e.Item.FindControl("chkEditActive")).Checked == true ? 1 : 0;
            string sjgdf = ((Label)e.Item.FindControl("lblFormId")).Text;
            formUpdate(
                   ((Label)e.Item.FindControl("lblFormId")).Text,
                   ((TextBox)e.Item.FindControl("txtFormNameEdit")).Text,
                   activeyn,
                   ((UserControlDate)e.Item.FindControl("ucPublishedDateEdit")).Text
                );

            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvFormList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        int activeyn = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkEditActive")).Checked == true ? 1 : 0;
    //        string sjgdf = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFormId")).Text;
    //        formUpdate(
    //               ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFormId")).Text,
    //               ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFormNameEdit")).Text,
    //               activeyn,
    //               ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPublishedDateEdit")).Text
    //            );
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        gvFormList.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvFormList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixFormBuilder.FormDelete(new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblFormId")).Text));
    //            _gridView.EditIndex = -1;
    //            BindData();
    //            gvFormList.Rebind();
    //        }
    //        if(e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            InsertForm(
    //                    ((TextBox)_gridView.FooterRow.FindControl("txtFormNameAdd")).Text);
    //            BindData();
    //            gvFormList.Rebind();
    //        }
    //        if (e.CommandName.ToUpper().Equals("CREATE"))
    //        {
    //            string formId = ((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblFormId")).Text;
    //            string formType = ((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblFormType")).Text;
    //            Response.Redirect("StandardFormFBFormCreate.aspx?FormId=" + formId + "&FormType=" + formType + "&cat=" + ViewState["CategoryId"]);
    //        }
    //        if (e.CommandName.ToUpper().Equals("REVISION"))
    //        {
    //            String formId = ((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblFormId")).Text;
    //            PhoenixFormBuilder.FormCopy(new Guid(formId));
    //            BindData();
    //            gvFormList.Rebind();
    //        }
    //        if (e.CommandName.ToUpper().Equals("PUBLISH"))
    //        {
    //            String formId = ((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblFormId")).Text;
    //            PhoenixFormBuilder.FormPublish(new Guid(formId));
    //            BindData();
    //            gvFormList.Rebind();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidLineItemEdit(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvFormList;
        decimal result;

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Item requested quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    //protected void gvFormList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void gvFormList_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (_gridView.EditIndex > -1)
    //            _gridView.UpdateRow(_gridView.EditIndex, false);

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;

    //        BindData();
    //        gvFormList.Rebind();
    //        //((CheckBox)_gridView.Rows[de.NewEditIndex].FindControl("chkEditActive")).();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BindData();
    //        gvFormList.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvFormList.SelectedIndex = -1;
    //        gvFormList.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}







    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }
    //}

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        try
        {
            RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;

            if (tvsne.Node.Value != null)
            {
                string selectednode = tvsne.Node.Value.ToString();
                string selectedvalue = tvsne.Node.Text.ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CategoryId"] = selectednode;
            }
            //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void formUpdate(string formId, string frmname, int activeYn, string publishDate)
    {
        try
        {
            PhoenixFormBuilder.FormUpdate(new Guid(formId), frmname, activeYn, General.GetNullableDateTime(publishDate));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGridItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvFormList.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!string.IsNullOrEmpty(ViewState["CategoryId"].ToString()))
                {
                    Response.Redirect("StandardFormFBFormCreate.aspx?CategoryId=" + ViewState["CategoryId"]);
                }
                else
                {
                    ucError.ErrorMessage = "Please select category";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertForm(string fromName)
    {
        if (!IsValidForms(fromName))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixFormBuilder.FormCreate(int.Parse(ViewState["CategoryId"].ToString()), fromName, null, 1);
    }
    private bool IsValidForms(string frmName)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (frmName.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if (string.IsNullOrEmpty(ViewState["CategoryId"].ToString()))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }

    protected void ddlActive_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvFormList.Rebind();
    }

    //protected void gvFormList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvFormList.SelectedIndex = e.NewSelectedIndex;

    //    ViewState["COMPONENTID"] = ((Label)gvFormList.Rows[e.NewSelectedIndex].FindControl("lblComponentId")).Text;
    //    //ResetMenu();
    //    BindData();
    //}
}