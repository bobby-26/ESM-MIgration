using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaWeeklyPlannerMakeCopy : PhoenixBasePage
{

    #region :   Page load and render Events :

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ucSemester.Enabled = false;
                PlanDetails.Visible = false;

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Weekly Planner", "WEEKPLAN");
                toolbarmain.AddButton("Copy Plan", "COPY");

                PreSeaMenu.AccessRights = this.ViewState;
                PreSeaMenu.MenuList = toolbarmain.Show();
                PreSeaMenu.SelectedMenuIndex = 1;

                PhoenixToolbar toolbar = new PhoenixToolbar();

                toolbar.AddButton("Make Copy", "COPY");
                MainMenuPreseaWeekPlanner.AccessRights = this.ViewState;
                MainMenuPreseaWeekPlanner.MenuList = toolbar.Show();

                ddlPlanned.Items.Clear();
                ddlUnPlanned.Items.Clear();
                ListItem li = new ListItem("--Select--", "DUMMY");

                ddlPlanned.Items.Add(li);
                ddlPlanned.DataBind();
                ddlUnPlanned.Items.Add(li);
                ddlUnPlanned.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Methods :

    private void BindPlanDetails()
    {

        try
        {
            DataTable dt =  PhoenixPreSeaWeeklyPlannerMakeCopy.ListDayPlanDetails(new Guid(ddlPlanned.SelectedValue));

            if (dt.Rows.Count > 0)
            {
                gvPlanDetails.DataSource = dt;
                gvPlanDetails.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPlanDetails);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCopy()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlPlanned.SelectedValue) == null)
            ucError.ErrorMessage = "Planned Date is required";
        if (General.GetNullableString(ddlUnPlanned.SelectedValue) == null)
            ucError.ErrorMessage = "Copy to Date is required";
     
        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        if (dt.Rows.Count <= 0)
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

    private void BindPlannedDates()
    {
        ddlPlanned.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlPlanned.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlannerMakeCopy.ListPlannedDaysinWeek(int.Parse(ucBatch.SelectedBatch)
                                                                                , int.Parse(ucSemester.SelectedSemester)
                                                                                , int.Parse(ddlSection.SelectedValue)  
                                                                                , int.Parse(ucWeek.SelectedWeek));

        ddlPlanned.DataSource = dt;
        ddlPlanned.DataBind();

    }

    private void BindUnPlannedDates()
    {
        ddlUnPlanned.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlUnPlanned.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlannerMakeCopy.ListUnPlannedDaysinWeek(int.Parse(ucWeek.SelectedWeek));

        ddlUnPlanned.DataSource = dt;
        ddlUnPlanned.DataBind();

    }

    private void BindSection()
    {
        ddlSection.Items.Clear();

        DataSet ds = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(ucBatch.SelectedBatch));
        if (ds.Tables.Count > 0)
        {
            ListItem li = new ListItem("--Select--", "DUMMY");

            ddlSection.Items.Add(li);
            ddlSection.DataBind();

            ddlSection.DataSource = ds.Tables[0];
            ddlSection.DataBind();
        }
    }

    #endregion

    # region :  Dropdown Change events  :

    protected void Batch_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ucSemester.Enabled = true;
            ucSemester.Batch = ucBatch.SelectedBatch;
            ucSemester.bind();
            BindSection();
        }
    }

    protected void Semester_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucSemester.SelectedSemester).HasValue)
        {
            ucWeek.Batch = ucBatch.SelectedBatch;
            ucWeek.Semester = ucSemester.SelectedSemester;
        }

    }

    protected void Week_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucWeek.SelectedWeek).HasValue)
        {
            BindPlannedDates();
            BindUnPlannedDates();
        }

    }

    protected void ddlPlanned_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableString(ddlPlanned.SelectedValue) != null)
        {
            PlanDetails.Visible = true;
            lblPlanDate.Text = Convert.ToDateTime(ddlPlanned.SelectedItem.Text).ToShortDateString();
            BindPlanDetails();
        }
        else
        {
            PlanDetails.Visible = false;
        }
    }

    # endregion

    # region :  Tabstrip Events :

    protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("WEEKPLAN"))
        {
            Response.Redirect("../PreSea/PreSeaWeeklyPlanner.aspx", false);
        }
        else if (dce.CommandName.ToUpper().Equals("COPY"))
        {
            Response.Redirect("../PreSea/PreSeaWeeklyPlannerMakeCopy.aspx", false);
        }

    }

    protected void MainMenuPreseaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                if (!IsValidCopy())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPreSeaWeeklyPlannerMakeCopy.CopyWeeklyPlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ddlPlanned.SelectedValue)
                                                                , int.Parse(ucBatch.SelectedBatch)
                                                                , int.Parse(ucSemester.SelectedSemester)
                                                                , int.Parse(ddlSection.SelectedValue)
                                                                , int.Parse(ucWeek.SelectedWeek)
                                                                , DateTime.Parse(ddlUnPlanned.SelectedValue));
  
                ucStatus.Text = "Copied Successfully.";
            }

            BindPlannedDates();
            BindUnPlannedDates();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

}
