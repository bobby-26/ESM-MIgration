using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewEmployeeAvailableDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "LIST",ToolBarDirection.Right);
        toolbar.AddButton("Course", "COURSE",ToolBarDirection.Right);
        toolbar.AddButton("Enrollment", "ENROLLMENT", ToolBarDirection.Right);
        toolbar.AddButton("Attendance", "ATTANDANCE", ToolBarDirection.Right);
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "LIST", ToolBarDirection.Right);
        toolbar.AddButton("Availability", "AVAIL");      
        MenuAvailability.AccessRights = this.ViewState;
        MenuAvailability.MenuList = toolbar.Show();
        MenuAvailability.SelectedMenuIndex = 1;

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Available Participant", "BACK");
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewEmployeeAvailableDate.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmpAvailability')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewEmployeeAvailableDate.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewEmployeeAvailableDate.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");        
        MenuCrewEmpAvailability.AccessRights = this.ViewState;
        MenuCrewEmpAvailability.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["empId"] != null)
                txtEmployeeId.Text = Request.QueryString["empId"].ToString();
            if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "ENROLLLIST")
            {
                MenuTitle.Visible = true;
                MenuHeader.Visible = true;
                //divmenu.Visible = true;
            }
            if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "AVAILABILITYADD")
            {
                MenuAvailability.Visible = true;
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            txtEmployeeId.Attributes.Add("style", "display:none");
            SetEmployeePrimaryDetails();
            gvEmpAvailability.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //Response.Cache.SetCacheability(HttpCacheability.Private);
       

        BindData();        
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDAVAILABLESTARTDATE", "FLDAVAILABLEENDDATE", "FLDCITYNAME" };
        string[] alCaptions = { "S.No", "Available From", "Available To", "Preferred Location" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableDatesSearch(General.GetNullableInteger(txtEmployeeId.Text)
                                                                                        , General.GetNullableDateTime(txtAvailableStartDate.Text)
                                                                                        , General.GetNullableDateTime(txtAvailableEndDate.Text)
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                        , gvEmpAvailability.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvEmpAvailability", "Seafarer Availability", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(txtEmployeeId.Text))
            {
                gvEmpAvailability.DataSource = ds;
                gvEmpAvailability.VirtualItemCount = iRowCount;
            }           
        }
        else
        {
            gvEmpAvailability.DataSource = "";
        }
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            string strEmployeeId = txtEmployeeId.Text;
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewEmpAvailability_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvEmpAvailability.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDAVAILABLESTARTDATE", "FLDAVAILABLEENDDATE", "FLDCITYNAME" };
                string[] alCaptions = { "S.No", "Available From", "Available To", "Preferred Location" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableDatesSearch(General.GetNullableInteger(txtEmployeeId.Text)
                                                                                        , General.GetNullableDateTime(txtAvailableStartDate.Text)
                                                                                        , General.GetNullableDateTime(txtAvailableEndDate.Text)
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                        , gvEmpAvailability.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtName.Text))
                        General.ShowExcel("Seafarer Availability", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
                    else
                    {
                        ds.Tables[0].Rows.Clear();
                        General.ShowExcel("Seafarer Availability", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
                    }
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtAvailableStartDate.Text = "";
                txtAvailableEndDate.Text = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvEmpAvailability.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private bool IsValidAvailability(string startDate, string endDate, string employee)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(employee))
            ucError.ErrorMessage = "Employee  is required";
        if (string.IsNullOrEmpty(startDate))
            ucError.ErrorMessage = "Available from is required";
        if (string.IsNullOrEmpty(endDate))
            ucError.ErrorMessage = "Available to is required";

        return (!ucError.IsError);
    }
    
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string from = null;
        if (Request.QueryString["from"] != null)
        {
            from = Request.QueryString["from"].ToString();
        }

        if (CommandName.ToUpper().Equals("BACK"))
        {
            if (from == "AVAILABILITYADD")
                Response.Redirect("../Crew/CrewEmployeeAvailabilityAdd.aspx", false);
            else if (from == "ENROLLLIST")
                Response.Redirect("../Crew/CrewAvailableEmployeeList.aspx?" + Request.QueryString, false);
        }
    }

    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string courseInstituteId = Request.QueryString["courseInstituteId"].ToString();
            
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("ENROLLMENT"))
            {
                Response.Redirect("../Crew/CrewBatchEnrollment.aspx?courseInstituteId=" + courseInstituteId , true);
            }
            if (CommandName.ToUpper().Equals("ATTANDANCE"))
            {
                Response.Redirect("../Crew/CrewBatchAttendance.aspx?courseInstituteId=" + courseInstituteId , true);
            }
            if (CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../Crew/CrewTrainingScheduleEdit.aspx?courseInstituteId=" + courseInstituteId , true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAvailability_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewEmployeeAvailabilityAdd.aspx");
            }
            if (CommandName.ToUpper().Equals("AVAIL"))
            {
                Response.Redirect("../Crew/CrewEmployeeAvailableDate.aspx?" + Request.QueryString);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvEmpAvailability_ItemDataBound(object sender, GridItemEventArgs e)
    {
        int empId = 0;
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if (!string.IsNullOrEmpty(txtEmployeeId.Text))
                empId = General.GetNullableInteger(txtEmployeeId.Text).Value;

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete?')");
            }
            ImageButton btnShowCityEdit = (ImageButton)e.Item.FindControl("btnShowCityEdit");
            
            if (btnShowCityEdit != null)
                btnShowCityEdit.Attributes.Add("onclick", "return showPickList('spnPickListCity', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListPreferredCity.aspx',true);");
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton btnShowCity = (ImageButton)e.Item.FindControl("btnShowCity");
            if (btnShowCity != null)
                btnShowCity.Attributes.Add("onclick", "return showPickList('spnPickListCity', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListPreferredCity.aspx',true);");
        }
    }

    protected void gvEmpAvailability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmpAvailability.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmpAvailability_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string startDate = null, endDate = null, location = null;
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                startDate = ((UserControlDate)eeditedItem.FindControl("txtAvailableStartDateInsert")).Text;
                endDate = ((UserControlDate)eeditedItem.FindControl("txtAvailableEndDateInsert")).Text;
                location = ((RadTextBox)eeditedItem.FindControl("txtPreferredLocationInsert")).Text;

                if (!IsValidAvailability(startDate, endDate, txtEmployeeId.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableDatesInsert(General.GetNullableInteger(txtEmployeeId.Text).Value
                                                                                   , General.GetNullableDateTime(startDate)
                                                                                   , General.GetNullableDateTime(endDate)
                                                                                   , General.GetNullableInteger(location));
                //ucStatus.Text = "Available Date is saved successfully";
                BindData();
                gvEmpAvailability.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                startDate = ((UserControlDate)eeditedItem.FindControl("txtAvailableStartDateEdit")).Text;
                endDate = ((UserControlDate)eeditedItem.FindControl("txtAvailableEndDateEdit")).Text;
                location = ((RadTextBox)eeditedItem.FindControl("txtPreferredLocationEdit")).Text;
                string availableDateId = eeditedItem.GetDataKeyValue("FLDEMPLOYEEAVAILABILITYDATESID").ToString();
                if (!IsValidAvailability(startDate, endDate, txtEmployeeId.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableDatesUpdate(General.GetNullableInteger(txtEmployeeId.Text).Value
                                                                                    , General.GetNullableDateTime(startDate)
                                                                                    , General.GetNullableDateTime(endDate)
                                                                                    , General.GetNullableGuid(availableDateId)
                                                                                    , General.GetNullableInteger(location));
                BindData();
                gvEmpAvailability.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                string empAvailabilityId = eeditedItem.GetDataKeyValue("FLDEMPLOYEEAVAILABILITYDATESID").ToString();
                PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailableDatesDelete(empAvailabilityId);              
                BindData();
                gvEmpAvailability.Rebind();
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
}