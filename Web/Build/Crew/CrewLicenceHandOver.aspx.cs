using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewLicenceHandOver : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			if (!Page.IsPostBack)
			{

				//PhoenixToolbar toolbarmain = new PhoenixToolbar();
				//toolbarmain.AddButton("CRA", "CRA");
				//toolbarmain.AddButton("Full Term", "FULLTERM");
				//CrewLicReq.AccessRights = this.ViewState;
				//CrewLicReq.MenuList = toolbarmain.Show();
				//CrewLicReq.SelectedMenuIndex = 0;

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				Filter.CurrentLicenceRequestFilter = null;
				//Filter.CurrentLicenceReqCovLetterFilter = null;
				ViewState["CURRENTINDEX"] = 1;
				//ViewState["PID"] = string.Empty;
				//ViewState["PAGEURL"] = "CrewLicenceRequestCRADetails.aspx";
				
			
			}

			BindData();
			SetPageNavigator();
			//if (!string.IsNullOrEmpty(ViewState["PID"].ToString()))
			//    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?pid=" + ViewState["PID"];

			PhoenixToolbar toolbargrid = new PhoenixToolbar();
		
			toolbargrid.AddImageButton("../Crew/CrewLicenceHandOver.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvLicReq')", "Print Grid", "icon_print.png", "PRINT");
			toolbargrid.AddImageLink("javascript:Openpopup('Filter','','CrewLicenceRequestFilter.aspx'); return false;", "Filter", "search.png", "FIND");
			toolbargrid.AddImageButton("../Crew/CrewLicenceHandOver.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
			MenuLicenceList.AccessRights = this.ViewState;
			MenuLicenceList.MenuList = toolbargrid.Show();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void CrewLicReq_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("CRA"))
			{
				ViewState["PAGEURL"] = "CrewLicenceRequestCRADetails.aspx";
			}
			else if (dce.CommandName.ToUpper().Equals("FULLTERM"))
			{
				ViewState["PAGEURL"] = "CrewLicenceRequestFullTermDetails.aspx";
			}
			
			//ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?pid=" + ViewState["PID"];
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

                string[] alColumns = { "FLDROWNUMBER", "FLDREFNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDZONE", "FLDVESSELNAME", "FLDJOINEDVESSEL",  "FLDCRASTATUS", "FLDFLAGNAME", "FLDLICENCE", "FLDCRANUMBER", "FLDDATEOFEXPIRY", "FLDREMARKS" };
                string[] alCaptions = { "S.No", "Request No", "File No", "Name", "Rank", "Zone", "Tentative Vessel", "Joined Vessel",  "Status", "Flag", "Licence", "CRA No", "Expiry", "Remarks" };

				string sortexpression;
				int? sortdirection = 1;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

				NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;

				DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessHandOverSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																		, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																		, sortexpression, sortdirection
																		, (int)ViewState["PAGENUMBER"], iRowCount
																		, ref iRowCount, ref iTotalPageCount
																		, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																		, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null
																		, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null,null);
				DataSet ds = new DataSet();
				ds.Tables.Add(dt.Copy());

				if (ds.Tables.Count > 0)
					General.ShowExcel("Licence Hand Over", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
			else if (dce.CommandName.ToUpper().Equals("CLEAR"))
			{
                ViewState["PAGENUMBER"] = 1;
				Filter.CurrentLicenceRequestFilterSelection = null;
                txtnopage.Text = "";
				BindData();
                SetPageNavigator();
				//cmdHiddenSubmit_Click(null, null);
			} 
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void gvLicReq_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            row.Attributes.Add("style", "position:static");
            TableCell cell = new TableCell();
            cell.ColumnSpan = 6;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Vessel";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 7;
            row.Cells.Add(cell);

            gvLicReq.Controls[0].Controls.AddAt(0, row);
            GridViewRow row1 = ((GridViewRow)gvLicReq.Controls[0].Controls[0]);

        }
    }


	protected void gvLicReq_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.SelectedIndex = e.NewEditIndex;

			Label lblPID = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblProcessId"));
			ViewState["PID"] = lblPID.Text;
			//ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?pid=" + ViewState["PID"];

			_gridView.EditIndex = e.NewEditIndex;

			Response.Redirect("../Crew/CrewLicenceRequestCRADetails.aspx?pid="+ ViewState["PID"], false);
			
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvLicReq_RowDataBound(object sender, GridViewRowEventArgs e)
	{
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //         && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {

        //        Label l = (Label)e.Row.FindControl("lblDtKey");
        //        Label lblcrastatus = (Label)e.Row.FindControl("lblCraStatus");
        //        ImageButton lb = (ImageButton)e.Row.FindControl("cmdNote");
        //        lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'CrewLicenceHandOverNote.aspx?dtkey=" + l.Text + "&crastatus=" + lblcrastatus.Text + "');return false;");
        //    }
        //}
		
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

                LinkButton lb = (LinkButton)e.Row.FindControl("lblName");

            if(lb!=null)
            {    

                if (drv["FLDNEWAPP"].ToString() == "1")
                {
                    lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
                }
                else
                {
                    lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
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


	protected void gvLicReq_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		try
		{
			if (e.CommandName.ToString().ToUpper() == "SORT") return;

			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToString().ToUpper() == "SELECT")
			{
				_gridView.SelectedIndex = nCurrentRow;

				Label lblPID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId"));
				Label lblflagid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFlagId"));
				ViewState["PID"] = lblPID.Text;
				ViewState["FLAGID"] = lblflagid.Text;
				Response.Redirect("../Crew/CrewLicenceRequestCRADetails.aspx?pid=" + ViewState["PID"], false);
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



	protected void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDREFNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDZONE", "FLDVESSELNAME", "FLDJOINEDVESSEL", "FLDCRASTATUS", "FLDFLAGNAME", "FLDLICENCE", "FLDCRANUMBER", "FLDDATEOFEXPIRY", "FLDREMARKS" };
        string[] alCaptions = { "S.No", "Request No", "File No", "Name", "Rank", "Zone", "Tentative Vessel", "Joined Vessel", "Status", "Flag", "Licence", "CRA No", "Expiry", "Remarks" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = 1;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		try
		{
			NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;
			DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessHandOverSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																		, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																		, sortexpression, sortdirection
																		, (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
																		, ref iRowCount, ref iTotalPageCount
																		, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																		, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null
																		, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null,null);
			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());

			General.SetPrintOptions("gvLicReq", "Licence Request", alCaptions, alColumns, ds);

			if (dt.Rows.Count > 0)
			{
				//if (ViewState["PID"].ToString() == string.Empty)
				//{
				//    ViewState["PID"] = dt.Rows[0]["FLDPROCESSID"].ToString();
				//    gvLicReq.SelectedIndex = 0;
				//    ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();

				//}
				gvLicReq.DataSource = dt;
				gvLicReq.DataBind();
			}
			else
			{
				//ifMoreInfo.Attributes["src"] = "CrewLicenceRequestCRADetails.aspx";
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
		ViewState["PID"] = string.Empty;
		BindData();
		SetPageNavigator();
		//ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?pid=" + ViewState["PID"];
        txtnopage.Text = "";
	}
	
	
}
