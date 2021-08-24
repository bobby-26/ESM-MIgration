using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalRA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["STATUS"] = string.Empty;
            if(!string.IsNullOrEmpty(Request.QueryString["s"])) {
                ViewState["STATUS"] = Request.QueryString["s"];
            }
        }
    }

    protected void gvForms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixDashboardTechnical.DashboardNonRoutineRASearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7)
                                        , sortexpression, sortdirection
                                        , grid.CurrentPageIndex + 1
                                        , grid.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);

        grid.DataSource = dt;
        grid.VirtualItemCount = iRowCount;
    }

    protected void gvForms_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton frm = (LinkButton)e.Item.FindControl("lnkForm");
        if (frm != null)
        {
            string vslid = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                vslid = "&vslid=" + drv["FLDVESSELID"].ToString();
            }
            frm.Attributes.Add("onclick", "javascript:openNewWindow('maint','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + vslid + "'); return false;");
        }
        LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkRiskCreate");
        if (riskCreate != null)
        {
            riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + drv["FLDCOMPONENTID"].ToString() + "&GroupId=" + drv["FLDWORKORDERGROUPID"].ToString() + "&ispopup=ra,DailyWorkPlan');return false;");
            riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvForms.Rebind();
    }
}