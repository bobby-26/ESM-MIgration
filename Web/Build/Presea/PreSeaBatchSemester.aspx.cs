using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchSemester : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                PhoenixToolbar MainToolbar = new PhoenixToolbar();

                MainToolbar.AddButton("List", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
                MainToolbar.AddButton("Semester", "SEMESTER");

                MenuBatchPlanner.AccessRights = this.ViewState;
                MenuBatchPlanner.MenuList = MainToolbar.Show();

                MenuBatchPlanner.SelectedMenuIndex = 2;

                PhoenixToolbar subToolbar = new PhoenixToolbar();                

                subToolbar.AddButton("Subjects", "BATCHSUBJECTS");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = subToolbar.Show();
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlCourse.SelectedCourse =Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;

                ucBatch.SelectedBatch = Filter.CurrentPreSeaCourseMasterBatchSelection;
                ucBatch.Enabled = false;
                BindSemesters();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvBatchSemesters_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = e.NewSelectedIndex;
        string BatchSemId = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblBatchSemId")).Text;
        //Filter.CurrentPreSeaCourseMasterBatchSelection = BatchId;
        _gridView.EditIndex = -1;
        BindSemesters(); 
        Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
    }
    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BATCHSUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.ToString();
            ucError.Visible = true;
        }
    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
                DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                if (dce.CommandName.ToUpper().Equals("BATCH"))
                {
                    Response.Redirect("../PreSea/PreSeaBatch.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
                }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSemesters()
    {
        DataTable dt = PhoenixPreSeaBatchManager.ListBatchSemesters(int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection), null);
        if (dt.Rows.Count > 0)
        {
            gvBatchSemesters.DataSource = dt;
            gvBatchSemesters.DataBind();
           
        }
        else
        {
            ShowNoRecordsFound(dt, gvBatchSemesters);
        }
    }

    protected void gvBatchSemesters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindSemesters();

    }
    protected void gvBatchSemesters_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaBatchSemester(((TextBox)_gridView.FooterRow.FindControl("txtSemesterNameAdd")).Text
                                            , ((UserControlDate)_gridView.FooterRow.FindControl("txtStartdateAdd")).Text
                                            , ((UserControlNumber)_gridView.FooterRow.FindControl("txtNoofWeeksAdd")).Text                                            
                                            , ((TextBox)_gridView.FooterRow.FindControl("txtSemesterCodeAdd")).Text                                            
                                            ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaBatchManager.InsertBatchSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             , int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection)
                                             , General.GetNullableInteger(((UserControlNumber)_gridView.FooterRow.FindControl("txtNoofWeeksAdd")).Text)
                                             , General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtStartdateAdd")).Text)
                                             , null
                                             , ((CheckBox)_gridView.FooterRow.FindControl("chkFeedbackAvailableAdd")).Checked == true ? 1 : 0
                                             , ((TextBox)_gridView.FooterRow.FindControl("txtSemesterCodeAdd")).Text
                                             , ((TextBox)_gridView.FooterRow.FindControl("txtSemesterNameAdd")).Text
                                             , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtOrderSequenceAdd")).Text)
                                             );
                BindSemesters();
                ((TextBox)_gridView.FooterRow.FindControl("txtSemesterCodeAdd")).Focus();
                
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaBatchSemester(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterNameEdit")).Text
                            , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtStartdateEdit")).Text
                            , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtNoofWeeksEdit")).Text                            
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterCodeEdit")).Text  
                            ))
                {
                    ucError.Visible = true;
                    return;
                }             

                PhoenixPreSeaBatchManager.UpdateBatchSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSemIdEdit")).Text)                                                           
                                                           , General.GetNullableInteger(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtNoofWeeksEdit")).Text)
                                                           , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtStartdateEdit")).Text)
                                                           , null
                                                           , ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkFeedbackAvaialableEdit")).Checked == true ? 1 : 0
                                                           , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterCodeEdit")).Text
                                                           , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterNameEdit")).Text
                                                           , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderSequenceEdit")).Text)
                                                           );
                _gridView.EditIndex = -1;
                BindSemesters();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblBatchSemIdEdit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSemId"));
                if (lblBatchSemIdEdit != null)
                {
                    PhoenixPreSeaBatchManager.DeleteBatchSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchSemId")).Text));
                }
                BindSemesters();
            }          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatchSemesters_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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

            DataRowView drv = (DataRowView)e.Row.DataItem;

            CheckBox cbfeedback = (CheckBox)e.Row.FindControl("chkFeedbackAvaialableEdit");
            if (cbfeedback != null && drv != null)
                cbfeedback.Checked = drv["FLDFEEDBACKAVAILABLE"].ToString() == "1" ? true : false;

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
    protected void gvBatchSemesters_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
    }
    protected void gvBatchSemesters_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindSemesters();
        ((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtSemesterCodeEdit")).Focus();
    }
    protected void gvBatchSemesters_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidPreSeaBatchSemester(string semester, string startdate, string nofweeks, string semestercode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (string.IsNullOrEmpty(semestercode))
            ucError.ErrorMessage = "Semester Code is required";

        if (string.IsNullOrEmpty(semester))
            ucError.ErrorMessage = "Semester Name is required.";

        if (General.GetNullableDateTime(startdate) == null)
            ucError.ErrorMessage = "Start date  is required.";

        if (General.GetNullableInteger(nofweeks) == null)
            ucError.ErrorMessage = "No of weeks  is required.";      

        return (!ucError.IsError);
    }
    private void SetRowSelection()
    {
        gvBatchSemesters.SelectedIndex = -1;
        for (int i = 0; i < gvBatchSemesters.Rows.Count; i++)
        {
            if (gvBatchSemesters.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaSemesterSelection))
            {
                gvBatchSemesters.SelectedIndex = i;

            }
        }
    }

}
