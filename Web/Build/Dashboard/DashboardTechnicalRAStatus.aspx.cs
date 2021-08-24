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
            ViewState["CODE"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["s"])) {
                ViewState["STATUS"] = Request.QueryString["s"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["c"]))
            {
                ViewState["CODE"] = Request.QueryString["c"];
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
        DataTable dt = PhoenixDashboardTechnical.DashboardNonRoutineRAStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(30)
                                        , ViewState["CODE"].ToString()
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
        //LinkButton frm = (LinkButton)e.Item.FindControl("lnkForm");
        //if (frm != null)
        //{
        //    if (ViewState["STATUS"].ToString() == string.Empty)
        //        frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Forms & Checklist - [" + drv["FLDFROMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&dwpaid=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "'); return false;");
        //    else
        //        frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Forms & Checklist - [" + drv["FLDFROMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&dwpaid=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&ReportId="+ drv["FLDREPORTID"].ToString() + "'); return false;");
        //}
        LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkRisk");
        RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lbltypeid");
        if (riskCreate != null)
        {            
            riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);

            if (lbltypeid.Text == "1")
            {
                riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericExtn.aspx?DashboardYN=1&genericid=" + drv["FLDRAID"].ToString() + "');");
            }
            if (lbltypeid.Text == "2")
            {
                riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationExtn.aspx?DashboardYN=1&navigationid=" + drv["FLDRAID"].ToString() + "');");
            }
            if (lbltypeid.Text == "3")
            {
                riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?DashboardYN=1&machineryid=" + drv["FLDRAID"].ToString() + "');");
            }
            if (lbltypeid.Text == "4")
            {
                riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoExtn.aspx?DashboardYN=1&genericid=" + drv["FLDRAID"].ToString() + "');");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvForms.Rebind();
    }
}