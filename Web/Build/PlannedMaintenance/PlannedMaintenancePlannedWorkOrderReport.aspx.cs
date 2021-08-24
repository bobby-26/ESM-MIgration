using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class PlannedMaintenancePlannedWorkOrderReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();


            toolbargrid.AddButton("Submit", "REPORT", ToolBarDirection.Right);

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                btnConfirm.Attributes.Add("style", "display:none");

                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["groupId"] = String.Empty;
                ViewState["UnPlannedJob"] = string.Empty;
                if (Request.QueryString["groupId"] != null)
                    ViewState["groupId"] = Request.QueryString["groupId"].ToString();
                if (Request.QueryString["UnplannedJob"] != null)
                    ViewState["UnPlannedJob"] = Request.QueryString["UnplannedJob"].ToString();



            }
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
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CREATE"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComJobListForPlan.aspx?groupId=" + ViewState["groupId"] + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                byte proceedyn = 0;

                string strWorkorderId = ",";
                string strDoneStatus = ",";
                string strDoneDate = ",";
                string strComments = ",";
                string strReportId = ",";
                string strRunHour = ",";
                foreach (GridDataItem item in gvWorkOrder.Items)
                {
                    int doneStatus = int.Parse(((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue);
                    if (ViewState["UnPlannedJob"].ToString() != "1")
                    {
                        if (doneStatus == 3)
                        {
                            ucError.ErrorMessage = "All the jobs should be completed to report planned work order";
                            ucError.Visible = true;
                            return;
                        }
                    }
                    UserControlJobParameterValue param = ((UserControlJobParameterValue)item.FindControl("ucParameter"));
                    string p = param.IsValidParameter;
                    if (!IsValidJobReport(int.Parse(((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue)
                                        , ((UserControlDate)item.FindControl("ucDoneDateEdit")).Text, p))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    strWorkorderId += ((RadLabel)item.FindControl("lblWorkorderId")).Text + ",";
                    strDoneStatus += ((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue + ",";
                    strDoneDate += ((UserControlDate)item.FindControl("ucDoneDateEdit")).Text + ",";
                    strComments += ((RadTextBox)item.FindControl("txtCommentEdit")).Text + ",";
                    strReportId += ((RadLabel)item.FindControl("lblReportId")).Text + ",";
                    strRunHour += "" + ",";

                    if (doneStatus != 3)
                    {
                        param.Save(new Guid(((RadLabel)item.FindControl("lblWorkorderId")).Text));
                    }
                }
                PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkOrderReportInsert(strWorkorderId, strDoneStatus, strDoneDate
                                                                            , strComments, strReportId, strRunHour);
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupReportValidation(new Guid(ViewState["groupId"].ToString()), ref proceedyn);
                if (proceedyn == 0)
                {
                    return;
                }
                if (proceedyn == 2 && ViewState["UnPlannedJob"].ToString() == "1")
                {
                    RadWindowManager1.RadConfirm("Some jobs are not done. Do you want to remove those from workorder?", "confirm", 320, 150, null, "Confirm");
                }
                else
                {
                    ReportConfirm();
                }

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
            gvWorkOrder.Rebind();
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
                ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
                if (cmdAtt != null)
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "'); return false;");
                    cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                    if (drv["FLDATTACHMENTYN"].ToString() == "0") cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                }
                RadRadioButtonList jobStatus = (RadRadioButtonList)e.Item.FindControl("rblJobDoneStatus");
                if (jobStatus != null)
                {
                    jobStatus.SelectedValue = drv["FLDJOBDONESTATUS"].ToString();
                    //jobStatus.Enabled = false;
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

            if (CommandName.ToUpper().Equals("TOOLBOX"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrderToolBoxMeeting.aspx?groupId=" + ViewState["groupId"]);
            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrderReport.aspx?groupId=" + ViewState["groupId"]);
            }

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
        lblworkorderNo.Text = lblworkorderNo.Text + dr["FLDWORKORDERNUMBER"].ToString();
        lblCategory.Text = lblCategory.Text + dr["FLDJOBCATEGORY"].ToString();
        lblPlanDate.Text = lblPlanDate.Text + General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"]);
        lblDuration.Text = lblDuration.Text + dr["FLDPLANNINGESTIMETDURATION"].ToString();
        lblResponsible.Text = lblResponsible.Text + dr["FLDDISCIPLINENAME"].ToString();
        lblStatus.Text = lblStatus.Text + dr["FLDSTATUS"].ToString();
    }

    private void resetGroupDetail()
    {
        lblworkorderNo.Text = "WORK ORDER NO : ";
        lblCategory.Text = "CATEGORY : ";
        lblPlanDate.Text = "PLAN DATE : ";
        lblDuration.Text = "DURATION : ";
        lblResponsible.Text = "ASSIGNED TO : ";
        lblStatus.Text = "STATUS : ";
    }
    private bool IsValidJobReport(int doneStatus, string doneDate, string param)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (string.IsNullOrEmpty(doneDate) && doneStatus != 3)
            ucError.ErrorMessage = "Done date is required.";

        DateTime? reportDate = General.GetNullableDateTime(doneDate);
        DateTime currentDate = DateTime.Now.Date;
        if (!string.IsNullOrEmpty(doneDate) && doneStatus != 3 && (DateTime.Compare(reportDate.Value, currentDate) > 0))
            ucError.ErrorMessage = "Done date should not be greater than current date.";

        if (param.Trim().Length > 0 && doneStatus != 3)
            ucError.ErrorMessage = param;

        return (!ucError.IsError);
    }
    protected void rblJobDoneStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectvalue = (sender as RadRadioButtonList).SelectedValue;
        var radio = sender as RadRadioButtonList;
        GridDataItem dataItem = radio.Parent.Parent as GridDataItem;
        if (selectvalue == "2")
        {
            var workorderId = ((RadLabel)dataItem.FindControl("lblWorkorderId")).Text;
            var componentId = ((RadLabel)dataItem.FindControl("lblComponentId")).Text;
            var compNo = ((RadLabel)dataItem.FindControl("lblCompNo")).Text;

            string sScript = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListAdd.aspx?WorkorderId=" + workorderId + "&ComponentId=" + componentId + "&ComponentNo="+ compNo + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {

        PhoenixPlannedMaintenanceWorkOrderGroup.PendingJobRemoveFromWorkorder(new Guid(ViewState["groupId"].ToString()));

        ReportConfirm();
    }
    private void ReportConfirm()
    {
        foreach (GridDataItem item in gvWorkOrder.Items)
        {
            string workorderId = ((RadLabel)item.FindControl("lblWorkorderId")).Text;
            int jobdoneStatus = int.Parse(((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue);
            if (jobdoneStatus != 3)              
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderGroupReportConfirm(new Guid(workorderId));
            }
        }
        if (ViewState["UnPlannedJob"].ToString() == "1") // regular jobs . not round jobs
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupComplete(new Guid(ViewState["groupId"].ToString()));
        }
        else
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkorderAutoGenerate(new Guid(ViewState["groupId"].ToString()));
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
         "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
    }
}
