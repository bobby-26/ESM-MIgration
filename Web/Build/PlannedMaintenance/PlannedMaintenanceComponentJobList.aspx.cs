using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx?p=" + Request.QueryString["p"] + (Request.QueryString["tv"] != null ? "&tv=1" : string.Empty), "Add Component Job", "<i class=\"fas fa-plus\"></i>", "Add");

            MenuDivComponentJob.AccessRights = this.ViewState;
            MenuDivComponentJob.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    divCancelledjob.Attributes.Add("style", "visibility:hidden");
                }
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTID"] = null;
                ViewState["COMPONENTJOBID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();

                if (Request.QueryString["COMPONENTJOBID"] != null)
                {
                    ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                }
                BindComponentData();
                gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = { "Job Code", "Job Title", "Frequency", "Last Done date", "Priority", "Resp Discipline", "Next Due Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            int cancelledjob = chkCancelledjob.Checked == true ? 1 : 0;

            DataSet ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), sortexpression, sortdirection,
                       gvComponentJob.CurrentPageIndex + 1,
                        gvComponentJob.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount
                        , cancelledjob);
            General.ShowExcel("Component - Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivComponentJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                gvComponentJob.CurrentPageIndex = 0;
                gvComponentJob.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "JOBMODE=AddJob&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&p=" + Request.QueryString["p"] + "';  </script>");
                //Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString());
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

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = { "Job Code", "Job Title", "Frequency", "Last Done date", "Priority", "Resp Discipline", "Next Due Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            int cancelledjob = chkCancelledjob.Checked == true ? 1 : 0;

            DataSet ds;
            ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), sortexpression, sortdirection,
                         gvComponentJob.CurrentPageIndex + 1,
                         gvComponentJob.PageSize,
                         ref iRowCount,
                         ref iTotalPageCount,
                         cancelledjob);
            General.SetPrintOptions("gvComponentJob", "Component - Job", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponentJob.DataSource = ds;
                gvComponentJob.VirtualItemCount = iRowCount;

                if (ViewState["COMPONENTJOBID"] == null)
                {
                    ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTJOBID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvComponentJob.DataSource = "";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            gvComponentJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindComponentData()
    {
        if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
        {
            PhoenixToolbar toolbargrid1 = new PhoenixToolbar();
            DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
            TabStrip1.Title =  "Component - Job ( " + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + ")";
            TabStrip1.AccessRights = this.ViewState;
            TabStrip1.MenuList = toolbargrid1.Show();
        }
    }

    protected void chkCancelledjob_CheckedChanged(object sender, EventArgs e)
    {
        gvComponentJob.Rebind();
    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceComponentJob.DeleteComponentJob(new Guid(((RadLabel)e.Item.FindControl("lblComponentJobId")).Text));
                ViewState["COMPONENTJOBID"] = null;
                gvComponentJob.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                string JobId = ((RadLabel)e.Item.FindControl("lblJobID")).Text.ToString();
                string CompJobId = ((RadLabel)e.Item.FindControl("lblComponentJobId")).Text.ToString();
                string CompId = ((RadLabel)e.Item.FindControl("lblComponentId")).Text.ToString();
                string CancelledJob = ((RadLabel)e.Item.FindControl("lblCnacelledJob")).Text.ToString();

                LinkButton lbJobTitle = (LinkButton)e.Item.FindControl("lnkStockItemName");
                UserControlToolTip uctJobTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipJobTitle");
                uctJobTitle.Position = ToolTipPosition.TopCenter;
                uctJobTitle.TargetControlId = lbJobTitle.ClientID;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }

                LinkButton jd = (LinkButton)e.Item.FindControl("cmdJobDesc");
                if (jd != null)
                {
                    jd.Attributes.Add("onclick", "return openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?framename=ifMoreInfo&JOBID=" + JobId + "', true);");
                    jd.Visible = SessionUtil.CanAccess(this.ViewState, jd.CommandName);
                }
                LinkButton vw = (LinkButton)e.Item.FindControl("cmdView");
                if (vw != null)
                {
                    vw.Attributes.Add("onclick", "window.parent.location.href='../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTJOBID=" + CompJobId+ "&COMPONENTID=" + CompId + "&p=" + Request.QueryString["p"] + "&Cancelledjob=" + CancelledJob + "'");
                    vw.Visible = SessionUtil.CanAccess(this.ViewState, vw.CommandName);
                }
                LinkButton form = (LinkButton)e.Item.FindControl("cmdform");
                if (form != null)
                {
                    form.Visible = SessionUtil.CanAccess(this.ViewState, "MAINFORM");
                    form.Attributes.Add("onclick", "javascript:parent.openNewWindow('MaintenanceForm','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobFormMapList.aspx?componentjobid=" + CompJobId + "'); return false;");
                }
                LinkButton cmdManuals = (LinkButton)e.Item.FindControl("cmdManuals");
                if (cmdManuals != null)
                {
                    cmdManuals.Visible = SessionUtil.CanAccess(this.ViewState, cmdManuals.CommandName);
                    cmdManuals.Attributes.Add("onclick", "javascript:parent.openNewWindow('Manuals','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + CompJobId + "&JOBYN=1'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_SortCommand(object source, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
