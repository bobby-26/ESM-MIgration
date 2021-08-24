using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
public partial class DashboardTechnicalOfficeCertificatelist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalOfficeCertificatelist.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalOfficeCertificatelist.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuCertificate.AccessRights = this.ViewState;
            MenuCertificate.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["Due"] = string.Empty;
                ViewState["vslid"] = string.Empty;
                if(!string.IsNullOrEmpty(Request.QueryString["Due"]))
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
            string[] alColumns = { "FLDVESSELNAME", "FLDCERTIFICATENAME", "FLDDUEDATE", "FLDDATEOFEXPIRY" };
            string[] alCaptions = { "Vessel", "Certificate", "Due Date", "Expiry Date" };
            string due = Request.QueryString["DUE"];
            string category = Request.QueryString["CAT"];
            string heading = Request.QueryString["title"];            
            DataTable dt;
            if (!string.IsNullOrEmpty(Request.QueryString["DUE"]))
            {
                dt = PhoenixDashboardTechnical.DashboardOfficeCertificateDueList(int.Parse(Request.QueryString["DUE"]),
                                                                                    Request.QueryString["CAT"],
                                                                                    null,1,
                                                                                    gvCertificate.CurrentPageIndex + 1,
                                                                                    gvCertificate.PageSize,
                                                                                    ref iRowCount,ref iTotalPageCount
                                                                                    ,General.GetNullableInteger(ViewState["vslid"].ToString()));
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions(gvCertificate.ClientID, heading, alCaptions, alColumns, ds);
                gvCertificate.DataSource = dt;
                gvCertificate.VirtualItemCount = iRowCount;
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
                cc.Attributes.Add("onclick", "javascript:openNewWindow('TOOLBOX', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx?vesselId=" + drv["FLDVESSELID"]+"&CID=" + drv["FLDCERTIFICATEID"] + "'); ");
                cc.Visible = SessionUtil.CanAccess(this.ViewState, cc.CommandName);
            }
        }
    }

    protected void MenuCertificate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDCERTIFICATENAME", "FLDDUEDATE", "FLDDATEOFEXPIRY" };
                string[] alCaptions = { "Vessel", "Certificate", "Due Date", "Expiry Date" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                int dueinterval = 0;
                if(!string.IsNullOrEmpty(Request.QueryString["DUE"]))
                {
                    dueinterval = int.Parse(Request.QueryString["DUE"]);
                }
                DataTable dt = PhoenixDashboardTechnical.DashboardOfficeCertificateDueList(dueinterval,
                                                                                    Request.QueryString["CAT"],
                                                                                    null, 1,
                                                                                    1,
                                                                                    iRowCount,
                                                                                    ref iRowCount, ref iTotalPageCount
                                                                                    , General.GetNullableInteger(ViewState["vslid"].ToString()));
                string due = Request.QueryString["DUE"];
                string category = Request.QueryString["CAT"];
                string heading = Request.QueryString["title"];
                
                General.ShowExcel(heading, dt, alColumns, alCaptions, null, null);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
