using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaMainSubject : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPreSeaMainSubject.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvPreSeaMainSubject.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Main Subject","MAINSUBJECT");
            toolbarmain.AddButton("Sub Subject", "SUBSUBJECT");
            MenuMainSub.MenuList = toolbarmain.Show();
            MenuMainSub.AccessRights = this.ViewState;

            MenuMainSub.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaMainSubject.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPreSeaMainSubject')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PreSea/PreSeaMainSubject.aspx", "Find", "search.png", "FIND");
            MenuPreSeaMainSubject.AccessRights = this.ViewState;
            MenuPreSeaMainSubject.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDMAINSUBJECTNAME", "FLDISACTIVE" };
        string[] alCaptions = { "Subject Code", "Main Subject Name", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaMainSubject.PreSeaMainSubjectSearch(General.GetNullableString(txtMainSubjectCode.Text)
            , General.GetNullableString(txtMainSubjectSearch.Text)
            , General.GetNullableInteger(ddlActive.SelectedValue)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaMainSubjects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre-Sea Main Subject</h3></td>");
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

    protected void PreSeaMainSubject_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaMainSubject.EditIndex = -1;
                gvPreSeaMainSubject.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    protected void MenuMainSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SUBSUBJECT"))
            {
                Response.Redirect("~/PreSea/PreSeaSubject.aspx", false);
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
        
	    string[] alColumns = {"FLDCODE", "FLDMAINSUBJECTNAME","FLDISACTIVE" };
        string[] alCaptions = {"Subject Code","Main Subject Name", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


         DataSet ds = PhoenixPreSeaMainSubject.PreSeaMainSubjectSearch(General.GetNullableString(txtMainSubjectCode.Text)
             , General.GetNullableString(txtMainSubjectSearch.Text)
             , General.GetNullableInteger(ddlActive.SelectedValue)
             , sortexpression, sortdirection
             , (int)ViewState["PAGENUMBER"]
             , General.ShowRecords(null)
             , ref iRowCount
             , ref iTotalPageCount
             );

         General.SetPrintOptions("gvPreSeaMainSubject", "Pre-Sea Main Subject", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaMainSubject.DataSource = ds;
            gvPreSeaMainSubject.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaMainSubject);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    
    protected void gvPreSeaMainSubject_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvPreSeaMainSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaMainSubject(((TextBox)_gridView.FooterRow.FindControl("txtMainSubjectCodeAdd")).Text
                    ,((TextBox)_gridView.FooterRow.FindControl("txtMainSubjectNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaMainSubject(
                    ((TextBox)_gridView.FooterRow.FindControl("txtMainSubjectCodeAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtMainSubjectNameAdd")).Text,
                    (((CheckBox)_gridView.FooterRow.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                );
                ((TextBox)_gridView.FooterRow.FindControl("txtMainSubjectNameAdd")).Focus();
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaMainSubject(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMainSubjectCodeEdit")).Text
                   , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMainSubjectNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaMainSubject(
                    Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMainSubjectIdEdit")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMainSubjectCodeEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMainSubjectNameEdit")).Text,
                     (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? "1" : "0"
                 );
                _gridView.EditIndex = -1;
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaMainSubject(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMainSubjectId")).Text));
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvPreSeaMainSubject_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvPreSeaMainSubject_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvPreSeaMainSubject_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        SetPageNavigator();
    }
    protected void gvPreSeaMainSubject_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaMainSubject.EditIndex = -1;
        gvPreSeaMainSubject.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaMainSubject.EditIndex = -1;
        gvPreSeaMainSubject.SelectedIndex = -1;
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
        gvPreSeaMainSubject.SelectedIndex = -1;
        gvPreSeaMainSubject.EditIndex = -1;
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

    private void InsertPreSeaMainSubject(string subjectcode,string mainsubjectname, int activeyn)
    {

        PhoenixPreSeaMainSubject.InsertPreSeaMainSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            ,subjectcode
            ,mainsubjectname
            ,activeyn);
        ucStatus.Text = "Main Subject information added";
    }

    private void UpdatePreSeaMainSubject(int mainsubjectid, string subjectcode,string mainsubjectname,string activeyn)
    {
        PhoenixPreSeaMainSubject.UpdatePreSeaMainSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            subjectcode,mainsubjectid, mainsubjectname, General.GetNullableByte(activeyn));
        ucStatus.Text = "Main Subject information updated";
    }

    private bool IsValidPreSeaMainSubject(string code,string subjectname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Subject Code is required.";

        if (subjectname.Trim().Equals(""))
            ucError.ErrorMessage = "Subject Name is required.";

        return (!ucError.IsError);
    }

    private void DeletePreSeaMainSubject(int mainsubjectid)
    {
        PhoenixPreSeaMainSubject.DeletePreseaMainSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode, mainsubjectid);
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

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
}
