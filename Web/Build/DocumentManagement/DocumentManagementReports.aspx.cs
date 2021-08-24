using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System.Web;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementReports : System.Web.UI.Page
{
    //int num = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH");
            //toolbarmain.AddButton("Close", "CLOSE");

            //toolbarmain.AddButton("Reports", "REPORTS", ToolBarDirection.Right);
            //toolbarmain.AddButton("Forms", "FORMS", ToolBarDirection.Right);
            //toolbarmain.AddButton("Back", "BACK");
            MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();
            MenuStoreItemInOutTransaction.SelectedMenuIndex = 0;

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DocumentManagement/DocumentManagementReports.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../DocumentManagement/DocumentManagementReports.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbargrid.AddFontAwesomeButton("../StandardForm/StandardFormFBFormReports.aspx", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
            MenuGridItem.MenuList = toolbargrid.Show();
            //toolbargrid.AddImageButton("../StandardForm/StandardFormFBformsList.aspx", "Add", "add.png", "ADD");
            MenuGridItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {


               // ViewState["CategoryId"] = "";
               // BindTreeData();
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
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void BindTreeData()
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();

    //        ds = PhoenixFormBuilder.CategoryList(0);

    //        tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
    //        tvwCategory.DataValueField = "FLDFORMCATEGORYID";
    //        tvwCategory.DataFieldParentID = "FLDPARENTCATEGORYID";

    //        tvwCategory.RootText = "Category";
    //        tvwCategory.PopulateTree(ds.Tables[0]);

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvFormList.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void BindTreeData()
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();

    //        ds = PhoenixFormBuilder.CategoryList(0);

    //        tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
    //        tvwCategory.DataValueField = "FLDFORMCATEGORYID";
    //        tvwCategory.ParentNodeField = "FLDPARENTCATEGORYID";
    //        tvwCategory.XPathField = "xpath";
    //        tvwCategory.RootText = "Category";
    //        tvwCategory.PopulateTree(ds);
    //        TreeView tvw = tvwCategory.ThisTreeView;
    //        ((TreeNode)tvw.Nodes[0]).Expand();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREPORTNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Name", "Created On" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixFormBuilder.DmsReportSearch(null, txtFormName.Text, sortexpression
                                                        , sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, txtcontent.Text, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            //DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
            //                                        , 1, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                                        , General.ShowRecords(null)
            //                                        , ref iRowCount
            //                                        , ref iTotalPageCount, 0);



            General.ShowExcel("Report List", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormList_UpdateCommand(object sender, GridCommandEventArgs e)
    {

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
            if (CommandName.ToUpper().Equals("FORMS"))
            {
                Response.Redirect("../StandardForm/StandardFormFBReportList.aspx");
            }
            if (CommandName.ToUpper().Equals("REPORTS"))
            {
                Response.Redirect("../StandardForm/StandardFormFBFormReports.aspx");
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            //if (dce.CommandName.ToUpper().Equals("BACK"))
            //{
            //    Response.Redirect("../StandardForm/StandardFormFBReportList.aspx");
            //}
            //if (CommandName.ToUpper().Equals("REFRESH"))
            //{
            //    Response.Redirect("../StandardForm/StandardFormFBFormReports.aspx");
            //}
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
            string[] alColumns = { "FLDREPORTNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Name", "Created On" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //DataSet ds = PhoenixFormBuilder.DmsReportSearch(null, txtFormName.Text, sortexpression
            //                                          , sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                                          , General.ShowRecords(null)
            //                                          , ref iRowCount
            //                                          , ref iTotalPageCount);

            DataSet ds = PhoenixFormBuilder.DmsReportSearch(null, txtFormName.Text, sortexpression
                                                        , sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvFormList.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, txtcontent.Text, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            //DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
            //                                            , 1, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                                            , General.ShowRecords(null)
            //                                            , ref iRowCount
            //                                            , ref iTotalPageCount, 1);



            General.SetPrintOptions("gvFormList", "Form List", alCaptions, alColumns, ds);
            gvFormList.DataSource = ds;
            gvFormList.VirtualItemCount = iRowCount;





            ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvFormList_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        gvFormList.Rebind();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvFormList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OnDeleteCommand(object sender, GridCommandEventArgs de)
    {
        try
        {

            string reportId = ((Label)de.Item.FindControl("lblReportId")).Text;
            PhoenixFormBuilder.ReportDelete(new Guid(reportId));

            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //	&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //	&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //	e.Row.TabIndex = -1;
        //	e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFormList, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}
        //SetKeyDownScroll(sender, e);
    }

    //protected void gvFormList_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (_gridView.EditIndex > -1)
    //            _gridView.UpdateRow(_gridView.EditIndex, false);

    //        _gridView.EditIndex = de.NewEditIndex;SetPageNavigato
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

    protected void gvFormList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;
                string reportId = ((Label)e.Item.FindControl("lblReportId")).Text;
                string formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                string statusname = ((Label)e.Item.FindControl("lblStatusname")).Text;

                LinkButton lnkReportName = (LinkButton)e.Item.FindControl("lnkReportName");
                if (lnkReportName != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkReportName.CommandName))
                    {
                        lnkReportName.Visible = false;
                    }

                    lnkReportName.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp1','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?ReportId=" + reportId + "&FORMTYPE=DMSForm'); return false;");

                }

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                    if (lblIsAtt.Text == string.Empty)
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }

                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.DOCUMENTMANAGEMENT + "'); return false;");
                }


                LinkButton cmddelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmddelete != null)
                {
                    cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
                }
                LinkButton ce = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ce != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ce.CommandName)) ce.Visible = false;

                }

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

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

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
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
}