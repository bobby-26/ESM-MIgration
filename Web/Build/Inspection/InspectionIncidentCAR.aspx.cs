using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class InspectionIncidentCAR : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }

        imgReportedByShore.Attributes.Add("onclick", "return showPickList('spnReportedByShore1', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
               + PhoenixModule.QUALITY + "', true);");

        imgReportedByShore2.Attributes.Add("onclick", "return showPickList('spnReportedByShore2', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
               + PhoenixModule.QUALITY + "', true);");

        imgReportedByShore3.Attributes.Add("onclick", "return showPickList('spnReportedByShore3', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
               + PhoenixModule.QUALITY + "', true);");

        imgShoreTeamAppointedBy.Attributes.Add("onclick", "return showPickList('spnShoreTeamAppointedBy', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
                + PhoenixModule.QUALITY + "', true);");

        imgShoreTeamAppointedBy.Attributes.Add("onclick", "return showPickList('spnShoreTeamAppointedBy', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
        + PhoenixModule.QUALITY + "', true);");

        gvInspection.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["VESSELID"] = null;
            ViewState["INCIDENTDATE"] = "";

            VesselConfiguration();

            txtReportedByShoreId1.Attributes.Add("style", "visibility:hidden");
            txtReportedByShoreEmail1.Attributes.Add("style", "visibility:hidden");
            txtReportedByShipId1.Attributes.Add("style", "visibility:hidden");

            txtReportedByShoreId2.Attributes.Add("style", "visibility:hidden");
            txtReportedByShoreEmail2.Attributes.Add("style", "visibility:hidden");
            txtReportedByShipId2.Attributes.Add("style", "visibility:hidden");

            txtReportedByShoreId3.Attributes.Add("style", "visibility:hidden");
            txtReportedByShoreEmail3.Attributes.Add("style", "visibility:hidden");
            txtReportedByShipId3.Attributes.Add("style", "visibility:hidden");

            txtShoreTeamAppointedById.Attributes.Add("style", "visibility:hidden");
            txtShoreTeamAppointedByEmail.Attributes.Add("style", "visibility:hidden");

            txtImmediateAssignedToId.Attributes.Add("style", "visibility:hidden");

            if (Filter.CurrentIncidentID != null)
            {
                DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["INCIDENTSTATUS"] = ds.Tables[0].Rows[0]["FLDSTATUSOFINCIDENT"].ToString();
                    ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    ViewState["INCIDENTDATE"] = ds.Tables[0].Rows[0]["FLDDATEOFINCIDENT"].ToString();
                }
            }

            if (Filter.CurrentSelectedIncidentMenu != null)
            {
                gvInspection.Columns[4].Visible = false;
            }

            ViewState["CARID"] = null;
            ViewState["OLDINCIDENTTYPE"] = null;

            if (Filter.CurrentIncidentID != null)
                EditNonConformity(new Guid(Filter.CurrentIncidentID));
            if (Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                chkDetailsInvestigationYN.Enabled = true;
                chkDetailsInvestigationYN_CheckedChanged(chkDetailsInvestigationYN, new EventArgs());
            }
            else
                chkDetailsInvestigationYN.Enabled = false;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            imgReportedByShip1.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip1', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ViewState["VESSELID"].ToString() + "&Date=" + ViewState["INCIDENTDATE"].ToString() + "', true); ");

            imgReportedByShip2.Attributes.Add("onclick",
                   "return showPickList('spnReportedByShip2', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                   + ViewState["VESSELID"].ToString() + "&Date=" + ViewState["INCIDENTDATE"].ToString() + "', true); ");

            imgReportedByShip3.Attributes.Add("onclick",
                   "return showPickList('spnReportedByShip3', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                   + ViewState["VESSELID"].ToString() + "&Date=" + ViewState["INCIDENTDATE"].ToString() + "', true); ");

            ViewState["VALIDYN"] = "0";
            ViewState["VALIDDATIONMSG"] = "";

            SetSupdtFeedbackRights();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                imgShoreTeamAppointedBy.Visible = false;
                imgReportedByShore.Visible = false;
                imgReportedByShore2.Visible = false;
                imgReportedByShore3.Visible = false;

                InvisibleDirectorComments();
            }
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Review", "COMPLETEINVESTIGATION", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuInspectionNonConformity.AccessRights = this.ViewState;

        if (Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            MenuInspectionNonConformity.MenuList = toolbar.Show();
        }
        BindInvestigation();
    }

    private void SetSupdtFeedbackRights()
    {
        bool flag = true;

        DataSet ds1 = PhoenixInspectionEventSupdtFeedback.EventSupdtFeedbackEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , 3, General.GetNullableGuid(Filter.CurrentIncidentID.ToString()));

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

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("DRUGALCOHOLTEST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDrugAndAlcoholTest.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void InspectionNonConformity_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (Filter.CurrentIncidentID != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    if (IsValidNonConformity())
                    {
                        string reportedbyshore = null;
                        string reportedbtship = null;
                        reportedbyshore = txtReportedByShoreId1.Text.ToString() + "," + txtReportedByShoreId2.Text.ToString() + "," + txtReportedByShoreId3.Text.ToString() + ",";
                        reportedbtship = txtReportedByShipId1.Text.ToString() + "," + txtReportedByShipId2.Text.ToString() + "," + txtReportedByShipId3.Text.ToString() + ",";
                        if (reportedbyshore.ToString().Contains(",,"))
                            reportedbyshore = reportedbyshore.Trim().Replace(",,", ",");
                        if (reportedbtship.ToString().Contains(",,"))
                            reportedbtship = reportedbtship.Trim().Replace(",,", ",");

                        PhoenixInspectionIncidentCAR.InsertInspectionIncidentCARDetails(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(Filter.CurrentIncidentID)
                            , General.GetNullableString(txtSummaryDesc.Text)
                            , General.GetNullableString(txtComprehensiveDesc.Text)
                            , General.GetNullableString(txtImmediateActionTaken.Text)
                            , General.GetNullableInteger(txtImmediateAssignedToId.Text)
                            , reportedbtship
                            , General.GetNullableInteger(ucVentilation.SelectedQuick)
                            , General.GetNullableInteger(ucLightingCond.SelectedQuick)
                            , General.GetNullableInteger(chkContractorIncident.Checked.Equals(true) ? "1" : "0")
                            , General.GetNullableString(txtContractorDetails.Text)
                            , General.GetNullableInteger(txtShoreTeamAppointedById.Text)
                            , chkDetailsInvestigationYN.Checked == true ? 1 : 0
                            , reportedbyshore
                            , General.GetNullableString(txtExecutiveSummary.Text)
                            , General.GetNullableString(txtPostIncident.Text)
                            , General.GetNullableString(txtReviewOfEmergencyProcedures.Text)
                            , General.GetNullableString(txtInvestigationAndEvidence.Text)
                            , General.GetNullableString(txtDirectorComment.Text)
                            , General.GetNullableString(txtReviewRemarks.Text)
                            );

                        ucStatus.Text = "Investigation details updated.";

                        EditNonConformity(new Guid(Filter.CurrentIncidentID));
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                else if (CommandName.ToUpper().Equals("COMPLETEINVESTIGATION"))
                {
                    RadWindowManager1.RadConfirm("No more further changes can be done after the investigation is reviewed. Do you want to proceed?", "Confirm", 320, 150, null, "Confirm");
                }
            }

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentIncidentID != null)
            {
                PhoenixInspectionIncident.IncidentStatusReviewUpdate(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                    , General.GetNullableString(txtReviewRemarks.Text)
                                                                  );
                ucStatus.Text = "Investigation reviewed.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (Filter.CurrentIncidentID != null)
                {
                    string reportedbyshore = null;
                    string reportedbtship = null;
                    reportedbyshore = txtReportedByShoreId1.Text.ToString() + "," + txtReportedByShoreId2.Text.ToString() + "," + txtReportedByShoreId3.Text.ToString() + ",";
                    reportedbtship = txtReportedByShipId1.Text.ToString() + "," + txtReportedByShipId2.Text.ToString() + "," + txtReportedByShipId3.Text.ToString() + ",";
                    if (reportedbyshore.ToString().Contains(",,"))
                        reportedbyshore = reportedbyshore.Trim().Replace(",,", ",");
                    if (reportedbtship.ToString().Contains(",,"))
                        reportedbtship = reportedbtship.Trim().Replace(",,", ",");

                    PhoenixInspectionIncidentCAR.InsertInspectionIncidentCARDetails(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(Filter.CurrentIncidentID)
                            , General.GetNullableString(txtSummaryDesc.Text)
                            , General.GetNullableString(txtComprehensiveDesc.Text)
                            , General.GetNullableString(txtImmediateActionTaken.Text)
                            , General.GetNullableInteger(txtImmediateAssignedToId.Text)
                            , reportedbtship
                            , General.GetNullableInteger(ucVentilation.SelectedQuick)
                            , General.GetNullableInteger(ucLightingCond.SelectedQuick)
                            , General.GetNullableInteger(chkContractorIncident.Checked.Equals(true) ? "1" : "0")
                            , General.GetNullableString(txtContractorDetails.Text)
                            , General.GetNullableInteger(txtShoreTeamAppointedById.Text)
                            , chkDetailsInvestigationYN.Checked == true ? 1 : 0
                            , reportedbyshore
                            , General.GetNullableString(txtExecutiveSummary.Text)
                            , General.GetNullableString(txtPostIncident.Text)
                            , General.GetNullableString(txtReviewOfEmergencyProcedures.Text)
                            , General.GetNullableString(txtInvestigationAndEvidence.Text)
                            , General.GetNullableString(txtDirectorComment.Text)
                            , General.GetNullableString(txtReviewRemarks.Text)
                            );

                    ucStatus.Text = "Investigation details updated.";

                    EditNonConformity(new Guid(Filter.CurrentIncidentID));
                }

                String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (ucCM.confirmboxvalue == 0)
            {
                EditNonConformity(new Guid(Filter.CurrentIncidentID));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void EditNonConformity(Guid incidentid)
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(incidentid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtSummaryDesc.Text = dr["FLDDESCRIPTION"].ToString();
            txtComprehensiveDesc.Text = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();

            txtImmediateActionTaken.Text = dr["FLDIMMEDIATEACTION"].ToString();

            txtImmediateAssignedToId.Text = dr["FLDIMMEDIATEACTIONASSIGNEDTO"].ToString();
            txtImmediateAssignedToName.Text = dr["FLDIMMEDIATEACTIONASSIGNEDNAME"].ToString();
            txtImmediateAssignedToRank.Text = dr["FLDIMMEDIATEACTIONASSIGNEDDESIGNATION"].ToString();

            txtReportedByShipname1.Text = dr["FLDREPORTEDBYSHIPNAME1"].ToString();
            txtReportedByShipname2.Text = dr["FLDREPORTEDBYSHIPNAME2"].ToString();
            txtReportedByShipname3.Text = dr["FLDREPORTEDBYSHIPNAME3"].ToString();

            txtReportedByShipId1.Text = dr["FLDREPORTEDBYSHIPID1"].ToString();
            txtReportedByShipId2.Text = dr["FLDREPORTEDBYSHIPID2"].ToString();
            txtReportedByShipId3.Text = dr["FLDREPORTEDBYSHIPID3"].ToString();

            txtReportedByShipRank1.Text = dr["FLDREPORTEDBYSHIPDESIGNATION1"].ToString();
            txtReportedByShipRank2.Text = dr["FLDREPORTEDBYSHIPDESIGNATION2"].ToString();
            txtReportedByShipRank3.Text = dr["FLDREPORTEDBYSHIPDESIGNATION3"].ToString();

            ucVentilation.SelectedQuick = dr["FLDVENTILATIONYN"].ToString();
            ucLightingCond.SelectedQuick = dr["FLDLIGHTINGCONDITION"].ToString();
            if (dr["FLDISCONTRACTORRELATEDINCIDENT"].ToString() == "1")
                chkContractorIncident.Checked = true;
            else
                chkContractorIncident.Checked = false;
            txtContractorDetails.Text = dr["FLDCONTRACTORDETAILS"].ToString();

            if (dr["FLDDRUGALCOHOLTESTYN"].ToString() == "1")
                chkAlcoholtest.Checked = true;
            else
                chkAlcoholtest.Checked = false;

            txttestDate.Text = General.GetDateTimeToString(dr["FLDALCOHOLTESTDATE"].ToString());

            if (dr["FLDMAINTENANCEREQUIRED"].ToString() == "1")
                chkMaintenanceRequired.Checked = true;
            else
                chkMaintenanceRequired.Checked = false;

            txtWorkOrderNumber.Text = dr["FLDWORKORDERTEXT"].ToString();
            txtDirectorComment.Text = dr["FLDDIRECTORCOMMENT"].ToString();
            txtDirectorCommentedByName.Text = dr["FLDDIRECTORCOMMENTDBY"].ToString();
            ucDirectorCommentDate.Text = dr["FLDDIRECTORCOMMENTDATE"].ToString();
            txtReviewRemarks.Text = dr["FLDREVIEWREMARKS"].ToString();

            if (dr["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S1") || dr["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S2") || dr["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S5"))
            {
                txtDirectorComment.CssClass = "input";
                txtDirectorComment.Enabled = true;
            }
            else
            {
                txtDirectorComment.CssClass = "readonlytextbox";
                txtDirectorComment.Enabled = false;
            }
        }

        ds = PhoenixInspectionIncidentCAR.EditInspectionIncidentCAR(incidentid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["CARID"] = dr["FLDINSPECTIONINCIDENTCARID"].ToString();

            txtExecutiveSummary.Text = dr["FLDEXECUTIVESUMMARY"].ToString();
            txtPostIncident.Text = dr["FLDPOSTINCIDENT"].ToString();
            txtReviewOfEmergencyProcedures.Text = dr["FLDREVIEW"].ToString();
            txtInvestigationAndEvidence.Text = dr["FLDINVESTIGATIONANDEVIDENCE"].ToString();

            txtReportedByShoreName1.Text = dr["FLDREPORTEDBYSHORENAME1"].ToString();
            txtReportedByShoreName2.Text = dr["FLDREPORTEDBYSHORENAME2"].ToString();
            txtReportedByShoreName3.Text = dr["FLDREPORTEDBYSHORENAME3"].ToString();
            txtShoreTeamAppointedByName.Text = dr["FLDSHORETEAMAPPOINTEDBYNAME"].ToString();

            txtReportedByShoreId1.Text = dr["FLDREPORTEDBYSHOREID1"].ToString();
            txtReportedByShoreId2.Text = dr["FLDREPORTEDBYSHOREID2"].ToString();
            txtReportedByShoreId3.Text = dr["FLDREPORTEDBYSHOREID3"].ToString();
            txtShoreTeamAppointedById.Text = dr["FLDSHORETEAMAPPOINTEDBY"].ToString();

            txtReportedByShoreDesignation1.Text = dr["FLDREPORTEDBYSHOREDESIGNATION1"].ToString();
            txtReportedByShoreDesignation2.Text = dr["FLDREPORTEDBYSHOREDESIGNATION2"].ToString();
            txtReportedByShoreDesignation3.Text = dr["FLDREPORTEDBYSHOREDESIGNATION3"].ToString();
            txtShoreTeamAppointedByDesignation.Text = dr["FLDSHORETEAMAPPOINTEDBYDESIGNATION"].ToString();

            string strDetailsInvestigationYN = dr["FLDDETAILINVESTIGATIONYN"].ToString();
            if (strDetailsInvestigationYN != null && !string.IsNullOrEmpty(strDetailsInvestigationYN) && int.Parse(strDetailsInvestigationYN) == 1)
                chkDetailsInvestigationYN.Checked = true;
            else
                chkDetailsInvestigationYN.Checked = false;

            if (chkDetailsInvestigationYN.Checked == true)
            {
                txtExecutiveSummary.Enabled = txtPostIncident.Enabled = txtReviewOfEmergencyProcedures.Enabled = true;
                txtExecutiveSummary.CssClass = txtPostIncident.CssClass = txtReviewOfEmergencyProcedures.CssClass = "input";
                imgShoreTeamAppointedBy.Visible = true;
                imgReportedByShore.Visible = true;
                imgReportedByShore2.Visible = true;
                imgReportedByShore3.Visible = true;
            }
            else
            {
                txtExecutiveSummary.Enabled = txtPostIncident.Enabled = txtReviewOfEmergencyProcedures.Enabled = false;
                txtExecutiveSummary.CssClass = txtPostIncident.CssClass = txtReviewOfEmergencyProcedures.CssClass = "readonlytextbox";
                imgShoreTeamAppointedBy.Visible = false;
                imgReportedByShore.Visible = false;
                imgReportedByShore2.Visible = false;
                imgReportedByShore3.Visible = false;
            }
        }
        else
        {
            imgShoreTeamAppointedBy.Visible = false;
            imgReportedByShore.Visible = false;
            imgReportedByShore2.Visible = false;
            imgReportedByShore3.Visible = false;
        }
    }

    private bool IsValidNonConformity()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);

    }
    protected void chkDetailsInvestigationYN_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;

        if (chkDetailsInvestigationYN.Checked == true)
        {
            txtExecutiveSummary.Enabled = txtPostIncident.Enabled = txtReviewOfEmergencyProcedures.Enabled = true;
            txtExecutiveSummary.CssClass = txtPostIncident.CssClass = txtReviewOfEmergencyProcedures.CssClass = "input";
            imgShoreTeamAppointedBy.Visible = true;
            imgReportedByShore.Visible = true;
            imgReportedByShore2.Visible = true;
            imgReportedByShore3.Visible = true;
        }
        else
        {
            txtExecutiveSummary.Enabled = txtPostIncident.Enabled = txtReviewOfEmergencyProcedures.Enabled = false;
            txtExecutiveSummary.Text = txtPostIncident.Text = txtReviewOfEmergencyProcedures.Text = "";
            txtShoreTeamAppointedByName.Text = txtShoreTeamAppointedByDesignation.Text = txtShoreTeamAppointedById.Text = txtShoreTeamAppointedByEmail.Text = "";
            txtReportedByShoreName1.Text = txtReportedByShoreDesignation1.Text = txtReportedByShoreId1.Text = txtReportedByShoreEmail1.Text = "";
            txtReportedByShoreName2.Text = txtReportedByShoreDesignation2.Text = txtReportedByShoreId2.Text = txtReportedByShoreEmail2.Text = "";
            txtReportedByShoreName3.Text = txtReportedByShoreDesignation3.Text = txtReportedByShoreId3.Text = txtReportedByShoreEmail3.Text = "";
            txtExecutiveSummary.CssClass = txtPostIncident.CssClass = txtReviewOfEmergencyProcedures.CssClass = "readonlytextbox";
            imgShoreTeamAppointedBy.Visible = false;
            imgReportedByShore.Visible = false;
            imgReportedByShore2.Visible = false;
            imgReportedByShore3.Visible = false;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncidentCAR.IncidentTimeLineSearch(
                      Filter.CurrentIncidentID != null ? General.GetNullableGuid(Filter.CurrentIncidentID.ToString()) : General.GetNullableGuid(null)
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount);

        gvInspection.DataSource = ds;
        gvInspection.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvInspection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidTimeLine(
                     Filter.CurrentIncidentID.ToString(),
                    ((UserControlDate)e.Item.FindControl("ucDateAdd")).Text,
                    ((RadTimePicker)e.Item.FindControl("txtTimeAdd")).SelectedTime.ToString(),
                    ((RadTextBox)e.Item.FindControl("txtEventAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSNOAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                DateTime starttime = DateTime.Parse(((UserControlDate)e.Item.FindControl("ucDateAdd")).Text + " " +
                                           ((RadTimePicker)e.Item.FindControl("txtTimeAdd")).SelectedTime);

                PhoenixInspectionIncidentCAR.IncidentTimeLineInsert(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    new Guid(Filter.CurrentIncidentID.ToString()),
                                                                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSNOAdd")).Text),
                                                                    ((RadTextBox)e.Item.FindControl("txtEventAdd")).Text,
                                                                    ((RadTextBox)e.Item.FindControl("txtRemarkAdd")).Text,
                                                                     starttime
                                                                    );
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspection_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            VesselConfiguration();
            if (Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                gvInspection.Columns[4].Visible = true;
            else
                gvInspection.Columns[4].Visible = false;
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
        }
    }
    private bool IsValidTimeLine(string incidentid, string strdate, string strtime, string strevent, string serialnumber)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (string.IsNullOrEmpty(serialnumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (General.GetNullableDateTime(strdate) == null)
            ucError.ErrorMessage = "Date is required.";

        if (strtime.ToString().Equals("") || string.IsNullOrEmpty(strtime) || strtime.ToString().Equals("__:__"))
            ucError.ErrorMessage = "Time is required.";

        if (incidentid.Trim().Equals(""))
            ucError.ErrorMessage = "Please select an 'Incident'.";

        if (strevent.Trim().Equals(""))
            ucError.ErrorMessage = "Event is required.";

        return (!ucError.IsError);
    }

    protected void chkContractorIncident_CheckedChanged(object sender, EventArgs e)
    {
        ChangeStatus();
    }
    private void ChangeStatus()
    {
        if (chkContractorIncident.Checked.Equals(true))
        {
            txtContractorDetails.CssClass = "input_mandatory";
            txtContractorDetails.Enabled = true;
        }
        else
        {
            txtContractorDetails.CssClass = "input";
            txtContractorDetails.Enabled = false;
            txtContractorDetails.Text = "";
        }
    }
    private void InvisibleDirectorComments()
    {
        lblDirectorComment.Visible = false;
        txtDirectorComment.Visible = false;

        lblDirectorCommentDate.Visible = false;
        ucDirectorCommentDate.Visible = false;

        lblDirectorCommentedBy.Visible = false;
        txtDirectorCommentedByName.Visible = false;
    }
    private void BindInvestigation()
    {
        DataTable dt = PhoenixInspectionIncident.ListInvestigation(new Guid(Filter.CurrentIncidentID));

        gvInvestigation.DataSource = dt;
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
            string script = "openNewWindow('Bank','', '" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=3&SOURCEREFERENCEID=" + Filter.CurrentIncidentID.ToString() + "');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
        }
    }

    protected void gvInspection_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidTimeLine(
                     Filter.CurrentIncidentID.ToString(),
                    ((UserControlDate)e.Item.FindControl("ucDateEdit")).Text,
                    ((RadTimePicker)e.Item.FindControl("txtTimeEdit")).SelectedTime.ToString(),
                    ((RadTextBox)e.Item.FindControl("txtEventEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSNOEdit")).Text))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            DateTime starttime = DateTime.Parse(((UserControlDate)e.Item.FindControl("ucDateEdit")).Text + " " +
                                           ((RadTimePicker)e.Item.FindControl("txtTimeEdit")).SelectedTime);

            PhoenixInspectionIncidentCAR.IncidentTimeLineUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                new Guid(((RadLabel)e.Item.FindControl("lblTimeLineId")).Text),
                                                                new Guid(Filter.CurrentIncidentID.ToString()),
                                                                int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSNOEdit")).Text),
                                                                ((RadTextBox)e.Item.FindControl("txtEventEdit")).Text,
                                                                ((RadTextBox)e.Item.FindControl("txtRemarkEdit")).Text,
                                                                 starttime
                                                                );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspection_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixInspectionIncidentCAR.IncidentTimeLineDelete(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(((RadLabel)e.Item.FindControl("lblTimeLineId")).Text));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspection.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvInspection.SelectedIndexes.Clear();
        gvInspection.EditIndexes.Clear();
        gvInspection.DataSource = null;
        gvInspection.Rebind();
    }

    protected void gvInspection_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
