using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class CrewOffshoreInitiateExamRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
           
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreInitiateExamRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvExamRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["courserequestid"] = "";
                ViewState["examrequestid"] = "";
                ViewState["courseid"] = "";
                ViewState["examid"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["courserequestid"] != null && Request.QueryString["courserequestid"].ToString() != "")
                    ViewState["courserequestid"] = Request.QueryString["courserequestid"].ToString();

                if (Request.QueryString["examrequestid"] != null && Request.QueryString["examrequestid"].ToString() != "")
                    ViewState["examrequestid"] = Request.QueryString["examrequestid"].ToString();

                if (Request.QueryString["courseid"] != null && Request.QueryString["courseid"].ToString() != "")
                    ViewState["courseid"] = Request.QueryString["courseid"].ToString();

                if (Request.QueryString["examid"] != null && Request.QueryString["examid"].ToString() != "")
                    ViewState["examid"] = Request.QueryString["examid"].ToString();

                if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"].ToString() != "")
                    ViewState["employeeid"]= Request.QueryString["employeeid"].ToString();
                gvExamRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindExams();

                toolbar = new PhoenixToolbar();
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindExams()
    {
        ddlExam.DataSource = PhoenixCrewOffshoreExamRequest.ListCourseExam(General.GetNullableInteger(ViewState["courseid"].ToString()));
        ddlExam.DataTextField = "FLDEXAMNAME";
        ddlExam.DataValueField = "FLDEXAMID";
        ddlExam.DataBind();
        ddlExam.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        if (ViewState["examid"].ToString() != null && ViewState["examid"].ToString() != "")
        {
            ddlExam.SelectedValue = ViewState["examid"].ToString();
            ddlExam.Enabled = false;
        }
            
    }

    protected void EditExamRequest()
    {
        DataTable dt = PhoenixCrewOffshoreExamRequest.EditExamRequest(General.GetNullableGuid(ViewState["examrequestid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            ddlExam.SelectedValue = dt.Rows[0]["FLDEXAMID"].ToString();
            ddlExam.Enabled = false;
            txtTargetDate.Text = dt.Rows[0]["FLDTARGETDATE"].ToString();
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXAMREQUEST"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? newid = null;

                PhoenixCrewOffshoreExamRequest.InsertExamRequest(General.GetNullableGuid(ViewState["courserequestid"].ToString())
                                                    , General.GetNullableGuid(ddlExam.SelectedValue)
                                                    , General.GetNullableDateTime(txtTargetDate.Text)
                                                    , ref newid
                                                    );

                ViewState["examrequestid"] = newid;
                ucStatus.Text = "Exam request initiated successfully.";
                EditExamRequest();
                BindData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('chml', null, true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlExam.SelectedValue) == null)
            ucError.ErrorMessage = "Exam is required.";

        if (General.GetNullableDateTime(txtTargetDate.Text) == null)
            ucError.ErrorMessage = "Targe Date is required.";

        return (!ucError.IsError);
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDCOURSE", "FLDEXAMNAME", "FLDTARGETDATE", "FLDSCORE", "FLDEXAMRESULTNAME", "FLDDATEATTENDED" };
                string[] alCaptions = { "Vessel", "Employee Name", "File No", "Rank", "Course", "Exam", "Target Date", "Score", "Exam Result", "Date Attended" };

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

                DataTable dt = PhoenixCrewOffshoreExamRequest.SearchExamRequest(null, null, General.GetNullableGuid(ViewState["courserequestid"].ToString())
                                                                                       , null, null, null
                                                                                       , sortexpression, sortdirection
                                                                                       , 1, iRowCount
                                                                                       , ref iRowCount, ref iTotalPageCount
                                                                                       , 1
                                                                                       );
                General.ShowExcel("Test Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDCOURSE", "FLDEXAMNAME", "FLDTARGETDATE", "FLDSCORE", "FLDEXAMRESULTNAME", "FLDDATEATTENDED" };
        string[] alCaptions = { "Vessel", "Employee Name", "File No", "Rank", "Course", "Exam", "Target Date", "Score", "Exam Result", "Date Attended" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.TrainingneedsEmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

            lblfname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
            //lblmname.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            //lbllname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            txtfileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
            lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();

            dt = PhoenixCrewOffshoreExamRequest.SearchExamRequest(null, null, General.GetNullableGuid(ViewState["courserequestid"].ToString())
                                                                       , null, null, null
                                                                       , sortexpression, sortdirection
                                                                       , gvExamRequest.CurrentPageIndex+1, gvExamRequest.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount,1);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvExamRequest", "Test Request", alCaptions, alColumns, ds);
           
                gvExamRequest.DataSource = dt;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExamRequest_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvExamRequest_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEmployeeName != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");

            }
        }
    }

  
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    protected void gvExamRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExamRequest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
