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
public partial class CrewCourseParticipantList : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseParticipantList.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvParticipantList')", "Print Grid", "icon_print.png", "PRINT");
			
            if (ViewState["BATCHID"] != null && !string.IsNullOrEmpty(ViewState["BATCHID"].ToString()))
            {
                toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=COURSEREGISTRATIONFORM&batchid=" + ViewState["BATCHID"].ToString() + "&showmenu=1" + "'); return false;", "Registration Form", "application-form.png", "REGISTRATION");
            }
			//toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseAddToParticipant.aspx?typelist=4&courseid="+Filter.CurrentCourseSelection + "&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "INL") + "')", "Move from Nomination List to Participant List", "tab-select.png", "ADDTOPARTICIPANT");
			MenuCrewParticipantList.AccessRights = this.ViewState;
			MenuCrewParticipantList.MenuList = toolbar.Show();
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			MenuCrewParticipantList.SetTrigger(pnlParticipantList);
            if (Session["COURSEID"] != null)
            {
                ucBatch.CourseId = Session["COURSEID"].ToString();
				Filter.CurrentCourseSelection = Session["COURSEID"].ToString();
            }
            else
            {
                ucBatch.CourseId = Filter.CurrentCourseSelection;
            }
		
			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
                ViewState["BATCHID"] = null;
				if (Session["COURSEID"] != null || Filter.CurrentCourseSelection!=null)
				{
					EditCourseDetails();
				}
				ucConfirm.Visible = false;
			
			}
			if (Request.QueryString["batchid"] != null )
			{
				ucBatch.SelectedBatch = Request.QueryString["batchid"];
				ucBatch.Enabled = false;
				lblRegForm.Visible = true;
				chkRF.Visible = true;
				lblRegFormCom.Visible = true;
				txtRegEditedby.Visible = true;
				txtRegDate.Visible = true;
				lblnote.Visible = true;
			}
			SetDetails(null, null);
			BindData();
			SetPageNavigator();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void SetDetails(object sender,EventArgs e)
	{
		BindData();
		SetBatchDetails();
		
	}
	protected void SetBatchDetails()
	{
		try
		{

			int courseid = Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection);
			if (Request.QueryString["batchid"] == null)
			{
				int batchid = Convert.ToInt32( General.GetNullableInteger( ucBatch.SelectedBatch) != null ? Convert.ToInt32(ucBatch.SelectedBatch) : 0);
				if (batchid == 0)
					ViewState["BATCHID"] = null;
				else
					ViewState["BATCHID"] = batchid.ToString();

			}
			else
			{
				ViewState["BATCHID"] = Request.QueryString["batchid"];
			}
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewCourseParticipantList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvParticipantList')", "Print Grid", "icon_print.png", "PRINT");
			
            if (ViewState["BATCHID"] != null && !string.IsNullOrEmpty(ViewState["BATCHID"].ToString()))
            {
				toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseEmployeeList.aspx?typelist=4&batchid=" + ViewState["BATCHID"].ToString() + "&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL") + "')", "Add Seafarer to Participant List", "add.png", "ADDPARTICIPANT");
				toolbar.AddImageButton("../Crew/CrewCourseParticipantList.aspx?calledfrom=enrollment", "Fill Participant List from Wait List", "green.png", "FILLLIST");
				toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseAddToParticipant.aspx?typelist=4&batchid=" + ViewState["BATCHID"] + "&courseid=" + Filter.CurrentCourseSelection + "&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "INL") + "')", "Move from Nomination List/ Participant List", "tab-select.png", "ADDTOPARTICIPANT");
			}
			if (ViewState["BATCHID"] != null && !string.IsNullOrEmpty(ViewState["BATCHID"].ToString()) && Request.QueryString["calledfrom"]=="batchmaster")
			{
				toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=COURSEREGISTRATIONFORM&batchid=" + ViewState["BATCHID"].ToString() + "&showmenu=1" + "'); return false;", "Registration Form", "application-form.png", "REGISTRATION");

			}
			
            MenuCrewParticipantList.AccessRights = this.ViewState;
            MenuCrewParticipantList.MenuList = toolbar.Show();

			DataSet ds = PhoenixCrewCourseEnrollment.ListCrewCourseEnrollment(Convert.ToInt32(ViewState["BATCHID"]));
			if (ds.Tables[0].Rows.Count > 0)
			{

				txtMinStrength.Text = ds.Tables[0].Rows[0]["FLDMINPARTICIPANT"].ToString();
				txtMaxStrength.Text = ds.Tables[0].Rows[0]["FLDMAXPARTICIPANT"].ToString();
				txtNominationList.Text = ds.Tables[0].Rows[0]["FLDCOUNTNL"].ToString();
				txtWaitList.Text = ds.Tables[0].Rows[0]["FLDCOUNTWL"].ToString();
				txtParticipantList.Text = ds.Tables[0].Rows[0]["FLDCOUNTPL"].ToString();
				txtFreeList.Text = ds.Tables[0].Rows[0]["FLDCOUNTFREE"].ToString();
				txtLastEditedby.Text = ds.Tables[0].Rows[0]["FLDMODIFIEDBY"].ToString();
				txtLastEditedDate.Text = ds.Tables[0].Rows[0]["FLDMODIFIEDDATE"].ToString();
			}
			else
			{

				txtNominationList.Text = "";
				txtWaitList.Text = "";
				txtParticipantList.Text = "";
				txtFreeList.Text = "";
				txtLastEditedby.Text = "";
				txtLastEditedDate.Text = "";
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void CheckRF(object sender,EventArgs e)
	{
		try
		{
			PhoenixCrewCourseEnrollment.UpdateCrewEnrollmentRF(Convert.ToInt32(ViewState["BATCHID"]), chkRF.Checked == true ? 1 : 0, DateTime.Today);
			EditCourseDetails();
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

			int courseid = Convert.ToInt32(Session["COURSEID"] != null ?(Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection);
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
                txtMinStrength.Text = ds.Tables[0].Rows[0]["FLDMINSTRENGTH"].ToString();
                txtMaxStrength.Text = ds.Tables[0].Rows[0]["FLDMAXSTRENGTH"].ToString();
				txtRegEditedby.Text = ds.Tables[0].Rows[0]["FLDRFCHECKEDBY"].ToString();
				txtRegDate.Text = ds.Tables[0].Rows[0]["FLDRFCHECKDATE"].ToString();
				chkRF.Checked = ds.Tables[0].Rows[0]["FLDRFCHECKYN"].ToString() == "1" ? true : false;

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
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDRANKNAME", "FLDPRIORITY", "FLDAPPROVEDDATE", "FLDBATCHNAME", };
        string[] alCaptions = { "Sr.No", "Employee Name", "Rank", "Priority", "Enrolled On", "Batch No" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection),
			   null, General.GetNullableInteger(ucBatch.SelectedBatch) == null ? 0 : General.GetNullableInteger(ucBatch.SelectedBatch),
			   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL")),
			   1
			   , sortexpression, sortdirection,
			   (int)ViewState["PAGENUMBER"],
			   iRowCount,
			   ref iRowCount,
			   ref iTotalPageCount, null, null, null, null, null);

		Response.AddHeader("Content-Disposition", "attachment; filename=ParticipantList.xls");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
				Response.Write("</td>");
			}
			Response.Write("</tr>");
		}
		Response.Write("</TABLE>");
		Response.End();
	}

	protected void CrewParticipantList_TabStripCommand(object sender, EventArgs e)
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
			if (dce.CommandName.ToUpper().Equals("FILLLIST"))
			{

				if (General.GetNullableInteger(ucBatch.SelectedBatch) != null)
				{
					if (!IsValidUpdate())
					{
						ucError.Visible = true;
						return;
					} 
					int courseid = Convert.ToInt32(Session["COURSEID"] != null ?(Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection);
					PhoenixCrewCourseEnrollment.UpdateCrewEnrollmentAuto(
						General.GetNullableInteger(txtFreeList.Text),
						General.GetNullableInteger(txtMaxStrength.Text),
						Convert.ToInt32(courseid),
						General.GetNullableInteger(ucBatch.SelectedBatch));
					ucStatus.Text = "Participant List updated";
					SetBatchDetails();
					BindData();
					SetPageNavigator();
				}

			}           
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	public bool IsValidUpdate()
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (txtMaxStrength.Text.Trim() == string.Empty)
			ucError.ErrorMessage = "Please configure the max strength of the course/batch.";

		if (txtFreeList.Text.Trim() == "0")
			ucError.ErrorMessage = "Participant List is full.";

		if (txtWaitList.Text.Trim() == string.Empty)
			ucError.ErrorMessage = "Wait List is empty.";


		return (!ucError.IsError);
	}

	protected void btn_Click(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

			if (ucCM.confirmboxvalue == 1)
			{

			
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
	//	string A = ViewState["BATCHID"];
		DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection),
		   null, ViewState["BATCHID"] == null ? 0 : General.GetNullableInteger( ViewState["BATCHID"].ToString()),
		   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL")),
		   1
		   , sortexpression, sortdirection,
		   (int)ViewState["PAGENUMBER"],
		  General.ShowRecords(null),
		   ref iRowCount,
		   ref iTotalPageCount, null, null, null, null, null);


		General.SetPrintOptions("gvParticipantList", "Participant List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvParticipantList.DataSource = ds;
			gvParticipantList.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvParticipantList);
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
	protected void gvParticipantList_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}
	protected void gvParticipantList_RowCommand(object sender, GridViewCommandEventArgs e)
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
							new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text), 2);
			}
			if (e.CommandName.ToUpper().Equals("WAITLIST"))
			{
				string stbemployeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
				string enrollmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text;
				PhoenixCrewCourseEnrollment.UpdateEnrollmenList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						stbemployeeid.ToString(),
						General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "WTL")), 1,
						Convert.ToInt32(ucBatch.SelectedBatch),
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

	protected void gvParticipantList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
	}

	protected void gvParticipantList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
				ImageButton dbwl = (ImageButton)e.Row.FindControl("cmdWL");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Nomination List?'); return false;");
				Label l = (Label)e.Row.FindControl("lblEnrollmentId");

				if (dbwl != null) dbwl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Wait List?'); return false;");


				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				db1.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewCourseEnrollmentDetails.aspx?typelist=4&enrollmentid=" + l.Text + "'); return false;");
				if (Request.QueryString["calledfrom"].ToUpper() ==  "BATCHMASTER")
				{
					db1.Visible = false;
					dbwl.Visible = false;
				}
			}
		}

	}

	protected void gvParticipantList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvParticipantList.SelectedIndex = -1;
		gvParticipantList.EditIndex = -1;

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
		gvParticipantList.SelectedIndex = -1;
		gvParticipantList.EditIndex = -1;
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
