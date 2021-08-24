using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionOfficeTasksDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();          
            toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
            toolbar.AddButton("Details", "DETAILS", ToolBarDirection.Right);
            MenuPreventiveGeneral.AccessRights = this.ViewState;
            MenuPreventiveGeneral.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionShipboard.AccessRights = this.ViewState;
            MenuInspectionShipboard.MenuList = toolbar.Show();

            if (Request.QueryString["CallFrom"] != null && Request.QueryString["CallFrom"].ToString() != "")
            {
                MenuPreventiveGeneral.Visible = false;
            }

            MenuPreventiveGeneral.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["RECENTRESCHEDULEDATE"] = "";

                if (Request.QueryString["preventiveactionid"] != null && Request.QueryString["preventiveactionid"].ToString() != "")
                    ViewState["preventiveactionid"] = Request.QueryString["preventiveactionid"].ToString();
                else
                    ViewState["preventiveactionid"] = "";
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                
                BindPreventiveAction();
                cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + ViewState["preventiveactionid"].ToString() + "');return true;");

                imgEvidence.Attributes["onclick"] = "openNewWindow('NATD','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                  + PhoenixModule.QUALITY + "&type=PREVENTIVETASKS" + "&cmdname=PREVENTIVETASKSUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindPreventiveAction()
    {
        DataSet ds;

        if (ViewState["preventiveactionid"] != null && !string.IsNullOrEmpty(ViewState["preventiveactionid"].ToString()))
        {
            ds = PhoenixInspectionLongTermAction.EditPreventiveAction(new Guid(ViewState["preventiveactionid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtPreventiveAction.Text = dr["FLDPREVENTIVEACTION"].ToString();
                ddlTaskStatus.SelectedValue = dr["FLDSTATUS"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();

                if (dr["FLDCOMPLETIONDATE"].ToString() != "")
                    ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();

                if (dr["FLDTASKCOMPLETIONREMARKS"].ToString() != "")
                    txtCompletionRemarks.Text = dr["FLDTASKCOMPLETIONREMARKS"].ToString();

                txtRescheduleReason.Text = dr["FLDRESCHEDULEREASON"].ToString();
                txtReportedByName.Text = dr["FLDREPORTEDBYNAME"].ToString();
                txtReportedByDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDREMARKS"].ToString();
                txtCloseOutByName.Text = dr["FLDVERIFIEDBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDVERIFIEDDESIGNATIONNAME"].ToString();
                ucCloseoutDate.Text = dr["FLDPACLOSEOUTVERIFIEDDATE"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();

                if (General.GetNullableDateTime(dr["FLDRECENTRESCHEDULEDATE"].ToString()) == null)
                {
                    ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
                }
                else
                    ViewState["OLDTARGETDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
                ViewState["RECENTRESCHEDULEDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
                ucRescheduleDate.Text = dr["FLDRECENTRESCHEDULEDATE"].ToString();
                ViewState["OLDEXTENSIONREASON"] = dr["FLDRESCHEDULEREASON"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                chkRescheduleRequired.Checked = false;
                ucTargetDate.Enabled = false;
                txtRescheduleReason.Enabled = false;
                if (dr["FLDISATTACHMENT"].ToString() == "0")                
                    imgEvidence.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgEvidence.ImageUrl = Session["images"] + "/attachment.png";

                imgEvidence.Attributes.Add("onclick", "openNewWindow('att','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.QUALITY + "&type=PREVENTIVETASKS&cmdname=PREVENTIVETASKSUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return true;");

                txtDefDesc.Text = dr["FLDDEFICIENCYDESCRIPTION"].ToString();
                txtItem.Text = dr["FLDITEMNAME"].ToString();
                txtCompletedByName.Text = dr["FLDCOMPLETEDBYNAME"].ToString();
                txtCompletedByDesignation.Text = dr["FLDCOMPLETEDBYDESIGNATION"].ToString();
                SetRights();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ddlTaskStatus.Enabled = false;
                }
            }
        }
    }
    private void SetRights()
    {
        ddlTaskStatus.Enabled = SessionUtil.CanAccess(this.ViewState, "TASKSTATUS");
        ucTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "TARGETDATE");
        ucVerficationLevel.Enabled = SessionUtil.CanAccess(this.ViewState, "VERFICATIONLEVEL");
        chkRescheduleRequired.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEREQUIRED");
        ucRescheduleDate.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEDATE");
        txtRescheduleReason.Enabled = SessionUtil.CanAccess(this.ViewState, "RESCHEDULEREASON");
        ucCompletionDate.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONDATE");
        txtCompletionRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONREMARKS");
        txtCloseOutRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "CLOSEOUTREMARKS");
    }
    protected void MenuPreventiveGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                    Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
                else if(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
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
                if (General.GetNullableGuid(ViewState["preventiveactionid"].ToString()) != null)
                {
                    if (!IsValidShipBoardTask())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionLongTermAction.PreventiveTaskUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["preventiveactionid"].ToString()),
                                                General.GetNullableString(txtPreventiveAction.Text),
                                                DateTime.Parse(ucTargetDate.Text),
                                                General.GetNullableDateTime(ucCompletionDate.Text),
                                                General.GetNullableString(txtRescheduleReason.Text),
                                                General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                                                int.Parse(ddlTaskStatus.SelectedValue),
                                                General.GetNullableString(txtCompletionRemarks.Text),
                                                General.GetNullableString(txtCloseOutRemarks.Text),
                                                General.GetNullableDateTime(ucCloseoutDate.Text));

                    if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
                    {
                        if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                        {
                            if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["preventiveactionid"].ToString())
                                                                , General.GetNullableString(txtRescheduleReason.Text)
                                                                , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                                );
                            }
                        }
                        else
                        {
                            PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["preventiveactionid"].ToString())
                                                                , General.GetNullableString(txtRescheduleReason.Text)
                                                                , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                                );
                        }
                    }

                    ucStatus.Text = "Task updated successfully.";
                    BindPreventiveAction();
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
    
    private bool IsValidShipBoardTask()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (General.GetNullableInteger(ddlTaskStatus.SelectedValue) == null)
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

    protected void RescheduleRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRescheduleRequired.Checked.Equals(true))
        {
            txtRescheduleReason.Enabled = true;
            ucTargetDate.Enabled = true;
        }
        else
        {
            txtRescheduleReason.Enabled = false;
            ucTargetDate.Enabled = false;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindPreventiveAction();
    }
}
