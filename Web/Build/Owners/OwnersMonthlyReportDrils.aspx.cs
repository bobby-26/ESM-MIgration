using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportDrils : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rddrildue.Visible = SessionUtil.CanAccess(this.ViewState, "DRILLDUE");
        rdTrainingDue.Visible = SessionUtil.CanAccess(this.ViewState, "TRAININGDUE");
        rdDrill.Visible = SessionUtil.CanAccess(this.ViewState, "DRILLDONE");
        rdTraining.Visible = SessionUtil.CanAccess(this.ViewState, "TRAININGDONE");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkDrillDue.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Drill Due', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportDrillDueList.aspx');");
            lnkDrillDone.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Drill Done', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportDrillDoneList.aspx');");
            lnkTrainingDue.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Training Due', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportTrainingDueList.aspx');");
            lnkTrainingDone.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Training Done', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportTrainingDoneList.aspx');");
            lnkDrillDueComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Drill Due', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=DDU');return false; ");
            lnkDrillDoneComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Drill Done', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=DDN');return false; ");
            lnkTrainingDueComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Training Due', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=TDU');return false; ");
            lnkTrainingDoneComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Training Done', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=TDN');return false; ");

            lnkDrillDueInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Drill Due','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=DDU" + "',false, 320, 250,'','',options); return false;");
            lnkTrainingDueInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Training Due','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=TDU" + "',false, 320, 250,'','',options); return false;");
            lnkDrillDoneInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Drill Done','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=DDN" + "',false, 320, 250,'','',options); return false;");
            lnkTrainingDoneInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Training Done','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=TDN" + "',false, 320, 250,'','',options); return false;");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("DDU", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkDrillDueComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("DDN", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkDrillDoneComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("TDU", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkTrainingDueComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("TDN", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkTrainingDoneComments.Controls.Add(html);
        }
    }

    protected void gvdrildue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDueCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvdrildue.DataSource = dt;
    }

    protected void gvTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportTrainingDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvTraining.DataSource = dt;
    }

    protected void gvTrainingDue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportTrainingDueCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvTrainingDue.DataSource = dt;
    }

    protected void gvDrill_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvDrill.DataSource = dt;
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    protected void gvDrill_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton Drill = (LinkButton)e.Item.FindControl("lblmeasure");
            RadLabel DrillScheduleid = (RadLabel)e.Item.FindControl("radid");
            if (Drill != null)
            {
                Drill.Attributes.Add("href", "javascript:openNewWindow('code1', 'Drill Done', '" + Session["sitepath"] + "/Inspection/InspectionDrillScheduleReport.aspx?drillscheduleid=" + DrillScheduleid.Text + "&ownerreport=y" + "');");

            }
        }
    }

    protected void gvTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton Training = (LinkButton)e.Item.FindControl("lblTmeasure");
            RadLabel TrainingScheduleid = (RadLabel)e.Item.FindControl("radTid");
            if (Training != null)
            {
                Training.Attributes.Add("href", "javascript:openNewWindow('code1', 'Drill Done', '" + Session["sitepath"] + "/Inspection/InspectionTrainingScheduleReport.aspx?Trainingscheduleid=" + TrainingScheduleid.Text + "&ownerreport=y" + "');");

            }
        }
    }
}