using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseSubjects : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar Maintoolbar = new PhoenixToolbar();
            Maintoolbar.AddButton("Course", "COURSE");
            Maintoolbar.AddButton("Batch", "BATCH");
            Maintoolbar.AddButton("Subjects", "SUBJECTS");
            MenuCourseMaster.AccessRights = this.ViewState;
            MenuCourseMaster.AccessRights = this.ViewState;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaCourseSubjects.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
            MenuPreSeaCourseSubjects.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;                
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSubjects.aspx");
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPreSeaCourseSubjects_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaExam.EditIndex = -1;
                gvPreSeaExam.SelectedIndex = -1;
                BindData();
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
        string[] alColumns = { "FLDSUBJECTNAME", "FLDSEMESTERID","FLDSUBJECTCODE", "FLDACTIVE"};
        string[] alCaptions = { "Subject Name", "Semester", "Subject Code","Active" };
        DataSet ds = PhoenixPreSeaCourseSubjects.ListPreSeaCourseSubjectTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection));

        General.SetPrintOptions("gvPreSeaExam", "Subjects", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaExam.DataSource = ds;
            gvPreSeaExam.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaExam);
        }
    }

    protected void ShowExcel()
    {

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBJECTNAME", "FLDSEMESTERID","FLDSUBJECTCODE", "FLDACTIVE" };
        string[] alCaptions = { "Subject Name", "Semester","Subject Code", "Active" };

        ds = PhoenixPreSeaCourseSubjects.ListPreSeaCourseSubjectTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection));

        Response.AddHeader("Content-Disposition", "attachment; filename=Subjects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
        Response.Write("<td><h3>PreSea Course Subjects </h3></td>");
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

    protected void gvPreSeaExam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaCourseSubjects(((UserControlMultiColumnPreSeaSubject)_gridView.FooterRow.FindControl("ucSubjectAdd")).SelectedValue)                                                 )

                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaCourseSubjects.InsertPreSeaCourseSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(Filter.CurrentPreSeaCourseMasterSelection)
                                                                    , General.GetNullableInteger((((DropDownList)_gridView.FooterRow.FindControl("ddlSemesterAdd")).SelectedValue))
                                                                    , int.Parse(((UserControlMultiColumnPreSeaSubject)_gridView.FooterRow.FindControl("ucSubjectAdd")).SelectedValue)
                                                                    , General.GetNullableByte(((CheckBox)_gridView.FooterRow.FindControl("chkAciveAdd")).Checked ? "1" : "0")
                                                                    ,(((TextBox)_gridView.FooterRow.FindControl("txtSubjectCodeAdd")).Text)
                                                                    );
                ucStatus.Text = "Course Subject added successfully.";                
                BindData();              
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPreSeaCourseSubjects.UpdatePreSeaCourseSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseSubjectIdEdit")).Text)
                                                                    , General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSemesterEdit")).SelectedValue)
                                                                    , General.GetNullableByte(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAciveEdit")).Checked ? "1" : "0")
                                                                    , (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubjectCodeEdit")).Text)
                                                                    );
                ucStatus.Text = "Course Subject updated successfully.";                
                _gridView.EditIndex = -1;
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaCourseSubjects.DeletePreSeaCourseSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseSubjectId")).Text));

                ucStatus.Text = "Course Subject deleted successfully.";                
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
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
            Label lblSemesterEdit = (Label)e.Row.FindControl("lblSemesterEdit");
            DropDownList ddlSemesterEdit = (DropDownList)e.Row.FindControl("ddlSemesterEdit");
            if (lblSemesterEdit != null)
            {
                ddlSemesterEdit.SelectedValue = lblSemesterEdit.Text;
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

            //DropDownList ddlSubjecttype = (DropDownList)e.Row.FindControl("ddlSubjectTypeAdd");

            //UserControlMultiColumnPreSeaSubject ucSub = (UserControlMultiColumnPreSeaSubject)e.Row.FindControl("ucSubjectAdd");
            //if (ucSub != null)
            //{                
            //    DataSet ds = PhoenixPreSeaSubject.PreSeaMainSubect(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlSubjecttype.SelectedValue));
            //    ucSub.MainSubjectList = ds.Tables[0];
            //    ucSub.DataBind();
            //}
        }
    }    

    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
    }

    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
       
    }

    private bool IsValidPreSeaCourseSubjects(string subjectid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (General.GetNullableInteger(subjectid) == null)
        {
            ucError.ErrorMessage = "Subject is required.";
        }
        
        return (!ucError.IsError);
    }
}

