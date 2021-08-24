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

public partial class PlannedMaintenancePlannedWorkOrderToolBoxMeeting : PhoenixBasePage
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
            toolbargrid.AddButton("Save Report", "REPORT", ToolBarDirection.Right);

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SAVECLICK"] = "0";
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

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkOrderReportList(new Guid(ViewState["groupId"].ToString()));

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
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                foreach (GridDataItem item in gvWorkOrder.Items)
                {

                    string workorderId = ((RadLabel)item.FindControl("lblWorkorderId")).Text;
                    string doneStatus = ((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue;
                    string doneDate = ((UserControlDate)item.FindControl("ucDoneDateEdit")).Text;
                    string comments = ((RadTextBox)item.FindControl("txtCommentEdit")).Text;
                    string reportId = ((RadLabel)item.FindControl("lblReportId")).Text;

                    if (!IsValidJobReport(doneStatus, doneDate, comments))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (ViewState["groupId"] != null)
                    {
                        PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkOrderToolBoxInsert(new Guid(workorderId)
                                                                                    , int.Parse(doneStatus)
                                                                                    , General.GetNullableDateTime(doneDate)
                                                                                    , comments
                                                                                    , new Guid(ViewState["groupId"].ToString())
                                                                                    , General.GetNullableGuid(reportId));
                    }

                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkOrderReportList(new Guid(ViewState["groupId"].ToString()));


            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

            bindGroupDetail(ds.Tables[1]);

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
                LinkButton lt = (LinkButton)e.Item.FindControl("lnkTemplates");
                if (lt != null)
                {
                    lt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false");
                    lt.Visible = SessionUtil.CanAccess(this.ViewState, lt.CommandName);
                }
                LinkButton lpu = (LinkButton)e.Item.FindControl("lnkPartUsed");
                if (lpu != null)
                {
                    lpu.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsUsed.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false");
                    lpu.Visible = SessionUtil.CanAccess(this.ViewState, lpu.CommandName);
                }
                LinkButton cmdAtt = (LinkButton)e.Item.FindControl("cmdAtt");
                if (cmdAtt != null)
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "'); return false;");
                    cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                }
                RadRadioButtonList jobStatus = (RadRadioButtonList)e.Item.FindControl("rblJobDoneStatus");
                if (jobStatus != null)
                {
                    jobStatus.SelectedValue = drv["FLDJOBDONESTATUS"].ToString();
                    if (!string.IsNullOrEmpty(drv["FLDWORKORDERREPORTID"].ToString()))
                    {
                        jobStatus.Enabled = false;
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

    //protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("JOBGROUP"))
    //        {
    //            Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedJobGroup.aspx?groupId=" + ViewState["groupId"]);
    //        }
    //        if (CommandName.ToUpper().Equals("JOBS"))
    //        {
    //            Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["groupId"]);
    //        }
    //        if (CommandName.ToUpper().Equals("TOOLBOX"))
    //        {
    //            Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrderToolBoxMeeting.aspx?groupId=" + ViewState["groupId"]);
    //        }
    //        if (CommandName.ToUpper().Equals("REPORT"))
    //        {
    //            Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrderReport.aspx?groupId=" + ViewState["groupId"]);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool IsValidJobReport(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";

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


    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (GridDataItem data in gvWorkOrder.MasterTableView.Items)
            {
                if (data is GridEditableItem)
                {
                    GridEditableItem editItem = data as GridDataItem;
                    editItem.Edit = true;
                }
            }
            gvWorkOrder.Rebind();
        }
    }

    protected void gvWorkOrder_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (!IsPostBack && e.Item is GridEditableItem)
        {
            e.Item.Edit = true;
        }
    }
    private void bindGroupDetail(DataTable dt)
    {
        resetGroupDetail();
        DataRow dr = dt.Rows[0];
        lblworkorderID.Text = lblworkorderID.Text + dr["FLDWORKORDERNUMBER"].ToString();
        lblCategory.Text = lblCategory.Text + dr["FLDCOMPCATEGORY"].ToString();
        lblPlanDate.Text = lblPlanDate.Text + General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"]);
        string duration = dr["FLDPLANNINGESTIMETDURATION"].ToString() != "" ? dr["FLDPLANNINGESTIMETDURATION"].ToString() + " hrs" : "";
        lblDuration.Text = lblDuration.Text + duration;
        //lblDuration.Text = lblDuration.Text + dr["FLDPLANNINGESTIMETDURATION"].ToString();
        lblResponsible.Text = lblResponsible.Text + dr["FLDDISCIPLINENAME"].ToString();
        lblStatus.Text = lblStatus.Text + dr["FLDSTATUS"].ToString();
    }

    private void resetGroupDetail()
    {
        lblworkorderID.Text = "WORK ORDER NO : ";
        lblCategory.Text = "CATEGORY : ";
        lblPlanDate.Text = "PLAN DATE : ";
        lblDuration.Text = "DURATION : ";
        lblResponsible.Text = "ASSIGNED TO : ";
        lblStatus.Text = "STATUS : ";
    }
    private bool IsValidJobReport(string doneStatus, string doneDate, string comments)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (doneStatus.Trim().Equals(""))
            ucError.ErrorMessage = "Job done is required.";

        if (string.IsNullOrEmpty(doneDate))
            ucError.ErrorMessage = "Done date is required.";

        if (comments.Trim().Equals(""))
            ucError.ErrorMessage = "Comments is required.";

        return (!ucError.IsError);
    }
    protected void rblJobDoneStatus_SelectedIndexChanged1(object sender, EventArgs e)
    {
        string selectvalue = (sender as RadRadioButtonList).SelectedValue;
        var radio = sender as RadRadioButtonList;
        GridDataItem dataItem = radio.Parent.Parent as GridDataItem;
        if (selectvalue == "2")
        {
            var workorderId = ((RadLabel)dataItem.FindControl("lblWorkorderId")).Text;

            string sScript = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancesWorkOrderDefectReport.aspx?WorkOrderId=" + workorderId + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
        }

    }
}
