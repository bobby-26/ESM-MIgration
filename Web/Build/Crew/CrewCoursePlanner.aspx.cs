using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;

public partial class CrewCoursePlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Show Planner", "SHOWPLANNER");
			CrewMenuCoursePlanner.AccessRights = this.ViewState;
			CrewMenuCoursePlanner.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				rblPlannerList.SelectedValue = "2";
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ViewState["courseid"] = "";
				ViewState["row"] = "";
				chkIncludeHoliday.Checked = true;
				chkExcludeSunday.Checked = true;
				SetPrimaryDetails(null, 0, System.DateTime.Today,null,chkExcludeSunday.Checked==true?1:0);

				PhoenixToolbar toolbarcourse = new PhoenixToolbar();
				toolbarcourse.AddImageButton("../Crew/CrewCoursePlanner.aspx", "Find", "search.png", "FIND");
				MenuCrewCourseList.AccessRights = this.ViewState;
				MenuCrewCourseList.MenuList = toolbarcourse.Show();
				BindCourse();

				PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
				toolbarPlannerList.AddImageButton("../Crew/CrewCoursePlanner.aspx?", "Export to Excel", "icon_xls.png", "Excel");
				MenuGridCoursePlannerList.AccessRights = this.ViewState;
				MenuGridCoursePlannerList.MenuList = toolbarPlannerList.Show();
				BindCoursePlannerList();
			}
			
			BindCourse();
			SetPageNavigator();
			if (rblPlannerList.SelectedValue == "1")
			{
				BindCoursePlannerList();
			}
			else
			{
				BindCourseRowPlannerList();
			}
			
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }


	protected void SetPrimaryDetails(int? type,int? dateadd,DateTime date,int? yn,int? excludesunday)
	{
		DataSet ds = PhoenixCrewCoursePlanner.SearchFirstLastWeekDay(type, dateadd, date, excludesunday);
		if (ds.Tables[0].Rows.Count > 0)
		{
			if (yn != 1)
			{
				txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFIRSTDATEOFWEEK"].ToString();
				ViewState["fromdate"] = txtFromDate.Text;
			}
			txtToDate.Text = ds.Tables[0].Rows[0]["FLDLASTDATEOFWEEK"].ToString();
		}
	}
	protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FIND"))
		{
			ViewState["PAGENUMBER"] = 1;
			BindCourse();
			SetPageNavigator();
		}
	
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		if (rblPlannerList.SelectedValue == "1")
		{
			BindCoursePlannerList();
		}
		else
		{
			BindCourseRowPlannerList();
		}
	}
	protected void ExcludeSunday(object sender, EventArgs e)
	{
		SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
		if (rblPlannerList.SelectedValue == "1")
		{
			BindCoursePlannerList();
		}
		else
		{
			BindCourseRowPlannerList();
		}
	}

	protected void IncludeHoliday(object sender, EventArgs e)
	{
		SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
		if (rblPlannerList.SelectedValue == "1")
		{
			BindCoursePlannerList();
		}
		else
		{
			BindCourseRowPlannerList();
		}
	}
	protected void WeekSetting(object sender, EventArgs e)
	{
		try
		{
			int yn=0;
			if(ViewState["fromdate"].ToString()!="")
			{
			DateTime dtfrom=Convert.ToDateTime(ViewState["fromdate"]);
			DateTime dtto=Convert.ToDateTime(txtFromDate.Text );
				if(dtfrom>dtto)
				{
					yn=1;
				}
			}

			SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), yn, chkExcludeSunday.Checked == true ? 1 : 0);
			if (rblPlannerList.SelectedValue == "1")
			{
				BindCoursePlannerList();
			}
			else
			{
				BindCourseRowPlannerList();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void PrevClick(object sender, EventArgs e)
	{
		try
		{

			SetPrimaryDetails(1,  chkExcludeSunday.Checked == true ? 7:8, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
			if (rblPlannerList.SelectedValue == "1")
			{
				BindCoursePlannerList();
			}
			else
			{
				BindCourseRowPlannerList();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void NextClick(object sender, EventArgs e)
	{
		SetPrimaryDetails(null, chkExcludeSunday.Checked == true ? 7 : 8, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
		if (rblPlannerList.SelectedValue == "1")
		{
			BindCoursePlannerList();
		}
		else
		{
			BindCourseRowPlannerList();
		}
	}
	  public bool IsValidDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
		DateTime dt;
		if (txtFromDate.Text == null)
			ucError.ErrorMessage = "DOA Given date is required.";

		else if (!string.IsNullOrEmpty(txtFromDate.Text)
			&& DateTime.TryParse(txtToDate.Text, out dt) && DateTime.Compare(dt, DateTime.Parse(txtFromDate.Text)) < 0)
		{
			ucError.ErrorMessage = "To Date should be later than 'From Date'";
		}


		 return (!ucError.IsError);

	}
	private void BindCourse()
	{
		try
		{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixCrewCoursePlanner.CourseRequestSearch(General.GetNullableString(txtSearchCourse.Text),
            General.GetNullableInteger(""),
			 sortexpression, sortdirection,
			(int)ViewState["PAGENUMBER"],
			10,
			ref iRowCount,
			ref iTotalPageCount,1);

	
		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseList.DataSource = ds;
			gvCourseList.DataBind();
		
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseList);
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
	
	public void BindCoursePlannerList()
	{

		try
		{
			gvCoursePlannerList.Columns.Clear();

			DataSet ds = PhoenixCrewCoursePlanner.ListCoursePlannerWeekly(General.GetNullableDateTime(txtFromDate.Text),
				General.GetNullableDateTime(txtToDate.Text), chkExcludeSunday.Checked == true ? 1 : 0, chkIncludeHoliday.Checked == true ? 1 : 0);

			string[] nonaggcol = { "Course Name", "No of Candidates","Batch No", "From Date", "To Date" };
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{

				for (int i = 0; i < nonaggcol.Length; i++)
				{
					if (nonaggcol[i] == "No of Candidates")
					{

					    HyperLinkField lnk = new HyperLinkField();
					    lnk.HeaderText = nonaggcol[i];
					    lnk.DataTextField = nonaggcol[i];

						gvCoursePlannerList.Columns.Add(lnk);
					}
					else
					{
						BoundField field = new BoundField();

						field.DataField = nonaggcol[i];
						field.HeaderText = nonaggcol[i];
						if (i == 3 || i == 4)
							field.DataFormatString = "{0:dd/MM/yyyy}";

						gvCoursePlannerList.Columns.Add(field);
					}
				}
				DataTable dt = ds.Tables[1];

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					BoundField field = new BoundField();

					field = new BoundField();
					field.DataField = (i + 1).ToString();
					field.HeaderText = dt.Rows[i]["FLDDATE"].ToString();
					field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
					field.ItemStyle.Font.Bold = true;
					gvCoursePlannerList.Columns.Add(field);

				}
				DataTable dtrow = ds.Tables[3];
				if (dtrow.Rows.Count > 0)
				{
					ViewState["row"] = dtrow.Rows[0]["FLDROW"];
					ViewState["indexcount"] = "4";
				}
				gvCoursePlannerList.DataSource = ds;
				gvCoursePlannerList.DataBind();
				GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
				row.Attributes.Add("style", "position:static");
				TableCell cell = new TableCell();
				cell.ColumnSpan = 5;
				row.Cells.Add(cell);

				DataTable dtheader = ds.Tables[2];
				int cnt = 0;
				for (int i = 0; i < dtheader.Rows.Count; i++)
				{
					cell = new TableCell();
					cell.ColumnSpan = cnt + 2;
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Text = dtheader.Rows[i]["FLDDATE"].ToString();
					row.Cells.Add(cell);
					gvCoursePlannerList.Controls[0].Controls.AddAt(0, row);
				}

				
			}
			else
			{
				ViewState["row"]="";
				BoundField field = new BoundField();
				field.HeaderText = "";
				gvCoursePlannerList.Columns.Add(field);
				DataTable dt = new DataTable();
				ShowNoRecordsFound(dt, gvCoursePlannerList);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	public void BindCourseRowPlannerList()
	{

		try
		{
			gvCoursePlannerList.Columns.Clear();

			DataSet ds = PhoenixCrewCoursePlanner.ListCourseRowPlannerWeekly(General.GetNullableDateTime(txtFromDate.Text),
				General.GetNullableDateTime(txtToDate.Text), chkExcludeSunday.Checked == true ? 1 : 0, chkIncludeHoliday.Checked == true ? 1 : 0);

			string[] nonaggcol = { "Course Name", "No of Candidates", "Batch No", "From Date", "To Date", "Session" };
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < nonaggcol.Length; i++)
				{
					if (nonaggcol[i] == "No of Candidates")
					{

						HyperLinkField lnk = new HyperLinkField();
						lnk.HeaderText = nonaggcol[i];
						lnk.DataTextField = nonaggcol[i];
						gvCoursePlannerList.Columns.Add(lnk);
					
					}
					else
					{
						BoundField field = new BoundField();

						field.DataField = nonaggcol[i];
						field.HeaderText = nonaggcol[i];
						if (i == 3 || i == 4)
							field.DataFormatString = "{0:dd/MM/yyyy}";

						gvCoursePlannerList.Columns.Add(field);
					}
				}

				DataTable dt = ds.Tables[1];
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					BoundField field = new BoundField();

					field = new BoundField();
					field.DataField = (i + 1).ToString();
					field.HeaderText = dt.Rows[i]["FLDDATE"].ToString();
					field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
					field.ItemStyle.Font.Bold = true;
					gvCoursePlannerList.Columns.Add(field);

				}
				DataTable dtrow = ds.Tables[2];
				if (dtrow.Rows.Count > 0)
				{
					ViewState["row"] = dtrow.Rows[0]["FLDROW"];
					ViewState["indexcount"] = "5";
				}
				gvCoursePlannerList.DataSource = ds;
				gvCoursePlannerList.DataBind();
				GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
				row.Attributes.Add("style", "position:static");
				TableCell cell = new TableCell();
				cell.ColumnSpan = 5;
				row.Cells.Add(cell);

			}
			else
			{
				ViewState["row"] = "";
				BoundField field = new BoundField();
				field.HeaderText = "";
				gvCoursePlannerList.Columns.Add(field);
				DataTable dt = new DataTable();
				ShowNoRecordsFound(dt, gvCoursePlannerList);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCoursePlannerList_DataBound(object sender, EventArgs e)
	{
		try
		{
			for (int rowIndex = gvCoursePlannerList.Rows.Count - 2; rowIndex >= 0; rowIndex--)
			{
				GridViewRow gvRow = gvCoursePlannerList.Rows[rowIndex];
				GridViewRow gvPreviousRow = gvCoursePlannerList.Rows[rowIndex + 1];
				for (int cellCount = 0; cellCount < gvRow.Cells.Count; cellCount++)
				{
					if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text &&
						gvRow.Cells[cellCount].Text != "&nbsp;" && gvPreviousRow.Cells[cellCount].Text != "&nbsp;")
					{
						if (gvRow.Cells[0].Text == gvPreviousRow.Cells[0].Text && gvRow.Cells[2].Text == gvPreviousRow.Cells[2].Text)
						{
							if (cellCount <= 4)
							{
								gvRow.Cells[cellCount].RowSpan = 2;
								gvPreviousRow.Cells[cellCount].Visible = false;					
							}
							else
							{
								gvPreviousRow.Cells[cellCount].Visible = true;
							}
						}

					}
				}
			}
		 }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void MenuGridCoursePlannerList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{

				if (rblPlannerList.SelectedValue == "1")
				{
					BindCoursePlannerList();
				}
				else
				{
					BindCourseRowPlannerList();
				}
				Response.ClearContent();
				Response.ContentType = "application/ms-excel";
				Response.AddHeader("content-disposition", "attachment;filename=PlannerList.xls");
				Response.Charset = "";
				System.IO.StringWriter stringwriter = new System.IO.StringWriter();
				stringwriter.Write("<table><tr><td colspan=\"" + gvCoursePlannerList.Columns.Count + "\"></td></tr></table>");
				HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
				gvCoursePlannerList.RenderControl(htmlwriter);
				Response.Write(stringwriter.ToString());
				Response.End();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCoursePlannerList_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (e.Row.Cells.Count > 1)
			{
				HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
				if (hl != null)
				{
					hl.NavigateUrl = "#";
					hl.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','CrewCourseRequestEmployeeList.aspx?courseid=" + drv["FLDCOURSEID"].ToString() + "'); return false;");
				}
				
			}

		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (ViewState["row"].ToString() != "")
			{
				for (int i = 0; i < gvCoursePlannerList.Columns.Count; i++)
				{
					string[] s = ViewState["row"].ToString().Split(',');
					for (int k = 0; k < s.Length; k++)
					{

						if (e.Row.Cells[Convert.ToInt32(ViewState["indexcount"]) + Convert.ToInt32(s[k])].Text == "&nbsp;")
						{
							e.Row.Cells[Convert.ToInt32(ViewState["indexcount"]) + Convert.ToInt32(s[k])].Text = "H";

						}
					}

				}

			}
			
		}
	}
	
	protected void CrewMenuCoursePlanner_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SHOWPLANNER"))
			{
				if (rblPlannerList.SelectedValue == "1")
				{
					BindCoursePlannerList();
				}
				else
				{
					BindCourseRowPlannerList();
				}
			}
			
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	public override void VerifyRenderingInServerForm(Control control)
	{
		return;
	}
	protected void gvCourseList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
				
				
			int a = chkExcludeSunday.Checked == true ? 1 : 0;
			int b = chkIncludeHoliday.Checked == true ? 1 : 0;
			Label lblcourseid = (Label)e.Row.FindControl("lblCourseId");

			LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkCourse");
			lbtn.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewCoursePlannerBatch.aspx?courseid=" + lblcourseid.Text + "&fromdate=" + txtFromDate.Text + "&excludesunday=" + a + "&todate=" + txtToDate.Text + "&includeholiday=" + b + "'); return false;");
		}

	}
	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindCourse();
		SetPageNavigator();
	}
	protected void gvCourseList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCourseList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
		BindCoursePlannerList();
	}
	protected void gvCourseList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvCourseList.SelectedIndex = -1;
		gvCourseList.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindCourse();
		
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		BindCourse();
		SetPageNavigator();
		
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
		BindCourse();
		SetPageNavigator();
		
	}

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvCourseList.SelectedIndex = -1;
		gvCourseList.EditIndex = -1;
		if (ce.CommandName == "prev")
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
		else
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

		BindCourse();
		SetPageNavigator();

		if (rblPlannerList.SelectedValue == "1")
		{
			BindCoursePlannerList();
		}
		else
		{
			BindCourseRowPlannerList();
		}
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
	protected void gvCoursePlanner_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = -1;
			
			BindCoursePlannerList();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvCoursePlanner_RowCommand(object sender, GridViewCommandEventArgs e)
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
	protected void gvCoursePlanner_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
		
			GridView _gridView = (GridView)sender;
			_gridView.SelectedIndex = e.NewEditIndex;

			_gridView.EditIndex = e.NewEditIndex;
			
			
			BindCoursePlannerList();
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCoursePlanner_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;



			string dateam = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSplitAM")).Text;
			string datepm = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSplitPM")).Text;
			string facultyam = ((UserControlFaculty)_gridView.Rows[nCurrentRow].FindControl("ucFacultyAMEdit")).SelectedFaculty;
			string facultypm = ((UserControlFaculty)_gridView.Rows[nCurrentRow].FindControl("ucFacultyPMEdit")).SelectedFaculty;

			if (!IsValidCoursePlanner(ViewState["courseid"].ToString()))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixCrewCoursePlanner.InsertCoursePlanner(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				  Convert.ToInt32(ViewState["courseid"].ToString())
				, Convert.ToDateTime(dateam)
				, Convert.ToDateTime(datepm)
				, General.GetNullableInteger(facultyam)
				, General.GetNullableInteger(facultypm)
				, null
				);
			_gridView.EditIndex = -1;
		
			BindCoursePlannerList();
			
		}

		catch (Exception ex)
		{
			BindCoursePlannerList();
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	private bool IsValidCoursePlanner(string courseid)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (courseid.Trim() == "")
			ucError.ErrorMessage = "Please select a course and then save the Plan";

		return (!ucError.IsError);
	}
	protected void gvCoursePlanner_ItemDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			DataRowView drvHoliday = (DataRowView)e.Row.DataItem;

			ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
			if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

			Label lblHoliday = (Label)e.Row.FindControl("lblHoliday");
			Label lblDate = (Label)e.Row.FindControl("lblSplitAM");
			Label lblSplitam = (Label)e.Row.FindControl("lblFacultyAM");
			Label lblSplitpm = (Label)e.Row.FindControl("lblFacultyPM");
			if (lblHoliday.Text == "H")
			{
				lblSplitam.Text = "Holiday";
				lblSplitpm.Text = "Holiday";
				ed.Visible = false;
				e.Row.BackColor = Color.FromName("silver");

			}
			

			if (Convert.ToDateTime(lblDate.Text).AddDays(1) < System.DateTime.Now)
			{
				ed.Visible = false;
			}

			UserControlFaculty ucFacultyAM = (UserControlFaculty)e.Row.FindControl("ucFacultyAMEdit");
			DataRowView drvFacultyAM = (DataRowView)e.Row.DataItem;
			if (ucFacultyAM != null) ucFacultyAM.SelectedFaculty = drvFacultyAM["FLDFACULTYAM"].ToString();

			UserControlFaculty ucFacultyPM = (UserControlFaculty)e.Row.FindControl("ucFacultyPMEdit");
			DataRowView drvFacultyPM = (DataRowView)e.Row.DataItem;
			if (ucFacultyPM != null) ucFacultyPM.SelectedFaculty = drvFacultyPM["FLDFACULTYPM"].ToString();

		}
	}
	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}
