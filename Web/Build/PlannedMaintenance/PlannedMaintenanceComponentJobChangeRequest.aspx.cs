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

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobChangeRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobChangeRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

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
                ViewState["JOBID"] = string.IsNullOrEmpty(Request.QueryString["JOBID"]) ? "" : Request.QueryString["JOBID"];
                gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Resp Discipline", "Priority" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataTable dt = PhoenixPlannedMaintenanceComponentJobRA.ListComponentJobChangeRequestHistory(
                            new Guid(ViewState["JOBID"].ToString())
                            , null
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvComponentJob", "Component Job Change Request", alCaptions, alColumns, ds);


            gvComponentJob.DataSource = dt;
            gvComponentJob.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (FormatException ex)
        {
            ucError.ErrorMessage = ex.Message + "Error come here";
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Resp Discipline", "Priority" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceComponentJobRA.ListComponentJobChangeRequestHistory(
                            new Guid(ViewState["JOBID"].ToString())
                            , null
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            General.ShowExcel("Component Job Change Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}