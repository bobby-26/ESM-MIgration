using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportIncidentView : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportIncidentView.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvincident')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuIncoident.AccessRights = this.ViewState;
        MenuIncoident.MenuList = toolbar.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
    }


    protected void gvincident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Ref. No", "Type", "Category", "Subcategory", "Cons", "Title", "Reported", "Incident", "Status" };
        
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportIncidentSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        gvincident.DataSource = dt;

        DataSet ds = new DataSet();
        DataTable dt1 = dt.Copy();
        ds.Tables.Add(dt1);
        General.SetPrintOptions("gvincident", "Incident / Near Miss", alCaptions, alColumns, ds);
        
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

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

                lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('incident', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentList.aspx?DashboardYN=1&IncidentId=" + lblInspectionIncidentId.Text + "');");

            }
        }
    }
    protected void ShowExcel()
    {
            string[] alColumns = { "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE","FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
            string[] alCaptions = { "Ref. No", "Type", "Category", "Subcategory", "Cons", "Title", "Reported", "Incident", "Status" };

            DataTable dt = PhoenixOwnerReportQuality.OwnersReportIncidentSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
            DataSet ds = new DataSet();
            DataTable dt1 = dt.Copy();
            ds.Tables.Add(dt1);

            General.ShowExcel("Incident / Near Miss", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    protected void MenuIncoident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
}