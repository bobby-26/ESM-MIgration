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
using Telerik.Web.UI;
public partial class CrewCoursePlannerBatch : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbarBatch = new PhoenixToolbar();
		toolbarBatch.AddButton("New", "NEW");
		toolbarBatch.AddButton("Save", "SAVE");
		MenuBatch.AccessRights = this.ViewState;
		MenuBatch.MenuList = toolbarBatch.Show();

		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddImageButton("../Crew/CrewCourePlannerBatch.aspx?courseid="+Request.QueryString["courseid"], "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvBatchList')", "Print Grid", "icon_print.png", "PRINT");

		MenuCrewBatchList.AccessRights = this.ViewState;
		MenuCrewBatchList.MenuList = toolbar.Show();
		cmdHiddenSubmit.Attributes.Add("style", "display:none;");
		MenuCrewBatchList.SetTrigger(pnlBatchPlanner);

        PhoenixToolbar toolbarmenu = new PhoenixToolbar();
        toolbarmenu.AddButton("Batch", "BATCH");
        toolbarmenu.AddButton("Faculty", "FACULTY");
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarmenu.Show();
        MenuHeader.SelectedMenuIndex = 0;
        if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["batchid"] = "";
			
		
			if (Request.QueryString["courseid"] != null)
			{
				EditCourseDetails();
				ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,152,"OPN");
				chkPublishYN.Checked = true;
				ucInstitution.Course = Request.QueryString["courseid"];
			}
			if (Request.QueryString["batchid"] != null)
			{
				ViewState["batchid"] = Request.QueryString["batchid"];
			}
			BindData();
			SetPageNavigator();
		
		}
		if (ViewState["batchid"].ToString() == "")
		{
			MenuHeader.SelectedMenuIndex = 0;
			BindData();
			SetPageNavigator();
		}
		
	}
	protected void EditCourseDetails()
	{
		try
		{

			int courseid = Convert.ToInt32(Request.QueryString["courseid"]);
			ucBatch.CourseId =Convert.ToString( courseid);
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
				ddlCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				txtCourseDuration.Text = ds.Tables[0].Rows[0]["FLDDURATION"].ToString();
				txtFromDate.Text=Request.QueryString["fromdate"];
				
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
		
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("FACULTY"))
		{
			if (ViewState["batchid"] == null || ViewState["batchid"].ToString() == "")
				{
					ShowError();
					return;
				}
			string A = Request.QueryString["includeholiday"];
			Response.Redirect("../Crew/CrewCoursePlannerFaculty.aspx?batchid=" + ViewState["batchid"].ToString() + "&includeholiday=" + Request.QueryString["includeholiday"] + "&excludesunday=" + Request.QueryString["excludesunday"], false);
		}
		

	}
	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Please select a batch and then click faculty.";
		ucError.Visible = true;
	
	}
	protected void SetBatchDetails(int batchid)
	{
		try
		{

			DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);

			    if (ds.Tables.Count > 0)
			    {
			        DataRow dr = ds.Tables[0].Rows[0];
			        ucInstitution.SelectedInstitution = dr["FLDINSTITUTIONID"].ToString();
			        ddlCourse.SelectedCourse = dr["FLDCOURSEID"].ToString();
			        txtBatch.Text = dr["FLDBATCH"].ToString();
			        txtFromDate.Text = dr["FLDFROMDATE"].ToString();
			        txtToDate.Text = dr["FLDTODATE"].ToString();
			        txtNoOfSeats.Text = dr["FLDNOOFSEATS"].ToString();
			        txtDuration.Text = dr["FLDDURATION"].ToString();
			        chkClosedYN.Checked = dr["FLDCLOSEDYN"].ToString() == "1" ? true : false;
					ViewState["batchid"] = dr["FLDBATCHID"].ToString();
			        ucStatus.SelectedHard = dr["FLDSTATUS"].ToString();
			        chkPublishYN.Checked = dr["FLDPUBLISHEDYN"].ToString() == "1" ? true : false;
			        if (dr["FLDCLOSEDYN"].ToString() == "1")
			        {
			            DisableAll();
			        }
			        if (dr["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			            152, "OPN"))
			        {
			            chkClosedYN.Enabled = true;
			        }

			 
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CrewBatch_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		try
		{

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidBatch())
				{
					ucError.Visible = true;
					return;
				}
				int status=0;
				PhoenixRegistersBatch.CourseBatchValidate(Convert.ToInt32(ucCourse.SelectedCourse), ref status);
				if (status == 1)
				{
					ucConfirmExtraApproval.Visible = true;
				}
				else
				{
					Save(null,null);
				}
				
			}
			else if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				Reset();
			}
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void Save(int? approvalpermonth,string remarks)
	{
		try
		{
			if (ViewState["batchid"].ToString() == "")
			{
				PhoenixRegistersBatch.InsertBatch(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Convert.ToInt64(ucInstitution.SelectedInstitution)
					, Convert.ToInt32(ucCourse.SelectedCourse)
					, Convert.ToDateTime(txtFromDate.Text)
					, Convert.ToDateTime(txtToDate.Text)
					, General.GetNullableInteger(txtNoOfSeats.Text)
					, General.GetNullableString(txtDuration.Text)
					, General.GetNullableInteger(chkClosedYN.Checked == true ? "1" : "0")
					, General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					152, "OPN")), 1, General.GetNullableInteger(Request.QueryString["excludesunday"]), General.GetNullableInteger(Request.QueryString["includeholiday"])
					, approvalpermonth, General.GetNullableString(remarks)
					,0,txtBatch.Text
                    ,null);
				Reset();

			}
			else
			{
				PhoenixRegistersBatch.UpdateBatch(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode
				, Convert.ToInt32(ViewState["batchid"])
				, Convert.ToInt64(ucInstitution.SelectedInstitution)
				, Convert.ToInt32(ucCourse.SelectedCourse)
				, Convert.ToDateTime(txtFromDate.Text)
				, Convert.ToDateTime(txtToDate.Text)
				, General.GetNullableInteger(txtNoOfSeats.Text)
				, General.GetNullableString(txtDuration.Text)
				, General.GetNullableInteger(chkClosedYN.Checked == true ? "1" : "0")
				, General.GetNullableInteger(ucStatus.SelectedHard)
				, General.GetNullableInteger(chkPublishYN.Checked == true ? "1" : "0"), 1
				, General.GetNullableInteger(Request.QueryString["excludesunday"])
				, General.GetNullableInteger(Request.QueryString["includeholiday"])
				, 0, txtBatch.Text,null);
			}
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void btnCrewApprove_Click(object sender, EventArgs e)
	{
		try
		{
			UserControlCourseExtraApproval ucCM = sender as UserControlCourseExtraApproval;
			if (ucCM.confirmboxvalue == 1)
			{

				TextBox txtrem = (TextBox)ucConfirmExtraApproval.FindControl("txtRemarks");
				UserControlMaskNumber txtapp = (UserControlMaskNumber)ucConfirmExtraApproval.FindControl("txtNoOfApprovals");
		
				if (!IsValidate(txtrem.Text, txtapp.Text))
				{
					ucError.Visible = true;
					return;
				}
				Save(Convert.ToInt32(txtapp.Text), txtrem.Text);

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidate(string remarks,string app)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(remarks))
			ucError.ErrorMessage = "Remarks is required for proceeding.";

		if (string.IsNullOrEmpty(app))
			ucError.ErrorMessage = "Extra Approval per month is required for proceeding.";
		if ((!string.IsNullOrEmpty(app)) && (Convert.ToInt32(app)<=0))
			ucError.ErrorMessage = "Approval shud be greater than 0";

		return (!ucError.IsError);
	}
	private void Reset()
	{
		ucInstitution.SelectedInstitution = "";
		txtBatch.Text = "";
		txtFromDate.Text = "";
		txtToDate.Text = "";
		txtNoOfSeats.Text = "";
		txtDuration.Text = "";
		chkClosedYN.Checked = false;
		ViewState["batchid"] = "";
		gvBatchList.SelectedIndex = -1;
		
	}
	private bool IsValidBatch()
	{
		Int32 resultInt;
		DateTime resultDate;

		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(txtBatch.Text))
			ucError.ErrorMessage = "Batch is required.";

		if (!Int32.TryParse(ucInstitution.SelectedInstitution, out resultInt))
			ucError.ErrorMessage = "Institution is required";

		if (!Int32.TryParse(ucCourse.SelectedCourse, out resultInt))
			ucError.ErrorMessage = "Course is required";

		if (string.IsNullOrEmpty(txtFromDate.Text))
			ucError.ErrorMessage = "From Date is required.";
	
		if (string.IsNullOrEmpty(txtToDate.Text))
			ucError.ErrorMessage = "To Date is required.";
		
		if (txtFromDate.Text != null && txtToDate.Text != null)
		{
			if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
				if ((DateTime.Parse(txtToDate.Text)) < (DateTime.Parse(txtFromDate.Text)))
					ucError.ErrorMessage = "'To Date' should not be less than 'From Date'";
		}
		if (txtNoOfSeats.Text != "")
		{
			if (!Int32.TryParse(txtNoOfSeats.Text, out resultInt))
				ucError.ErrorMessage = "Enter a valid number";
		}
		return (!ucError.IsError);
	}
	protected void DisableAll()
	{
		ucCourse.Enabled = false;
		ucInstitution.Enabled = false;
		txtFromDate.Enabled = false;
		txtToDate.Enabled = false;
		txtBatch.Enabled = false;

	}

	protected void CalculateDuration(object sender, EventArgs e)
	{

		UserControlDate d = sender as UserControlDate;
		if (d != null)
		{
			if (txtFromDate.Text != null && txtToDate.Text != null)
			{
				DateTime fd = Convert.ToDateTime(txtFromDate.Text);
				DateTime sd = Convert.ToDateTime(txtToDate.Text);
				TimeSpan s = sd - fd;

				txtDuration.Text = Convert.ToString(s.Days + 1);

			}
		}


	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDBATCHNO", "FLDFROMDATE", "FLDTODATE", "FLDBATCHLOCATION", "FLDSTATUS", "FLDPUBLISHEDYN" };
			string[] alCaptions = { "Batch No", "From Date", "To Date", "Batch Location", "Status", "Published Y/N" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


			DataSet ds = PhoenixRegistersBatch.BatchSearch(null,
					General.GetNullableInteger(Request.QueryString["courseid"]), null, sortexpression, sortdirection,
					(int)ViewState["PAGENUMBER"],
					General.ShowRecords(null),
					ref iRowCount,
					ref iTotalPageCount, null, null, null,1,null);

			General.SetPrintOptions("gvBatchList", "Batch List", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvBatchList.DataSource = ds;
				gvBatchList.DataBind();
				if (Request.QueryString["batchid"] == "")
				{

					ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
					gvBatchList.SelectedIndex = 0;
					
				}
				
				if (ViewState["batchid"].ToString() != "")
				{
					SetRowSelection();
					SetBatchDetails(Convert.ToInt32(ViewState["batchid"].ToString()));
				}
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvBatchList);
				ViewState["batchid"] = "";
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
	private void SetRowSelection()
	{
		gvBatchList.SelectedIndex = -1;
		for (int i = 0; i < gvBatchList.Rows.Count; i++)
		{
			if (gvBatchList.DataKeys[i].Value.ToString().Equals(Request.QueryString["batchid"]))
			{
				gvBatchList.SelectedIndex = i;
				ViewState["DTKEY"] = ((Label)gvBatchList.Rows[gvBatchList.SelectedIndex].FindControl("lbldtkey")).Text;

			}
		}
	}
	protected void CrewBatchList_TabStripCommand(object sender, EventArgs e)
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

	protected void gvBatchList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixRegistersBatch.DeleteBatch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text));
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


	protected void gvBatchList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;

		string batchid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text;
		ViewState["batchid"] = batchid;
		SetBatchDetails(Convert.ToInt32(ViewState["batchid"]));
	}

	protected void gvBatchList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
				Label l = (Label)e.Row.FindControl("lblBatchId");

				
			}
		}


	}

	

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		BindData();
		
	}
	protected void gvBatchList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvBatchList.SelectedIndex = -1;
		gvBatchList.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
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
		gvBatchList.SelectedIndex = -1;
		gvBatchList.EditIndex = -1;
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
