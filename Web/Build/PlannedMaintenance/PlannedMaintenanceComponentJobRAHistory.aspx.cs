using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobRAHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobRAHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRAHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PARENTPAGENO"] = string.IsNullOrEmpty(Request.QueryString["p"]) ? 1 : int.Parse(Request.QueryString["p"]);
                ViewState["COMPONENTID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]) ? "" : Request.QueryString["COMPONENTID"];
                ViewState["COMPONENTJOBID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTJOBID"]) ? "" : Request.QueryString["COMPONENTJOBID"];
                gvRAHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNAME", "FLDRANUMBER", "FLDDATEDONE", "FLDWORKDONEBY", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Name", "RA Number", "Done Date", "Done By", "Approve by" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceComponentJobRA.ListComponentJobRAHistory(new Guid(ViewState["COMPONENTJOBID"].ToString()),
                                                                                             PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvRAHistory", "RA Done History", alCaptions, alColumns, ds);
            gvRAHistory.DataSource = dt;
            //gvRAHistory.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
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
            int iRowCount = 0;
            //int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNAME", "FLDRANUMBER", "FLDDATEDONE", "FLDWORKDONEBY", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Name", "RA Number", "Done Date", "Done By", "Approve by" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string CompID = ViewState["COMPONENTJOBID"].ToString();

            DataTable dt = PhoenixPlannedMaintenanceComponentJobRA.ListComponentJobRAHistory(new Guid(ViewState["COMPONENTJOBID"].ToString()),
                                                                                           PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            General.ShowExcel("RA Done History", dt, alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
