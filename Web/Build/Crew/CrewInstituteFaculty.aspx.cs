using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewInstituteFaculty : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFaculty.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInstituteFaculty')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFaculty.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFaculty.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        
        MenuInstituteFaculty.AccessRights = this.ViewState;
        MenuInstituteFaculty.MenuList = toolbar.Show();
       
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Add", "SAVE", ToolBarDirection.Right);
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        //MenuTitle.SetTrigger(pnlInistiuteFaculty);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            SetInstitute();
            gvInstituteFaculty.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void SetInstitute()
    {
        string strInstituteId = Request.QueryString["instituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(strInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInistituteSearch.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string instititeId = null;
        string[] alColumns = { "FLDNAME" , "FLDFACULTYNAME", "FLDINITIAL" ,"FLDROLE" };
        string[] alCaptions = { "Institute","Faculty", "Initial","Faculty Role" };
        if (Request.QueryString["instituteId"] != null)
            instititeId = Request.QueryString["instituteId"].ToString();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixCrewInstituteFaculty.CrewInstituteFacultySearch(txtFacultySearch.Text.Trim()
                                                                                  , txtInistituteSearch.Text.Trim()
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
            gvInstituteFaculty.DataSource = ds;
            gvInstituteFaculty.VirtualItemCount = iRowCount;
        }
        else
        {
            gvInstituteFaculty.DataSource = "";
        }
    }
    protected void MenuInstituteFaculty_TabStripCommand(object sender, EventArgs e)
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

                string[] alColumns = { "FLDNAME", "FLDFACULTYNAME", "FLDINITIAL", "FLDROLE" };
                string[] alCaptions = { "Institute", "Faculty", "Initial", "Faculty Role" };
                string instititeId = Request.QueryString["instituteId"].ToString();

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInstituteFaculty.CrewInstituteFacultySearch(txtFacultySearch.Text.Trim()
                                                                                   , txtInistituteSearch.Text.Trim()
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
                txtFacultySearch.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvInstituteFaculty.CurrentPageIndex = 0;
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
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string courseId = Request.QueryString["courseId"].ToString();
            if (string.IsNullOrEmpty(courseId))
            {
                ucError.Text = "Course is required";
                ucError.Visible = true;
                return;
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem dataItem in gvInstituteFaculty.MasterTableView.Items)
                {
                    // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    string instituteFacultyId = dataItem.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();
                    //string instituteFacultyId = gvInstituteFaculty.DataKeys[row.RowIndex].Value.ToString();
                    if (ck != null)
                    {
                        if (ck.Checked == true)
                        {
                            if (Request.QueryString["from"] != null)
                            {

                                string instituteId = Request.QueryString["instituteId"].ToString();
                                string calendarid = Request.QueryString["calendarId"].ToString();

                                PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarInsert(General.GetNullableInteger(instituteFacultyId).Value
                                                                                            , General.GetNullableInteger(instituteId)
                                                                                            , General.GetNullableInteger(courseId)
                                                                                            , General.GetNullableGuid(calendarid));
                            }
                            else
                            {
                                PhoenixCrewInstituteFaculty.CrewInstituteCourseContactInsert(General.GetNullableInteger(courseId).Value
                                                                                    , General.GetNullableInteger(instituteFacultyId).Value);
                            }
                            
                        }
                    }
                }
        
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
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

    protected void gvInstituteFaculty_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string faculty = null, role = null, instituteId = null, initial = null;
            instituteId = Request.QueryString["instituteId"].ToString();

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string instituteFacultyId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();
                PhoenixCrewInstituteFaculty.CrewInstituteFacultyDelete(General.GetNullableInteger(instituteFacultyId).Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
                BindData();
                gvInstituteFaculty.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                faculty = ((RadTextBox)eeditedItem.FindControl("txtFacultyInsert")).Text;
                role = ((RadTextBox)eeditedItem.FindControl("txtFacultyRoleInsert")).Text;
                initial = ((RadTextBox)eeditedItem.FindControl("txtFacultyInitialInsert")).Text;

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
                instituteId = Request.QueryString["instituteId"].ToString();
                faculty = ((RadTextBox)eeditedItem.FindControl("txtFacultyEdit")).Text;
                role = ((RadTextBox)eeditedItem.FindControl("txtFacultyRoleEdit")).Text;
                initial = ((RadTextBox)eeditedItem.FindControl("txtFacultyInitialEdit")).Text;
                string facultyId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACULTYID").ToString();
                if (!IsValidFaculty(instituteId, faculty, role))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewInstituteFaculty.CrewInstituteFacultyUpdate(General.GetNullableInteger(facultyId).Value
                                                                        , faculty
                                                                        , initial
                                                                        , General.GetNullableInteger(instituteId).Value
                                                                        , role);


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
        LinkButton  eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null)
            eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton delete = (LinkButton)e.Item.FindControl("cmdDelete");
        if (delete != null)
        {
            delete.Visible = SessionUtil.CanAccess(this.ViewState, delete.CommandName);
            delete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to delete?')");
        }
    }
}