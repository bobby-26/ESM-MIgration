using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingNeedsVessel : PhoenixBasePage
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
                ViewState["examrequestid"] = "";
                ViewState["VESSELID"] = "";
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");

                gvOffshoreTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Pending Training Needs", "PENDINGTRAININGNEED");
            toolbarsub.AddButton("Completed Training Needs", "COMPLETEDTRAININGNEED");
            toolbarsub.AddButton("Overridden Training Needs", "OVERRIDDENTRAININGNEED");
            TrainingNeed.AccessRights = this.ViewState;
            TrainingNeed.MenuList = toolbarsub.Show();
            TrainingNeed.SelectedMenuIndex = 0;

            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingNeedsVessel.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingNeedsVesselSearch.aspx?PendingNeedsYN=1&Vessel=1&Override=0", "Filter", "search.png", "SEARCH");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingNeedsVessel.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageLink("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingNeedsAdd.aspx')", "Add", "add.png", "ADDTRAINING");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCategory(RadDropDownList ddl)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
        ddl.DataTextField = "FLDCATEGORYNAME";
        ddl.DataValueField = "FLDCATEGORYID";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new DropDownListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindSubCategory(RadDropDownList ddl, string categoryid)
    {
        ddl.Items.Clear();
        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingSubCategory(General.GetNullableInteger(categoryid));
        ddl.DataTextField = "FLDNAME";
        ddl.DataValueField = "FLDSUBCATEGORYID";
        ddl.DataSource = dt;
        ddl.Items.Insert(0, new DropDownListItem("--Select--", ""));
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

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeedsVessel(null,
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            nvc != null ? nvc["txtName"] : "",
                            nvc != null ? nvc["txtFileNo"] : "",
                            General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : ""),
                            1,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()),
                            vesselid
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

        if (CommandName.ToUpper().Equals("COMPLETEDTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeedVessel.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("OVERRIDDENTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeedDetailVessel.aspx?employeeid=null&coursetype=" + ViewState["coursetype"].ToString(), true);
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
            alCaptions = new string[] {"Status",  "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
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

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeedsVessel(null,
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                nvc != null ? nvc["txtName"] : "",
                nvc != null ? nvc["txtFileNo"] : "",
                General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : ""),
                1,
                General.GetNullableInteger(ViewState["coursetype"].ToString()),

                vesselid

                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Pending Training Needs", alCaptions, alColumns, ds);


        gvOffshoreTraining.DataSource = ds;
        gvOffshoreTraining.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvOffshoreTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("DetailsofTrainingImparted").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("NameofTrainer").Visible = false;


                //e.Item.Cells[14].Visible = false;
                //e.Item.Cells[15].Visible = false;
            }
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            {

                gvOffshoreTraining.MasterTableView.GetColumn("Statusid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Tobedonebyid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Resultid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("DateAttendedid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("OverrideY/Nid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Actionid").Visible = false;
                //  gvOffshoreTraining.MasterTableView.GetColumn("Statusid").Visible = false;

                //e.Item.Cells[1].Visible = false;
                //e.Item.Cells[12].Visible = false;
                //e.Item.Cells[17].Visible = false;
                //e.Item.Cells[18].Visible = false;
                //e.Item.Cells[19].Visible = false;
                //e.Item.Cells[20].Visible = false;
                //e.Item.Cells[21].Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("DetailsofTrainingImparted").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("NameofTrainer").Visible = false;
                //e.Item.Cells[14].Visible = false;
                //e.Item.Cells[15].Visible = false;
            }
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            {
                gvOffshoreTraining.MasterTableView.GetColumn("Statusid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Tobedonebyid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Resultid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("DateAttendedid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("OverrideY/Nid").Visible = false;
                gvOffshoreTraining.MasterTableView.GetColumn("Actionid").Visible = false;

                //e.Item.Cells[1].Visible = false;
                //e.Item.Cells[12].Visible = false;
                //e.Item.Cells[17].Visible = false;
                //e.Item.Cells[18].Visible = false;
                //e.Item.Cells[19].Visible = false;
                //e.Item.Cells[20].Visible = false;
                //e.Item.Cells[21].Visible = false;
            }

            RadDropDownList ddlCategoryEdit = (RadDropDownList)e.Item.FindControl("ddlCategoryEdit");
            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            RadDropDownList ddlSubCategoryEdit = (RadDropDownList)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.Equals((dr["FLDSUBCATEGORY"].ToString()) != null))
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

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                    att.Visible = false;
            }
            ImageButton cmdMaterial = (ImageButton)e.Item.FindControl("cmdMaterial");
            if (cmdMaterial != null)
            {
                cmdMaterial.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                cmdMaterial.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDCOURSEDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&U=false'); return true;");
            }

            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            //if (db1 != null)
            //{
            //    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            //    if (dr["FLDACTIVEYN"].ToString() == "1") db1.Visible = true;
            //    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            //}

            ImageButton db2 = (ImageButton)e.Item.FindControl("cmdDeArchive");
            //if (db2 != null)
            //{
            //    db2.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
            //    if (dr["FLDACTIVEYN"].ToString() == "0") db2.Visible = true;
            //    if (!SessionUtil.CanAccess(this.ViewState, db2.CommandName)) db2.Visible = false;
            //}

            ImageButton cmdCourseReq = (ImageButton)e.Item.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }
            ImageButton cmdExamReqHistory = (ImageButton)e.Item.FindControl("cmdExamReqHistory");

            if (cmdExamReqHistory != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReqHistory.CommandName))
                    cmdExamReqHistory.Visible = false;

                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");

                cmdExamReqHistory.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreInitiateExamRequest.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString()
                    + "&examrequestid=" + dr["FLDEXAMREQUESTID"].ToString()
                    + "&courseid=" + dr["FLDCOURSEID"].ToString()
                    + "&examid=" + dr["FLDEXAMID"].ToString()
                    + "&employeeid=" + empid.Text +
                    "'); return true;");
            }
            ImageButton cmdExamReq = (ImageButton)e.Item.FindControl("cmdExamReq");
            if (cmdExamReq != null)
            {
                if (dr["FLDACTIVEEXAMYN"].ToString().Equals("0"))
                    cmdExamReq.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReq.CommandName))
                    cmdExamReq.Visible = false;

                if (dr["FLDISTESTYN"].ToString().Equals("0"))
                    cmdExamReq.Visible = false;

                cmdExamReq.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreExamQuestion.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString() + "&examid=" + dr["FLDEXAMID"].ToString()
                     + "&courseid=" + dr["FLDCOURSEID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Item.FindControl("imgCourseName");
            if (imgCourseName != null)
            {
                if (lblCourseName != null)
                {
                    if (lblCourseName.Text != "")
                    {
                        //imgWD.Visible = true;
                        imgCourseName.ImageUrl = Session["images"] + "/te_view.png";
                        if (ucToolTipCourseName != null)
                        {
                            imgCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'visible');");
                            imgCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipCourseName.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        imgCourseName.ImageUrl = Session["images"] + "/no-remarks.png";
                }
            }

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
            }
            ImageButton cmdOverride = (ImageButton)e.Item.FindControl("cmdOverride");

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
                else
                {
                    imgFlagP.Visible = false;

                }
            }

        }
        if (e.Item is GridFooterItem)
        {
            RadDropDownList ddlCategoryAdd = (RadDropDownList)e.Item.FindControl("ddlCategoryAdd");
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

    protected void gvOffshoreTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadDropDownList)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((RadDropDownList)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                gvOffshoreTraining.Rebind();
                ((RadDropDownList)e.Item.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text);
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadDropDownList)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((RadDropDownList)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text);

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlOffshoreToBeDoneBy)e.Item.FindControl("ucDonebyEdit")).SelectedToBeDoneBy),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text)
                    );

                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("INITIATEEXAMREQ"))
            {
                //string ExamRequestId = "";
                //string CourseRequestId = "";
                //ExamRequestId = ((RadLabel)e.Item.FindControl("lblExamRequestId")).Text;
                //CourseRequestId = ((RadLabel)e.Item.FindControl("lblCourseRequestId")).Text;

                //Guid? newid = null;
                //PhoenixCrewOffshoreExamRequest.InsertExamRequest(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCourseRequestId")).Text)
                //                                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblExamID")).Text)
                //                                    , General.GetNullableDateTime(DateTime.Now.ToString())
                //                                    , ref newid
                //                                    );

                //    ViewState["examrequestid"] = newid;
                //    ucStatus.Text = "Exam request initiated successfully.";                    
                //    BindData();
                //    //SetPageNavigator();
                //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //    //                     "BookMarkScript", "fnReloadList('chml', null, true);", true);

            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 0);
                //_gridView.EditIndex = -1;
                //_gridView.SelectedIndex = -1;
                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 1);
                //_gridView.EditIndex = -1;
                //_gridView.SelectedIndex = -1;
                gvOffshoreTraining.Rebind();
            }
            //else if (e.CommandName == "Page")
            //{
            //    ViewState["PAGENUMBER"] = null;
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreTraining_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "As reported by the vessel";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 7;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                HeaderCell.ColumnSpan = 8;
            else
                HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshoreTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    private bool IsValidData(string Category, string SubCategory, string trainingneed, string improvementlevel, string completiondate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableInteger(SubCategory) == null)
            ucError.ErrorMessage = "SubCategory is required.";

        if (General.GetNullableString(trainingneed) == null)
            ucError.ErrorMessage = "Identified training need is required.";

        if (General.GetNullableInteger(improvementlevel) == null)
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
        RadDropDownList ddl = (RadDropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        RadDropDownList ddlSubCategoryEdit = (RadDropDownList)gvrow.FindControl("ddlSubCategoryEdit");
        if (ddlSubCategoryEdit != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryEdit, ddl.SelectedValue);
        }
    }

    protected void ddlCategoryAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadDropDownList ddl = (RadDropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        RadDropDownList ddlSubCategoryAdd = (RadDropDownList)gvrow.FindControl("ddlSubCategoryAdd");
        if (ddlSubCategoryAdd != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryAdd, ddl.SelectedValue);
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
}
