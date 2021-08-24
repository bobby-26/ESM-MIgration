using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewTrainingSchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarHead = new PhoenixToolbar();
        toolbarHead.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarHead.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarHead.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingSchedule.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewRecommendedCourseFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingSchedule.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

        MenuGrid.AccessRights = this.ViewState;
        MenuGrid.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COURSEID"] = null;

            txtInstituteId.Attributes.Add("style", "display:none");
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
            gvCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
        setCourse();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDABBREVIATION", "FLDCOURSE", "FLDCOUNT", "FLDENROLLED" };
        string[] alCaptions = { "S.No", "Course Code", "Course", "Recommended", "Enrolled" };

        NameValueCollection nvc = Filter.CurrentRecommendedCourseFilter;


        DataTable dt = PhoenixCrewRecommendCourse.CrewRecommendedCourseSearch(null
                                                                            , nvc != null ? nvc.Get("txtcourseName") : null
                                                                            , nvc != null ? nvc.Get("txtcourseCode") : null
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvCourse.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                           );
        DataSet ds = new DataSet();
        DataTable newdt = dt.Copy();
        ds.Tables.Add(newdt);
        General.SetPrintOptions("gvCourse", "Recommended Courses", alCaptions, alColumns, ds);

        gvCourse.DataSource = dt;
        gvCourse.VirtualItemCount = iRowCount;

        if (gvCourse.MasterTableView.Items.Count > 0)
            gvCourse.MasterTableView.Items[0].Selected = true;
    }

    private void setCourse()
    {
        if (ViewState["COURSEID"] != null)
        {
            int courseId = General.GetNullableInteger(ViewState["COURSEID"].ToString()).Value;
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseId);
            if (ds.Tables.Count > 0)
            {
                txtCourseId.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
                txtCourseCode.Text = ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();
                txtCourseName.Text = ds.Tables[0].Rows[0]["FLDABBREVIATION"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();
            }
        }
    }

    protected void MenuCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TRAINING"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleList.aspx", true);
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDABBREVIATION", "FLDCOURSE", "FLDCOUNT", "FLDENROLLED" };
            string[] alCaptions = { "S.No", "Course Code", "Course", "Recommended", "Enrolled" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentRecommendedCourseFilter;
            DataTable dt = PhoenixCrewRecommendCourse.CrewRecommendedCourseSearch(null
                                                                               , nvc != null ? nvc.Get("txtcourseName") : null
                                                                               , nvc != null ? nvc.Get("txtcourseCode") : null
                                                                               , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                               , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                               , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                               , gvCourse.PageSize
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount
                                                                              );

            if (dt.Rows.Count > 0)
                General.ShowExcel("Recommended Courses", dt, alColumns, alCaptions, null, null);

        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvCourse.CurrentPageIndex = 0;
            txtCourseName.Text = "";
            Filter.CurrentRecommendedCourseFilter.Clear();
            BindData();
            gvCourse.Rebind();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            if (nvc != null)
            {
                txtInstituteId.Text = nvc[1];
                txtInstituteName.Text = nvc[2];
            }

            //ViewState["COURSEID"] = null;
            BindData();
            gvCourse.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCourse())
                {
                    ucError.Visible = true;
                    return;
                }
                string courseInstituteId = null;
                PhoenixCrewCourseInitiation.CrewCourseInstituteInsert(General.GetNullableInteger(txtCourseId.Text)
                                                                      , General.GetNullableInteger(txtInstituteId.Text)
                                                                      , General.GetNullableInteger(txtDuration.Text)
                                                                      , General.GetNullableInteger(txtMinParticipant.Text)
                                                                      , General.GetNullableInteger(txtMaxParticipant.Text)
                                                                      , ref courseInstituteId);
                ucStatus.Text = "Course scheduled successfully.";
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + courseInstituteId, true);
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("..\\Crew\\CrewTrainingScheduleList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidCourse()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtCourseName.Text))
            ucError.ErrorMessage = "Course is required.";

        if (string.IsNullOrEmpty(txtInstituteId.Text))
            ucError.ErrorMessage = "Institute is required.";

        if (string.IsNullOrEmpty(txtDuration.Text))
            ucError.ErrorMessage = "Duration is required.";

        if (!string.IsNullOrEmpty(txtMinParticipant.Text) && !string.IsNullOrEmpty(txtMaxParticipant.Text))
        {
            if (General.GetNullableInteger(txtMaxParticipant.Text).Value < General.GetNullableInteger(txtMinParticipant.Text).Value)
                ucError.ErrorMessage = "Minimum participant should be lesser then maximum participant";
        }
        return (!ucError.IsError);
    }


    protected void lblCount_Click(object sender, EventArgs e)
    {
        GridDataItem item = (GridDataItem)(sender as LinkButton).NamingContainer;
        int rowIndex = item.RowIndex-2;
        string courseId = ((RadLabel)gvCourse.Items[rowIndex].FindControl("lblDocumentId")).Text;
        string scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewCourseRequestedList.aspx?courseId=" + courseId + " ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", scriptpopup, true);
    }

    protected void gvCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                ViewState["COURSEID"] = ((RadLabel)eeditedItem.FindControl("lblDocumentId")).Text;
                txtCourseId.Text = ((RadLabel)eeditedItem.FindControl("lblDocumentId")).Text;
                txtCourseName.Text = ((LinkButton)eeditedItem.FindControl("lblCourseName")).Text;

                BindData();
                gvCourse.Rebind();
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

    protected void gvCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourse.CurrentPageIndex + 1;
            BindData();
            setCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (gvCourse.Items.Count > 0)
            {
                gvCourse.MasterTableView.Items[0].Selected = true;
                GridDataItem dataItem = gvCourse.SelectedItems[0] as GridDataItem;
                if (ViewState["COURSEID"] == null)
                    ViewState["COURSEID"] = ((RadLabel)dataItem.FindControl("lblDocumentId")).Text;
            }

        }
    }
}
