using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewInstituteFacultyPlannerList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFacultyPlannerList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInstituteFaculty')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFacultyPlannerList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFacultyPlannerList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");        
        MenuFacultyGrid.AccessRights = this.ViewState;
        MenuFacultyGrid.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("Planner", "PLAN");
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        MenuTitle.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtInstituteId.Attributes.Add("style", "display:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            btnShowInstitute.Attributes.Add("onclick", "javascript:return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
            gvInstituteFaculty.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string instititeId = null;
        string[] alColumns = { "FLDFACULTYNAME", "FLDINITIAL", "FLDROLE", "FLDNAME" };
        string[] alCaptions = { "Faculty","Initial", "Role",  "Institute" };
        instititeId = txtInstituteId.Text;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixCrewInstituteFaculty.CrewInstituteFacultySearch(null
                                                                           , txtInstituteName.Text
                                                                           , General.GetNullableInteger(instititeId)
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvInstituteFaculty.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvInstituteFaculty", "Institute Faculty", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(txtInstituteName.Text))
            {
                gvInstituteFaculty.DataSource = ds;
                gvInstituteFaculty.VirtualItemCount = iRowCount;
            }
        }
        else
        {
            gvInstituteFaculty.DataSource = "";
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
            gvInstituteFaculty.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PLAN"))
        {
            Response.Redirect("../Crew/CrewInstituteFacultyPlanView.aspx", true);
        }
    }
  

    protected void MenuFacultyGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvInstituteFaculty.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDFACULTYNAME", "FLDINITIAL", "FLDROLE", "FLDNAME" };
                string[] alCaptions = { "Faculty", "Initial", "Role", "Institute" };
                string instititeId = txtInstituteId.Text;

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInstituteFaculty.CrewInstituteFacultySearch(null
                                                                                   , txtInstituteName.Text.Trim()
                                                                                   , General.GetNullableInteger(instititeId)
                                                                                   , sortexpression
                                                                                   , sortdirection
                                                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                   , gvInstituteFaculty.PageSize
                                                                                   , ref iRowCount
                                                                                   , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Institute Faculty", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtInstituteName.Text = "";
                txtInstituteId.Text = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvInstituteFaculty.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidFaculty(string institute, string faculty, string role)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(institute))
            ucError.ErrorMessage = "Institute is required";
        if (string.IsNullOrEmpty(faculty))
            ucError.ErrorMessage = "Faculty is required";
        if (string.IsNullOrEmpty(role))
            ucError.ErrorMessage = "Role is required";

        return (!ucError.IsError);
    }

    protected void gvInstituteFaculty_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string faculty = null, role = null, instituteId = null, initial = null;
        instituteId = txtInstituteId.Text;
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eeditedItem = e.Item as GridFooterItem;

                faculty = ((RadTextBox)eeditedItem.FindControl("txtFacultyInsert")).Text;
                role = ((RadTextBox)eeditedItem.FindControl("txtFacultyRoleInsert")).Text;
                initial = ((RadTextBox)eeditedItem.FindControl("txtFacultyInitialInsert")).Text;
                instituteId = txtInstituteId.Text;
                if (!IsValidFaculty(instituteId, faculty, role))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewInstituteFaculty.CrewInstituteFacultyInsert(General.GetNullableInteger(instituteId).Value
                                                                       , faculty
                                                                       , initial
                                                                       , role);

                BindData();
                gvInstituteFaculty.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                instituteId = txtInstituteId.Text;
                faculty = ((RadTextBox)eeditedItem.FindControl("txtFacultyEdit")).Text;
                role = ((RadTextBox)eeditedItem.FindControl("txtFacultyRoleEdit")).Text;
                initial = ((RadTextBox)eeditedItem.FindControl("txtFacultyInitialEdit")).Text;
                string facultyId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();
               
                PhoenixCrewInstituteFaculty.CrewInstituteFacultyUpdate(General.GetNullableInteger(facultyId).Value
                                                                        , faculty
                                                                        , initial
                                                                        , General.GetNullableInteger(instituteId).Value
                                                                        , role);
                BindData();
                gvInstituteFaculty.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string instituteFacultyId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();
                PhoenixCrewInstituteFaculty.CrewInstituteFacultyDelete(General.GetNullableInteger(instituteFacultyId).Value);
                BindData();
                gvInstituteFaculty.Rebind();
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
            BindData();
        }
    }

    protected void gvInstituteFaculty_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInstituteFaculty.CurrentPageIndex + 1;
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInstituteFaculty_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdPlan = (LinkButton)e.Item.FindControl("cmdPlan");
            LinkButton cmdCourse = (LinkButton)e.Item.FindControl("cmdCourse");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");

            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            string facultyId = item.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();

            if (cmdPlan != null)
                cmdPlan.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewInstituteFacultyPlanner.aspx?instituteId=" + txtInstituteId.Text + "&facultyId=" + facultyId + "'); return false;");

            if (cmdCourse != null)
                cmdCourse.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewInstituteFacultyCourseAdd.aspx?instituteId=" + txtInstituteId.Text + "&facultyId=" + facultyId + "'); return false;");

            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
            }

            LinkButton al = (LinkButton)e.Item.FindControl("cmdContact");
            if (al != null)
                al.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewInstituteFacultyContact.aspx?instituteFacultyId=" + facultyId + "&instituteId=" + txtInstituteId.Text + "');return false;");
        }
    }
}