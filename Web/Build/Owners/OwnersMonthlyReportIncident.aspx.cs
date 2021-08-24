using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportIncident : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdincident.Visible = SessionUtil.CanAccess(this.ViewState, "INCIDENT");
        rdunsafeact.Visible = SessionUtil.CanAccess(this.ViewState, "UNSAFEACT");
        rdRA.Visible = SessionUtil.CanAccess(this.ViewState, "RISKASSESSMENT");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if(!IsPostBack)
        {
            lnkincident.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Incident / Near miss', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportIncidentView.aspx');return false;");
            lnkunsafe.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Unsafe Acts / Conditions', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportUnsafeActsView.aspx');return false;");
            lnkNRRA.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Non Routine Risk Assessments', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportNRRAView.aspx');return false;");
            lnkincidentComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Incident / Near miss', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=INM');return false; ");
            lnkunsafeComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Unsafe Acts/Conditions', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=USA');return false; ");
            lnkNRRAComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Non Routine Risk Assessments', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=NRA');return false; ");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("INM", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkincidentComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("USA", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkunsafeComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("NRA", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkNRRAComments.Controls.Add(html);
        }
    }
    protected void gvincident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportIncidentSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvincident.DataSource = dt;
    }

    protected void gvunsafeact_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportUnsafeActSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvunsafeact.DataSource = dt;
    }

    protected void gvunsafeact_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblSummary = (RadLabel)e.Item.FindControl("lblSummaryFirstLine");
            if (lblSummary.Text != "")
            {
                //lblSummary.CssClass = "tooltip";
            }
            UserControlToolTip uctt = (UserControlToolTip)e.Item.FindControl("ucToolTipSummary");
            if (uctt != null)
            {
                uctt.Position = ToolTipPosition.TopCenter;
                uctt.TargetControlId = lblSummary.ClientID;
            }
            RadLabel lblActionTaken = (RadLabel)e.Item.FindControl("lblActionTaken");
            if (lblActionTaken.Text != "")
            {
                //lblActionTaken.CssClass = "tooltip";
            }
            UserControlToolTip uc = (UserControlToolTip)e.Item.FindControl("ucToolTipActionTaken");
            if (uc != null)
            {
                uc.Position = ToolTipPosition.TopCenter;
                uc.TargetControlId = lblActionTaken.ClientID;
            }

            LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkRefno");
            RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
            if (lnkRefno != null)
            {
                lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('uacts', '', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditions.aspx?DashboardYN=1&directincidentid=" + lblDirectIncidentId.Text + "&OfficeDashboard=" + ViewState["OfficeDashboard"] + "');");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    protected void GVRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportNonRoutineRASummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVRA.DataSource = dt;
    }

    protected void GVRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

            LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkJobActivity");
            if (riskCreate != null)
            {
                riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);

                if (lbltypeid.Text == "1")
                {
                    riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericExtn.aspx?DashboardYN=1&genericid=" + drv["FLDRISKASSESSMENTID"].ToString() + "');");
                }
                if (lbltypeid.Text == "2")
                {
                    riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationExtn.aspx?DashboardYN=1&navigationid=" + drv["FLDRISKASSESSMENTID"].ToString() + "');");
                }
                if (lbltypeid.Text == "3")
                {
                    riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?DashboardYN=1&machineryid=" + drv["FLDRISKASSESSMENTID"].ToString() + "');");
                }
                if (lbltypeid.Text == "4")
                {
                    riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoExtn.aspx?DashboardYN=1&genericid=" + drv["FLDRISKASSESSMENTID"].ToString() + "');");
                }
            }
        }
    }

    protected void gvincident_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {              
             LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkIncidentRefNo");
             RadLabel lblInspectionIncidentId = (RadLabel)e.Item.FindControl("lblInspectionIncidentId");
            if (lnkRefno != null)
             {
                 lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                 lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('code1', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentList.aspx?DashboardYN=1&IncidentId=" + lblInspectionIncidentId.Text + "');");

             }
        }
    }
}
