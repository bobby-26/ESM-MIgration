using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewCourseRequestedList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewCourseRequestedList.aspx?"+Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();
            if (!Page.IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
            BindData();
            gvReq.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCourseRequestFilter = null;
                BindData();
                gvReq.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDZONE",   "FLDFROMDATE", "FLDTODATE", "FLDREMARKS", "FLDENROLLED" };
                string[] alCaptions = {  "File No", "Name",  "Rank", "Zone", "Available From", "Available To", "Remarks", "EnrolledYN" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                string courseId = Request.QueryString["courseId"].ToString();


                DataTable dt = PhoenixCrewRecommendCourse.CrewRequestedCourseEmployeeSearch(null
                                                            , General.GetNullableInteger(courseId)
                                                            , null
                                                            , sortexpression
                                                            , sortdirection
                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvReq.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
                General.ShowExcel("Course Request List", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDZONE", "FLDFROMDATE", "FLDTODATE", "FLDREMARKS", "FLDENROLLED" };
            string[] alCaptions = { "File No", "Name", "Rank", "Zone", "Available From", "Available To", "Remarks", "EnrolledYN" };

            string courseId = Request.QueryString["courseId"].ToString();

            DataTable dt = PhoenixCrewRecommendCourse.CrewRequestedCourseEmployeeSearch(null
                                                        , General.GetNullableInteger(courseId)
                                                        , null                                                        
                                                        , sortexpression
                                                        , sortdirection
                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , gvReq.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);
            DataSet ds = new DataSet();
            DataTable newdt = dt.Copy();
            ds.Tables.Add(newdt);
            General.SetPrintOptions("gvReq", "Course Request List", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvReq.DataSource = ds;
                gvReq.VirtualItemCount = iRowCount;
            }
            else
            {
                gvReq.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
    protected void gvReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}