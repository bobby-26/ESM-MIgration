using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCourseTargetGroup : PhoenixBasePage
{
	
	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{

			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseTargetGroup.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvTargetGroup')", "Print Grid", "icon_print.png", "PRINT");
			MenuCrewCourseTargetGroup.AccessRights = this.ViewState;
			MenuCrewCourseTargetGroup.MenuList = toolbar.Show();
			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				if (Session["COURSEID"] != null)
				{
					EditCourseDetails();
				}
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

	public void BindData()
	{

		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDDESCRIPTION"};
			string[] alCaptions = { "Rank Code", "Rank Name", "Notes" };
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixCrewCourseTargetGroup.CrewCourseTargetGroupSearch(
			Convert.ToInt32(Session["COURSEID"].ToString()),
				sortexpression, sortdirection,
				Int32.Parse(ViewState["PAGENUMBER"].ToString()),
				General.ShowRecords(null),
				ref iRowCount,
				ref iTotalPageCount);
			General.SetPrintOptions("gvTargetGroup", "Course Target Group", alCaptions, alColumns, ds);
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvTargetGroup.DataSource = ds;
				gvTargetGroup.DataBind();

			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvTargetGroup);
			}
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
	protected void CrewCourseTargetGroup_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
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
	protected void gvTargetGroup_RowDataBound(object sender, GridViewRowEventArgs e)
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
			
				ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

				ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
				if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

				ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
				if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

				UserControlRank ucRank = (UserControlRank)e.Row.FindControl("ucRankEdit");
				DataRowView drv = (DataRowView)e.Row.DataItem;
				if (ucRank != null) ucRank.SelectedRank = drv["FLDRANKID"].ToString();
				
			

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

	protected void gvTargetGroup_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}

	

	protected void gvTargetGroup_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		if (e.CommandName.ToUpper().Equals("ADD"))
		{
			if (!IsValidTargetGroup(((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank))
			{
				ucError.Visible = true;
				return;
			}
			PhoenixCrewCourseTargetGroup.InsertCrewCourseTargetGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			Convert.ToInt32(((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank),
				((TextBox)_gridView.FooterRow.FindControl("txtNotesAdd")).Text,
				Convert.ToInt32 (Session["COURSEID"].ToString()),1
			);
			BindData();
		}
		else if (e.CommandName.ToUpper().Equals("SAVE"))
		{
			if (!IsValidTargetGroup(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank))
			{
				ucError.Visible = true;
				return;
			}
			PhoenixCrewCourseTargetGroup.UpdateCrewCourseTargetGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEditTargetGroupid")).Text),
				Convert.ToInt32(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank),
				((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNotesEdit")).Text,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				1 );

			_gridView.EditIndex = -1;
			BindData();
		}
		else if (e.CommandName.ToUpper().Equals("DELETE"))
		{
			PhoenixCrewCourseTargetGroup.DeleteCrewCourseTargetGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTargetGroupid")).Text));
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

	protected void gvTargetGroup_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = e.NewEditIndex;
			BindData();
			SetPageNavigator();
			((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtNotesEdit")).Focus();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvTargetGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = -1;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvTargetGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			
			if (!IsValidTargetGroup(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank))
			{
				ucError.Visible = true;
				return;
			}
			PhoenixCrewCourseTargetGroup.UpdateCrewCourseTargetGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEditTargetGroupid")).Text),
				Convert.ToInt32(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank),
				((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNotesEdit")).Text,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				1);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		_gridView.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDDESCRIPTION" };
		string[] alCaptions = { "Rank Code", "Rank Name", "Notes"};
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		 ds = PhoenixCrewCourseTargetGroup.CrewCourseTargetGroupSearch(
		Convert.ToInt32(Session["COURSEID"].ToString()),
		sortexpression, sortdirection,
		Int32.Parse(ViewState["PAGENUMBER"].ToString()),
		iRowCount,
		ref iRowCount,
		ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=CourseTargetGroup.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Course Target Group</h3></td>");
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

	protected void gvTargetGroup_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		BindData();
		SetPageNavigator();
	}

	private bool IsValidTargetGroup(string rankid)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		if (rankid == null || !Int16.TryParse(rankid, out result))
			ucError.ErrorMessage = "Rank is required.";
		return (!ucError.IsError);
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
	protected void cmdGo_Click(object sender, EventArgs e)
	{
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
		gvTargetGroup.SelectedIndex = -1;
		gvTargetGroup.EditIndex = -1;
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
	public StateBag ReturnViewState()
	{
		return ViewState;
	}

}
