using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class OptionsCrewAppraisalPending : PhoenixBasePage
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
        if (tasktype.Equals("36"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.CrewAppraisalIncompleteBeforeSignoff(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            gvAlertsTask.DataBind();
        }
    }

    protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tasktype = Request.QueryString["tasktype"].ToString();

        int nRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;

        PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);
    }

    protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        
    }
}
