using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchContact : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
                PhoenixToolbar MainToolbar = new PhoenixToolbar();

                MainToolbar.AddButton("Batch", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
                MainToolbar.AddButton("Events", "EVENT");
                MainToolbar.AddButton("Semester", "SEMESTER");
                MainToolbar.AddButton("Subjects", "SUBJECTS");
                MainToolbar.AddButton("Exam", "EXAM");
                MainToolbar.AddButton("Contact", "CONTACT");

                MenuBatchManager.AccessRights = this.ViewState;
                MenuBatchManager.MenuList = MainToolbar.Show();

                MenuBatchManager.SelectedMenuIndex = 6;

                ucBatch.SelectedBatch = Filter.CurrentPreSeaBatchManagerSelection;
                ucBatch.Enabled = false;
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
			DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchContact(Convert.ToInt32(Filter.CurrentPreSeaBatchManagerSelection));
		
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvBatchContact.DataSource = ds;
				gvBatchContact.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvBatchContact);
			}
            //DataSet ds1 = PhoenixPreSeaCourseContact.ListPreSeaCourseContact(Convert.ToInt32(Session["BATCHMANAGECOURSE"].ToString()));
            DataSet ds1 = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(Filter.CurrentPreSeaBatchManagerSelection));
			DropDownList ddlCourseOfficer = (DropDownList)gvBatchContact.FooterRow.FindControl("ddlCourseContactAdd");
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

    protected void MenuBatchManager_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManageDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EVENT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchEvents.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchContact.aspx");
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void gvBatchContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void gvBatchContact_RowDataBound(object sender, GridViewRowEventArgs e)
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

	protected void gvBatchContact_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
	}

	protected void gvBatchContact_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADD"))
			{
                if (!IsValidBatchContact(((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue))
				{
					ucError.Visible = true;
					return;
				}
			PhoenixPreSeaBatchManager.InsertPreSeaBatchContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32( ((DropDownList)_gridView.FooterRow.FindControl("ddlCourseContactAdd")).SelectedValue),
				null,null,
				Convert.ToInt32(Filter.CurrentPreSeaBatchManagerSelection));

				BindData();
			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixPreSeaBatchManager.DeletePreSeaBatchContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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

	protected void gvBatchContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

			if (!IsValidBatchContact(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCourseContactEdit")).SelectedValue))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixPreSeaBatchManager.UpdatePreSeaBatchContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContactEditId")).Text),
				Convert.ToInt32(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlContactTypeEdit")).SelectedValue),
				null, null,
				Convert.ToInt32(Filter.CurrentPreSeaBatchManagerSelection));

			_gridView.EditIndex = -1;
			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidBatchContact(string batchcontact)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
        if (batchcontact == null || !Int16.TryParse(batchcontact, out result))
			ucError.ErrorMessage = "Contact Name is required.";
	
		return (!ucError.IsError);
	}

	protected void gvBatchContact_RowEditing(object sender, GridViewEditEventArgs e)
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
