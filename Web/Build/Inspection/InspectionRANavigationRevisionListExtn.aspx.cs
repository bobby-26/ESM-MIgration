using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRANavigationRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRANavigationRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentNavigationRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["NAVIGATIONID"] = "";
            if (Request.QueryString["navigationid"] != null && Request.QueryString["navigationid"].ToString() != "")
                ViewState["NAVIGATIONID"] = Request.QueryString["navigationid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvRiskAssessmentNavigationRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentNavigationExtn.InspectionRiskAssessmentNavigationRevisionSearch(
                    new Guid(ViewState["NAVIGATIONID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentNavigationRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentNavigationRevision", "Navigation Revisions", alCaptions, alColumns, ds);
        gvRiskAssessmentNavigationRevision.DataSource = ds;
        gvRiskAssessmentNavigationRevision.VirtualItemCount = iRowCount;

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

            DataSet ds = PhoenixInspectionRiskAssessmentNavigationExtn.InspectionRiskAssessmentNavigationRevisionSearch(
                    new Guid(ViewState["NAVIGATIONID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Navigation Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

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

    protected void gvRiskAssessmentNavigationRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentNavigationID");
            LinkButton lnkWorkActivity = (LinkButton)e.Item.FindControl("lnkWorkActivity");
            if (lnkWorkActivity != null)
            {
                lnkWorkActivity.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationExtn.aspx?navigationid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRiskAssessmentNavigationRevision.Rebind();
    }
    protected void gvRiskAssessmentNavigationRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentNavigationRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
