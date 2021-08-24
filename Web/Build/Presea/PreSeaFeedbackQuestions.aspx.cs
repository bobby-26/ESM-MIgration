using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaFeedbackQuestions : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPreSeaFeedBackQuestions.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvPreSeaFeedBackQuestions.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../PreSea/PreSeaFeedbackQuestions.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvPreSeaFeedBackQuestions')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../PreSea/PreSeaFeedbackQuestions.aspx", "Find", "search.png", "FIND");
        MenuPreSeaFeedBackQuestions.AccessRights = this.ViewState;
        MenuPreSeaFeedBackQuestions.MenuList = toolbar.Show();
        MenuPreSeaFeedBackQuestions.SetTrigger(pnlPreSeaFeedBackQuestions);


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACADEMICTYPE"] = null;
        }
        BindData();

    }
    protected void ShowExcel()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPreSeaFeedbackQuestions.FeedbackQuestionsSearch(
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           General.ShowRecords(null),
                           ref iRowCount,
                           ref iTotalPageCount);

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=Subjects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Course Subjects</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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
    protected void PreSeaFeedBackQuestions_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            gvPreSeaFeedBackQuestions.SelectedIndex = -1;
            gvPreSeaFeedBackQuestions.EditIndex = -1;

            ViewState["PAGENUMBER"] = 1;
            BindData();

        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPreSeaFeedbackQuestions.FeedbackQuestionsSearch(
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           General.ShowRecords(null),
                           ref iRowCount,
                           ref iTotalPageCount);

        General.SetPrintOptions("gvPreSeaFeedBackQuestions", "", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaFeedBackQuestions.DataSource = ds;
            gvPreSeaFeedBackQuestions.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaFeedBackQuestions);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvPreSeaFeedBackQuestions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvPreSeaFeedBackQuestions_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvPreSeaFeedBackQuestions.SelectedIndex = e.NewSelectedIndex;
    }
    protected void gvPreSeaFeedBackQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            PhoenixPreSeaFeedbackQuestions.InsertPreSeaFeedbackQuestions(
                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                     ,((TextBox)_gridView.FooterRow.FindControl("txtQuestionAdd")).Text
                                     , General.GetNullableInteger(((RadioButtonList)_gridView.FooterRow.FindControl("rblQuestionTypeAdd")).SelectedValue)
                                     , ((TextBox)_gridView.FooterRow.FindControl("txtOptionsAdd")).Text);
            BindData();
            ((TextBox)_gridView.FooterRow.FindControl("txtQuestionAdd")).Focus();

        }

    }
    protected void gvPreSeaFeedBackQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            DeletePreSeaCourseSubjects(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectId")).Text));
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvPreSeaFeedBackQuestions_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuestionEdit")).Focus();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSeaFeedBackQuestions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {

            PhoenixPreSeaFeedbackQuestions.UpdatePreSeaFeedbackQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionIdEdit")).Text)
                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuestionEdit")).Text
                                , General.GetNullableInteger(((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblQuestionTypeEdit")).SelectedValue)
                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOptionsEdit")).Text);
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;

        BindData();
    }
    protected void gvPreSeaFeedBackQuestions_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPreSeaFeedBackQuestions, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvPreSeaFeedBackQuestions_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;

            RadioButtonList rboptions = (RadioButtonList)e.Row.FindControl("rblQuestionTypeEdit");
            if (rboptions != null && drv != null)
            {
                rboptions.SelectedValue = drv["FLDQUESTIONTYPE"].ToString();
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaFeedBackQuestions.SelectedIndex = -1;
        gvPreSeaFeedBackQuestions.EditIndex = -1;
        BindData();

    }
    //private void InsertPreSeaCourseSubjects(string subjectname, string qualification, string subjectcode)
    //{
    //    try
    //    {

    //        if (!IsValidSubject(subjectname, qualification))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixPreSeaAcadamicSubjects.InsertPreSeaCourseSubjects(
    //            PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //            , Convert.ToInt32(ddlCertificate.SelectedQualification)
    //            , subjectname
    //            , subjectcode);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //private void UpdatePreSeaCourseSubjects(int subjectid, string subjectname, string subjectcode)
    //{
    //    try
    //    {
    //        if (!IsValidSubject(subjectname, ddlCertificate.SelectedQualification))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixPreSeaAcadamicSubjects.UpdatePreSeaCourseSubjects(
    //            PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //            , Convert.ToInt32(ddlCertificate.SelectedQualification)
    //            , int.Parse(subjectid.ToString())
    //            , subjectname
    //            , subjectcode);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool IsValidSubject(string subjectname, string qualification)
    {
        ucError.HeaderMessage = "Please provide the following required information";

       
        return (!ucError.IsError);
    }
    private void DeletePreSeaCourseSubjects(int subjectid)
    {
        try
        {
            PhoenixPreSeaAcadamicSubjects.DeletePreSeaCourseSubjects(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , subjectid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void ddlPreSeaQualification_OnTextChanged(object senders, EventArgs e)
    {
        BindData();
    }
}
