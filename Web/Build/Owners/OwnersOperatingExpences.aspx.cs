using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersOperatingExpences : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //  cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkExpenses.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Vessel Operating Expenses', '" + Session["sitepath"] + "/Owners/OwnerOperatingExpensesView.aspx',500,800);");
            lnkExpensesComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Vessel Operating Expenses', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=VOE');return false; ");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("VOE", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkExpensesComments.Controls.Add(html);
        }
    }
    protected void GVPUR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        DataTable dt = PhoenixOwnerReportQuality.OwnersMonthlyVesselOperatingExpences(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        if (dt.Rows.Count > 0)
        {
            lblmonth.Text = DateTime.Parse(dt.Rows[0]["FLDBUDGETENDDATE"].ToString()).ToString("MMMM yyyy");
        }
        GVPUR.DataSource = dt;
    }

    protected void GVPUR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["FLDBUDGETTYPE"].ToString() ;
        }
        if(e.Item is GridEditableItem)
        {
            //DataRowView drv = (DataRowView) e.Item.DataItem;
            //LinkButton lb = (LinkButton)e.Item.FindControl("lblAutuals");
            //if(lb!=null)
            //lb.Attributes.Add("onclick", "javascript: top.openNewWindow('codehelp1','"+ drv["FLDBUDGETGROUP"].ToString() + "','" + Session["sitepath"] + "/Owners/OwnersSOAWithSupportingsVoucherBreakup.aspx?debitnotereference="
            //                + drv["FLDDEBITNOTEREFERENCE"].ToString() + "&accountid=" + drv["FLDACCOUNTID"].ToString() + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "'); return false;");
        }
        if (e.Item is GridFooterItem)
        {
            var valueFooterCell = (e.Item as GridFooterItem)["Expenses"];
            valueFooterCell.Text = "Grand Total: " + valueFooterCell.Text;
        }
        else if (e.Item is GridGroupFooterItem)
        {
            var valueFooterCell = (e.Item as GridGroupFooterItem)["Expenses"];
            valueFooterCell.Text = "Sub Total: " + valueFooterCell.Text;
        }
    }
    protected void gvYearly_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        DataTable dt = PhoenixOwnerReportQuality.OwnersYearlyVesselOperatingExpences(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        if(dt.Rows.Count>0)
        {
            lblyeardays.Text =dt.Rows[0]["FLDNOOFDAYS"].ToString();
            lblbudgetdays.Text = dt.Rows[0]["FLDNOOFDAYS"].ToString();
            lblvardays.Text = dt.Rows[0]["FLDNOOFDAYS"].ToString();
        }
        gvYearly.DataSource = dt;
    }
    protected void GVPUR_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }

    protected void GVPUR_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }
    protected void gvYearly_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }

    protected void gvYearly_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }

    protected void gvYearly_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["FLDBUDGETTYPE"].ToString();
        }
        if (e.Item is GridEditableItem)
        {
            //DataRowView drv = (DataRowView)e.Item.DataItem;
            //LinkButton lb = (LinkButton)e.Item.FindControl("lblAutuals");
            //if (lb != null)
            //    lb.Attributes.Add("onclick", "javascript: top.openNewWindow('codehelp1','" + drv["FLDBUDGETGROUP"].ToString() + "','" + Session["sitepath"] + "/Owners/OwnersSOAWithSupportingsVoucherYearlyBreakup.aspx?debitnotereference="
            //                    + drv["FLDDEBITNOTEREFERENCELIST"].ToString() + "&accountid=" + drv["FLDACCOUNTID"].ToString() + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "'); return false;");

        }
    }

    protected void GVPUR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("ACTUAL"))
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Filter.SelectedOwnersDebitnoteReference = drv["FLDDEBITNOTEREFERENCE"].ToString();
            string script = "javascript:top.openNewWindow('codehelp1','" + drv["FLDBUDGETGROUP"].ToString() + "','" + Session["sitepath"] + "/Owners/OwnersSOAWithSupportingsVoucherBreakup.aspx?accountid=" + 
                drv["FLDACCOUNTID"].ToString() + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
        }
    }
    protected void gvYearly_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("ACTUAL"))
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Filter.SelectedOwnersDebitnoteReference = drv["FLDDEBITNOTEREFERENCELIST"].ToString();
            string script = "javascript:top.openNewWindow('codehelp1','" + drv["FLDBUDGETGROUP"].ToString() + "','" + Session["sitepath"] + "/Owners/OwnersSOAWithSupportingsVoucherYearlyBreakup.aspx?accountid=" +
                drv["FLDACCOUNTID"].ToString() + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "')";
            RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }
}