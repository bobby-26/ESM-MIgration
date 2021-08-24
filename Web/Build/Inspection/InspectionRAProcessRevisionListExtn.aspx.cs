using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAProcessRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentProcessRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RISKASSESSMENTPROCESSID"] = "";

            if (Request.QueryString["referenceid"] != null && Request.QueryString["referenceid"].ToString() != "")
            {
                ViewState["REFNO"] = Request.QueryString["referenceid"].ToString();
            }
            ViewState["callfrom"] = "";

            gvRiskAssessmentProcessRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Ref. No", "Prepared", "Process", "Activity", "Status", "Rev No", "Approved" };

        DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentProcessRevisionSearch(
                                                                                General.GetNullableString(null),
                                                                                General.GetNullableInteger(null),
                                                                                General.GetNullableInteger(null),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvRiskAssessmentProcessRevision.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentProcessRevision", "Risk Assessment-Process", alCaptions, alColumns, ds);

        gvRiskAssessmentProcessRevision.DataSource = ds;
        gvRiskAssessmentProcessRevision.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Ref. No", "Prepared", "Process", "Activity", "Status", "Rev No", "Approved" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentProcessRevisionSearch(
                                                                                General.GetNullableString(null),
                                                                                General.GetNullableInteger(null),
                                                                                General.GetNullableInteger(null),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                iRowCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentProcessRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentProcessRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
