using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;

public partial class PlannedMaintenancePlannedWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            //toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx", "Add Job", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
            //toolbargrid.AddButton("Report Work", "REPORT", ToolBarDirection.Right);
            toolbargrid.AddButton("Report Work", "REPORTWORK", ToolBarDirection.Right);
            toolbargrid.AddButton("Generate ToolBox", "TOOLBOX", ToolBarDirection.Right);
            toolbargrid.AddButton("Generate", "GENERATE", ToolBarDirection.Right);

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            
            ResetMenu();
            if (!IsPostBack)
            {
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["groupId"] = "";
                if (Request.QueryString["groupId"] != null)
                    ViewState["groupId"] = Request.QueryString["groupId"].ToString();

                
            }
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.PlannedsubWorkOrderList(new Guid(ViewState["groupId"].ToString()), sortexpression, sortdirection,
                             1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CREATE"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComJobListForPlan.aspx?groupId=" + ViewState["groupId"] + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (CommandName.ToUpper().Equals("REPORTWORK"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancePlannedWorkOrderReport.aspx?groupId=" + ViewState["groupId"] + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);

            }
            else if (CommandName.ToUpper().Equals("TOOLBOX"))
            {
               PhoenixPlannedMaintenanceWorkOrderGroup.PlannedJobToolboxCheck(new Guid(ViewState["groupId"].ToString()));
    
               BindData();
               ucStatus.Text = "Tool box meet generated";
            }
            else if (CommandName.ToUpper().Equals("GENERATE"))
            {
                if(ViewState["groupId"] != null)
                    PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkorderIssue(new Guid(ViewState["groupId"].ToString()));
                ucStatus.Text = "Jobs generated";
                BindData();
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.PlannedsubWorkOrderList(new Guid(ViewState["groupId"].ToString()), sortexpression, sortdirection,
                            gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount);

            
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

            BindWorkOrder(ds.Tables[1]);
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
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void gvWorkOrder_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel subJobid = item.FindControl("lblSubJobId") as RadLabel;
                PhoenixPlannedMaintenanceWorkOrderGroup.PlannedSubjobDelete(new Guid(subJobid.Text));
                BindData();
                gvWorkOrder.Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string workorderID = ((RadLabel)item.FindControl("lblWorkorderId")).Text;
                string riskassessId = ((RadTextBox)item.FindControl("txtRAId")).Text;

                if (!IsValidJobReport(riskassessId))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderRAmapping(General.GetNullableGuid(workorderID)
                                                                            , General.GetNullableGuid(riskassessId));
                                                                      
                BindData();
                gvWorkOrder.Rebind();
            }
           else if(e.CommandName.ToUpper().Equals("RA"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string riskassessId = ((RadLabel)item.FindControl("lblRaId")).Text;
                string scriptpopup = string.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + riskassessId + "&showmenu=0&showexcel=NO');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
            }
            else if(e.CommandName.ToUpper().Equals("RACREATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string workorderID = ((RadLabel)item.FindControl("lblWorkorderId")).Text;
                string compRaId = ((RadLabel)item.FindControl("lblCompRaId")).Text;
                string componentId = ((RadLabel)item.FindControl("lblComponentId")).Text;
                string jobId = ((RadLabel)item.FindControl("lblJobId")).Text;
                Guid Raid = Guid.Empty;
                PhoenixPlannedMaintenanceWorkOrderGroup.RaTemplateCopyForPmsJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , new Guid(componentId)
                                                                                , General.GetNullableGuid(jobId)
                                                                                , new Guid(compRaId)
                                                                                , ref Raid
                                                                                , General.GetNullableGuid(workorderID));

               PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderRAmapping(new Guid(workorderID)
                                                                              , Raid);

                ucStatus.Text = "Risk Assessment created";

                BindData();
                gvWorkOrder.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("RAVIEW"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string riskassessId = ((RadLabel)item.FindControl("lblRaId")).Text;
                string scriptpopup = string.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + riskassessId + "&showmenu=0&showexcel=NO');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        gvWorkOrder.Rebind();
    }

    protected void gvWorkOrder_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                LinkButton cmddelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmddelete != null)
                {
                    cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
                }
                LinkButton db = (LinkButton)e.Item.FindControl("lnkJobDetail");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobDetailForWorkOrder.aspx?JOBID=" + drv["FLDJOBID"] + "'); ");
                }
                LinkButton lt = (LinkButton)e.Item.FindControl("lnkTemplates");
                if (lt != null)
                {
                    lt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false");
                    lt.Visible = SessionUtil.CanAccess(this.ViewState, lt.CommandName);
                }
                LinkButton lpu = (LinkButton)e.Item.FindControl("lnkPartRequired");
                if (lpu != null)
                {
                    lpu.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderPartsUsed.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return fa;se");
                    lpu.Visible = SessionUtil.CanAccess(this.ViewState, lpu.CommandName);
                }
                LinkButton jha = (LinkButton)e.Item.FindControl("lnkJHA");
                if (jha != null)
                {
                    jha.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobsJHAlist.aspx?jobId=" + drv["FLDJOBID"] + "'); return false");
                    jha.Visible = SessionUtil.CanAccess(this.ViewState, jha.CommandName);
                }

                LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkRiskCreate");
                LinkButton riskView = (LinkButton)e.Item.FindControl("lnkRiskView");
                if (riskCreate != null && riskView != null)
                {
                    if (string.IsNullOrEmpty(drv["FLDCOMPJOBRAID"].ToString()))
                    {
                        riskView.Visible = true;
                        riskView.Text = "NA";
                        riskView.Enabled = false;
                    }
                    if (!string.IsNullOrEmpty(drv["FLDCOMPJOBRAID"].ToString()) && string.IsNullOrEmpty(drv["FLDRAID"].ToString()))
                    {
                        riskCreate.Visible = true;
                    }
                    if(!string.IsNullOrEmpty(drv["FLDRAID"].ToString()))
                    {
                        riskView.Visible = true;
                    }
                }
                if (e.Item is GridEditableItem)
                {
                    ImageButton imgRA = (ImageButton)e.Item.FindControl("imgShowRA");
                    if (imgRA != null)
                    {
                        imgRA.Attributes.Add("onclick", "return showPickList('spnRA', 'codehelp1', '', '../Common/CommonPickListMachineryRACopyforPmsJob.aspx?catid=3&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&status=4,5&ComponentId=" + drv["FLDCOMPONENTID"] + "', true); ");
                        imgRA.Visible = SessionUtil.CanAccess(this.ViewState, imgRA.CommandName);
                    }
                    RadTextBox txtRanumber = ((RadTextBox)e.Item.FindControl("txtRANumber"));
                    if (txtRanumber != null)
                    {
                        txtRanumber.Text = drv["FLDRANUMBER"].ToString();
                    }
                    RadTextBox txtRaid = ((RadTextBox)e.Item.FindControl("txtRAId"));
                    if (txtRaid != null)
                    {
                        txtRaid.Text = drv["FLDRAID"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("JOBGROUP"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedJobGroup.aspx?groupId=" + ViewState["groupId"]);
            }
            else if (CommandName.ToUpper().Equals("JOBS"))
            {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["groupId"]);
            }
            else if (CommandName.ToUpper().Equals("TOOLBOX"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrderToolBoxMeeting.aspx?groupId=" + ViewState["groupId"]);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Work Order", "JOBGROUP");
        toolbarmain.AddButton("Jobs", "JOBS");

        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        //MenuWorkOrder.SetTrigger(pnlComponent);
        MenuWorkOrder.SelectedMenuIndex = 1;
    }
    private bool IsValidJobReport (string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "RA is required.";
        
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvWorkOrder.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindWorkOrder(DataTable dt)
    {
        ClearWorkOrder();
        DataRow dr = dt.Rows[0];
        lblworkorderNo.Text = lblworkorderNo.Text + " " + dr["FLDWORKORDERNUMBER"].ToString();
        lblCategory.Text = lblCategory.Text + " " + dr["FLDJOBCATEGORY"].ToString();
        lblPlanDate.Text = lblPlanDate.Text + " " + General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
        lblResponsible.Text = lblResponsible.Text + " " + dr["FLDDISCIPLINENAME"].ToString();
        lblStatus.Text = lblStatus.Text + " " + dr["FLDSTATUS"].ToString();
        lblDuration.Text = lblDuration.Text + " " + dr["FLDPLANNINGESTIMETDURATION"].ToString();
    }
    private void ClearWorkOrder()
    {
        lblworkorderNo.Text = "Work order No : ";
        lblCategory.Text = "Category : ";
        lblPlanDate.Text = "Plan Date : ";
        lblResponsible.Text = "Responsibility : ";
        lblStatus.Text = "Status : ";
        lblDuration.Text = "Duration : ";
    }

}
