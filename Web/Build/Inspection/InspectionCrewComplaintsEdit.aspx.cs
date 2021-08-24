using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Inspection_InspectionCrewComplaintsEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Raise Open Report", "RAISEOPENREPORT",ToolBarDirection.Right);

        MenuInspectionIncident.AccessRights = this.ViewState;
        MenuInspectionIncident.MenuList = toolbar.Show();

       

        try
        {
            if (!IsPostBack)
            {
                ucConfirm.Visible = false;

                

                if (Request.QueryString["directincidentid"] != null && Request.QueryString["directincidentid"].ToString() != "")
                    ViewState["crewcomplaintid"] = Request.QueryString["directincidentid"].ToString();
                else
                    ViewState["crewcomplaintid"] = "";
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                
                BindInspectionIncident();

                imgEvidence.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                  + PhoenixModule.QUALITY + "&type=OPENREPORT" + "&cmdname=OPENREPORTUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";

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
                toolbar1.AddButton("List", "LIST");
                toolbar1.AddButton("General", "General");
                MenuInspectionGeneral.AccessRights = this.ViewState;
                MenuInspectionGeneral.MenuList = toolbar1.Show();

                MenuInspectionGeneral.SelectedMenuIndex = 1;
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
        , 5, General.GetNullableGuid(ViewState["crewcomplaintid"].ToString()));

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

        if (ViewState["crewcomplaintid"] != null && !string.IsNullOrEmpty(ViewState["crewcomplaintid"].ToString()))
        {
            ds = PhoenixInspectionIncident.CrewComplaintEdit(new Guid(ViewState["crewcomplaintid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtInvestigationAndEvidence.Text = dr["FLDSUMMARY"].ToString();
                ucReviewCategory.SelectedQuick = dr["FLDREVIEWCATEGORY"].ToString();
                //txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                ucStatusofCC.SelectedHard = dr["FLDSTATUS"].ToString();
                ucStatusofCC.bind();
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
                ucCloseOutDate.Text = dr["FLDCLOSEOUTDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelReason.Text = dr["FLDCREWCOMPLAINTCANCELREASON"].ToString();
                if (dr["FLDCREWCLOSEDWITHOUTACTION"] != null && dr["FLDCREWCLOSEDWITHOUTACTION"].ToString() == "1")
                    chkClosed.Checked = true;
                else
                    chkClosed.Checked = false;
                txtCloseOutRemarks.Text = dr["FLDCREWCLOSEOUTREMARKS"].ToString();
                txtRank.Text = dr["FLDREPORTEDBYRANK"].ToString();
                txtName.Text = dr["FLDREPORTEDBYNAME"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();

                if (dr["FLDISATTACHMENT"].ToString() == "0")
                    imgEvidence.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgEvidence.ImageUrl = Session["images"] + "/attachment.png";

                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                if (chkEvidenceRequired.Checked)
                    imgEvidence.Visible = true;
                else
                    imgEvidence.Visible = false;
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
                Response.Redirect("../Inspection/InspectionDirectIncidentList.aspx?category=3", true);
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
                PhoenixInspectionIncident.UpdateCrewComplaintCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["crewcomplaintid"].ToString()),
                                            General.GetNullableInteger(ucReviewCategory.SelectedQuick),
                                            General.GetNullableString(txtRemarks.Text),
                                            General.GetNullableInteger(chkEvidenceRequired.Checked == true ? "1" : "0"),
                                            General.GetNullableInteger(ucDept.SelectedDepartment),
                                            General.GetNullableInteger(txtPIC.Text),
                                            General.GetNullableDateTime(ucDueDate.Text),
                                            General.GetNullableDateTime(ucCompletiondate.Text),
                                            General.GetNullableString(txtAction.Text),
                                            General.GetNullableString(txtCancelReason.Text),
                                            int.Parse(ucStatusofCC.SelectedHard),
                                            int.Parse(chkClosed.Checked == true ? "1" : "0"),
                                            General.GetNullableString(txtCloseOutRemarks.Text));

                ucStatus.Text = "Category updated successfully.";
                BindInspectionIncident();
            }
            else if (CommandName.ToUpper().Equals("RAISEOPENREPORT"))
            {
                if (!IsValidCategoryRaiseOR())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionIncident.RaiseOpenReport(new Guid(ViewState["crewcomplaintid"].ToString()));
                ucStatus.Text = "Open Report raised successfully.";
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
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (!IsValidCategory())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionIncident.UpdateCrewComplaintCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["crewcomplaintid"].ToString()),
                                            General.GetNullableInteger(ucReviewCategory.SelectedQuick),
                                            General.GetNullableString(txtRemarks.Text),
                                            General.GetNullableInteger(chkEvidenceRequired.Checked == true ? "1" : "0"),
                                            General.GetNullableInteger(ucDept.SelectedDepartment),
                                            General.GetNullableInteger(txtPIC.Text),
                                            General.GetNullableDateTime(ucDueDate.Text),
                                            General.GetNullableDateTime(ucCompletiondate.Text),
                                            General.GetNullableString(txtAction.Text),
                                            General.GetNullableString(txtCancelReason.Text),
                                            int.Parse(ucStatusofCC.SelectedHard),
                                            int.Parse(chkClosed.Checked == true ? "1" : "0"),
                                            General.GetNullableString(txtCloseOutRemarks.Text));

                BindInspectionIncident();

            }
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

        if (General.GetNullableInteger(ucStatusofCC.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";

        else if (ucStatusofCC.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
        {
            if (General.GetNullableDateTime(ucCompletiondate.Text) == null)
                ucError.ErrorMessage = "Completion Date is required.";
        }
        else if (ucStatusofCC.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CAD"))
        {
            if (General.GetNullableString(txtCancelReason.Text) == null)
                ucError.ErrorMessage = "Cancel Reason is required.";
        }

        if (General.GetNullableDateTime(ucCompletiondate.Text) != null)
        {
            if (DateTime.Parse(ucCompletiondate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Completion date should not be the future date.";
        }

        if (chkClosed.Checked)
        {
            if (General.GetNullableString(txtCloseOutRemarks.Text) == null)
                ucError.ErrorMessage = "Close Out Remarks is required.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidCategoryRaiseOR()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucStatusofCC.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";

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

    protected void chkClosed_CheckedChanged(object sender, EventArgs e)
    {
        if (chkClosed.Checked)
            ucStatusofCC.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CLD");
        else
            BindInspectionIncident();
    }

    protected void chkEvidenceRequired_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkEvidenceRequired.Checked)
        //    imgEvidence.Visible = true;
        //else
        //    imgEvidence.Visible = false;
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
            if (ViewState["crewcomplaintid"] != null)
            {
                string script = "openNewWindow('Bank','','"+Session["sitepath"]+"/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=5&SOURCEREFERENCEID=" + ViewState["crewcomplaintid"].ToString() + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
        }
    }
}
