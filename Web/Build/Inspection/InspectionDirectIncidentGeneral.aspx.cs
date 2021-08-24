using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionDirectIncidentGeneral : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Raise Incident", "RAISEINCIDENT", ToolBarDirection.Right);
        toolbar.AddButton("Raise Near Miss", "RAISENEARMISS", ToolBarDirection.Right);
        toolbar.AddButton("Raise NC", "RAISENC", ToolBarDirection.Right);
        toolbar.AddButton("Raise Observation", "RAISEOBS", ToolBarDirection.Right);
        toolbar.AddButton("Raise Crew Complaints", "RAISECREWCOMP", ToolBarDirection.Right);
        MenuInspectionIncident.AccessRights = this.ViewState;
        MenuInspectionIncident.MenuList = toolbar.Show();
        
        try
        {
            if (!IsPostBack)
            {
                ucConfirm.Attributes.Add("style", "display:none");

                if (Request.QueryString["viewonly"] == null)
                {
                    ucTitle.ShowMenu = "true";
                    ucTitle.Text = "General";
                }
                else
                {
                    ucTitle.ShowMenu = "false";
                    ucTitle.Text = "Open Report";
                }

                if (Request.QueryString["directincidentid"] != null && Request.QueryString["directincidentid"].ToString() != "")
                    ViewState["directincidentid"] = Request.QueryString["directincidentid"].ToString();
                else
                    ViewState["directincidentid"] = "";
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                BindInspectionIncident();

                imgEvidence.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                  + PhoenixModule.QUALITY + "&type=OPENREPORT" + "&cmdname=OPENREPORTUPLOAD&VESSELID=" + ViewState["FLDVESSELID"].ToString() + "'); return false;";

                ViewState["VALIDYN"] = "0";
                ViewState["VALIDDATIONMSG"] = "";
                ViewState["DashboardYN"] = "";

                if (!string.IsNullOrEmpty(Request.QueryString["DashboardYN"]))
                {
                    ViewState["DashboardYN"] = Request.QueryString["DashboardYN"];
                }

                SetRights();
            }

            if (ViewState["DashboardYN"].ToString() == "")
            {
                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                toolbar1.AddButton("General", "General", ToolBarDirection.Right);
                toolbar1.AddButton("List", "LIST", ToolBarDirection.Right);
                MenuInspectionGeneral.AccessRights = this.ViewState;
                MenuInspectionGeneral.MenuList = toolbar1.Show();
                MenuInspectionGeneral.SelectedMenuIndex = 0;
            }
            else
                MenuInspectionGeneral.Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void SetRights()
    {
        bool flag = false;

        DataSet ds1 = PhoenixInspectionEventSupdtFeedback.EventSupdtFeedbackEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , 4, General.GetNullableGuid(ViewState["directincidentid"].ToString()));

        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            if (dr["FLDVALIDYN"].ToString() == "1")
            {
                ViewState["VALIDYN"] = "1";
            }
            else
            {
                ViewState["VALIDYN"] = "0";
                ViewState["VALIDDATIONMSG"] = dr["FLDVALIDATIONMSG"].ToString();
            }
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            flag = SessionUtil.CanAccess(this.ViewState, "SUPDTEVENTFEEDBACK");

            if (flag == true)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback != 1)
                {
                    flag = false;
                }
            }
        }
        else
        {
            flag = false;
        }
        if (flag == true)
        {
            imgSupdtEventFeedback.Visible = true;
        }
        else
        {
            imgSupdtEventFeedback.Visible = false;
        }
    }
    private void BindInspectionIncident()
    {
        DataSet ds;

        if (ViewState["directincidentid"] != null && !string.IsNullOrEmpty(ViewState["directincidentid"].ToString()))
        {
            ds = PhoenixInspectionIncident.DirectIncidentEdit(new Guid(ViewState["directincidentid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtInvestigationAndEvidence.Text = dr["FLDSUMMARY"].ToString();
                ucReviewCategory.SelectedQuick = dr["FLDREVIEWCATEGORY"].ToString();
                txtStatus.Text = dr["FLDOPENREPORTSTATUSNAME"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                if (dr["FLDEVIDENCEREQUIRED"].ToString() == "1")
                    chkEvidenceRequired.Checked = true;
                else
                    chkEvidenceRequired.Checked = false;
                ucDept.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
                txtPIC.Text = dr["FLDASSIGNEDTO"].ToString();
                txtPICName.Text = dr["FLDASSIGNEDPERSONNAME"].ToString();
                ucDueDate.Text = dr["FLDDUEDATE"].ToString();
                ucCompletiondate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                txtAction.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                if (dr["FLDCLOSEDWITHOUTACTION"] != null && dr["FLDCLOSEDWITHOUTACTION"].ToString().Equals("1"))
                    chkClosed.Checked = true;
                ucCloseOutDate.Text = dr["FLDCLOSEOUTDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCancelReason.Text = dr["FLDCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDUNSAFECLOSEOUTREMARKS"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();

                if (chkEvidenceRequired.Checked == true)
                    imgEvidence.Visible = true;
                else
                    imgEvidence.Visible = false;

                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    imgEvidence.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgEvidence.ImageUrl = Session["images"] + "/attachment.png";

                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["FLDVESSELID"] = dr["FLDVESSELID"].ToString();
            }
        }
    }
    protected void MenuInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionDirectIncidentList.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCategory())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionIncident.UpdateOpenReportCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["directincidentid"].ToString()),
                                            int.Parse(ucReviewCategory.SelectedQuick),
                                            General.GetNullableString(txtRemarks.Text),
                                            General.GetNullableInteger(chkEvidenceRequired.Checked == true ? "1" : "0"),
                                            General.GetNullableInteger(ucDept.SelectedDepartment),
                                            General.GetNullableInteger(txtPIC.Text),
                                            General.GetNullableDateTime(ucDueDate.Text),
                                            General.GetNullableDateTime(ucCompletiondate.Text),
                                            General.GetNullableString(txtAction.Text),
                                            General.GetNullableInteger(chkClosed.Checked == true ? "1" : "0"),
                                            General.GetNullableString(txtCancelReason.Text),
                                            General.GetNullableString(txtCloseOutRemarks.Text));

                ucStatus.Text = "Information updated successfully.";
                BindInspectionIncident();
            }
            else if (CommandName.ToUpper().Equals("RAISEINCIDENT"))
            {
                ViewState["raise"] = "incident";
                RadWindowManager1.RadConfirm("Review category cannot be changed once an Incident is raised. Are you sure to raise an Incident?", "Confirm", 320, 150, null, "Raise Incident");
            }

            else if (CommandName.ToUpper().Equals("RAISENEARMISS"))
            {
                ViewState["raise"] = "nearmiss";
                RadWindowManager1.RadConfirm("Review category cannot be changed once Near Miss is raised. Are you sure to raise Near Miss?", "Confirm", 320, 150, null, "Raise Near Miss");

            }
            else if (CommandName.ToUpper().Equals("RAISENC"))
            {
                ViewState["raise"] = "nc";
                RadWindowManager1.RadConfirm("Review category cannot be changed once Non conformity is raised. Are you sure to raise Non conformity?", "Confirm", 320, 150, null, "Raise Non Conformity");

            }
            else if (CommandName.ToUpper().Equals("RAISEOBS"))
            {
                ViewState["raise"] = "observation";
                RadWindowManager1.RadConfirm("Review category cannot be changed once an Observation is raised. Are you sure to raise an Observation?", "Confirm", 320, 150, null, "Raise Observation");

            }
            else if (CommandName.ToUpper().Equals("RAISECREWCOMP"))
            {
                ViewState["raise"] = "crewcomp";
                RadWindowManager1.RadConfirm("Review category cannot be changed once an Crew Complaint is raised. Are you sure to raise an Crew Complaints?", "Confirm", 320, 150, null, "Raise Crew Complaint");

            }
            else if (CommandName.ToUpper().Equals("CLOSE"))
            {
                if (!IsValidCategory())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionIncident.UpdateOpenReportCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["directincidentid"].ToString()),
                                            int.Parse(ucReviewCategory.SelectedQuick),
                                            General.GetNullableString(txtRemarks.Text),
                                            General.GetNullableInteger(chkEvidenceRequired.Checked == true ? "1" : "0"),
                                            General.GetNullableInteger(ucDept.SelectedDepartment),
                                            General.GetNullableInteger(txtPIC.Text),
                                            General.GetNullableDateTime(ucDueDate.Text),
                                            General.GetNullableDateTime(ucCompletiondate.Text),
                                            General.GetNullableString(txtAction.Text),
                                            General.GetNullableInteger(chkClosed.Checked == true ? "1" : "0"),
                                            General.GetNullableString(txtCancelReason.Text),
                                            General.GetNullableString(txtCloseOutRemarks.Text));

                PhoenixInspectionIncident.CloseOpenReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["directincidentid"].ToString()),
                                            int.Parse(chkClosed.Checked == true ? "1" : "0"),
                                            General.GetNullableString(txtCloseOutRemarks.Text));

                ucStatus.Text = "Open Report is closed successfully.";
                BindInspectionIncident();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindInspectionIncident();
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidCategory())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionIncident.UpdateOpenReportCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["directincidentid"].ToString()),
                                        int.Parse(ucReviewCategory.SelectedQuick),
                                        General.GetNullableString(txtRemarks.Text),
                                        General.GetNullableInteger(chkEvidenceRequired.Checked == true ? "1" : "0"),
                                        General.GetNullableInteger(ucDept.SelectedDepartment),
                                        General.GetNullableInteger(txtPIC.Text),
                                        General.GetNullableDateTime(ucDueDate.Text),
                                        General.GetNullableDateTime(ucCompletiondate.Text),
                                        General.GetNullableString(txtAction.Text),
                                        General.GetNullableInteger(chkClosed.Checked == true ? "1" : "0"),
                                        General.GetNullableString(txtCancelReason.Text),
                                        General.GetNullableString(txtCloseOutRemarks.Text));

            BindInspectionIncident();

            if (ViewState["raise"] != null && ViewState["raise"].ToString() == "incident")
            {
                PhoenixInspectionIncident.RaiseIncidentNearmiss(new Guid(ViewState["directincidentid"].ToString()));
                ucStatus.Text = "Incident is raised successfully.";
            }
            else if (ViewState["raise"] != null && ViewState["raise"].ToString() == "nearmiss")
            {
                PhoenixInspectionIncident.RaiseNearmiss(new Guid(ViewState["directincidentid"].ToString()));
                ucStatus.Text = "Near Miss is raised successfully.";
            }
            else if (ViewState["raise"] != null && ViewState["raise"].ToString() == "nc")
            {
                PhoenixInspectionIncident.RaiseNC(new Guid(ViewState["directincidentid"].ToString()));
                ucStatus.Text = "Non Conformity is raised successfully.";
            }
            else if (ViewState["raise"] != null && ViewState["raise"].ToString() == "observation")
            {
                PhoenixInspectionIncident.RaiseObservation(new Guid(ViewState["directincidentid"].ToString()));
                ucStatus.Text = "Observation is raised successfully.";
            }
            else if (ViewState["raise"] != null && ViewState["raise"].ToString() == "crewcomp")
            {
                PhoenixInspectionIncident.RaiseCrewComplaint(new Guid(ViewState["directincidentid"].ToString()));
                ucStatus.Text = "Crew Complaint is raised successfully.";
            }
            BindInspectionIncident();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCategory()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucReviewCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Review Category is required.";

        if (General.GetNullableInteger(ucDept.SelectedDepartment) == null)
            ucError.ErrorMessage = "Assigned to is required.";

        if (General.GetNullableDateTime(ucDueDate.Text) == null)
            ucError.ErrorMessage = "Due Date is required.";

        if (General.GetNullableDateTime(ucCompletiondate.Text) != null)
        {
            if (DateTime.Parse(ucCompletiondate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Completion date should not be the future date.";
        }

        if (chkClosed.Checked == true)
        {
            if (General.GetNullableString(txtCloseOutRemarks.Text) == null)
                ucError.ErrorMessage = "Close Out Remarks is required.";
        }

        return (!ucError.IsError);
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {

    }

    protected void ucDept_Changed(object sender, EventArgs e)
    {
        if (ucDept != null)
        {
            //txtPICAdd.Text = "";
            if (imgPIC != null)
            {
                imgPIC.Attributes.Add("onclick", "return showPickList('spnPIC', 'codehelp1', '', '../Common/CommonPickListInspectionUser.aspx?departmentid=" + General.GetNullableInteger(ucDept.SelectedDepartment) + "', true);");
            }
        }
    }
    protected void txtInvestigationAndEvidence_TextChanged(object sender, EventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindInspectionIncident();
    }
    protected void SupdtFeedback_Click(object sender, EventArgs e)
    {
        if (ViewState["VALIDYN"].ToString() != "1")
        {
            ucError.ErrorMessage = ViewState["VALIDDATIONMSG"].ToString();
            ucError.Visible = true;
            return;
        }
        else
        {
            if (ViewState["directincidentid"] != null)
            {
                string script = "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=4&SOURCEREFERENCEID=" + ViewState["directincidentid"].ToString() + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
        }
    }
}
