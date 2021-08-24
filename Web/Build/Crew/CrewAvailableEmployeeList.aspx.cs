using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.Data;
using Telerik.Web.UI;
using Telerik.Web.UI.PivotGrid.Core;

public partial class Crew_CrewAvailableEmployeeList : PhoenixBasePage
{
    
    string strPreviousRowID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewAvailableEmployeeList.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewAvailableEmployeeList.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        
        MenuGrid.AccessRights = this.ViewState;
        MenuGrid.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Course", "COURSE");
        toolbar.AddButton("Attendance", "ATTENDANCE");
        toolbar.AddButton("Enrollment", "ENROLL");
        toolbar.AddButton("Back", "LIST");
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");          
            //txtAvailableFrom.Text = DateTime.UtcNow.Date.ToShortDateString();
            //txtAvailableTo.Text = DateTime.UtcNow.Date.AddMonths(2).ToShortDateString();
            SetBatch();
            if (Filter.CurrentBatchFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentBatchFilter;
                ddlbatch.SelectedValue = nvc[0].ToString();
            }
            gvAvailableEmployee.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }     
    }

    private void SetBatch()
    {
        string courseInstituteId = null;
        //if (Request.QueryString["batchId"] != null)
        //    batchId = Request.QueryString["batchId"].ToString();
        if (Request.QueryString["courseInstituteId"] != null)
            courseInstituteId = Request.QueryString["courseInstituteId"].ToString();

        DataTable dt = PhoenixCrewCourseInitiation.CrewCourseInstituteEdit(General.GetNullableGuid(courseInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtcourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtCourse.Text = dt.Rows[0]["FLDABBREVIATION"].ToString() + "-" + dt.Rows[0]["FLDCOURSE"].ToString();
            txtInstitute.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            txtcourseInstitute.Text = courseInstituteId;
            //txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
        }
        ddlbatch.CourseInstituteId = courseInstituteId;
    }
    
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string batchId = null;
        string courseId = null;

        string[] alColumns = { "FLDFIRSTNAME", "FLDAVAILABLESTARTDATE", "FLDAVAILABLEENDDATE", "FLDPREFERREDLOCATION" };
        string[] alCaptions = { "Employee Name", "Available From", "Available To", "Preferred Location" };

       
        if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
        {
            batchId = ddlbatch.SelectedValue;
            courseId = txtcourseId.Text;
        }
        DataSet ds = PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableSearch( General.GetNullableGuid(batchId)
                                                                                  , General.GetNullableInteger(hdnBatchVenue.Value)
                                                                                  , General.GetNullableInteger(courseId)  
                                                                                  , General.GetNullableDateTime(txtAvailableFrom.Text)
                                                                                  , General.GetNullableDateTime(txtAvailableTo.Text)
                                                                                  , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                  , gvAvailableEmployee.PageSize
                                                                                  , ref iRowCount
                                                                                  , ref iTotalPageCount);

        General.SetPrintOptions("gvAvailableEmployee", "Employee Availability", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strPreviousRowID = string.Empty;
            DataColumn column = new DataColumn();          
            column.ColumnName = "FLDGROUPBY";
            ds.Tables[0].Columns.Add(column);
            for (int i = 0; i< ds.Tables[0].Rows.Count;i++)
            {                
                ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDNAME"].ToString();
                if (ds.Tables[0].Rows[i]["FLDBATCHNO"].ToString() != "")
                    ds.Tables[0].Rows[i]["FLDGROUPBY"] += " ;Enrolled to Batch :" + ds.Tables[0].Rows[i]["FLDBATCHNO"].ToString();
               
            }
            gvAvailableEmployee.DataSource = ds;
            gvAvailableEmployee.VirtualItemCount = iRowCount;
            //cnt = 1;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            gvAvailableEmployee.DataSource = "";
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvAvailableEmployee.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("ATTENDANCE"))
            {
                Response.Redirect("../Crew/CrewBatchAttendance.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
            }
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
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
            if (CommandName.ToUpper().Equals("FIND"))
            {
               // gvAvailableEmployee.EditIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvAvailableEmployee.Rebind();
            }         
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtAvailableFrom.Text = "";
                txtAvailableTo.Text = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvAvailableEmployee.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidBatch(string batch)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(General.GetNullableString(batch)))
            ucError.ErrorMessage = "Batch No is required.";
        
        return (!ucError.IsError);
    }

    protected void gvAvailableEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAvailableEmployee.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAvailableEmployee_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {    
            string batchId = ddlbatch.SelectedValue;

            if (e.CommandName.ToUpper().Equals("ADDTOBATCH"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string empAvailableId = (((HiddenField)eeditedItem.FindControl("hdnempavailabilityDateId")).Value);
                string referencedtkey = (((HiddenField)eeditedItem.FindControl("hdnreferencedtkey")).Value);
                string enrollmentId = (((HiddenField)eeditedItem.FindControl("hdnenrollmentId")).Value);
                string employeeId = (((HiddenField)eeditedItem.FindControl("hdnEmployeeId")).Value);

                if (!IsValidBatch(batchId))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!string.IsNullOrEmpty(enrollmentId))
                {
                    PhoenixCrewBatchEnrollment.CrewMoveEmployeeToAnotherBatch(General.GetNullableGuid(enrollmentId).Value
                                                                             , General.GetNullableGuid(batchId).Value);
                }
                else
                {
                    PhoenixCrewBatchEnrollment.CrewBatchEnrollmentInsert(General.GetNullableGuid(empAvailableId)
                                                                        , General.GetNullableGuid(referencedtkey).Value
                                                                        , General.GetNullableInteger(txtcourseId.Text).Value
                                                                        , General.GetNullableInteger(employeeId).Value
                                                                        , General.GetNullableGuid(batchId).Value);

                }
                BindData();
                gvAvailableEmployee.Rebind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            }
            else if (e.CommandName.ToUpper().Equals("AVAILABILITY"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseInstituteId = Request.QueryString["courseInstituteId"].ToString();
                string empAvailableId = (((HiddenField)eeditedItem.FindControl("hdnEmployeeId")).Value);

                Response.Redirect("../Crew/CrewEmployeeAvailableDate.aspx?empId=" + empAvailableId + "&from=ENROLLLIST&courseInstituteId=" + courseInstituteId, true);
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


    protected void gvAvailableEmployee_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            strPreviousRowID = drv["FLDEMPLOYEEID"].ToString();
            RadLabel lblstartDate = (RadLabel)e.Item.FindControl("lblAvailableStartDate");
            RadLabel lblendDate = (RadLabel)e.Item.FindControl("lblAvailableEndDate");
            RadLabel lblpreferredLocation = (RadLabel)e.Item.FindControl("lblCity");
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgPreferredLocation");
            HtmlImage imgAvailability = (HtmlImage)e.Item.FindControl("imgAvailability");

            if (string.IsNullOrEmpty(drv["FLDEMPLOYEEAVAILABILITYDATESID"].ToString()))
            {
                imgAvailability.Visible = true;
            }
            if (lblstartDate != null && lblendDate != null)
            {
                lblendDate.Visible = false;
                lblstartDate.Text = lblstartDate.Text + " - " + lblendDate.Text;
            }
            if (string.IsNullOrEmpty(lblpreferredLocation.Text))
            {
                img.Visible = true;
            }
        }
        
    }

}