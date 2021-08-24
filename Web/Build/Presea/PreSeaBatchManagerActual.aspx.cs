using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchManagerActual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("Batch", "BATCHACTUAL");
            MainToolbar.AddButton("Weekly Planner Actual", "WEEKLYPLANACTUAL");
            MainToolbar.AddButton("Buddy Planner", "BUDDYPLANACTUAL");
            MainToolbar.AddButton("Mentoring Planner", "MENTORPLANACTUAL");            
            MenuBatchPlanner.AccessRights = this.ViewState;
            MenuBatchPlanner.MenuList = MainToolbar.Show();

            MenuBatchPlanner.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                BindCourse();

                if (Filter.CurrentPreSeaActualCourseSelection != null)
                    ddlCourse.SelectedValue = Filter.CurrentPreSeaActualCourseSelection;
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
            if (Filter.CurrentPreSeaActualCourseSelection != null)
                course = Filter.CurrentPreSeaActualCourseSelection;
            DataSet ds = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), 0, 1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBatch.DataSource = ds;
                gvBatch.DataBind();

                gvBatch.SelectedIndex = 0;

                if (String.IsNullOrEmpty(Filter.CurrentPreSeaActualBatchSelection))
                    Filter.CurrentPreSeaActualBatchSelection = ((Label)gvBatch.Rows[0].FindControl("lblBatchId")).Text;

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
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BATCHACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManagerActual.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BUDDYPLANACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlannerActual.aspx?type=2");
            }
            else if (dce.CommandName.ToUpper().Equals("MENTORPLANACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlannerActual.aspx?type=1");
            }
            else
            {
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaActualBatchSelection))
                {
                    ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                    ucError.Visible = true;
                    return;
                }

                if (dce.CommandName.ToUpper().Equals("WEEKLYPLANACTUAL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchWeeklyPlannerActual.aspx?TYPE=1");
                }
                else if (dce.CommandName.ToUpper().Equals("EXAM"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
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
        Filter.CurrentPreSeaActualBatchSelection = BatchId;
        BindData();
        Response.Redirect("../PreSea/PreSeaBatchWeeklyPlannerActual.aspx?TYPE=1");
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
            Filter.CurrentPreSeaActualCourseSelection = ddlCourse.SelectedValue;

            if (!String.IsNullOrEmpty(Filter.CurrentPreSeaActualBatchSelection))
                Filter.CurrentPreSeaActualBatchSelection = null;

            BindData();
        }
    }

    private void SetRowSelection()
    {
        gvBatch.SelectedIndex = -1;
        for (int i = 0; i < gvBatch.Rows.Count; i++)
        {
            if (gvBatch.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaActualBatchSelection))
            {
                gvBatch.SelectedIndex = i;

            }
        }
    }
}

