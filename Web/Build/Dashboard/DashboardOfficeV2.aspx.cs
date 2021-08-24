using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardOfficeV2 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["type"] = string.Empty;
        if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            ViewState["type"] = Request.QueryString["type"];
        BtnAccounts.Visible = SessionUtil.CanAccess(this.ViewState, "ACCOUNTS");
        btnCrew.Visible = SessionUtil.CanAccess(this.ViewState, "CREW");
        BtnTech.Visible = SessionUtil.CanAccess(this.ViewState, "TECH");
        BtnHSQEA.Visible = SessionUtil.CanAccess(this.ViewState, "HSQEA");
        lnkVetting.Visible = SessionUtil.CanAccess(this.ViewState, "VETTING");
        lnkPhoenixAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICS");
        lnkWRHAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICSREPORTING");
        lnkAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "ANALYTICS");
        lnkDashboard.Visible = SessionUtil.CanAccess(this.ViewState, "TITLE");
        if (ViewState["type"].ToString().ToLower() == "t")
            liPage2.Visible = SessionUtil.CanAccess(this.ViewState, "TECHPAGE2");
        else
            liPage2.Visible = SessionUtil.CanAccess(this.ViewState, "HSEQAPAGE2");

        if (!IsPostBack)
        {

            LoadMap();
            BindExternalLink();
            if (ViewState["type"].ToString().ToLower() == "t")
            {
                divPMS.Visible = true;
                divPurchase.Visible = true;
                BtnTech.Visible = false;
                divAudit.Visible = false;
                divDeficiencies.Visible = false;
                lnkDashboard.InnerHtml = "Tech<span class=\"icon\"><i class=\"fas fa-filter\"></i></span>";
                //divQMSMOC.Visible = false;
                //divTechMOC.Visible = true;
                //divTraining.Visible = false;
                //divauditdeficiency.Visible = true;
                divTechTask.Visible = true;
                divQMSTask.Visible = false;
                divAudit.Visible = false;
                divTechAudit.Visible = true;
                divvess.Visible = true;
				//divProposal.Visible = true;
                //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                //{
                //    divProposal.Visible = false;
                //    divDeficiency.Style.Add("Height", "550px");
                //}
                idIncidents.Visible = false;
                idInternalAudit.Visible = false;
                idExternalAudit.Visible = false;
                idTask.Visible = false;
                idRiskAssessment.Visible = false;             
            }
            else
            {
                divPMS.Visible = false;
                divPurchase.Visible = false;
                BtnHSQEA.Visible = false;
                divAudit.Visible = true;
                divDeficiencies.Visible = true;
                lnkDashboard.InnerHtml = "HSEQA<span class=\"icon\"><i class=\"fas fa-filter\"></i></span>";
                //divQMSMOC.Visible = true;
                //divTechMOC.Visible = false;
                //divTraining.Visible = true;
                //divauditdeficiency.Visible = false;
                divTechTask.Visible = false;
                divQMSTask.Visible = true;
                gvCertificateSchedule.Height = 440;
                divAudit.Visible = true;
                divTechAudit.Visible = false;
                divvess.Visible = false;
                devvessel.Style.Add("Height", "991px");
				//divProposal.Visible = false;
                //idIncidents.Visible = true;
                //idInternalAudit.Visible = true;
                //idExternalAudit.Visible = true;
                //idTask.Visible = true;
                //idRiskAssessment.Visible = true;
                idIncidents.Visible = SessionUtil.CanAccess(this.ViewState, "INCIDENTS");
                idInternalAudit.Visible = SessionUtil.CanAccess(this.ViewState, "INTERNALAUDIT");
                idExternalAudit.Visible = SessionUtil.CanAccess(this.ViewState, "EXTERNALAUDIT");
                idTask.Visible = SessionUtil.CanAccess(this.ViewState, "TASK");
                idRiskAssessment.Visible = SessionUtil.CanAccess(this.ViewState, "RISKASSESSMENT");                
            }
        }
    }

    protected void gvVessel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
        DataTable dt = PhoenixDashBoardOffice.DashboardActiveVessel(null, General.GetNullableString(txtvesselsearch.Text)
                                        , sortexpression, sortdirection
                                        , gvVessel.CurrentPageIndex + 1
                                        , gvVessel.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount, 1, ",TEC,FUL,");
        gvVessel.DataSource = dt;
        gvVessel.VirtualItemCount = iRowCount;

    }

    protected void gvVessel_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            if (lnkvessel != null)
            {
                lnkvessel.Visible = SessionUtil.CanAccess(this.ViewState, lnkvessel.CommandName);
                string url;
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {
                    url = "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardOffshoreVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');";
                    gvVessel.MasterTableView.GetColumn("CREWLIST").Visible = false;
                    gvVessel.MasterTableView.GetColumn("CREWEVENT").Visible = false;
                    gvVessel.MasterTableView.GetColumn("CREWRELIEFPLAN").Visible = false; 
                    gvVessel.MasterTableView.GetColumn("FLDVERSION").Visible = false;
                }
                else
                {
                    url = "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');";
                    
                }

                lnkvessel.Attributes.Add("onclick", url);
            }
            LinkButton lnkCrewList = (LinkButton)e.Item.FindControl("lnkcrewlist");
            if (lnkCrewList != null)
            {
                lnkCrewList.Visible = SessionUtil.CanAccess(this.ViewState, lnkCrewList.CommandName);
            }
            LinkButton lnkcrewevent = (LinkButton)e.Item.FindControl("lnkcrewevent");
            if (lnkcrewevent != null)
            {
                lnkcrewevent.Visible = SessionUtil.CanAccess(this.ViewState, lnkcrewevent.CommandName);
            }
            LinkButton lnkReliefPlan = (LinkButton)e.Item.FindControl("lnkReliefPlan");
            if (lnkReliefPlan != null)
            {
                lnkReliefPlan.Visible = SessionUtil.CanAccess(this.ViewState, lnkReliefPlan.CommandName);
            }
            LinkButton lnkDashboard = (LinkButton)e.Item.FindControl("lnkDashboard");
            if (lnkDashboard != null)
            {
                lnkDashboard.Visible = SessionUtil.CanAccess(this.ViewState, lnkDashboard.CommandName);
            }
            LinkButton lnkComponentHierarchy = (LinkButton)e.Item.FindControl("lnkComponentHierarchy");
            if (lnkComponentHierarchy != null)
            {
                lnkComponentHierarchy.Visible = SessionUtil.CanAccess(this.ViewState, lnkComponentHierarchy.CommandName);
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
        if (e.CommandName.ToUpper() == "DASHBOARD")
        {

            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            LinkButton vessel = (LinkButton)e.Item.FindControl("lnkvessel");
            PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);
            //DataSet ds = PhoenixDashBoardOffice.DashboardActiveVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
            //    , null, null
            //                            , gvVessel.CurrentPageIndex + 1
            //                            , gvVessel.PageSize
            //                            , ref iRowCount
            //                            , ref iTotalPageCount, 1);
            PhoenixSecurityContext.CurrentSecurityContext.VesselName = vessel.Text;
            Response.Redirect("DashboardVessel.aspx?from=dashboard");
        }
        if (e.CommandName.ToUpper() == "CREWLIST")
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            //PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);
            //PhoenixSecurityContext.CurrentSecurityContext.VesselName = lnkvessel.Text;

            string url = "";
            if (lnkvessel != null)
            {
                url = String.Format(
                  "javascript:parent.openNewWindow('codehelp1', 'Crew List - " + lnkvessel.Text + "', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + lblvesselid.Text + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }

        }
        if (e.CommandName == "CREWRELIEFPLAN")
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            //PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);
            //PhoenixSecurityContext.CurrentSecurityContext.VesselName = lnkvessel.Text;

            string url = "";
            if (lnkvessel != null)
            {
                url = String.Format(
                   "javascript:top.openNewWindow('event', 'Relief Plan - " + lnkvessel.Text + "', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReliefPlan.aspx?vesselid=" + lblvesselid.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
            gvVessel.Rebind();

        }
        if (e.CommandName == "CREWEVENT")
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            //PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);
            //PhoenixSecurityContext.CurrentSecurityContext.VesselName = lnkvessel.Text;

            string url = "";
            if (lnkvessel != null)
            {
                url = String.Format(
                  "javascript:top.openNewWindow('event', 'Crew Event - " + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "', '" + Session["sitepath"] + "/Crew/CrewPlanEvent.aspx?vesselid=" + lblvesselid.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
            gvVessel.Rebind();
        }
        if (e.CommandName.ToUpper() == "COMPONENTHIERARCHY")
        {
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            PhoenixSecurityContext.CurrentSecurityContext.VesselID = Convert.ToInt32(lblvesselid.Text);

            PhoenixSecurityContext.CurrentSecurityContext.VesselName = lnkvessel.Text;

            if (lnkvessel != null)
            {
                string url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Component Hierarchy - " + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "', '" + Session["sitepath"] + "/Inventory/InventoryComponentTreeDashboard.aspx');");

                //lnkvessel.Attributes.Add("onclick", url);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }

        }

		if (e.CommandName.ToUpper() == "OWNERSREPORT")
        {

            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            LinkButton vessel = (LinkButton)e.Item.FindControl("lnkvessel");

            Response.Redirect("../Owners/OwnerReportMasterPage.aspx?VESSELID="+ lblvesselid.Text.Trim());
        }
    }

    protected void txtvesselsearch_TextChanged(object sender, EventArgs e)
    {

        BindActiveVessel();
        LoadMap();
        gvVessel.Rebind();
    }
    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument.ToUpper() == "VESSEL")
        {
            gvVessel.Rebind();
        }
        if (e.Argument.ToUpper() == "INCIDENTS")
        {
            ifMoreInfoQuality.Attributes["src"] = "../Inspection/InspectionDashboardOfficeIncidentListExtn.aspx?STATUS=S1";
        }
        if (e.Argument.ToUpper() == "TASK")
        {
            ifMoreInfoQuality.Attributes["src"] = "..//Inspection/InspectionOfficeDashboardShipBoardTasksList.aspx?TASK=VIR&STATUS=";
        }
        if (e.Argument.ToUpper() == "RISKASSESSMENT")
        {
            ifMoreInfoQuality.Attributes["src"] = "..//Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx";
        }
        if (e.Argument.ToUpper() == "EXTERNALAUDIT")
        {
            ifMoreInfoQuality.Attributes["src"] = "..//Inspection/InspectionDashBoardExternalPSCAuditRecord.aspx";
        }
        if (e.Argument.ToUpper() == "INTERNALAUDIT")
        {
            ifMoreInfoQuality.Attributes["src"] = "..//Inspection/InspectionDashBoardInternalAICL1AuditRecord.aspx";
        }
        if (e.Argument.ToUpper() == "DASHBOARD2")
        {
            ifMoreInfoDahboard2.Attributes["src"] = "../Dashboard/DashboardOfficeV2Extn.aspx?type=" + ViewState["type"];
        }
    }

    protected void btnCrew_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2Crew.aspx", false);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCertificateSchedule.Rebind();
        //gvOpenReports.Rebind();
        //gvNonRoutineRA.Rebind();
        //gvCrewComplaints.Rebind();
        //gvUnSafeAct.Rebind();
        //gvIncident.Rebind();
        //gvMachineryDamage.Rebind();
        //gvDrills.Rebind();
        LoadMap();
        gvVessel.Rebind();
        gvVPRSReports.Rebind();
        if (ViewState["type"].ToString().ToLower() == "t")
        {
            divPMS.Visible = true;
            divPurchase.Visible = true;
            BtnTech.Visible = false;
            divAudit.Visible = false;
            divDeficiencies.Visible = false;
            GvPMS.Rebind();
            GvPurchase.Rebind();
            GvPurchase1.Rebind();
            lnkDashboard.InnerHtml = "Tech<span class=\"icon\"><i class=\"fas fa-filter\"></i></span>";
            //divQMSMOC.Visible = false;
            //divTechMOC.Visible = true;
            //divTraining.Visible = false;
            //divauditdeficiency.Visible = true;
            gvTechTask.Rebind();
            gvTechAudit.Rebind();
            //gvMOC.Rebind();
            //gvDeficiencyStatus.Rebind();
            divTechTask.Visible = true;
            divQMSTask.Visible = false;
            divAudit.Visible = false;
            divTechAudit.Visible = true;
            idIncidents.Visible = false;
            idInternalAudit.Visible = false;
            idExternalAudit.Visible = false;
            idTask.Visible = false;
            idRiskAssessment.Visible = false;
        }
        else
        {

            BtnHSQEA.Visible = false;
            divPMS.Visible = false;
            divPurchase.Visible = false;
            divAudit.Visible = true;
            divDeficiencies.Visible = true;
            gvDeficiency.Rebind();
            lnkDashboard.InnerHtml = "HSEQA<span class=\"icon\"><i class=\"fas fa-filter\"></i></span>";
            //divQMSMOC.Visible = true;
            //divTechMOC.Visible = false;
            //divTraining.Visible = true;
            //divauditdeficiency.Visible = false;
            //gvQMSMOC.Rebind();
            //gvQMSTraining.Rebind();
            gvTask.Rebind();
            gvInspectionStatus.Rebind();
            divTechTask.Visible = false;
            divQMSTask.Visible = true;
            divAudit.Visible = true;
            divTechAudit.Visible = false;
            //idIncidents.Visible = true; 
            //idInternalAudit.Visible = true;
            //idExternalAudit.Visible = true;
            //idTask.Visible = true;
            //idRiskAssessment.Visible = true;
            idIncidents.Visible = SessionUtil.CanAccess(this.ViewState, "INCIDENTS");
            idInternalAudit.Visible = SessionUtil.CanAccess(this.ViewState, "INTERNALAUDIT");
            idExternalAudit.Visible = SessionUtil.CanAccess(this.ViewState, "EXTERNALAUDIT");
            idTask.Visible = SessionUtil.CanAccess(this.ViewState, "TASK");
            idRiskAssessment.Visible = SessionUtil.CanAccess(this.ViewState, "RISKASSESSMENT");
        }
    }

    protected void BtnAccounts_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2Accounts.aspx", true);
    }

    protected void BtnTech_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx?type=t", true);
    }

    protected void BtnHSQEA_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx", true);
    }
    private void LoadMap()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, General.GetNullableString(txtvesselsearch.Text), 1);
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

    protected void gvTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardTaskSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvTask.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvTask.Columns[1].Visible = false;

            if (dt.Rows[0]["FLD30VISIBLE"].ToString().Equals("0"))
                gvTask.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDPENVISIBLE"].ToString().Equals("0"))
                gvTask.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDEXTVISIBLE"].ToString().Equals("0"))
                gvTask.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDPSAVISIBLE"].ToString().Equals("0"))
                gvTask.Columns[5].Visible = false;
        }
    }

    protected void gvTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk2ryPndg = (LinkButton)e.Item.FindControl("lnk2ryPndg");
            LinkButton lnkExtnReq = (LinkButton)e.Item.FindControl("lnkExtnReq");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk30Days");
            LinkButton lnkPndgClosure = (LinkButton)e.Item.FindControl("lnkPndgClosure");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl2ryPndg = (RadLabel)e.Item.FindControl("lbl2ryPndg");
            RadLabel lblExtnReq = (RadLabel)e.Item.FindControl("lblExtnReq");
            RadLabel lblPndgClosure = (RadLabel)e.Item.FindControl("lblPndgClosure");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl30Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkOverdue != null)
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");

                if (!string.IsNullOrEmpty(drv["FLDOVERDUECOUNT"].ToString()) && int.Parse(drv["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if (lnk2ryPndg != null)
            {
                lnk2ryPndg.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Secondary Approval','" + lbl2ryPndg.Text + "'); return false;");
            }

            if (lnkExtnReq != null)
            {
                lnkExtnReq.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Extension Requested','" + lblExtnReq.Text + "'); return false;");
            }

            if (lnk30Days != null)
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 15 Days','" + lbl30Daysurl.Text + "'); return false;");
            }

            if (lnkPndgClosure != null)
            {
                lnkPndgClosure.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Closure','" + lblPndgClosure.Text + "'); return false;");
            }
        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["type"].ToString().ToLower() != "t")
        {
            DataTable dt = PhoenixDashboardQuality.DashboardDeficiencySummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            gvDeficiency.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
                    gvDeficiency.Columns[2].Visible = false;

                if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
                    gvDeficiency.Columns[1].Visible = false;
            }
        }

    }
    protected void gvVPRSReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["type"].ToString().ToLower() == "t")
        {
            DataTable dt = PhoenixDashboardCommercialPerformance.DashboardReportsOverDue();
            gvVPRSReports.DataSource = dt;
        }
    }
    protected void gvVPRSReports_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkoverdue = (LinkButton)e.Item.FindControl("lnkoverdue");
            LinkButton lnkreview = (LinkButton)e.Item.FindControl("lnkreview");


            RadLabel lbloverdue = (RadLabel)e.Item.FindControl("lbloverdue");
            RadLabel lblreview = (RadLabel)e.Item.FindControl("lblreview");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkoverdue != null)
            {
                if (lblmeasure.Text == "Arrival Reports" || lblmeasure.Text == "Departure Reports" || lblmeasure.Text == "Shifting Reports")
                    lnkoverdue.Text = "0";
                else
                    lnkoverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lbloverdue.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOVERDUECOUNT"].ToString()) && int.Parse(drv["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkoverdue.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkreview != null)
            {
                if (lblmeasure.Text == "Monthly Reports" || lblmeasure.Text == "Quarterly Reports")
                    lnkreview.Text = "0";
                else if (lblmeasure.Text == "Shifting Reports")
                {
                    lnkreview.Text = "-";
                    lnkreview.Attributes.Add("onclick", "return false;");
                    lnkreview.ForeColor= System.Drawing.Color.Black;
                }
                else
                    lnkreview.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblreview.Text + "'); return false;");

                
            }
        }
    }
    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");
            LinkButton lnkOffice = (LinkButton)e.Item.FindControl("lnkOffice");


            RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
            RadLabel lblOffice = (RadLabel)e.Item.FindControl("lblOffice");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkShip != null)
            {
                lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
                        lnkShip.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (lnkOffice != null)
            {
                lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
                        lnkOffice.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    //protected void gvMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardMOCSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvMOC.DataSource = dt;

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
    //            gvMOC.Columns[2].Visible = false;

    //        if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
    //            gvMOC.Columns[1].Visible = false;
    //    }
    //}

    //protected void gvMOC_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");
    //        LinkButton lnkOffice = (LinkButton)e.Item.FindControl("lnkOffice");


    //        RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
    //        RadLabel lblOffice = (RadLabel)e.Item.FindControl("lblOffice");
    //        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

    //        if (lnkShip != null)
    //        {
    //            lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
    //            {
    //                if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
    //                    lnkShip.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }

    //        if (lnkOffice != null)
    //        {
    //            lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
    //            {
    //                if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
    //                    lnkOffice.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    //protected void gvOpenReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardOpenReportSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvOpenReports.DataSource = dt;
    //}
    //protected void gvOpenReports_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
    //        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
    //        LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

    //        if (lnkcount != null)
    //        {
    //            lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Open Reports - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
    //        }
    //    }
    //}

    //protected void gvCrewComplaints_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboarCrewComplaintSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvCrewComplaints.DataSource = dt;
    //}

    //protected void gvCrewComplaints_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
    //        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
    //        LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

    //        if (lnkcount != null)
    //        {
    //            lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Crew Complaints - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
    //        }
    //    }
    //}

    //protected void gvUnSafeAct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardUnsafeActSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvUnSafeAct.DataSource = dt;
    //}

    //protected void gvUnSafeAct_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
    //        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
    //        LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

    //        if (lnkcount != null)
    //        {
    //            lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Unsafe Acts / Conditions - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
    //        }
    //    }
    //}

    //protected void gvMachineryDamage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardMachineryDamageSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvMachineryDamage.DataSource = dt;
    //}

    //protected void gvMachineryDamage_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
    //        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
    //        LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

    //        if (lnkcount != null)
    //        {
    //            lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Machinery Damage / Failure - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
    //            if (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
    //            {
    //                if (int.Parse(drv["FLDCOUNT"].ToString()) > 0)
    //                    lnkcount.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    //protected void gvNonRoutineRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardNonRoutineRASummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvNonRoutineRA.DataSource = dt;
    //}

    //protected void gvNonRoutineRA_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        LinkButton lnkOld = (LinkButton)e.Item.FindControl("lnkOld");
    //        LinkButton lnkNew = (LinkButton)e.Item.FindControl("lnkNew");


    //        RadLabel lblOld = (RadLabel)e.Item.FindControl("lblOld");
    //        RadLabel lblNew = (RadLabel)e.Item.FindControl("lblNew");
    //        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblname");

    //        if (lnkOld != null)
    //        {
    //            lnkOld.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Old - Non Routine RA - " + lblmeasure.Text + "','" + lblOld.Text + "'); return false;");
    //        }

    //        if (lnkNew != null)
    //        {
    //            lnkNew.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','New - Non Routine RA - " + lblmeasure.Text + "','" + lblNew.Text + "'); return false;");
                
    //            if (!string.IsNullOrEmpty(drv["FLDNEWCOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("expired"))
    //            {
    //                if (int.Parse(drv["FLDNEWCOUNT"].ToString()) > 0)
    //                    lnkNew.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    //protected void gvIncident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardIncidentSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvIncident.DataSource = dt;
    //}

    //protected void gvIncident_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
    //        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
    //        LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

    //        if (lnkcount != null)
    //        {
    //            lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Accident and Near Miss - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
    //            if (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
    //            {
    //                if (int.Parse(drv["FLDCOUNT"].ToString()) > 0)
    //                    lnkcount.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    protected void gvInspectionStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardInspectionStatusSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvInspectionStatus.DataSource = dt;

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["FLD60VISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[1].Visible = false;

            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDCMPVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDREVOVDVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDREVVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[5].Visible = false;

            if (dt.Rows[0]["FLDCLDOVDVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[6].Visible = false;
        }
    }

    protected void gvInspectionStatus_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnk60count = (LinkButton)e.Item.FindControl("lnk60count");
            LinkButton lnkOverduecount = (LinkButton)e.Item.FindControl("lnkOverduecount");
            LinkButton lnkCompleted = (LinkButton)e.Item.FindControl("lnkCompleted");
            LinkButton lnkReviewOverduecount = (LinkButton)e.Item.FindControl("lnkReviewOverduecount");
            LinkButton lnkReviewedcount = (LinkButton)e.Item.FindControl("lnkReviewedcount");
            LinkButton lnkClosureOverduecount = (LinkButton)e.Item.FindControl("lnkClosureOverduecount");

            RadLabel lbl60url = (RadLabel)e.Item.FindControl("lbl60url");
            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lblCompletedurl = (RadLabel)e.Item.FindControl("lblCompletedurl");
            RadLabel lblReviewOverdueurl = (RadLabel)e.Item.FindControl("lblReviewOverdueurl");
            RadLabel lblReviewedurl = (RadLabel)e.Item.FindControl("lblReviewedurl");
            RadLabel lblClosureOverdueurl = (RadLabel)e.Item.FindControl("lblClosureOverdueurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnk60count != null)
            {
                lnk60count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60url.Text + "'); return false;");
            }

            if (lnkOverduecount != null)
            {
                lnkOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");

                if (!string.IsNullOrEmpty(drv["FLDOVERCOUNT"].ToString()) && int.Parse(drv["FLDOVERCOUNT"].ToString()) > 0)
                    lnkOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkCompleted != null)
            {
                lnkCompleted.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Completed','" + lblCompletedurl.Text + "'); return false;");
            }

            if (lnkReviewOverduecount != null)
            {
                lnkReviewOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Review Overdue','" + lblReviewOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDREVOVDCOUNT"].ToString()) && int.Parse(drv["FLDREVOVDCOUNT"].ToString()) > 0)
                    lnkReviewOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkReviewedcount != null)
            {
                lnkReviewedcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Reviewed','" + lblReviewedurl.Text + "'); return false;");
            }

            if (lnkClosureOverduecount != null)
            {
                lnkClosureOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Closure Overdue','" + lblClosureOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDCLDOVDCOUNT"].ToString()) && int.Parse(drv["FLDCLDOVDCOUNT"].ToString()) > 0)
                    lnkClosureOverduecount.ForeColor = System.Drawing.Color.Red;
            }


        }
    }

    //protected void gvDeficiencyStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardDeficiencySummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvDeficiencyStatus.DataSource = dt;

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
    //            gvDeficiencyStatus.Columns[2].Visible = false;

    //        if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
    //            gvDeficiencyStatus.Columns[1].Visible = false;
    //    }
    //}

    //protected void gvDeficiencyStatus_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");
    //        LinkButton lnkOffice = (LinkButton)e.Item.FindControl("lnkOffice");


    //        RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
    //        RadLabel lblOffice = (RadLabel)e.Item.FindControl("lblOffice");
    //        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

    //        if (lnkShip != null)
    //        {
    //            lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
    //            {
    //                if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
    //                    lnkShip.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }

    //        if (lnkOffice != null)
    //        {
    //            lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
    //            {
    //                if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
    //                    lnkOffice.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    //protected void gvDrills_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixInspectionDrillSchedule.Drilldashboardoverduelist(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //    gvDrills.DataSource = dt;

    //}

    //protected void gvDrills_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        GridDataItem item = e.Item as GridDataItem;

    //        LinkButton drillname = (LinkButton)item.FindControl("Drilloverdueanchor");

    //        drillname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Drill Due Across Vessels','Inspection/InspectionDrillsvsVesselList.aspx?drillid=" + DataBinder.Eval(e.Item.DataItem, "FLDDRILLID") + "&i=-1" + "&j=-1500&a=d" + "&type=" + DataBinder.Eval(e.Item.DataItem, "FLDTYPE") + "');return false");

    //        if (int.Parse(drv["FLDOVERDUE"].ToString()) > 0)
    //            drillname.ForeColor = System.Drawing.Color.Red;
    //    }


    //}

    protected void GvPurchase_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["type"].ToString().ToLower() == "t")
        {
            RadGrid grid = (RadGrid)sender;
            DataTable dt = PhoenixDashboardTechnical.DashboardPurchase();
            var halfDT = dt.Copy();

            var lastRowIndex = halfDT.Rows.Count - 1;
            var halfwayIndex = halfDT.Rows.Count / 2 - 1;
            if (grid.ClientID.ToLower() == "gvpurchase")
            {
                for (int i = lastRowIndex; i > halfwayIndex; i--)
                {
                    halfDT.Rows.RemoveAt(i);
                }
                halfDT.AcceptChanges();
            }
            else
            {
                for (int i = halfwayIndex; i > -1; i--)
                {
                    halfDT.Rows.RemoveAt(i);
                }
                halfDT.AcceptChanges();
            }
            grid.DataSource = halfDT;
        }
    }

    protected void GvPurchase_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;
            LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

            if (cnt != null)
            {
                string querystring = "?code=" + drv["FLDMEASURECODE"].ToString();
                if(drv["FLDURL"].ToString() != string.Empty)
                {
                    string link = drv["FLDURL"].ToString();
                    int index = link.IndexOf('?');
                    if (index > -1)
                    {
                        querystring = querystring.Replace("?", "&");
                    }
                    cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURE"].ToString() + "','" + link + querystring + "'); return false;";
                }
                
            }
            else
            {
                cnt.Enabled = false;
                cnt.Attributes["style"] = "color: black";
                cnt.Text = "-";
            }
        }
    }
    protected void GvPMS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["type"].ToString().ToLower() == "t")
        {
            RadGrid grid = (RadGrid)sender;
            DataTable dt = PhoenixDashboardTechnical.DashboardPMS();
            grid.DataSource = dt;
        }
    }

    protected void GvPMS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;
            LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

            if (drv["FLDURL"].ToString() != string.Empty && cnt != null)
            {
                string querystring = "?code=" + drv["FLDMEASURECODE"].ToString();
                string link = drv["FLDURL"].ToString();
                int index = link.IndexOf('?');
                if (index > -1)
                {
                    querystring = querystring.Replace("?", "&");
                }
                //cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURE"].ToString() + "','" + link + querystring + "'); return false;";

                if (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(drv["FLDCOUNT"].ToString()) > 0)
                        cnt.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                cnt.Enabled = false;
                cnt.Attributes["style"] = "color: black";
            }
        }
    }
    protected void GvPMS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper() == "MEASURE")
        {
            string code = ((RadLabel)item.FindControl("lblMeasureCode")).Text;
            string link = ((RadLabel)item.FindControl("lblURL")).Text;
            if (code == "TECH-PMS-OODWO")
            {
                link = "Dashboard/DashboardTechnicalJobPlanned.aspx";
            }
            string querystring = "?code=" + code;            
            string measure = ((RadLabel)item.FindControl("lblMeasure")).Text;
            if (link != string.Empty)
            {
                int index = link.IndexOf('?');
                if (index > -1)
                {
                    querystring = querystring.Replace("?", "&");
                }
                Filter.CurrentWorkOrderFilter = null;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", "javascript: top.openNewWindow('detail','" + measure + "','" + link + querystring + "&title="+ measure + "');", true);
            }
        }
    }
    //protected void gvQMSMOC_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");
    //        LinkButton lnkOffice = (LinkButton)e.Item.FindControl("lnkOffice");


    //        RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
    //        RadLabel lblOffice = (RadLabel)e.Item.FindControl("lblOffice");
    //        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

    //        if (lnkShip != null)
    //        {
    //            lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
    //            {
    //                if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
    //                    lnkShip.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }

    //        if (lnkOffice != null)
    //        {
    //            lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
    //            if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
    //            {
    //                if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
    //                    lnkOffice.ForeColor = System.Drawing.Color.Red;
    //            }
    //        }
    //    }
    //}

    //protected void gvQMSMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixDashboardQuality.DashboardMOCSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
    //    gvQMSMOC.DataSource = dt;

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
    //            gvQMSMOC.Columns[2].Visible = false;

    //        if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
    //            gvQMSMOC.Columns[1].Visible = false;
    //    }
    //}

    //protected void gvQMSTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    DataTable dt = PhoenixInspectionTrainingSummary.OverdueTrainingsDashboardSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //    gvQMSTraining.DataSource = dt;
    //}

    //protected void gvQMSTraining_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if (e.Item is GridDataItem)
    //    {
    //        GridDataItem item = e.Item as GridDataItem;
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        LinkButton trainingname = (LinkButton)item.FindControl("trainingoverdueanchor");

    //        trainingname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Training Due Across Vessels','Inspection/InspectionTrainingvsVessellist.aspx?trainingid=" + DataBinder.Eval(e.Item.DataItem, "FLDTRAININGID") + "&i=-1" + "&j=-1500&a=d" + "&type=" + DataBinder.Eval(e.Item.DataItem, "FLDTYPE") + "');return false");
    //        if (int.Parse(drv["FLDOVERDUE"].ToString()) > 0)
    //            trainingname.ForeColor = System.Drawing.Color.Red;
    //    }
    //}

    protected void gvCertificateSchedule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk60Days = (LinkButton)e.Item.FindControl("lnk60Days");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk30Days");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl60Daysurl = (RadLabel)e.Item.FindControl("lbl60Daysurl");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl30Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if ((lnkOverdue != null) && (lnkOverdue.Text != "") && (lnkOverdue.Text != null))
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - Overdue'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOVERDUECOUNT"].ToString()) && int.Parse(drv["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if ((lnk30Days != null) && (lnk30Days.Text != "") && (lnk30Days.Text != null))
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 15 Days','" + lbl30Daysurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 15 Days'); return false;");
            }

            if ((lnk60Days != null) && (lnk60Days.Text != "") && (lnk60Days.Text != null))
            {
                lnk60Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60Daysurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 60 Days'); return false;");
            }

        }
    }

    protected void gvCertificateSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardCertificateScheduleSummary();
        gvCertificateSchedule.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[1].Visible = false;

            if (dt.Rows[0]["FLD30VISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[2].Visible = false;

            if (dt.Rows[0]["FLD60VISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[3].Visible = false;

        }
    }

    protected void gvTechTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardTaskSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvTechTask.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[1].Visible = false;

            if (dt.Rows[0]["FLD30VISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDPENVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDEXTVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDPSAVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[5].Visible = false;
        }
    }

    protected void gvTechTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk2ryPndg = (LinkButton)e.Item.FindControl("lnk2ryPndg");
            LinkButton lnkExtnReq = (LinkButton)e.Item.FindControl("lnkExtnReq");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk30Days");
            LinkButton lnkPndgClosure = (LinkButton)e.Item.FindControl("lnkPndgClosure");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl2ryPndg = (RadLabel)e.Item.FindControl("lbl2ryPndg");
            RadLabel lblExtnReq = (RadLabel)e.Item.FindControl("lblExtnReq");
            RadLabel lblPndgClosure = (RadLabel)e.Item.FindControl("lblPndgClosure");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl30Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkOverdue != null)
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOVERDUECOUNT"].ToString()) && int.Parse(drv["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if (lnk2ryPndg != null)
            {
                lnk2ryPndg.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Secondary Approval','" + lbl2ryPndg.Text + "'); return false;");
            }

            if (lnkExtnReq != null)
            {
                lnkExtnReq.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Extension Requested','" + lblExtnReq.Text + "'); return false;");
            }

            if (lnk30Days != null)
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 15 Days','" + lbl30Daysurl.Text + "'); return false;");
            }

            if (lnkPndgClosure != null)
            {
                lnkPndgClosure.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Closure','" + lblPndgClosure.Text + "'); return false;");
            }
        }

    }

    protected void gvTechAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardInspectionStatusSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvTechAudit.DataSource = dt;

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["FLD60VISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[1].Visible = false;

            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDCMPVISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDREVOVDVISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDREVVISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[5].Visible = false;

            if (dt.Rows[0]["FLDCLDOVDVISIBLE"].ToString().Equals("0"))
                gvTechAudit.Columns[6].Visible = false;
        }
    }

    protected void gvTechAudit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnk60count = (LinkButton)e.Item.FindControl("lnk60count");
            LinkButton lnkOverduecount = (LinkButton)e.Item.FindControl("lnkOverduecount");
            LinkButton lnkCompleted = (LinkButton)e.Item.FindControl("lnkCompleted");
            LinkButton lnkReviewOverduecount = (LinkButton)e.Item.FindControl("lnkReviewOverduecount");
            LinkButton lnkReviewedcount = (LinkButton)e.Item.FindControl("lnkReviewedcount");
            LinkButton lnkClosureOverduecount = (LinkButton)e.Item.FindControl("lnkClosureOverduecount");

            RadLabel lbl60url = (RadLabel)e.Item.FindControl("lbl60url");
            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lblCompletedurl = (RadLabel)e.Item.FindControl("lblCompletedurl");
            RadLabel lblReviewOverdueurl = (RadLabel)e.Item.FindControl("lblReviewOverdueurl");
            RadLabel lblReviewedurl = (RadLabel)e.Item.FindControl("lblReviewedurl");
            RadLabel lblClosureOverdueurl = (RadLabel)e.Item.FindControl("lblClosureOverdueurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnk60count != null)
            {
                lnk60count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60url.Text + "'); return false;");
            }

            if (lnkOverduecount != null)
            {
                lnkOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOVERCOUNT"].ToString()) && int.Parse(drv["FLDOVERCOUNT"].ToString()) > 0)
                    lnkOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkCompleted != null)
            {
                lnkCompleted.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Completed','" + lblCompletedurl.Text + "'); return false;");
            }

            if (lnkReviewOverduecount != null)
            {
                lnkReviewOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Review Overdue','" + lblReviewOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDREVOVDCOUNT"].ToString()) && int.Parse(drv["FLDREVOVDCOUNT"].ToString()) > 0)
                    lnkReviewOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkReviewedcount != null)
            {
                lnkReviewedcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Reviewed','" + lblReviewedurl.Text + "'); return false;");
            }

            if (lnkClosureOverduecount != null)
            {
                lnkClosureOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Closure Overdue','" + lblClosureOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDCLDOVDCOUNT"].ToString()) && int.Parse(drv["FLDCLDOVDCOUNT"].ToString()) > 0)
                    lnkClosureOverduecount.ForeColor = System.Drawing.Color.Red;
            }


        }
    }   
	//protected void gvProposal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
 //   {
 //       RadGrid grid = (RadGrid)sender;

 //       if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode != "OFFSHORE")
 //       {
 //           DataTable dt = PhoenixDashboardCrew.DashboardOfficeCrewTechByRankVesselCount("CREWAPP", 0, 0, 0);
 //           gvProposal.DataSource = dt;
 //       }
 //   }

 //   protected void gvProposal_ItemDataBound(object sender, GridItemEventArgs e)
 //   {
 //       if (e.Item is GridDataItem)
 //       {
 //           DataRowView drv = (DataRowView)e.Item.DataItem;
 //           GridDataItem item = e.Item as GridDataItem;
 //           LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

 //           if (drv["FLDURL"].ToString() != string.Empty && cnt != null)
 //           {
 //               string querystring = "?code=" + drv["FLDMEASURECODE"].ToString();
 //               string link = drv["FLDURL"].ToString();
 //               int index = link.IndexOf('?');
 //               if (index > -1)
 //               {
 //                   querystring = querystring.Replace("?", "&");
 //               }
 //               cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURE"].ToString() + "','" + link + querystring + "'); return false;";
 //           }
 //           else
 //           {
 //               cnt.Enabled = false;
 //               cnt.Attributes["style"] = "color: black";
 //               cnt.Text = "-";
 //           }
 //       }
 //   }
 //   protected void gvWRH_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
 //   {
 //       RadGrid grid = (RadGrid)sender;
 //       DataTable dt = PhoenixDashboardTechnical.ListWorkandRestHourtAlert(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
 //       grid.DataSource = dt;
 //   }

 //   protected void gvWRH_ItemDataBound(object sender, GridItemEventArgs e)
 //   {
 //       if (e.Item is GridDataItem)
 //       {
 //           DataRowView drv = (DataRowView)e.Item.DataItem;
 //           LinkButton lnkSeafarerCount = (LinkButton)e.Item.FindControl("lnkSeafarerCount");
 //           LinkButton lnkHODCount = (LinkButton)e.Item.FindControl("lnkHODCount");
 //           LinkButton lblMasterCount = (LinkButton)e.Item.FindControl("lblMasterCount");

 //           RadLabel lnkSeafarerUrl = (RadLabel)e.Item.FindControl("lblOverdueurl");
 //           RadLabel lnkHODUrl = (RadLabel)e.Item.FindControl("lnkHODUrl");
 //           RadLabel lblMasterUrl = (RadLabel)e.Item.FindControl("lblMasterUrl");
 //           RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

 //           //if ((lnkSeafarerCount != null) && (lnkSeafarerCount.Text != "") && (lnkSeafarerCount.Text != null) && drv["FLDSEAFARERURL"].ToString() != string.Empty)
 //           //{
 //           //    lnkSeafarerCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Seafarer','" + drv["FLDSEAFARERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - Overdue'); return false;");
 //           //}
 //           //else
 //           //{
 //           //    lnkSeafarerCount.Enabled = false;
 //           //    lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
 //           //}
 //           //if ((lnkHODCount != null) && (lnkHODCount.Text != "") && (lnkHODCount.Text != null) && drv["FLDHODURL"].ToString() != string.Empty)
 //           //{
 //           //    lnkHODCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - HOD','" + drv["FLDHODURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 30 Days'); return false;");
 //           //}
 //           //else
 //           //{
 //           //    lnkHODCount.Enabled = false;
 //           //    lnkHODCount.ForeColor = Color.FromName("#1e395b");
 //           //}
 //           //if ((lblMasterCount != null) && (lblMasterCount.Text != "") && (lblMasterCount.Text != null) && drv["FLDMASTERURL"].ToString() != string.Empty)
 //           //{
 //           //    lblMasterCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Master','" + drv["FLDMASTERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 60 Days'); return false;");
 //           //}
 //           //else
 //           //{
 //           //    lblMasterCount.Enabled = false;
 //           //    lblMasterCount.ForeColor = Color.FromName("#1e395b");
 //           //}
 //           if (drv["FLDSEAFARERCOUNT"].ToString().Equals(""))
 //           {
 //               lnkSeafarerCount.Text = "N/A";
 //               lnkSeafarerCount.Enabled = false;
 //               lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
 //           }
 //           if (drv["FLDHODCOUNT"].ToString().Equals(""))
 //           {
 //               lnkHODCount.Text = "N/A";
 //               lnkHODCount.Enabled = false;
 //               lnkHODCount.ForeColor = Color.FromName("#1e395b");
 //           }
 //           if (drv["FLDMASTERCOUNT"].ToString().Equals(""))
 //           {
 //               lblMasterCount.Text = "N/A";
 //               lblMasterCount.Enabled = false;
 //               lblMasterCount.ForeColor = Color.FromName("#1e395b");
 //           }
 //           lnkSeafarerCount.Enabled = false;
 //           lnkHODCount.Enabled = false;
 //           lblMasterCount.Enabled = false;
 //       }
 //   }
}