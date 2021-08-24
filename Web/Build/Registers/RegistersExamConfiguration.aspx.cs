using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class Registers_RegistersExamConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersExamConfiguration.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvTestConfiguration')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersExamConfiguration.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','RegistersExamConfigurationAdd.aspx?type=ADD&ExamId=');", "Add New", "add.png", "ADD");
            MenuRegistersExamConfiguration.AccessRights = this.ViewState;
            MenuRegistersExamConfiguration.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

    protected void BindCourse(DropDownList ddl)
    {
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
        ddl.DataTextField = "FLDCOURSE";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOURSENAME", "FLDEXAMNAME", "FLDNOOFQUESTIONS", "FLDPASSMARK" };
        string[] alCaptions = { "Course", "Exam", "Max Questions", "Pass Mark" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersExamConfiguration.SearchConfiguration(General.GetNullableString(txtExamname.Text)
                , General.GetNullableInteger(ddlCourse.SelectedValue)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ExamConfiguration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Exam Configuration</h3></td>");
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

    protected void RegistersExamConfiguration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvTestConfiguration.EditIndex = -1;
                gvTestConfiguration.SelectedIndex = -1;
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCOURSENAME", "FLDEXAMNAME", "FLDNOOFQUESTIONS", "FLDPASSMARK" };
        string[] alCaptions = { "Course", "Exam", "Max Questions", "Pass Mark" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersExamConfiguration.SearchConfiguration(General.GetNullableString(txtExamname.Text),
                    General.GetNullableInteger(ddlCourse.SelectedValue),
                    sortexpression,
                    sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvTestConfiguration", "Exam Configuration", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTestConfiguration.DataSource = ds;
            gvTestConfiguration.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvTestConfiguration);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvTestConfiguration_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidConfiguration(((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text,
                    ddlCourse.SelectedValue,
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucMaxQuestionAdd")).Text,
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucPassmarkAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string courseid = ddlCourse.SelectedValue;
                InsertConfiguration(
                    ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text,
                    int.Parse(courseid), 
                    General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucMaxQuestionAdd")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucPassmarkAdd")).Text));

            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidConfiguration(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text,
                   ddlCourse.SelectedValue,
                   ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucMaxQuestionEdit")).Text,
                   ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucPassmarkEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string courseid = ddlCourse.SelectedValue;
                UpdateConfiguration(
                     General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamidEdit")).Text),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text,
                     int.Parse(courseid),
                     General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucMaxQuestionEdit")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucPassmarkEdit")).Text)
                 );
                _gridView.EditIndex = -1;
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteConfiguration(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamid")).Text));
            }
            else if (e.CommandName.ToString().ToUpper() == "CEDIT")
            {


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

    protected void gvTestConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            //string sExamid = ((Label)e.Row.FindControl("lblExamid")).Text;
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                //ed.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','RegistersExamConfigurationAdd.aspx?type=EDIT&ExamId=" + sExamid +"');");
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }

            UserControlCourse ucCourse = (UserControlCourse)e.Row.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Row.DataItem;
            if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSEID"].ToString();
        }
    }

    protected void gvTestConfiguration_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
   
    protected void gvTestConfiguration_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvTestConfiguration_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvTestConfiguration_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvTestConfiguration.EditIndex = -1;
        gvTestConfiguration.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private void InsertConfiguration(string Examename, int courseid, int? maxquestion, int? passmark)
    {
        PhoenixRegistersExamConfiguration.InsertConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Examename, courseid, null, maxquestion, passmark);
    }

    private void UpdateConfiguration(Guid? examid, string Examename, int courseid, int? maxquestion, int? passmark)
    {
        PhoenixRegistersExamConfiguration.UpdateConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            examid, Examename, courseid, null, maxquestion, passmark);
    }

    private void DeleteConfiguration(Guid? examid)
    {
        PhoenixRegistersExamConfiguration.DeleteConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            examid);
    }

    private bool IsValidConfiguration(string examname, string courseid, string maxquestion, string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is required.";
        if (examname.Trim().Equals(""))
            ucError.ErrorMessage = "Exam is required.";        
        if (General.GetNullableInteger(maxquestion) == null)
            ucError.ErrorMessage = "Max Question is required.";
        if (General.GetNullableInteger(passmark) == null)
            ucError.ErrorMessage = "Passmark is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvTestConfiguration.EditIndex = -1;
        gvTestConfiguration.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvTestConfiguration.EditIndex = -1;
        gvTestConfiguration.SelectedIndex = -1;
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
        gvTestConfiguration.SelectedIndex = -1;
        gvTestConfiguration.EditIndex = -1;
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
