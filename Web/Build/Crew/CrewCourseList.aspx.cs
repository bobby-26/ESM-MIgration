﻿using System;
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
using Telerik.Web.UI;
public partial class CrewCourseList : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        //ucCourseType.HardTypeCode = "103";
        //ucCourseType.ShortNameFilter = "447,448,449,451,452,532";
    }
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();

		toolbar.AddImageButton("../Crew/CrewCourseList.aspx?type=" + Request.QueryString["type"], "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvCourseList')", "Print Grid", "icon_print.png", "PRINT");
		toolbar.AddImageButton("../Crew/CrewCourseList.aspx?type=" + Request.QueryString["type"], "Find", "search.png", "FIND");
		MenuCrewCourseList.AccessRights = this.ViewState;
		MenuCrewCourseList.MenuList = toolbar.Show();
		MenuCrewCourseList.SetTrigger(pnlCourseListEntry);
		cmdHiddenSubmit.Attributes.Add("style", "display:none;");

		if (!IsPostBack)
		{
			ViewState["type"] = "";
			if (Request.QueryString["type"] != null)
			{
				ViewState["type"] = Request.QueryString["type"];
			}
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
		}
		BindData();
		SetPageNavigator();
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDABBREVIATION", "FLDCOURSE", "FLDDOCUMENTTYPENAME" };
		string[] alCaptions = { "Code", "Course", "Course Type" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		ds = PhoenixRegistersDocumentCourse.DocumentCourseSearch(null, General.GetNullableString(txtSearchCourse.Text), null, null, null, null, null, "", null, sortexpression, sortdirection,
			1,
			iRowCount,
			ref iRowCount,
			ref iTotalPageCount, null, null, null);

		Response.AddHeader("Content-Disposition", "attachment; filename=CourseList.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Course List</h3></td>");
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

	protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FIND"))
		{
			ViewState["PAGENUMBER"] = 1;
			BindData();
			SetPageNavigator();
		}
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDABBREVIATION", "FLDCOURSE", "FLDDOCUMENTTYPENAME" };
		string[] alCaptions = { "Code", "Course", "Course Type" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		string courseSearch = (txtSearchCourse.Text == null) ? "" : txtSearchCourse.Text;

		DataSet ds = PhoenixCrewCoursePlanner.CourseRequestSearch(
			                                    General.GetNullableString(txtSearchCourse.Text)
                                            ,   General.GetNullableInteger(ucCourseType.SelectedHard)
                                            ,   sortexpression, sortdirection,(int)ViewState["PAGENUMBER"]
                                            ,   General.ShowRecords(null),ref iRowCount,ref iTotalPageCount
                                            ,   General.GetNullableInteger(ViewState["type"].ToString())
                                            );

		General.SetPrintOptions("gvCourseList", "Course List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseList.DataSource = ds;
			gvCourseList.DataBind();
			if (Session["COURSEID"] == null)
			{
				gvCourseList.SelectedIndex = 0;
				Session["COURSEID"] = ((Label)gvCourseList.Rows[0].FindControl("lblDocumentId")).Text;

			}
			SetRowSelection();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseList);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}
	private void SetRowSelection()
	{
		gvCourseList.SelectedIndex = -1;
		for (int i = 0; i < gvCourseList.Rows.Count; i++)
		{
			if (gvCourseList.DataKeys[i].Value.ToString().Equals(Session["COURSEID"].ToString()))
			{
				gvCourseList.SelectedIndex = i;
				
			}
		}
	}
	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
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
			if (e.CommandName.ToUpper().Equals("EDIT"))
			{
				
				Session["COURSEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId")).Text;
				if (Request.QueryString["type"] == "1" || Request.QueryString["type"] == "3")
				{
					string Script = "";
					Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
					Script += "parent.selectTab('MenuCourseEnrollment', 'Nomination List');";
					Script += "</script>" + "\n";
					Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
				}
				else
				{
					string Script = "";
					Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
					Script += "parent.selectTab('MenuCourseMaster', 'Organization');";
					Script += "</script>" + "\n";
					Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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


	protected void gvCourseList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
		Session["COURSEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId")).Text;
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
			if (Request.QueryString["type"] == "1" || Request.QueryString["type"] == "3")
			{
				e.Row.Cells[3].Visible = false;
			}
			else
			{
				e.Row.Cells[4].Visible = false;
			}
		}

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
			
				Label l = (Label)e.Row.FindControl("lblDocumentId");

				LinkButton lb = (LinkButton)e.Row.FindControl("lnkDocumentType");
				//lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersDocumentCourseList.aspx?DocumentCourseId=" + l.Text + "');return false;");
				if (Request.QueryString["type"] == "1" || Request.QueryString["type"] == "3")
				{
					e.Row.Cells[3].Visible = false;
				}
				else
				{
					e.Row.Cells[4].Visible = false;
				}
			
			}
		}

	
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

		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		BindData();
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
		BindData();
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
