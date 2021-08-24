using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshoreEmployeePendingTrainingNeeds : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                ViewState["PAGENUMBERPTC"] = 1;
                ViewState["PAGENUMBERPCBT"] = 1;
                ViewState["Trainingcoursetype"] = "";
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                {
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
                    ViewState["Trainingcoursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
                }

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["empid"].ToString();

                SetEmployeePrimaryDetails();
                gvOffshorePCBTTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvOffshoreTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvOffshorePCourseTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            RecommendedCourses.AccessRights = this.ViewState;
            RecommendedCourses.MenuList = toolbar1.Show();

            PhoenixToolbar toolbarPCBT = new PhoenixToolbar();
            toolbarPCBT.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarPCBT.AddFontAwesomeButton("javascript:CallPrint('gvOffshorePCBTTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshorePendingCBTTraining.AccessRights = this.ViewState;
            MenuOffshorePendingCBTTraining.MenuList = toolbarPCBT.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

            PhoenixToolbar toolbarPTC = new PhoenixToolbar();
            toolbarPTC.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarPTC.AddFontAwesomeButton("javascript:CallPrint('gvOffshorePCourseTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshorePendingCourseTraining.AccessRights = this.ViewState;
            MenuOffshorePendingCourseTraining.MenuList = toolbarPTC.Show();

         
            //BindDataPCBT();
            //SetPageNavigatorPCBT();
            //BindData();
            //SetPageNavigator();
            //BindDataPTC();
            //SetPageNavigatorPTC();
            PhoenixCrewOffshoreTrainingNeeds.duplicateTrainingneedsoverridesAuto();
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
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

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

    protected void TrainingNeed_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype = "";

        if (CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + coursetype + "&empid=" + ViewState["employeeid"].ToString(), true);
        }
        else if (CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + coursetype + "&empid=" + ViewState["employeeid"].ToString(), true);
        }
        else if (CommandName.ToUpper().Equals("SEAGULL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "8");
            Response.Redirect("../CrewOffshore/CrewOffshoreEmployeePendingTrainingNeeds.aspx?coursetype=" + coursetype + "&empid=" + ViewState["employeeid"].ToString(), true);
        }
    }

    protected void BindCategory(RadComboBox ddl)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
        ddl.DataTextField = "FLDCATEGORYNAME";
        ddl.DataValueField = "FLDCATEGORYID";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindSubCategory(RadComboBox ddl, string categoryid)
    {
        ddl.Items.Clear();
        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingSubCategory(General.GetNullableInteger(categoryid));
        ddl.DataTextField = "FLDNAME";
        ddl.DataValueField = "FLDSUBCATEGORYID";
        ddl.DataSource = dt;
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Score %", "Result" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchCBTTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            1,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()), 1, null, 0);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending CBT </h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOffshoreTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindData();
                gvOffshoreTraining.Rebind();
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
        gvOffshoreTraining.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchCBTTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreTraining.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                1,
                General.GetNullableInteger(ViewState["coursetype"].ToString()), 0, null, 0);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Pending CBT", alCaptions, alColumns, ds);

        gvOffshoreTraining.DataSource = ds;
        gvOffshoreTraining.VirtualItemCount = iRowCount;

      

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }

    
   
  
    //protected void gvOffshoreTraining_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

    //        TableCell HeaderCell;

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "As reported by the vessel";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 6;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 3;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "To be Completed by Manning Office / Agent";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 6;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        gvOffshoreTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    }
    //}

    private bool IsValidData(string Category, string SubCategory, string trainingneed, string improvementlevel, string completiondate, string israisedfromcbt)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Category) == null && General.GetNullableInteger(israisedfromcbt) == 0)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableInteger(SubCategory) == null && General.GetNullableInteger(israisedfromcbt) == 0)
            ucError.ErrorMessage = "SubCategory is required.";

        if (General.GetNullableString(trainingneed) == null && General.GetNullableInteger(israisedfromcbt) == 0)
            ucError.ErrorMessage = "Identified training need is required.";

        if (General.GetNullableInteger(improvementlevel) == null && General.GetNullableInteger(israisedfromcbt) == 0)
            ucError.ErrorMessage = "Level of improvement is required.";

        if (General.GetNullableDateTime(completiondate) != null)
        {
            if (General.GetNullableDateTime(completiondate) > System.DateTime.Now)
                ucError.ErrorMessage = "Completion date cannot be future date.";
        }

        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlCategoryEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        RadComboBox ddlSubCategoryEdit = (RadComboBox)gvrow.FindControl("ddlSubCategoryEdit");
        if (ddlSubCategoryEdit != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryEdit, ddl.SelectedValue);
        }
    }

    protected void ddlCategoryAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        RadComboBox ddlSubCategoryAdd = (RadComboBox)gvrow.FindControl("ddlSubCategoryAdd");
        if (ddlSubCategoryAdd != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryAdd, ddl.SelectedValue);
        }
    }

    /* Pending Recommended CBT Training*/
    protected void ShowExcelPCBT()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Score %", "Result" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchCBTTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBERPCBT"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            1,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()), 1, null, 1);

        Response.AddHeader("Content-Disposition", "attachment; filename=PendingCBTTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending Recommended CBT</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOffshorePendingCBTTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelPCBT();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataPCBT();
                gvOffshorePCBTTraining.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindDataPCBT()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchCBTTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERPCBT"].ToString()),
               gvOffshorePCBTTraining.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                1,
                General.GetNullableInteger(ViewState["coursetype"].ToString()), 1, null, 1);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshorePCBTTraining", "Pending Recommended CBT", alCaptions, alColumns, ds);
        gvOffshorePCBTTraining.DataSource = ds;
        gvOffshorePCBTTraining.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNTPCBT"] = iRowCount;
        ViewState["TOTALPAGECOUNTPCBT"] = iTotalPageCount;

    }
    //protected void gvOffshorePCBTTraining_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindDataPCBT();
    //        ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
    //        SetPageNavigatorPCBT();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void gvOffshorePCBTTraining_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

    //        TableCell HeaderCell;

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 2;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 3;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "To be Completed by Manning Office / Agent";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 6;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        gvOffshorePCBTTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    }
    //}




    /*Pending Training Course*/
    protected void ShowExcelPTC()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBERPTC"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            1,
                            General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=PendingTrainingCourse.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending Training Course</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOffshorePendingCourseTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelPTC();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataPTC();
                gvOffshorePCourseTraining.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindDataPTC()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERPTC"].ToString()),
               gvOffshorePCourseTraining.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                1,
                General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString())
                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshorePCourseTraining", "Pending Training Course", alCaptions, alColumns, ds);
        gvOffshorePCourseTraining.DataSource = ds;
        gvOffshorePCourseTraining.VirtualItemCount = iRowCount;
     

        ViewState["ROWCOUNTPTC"] = iRowCount;
        ViewState["TOTALPAGECOUNTPTC"] = iTotalPageCount;
       
    }
   

  
   
    
    //protected void gvOffshorePCourseTraining_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

    //        TableCell HeaderCell;

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "As reported by the vessel";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 6;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 3;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "To be Completed by Manning Office / Agent";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 6;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        gvOffshorePCourseTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    }
    //}


    protected void gvOffshorePCBTTraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERPCBT"] = ViewState["PAGENUMBERPCBT"] != null ? ViewState["PAGENUMBERPCBT"] : gvOffshorePCBTTraining.CurrentPageIndex + 1;
            BindDataPCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCBTTraining_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick
                    , null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataPCBT();
                gvOffshorePCBTTraining.Rebind();
                ((DropDownList)e.Item.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataPCBT();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text
                    , ((Label)e.Item.FindControl("lblIsRaisedFromCBTEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit")).SelectedToBeDoneBy),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text)
                    );


                BindDataPCBT();
                gvOffshorePCBTTraining.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERPCBT"] = null;
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCBTTraining_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ddlCategoryEdit.CssClass = "input";
            }

            RadComboBox ddlSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindItemByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ddlSubCategoryEdit.CssClass = "input";
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Item.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ucImprovementEdit.CssClass = "input";
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ucTypeofTrainingEdit.CssClass = "input";
            }

            TextBox txtTrainingNeedEdit = (TextBox)e.Item.FindControl("txtTrainingNeedEdit");
            if (txtTrainingNeedEdit != null)
            {
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    txtTrainingNeedEdit.CssClass = "input";
            }

            UserControlOffshoreToBeDoneBy ucDonebyEdit = (UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit");
            if (ucDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                {
                    ucDonebyEdit.TypeOfTraining = ucTypeofTrainingEdit.SelectedQuick;
                    ucDonebyEdit.bind();
                }
                ucDonebyEdit.SelectedToBeDoneBy = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

           
            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Item.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        { 
      
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreTraining.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvOffshoreTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick
                    , null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindData();
                ((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).Focus();
                gvOffshoreTraining.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text
                    , ((Label)e.Item.FindControl("lblIsRaisedFromCBTEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit")).SelectedToBeDoneBy),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text)
                    );

              
                BindData();
                gvOffshoreTraining.Rebind();
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

    protected void gvOffshoreTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            RadComboBox ddlSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindItemByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Item.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            UserControlOffshoreToBeDoneBy ucDonebyEdit = (UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit");
            if (ucDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                {
                    ucDonebyEdit.TypeOfTraining = ucTypeofTrainingEdit.SelectedQuick;
                    ucDonebyEdit.bind();
                }
                ucDonebyEdit.SelectedToBeDoneBy = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:openNewWindow('att','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
          

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Item.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshorePCourseTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERPTC"] = ViewState["PAGENUMBERPTC"] != null ? ViewState["PAGENUMBERPTC"] : gvOffshorePCourseTraining.CurrentPageIndex + 1;
            BindDataPTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCourseTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick
                    , null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataPTC();
                ((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).Focus();
                gvOffshorePCourseTraining.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataPTC();
                gvOffshorePCourseTraining.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text
                    , ((RadLabel)e.Item.FindControl("lblIsRaisedFromCBTEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit")).SelectedToBeDoneBy),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text)
                    );

              
                BindDataPTC();
                gvOffshorePCourseTraining.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERPTC"] = null;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCourseTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ddlCategoryEdit.CssClass = "input";
            }

            RadComboBox ddlSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindItemByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ddlSubCategoryEdit.CssClass = "input";
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Item.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ucImprovementEdit.CssClass = "input";
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ucTypeofTrainingEdit.CssClass = "input";
            }

            RadTextBox txtTrainingNeedEdit = (RadTextBox)e.Item.FindControl("txtTrainingNeedEdit");
            if (txtTrainingNeedEdit != null)
            {
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    txtTrainingNeedEdit.CssClass = "input";
            }

            UserControlOffshoreToBeDoneBy ucDonebyEdit = (UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit");
            if (ucDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                {
                    ucDonebyEdit.TypeOfTraining = ucTypeofTrainingEdit.SelectedQuick;
                    ucDonebyEdit.bind();
                }
                ucDonebyEdit.SelectedToBeDoneBy = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:openNewWindow('att','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            //ImageButton imgCourseName = (ImageButton)e.Item.FindControl("imgCourseName");
            //if (imgCourseName != null)
            //{
            //    if (lblCourseName != null)
            //    {
            //        if (lblCourseName.Text != "")
            //        {
            //            //imgWD.Visible = true;
            //            imgCourseName.ImageUrl = Session["images"] + "/te_view.png";
            //            if (ucToolTipCourseName != null)
            //            {
            //                imgCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'visible');");
            //                imgCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'hidden');");
            //            }
            //        }
            //        else
            //            imgCourseName.ImageUrl = Session["images"] + "/no-remarks.png";
            //    }
            //}

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Item.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }
}
