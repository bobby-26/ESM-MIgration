using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchSubjects : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                PhoenixToolbar MainToolbar = new PhoenixToolbar();

                MainToolbar.AddButton("Batch", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
                MainToolbar.AddButton("Semester", "SEMESTER");

                MenuBatchPlanner.AccessRights = this.ViewState;
                MenuBatchPlanner.MenuList = MainToolbar.Show();
                    
               // MenuBatchPlanner.SelectedMenuIndex = 3;

                PhoenixToolbar subToolbar = new PhoenixToolbar();
                subToolbar.AddButton("Subjects", "BATCHSUBJECTS");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = subToolbar.Show();
                MenuPreSea.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;
                ucBatch.SelectedBatch = Filter.CurrentPreSeaCourseMasterBatchSelection;
                ucBatch.Enabled = false;
               
                BindData();
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("BATCHSUBJECTS"))
        {
            Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
        }
    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
                DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                if (dce.CommandName.ToUpper().Equals("BATCH"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchManageDetails.aspx");
                }              
                else if (dce.CommandName.ToUpper().Equals("EVENT"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchEvents.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("EXAM"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("CONTACT"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchContact.aspx");
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
        DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection),null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBatchSubject.DataSource = ds;
            gvBatchSubject.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBatchSubject);
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
    

    protected void gvBatchSubject_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvBatchSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaBatchSubjects(((UserControlPreSeaSemester)_gridView.FooterRow.FindControl("ucSemesterAdd")).SelectedSemester
                                                , ((DropDownList)_gridView.FooterRow.FindControl("ddlBatchSubjectAdd")).SelectedValue
                                                , ((TextBox)_gridView.FooterRow.FindControl("txtSubjectCodeAdd")).Text
                                                , ((UserControlNumber)_gridView.FooterRow.FindControl("txtMaxMarkAdd")).Text
                                                , ((UserControlNumber)_gridView.FooterRow.FindControl("txtPassMarkAdd")).Text
                                                , ((UserControlNumber)_gridView.FooterRow.FindControl("txtTotalClasssAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaBatchManager.InsertPreSeaBatchSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection)
                                                                    , int.Parse(((UserControlPreSeaSemester)_gridView.FooterRow.FindControl("ucSemesterAdd")).SelectedSemester)
                                                                    , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlBatchSubjectAdd")).SelectedValue)
                                                                    , ((TextBox)_gridView.FooterRow.FindControl("txtSubjectCodeAdd")).Text
                                                                    ,  decimal.Parse(((UserControlNumber)_gridView.FooterRow.FindControl("txtMaxMarkAdd")).Text)
                                                                    , decimal.Parse(((UserControlNumber)_gridView.FooterRow.FindControl("txtPassMarkAdd")).Text)
                                                                    , decimal.Parse(((UserControlNumber)_gridView.FooterRow.FindControl("txtTotalClasssAdd")).Text)
                                                                    , General.GetNullableByte(((CheckBox)_gridView.FooterRow.FindControl("chkAciveAdd")).Checked ? "1" : "0")
                                                                    );
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaBatchSubjects(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterIdEdit")).Text
                                                , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSubjectIdEdit")).Text
                                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubjectCodeEdit")).Text
                                                , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text
                                                , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text
                                                , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtTotalClassEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPreSeaBatchManager.UpdatePreSeaBatchSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSubjectIdEdit")).Text)
                                                                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubjectCodeEdit")).Text
                                                                    , decimal.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text)
                                                                    , decimal.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text)
                                                                    , decimal.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtTotalClassEdit")).Text)
                                                                    , General.GetNullableByte(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAciveEdit")).Checked ? "1" : "0")
                                                                    );
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaBatchManager.DeletePreSeaBatchSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSubjectId")).Text));
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatchSubject_RowDataBound(object sender, GridViewRowEventArgs e)
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

            UserControlPreSeaExam ddlExamEdit = (UserControlPreSeaExam)e.Row.FindControl("ddlExamEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ddlExamEdit != null)
            {
                ddlExamEdit.SelectedExam = drv["FLDEXAMID"].ToString();
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

            UserControlPreSeaSemester ucSem = (UserControlPreSeaSemester)e.Row.FindControl("ucSemesterAdd");
            if (ucSem != null)
            {
                
                DataSet ds = PhoenixPreSeaBatchManager.ListBatchSemesters(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            ,General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection),General.GetNullableInteger(null));                
                ucSem.Batch = Filter.CurrentPreSeaCourseMasterBatchSelection;                
                ucSem.SemesterList = ds;
                ucSem.bind();
            }

            DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlBatchSubjectAdd");
            if (ddlSubject != null)
            {

                ddlSubject.DataSource = PhoenixPreSeaCourseSubjects.ListPreSeaCourseSubjectTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection));//ViewState["SEMESTERID"].ToString())
                ddlSubject.DataTextField = "FLDSUBJECTNAME";
                ddlSubject.DataValueField = "FLDSUBJECTID";
                ddlSubject.DataBind();
                ddlSubject.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            }
        }
    }

    protected void gvBatchSubject_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvBatchSubject_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvBatchSubject_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    private bool IsValidPreSeaBatchSubjects(string semesterid, string subjectid, string subjectcode, string maxscore, string passscore, string totalclass)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(semesterid) == null)
        {
            ucError.ErrorMessage = "Semester is required.";
        }
        if (General.GetNullableInteger(subjectid) == null)
        {
            ucError.ErrorMessage = "Subject is required.";
        }
        if (String.IsNullOrEmpty(subjectcode))
        {
            ucError.ErrorMessage = "Subject code is required.";
        }
        if (!General.GetNullableDecimal(maxscore).HasValue)
        {
            ucError.ErrorMessage = "Max score for the subject is required.";
        }
        if (!General.GetNullableDecimal(passscore).HasValue)
        {
            ucError.ErrorMessage = "Pass score for the subject is required.";
        }
        if (!General.GetNullableDecimal(totalclass).HasValue)
        {
            ucError.ErrorMessage = "Total class hour for the subject is required.";
        }

        return (!ucError.IsError);
    }
}

