using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseContact : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
                PhoenixToolbar Maintoolbar = new PhoenixToolbar();

                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Course Contact", "COURSECONTACT");
                Maintoolbar.AddButton("Fees", "FEES");  
                Maintoolbar.AddButton("Semester", "SEMESTER");                              
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");

                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();

                MenuCourseMaster.SelectedMenuIndex = 3;

				ddlDepartment.DataSource = PhoenixRegistersTrainingDepartment.ListTrainingDepartment();
				ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
				ddlDepartment.DataValueField = "FLDDEPARTMENTID";
				ddlDepartment.DataBind();

                ucCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ucCourse.Enabled = false;
			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	public void BindData()
	{

		try
		{

			DataSet ds = PhoenixPreSeaCourseContact.ListPreSeaCourseContact(Convert.ToInt32(Filter.CurrentPreSeaCourseMasterSelection));
		
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvCourseContact.DataSource = ds;
				gvCourseContact.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvCourseContact);
			}
			DataSet ds1 = PhoenixPreSeaCourseContact.ListPreSeaCourseContactUser(ddlDepartment.SelectedValue);
			DropDownList ddlCourseOfficer = (DropDownList)gvCourseContact.FooterRow.FindControl("ddlCourseContactAdd");
			ddlCourseOfficer.DataSource = ds1.Tables[0];
			ddlCourseOfficer.DataTextField = "FLDCONTACTNAME";
			ddlCourseOfficer.DataValueField = "FLDUSERCODE";
			ddlCourseOfficer.DataBind();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            {
                Response.Redirect("../PreSea/PreSeaCourseEligibiltyDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("COURSECONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaCourseContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaCourseSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaCourseFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaCourseExam.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void gvCourseContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}
	protected void gvCourseContact_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
			if (edit != null)
			{
				edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
			}

			ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
			if (save != null)
			{
				save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
			}

			ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cancel != null)
			{
				cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
			}
			DropDownList ucContactid = (DropDownList)e.Row.FindControl("ddlCourseContactEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (ucContactid != null) ucContactid.SelectedValue = drv["FLDUSERCODE"].ToString();

			DropDownList ucContactType = (DropDownList)e.Row.FindControl("ddlContactTypeEdit");
			DataRowView drv1 = (DataRowView)e.Row.DataItem;
			if (ucContactType != null) ucContactType.SelectedValue = drv1["FLDDESIGNATIONID"].ToString();
		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}
	}
	protected void gvCourseContact_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
	}
	protected void gvCourseContact_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				if (!IsValidCourseContact(((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue))
				{
					ucError.Visible = true;
					return;
				}
			PhoenixPreSeaCourseContact.InsertPreSeaCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32( ((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue),
				null,null,
				Convert.ToInt32(Filter.CurrentPreSeaCourseMasterSelection));

				BindData();
			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixPreSeaCourseContact.DeletePreSeaCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						Convert.ToInt32((((Label)_gridView.Rows[nCurrentRow].FindControl("lblContactId")).Text)));
				_gridView.EditIndex = -1;
				BindData();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvCourseContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

			if (!IsValidCourseContact(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCourseContactEdit")).SelectedValue))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixPreSeaCourseContact.UpdatePreSeaCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContactEditId")).Text),
				Convert.ToInt32(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlContactTypeEdit")).SelectedValue),
				null, null,
				Convert.ToInt32(Filter.CurrentPreSeaCourseMasterSelection));

			_gridView.EditIndex = -1;
			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidCourseContact(string coursecontact)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		if (coursecontact == null || !Int16.TryParse(coursecontact, out result))
			ucError.ErrorMessage = "Contact Name is required.";
	
		return (!ucError.IsError);
	}

	protected void gvCourseContact_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = e.NewEditIndex;
			_gridView.SelectedIndex = e.NewEditIndex;
			BindData();
			
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
	}

}
