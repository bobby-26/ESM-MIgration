using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalPTWStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["STATUS"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                ViewState["STATUS"] = Request.QueryString["s"];
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
        DataTable dt = PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), ViewState["STATUS"].ToString()
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
        LinkButton lnkForm = (LinkButton)e.Item.FindControl("lnkForm");
        if (lnkForm != null)
        {
            lnkForm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Work Permits - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&ReportId=" + drv["FLDREPORTID"].ToString() + "'); return false;");
            lnkForm.Visible = SessionUtil.CanAccess(this.ViewState, lnkForm.CommandName);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvForms.Rebind();
    }
}