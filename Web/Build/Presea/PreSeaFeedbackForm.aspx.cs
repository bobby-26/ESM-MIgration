using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Presea_PreSeaFeedbackForm : PhoenixBasePage
{
    DataSet dsgrid = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuPreSeaFeedBackForm.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["feedbackid"] = "0";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            FeedBackAvailable();
        }
        BindData();
        BindCourseData();
        BindFeedback();
    }

    protected void PreSeaFeedBackForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            int feedbackid = 0;

            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                if (ViewState["feedbackid"].ToString() == "0")
                {
                    PhoenixPreSeaCandidateFeedback.InsertCandidateFeedBack(General.GetNullableInteger(lblCOurseId.Text),
                                                                           General.GetNullableInteger(lblBatchId.Text),
                                                                           General.GetNullableInteger(lblSemesterId.Text),
                                                                           General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection),
                                                                           General.GetNullableDateTime(ucFromDate.Text),
                                                                           General.GetNullableDateTime(ucToDate.Text),
                                                                           General.GetNullableInteger(ucOccassion.SelectedQuick),
                                                                           ref feedbackid,
                                                                           General.GetNullableString(txtFutureImprovements.Text),
                                                                           General.GetNullableString(txtSuggestionComments.Text));

                    ViewState["feedbackid"] = feedbackid;
                }
                else
                {
                    PhoenixPreSeaCandidateFeedback.UpdateCandidateFeedback(General.GetNullableInteger(ViewState["feedbackid"].ToString()),
                                                                           General.GetNullableDateTime(ucFromDate.Text),
                                                                           General.GetNullableDateTime(ucToDate.Text),
                                                                           General.GetNullableInteger(ucOccassion.SelectedQuick), 
                                                                           General.GetNullableString(txtFutureImprovements.Text),
                                                                           General.GetNullableString(txtSuggestionComments.Text));

                    FeedBackAvailable();
                }

                

                divForm.Visible = true;

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Presea/PreSeaFacultyFeedback.aspx?feedbackid=" + ViewState["feedbackid"].ToString() + "&batchid=" + lblBatchId.Text + "')", "Assign/Edit Faculty evaluation", "add.png", "FACULTYEVALUATION");
                MenuFacultyFeedback.MenuList = toolbarsub.Show();

                BindData();
                BindCourseData();
                BindFeedback();
            }
        }

    }

    protected void BindFeedback()
    {
        dsgrid = null;

        dsgrid = PhoenixPreSeaCandidateFeedback.FacultyRatingSearch(General.GetNullableInteger(ViewState["feedbackid"].ToString()));

        if (dsgrid.Tables.Count > 0 && dsgrid.Tables[0].Rows.Count > 0)
        {
            DataTable dt = dsgrid.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BoundField field = new BoundField();
                field.HeaderText = dt.Rows[i]["FLDCOLUMNAME"].ToString();
                field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                field.ItemStyle.Wrap = false;
                gvPreSea.Columns.Add(field);
            }
            gvPreSea.DataSource = dsgrid.Tables[1];
            gvPreSea.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dsgrid.Tables[1], gvPreSea);
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            gv.HeaderRow.Cells[0].Text = "&nbsp;";

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void FeedBackAvailable()
    {
        DataSet ds = PhoenixPreSeaCandidateFeedback.IsFeedbackAvailable(int.Parse(Filter.CurrentPreSeaNewApplicantBatch));

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblName.Text = PhoenixSecurityContext.CurrentSecurityContext.FirstName + ' ' + PhoenixSecurityContext.CurrentSecurityContext.MiddleName + ' ' + PhoenixSecurityContext.CurrentSecurityContext.LastName;
            lblCurrentDate.Text = ds.Tables[0].Rows[0]["FLDDATE"].ToString();
            lblCourseName.Text = ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();
            lblBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNO"].ToString();
            lblCOurseId.Text = ds.Tables[0].Rows[0]["FLDCOURSEID"].ToString();
            lblSemesterId.Text = ds.Tables[0].Rows[0]["FLDSEMESTERID"].ToString();
            lblBatchId.Text = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();

            DataSet dsdetauls = PhoenixPreSeaCandidateFeedback.CandidateFeedbackFetch(General.GetNullableInteger(lblCOurseId.Text)
                                                                                    , General.GetNullableInteger(lblBatchId.Text)
                                                                                    , General.GetNullableInteger(lblSemesterId.Text)
                                                                                    , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));
            if (dsdetauls.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsdetauls.Tables[0].Rows[0];

                ViewState["feedbackid"] = dr["FLDFEEDBACKID"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();
                ucOccassion.SelectedQuick = dr["FLDOCCASSION"].ToString();
                txtFutureImprovements.Text = dr["FLDCOURSESUGGESTIONS"].ToString();
                txtSuggestionComments.Text = dr["FLDIMPROVINGFEEDBACK"].ToString();

                divForm.Visible = true;

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Presea/PreSeaFacultyFeedback.aspx?feedbackid=" + ViewState["feedbackid"].ToString() + "&batchid=" + lblBatchId.Text + "')", "Assign/Edit Faculty evaluation", "add.png", "FACULTYEVALUATION");
                MenuFacultyFeedback.MenuList = toolbarsub.Show();
            }
        }
        else
        {
            Response.Redirect("../Presea/PreSeaCandidateFeedbackNotAvailable.aspx", true);
        }
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ucFromDate.Text))
        {
            ucError.ErrorMessage = "From Date is required";
        }

        if (string.IsNullOrEmpty(ucToDate.Text))
        {
            ucError.ErrorMessage = "To Date is required";
        }

        if (ucOccassion.SelectedQuick == "" || ucOccassion.SelectedQuick == "Dummy")
        {
            ucError.ErrorMessage = "Occassion is required";
        }
        return (!ucError.IsError);
    }

    protected void gvFeedbackQuestions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedbackQuestions_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedbackQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedBackQuestionId")).Text == "")
                {
                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "1")
                    {


                        PhoenixPreSeaCandidateFeedback.FeedbackQuestionInsert(General.GetNullableInteger(ViewState["feedbackid"].ToString())
                            , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionIdEdit")).Text)
                            , ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblOptions")).SelectedValue
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection)
                            );
                    }

                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "2")
                    {
                        StringBuilder stroptions = new StringBuilder();

                        CheckBoxList chklist = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkOptions");

                        foreach (ListItem item in chklist.Items)
                        {
                            if (item.Selected == true)
                            {
                                stroptions.Append(item.Value.ToString());
                                stroptions.Append(",");
                            }
                        }

                        if (stroptions.Length > 1)
                        {
                            stroptions.Remove(stroptions.Length - 1, 1);
                        }

                        PhoenixPreSeaCandidateFeedback.FeedbackQuestionInsert(General.GetNullableInteger(ViewState["feedbackid"].ToString())
                            , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionIdEdit")).Text)
                            , stroptions.ToString()
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection)
                            );
                    }

                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "3")
                    {
                        PhoenixPreSeaCandidateFeedback.FeedbackQuestionInsert(General.GetNullableInteger(ViewState["feedbackid"].ToString())
                            , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionIdEdit")).Text)
                            , null
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection)
                            );
                    }


                }

                else
                {
                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "1")
                    {
                        PhoenixPreSeaCandidateFeedback.FeedBackQuestionUpdate(
                            General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedBackQuestionId")).Text),
                            ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblOptions")).SelectedValue,
                            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            );
                    }

                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "2")
                    {
                        StringBuilder stroptions = new StringBuilder();

                        CheckBoxList chklist = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkOptions");

                        foreach (ListItem item in chklist.Items)
                        {
                            if (item.Selected == true)
                            {
                                stroptions.Append(item.Value.ToString());
                                stroptions.Append(",");
                            }
                        }

                        if (stroptions.Length > 1)
                        {
                            stroptions.Remove(stroptions.Length - 1, 1);
                        }

                        PhoenixPreSeaCandidateFeedback.FeedBackQuestionUpdate(
                            General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedBackQuestionId")).Text),
                            stroptions.ToString(),
                            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            );
                    }

                    if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOptionTypeEdit")).Text == "3")
                    {
                        PhoenixPreSeaCandidateFeedback.FeedBackQuestionUpdate(
                            General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedBackQuestionId")).Text),
                            null,
                            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text
                            );
                    }

                }


            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedbackQuestions_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;

            string options = drv["FLDOPTIONS"].ToString();
            string[] split = options.Split(',');

            if (drv["FLDQUESTIONTYPE"].ToString() == "1")
            {
                RadioButtonList rblList = (RadioButtonList)e.Row.FindControl("rblOptions");

                if (rblList != null)
                {
                    rblList.Visible = true;
                    rblList.DataSource = split;
                    rblList.DataBind();

                    if (drv["FLDANSWER"].ToString() != "")
                        rblList.SelectedValue = drv["FLDANSWER"].ToString();
                }


            }

            if (drv["FLDQUESTIONTYPE"].ToString() == "2")
            {
                CheckBoxList chklist = (CheckBoxList)e.Row.FindControl("chkOptions");

                if (chklist != null)
                {
                    chklist.Visible = true;
                    chklist.DataSource = split;
                    chklist.DataBind();

                    if (drv["FLDANSWER"].ToString() != "")
                    {
                        string[] answer = drv["FLDANSWER"].ToString().Split(',');

                        foreach (String option in answer)
                        {
                            foreach (ListItem item in chklist.Items)
                            {
                                if (item.Value == option)
                                    item.Selected = true;
                            }
                        }
                    }

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

    protected void gvFeedbackQuestions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPreSeaCandidateFeedback.QuestionSearch(General.GetNullableInteger(ViewState["feedbackid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvFeedbackQuestions.DataSource = ds;
            gvFeedbackQuestions.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvFeedbackQuestions);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindCourseData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPreSeaCandidateFeedback.COurseEvaluationSearch(General.GetNullableInteger(ViewState["feedbackid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvCourseEvaluation.DataSource = ds;
            gvCourseEvaluation.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCourseEvaluation);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gv = (GridView)sender;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string questionid = drv["FLDQUESTIONID"].ToString();
                DataTable header = dsgrid.Tables[0];
                DataTable data = dsgrid.Tables[2];
                LinkButton lbtn = new LinkButton();
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] dr = data.Select("FLDQUESTIONID = " + questionid + "AND FLDCOLUMNAME ='" + header.Rows[i]["FLDCOLUMNAME"].ToString() + "'");
                    e.Row.Cells[i + 1].Text = (dr.Length > 0 ? dr[0]["FLDRATING"].ToString() : "");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedbackQuestions_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvFeedbackQuestions.SelectedIndex = -1;
        gvFeedbackQuestions.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvFeedbackQuestions_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            //e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFeedbackQuestions, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCourseEvaluation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;
            BindCourseData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseEvaluation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;

            BindCourseData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseEvaluation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedbackCOurseId")).Text == "")
                {
                    if (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGrade")).Text == "")
                    {
                        ucError.ErrorMessage = "Grade is required.";
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {

                        PhoenixPreSeaCandidateFeedback.CourseEvaluationInsert(General.GetNullableInteger(ViewState["feedbackid"].ToString())
                                                            , General.GetNullableInteger(lblCOurseId.Text)
                                                            , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionId")).Text)
                                                            , decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGrade")).Text)
                                                            , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection)
                                                            , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantBatch));
                    }
                }
                else
                {
                    PhoenixPreSeaCandidateFeedback.CourseEvaluationUpdate(General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFeedbackCOurseId")).Text)
                                                                            , decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGrade")).Text));
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseEvaluation_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
    }

    protected void gvCourseEvaluation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindCourseData();
    }

    protected void gvCourseEvaluation_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCourseEvaluation.SelectedIndex = -1;
        gvCourseEvaluation.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindCourseData();
    }

    protected void gvCourseEvaluation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            //e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFeedbackQuestions, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void MenuFacultyFeedback_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindFeedback();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
