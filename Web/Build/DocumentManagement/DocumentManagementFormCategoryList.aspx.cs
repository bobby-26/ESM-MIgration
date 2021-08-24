using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Web.UI.HtmlControls;

public partial class DocumentManagementFormCategoryList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        string lastCategoryType = String.Empty;
        if (gvFormCategory.Controls.Count > 0 && gvFormCategory.Controls[0] != null)
        {
            Table gridTable = (Table)gvFormCategory.Controls[0];
            foreach (GridViewRow gvr in gvFormCategory.Rows)
            {
                Label lblTypeName = (Label)gvr.FindControl("lblTypeName");
                Label lblType = (Label)gvr.FindControl("lblType");

                Label lblTypeNameEdit = (Label)gvr.FindControl("lblTypeNameEdit");
                Label lblTypeEdit = (Label)gvr.FindControl("lblTypeEdit");

                string categorytype = "";
                string typename = "";

                if (lblTypeName != null) typename = lblTypeName.Text;
                if (lblTypeNameEdit != null) typename = lblTypeNameEdit.Text;

                if (lblType != null) categorytype = lblType.Text;
                if (lblTypeEdit != null) categorytype = lblTypeEdit.Text;

                if (typename != null && categorytype != null)
                {

                    if (lastCategoryType.CompareTo(categorytype) != 0)
                    {
                        int rowIndex = gridTable.Rows.GetRowIndex(gvr);

                        // Add new group header row  

                        GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

                        TableCell headerCell = new TableCell();

                        headerCell.ColumnSpan = gvFormCategory.Columns.Count;

                        headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", typename) + "</b></font>";

                        headerCell.CssClass = "GroupHeaderRowStyle";

                        // Add header Cell to header Row, and header Row to gridTable  

                        headerRow.Cells.Add(headerCell);

                        gridTable.Controls.AddAt(rowIndex, headerRow);

                        // Update lastValue  

                        lastCategoryType = categorytype;
                    }
                }
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
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementFormCategoryList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvFormCategory')", "Print Grid", "icon_print.png", "PRINT");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Category", "CATEGORY");
                toolbar.AddButton("Form", "FORM");
                //toolbar.AddButton("Form Definition", "FORMDEFINITION");

                MenuDocument.AccessRights = this.ViewState;
                MenuDocument.MenuList = toolbar.Show();
                MenuDocument.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = "";
                ViewState["FORMREVISIONID"] = "";

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

        string[] alColumns = { "FLDROWNUMBER", "FLDCATEGORYNAME", "FLDTYPENAME", "FLDCOUNT" };
        string[] alCaptions = { "No.", "Category", "Type", "Count" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixDocumentManagementForm.FormCategorySearch(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , null
                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                        , null
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , iRowCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormCategoryList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Form Category List</h3></td>");
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
                gvFormCategory.EditIndex = -1;
                gvFormCategory.SelectedIndex = -1;
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
                ViewState["FORMID"] = "";
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
            if (dce.CommandName.ToUpper().Equals("FORMDEFINITION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"] + "&FORMREVISIONID=" + ViewState["FORMREVISIONID"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("FORM"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?CATEGORYID=" + ViewState["CATEGORYID"]);
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

        string[] alColumns = { "FLDCATEGORYNUMBER", "FLDCATEGORYNAME", "FLDTYPENAME", "FLDCOUNT" };
        string[] alCaptions = { "No.", "Category", "Type", "Count"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementForm.FormCategorySearch(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , null
                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                        , null                                                        
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvFormCategory", "Form Category List", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFormCategory.DataSource = ds;
            gvFormCategory.DataBind();
            if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) == null)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();                
                gvFormCategory.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvFormCategory);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvFormCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvFormCategory_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvFormCategory.EditIndex = -1;
        gvFormCategory.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvFormCategory_RowCommand(object sender, GridViewCommandEventArgs e)
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
                Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?CATEGORYID=" + ViewState["CATEGORYID"]);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCategory(
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucCategoryNumberAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNameAdd")).Text,
                    ((RadioButtonList)_gridView.FooterRow.FindControl("rListAdd")).SelectedItem.Value))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDocumentManagementForm.FormCategoryInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCategoryNameAdd")).Text
                    , ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucCategoryNumberAdd")).Text
                    , General.GetNullableInteger(((RadioButtonList)_gridView.FooterRow.FindControl("rListAdd")).SelectedItem.Value)
                    , null
                    , General.GetNullableGuid(((DropDownList)_gridView.FooterRow.FindControl("ddlDirectoryAdd")).SelectedValue)
                    );

                ucStatus.Text = "Category is added.";
                ViewState["CATEGORYID"] = "";
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormCategoryDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryId")).Text));

                ViewState["CATEGORYID"] = "";
                ucStatus.Text = "Category is deleted.";
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormCategory_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormCategory_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvFormCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidCategory(
                   ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucCategoryNumberEdit")).Text,
                   ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNameEdit")).Text,
                   ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rListEdit")).SelectedItem.Value))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementForm.FormCategoryInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryNameEdit")).Text
                , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucCategoryNumberEdit")).Text
                , General.GetNullableInteger(((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rListEdit")).SelectedItem.Value)
                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryIdEdit")).Text)
                , General.GetNullableGuid(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDirectoryEdit")).SelectedValue));
           
            ucStatus.Text = "Category details updated.";
            _gridView.EditIndex = -1;
            BindData();

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormCategory_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            ImageButton cmgViewContent = (ImageButton)e.Row.FindControl("cmgViewContent");            
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");

            if (cmgViewContent != null && !SessionUtil.CanAccess(this.ViewState, cmgViewContent.CommandName))
                cmgViewContent.Visible = false;
            if (cmdEdit != null && !SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                cmdEdit.Visible = false;
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;

            DataRowView dr = (DataRowView)e.Row.DataItem;
            RadioButtonList rListEdit = (RadioButtonList)e.Row.FindControl("rListEdit");
            if (rListEdit != null)
            {
                if (dr != null && dr["FLDTYPE"].ToString() == "0")
                    rListEdit.Items[0].Selected = true;
                if (dr != null && dr["FLDTYPE"].ToString() == "1")
                    rListEdit.Items[1].Selected = true;
            }

            DropDownList ddlDirectoryEdit = (DropDownList)e.Row.FindControl("ddlDirectoryEdit");
            if (ddlDirectoryEdit != null)
            {
                ddlDirectoryEdit.DataSource = PhoenixDocumentManagementDirectory.DirectoryList();
                ddlDirectoryEdit.DataTextField = "FLDDIRECTORYNAME";
                ddlDirectoryEdit.DataValueField = "FLDDIRECTORYID";
                ddlDirectoryEdit.DataBind();

                ddlDirectoryEdit.Items.Insert(0, new ListItem("--Select", "Dummy"));

                if (dr != null)
                    ddlDirectoryEdit.SelectedValue = dr["FLDDIRECTORYID"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList ddlDirectoryAdd = (DropDownList)e.Row.FindControl("ddlDirectoryAdd");
            if (ddlDirectoryAdd != null)
            {
                ddlDirectoryAdd.DataSource = PhoenixDocumentManagementDirectory.DirectoryList();
                ddlDirectoryAdd.DataTextField = "FLDDIRECTORYNAME";
                ddlDirectoryAdd.DataValueField = "FLDDIRECTORYID";
                ddlDirectoryAdd.DataBind();
                ddlDirectoryAdd.Items.Insert(0, new ListItem("--Select", "Dummy"));
            }
        }
    }

    protected void gvFormCategory_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvFormCategory.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["CATEGORYID"] = ((Label)gvFormCategory.Rows[rowindex].FindControl("lblCategoryId")).Text;            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvFormCategory.EditIndex = -1;
        gvFormCategory.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvFormCategory.EditIndex = -1;
        gvFormCategory.SelectedIndex = -1;
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
        gvFormCategory.SelectedIndex = -1;
        gvFormCategory.EditIndex = -1;
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

    private bool IsValidCategory(string categorynumber ,string categoryname, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(categorynumber) == null)
            ucError.ErrorMessage = "Category Number is required.";

        if (General.GetNullableString(categoryname) == null)
            ucError.ErrorMessage = "Category Name is required.";

        if (General.GetNullableInteger(type) == null)
            ucError.ErrorMessage = "Category Type is required.";

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
        gvFormCategory.SelectedIndex = -1;

        for (int i = 0; i < gvFormCategory.Rows.Count; i++)
        {
            if (gvFormCategory.DataKeys[i].Value.ToString().Equals(ViewState["CATEGORYID"].ToString()))
            {
                gvFormCategory.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormCategory.EditIndex = -1;
        gvFormCategory.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
}
