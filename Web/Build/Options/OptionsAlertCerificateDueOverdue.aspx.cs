using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;

public partial class OptionsAlertCerificateDueOverdue : PhoenixBasePage
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
        if (tasktype.Equals("9"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.CertificatesDueIn45Days(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("10"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.CertificatesOverDue(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
    }

    protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tasktype = Request.QueryString["tasktype"].ToString();

        if (int.Parse(tasktype) == 9 || int.Parse(tasktype) == 10)
        {
            ShowVesselMaster(sender, e, tasktype);
        }
    }

    protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    private void ShowVesselMaster(object sender, GridViewCommandEventArgs e, string tasktype)
    {
        int nRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;

        PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

        DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
        SessionUtil.ReBuildMenu();

        Filter.CurrentVesselMasterFilter = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "parent.showVessel();";
        Script += "parent.OpenSearchPage('Registers/RegistersVesselMaster.aspx?certificatesalert=true', 'REG-GEN-VLM');";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
