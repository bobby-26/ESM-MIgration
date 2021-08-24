using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersFaculty : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Registers/RegistersFaculty.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvFaculty')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageButton("../Registers/RegistersFaculty.aspx", "Find", "search.png", "FIND");
			toolbar.AddImageLink("javascript:Openpopup('codehelp1','','RegistersFacultyList.aspx')", "Add", "add.png", "ADDFACULTY");
            MenuFaculty.AccessRights = this.ViewState;
			MenuFaculty.MenuList = toolbar.Show();
            

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				
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

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		ViewState["PAGENUMBER"] = 1;
		gvFaculty.EditIndex = -1;
		gvFaculty.SelectedIndex = -1;
		BindData();
		SetPageNavigator();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds;
		string[] alColumns = { "FLDFACULTYCODE", "FLDFACULTYNAME", "FLDCOURSENAME", "FLDACTIVEYN" };
		string[] alCaptions = { "Code", "Name", "Course Name", "Active YN" }; ;
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		ds = PhoenixRegistersFaculty.FacultySearch(
			General.GetNullableString(txtFacultyCode.Text),
			General.GetNullableString(txtFacultyName.Text),
			null,
			null,
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);
		
		General.SetPrintOptions("gvFaculty", "Faculty", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvFaculty.DataSource = ds;
			gvFaculty.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvFaculty);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


	}
	protected void gvFaculty_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void gvFaculty_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		if (e.CommandName.ToUpper().Equals("DELETE"))
		{
			PhoenixRegistersFaculty.DeleteFaculty(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyid")).Text));
		}

	}

	protected void gvFaculty_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}


	protected void gvFaculty_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkFacultyName");
				Label lblcourse = (Label)e.Row.FindControl("lblCourses");
				lbtn.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersFacultyList.aspx?facultyid=" + lbtn.CommandArgument + "'); return false;");

				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete()");             

				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				db1.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersFacultyList.aspx?facultyid=" + lbtn.CommandArgument + "'); return false;");
                

				UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipFaculty");
				lblcourse.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
				lblcourse.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
			}

		}
		
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDFACULTYCODE", "FLDFACULTYNAME", "FLDCOURSENAME", "FLDACTIVEYN" };
		string[] alCaptions = { "Code", "Name", "Course Name", "Active YN" }; ;
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersFaculty.FacultySearch(
			General.GetNullableString(txtFacultyCode.Text),
			General.GetNullableString(txtFacultyName.Text),
			null,
			null,
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=Faculty.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Faculty List</h3></td>");
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
		gvFaculty.SelectedIndex = -1;
		gvFaculty.EditIndex = -1;
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
	}


	protected void MenuFaculty_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if (dce.CommandName.ToUpper().Equals("FIND"))
			{
				BindData();
			}
			
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
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
}
