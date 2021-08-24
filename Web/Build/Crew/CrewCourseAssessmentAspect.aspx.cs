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
public partial class CrewCourseAssessmentAspect : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{

			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseAssessmentAspect.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvAssessmentAspect')", "Print Grid", "icon_print.png", "PRINT");
			MenuCrewCourseAssessmentAspect.AccessRights = this.ViewState;
			MenuCrewCourseAssessmentAspect.MenuList = toolbar.Show();
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
			string[] alColumns = { "FLDCODE", "FLDNAME" };
			string[] alCaptions = { "Code", "Name" };
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixCrewCourseAssessmentAspect.CourseAssessmentAspectSearch(
			Convert.ToInt32(Session["COURSEID"].ToString()),
				sortexpression, sortdirection,
				Int32.Parse(ViewState["PAGENUMBER"].ToString()),
				General.ShowRecords(null),
				ref iRowCount,
				ref iTotalPageCount);
			General.SetPrintOptions("gvAssessmentAspect", "Course Target Group", alCaptions, alColumns, ds);
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvAssessmentAspect.DataSource = ds;
				gvAssessmentAspect.DataBind();

			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvAssessmentAspect);
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
	protected void CrewCourseAssessmentAspect_TabStripCommand(object sender, EventArgs e)
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
	protected void gvAssessmentAspect_RowDataBound(object sender, GridViewRowEventArgs e)
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

			UserControlAssessment ucAssess = (UserControlAssessment)e.Row.FindControl("ucAssessmentEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (ucAssess != null) ucAssess.SelectedAssessment = drv["FLDASSESSMENTID"].ToString();



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

	protected void gvAssessmentAspect_Sorting(object sender, GridViewSortEventArgs se)
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



	protected void gvAssessmentAspect_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				if (!IsValidAssessmentAspect(((UserControlAssessment)_gridView.FooterRow.FindControl("ucAssessmentAdd")).SelectedAssessment))
				{
					ucError.Visible = true;
					return;
				}
				PhoenixCrewCourseAssessmentAspect.InsertCourseAssessmentAspect(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(((UserControlAssessment)_gridView.FooterRow.FindControl("ucAssessmentAdd")).SelectedAssessment),
				Convert.ToInt32(Session["COURSEID"].ToString())
				);
				BindData();
				ucStatus.Text = "Assessment Aspects addded.";
			}
			else if (e.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidAssessmentAspect(((UserControlAssessment)_gridView.Rows[nCurrentRow].FindControl("ucAssessmentEdit")).SelectedAssessment))
				{
					ucError.Visible = true;
					return;
				}
				PhoenixCrewCourseAssessmentAspect.UpdateCourseAssessmentAspect(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentAspectid")).Text),
					Convert.ToInt32(((UserControlAssessment)_gridView.Rows[nCurrentRow].FindControl("ucAssessmentEdit")).SelectedAssessment),
					Convert.ToInt32(Session["COURSEID"].ToString()));

				_gridView.EditIndex = -1;
				BindData();
				
				
			}
			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixCrewCourseAssessmentAspect.DeleteCourseAssessmentAspect(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentAspectid")).Text));

				_gridView.EditIndex = -1;
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

	protected void gvAssessmentAspect_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = e.NewEditIndex;
			BindData();
			SetPageNavigator();
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvAssessmentAspect_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
	protected void gvAssessmentAspect_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{

			if (!IsValidAssessmentAspect(((UserControlAssessment)_gridView.Rows[nCurrentRow].FindControl("ucAssessmentEdit")).SelectedAssessment))
			{
				ucError.Visible = true;
				return;
			}
		
			PhoenixCrewCourseAssessmentAspect.UpdateCourseAssessmentAspect(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentAspectidEdit")).Text),
				Convert.ToInt32(((UserControlAssessment)_gridView.Rows[nCurrentRow].FindControl("ucAssessmentEdit")).SelectedAssessment),
				Convert.ToInt32(Session["COURSEID"].ToString()));
			ucStatus.Text = "Assessment Aspects updated.";
			BindData();
			SetPageNavigator();
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
		string[] alColumns = { "FLDCODE", "FLDNAME" };
		string[] alCaptions = { "Code", "Name"};
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		ds = PhoenixCrewCourseAssessmentAspect.CourseAssessmentAspectSearch(
	   Convert.ToInt32(Session["COURSEID"].ToString()),
	   sortexpression, sortdirection,
	   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
	   iRowCount,
	   ref iRowCount,
	   ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=CourseAssessmentAspect.xls");
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

	protected void gvAssessmentAspect_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		BindData();
		SetPageNavigator();
	}

	private bool IsValidAssessmentAspect(string assesid)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		if (!Int16.TryParse(assesid, out result))
			ucError.ErrorMessage = "Assessment is required.";
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
		gvAssessmentAspect.SelectedIndex = -1;
		gvAssessmentAspect.EditIndex = -1;
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
