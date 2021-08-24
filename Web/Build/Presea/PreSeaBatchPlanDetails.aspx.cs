using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using System.Text;
using System.Web.UI.HtmlControls;


public partial class PreSeaBatchPlanDetails : PhoenixBasePage
{
    string strBatchId;

    protected void Page_Load(object sender, EventArgs e)
    {       
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("List", "BATCH");
            MainToolbar.AddButton("Details", "DETAIL");
            //MainToolbar.AddButton("Semester", "SEMESTER");
            MenuBatchPlanner.AccessRights = this.ViewState;
            MenuBatchPlanner.MenuList = MainToolbar.Show();
            MenuBatchPlanner.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbar.Show();

            PhoenixToolbar gridtoolbar = new PhoenixToolbar();
            gridtoolbar.AddImageButton("../PreSea/PreSeaBatchPlanDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            gridtoolbar.AddImageLink("javascript:CallPrint('gvPreSea')", "Print Grid", "icon_print.png", "PRINT");
            gridtoolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../PreSea/PreSeaBatchPlanExamDetails.aspx?calledfrom=" + strBatchId + "');return false;", "Add", "add.png", "ADDEXAMVENUE");
            MenuPreSeaBatch.AccessRights = this.ViewState;
            MenuPreSeaBatch.MenuList = gridtoolbar.Show();
            strBatchId = Filter.CurrentPreSeaCourseMasterBatchSelection;
            if (!IsPostBack)
            {
                txtFacultyCourseInchargeDesignation.Attributes.Add("style", "visibility:hidden");
                txtTDCInchargeDesignation.Attributes.Add("style", "visibility:hidden");
                txtTDCInchargeId.Attributes.Add("style", "visibility:hidden");
                txtTDCInchargeEmail.Attributes.Add("style", "visibility:hidden");
                txtFacultyCourseInchargeId.Attributes.Add("style", "visibility:hidden");
                txtFacultyCourseInchargeEmail.Attributes.Add("style", "visibility:hidden");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlScoreCard.DataSource = PhoenixPreSeaTemplate.ListScoreCardTemplate(null);
                ddlScoreCard.DataBind();
                ddlScoreCard.Items.Insert(0, new ListItem("--Select--", ""));
                BindPreSeaExamVenues();             
                SetPrimaryBatchDetails();
                FillBatchDetailsFields();
               
                //imgTDCIncharge.Attributes.Add("onclick", "return showPickList('spnTDCIncharge', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=25,28,27', true);");
                //imgFacultyCourseIncharge.Attributes.Add("onclick", "return showPickList('spnFacultyCourseIncharge', 'codehelp1', '', '../Common/CommonPickListPreSeaCourseUser.aspx?departmentlist=25,28,27', true);");
            }
            BindData();
            SetPageNavigator();
            BindAttributes();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaCourseMasterBatchSelection))
            {
                ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                ucError.Visible = true;
                return;
            }
            else
            {

                DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                if (dce.CommandName.ToUpper().Equals("BATCH"))
                {
                    Response.Redirect("../PreSea/PreSeaBatch.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
                }
                //else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
                //{
                //    Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
                //}
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindPreSeaExamVenues()
    {
        DataSet ds = PhoenixPreSeaExamVenue.SearchExamVenueList();
        if (ds.Tables[0].Rows.Count > 0)
        {
            
        }
    }
    protected void BindAttributes()
    {
        DataTable dt = new DataTable();
        dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

        imgTDCIncharge.Attributes.Add("onclick", "return showPickList('spnTDCIncharge', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);");
        imgFacultyCourseIncharge.Attributes.Add("onclick", "return showPickList('spnFacultyCourseIncharge', 'codehelp1', '', '../Common/CommonPickListPreSeaCourseUser.aspx?departmentlist=" + DepartmentList + "', true);");

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEXAMVENUENAME", "FLDZONE", "FLDVENUEADDRESS", "FLDSTARTDATE", "FLDNOFDAYS", "FLDMEDICALFEES", "FLDCONTACTPESON", "FLDCONTACTNUMBERS", "FLDCONTACTMAIL" };
        string[] alCaptions = { "Venue", "Zone", "Address", "Start date", "No of days", "Medical fee", "Contact Person", "Phone Numbers", "E-Mail" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixPreSeaBatchPlanner.SearchEntranceExamPlan(General.GetNullableInteger(strBatchId),                        
                          sortexpression, sortdirection,
                        (int)ViewState["PAGENUMBER"],
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);

        General.SetPrintOptions("gvPreSea", "Exam Venue", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSea.DataSource = ds;
            gvPreSea.DataBind();         
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSea); 
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    private void FillBatchDetailsFields()
    {
        DataTable dt = PhoenixPreSeaBatchPlanner.ListBatchDetails(General.GetNullableInteger(strBatchId));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucMinLimit.Text = dr["FLDMINNOOFSTUDENTS"].ToString();
            ucMaxLimit.Text = dr["FLDMAXNOOFSTUDENTS"].ToString();
            txtAppIssStrt.Text = dr["FLDAPPLICATIONISSUESTART"].ToString();
            txtLstDtAppl.Text = dr["FLDLASTDATEFORAPPLY"].ToString();
            txtNoOfSem.Text = dr["FLDNOOFSEMESTER"].ToString();
            txtNoOfSem.ReadOnly = true;
            chkIsIMU.Checked = dr["FLDISIMUAPPLICABLE"].ToString().Equals("1") ? true : false;
            ddlScoreCard.SelectedValue = dr["FLDSCORECARDID"].ToString();
            if (!String.IsNullOrEmpty(dr["FLDENTRANCEVENUES"].ToString()))
            {
                string strlist = "," + dr["FLDENTRANCEVENUES"].ToString() + ",";
               
            }
            txtTDCInchargeName.Text = dr["FLDTDCINCHARGENAME"].ToString();
            txtFacultyCourseName.Text = dr["FLDCOURSEINCHARGENAME"].ToString();
        }
        else
            ResetFields();
    }
    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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

                Label lblExamPlanId = (Label)e.Row.FindControl("lblEntranceExamPlanID");
                Label lblExamVenueId = (Label)e.Row.FindControl("lblExamVenueID");
                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null)
                {
                   eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                   eb.Attributes.Add("onclick", "parent.Openpopup('codehelp1','','../PreSea/PreSeaBatchPlanExamDetails.aspx?batchid=" + strBatchId + "&ExamPlanId=" + lblExamPlanId.Text + "&ExamVenueID=" + lblExamVenueId.Text + "')");
                }

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridview = (GridView)sender;            
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {                
                
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {

                _gridview.EditIndex = Int32.Parse(e.CommandArgument.ToString());

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //PhoenixPreSeaBatchPlanner.DeleteBatchEntranceExamPlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                            , Int32.Parse(((Label)_gridview.Rows[nCurrentRow].FindControl("lblEntranceExamPlanID")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSea_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string EntranceExamPlanID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEntranceExamPlanID")).Text;
            PhoenixPreSeaBatchPlanner.DeleteBatchEntranceExamPlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(EntranceExamPlanID));
            ucStatus.Text = "Exam Venue Deleted Successfully";
            BindData();
            SetPageNavigator();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
    }

    protected void gvPreSea_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void PreSeaBatch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDEXAMVENUENAME", "FLDZONE", "FLDVENUEADDRESS", "FLDSTARTDATE", "FLDNOFDAYS", "FLDMEDICALFEES", "FLDCONTACTPESON", "FLDCONTACTNUMBERS", "FLDCONTACTMAIL" };
                string[] alCaptions = { "Venue", "Zone", "Address", "Start date", "No of days", "Medical fee", "Contact Person", "Phone Numbers", "E-Mail" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataSet ds = PhoenixPreSeaBatchPlanner.SearchEntranceExamPlan(General.GetNullableInteger(strBatchId),
                          sortexpression, sortdirection,
                        (int)ViewState["PAGENUMBER"],
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);

                //General.ShowExcel("Pre-Sea Batch", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
                Response.AddHeader("Content-Disposition", "attachment; filename=Entrance Exam Venue.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
                Response.Write("<td><h3>Entrance Exam Venue</h3></td>");
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
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write("<td>");
                        Response.Write(dr[alColumns[i]]);
                        Response.Write("</td>");

                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.End();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSea.EditIndex = -1;
        gvPreSea.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSea.EditIndex = -1;
        gvPreSea.SelectedIndex = -1;
        int result;
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
        gvPreSea.SelectedIndex = -1;
        gvPreSea.EditIndex = -1;
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
            return true;

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



    protected void DisableBatchPlanDetails()
    {
        ucMinLimit.ReadOnly = true;
        ucMinLimit.CssClass = "readonlytextbox";
        ucMaxLimit.ReadOnly = true;
        ucMaxLimit.CssClass = "readonlytextbox";
        txtAppIssStrt.ReadOnly = true;
        txtAppIssStrt.CssClass = "readonlytextbox";
        txtLstDtAppl.ReadOnly = true;
        txtLstDtAppl.CssClass = "readonlytextbox";
        txtNoOfSem.ReadOnly = true;
        txtNoOfSem.CssClass = "readonlytextbox";
        chkIsIMU.Enabled = false;
    }

    private void ResetFields()
    {
        string empty = String.Empty;
        ucMinLimit.Text = empty;
        ucMaxLimit.Text = empty;
        txtAppIssStrt.Text = empty;
        txtLstDtAppl.Text = empty;
        txtNoOfSem.Text = empty;
        chkIsIMU.Checked = false;
    }

    public bool IsValidBatchPlanDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dt;
        if (General.GetNullableInteger(strBatchId) == null)
            ucError.ErrorMessage = "Select a Batch from Batch Planner";
        if (String.IsNullOrEmpty(ucMinLimit.Text))
            ucError.ErrorMessage = "No of Students (Min) is required.";
        if (String.IsNullOrEmpty(ucMaxLimit.Text))
            ucError.ErrorMessage = "No of Students (Max) is required.";
        if (string.IsNullOrEmpty(txtAppIssStrt.Text))
            ucError.ErrorMessage = "Application Issue start is required.";
        else if (DateTime.TryParse(txtAppIssStrt.Text, out dt) == false)
            ucError.ErrorMessage = "Application Issue start  is not in correct format";
        if (string.IsNullOrEmpty(txtLstDtAppl.Text))
            ucError.ErrorMessage = "Last date for application is required.";
        else if (DateTime.TryParse(txtLstDtAppl.Text, out dt) == false)
            ucError.ErrorMessage = "Last date for apply is not in correct format";
        else if (General.GetNullableDateTime(txtLstDtAppl.Text).HasValue && General.GetNullableDateTime(txtAppIssStrt.Text).HasValue)
        {
            if (DateTime.Parse(txtLstDtAppl.Text) < DateTime.Parse(txtAppIssStrt.Text))
                ucError.ErrorMessage = "Last date for apply should not be less Appication Issue Start Date";
        }
        if (!General.GetNullableInteger(ddlScoreCard.SelectedValue).HasValue)
            ucError.ErrorMessage = "Scorecard Template is required";
		
        return (!ucError.IsError);

    }

    private void SetPrimaryBatchDetails()
    {
        DataSet ds = PhoenixPreSeaBatch.EditBatch(int.Parse(strBatchId));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCourse.Text = ds.Tables[0].Rows[0]["FLDPRESEACOURSENAME"].ToString();
            txtBatchName.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }


    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBatchPlanDetails())
                {
                    ucError.Visible = true;
                    return;
                }

                StringBuilder strlist = new StringBuilder();
               
                if (strlist.Length > 1)
                {
                    strlist.Remove(strlist.Length - 1, 1);
                }

                PhoenixPreSeaBatchPlanner.UpdateBatchDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(strBatchId)
                    , General.GetNullableInteger(ucMinLimit.Text)
                    , General.GetNullableInteger(ucMaxLimit.Text)
                    , General.GetNullableDateTime(txtAppIssStrt.Text)
                    , General.GetNullableDateTime(txtLstDtAppl.Text)
                    , General.GetNullableByte(chkIsIMU.Checked ? "1" : "0")
                    , General.GetNullableByte(txtNoOfSem.Text)
                    , General.GetNullableString(strlist.ToString())
					, General.GetNullableInteger("")
					, General.GetNullableInteger("")
					, General.GetNullableInteger(ddlScoreCard.SelectedValue)
                    , General.GetNullableInteger(txtTDCInchargeId.Text)
                    , General.GetNullableInteger(txtFacultyCourseInchargeId.Text)
					);

                ucStatus.Text = "Batch Plan Details saved Successfully.";
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
