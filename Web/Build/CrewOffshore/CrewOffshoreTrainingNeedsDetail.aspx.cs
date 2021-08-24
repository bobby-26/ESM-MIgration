using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingNeedsDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsVessel.aspx");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                ViewState["examrequestid"] = "";
                ViewState["employeeid"] = null;
                rbtoption.SelectedIndex = 0;
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");

                gvOffshoreTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("CBT", "CBT");
            toolbarsub.AddButton("Training Course", "TRAINING COURSE");
            toolbarsub.AddButton("Task", "TASK", ToolBarDirection.Left);
            toolbarsub.AddButton("Back", "BACK");

            TrainingNeed.AccessRights = this.ViewState;
            TrainingNeed.MenuList = toolbarsub.Show();
            TrainingNeed.SelectedMenuIndex = 1;

            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();



            if (Request.QueryString["employeeid"].ToString() != null && Request.QueryString["employeeid"].ToString() != "")
            {
                ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
            }

            //BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CourseRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype = "";

        if (CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
    }
    protected void TrainingNeedDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype = "";
        if (CommandName.ToUpper().Equals("CBTDETAIL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
            TrainingNeed.SelectedMenuIndex = 1;
        }

        if (CommandName.ToUpper().Equals("TQCOURCEDETAIL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
            TrainingNeed.SelectedMenuIndex = 1;
        }
        if (CommandName.ToUpper().Equals("BACK"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
            TrainingNeed.SelectedMenuIndex = 1;
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

    protected void BindToBeDoneBy(RadComboBox ddl, int? typeoftraining)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingNeeds.ListTrainingToBeDoneBy(130, typeoftraining);
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDTRAININGNEEDSTATUSNAME","FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDEXAMNAME","FLDNOOFATTEMPTS","FLDSCORE","FLDEXAMRESULT","FLDDATEATTENDED"};
            alCaptions = new string[] {"Status", "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Test", "No Of Attempts","Score %","Result","Date Attended"};
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED"};
            alCaptions = new string[] { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed"};
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeedsDetailSearch(General.GetNullableInteger(ViewState["employeeid"].ToString()),
            General.GetNullableInteger(ViewState["coursetype"].ToString()),
                            sortexpression,
                            sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount, 1, "u"
                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending Training Needs</h3></td>");
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
    protected void TrainingNeed_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype;
        if (CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("TRAINING COURSE"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("BACK"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("TASK"))
        {
            Response.Redirect("../CrewOffshore/CrewoffshoreTrainingNeedTasksDetails.aspx?employeeid=" + ViewState["employeeid"].ToString(), true);
        }
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
                Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindData();

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
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvOffshoreTraining.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDTRAININGNEEDSTATUSNAME","FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDEXAMNAME","FLDNOOFATTEMPTS","FLDSCORE","FLDEXAMRESULT","FLDDATEATTENDED"};
            alCaptions = new string[] {"Status", "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Test", "No Of Attempts","Score %","Result","Date Attended"};
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED"};
            alCaptions = new string[] { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed"};
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.TrainingneedsEmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

        lblfname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
        //lblmname.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
        //lbllname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
        txtfileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
        lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();

        dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeedsDetailSearch(General.GetNullableInteger(ViewState["employeeid"].ToString()),
           General.GetNullableInteger(ViewState["coursetype"].ToString()),
                         sortexpression, sortdirection,
                         Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                         gvOffshoreTraining.PageSize,
                         ref iRowCount,
                         ref iTotalPageCount, 1, "u");

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Pending Training Needs", alCaptions, alColumns, ds);
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
    //        HeaderCell.ColumnSpan = 4;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 4;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "To be Completed by Manning Office / Agent";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
    //            HeaderCell.ColumnSpan = 8;
    //        else
    //            HeaderCell.ColumnSpan = 5;
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvOffshoreTraining.Rebind();
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

    protected void rbtoption_SelectedIndexChanged(object sender, EventArgs e)
    {
        string coursetype;
        if (rbtoption.SelectedIndex == 0)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 1)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeedDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 2)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeedDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
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

                BindData();
                ((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(gvOffshoreTraining.MasterTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindData();
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
                Guid trainingneedid = new Guid(gvOffshoreTraining.MasterTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());

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


                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("INITIATEEXAMREQ"))
            {
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 0);

                BindData();
            }
            else if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 1);

                BindData();
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
           
            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("TI14").Visible = false;// e.Item.Cells[14].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("NTI15").Visible = false;  //e.Item.Cells[15].Visible = false;
            }
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("Status1").Visible = false;// e.Item.Cells[1].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Material12").Visible = false;  //e.Item.Cells[12].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("test17").Visible = false; // e.Item.Cells[17].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("attempt18").Visible = false; // e.Item.Cells[18].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("score19").Visible = false; // e.Item.Cells[19].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("result20").Visible = false; // e.Item.Cells[20].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("DA21").Visible = false; //e.Item.Cells[21].Visible = false;
                lblRed.Visible = false;
                lblOverDue.Visible = false;
                lblGreen.Visible = false;
                lblDue.Visible = false;
            }

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
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                    eb.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                   
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                    att.Visible = false;
            }
            LinkButton cmdMaterial = (LinkButton)e.Item.FindControl("cmdMaterial");
            if (cmdMaterial != null)
            {
                cmdMaterial.Visible = SessionUtil.CanAccess(this.ViewState, cmdMaterial.CommandName);
                cmdMaterial.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDCOURSEDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&U=false'); return true;");
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");

            LinkButton db2 = (LinkButton)e.Item.FindControl("cmdDeArchive");


            LinkButton cmdCourseReq = (LinkButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }
            LinkButton cmdExamReqHistory = (LinkButton)e.Item.FindControl("cmdExamReqHistory");

            if (cmdExamReqHistory != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReqHistory.CommandName))
                    cmdExamReqHistory.Visible = false;

                cmdExamReqHistory.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreInitiateExamRequest.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString()
                    + "&examrequestid=" + dr["FLDEXAMREQUESTID"].ToString()
                    + "&courseid=" + dr["FLDCOURSEID"].ToString()
                    + "&examid=" + dr["FLDEXAMID"].ToString()
                     + "&employeeid=" + ViewState["employeeid"].ToString() +
                    "'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                    cmdExamReqHistory.Visible = false;
            }
            LinkButton cmdExamReq = (LinkButton)e.Item.FindControl("cmdExamReq");
            if (cmdExamReq != null)
            {
                if (dr["FLDACTIVEEXAMYN"].ToString().Equals("0"))
                    cmdExamReq.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReq.CommandName))
                    cmdExamReq.Visible = false;
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7") || PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    cmdExamReq.Visible = false;
                cmdExamReq.Attributes.Add("onclick", "javascript:parent.openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreExamQuestion.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString() + "&examid=" + dr["FLDEXAMID"].ToString()
                     + "&courseid=" + dr["FLDCOURSEID"].ToString() + "'); return true;");
            }
            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            LinkButton imgCourseName = (LinkButton)e.Item.FindControl("imgCourseName");
            if (imgCourseName != null)
            {
                if (lblCourseName != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    if (lblCourseName.Text != "")
                    {
                        

                        html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-glasses\"></i></span>";
                        imgCourseName.Controls.Add(html);

                        //imgWD.Visible = true;
                        //imgCourseName.ImageUrl = Session["images"] + "/te_view.png";
                        if (ucToolTipCourseName != null)
                        {
                            ucToolTipCourseName.Position = ToolTipPosition.TopCenter;
                            ucToolTipCourseName.TargetControlId = imgCourseName.ClientID;
                            //imgCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'visible');");
                            //imgCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-glasses\"></i></span>";
                        imgCourseName.Controls.Add(html);
                    }
                        
                }
            }

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
            }
            LinkButton cmdOverride = (LinkButton)e.Item.FindControl("cmdOverride");
            if (cmdOverride != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOverride.CommandName))
                    cmdOverride.Visible = false;

                cmdOverride.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingNeedOverride.aspx?empid=" + lblEmployeeid.Text + "&trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            Image imgFlagP = (Image)e.Item.FindControl("ImgFlagP");
            if (dr["FLDTRAININGNEEDSTATUS"] != null && dr["FLDTRAININGNEEDSTATUS"].ToString() != "")
            {
                if (dr["FLDTRAININGNEEDSTATUS"].ToString().Equals("1"))
                {
                    imgFlagP.ImageUrl = Session["images"] + "/red-symbol.png";
                    imgFlagP.Visible = true;
                    if (dr["FLDTOBEDONEBY"].ToString() == "")
                    {
                        imgFlagP.Visible = false;
                    }
                }
                else if (dr["FLDTRAININGNEEDSTATUS"].ToString().Equals("0"))
                {
                    imgFlagP.ImageUrl = Session["images"] + "/green-symbol.png";
                    imgFlagP.Visible = true;
                    if (dr["FLDTOBEDONEBY"].ToString() == "")
                    {
                        imgFlagP.Visible = false;
                    }
                }

            }
            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucCategory");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblCategory.ClientID;
                //lblCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //lblCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");
            UserControlToolTip ucst = (UserControlToolTip)e.Item.FindControl("ucSubCategory");
            if (ucst != null)
            {
                ucst.Position = ToolTipPosition.TopCenter;
                ucst.TargetControlId = lblSubCategory.ClientID;
                //lblSubCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucst.ToolTip + "', 'visible');");
                //lblSubCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucst.ToolTip + "', 'hidden');");
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

            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }
}
