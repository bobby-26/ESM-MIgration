using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web;
using Telerik.Web.UI;
using System.Configuration;
using System.Drawing;

public partial class DashboardOfficeV2Crew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        BtnAccounts.Visible = SessionUtil.CanAccess(this.ViewState, "ACCOUNTS");       
        BtnTech.Visible = SessionUtil.CanAccess(this.ViewState, "TECH");
        btnHSQEA.Visible = SessionUtil.CanAccess(this.ViewState, "HSQEA");
        lnkVetting.Visible = SessionUtil.CanAccess(this.ViewState, "VETTING");
        lnkPhoenixAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICS");
        lnkWRHAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICSREPORTING");
        lnkAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "ANALYTICS");

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["VSORTEXPRESSION"] = null;
            ViewState["VSORTDIRECTION"] = null;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            //gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            LoadExpireDocument();
            LoadMap();
            BindExternalLink();
           
        }

    }
    private void BindExternalLink()
    {
        if (Filter.CurrentLoginToken != null)
        {
            if (ConfigurationManager.AppSettings["AnalyticsUrl"] != null)
            {
                string WrhUrl = ConfigurationManager.AppSettings["AnalyticsUrl"].ToString();

                lnkWRHAnalytics.Attributes["href"] = WrhUrl + "?Token=" + Filter.CurrentLoginToken;
            }
            else
            {
                lnkWRHAnalytics.Visible = false;
            }
            if (ConfigurationManager.AppSettings["PhoenixAnalyticsUrl"] != null)
            {
                string cubeUrl = ConfigurationManager.AppSettings["PhoenixAnalyticsUrl"].ToString();
                lnkPhoenixAnalytics.Attributes["href"] = cubeUrl + "?Token=" + Filter.CurrentLoginToken;
            }
            else
            {
                lnkPhoenixAnalytics.Visible = false;
            }
        }
    }
   

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.ToUpper() == "VESSEL")
        {
            gvVessel.Rebind();
        }
    }

   

    protected void gvVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindActiveVessel();
    }
    public void BindActiveVessel()

    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixDashBoardOffice.DashboardActiveVessel(null, General.GetNullableString(txtvesselsearch.Text)
                                        , sortexpression, sortdirection
                                        , gvVessel.CurrentPageIndex + 1
                                        , gvVessel.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount, 2);
        gvVessel.DataSource = ds.Tables[0];
        gvVessel.VirtualItemCount = iRowCount;

    }

    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            if (lnkvessel != null)
            {
                string url = "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');";
                lnkvessel.Attributes.Add("onclick", url);
            }
            LinkButton lnkCertificates = (LinkButton)e.Item.FindControl("lnkCertificates");
            if (lnkCertificates != null)
            {
                lnkCertificates.Visible = SessionUtil.CanAccess(this.ViewState, lnkCertificates.CommandName);
                lnkCertificates.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Certificates','PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx?vesselId=" + lblvesselid.Text + "'); return false;");
            }

            LinkButton lnkownerreport = (LinkButton)e.Item.FindControl("lnkownerreport");
            if (lnkownerreport != null)
            {
                lnkownerreport.Visible = SessionUtil.CanAccess(this.ViewState, lnkownerreport.CommandName);
            }
        }
    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "DASHBOARD")
        {

            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            LinkButton vessel = (LinkButton)e.Item.FindControl("lnkvessel");
            PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);
            //DataSet ds = PhoenixDashBoardOffice.DashboardActiveVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
            //    , null, null
            //                            , gvVessel.CurrentPageIndex + 1
            //                            , gvVessel.PageSize
            //                            , ref iRowCount
            //                            , ref iTotalPageCount);
            PhoenixSecurityContext.CurrentSecurityContext.VesselName = vessel.Text;
            Response.Redirect("DashboardVessel.aspx?from=dashboard");
        }
        if (e.CommandName == "CREWLIST")
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            if (lnkvessel != null)
            {
                string url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Crew List - " + lnkvessel.Text + "', '" + Session["sitepath"] + "/Crew/CrewListSimple.aspx?vslid=" + lblvesselid.Text + "');");

                //lnkvessel.Attributes.Add("onclick", url);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }

        }

        if (e.CommandName.ToUpper() == "OWNERSREPORT")
        {
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            LinkButton vessel = (LinkButton)e.Item.FindControl("lnkvessel");

            Response.Redirect("../Owners/OwnerReportMasterPage.aspx?VESSELID=" + lblvesselid.Text.Trim());
        }
    }

    protected void txtvesselsearch_TextChanged(object sender, EventArgs e)
    {

        BindActiveVessel();
        LoadMap();
        gvVessel.Rebind();
    }

    protected void BtnAccounts_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2Accounts.aspx", true);
    }
    protected void btnHSQEA_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx", true);
    }
    protected void BtnTech_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx?type=t", true);
    }
   

    private void LoadExpireDocument()
    {
        DataTable dt = PhoenixDashboardCrew.CrewAssessmentStatus();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Control r = Page.FindControl("BtnAssessment" + dr["FLDASSESMENTSTATUS"].ToString());
                if (r != null)
                {
                    HtmlButton btn = (HtmlButton)r;
                    btn.InnerHtml = dr["FLDCOUNT"].ToString();
                    btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewAssessmentStatusDetails.aspx?intstatus=" + dr["FLDASSESMENTSTATUS"].ToString() + "')");
                }
            }
        }
        else
        {
            Control r = Page.FindControl("BtnAssessmentCP");
            HtmlButton btn = (HtmlButton)r;
            btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewAssessmentStatusDetails.aspx?intstatus=CP')");

            Control r1 = Page.FindControl("BtnAssessmentTP");
            HtmlButton btn1 = (HtmlButton)r1;
            btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewAssessmentStatusDetails.aspx?intstatus=TP')");

            Control r2 = Page.FindControl("BtnAssessmentEP");
            HtmlButton btn2 = (HtmlButton)r2;
            btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewAssessmentStatusDetails.aspx?intstatus=EP')");

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        LoadMap();
        gvVessel.Rebind();
        gvCrewSummary.Rebind();
        GvCrewVslSummary.Rebind();

    }
    private void LoadMap()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, General.GetNullableString(txtvesselsearch.Text), 2);
        RadMap1.DataSource = ds;
        RadMap1.DataBind();
    }
    private static string TOOLTIP_TEMPLATE = @"
            <div class=""leftCol"">
                <div class=""vessel"">{0}</div>
                <div>Date: {1}</div>
                <div>Course: {3}</div>
                <div>Wind Direction / Force: {4} / {5}</div>
                <div>Speed: {6}</div>
                <div>ETA: {7}</div>
                <div class=""location"">Location: {2}</div>
            </div>
            ";
    protected void RadMap1_ItemDataBound(object sender, Telerik.Web.UI.Map.MapItemDataBoundEventArgs e)
    {
        MapMarker marker = e.Item as MapMarker;
        if (marker != null)
        {
            DataRowView item = e.DataItem as DataRowView;
            string vessel = item.Row["FLDVESSELNAME"] as string;
            string imo = item.Row["FLDIMONUMBER"] as string;
            string lat = item.Row["FLDLATITUDE"] as string;
            string log = item.Row["FLDLONGITUDE"] as string;
            string date = item.Row["FLDNOONREPORTDATE"].ToString();
            string course = item.Row["FLDCOURSE"].ToString();
            string windforce = item.Row["FLDWINDFORCE"].ToString();
            string winddirection = item.Row["FLDWINDDIRECTION"] as string;
            string eta = item.Row["FLDETA"].ToString();
            string logspeed = item.Row["FLDLOGSPEED"].ToString();
            marker.TooltipSettings.Content = String.Format(TOOLTIP_TEMPLATE, vessel + " (" + imo + ")", General.GetDateTimeToString(date), lat + " , " + log, course, winddirection, windforce, logspeed, General.GetDateTimeToString(eta));
        }
    }
    protected void gvCrewSummary_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        DataTable dt = PhoenixDashboardCrew.DashboardOfficeCrewByGroupRank("CREWV2");
        gvCrewSummary.DataSource = dt;
    }

    protected void gvCrewSummary_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
            if (itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                bool isNumeric = true;
                if (dr.Length > 0)
                {
                    isNumeric = (dr[0]["FLDISNUMERIC"].ToString() == "1" ? true : false);
                }
                if (cell.Text.Trim() == string.Empty)
                {
                    //cell.Text = cell.DataItem.ToString();
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: top.openNewWindow('color', 'Color', 'Dashboard/DashboardGroupRankKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                           + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: top.openNewWindow('chart', 'Chart', 'Dashboard/DashboardV2GroupRankChart.aspx?mod=CREWV2&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>"
                                               + "</a>" : String.Empty) + cell.DataItem.ToString();
                }
            }
        }
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataTable dt = (DataTable)grid.DataSource;
                string code = string.Empty;
                string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
                string Rank = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
                //if (row.IndexOf('~') > -1)
                //{
                //    string[] arr = row.Split('~');
                //    code = arr[0].Trim();
                //    measure = arr[1].Trim();
                //}
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND RankName ='" + Rank + "'");

                //string employees = string.Empty;
                foreach (DataRow d in dr)
                {
                    string text = cell.Text;

                    //Font font = new Font("open sans", 13F);
                    //System.Drawing.Image fakeImage = new Bitmap(1, 1); //As we cannot use CreateGraphics() in a class library, so the fake image is used to load the Graphics.
                    //Graphics graphics = Graphics.FromImage(fakeImage);
                    //SizeF size = graphics.MeasureString("text", font);

                    if (d["FLDISNUMERIC"].ToString() != "1" || d["FLDDETAILLINK"].ToString().Trim().Equals("") || d["FLDMEASURE"].ToString() == "0")
                    {
                        cell.Text = d["FLDMEASURE"].ToString();
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        //cell.Width =  size.Width;
                    }
                    else
                    {
                        //cell.CssClass = "label";
                        //cell.BackColor =  ColorTranslator.FromHtml("#27727B");
                        string querystring = "?code=" + d["Code"].ToString() + " &Rankname=" + Rank + "&RANKID=" + d["FLDGROUPRANKID"].ToString();
                        string link = d["FLDDETAILLINK"].ToString();
                        int index = link.IndexOf('?');
                        if (index > -1)
                        {
                            querystring = querystring.Replace("?", "&");
                        }
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" href=\"javascript:top.openNewWindow('reliefplan', '" + d["Measure"].ToString() + "', '" + link + querystring + "',false);\" >" + cell.Text + "</a>";
                    }
                }
            }
        }
    }
    protected void GvCrewVslSummary_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;      
        DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTechnicalByVessel("CREWV2",2);
        GvCrewVslSummary.DataSource = dt;
    }

    protected void GvCrewVslSummary_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {

        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
            if (itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                bool isNumeric = true;
                if (dr.Length > 0)
                {
                    isNumeric = (dr[0]["FLDISNUMERIC"].ToString() == "1" ? true : false);
                }
                if (cell.Text.Trim() == string.Empty)
                {
                    //cell.Text = cell.DataItem.ToString();
                    //"<input type=\"image\" class=\"customIcon\" onclick=\"javascript: return Openpopup('codehelp1', '', '../Dashboard/DashboardKPI.aspx?measureid=e7fe7088-2489-e911-b585-06089601e630'); return false;\" src=\"/Phoenix/css/Theme1/images/settings.svg\" alt=\"Color\" style=\"border-width:0px;\">";
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: top.openNewWindow('color', 'Color', 'Dashboard/DashboardKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                           + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript:  top.openNewWindow('chart', 'Chart', 'Dashboard/DashboardV2Chart.aspx?mod=CREWV2&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>"
                                               + "</a>" : String.Empty) + cell.DataItem.ToString();
                }
            }
        }
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataTable dt = (DataTable)grid.DataSource;
                string code = string.Empty;                  
                string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
                string vessel = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
              
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND Vessel='" + vessel + "'");                
                foreach (DataRow d in dr)
                {
                    string text = cell.Text;
                    if (d["FLDISNUMERIC"].ToString() != "1" || d["FLDDETAILLINK"].ToString().Trim().Equals("") || d["FLDMEASURE"].ToString() == "0")
                    { 
                        cell.Text = d["FLDMEASURE"].ToString();
                        cell.HorizontalAlign = HorizontalAlign.Right;
                    }
                    else
                    {
                        //cell.CssClass = "label";
                        //cell.BackColor =  ColorTranslator.FromHtml("#27727B");
                        string querystring = "?code=" + d["Code"].ToString() + " &vslname=" + vessel + "&Vesselid=" + d["FLDVESSELID"].ToString();
                        string link = d["FLDDETAILLINK"].ToString();
                        int index = link.IndexOf('?');
                        if (index > -1)
                        {
                            querystring = querystring.Replace("?", "&");
                        }
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" href=\"javascript:top.openNewWindow('wo', '" + d["Measure"].ToString() + "', '" + link + querystring + "',false);\" >" + cell.Text + "</a>";
                    }
                }
            }
        }
    }   
}