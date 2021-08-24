using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdpmsSummary.Visible = SessionUtil.CanAccess(this.ViewState, "PMSSUMMARY");
        rdRunningHours.Visible = SessionUtil.CanAccess(this.ViewState, "RUNNINGHOURS");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkRunning.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Running Hours', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportPMSRunningHoursView.aspx');");
            lnkSummaryComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Summary', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=PSM');return false; ");
            lnkRunningHoursComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Running Hours', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=RHR');return false; ");
            CheckComments();
        }
    }

    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("PSM", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkSummaryComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("RHR", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkRunningHoursComments.Controls.Add(html);
        }
    }
    protected void gvSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportPMSSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvSummary.DataSource = dt;
    }

    protected void gvSummary_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvRunningHours_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportRunningHours(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvRunningHours.DataSource = dt;
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }
}