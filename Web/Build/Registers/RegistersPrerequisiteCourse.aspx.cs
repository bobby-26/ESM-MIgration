using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;


public partial class RegistersPrerequisiteCourse : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in dgPrerequisiteCourse.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClick");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersPrerequisiteCourse.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('divGrid')", "Print Grid", "icon_print.png", "PRINT");
        MenuRegistersPrerequisiteCourse.AccessRights = this.ViewState;
        MenuRegistersPrerequisiteCourse.MenuList = toolbar.Show();
        MenuRegistersPrerequisiteCourse.SetTrigger(pnlPrerequisiteCourseEntry);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        BindData();
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOURSE", "FLDPREREQUISITECOURSE" };
        string[] alCaptions = { "Course", "Prerequisite Course" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixRegistersPrerequisiteCourse.PrerequisiteCourseSearch("", "", sortexpression, sortdirection,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PrerequisiteCourse.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Prerequisite Course</h3></td>");
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
    protected void RegistersPrerequisiteCourse_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersPrerequisiteCourse.PrerequisiteCourseSearch("", "", sortexpression, sortdirection,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            dgPrerequisiteCourse.DataSource = ds;
            dgPrerequisiteCourse.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, dgPrerequisiteCourse);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }
    protected void dgPrerequisiteCourse_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void dgPrerequisiteCourse_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertPrerequisiteCourse(
                ((TextBox)_gridView.FooterRow.FindControl("txtCourseAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtPrerequisiteCourseAdd")).Text
            );
            BindData();
            ((TextBox)_gridView.FooterRow.FindControl("txtCourseAdd")).Focus();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdatePrerequisiteCourse(
                 Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text),
                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPrerequisiteCourseEdit")).Text
             );
            _gridView.EditIndex = -1;
            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeletePrerequisiteCourse(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text));
        }
        else
        {
            _gridView.EditIndex = -1;
            BindData();
        }
        SetPageNavigator();
    }
    protected void dgPrerequisiteCourse_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void dgPrerequisiteCourse_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void dgPrerequisiteCourse_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int nCount = 0; nCount < e.Row.Cells.Count; nCount++)
                e.Row.Cells[nCount].Attributes.Add("columnname", "colname" + nCount);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete()");

                Label l = (Label)e.Row.FindControl("lblCourseId");

                LinkButton lb = (LinkButton)e.Row.FindControl("lnkCourse");
                lb.Attributes.Add("onclick", "OpenDataForm('Registers/RegistersPrerequisiteCourseEntry.aspx?CourseId=" + l.Text + "');");
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.FindControl("lnkDoubleClick");  //(LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
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
        dgPrerequisiteCourse.SelectedIndex = -1;
        dgPrerequisiteCourse.EditIndex = -1;
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


    private void InsertPrerequisiteCourse(string Course, string PrerequisiteCourse)
    {
        if (!IsValidPrerequisiteCourse(Course, PrerequisiteCourse))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersPrerequisiteCourse.InsertPrerequisiteCourse(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Course, PrerequisiteCourse);
    }

    private void UpdatePrerequisiteCourse(int CourseId, string Course, string PrerequisiteCourse)
    {
        if (!IsValidPrerequisiteCourse(Course, PrerequisiteCourse))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersPrerequisiteCourse.UpdatePrerequisiteCourse(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, CourseId, Course, PrerequisiteCourse);
    }

    private bool IsValidPrerequisiteCourse(string Course, string PrerequisiteCourse)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Course.Trim().Equals(""))
            ucError.ErrorMessage = "Course is required.";

        if (PrerequisiteCourse.Trim().Equals(""))
            ucError.ErrorMessage = "Prerequisite Course is required.";

        return (!ucError.IsError);
    }

    private void DeletePrerequisiteCourse(int CourseId)
    {
        PhoenixRegistersPrerequisiteCourse.DeletePrerequisiteCourse(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, CourseId);
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
