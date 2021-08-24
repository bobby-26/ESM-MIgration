using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;
public partial class CrewLicenceRequestCancelList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			if (!Page.IsPostBack)
			{
				PhoenixToolbar toolbargrid = new PhoenixToolbar();
				toolbargrid.AddImageButton("../Crew/CrewLicenceRequestCancelList.aspx", "Export to Excel", "icon_xls.png", "Excel");
				toolbargrid.AddImageLink("javascript:CallPrint('gvLicReq')", "Print Grid", "icon_print.png", "PRINT");
				MenuLicenceList.AccessRights = this.ViewState;
				MenuLicenceList.MenuList = toolbargrid.Show();

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

	protected void MenuLicenceList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		try
		{
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;

				string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCANCELDATE", "FLDCANCELLEDBY", "FLDSTATUSNAME" };
				string[] alCaptions = { "Request Number", "Flag", "Vessel Name", "Crew Change Date", "Requested Date", "CRA Reqd Date", "Cancelled Date", " Cancelled By", "Status" };
				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

				NameValueCollection nvc = null;
				DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																				, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																				, sortexpression, sortdirection
																				, (int)ViewState["PAGENUMBER"], iRowCount
																				, ref iRowCount, ref iTotalPageCount
																				, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																				, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																				, null
																				, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																				, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null, 0
																				, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                                , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);

				DataSet ds = new DataSet();
				ds.Tables.Add(dt.Copy());

				if (ds.Tables.Count > 0)
					General.ShowExcel("Licence Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	
	protected void gvLicReq_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			Label l = (Label)e.Row.FindControl("lblProcessId");

			LinkButton lb = (LinkButton)e.Row.FindControl("lnkRefNo");
			lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'CrewLicenceRequestCancel.aspx?pid=" + l.Text + "');return false;");
			
		
		}
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
	}


	protected void gvLicReq_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
		SetPageNavigator();
	}

	protected void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCANCELDATE", "FLDCANCELLEDBY", "FLDSTATUSNAME" };
		string[] alCaptions = { "Request Number", "Flag", "Vessel Name", "Crew Change Date", "Requested Date", "CRA Reqd Date", "Cancelled Date", " Cancelled By" ,"Status"};

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		try
		{
			NameValueCollection nvc = null; 
			DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																			, sortexpression, sortdirection
																			, (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
																			, ref iRowCount, ref iTotalPageCount
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																			, null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null, 0
																			, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                            , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);

		
			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());

			General.SetPrintOptions("gvLicReq", "Cancelled Licence Request", alCaptions, alColumns, ds);

			if (dt.Rows.Count > 0)
			{
				gvLicReq.DataSource = dt;
				gvLicReq.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvLicReq);
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
			gvLicReq.SelectedIndex = -1;
			gvLicReq.EditIndex = -1;
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
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
}
