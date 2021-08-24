using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportCrew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        tdsummary.Visible = SessionUtil.CanAccess(this.ViewState, "CREWSUMMARY");
        rdReleif.Visible = SessionUtil.CanAccess(this.ViewState, "RELEIF");
        rdSignoff.Visible = SessionUtil.CanAccess(this.ViewState, "SIGNOFF");
        if(!IsPostBack)
        {
            lnksign.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Sign on/Sign Off', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportCrewSign.aspx');");
            lnkRelief.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Relief Planner', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportCrewReliever.aspx');");
            lnkSummaryComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Summary', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=SMR');return false; ");
            lnksignComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Sign on/Sign Off', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=SOF');return false; ");
            lnkReliefComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Relief Planner', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=RPL');return false; ");

            lnkSummaryInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Summary','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=SMR" + "',false, 320, 250,'','',options); return false;");
            lnkReliefInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Relief Planner','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=RPL" + "',false, 320, 250,'','',options); return false;");
            lnksignInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Sign on/Sign Off','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=SOF" + "',false, 320, 250,'','',options); return false;");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("SMR", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkSummaryComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("SOF", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnksignComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("RPL", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkReliefComments.Controls.Add(html);
        }
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixOwnersReportCrew.CrewMeasureSearch("CREW", int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        var halfDT = dt.Copy();

        var lastRowIndex = halfDT.Rows.Count - 1;
        var firstend = halfDT.Rows.Count / 2 - 2;
        var secondstart = firstend + 1;
        var secondend = secondstart + 3;
        var thirdstart = secondend + 1;
        if (grid.ClientID.Contains("gvCrew1"))
        {
            for (int i = lastRowIndex; i > firstend; i--)
            {
                halfDT.Rows.RemoveAt(i);
            }
            halfDT.AcceptChanges();
        }
        if (grid.ClientID.Contains("gvCrew2"))
        {
            for (int i = lastRowIndex; i > secondend; i--)
            {
                halfDT.Rows.RemoveAt(i);
            }
            firstend = halfDT.Rows.Count / 2 - 1;
            //lastRowIndex = halfDT.Rows.Count - 1;
            for (int i = firstend; i > -1; i--)
            {
                halfDT.Rows.RemoveAt(i);
            }
            halfDT.AcceptChanges();
        }
        if (grid.ClientID.Contains("gvCrew3"))
        {
            for (int i = secondend; i > -1; i--)
            {
                halfDT.Rows.RemoveAt(i);
            }
            halfDT.AcceptChanges();
        }
        grid.DataSource = halfDT;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;
            LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

            if (!string.IsNullOrEmpty(cnt.Text) && int.Parse(cnt.Text) > 0)
            {
                string querystring = "?code=" + drv["FLDSHORTCODE"].ToString() + "&vesselid=" + int.Parse(Filter.SelectedOwnersReportVessel) + "&reportdate=" + Filter.SelectedOwnersReportDate;
                string link = drv["FLDURL"].ToString();
                if (!string.IsNullOrEmpty(link))
                {
                    int index = link.IndexOf('?');
                    if (index > -1)
                    {
                        querystring = querystring.Replace("?", "&");
                    }
                    cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURENAME"].ToString() + "','" + link + querystring + "'); return false;";
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

    protected void gvReleif_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = new DataTable();
        dt = PhoenixOwnersReportCrew.ReliefPlan(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , gvReleif.CurrentPageIndex + 1
            , gvReleif.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        gvReleif.DataSource = dt;
        gvReleif.VirtualItemCount = iRowCount;
    }

    protected void gvReleif_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            Label lblRemarks = (Label)e.Item.FindControl("lblRemarks");
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (imgRemarks != null)
            {
                if (lblRemarks != null)
                {
                    if (lblRemarks.Text != "")
                    {
                        imgRemarks.Visible = true;

                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = imgRemarks.ClientID;
                            //imgRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            //imgRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        imgRemarks.Visible = false;
                }
            }
        }
    }
    protected void gvSignoff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnersReportCrew.SignoffList(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvSignoff.DataSource = dt;
    }

    protected void gvSignoff_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }
}