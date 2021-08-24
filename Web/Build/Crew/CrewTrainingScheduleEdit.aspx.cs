using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewTrainingScheduleEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarHead = new PhoenixToolbar();
        toolbarHead.AddButton("Course", "COURSE", ToolBarDirection.Left);
        toolbarHead.AddButton("Enrollment", "ENROLL", ToolBarDirection.Left);
        toolbarHead.AddButton("Attendance", "ATTENDANCE", ToolBarDirection.Left);       
        toolbarHead.AddButton("Back", "LIST",ToolBarDirection.Right);
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarHead.Show();
        MenuHeader.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarEdit = new PhoenixToolbar();
        toolbarEdit.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuTrainingSchedule.AccessRights = this.ViewState;
        MenuTrainingSchedule.MenuList = toolbarEdit.Show();
        MenuTrainingSchedule.Visible = true;
        if (Request.QueryString["courseInstituteId"] != null && Request.QueryString["courseInstituteId"].ToString() != "")
            lblcourseInstituteId.Text = Request.QueryString["courseInstituteId"].ToString();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
            gvFaculty.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            SetCourse();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewInstituteFaculty.aspx?isEdit=1&instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;", "Add Contact", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuFaculty.AccessRights = this.ViewState;
        MenuFaculty.MenuList = toolbar.Show();
        imgAddPlan.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewInstituteBatchPlanner.aspx?instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;");
        if (txtInstituteName.Text != "")
            imgInstitutecal.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersInstitutionCalender.aspx?instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;");
        else
        {
            ucError.Text = "Institute is required";
            ucError.Visible = true;
        }
        BindFaculty();
    }
    
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
           
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (Request.QueryString["courseInstituteId"] != null)
            //    courseInstituteId = General.GetNullableString(Request.QueryString["courseInstituteId"].ToString());           

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("ENROLL"))
            {
                Response.Redirect("../Crew/CrewBatchEnrollment.aspx?courseInstituteId=" + lblcourseInstituteId.Text, true);
            }
            if (CommandName.ToUpper().Equals("ATTENDANCE"))
            {
                Response.Redirect("../Crew/CrewBatchAttendance.aspx?courseInstituteId=" + lblcourseInstituteId.Text, true);
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewTrainingSchedule.aspx", true);
            }
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?" + Request.QueryString, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTrainingSchedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            //string courseInstituteId = null;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (Request.QueryString["courseInstituteId"] != null)
            //    courseInstituteId = Request.QueryString["courseInstituteId"].ToString();

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string minParticipant = txtMinParticipant.Text;
                PhoenixCrewCourseInitiation.CrewCourseInstituteUpdate(General.GetNullableInteger(txtCourseId.Text)
                                                                    , General.GetNullableInteger(txtInstituteId.Text)
                                                                    , General.GetNullableInteger(txtDuration.Text)
                                                                    , General.GetNullableInteger(txtMinParticipant.Text)
                                                                    , General.GetNullableInteger(txtMaxParticipant.Text)
                                                                    , General.GetNullableGuid(lblcourseInstituteId.Text).Value);
                SetCourse();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../Crew/CrewInstituteBatchList.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvFaculty_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentrow = de.RowIndex;
           
            string courseContactId = ((Label)_gridView.Rows[nCurrentrow].FindControl("lblCourseContactId")).Text;
            PhoenixCrewInstituteFaculty.CrewInstituteCourseContactDelete(General.GetNullableInteger(courseContactId).Value);
            _gridView.EditIndex = -1;
            SetCourse();
            BindFaculty();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void imgAddPlan_Click(object sender, ImageClickEventArgs e)
    //{
    //    string scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewInstituteBatchPlanner.aspx?instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;");
    //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    //}
    private void BindFaculty()
    {
        //divGrid.Visible = true;       
        DataTable dt = PhoenixCrewInstituteFaculty.CrewCourseContactList(General.GetNullableInteger(txtCourseId.Text).Value                                                                         
                                                                         , General.GetNullableInteger(txtInstituteId.Text).Value);

        if (dt.Rows.Count > 0)
        {
            gvFaculty.DataSource = dt;
            //gvFaculty.VirtualItemCount = iRowCount;
        }
        else
        {
            gvFaculty.DataSource = "";
        }
    }

    private void SetCourse()
    {
        //string courseInstituteId = null;
        //if (Request.QueryString["courseInstituteId"] != null)
        //    courseInstituteId = Request.QueryString["courseInstituteId"].ToString();
        DataTable dt = PhoenixCrewCourseInitiation.CrewCourseInstituteEdit(General.GetNullableGuid(lblcourseInstituteId.Text).Value);
        if (dt.Rows.Count > 0)
        {
            txtCourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtCourseName.Text = dt.Rows[0]["FLDCOURSE"].ToString();
            txtCourseCode.Text = dt.Rows[0]["FLDABBREVIATION"].ToString();
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteId.Text= dt.Rows[0]["FLDINSTITUTEID"].ToString();
            txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
            txtMinParticipant.Text = dt.Rows[0]["FLDMINPARTICIPANT"].ToString();
            txtMaxParticipant.Text = dt.Rows[0]["FLDMAXPARTICIPANT"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        if (nvc != null)
        {
            txtInstituteName.Text = nvc[2].ToString();
            txtInstituteId.Text = nvc[1].ToString();

        }
        BindFaculty();
        gvFaculty.Rebind();
    }

    //protected void imgInstitutecal_Click(object sender, ImageClickEventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(txtInstituteId.Text))
    //    {
    //        string scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersInstitutionCalender.aspx?instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;");
    //        //cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersInstitutionCalender.aspx?instituteId=" + txtInstituteId.Text + "&courseId=" + txtCourseId.Text + "'); return false;");
    //        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    //    }
    //    else
    //    {
    //        ucError.Text = "Institute is required";
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvFaculty_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseContactId = ((RadLabel)eeditedItem.FindControl("lblCourseContactId")).Text;
                PhoenixCrewInstituteFaculty.CrewInstituteCourseContactDelete(General.GetNullableInteger(courseContactId).Value);                
                BindFaculty();
                gvFaculty.Rebind();
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

    protected void gvFaculty_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFaculty.CurrentPageIndex + 1;
            BindFaculty();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFaculty_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string instituteFacultyId = null;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            instituteFacultyId = item.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
            {
                cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewFacultyPlanner.aspx?facultyId=" + instituteFacultyId + "&courseInstituteId=" + lblcourseInstituteId.Text + "'); return false;");
            }
            //cmdEdit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewFacultyPlanner.aspx?facultyId=" + instituteFacultyId + "&courseInstituteId=" + Request.QueryString["courseInstituteId"] + "');return false;");

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the faculty?')");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
        }
       
    }
}