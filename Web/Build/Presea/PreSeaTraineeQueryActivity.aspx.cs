using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.CrewManagement;

public partial class PreSeaTraineeQueryActivity : PhoenixBasePage 
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			PhoenixToolbar toolbarsub = new PhoenixToolbar();
			toolbarsub.AddImageButton("../PreSea/PreSeaTraineeQueryActivity.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarsub.AddImageLink("javascript:Openpopup('Filter','','PreSeaTraineeQueryActivityFilter.aspx'); return false;", "Filter", "search.png", "FIND");
			toolbarsub.AddImageButton("../PreSea/PreSeaTraineeQueryActivity.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
			PreSeaQueryMenu.AccessRights = this.ViewState;
			PreSeaQueryMenu.MenuList = toolbarsub.Show();

			if (!IsPostBack)
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
			gvPreSeaSearch.SelectedIndex = -1;
			gvPreSeaSearch.EditIndex = -1;
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
			gv.Rows[0].Attributes["onclick"] = "";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void PreSeaQueryMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			else if (dce.CommandName.ToUpper().Equals("SEARCH"))
			{
				BindData();
				SetPageNavigator();
			}
			else if (dce.CommandName.ToUpper().Equals("CLEAR"))
			{
				Filter.CurrentPreSeaNewApplicantFilterCriteria = null;
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
	protected void PreSeaQuery_TabStripCommand(object sender, EventArgs e)
	{
		//Filter.CurrentPreSeaTraineeSelection = null;
		//Session["REFRESHFLAG"] = null;
		//Response.Redirect("..\\PreSea\\PreSeaPersonalGeneral.aspx?t=n");
	}
	protected void gvPreSeaSearch_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void gvPreSeaSearch_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper() == "SORT") return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper() == "GETEMPLOYEE")
			{

				Filter.CurrentPreSeaTraineeSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
				//string familyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblfamlyid")).Text;
				Session["REFRESHFLAG"] = null;

				Response.Redirect("..\\PreSea\\PreSeaTraineePersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes");
			
			}
			else if (e.CommandName.ToUpper() == "SENDMAIL")
			{
				try
				{
					Filter.CurrentPreSeaTraineeSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
					PhoenixPreSeaCommon.TraineeDocsSendMail(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection.ToString()), null);
					
					ucStatus.Text = "Mail sent successfully";
				}
				catch (Exception ex)
				{
					ucError.ErrorMessage = ex.Message;
					ucError.Visible = true;
				}
			}
            else if (e.CommandName.ToUpper() == "MOVE")
            {
                string employeeId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
                PhoenixPreSeaTrainee.MovePreSeaTraineeToCrew(General.GetNullableInteger(employeeId).Value);
                ucStatus.Text = "Candidate moved successfully";
            }
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvPreSeaSearch_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Label traineeid = (Label)e.Row.FindControl("lblEmployeeid");
            Label batch= (Label)e.Row.FindControl("lblBatch");
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{							
				ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

			}
            ImageButton imgAdmission = (ImageButton)e.Row.FindControl("cmdAdmission");
            ImageButton imgAdministration = (ImageButton)e.Row.FindControl("cmdAdministration");
            ImageButton imgMove = (ImageButton)e.Row.FindControl("CmdMove");

            if (imgAdmission != null)
            {
                imgAdmission.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'PreSeaAdmissionHandOver.aspx?traineeid=" + traineeid.Text
                    + "&batchid=" + batch.Text + "');return false;");
            }
            if (imgAdministration != null)
            {
                imgAdministration.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'PreSeaAdministrationHandOver.aspx?traineeid=" + traineeid.Text
                    + "&batchid=" + batch.Text + "');return false;");
            }
            ImageButton imgRollNo = (ImageButton)e.Row.FindControl("imgRollno");
            if (imgRollNo != null)
            {
                Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");

                imgRollNo.Visible = SessionUtil.CanAccess(this.ViewState, imgRollNo.CommandName);
                imgRollNo.Attributes.Add("onclick", "Openpopup('RollNo', '', '../PreSea/PreSeaTraineeRollNoAssignment.aspx?candidateid=" + lblEmployeeid.Text  + "&batchid=" + batch.Text +"');return false;");
            }
            ImageButton imgInterview = (ImageButton)e.Row.FindControl("cmdInterview");
            if (imgInterview != null)
                imgInterview.Attributes.Add("onclick", "Openpopup('PDForm', '', '../PreSea/PreSeaEntranceExamInterview.aspx?type=0&candidateid=" + traineeid.Text + "');return false;");
            if (imgMove != null)
            {
                imgMove.Visible = SessionUtil.CanAccess(this.ViewState, imgMove.CommandName);
                imgMove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move the candidate to crew?')");
            }
		}
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		ViewState["PAGENUMBER"] = 1;
		BindData();
		SetPageNavigator();
	}
	public void BindData()
	{

		int iRowCount = 0;
		int iTotalPageCount = 0;

        string[] alColumns = { "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDTRBATCHROLLNUMBER", "FLDDATEOFBIRTH", "FLDCOURSENAME", "FLDBATCHNAME", "FLDSTATUS" };
        string[] alCaptions = { "First Name", "Middle Name", "Last Name", "Batch Roll No", "Date of Birth", "Course", "Batch", "Status" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		try
		{

			NameValueCollection nvc = Filter.CurrentPreSeaNewApplicantFilterCriteria;

			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeQueryActivity(
															  nvc != null ? nvc.Get("txtName") : string.Empty
                                                           , nvc != null ? nvc.Get("txtRollNo") : string.Empty 
														   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
														   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
														   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
														   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
														   , nvc != null ? nvc.Get("lstNationality") : string.Empty
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSex") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ucBatch") : string.Empty)
                                                           , null
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
														   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
                                                           , General.GetNullableString(nvc != null ? nvc.Get("cblStatus") : string.Empty)
														   , sortexpression, sortdirection
														   , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
														   , ref iRowCount, ref iTotalPageCount);

			DataSet ds = new DataSet();
			DataTable dtCopy = dt.Copy();
			ds.Tables.Add(dtCopy);
			General.SetPrintOptions("gvPreSeaSearch", "Trainee", alCaptions, alColumns, ds);
			if (dt.Rows.Count > 0)
			{
				gvPreSeaSearch.DataSource = dt;
				gvPreSeaSearch.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvPreSeaSearch);
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
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

        string[] alColumns = { "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDTRBATCHROLLNUMBER", "FLDDATEOFBIRTH", "FLDCOURSENAME", "FLDBATCHNAME", "FLDSTATUS" };
        string[] alCaptions = { "First Name", "Middle Name", "Last Name", "Batch Roll No", "Date of Birth", "Course", "Batch", "Status" };

		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		NameValueCollection nvc = new NameValueCollection();
		DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtRollNo") : string.Empty 
																   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
																   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
																   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
																   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
																   , nvc != null ? nvc.Get("lstNationality") : string.Empty
																   , null
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ucBatch") : string.Empty)
                                                                   , null
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
																   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
                                                                   , General.GetNullableString(nvc != null ? nvc.Get("cblStatus") : string.Empty)
                                                                   , sortexpression
																   , sortdirection
																   , (int)ViewState["PAGENUMBER"]
																   , General.ShowRecords(null)
																   , ref iRowCount
																   , ref iTotalPageCount);


		Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaTrainee.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
		Response.Write("<td><h3>Trainee</h3></td>");
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
		foreach (DataRow dr in dt.Rows)
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
}
