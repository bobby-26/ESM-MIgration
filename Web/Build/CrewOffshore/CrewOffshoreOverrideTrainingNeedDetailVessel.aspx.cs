using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreOverrideTrainingNeedDetailVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ucConfirmCrew.Attributes.Add("style", "display:none");
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
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Pending Training Needs", "PENDINGTRAININGNEED");
            toolbarsub.AddButton("Completed Training Needs", "COMPLETEDTRAININGNEED");
            toolbarsub.AddButton("Overridden Training Needs", "OVERRIDDENTRAININGNEED");
            TrainingNeed.AccessRights = this.ViewState;
            TrainingNeed.MenuList = toolbarsub.Show();
            TrainingNeed.SelectedMenuIndex = 2;

            string vesselyn = "0";
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                vesselyn = "1";
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            }
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingNeedsSearch.aspx?PendingNeedsYN=0&Vessel=" + vesselyn + "&Override=1", "Filter", "search.png", "SEARCH");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

            PhoenixToolbar toolbarcourse = new PhoenixToolbar();
            toolbarcourse.AddButton("CBT", "CBT");
            toolbarcourse.AddButton("Training Course", "HSEQA");

            //CourseRequest.AccessRights = this.ViewState;
            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            //{
            //    CourseRequest.TabStrip = "false";
            //    CourseRequest.MenuList = toolbarcourse.Show();
            //    if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
            //        CourseRequest.SelectedMenuIndex = 0;
            //    else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            //        CourseRequest.SelectedMenuIndex = 1;
            //    else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "8"))
            //        CourseRequest.SelectedMenuIndex = 2;
            //}
            if (Request.QueryString["employeeid"].ToString() != null && Request.QueryString["employeeid"].ToString() != "")
            {
                ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
            }
            PhoenixToolbar toobarDetail = new PhoenixToolbar();
            toobarDetail.AddButton("CBT Detail", "CBTDETAIL");
            toobarDetail.AddButton("Training Course Detail", "TQCOURCEDETAIL");
            toobarDetail.AddButton("Back", "BACK");

            //TrainingNeedDetail.AccessRights = this.ViewState;
            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            //{
            //    TrainingNeedDetail.TabStrip = "false";
            //    TrainingNeedDetail.MenuList = toobarDetail.Show();
            //    if (ViewState["coursetype"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
            //    {
            //        TrainingNeedDetail.SelectedMenuIndex = 0;
            //    }
            //    else
            //    {
            //        TrainingNeedDetail.SelectedMenuIndex = 1;
            //    }
            //}

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
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("SEAGULL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "8");
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
    }
    //protected void TrainingNeedDetail_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dtl = (DataListCommandEventArgs)e;
    //    string coursetype = "";

    //    if (dtl.CommandName.ToUpper().Equals("CBTDETAIL"))
    //    {
    //        coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
    //        Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeedDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
    //        TrainingNeedDetail.SelectedMenuIndex = 1;
    //    }

    //    if (dtl.CommandName.ToUpper().Equals("TQCOURCEDETAIL"))
    //    {
    //        coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
    //        Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeedDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
    //        TrainingNeedDetail.SelectedMenuIndex = 1;
    //    }

    //    if (dtl.CommandName.ToUpper().Equals("BACK"))
    //    {
    //        Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx?coursetype=" + Request.QueryString["coursetype"], true);
    //    }
    //}

    protected void BindCategory(DropDownList ddl)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
        ddl.DataTextField = "FLDCATEGORYNAME";
        ddl.DataValueField = "FLDCATEGORYID";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindSubCategory(DropDownList ddl, string categoryid)
    {
        ddl.Items.Clear();
        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingSubCategory(General.GetNullableInteger(categoryid));
        ddl.DataTextField = "FLDNAME";
        ddl.DataValueField = "FLDSUBCATEGORYID";
        ddl.DataSource = dt;
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindToBeDoneBy(DropDownList ddl, int? typeoftraining)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingNeeds.ListTrainingToBeDoneBy(130, typeoftraining);
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME","FLDOVERRIDE"};
        string[] alCaptions = { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by","Override Y/N"};
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
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            1, "u"
                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreOverrideTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Overridden Training Needs</h3></td>");
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
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeedVessel.aspx", true);
            else
                Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("PENDINGTRAININGNEED"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsVessel.aspx", true);
            else
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx", true);
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
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME","FLDOVERRIDE"};
        string[] alCaptions = { "Name", "Rank", "File No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by","Override Y/N"};

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

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeedsDetailSearch(null,//General.GetNullableInteger(ViewState["employeeid"].ToString()),
            General.GetNullableInteger(ViewState["coursetype"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount
                , 1
                , 1
                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Overridden Training Needs", alCaptions, alColumns, ds);

        gvOffshoreTraining.DataSource = ds;
        gvOffshoreTraining.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ucTitle.Text.Contains(ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString()) == false)
            {
                ucTitle.Text = ucTitle.Text + " [" + ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString() + "]";
            }
        }
        else
        {
            //   ShowNoRecordsFound(dt, gvOffshoreTraining);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvOffshoreTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Item.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
                if (dr["FLDISRAISEDFROMCBT"].ToString() == "1")
                    ddlCategoryEdit.CssClass = "input";
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
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
            //DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            //if (ddlDonebyEdit != null)
            //{
            //    if (ucTypeofTrainingEdit != null)
            //        BindToBeDoneBy(ddlDonebyEdit, ucTypeofTrainingEdit.SelectedQuick));
            //    ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            //}

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
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName))
                    att.Visible = false;
                //att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }
            ImageButton cmdMaterial = (ImageButton)e.Item.FindControl("cmdMaterial");
            if (cmdMaterial != null)
            {
                cmdMaterial.Visible = SessionUtil.CanAccess(this.ViewState, cmdMaterial.CommandName);
                cmdMaterial.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDCOURSEDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&U=false'); return true;");
            }

            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");


            ImageButton db2 = (ImageButton)e.Item.FindControl("cmdDeArchive");

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

                cmdExamReqHistory.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreInitiateExamRequest.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString()
                    + "&examrequestid=" + dr["FLDEXAMREQUESTID"].ToString()
                    + "&courseid=" + dr["FLDCOURSEID"].ToString()
                    + "&examid=" + dr["FLDEXAMID"].ToString() +
                    "'); return true;");
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                    cmdExamReqHistory.Visible = false;
            }
            ImageButton cmdExamReq = (ImageButton)e.Item.FindControl("cmdExamReq");
            if (cmdExamReq != null)
            {
                if (dr["FLDACTIVEEXAMYN"].ToString().Equals("0"))
                    cmdExamReq.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReq.CommandName))
                    cmdExamReq.Visible = false;
                if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                    cmdExamReq.Visible = false;
                cmdExamReq.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreExamQuestion.aspx?courserequestid=" + dr["FLDCOURSEREQUESTID"].ToString() + "&examid=" + dr["FLDEXAMID"].ToString()
                     + "&courseid=" + dr["FLDCOURSEID"].ToString() + "'); return true;");
            }
            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Item.FindControl("ucToolTipCourseName");
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            //ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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
            if (lnkEployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
            }
            //ImageButton cmdOverride = (ImageButton)e.Item.FindControl("cmdOverride");
            //if (cmdOverride != null)
            //{
            //    if (!SessionUtil.CanAccess(this.ViewState, cmdOverride.CommandName))
            //        cmdOverride.Visible = false;

            //    // cmdOverride.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','../CrewOffshore/CrewOffshoreTrainingNeedOverride.aspx?empid=" + lblEmployeeid.Text + "&trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");

            //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            //        cmdOverride.ToolTip = "Show Override Remarks";
            //}
            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucCategory");
            if (uct != null)
            {
                lblCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");
            UserControlToolTip ucst = (UserControlToolTip)e.Item.FindControl("ucSubCategory");
            if (ucst != null)
            {
                lblSubCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucst.ToolTip + "', 'visible');");
                lblSubCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucst.ToolTip + "', 'hidden');");
            }

        }
        if (e.Item is GridFooterItem)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Item.FindControl("ddlCategoryAdd");
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

                gvOffshoreTraining.Rebind();
                ((DropDownList)e.Item.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text);
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)e.Item.FindControl("ucCompletionDate")).Text
                    , ((RadLabel)e.Item.FindControl("lblIsRaisedFromCBTEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text);

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

                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("INITIATEEXAMREQ"))
            {
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 0);

                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId")).Text;
                PhoenixCrewOffshoreTrainingNeeds.ArchiveTrainingNeed(General.GetNullableGuid(trainingneedid), 1);

                gvOffshoreTraining.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "OVERRIDE")
            {
                RadLabel trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId"));
                ViewState["trainingneedid"] = trainingneedid.Text;
                UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

                RadWindowManager1.RadConfirm("Do you want to cancel override for this course?", "ConfirmRevision", 320, 150, null, "Remarks");
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
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            //HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 11;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshoreTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
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
        DropDownList ddl = (DropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        DropDownList ddlSubCategoryEdit = (DropDownList)gvrow.FindControl("ddlSubCategoryEdit");
        if (ddlSubCategoryEdit != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryEdit, ddl.SelectedValue);
        }
    }

    protected void ddlCategoryAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        DropDownList ddlSubCategoryAdd = (DropDownList)gvrow.FindControl("ddlSubCategoryAdd");
        if (ddlSubCategoryAdd != null)
        {
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
                BindSubCategory(ddlSubCategoryAdd, ddl.SelectedValue);
        }
    }
    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeedOverride(General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                   , 0//General.GetNullableInteger(ChkOverrideYN.Checked == true ? "1" : "0")                       
                   , General.GetNullableString(""));
            gvOffshoreTraining.Rebind();

            ucStatus.Text = "Overrided cancel successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
