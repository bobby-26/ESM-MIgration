using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAGenericRevisionListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAGenericRevisionListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentGenericRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["GENERICID"] = "";
            if (Request.QueryString["genericid"] != null && Request.QueryString["genericid"].ToString() != "")
                ViewState["GENERICID"] = Request.QueryString["genericid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvRiskAssessmentGenericRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentGenericRevision.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionRiskAssessmentGenericRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentGenericRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentGenericRevision", "Generic Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentGenericRevision.DataSource = ds;
        gvRiskAssessmentGenericRevision.VirtualItemCount = iRowCount;

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

            DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionRiskAssessmentGenericRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Generic Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }


    protected void gvRiskAssessmentGenericRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
            LinkButton lnkWorkActivity = (LinkButton)e.Item.FindControl("lnkWorkActivity");
            if (lnkWorkActivity != null)
            {
                lnkWorkActivity.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericExtn.aspx?genericid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
            }            
        }
    }

    protected void gvRiskAssessmentGenericRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentGenericRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
