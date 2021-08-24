using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewReports;
using Telerik.Web.UI;
public partial class CrewReportEmployeeGrowth : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuEmployeeGrowthReport.AccessRights = this.ViewState;
            MenuEmployeeGrowthReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            
            toolbar.AddFontAwesomeButton("../Crew/CrewReportEmployeeGrowth.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            
            toolbar.AddFontAwesomeButton("../Crew/CrewReportEmployeeGrowth.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuEmployeeStatistics.AccessRights = this.ViewState;
            MenuEmployeeStatistics.MenuList = toolbar.Show();

            if (!IsPostBack)
			{
				ViewState["type"] = "";		
				ViewState["type"] = "3";
				rblGrowth.SelectedValue = "3";
			
				if (Filter.CurrentEmployeeGrowthReportFilter != null)
				{
					NameValueCollection nvc = Filter.CurrentEmployeeGrowthReportFilter;
				
					ucPool.SelectedPool=nvc.Get("poolid");
					ucZone.selectedlist = nvc.Get("zoneid");
					ucFleet.SelectedList = nvc.Get("fleetid");
					ucNationality.SelectedList = nvc.Get("nationality");
					lstRank.selectedlist = nvc.Get("rankid");
					ucFromDate.Text = nvc.Get("fromdate");
					ucToDate.Text = nvc.Get("todate");
                    ucToDate.Text = nvc.Get("principal");                    
                }			
			}
			//  BindData();			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }
	protected void MenuEmployeeGrowthReport_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
		{
		    if (!IsValidTestFilter(ucFromDate.Text, ucToDate.Text))
		    {
		        ucError.Visible = true;
		        return;
		    }
		
			NameValueCollection criteria = new NameValueCollection();
			criteria.Clear();
			criteria.Add("zoneid", ucZone.selectedlist);
			criteria.Add("poolid", ucPool.SelectedPool);
			criteria.Add("fleetid", ucFleet.SelectedList);
			criteria.Add("nationality", ucNationality.SelectedList);
			criteria.Add("rankid", lstRank.selectedlist);
			criteria.Add("fromdate", ucFromDate.Text);
			criteria.Add("todate", ucToDate.Text);
			criteria.Add("batchid", ucBatch.SelectedList);
            criteria.Add("principal", ucPrincipal.SelectedAddress);
			Filter.CurrentEmployeeGrowthReportFilter = criteria;

		    BindData();
            gvEmployeeStatistics.Rebind();
		}

	}

	protected void BindStatisticsData()
	{
		try
		{

			string[] alColumns = {  "FLDTOTALACTIVE","FLDONBOARDCOUNT","FLDONBOARDPERCENTAGE","FLDONLEAVECOUNT"
								 ,"FLDONLEAVEPERCENTAGE","FLDACTIVE","FLDACTIVEPERCENTAGE","FLDACTIVEPERCENTAGE","FLDINACTIVEPERCENTAGE"};
			string[] alCaptions = { "Total Recruited", "OnBoard", " OnBoard %", "OnLeave", " OnLeave %", "Active", " Active %","InActive","InActive %" };

			NameValueCollection nvc = Filter.CurrentEmployeeGrowthReportFilter;

			DataSet ds = new DataSet();

			ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISStatisticsByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
									, (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
									, (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
									, (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
									, (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
									, (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
									, (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
									, null
									, (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
									, 0
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")): null)
									);

           
			General.SetPrintOptions("gvEmployeeStatistics", "Employee Growth Report", alCaptions, alColumns, ds);
            
			gvEmployeeStatistics.DataSource = ds;
            gvEmployeeStatistics.VirtualItemCount = ds.Tables[0].Rows.Count;            
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void ShowStatisticsExcel()
	{
		string[] alColumns = {  "FLDTOTALACTIVE","FLDONBOARDCOUNT","FLDONBOARDPERCENTAGE","FLDONLEAVECOUNT"
								 ,"FLDONLEAVEPERCENTAGE","FLDACTIVE","FLDACTIVEPERCENTAGE","FLDACTIVEPERCENTAGE","FLDINACTIVEPERCENTAGE"};
		string[] alCaptions = { "Total Recruited", "OnBoard", " OnBoard %", "OnLeave", " OnLeave %", "Active", " Active %", "InActive", "InActive %" };

		NameValueCollection nvc = Filter.CurrentEmployeeGrowthReportFilter;

		DataSet ds = new DataSet();

		ds = PhoenixCrewReportEmployeeGrowth.CrewReportMISStatisticsByYear((nvc != null ? General.GetNullableString(nvc.Get("zoneid")) : null)
								, (nvc != null ? General.GetNullableString(nvc.Get("poolid")) : null)
								, (nvc != null ? General.GetNullableString(nvc.Get("fleetid")) : null)
								, (nvc != null ? General.GetNullableString(nvc.Get("nationality")) : null)
								, (nvc != null ? General.GetNullableString(nvc.Get("rankid")) : null)
								, (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
								, (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
								, null
								, (nvc != null ? General.GetNullableString(nvc.Get("batchid")) : null)
								, 0
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("principal")) : null)
								);
		Response.AddHeader("Content-Disposition", "attachment; filename=CrewEmployeeGrowthReport.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Employee Growth Recruitment Report</center></h5></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Recruited From: " + ucFromDate.Text + " To: " + ucToDate.Text + " </center></h5></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>As of Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
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
				Response.Write(dr[alColumns[i]]);
				Response.Write("</td>");
			}
			Response.Write("</tr>");
		}
		Response.Write("</TABLE>");
		Response.End();
	}

	private void BindData()
	{
		BindStatisticsData();
	}

	protected void ChangeBind(object sender, EventArgs e)
	{
		ViewState["type"] = rblGrowth.SelectedValue;
		BindData();
	}

	private void BindListBox(ListBox lstBox, string list)
	{
		foreach (string item in list.Split(','))
		{
			if (item.Trim() != "")
			{
				if (lstBox.Items.FindByValue(item) != null)
					lstBox.Items.FindByValue(item).Selected = true;
			}
		}
	}

	private bool IsValidTestFilter(string testfromdate, string testtodate)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		DateTime resultdate;
	
		if (General.GetNullableDateTime(testfromdate) == null)
		{
			ucError.ErrorMessage = "From Date is required.";
		}

		else if (!string.IsNullOrEmpty(testfromdate))
		{
			if (DateTime.TryParse(testfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
			{
				ucError.ErrorMessage = "Test From Date  should be earlier than current date.";
			}
		}
		if (General.GetNullableDateTime(testtodate) == null)
		{
			ucError.ErrorMessage = "To Date is required.";
		}
		return (!ucError.IsError);
	}
	protected void EmployeeStatistics_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowStatisticsExcel();
			}
			else if (CommandName.ToUpper().Equals("CLEAR"))
			{
				Filter.CurrentEmployeeGrowthReportFilter = null;
				ucPool.SelectedPool = "";
				ucZone.selectedlist = "";
				ucFleet.SelectedList = "";
				ucNationality.SelectedList = "";
				lstRank.SelectedRankValue = "";
				ucFromDate.Text = "";
				ucToDate.Text = "";
				ViewState["type"] = "1";
                //rblGrowth.SelectedValue = ViewState["type"].ToString();
				BindData();
                gvEmployeeStatistics.Rebind();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvEmployeeStatistics_ItemCommand(object sender, GridCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void gvEmployeeStatistics_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkOnboard = (LinkButton)e.Item.FindControl("lnkOnBoard");
            LinkButton lnkOnleave = (LinkButton)e.Item.FindControl("lnkOnLeave");
            LinkButton lnkInActive = (LinkButton)e.Item.FindControl("lnkInActive");
            lnkOnboard.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewEmployeeGrowthList.aspx?type=3&t=1&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&principal=" + General.GetNullableInteger(ucPrincipal.SelectedAddress) + "');return false;");
            lnkOnleave.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewEmployeeGrowthList.aspx?type=3&t=2&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&principal=" + ucToDate.Text + "');return false;");
            lnkInActive.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewEmployeeGrowthList.aspx?type=3&t=3&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&principal=" + ucToDate.Text + "');return false;");
        }
    }    
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		//BindData();
		//SetPageNavigator();
	}

    protected void gvEmployeeStatistics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
