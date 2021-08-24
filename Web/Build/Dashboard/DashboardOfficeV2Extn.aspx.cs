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

public partial class Dashboard_DashboardOfficeV2Extn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["type"] = string.Empty;
        if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            ViewState["type"] = Request.QueryString["type"];
       
        if (!IsPostBack)
        {           
            if (ViewState["type"].ToString().ToLower() == "t")
            {
                
                divQMSMOC.Visible = false;
                divTechMOC.Visible = true;
                divTraining.Visible = false;
                divauditdeficiency.Visible = true;               
				divProposal.Visible = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {
                    divProposal.Visible = false;
                    divDeficiency.Style.Add("Height", "550px");
                }                   
            }
            else
            {               
                divQMSMOC.Visible = true;
                divTechMOC.Visible = false;
                divTraining.Visible = true;
                divauditdeficiency.Visible = false;               
				divProposal.Visible = false;               
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvOpenReports.Rebind();
        gvNonRoutineRA.Rebind();
        gvCrewComplaints.Rebind();
        gvUnSafeAct.Rebind();
        gvIncident.Rebind();
        gvMachineryDamage.Rebind();
        gvDrills.Rebind();
        if (ViewState["type"].ToString().ToLower() == "t")
        {
            divQMSMOC.Visible = false;
            divTechMOC.Visible = true;
            divTraining.Visible = false;
            divauditdeficiency.Visible = true;
            gvMOC.Rebind();
            gvDeficiencyStatus.Rebind();
            divProposal.Visible = true;
        }
        else
        {            
            divQMSMOC.Visible = true;
            divTechMOC.Visible = false;
            divTraining.Visible = true;
            divauditdeficiency.Visible = false;
            gvQMSMOC.Rebind();
            gvQMSTraining.Rebind();
            divProposal.Visible = false;
        }
    }
    
    protected void gvMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardMOCSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvMOC.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
                gvMOC.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
                gvMOC.Columns[1].Visible = false;
        }
    }

    protected void gvMOC_ItemDataBound(object sender, GridItemEventArgs e)
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
                if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
                {
                    if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
                        lnkShip.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (lnkOffice != null)
            {
                lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
                {
                    if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
                        lnkOffice.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    protected void gvOpenReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardOpenReportSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvOpenReports.DataSource = dt;
    }
    protected void gvOpenReports_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

            if (lnkcount != null)
            {
                lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Open Reports - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
            }
        }
    }

    protected void gvCrewComplaints_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboarCrewComplaintSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvCrewComplaints.DataSource = dt;
    }

    protected void gvCrewComplaints_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

            if (lnkcount != null)
            {
                lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Crew Complaints - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
            }
        }
    }

    protected void gvUnSafeAct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardUnsafeActSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvUnSafeAct.DataSource = dt;
    }

    protected void gvUnSafeAct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

            if (lnkcount != null)
            {
                lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Unsafe Acts / Conditions - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
            }
        }
    }

    protected void gvMachineryDamage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardMachineryDamageSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvMachineryDamage.DataSource = dt;
    }

    protected void gvMachineryDamage_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

            if (lnkcount != null)
            {
                lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Machinery Damage / Failure - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
                if (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(drv["FLDCOUNT"].ToString()) > 0)
                        lnkcount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    protected void gvNonRoutineRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardNonRoutineRASummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvNonRoutineRA.DataSource = dt;
    }

    protected void gvNonRoutineRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkOld = (LinkButton)e.Item.FindControl("lnkOld");
            LinkButton lnkNew = (LinkButton)e.Item.FindControl("lnkNew");


            RadLabel lblOld = (RadLabel)e.Item.FindControl("lblOld");
            RadLabel lblNew = (RadLabel)e.Item.FindControl("lblNew");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblname");

            if (lnkOld != null)
            {
                lnkOld.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Old - Non Routine RA - " + lblmeasure.Text + "','" + lblOld.Text + "'); return false;");
            }

            if (lnkNew != null)
            {
                lnkNew.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','New - Non Routine RA - " + lblmeasure.Text + "','" + lblNew.Text + "'); return false;");
                
                if (!string.IsNullOrEmpty(drv["FLDNEWCOUNT"].ToString()) && drv["FLDMEASURE"].ToString().ToLower().Contains("expired"))
                {
                    if (int.Parse(drv["FLDNEWCOUNT"].ToString()) > 0)
                        lnkNew.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    protected void gvIncident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardIncidentSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvIncident.DataSource = dt;
    }

    protected void gvIncident_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblname = (RadLabel)e.Item.FindControl("lblname");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkcount = (LinkButton)e.Item.FindControl("lnkcount");

            if (lnkcount != null)
            {
                lnkcount.Attributes.Add("onclick", "javascript: top.openNewWindow('wo','Accident and Near Miss - " + lblname.Text + "','" + lblurl.Text + "'); return false;");
                if (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(drv["FLDCOUNT"].ToString()) > 0)
                        lnkcount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    
    protected void gvDeficiencyStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardDeficiencySummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvDeficiencyStatus.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
                gvDeficiencyStatus.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
                gvDeficiencyStatus.Columns[1].Visible = false;
        }
    }

    protected void gvDeficiencyStatus_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvDrills_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixInspectionDrillSchedule.Drilldashboardoverduelist(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        gvDrills.DataSource = dt;

    }

    protected void gvDrills_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;

            LinkButton drillname = (LinkButton)item.FindControl("Drilloverdueanchor");

            drillname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Drill Due Across Vessels','Inspection/InspectionDrillsvsVesselList.aspx?drillid=" + DataBinder.Eval(e.Item.DataItem, "FLDDRILLID") + "&i=-1" + "&j=-1500&a=d" + "&type=" + DataBinder.Eval(e.Item.DataItem, "FLDTYPE") + "');return false");

            if (int.Parse(drv["FLDOVERDUE"].ToString()) > 0)
                drillname.ForeColor = System.Drawing.Color.Red;
        }


    }

    protected void gvQMSMOC_ItemDataBound(object sender, GridItemEventArgs e)
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
                if (!string.IsNullOrEmpty(drv["FLDSHIPCOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
                {
                    if (int.Parse(drv["FLDSHIPCOUNT"].ToString()) > 0)
                        lnkShip.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (lnkOffice != null)
            {
                lnkOffice.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOffice.Text + "'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOFFICECOUNT"].ToString()) && (drv["FLDMEASURE"].ToString().ToLower().Contains("overdue") || drv["FLDMEASURE"].ToString().ToLower().Contains("exceeded")))
                {
                    if (int.Parse(drv["FLDOFFICECOUNT"].ToString()) > 0)
                        lnkOffice.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    protected void gvQMSMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixDashboardQuality.DashboardMOCSummary(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        gvQMSMOC.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOFFICEVISIBLE"].ToString().Equals("0"))
                gvQMSMOC.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
                gvQMSMOC.Columns[1].Visible = false;
        }
    }

    protected void gvQMSTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixInspectionTrainingSummary.OverdueTrainingsDashboardSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        gvQMSTraining.DataSource = dt;
    }

    protected void gvQMSTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton trainingname = (LinkButton)item.FindControl("trainingoverdueanchor");

            trainingname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Training Due Across Vessels','Inspection/InspectionTrainingvsVessellist.aspx?trainingid=" + DataBinder.Eval(e.Item.DataItem, "FLDTRAININGID") + "&i=-1" + "&j=-1500&a=d" + "&type=" + DataBinder.Eval(e.Item.DataItem, "FLDTYPE") + "');return false");
            if (int.Parse(drv["FLDOVERDUE"].ToString()) > 0)
                trainingname.ForeColor = System.Drawing.Color.Red;
        }
    }

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

	protected void gvProposal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode != "OFFSHORE")
        {
            DataTable dt = PhoenixDashboardCrew.DashboardOfficeCrewTechByRankVesselCount("CREWAPP", 0, 0, 0);
            gvProposal.DataSource = dt;
        }
    }

    protected void gvProposal_ItemDataBound(object sender, GridItemEventArgs e)
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
                cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURE"].ToString() + "','" + link + querystring + "'); return false;";
            }
            else
            {
                cnt.Enabled = false;
                cnt.Attributes["style"] = "color: black";
                cnt.Text = "-";
            }
        }
    }
    protected void gvWRH_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.ListWorkandRestHourtAlert(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        grid.DataSource = dt;
    }

    protected void gvWRH_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkSeafarerCount = (LinkButton)e.Item.FindControl("lnkSeafarerCount");
            LinkButton lnkHODCount = (LinkButton)e.Item.FindControl("lnkHODCount");
            LinkButton lblMasterCount = (LinkButton)e.Item.FindControl("lblMasterCount");

            RadLabel lnkSeafarerUrl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lnkHODUrl = (RadLabel)e.Item.FindControl("lnkHODUrl");
            RadLabel lblMasterUrl = (RadLabel)e.Item.FindControl("lblMasterUrl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            //if ((lnkSeafarerCount != null) && (lnkSeafarerCount.Text != "") && (lnkSeafarerCount.Text != null) && drv["FLDSEAFARERURL"].ToString() != string.Empty)
            //{
            //    lnkSeafarerCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Seafarer','" + drv["FLDSEAFARERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - Overdue'); return false;");
            //}
            //else
            //{
            //    lnkSeafarerCount.Enabled = false;
            //    lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
            //}
            //if ((lnkHODCount != null) && (lnkHODCount.Text != "") && (lnkHODCount.Text != null) && drv["FLDHODURL"].ToString() != string.Empty)
            //{
            //    lnkHODCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - HOD','" + drv["FLDHODURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 30 Days'); return false;");
            //}
            //else
            //{
            //    lnkHODCount.Enabled = false;
            //    lnkHODCount.ForeColor = Color.FromName("#1e395b");
            //}
            //if ((lblMasterCount != null) && (lblMasterCount.Text != "") && (lblMasterCount.Text != null) && drv["FLDMASTERURL"].ToString() != string.Empty)
            //{
            //    lblMasterCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Master','" + drv["FLDMASTERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 60 Days'); return false;");
            //}
            //else
            //{
            //    lblMasterCount.Enabled = false;
            //    lblMasterCount.ForeColor = Color.FromName("#1e395b");
            //}
            if (drv["FLDSEAFARERCOUNT"].ToString().Equals(""))
            {
                lnkSeafarerCount.Text = "N/A";
                lnkSeafarerCount.Enabled = false;
                lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLDHODCOUNT"].ToString().Equals(""))
            {
                lnkHODCount.Text = "N/A";
                lnkHODCount.Enabled = false;
                lnkHODCount.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLDMASTERCOUNT"].ToString().Equals(""))
            {
                lblMasterCount.Text = "N/A";
                lblMasterCount.Enabled = false;
                lblMasterCount.ForeColor = Color.FromName("#1e395b");
            }
            lnkSeafarerCount.Enabled = false;
            lnkHODCount.Enabled = false;
            lblMasterCount.Enabled = false;
        }
    }
}