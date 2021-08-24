using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaAdmissionHandOverList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			if (!IsPostBack)
			{

				PhoenixToolbar toolbarsub = new PhoenixToolbar();
				toolbarsub.AddImageButton("../PreSea/PreSeaAdmissionHandOverList.aspx", "Export to Excel", "icon_xls.png", "Excel");
				toolbarsub.AddImageLink("javascript:CallPrint('gvAdminHandOver')", "Print Grid", "icon_print.png", "PRINT");
				MenuPreSeaAdminHandOver.AccessRights = this.ViewState;
				MenuPreSeaAdminHandOver.MenuList = toolbarsub.Show();
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
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
			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvAdminHandOver_RowCommand(object sender, GridViewCommandEventArgs e)
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

	protected void PreSeaAdminHandOver_TabStripCommand(object sender, EventArgs e)
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

		string[] alColumns = { "FLDBATCHROLLNUMBER", "FLDTRAINEENAME", "FLDSECTIONNAME", "FLDGROUPNAME", "FLDSTATENAME", "FLDNATIONALITY", "FLDDATEOFBIRTH" };
		string[] alCaptions = { "Roll Number", "Trainee Name", "Section", "Practical", "State", "Nationality", "Date of birth" };


		DataSet ds = PhoenixPreSeaTrainee.ListPreSeaTraineeAdministration(General.GetNullableInteger(ddlBatch.SelectedBatch),
																	General.GetNullableInteger(ddlSection.SelectedValue),
																	General.GetNullableInteger(ddlPractical.SelectedValue));

		General.SetPrintOptions("gvAdminHandOver", "Adminstration HandOver Docs ", alCaptions, alColumns, ds);
		if (ds.Tables[0].Rows.Count > 0)
		{

			gvAdminHandOver.DataSource = ds.Tables[0];
			gvAdminHandOver.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvAdminHandOver);
		}
		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}
	protected void OnBatchChanged(object sender, EventArgs e)
	{

		ddlSection.Items.Clear();
		ddlSection.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlSection.DataSource = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(ddlBatch.SelectedBatch));
		ddlSection.DataBind();


		ddlPractical.Items.Clear();
		ddlPractical.Items.Insert(0, new ListItem("--Select--", "Dummy"));
		ddlPractical.DataSource = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(ddlBatch.SelectedBatch), General.GetNullableInteger(ddlSection.SelectedValue));
		ddlPractical.DataBind();

		PhoenixToolbar toolbarsub = new PhoenixToolbar();
		toolbarsub.AddImageButton("../PreSea/PreSeaTraineePractical.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbarsub.AddImageLink("javascript:CallPrint('gvAdminHandOver')", "Print Grid", "icon_print.png", "PRINT");



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
	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string date = DateTime.Now.ToShortDateString();

		string[] alColumns = { "FLDBATCHROLLNUMBER", "FLDTRAINEENAME", "FLDSECTIONNAME", "FLDGROUPNAME", "FLDSTATENAME", "FLDNATIONALITY", "FLDDATEOFBIRTH" };
		string[] alCaptions = { "Roll Number", "Trainee Name", "Section", "Practical", "State", "Nationality", "Date of birth" };


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
	protected void gvAdminHandOver_RowDataBound(Object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			Label lbltraineeid = (Label)e.Row.FindControl("lblTraineeid");
			ImageButton imgHandover = (ImageButton)e.Row.FindControl("cmdHandover");
			if (imgHandover != null)
			{
				imgHandover.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'PreSeaAdmissionHandOver.aspx?traineeid=" + lbltraineeid.Text
					+ "&batchid=" + ddlBatch.SelectedBatch + "');return false;");
			}
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

	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}
