using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionShipBoardTasksDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        

        if (Request.QueryString["OVERDUEYN"] != null && Request.QueryString["OVERDUEYN"].ToString() != "")
            ViewState["OVERDUEYN"] = Request.QueryString["OVERDUEYN"].ToString();
        else
            ViewState["OVERDUEYN"] = "0";

        if (Request.QueryString["CallFrom"] != null && Request.QueryString["CallFrom"].ToString() != "")
        {
            MenuShipboardGeneral.Visible = false;
        }

        try
        {
            if (!IsPostBack)
            {
                ViewState["ATTACHMENTDELETELOCKYN"] = "0";
                ViewState["RECENTRESCHEDULEDATE"] = "";
                ViewState["DASHBOARD"] = "";
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                if (Request.QueryString["DASHBOARD"] != null && Request.QueryString["DASHBOARD"].ToString() != "")
                    ViewState["DASHBOARD"] = Request.QueryString["DASHBOARD"].ToString();
                else
                    ViewState["DASHBOARD"] = "";

                if (Request.QueryString["OFFICEDASHBOARD"] != null && Request.QueryString["OFFICEDASHBOARD"].ToString() != "")
                    ViewState["OFFICEDASHBOARD"] = Request.QueryString["OFFICEDASHBOARD"].ToString();
                else
                    ViewState["OFFICEDASHBOARD"] = "";

                if (Request.QueryString["correctiveactionid"] != null && Request.QueryString["correctiveactionid"].ToString() != "")
                    ViewState["correctiveactionid"] = Request.QueryString["correctiveactionid"].ToString();
                else
                    ViewState["correctiveactionid"] = "";
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["Officetask"] != null && Request.QueryString["Officetask"].ToString() != "")
                    ViewState["Officetask"] = Request.QueryString["Officetask"].ToString();
                else
                    ViewState["Officetask"] = "";

                BindCorrectiveAction();
                cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + ViewState["correctiveactionid"].ToString() + "');return true;");
                imgReopenhistory.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionReOpenTaskHistory.aspx?correctiveactionid=" + ViewState["correctiveactionid"].ToString() + "');return true;");
                SetRights();
               
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ucTaskStatus.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
        BindMenu();
    }
    private void SetRights()
    {
        ucTaskStatus.Enabled = SessionUtil.CanAccess(this.ViewState, "TASKSTATUS");
        ucTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "TARGETDATE");
        ucVerficationLevel.Enabled = SessionUtil.CanAccess(this.ViewState, "VERFICATIONLEVEL");
        chkRescheduleRequired.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEREQUIRED");
        ucRescheduleDate.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEDATE");
        txtRescheduleReason.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEREASON");
        ucCompletionDate.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONDATE");
        txtCompletionRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONREMARKS");
        txtCloseOutRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "CLOSEOUTREMARKS");
        txtreopenreason.Enabled = SessionUtil.CanAccess(this.ViewState, "REOPENREASON");
        txtnewduedate.Enabled = SessionUtil.CanAccess(this.ViewState, "NEWDUEDATE");
    }
    private void SetEvidence()
    {
        if (ViewState["ATTACHMENTDELETELOCKYN"].ToString() == "1")
        {
            imgEvidence.Attributes["onclick"] = "openNewWindow('NATD','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?U=1&ratingyn=1&dtkey=" + ViewState["DTKEY"] + "&mod="
                     + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE" + "&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";
        }
        else
        {
            imgEvidence.Attributes["onclick"] = "openNewWindow('NATD','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                   + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE" + "&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";

        }
    }
    private void BindCorrectiveAction()
    {
        DataSet ds;

        if (ViewState["correctiveactionid"] != null && !string.IsNullOrEmpty(ViewState["correctiveactionid"].ToString()))
        {
            ds = PhoenixInspectionLongTermAction.ShipBoardTaskEdit(new Guid(ViewState["correctiveactionid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCorrectiveAction.Text = dr["FLDCORRECTIVEACTION"].ToString();
                ucTaskStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                if (dr["FLDCOMPLETIONDATE"].ToString() != "")
                    ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                else
                    ucCompletionDate.Text = "";

                if (dr["FLDTASKCOMPLETIONREMARKS"].ToString() != "")
                    txtCompletionRemarks.Text = dr["FLDTASKCOMPLETIONREMARKS"].ToString();
                else
                    txtCompletionRemarks.Text = "";

                txtCompletedByName.Text = dr["FLDCOMPLETEDBYNAME"].ToString();
                txtRescheduleReason.Text = dr["FLDRESCHEDULEREASON"].ToString();
                txtReportedByName.Text = dr["FLDREPORTEDBYNAME"].ToString();
                txtReportedByDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDREMARKS"].ToString();
                txtCloseOutByName.Text = dr["FLDVERIFIEDBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDVERIFIEDDESIGNATIONNAME"].ToString();
                ucCloseoutDate.Text = dr["FLDCACLOSEOUTVERIFIEDDATE"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
                ViewState["OLDEXTENSIONREASON"] = dr["FLDRESCHEDULEREASON"].ToString();
                if (General.GetNullableDateTime(dr["FLDRECENTRESCHEDULEDATE"].ToString()) == null)
                {
                    ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
                }
                else
                    ViewState["OLDTARGETDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
                ViewState["RECENTRESCHEDULEDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
                ucRescheduleDate.Text = dr["FLDRECENTRESCHEDULEDATE"].ToString();             
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                chkRescheduleRequired.Checked = false;
                ucTargetDate.Enabled = false;                
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    imgEvidence.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgEvidence.ImageUrl = Session["images"] + "/attachment.png";

                txtDefDesc.Text = dr["FLDDEFICIENCYDESCRIPTION"].ToString();
                txtItem.Text = dr["FLDITEMNAME"].ToString();
                txtChecklistRefNo.Text = dr["FLDCACHECKLISTREFNUMBER"].ToString();
                txtDefDetails.Text = dr["FLDDEFICIENCYDETAILS"].ToString();
                ViewState["ATTACHMENTDELETELOCKYN"] = dr["FLDATTACHMENTDELETELOCKYN"].ToString();
                txtCompletedByDesignation.Text = dr["FLDCOMPLETEDBYDESIGNATION"].ToString();
                if (dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1")
                {
                    txtSuperintendentcomments.Text = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
                    ViewState["FLDSUPERINTENDENTCOMMENTS"] = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
                }
                else
                {
                    txtSuperintendentcomments.Text = dr["FLDRESCHEDULEREASON"].ToString();
                    ViewState["FLDRESCHEDULEREASON"] = dr["FLDRESCHEDULEREASON"].ToString();
                }

                if (dr["FLDSECONDARYAPPROVEDYN"].ToString() == "1")
                {
                    txtSecondaryApproverComments.Text = dr["FLDSECONDARYAPPROVEDCOMMENTS"].ToString();
                    ViewState["FLDSECONDARYAPPROVEDCOMMENTS"] = dr["FLDSECONDARYAPPROVEDCOMMENTS"].ToString();
                }
                if (dr["FLDRESCHEDULETASKYN"].ToString() == "1" && dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && dr["FLDSECONDARYAPPROVALREQUIREDYN"].ToString() == "1")
                {
                    txtSecondaryApproverComments.Text = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
                    ViewState["FLDSUPERINTENDENTCOMMENTS"] = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
                }

                ViewState["SUPERINTENTAPPROVALYN"] = dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString();
                ViewState["FLDRESCHEDULETASKYN"] = dr["FLDRESCHEDULETASKYN"].ToString();
                ViewState["SECONDARYAPPROVALYN"] = dr["FLDSECONDARYAPPROVEDYN"].ToString();
                ViewState["DBCODE"] = dr["FLDDBCODE"].ToString();
                if (dr["FLDREOPENTASKYN"].ToString() == "1")
                {
                    chkreopentask.Checked = true;
                    chkreopentask.Enabled = false;
                }
                else
                {
                    chkreopentask.Checked = false;
                    chkreopentask.Enabled = true;
                }
                txtreopenreason.Text = dr["FLDREOPENREASON"].ToString();
                txtnewduedate.Text = dr["FLDNEWTARGETDATE"].ToString();
                if (dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1")
                {
                    txtApprovedByName.Text = dr["FLDAPPROVEDBY"].ToString();
                    ucApprovedDate.Text = dr["FLDSUPERINTENDENTAPPROVEDDATE"].ToString();
                    txtApprovedByDesignation.Text = dr["FLDSUPERINTENDENTAPPROVERDESIGNATION"].ToString();
                }
                if (dr["FLDSECONDARYAPPROVEDYN"].ToString() == "1")
                {
                    txtSecondaryApprovedByName.Text = dr["FLDSECONDARYAPPROVEDBY"].ToString();
                    ucSecondaryApprovedDate.Text = dr["FLDSECONDARYAPPROVEDDATE"].ToString();
                    txtSecondaryApprovedByDesignation.Text = dr["FLDSECONDARYAPPROVERDESIGNATION"].ToString();
                }
                SetEvidence();
                BindMenu();
            }
        }
    }

    protected void MenuShipboardGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["Officetask"].ToString() == "yes")
                {
                    Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", true);
                }

                if (ViewState["DASHBOARD"].ToString() !=string.Empty && ViewState["DASHBOARD"].ToString()=="1")
                {
                    Response.Redirect("../Inspection/InspectionDashboardShipBoardTasks.aspx?OVERDUEYN=" + ViewState["OVERDUEYN"], true);
                }

                if (ViewState["OFFICEDASHBOARD"].ToString() != string.Empty && ViewState["OFFICEDASHBOARD"].ToString() == "1")
                {
                    Response.Redirect("../Inspection/InspectionOfficeDashboardShipBoardTasks.aspx?", true);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuInspectionShipboard_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["correctiveactionid"].ToString()) != null)
                {
                    if (!IsValidShipBoardTask())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionLongTermAction.ShipBoardTaskUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["correctiveactionid"].ToString()),
                                                General.GetNullableString(txtCorrectiveAction.Text),
                                                DateTime.Parse(ucTargetDate.Text),
                                                General.GetNullableDateTime(ucCompletionDate.Text),
                                                General.GetNullableString(txtRescheduleReason.Text),
                                                General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                                                int.Parse(ucTaskStatus.SelectedHard),
                                                General.GetNullableString(txtCompletionRemarks.Text),
                                                General.GetNullableString(txtCloseOutRemarks.Text),
                                                General.GetNullableInteger(chkreopentask.Checked.Equals(true) ? "1" : "0"),
                                                General.GetNullableString(txtreopenreason.Text),
                                                General.GetNullableDateTime(txtnewduedate.Text),
                                                General.GetNullableDateTime(ucRescheduleDate.Text),
                                                General.GetNullableDateTime(ucCloseoutDate.Text),
                                                General.GetNullableDateTime(ucApprovedDate.Text),
                                                General.GetNullableDateTime(ucSecondaryApprovedDate.Text)
                                                );


                    if ((General.GetNullableDateTime(ucRescheduleDate.Text) != null) && (ViewState["VESSELID"].ToString()== "0"))
                    {
                        if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                        {
                            if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["correctiveactionid"].ToString())
                                                                   , General.GetNullableString(txtRescheduleReason.Text)
                                                                   , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                   , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                                   );
                            }
                        }
                        else
                            PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["correctiveactionid"].ToString())
                                                                   , General.GetNullableString(txtRescheduleReason.Text)
                                                                   , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                   , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                                   );


                    }
                    ucStatus.Text = "Task updated successfully.";
                    BindCorrectiveAction();

                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','','');");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
            else if (CommandName.ToUpper().Equals("SUPDTAPPROVAL"))
            {
                if (!IsValidReschduleTask())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionLongTermAction.RescheduleTaskSuperintendentApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["correctiveactionid"].ToString()),
                                            General.GetNullableString(txtSuperintendentcomments.Text));

                if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
                {
                    if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                    {
                        if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                        {
                            PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , new Guid(ViewState["correctiveactionid"].ToString())
                                                               , General.GetNullableString(txtRescheduleReason.Text)
                                                               , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                               , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                               , int.Parse(ViewState["VESSELID"].ToString())
                                                               );
                        }
                    }
                    else
                        PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , new Guid(ViewState["correctiveactionid"].ToString())
                                                               , General.GetNullableString(txtRescheduleReason.Text)
                                                               , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                               , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                               , int.Parse(ViewState["VESSELID"].ToString())
                                                               );

                }

                ucStatus.Text = "Reschedule Task Approved Successfully";
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
        BindCorrectiveAction();
    }


    private bool IsValidShipBoardTask()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (General.GetNullableInteger(ucTaskStatus.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";

        if (General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date cannot be the future date.";

        if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
        {
            if (string.IsNullOrEmpty(txtRescheduleReason.Text))
                ucError.ErrorMessage = "Reschedule Reason is required.";

            if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
            {
                if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                {
                    if (General.GetNullableString(txtRescheduleReason.Text) == General.GetNullableString(ViewState["OLDEXTENSIONREASON"].ToString()))
                        ucError.ErrorMessage = "You have not modified the Reschedule Reason.";
                }
            }
        }
        return (!ucError.IsError);
    }

    private bool IsValidReschduleTask()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableString(txtSuperintendentcomments.Text) == null && (ViewState["DBCODE"].ToString() == "OFFSHORE"))
            ucError.ErrorMessage = "Superintendent Comments is required.";

        if (General.GetNullableString(txtSuperintendentcomments.Text) == null && (ViewState["DBCODE"].ToString() == "PHOENIX"))
            ucError.ErrorMessage = "Fleet Manager Comments is required.";

        if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
        {
            if (string.IsNullOrEmpty(txtRescheduleReason.Text))
                ucError.ErrorMessage = "Reschedule Reason is required.";
        }

        if (General.GetNullableString(txtRescheduleReason.Text) != null && General.GetNullableDateTime(ucRescheduleDate.Text) == null)
            ucError.ErrorMessage = "Reschedule Date is required.";

        return (!ucError.IsError);
    }

    protected void RescheduleRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRescheduleRequired.Checked.Equals(true))
        {
            txtRescheduleReason.Enabled = true;
        }
        else
        {
            txtRescheduleReason.Enabled = false;
            ucTargetDate.Enabled = false;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {       
        BindCorrectiveAction();
    }
    protected void BindMenu()
    {
        if (ViewState["DASHBOARD"].ToString() == "")
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Details", "DETAILS", ToolBarDirection.Right);
            toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
            MenuShipboardGeneral.AccessRights = this.ViewState;
            MenuShipboardGeneral.MenuList = toolbarmain.Show();
            MenuShipboardGeneral.SelectedMenuIndex = 0;
        }
        else
            MenuShipboardGeneral.Visible = false;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuInspectionShipboard.AccessRights = this.ViewState;
        MenuInspectionShipboard.MenuList = toolbar.Show();
    }
}
