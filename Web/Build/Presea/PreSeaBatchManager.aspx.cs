using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchManager : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("Batch", "BATCH");
            MainToolbar.AddButton("Weekly Planner", "WEEKLYPLAN");
            MainToolbar.AddButton("Buddy Planner", "BUDDYPLAN");
            MainToolbar.AddButton("Mentor Planner", "MENTORPLAN");
            MainToolbar.AddButton("Exam Planner", "EXAMPLAN");
            MainToolbar.AddButton("Exam Results", "EXAMRESULTS");
            MainToolbar.AddButton("Semester Planner", "SEMESTERPLAN");
            MainToolbar.AddButton("Semester Results", "SEMEXAMRESULTS");

            MenuBatchPlanner.AccessRights = this.ViewState;
            MenuBatchPlanner.MenuList = MainToolbar.Show();

            MenuBatchPlanner.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                BindCourse();

                if (Session["BATCHMANAGECOURSE"] != null)
                    ddlCourse.SelectedValue = Session["BATCHMANAGECOURSE"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void BindCourse()
    {
        DataTable dt = PhoenixPreSeaCourse.EditPreSeaCourse(null);
        ddlCourse.DataSource = dt;
        ddlCourse.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlCourse.Items.Insert(0, li);        
    }

    private void BindData()
    {
        try
        {
            string course = "";
            if (Session["BATCHMANAGECOURSE"] != null)
                course = Session["BATCHMANAGECOURSE"].ToString();
            DataSet ds = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), 0, 1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBatch.DataSource = ds;
                gvBatch.DataBind();

                gvBatch.SelectedIndex = 0;

                if (String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
                    Filter.CurrentPreSeaBatchManagerSelection = ((Label)gvBatch.Rows[0].FindControl("lblBatchId")).Text;

                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvBatch);
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("MENTORPLAN"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlanner.aspx?type=1");
            }
            if (dce.CommandName.ToUpper().Equals("BUDDYPLAN"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlanner.aspx?type=2");
            }
            if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
            }

            else
            {
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
                {
                    ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (dce.CommandName.ToUpper().Equals("WEEKLYPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchWeeklyPlanner.aspx?TYPE=1");
                    }
                    else if (dce.CommandName.ToUpper().Equals("EXAMRESULTS"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchInternalExamResults.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("EXAMPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("SEMESTERPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchSemesterPlanner.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("SEMEXAMRESULTS"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchSemesterExamResults.aspx");
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

    protected void PreSeaBatch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvBatch_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvBatch.SelectedIndex = se.NewSelectedIndex;
        string BatchId = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblBatchId")).Text;
        Filter.CurrentPreSeaBatchManagerSelection = BatchId;
        BindData();
        Response.Redirect("../PreSea/PreSeaBatchWeeklyPlanner.aspx?TYPE=1");
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlCourse.SelectedValue).HasValue)
        {
            Session["BATCHMANAGECOURSE"] = ddlCourse.SelectedValue;

            if (!String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
                Filter.CurrentPreSeaBatchManagerSelection = null;

            BindData();
        }
    }

    private void SetRowSelection()
    {
        gvBatch.SelectedIndex = -1;
        for (int i = 0; i < gvBatch.Rows.Count; i++)
        {
            if (gvBatch.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaBatchManagerSelection))
            {
                gvBatch.SelectedIndex = i;

            }
        }
    }
}
