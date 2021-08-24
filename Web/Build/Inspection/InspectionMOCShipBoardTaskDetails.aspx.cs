using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web;

public partial class InspectionMOCShipBoardTaskDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if ((Request.QueryString["MOCID"] == null) || (Request.QueryString["MOCID"] == ""))
            {
                toolbar.AddButton("Details", "DETAILS", ToolBarDirection.Right);
                if (Request.QueryString["Regulation"] == null)
                    toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
                MenuShipboardGeneral.AccessRights = this.ViewState;
                MenuShipboardGeneral.MenuList = toolbar.Show();
            }

            ViewState["RECENTRESCHEDULEDATE"] = "";
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionShipboard.AccessRights = this.ViewState;
            MenuInspectionShipboard.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["MOCActionplanid"] != null && Request.QueryString["MOCActionplanid"].ToString() != "")
                    ViewState["MOCActionplanid"] = Request.QueryString["MOCActionplanid"].ToString();
                else
                    ViewState["MOCActionplanid"] = "";
                if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() != "")
                    ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();

                if (Request.QueryString["departmentid"] != null && Request.QueryString["departmentid"].ToString() != "")
                    ViewState["departmentid"] = Request.QueryString["departmentid"].ToString();
                else
                    ViewState["departmentid"] = "";

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ucTaskStatus.Enabled = false;
                }
                BindActionPlan();
            }

            MenuShipboardGeneral.SelectedMenuIndex = 0;

            cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('codehelpactivity1','','" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + ViewState["MOCActionplanid"].ToString() + "'); return true;");
            //cmdReschedule.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + ViewState["MOCActionplanid"].ToString() + "');return true;");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void SetEvidence()
    {
        imgEvidence.Attributes["onclick"] = "javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
              + PhoenixModule.QUALITY + "&type=MOCACTIONPLAN" + "&cmdname=MOCACTIONPLANUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";
    }
    private void BindActionPlan()
    {
        DataSet ds;

        if (ViewState["MOCActionplanid"] != null && !string.IsNullOrEmpty(ViewState["MOCActionplanid"].ToString()))
        {
            ds = PhoenixInspectionMOCActionPlan.MOCActionPlanEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCActionplanid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCorrectiveAction.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                ucTaskStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                txtCompletionRemarks.Text = dr["FLDCOMPLETIONREMARKS"].ToString();
                txtCompletedByName.Text = dr["FLDCOMPLETEDBY"].ToString();
                txtRescheduleReason.Text = dr["FLDRESCHEDULEREMARKS"].ToString();
                txtReportedByName.Text = dr["FLDREPORTEDBY"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEDBY"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEDDATE"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
                ViewState["OLDEXTENSIONREASON"] = dr["FLDRESCHEDULEREMARKS"].ToString();
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
                txtCloseOutRemarks.Text = dr["FLDCLOSEDOUTREMARKS"].ToString();
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    imgEvidence.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgEvidence.ImageUrl = Session["images"] + "/attachment.png";

                SetEvidence();
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
                if ((ViewState["departmentid"].ToString() == "1") && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX")))
                {
                    Response.Redirect("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", true);
                }
                if ((ViewState["departmentid"].ToString() == "1") && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
                {
                    Response.Redirect("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", true);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionMOCActionPlanShipboardTaskList.aspx", true);
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
                if (General.GetNullableGuid(ViewState["MOCActionplanid"].ToString()) != null)
                {
                    if (!IsValidShipBoardTask())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionMOCActionPlan.MOCShipBoardTaskUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["MOCActionplanid"].ToString()),
                                                General.GetNullableString(txtCorrectiveAction.Text),
                                                DateTime.Parse(ucTargetDate.Text),
                                                General.GetNullableDateTime(ucCompletionDate.Text),
                                                General.GetNullableString(txtRescheduleReason.Text),
                                                General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                                                int.Parse(ucTaskStatus.SelectedHard),
                                                General.GetNullableString(txtCompletionRemarks.Text),
                                                General.GetNullableString(txtCloseOutRemarks.Text));

                    if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
                    {
                        if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                        {
                            if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["MOCActionplanid"].ToString())
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
                                                                   , new Guid(ViewState["MOCActionplanid"].ToString())
                                                                   , General.GetNullableString(txtRescheduleReason.Text)
                                                                   , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                   , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                                   );

                    }
                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                    ucStatus.Text = "Task updated successfully.";
                    BindActionPlan();
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

        if (General.GetNullableString(txtRescheduleReason.Text) != null && General.GetNullableDateTime(ucRescheduleDate.Text) == null)
            ucError.ErrorMessage = "Reschedule Date is required.";

        return (!ucError.IsError);
    }

    protected void RescheduleRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRescheduleRequired.Checked)
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
        BindActionPlan();
    }
}
