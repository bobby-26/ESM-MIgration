using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Owners;

public partial class OwnerReportCertificatelist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["Due"] = string.Empty;
                ViewState["vslid"] = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Due"]))
                {
                    ViewState["Due"] = Request.QueryString["Due"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
                {
                    ViewState["vslid"] = Request.QueryString["vslid"];
                }
                gvCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

            DataTable dt;
            if (!string.IsNullOrEmpty(Request.QueryString["DUE"]))
            {
                dt = PhoenixOwnerReportPMS.CertificateDueList(Request.QueryString["CAT"],
                                                              null, 1,
                                                              gvCertificate.CurrentPageIndex + 1,
                                                              gvCertificate.PageSize,
                                                              ref iRowCount, ref iTotalPageCount
                                                              , General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
                                                              , General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
                gvCertificate.DataSource = dt;
                gvCertificate.VirtualItemCount = iRowCount;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton cc = (LinkButton)e.Item.FindControl("lnkCertificate");
            if (cc != null)
            {
                cc.Attributes.Add("onclick", "javascript:openNewWindow('TOOLBOX', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx?vesselId=" + drv["FLDVESSELID"] + "&CID=" + drv["FLDCERTIFICATEID"] + "'); ");
                cc.Visible = SessionUtil.CanAccess(this.ViewState, cc.CommandName);
            }
        }
    }
}