using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class OptionsAlertAuditReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string tasktype = Request.QueryString["tasktype"].ToString();
            Filter.CurrentAlertTaskType = tasktype;           
        }
        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }

    private void GetAlertItems(string tasktype)
    {
        if (tasktype.Equals("27"))
        {
            gvAlertsTask.DataSource = PhoenixInspectionAuditSchedule.AlertAuditReport(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            gvAlertsTask.DataBind();
        }        
    }

    protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string tasktype = Request.QueryString["tasktype"].ToString();

        if (int.Parse(tasktype) == 27)
        {
            int nRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridView = (GridView)sender;

            PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "parent.OpenSearchPage('Inspection/InspectionInternalAuditReport.aspx?alert=1&reviewscheduleid=" + ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text + "&date="
                + ((Label)_gridView.Rows[nRow].FindControl("lblReportDate")).Text + "&vesselid=" + ((Label)_gridView.Rows[nRow].FindControl("lblVesselid")).Text + "', 'REP-QUA-IAR');";
            Script += "</script>" + "\n";

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }        
    }

    protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        
    }
}
