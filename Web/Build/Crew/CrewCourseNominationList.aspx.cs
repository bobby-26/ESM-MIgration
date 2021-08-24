using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewCourseNominationList : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseNominationList.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvNominationList')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','CrewCourseEnrollmentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
			toolbar.AddImageButton("../Crew/CrewCourseNominationList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
			toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseEmployeeList.aspx?typelist=1&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "INL") + "')", "Add Seafarer to Nomination List", "add.png", "ADDNOMINATION");
			MenuCrewNominationList.AccessRights = this.ViewState;
			MenuCrewNominationList.MenuList = toolbar.Show();
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			MenuCrewNominationList.SetTrigger(pnlNominationList);
			PhoenixToolbar toolbarMenu = new PhoenixToolbar();
			toolbarMenu.AddButton("Assign Batch", "ASSIGNBATCH");
			toolbarMenu.AddButton("Add to Participant", "ADDPARTICIPANT");
			CourseEnrollmentMenu.AccessRights = this.ViewState;
			CourseEnrollmentMenu.MenuList = toolbarMenu.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				if (Session["COURSEID"] != null)
				{
					EditCourseDetails();
					ucBatch.CourseId = Session["COURSEID"].ToString();
					Filter.CurrentCourseSelection = Session["COURSEID"].ToString();
				}
				else
				{
					ucBatch.CourseId = Filter.CurrentCourseSelection;
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
                txtMinStrength.Text = ds.Tables[0].Rows[0]["FLDMINSTRENGTH"].ToString();
                txtMaxStrength.Text = ds.Tables[0].Rows[0]["FLDMAXSTRENGTH"].ToString();
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDNAME", "FLDRANKNAME", "FLDPRIORITY","FLDNOMINATEDBY", "FLDAPPROVEDDATE", "FLDFROMDATE","FLDTODATE","FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Nominated by", "Created Date","Avl. From Date" ," Avl. To Date","Batch No" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		NameValueCollection nvc = Filter.CurrentCourseRequestFilter;
		ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(General.GetNullableInteger(Session["COURSEID"].ToString()),
			   null, null,
			   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,153,"INL")),
			   1
			   , sortexpression, sortdirection,
			   (int)ViewState["PAGENUMBER"],
			   iRowCount,
			   ref iRowCount,
			   ref iTotalPageCount,
			   General.GetNullableString(nvc != null ? nvc["txtFileno"] : string.Empty),
			   General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty),
			   General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
			   General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty),
			   General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty));

		Response.AddHeader("Content-Disposition", "attachment; filename=NominationList.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Interst List</h3></td>");
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

	protected void CrewNominationList_TabStripCommand(object sender, EventArgs e)
	{
		try
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
			if (dce.CommandName.ToUpper().Equals("CLEAR"))
			{
				Filter.CurrentCourseEnrollmentFilter = null;
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
	protected void CourseEnrollmentMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("ASSIGNBATCH"))
			{
				string csvEmployeeid = string.Empty;
				foreach (GridViewRow gv in gvNominationList.Rows)
				{
					CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
					if (ck.Checked)
					{
						csvEmployeeid += ((Label)gv.FindControl("lblEmployeeId")).Text + ",";
					
					}
				}

				if (!IsValidBatch(csvEmployeeid.ToString()))
				{
					ucError.Visible = true;
					return;
				}

				PhoenixCrewCourseEnrollment.UpdateEnrollmenNomination(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							csvEmployeeid.ToString(),
							General.GetNullableInteger(ucBatch.SelectedBatch), 1);

				ucStatus.Text = "Batch updated";
			}
			if (dce.CommandName.ToUpper().Equals("ADDPARTICIPANT"))
			{
				string csvEmployeeid = string.Empty;
				foreach (GridViewRow gv in gvNominationList.Rows)
				{
					CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
					if (ck.Checked)
					{
						csvEmployeeid += ((Label)gv.FindControl("lblEmployeeId")).Text + ",";

					}
				}

				PhoenixCrewCourseEnrollment.UpdateEnrollmenNomination(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							csvEmployeeid.ToString(),
							General.GetNullableInteger(ucBatch.SelectedBatch), null);

				ucStatus.Text = "Participant List updated";
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
		try
		{
			BindData();
			SetPageNavigator();
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

		string[] alColumns = { "FLDNAME", "FLDRANKNAME", "FLDPRIORITY", "FLDNOMINATEDBY", "FLDAPPROVEDDATE", "FLDFROMDATE", "FLDTODATE", "FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Nominated by", "Created Date", "Avl. From Date", " Avl. To Date", "Batch No" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		NameValueCollection nvc = Filter.CurrentCourseEnrollmentFilter;
		DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(General.GetNullableInteger(Session["COURSEID"].ToString()),
		   null, null,
		   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "INL")),
		   1
		   , sortexpression, sortdirection,
		   (int)ViewState["PAGENUMBER"],
		  General.ShowRecords(null),
		   ref iRowCount,
		   ref iTotalPageCount,
		   General.GetNullableString(nvc != null ? nvc["txtFileno"] : string.Empty),
		   General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty),
		   General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
		   General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty), 
		   General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty));


		General.SetPrintOptions("gvNominationList", "Nomination List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvNominationList.DataSource = ds;
			gvNominationList.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvNominationList);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}

	private bool IsValidBatch(string empid)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		if (General.GetNullableInteger(ucBatch.SelectedBatch)== null)
		{
				ucError.ErrorMessage = "Batch is required.";
		}
		if (General.GetNullableString(empid) == null)
		{
			ucError.ErrorMessage = "Select atleast 1 seafarer.";
		}
		return (!ucError.IsError);
	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}

	protected void gvNominationList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixCrewCourseEnrollment.DeleteCrewCourseEnrollment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text),null);
			}
			if (e.CommandName.ToUpper().Equals("PARTICIPANTLIST"))
			{
				string stbemployeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
				string strBatchid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchid")).Text;
				string enrollmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text;
				PhoenixCrewCourseEnrollment.UpdateEnrollmenList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						stbemployeeid.ToString(),
						General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL")), 1,
						General.GetNullableInteger(strBatchid),
						General.GetNullableGuid(enrollmentid));

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

	protected void gvNominationList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
	}

    protected void gvNominationList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

	protected void gvNominationList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				DataRowView drv = (DataRowView)e.Row.DataItem;

				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				Label l = (Label)e.Row.FindControl("lblEnrollmentId");
				if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

				ImageButton dbwl = (ImageButton)e.Row.FindControl("cmdNL");
				if (dbwl != null) dbwl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Participant List?'); return false;");


				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				db1.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewCourseEnrollmentDetails.aspx?enrollmentid=" + l.Text + "'); return false;");

				if (drv["FLDBATCHID"].ToString() == "")
				{
					dbwl.Visible = false;
				}

			}
		}


	}

	protected void gvNominationList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvNominationList.SelectedIndex = -1;
		gvNominationList.EditIndex = -1;

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
		gvNominationList.SelectedIndex = -1;
		gvNominationList.EditIndex = -1;
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
