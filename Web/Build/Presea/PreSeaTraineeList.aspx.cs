using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web.UI;

public partial class PreSeaTraineeList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Back", "BACK");

			PreSeaQuery.AccessRights = this.ViewState;
			PreSeaQuery.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				if (Request.QueryString["p"] != null)
					ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
				else
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
		//Filter.CurrentPreSeaNewApplicantSelection = null;
		//Session["REFRESHFLAG"] = null;
		Response.Redirect("..\\PreSea\\PreSeaTraineeQueryFilter.aspx",false);
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

			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvPreSeaSearch_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewSelectedIndex;
		string Script = "";
		NameValueCollection nvc = Filter.CurrentPickListSelection;
		string id = _gridView.DataKeys[nCurrentRow].Value.ToString();
		Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
		Script += "fnClosePickList('codehelp1','ifMoreInfo');";
		Script += "</script>" + "\n";

		LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkEployeeName");
		nvc.Set(nvc.GetKey(1), lb.Text.ToString());
		nvc.Set(nvc.GetKey(2), id);

		Filter.CurrentPickListSelection = nvc;
		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

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
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				DataRowView drv = (DataRowView)e.Row.DataItem;
				Label empid = (Label)e.Row.FindControl("lblEmployeeid");

				ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
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
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		try
		{

			NameValueCollection nvc = Filter.CurrentPreSeaNewApplicantFilterCriteria;

			DataTable dt = PhoenixPreSeaCommon.PreSeaNewApplicantTraineeSearch(
																								  nvc != null ? nvc.Get("txtName") : string.Empty
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
																							   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
																							   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
																							   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
																							   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
																							   , sortexpression, sortdirection
																							   , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
																							   , ref iRowCount, ref iTotalPageCount);


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

		string[] alColumns = { "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDDATEOFBIRTH", "FLDCOURSENAME", "FLBATCHNAME" };
		string[] alCaptions = { "First Name", "Middle Name", "Last Name", "Date of Birth", "Course Name", "Batch Name" };
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
		DataTable dt = PhoenixPreSeaCommon.PreSeaNewApplicantTraineeSearch(nvc != null ? nvc.Get("txtName") : string.Empty
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
																						   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
																						   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
																						   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
																						   , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
																						   , sortexpression
																						   , sortdirection
																						   , (int)ViewState["PAGENUMBER"]
																						   , General.ShowRecords(null)
																						   , ref iRowCount
																						   , ref iTotalPageCount);


		Response.AddHeader("Content-Disposition", "attachment; filename=PersonnelMaster.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Personnel Master</h3></td>");
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
