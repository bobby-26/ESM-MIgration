using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRACargoRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentCargoRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCargo.AccessRights = this.ViewState;
        MenuCargo.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["GENERICID"] = "";
            if (Request.QueryString["genericid"] != null && Request.QueryString["genericid"].ToString() != "")
                ViewState["GENERICID"] = Request.QueryString["genericid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvRiskAssessmentCargoRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentCargoExtn.InspectionRiskAssessmentCargoRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentCargoRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentCargoRevision", "Cargo Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentCargoRevision.DataSource = ds;
        gvRiskAssessmentCargoRevision.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentCargoRevision.Rebind();
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

            DataSet ds = PhoenixInspectionRiskAssessmentCargoExtn.InspectionRiskAssessmentCargoRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Cargo Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentCargoRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentCargoID");
            LinkButton lnkWorkActivity = (LinkButton)e.Item.FindControl("lnkWorkActivity");
            if (lnkWorkActivity != null)
            {
                lnkWorkActivity.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoExtn.aspx?genericid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
            }
        }
    }

    protected void gvRiskAssessmentCargoRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentCargoRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
