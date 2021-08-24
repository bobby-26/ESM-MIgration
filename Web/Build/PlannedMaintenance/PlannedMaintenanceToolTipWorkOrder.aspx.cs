using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceToolTipWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["workorderid"] != null && Request.QueryString["vesselid"] != null)
                BindData(Request.QueryString["workorderid"].ToString(), Request.QueryString["vesselid"].ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(string workorderid, string vesselid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixPlannedMaintenanceWorkOrder.EditWorkOrder(new Guid(workorderid), int.Parse(vesselid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
            lbltFormTitle.Text = dr["FLDWORKORDERNAME"].ToString();
            lbltStartedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERSTARTEDDATE"].ToString());
            lbltLastDoneDate.Text = General.GetDateTimeToString(dr["FLDJOBLASTDONEDATE"].ToString());
            lbltCompletedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERCOMPLETEDDATE"].ToString());
            lbltDueDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
            lbltWindowDays.Text = dr["FLDPLANNINGWINDOWINDAYS"].ToString();
            lbltOverdueDays.Text = dr["FLDOVERDUEDATE"].ToString();
        }
    }
}
