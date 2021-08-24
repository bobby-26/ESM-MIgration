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

public partial class OptionsVesselAccountsProvisionOnBoardAlert : PhoenixBasePage
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
        if (tasktype.Equals("33"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.ProvisionOnBoardAlert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            gvAlertsTask.DataBind();
        }
    }

    protected void gvAlertsTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAlertsTask.SelectedIndex = -1;
        gvAlertsTask.EditIndex = -1;

        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
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
