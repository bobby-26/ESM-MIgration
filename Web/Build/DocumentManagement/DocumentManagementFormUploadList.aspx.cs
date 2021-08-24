using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class DocumentManagementFormUploadList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            string lastSubCategory = String.Empty;
            if (gvForm.Controls.Count > 0 && gvForm.Controls[0] != null)
            {
                Table gridTable = (Table)gvForm.Controls[0];
                foreach (GridViewRow gvr in gvForm.Rows)
                {
                    Label lblCategoryName = (Label)gvr.FindControl("lblCategoryName");
                    Label lblCategoryId = (Label)gvr.FindControl("lblCategoryId");

                    string categoryid = "";
                    string categoryname = "";

                    if (lblCategoryName != null) categoryname = lblCategoryName.Text;
                    if (lblCategoryId != null) categoryid = lblCategoryId.Text;                    

                    if (categoryid != null && categoryname != null)
                    {

                        if (lastSubCategory.CompareTo(categoryid) != 0)
                        {
                            int rowIndex = gridTable.Rows.GetRowIndex(gvr);

                            // Add new group header row  

                            GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

                            TableCell headerCell = new TableCell();

                            headerCell.ColumnSpan = gvForm.Columns.Count;

                            headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", categoryname) + "</b></font>";

                            headerCell.CssClass = "GroupHeaderRowStyle";

                            // Add header Cell to header Row, and header Row to gridTable  

                            headerRow.Cells.Add(headerCell);

                            gridTable.Controls.AddAt(rowIndex, headerRow);

                            // Update lastValue  

                            lastSubCategory = categoryid;
                        }
                    }
                }
            }
            foreach (GridViewRow r in gvForm.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvForm.UniqueID, "Select$" + r.RowIndex.ToString());
                }
            }

            base.Render(writer);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementFormUploadList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvForm')", "Print Grid", "icon_print.png", "PRINT");

            //MenuRegistersCountry.AccessRights = this.ViewState;
            //MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                //toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Category", "CATEGORY");
                //toolbar.AddButton("Form Upload", "FORM");
                //toolbar.AddButton("Form Definition", "FORMDEFINITION");

                //MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();
                //MenuDocument.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = "";
                ViewState["FORMREVISIONID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";

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

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS" };
        string[] alCaptions = { "Form No.", "Name", "Category", "Active Y/N", "Purpose", "Added Date", "Added By", "Revision" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentManagementDocumentAdmin.FormSearchByCategory(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , null
                                                        , null
                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                        , 1
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , iRowCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        , null
                                                        , null);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Form List</h3></td>");
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

    protected void MenuFormByCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvForm.EditIndex = -1;
                gvForm.SelectedIndex = -1;
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

    protected void MenuForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FORMDEFINITION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"] + "&CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("CATEGORY"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormCategoryList.aspx?CATEGORYID=" + ViewState["CATEGORYID"].ToString());
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

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS" };
        string[] alCaptions = { "Form No.", "Name", "Category", "Active Y/N", "Purpose", "Added Date", "Added By", "Revision" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementDocumentAdmin.FormSearchByCategory(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , null
                                                        , null
                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                        , 1
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , 50
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        , null
                                                        , null);

        General.SetPrintOptions("gvForm", "Form List", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvForm.DataSource = ds;
            gvForm.DataBind();
            if (General.GetNullableGuid(ViewState["FORMID"].ToString()) == null)
            {
                ViewState["FORMID"] = ds.Tables[0].Rows[0]["FLDFORMID"].ToString();
                ViewState["FORMREVISIONID"] = ds.Tables[0].Rows[0]["FLDFORMREVISIONID"].ToString();
                gvForm.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvForm);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvForm_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvForm_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvForm.EditIndex = -1;
        gvForm.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvForm_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
            }                  
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvForm_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvForm, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvForm_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvForm_RowEditing(object sender, GridViewEditEventArgs de)
    {
    }

    protected void gvForm_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        
    }

    protected void gvForm_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            HyperLink lnkfilename = (HyperLink)e.Row.FindControl("lnkfilename");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (lnkfilename != null)
            {
                if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "1")
                {
                    if (dr["FLDREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDREVISIONDTKEY"].ToString()) != null)
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDREVISIONDTKEY"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow drRow = dt.Rows[0];
                            lnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + dr["FLDDTKEY"].ToString();
                            //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                        }
                    }
                }
            }            
        }        
    }

    protected void gvForm_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvForm.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {            
            ViewState["FORMID"] = ((Label)gvForm.Rows[rowindex].FindControl("lblFormId")).Text;
            ViewState["FORMREVISIONID"] = ((Label)gvForm.Rows[rowindex].FindControl("lblFormRevisionId")).Text;         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvForm.EditIndex = -1;
        gvForm.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvForm.EditIndex = -1;
        gvForm.SelectedIndex = -1;
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
        gvForm.SelectedIndex = -1;
        gvForm.EditIndex = -1;
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

    private bool IsValidField(string formno, string formname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(formno) == null)
            ucError.ErrorMessage = "Form Number is required.";

        if (General.GetNullableString(formname) == null)
            ucError.ErrorMessage = "Form Name is required.";

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
        gvForm.SelectedIndex = -1;

        for (int i = 0; i < gvForm.Rows.Count; i++)
        {
            if (gvForm.DataKeys[i].Value.ToString().Equals(ViewState["FORMID"].ToString()))
            {
                gvForm.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvForm.EditIndex = -1;
        gvForm.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
}
