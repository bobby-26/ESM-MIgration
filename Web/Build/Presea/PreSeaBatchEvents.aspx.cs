using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchEvents : PhoenixBasePage
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
                MainToolbar.AddButton("Events", "EVENT");
                MainToolbar.AddButton("Semester", "SEMESTER");
                MainToolbar.AddButton("Subjects", "SUBJECTS");
                MainToolbar.AddButton("Exam", "EXAM");
                //MainToolbar.AddButton("Contact", "CONTACT");

                MenuBatchPlanner.AccessRights = this.ViewState;
                MenuBatchPlanner.MenuList = MainToolbar.Show();

                MenuBatchPlanner.SelectedMenuIndex = 2;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucBatch.SelectedBatch = Filter.CurrentPreSeaBatchManagerSelection;
                ucBatch.Enabled = false;
                BindEvents();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
            {
                ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                ucError.Visible = true;
                return;
            }
            else
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindEvents()
    {
        DataTable dt = PhoenixPreSeaBatchManager.ListBatchEvents(int.Parse(Filter.CurrentPreSeaBatchManagerSelection), null);
        if (dt.Rows.Count > 0)
        {
            gvBatchEvents.DataSource = dt;
            gvBatchEvents.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvBatchEvents);
        }
    }

    protected void gvBatchEvents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindEvents();
    }
    protected void gvBatchEvents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaBatchEvents(((UserControlPreSeaExam)_gridView.FooterRow.FindControl("ucEvents")).SelectedExam
                                            , ((UserControlDate)_gridView.FooterRow.FindControl("txtEventStartAdd")).Text
                                            , ((UserControlDate)_gridView.FooterRow.FindControl("txtEventEndAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaBatchManager.InsertBatchEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             , int.Parse(Filter.CurrentPreSeaBatchManagerSelection)
                                             , int.Parse(((UserControlPreSeaExam)_gridView.FooterRow.FindControl("ucEvents")).SelectedExam)
                                             , General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtEventStartAdd")).Text)
                                             , General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtEventEndAdd")).Text));
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaBatchEvents(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEventIdEdit")).Text
                            , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEventStartEdit")).Text
                            , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEventEndEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPreSeaBatchManager.UpdateBatchEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchEventIdEdit")).Text)
                                                           , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEventIdEdit")).Text)
                                                           , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEventStartEdit")).Text)
                                                           , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEventEndEdit")).Text));
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaBatchManager.DeleteBatchEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchEventId")).Text));
            }
            BindEvents();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatchEvents_RowDataBound(object sender, GridViewRowEventArgs e)
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

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlPreSeaExam uc = (UserControlPreSeaExam)e.Row.FindControl("ucEvents");
            if(uc != null)
            {
                //uc.ExamList = PhoenixPreSeaCourseExam.ListPreSeaCourseExam(General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString()));
                //uc.Course = Session["BATCHMANAGECOURSE"].ToString();
                //uc.ExamList = PhoenixPreSeaCourseExam.ListPreSeaCourseExam(null);
                //uc.Course = Session["BATCHMANAGECOURSE"].ToString();
                //uc.DataBind();

            }

        }
    }
    protected void gvBatchEvents_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvBatchEvents_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindEvents();
    }
    protected void gvBatchEvents_RowUpdating(object sender, GridViewUpdateEventArgs e)
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

    private bool IsValidPreSeaBatchEvents(string eventname, string startdate, string enddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(eventname) == null)
            ucError.ErrorMessage = "Event Name is required.";

        if (General.GetNullableDateTime(startdate) == null)
            ucError.ErrorMessage = "Start date  is required.";

        if (General.GetNullableDateTime(enddate) == null)
            ucError.ErrorMessage = "End date  is required.";

        return (!ucError.IsError);
    }

}
