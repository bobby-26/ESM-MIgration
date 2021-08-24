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
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class Portal_PortalSeafarerTrainingOverdue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                rbtoption.SelectedIndex = 3;
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


            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCompletedTrainingNeedDetail.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddLinkButton("../CrewOffshore/CrewOffshoreTrainingNeedsCompletedSearch.aspx?PendingNeedsYN=0&Vessel=0&Override=0", "Filter", "search.png", "SEARCH");
            //toolbar.AddLinkButton("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();



            if (Request.QueryString["employeeid"].ToString() != null && Request.QueryString["employeeid"].ToString() != "")
            {
                ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        ddl.DataTextField = "FLDQUICKNAME";
        ddl.DataValueField = "FLDQUICKCODE";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
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
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDEXAMNAME","FLDNOOFATTEMPTS","FLDSCORE","FLDEXAMRESULT","FLDDATEATTENDED"};
            alCaptions = new string[] { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
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



        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.TrainingNeedsOverdueCompleteCBTSearch(General.GetNullableInteger(ViewState["employeeid"].ToString()),
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            General.GetNullableInteger(ViewState["coursetype"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount, 0, "u");

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreCompletedTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training Needs</h3></td>");
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
            Response.Redirect("../Portal/PortalSeafarerTrainingCBT.aspx?empid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("TRAINING COURSE"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../Portal/PortalSeafarerTrainingNeeds.aspx?empid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("BACK"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../Portal/PortalSeafarerTraining.aspx?empid=" + ViewState["employeeid"].ToString(), true);
        }
        else if (CommandName.ToUpper().Equals("TASK"))
        {
            Response.Redirect("../Portal/PortalSeafarerTrainingTasks.aspx?employeeid=" + ViewState["employeeid"].ToString(), true);
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

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDEXAMNAME","FLDNOOFATTEMPTS","FLDSCORE","FLDEXAMRESULT","FLDDATEATTENDED"};
            alCaptions = new string[] { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
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

        dt = PhoenixCrewOffshoreTrainingNeeds.TrainingNeedsOverdueCompleteCBTSearch(General.GetNullableInteger(ViewState["employeeid"].ToString()),
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
           General.GetNullableInteger(ViewState["coursetype"].ToString()),
               sortexpression, sortdirection,
               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
              gvOffshoreTraining.PageSize,
               ref iRowCount,
               ref iTotalPageCount
               , 0, "u");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Completed Training Needs", alCaptions, alColumns, ds);
        gvOffshoreTraining.DataSource = ds;
        gvOffshoreTraining.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }









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

    protected void rbtoption_SelectedIndexChanged(object sender, EventArgs e)
    {
        string coursetype;
        if (rbtoption.SelectedIndex == 0)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../Portal/PortalSeafarerTrainingNeeds.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 1)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../Portal/PortalSeafarerTrainingCompleted.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 2)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../Portal/PortalSeafarerTrainingOverride.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 3)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../Portal/PortalSeafarerTrainingOverdue.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }

    }

    protected void gvOffshoreTraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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

    protected void gvOffshoreTraining_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                Guid trainingneedid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTRAININGNEEDID"].ToString());
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


                BindData();
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
            else if (e.CommandName.ToUpper().Equals("CERTIFICATE"))
            {
                //Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());               
                //BindData();
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

    protected void gvOffshoreTraining_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("TIC12").Visible = false; //e.Item.Cells[12].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("NTI13").Visible = false; //e.Item.Cells[13].Visible = false;
            }
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            {
                // e.Item.Cells[11].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("test15").Visible = false; //e.Item.Cells[15].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("attempt16").Visible = false; //e.Item.Cells[16].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("score17").Visible = false; //e.Item.Cells[17].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("result18").Visible = false; //e.Item.Cells[18].Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("attended19").Visible = false;// e.Item.Cells[19].Visible = false;
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
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }


                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                    att.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            LinkButton db2 = (LinkButton)e.Item.FindControl("cmdDeArchive");

            LinkButton cmdCourseReq = (LinkButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }
            LinkButton cmdExamReqHistory = (LinkButton)e.Item.FindControl("cmdExamReqHistory");

            if (cmdExamReqHistory != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReqHistory.CommandName))
                    cmdExamReqHistory.Visible = false;

                cmdExamReqHistory.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreInitiateExamRequest.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString()
                    + "&examrequestid=" + dr["FLDEXAMREQUESTID"].ToString()
                    + "&courseid=" + dr["FLDCOURSEID"].ToString()
                    + "&examid=" + dr["FLDEXAMID"].ToString() +
                    "'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                    cmdExamReqHistory.Visible = false;
            }
            LinkButton imgCertificate = (LinkButton)e.Item.FindControl("imgCertificate");
            if (imgCertificate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCertificate.CommandName))
                    imgCertificate.Visible = false;
                imgCertificate.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Reports/ReportsView.aspx?applicationcode=11&reportcode=TRAININGCOURSECERTIFICATE&examrequestid=" + dr["FLDEXAMREQUESTID"].ToString() + "&showmenu=0&showword=NO&showexcel=NO'); return true;");
            }

            if (dr["FLDEXAMRESULT"].ToString().ToUpper().Equals("PASS"))
            {
                if (imgCertificate != null)
                    imgCertificate.Visible = true;
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
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
        if (e.Item is GridDataItem)
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