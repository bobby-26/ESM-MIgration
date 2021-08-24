using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTraineePractical : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			PhoenixToolbar toolbarsub = new PhoenixToolbar();
			toolbarsub.AddImageButton("../PreSea/PreSeaTraineePractical.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbarsub.AddImageLink("javascript:CallPrint('gvPractical')", "Print Grid", "icon_print.png", "PRINT");
			if (!IsPostBack)
			{

				ViewState["confirmedyn"] = "";
				if (Request.QueryString["batchid"] != null)
				{
					ddlBatch.SelectedBatch = Request.QueryString["batchid"];
					ddlSection.DataSource = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(Request.QueryString["batchid"]));
					ddlSection.DataBind();

					if (Request.QueryString["sectionid"] != null)
					{
						ddlSection.SelectedValue = Request.QueryString["sectionid"];
					}
					ddlPractical.DataSource = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(Request.QueryString["batchid"]), General.GetNullableInteger(Request.QueryString["sectionid"]));
					ddlPractical.DataBind();
				}
				

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("Roll No", "ROLLNOMAPPING");
				toolbar.AddButton("Section", "SECTIONMAPPING");
				toolbar.AddButton("Practical", "PRACTICALMAPPING");
				PreSeaQuery.AccessRights = this.ViewState;
				PreSeaQuery.MenuList = toolbar.Show();
				PreSeaQuery.SelectedMenuIndex = 2;
				if (General.GetNullableInteger(Request.QueryString["batchid"]) != null)
				{
					DataTable dt = PhoenixPreSeaBatchManager.ListBatchDetails(General.GetNullableInteger(Request.QueryString["batchid"]));

					if (dt.Rows.Count > 0)
					{
						DataRow dr = dt.Rows[0];
						ViewState["confirmedyn"] = dr["FLDISFINALISED"].ToString();
					}
					else
					{
						ViewState["confirmedyn"] = "";

					}
				}
			}
			if (ddlSection.SelectedValue != "Dummy" && ddlPractical.SelectedValue != "Dummy" && ViewState["confirmedyn"].ToString() != "1")
			{
				toolbarsub.AddImageLink("javascript:Openpopup('codehelp1','','../PreSea/PreSeaTraineeSelectedList.aspx?practicalid="+ddlPractical.SelectedValue+"&sectionid=" + ddlSection.SelectedValue + "&batchid=" + ddlBatch.SelectedBatch + "');", "Add", "add.png", "ADDTRAINEE");

			}
			MenuPreSeaPractical.AccessRights = this.ViewState;
			MenuPreSeaPractical.MenuList = toolbarsub.Show();
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvPractical_RowCommand(object sender, GridViewCommandEventArgs e)
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
	protected void PreSeaQuery_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("ROLLNOMAPPING"))
		{
			Response.Redirect("..\\PreSea\\PreSeaTraineeMapping.aspx?batchid=" + General.GetNullableInteger(ddlBatch.SelectedBatch), false);
		}
		if (dce.CommandName.ToUpper().Equals("SECTIONMAPPING"))
		{
			Response.Redirect("..\\PreSea\\PreSeaTraineeSection.aspx?batchid=" + General.GetNullableInteger(ddlBatch.SelectedBatch)+"&sectionid="+ddlSection.SelectedValue, false);
		}
	}
	protected void PreSeaPractical_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}

	}
	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDROWNUMBER", "FLDBATCHROLLNUMBER", "FLDTRAINEENAME", "FLDSTATENAME", "FLDNATIONALITY", "FLDDATEOFBIRTH" };
		string[] alCaptions = { "Sl no", "Roll Number", "Trainee Name", "State", "Nationality", "Date of birth" };


		DataSet ds = PhoenixPreSeaTrainee.PreSeaTraineePracticalSearch(General.GetNullableInteger(ddlBatch.SelectedBatch),
																	General.GetNullableInteger(ddlSection.SelectedValue),
																	General.GetNullableInteger(ddlPractical.SelectedValue),
																	(int)ViewState["PAGENUMBER"]
																   , General.ShowRecords(null)
																   , ref iRowCount
																   , ref iTotalPageCount);

		General.SetPrintOptions("gvPractical", "Practical Mapping", alCaptions, alColumns, ds);
		if (ds.Tables[0].Rows.Count > 0)
		{

			gvPractical.DataSource = ds.Tables[0];
			gvPractical.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvPractical);
		}
		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		SetPageNavigator();
	}
	protected void OnBatchChanged(object sender, EventArgs e)
	{

		ddlSection.Items.Clear();
		ddlSection.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlSection.DataSource = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(ddlBatch.SelectedBatch));
		ddlSection.DataBind();


		ddlPractical.Items.Clear();
		ddlPractical.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlPractical.DataSource = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(ddlBatch.SelectedBatch),
																		General.GetNullableInteger(ddlSection.SelectedValue));
		ddlPractical.DataBind();

		PhoenixToolbar toolbarsub = new PhoenixToolbar();
		toolbarsub.AddImageButton("../PreSea/PreSeaTraineePractical.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbarsub.AddImageLink("javascript:CallPrint('gvPractical')", "Print Grid", "icon_print.png", "PRINT");
		if (ddlSection.SelectedValue != "Dummy" && ddlPractical.SelectedValue != "Dummy")
		{
			toolbarsub.AddImageLink("javascript:Openpopup('codehelp1','','../PreSea/PreSeaTraineeSelectedList.aspx?practicalid="+ddlPractical.SelectedValue+"&sectionid=" + ddlSection.SelectedValue + "&batchid=" + ddlBatch.SelectedBatch + "')", "Add", "add.png", "ADDTRAINEE");
		}
		MenuPreSeaPractical.AccessRights = this.ViewState;
		MenuPreSeaPractical.MenuList = toolbarsub.Show();
		if (General.GetNullableInteger(ddlBatch.SelectedBatch) != null)
		{
			DataTable dt = PhoenixPreSeaBatchManager.ListBatchDetails(General.GetNullableInteger(ddlBatch.SelectedBatch));

			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				ViewState["confirmedyn"] = dr["FLDISFINALISED"].ToString();
			}
			else
			{
				ViewState["confirmedyn"] = "";

			}
		}
	}
	protected void OnSectionChanged(object sender, EventArgs e)
	{

		ddlPractical.Items.Clear();
		ddlPractical.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlPractical.DataSource = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(ddlBatch.SelectedBatch),
															General.GetNullableInteger(ddlSection.SelectedValue));
		ddlPractical.DataBind();

	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string date = DateTime.Now.ToShortDateString();

		string[] alColumns = { "FLDROWNUMBER", "FLDBATCHROLLNUMBER", "FLDTRAINEENAME", "FLDSTATENAME", "FLDNATIONALITY", "FLDDATEOFBIRTH" };
		string[] alCaptions = { "Sl no", "Roll Number", "Trainee Name", "State", "Nationality", "Date of birth" };


		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		DataSet ds = PhoenixPreSeaTrainee.PreSeaTraineePracticalSearch(General.GetNullableInteger(ddlBatch.SelectedBatch),
																General.GetNullableInteger(ddlSection.SelectedValue),
																General.GetNullableInteger(ddlPractical.SelectedValue),
																(int)ViewState["PAGENUMBER"]
															   , iRowCount
															   , ref iRowCount
															   , ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaTraineePracticalWiseList.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Practical Mapping</center></h3></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Batch : " + ddlBatch.SelectedName + "</center></h3></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Section : " + ddlSection.SelectedItem.Text + "</center></h3></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		Response.Write("<tr>");
		for (int i = 0; i < alCaptions.Length; i++)
		{
			Response.Write("<td>");
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
	protected void gvPractical_RowDataBound(Object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (ViewState["confirmedyn"].ToString() == "1")
			{
				e.Row.Cells[7].Visible = false;

			}

		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
			if (del != null)
			{
				del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
				del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			}
			if (ViewState["confirmedyn"].ToString() == "1")
			{
				e.Row.Cells[7].Visible = false;

			}
		}

	}
	protected void gvPractical_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = de.RowIndex;
			string traineeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeid")).Text;
			string sectionid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSectionid")).Text;
			string batchid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblbatchid")).Text;
			string practicalid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPracticalid")).Text;
			PhoenixPreSeaTrainee.DeletePreSeaTraineePractical(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
								Convert.ToInt32(sectionid),
								traineeid,
								Convert.ToInt32(batchid),
								Convert.ToInt32(practicalid));
			_gridView.EditIndex = -1;
			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void PreSeaBatch_TabStripCommand(object sender, EventArgs e)
	{

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
			gvPractical.SelectedIndex = -1;
			gvPractical.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
			else
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

			BindData();
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
	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}
