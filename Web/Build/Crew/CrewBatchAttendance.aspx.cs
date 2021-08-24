using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewBatchAttendance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewBatchAttendance.aspx?" + Request.QueryString, "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Crew/CrewBatchAttendance.aspx?" + Request.QueryString, "Clear Filter", "clear-filter.png", "CLEAR");
        MenuAttendanceList.AccessRights = this.ViewState;
        MenuAttendanceList.MenuList = toolbar.Show();
        MenuAttendanceList.Visible = false;

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Course", "COURSE", ToolBarDirection.Left);
        toolbar.AddButton("Enrollment", "ENROLL", ToolBarDirection.Left);
        toolbar.AddButton("Attendance", "ATTENDANCE", ToolBarDirection.Left);
        toolbar.AddButton("Back", "LIST", ToolBarDirection.Right);

        MenuBatchAttendance.AccessRights = this.ViewState;
        MenuBatchAttendance.MenuList = toolbar.Show();


        if (!IsPostBack)
        {
            MenuBatchAttendance.SelectedMenuIndex = 2;
            BindCourse();
            ViewState["CMPSTATUS"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP");
            BindAttandanceDate();
            lblAllPresent.Text = "Mark All Present On " + General.GetDateTimeToString(DateTime.Now);
        }
        ViewState["EDIT"] = "0";
        BindAttendanceList();
        Guidlines();
    }
    private void BindCourse()
    {
        string courseInstituteId = null;
        if (Request.QueryString["courseInstituteId"] != null)
            courseInstituteId = Request.QueryString["courseInstituteId"].ToString();
        DataTable dt = PhoenixCrewCourseInitiation.CrewCourseInstituteEdit(General.GetNullableGuid(courseInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtcourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtCourse.Text = dt.Rows[0]["FLDABBREVIATION"].ToString() + "-" + dt.Rows[0]["FLDCOURSE"].ToString();
            txtInstitute.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
        }
        ddlbatch.CourseInstituteId = courseInstituteId;
        txtcourseInstitute.Text = courseInstituteId;
    }
    private void BindBatch()
    {

        string batchId = null;
        if (Request.QueryString["batchId"] != null)
            batchId = Request.QueryString["batchId"].ToString();
    }

    private void BindAttandanceDate()
    {
        string batchId = ddlbatch.SelectedValue;
        DataSet ds = PhoenixCrewBatchAttendance.CrewBatchAttendanceDaysList(General.GetNullableGuid(batchId));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds.Tables[0];
                ddlMonth.DataTextField = ds.Tables[0].Columns["FLDMONTH"].ToString();
                ddlMonth.DataBind();
                ddlMonth.Items.Insert(0, "--Select--");
                ddlMonth.FindItemByText(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month)).Selected = true;

            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddlYear.DataSource = ds.Tables[1];
                ddlYear.DataTextField = ds.Tables[1].Columns["FLDYEAR"].ToString();
                ddlYear.DataBind();
                ddlYear.Items.Insert(0, "--Select--");
                ddlYear.FindItemByText(DateTime.Now.Year.ToString()).Selected = true;
            }
        }
    }
    protected void MenuBatchAttendance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("ENROLL"))
            {
                Response.Redirect("../Crew/CrewBatchEnrollment.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
            }
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
            }
            if (CommandName.ToUpper().Equals("ATTENDANCE"))
            {
                Response.Redirect("../Crew/CrewBatchAttendance.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAttendance()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ddlMonth.Text) || ddlMonth.SelectedIndex == 0)
        {
            ucError.ErrorMessage = "Month is required";
        }

        if (string.IsNullOrEmpty(ddlYear.Text) || ddlYear.SelectedIndex == 0)
        {
            ucError.ErrorMessage = "Year is required";
        }
        return (!ucError.IsError);
    }
    private void Btn_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            GridDataItem Item = btn.NamingContainer as GridDataItem;

            if (PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP") == txtBatchStatus.Text)
            {
                ucError.Text = "Completed batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }

            if (PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CNL") == txtBatchStatus.Text)
            {
                ucError.Text = "Canceled batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }

            if (!IsValidAttendance())
            {
                ucError.Visible = true;
                return;
            }
            string enrollmentId = ((RadLabel)Item.FindControl("lblEnrollmentId")).Text;
            string attendanceDate = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[1]).Text;
            string attendanceId = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[2]).Text;
            int attendanceyn = 0;

            if (string.IsNullOrEmpty(attendanceId))
            {
                PhoenixCrewBatchAttendance.CrewBatchAttendanceInsert(General.GetNullableGuid(enrollmentId).Value
                                                                     , General.GetNullableDateTime(attendanceDate)
                                                                     , 1);
            }
            else
            {
                attendanceyn = btn.Text == "A" ? 1 : btn.Text == "P" ? 2 : btn.Text == "LM" ? 0 : 0;
                if (attendanceyn == 1)
                    PhoenixCrewBatchAttendance.CrewBatchAttendanceDelete(General.GetNullableGuid(attendanceId).Value);

                PhoenixCrewBatchAttendance.CrewBatchAttendanceUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                     , General.GetNullableDateTime(attendanceDate)
                                                                     , attendanceyn
                                                                     , General.GetNullableGuid(attendanceId).Value);
            }
            ViewState["EDIT"] = "1";
            BindAttendanceList();
            gvAttendance.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void btnAttendanceyn_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BindAttendanceList()
    {
        try
        {

            string batchId = ddlbatch.SelectedValue;

            if (ddlMonth.SelectedIndex == 0)
            {
                ucError.ErrorMessage = "Month is required";
                ucError.Visible = true;
                return;
            }

            if (ddlYear.SelectedIndex == 0)
            {
                ucError.ErrorMessage = "Year is required";
                ucError.Visible = true;
                return;
            }

            DataSet ds = PhoenixCrewBatchAttendance.CrewBatchAttendanceDayWise(General.GetNullableGuid(batchId)
                                                                               , ddlMonth.SelectedItem.Text
                                                                               , General.GetNullableInteger(ddlYear.SelectedItem.Text).Value
                                                                               , General.GetNullableInteger(txtcourseId.Text).Value);

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                //adding columns dynamically
                if (ViewState["EDIT"].ToString() != "1")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridBoundColumn field = new GridBoundColumn();
                        field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDDAY"].ToString());
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        //field.HeaderStyle.Width = Unit.Percentage(100);
                        field.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Pixel(30);
                        field.Resizable = false;

                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        gvAttendance.Columns.Insert(gvAttendance.Columns.Count, field);
                    }
                }

                gvAttendance.DataSource = ds;
                ViewState["EDIT"] = "1";
            }
            else
                gvAttendance.DataSource = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAttendanceList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            BindAttendanceList();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            ddlMonth.SelectedIndex = 1;
            ddlYear.SelectedIndex = 1;
            BindAttendanceList();
        }
    }
    private void PrepareGridViewForExport(Control gridView)
    {
        for (int i = 0; i < gridView.Controls.Count; i++)
        {
            //Get the control
            Control currentControl = gridView.Controls[i];
            if (currentControl is LinkButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
            }
            else if (currentControl is ImageButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
            }
            else if (currentControl is HyperLink)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
            }
            else if (currentControl is DropDownList)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
            }
            else if (currentControl is CheckBox)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
            }
            if (currentControl.HasControls())
            {
                // if there is any child controls, call this method to prepare for export
                PrepareGridViewForExport(currentControl);
            }
        }
    }
    protected void MenuAttendanceExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                BindAttendanceList();
                PrepareGridViewForExport(gvAttendance);
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=Attandance-" + txtBatchNo.Text + ".xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvAttendance.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string batchId = Request.QueryString["batchId"].ToString();
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                BindAttendanceList();
                string[] nonaggcol = { "File No", "Name", "Rank" };

                DataSet ds = PhoenixCrewBatchAttendance.CrewBatchAttendanceDayWise(General.GetNullableGuid(batchId)
                                                                              , General.GetNullableString(ddlMonth.SelectedValue)
                                                                              , General.GetNullableInteger(ddlYear.SelectedValue).Value
                                                                              , General.GetNullableInteger(txtcourseId.Text).Value);

                int a = Convert.ToInt32(nonaggcol.Length.ToString());
                int b = Convert.ToInt32(ds.Tables[1].Rows.Count.ToString());

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=Attendance.xls");
                Response.Charset = "";

                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td colspan=" + Convert.ToString(a + b) + " align='center'><h3>Candidate's Attendance Sheet</h3></td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td colspan='6'><b>Course : </b>" + txtCourse.Text + " </td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td colspan='3'><b>Batch No :</b>" + txtBatchNo.Text + " </td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td colspan='3'><b>Month :</b>" + ddlMonth.SelectedValue + " </td>");
                Response.Write("<td colspan='3'><b>Year :</b>" + ddlYear.SelectedValue + " </td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");

                System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                stringwriter1.Write("<table><tr><td colspan=\"" + gvAttendance.Columns.Count + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter1 = new HtmlTextWriter(stringwriter1);
                gvAttendance.RenderControl(htmlwriter1);
                Response.Write(stringwriter1.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkAllPresent_CheckedChanged(object sender, EventArgs e)
    {
        int count = gvAttendance.Items.Count;
        if (chkAllPresent.Checked == true)
        {
            for (int i = 0; i < count; i++)
            {
                string enrollmentId = ((RadLabel)gvAttendance.Items[i].FindControl("lblEnrollmentId")).Text;
                PhoenixCrewBatchAttendance.CrewBatchAttendanceAllPresent(General.GetNullableGuid(enrollmentId).Value
                                                                        , General.GetNullableDateTime(DateTime.Now.ToShortDateString()).Value);
            }
        }
        ViewState["EDIT"] = "1";
        BindAttendanceList();

    }

    protected void ddlbatch_TextChangedEvent(object sender, EventArgs e)
    {
        BindAttendanceList();
        ViewState["EDIT"] = "1";
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. To mark attendance click once.</td></tr><tr><td>&nbsp;2. To mark late mark click twice.</td> </tr><tr><td>&nbsp;3. To mark absent click thrice.</td></tr><tr><td>&nbsp;4. To delete attendance click four times.</td></tr></tr></table>";
    }

    protected void gvAttendance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttendance.CurrentPageIndex + 1;
            BindAttendanceList();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttendance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            DataSet ds = (DataSet)gv.DataSource;

            if (drv.Row.Table.Columns.Count > 0)
            {
                string empId = drv["FLDEMPLOYEEID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                DataTable course = ds.Tables[3];

                for (int i = 5; i < header.Rows.Count + 5; i++)
                {
                    DataRow[] dr = data.Select("FLDEMPLOYEEID = " + empId +
                                                " AND FLDATTENDANCEDATE = '" + header.Rows[i - 5]["FLDDATE"].ToString() + "'");

                    DataRow[] drCourse = course.Select("FLDDATE = '" + header.Rows[i - 5]["FLDDATE"].ToString() + "'");

                    LinkButton btn = new LinkButton();
                    btn.Text = (dr.Length > 0 && dr[0]["FLDATTENDANCEYN"].ToString() == "1" ? "P" :
                                                 dr.Length > 0 && dr[0]["FLDATTENDANCEYN"].ToString() == "0" ? "A" :
                                                 dr.Length > 0 && dr[0]["FLDATTENDANCEYN"].ToString() == "2" ? "LM" : "-");

                    //mark course planned dates
                    if (drCourse.Length > 0)
                    {
                        btn.Attributes.Add("style", "border-bottom:3px solid blue;");

                    }
                    e.Item.Cells[i].Controls.Add(btn);
                    btn.ID = "b" + e.Item.Cells.GetCellIndex(e.Item.Cells[i]).ToString();
                    btn.Click += Btn_Click;
                    btn.Width = System.Web.UI.WebControls.Unit.Pixel(10);
                    btn.Style.Add("Width", "10px");
                    RadLabel lblDate = new RadLabel();
                    lblDate.Text = header.Rows[i - 5]["FLDDATE"].ToString();
                    lblDate.ID = "lbl" + e.Item.Cells.GetCellIndex(e.Item.Cells[i]);
                    lblDate.Visible = false;
                    e.Item.Cells[i].Controls.Add(lblDate);

                    RadLabel lblBatchAttendanceId = new RadLabel();
                    lblBatchAttendanceId.ID = "lblBatchAttendanceId" + e.Item.Cells.GetCellIndex(e.Item.Cells[i]);
                    lblBatchAttendanceId.Visible = false;
                    e.Item.Cells[i].Controls.Add(lblBatchAttendanceId);
                    if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["FLDCREWBATCHATTENDANCEID"].ToString()))
                        lblBatchAttendanceId.Text = dr[0]["FLDCREWBATCHATTENDANCEID"].ToString();
                }
            }
        }
    }
}
            
            