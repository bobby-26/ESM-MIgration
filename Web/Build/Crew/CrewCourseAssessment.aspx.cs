using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Globalization;
using Telerik.Web.UI;
public partial class CrewCourseAssessment : PhoenixBasePage 
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCourseAssessment.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCourseAssessment.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        foreach (GridViewRow r in gvCrewBehavioralAspects.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCrewBehavioralAspects.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Crew/CrewCourseAssessment.aspx?batchid=" + Request.QueryString["batchid"] + "&employeeid=" + Request.QueryString["employeeid"], "Export to Excel", "icon_xls.png", "Excel");
            MenuCrewCourseAssessment.AccessRights = this.ViewState;
            MenuCrewCourseAssessment.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["attachmentcode"] = "";
                ucConfirm.Visible = false;              

                if (Filter.CurrentCourseSelection != null)
                {
                    EditCourseDetails();
                }
                if (Request.QueryString["batchid"] != null)
                {
                    EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
                }
                BindNoOfAttempts();
                imgPPClip.Visible = false;
                if (ViewState["attachmentcode"].ToString() != "")
                {
                    imgPPClip.Visible = true;
                    imgPPClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                     + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + "&cmdname=ASSESSMENTUPLOAD'); return false;";
                }
            }
            SetResult();
            toolbar.AddButton("Save", "SAVE");
            CrewMenuCourseAssessment.AccessRights = this.ViewState;
            if (txtResult.Text.ToUpper() == "FAIL")
            {
                toolbar.AddButton("Re-Assessment", "REASSESSMENT");
            }
            CrewMenuCourseAssessment.MenuList = toolbar.Show();
            BindCourseAssessment();
            BindBehavioralAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewMenuCourseAssessment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string noofattempts = "";
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("REASSESSMENT"))
            {
                ucConfirm.Visible = true;
                ucConfirm.Text = "Are you sure you want to do Re-Assessment?";

            }
            else if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                noofattempts = ddlNoofattempts.SelectedValue;
                if (noofattempts == "")
                {
                    noofattempts = "1";
                }
                PhoenixCrewCourseAssessment.SaveCourseBatchExtras(
                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                       Convert.ToInt32(Request.QueryString["batchid"]),
                                       Convert.ToInt32(Request.QueryString["employeeid"]),
                                       General.GetNullableString(txtRemarks.Text),
                                       General.GetNullableInteger(noofattempts));
                //BindNoOfAttempts();
                //BindCourseAssessment();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnSignOn_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

            if (ucCM.confirmboxvalue == 1)
            {

                PhoenixCrewCourseAssessment.UpdateCourseReAssessment(
                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                           Convert.ToInt32(Request.QueryString["employeeid"]),
                                           Convert.ToInt32(Request.QueryString["batchid"]));
                PhoenixCrewCourseAssessment.BehavioralAspectsReAssessment(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            Convert.ToInt32(Request.QueryString["employeeid"]),
                                            Convert.ToInt32(Request.QueryString["batchid"]));
                BindNoOfAttempts();
                BindCourseAssessment();
                BindBehavioralAssessment();
                txtResult.Text = "";
                CrewMenuCourseAssessment.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindNoOfAttempts()
    {
        try
        {
            DataSet ds = PhoenixCrewCourseAssessment.ListCrewCourseAttempt(Convert.ToInt32(Request.QueryString["employeeid"]),
                Convert.ToInt32(Request.QueryString["batchid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlNoofattempts.Items.Clear();

                ddlNoofattempts.DataSource = ds.Tables[0];
                ddlNoofattempts.DataTextField = "FLDNUMBER";
                ddlNoofattempts.DataValueField = "FLDNUMBER";
                ddlNoofattempts.DataBind();
                ddlNoofattempts.SelectedIndex = 0;
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                ViewState["attachmentcode"] = ds.Tables[2].Rows[0]["FLDDTKEY"].ToString();
                txtRemarks.Text = ds.Tables[2].Rows[0]["FLDREMARKS"].ToString();
                txtLastUpdatedBy.Text = ds.Tables[2].Rows[0]["FLDMODIFIEDBY"].ToString();
                txtLastUpdatedDate.Text = ds.Tables[2].Rows[0]["FLDMODIFIEDDATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetResult()
    {
        try
        {

            string result = "";
            DataSet ds = PhoenixCrewCourseAssessment.CrewCourseBatchAssessmentResultSearch(Convert.ToInt32(Request.QueryString["batchid"]),
                                                                            Convert.ToInt32(Request.QueryString["employeeid"]), ref result);
            if (result != null)
            {
                txtResult.Text = result;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void EditCourseDetails()
    {
        try
        {

            int courseid = Convert.ToInt32(Filter.CurrentCourseSelection);
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
                ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditBatchDetails(int batchid)
    {
        try
        {

            DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNAME"].ToString();

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
        try
        {
            BindCourseAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCourseAssessment()
    {
        try
        {
            string[] alColumns = { "FLDCODE", "FLDNAME", "FLDMAXMARKS", "FLDVALUE" };
            string[] alCaptions = { "Code", "Topic", "Max Marks", "Marks" };


            DataSet ds = PhoenixCrewCourseAssessment.ListCrewCourseAssessmentTopic(General.GetNullableInteger(Request.QueryString["employeeid"]),
                        General.GetNullableInteger(Request.QueryString["batchid"]), General.GetNullableInteger(ddlNoofattempts.SelectedValue));

            General.SetPrintOptions("gvCourseAssessment", "Course Assessment", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCourseAssessment.DataSource = ds;
                gvCourseAssessment.DataBind();

                txtEmployeeName.Text = ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCourseAssessment);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCourseAssessment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindCourseAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCourseAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCourseAssessment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.SelectedIndex = e.NewEditIndex;

            _gridView.EditIndex = e.NewEditIndex;
            BindCourseAssessment();
            ((TextBox)(((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtMarksEdit"))).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCourseAssessment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string assessmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentId")).Text;
            string courseassementid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseAssessmentId")).Text;
            string code = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCode")).Text;
            string marks = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text;


            if (!IsValidCourseAssessment(marks))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewCourseAssessment.SaveCourseAssessmentTopic(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  General.GetNullableInteger(courseassementid)
                , Convert.ToInt32(Request.QueryString["batchid"])
                , Convert.ToInt32(Request.QueryString["employeeid"])
                , Convert.ToInt32(assessmentid)
                , Convert.ToDecimal(marks)
                );
            _gridView.EditIndex = -1;
            BindCourseAssessment();
            BindNoOfAttempts();
            SetResult();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            CrewMenuCourseAssessment.AccessRights = this.ViewState;
            if (txtResult.Text.ToUpper() == "FAIL")
            {
                toolbar.AddButton("Re-Assessment", "REASSESSMENT");
            }
            CrewMenuCourseAssessment.MenuList = toolbar.Show();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCourseAssessment(string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (value.Trim() == "")
            ucError.ErrorMessage = "Marks is required";

        //if (General.GetNullableInteger(value) > 100 || General.GetNullableInteger(value) < 0)
        //    ucError.ErrorMessage = "Rating should be between 0 to 100";


        return (!ucError.IsError);
    }
    private bool IsValidBehavioralRating(string value)
    {
        ucError.HeaderMessage = "Please enter marks between 1 to 10";

        if (value.Trim() == "")
            ucError.ErrorMessage = "Rating is Required";

        if (General.GetNullableInteger(value) > 10 || General.GetNullableInteger(value) < 0)
            ucError.ErrorMessage = "Rating should be between 0 to 10";

        return (!ucError.IsError);
    }
    decimal r;
    protected void gvCourseAssessment_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
                if (drv["FLDVALUE"].ToString() != "")
                {
                    r = r + decimal.Parse(drv["FLDVALUE"].ToString());
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Font.Bold = true;
            e.Row.Cells[3].Font.Bold = true;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = r.ToString();

        }
    }
    protected void gvCourseAssessment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCourseAssessment, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }
    protected void gvCourseAssessment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string CourseAssessmentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseAssessmentId")).Text;
            if (CourseAssessmentId != "")
            {
                PhoenixCrewCourseAssessment.DeleteCourseAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(CourseAssessmentId));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
            }
            BindCourseAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void CrewCourseAssessment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {

                string[] alColumns1 = { "FLDCODE", "FLDNAME", "FLDMAXMARKS", "FLDVALUE" };
                string[] alCaptions1 = { "Code", "Topic", "Max Marks", "Marks" };

                string[] alColumns2 = { "FLDCODE", "FLDNAME", "FLDVALUE" };
                string[] alCaptions2 = { "Code", "Description", "Ratings" };


                DataSet ds = PhoenixCrewCourseAssessment.ListCrewCourseAssessmentOverview(General.GetNullableInteger(Request.QueryString["employeeid"]),
                            General.GetNullableInteger(Request.QueryString["batchid"]), General.GetNullableInteger(ddlNoofattempts.SelectedValue));

                if (ds.Tables.Count > 0)

                    Response.AddHeader("Content-Disposition", "attachment; filename=CourseAssessmentOverview.xls");
                HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
                HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                HttpContext.Current.Response.Write("<tr>");
                HttpContext.Current.Response.Write("<td><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                HttpContext.Current.Response.Write("<td colspan='" + (alColumns1.Length - 1).ToString() + "'><h3>Course Assessment Overview</h3></td>");
                HttpContext.Current.Response.Write("</tr>");
                HttpContext.Current.Response.Write("</TABLE>");

                HttpContext.Current.Response.Write("<br />");
                HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                HttpContext.Current.Response.Write("<tr>");
                HttpContext.Current.Response.Write("<td><b>Employee Name</b> </td>");
                HttpContext.Current.Response.Write("<td colspan='2'>" + txtEmployeeName.Text + " </td>");
                HttpContext.Current.Response.Write("<td><b>Course Name</b> </td>");
                HttpContext.Current.Response.Write("<td>" + ucCourse.SelectedName + " </td>");
                HttpContext.Current.Response.Write("</tr>");
                HttpContext.Current.Response.Write("<tr>");
                HttpContext.Current.Response.Write("<td><b>Course Type</b> </td>");
                HttpContext.Current.Response.Write("<td colspan='2'>" + ucCourseType.SelectedName + " </td>");
                HttpContext.Current.Response.Write("<td><b>Batch No</b> </td>");
                HttpContext.Current.Response.Write("<td >" + txtBatchNo.Text + " </td>");
                HttpContext.Current.Response.Write("</tr>");
                HttpContext.Current.Response.Write("<tr>");
                HttpContext.Current.Response.Write("<td><b>Pass Marks</b> </td>");
                HttpContext.Current.Response.Write("<td><b>No Of Attempts</b> </td>");
                HttpContext.Current.Response.Write("<td align='left'>" + ddlNoofattempts.SelectedItem.Text + " </td>");
                HttpContext.Current.Response.Write("</tr>");
                HttpContext.Current.Response.Write("<tr>");
                HttpContext.Current.Response.Write("<td><b>Result</b> </td>");
                HttpContext.Current.Response.Write("<td colspan='2'>" + txtResult.Text + " </td>");
                HttpContext.Current.Response.Write("<td><b>Remarks</b> </td>");
                HttpContext.Current.Response.Write("<td >" + txtRemarks.Text + " </td>");
                HttpContext.Current.Response.Write("</tr>");
                HttpContext.Current.Response.Write("</TABLE>");

                HttpContext.Current.Response.Write("<br />");
                HttpContext.Current.Response.Write("<b>Academic</b>");
                HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                HttpContext.Current.Response.Write("<tr>");
                for (int i = 0; i < alCaptions1.Length; i++)
                {
                    HttpContext.Current.Response.Write("<td width='20%'>");
                    HttpContext.Current.Response.Write("<b>" + alCaptions1[i] + "</b>");
                    HttpContext.Current.Response.Write("</td>");
                }
                HttpContext.Current.Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HttpContext.Current.Response.Write("<tr>");
                    for (int i = 0; i < alColumns1.Length; i++)
                    {
                        HttpContext.Current.Response.Write("<td>");
                        HttpContext.Current.Response.Write(dr[alColumns1[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns1[i]].ToString()) : dr[alColumns1[i]]);
                        HttpContext.Current.Response.Write("</td>");
                    }
                    HttpContext.Current.Response.Write("</tr>");
                }
                HttpContext.Current.Response.Write("</TABLE>");
                HttpContext.Current.Response.Write("<br />");
                HttpContext.Current.Response.Write("<b>Behavioral Aspects</b>");
                HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                HttpContext.Current.Response.Write("<tr>");
                for (int i = 0; i < alCaptions2.Length; i++)
                {
                    HttpContext.Current.Response.Write("<td width='20%'>");
                    HttpContext.Current.Response.Write("<b>" + alCaptions2[i] + "</b>");
                    HttpContext.Current.Response.Write("</td>");
                }
                HttpContext.Current.Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    HttpContext.Current.Response.Write("<tr>");
                    for (int i = 0; i < alColumns2.Length; i++)
                    {
                        HttpContext.Current.Response.Write("<td>");
                        HttpContext.Current.Response.Write(dr[alColumns2[i]]);
                        HttpContext.Current.Response.Write("</td>");
                    }
                    HttpContext.Current.Response.Write("</tr>");
                }
                HttpContext.Current.Response.Write("</TABLE>");
                HttpContext.Current.Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
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
    private void BindBehavioralAssessment()
    {
        try
        {
            string[] alColumns = { "FLDCODE", "FLDNAME", "FLDVALUE" };
            string[] alCaptions = { "Code", "Topic", "Marks" };


            DataSet ds = PhoenixCrewCourseAssessment.CrewBehavioralAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(Request.QueryString["employeeid"]), General.GetNullableInteger(ddlNoofattempts.SelectedValue));

            General.SetPrintOptions("gvCrewBehavioralAspects", "Behavioral Assessment", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewBehavioralAspects.DataSource = ds;
                gvCrewBehavioralAspects.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewBehavioralAspects);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewBehavioralAspects_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;

            }
        }
    }

  
    protected void gvCrewBehavioralAspects_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewBehavioralAspects, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }
    protected void gvCrewBehavioralAspects_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.SelectedIndex = e.NewEditIndex;

            _gridView.EditIndex = e.NewEditIndex;
            BindBehavioralAssessment();
           ((TextBox)(((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtBehavioralMarksEdit"))).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewBehavioralAspects_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string behavioralid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBehavioralIdEdit")).Text;
            string behavioralaspectsid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBehavioralAspectsId")).Text;
            string marks = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtBehavioralMarksEdit")).Text;
       

            if (!IsValidBehavioralRating(marks))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewCourseAssessment.SaveBehavioralAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(behavioralaspectsid)
                   , Convert.ToInt32(Request.QueryString["batchid"])
                   , Convert.ToInt32(Request.QueryString["employeeid"])
                   , General.GetNullableInteger(behavioralid)
                   , Convert.ToInt32(marks)
                   );
            _gridView.EditIndex = -1;
            BindBehavioralAssessment();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
    protected void gvCrewBehavioralAspects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindBehavioralAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewBehavioralAspects_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrewBehavioralAspects_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string behavioralaspectsid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBehavioralAspectsId")).Text;

            if (behavioralaspectsid != "")
            {
                PhoenixCrewCourseAssessment.BehavioralAspectsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(behavioralaspectsid));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
            }
            BindBehavioralAssessment();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
