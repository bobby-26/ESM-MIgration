using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsSeniorOfficersPlannerSearch : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSeniorOfficersPlannerSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSeniorOfficersPlannerSearch.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
        
            if (!IsPostBack)
            {
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
      
            
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                ucBatch.SelectedBatch = "";
                lstSeniorRank.selectedlist = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                lstPool.SelectedPool = "";
                lstZone.SelectedZoneValue = "";
                ShowReport();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ViewState["PAGENUMBER"] = 1;
                    ShowReport();
            }

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
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKCODE", "FLDBATCH", "FLDVESSELCODE", "FLDONSIGNER", "FLDOFFSIGNER", "FLDEXNEW", "FLDPORTDATE", "FLDOWNERBRIEFING", "FLDSPOREBRIEFING", "FLDPARELLELSAILING", "FLDREMARKS" };
        string[] alCaptions = { "Rank", "Batch", "Vessel", "On Signer", "Off Signer", "EX/NEW", "On Signing Port/Date", "Owners Briefing", "S'pore Briefing", "Parallel Sailing", "Remarks" };
        string[] FilterColumns = { "FLDSELECTEDVESTYPE", "FLDSELECTEDBATCH", "FLDSELECTEDVESSEL", "FLDSELECTEDPRINCIPAL" };
        string[] FilterCaptions = { "Vessel Type", "Batch", " Vessel", "Principal" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsSeniorOfficersPlanner.CrewSeniorOfficersPlanner(
                            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                            (lstSeniorRank.selectedlist.ToString())=="Dummy" ? null:General.GetNullableString(lstSeniorRank.selectedlist.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                            lstPool.SelectedPool,
                            lstZone.selectedlist,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewSeniorOfficersPlanner.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Senior Officers Planner</center></h5></td></tr>");
        
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'align='LEFT'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKCODE", "FLDBATCH", "FLDVESSELCODE", "FLDONSIGNER", "FLDOFFSIGNER", "FLDEXNEW", "FLDPORTDATE", "FLDOWNERBRIEFING", "FLDSPOREBRIEFING", "FLDPARELLELSAILING", "FLDREMARKS" };
        string[] alCaptions = { "Rank", "Batch", "Vessel", "On Signer", "Off Signer", "EX/NEW", "On Signing Port/Date", "Owners Briefing", "S'pore Briefing", "Parallel Sailing", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportsSeniorOfficersPlanner.CrewSeniorOfficersPlanner(
                           (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                           (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           (lstSeniorRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(lstSeniorRank.selectedlist.ToString()),
                           (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                           lstPool.SelectedPool,
                           lstZone.selectedlist,
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Senior Officers Planner Search", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

                RadLabel empid = (RadLabel)e.Item.FindControl("lblOnSignerId");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkOnSigner");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

                RadLabel empid1 = (RadLabel)e.Item.FindControl("lblOffSignerId");
                LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkOffSigner");
                lbr1.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid1.Text + "'); return false;");
            }

    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public bool IsValidFilter(string vessellist, string rblselectfrom)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (rblselectfrom.Equals("") || rblselectfrom.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select From";
        }
        return (!ucError.IsError);
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}
