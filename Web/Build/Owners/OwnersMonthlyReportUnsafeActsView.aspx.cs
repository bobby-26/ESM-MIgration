using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportUnsafeActsView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportUnsafeActsView.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvunsafeact')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuUnSafeact.AccessRights = this.ViewState;
        MenuUnSafeact.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
    }
    protected void gvunsafeact_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDCORRECTIVEACTION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Reported", "Incident", "Time", "Category", "Subcategory", "Details", "Location", "Action Taken", "Status" };

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportUnsafeActSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvunsafeact.DataSource = dt;

        DataSet ds = new DataSet();
        DataTable dt1 = dt.Copy();
        ds.Tables.Add(dt1);
        General.SetPrintOptions("gvunsafeact", "Unsafe Act / Conditions ", alCaptions, alColumns, ds);
        
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

    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDCORRECTIVEACTION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No",  "Reported", "Incident", "Time", "Category", "Subcategory", "Details", "Location", "Action Taken", "Status" };

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportUnsafeActSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        DataSet ds = new DataSet();
        DataTable dt1 = dt.Copy();
        ds.Tables.Add(dt1);

        General.ShowExcel("Unsafe Act / Conditions ", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    protected void MenuUnSafeact_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
}