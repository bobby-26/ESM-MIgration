using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRACargoFormList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbar = new PhoenixToolbar();

		toolbar.AddImageButton("../Inspection/InspectionRACargoFormList.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvRiskAssessmentCargo')", "Print Grid", "icon_print.png", "PRINT");
		toolbar.AddImageButton("../Inspection/InspectionRACargoFormList.aspx", "New Template", "add.png", "ADD");
	
		MenuCargo.MenuList = toolbar.Show();

		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
		}
		BindData();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDWORKACTIVITY", "FLDINTENDEDWORKDATE" };
		string[] alCaptions = { "Number", "Date", "Type", "Work Activity", "Intended Work Date" };

		DataSet ds = PhoenixInspectionRiskAssessmentCargoForm.RiskAssessmentCargoFormSearch(
					null, null, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
					Int32.Parse(ViewState["PAGENUMBER"].ToString()),
					General.ShowRecords(null),
					ref iRowCount,
					ref iTotalPageCount);

		General.SetPrintOptions("gvRiskAssessmentCargo", "Risk Assessment-Cargo", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvRiskAssessmentCargo.DataSource = ds;
			gvRiskAssessmentCargo.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvRiskAssessmentCargo);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		SetPageNavigator();
	}
	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDWORKACTIVITY", "FLDINTENDEDWORKDATE" };
			string[] alCaptions = { "Number", "Date", "Type", "Work Activity", "Intended Work Date" };

			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

			DataSet ds = PhoenixInspectionRiskAssessmentCargoForm.RiskAssessmentCargoFormSearch(
					null, null, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
					Int32.Parse(ViewState["PAGENUMBER"].ToString()),
					iRowCount,
					ref iRowCount,
					ref iTotalPageCount);

			General.ShowExcel("Risk Assessment-Cargo", ds.Tables[0], alColumns, alCaptions, null, null);

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


		if (dce.CommandName.ToUpper().Equals("ADD"))
		{
			Response.Redirect("../Inspection/InspectionRACargoForm.aspx", false);
		}
		else if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}

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

	protected void gvRiskAssessmentCargo_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{

				ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);


			}
		}
	}

	protected void gvRiskAssessmentCargo_RowCommand(object sender, GridViewCommandEventArgs gce)
	{
		GridView _gridView = (GridView)sender;
		int nRow = int.Parse(gce.CommandArgument.ToString());

		if (gce.CommandName.ToUpper().Equals("EDITROW"))
		{
			Label lbl = (Label)_gridView.Rows[nRow].FindControl("lblCargoFormid");
			Response.Redirect("../Inspection/InspectionRACargoForm.aspx?genericid=" + lbl.Text, false);
		}
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
}
