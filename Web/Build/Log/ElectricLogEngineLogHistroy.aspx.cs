using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogEngineLogHistroy : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
       // ShowToolBar();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvEngineLogHistory.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddFontAwesomeButton("../Log/ElectricLogEngineLogHistroy.aspx", "Export to PDF", "<i class=\"fas fa-file-excel\"></i>", "PDF");
        toolbarmain.AddFontAwesomeButton("../Log/ElectricLogEngineLogHistroy.aspx", "Export to PDF", "<img src = \"../css/Theme1/images/pdf.png\" title=\"Export PDF\">", "PDF");
        gvTabStrip.MenuList = toolbarmain.Show();
        // get the session code here
    }

    protected void gvEngineLogHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Request.QueryString["date"]) == false)
        {
            BindData(DateTime.Parse(Request.QueryString["date"]));
            return;
        }
        BindData(null);
    }

    private void BindData(DateTime? date)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEngineLogHistory.CurrentPageIndex + 1;
        if (date == null)
        {
            date = DateTime.Now;
        }
        int iPageCount = 0;
        int iRowCount = 0;
        DataSet ds = PhoenixEngineLog.MainEngineLogHistorySearch(usercode, vesselId, date.Value, null,(int)ViewState["PAGENUMBER"], gvEngineLogHistory.PageSize, ref iRowCount, ref iPageCount);
        gvEngineLogHistory.DataSource = ds;
        gvEngineLogHistory.VirtualItemCount = iRowCount;
        // pagination is missing
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PDF"))
            {
                gvEngineLogHistory.MasterTableView.ExportToPdf();
            }
            else if (CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}