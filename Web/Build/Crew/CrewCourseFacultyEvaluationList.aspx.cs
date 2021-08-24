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
public partial class CrewCourseFacultyEvaluationList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {

            foreach (GridViewRow r in gvFacultyEvaluation.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvFacultyEvaluation.UniqueID, "Edit$" + r.RowIndex.ToString());
                }
            }
            base.Render(writer);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
		
			if (!IsPostBack)
			{
                ViewState["FACULTYEVALUATIONID"] = "";
				if (Filter.CurrentCourseSelection!= null)
				{
					EditCourseDetails();
				}
				if (Request.QueryString["batchid"] != null)
				{
					EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
					ddlFaculty.DataSource = PhoenixCrewCourseEnrollment.ListCrewCourseFaculty(Convert.ToInt32(Request.QueryString["batchid"]));
					ddlFaculty.DataValueField = "FLDUSERCODE";
					ddlFaculty.DataTextField = "FLDNAME";
					ddlFaculty.DataBind();

				}
			}
			BindFacultyEvaluation();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void FacultyEvaluation_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FACULTYEVALUATION"))
		{

			
		}

	}
	protected void EditCourseDetails()
	{
		try
		{

			int Facultyid = Convert.ToInt32(Filter.CurrentCourseSelection);
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Facultyid);
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

	private void BindFacultyEvaluation()
	{


		DataSet ds = PhoenixCrewCourseFeedback.SearchFacultyEvaluation(General.GetNullableInteger(ddlFaculty.SelectedValue),
			General.GetNullableInteger(Request.QueryString["empid"]),
		   General.GetNullableInteger(Request.QueryString["batchid"]));


		if (ds.Tables[0].Rows.Count > 0)
		{
			gvFacultyEvaluation.DataSource = ds;
			gvFacultyEvaluation.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvFacultyEvaluation);
		}
	}
	protected void gvFacultyEvaluation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = -1;
			BindFacultyEvaluation();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvFacultyEvaluation_RowCommand(object sender, GridViewCommandEventArgs e)
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
	protected void gvFacultyEvaluation_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

			_gridView.SelectedIndex = e.NewEditIndex;
			_gridView.EditIndex = e.NewEditIndex;

            ViewState["FACULTYEVALUATIONID"] = ((Label)gvFacultyEvaluation.Rows[e.NewEditIndex].FindControl("lblFacultyEvaluationId")).Text;

			BindFacultyEvaluation();

            ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtMarksEdit")).SetFocus();
            ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtMarksEdit")).Attributes.Add("onfocus", "this.select()");
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void gvFacultyEvaluation_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

			string Facultyevaluationid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyEvaluationId")).Text;
			string lblEvaluationId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationId")).Text;
			string marks = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text;


			if (!IsValidFacultyEvaluation(marks))
			{
				ucError.Visible = true;
				return;
			}

			if (General.GetNullableInteger(Facultyevaluationid) == null)
			{
				PhoenixCrewCourseFeedback.InsertFacultyEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(lblEvaluationId), 
				Convert.ToInt32(marks), null,
				Convert.ToInt32(ddlFaculty.SelectedValue),
				Convert.ToInt32(Request.QueryString["batchid"]), 
				Convert.ToInt32(Request.QueryString["empid"]));
			}
			else
			{

				PhoenixCrewCourseFeedback.UpdateFacultyEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(Facultyevaluationid),
					Convert.ToInt32(lblEvaluationId),
					Convert.ToInt32(marks), null,
					Convert.ToInt32(ddlFaculty.SelectedValue),
					Convert.ToInt32(Request.QueryString["batchid"]),
					Convert.ToInt32(Request.QueryString["empid"]));
			}
			_gridView.EditIndex = -1;
			BindFacultyEvaluation();

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidFacultyEvaluation(string value)
	{
		ucError.HeaderMessage = "Please enter marks between 1 to 10";

		if (value.Trim() == "")
			ucError.ErrorMessage = "Rating is Required";

		if (General.GetNullableInteger(value) > 10 || General.GetNullableInteger(value) < 0)
			ucError.ErrorMessage = "Rating should be between 0 to 10";

		if (General.GetNullableInteger(ddlFaculty.SelectedValue) == null)
			ucError.ErrorMessage = "Please select a Faculty";

		return (!ucError.IsError);
	}

	protected void gvFacultyEvaluation_ItemDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvFacultyEvaluation_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvFacultyEvaluation.SelectedIndex = se.NewSelectedIndex;
        Label lblFacultyEvaluationId = (Label)gvFacultyEvaluation.Rows[se.NewSelectedIndex].FindControl("lblFacultyEvaluationId");
        if (lblFacultyEvaluationId != null)
            ViewState["FACULTYEVALUATIONID"] = lblFacultyEvaluationId.Text;

        if (gvFacultyEvaluation.EditIndex > -1)
            gvFacultyEvaluation.UpdateRow(gvFacultyEvaluation.EditIndex, false);

        gvFacultyEvaluation.EditIndex = -1;
        BindFacultyEvaluation();
    }
    protected void gvFacultyEvaluation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFacultyEvaluation, "Edit$" + e.Row.RowIndex.ToString(), false);
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
	protected void gvFacultyEvaluation_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string Facultyevaluationid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyEvaluationId")).Text;
			if (Facultyevaluationid != "")
			{
				PhoenixCrewCourseFeedback.DeleteFacultyEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
										Convert.ToInt32(Facultyevaluationid));
			}
			BindFacultyEvaluation();

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


}
