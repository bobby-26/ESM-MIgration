using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Text;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTrainingStaff : PhoenixBasePage
{

	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvMapping.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvMapping.UniqueID, "Edit$" + r.RowIndex.ToString());
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
			toolbar.AddImageButton("../PreSea/PreSeaTrainingStaff.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvMapping')", "Print Grid", "icon_print.png", "PRINT");
			MenuTrainingStaff.AccessRights = this.ViewState;
			MenuTrainingStaff.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Department", "DEPARTMENT");
            toolbar.AddButton("Staff", "STAFF");
            MenuTraining.AccessRights = this.ViewState;
            MenuTraining.MenuList = toolbar.Show();
            MenuTraining.SelectedMenuIndex = 1;

            if (!IsPostBack)
			{				
				ddlDepartment.DataSource = PhoenixPreSeaTrainingDepartment.ListTrainingDepartment();
				ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
				ddlDepartment.DataValueField = "FLDDEPARTMENTID";
				ddlDepartment.DataBind();
				if (Request.QueryString["departmentid"] != null)
				{
					ddlDepartment.SelectedValue = Request.QueryString["departmentid"];
					ddlDepartment.Enabled = false;
				}
			}			
			BindMappingData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void Training_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		UserControlTabs ucTabs = (UserControlTabs)sender;

		if (dce.CommandName.ToUpper().Equals("DEPARTMENT"))
		{
			Response.Redirect("../PreSea/preSeaTrainingDepartment.aspx?departmentid=" + Request.QueryString["departmentid"], false);
		}
		
	}
	protected void ShowExcel()
	{
		
		DataSet ds = new DataSet();
		string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATIONNAME", "FLDFACULTYCODE" };
		string[] alCaptions = { "Name","Designation", "Code" };

		ds = PhoenixPreSeaFacultyDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));

		Response.AddHeader("Content-Disposition", "attachment; filename=TrainingStaff.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> Training Staff</h3></td>");
		Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<b>Department</b> " + ddlDepartment.SelectedItem.Text);
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

	protected void TrainingStaff_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
				BindMappingData();

			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	public void BindMappingData()
	{

		try
		{
			string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATIONNAME", "FLDFACULTYCODE" };
			string[] alCaptions = { "Name", "Designation", "Code" };
			DataSet ds = PhoenixPreSeaFacultyDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));
			General.SetPrintOptions("gvMapping", "Training Staff", alCaptions, alColumns, ds);
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvMapping.DataSource = ds;
				gvMapping.DataBind();

			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvMapping);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvMapping_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindMappingData();
	}

	protected void gvMapping_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			CheckBoxList chkdesignation = ((CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("cblDesignationEdit"));
			StringBuilder strdesignationlist = new StringBuilder();              
			foreach (ListItem item in chkdesignation.Items)
			{
				if (item.Selected == true)
				{
					strdesignationlist.Append(item.Value.ToString());
					strdesignationlist.Append(",");
				}
			}
			if (strdesignationlist.Length > 1)
			{
				strdesignationlist.Remove(strdesignationlist.Length - 1, 1);
			}

			if (!IsValidMapping(strdesignationlist.ToString(),
												((Label)_gridView.Rows[nCurrentRow].FindControl("lblUserCodeEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}
			if (General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingIdEdit")).Text) == null)
			{
				PhoenixPreSeaFacultyDesignation.InsertDesignationMapping(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					strdesignationlist.ToString(),
					Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUserCodeEdit")).Text),
				   ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyCodeEdit")).Text,null
			);
			}
			else
			{
				PhoenixPreSeaFacultyDesignation.UpdateDesignationMapping(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingIdEdit")).Text),
					strdesignationlist.ToString(),
					Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUserCodeEdit")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyCodeEdit")).Text,
                    General.GetNullableDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyLoadEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucRoleEdit")).SelectedQuick)
				);
			}
			_gridView.EditIndex = -1;
			BindMappingData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		_gridView.EditIndex = -1;
		BindMappingData();
	}
	protected void gvMapping_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixPreSeaFacultyDesignation.DeleteDesignationMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingIdadd")).Text));
				BindMappingData();
			}

		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidMapping(string desingationid, string userid)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableString(desingationid) == null)
			ucError.ErrorMessage = "Designation is required.";

		if (General.GetNullableInteger(userid) == null)
			ucError.ErrorMessage = "Name is required.";

		return (!ucError.IsError);
	}
	protected void gvMapping_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindMappingData();
	}
	protected void gvMapping_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMapping, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}
	protected void gvMapping_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;
			BindMappingData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvMapping_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null)
			{
				db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
			}

			ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
			if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

			ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
			if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

			ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

			DropDownList userid = (DropDownList)e.Row.FindControl("ddlCourseContactEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (userid != null) userid.SelectedValue = drv["FLDUSERID"].ToString();

			CheckBoxList cbldesignation = (CheckBoxList)e.Row.FindControl("cblDesignationEdit");
			DataRowView drv1 = (DataRowView)e.Row.DataItem;
			if (cbldesignation != null)
			{
				string[] strdesignation = drv1["FLDDESIGNATIONID"].ToString().Split(',');
				foreach (string item in strdesignation)
				{
					if (item.Trim() != "")
					{
						cbldesignation.Items.FindByValue(item).Selected = true;
					}

				}
				
			}
            UserControlQuick ucRoleEdit = (UserControlQuick)e.Row.FindControl("ucRoleEdit");
            if (ucRoleEdit != null)
            {
                ucRoleEdit.bind();
                ucRoleEdit.SelectedQuick = drv1["FLDROLEID"].ToString();
            }
		}
		
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}