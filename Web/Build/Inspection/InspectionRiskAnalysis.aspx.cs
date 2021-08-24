using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRiskAnalysis : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../Inspection/InspectionRiskAnalysis.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvRiskAnalysis')", "Print Grid", "icon_print.png", "PRINT");
			toolbargrid.AddImageButton("../Inspection/InspectionRiskAnalysis.aspx", "Find", "search.png", "FIND");
			toolbargrid.AddImageButton("../Inspection/InspectionRiskAnalysis.aspx", "Add", "add.png", "ADD");
			MenuRiskAnalysisGrid.AccessRights = this.ViewState;
			MenuRiskAnalysisGrid.MenuList = toolbargrid.Show();

			if (!IsPostBack)
			{

				PhoenixToolbar toolbarmain = new PhoenixToolbar();
				toolbarmain.AddButton("List", "LIST");
				toolbarmain.AddButton("Details", "DETAIL");
				MenuRiskAnalysis.AccessRights = this.ViewState;
				MenuRiskAnalysis.MenuList = toolbarmain.Show();

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["JOBID"] = null;
				if (Request.QueryString["ANALYSISID"] != null)
				{
					ViewState["ANALYSISID"] = Request.QueryString["ANALYSISID"].ToString();
				}
				MenuRiskAnalysis.SelectedMenuIndex = 0;
			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void RiskAnalysis_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("DETAIL"))
			{
				if (ViewState["ANALYSISID"] == null)
				{
					throw new Exception("Select a category to view details.");
				}
				Response.Redirect("../Inspection/InspectionRiskAnalysisDetail.aspx?analysisid=" + ViewState["ANALYSISID"].ToString(), false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
			MenuRiskAnalysis.SelectedMenuIndex = 0;
		}
	}

	protected void gvRiskAnalysis_RowCommand(object sender, GridViewCommandEventArgs e)
	{

		if (e.CommandName.ToString().ToUpper() == "SORT")
			return;

		GridView _gridView = (GridView)sender;
		int nCurrentRow = int.Parse(e.CommandArgument.ToString());

		//if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
		//{
		//    string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
		//    int jobregister = 2; // Standard Jobs
		//    int selectedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
		//    PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
		//        jobregister, General.GetNullableGuid(jobid), selectedyn);
		//}

		//if (e.CommandName.ToString().ToUpper() == "EDIT")
		//{
		//    try
		//    {
		//        string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
		//        Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + jobid, false);
		//    }
		//    catch (Exception ex)
		//    {
		//        ucError.ErrorMessage = ex.Message;
		//        ucError.Visible = true;
		//    }
		//}
		BindData();
	}


	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDACTIVITY", "FLDCREATEDDATE" };
			string[] alCaptions = { "Category Name", "Sub Category Name", "Activity", "Date" };
			string sortexpression;
			int? sortdirection = null;

			sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

			DataSet ds;

			ds = PhoenixInspectionRiskAnalysis.RiskAnalysisSearch
				(null, null, null, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
				sortexpression, sortdirection,
				 Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
				  iRowCount, ref iRowCount, ref iTotalPageCount);

			General.ShowExcel("Risk Analysis", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void RiskAnalysisGrid_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if (dce.CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
				SetPageNavigator();
			}
			if (dce.CommandName.ToUpper().Equals("ADD"))
			{
				Response.Redirect("../Inspection/InspectionRiskAnalysisDetail.aspx?analysisid=", false);
			}
		
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	

	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDACTIVITY", "FLDCREATEDDATE" };
			string[] alCaptions = { "Category Name", "Sub Category Name", "Activity", "Date" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds;

			ds = PhoenixInspectionRiskAnalysis.RiskAnalysisSearch
				(null,null,null,PhoenixSecurityContext.CurrentSecurityContext.VesselID,
				sortexpression, sortdirection,
				 Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
				  General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

			General.SetPrintOptions("gvRiskAnalysis", "Risk Analysis", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvRiskAnalysis.DataSource = ds;
				gvRiskAnalysis.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvRiskAnalysis);
			}

			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvRiskAnalysis_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

	}

	protected void gvRiskAnalysis_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void gvRiskAnalysis_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	{
		GridView _gridview = (GridView)sender;
		_gridview.SelectedIndex = se.NewSelectedIndex;
		ViewState["ANALYSISID"] = ((Label)_gridview.Rows[se.NewSelectedIndex].FindControl("lblAnalysisid")).Text;
	}

	protected void gvRiskAnalysis_OnRowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		try
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
					if (db != null)
					{
						db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
						if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
					}
				}

				//CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
				//Button b = (Button)e.Row.FindControl("cmdSelectedYN");
				//DataRowView drv = (DataRowView)e.Row.DataItem;
				//cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true;

				//string jvscript = "";
				//if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
				//if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
				//if (b != null) b.Attributes.Add("style", "visibility:hidden");
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		try
		{
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
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
			ViewState["JOBID"] = null;
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
			gvRiskAnalysis.SelectedIndex = -1;
			gvRiskAnalysis.EditIndex = -1;
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

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		try
		{

			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
}
