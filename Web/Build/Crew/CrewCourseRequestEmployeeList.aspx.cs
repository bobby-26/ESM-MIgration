using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewCourseRequestEmployeeList : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbarsub = new PhoenixToolbar();
			toolbarsub.AddImageButton("../Crew/CrewCourseRequestEmployeeList.aspx?courseid="+Request.QueryString["courseid"], "Export to Excel", "icon_xls.png", "Excel");
			MRMenu.AccessRights = this.ViewState;
			MRMenu.MenuList = toolbarsub.Show();
			if (!Page.IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
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
	protected void MRMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;
				string[] alColumns = { "FLDREFNO","FLDNAME", "FLDACCOMYN", "FLDRANKCODE", "FLDZONE", "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE"};
				string[] alCaptions = {"Ref No", "Name", "Accom. Req.", "Rank", "Zone", "VSL", "Available From", "Available To" };

				string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
				int sortdirection;
				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
				else
					sortdirection = 0;


				DataTable dt = PhoenixCrewCoursePlanner.CourseRequestEmployeeSearch(Convert.ToInt32(Request.QueryString["courseid"].ToString())
														, sortexpression, sortdirection
														, (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
														, ref iRowCount, ref iTotalPageCount);
				General.ShowExcel("COURSE EMPLOYEE LIST", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
		ViewState["PAGENUMBER"] = 1;
		BindData();
		SetPageNavigator();
	}
	protected void gvCourseEmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
				 && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				DataRowView drv = (DataRowView)e.Row.DataItem;
			
				LinkButton lb = (LinkButton)e.Row.FindControl("lnkEmployee");
				if (drv["FLDNEWAPP"].ToString() == "1")
				{
					lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
				}
				else
				{
					lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
				}
				
			}
		}
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
	}
	protected void gvCourseEmployeeList_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
		SetPageNavigator();
	}

	protected void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = 0; 
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		try
		{
	
			DataTable dt = PhoenixCrewCoursePlanner.CourseRequestEmployeeSearch(Convert.ToInt32(Request.QueryString["courseid"].ToString())
														, sortexpression, sortdirection
														, (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
														, ref iRowCount, ref iTotalPageCount);

			if (dt.Rows.Count > 0)
			{
				gvCourseEmployeeList.DataSource = dt;
				gvCourseEmployeeList.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvCourseEmployeeList);
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
	private void SetPageNavigator()
	{
		try
		{
			cmdPrevious.Enabled = IsPreviousEnabled();
			cmdNext.Enabled = IsNextEnabled();
			lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
			lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
			lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
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

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		try
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
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		try
		{
			gvCourseEmployeeList.SelectedIndex = -1;
			gvCourseEmployeeList.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
			else
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void ShowNoRecordsFound(DataTable dt, GridView gv)
	{
		try
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
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
}
