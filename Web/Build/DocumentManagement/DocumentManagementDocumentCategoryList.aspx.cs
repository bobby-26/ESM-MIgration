using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;

public partial class DocumentManagementDocumentCategoryList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDocumentCategory.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDocumentCategory.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentCategoryList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentCategory')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentManagementDocumentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Document", "DOCUMENT");
                toolbar.AddButton("Section", "SECTION");
                //toolbar.AddButton("Content", "CONTENT");                

                //MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();
                //MenuDocument.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CATEGORYID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDAUTHORNAME", "FLDDATE"};
        string[] alCaptions = { "Name", "Active Y/N", "Added By", "Added Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        
        ds = PhoenixDocumentManagementCategory.DocumentCategorySearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Category</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvDocumentCategory.EditIndex = -1;
                gvDocumentCategory.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                ViewState["CATEGORYID"] = "";
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SECTION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionList.aspx?CATEGORYID=" + ViewState["CATEGORYID"]);
            }
            else if (dce.CommandName.ToUpper().Equals("CONTENT"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?CATEGORYID=" + ViewState["CATEGORYID"]);
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

        string[] alColumns = { "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDAUTHORNAME", "FLDDATE" };
        string[] alCaptions = { "Name", "Active Y/N", "Added By", "Added Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        
        ds = PhoenixDocumentManagementCategory.DocumentCategorySearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , null
                                                                , null
                                                                , null                                                                                                                                
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentCategory", "Document Category", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentCategory.DataSource = ds;
            gvDocumentCategory.DataBind();
            if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) == null)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                gvDocumentCategory.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDocumentCategory);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocumentCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvDocumentCategory_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDocumentCategory.EditIndex = -1;
        gvDocumentCategory.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDocumentCategory_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDocumentCategory, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvDocumentCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocument(
                    ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNameAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNumberAdd")).Text))                    
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentCategory(
                       null
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNameAdd")).Text                    
                    , (((CheckBox)_gridView.FooterRow.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNumberAdd")).Text                    
                );
                ucStatus.Text = "Category is added.";
                ViewState["CATEGORYID"] = "";
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentCategory((((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryId")).Text));
                ViewState["CATEGORYID"] = "";
                ucStatus.Text = "Category is deleted.";
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentCategory_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvDocumentCategory_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCategoryNameEdit")).Focus();
        SetPageNavigator();

    }

    protected void gvDocumentCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidDocument(
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNameEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNumberEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateDocumentCategory(
                  ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryIdEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNameEdit")).Text
                , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNumberEdit")).Text
             );

            ucStatus.Text = "Category details updated.";
            _gridView.EditIndex = -1;
            BindData();

            SetPageNavigator();

            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocumentCategory_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

            ImageButton cmdSubCategory = (ImageButton)e.Row.FindControl("cmdSubCategory");
            Label lblCategoryId = (Label)e.Row.FindControl("lblCategoryId");
            if (cmdSubCategory != null && lblCategoryId != null)
                cmdSubCategory.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentSubCategoryAdd.aspx?PARENTCATEGORYID=" + lblCategoryId.Text + "','medium'); return false;");
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }            
        }
    }

    protected void gvDocumentCategory_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvDocumentCategory.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["CATEGORYID"] = ((Label)gvDocumentCategory.Rows[rowindex].FindControl("lblCategoryId")).Text;
            //Filter.CurrentSelectedBulkOrderId = ((Label)gvDocumentCategory.Rows[rowindex].FindControl("lblOrderId")).Text;            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDocumentCategory.EditIndex = -1;
        gvDocumentCategory.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvDocumentCategory.EditIndex = -1;
        gvDocumentCategory.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDocumentCategory.SelectedIndex = -1;
        gvDocumentCategory.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void InsertDocumentCategory(string parentid, string categoryname, int? activyn, string categorynumber)
    {
        //PhoenixDocumentManagementCategory.InsertDocumentCategory(
        //      General.GetNullableGuid(parentid)
        //    , categoryname
        //    , activyn
        //    , categorynumber
        //    , null
        //    , null);
    }

    private void UpdateDocumentCategory(string categorid, string categoryname, int? activyn, string categorynumber)
    {
        //PhoenixDocumentManagementCategory.UpdateDocumentCategory(
        //      new Guid(categorid)
        //    , categoryname
        //    , activyn
        //    , categorynumber
        //    , null
        //    , null);            
    }

    private void DeleteDocumentCategory(string categorid)
    {
        PhoenixDocumentManagementCategory.DeleteDocumentCategory(new Guid(categorid));
    }

    private bool IsValidDocument(string categoryname, string categorynumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(categoryname) == null)
            ucError.ErrorMessage = "Category number is required.";

        if (General.GetNullableString(categoryname) == null)
            ucError.ErrorMessage = "Category name is required.";

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {
        gvDocumentCategory.SelectedIndex = -1;

        for (int i = 0; i < gvDocumentCategory.Rows.Count; i++)
        {
            if (gvDocumentCategory.DataKeys[i].Value.ToString().Equals(ViewState["CATEGORYID"].ToString()))
            {
                gvDocumentCategory.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvDocumentCategory.EditIndex = -1;
        gvDocumentCategory.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

}
