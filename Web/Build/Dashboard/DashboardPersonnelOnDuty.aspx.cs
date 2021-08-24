using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardPersonnelOnDuty : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        if (!IsPostBack)
        {
           
        }
    }


    protected void GvOnDuty_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid pivot = (RadPivotGrid)sender;
        DateTime localtime = Session["localtime"] != null ? DateTime.Parse(Session["localtime"].ToString()) : DateTime.Now;
        DataSet ds = PhoenixDashboardTechnical.DashboardPersonalOnDuty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , localtime, localtime.Hour + 1);
        pivot.DataSource = ds;
    }

    protected void GvOnDuty_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataSet ds = (DataSet)grid.DataSource;
                DataTable dt = ds.Tables[0];
                //DataTable dt1 = ds.Tables[1];
                string row = cell.ParentRowIndexes.Length > 0 ? cell.ParentRowIndexes[0].ToString() : string.Empty;
                string col = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
                DataRow[] dr = dt.Select("FLDMANAGEMENT = '" + row + "' AND FLDDEPARTMENT='" + col + "'");
                string employees = string.Empty;
                foreach (DataRow d in dr)
                {
                    //string remainnghours = string.Empty;
                    //DataRow[] rdr = dt1.Select("FLDEMPLOYEEID = " + d["FLDEMPLOYEEID"].ToString() + "");
                    //if (rdr.Length > 0)
                    //    remainnghours = rdr[0]["FLDREMAININGHOURS"].ToString();
                    employees += "<a href=\"javascript:top.openNewWindow('wo', 'Off Duty', 'Dashboard/DashboardRestHourDuty.aspx?refwin=CompCategory&d=off&e=" + d["FLDEMPLOYEEID"].ToString() + "',false,400,300);\" >" + d["FLDEMPLOYEENAME"].ToString() + "</a><br />";
                    //employees += d["FLDEMPLOYEENAME"].ToString() + " (" + remainnghours + " Hours)" + "<br />";
                }
                cell.Text = employees.Equals("") ? "N/A" : employees;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        GvOnDuty.Rebind();
    }
}