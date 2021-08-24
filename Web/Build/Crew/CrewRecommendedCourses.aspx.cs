using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;
public partial class CrewRecommendedCourses : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            //{
            //    Response.Redirect("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?empid=" + Filter.CurrentCrewSelection, true);
            //}
            
            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            RecommendedCourses.AccessRights = this.ViewState;
            RecommendedCourses.MenuList = toolbarmenu.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewRecommendedCourses.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewRecommendedCourses')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewRecommendedCourseAdd.aspx?empid=" + Filter.CurrentCrewSelection + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDRECOMMENDEDCOURSE");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewCourseMissing.aspx?empid=" + Filter.CurrentCrewSelection + "&t=p&type=2'); return false;", "Initiate Course Request", "<i class=\"fas fa-book\"></i>", "COURSEREQUEST");
            MenuCrewRecommendedCourse.AccessRights = this.ViewState;
            MenuCrewRecommendedCourse.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                SetEmployeePrimaryDetails();

                gvCrewRecommendedCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewRecommendedCourses.Rebind();
    }

    protected void CrewRecommendedCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDRECOMMENDEDDATE", "FLDRECOMMENDEDBYNAME", "FLDCOURSELASTDONE", "FLDCOMPLETEDYN", "FLDAPPROVEDYN", "FLDNOMINATEDDATE" };
                string[] alCaptions = { "Sl No", "Course", "Recommended Date", "Recommended By", "Last Done", "Status", "Approved", "Approved Date" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(Filter.CurrentCrewSelection.ToString())
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                {
                    Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Recommended_Courses.xls");
                    Response.ContentType = "application/vnd.msexcel";
                    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                    Response.Write("<tr>");
                    Response.Write("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                    Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp Crew Recommended Courses</h3></td>");
                    Response.Write("</tr>");
                    Response.Write("</TABLE>");
                    Response.Write("<br />");
                    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='2' width='50%'>");
                    Response.Write("<tr>");
                    Response.Write("<td width='80px'><h5><left>First Name  : </left></h5></td> <td width='70px'><h5><left> " + txtFirstName.Text.Trim() + "</left></h5></td>");
                    Response.Write("<td width='80px'><h5><left>Middle Name : </left></h5></td><td width='70px'><h5><left> " + txtMiddleName.Text.Trim() + "</left></h5></td>");
                    Response.Write("<td width='80px'><h5><left>Last Name   : </left></h5></td><td width='70px'><h5><left> " + txtLastName.Text.Trim() + "</left></h5></td><td></td>");
                    Response.Write("</tr>");
                    Response.Write("<tr>");
                    Response.Write("<td width='80px'><h5><left>Employee Number :</left></h5></td> <td ><h5><left> " + txtEmployeeNumber.Text.Trim() + "</left></h5></td>");
                    Response.Write("<td width='40px'><h5><left>Rank :</left></h5></td> <td ><h5><left> " + txtRank.Text.Trim() + "</left></h5></td>");
                    Response.Write("</tr>");
                    Response.Write("</TABLE>");
                    Response.Write("<br />");
                    Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                    Response.Write("<tr>");
                    for (int i = 0; i < alCaptions.Length; i++)
                    {
                        Response.Write("<td>");
                        Response.Write("<b>" + alCaptions[i] + "</b>");
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            Response.Write("<td align='left'>");
                            Response.Write(dr[alColumns[i]].GetType().Name.Equals("DateTime") ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                            Response.Write("</td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</TABLE>");
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDRECOMMENDEDDATE", "FLDRECOMMENDEDBYNAME", "FLDCOURSELASTDONE", "FLDCOMPLETEDYN", "FLDAPPROVEDYN", "FLDNOMINATEDDATE" };
            string[] alCaptions = { "S.No.", "Course", "Recommended Date", "Recommended By", "Last Done", "Status", "Approved", "Approved Date" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewRecommendedCourses.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewRecommendedCourses", "Crew Recommended Courses", alCaptions, alColumns, ds);

            gvCrewRecommendedCourses.DataSource = ds;
            gvCrewRecommendedCourses.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewRecommendedCourses.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvCrewRecommendedCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    
    protected void gvCrewRecommendedCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            RadLabel lblRankid = (RadLabel)e.Item.FindControl("lblRankid");
            RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseId");         
            LinkButton dbedit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (dbedit != null) if (!SessionUtil.CanAccess(this.ViewState, dbedit.CommandName)) dbedit.Visible = false;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadTextBox txtremarks = (RadTextBox)e.Item.FindControl("txtRemarks");
            if (txtremarks != null)
            {
                if (drv["FLDAPPROVED"].ToString() == "0")
                    txtremarks.CssClass = "input_mandatory";
            }
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddlapprove = (RadComboBox)e.Item.FindControl("ddlApproveEdit");
            if (ddlapprove != null) ddlapprove.SelectedValue = drv["FLDAPPROVED"].ToString();
        }
        
    }

   
    protected void gvCrewRecommendedCourses_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text;
            string remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
            RadComboBox ddlApproveEdit = ((RadComboBox)e.Item.FindControl("ddlApproveEdit"));
            if (!IsValidRecommendedCourse(ddlApproveEdit.SelectedValue, remarks))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewRecommendedCourse.UpdateApprovalRecommendedCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(dtkey), General.GetNullableInteger(ddlApproveEdit.SelectedValue), General.GetNullableString(remarks));
            
            BindData();
            gvCrewRecommendedCourses.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    private bool IsValidRecommendedCourse(string approveyn, string remarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(approveyn) == 0 && General.GetNullableString(remarks) == null)
            ucError.ErrorMessage = "Remarks is required when approval is No";

        return (!ucError.IsError);
    }
    
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlApprovalType_TextChanged(object sender, EventArgs e)
    {
        RadComboBox dc = (RadComboBox)sender;
        RadTextBox txtRemarks;
        GridDataItem row = (GridDataItem)dc.NamingContainer;

        txtRemarks = (RadTextBox)row.FindControl("txtRemarks");        
        if (dc.SelectedValue == "0")
        {
            txtRemarks.CssClass = "input_mandatory";
        }
        else
        {
            txtRemarks.CssClass = "";
        }

    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

}
