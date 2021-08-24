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
using Telerik.Web.UI;

public partial class CrewCourseConfirmedList : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseConfirmedList.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvConfirmedList')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseEmployeeList.aspx?typelist=2&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML") + "')", "Add Seafarer to Confirmed List", "add.png", "ADDCONFIRMED");
			toolbar.AddImageButton("../Crew/CrewCourseConfirmedList.aspx", "Move Seafarer to Participant List", "45.png", "PARTICIPANTLIST");
			toolbar.AddImageButton("../Crew/CrewCourseConfirmedList.aspx", "Move Seafarer to Wait List", "yellow-symbol.png", "WAITLIST");
			MenuCrewConfirmedList.AccessRights = this.ViewState;
			MenuCrewConfirmedList.MenuList = toolbar.Show();
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			MenuCrewConfirmedList.SetTrigger(pnlConfirmedList);

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
				ucConfirmParticipantlist.Visible = false;
				ucConfirmWaitlist.Visible = false;
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
		string[] alColumns = { "FLDNAME", "FLDRANK", "FLDPRIORITY", "FLDAPPROVEDDATE", "FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Approved Date", "Batch No" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(General.GetNullableInteger(Session["COURSEID"].ToString()),
			   null, null,
			   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML")),
			   1
			   , sortexpression, sortdirection,
			   (int)ViewState["PAGENUMBER"],
			   iRowCount,
			   ref iRowCount,
			   ref iTotalPageCount, null, null, null, null, null);

		Response.AddHeader("Content-Disposition", "attachment; filename=ConfirmedList.xls");
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

	protected void CrewConfirmedList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			StringBuilder stbemployeeid = new StringBuilder();

			if (dce.CommandName.ToUpper().Equals("PARTICIPANTLIST") || dce.CommandName.ToUpper().Equals("WAITLIST"))
			{
				if (Convert.ToInt32(ViewState["ROWCOUNT"]) > 0)
				{
					foreach (GridViewRow gvr in gvConfirmedList.Rows)
					{
						if (((CheckBox)gvr.FindControl("chkEmployee")).Checked == true)
						{

							string empid = ((Label)gvr.FindControl("lblEmployeeId")).Text;
							stbemployeeid.Append(empid);
							stbemployeeid.Append(",");

						}
					}
				}
			}
			if (stbemployeeid.Length > 1)
			{
				stbemployeeid.Remove(stbemployeeid.Length - 1, 1);
			}
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
			if (dce.CommandName.ToUpper().Equals("PARTICIPANTLIST"))
			{
				if (stbemployeeid.ToString() != "")
				{
					ucConfirmParticipantlist.Visible = true;
					ucConfirmParticipantlist.Text = "Are you sure you want to move the selected seafarer to Participant List?";
				}

			}
			else if (dce.CommandName.ToUpper().Equals("WAITLIST"))
			{
				if (stbemployeeid.ToString() != "")
				{
					ucConfirmParticipantlist.Visible = true;
					ucConfirmParticipantlist.Text = "Are you sure you want to move the selected seafarer to Wait List?";
				}
				
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void btnWaitList_Click(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

			if (ucCM.confirmboxvalue == 1)
			{
				StringBuilder stbemployeeid = new StringBuilder();
				foreach (GridViewRow gvr in gvConfirmedList.Rows)
				{
					if (((CheckBox)gvr.FindControl("chkEmployee")).Checked == true)
					{

						string empid = ((Label)gvr.FindControl("lblEmployeeId")).Text;
						stbemployeeid.Append(empid);
						stbemployeeid.Append(",");

					}
				}
				if (stbemployeeid.Length > 1)
				{
					stbemployeeid.Remove(stbemployeeid.Length - 1, 1);
				}
				if (stbemployeeid.ToString() != "")
				{
					//PhoenixCrewCourseEnrollment.UpdateEnrollmenList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					//stbemployeeid.ToString(),
					//General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					//153, "WTL")), 1, Convert.ToInt32((Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection)));
				}
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
	protected void btnParticipantList_Click(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

			if (ucCM.confirmboxvalue == 1)
			{
				StringBuilder stbemployeeid = new StringBuilder();
				foreach (GridViewRow gvr in gvConfirmedList.Rows)
				{
					if (((CheckBox)gvr.FindControl("chkEmployee")).Checked == true)
					{

						string empid = ((Label)gvr.FindControl("lblEmployeeId")).Text;
						stbemployeeid.Append(empid);
						stbemployeeid.Append(",");

					}
				}
				if (stbemployeeid.Length > 1)
				{
					stbemployeeid.Remove(stbemployeeid.Length - 1, 1);
				}
				if (stbemployeeid.ToString() != "")
				{
					//PhoenixCrewCourseEnrollment.UpdateEnrollmenList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					//stbemployeeid.ToString(),
					//General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					//153, "PTL")), 1,Convert.ToInt32(Session["COURSEID"].ToString()));
				}
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

		string[] alColumns = { "FLDNAME", "FLDRANK", "FLDPRIORITY", "FLDAPPROVEDDATE", "FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Approved Date", "Batch No" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(General.GetNullableInteger(Session["COURSEID"].ToString()),
		   null,null,
		   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML")),
		   1
		   , sortexpression, sortdirection,
		   (int)ViewState["PAGENUMBER"],
		  General.ShowRecords(null),
		   ref iRowCount,
		   ref iTotalPageCount, null, null, null, null, null);


		General.SetPrintOptions("gvConfirmedList", "Confirmed List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvConfirmedList.DataSource = ds;
			gvConfirmedList.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvConfirmedList);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}

	protected void gvConfirmedList_RowCommand(object sender, GridViewCommandEventArgs e)
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

			BindData();
			SetPageNavigator();
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	
	protected void gvConfirmedList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
	}

	protected void gvConfirmedList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				Label l = (Label)e.Row.FindControl("lblEnrollmentId");

				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				db1.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewCourseEnrollmentDetails.aspx?enrollmentid=" + l.Text + "'); return false;");
			}
		}


	}

	protected void gvConfirmedList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvConfirmedList.SelectedIndex = -1;
		gvConfirmedList.EditIndex = -1;

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
		gvConfirmedList.SelectedIndex = -1;
		gvConfirmedList.EditIndex = -1;
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
