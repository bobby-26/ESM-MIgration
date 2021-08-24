using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;
public partial class CrewPlanReliever : PhoenixBasePage
{
    string strVessel = string.Empty;
    string strRankId = string.Empty;
    string strOffSigner = string.Empty;
    string strRelieverId = string.Empty;
    string strDate = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewPlanReliever.aspx?empid=" + Request.QueryString["empid"] + "&vesselid=" + Request.QueryString["vesselid"] + "&rankid=" + Request.QueryString["rankid"] + "&relieverid=" + Request.QueryString["relieverid"] + "&IsTop4=" + Request.QueryString["IsTop4"], "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvRelieverSearch')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageLink("javascript:Openpopup('Filter','','CrewPlanRelieverFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbar.AddImageButton("../Crew/CrewPlanReliever.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
			RelieverMenu.AccessRights = this.ViewState;
			RelieverMenu.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
			if (!IsPostBack)
			{
				ViewState["RPAGENUMBER"] = 1;
				ViewState["RSORTEXPRESSION"] = null;
				ViewState["RSORTDIRECTION"] = null;
				ViewState["RCURRENTINDEX"] = 1;
				ViewState["RROWCOUNT"] = 0;
				ViewState["RTOTALPAGECOUNT"] = 0;
                ViewState["EMPID"] = string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"];
                ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["vesselid"]) ? "" : Request.QueryString["vesselid"];
                ViewState["RANKID"] = string.IsNullOrEmpty(Request.QueryString["rankid"]) ? "" : Request.QueryString["rankid"];
                ViewState["RELIEFDATE"] = string.IsNullOrEmpty(Request.QueryString["reliefdate"]) ? "" : Request.QueryString["reliefdate"];
                ViewState["SELECTEDINDEX"] = string.IsNullOrEmpty(Request.QueryString["selectedindex"]) ? "" : Request.QueryString["selectedindex"];
                ViewState["ISTOP4"] = string.IsNullOrEmpty(Request.QueryString["IsTop4"]) ? "" : Request.QueryString["IsTop4"];
				ucConfirm.Visible = false;
                if (Request.QueryString["IsTop4"] != "1")
                {
                    gvRelieverMatrix.Visible = false;
                    lblCombinedExp.Visible = false;
                }
			}
            BindRelieverMatrixData();
			BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
			SetRPageNavigator();
			CreateTabs();
			CrewRelieverTabs.SelectedMenuIndex = 1;
                                   
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }       
    }
	private void CreateTabs()
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Relief Plan", "RELIEFPLAN");
		toolbarmain.AddButton("Reliever", "RELIEVER");

		CrewRelieverTabs.AccessRights = this.ViewState;
		CrewRelieverTabs.MenuList = toolbarmain.Show();
		CrewRelieverTabs.SelectedMenuIndex = 1;
	}
	protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			
			if (dce.CommandName.ToUpper().Equals("RELIEVER"))
			{
				Response.Redirect("CrewPlanReliever.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"], false);
			}
			else if (dce.CommandName.ToUpper().Equals("RELIEFPLAN"))
			{
                if (Request.QueryString["selectedindex"] != null)
                    Response.Redirect("CrewPlanRelievee.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&selectedindex=" + ViewState["SELECTEDINDEX"].ToString(), false);
                else
                    Response.Redirect("CrewPlanRelievee.aspx", false);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void btnPlan_Click(object sender, EventArgs e)
	{
		UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
		try
		{

			if (ucCM.confirmboxvalue == 1 )
			{
			
				string strRelieverId = ((Label)gvRelieverSearch.Rows[gvRelieverSearch.SelectedIndex].FindControl("lblRelieverId")).Text;
				string vesselid = ViewState["VESSELID"].ToString();
				string rankid = ViewState["RANKID"].ToString();
                string reliefDate = ViewState["RELIEFDATE"].ToString();
                Guid g = Guid.Empty;
                PhoenixCrewPlanning.PlanRelieverInsert(int.Parse(strRelieverId), int.Parse(vesselid), int.Parse(rankid), General.GetNullableInteger(ViewState["EMPID"].ToString()), null, DateTime.Parse(reliefDate).AddDays(1), ref g);
			}
		}
		catch (Exception ex)
		{
			ucCM.Visible = false;
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void RelieverMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("CLEAR"))
			{
				Filter.CurrentPlanRelieverFilterSelection = null;
				gvRelieverSearch.SelectedIndex = -1;
				BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
                SetRPageNavigator();
			}
			else if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;

                string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDEMPLOYEERANKNAME", "FLDEXPERIENCE", "FLDVESSELTYPEEXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDSTATUSDESCRIPTION" };
                string[] alCaptions = { "File no", "Name", "Rank", "Rank Experience", "Vessel Type Experience", "Sign-off date", "DOA", "Status" };

				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

				NameValueCollection nvc = Filter.CurrentPlanRelieverFilterSelection;
                DataTable dt = PhoenixCrewPlanning.RelieverPlanQueryActivity(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                        , nvc != null ? nvc.Get("ucVslType") : string.Empty
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtVslTypeExperience")) : General.GetNullableInteger(string.Empty)
                                                                        , nvc != null ? nvc.Get("ucEngType") : string.Empty
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtEngTypeExperience")) : General.GetNullableInteger(string.Empty)
                                                                        , nvc != null ? nvc.Get("ucRank") : string.Empty
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtRankExperience")) : General.GetNullableInteger(string.Empty)
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtCompanyExperience")) : General.GetNullableInteger(string.Empty)
                                                                        , (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkZone")) : General.GetNullableInteger(string.Empty))
                                                                        , (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkPool")) : General.GetNullableInteger(string.Empty))
                                                                        , (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkNationality")) : General.GetNullableInteger(string.Empty))
                                                                        , null
                                                                        , (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkDocuments")) : General.GetNullableInteger(string.Empty))
                                                                        , (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkPlanned")) : General.GetNullableInteger(string.Empty))
                                                                        , General.GetNullableInteger(rblOtherRank.SelectedValue)
                                                                        , (byte?)General.GetNullableInteger(rblOtherRank.SelectedValue == "2" ? "1" : "0")
                                                                        , sortexpression, sortdirection
                                                                        , (int)ViewState["RPAGENUMBER"], General.ShowRecords(null)
                                                                        , ref iRowCount, ref iTotalPageCount
                                                                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDOAFromDate")) : General.GetNullableDateTime(string.Empty)
                                                                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDOAToDate")) : General.GetNullableDateTime(string.Empty)
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucPrinicpal")) : null);

				DataSet ds = new DataSet();
				ds.Tables.Add(dt.Copy());


				if (ds.Tables.Count > 0)
					General.ShowExcel("Reliever List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvRelieverSearch_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper() == "SORT") return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper() == "PLANRELIEVER")
			{
				//if (gvSearch.SelectedIndex > -1)
				//{
				//    Filter.CurrentCrewSelection = null;
					Filter.CurrentNewApplicantSelection = null;
					string relieverid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRelieverId")).Text;
					if ((rblOtherRank.SelectedIndex == 0) || (rblOtherRank.SelectedIndex == 1))
					{
						Filter.CurrentCrewSelection = relieverid.ToString();
					}
					else
					{
						Filter.CurrentNewApplicantSelection = relieverid.ToString();
					}
					gvRelieverSearch.SelectedIndex = nCurrentRow;
					if (Filter.CurrentNewApplicantSelection != null)
					{
						Response.Redirect("../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRelieverId")).Text + "&evaluation=1");
					}
					else if (Filter.CurrentCrewSelection != null)
					{
						Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRelieverId")).Text + "&evaluation=1");
					}
				//}
				//else
				//{
				//    ucError.ErrorMessage = "Select a Employee from Relievee list to plan Reliever.";
				//    ucError.Visible = true;
				//}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    decimal r3;
    decimal r4;
	protected void gvRelieverSearch_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (ViewState["RSORTEXPRESSION"] != null)
			{
				HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["RSORTEXPRESSION"].ToString());
				if (img != null)
				{
					if (ViewState["RSORTDIRECTION"] == null || ViewState["RSORTDIRECTION"].ToString() == "0")
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

				Label l = (Label)e.Row.FindControl("lblPlanned");
				if (l.Text == "1") e.Row.CssClass = "bluefont";
				Label lblRelieverId = (Label)e.Row.FindControl("lblRelieverId");
				LinkButton lb = (LinkButton)e.Row.FindControl("lnkReliever");
                Label lblnewappyn = (Label)e.Row.FindControl("lblnewappyn");
                Label lblRankid = (Label)e.Row.FindControl("lblRankid");
                Label lblIsTop4 = (Label)e.Row.FindControl("lblIsTop4"); 
                Label lblrank = (Label)e.Row.FindControl("lblEmployeeRank");
                //string a = "Rank Exp" + "  " + "Vsl Type Exp<br/>" + "12.45" + " " + "55.44";
                //lblrank.ToolTip = HttpContext.Current.Server.HtmlDecode(a);
               
                string strnewpappyn ;
                if (lblnewappyn.Text == "1")
                {
                    strnewpappyn = "false";
                }
                else
                {
                    strnewpappyn = "true";
                }
               
				lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblRelieverId.Text + "'); return false;");

				ImageButton ib = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ib != null) ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);

                ImageButton imgSuitableCheck = (ImageButton)e.Row.FindControl("imgSuitableCheck");
                imgSuitableCheck.Attributes.Add("onclick", "parent.Openpopup('codehelp', '', 'CrewSuitabilityCheck.aspx?empid=" + lblRelieverId.Text + "&vesselid="+ViewState["VESSELID"].ToString()+"&personalmaster="+strnewpappyn+"&rankid="+lblRankid.Text+"');return false;");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
               
                DataRowView drv = (DataRowView)e.Row.DataItem;
                decimal main;

                if (drv["FLDEXPERIENCE"].ToString() != "")
                {
                  
                    if (txt1.Text == "")
                    {
                        main = 0.00m;
                    }
                    else
                    {
                        main = decimal.Parse(txt1.Text);
                    }
                    r3 = main + decimal.Parse(drv["FLDEXPERIENCE"].ToString());
                }
                else
                {
                    if (txt1.Text == "")
                    {
                        main = 0.00m;
                    }
                    else
                    {
                        main = decimal.Parse(txt1.Text);
                    }
                    r3 = main;

                }
                if (drv["FLDVESSELTYPEEXP"].ToString() != "")
                {
                   
                    if (txt1.Text == "")
                    {
                        main = 0.00m;
                    }
                    else
                    {
                        main = decimal.Parse(txt1.Text);
                    }
                    r4 = main + decimal.Parse(drv["FLDVESSELTYPEEXP"].ToString());
                }
                else
                {
                    if (txt2.Text == "")
                    {
                        main = 0.00m;
                    }
                    else
                    {
                        main = decimal.Parse(txt2.Text);
                    }
                    r4 = main;
                  

                }
                if (ViewState["ISTOP4"].ToString() == "1")
                {
                    uct.Visible = true;

                    uct.Text = "<table border='1'><tr><td></td><td>" + "<u>Rank Experience</u></td><td>" + "  " + "<u>Vessel Type Experience</u></td></tr>"
                                + "<tr><td>Reliever</td><td>" + drv["FLDEXPERIENCE"].ToString() + "</td><td>" + drv["FLDVESSELTYPEEXP"].ToString()
                                + "</td></tr><tr><td>Onboard</td><td>" + txt1.Text + "</td><td>" + txt2.Text
                                + "</td></tr><tr><td>Total</td><td>" + r3
                                + "</td><td>" + r4 + "</td></tr></table>";
                    lblrank.Attributes.Add("onmouseover", "showTooltip(event,'" + uct.ToolTip + "', 'visible');");
                    lblrank.Attributes.Add("onmouseout", "showTooltip(event,'" + uct.ToolTip + "', 'hidden');");
                    
                }
               
              
			}
			Label lblSuitableDoc = (Label)e.Row.FindControl("lblSuitableDoc");
		}
	}
	protected void gvRelieverSearch_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.Header)
		{
			GridView HeaderGrid = (GridView)sender;
			GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

			TableCell HeaderCell;

			HeaderCell = new TableCell();
			HeaderCell.Text = "On-Signer";
			HeaderCell.HorizontalAlign = HorizontalAlign.Center;
			HeaderCell.ColumnSpan = 9;
			HeaderGridRow.Cells.Add(HeaderCell);

			gvRelieverSearch.Controls[0].Controls.AddAt(0, HeaderGridRow);
		}
	}
	protected void gvRelieverSearch_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		ViewState["RSORTEXPRESSION"] = se.SortExpression;

		if (ViewState["RSORTDIRECTION"] != null && ViewState["RSORTDIRECTION"].ToString() == "0")
			ViewState["RSORTDIRECTION"] = 1;
		else
			ViewState["RSORTDIRECTION"] = 0;
		if (ViewState["EMPID"] != null)
			BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		ViewState["PAGENUMBER"] = 1;
		gvRelieverSearch.SelectedIndex = -1;
		BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));

	}
	public void BindRelieverData(int? iEmployeeId, int? iVesselId, int? iRankId)
	{

		int iRowCount = 0;
		int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDEMPLOYEERANKNAME", "FLDEXPERIENCE", "FLDVESSELTYPEEXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDSTATUSDESCRIPTION" };
        string[] alCaptions = { "File no", "Name", "Rank", "Rank Experience", "Vessel Type Experience", "Sign-off date", "DOA", "Status" };

		string sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["RSORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());
		else
			sortdirection = 1; //defaulting descending order for Relief due date
		try
		{

			NameValueCollection nvc = Filter.CurrentPlanRelieverFilterSelection;
			DataTable dt = PhoenixCrewPlanning.RelieverPlanQueryActivity(iEmployeeId, iVesselId, iRankId
																		, nvc != null ? nvc.Get("ucVslType") : string.Empty
																		, nvc != null ? General.GetNullableInteger(nvc.Get("txtVslTypeExperience")) : General.GetNullableInteger(string.Empty)
																		, nvc != null ? nvc.Get("ucEngType") : string.Empty
																		, nvc != null ? General.GetNullableInteger(nvc.Get("txtEngTypeExperience")) : General.GetNullableInteger(string.Empty)
																		, nvc != null ? nvc.Get("ucRank") : string.Empty
																		, nvc != null ? General.GetNullableInteger(nvc.Get("txtRankExperience")) : General.GetNullableInteger(string.Empty)
																		, nvc != null ? General.GetNullableInteger(nvc.Get("txtCompanyExperience")) : General.GetNullableInteger(string.Empty)
																		, (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkZone")) : General.GetNullableInteger(string.Empty))
																		, (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkPool")) : General.GetNullableInteger(string.Empty))
																		, (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkNationality")) : General.GetNullableInteger(string.Empty))
																		, null
																		, (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkDocuments")) : General.GetNullableInteger(string.Empty))
																		, (byte?)(nvc != null ? General.GetNullableInteger(nvc.Get("chkPlanned")) : General.GetNullableInteger(string.Empty))
																		, General.GetNullableInteger(rblOtherRank.SelectedValue)
																		, (byte?)General.GetNullableInteger(rblOtherRank.SelectedValue == "2" ? "1" : "0")
																		, sortexpression, sortdirection
																		, (int)ViewState["RPAGENUMBER"], General.ShowRecords(null)
																		, ref iRowCount, ref iTotalPageCount
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtDOAFromDate")) : General.GetNullableDateTime(string.Empty)
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtDOAToDate")) : General.GetNullableDateTime(string.Empty)
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucPrinicpal")) : null);

			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());

			General.SetPrintOptions("gvRelieverSearch", "Reliever List", alCaptions, alColumns, ds);

			if (dt.Rows.Count > 0)
			{
				gvRelieverSearch.DataSource = dt;
				gvRelieverSearch.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvRelieverSearch);
			}
			ViewState["RROWCOUNT"] = iRowCount;
			ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;

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

	private void SetRPageNavigator()
	{
		try
		{
			cmdRPrevious.Enabled = IsRPreviousEnabled();
			cmdRNext.Enabled = IsRNextEnabled();
			lblRPagenumber.Text = "Page " + ViewState["RPAGENUMBER"].ToString();
			lblRPages.Text = " of " + ViewState["RTOTALPAGECOUNT"].ToString() + " Pages. ";
			lblRRecords.Text = "(" + ViewState["RROWCOUNT"].ToString() + " records found)";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private Boolean IsRPreviousEnabled()
	{
		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
		iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

		if (iTotalPageCount == 0)
			return false;

		if (iCurrentPageNumber > 1)
		{
			return true;
		}

		return false;
	}

	private Boolean IsRNextEnabled()
	{
		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
		iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

		if (iCurrentPageNumber < iTotalPageCount)
		{
			return true;
		}
		return false;
	}

	protected void cmdRGo_Click(object sender, EventArgs e)
	{
		try
		{
			int result;
			if (Int32.TryParse(txtRnopage.Text, out result))
			{
				ViewState["RPAGENUMBER"] = Int32.Parse(txtRnopage.Text);

				if ((int)ViewState["RTOTALPAGECOUNT"] < Int32.Parse(txtRnopage.Text))
					ViewState["RPAGENUMBER"] = ViewState["RTOTALPAGECOUNT"];


				if (0 >= Int32.Parse(txtRnopage.Text))
					ViewState["RPAGENUMBER"] = 1;

				if ((int)ViewState["RPAGENUMBER"] == 0)
					ViewState["RPAGENUMBER"] = 1;

				txtRnopage.Text = ViewState["RPAGENUMBER"].ToString();
			}
			if (ViewState["EMPID"] != null)
				BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
			SetRPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void PagerRButtonClick(object sender, CommandEventArgs ce)
	{
		try
		{
			gvRelieverSearch.SelectedIndex = -1;
			gvRelieverSearch.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["RPAGENUMBER"] = (int)ViewState["RPAGENUMBER"] - 1;
			else
				ViewState["RPAGENUMBER"] = (int)ViewState["RPAGENUMBER"] + 1;

			if (ViewState["EMPID"] != null)
				BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
			SetRPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void rblOtherRank_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ViewState["EMPID"] != null)
			BindRelieverData(General.GetNullableInteger(ViewState["EMPID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["RANKID"].ToString()));
		SetRPageNavigator();
	}
    
    public void BindRelieverMatrixData()
    {

        try
        {


            DataTable dt = PhoenixCrewPlanning.CrewPlanRelieverMatrixList(General.GetNullableInteger(ViewState["RANKID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("FLDRANKPOSTED <> " + ViewState["RANKID"].ToString());
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        txt1.Text = dr["FLDRANKEXP"].ToString();
                        txt2.Text = dr["FLDVESSELTYPEEXP"].ToString();
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                gvRelieverMatrix.DataSource = dt;
                gvRelieverMatrix.DataBind();
                
            }
            else
            {                
                ShowNoRecordsFound(dt, gvRelieverMatrix);
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal r;
    decimal r2;
    protected void gvRelieverMatrix_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                if (drv["FLDRANKEXP"].ToString() != "")
                {
                    r = r + decimal.Parse(drv["FLDRANKEXP"].ToString());
                }
                if (drv["FLDVESSELTYPEEXP"].ToString() != "")
                {
                    r2 = r2 + decimal.Parse(drv["FLDVESSELTYPEEXP"].ToString());
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Font.Bold = true;
            e.Row.Cells[3].Font.Bold = true;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = r.ToString();
            e.Row.Cells[4].Font.Bold = true;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].Text = r2.ToString();
        }
    }
}
