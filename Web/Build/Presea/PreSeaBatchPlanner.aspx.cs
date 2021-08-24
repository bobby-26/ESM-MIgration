using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchPlanner : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                BindCourse();

                if (Filter.CurrentPreSeaCourseMasterSelection != null)
                {
                    ddlCourse.SelectedValue = Filter.CurrentPreSeaCourseMasterSelection;
                    ddlCourse.Enabled = false;
                }

                PhoenixToolbar MainToolbar = new PhoenixToolbar();

                MainToolbar.AddButton("Batch", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
                MainToolbar.AddButton("Entrance Exam Plan", "ENTRANCE");

                MenuBatchPlanner.AccessRights = this.ViewState;
                MenuBatchPlanner.MenuList = MainToolbar.Show();

                MenuBatchPlanner.SelectedMenuIndex = 0;
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
            if (Filter.CurrentPreSeaCourseMasterSelection != null)
                course = Filter.CurrentPreSeaCourseMasterSelection;
            DataSet ds = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), 0, 1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBatch.DataSource = ds;
                gvBatch.DataBind();

                gvBatch.SelectedIndex = 0;
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaPlanningBatchSelection))
                    Filter.CurrentPreSeaPlanningBatchSelection = ((Label)gvBatch.Rows[0].FindControl("lblBatchId")).Text;

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
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaPlanningBatchSelection))
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
                    Response.Redirect("../PreSea/PreSeaBatchPlanner.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("ENTRANCE"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanExamDetails.aspx");
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
        Filter.CurrentPreSeaPlanningBatchSelection = BatchId;
        BindData();
        Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
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
            Filter.CurrentPreSeaCourseMasterSelection = ddlCourse.SelectedValue;

            if (!String.IsNullOrEmpty(Filter.CurrentPreSeaPlanningBatchSelection))
                Filter.CurrentPreSeaPlanningBatchSelection = null;

            BindData();
        }
    }

    private void SetRowSelection()
    {
        gvBatch.SelectedIndex = -1;
        for (int i = 0; i < gvBatch.Rows.Count; i++)
        {
            if (gvBatch.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaPlanningBatchSelection))
            {
                gvBatch.SelectedIndex = i;

            }
        }
    }
}
