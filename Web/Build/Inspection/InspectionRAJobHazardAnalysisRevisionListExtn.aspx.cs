using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAJobHazardAnalysisRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvJHARevisions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuJobHazardAnalysis.AccessRights = this.ViewState;
        MenuJobHazardAnalysis.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["JOBHAZARDID"] = "";

            
            if (Request.QueryString["referenceid"] != null && Request.QueryString["referenceid"].ToString() != "")
            {
                ViewState["REFNO"] = Request.QueryString["referenceid"].ToString();
            }

            gvJHARevisions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHAZARDNUMBER", "FLDVESSELNAME", "FLDPROCESS", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Hazard Number", "Vessel", "Process", "Job", "Status", "Revision No", "Issued Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardRevisionSearch(
                    General.GetNullableString(null),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(null), 
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvJHARevisions.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null);

        General.SetPrintOptions("gvJHARevisions", "Job Hazard Analysis Revisions", alCaptions, alColumns, ds);

        gvJHARevisions.DataSource = ds;
        gvJHARevisions.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDHAZARDNUMBER","FLDVESSELNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Hazard Number","Vessel", "Element", "Job", "Status", "Revision No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardRevisionSearch(
                    General.GetNullableString(null),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(null),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null);

            General.ShowExcel("Job Hazard Analysis Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuJobHazardAnalysis_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisExtn.aspx?status=", false);
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            gvJHARevisions.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            gvJHARevisions.Rebind();
        }
    }

    protected void gvJHARevisions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJHARevisions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
