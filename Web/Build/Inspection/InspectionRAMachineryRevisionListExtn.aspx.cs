using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAMachineryRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMachineryRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentMachineryRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["MACHINERYID"] = "";
            if (Request.QueryString["machineryid"] != null && Request.QueryString["machineryid"].ToString() != "")
                ViewState["MACHINERYID"] = Request.QueryString["machineryid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvRiskAssessmentMachineryRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.InspectionRiskAssessmentMachineryRevisionSearch(
                    new Guid(ViewState["MACHINERYID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentMachineryRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentMachineryRevision", "Machinery Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentMachineryRevision.DataSource = ds;
        gvRiskAssessmentMachineryRevision.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.InspectionRiskAssessmentMachineryRevisionSearch(
                    new Guid(ViewState["MACHINERYID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Machinery Revisions", ds.Tables[0], alColumns, alCaptions, null, null);
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMachinery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvRiskAssessmentMachineryRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentMachineryID");
            LinkButton lnkWorkActivity = (LinkButton)e.Item.FindControl("lnkWorkActivity");
            if (lnkWorkActivity != null)
            {
                //lnkWorkActivity.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
                lnkWorkActivity.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentMachineryRevision.Rebind();
    }
    protected void gvRiskAssessmentMachineryRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentMachineryRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
