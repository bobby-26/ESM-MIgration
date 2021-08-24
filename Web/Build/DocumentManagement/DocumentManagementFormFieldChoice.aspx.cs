using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Web.UI.HtmlControls;

public partial class DocumentManagementFormFieldChoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementFormFieldChoice.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvFormFieldChoice')", "Print Grid", "icon_print.png", "PRINT");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                                

                if (Request.QueryString["FIELDID"] != null && Request.QueryString["FIELDID"].ToString() != "")
                    ViewState["FIELDID"] = Request.QueryString["FIELDID"].ToString();
                else
                    ViewState["FIELDID"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";

                if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != "")
                    ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();
                else
                    ViewState["REVISIONID"] = "";

                ViewState["CHOICEID"] = "";
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

        string[] alColumns = { "FLDTEXT" };
        string[] alCaptions = { "Text" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentManagementForm.FormFieldChoiceSearch(
                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , General.GetNullableGuid((ViewState["FIELDID"].ToString()))
                                                         , General.GetNullableGuid((ViewState["REVISIONID"].ToString()))
                                                         , sortexpression
                                                         , sortdirection
                                                         , (int)ViewState["PAGENUMBER"]
                                                         , iRowCount
                                                         , ref iRowCount
                                                         , ref iTotalPageCount);

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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvFormFieldChoice.EditIndex = -1;
                gvFormFieldChoice.SelectedIndex = -1;
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
                Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"] + "&REVISIONID=" + ViewState["REVISIONID"].ToString());
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

        string[] alColumns = { "FLDTEXT"};
        string[] alCaptions = { "Text"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementForm.FormFieldChoiceSearch(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , General.GetNullableGuid((ViewState["FIELDID"].ToString()))
                                                        , General.GetNullableGuid((ViewState["REVISIONID"].ToString()))                                                        
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvFormFieldChoice", "Choice List", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFormFieldChoice.DataSource = ds;
            gvFormFieldChoice.DataBind();
            if (General.GetNullableGuid(ViewState["CHOICEID"].ToString()) == null)
            {
                ViewState["CHOICEID"] = ds.Tables[0].Rows[0]["FLDCHOICEID"].ToString();               
                gvFormFieldChoice.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvFormFieldChoice);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvFormFieldChoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvFormFieldChoice_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvFormFieldChoice.EditIndex = -1;
        gvFormFieldChoice.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvFormFieldChoice_RowCommand(object sender, GridViewCommandEventArgs e)
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
                Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidField(
                     ((TextBox)_gridView.FooterRow.FindControl("txtTextAdd")).Text))                    
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDocumentManagementForm.FormFieldChoiceInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode                    
                    , new Guid(ViewState["FORMID"].ToString())
                    , new Guid(ViewState["REVISIONID"].ToString())
                    , new Guid(ViewState["FIELDID"].ToString())
                    , ((TextBox)_gridView.FooterRow.FindControl("txtValueAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtTextAdd")).Text
                    , null);

                ucStatus.Text = "Choice is added.";
                ViewState["CHOICEID"] = "";
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblChoiceId")).Text));

                ViewState["CHOICEID"] = "";
                ucStatus.Text = "Choice is deleted.";
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

    protected void gvFormFieldChoice_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormFieldChoice_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvFormFieldChoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidField(
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTextEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            Label lblChoiceIdEdit = (Label)_gridView.Rows[nCurrentRow].FindControl("lblChoiceIdEdit");
            string str = lblChoiceIdEdit.Text;     


            PhoenixDocumentManagementForm.FormFieldChoiceInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["FORMID"].ToString())
                , new Guid(ViewState["REVISIONID"].ToString())
                , new Guid(ViewState["FIELDID"].ToString())
                , null
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTextEdit")).Text
                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblChoiceIdEdit")).Text));                      

            ucStatus.Text = "Choice details updated.";
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

    protected void gvFormFieldChoice_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            
            if (cmdEdit != null && !SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                cmdEdit.Visible = false;
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;

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

    protected void gvFormFieldChoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvFormFieldChoice.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["CHOICEID"] = ((Label)gvFormFieldChoice.Rows[rowindex].FindControl("lblChoiceId")).Text;                       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvFormFieldChoice.EditIndex = -1;
        gvFormFieldChoice.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvFormFieldChoice.EditIndex = -1;
        gvFormFieldChoice.SelectedIndex = -1;
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
        gvFormFieldChoice.SelectedIndex = -1;
        gvFormFieldChoice.EditIndex = -1;
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

    private bool IsValidField(string text)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(text) == null)
            ucError.ErrorMessage = "Text is required.";      

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
        gvFormFieldChoice.SelectedIndex = -1;

        for (int i = 0; i < gvFormFieldChoice.Rows.Count; i++)
        {
            if (gvFormFieldChoice.DataKeys[i].Value.ToString().Equals(ViewState["CHOICEID"].ToString()))
            {
                gvFormFieldChoice.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormFieldChoice.EditIndex = -1;
        gvFormFieldChoice.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
}
