using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchAdmissionSubjects : PhoenixBasePage
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
                Maintoolbar.AddButton("Batch", "BATCH");
                //Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                //Maintoolbar.AddButton("Batch Contact", "CONTACT");
                //Maintoolbar.AddButton("Fees", "FEES");
                //Maintoolbar.AddButton("Semester", "SEMESTER");
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                //Maintoolbar.AddButton("Exam", "EXAM");
                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();

                MenuCourseMaster.SelectedMenuIndex = 2;
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../PreSea/PreSeaBatchAdmissionSubjects.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
                MenuPreSeaCourseSubjects.MenuList = toolbargrid.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;                

                BindData();
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
            //else if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchEligiblityDetails.aspx");
            //}
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchAdmissionContact.aspx");
            //}
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSubjects.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchAdmissionSemester.aspx");
            //}
            //else if (dce.CommandName.ToUpper().Equals("FEES"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchFees.aspx");
            //}
            //else if (dce.CommandName.ToUpper().Equals("EXAM"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchAdmissionExam.aspx");
            //}
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
        string[] alColumns = { "FLDSUBJECTNAME", "FLDSUBJECTTYPE" };
        string[] alCaptions = { "Subject Name", "Type" };
        DataSet ds = PhoenixPreSeaBatchAdmissionSubjects.ListPreSeaApplicableCourseSubject(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                                                                            , (Filter.CurrentPreSeaCourseMasterSelection)); 


        General.SetPrintOptions("gvPreSeaExam", "Pre-Sea Subjects", alCaptions, alColumns, ds);
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
  
        string[] alColumns = { "FLDSUBJECTNAME", "FLDSUBJECTTYPE" };
        string[] alCaptions = { "Subject Name", "Type" };

        ds = PhoenixPreSeaBatchAdmissionSubjects.ListPreSeaApplicableCourseSubject(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                                                                            ,(Filter.CurrentPreSeaCourseMasterSelection));

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSea Course Subjects.xls");
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
        
    }

    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
       
    }

    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
   



    private bool IsValidPreSeaCourseSubjects(string semesterid, string subjectid, string subjectcode, string totalclass)
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
        if (!General.GetNullableDecimal(totalclass).HasValue)
        {
            ucError.ErrorMessage = "Total class hour for the subject is required.";
        }

        return (!ucError.IsError);
    }
}


