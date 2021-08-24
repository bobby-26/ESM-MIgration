using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class OwnerOperatingExpensesView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //  cmdHiddenSubmit.Attributes.Add("style", "display:none");
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
            item.DataCell.Text = groupDataRow["FLDBUDGETTYPE"].ToString();
        }

        if (e.Item is GridDataItem)
        {
            LinkButton lblAutuals = (LinkButton)e.Item.FindControl("lblAutuals");
            RadLabel lblBudgeted = (RadLabel)e.Item.FindControl("lblBudgeted");
            RadLabel lblVar = (RadLabel)e.Item.FindControl("lblVar");
            RadLabel lblVarper = (RadLabel)e.Item.FindControl("lblVarper");
            if (lblAutuals != null)
            {
                if (lblAutuals.Text.Contains("-"))
                    lblAutuals.Attributes.Add("style", "color:Red !important");
            }
            if (lblBudgeted != null)
            {
                if (lblBudgeted.Text.Contains("-"))
                    lblBudgeted.Attributes.Add("style", "color:Red !important");
            }
            if (lblVar != null)
            {
                if (lblVar.Text.Contains("-"))
                    lblVar.Attributes.Add("style", "color:Red !important");
            }
            if (lblVarper != null)
            {
                if (lblVarper.Text.Contains("-"))
                    lblVarper.Attributes.Add("style", "color:Red !important");
            }

        }
        if (e.Item is GridEditableItem)
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
            var valueFooterActuals = (e.Item as GridFooterItem)["Actuals"];
            var valueFooterBudgeted = (e.Item as GridFooterItem)["Budgeted"];
            var valueFooterAbs = (e.Item as GridFooterItem)["Var(Abs)"];
            var valueFooterVarpercent = (e.Item as GridFooterItem)["Varpercent"];

            valueFooterCell.Text = "Grand Total:" + valueFooterCell.Text;
            if (valueFooterActuals.Text.Contains("-"))
                valueFooterActuals.Style["color"] = "Red";
            if (valueFooterBudgeted.Text.Contains("-"))
                valueFooterBudgeted.Style["color"] = "Red";
            if (valueFooterAbs.Text.Contains("-"))
                valueFooterAbs.Style["color"] = "Red";
            if (valueFooterVarpercent.Text.Contains("-"))
                valueFooterVarpercent.Style["color"] = "Red";
        }
        else if (e.Item is GridGroupFooterItem)
        {
            var valueFooterCell = (e.Item as GridGroupFooterItem)["Expenses"];
            var valueFooterActuals = (e.Item as GridGroupFooterItem)["Actuals"];
            var valueFooterBudgeted = (e.Item as GridGroupFooterItem)["Budgeted"];
            var valueFooterAbs = (e.Item as GridGroupFooterItem)["Var(Abs)"];
            var valueFooterVarpercent = (e.Item as GridGroupFooterItem)["Varpercent"];

            valueFooterCell.Text = "Sub Total:" + valueFooterCell.Text;
            if (valueFooterActuals.Text.Contains("-"))
                valueFooterActuals.Style["color"] = "Red";
            if (valueFooterBudgeted.Text.Contains("-"))
                valueFooterBudgeted.Style["color"] = "Red";
            if (valueFooterAbs.Text.Contains("-"))
                valueFooterAbs.Style["color"] = "Red";
            if (valueFooterVarpercent.Text.Contains("-"))
                valueFooterVarpercent.Style["color"] = "Red";
        }
    }
    protected void gvYearly_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        DataTable dt = PhoenixOwnerReportQuality.OwnersYearlyVesselOperatingExpences(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        if (dt.Rows.Count > 0)
        {
            lblyeardays.Text = dt.Rows[0]["FLDNOOFDAYS"].ToString();
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
        if (e.Item is GridFooterItem)
        {
            var FLDTOTAL = (e.Item as GridFooterItem)["FLDTOTAL"];
            var FLDBUDGET = (e.Item as GridFooterItem)["FLDBUDGET"];
            var FLDVARIANCE = (e.Item as GridFooterItem)["FLDVARIANCE"];
            var FLDVARIANCEPERCENTAGE = (e.Item as GridFooterItem)["FLDVARIANCEPERCENTAGE"];
            var FLDDAILYAVERAGE = (e.Item as GridFooterItem)["FLDDAILYAVERAGE"];
            if (FLDTOTAL.Text.Contains("-"))
                FLDTOTAL.Style["color"] = "Red";
            if (FLDBUDGET.Text.Contains("-"))
                FLDBUDGET.Style["color"] = "Red";
            if (FLDVARIANCE.Text.Contains("-"))
                FLDVARIANCE.Style["color"] = "Red";
            if (FLDVARIANCEPERCENTAGE.Text.Contains("-"))
                FLDVARIANCEPERCENTAGE.Style["color"] = "Red";
            if (FLDDAILYAVERAGE.Text.Contains("-"))
                FLDDAILYAVERAGE.Style["color"] = "Red";
        }
        if (e.Item is GridGroupFooterItem)
        {
            var FLDTOTAL = (e.Item as GridGroupFooterItem)["FLDTOTAL"];
            var FLDBUDGET = (e.Item as GridGroupFooterItem)["FLDBUDGET"];
            var FLDVARIANCE = (e.Item as GridGroupFooterItem)["FLDVARIANCE"];
            var FLDVARIANCEPERCENTAGE = (e.Item as GridGroupFooterItem)["FLDVARIANCEPERCENTAGE"];
            var FLDDAILYAVERAGE = (e.Item as GridGroupFooterItem)["FLDDAILYAVERAGE"];
            if (FLDTOTAL.Text.Contains("-"))
                FLDTOTAL.Style["color"] = "Red";
            if (FLDBUDGET.Text.Contains("-"))
                FLDBUDGET.Style["color"] = "Red";
            if (FLDVARIANCE.Text.Contains("-"))
                FLDVARIANCE.Style["color"] = "Red";
            if (FLDVARIANCEPERCENTAGE.Text.Contains("-"))
                FLDVARIANCEPERCENTAGE.Style["color"] = "Red";
            if (FLDDAILYAVERAGE.Text.Contains("-"))
                FLDDAILYAVERAGE.Style["color"] = "Red";
        }
        if (e.Item is GridDataItem)
        {
            LinkButton lblAutuals = (LinkButton)e.Item.FindControl("lblAutuals");
            RadLabel lblBudgeted = (RadLabel)e.Item.FindControl("lblBudgeted");
            RadLabel lblVar = (RadLabel)e.Item.FindControl("lblVar");
            RadLabel lblVarper = (RadLabel)e.Item.FindControl("lblVarper");
            RadLabel lblDailyAvg = (RadLabel)e.Item.FindControl("lblDailyAvg");

            if (lblAutuals.Text.Contains("-"))
                lblAutuals.Style["color"] = "Red !important";
            if (lblBudgeted.Text.Contains("-"))
                lblBudgeted.Style["color"] = "Red !important";
            if (lblVar.Text.Contains("-"))
                lblVar.Style["color"] = "Red !important";
            if (lblVarper.Text.Contains("-"))
                lblVarper.Style["color"] = "Red !important";
            if (lblDailyAvg.Text.Contains("-"))
                lblDailyAvg.Style["color"] = "Red !important";
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
}