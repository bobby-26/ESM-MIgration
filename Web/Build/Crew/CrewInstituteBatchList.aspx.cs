using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewInstituteBatchList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteBatchList.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBatchList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteBatchList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteBatchList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteBatchAdd.aspx?" + Request.QueryString, "Add Training Schedule", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuCrewBatchList.AccessRights = this.ViewState;
        MenuCrewBatchList.MenuList = toolbar.Show();
      
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;            
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtCourseId.Attributes.Add("style", "display:none;");
            txtInstituteId.Attributes.Add("style", "display:none;");
            hdncnlStatus.Value = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CNL");            
            hdnErcStatus.Value = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "ERC");
            hdnOpnStatus.Value = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "OPN");
            hdncmpStatus.Value = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP");
            ucBatchStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "OPN");

            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            txtStartDate.Text = thisWeekStart.ToString();
            txtEndDate.Text = thisWeekEnd.ToString();
            gvBatchList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
        } 
    }
    private void BindBatch()
    {
        string courseInstituteId = null;
        if (Request.QueryString["courseInstituteId"] != null)
            courseInstituteId = Request.QueryString["courseInstituteId"].ToString();
        DataTable dt = PhoenixCrewCourseInitiation.CrewCourseInstituteEdit(General.GetNullableGuid(courseInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtCourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtcourseName.Text = dt.Rows[0]["FLDCOURSE"].ToString();
           // txtcourseCode.Text = dt.Rows[0]["FLDABBREVIATION"].ToString();
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();          
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string status = null;

        string[] alColumns = { "FLDROWNUMBER", "FLDBATCHNO", "FLDNAME" };
        string[] alCaptions = { "S.No", "Batch No", "Institute" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (!string.IsNullOrEmpty(ucBatchStatus.SelectedHard) && ucBatchStatus.SelectedHard.ToUpper()!="DUMMY")
            status = ucBatchStatus.SelectedHard;
      
        DataSet ds = PhoenixCrewInstituteBatch.CrewInstituteBatchSearch(General.GetNullableString(txtBatchNoSearch.Text)
                                                                     , General.GetNullableInteger(txtInstituteId.Text)
                                                                     , General.GetNullableInteger(txtCourseId.Text)
                                                                     , General.GetNullableString(txtcourseName.Text)
                                                                     , General.GetNullableString(txtInstituteName.Text)
                                                                     , General.GetNullableDateTime(txtStartDate.Text)
                                                                     , General.GetNullableDateTime(txtEndDate.Text)
                                                                     , General.GetNullableInteger(ucBatchStatus.SelectedHard)
                                                                     , sortexpression
                                                                     , sortdirection
                                                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                     , gvBatchList.PageSize
                                                                     , ref iRowCount
                                                                     , ref iTotalPageCount);
        General.SetPrintOptions("gvBatchList", "Batch List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBatchList.DataSource = ds;
            gvBatchList.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBatchList.DataSource = "";
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
         
            BindData();
            gvBatchList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewBatchList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvBatchList.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDBATCHNO", "FLDNAME" };
            string[] alCaptions = { "S.No", "Batch No", "Institute" };

            string sortexpression, status = null, courseInstituteId = null;
            int? sortdirection = 1;

            if (Request.QueryString["courseInstituteId"] != null)
                courseInstituteId = Request.QueryString["courseInstituteId"].ToString();

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (!string.IsNullOrEmpty(ucBatchStatus.SelectedHard) && ucBatchStatus.SelectedHard.ToUpper() != "DUMMY")
                status = ucBatchStatus.SelectedHard;

            DataSet ds = PhoenixCrewInstituteBatch.CrewInstituteBatchSearch(General.GetNullableString(txtBatchNoSearch.Text)
                                                                     , General.GetNullableInteger(txtInstituteId.Text)
                                                                     , General.GetNullableInteger(txtCourseId.Text)
                                                                     , txtcourseName.Text
                                                                     , txtInstituteName.Text
                                                                     , General.GetNullableDateTime(txtStartDate.Text)
                                                                     , General.GetNullableDateTime(txtEndDate.Text)
                                                                     , General.GetNullableInteger(ucBatchStatus.SelectedHard)
                                                                     , sortexpression
                                                                     , sortdirection
                                                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                     , gvBatchList.PageSize
                                                                     , ref iRowCount
                                                                     , ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Batch List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;       
            txtBatchNoSearch.Text = "";            
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtInstituteId.Text = "";
            txtInstituteName.Text = "";
            ucBatchStatus.SelectedHard = "0";
            gvBatchList.CurrentPageIndex = 0;
            BindData();
            gvBatchList.Rebind();
        }
    }    

    protected void gvBatchList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string batchId;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                //_gridView.SelectedIndex = nCurrentRow;
            }
            else if (e.CommandName.ToString().ToUpper() == "CANCELBATCH")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                batchId = dataItem.GetDataKeyValue("FLDCREWINSTITUTEBATCHID").ToString();
                PhoenixCrewInstituteBatch.CrewInstituteBatchCancel(General.GetNullableGuid(batchId).Value);
                BindData();
                gvBatchList.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ENROLL")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                batchId = dataItem.GetDataKeyValue("FLDCREWINSTITUTEBATCHID").ToString();
                Response.Redirect("../Crew/CrewBatchEnrollment.aspx?batchId=" + batchId, true);
                BindData();
                gvBatchList.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "PLAN")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                batchId = dataItem.GetDataKeyValue("FLDCREWINSTITUTEBATCHID").ToString();
                Response.Redirect("../Crew/CrewInstituteBatchEdit.aspx?batchId=" + batchId);
                BindData();
                gvBatchList.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                batchId = dataItem.GetDataKeyValue("FLDCREWINSTITUTEBATCHID").ToString();
                PhoenixCrewInstituteBatch.CrewInstituteBatchDelete(General.GetNullableGuid(batchId).Value);
                BindData();
                gvBatchList.Rebind();
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

    protected void gvBatchList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatchList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatchList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            string batchId = dataItem.GetDataKeyValue("FLDCREWINSTITUTEBATCHID").ToString();
            
            LinkButton btnEnrollmentCmpt = (LinkButton)e.Item.FindControl("cmdEnrollmentCompleted");
            LinkButton btnEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdCancelBatch = (LinkButton)e.Item.FindControl("cmdCancelBatch");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton cmdAttendance = (LinkButton)e.Item.FindControl("cmdAttendance");
            LinkButton imgAddPlan = (LinkButton)e.Item.FindControl("imgAddPlan");
            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblBatchStatus");
            
            if (cmdCancelBatch != null) cmdCancelBatch.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this batch?')");
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this batch?')");

            if (cmdAttendance != null)
                cmdAttendance.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', 'Batch Attandance', '" + Session["sitepath"] + "/Crew/CrewBatchAttendance.aspx?batchId=" + batchId + "');return false;");
        }
    }
}