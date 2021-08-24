using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OptionScheduleJobHistoryMessage : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionScheduleJobHistoryMessage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSchedulejobHistoryMessage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            Menujobhistory.AccessRights = this.ViewState;
            Menujobhistory.MenuList = toolbar.Show();
           
            if (!IsPostBack)
            {
                if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
                {
                    ViewState["Id"] = Request.QueryString["id"];
                }
                gvSchedulejobHistoryMessage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSchedulejobHistoryMessage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string[] alColumns = { "LAST_OUTCOME_MESSAGE", "LAST_RUN_DATE", "LAST_RUN_TIME" };
            string[] alCaptions = { "Last Outcome Message", "Last Run Date", "Last Run Time" };

            DataSet ds = new DataSet();
           ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistorymessagelist(new Guid(ViewState["Id"].ToString()));


            General.SetPrintOptions("gvSchedulejobHistoryMessage", "Schedule Job History Message", alCaptions, alColumns, ds);
            gvSchedulejobHistoryMessage.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {

        DataSet ds = new DataSet();

        string sortexpression = null;
        int? sortdirection = null;

        string[] alColumns = { "LAST_OUTCOME_MESSAGE", "LAST_RUN_DATE", "LAST_RUN_TIME" };
        string[] alCaptions = { "Last Outcome Message", "Last Run Date", "Last Run Time" };
        ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistorymessagelist(new Guid(ViewState["Id"].ToString()));

        if (ds.Tables.Count > 0)
            General.ShowExcel("Schedule Job History Message", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void Menujobhistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
}