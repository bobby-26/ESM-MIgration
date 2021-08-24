using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenancesSubWorkOrderReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Submit", "REPORT", ToolBarDirection.Right);
            if (Request.QueryString["attachment"] != null && ViewState["WOATTACHYN"] == null)
                ViewState["WOATTACHYN"] = Request.QueryString["attachment"];
            string isattachment = "attachment.png";
            if(ViewState["WOATTACHYN"] != null && (ViewState["WOATTACHYN"].ToString() == "0" || ViewState["WOATTACHYN"].ToString() == ""))
                isattachment = "no-attachment.png";
            
            toolbargrid.AddImageButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?mimetype=.pdf&dtkey=" + Request.QueryString["groupId"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=WOGRP');", "Attachment Applicable for all Jobs", isattachment, "ATTACHMENT");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                //gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["groupId"] = string.Empty;
                ViewState["UnPlannedJob"] = string.Empty;
                ViewState["PLANDATE"] = string.Empty;
                if (Request.QueryString["groupId"] != null)
                    ViewState["groupId"] = Request.QueryString["groupId"].ToString();

                if (Request.QueryString["UnplannedJob"] != null)
                    ViewState["UnPlannedJob"] = Request.QueryString["UnplannedJob"].ToString();
                ViewState["INP"] = string.Empty;
                ViewState["ISCOMPLETED"] = "1";
                ViewState["WOACTID"] = string.Empty;
                btnConfirm.Attributes.Add("style", "display:none");
                btnCancel.Attributes.Add("style", "display:none");
                bindGroupDetail();
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

            DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderReportList(new Guid(ViewState["groupId"].ToString()));

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

            if (CommandName.ToUpper().Equals("REPORT"))
            {
                if (!IsValidTime())
                {
                    ucError.Visible = true;
                    return;
                }
                byte proceedyn = 0;
                PhoenixPlannedMaintenanceWorkOrderGroup.ValidateReporting(new Guid(ViewState["groupId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
                            ucError.ErrorMessage = "All the jobs should be completed to report routine work order";
                            ucError.Visible = true;
                            return;
                        }
                    }
                    
                    string p = string.Empty;
                    if (!IsValidJobReport(doneStatus, ((UserControlDate)item.FindControl("ucDoneDateEdit")).Text, p))
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

                }                
                PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkOrderReportInsert(strWorkorderId, strDoneStatus, strDoneDate
                                                                            , strComments, strReportId, strRunHour);
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupReportValidation(new Guid(ViewState["groupId"].ToString()), ref proceedyn);
                if (ViewState["ISCOMPLETED"].ToString() == "0")
                    UpdateWorkOrderActivity();
                bindGroupDetail();
                if (proceedyn == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "BookMarkScript", "refreshParent();", true);
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderReportList(new Guid(ViewState["groupId"].ToString()));


            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = ds.Tables[0].Rows.Count;

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
                    cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('reportattachment','" + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?mimetype=.pdf&dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=WORKORDER'); return true;");
                    cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                    if (drv["FLDATTACHMENTYN"].ToString() == "0") cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                    //if (drv["FLDATTACHMENTYN"].ToString() == "0")
                    //{
                    //    HtmlGenericControl html = new HtmlGenericControl();
                    //    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    //    cmdAtt.Controls.Add(html);
                    //}

                    LinkButton lnkDetails = (LinkButton)e.Item.FindControl("lnkDetails");
                    if (drv["FLDATTACHMENTREQYN"].ToString() == "1" && lnkDetails != null && drv["FLDATTINSTRUCTIONS"].ToString() != string.Empty)
                    {
                        RadToolTipManager1.Text = drv["FLDATTINSTRUCTIONS"].ToString();
                        RadToolTipManager1.TargetControls.Add(lnkDetails.ID);
                        //lnkDetails.ToolTip = drv["FLDATTINSTRUCTIONS"].ToString();
                        
                    }
                    else if (drv["FLDATTACHMENTREQYN"].ToString() != "1" && lnkDetails != null)
                    {
                        RadToolTipManager1.TargetControls.Add(lnkDetails.ID);
                        lnkDetails.Visible = false;
                    }                                                
                }
                RadRadioButtonList jobStatus = (RadRadioButtonList)e.Item.FindControl("rblJobDoneStatus");
                if (jobStatus != null)
                {
                    jobStatus.SelectedValue = drv["FLDJOBDONESTATUS"].ToString();
                    if (drv["FLDISDEFECTRAISED"].ToString() == "1")
                    {
                        jobStatus.SelectedValue = "2";
                        jobStatus.Items[1].Text = "Defects ["+ drv["FLDDEFECTNO"].ToString() + "]";
                        jobStatus.Enabled = false;
                    }
                }
                LinkButton tm = (LinkButton)e.Item.FindControl("lnkTemplate");
                if (tm != null)
                {
                    tm.Visible = SessionUtil.CanAccess(this.ViewState, tm.CommandName);
                    if (drv["FLDTEMPLATEMANDATORY"].ToString() == "1" && drv["FLDREPORTFILLED"].ToString() == "0")
                    {
                        tm.Text = "NOT DONE";
                        tm.Enabled = true;
                        tm.Attributes.Add("onclick", "javascript:openNewWindow('template', 'Template - " + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?Workorderid=" + drv["FLDWORKORDERID"] + "&frm=grpwo'); ");
                    }
                    if (drv["FLDREPORTFILLED"].ToString() == "1")
                    {
                        tm.Text = "DONE";
                        tm.Enabled = true;
                        tm.Attributes.Add("onclick", "javascript:openNewWindow('template', 'Template - " + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?Workorderid=" + drv["FLDWORKORDERID"] + "&frm=grpwo'); ");
                    }
                    LinkButton pu = (LinkButton)e.Item.FindControl("lnkpartUsed");
                    if (pu != null)
                    {
                        pu.Visible = SessionUtil.CanAccess(this.ViewState, pu.CommandName);
                        pu.Attributes.Add("onclick", "javascript:openNewWindow('NAFA', '" + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsConsumed.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&COMPNO=" + drv["FLDCOMPONENTNUMBER"] + "'); ");

                    }
                    LinkButton title = (LinkButton)e.Item.FindControl("lnkWorkorderName");
                    if(title != null)
                    {
                        title.Visible = SessionUtil.CanAccess(this.ViewState, title.CommandName);
                        title.Attributes.Add("onclick", "javascript:openNewWindow('jobdetail', '" + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0'); ");
                    }
                    //jobStatus.Enabled = false;
                }
                LinkButton jp = (LinkButton)e.Item.FindControl("lnkParameter");
                if (jp != null)
                {
                    jp.Visible = SessionUtil.CanAccess(this.ViewState, tm.CommandName);
                    if (drv["FLDISJOBPARAMETER"].ToString() == "0" || drv["FLDTEMPLATEMANDATORY"].ToString() == "1")
                    {
                        jp.Text = "NA";
                        jp.Enabled = false;
                    }
                    else if (drv["FLDISJOBPARAMETER"].ToString() == "1" && drv["FLDISPARAMETERFILLED"].ToString() == "0")
                    {
                        jp.Text = "NOT DONE";
                        jp.Enabled = true;
                        jp.Attributes.Add("onclick", "javascript:openNewWindow('parameter', 'Parameter [" + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'","''") + "]', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderParameter.aspx?workorderid=" + drv["FLDWORKORDERID"] + "'); ");
                    }
                    else if(drv["FLDISJOBPARAMETER"].ToString() == "1" && drv["FLDISPARAMETERFILLED"].ToString() == "1")
                    {
                        jp.Text = "DONE";
                        jp.Enabled = true;
                        jp.Attributes.Add("onclick", "javascript:openNewWindow('parameter', 'Parameter [" + drv["FLDWORKORDERNUMBER"] + " - " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "''") + "]', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderParameter.aspx?workorderid=" + drv["FLDWORKORDERID"] + "'); ");
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
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvWorkOrder.Rebind();
    }
    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    foreach (GridDataItem data in gvWorkOrder.MasterTableView.Items)
        //    {
        //        if (data is GridEditableItem)
        //        {
        //            GridEditableItem editItem = data as GridDataItem;
        //            editItem.Edit = true;
        //        }
        //    }
        //    gvWorkOrder.Rebind();
        //}
    }

    protected void gvWorkOrder_ItemCreated(object sender, GridItemEventArgs e)
    {
        //if (!IsPostBack && e.Item is GridEditableItem)
        //{
        //    e.Item.Edit = true;
        //}
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
    private bool IsValidTime()
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (ViewState["INP"].ToString().Equals("0"))
        {
            if(string.IsNullOrEmpty(rblPlanned.SelectedValue))
                ucError.ErrorMessage = "Completed Status is required";
            if (!string.IsNullOrEmpty(rblPlanned.SelectedValue) && rblPlanned.SelectedValue == "4" && txtStartTime.SelectedTime == null)
                ucError.ErrorMessage = "Start Time is required";
        }
        else
        {
            if (string.IsNullOrEmpty(rblInProgress.SelectedValue))
                ucError.ErrorMessage = "Completed Status is required";
        }

        if (txtCompletedTime.SelectedTime == null)
            ucError.ErrorMessage = "Completed Time is required";

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

            string sScript = "javascript:openNewWindow('Defect','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListAdd.aspx?CALLFROM=WORKORDER&WorkorderId=" + workorderId + "&ComponentId=" + componentId + "&ComponentNo=" + compNo + "&refreshframe=maint,RadWindow_NavigateUrl',null,null,null,null,null,{'onClose':function() { deselect('" + radio.ClientID + "');}});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
        }
    }

    private void bindGroupDetail()
    {
        DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroup.EditWorkOrderGroup(new Guid(ViewState["groupId"].ToString()));
        resetGroupDetail();
        if (ds.Tables.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblworkorderNo.Text += dr["FLDWORKORDERNUMBER"].ToString();
            lblCategory.Text += dr["FLDJOBCATEGORY"].ToString();
            lblPlanDate.Text += General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"]);
            lblDuration.Text += dr["FLDPLANNINGESTIMETDURATION"].ToString();
            lblResponsible.Text += dr["FLDDISCIPLINENAME"].ToString();
            lblStatus.Text += dr["FLDSTATUS"].ToString();
            ViewState["WOATTACHYN"] = dr["FLDATTACHMENTYN"].ToString();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (dr["FLDISCOMPLETED"].ToString() == "0")
                toolbargrid.AddButton("Submit", "REPORT", ToolBarDirection.Right);
            if (Request.QueryString["attachment"] != null && ViewState["WOATTACHYN"] == null)
                ViewState["WOATTACHYN"] = Request.QueryString["attachment"];
            string isattachment = "attachment.png";
            if (ViewState["WOATTACHYN"] != null && (ViewState["WOATTACHYN"].ToString() == "0" || ViewState["WOATTACHYN"].ToString() == ""))
                isattachment = "no-attachment.png";

            toolbargrid.AddImageButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?mimetype=.pdf&dtkey=" + Request.QueryString["groupId"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=WOGRP');", "Attachment Applicable for all Jobs", isattachment, "ATTACHMENT");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
        }
        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[1].Rows[0];
            DateTime? start = General.GetNullableDateTime(dr["FLDSTARTTIME"].ToString());
            TimeSpan? time = null;
            if (start != null)
                time = start.Value.TimeOfDay;
            txtStartDate.SelectedDate = start;       
            txtStartTime.SelectedTime = time;
            ViewState["PLANDATE"] = dr["FLDDATE"].ToString();
            DateTime? complete = General.GetNullableDateTime(dr["FLDCOMPLETEDTIME"].ToString());
            if(complete == null)
            {
                txtCompletedDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
                txtCompletedTime.SelectedTime = DateTime.Now.TimeOfDay;
            }
            else
            {
                txtCompletedDate.SelectedDate = complete;
                txtCompletedTime.SelectedTime = complete.Value.TimeOfDay;

                rblPlanned.Enabled = false;
                rblInProgress.Enabled = false;
            }

            if (dr["FLDISINPROGRESS"].ToString().Equals("1") || dr["FLDISPLANCARRYOVER"].ToString().Equals("0"))
            {
                ViewState["INP"] = "1";
                rblPlanned.Visible = false;
                rblInProgress.Visible = true;
                rblInProgress.SelectedValue = "4";
            }
            else
            {
                ViewState["INP"] = "0";
                rblPlanned.Visible = true;
                rblInProgress.Visible = false;
            }
            if (dr["FLDISCOMPLETED"].ToString().Equals("1") || dr["FLDISREPORTPENDING"].ToString().Equals("1"))
            {
                rblPlanned.Enabled = false;
                rblInProgress.Enabled = false;
                rblPlanned.Visible = false;
                rblInProgress.Visible = false;
                txtStartTime.Enabled = false;
                txtCompletedTime.Enabled = false;
                
                if (",1,2,".Contains("," + dr["FLDCOMPLETIONSTATUS"].ToString() + ","))
                {
                    rblPlanned.Visible = true;
                    rblPlanned.SelectedValue = dr["FLDCOMPLETIONSTATUS"].ToString();
                }
                else
                {
                    rblInProgress.Visible = true;
                    rblInProgress.SelectedValue = dr["FLDCOMPLETIONSTATUS"].ToString();
                }
            }
            ViewState["ISCOMPLETED"] = dr["FLDCOMPLETIONSTATUS"].ToString().Equals("") ? "0" : "1";
            ViewState["WOACTID"] = dr["FLDWODETAILID"].ToString();
        }
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
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        PhoenixPlannedMaintenanceWorkOrderGroup.PendingJobRemoveFromWorkorder(new Guid(ViewState["groupId"].ToString()));
        ReportConfirm();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //ReportConfirm();
    }
    private void ReportConfirm()
    {
        foreach (GridDataItem item in gvWorkOrder.Items)
        {
            string workorderId = ((RadLabel)item.FindControl("lblWorkorderId")).Text;
            int jobdoneStatus = int.Parse(((RadRadioButtonList)item.FindControl("rblJobDoneStatus")).SelectedValue);
            if (jobdoneStatus != 3)              // rounds jobs will get report if job report as not done also
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderGroupReportConfirm(new Guid(workorderId));
            }
        }
        if (ViewState["UnPlannedJob"].ToString() == "1") // regular jobs . not rounds jobs
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupComplete(new Guid(ViewState["groupId"].ToString()));
        }
        else
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.PlannedWorkorderAutoGenerate(new Guid(ViewState["groupId"].ToString()));
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
         "BookMarkScript", "refreshParent();", true);
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        foreach (GridDataItem item in gvWorkOrder.Items)
        {
            if (chk.Checked == true)
            {
                RadRadioButtonList rdlist = ((RadRadioButtonList)item.FindControl("rblJobDoneStatus"));
                rdlist.SelectedValue = "1";
            }
            else
            {
                RadRadioButtonList rdlist = ((RadRadioButtonList)item.FindControl("rblJobDoneStatus"));
                rdlist.SelectedValue = "3";
            }
        }
    }

    protected void UpdateWorkOrderActivity()
    {

        string completionstatus = "4";
        if (ViewState["INP"].ToString().Equals("0"))
            completionstatus = rblPlanned.SelectedValue;
        string id = ViewState["WOACTID"].ToString();
        string status = "3";
        Guid? ActivityId = General.GetNullableGuid(id);
        int? Status = General.GetNullableInteger(status);

        DateTime? d = General.GetNullableDateTime(ViewState["PLANDATE"].ToString());
        if (d.HasValue)
        {
            DateTime start = d.Value.Add(txtStartTime.SelectedTime.Value);
            DateTime end = d.Value.Add(txtCompletedTime.SelectedTime.Value);
            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(id)
                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(status), d.Value, General.GetNullableInteger(completionstatus), start, end);
            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ActivityId, 2, General.GetNullableInteger(completionstatus), start, end);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(ActivityId, 2, General.GetNullableInteger(completionstatus));
        }

    }

    protected void rblPlanned_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadRadioButtonList rbl = (RadRadioButtonList)sender;
        if (rbl.SelectedValue == "2")
        {
            txtStartTime.Enabled = true;
            txtCompletedTime.Enabled = true;
        }
        if (rbl.SelectedValue == "1")
        {
            txtStartTime.Enabled = false;
            txtCompletedTime.Enabled = false;
        }
        else
        {
            txtStartTime.Enabled = false;
        }
    }
}
