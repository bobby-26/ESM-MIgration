using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewCourseContact : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
				if (Session["COURSEID"] != null)
				{
					EditCourse();
				}
				ddlDepartment.DataSource = PhoenixRegistersTrainingDepartment.ListTrainingDepartment();
				ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
				ddlDepartment.DataValueField = "FLDDEPARTMENTID";
				ddlDepartment.DataBind();
				ddlContactType.DataSource = PhoenixRegistersCourseContact.ListDesignation();
				ddlContactType.DataTextField = "FLDDESIGNATIONNAME";
				ddlContactType.DataValueField = "FLDDESIGNATIONID";
				ddlContactType.DataBind();
			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void EditCourse()
	{
		try
		{
			int courseid = Convert.ToInt32(Session["COURSEID"].ToString());
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

	public void BindData()
	{

		try
		{

			DataSet ds = PhoenixRegistersCourseContact.ListCourseContact(Convert.ToInt32(Session["COURSEID"].ToString()));
		
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
			DataSet ds1 = PhoenixRegistersCourseContact.ListCourseContactUser(General.GetNullableInteger(ddlDepartment.SelectedValue), 
							General.GetNullableInteger(ddlContactType.SelectedValue));

			DropDownList ddlCourseOfficer = (DropDownList)gvCourseContact.FooterRow.FindControl("ddlCourseContactAdd");
			ddlCourseOfficer.DataSource = ds1.Tables[0];
			ddlCourseOfficer.DataTextField = "FLDCONTACTNAME";
			ddlCourseOfficer.DataValueField = "FLDUSERCODE";
			ddlCourseOfficer.DataBind();

			DropDownList ddlDesignation = (DropDownList)gvCourseContact.FooterRow.FindControl("ddlContactTypeAdd");
			ddlDesignation.DataSource = PhoenixRegistersCourseContact.ListDesignation();
			ddlDesignation.DataTextField = "FLDDESIGNATIONNAME";
			ddlDesignation.DataValueField = "FLDDESIGNATIONID";
			ddlDesignation.DataBind();

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
				if (!IsValidCourseContact(((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue,
					((DropDownList)_gridView.FooterRow.FindControl("ddlContactTypeAdd")).SelectedValue))
				{
					ucError.Visible = true;
					return;
				}
			PhoenixRegistersCourseContact.InsertCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32( ((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue),
				null,null,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				Convert.ToInt32( ((DropDownList)_gridView.FooterRow.FindControl("ddlContactTypeAdd")).SelectedValue));

				BindData();
			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixRegistersCourseContact.DeleteCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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

			if (!IsValidCourseContact(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCourseContactEdit")).SelectedValue,
				((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlContactTypeEdit")).SelectedValue))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixRegistersCourseContact.UpdateCourseContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContactEditId")).Text),
				Convert.ToInt32(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlContactTypeEdit")).SelectedValue),
				null, null,
				Convert.ToInt32(Session["COURSEID"].ToString()));

			_gridView.EditIndex = -1;
			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidCourseContact(string coursecontact,string designationid)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		if (coursecontact == null || !Int16.TryParse(coursecontact, out result))
			ucError.ErrorMessage = "Contact Name is required.";

		if (General.GetNullableInteger(designationid)==null)
			ucError.ErrorMessage = "Contact Type is required.";

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
