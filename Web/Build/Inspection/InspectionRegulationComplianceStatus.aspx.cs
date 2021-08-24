using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationComplianceStatus : PhoenixBasePage
{
    string RegulationId;
    string screen;
    bool IsMoc;
    protected void Page_Load(object sender, EventArgs e)
    {
        screen = Request.QueryString["screen"];
        RegulationId = Request.QueryString["RegulationID"];
        IsMoc = Convert.ToBoolean(Request.QueryString["ismoc"]);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvRegulation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["COMPANYID"] = "";
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");

            }
        }
        ShowToolBar();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRegulation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Filter','Inspection/InspectionRegulationComplianceStatusFilter.aspx')", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRegulationComplianceStatus.aspx?RegulationId=" + RegulationId, "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRegulationComplianceStatus.aspx?RegulationId=" + RegulationId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegulation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (IsMoc == false)
        {
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        MenuDivRegulation.AccessRights = this.ViewState;
        MenuDivRegulation.MenuList = toolbar.Show();
    }
    private DataSet GetRegulationVesselList(ref int iRowCount, ref int iTotalPageCount)
    {
        bool IsComplian = false;
        if (Filter.CurrentRegulationComplianceStatusFilterCriteria != null)
        {
            NameValueCollection criteria = Filter.CurrentRegulationComplianceStatusFilterCriteria;
            IsComplian = Convert.ToBoolean(criteria["chkComplianceStatusAdd"]);
        }
        DataSet ds = PhoenixInspectionNewRegulationActionPlan.RegulationSummaryList(new Guid(RegulationId), IsComplian, (int)ViewState["PAGENUMBER"], gvRegulation.PageSize, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        return ds;
    }
    protected void gvRegulation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        string sortexpression = null;
        int? sortdirection = null;
        DataSet ds = GetRegulationVesselList(ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Compliance Status", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    private string[] getAlColumns()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDACTIONTOBETAKEN", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCLOSEDDATE", "FLDSTATUS" };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = { "Vessel", "Task", "Target Date", "Completion Date", "Closed Date", "Status" };
        return alCaptions;
    }
    private void PrintReport(DataSet ds)
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        General.SetPrintOptions("gvRegulation", "Compliance Status", alCaptions, alColumns, ds);
    }
    public void BindData()
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = GetRegulationVesselList(ref iRowCount, ref iTotalPageCount);
        gvRegulation.DataSource = ds;
        gvRegulation.VirtualItemCount = iRowCount;
        PrintReport(ds);
    }
    protected void gvRegulation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegulation.CurrentPageIndex + 1;
        BindData();
    }
    protected void plan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvRegulation.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionRegulation.aspx");
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentRegulationComplianceStatusFilterCriteria = null;
                gvRegulation.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvRegulation_PreRender(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            gvRegulation.MasterTableView.GetColumn("Vessel").Visible = false;
        }
    }
}