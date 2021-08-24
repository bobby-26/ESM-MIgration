using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Crew_CrewInstituteBatchPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarHead = new PhoenixToolbar();
        toolbarHead.AddButton("Show Plan", "SHOW",ToolBarDirection.Right);
        //toolbarHead.AddButton("Generate Batch", "BATCH");
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbarHead.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            btnconfirm.Attributes.Add("style", "display:none");

            txtcourseId.Attributes.Add("style", "display:none;");
            txtInstituteMappingId.Attributes.Add("style", "display:none;");
            txtBatchId.Attributes.Add("style", "display:none;");         
            SetInstitute();
            SetCourse();
            txtplanFrom.Text = DateTime.Now.AddMonths(-2).ToShortDateString();
            txtPlanTo.Text = DateTime.Now.AddMonths(2).ToShortDateString();
            
            gvplan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            
        }      
        BindData();
        Guidlines();
    }

    private void SetInstitute()
    {
        string instituteId = Request.QueryString["instituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(instituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteMappingId.Text = instituteId;
        }
    }
    private void SetCourse()
    {
        string courseId = null;
        if (Request.QueryString["courseId"] != null)
        {
            courseId = Request.QueryString["courseId"].ToString();
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(courseId).Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtcourseId.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();                
                txtCourse.Text = ds.Tables[0].Rows[0]["FLDABBREVIATION"].ToString()+" - "+ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();
            }
        }
    }
   

    protected void gvplan_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //gvplan.SelectedIndex = e.NewSelectedIndex;
        //string month = ((Label)gvplan.Rows[gvplan.SelectedIndex].FindControl("lblMonthNumber")).Text;
       
        //BindData();
    }
 
    private void BindData()
    {
        try
        {
           
            int year,month;

            if (string.IsNullOrEmpty(hdnMonth.Value))
                month = DateTime.Now.Month;
            else
                month = General.GetNullableInteger(hdnMonth.Value).Value;

            if (string.IsNullOrEmpty(hdnYear.Value))
                year = DateTime.Now.Year;           
            else
                year = General.GetNullableInteger(hdnYear.Value).Value;
           
            if (string.IsNullOrEmpty(txtInstituteName.Text) && string.IsNullOrEmpty(txtcourseId.Text))                         
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("FLDMONTH"));
                dt.Columns.Add(new DataColumn("FLDMONTHNUMBER"));
                return;

            }          
            DataSet ds = PhoenixCrewCoursePlanner.CrewCourseCalendarList(General.GetNullableInteger(txtInstituteMappingId.Text).Value
                                                                        , General.GetNullableInteger(txtcourseId.Text).Value
                                                                        , year
                                                                        , month);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string subject = "";
                    DataColumn column = new DataColumn();
                    column.ColumnName = "FLDSUBJECT";
                    ds.Tables[1].Columns.Add(column);
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        if (ds.Tables[1].Rows[i]["FLDCOURSECALENDARID"].ToString() != "")
                            subject = "Course Planned" + Environment.NewLine;
                        if (ds.Tables[1].Rows[i]["FLDFACULTYCOURSEID"].ToString() != "")
                            subject += "Faculty Assigned";
                        ds.Tables[1].Rows[i]["FLDSUBJECT"] = subject;
                    }
                }
                cldBathPlanner1.DataSubjectField = "FLDSUBJECT";
                cldBathPlanner1.DataKeyField = "FLDCOURSECALENDARID";
                cldBathPlanner1.DataStartField = "FLDDATE";              
                cldBathPlanner1.DataEndField = "FLDDATE";                      
                DataView dv = ds.Tables[1].DefaultView;
                cldBathPlanner1.DataSource = ds.Tables[1];
                cldBathPlanner1.DataBind();              
                cldBathPlanner1.SelectedView = SchedulerViewType.MonthView;
                
                //RadScheduler1.SelectedDate = Convert.ToDateTime(dv.Table.Rows[0]["EndDate"]);
                cldBathPlanner1.Visible = true;            
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;           
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("FLDMONTH"));
            dt.Columns.Add(new DataColumn("FLDMONTHNUMBER")); 
        }
    }

    private void BindMonthAndYear()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (!IsValidDate())
        {
            ucError.Visible = true;
           
        }
        DataSet dsPlannedMonths = PhoenixCrewCoursePlanner.CrewCoursePlannedMonthSearch(General.GetNullableInteger(txtInstituteMappingId.Text)
                                                              , General.GetNullableInteger(txtcourseId.Text)
                                                              , General.GetNullableDateTime(txtplanFrom.Text)
                                                              , General.GetNullableDateTime(txtPlanTo.Text)
                                                              , sortexpression
                                                              , sortdirection
                                                              , int.Parse(ViewState["PAGENUMBER"].ToString())                                                                                
                                                              , gvplan.PageSize
                                                              , ref iRowCount
                                                              , ref iTotalPageCount);

        if (dsPlannedMonths.Tables.Count > 0 && dsPlannedMonths.Tables[0].Rows.Count > 0)
        {
            gvplan.DataSource = dsPlannedMonths;
            gvplan.VirtualItemCount = iRowCount;
        }
        else
        {
            gvplan.DataSource = "";
        }
    }
    
    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                ViewState["PAGENUMBER"] = 1;               
                BindData();
                BindMonthAndYear();
                gvplan.Rebind();
            }         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDate()
    {
        ucError.IsError = false;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(General.GetNullableString(txtplanFrom.Text)))
            ucError.ErrorMessage = "From date is required.";

        if (string.IsNullOrEmpty(General.GetNullableString(txtPlanTo.Text)))
            ucError.ErrorMessage = "To date is required.";

        return (!ucError.IsError);
    }

    
    protected void ucConfirm_ConfirmMesage(object sender, EventArgs e)
    {
        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

        if (ucCM.confirmboxvalue == 1)
        {
            string coursecalendarId = hdnCourseCalendarId.Value;
            PhoenixCrewCoursePlanner.CrewCourseCalendarDelete(General.GetNullableGuid(coursecalendarId).Value);
            BindData();
        }            
    }

    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course planned days are highlighted in blue color, Faculty assigned days are highlighted in red color</td></tr><tr><td>&nbsp;2. To plan a course click on the date and to unplan click on the date again.</td> </tr><tr><td>&nbsp;3. When unplan a course faculty assigned also get deleted.</td></tr></tr></table>";        
    }

    protected void gvplan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PLAN"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                hdnMonth.Value = ((RadLabel)eeditedItem.FindControl("lblMonthNumber")).Text;
                hdnYear.Value = ((RadLabel)eeditedItem.FindControl("lblYear")).Text;

                string date = "01/" + hdnMonth.Value + "/" + hdnYear.Value;
                //RadCalendarDay d = new RadCalendarDay();
                //d.Date = Convert.ToDateTime(date);
                BindData();
                cldBathPlanner1.SelectedDate=Convert.ToDateTime(date);
                //cldBathPlanner1.fo = Convert.ToDateTime(date);

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvplan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvplan.CurrentPageIndex + 1;
            BindMonthAndYear();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvplan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            HtmlImage imgPlan = (HtmlImage)e.Item.FindControl("imgPlan");
            if (drv["FLDCOURSEPLANNED"].ToString() == "1")
            {
                imgPlan.Visible = true;
            }
        } 
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        PhoenixCrewCoursePlanner.CrewCourseCalendarDelete(General.GetNullableGuid(hdnCourseCalendarId.Value.ToString()).Value);
        BindData();
    }
    protected void cldBathPlanner1_AppointmentDelete(object sender, AppointmentDeleteEventArgs e)
    {
      try
        {
            string coursecalendarId = e.Appointment.ID.ToString(); 
            DataTable dtbatch = PhoenixCrewInstituteBatch.CrewRunningBatchList(General.GetNullableGuid(coursecalendarId).Value);
            if (dtbatch.Rows.Count > 0)
            {
                hdnCourseCalendarId.Value = coursecalendarId;
                RadWindowManager1.RadConfirm("Enrollment done for the course. Kindly confirm you still wish to unplan?", "btnconfirm", 320, 150, null, "Confirm");
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Enrollment done for the course. Kindly confirm you still wish to unplan.";                
                //return;
            }
            else
            {
                PhoenixCrewCoursePlanner.CrewCourseCalendarDelete(General.GetNullableGuid(coursecalendarId).Value);
            }
          
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void cldBathPlanner1_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {
        try
        {
            if (e.Command == SchedulerNavigationCommand.SwitchToSelectedDay)
            {
                e.Cancel = true;
                string coursecalendarId = null;
                DataTable dt = ((DataTable)cldBathPlanner1.DataSource).DataSet.Tables[0];
                DataRow[] dv = dt.Select("FLDDATE = '" + e.SelectedDate + "'");
                string calendarId = dv[0]["FLDCALENDARID"].ToString();
                PhoenixCrewCoursePlanner.CrewCourseCalendarInsert(General.GetNullableInteger(txtcourseId.Text).Value
                                                                    , General.GetNullableInteger(txtInstituteMappingId.Text).Value
                                                                    , General.GetNullableGuid(calendarId).Value
                                                                    , ref coursecalendarId);
                BindData();
            }
            if (e.Command == SchedulerNavigationCommand.SwitchToMonthView)
            {
                e.Cancel = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cldBathPlanner1_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        if (cldBathPlanner1.SelectedView == SchedulerViewType.MonthView)
        {
            String month = cldBathPlanner1.SelectedDate.Month.ToString();
            if (e.TimeSlot.Start.Date.Month.ToString() != month)
            {                
                e.TimeSlot.Control.Controls.Clear();
            }
        }
    }

    protected void cldBathPlanner1_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {
        RadScheduler scheduler1 = sender as RadScheduler;
        DataTable dt = (DataTable)scheduler1.DataSource;
        DataRow[] dr = dt.Select("FLDCOURSECALENDARID='" + e.Appointment.ID + "'");
        if (dr[0]["FLDFACULTYCOURSEID"].ToString() != "")
            e.Appointment.BackColor = System.Drawing.Color.LightGreen;
        else
            e.Appointment.BackColor = System.Drawing.Color.LightSkyBlue;

    }


    protected void cldBathPlanner1_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
    }
}