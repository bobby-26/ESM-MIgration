using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;

public partial class RegistersCourseTestQuestion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersCourseTestQuestion.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCourseTestQuestion')", "Print Grid", "icon_print.png", "PRINT");
            MenuCourseTestQuestion.AccessRights = this.ViewState;
            MenuCourseTestQuestion.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                BindCourse(ddlCourse);
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

        string[] alColumns = { "FLDCOURSE","FLDQUESTION" };
        string[] alCaptions = { "Course","Question" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersCourseTestQuestion.SearchCourseTestQuestion(General.GetNullableInteger(ddlCourse.SelectedValue), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CourseTestQuestion.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Course Test Question</h3></td>");
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

    protected void MenuCourseTestQuestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCOURSE", "FLDQUESTION"  };
        string[] alCaptions = { "Course", "Question"  };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixRegistersCourseTestQuestion.SearchCourseTestQuestion(General.GetNullableInteger(ddlCourse.SelectedValue), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvCourseTestQuestion", "Course Test Question", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCourseTestQuestion.DataSource = ds;
            gvCourseTestQuestion.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvCourseTestQuestion);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvCourseTestQuestion_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseTestQuestion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCourseTestQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DataRowView dr = (DataRowView)e.Row.DataItem;
            //DropDownList ddlCourseEdit = (DropDownList)e.Row.FindControl("ddlCourseEdit");
            //if (ddlCourseEdit != null)
            //{
            //    BindCourse(ddlCourseEdit);
            //    ddlCourseEdit.SelectedValue = dr["FLDCOURSEID"].ToString();
            //}
            DropDownList ddlQuestionEdit = (DropDownList)e.Row.FindControl("ddlQuestionEdit");
            if (ddlQuestionEdit != null)
            {
                BindQuestion(ddlQuestionEdit);
                ddlQuestionEdit.SelectedValue = dr["FLDQUESTIONID"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            //DropDownList ddlCourseAdd = (DropDownList)e.Row.FindControl("ddlCourseAdd");
            //if (ddlCourseAdd != null)
            //{
            //    BindCourse(ddlCourseAdd);
            //}
            DropDownList ddlQuestionAdd = (DropDownList)e.Row.FindControl("ddlQuestionAdd");
            if (ddlQuestionAdd != null)
                BindQuestion(ddlQuestionAdd);
        }
    }

    protected void BindCourse(DropDownList ddl)
    {
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
        ddl.DataTextField = "FLDCOURSE";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }
    protected void BindQuestion(DropDownList ddl)
    {
        ddl.DataSource = PhoenixRegistersTestAnswers.ListTestQuestions(null);
        ddl.DataTextField = "FLDQUESTION";
        ddl.DataValueField = "FLDQUESTIONID";
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void gvCourseTestQuestion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(ddlCourse.SelectedValue,
                    ((DropDownList)_gridView.FooterRow.FindControl("ddlQuestionAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersCourseTestQuestion.InsertCourseTestQuestion(int.Parse(ddlCourse.SelectedValue),
                   General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlQuestionAdd")).SelectedValue),
                   null);
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string coursetestquestionid = (_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixRegistersCourseTestQuestion.DeleteCourseTestQuestion(new Guid(coursetestquestionid));
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(ddlCourse.SelectedValue,
                    ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlQuestionEdit")).SelectedValue))
                    
                {
                    ucError.Visible = true;
                    return;
                }
                string coursetestquestionid = _gridView.DataKeys[nCurrentRow].Value.ToString();

                PhoenixRegistersCourseTestQuestion.UpdateCourseTestQuestion(new Guid(coursetestquestionid),
                    int.Parse(ddlCourse.SelectedValue),
                    int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlQuestionEdit")).SelectedValue),
                    null);

                _gridView.EditIndex = -1;
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
    protected void gvCourseTestQuestion_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCourseTestQuestion_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    private bool IsValidData(string courseid, string questionid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(courseid) == null)
            ucError.ErrorMessage = "Course is required.";

        if (General.GetNullableInteger(questionid) == null)
            ucError.ErrorMessage = "Question is required.";
        
        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvCourseTestQuestion.SelectedIndex = -1;
        gvCourseTestQuestion.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvCourseTestQuestion.SelectedIndex = -1;
        gvCourseTestQuestion.EditIndex = -1;
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
        gvCourseTestQuestion.SelectedIndex = -1;
        gvCourseTestQuestion.EditIndex = -1;
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
        {
            return true;
        }

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
}
