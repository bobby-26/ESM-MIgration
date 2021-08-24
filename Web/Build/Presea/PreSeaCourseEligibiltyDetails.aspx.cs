using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseEligibiltyDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Course Contact", "COURSECONTACT");                
                Maintoolbar.AddButton("Fees", "FEES");
                Maintoolbar.AddButton("Semester", "SEMESTER");
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");

                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();

                MenuCourseMaster.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;
                BindElgibilty();
            }
           
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
            else if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            {
                Response.Redirect("../PreSea/PreSeaCourseEligibiltyDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("COURSECONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaCourseContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaCourseSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaCourseFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaCourseExam.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindElgibilty()
    {
        DataTable dt = PhoenixPreSeaCourseMaster.ListCourseEligibility(int.Parse(Filter.CurrentPreSeaCourseMasterSelection), null);
        if (dt.Rows.Count > 0)
        {
            gvPreSea.DataSource = dt;
            gvPreSea.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvPreSea);
        }
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridview = (GridView)sender;
            Label lblcode = (Label)_gridview.Rows[nCurrentRow].FindControl("lblQuickId");

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (lblcode != null)
                {
                    TextBox txtDetails = (TextBox)_gridview.Rows[nCurrentRow].FindControl("txtElgDtls");
                    PhoenixPreSeaCourseMaster.UpdateCourseEligibilty(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(Filter.CurrentPreSeaCourseMasterSelection)
                                                                    , int.Parse(lblcode.Text)
                                                                    , txtDetails.Text);
                    ucStatus.Text = "Eligibility Details saved successfully.";
                }
            }
            BindElgibilty();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

}
