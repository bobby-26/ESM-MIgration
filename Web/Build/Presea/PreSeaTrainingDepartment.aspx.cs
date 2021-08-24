using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTrainingDepartment : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);

			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaTrainingDepartment.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvTrainingDepartment')", "Print Grid", "icon_print.png", "PRINT");
			MenuTrainingDepartment.AccessRights = this.ViewState;
			MenuTrainingDepartment.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Department", "DEPARTMENT");
            toolbar.AddButton("Staff", "STAFF");
            MenuTraining.AccessRights = this.ViewState;
            MenuTraining.MenuList = toolbar.Show();
            MenuTraining.SelectedMenuIndex = 0;

            if (!IsPostBack)
			{
				ViewState["deptid"] = "";			
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["departmentname"] = "";

				if (Request.QueryString["departmentid"] != null)

					ViewState["departmentid"] = Request.QueryString["departmentid"].ToString();
				else
					ViewState["departmentid"] = "";
            }			
			BindData();
			SetPageNavigator();
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

		if (ViewState["deptid"].ToString()=="")
			{
				ucError.HeaderMessage = "Navigation Error";
				ucError.ErrorMessage = "Please select a department and navigate to Staff page.";
				ucError.Visible = true;
				return;
			}
		else
		{
			if (dce.CommandName.ToUpper().Equals("STAFF"))
			{
				Response.Redirect("../PreSea/PreSeaTrainingStaff.aspx?departmentid=" + ViewState["deptid"].ToString(),false);
			}
		}
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDDEPARTMENTNAME" };
		string[] alCaptions = { "Department Name" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixPreSeaTrainingDepartment.TrainingDepartmentSearch(sortexpression, sortdirection,
				(int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=TrainingDepartment.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Staff</h3></td>");
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


	protected void TrainingDepartment_TabStripCommand(object sender, EventArgs e)
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


	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDDEPARTMENTNAME" };
		string[] alCaptions = { "Department Name" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = new DataSet();

		ds = PhoenixPreSeaTrainingDepartment.TrainingDepartmentSearch(sortexpression, sortdirection,
				(int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

		General.SetPrintOptions("gvTrainingDepartment", "Staff", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvTrainingDepartment.DataSource = ds;
			gvTrainingDepartment.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvTrainingDepartment);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}

	protected void gvTrainingDepartment_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvTrainingDepartment.SelectedIndex = -1;
		gvTrainingDepartment.EditIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;
		BindData();
		SetPageNavigator();
	}

	protected void gvTrainingDepartment_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTrainingDepartment, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvTrainingDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				if (!IsValidDepartment(((TextBox)_gridView.FooterRow.FindControl("txtDepartmentIdAdd")).Text))
				{
					ucError.Visible = true;
					return;
				}

				PhoenixPreSeaTrainingDepartment.InsertTrainingDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
									Convert.ToInt32	(((TextBox)_gridView.FooterRow.FindControl("txtDepartmentIdAdd")).Text));

				BindData();

			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixPreSeaTrainingDepartment.DeleteTrainingDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTrainingId")).Text));
			}
			else if (e.CommandName.ToUpper().Equals("SELECT"))
			{
				ViewState["deptid"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentId")).Text;
			}
			SetPageNavigator();
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvTrainingDepartment_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}

	protected void gvTrainingDepartment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvTrainingDepartment.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvTrainingDepartment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}

			TextBox tb1 = (TextBox)e.Row.FindControl("txtDepartmentIdAdd");
			if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

			ImageButton ib1 = (ImageButton)e.Row.FindControl("btnDepartmentAdd");
			if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListDepartmentAdd', 'codehelp1', '', '../Common/CommonPickListDepartment.aspx')");
		}

	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvTrainingDepartment.SelectedIndex = -1;
		gvTrainingDepartment.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;
		gvTrainingDepartment.SelectedIndex = -1;
		gvTrainingDepartment.EditIndex = -1;
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
		gvTrainingDepartment.SelectedIndex = -1;
		gvTrainingDepartment.EditIndex = -1;
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
	private bool IsValidDepartment(string departmentid)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		GridView _gridview = gvTrainingDepartment;

		if (departmentid.Trim().Equals(""))
			ucError.ErrorMessage = "Select Department";

		return (!ucError.IsError);
	}
}
