using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OptionScheduleJobHistoryStatus : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionScheduleJobHistoryStatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSchedulejobHistoryStatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            Menujobhistorystatus.AccessRights = this.ViewState;
            Menujobhistorystatus.MenuList = toolbar.Show();

           
            if (!IsPostBack)
            {
                if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
                {
                    ViewState["Id"] =Request.QueryString["id"];
                }
                gvSchedulejobHistoryStatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulejobHistoryStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string[] alColumns = { "RUN_STATUS", "RUN_DATE" , "STEP_NAME", "RUN_TIME" };
            string[] alCaptions = { "Run Status", "Run Date", "Step Name", "Run Time" };

            DataSet ds = new DataSet();
            ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistorystatuslist(new Guid(ViewState["Id"].ToString()));

            General.SetPrintOptions("gvSchedulejobHistoryStatus", "Schedule Job History Status", alCaptions, alColumns, ds);
            gvSchedulejobHistoryStatus.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            DataSet ds = new DataSet();

            string sortexpression = null;
            int? sortdirection = null;

            string[] alColumns = { "RUN_STATUS", "RUN_DATE", "STEP_NAME", "RUN_TIME" };
            string[] alCaptions = { "Run Status", "Run Date", "Step Name", "Run Time" };
            ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistorystatuslist(new Guid(ViewState["Id"].ToString()));

            if (ds.Tables.Count > 0)
                General.ShowExcel("Schedule Job History Status", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
         }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menujobhistorystatus_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
}