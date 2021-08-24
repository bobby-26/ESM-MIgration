using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;
public partial class CrewTrainingScheduleList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingScheduleList.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourseList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingScheduleList.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingScheduleList.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Crew/CrewTrainingSchedule.aspx", "Add Training Schedule", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuCrewTrainingList.AccessRights = this.ViewState;
        MenuCrewTrainingList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            //txtCourseId.Attributes.Add("style", "display:none;");
            //txtInstituteId.Attributes.Add("style", "display:none;");
            SetInstitute();
            gvCourseList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDABBREVIATION", "FLDCOURSE", "FLDINSTITUTENAME" };
        string[] alCaptions = { "S.No", "Course Code", "Course", "Institute" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (!string.IsNullOrEmpty(txtInstituteId.Text))
        {
            NameValueCollection nvcSelectedInstitute = new NameValueCollection();
            nvcSelectedInstitute.Add("InstituteId", txtInstituteId.Text);
            Filter.CurrentInstituteFilter = nvcSelectedInstitute;
        }
        DataSet ds = PhoenixCrewCourseInitiation.CrewCourseInstituteSearch(General.GetNullableString(txtcourseName.Text)
                                                                         , General.GetNullableString(txtcourseCode.Text)
                                                                         , General.GetNullableString(txtInstituteName.Text)
                                                                         , General.GetNullableInteger(txtInstituteId.Text)
                                                                         , General.GetNullableInteger(txtCourseId.Text)
                                                                         , null
                                                                         , sortexpression
                                                                         , sortdirection
                                                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                         , gvCourseList.PageSize
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount);
        General.SetPrintOptions("gvCourseList", "Training Schedule", alCaptions, alColumns, ds);

        gvCourseList.DataSource = ds;
        gvCourseList.VirtualItemCount = iRowCount;

    }
    private void SetInstitute()
    {
        string strInstituteId = null;
        NameValueCollection nvc = Filter.CurrentInstituteFilter;
        if (nvc != null)
        {
            strInstituteId = nvc[0].ToString();
            DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(strInstituteId).Value);
            if (dt.Rows.Count > 0)
            {
                txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
                txtInstituteId.Text = strInstituteId;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtInstituteId.Text = nvc[1];
            txtInstituteName.Text = nvc[2];
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingSchedule.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewTrainingList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            if (string.IsNullOrEmpty(txtInstituteName.Text))
            {
                ucError.Text = "Institute name is required";
                ucError.Visible = true;
                return;
            }
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvCourseList.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDABBREVIATION", "FLDCOURSE", "FLDINSTITUTENAME", };
            string[] alCaptions = { "S.No", "Course Code", "Course", "Institute" };

            string sortexpression;
            int? sortdirection = 1;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = PhoenixCrewCourseInitiation.CrewCourseInstituteSearch(General.GetNullableString(txtcourseName.Text)
                                                                                  , General.GetNullableString(txtcourseCode.Text)
                                                                                  , General.GetNullableString(txtInstituteName.Text)
                                                                                  , General.GetNullableInteger(txtInstituteId.Text)
                                                                                  , General.GetNullableInteger(txtCourseId.Text)
                                                                                  , null
                                                                                  , sortexpression
                                                                                  , sortdirection
                                                                                  , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                  , gvCourseList.PageSize
                                                                                  , ref iRowCount
                                                                                  , ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Training Schedule", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            txtInstituteId.Text = "";
            txtInstituteName.Text = "";
            txtcourseCode.Text = "";
            txtcourseName.Text = "";
            txtfromDate.Text = "";
            txtToDate.Text = "";
            Filter.CurrentInstituteFilter = null;
            gvCourseList.CurrentPageIndex = 0;
            BindData();
            gvCourseList.Rebind();
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
    }
    protected void gvCourseList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseInstitueId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOURSEINSTITUTEID"].ToString();
                PhoenixCrewCourseInitiation.CrewCourseInstituteStatusUpdate(0, General.GetNullableGuid(courseInstitueId).Value);
                BindData();
                gvCourseList.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseInstituteId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOURSEINSTITUTEID"].ToString();
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + courseInstituteId, true);
            }
            else if (e.CommandName.ToUpper().Equals("LIST"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseInstituteId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOURSEINSTITUTEID"].ToString();
                Response.Redirect("../Crew/CrewInstituteBatchList.aspx?courseInstituteId=" + courseInstituteId, true);
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

    protected void gvCourseList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourseList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}