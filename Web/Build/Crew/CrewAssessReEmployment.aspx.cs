using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Crew_CrewAssessReEmployment : PhoenixBasePage
{
    string strEmployeeId;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            if (string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                strEmployeeId = string.Empty;
            }
            else
            {
                strEmployeeId = Request.QueryString["empid"];
            }

            ViewState["PENDINGREVIEWID"] = Request.QueryString["PendingReviewid"];

            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDO.AccessRights = this.ViewState;
            MenuDO.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERT"] = 1;
                ViewState["SORTEXPRESSIONT"] = null;
                ViewState["SORTDIRECTIONT"] = null;

                SetEmployeePrimaryDetails();
                PendingReviewList();
                DAO();
                //BindTrainingNeeds();
                //BindData();
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewRecommendedCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rblstblremep_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtDOA.CssClass = "input_mandatory";
    }
    private void DAO()
    {
        DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(strEmployeeId));

        if (dtDAOEdit.Rows.Count > 0)
        {
            ViewState["DOAID"] = dtDAOEdit.Rows[0]["FLDDOAID"].ToString();
            txtDOAGivenDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOAGIVENDATE"].ToString());
            ddlDOAMethod.SelectedQuick = dtDAOEdit.Rows[0]["FLDDOAMETHOD"].ToString();
            txtDTOfTelConf.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDATEOFTELCONF"].ToString());
            txtDOA.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOA"].ToString());
            txtStandbyDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDSTANDBYDATE"].ToString());
            txtFollowupDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDFOLLOWUPDATE"].ToString());
            txtExpectedsalary.Text = dtDAOEdit.Rows[0]["FLDEXPECTEDSALARY"].ToString();

        }
    }

    private void PendingReviewList()
    {
        DataTable dtPendingEdit = PhoenixCrewDateOfAvailability.PendingReviewEdit(General.GetNullableInteger(strEmployeeId), General.GetNullableGuid(ViewState["PENDINGREVIEWID"].ToString()));

        if (dtPendingEdit.Rows.Count > 0)
        {
            rblAppraisalSatisfactory.SelectedValue = General.GetNullableString(dtPendingEdit.Rows[0]["FLDAPPRAISALSATISFACTORY"].ToString());
            rblTraining.SelectedValue = General.GetNullableString(dtPendingEdit.Rows[0]["FLDCANDIDATEADVISEDFORREQUIREMENT"].ToString());
            rblstblremep.SelectedValue = General.GetNullableString(dtPendingEdit.Rows[0]["FLDSUITABLEFORREEMPLOYMENT"].ToString());
        }

    }
    private void ResetFields()
    {
        txtDOAGivenDate.Text = string.Empty;
        ddlDOAMethod.SelectedQuick = string.Empty;
        txtDTOfTelConf.Text = string.Empty;
        txtDOA.Text = string.Empty;
        txtStandbyDate.Text = string.Empty;
        txtFollowupDate.Text = string.Empty;
    }
    protected void DOA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidDOA())
                {
                    ucError.Visible = true;
                    return;
                }

                int? iid = General.GetNullableInteger(strEmployeeId);
                string strdate = txtDOAGivenDate.Text;
                int? iidmethod = General.GetNullableInteger(ddlDOAMethod.SelectedQuick);
                DateTime? date = General.GetNullableDateTime(txtDTOfTelConf.Text);
                DateTime? sdate = General.GetNullableDateTime(txtStandbyDate.Text);
                string doa = txtDOA.Text;
                int? doaid = (ViewState["DOAID"] != null) ? General.GetNullableInteger(ViewState["DOAID"].ToString()) : null;

                if (General.GetNullableDateTime(txtDOA.Text).HasValue)
                {
                    PhoenixCrewDateOfAvailability.DateOfAvailabilityInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , General.GetNullableInteger(strEmployeeId)
                                                                         , txtDOAGivenDate.Text
                                                                         , General.GetNullableInteger(ddlDOAMethod.SelectedQuick)
                                                                         , General.GetNullableDateTime(txtDTOfTelConf.Text)
                                                                         , General.GetNullableDateTime(txtStandbyDate.Text)
                                                                         , txtDOA.Text
                                                                         , (ViewState["DOAID"] != null) ? General.GetNullableInteger(ViewState["DOAID"].ToString()) : null
                                                                         , null
                                                                         , General.GetNullableDateTime(txtFollowupDate.Text)
                                                                         , null
                                                                         , General.GetNullableInteger(txtExpectedsalary.Text));
                }
                if (General.GetNullableInteger(rblstblremep.SelectedValue.Trim()).HasValue
                    && (General.GetNullableInteger(rblTraining.SelectedValue.Trim())).HasValue
                    && (General.GetNullableInteger(rblAppraisalSatisfactory.SelectedValue.Trim())).HasValue
                    )
                {

                    PhoenixCrewDateOfAvailability.ReEmploymentAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                       General.GetNullableInteger(strEmployeeId),
                                                                       txtDOAGivenDate.Text,
                                                                       General.GetNullableInteger(ddlDOAMethod.SelectedQuick),
                                                                       General.GetNullableDateTime(txtDTOfTelConf.Text),
                                                                       txtDOA.Text,
                                                                       General.GetNullableDateTime(txtFollowupDate.Text),
                                                                       General.GetNullableDateTime(txtStandbyDate.Text),
                                                                       General.GetNullableInteger(rblAppraisalSatisfactory.SelectedValue.Trim()),
                                                                       General.GetNullableInteger(rblTraining.SelectedValue.Trim()),
                                                                       General.GetNullableInteger(rblstblremep.SelectedValue.Trim()),
                                                                       General.GetNullableGuid(ViewState["PENDINGREVIEWID"].ToString()),
                                                                       General.GetNullableInteger(txtExpectedsalary.Text)
                                                                      );

                }
                //else
                //{
                //    PhoenixCrewDateOfAvailability.ReEmploymentAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //                                                       General.GetNullableInteger(strEmployeeId),
                //                                                       txtDOAGivenDate.Text,
                //                                                       General.GetNullableInteger(ddlDOAMethod.SelectedQuick),
                //                                                       General.GetNullableDateTime(txtDTOfTelConf.Text),
                //                                                       txtDOA.Text,
                //                                                       General.GetNullableDateTime(txtFollowupDate.Text),
                //                                                       General.GetNullableDateTime(txtStandbyDate.Text),
                //                                                       General.GetNullableInteger(rblAppraisalSatisfactory.SelectedValue.Trim()),
                //                                                       General.GetNullableInteger(rblTraining.SelectedValue.Trim()),
                //                                                       General.GetNullableInteger(rblstblremep.SelectedValue.Trim()),
                //                                                       General.GetNullableGuid(ViewState["PENDINGREVIEWID"].ToString()),
                //                                                       General.GetNullableInteger(txtExpectedsalary.Text),
                //                                                       1
                //                                                  );
                //}


            }

            ViewState["DOAID"] = string.Empty;
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidDOA()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dt;

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";
        }
        //if (txtDOA.Text == null)
        //{
        //   if( General.GetNullableInteger(rblstblremep.SelectedValue) == 1)
        //        ucError.ErrorMessage = "DOA is required.";
        //}


        //if (ViewState["STATUS"].ToString().ToUpper() != "0")
        //{
        //    if (string.IsNullOrEmpty(txtDOA.Text))
        //        ucError.ErrorMessage = "DOA is required.";
        //    else
        if (txtDOA.Text != null)
        {
            if (DateTime.TryParse(txtDOA.Text, out dt) == false)
                ucError.ErrorMessage = "Date of availability  is not in correct format";
        }
        //}
        //if (!string.IsNullOrEmpty(txtDOA.Text)
        //     && DateTime.TryParse(txtDOA.Text, out resultdate))
        //{
        //    if (DateTime.Compare(resultdate, DateTime.Now) < 0)
        //        ucError.ErrorMessage = "Date of Availability should be later than current date";
        //}
        if (txtDOAGivenDate.Text != null)
        {
            if (DateTime.TryParse(txtDOAGivenDate.Text, out dt) == false)
                ucError.ErrorMessage = "DOA Given date is not in correct format";
        }
        if (txtDTOfTelConf.Text != null)
        {
            if (DateTime.TryParse(txtDTOfTelConf.Text, out dt) == false)
                ucError.ErrorMessage = "Date Of Teleconference  is not in correct format";
        }
        if (txtStandbyDate.Text != null)
        {
            if (DateTime.TryParse(txtStandbyDate.Text, out dt) == false)
                ucError.ErrorMessage = "Stand by date  is not in correct format";
        }

        return (!ucError.IsError);

    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                ViewState["STATUS"] = dt.Rows[0]["FLDSTATUSTYPE"].ToString();
                ViewState["vesselid"] = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
                ViewState["trainingmatrixid"] = dt.Rows[0]["FLDTRAININGMATRIXID"].ToString();
                ViewState["rankid"] = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                ViewState["ONBOARD"] = dt.Rows[0]["FLDPRESENTVESSEL"].ToString();
                if (ViewState["STATUS"].ToString() == "0")
                {
                    txtDOA.CssClass = "input";
                }
                if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
                {
                    lblDateOfTeleconference.Text = "Last Contact Date";
                    lblFollowUpDate.Text = "Next Contact Date";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // Training Needs

    private void BindTrainingNeeds()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            ViewState["pendingtraining"] = 0;
            string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDTOBEDONEBYNAME" };
            string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "To be done by" };
            string sortexpression = (ViewState["SORTEXPRESSIONT"] == null) ? null : (ViewState["SORTEXPRESSIONT"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTIONT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONT"].ToString());

            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchPendingTrainingCourse(int.Parse(strEmployeeId),
                General.GetNullableInteger(ViewState["rankid"] != null ? ViewState["rankid"].ToString() : ""),
                General.GetNullableInteger(ViewState["vesselid"].ToString()),
                General.GetNullableInteger(ViewState["trainingmatrixid"] != null ? ViewState["trainingmatrixid"].ToString() : ""),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBERT"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount, 1);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewRecommendedCourses", "Training Needs", alCaptions, alColumns, ds);
            gvCrewRecommendedCourses.DataSource = ds;
            gvCrewRecommendedCourses.VirtualItemCount = iRowCount;



            ViewState["ROWCOUNTT"] = iRowCount;
            ViewState["TOTALPAGECOUNTT"] = iTotalPageCount;


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

            if (CommandName.ToUpper().Equals("EXCELTN"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDTOBEDONEBYNAME" };
                string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "To be done by" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSIONT"] == null) ? null : (ViewState["SORTEXPRESSIONT"].ToString());

                if (ViewState["SORTDIRECTIONT"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONT"].ToString());

                if (ViewState["ROWCOUNTT"] == null || Int32.Parse(ViewState["ROWCOUNTT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTT"].ToString());

                DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchPendingTrainingCourse(int.Parse(strEmployeeId),
                                        General.GetNullableInteger(ViewState["rankid"].ToString()),
                                        General.GetNullableInteger(ViewState["vesselid"].ToString()),
                                        General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()),
                                        sortexpression, sortdirection,
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        iRowCount,
                                        ref iRowCount,
                                        ref iTotalPageCount, 1);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Training Needs", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvCrewRecommendedCourses_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        //_gridView.SelectedIndex = e.NewSelectedIndex;
        //_gridView.EditIndex = -1;

    }
    protected void gvCrewRecommendedCourses_RowCreated(object sender, GridViewRowEventArgs e)
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
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvCrewRecommendedCourses.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }



    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDTOTALSCORE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report", "Total Score", "Recommended Promotion Y/N", "Fit for Re-employment Y/N", "HOD Remarks", "Master Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(strEmployeeId)
               , General.GetNullableInteger(ViewState["vesselid"].ToString())
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , iRowCount
               , ref iRowCount
               , ref iTotalPageCount
               );

        Response.AddHeader("Content-Disposition", "attachment; filename=Appraisal.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Appraisal</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void gvAQ_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;

            BindData();

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
        int? sortdirection = 1; //DEFAULT DESC SORT
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDTOTALSCORE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report", "Total Score", "Recommended Promotion Y/N", "Fit for Re-employment Y/N", "HOD Remarks", "Master Remarks" };
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(strEmployeeId)
               , General.GetNullableInteger(ViewState["vesselid"].ToString())
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

            General.SetPrintOptions("gvAQ", "Appraisal", alCaptions, alColumns, ds);
            gvAQ.DataSource = ds.Tables[0];
            gvAQ.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rblWaivingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //RadioButtonList rbl = (RadioButtonList)sender;
            //GridViewRow gvRow = (GridViewRow)rbl.Parent.Parent;

            //CheckBox cb = (CheckBox)gvRow.FindControl("chkWaivedYN");
            //gvCrewRecommendedCourses.SelectedIndex = gvRow.DataItemIndex;
            //if (General.GetNullableInteger(rbl.SelectedValue) != null)
            //    cb.Enabled = true;
            //else
            //    cb.Enabled = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindTrainingNeeds();
        }
    }
    protected void chkWaivedYNTN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //CheckBox cb = (CheckBox)sender;
            //GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            //TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");
            //RadioButtonList rbl = (RadioButtonList)gvRow.FindControl("rblWaivingType");

            //gvCrewRecommendedCourses.EditIndex = gvRow.DataItemIndex;
            //gvCrewRecommendedCourses.SelectedIndex = gvRow.DataItemIndex;
            //ViewState["trainingneedrowindex"] = gvRow.DataItemIndex;
            //ViewState["trainingneedwaivedyn"] = cb.Checked ? 1 : 0;
            //ViewState["trainingneededititem"] = 1;

            //BindTrainingNeeds();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindTrainingNeeds();
        }
    }

    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERT"] = ViewState["PAGENUMBERT"] != null ? ViewState["PAGENUMBERT"] : gvCrewRecommendedCourses.CurrentPageIndex + 1;
            BindTrainingNeeds();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewRecommendedCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBERT"] = null;
        }

    }



    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            Filter.CurrentAppraisalSelection = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;

        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
             && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                {                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
              

                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=NAPPRAISALUPLOAD'); return false;");
                }
                else
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=NAPPRAISALUPLOAD'); return false;");
                }

                LinkButton cmdAppraisal = (LinkButton)e.Item.FindControl("cmdAppraisal");
                RadLabel lblAppraisalId = (RadLabel)e.Item.FindControl("lblAppraisalId");
                if (cmdAppraisal != null)
                {
                    cmdAppraisal.Visible = SessionUtil.CanAccess(this.ViewState, cmdAppraisal.CommandName);
                    if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                    {
                        cmdAppraisal.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                    else
                    {
                        cmdAppraisal.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                }

                LinkButton cmdPromotion = (LinkButton)e.Item.FindControl("cmdPromotion");
                if (cmdPromotion != null)
                {
                    cmdPromotion.Visible = SessionUtil.CanAccess(this.ViewState, cmdPromotion.CommandName);
                    if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                    {
                        cmdPromotion.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                    else
                    {
                        cmdPromotion.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                }
            }

            RadLabel lblHODRemarks = (RadLabel)e.Item.FindControl("lblHODRemarks");
            if (lblHODRemarks != null)
            {
              
                UserControlToolTip ucHODRemarks = (UserControlToolTip)e.Item.FindControl("ucHODRemarks");

                ucHODRemarks.Position = ToolTipPosition.TopCenter;
                ucHODRemarks.TargetControlId = lblHODRemarks.ClientID;          
           
            }

            RadLabel lblMasterRemarks = (RadLabel)e.Item.FindControl("lblMasterRemarks");
            if (lblMasterRemarks != null)
            {
                UserControlToolTip ucMasterRemarks = (UserControlToolTip)e.Item.FindControl("ucMasterRemarks");
                ucMasterRemarks.Position = ToolTipPosition.TopCenter;
                ucMasterRemarks.TargetControlId = lblMasterRemarks.ClientID;
              
            }
        }

    }

    protected void gvAQ_SortCommand(object sender, GridSortCommandEventArgs e)
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
}