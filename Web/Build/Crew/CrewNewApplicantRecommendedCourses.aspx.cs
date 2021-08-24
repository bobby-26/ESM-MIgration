using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantRecommendedCourses : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?empid=" + Filter.CurrentNewApplicantSelection, true);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantRecommendedCourses.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewRecommendedCourses')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewRecommendedCourseAdd.aspx?empid=" + Filter.CurrentNewApplicantSelection + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDRECOMMENDEDCOURSE");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewCourseMissing.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&t=n&type=2'); return false;", "Initiate Course Request", "<i class=\"fas fa-book\"></i>", "COURSEREQUEST");
            MenuCrewRecommendedCourse.AccessRights = this.ViewState;
            MenuCrewRecommendedCourse.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            SetEmployeePrimaryDetails();
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();

                if (string.IsNullOrEmpty(txtRank.Text.ToString()))
                {
                    txtRank.CssClass = "mandatory";
                    Span1.Visible = true;
                }
                //txtRank.Attributes.Add("")
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewRecommendedCourses.Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDRECOMMENDEDDATE", "FLDRECOMMENDEDBYNAME", "FLDCOURSELASTDONE", "FLDCOMPLETEDYN", "FLDAPPROVEDYN", "FLDNOMINATEDDATE" };
            string[] alCaptions = { "Sl No", "Course", "Recommended Date", "Recommended By", "Last Done", "Status", "Approved", "Approved Date" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewRecommendedCourses.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewRecommendedCourses", "New Applicant Recommended Courses", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewRecommendedCourses.DataSource = ds;
                gvCrewRecommendedCourses.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewRecommendedCourses.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

                DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("New Applicant Recommended Courses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
        GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
        RadTextBox txtRemarks = (RadTextBox)dataItem.FindControl("txtRemarks");
        if (dc.SelectedValue == "0")
            txtRemarks.CssClass = "input_mandatory";
        else
            txtRemarks.CssClass = "input";
    }

    private bool IsValidRecommendedCourse(string approveyn, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(approveyn) == 0 && General.GetNullableString(remarks) == null)
            ucError.ErrorMessage = "Remarks is required when approval is No";

        return (!ucError.IsError);
    }

    protected void gvCrewRecommendedCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string dtkey = ((RadLabel)eeditedItem.FindControl("lblDtkeyEdit")).Text;
                string remarks = ((RadTextBox)eeditedItem.FindControl("txtRemarks")).Text;
                RadComboBox ddlApproveEdit = ((RadComboBox)eeditedItem.FindControl("ddlApproveEdit"));
                if (!IsValidRecommendedCourse(ddlApproveEdit.SelectedValue, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewRecommendedCourse.UpdateApprovalRecommendedCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(dtkey)
                                                                            , General.GetNullableInteger(ddlApproveEdit.SelectedValue)
                                                                            , General.GetNullableString(remarks));
                BindData();
                gvCrewRecommendedCourses.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewRecommendedCourses.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewRecommendedCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton dbwl = (LinkButton)e.Item.FindControl("cmdNL");
            if (dbwl != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dbwl.CommandName)) dbwl.Visible = false;
                dbwl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Nomination List?'); return false;");
            }


            RadLabel lblrecid = (RadLabel)e.Item.FindControl("lblrecommendedid");
            RadLabel lbldtkey = (RadLabel)e.Item.FindControl("lblDtkey");
            LinkButton dbedit = (LinkButton)e.Item.FindControl("cmdEdit");
            //dbedit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '','CrewRecommendedCourseAdd.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&recommendedid=" + lbldtkey.Text + "');return false;");
            RadTextBox txtremarks = (RadTextBox)e.Item.FindControl("txtRemarks");
            if (txtremarks != null)
            {
                if (drv["FLDAPPROVED"].ToString() == "0")
                    txtremarks.CssClass = "input_mandatory";
            }

            RadComboBox ddlapprove = (RadComboBox)e.Item.FindControl("ddlApproveEdit");
            if (ddlapprove != null && ddlapprove.SelectedValue == "" && drv["FLDAPPROVED"].ToString() != "")
            {
                ddlapprove.FindItemByValue(drv["FLDAPPROVED"].ToString()).Selected = true;
                ddlapprove.SelectedValue = drv["FLDAPPROVED"].ToString();
            }
        }
    }
}