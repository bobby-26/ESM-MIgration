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
public partial class CrewCourseAddToParticipant : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Nomination to Participant", "NOMINATIONTOPARTICIPANT");
			toolbar.AddButton("Participant to Participant", "PARTICIPANTTOPARTICIPANT");
			CrewMenu.AccessRights = this.ViewState;
			CrewMenu.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				if (Request.QueryString["courseid"] != null)
				{
					EditCourseDetails();
				}
				if (Request.QueryString["batchid"] != null)
				{
					EditBatchDetails();
				}
				//txtFromDate.Text = DateTime.Today.ToString();
				WeekSetting(null, null);
				CrewMenu.SelectedMenuIndex = 0;
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

	protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("PARTICIPANTTOPARTICIPANT"))
			{
				Response.Redirect("..\\Crew\\CrewCourseMoveParticipantList.aspx?currentbatchid="+Request.QueryString["batchid"]+"&fromdate="+txtFromDate.Text, false);
				return;
				
			}
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

			int courseid = Convert.ToInt32(Request.QueryString["courseid"].ToString());
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
	protected void EditBatchDetails()
	{
		try
		{

			int batchid = Convert.ToInt32(Request.QueryString["batchid"].ToString());
			DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
				
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
	protected void WeekSetting(object sender, EventArgs e)
	{
		try
		{

			int yn = 0;
			
			DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
			DateTime dtto = Convert.ToDateTime(txtToDate.Text);
			if (dtfrom > dtto)
			{

				yn = 1;
			}

			SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), yn,  0);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void SetPrimaryDetails(int? type, int? dateadd, DateTime date, int? yn, int? excludesunday)
	{
		DataSet ds = PhoenixCrewCoursePlanner.SearchFirstLastWeekDay(type, dateadd, date, excludesunday);
		if (ds.Tables[0].Rows.Count > 0)
		{
			if (yn != 0)
			{
				txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFIRSTDATEOFWEEK"].ToString();
				ViewState["fromdate"] = txtFromDate.Text;
			}
			txtToDate.Text = ds.Tables[0].Rows[0]["FLDLASTDATEOFWEEK"].ToString();
			//Session["FROMDATE"] = txtFromDate.Text;
			//Session["TODATE"] = txtToDate.Text;
		}
	}
	protected void ExcludeSunday(object sender, EventArgs e)
	{
		SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), null, 0);
		//BindCourse();
		//if (Filter.CurrentCourseSelection != null)
		//{
		//    ddlCourse.SelectedValue = Filter.CurrentCourseSelection;
		//}
		BindData();
		SetPageNavigator();
	}
	protected void PrevClick(object sender, EventArgs e)
	{
		try
		{

			SetPrimaryDetails(1,  8, Convert.ToDateTime(txtFromDate.Text), null, 0);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void NextClick(object sender, EventArgs e)
	{
		SetPrimaryDetails(null, 8, Convert.ToDateTime(txtFromDate.Text), null,0);
		BindData();
		SetPageNavigator();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDNAME", "FLDRANKNAME", "FLDPRIORITY", "FLDAPPROVEDDATE", "FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Approved Date", "Batch No" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentNominationSearch(null,
			General.GetNullableInteger(Request.QueryString["batchid"].ToString()),
			General.GetNullableDateTime(txtFromDate.Text),
			1,
			sortexpression, sortdirection,
			(int)ViewState["PAGENUMBER"],
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount,
			General.GetNullableString(txtName.Text),
			General.GetNullableInteger(ddlRank.SelectedRank),
			General.GetNullableString(txtFileNo.Text));


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
			string Script = "";
			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "fnReloadList('codehelp1','ifMoreInfo','');";
			Script += "</script>" + "\n";

			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			
			if (e.CommandName.ToUpper().Equals("PARTICIPANTLIST"))
			{
				string stbemployeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
				string enrollmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text;
				PhoenixCrewCourseEnrollment.UpdateEnrollmenList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						stbemployeeid.ToString(),
						General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL")), 1,
						Convert.ToInt32(Request.QueryString["batchid"]),
						General.GetNullableGuid(enrollmentid)
						);

			}
			BindData();
			SetPageNavigator();
			Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
				Label l = (Label)e.Row.FindControl("lblEnrollmentId");

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
