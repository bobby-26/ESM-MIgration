using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OptionScheduleJobHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionScheduleJobHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSchedulejobHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            Menujobhistory.AccessRights = this.ViewState;
            Menujobhistory.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                gvSchedulejobHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvSchedulejobHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            string[] alColumns = { "NAME", "ENABLED" };
            string[] alCaptions = { "Name", "Enable"};

            DataSet ds = new DataSet();
            ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistory();

            General.SetPrintOptions("gvSchedulejobHistory", "Schedule Job History", alCaptions, alColumns, ds);
            gvSchedulejobHistory.DataSource = ds;
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

        string[] alColumns = { "NAME", "ENABLED" };
        string[] alCaptions = { "Name", "Enable" };
        ds = PhoenixCommonScheduleJobHistory.ScheduleJobHistory();
        if (ds.Tables.Count > 0)
            General.ShowExcel("Schedule Job History", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
    protected void gvSchedulejobHistory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdEdit1");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);


        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton editBtn = (ImageButton)e.Item.FindControl("cmdEdit");

            if (editBtn != null)
            {
                if (string.IsNullOrWhiteSpace(drv["JOB_ID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Options/OptionScheduleJobHistoryStatus.aspx?id={1}'); return false", Session["sitepath"], drv["JOB_ID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }


            ImageButton editBtn1 = (ImageButton)e.Item.FindControl("cmdEdit1");
            if (editBtn1 != null)
            {
                if (string.IsNullOrWhiteSpace(drv["JOB_ID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Options/OptionScheduleJobHistoryMessage.aspx?id={1}'); return false", Session["sitepath"], drv["JOB_ID"].ToString());
                    editBtn1.Attributes.Add("onclick", script);
                }
            }

        }
    }
}