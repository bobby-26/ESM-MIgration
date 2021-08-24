using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;

public partial class CrewOffshoreRecommendedTraining : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();           

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;                
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                ViewState["PAGENUMBERPCBT"] = 1;
                ViewState["PAGENUMBERPTC"] = 1;
                ViewState["PAGENUMBERCCBT"] = 1;
                ViewState["PAGENUMBERCTC"] = 1;
                ViewState["Trainingcoursetype"] = "";

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["empid"].ToString();
               
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                {
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
                    ViewState["Trainingcoursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
                }

                SetEmployeePrimaryDetails();                
            }

            PhoenixToolbar toolbarPCBT = new PhoenixToolbar();
            toolbarPCBT.AddImageButton("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbarPCBT.AddImageLink("javascript:CallPrint('gvOffshorePCBTTraining')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshorePendingCBTTraining.AccessRights = this.ViewState;
            MenuOffshorePendingCBTTraining.MenuList = toolbarPCBT.Show();

            PhoenixToolbar toolbarPTC = new PhoenixToolbar();
            toolbarPTC.AddImageButton("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbarPTC.AddImageLink("javascript:CallPrint('gvOffshorePCourseTraining')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshorePendingCourseTraining.AccessRights = this.ViewState;
            MenuOffshorePendingCourseTraining.MenuList = toolbarPTC.Show();

            PhoenixToolbar toolbarCCBT = new PhoenixToolbar();
            toolbarCCBT.AddImageButton("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbarCCBT.AddImageLink("javascript:CallPrint('gvOffshoreCCBTTraining')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreCompletedCBTTraining.AccessRights = this.ViewState;
            MenuOffshoreCompletedCBTTraining.MenuList = toolbarCCBT.Show();

            PhoenixToolbar toolbarCTC = new PhoenixToolbar();
            toolbarCTC.AddImageButton("../CrewOffshore/CrewOffshoreRecommendedTraining.aspx?coursetype=" + ViewState["coursetype"].ToString() + "&empid=" + ViewState["employeeid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbarCTC.AddImageLink("javascript:CallPrint('gvOffshoreCCourseTraining')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreCompletedCourseTraining.AccessRights = this.ViewState;
            MenuOffshoreCompletedCourseTraining.MenuList = toolbarCTC.Show();

            BindData();
            SetPageNavigator();
            BindDataPCBT();
            SetPageNavigatorPCBT();
            BindDataPTC();
            SetPageNavigatorPTC();
            BindDataCCBT();
            SetPageNavigatorCCBT();
            BindDataCTC();
            SetPageNavigatorCTC();
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
        ddl.DataTextField = "FLDQUICKNAME";
        ddl.DataValueField = "FLDQUICKCODE";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED" };
        string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
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

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(int.Parse(ViewState["employeeid"].ToString()),
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreTrainingNeeds.xls");
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

    protected void MenuOffshoreTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED" };
        string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(int.Parse(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Training Needs", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreTraining.DataSource = ds;
            gvOffshoreTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreTraining);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvOffshoreTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvOffshoreTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Row.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Row.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Row.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Row.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            if (ddlDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                    BindToBeDoneBy(ddlDonebyEdit,General.GetNullableInteger(ucTypeofTrainingEdit.SelectedQuick)); 
                ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Row.FindControl("cmdCourseReq");
            if(cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Row.FindControl("ucToolTipCourseName");
            Label lblCourseName = (Label)e.Row.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Row.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Row.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindData();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoneByEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text)
                    );

                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
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
            HeaderCell.Text = "To be assessed by Company Training Officer";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
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

    /* Pending CBT Training*/
    protected void ShowExcelPCBT()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };
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
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            1,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=PendingCBTTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending CBT Training Needs</h3></td>");
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelPCBT();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataPCBT();
                SetPageNavigatorPCBT();
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
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                1,
                General.GetNullableInteger(ViewState["coursetype"].ToString())
                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshorePCBTTraining", "Pending CBT Training Needs", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshorePCBTTraining.DataSource = ds;
            gvOffshorePCBTTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshorePCBTTraining);
        }

        ViewState["ROWCOUNTPCBT"] = iRowCount;
        ViewState["TOTALPAGECOUNTPCBT"] = iTotalPageCount;
        SetPageNavigatorPCBT();
    }
    protected void gvOffshorePCBTTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindDataPCBT();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
            SetPageNavigatorPCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCBTTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataPCBT();
    }

    protected void gvOffshorePCBTTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Row.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Row.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Row.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Row.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            //UserControlQuick ucDonebyEdit = (UserControlQuick)e.Row.FindControl("ucDonebyEdit");
            //if (ucDonebyEdit != null)
            //{
            //    ucDonebyEdit.bind();
            //    ucDonebyEdit.SelectedQuick = dr["FLDTOBEDONEBY"].ToString();
            //}
            DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            if (ddlDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                    BindToBeDoneBy(ddlDonebyEdit, General.GetNullableInteger(ucTypeofTrainingEdit.SelectedQuick));
                ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Row.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Row.FindControl("ucToolTipCourseName");
            Label lblCourseName = (Label)e.Row.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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

            LinkButton lnkEployeeName = (LinkButton)e.Row.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Row.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Row.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshorePCBTTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataPCBT();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataPCBT();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoneByEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text)
                    );

                _gridView.EditIndex = -1;
                BindDataPCBT();
            }
            SetPageNavigatorPCBT();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshorePCBTTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindDataPCBT();
            SetPageNavigatorPCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshorePCBTTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataPCBT();
        SetPageNavigatorPCBT();
    }

    protected void gvOffshorePCBTTraining_RowCreated(object sender, GridViewRowEventArgs e)
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
            HeaderCell.Text = "To be assessed by Company Training Officer";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshorePCBTTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }    

    protected void cmdGoPCBT_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshorePCBTTraining.SelectedIndex = -1;
        gvOffshorePCBTTraining.EditIndex = -1;
        if (Int32.TryParse(txtnopagePCBT.Text, out result))
        {
            ViewState["PAGENUMBERPCBT"] = Int32.Parse(txtnopagePCBT.Text);

            if ((int)ViewState["TOTALPAGECOUNTPCBT"] < Int32.Parse(txtnopagePCBT.Text))
                ViewState["PAGENUMBERPCBT"] = ViewState["TOTALPAGECOUNTPCBT"];


            if (0 >= Int32.Parse(txtnopagePCBT.Text))
                ViewState["PAGENUMBERPCBT"] = 1;

            if ((int)ViewState["PAGENUMBERPCBT"] == 0)
                ViewState["PAGENUMBERPCBT"] = 1;

            txtnopagePCBT.Text = ViewState["PAGENUMBERPCBT"].ToString();
        }
        BindDataPCBT();
        SetPageNavigatorPCBT();
    }

    protected void PagerButtonClickPCBT(object sender, CommandEventArgs ce)
    {
        gvOffshorePCBTTraining.SelectedIndex = -1;
        gvOffshorePCBTTraining.EditIndex = -1;
        if (ce.CommandName == "prevPCBT")
            ViewState["PAGENUMBERPCBT"] = (int)ViewState["PAGENUMBERPCBT"] - 1;
        else
            ViewState["PAGENUMBERPCBT"] = (int)ViewState["PAGENUMBERPCBT"] + 1;

        BindDataPCBT();
        SetPageNavigatorPCBT();
    }

    private void SetPageNavigatorPCBT()
    {
        cmdPreviousPCBT.Enabled = IsPreviousEnabledPCBT();
        cmdNextPCBT.Enabled = IsNextEnabledPCBT();
        lblPagenumberPCBT.Text = "Page " + ViewState["PAGENUMBERPCBT"].ToString();
        lblPagesPCBT.Text = " of " + ViewState["TOTALPAGECOUNTPCBT"].ToString() + " Pages. ";
        lblRecordsPCBT.Text = "(" + ViewState["ROWCOUNTPCBT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledPCBT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERPCBT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTPCBT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledPCBT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERPCBT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTPCBT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }


    /* Pending Training Course */
    protected void ShowExcelPTC()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };
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
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelPTC();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataPTC();
                SetPageNavigatorPTC();
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
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
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

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshorePCourseTraining.DataSource = ds;
            gvOffshorePCourseTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshorePCourseTraining);
        }

        ViewState["ROWCOUNTPTC"] = iRowCount;
        ViewState["TOTALPAGECOUNTPTC"] = iTotalPageCount;
        SetPageNavigatorPTC();
    }
    protected void gvOffshorePCourseTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindDataPTC();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
            SetPageNavigatorPTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCourseTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataPTC();
    }

    protected void gvOffshorePCourseTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Row.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Row.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Row.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Row.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            //UserControlQuick ucDonebyEdit = (UserControlQuick)e.Row.FindControl("ucDonebyEdit");
            //if (ucDonebyEdit != null)
            //{
            //    ucDonebyEdit.bind();
            //    ucDonebyEdit.SelectedQuick = dr["FLDTOBEDONEBY"].ToString();
            //}
            DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            if (ddlDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                    BindToBeDoneBy(ddlDonebyEdit, General.GetNullableInteger(ucTypeofTrainingEdit.SelectedQuick));
                ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Row.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Row.FindControl("ucToolTipCourseName");
            Label lblCourseName = (Label)e.Row.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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

            LinkButton lnkEployeeName = (LinkButton)e.Row.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Row.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Row.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshorePCourseTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataPTC();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataPTC();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoneByEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text)
                    );

                _gridView.EditIndex = -1;
                BindDataPTC();
            }
            SetPageNavigatorPTC();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshorePCourseTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindDataPTC();
            SetPageNavigatorPTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshorePCourseTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataPTC();
        SetPageNavigatorPTC();
    }

    protected void gvOffshorePCourseTraining_RowCreated(object sender, GridViewRowEventArgs e)
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
            HeaderCell.Text = "To be assessed by Company Training Officer";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshorePCourseTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }
    protected void cmdGoPTC_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshorePCourseTraining.SelectedIndex = -1;
        gvOffshorePCourseTraining.EditIndex = -1;
        if (Int32.TryParse(txtnopagePTC.Text, out result))
        {
            ViewState["PAGENUMBERPTC"] = Int32.Parse(txtnopagePTC.Text);

            if ((int)ViewState["TOTALPAGECOUNTPTC"] < Int32.Parse(txtnopagePTC.Text))
                ViewState["PAGENUMBERPTC"] = ViewState["TOTALPAGECOUNTPTC"];


            if (0 >= Int32.Parse(txtnopagePTC.Text))
                ViewState["PAGENUMBERPTC"] = 1;

            if ((int)ViewState["PAGENUMBERPTC"] == 0)
                ViewState["PAGENUMBERPTC"] = 1;

            txtnopagePTC.Text = ViewState["PAGENUMBERPTC"].ToString();
        }
        BindDataPTC();
        SetPageNavigatorPTC();
    }

    protected void PagerButtonClickPTC(object sender, CommandEventArgs ce)
    {
        gvOffshorePCourseTraining.SelectedIndex = -1;
        gvOffshorePCourseTraining.EditIndex = -1;
        if (ce.CommandName == "prevPTC")
            ViewState["PAGENUMBERPTC"] = (int)ViewState["PAGENUMBERPTC"] - 1;
        else
            ViewState["PAGENUMBERPTC"] = (int)ViewState["PAGENUMBERPTC"] + 1;

        BindDataPTC();
        SetPageNavigatorPTC();
    }

    private void SetPageNavigatorPTC()
    {
        cmdPreviousPTC.Enabled = IsPreviousEnabledPTC();
        cmdNextPTC.Enabled = IsNextEnabledPTC();
        lblPagenumberPTC.Text = "Page " + ViewState["PAGENUMBERPTC"].ToString();
        lblPagesPTC.Text = " of " + ViewState["TOTALPAGECOUNTPTC"].ToString() + " Pages. ";
        lblRecordsPTC.Text = "(" + ViewState["ROWCOUNTPTC"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledPTC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERPTC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTPTC"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledPTC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERPTC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTPTC"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    /* Completed CBT Training*/
    protected void ShowExcelCCBT()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };
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
                            Int32.Parse(ViewState["PAGENUMBERCCBT"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            0,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=CompletedCBTTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Completed CBT Training Needs</h3></td>");
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

    protected void MenuOffshoreCompletedCBTTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelCCBT();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataCCBT();
                SetPageNavigatorCCBT();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
   
    private void BindDataCCBT()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERCCBT"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                0,
                General.GetNullableInteger(ViewState["coursetype"].ToString())
                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreCCBTTraining", "Completed CBT Training Needs", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreCCBTTraining.DataSource = ds;
            gvOffshoreCCBTTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreCCBTTraining);
        }

        ViewState["ROWCOUNTCCBT"] = iRowCount;
        ViewState["TOTALPAGECOUNTCCBT"] = iTotalPageCount;
        SetPageNavigatorCCBT();
    }

    protected void gvOffshoreCCBTTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindDataCCBT();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
            SetPageNavigatorCCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreCCBTTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataCCBT();
    }

    protected void gvOffshoreCCBTTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Row.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Row.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Row.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Row.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            //UserControlQuick ucDonebyEdit = (UserControlQuick)e.Row.FindControl("ucDonebyEdit");
            //if (ucDonebyEdit != null)
            //{
            //    ucDonebyEdit.bind();
            //    ucDonebyEdit.SelectedQuick = dr["FLDTOBEDONEBY"].ToString();
            //}
            DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            if (ddlDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                    BindToBeDoneBy(ddlDonebyEdit, General.GetNullableInteger(ucTypeofTrainingEdit.SelectedQuick));
                ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Row.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Row.FindControl("ucToolTipCourseName");
            Label lblCourseName = (Label)e.Row.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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

            LinkButton lnkEployeeName = (LinkButton)e.Row.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Row.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Row.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreCCBTTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataCCBT();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataCCBT();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoneByEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text)
                    );

                _gridView.EditIndex = -1;
                BindDataCCBT();
            }
            SetPageNavigatorCCBT();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreCCBTTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindDataCCBT();
            SetPageNavigatorCCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreCCBTTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataCCBT();
        SetPageNavigatorCCBT();
    }

    protected void gvOffshoreCCBTTraining_RowCreated(object sender, GridViewRowEventArgs e)
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
            HeaderCell.Text = "To be assessed by Company Training Officer";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshoreCCBTTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }
    protected void cmdGoCCBT_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreCCBTTraining.SelectedIndex = -1;
        gvOffshoreCCBTTraining.EditIndex = -1;
        if (Int32.TryParse(txtnopageCCBT.Text, out result))
        {
            ViewState["PAGENUMBERCCBT"] = Int32.Parse(txtnopageCCBT.Text);

            if ((int)ViewState["TOTALPAGECOUNTCCBT"] < Int32.Parse(txtnopageCCBT.Text))
                ViewState["PAGENUMBERCCBT"] = ViewState["TOTALPAGECOUNTCCBT"];


            if (0 >= Int32.Parse(txtnopageCCBT.Text))
                ViewState["PAGENUMBERCCBT"] = 1;

            if ((int)ViewState["PAGENUMBERCCBT"] == 0)
                ViewState["PAGENUMBERCCBT"] = 1;

            txtnopageCCBT.Text = ViewState["PAGENUMBERCCBT"].ToString();
        }
        BindDataCCBT();
        SetPageNavigatorCCBT();
    }

    protected void PagerButtonClickCCBT(object sender, CommandEventArgs ce)
    {
        gvOffshoreCCBTTraining.SelectedIndex = -1;
        gvOffshoreCCBTTraining.EditIndex = -1;
        if (ce.CommandName == "prevCCBT")
            ViewState["PAGENUMBERCCBT"] = (int)ViewState["PAGENUMBERCCBT"] - 1;
        else
            ViewState["PAGENUMBERCCBT"] = (int)ViewState["PAGENUMBERCCBT"] + 1;

        BindDataCCBT();
        SetPageNavigatorCCBT();
    }

    private void SetPageNavigatorCCBT()
    {
        cmdPreviousCCBT.Enabled = IsPreviousEnabledCCBT();
        cmdNextCCBT.Enabled = IsNextEnabledCCBT();
        lblPagenumberCCBT.Text = "Page " + ViewState["PAGENUMBERCCBT"].ToString();
        lblPagesCCBT.Text = " of " + ViewState["TOTALPAGECOUNTCCBT"].ToString() + " Pages. ";
        lblRecordsCCBT.Text = "(" + ViewState["ROWCOUNTCCBT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledCCBT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCCBT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCCBT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledCCBT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCCBT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCCBT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    /* Completed Training Course*/
    protected void ShowExcelCTC()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };
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
                            Int32.Parse(ViewState["PAGENUMBERCTC"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            null,
                            null,
                            null,
                            0,
                            General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=CompletedTrainingCourse.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Completed Training Course</h3></td>");
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

    protected void MenuOffshoreCompletedCourseTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelCTC();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                //Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindDataCTC();
                SetPageNavigatorCTC();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindDataCTC()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED", "FLDSCORE", "FLDEXAMRESULT" };
        string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed", "Score %", "Result" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERCTC"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                null,
                null,
                null,
                0,
                General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString())
                );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreCCourseTraining", "Completed Training Course", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreCCourseTraining.DataSource = ds;
            gvOffshoreCCourseTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreCCourseTraining);
        }

        ViewState["ROWCOUNTCTC"] = iRowCount;
        ViewState["TOTALPAGECOUNTCTC"] = iTotalPageCount;
        SetPageNavigatorCTC();
    }

    protected void gvOffshoreCCourseTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindDataCTC();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
            SetPageNavigatorCTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreCCourseTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataCTC();
    }

    protected void gvOffshoreCCourseTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DropDownList ddlCategoryEdit = (DropDownList)e.Row.FindControl("ddlCategoryEdit");

            if (ddlCategoryEdit != null)
            {
                BindCategory(ddlCategoryEdit);
                if (dr["FLDCATEGORY"] != null && dr["FLDCATEGORY"].ToString() != "")
                    ddlCategoryEdit.SelectedValue = dr["FLDCATEGORY"].ToString();
            }

            DropDownList ddlSubCategoryEdit = (DropDownList)e.Row.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ddlCategoryEdit.SelectedValue);
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Row.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Row.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }

            //UserControlQuick ucDonebyEdit = (UserControlQuick)e.Row.FindControl("ucDonebyEdit");
            //if (ucDonebyEdit != null)
            //{
            //    ucDonebyEdit.bind();
            //    ucDonebyEdit.SelectedQuick = dr["FLDTOBEDONEBY"].ToString();
            //}

            DropDownList ddlDonebyEdit = (DropDownList)e.Row.FindControl("ddlDoneByEdit");
            if (ddlDonebyEdit != null)
            {
                if (ucTypeofTrainingEdit != null)
                    BindToBeDoneBy(ddlDonebyEdit, General.GetNullableInteger(ucTypeofTrainingEdit.SelectedQuick));
                ddlDonebyEdit.SelectedValue = dr["FLDTOBEDONEBY"].ToString();
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
            }

            ImageButton cmdCourseReq = (ImageButton)e.Row.FindControl("cmdCourseReq");
            if (cmdCourseReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCourseReq.CommandName))
                    cmdCourseReq.Visible = false;

                cmdCourseReq.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../CrewOffshore/CrewOffshoreTrainingCourseRequest.aspx?trainingneedid=" + dr["FLDTRAININGNEEDID"].ToString() + "'); return true;");
            }

            UserControlToolTip ucToolTipCourseName = (UserControlToolTip)e.Row.FindControl("ucToolTipCourseName");
            Label lblCourseName = (Label)e.Row.FindControl("lblCourseName");
            ImageButton imgCourseName = (ImageButton)e.Row.FindControl("imgCourseName");
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

            LinkButton lnkEployeeName = (LinkButton)e.Row.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlCategoryAdd = (DropDownList)e.Row.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                BindCategory(ddlCategoryAdd);
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Row.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreCCourseTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick
                    , null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucImprovementAdd")).SelectedQuick),
                    General.GetNullableString(""),
                    General.GetNullableString(""));

                BindDataCTC();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindDataCTC();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue
                    , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue
                    , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid trainingneedid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCategoryEdit")).SelectedValue),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSubCategoryEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucImprovementEdit")).SelectedQuick),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoneByEdit")).SelectedValue),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailsEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTrainerEdit")).Text),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDate")).Text)
                    );

                _gridView.EditIndex = -1;
                BindDataCTC();
            }
            SetPageNavigatorCTC();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreCCourseTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindDataCTC();
            SetPageNavigatorCTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreCCourseTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataCTC();
        SetPageNavigatorCTC();
    }

    protected void gvOffshoreCCourseTraining_RowCreated(object sender, GridViewRowEventArgs e)
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
            HeaderCell.Text = "To be assessed by Company Training Officer";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "To be Completed by Manning Office / Agent";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvOffshoreCCourseTraining.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }
    protected void cmdGoCTC_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreCCourseTraining.SelectedIndex = -1;
        gvOffshoreCCourseTraining.EditIndex = -1;
        if (Int32.TryParse(txtnopageCTC.Text, out result))
        {
            ViewState["PAGENUMBERCTC"] = Int32.Parse(txtnopageCTC.Text);

            if ((int)ViewState["TOTALPAGECOUNTCTC"] < Int32.Parse(txtnopageCTC.Text))
                ViewState["PAGENUMBERCTC"] = ViewState["TOTALPAGECOUNTCTC"];


            if (0 >= Int32.Parse(txtnopageCTC.Text))
                ViewState["PAGENUMBERCTC"] = 1;

            if ((int)ViewState["PAGENUMBERCTC"] == 0)
                ViewState["PAGENUMBERCTC"] = 1;

            txtnopageCTC.Text = ViewState["PAGENUMBERCTC"].ToString();
        }
        BindDataCTC();
        SetPageNavigatorCTC();
    }
    protected void PagerButtonClickCTC(object sender, CommandEventArgs ce)
    {
        gvOffshoreCCourseTraining.SelectedIndex = -1;
        gvOffshoreCCourseTraining.EditIndex = -1;
        if (ce.CommandName == "prevCTC")
            ViewState["PAGENUMBERCTC"] = (int)ViewState["PAGENUMBERCTC"] - 1;
        else
            ViewState["PAGENUMBERCTC"] = (int)ViewState["PAGENUMBERCTC"] + 1;

        BindDataCTC();
        SetPageNavigatorCTC();
    }

    private void SetPageNavigatorCTC()
    {
        cmdPreviousCTC.Enabled = IsPreviousEnabledCTC();
        cmdNextCTC.Enabled = IsNextEnabledCTC();
        lblPagenumberCTC.Text = "Page " + ViewState["PAGENUMBERCTC"].ToString();
        lblPagesCTC.Text = " of " + ViewState["TOTALPAGECOUNTCTC"].ToString() + " Pages. ";
        lblRecordsCTC.Text = "(" + ViewState["ROWCOUNTCTC"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledCTC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCTC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCTC"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledCTC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCTC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCTC"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }


    
}
