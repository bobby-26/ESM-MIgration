using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersInventoryPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkInventory.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Inventory - List of Major Parts onboard', '" + Session["sitepath"] + "/Owners/OwnersInventoryView.aspx');return false;");
            lnkspareitem.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Inventory - Spare Item Transaction', '" + Session["sitepath"] + "/Owners/OwnersInventorySpareTransaction.aspx');return false;");
            lnkSpareItemTransactioncomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Inventory - Spare Item Transaction - Comments', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=SIT');return false; ");
            lnkPurchaseSummaryComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Inventory - Spare Item Transaction - Comments', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=PSU');return false; ");
            lnkListofMajorPartsComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Inventory - Spare Item Transaction - Comments', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=ILS'); return false;");

            lnkListofMajorPartsInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Inventory - List of Major Parts onboard','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=ILS" + "',false, 320, 250,'','',options); return false;");
            lnkPurchaseSummaryInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Purchase Summary','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=PSU" + "',false, 320, 250,'','',options); return false;");
            lnkSpareItemTransactionInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Inventory - Spare Item Transaction','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=SIT" + "',false, 320, 250,'','',options); return false;");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("SIT", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if(dt.Rows.Count>0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkSpareItemTransactioncomment.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("ILS", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkListofMajorPartsComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("PSU", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkPurchaseSummaryComments.Controls.Add(html);
        }
    }
    protected void GVPUR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportPurchaseSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVPUR.DataSource = dt;
    }

    protected void GVPUR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;
            LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

            if (drv["FLDOVERDUEURL"].ToString() != string.Empty && cnt != null)
            {
                string querystring = "?code=";// + drv["FLDMEASURECODE"].ToString();
                string link = drv["FLDOVERDUEURL"].ToString();
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
                cnt.Attributes["style"] = "pointer-events: none";
               // cnt.Text = "-";
            }
        }
    }

    protected void GVR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportInventorySummary(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVR.DataSource = dt;
    }
    protected void GVR_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    protected void GVR_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }

    protected void GVR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["FLDTYPE"].ToString();
        }

        if (e.Item is GridDataItem)
        {

            RadLabel lblSMQ = (RadLabel)e.Item.FindControl("lblSMQ");
            RadLabel lblshortfallyn = (RadLabel)e.Item.FindControl("lblshortfallyn");

            if (lblshortfallyn.Text.Equals("1"))
            {
                lblSMQ.Attributes["style"] = "color:Red !important";
                lblSMQ.ToolTip = "Shortfalls";
                lblSMQ.Font.Bold = true;
            }
        }
    }

    protected void GVINV_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportInventoryTransactionSummary(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate),null,null,null);
        GVINV.DataSource = dt;
    }

    protected void GVINV_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }

    protected void GVINV_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["FLDTYPE"].ToString();
        }

    }

    protected void GVINV_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }
}