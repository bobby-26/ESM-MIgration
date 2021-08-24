using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
public partial class CrewCourseFeedback : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvCourseEvaluation.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvCourseEvaluation.UniqueID, "Edit$" + r.RowIndex.ToString());
			}
		}
		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");           
			toolbar.AddButton("Save", "SAVE");
            toolbar.AddButton("Generate Feedback Report", "GENERATEFEEDBACK");
			MenuCourseFeedback.AccessRights = this.ViewState;
			MenuCourseFeedback.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ViewState["FEEDBACKID"] = "";
			
				if (Filter.CurrentCourseSelection != null)
				{
					EditCourseDetails();
				}
				if (Request.QueryString["batchid"] != null)
				{
					EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
					ddlCandidate.DataSource = PhoenixCrewCourseEnrollment.ListCrewCourseParticipant(Convert.ToInt32(Request.QueryString["batchid"]));
					ddlCandidate.DataValueField = "FLDEMPLOYEEID";
					ddlCandidate.DataTextField= "FLDNAME";
					ddlCandidate.DataBind();
					
					
				}
				
				editfeedback(General.GetNullableInteger(ViewState["FEEDBACKID"].ToString()), Convert.ToInt32(ddlCandidate.SelectedValue), Convert.ToInt32(Request.QueryString["batchid"]));
			}
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseFacultyEvaluationList.aspx?empid=" + ddlCandidate.SelectedValue + "&batchid=" + Request.QueryString["batchid"] + "')", "Assign/Edit Faculty evaluation", "add.png", "FACULTYEVALUATION");
			MenuFacultyEvaluation.AccessRights = this.ViewState;
			MenuFacultyEvaluation.MenuList = toolbargrid.Show();
			BindRadioButtonToTextBox();

			Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
			BindCourseEvaluation();
			BindFacultyEvaluation();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void BindRadioButtonToTextBox()
	{
		if (rbComment2.SelectedValue == "1")
		{
			txtComment2.ReadOnly = true;
			txtComment2.CssClass = "readonlytextbox";
			txtComment2.Text = "";
		}
		else
		{
			txtComment2.ReadOnly = false;
			txtComment2.CssClass = "input";
		}
		if (rbComment3.SelectedValue == "2")
		{
			txtComment3.ReadOnly = true;
			txtComment3.CssClass = "readonlytextbox";
			txtComment3.Text = "";
		}
		else
		{
			txtComment3.ReadOnly = false;
			txtComment3.CssClass = "input";
		}
		//
		if (rbComment4.SelectedValue == "1")
		{
			txtComment4.ReadOnly = true;
			txtComment4.CssClass = "readonlytextbox";
			txtComment4.Text = "";
		}
		else
		{
			txtComment4.ReadOnly = false;
			txtComment4.CssClass = "input";
		}
		if (rbComment5.SelectedValue == "0")
		{
			txtComment7.ReadOnly = true;
			txtComment7.CssClass = "readonlytextbox";
			txtComment7.Text = "";
		}
		else
		{
			txtComment7.ReadOnly = false;
			txtComment7.CssClass = "input";
		}
	}
	protected void SelectCandidate(object sender, EventArgs e)
	{
		editfeedback(General.GetNullableInteger(ViewState["FEEDBACKID"].ToString()), Convert.ToInt32(ddlCandidate.SelectedValue), Convert.ToInt32(Request.QueryString["batchid"]));
	}
	protected void FacultyEvaluation_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FACULTYEVALUATION"))
		{

			//BindFacultyEvaluation();
		}
		
	}
	protected void CourseFeedback_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidFeedback())
                {
                    ucError.Visible = true;
                    return;
                }
                SaveFeedback();             
            }
            else if (dce.CommandName.ToUpper().Equals("GENERATEFEEDBACK"))
			{
				if (!IsValidReport())
				{
					ucError.Visible = true;
					return;
				}
			
                string prams = "";

                prams += "&feedbackid=" + General.GetNullableInteger(ViewState["FEEDBACKID"].ToString());
                prams += "&employeeid=" + General.GetNullableString(ddlCandidate.SelectedValue.ToString());
                prams += "&batchid=" + General.GetNullableString(Request.QueryString["batchid"].ToString());
                prams += exceloptions();

                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PARTICIPANTSFEEDBACK" + prams,false);
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
			//BindCourseEvaluation();
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindCourseEvaluation()
	{

		
		DataSet ds = PhoenixCrewCourseFeedback.ListCourseEvaluation(Convert.ToInt32(ddlCandidate.SelectedValue),
		   General.GetNullableInteger(Request.QueryString["batchid"]));


		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseEvaluation.DataSource = ds;
			gvCourseEvaluation.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseEvaluation);
		}
	}
	protected void gvCourseEvaluation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = -1;
			BindCourseEvaluation();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvCourseEvaluation_RowCommand(object sender, GridViewCommandEventArgs e)
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
	protected void gvCourseEvaluation_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

			_gridView.SelectedIndex = e.NewEditIndex;
			_gridView.EditIndex = e.NewEditIndex;
		
			BindCourseEvaluation();

            ViewState["COURSEEVALUATIONID"] = ((Label)gvCourseEvaluation.Rows[e.NewEditIndex].FindControl("lblCourseEvaluationId")).Text;

			((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtMarksEdit")).FindControl("txtNumber").Focus();
            ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtMarksEdit")).Attributes.Add("onfocus", "this.select()");
			//((UserControlNumber)_gridView.Rows[e.NewEditIndex].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void gvCourseEvaluation_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

			string courseevaluationid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseEvaluationId")).Text;
			string lblEvaluationId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationId")).Text;
			string marks = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text;


			if (!IsValidCourseEvaluation(marks))
			{
				ucError.Visible = true;
				return;
			}

			if (General.GetNullableInteger(courseevaluationid) == null)
			{
				PhoenixCrewCourseFeedback.InsertCourseEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				General.GetNullableInteger(lblEvaluationId), Convert.ToInt32(marks), null,
				Convert.ToInt32(Request.QueryString["batchid"]), Convert.ToInt32(ddlCandidate.SelectedValue));
			}
			else
			{

				PhoenixCrewCourseFeedback.UpdateCourseEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(courseevaluationid), General.GetNullableInteger(lblEvaluationId), Convert.ToInt32(marks), null,
					Convert.ToInt32(Request.QueryString["batchid"]), Convert.ToInt32(ddlCandidate.SelectedValue));
			}
			_gridView.EditIndex = -1;
			BindCourseEvaluation();

			//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void gvCourseEvaluation_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvCourseEvaluation.SelectedIndex = se.NewSelectedIndex;
        Label lblCourseEvaluationId = (Label)gvCourseEvaluation.Rows[se.NewSelectedIndex].FindControl("lblCourseEvaluationId");
        if (lblCourseEvaluationId != null)
            ViewState["COURSEEVALUATIONID"] = lblCourseEvaluationId.Text;

        if (gvCourseEvaluation.EditIndex > -1)
            gvCourseEvaluation.UpdateRow(gvCourseEvaluation.EditIndex, false);

        gvCourseEvaluation.EditIndex = -1;
        BindCourseEvaluation();
    }
    protected void gvCourseEvaluation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCourseEvaluation, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
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
	private bool IsValidCourseEvaluation(string value)
	{
		ucError.HeaderMessage = "Please enter marks between 1 to 10";

		if (value.Trim() == "")
			ucError.ErrorMessage = "Rating is Required";

		if (General.GetNullableInteger(value) > 10 || General.GetNullableInteger(value) < 0)
			ucError.ErrorMessage = "Rating should be between 0 to 10";

		if (General.GetNullableInteger(ddlCandidate.SelectedValue)==null)
			ucError.ErrorMessage = "Please select a candidate";

		return (!ucError.IsError);
	}

	protected void gvCourseEvaluation_ItemDataBound(object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{

				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

			}

		}
		
	}

	protected void gvCourseEvaluation_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string courseevaluationid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseEvaluationId")).Text;
			if (courseevaluationid != "")
			{
				PhoenixCrewCourseFeedback.DeleteCourseEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
										Convert.ToInt32(courseevaluationid));
			}
			BindCourseEvaluation();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
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

	public void BindFacultyEvaluation()
	{

		try
		{
			
			gvBindFacultyEvaluation.Columns.Clear();
			TemplateField ItemTmpField = new TemplateField();
			DataSet ds = PhoenixCrewCourseFeedback.ListFacultyEvaluation(General.GetNullableInteger(ddlCandidate.SelectedValue),
								General.GetNullableInteger(Request.QueryString["batchid"]));

			string[] nonaggcol = { "Faculty Evaluation" };

			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < nonaggcol.Length ; i++)
				{
					if (nonaggcol[i] == "Faculty Evaluation")
					{

						HyperLinkField lnk = new HyperLinkField();
						lnk.HeaderText = nonaggcol[i];
						lnk.DataTextField = nonaggcol[i];

						gvBindFacultyEvaluation.Columns.Add(lnk);
					}
					else
					{
						BoundField field = new BoundField();

						field.DataField = nonaggcol[i];
						field.HeaderText = nonaggcol[i];
						gvBindFacultyEvaluation.Columns.Add(field);
					}
				}
				if (ds.Tables[1].Rows.Count > 0)
				{
					DataTable dt = ds.Tables[1];

					for (int i = 0; i < dt.Rows.Count; i++)
					{
						BoundField field = new BoundField();

						field = new BoundField();
						field.DataField = dt.Rows[i]["FLDNAME"].ToString();
						field.HeaderText = dt.Rows[i]["FLDNAME"].ToString();
						field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
						field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

						gvBindFacultyEvaluation.Columns.Add(field);

					}
				
				}

				gvBindFacultyEvaluation.DataSource = ds;
				gvBindFacultyEvaluation.DataBind();
				GridViewRow row = new GridViewRow(0, 2, DataControlRowType.Header, DataControlRowState.Normal);
				row.Attributes.Add("style", "position:static");
				TableCell cell = new TableCell();
				cell.ColumnSpan = 1;
				row.Cells.Add(cell);

				if (ds.Tables[2].Rows.Count > 0)
				{
					DataTable dtheader = ds.Tables[2];
					cell = new TableCell();
					cell.ColumnSpan = Convert.ToInt32(dtheader.Rows[0]["FLDCOUNT"].ToString());
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Text = "Faculty";
					row.Cells.Add(cell);
					gvBindFacultyEvaluation.Controls[0].Controls.AddAt(0, row);
				}
				gvBindFacultyEvaluation.Controls[0].Controls.AddAt(0, row);

			}
			else
			{
				BoundField field = new BoundField();
				field.HeaderText = "";
				gvBindFacultyEvaluation.Columns.Add(field);
				DataTable dt = new DataTable();
				ShowNoRecordsFound(dt, gvBindFacultyEvaluation);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvBindFacultyEvaluation_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			
		}
	}
	protected void gvBindFacultyEvaluation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindFacultyEvaluation();
	}
	protected void gvBindFacultyEvaluation_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

		_gridView.EditIndex = de.NewEditIndex;
		_gridView.SelectedIndex = de.NewEditIndex;

		BindFacultyEvaluation();
    

	}
	protected void gvBindFacultyEvaluation_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			GridViewRow gr = _gridView.Rows[nCurrentRow];
			
			BindFacultyEvaluation();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidRemarks(string remarks)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (remarks.Trim().Equals("") || remarks.Trim('.').Equals(""))
			ucError.ErrorMessage = "Office Remarks is required.";

		return (!ucError.IsError);
	}
	private bool IsValidFeedback()
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(txtComment1.Text))
			ucError.ErrorMessage = "Fill in the 1st mandatory comment";

		return (!ucError.IsError);
	}
	private bool IsValidReport()
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableInteger(ddlCandidate.SelectedValue)==null)
			ucError.ErrorMessage = "Candidate is required.";

		return (!ucError.IsError);
	}
	protected void editfeedback(int? feedbackid ,int empid,int batchid)
	{
		DataSet ds = PhoenixCrewCourseFeedback.EditCrewCourseFeedback(null, empid, batchid);
		if (ds.Tables[0].Rows.Count > 0)
		{
			ViewState["FEEDBACKID"] = ds.Tables[0].Rows[0]["FLDFEEDBACKID"].ToString();
			ddlCandidate.SelectedValue = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();

			txtComment1.Text = ds.Tables[0].Rows[0]["FLDCOMMENT1"].ToString();

			rbComment2.SelectedValue = ds.Tables[0].Rows[0]["FLDCOMMENTYN2"].ToString();
			txtComment2.Text = ds.Tables[0].Rows[0]["FLDCOMMENT2"].ToString();

			rbComment3.SelectedValue = ds.Tables[0].Rows[0]["FLDCOMMENYN3"].ToString();
			txtComment3.Text = ds.Tables[0].Rows[0]["FLDCOMMENT3"].ToString();

			rbComment4.SelectedValue = ds.Tables[0].Rows[0]["FLDCOMMENTYN4"].ToString();
			txtComment4.Text = ds.Tables[0].Rows[0]["FLDCOMMENT4"].ToString();

			txtComment5.Text = ds.Tables[0].Rows[0]["FLDCOMMENT5"].ToString();
			txtComment6.Text = ds.Tables[0].Rows[0]["FLDCOMMENT6"].ToString();

			rbComment5.SelectedValue = ds.Tables[0].Rows[0]["FLDCOMMENTYN7"].ToString();
			txtComment7.Text = ds.Tables[0].Rows[0]["FLDCOMMENT7"].ToString();

			txtComment8.Text = ds.Tables[0].Rows[0]["FLDCOMMENT8"].ToString();
		}
		else
		{
		
			txtComment1.Text = "";
			rbComment2.SelectedValue = null;
			txtComment2.Text = "";
			rbComment3.SelectedValue = null;
			txtComment3.Text = "";
			rbComment4.SelectedValue = null;
			txtComment4.Text = "";
			txtComment5.Text = "";
			txtComment6.Text = "";
			rbComment5.SelectedValue = null;
			txtComment7.Text = "";
			txtComment8.Text = "";
			ViewState["FEEDBACKID"] = "";
		}
		BindRadioButtonToTextBox();
	}
	protected void SaveFeedback()
	{
		try
		{
			if (ViewState["FEEDBACKID"].ToString() == "")
			{
				int? feedbackid = null;
				PhoenixCrewCourseFeedback.InsertCrewCourseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(ddlCandidate.SelectedValue),
					Convert.ToInt32(Request.QueryString["batchid"]),
					General.GetNullableString(txtComment1.Text),
					General.GetNullableInteger(rbComment2.SelectedValue),
					General.GetNullableString(txtComment2.Text),
					General.GetNullableInteger(rbComment3.SelectedValue),
					General.GetNullableString(txtComment3.Text),
					General.GetNullableInteger(rbComment4.SelectedValue),
					General.GetNullableString(txtComment4.Text),
					General.GetNullableString(txtComment5.Text),
					General.GetNullableString(txtComment6.Text),
					General.GetNullableInteger(rbComment5.Text),
					General.GetNullableString(txtComment7.Text),
					General.GetNullableString(txtComment8.Text),
					General.GetNullableString(txtCourseRemarks.Text),
					General.GetNullableString(txtFacultyRemarks.Text),
					ref feedbackid);
				ViewState["FEEDBACKID"] = feedbackid;

			}
			else
			{
				PhoenixCrewCourseFeedback.UpdateCrewCourseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(ViewState["FEEDBACKID"].ToString()),
				Convert.ToInt32(ddlCandidate.SelectedValue),
				Convert.ToInt32(Request.QueryString["batchid"]),
				General.GetNullableString(txtComment1.Text),
				General.GetNullableInteger(rbComment2.SelectedValue),
				General.GetNullableString(txtComment2.Text),
				General.GetNullableInteger(rbComment3.SelectedValue),
				General.GetNullableString(txtComment3.Text),
				General.GetNullableInteger(rbComment4.SelectedValue),
				General.GetNullableString(txtComment4.Text),
				General.GetNullableString(txtComment5.Text),
				General.GetNullableString(txtComment6.Text),
				General.GetNullableInteger(rbComment5.Text),
				General.GetNullableString(txtComment7.Text),
				General.GetNullableString(txtComment8.Text),
				General.GetNullableString(txtCourseRemarks.Text),
				General.GetNullableString(txtFacultyRemarks.Text));
			}
			ucStatusConf.Text = "Batch Details updated";


		
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }
}
