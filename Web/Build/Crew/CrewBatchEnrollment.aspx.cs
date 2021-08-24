using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewBatchEnrollment : PhoenixBasePage
{
    public enum AttachmentType
    {
        Feedback
      , Marksheet
      , Certificate

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {          
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Course", "COURSE",ToolBarDirection.Left);
            toolbar.AddButton("Enrollment", "ENROLL", ToolBarDirection.Left);
            toolbar.AddButton("Attendance", "ATTENDANCE", ToolBarDirection.Left);            
            toolbar.AddButton("Back", "LIST", ToolBarDirection.Right);            
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            MenuTitle.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewBatchEnrollment.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEnrollmentList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewBatchEnrollment.aspx?" + Request.QueryString.ToString(), "Add Participant", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            
            MenuBatchEnrollment.AccessRights = this.ViewState;
            MenuBatchEnrollment.MenuList = toolbar.Show();
            
            
            if (!IsPostBack)
            {
                
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;   
                ViewState["CMPSTATUS"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP");
                ViewState["CNLSTATUS"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CNL");

                if(Filter.CurrentBatchFilter!=null)
                {
                    NameValueCollection nvc = Filter.CurrentBatchFilter;
                    ddlbatch.SelectedValue = nvc[0].ToString();
                }
                gvEnrollmentList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            Response.Cache.SetCacheability(HttpCacheability.Private);
            BindCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            //txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
        }
        ddlbatch.CourseInstituteId = courseInstituteId;
        txtcourseInstitute.Text = courseInstituteId;
    }
    private void BindData()
    {
        int irowcount = 0;
        int itotalpagecount = 0;
        string batchId = null;

        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME" , "FLDENROLLEDSTATUS" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank" ,"Status"};

        string sortexpression = (ViewState["sortexpression"] == null) ? null : (ViewState["sortexpression"].ToString());
        int? sortdirection = null;
        if (ViewState["sortdirection"] != null)
            sortdirection = Int32.Parse(ViewState["sortdirection"].ToString());

        if (Filter.CurrentBatchFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentBatchFilter;
            batchId = nvc[0].ToString();
        }
        //if (Request.QueryString["batchId"] != null)
        else if (!string.IsNullOrEmpty(ddlbatch.SelectedValue.ToString()))
            batchId = ddlbatch.SelectedValue.ToString();

        
        DataSet ds = PhoenixCrewBatchEnrollment.CrewBatchEnrollmentSearch(null
                                                                           , General.GetNullableString(txtBatchNo.Text)
                                                                           , General.GetNullableGuid(batchId)
                                                                           , General.GetNullableInteger(txtcourseId.Text).Value
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvEnrollmentList.PageSize
                                                                           , ref irowcount
                                                                           , ref itotalpagecount);

        General.SetPrintOptions("gvEnrollmentList", "Participant List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvEnrollmentList.DataSource = ds;
            gvEnrollmentList.VirtualItemCount = irowcount;
            //if (gvEnrollmentList.SelectedIndex < 0)
            //    gvEnrollmentList.SelectedIndex = 0;
        }
        else
        {
            gvEnrollmentList.DataSource = "";
        }
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvEnrollmentList.Rebind();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }  
    protected void MenuBatchEnrollment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string batchId = null;

            string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDENROLLEDSTATUS" };
            string[] alCaptions = { "S.No", "File No", "Name", "Rank", "Status" };

            string sortexpression;
            int? sortdirection = 1;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (Request.QueryString["batchId"] != null)
                batchId = Request.QueryString["batchId"].ToString();

            DataSet ds = PhoenixCrewBatchEnrollment.CrewBatchEnrollmentSearch(null
                                                                           , General.GetNullableString(txtBatchNo.Text)
                                                                           , General.GetNullableGuid(batchId)
                                                                           , General.GetNullableInteger(txtcourseId.Text).Value
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , General.ShowRecords(null)
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);
            if (ds.Tables.Count > 0)
                General.ShowExcel("Participant List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (ViewState["CMPSTATUS"] != null && ViewState["CMPSTATUS"].ToString() == txtBatchStatus.Text)
            {
                ucError.Text = "Completed batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }
            if (ViewState["CNLSTATUS"] != null && ViewState["CNLSTATUS"].ToString() == txtBatchStatus.Text)
            {
                ucError.Text = "Canceled batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }
          
            Response.Redirect("../Crew/CrewAvailableEmployeeList.aspx?" + Request.QueryString, true);
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
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
            }
            if (CommandName.ToUpper().Equals("ATTENDANCE"))
            {
                Response.Redirect("../Crew/CrewBatchAttendance.aspx?courseInstituteId=" + txtcourseInstitute.Text, true);
                // Response.Redirect("../Crew/CrewInstituteBatchEdit.aspx?batchId=" + batchId + "&instituteId=" + InstituteId + "&courseId=" + CourseId, true);
            }
            if (CommandName.ToUpper().Equals("ENROLL"))
            {
                Response.Redirect("../Crew/CrewBatchEnrollment.aspx?" + Request.QueryString.ToString(), true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            MenuBatchEnrollment.ClearSelection();
        }
    }
  
    private bool IsValidAttachment(string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(General.GetNullableString(type)) || type.ToUpper() == "SELECT")
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }

    protected void ddlbatch_TextChangedEvent(object sender, EventArgs e)
    {
        NameValueCollection nvc= new NameValueCollection();
        nvc.Add("selectedBatch", ddlbatch.SelectedValue);
        Filter.CurrentBatchFilter = nvc;
        BindData();
        gvEnrollmentList.Rebind();
    }

    protected void gvEnrollmentList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            string enrollmentId = eeditedItem.GetDataKeyValue("FLDCREWBATCHENROLLMENTID").ToString();
            //string enollmentId = _gridView.DataKeys[nCurrentRow].Value.ToString();
            string referencedtKey = ((RadLabel)eeditedItem.FindControl("lblReferencedtkey")).Text;
            string status = string.Empty;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //GridEditableItem eeditedItem = e.Item as GridEditableItem;
                //string enrollmentId = eeditedItem.GetDataKeyValue("FLDCREWBATCHENROLLMENTID").ToString();                
                PhoenixCrewBatchEnrollment.CrewBatchEnrollmentDelete(General.GetNullableGuid(enrollmentId).Value);
                BindData();
                gvEnrollmentList.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DISCONTINUE"))
            {
                //string batchId = Request.QueryString["batchId"].ToString();
                string batchId = ddlbatch.SelectedValue;
                PhoenixCrewBatchEnrollment.CrewBatchEnrollmentStatusUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                          , "CNL"
                                                                          , null
                                                                         );
                BindData();
                gvEnrollmentList.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COMPLETED"))
            {
                status = "COMPLETED";
                string empId = ((RadLabel)eeditedItem.FindControl("lblEmpId")).Text;
                string courseId = txtcourseId.Text;
                PhoenixCrewBatchEnrollment.CrewBatchEnrollmentStatusUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                             , status
                                                                             , null
                                                                             );

                //updating recommended status completed 
                PhoenixCrewRecommendCourse.CrewRecommendedCourseStatusUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                            , 1);
                BindData();
                gvEnrollmentList.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("INCOMPLETED"))
            {
                status = "ENROLLED";
                PhoenixCrewBatchEnrollment.CrewBatchEnrollmentStatusUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                            , status
                                                                            , null);
                //updating recommended status not completed
                PhoenixCrewRecommendCourse.CrewRecommendedCourseStatusUpdate(General.GetNullableGuid(enrollmentId).Value
                                                                            , 0);
                BindData();
                gvEnrollmentList.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EMPCOURSE"))
            {
                string lblEmpId = ((RadLabel)eeditedItem.FindControl("lblEmpId")).Text;
                string courseId = txtcourseId.Text;
                //string batchId = Request.QueryString["batchId"].ToString();
                string batchId = ddlbatch.SelectedValue;
                string instituteId = txtInstituteId.Text;
                string dtKey = ((RadLabel)eeditedItem.FindControl("lbldtKey")).Text;

                PhoenixCrewBatchEnrollment.CrewMoveToEmployeeCourse(General.GetNullableGuid(enrollmentId).Value
                                                                    , General.GetNullableInteger(courseId).Value
                                                                    , General.GetNullableInteger(lblEmpId).Value
                                                                    , General.GetNullableInteger(instituteId).Value
                                                                    , General.GetNullableGuid(batchId).Value
                                                                    , General.GetNullableGuid(dtKey).Value);

                ucStatus.Text = "Certificates moved successfully.";
                BindData();
                gvEnrollmentList.Rebind();
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

    protected void gvEnrollmentList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEnrollmentList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnrollmentList_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton cmdCanel = (LinkButton)e.Item.FindControl("cmdCanel");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAttachment");
            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            LinkButton cmdComplete = (LinkButton)e.Item.FindControl("cmdComplete");
            LinkButton cmdInComplete = (LinkButton)e.Item.FindControl("cmdInComplete");
            LinkButton cmdMovePersonal = (LinkButton)e.Item.FindControl("cmdMovePersonal");
            LinkButton cmdPlan = (LinkButton)e.Item.FindControl("cmdPlan");
            LinkButton cmdReMovePersonal = (LinkButton)e.Item.FindControl("cmdReMovePersonal");
            string status = ((RadLabel)e.Item.FindControl("lblStatusShort")).Text;

            if (cmdDelete != null)
            {
                if (ViewState["CMPSTATUS"] != null && ViewState["CMPSTATUS"].ToString() == txtBatchStatus.Text)
                    cmdDelete.Visible = false;

                if (ViewState["CNLSTATUS"] != null && ViewState["CNLSTATUS"].ToString() == txtBatchStatus.Text)
                    cmdDelete.Visible = false;

                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
            }
            if (cmdCanel != null)
            {
                cmdCanel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to discontinue this seafarer?')");
                cmdCanel.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewBatchDiscontinueRemarks.aspx?enrollmentId=" + drv["FLDCREWBATCHENROLLMENTID"].ToString() + "','medium'); return false;");
                cmdCanel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCanel.CommandName);
            }
            if (att != null)
            {
                string batchId = null;

                if (Filter.CurrentBatchFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentBatchFilter;
                    batchId = nvc[0].ToString();
                }
                //if (Request.QueryString["batchId"] != null)
                else if (!string.IsNullOrEmpty(ddlbatch.SelectedValue.ToString()))
                    batchId = ddlbatch.SelectedValue.ToString();

                if(att!=null)
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewBatchEvaluationAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&empId=" + drv["FLDEMPLOYEEID"].ToString() +
                                                                "&enrollmentId=" + drv["FLDCREWBATCHENROLLMENTID"].ToString() +
                                                                "&batchId=" + batchId +
                                                                "&" + Request.QueryString + "'); return false;");
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (!string.IsNullOrEmpty(drv["FLDISATTACHMENT"].ToString()))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    else
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);                        
                    }
                }
               
            }
            if (lnkName != null)
            {
                lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&course=1'); return false;");
            }

            if (status == "ENR")
            {
                cmdComplete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure seafarer completed the course?')");
                cmdComplete.Visible = true;
            }
           
            if (status == "COMPLETED")
            {
                cmdInComplete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to rollback to enrolled status ?')");
                cmdInComplete.Visible = true;
                cmdDelete.Visible = false;
                cmdMovePersonal.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move the certificate to personal record ?')");
                cmdMovePersonal.Visible = true;                  
            }

            if (status == "MOVEMPCOUR")
            {
                cmdComplete.Visible = false;
                cmdInComplete.Visible = false;
                cmdCanel.Visible = false;
                cmdDelete.Visible = false;
                //cmdReMovePersonal.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to remove the certificate from personal record ?')");
                //cmdReMovePersonal.Visible = true;
            }

            if (cmdPlan != null)
            {
                string empId = ((RadLabel)e.Item.FindControl("lblEmpId")).Text;
                cmdPlan.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewEmployeePlanner.aspx?empId=" + empId + "');return false;");
            }
           // e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            // when mouse leaves the row, change the bg color to its original value   
            //e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
        }
    }
}

