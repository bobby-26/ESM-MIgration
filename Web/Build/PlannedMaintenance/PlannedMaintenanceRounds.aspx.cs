using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRounds : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceRounds.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRounds')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivComponentJob.MenuList = toolbargrid.Show();
            //MenuDivComponentJob.SetTrigger(pnlComponent);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Job Description", "DETAILS");
                toolbarmain.AddButton("Jobs", "JOBS");

                MenuComponentJob.MenuList = toolbarmain.Show();
                MenuComponentJob.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTJOBID"] = null;
                ViewState["JOBID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["COMPONENTJOBID"] != null)
                    ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();

                gvRounds.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            NameValueCollection nvc = Filter.CurrentJobFilter;
            DataSet ds = PhoenixPlannedMaintenanceComponentJob.RoundsSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , nvc != null ? nvc["txtJobCode"] : null
                                                                            , nvc != null ? nvc["txtJobTitle"] : null
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ucJobClass"] : null)
                                                                            , sortexpression, sortdirection
                                                                            , gvRounds.CurrentPageIndex + 1
                                                                            , gvRounds.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
            General.ShowExcel("Rounds", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRounds.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL") || ViewState["COMPONENTJOBID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceRoundsGeneral.aspx";
                
            }
            if (CommandName.ToUpper().Equals("JOBS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceRoundsJob.aspx";
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobDetail.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?JOBID=" + ViewState["JOBID"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + (ViewState["COMPONENTJOBID"] != null ? "?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] : string.Empty);
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

            NameValueCollection nvc = Filter.CurrentJobFilter;
            DataSet ds = PhoenixPlannedMaintenanceComponentJob.RoundsSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , nvc != null ? nvc["txtJobCode"] : null
                                                                            , nvc != null ? nvc["txtJobTitle"] : null
                                                                            , General.GetNullableInteger(nvc != null ? nvc["ucJobClass"] : null)
                                                                            , sortexpression, sortdirection
                                                                            , gvRounds.CurrentPageIndex + 1
                                                                            , gvRounds.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
            General.SetPrintOptions("gvRounds", "Rounds", alCaptions, alColumns, ds);

            gvRounds.DataSource = ds;
            gvRounds.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {

                ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTJOBID"].ToString();
                ViewState["JOBID"] = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                BindUrl();
            }
            //    if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            //    {
            //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceRoundsGeneral.aspx";
            //    }

            //    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];

            //    SetTabHighlight();
            //    //BindComponentData();
            //}
            //else
            //{
            //    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceRoundsGeneral.aspx";
            //}


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRounds_SortCommand(object source, GridSortCommandEventArgs e)
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

    protected void gvRounds_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRounds.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceRoundsGeneral.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobDetail.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceRoundsJob.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 2;
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindComponentData()
    {
        if (ViewState["COMPONENTJOBID"] != null)
        {
            DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(ViewState["COMPONENTJOBID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
        }
    }

    protected void gvRounds_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceComponentJob.DeleteComponentJob(new Guid(((RadLabel)e.Item.FindControl("lblComponentJobId")).Text));
                ViewState["COMPONENTJOBID"] = null;
                gvRounds.Rebind();
            }
            if(e.CommandName.ToUpper().Equals("SELECT") || e.CommandName == "RowClick")
            {
                ViewState["COMPONENTJOBID"] = ((RadLabel)e.Item.FindControl("lblComponentJobId")).Text;
                ViewState["JOBID"] = ((RadLabel)e.Item.FindControl("lbljobId")).Text;
                BindUrl();
            }
            if (e.CommandName.ToUpper().Equals("REPORT"))
            {
                string workorderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text; 
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderRoundsReport.aspx?WORKORDERID=" + workorderId + "&WORKORDERNO=");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRounds_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvRounds_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton rpt = (LinkButton)e.Item.FindControl("cmdReport");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton lbJobTitle = (LinkButton)e.Item.FindControl("lnkStockItemName");
                string workorderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                if(rpt != null && string.IsNullOrEmpty(workorderId))
                {
                    rpt.Visible = SessionUtil.CanAccess(this.ViewState, rpt.CommandName);
                    rpt.Visible = false;
                }
                UserControlToolTip uctJobTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipJobTitle");
                uctJobTitle.Position = ToolTipPosition.TopCenter;
                uctJobTitle.TargetControlId = lbJobTitle.ClientID;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindUrl()
    {
        if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceRoundsGeneral.aspx";
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];
        }
        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceJobDetail.aspx"))
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?JOBID=" + ViewState["JOBID"];
        }
        else
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];
        }
        SetTabHighlight();
    }
}
