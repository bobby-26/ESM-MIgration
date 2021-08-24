using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionIncidentGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        imgPersonInCharge.Attributes.Add("onclick",
               "return showPickList('spnPersonInCharge', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");
        imgImmediateAssignedTo.Attributes.Add("onclick",
               "return showPickList('spnAssignedTo', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

        imgReportedByShip1.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip1', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

        imgReportedByShip2.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip2', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

        imgReportedByShip3.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip3', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

        imgReportedByOffice1.Attributes.Add("onclick", "return showPickList('spnReportedByOffice1', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
                + PhoenixModule.QUALITY + "', true);");
        imgReportedByOffice2.Attributes.Add("onclick", "return showPickList('spnReportedByOffice2', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
                + PhoenixModule.QUALITY + "', true);");
        imgReportedByOffice3.Attributes.Add("onclick", "return showPickList('spnReportedByOffice3', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
               + PhoenixModule.QUALITY + "', true);");
        imgPersonInChargeOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
              + PhoenixModule.QUALITY + "', true);");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {            
            if (!IsPostBack)
            {                
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["RISKASSESMENTID"] = "";
                ViewState["RISKASSESMENTTYPE"] = "";
                ViewState["NEWRISKASSESMENTID"] = "";
                ViewState["NEWRISKASSESMENTTYPE"] = "";
                ViewState["INCIDENTDATE"] = "";
                ViewState["OFFICEYN"] = "0";
                ViewState["VESSELID"] = "-1";


                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    BindVessel();
                }

                txtCrewId.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId.Attributes.Add("style", "visibility:hidden");
                txtImmediateAssignedToId.Attributes.Add("style", "visibility:hidden");

                txtReportedByShipId1.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId2.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId3.Attributes.Add("style", "visibility:hidden");

                txtReportedByOfficeId1.Attributes.Add("style", "visibility:hidden");
                txtReportedByOfficeId2.Attributes.Add("style", "visibility:hidden");
                txtReportedByOfficeId3.Attributes.Add("style", "visibility:hidden");

                txtPersonInChargeOfficeId.Attributes.Add("style", "visibility:hidden");

                txtReportedByOfficeEmail1.Attributes.Add("style", "visibility:hidden");
                txtReportedByOfficeEmail2.Attributes.Add("style", "visibility:hidden");
                txtReportedByOfficeEmail3.Attributes.Add("style", "visibility:hidden");

                txtPersonInChargeOfficeEmail.Attributes.Add("style", "visibility:hidden");
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                imgReportedByShip1.Attributes.Add("onclick",
                        "return showPickList('spnReportedByShip1', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                        + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

                imgReportedByShip2.Attributes.Add("onclick",
                       "return showPickList('spnReportedByShip2', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                       + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

                imgReportedByShip3.Attributes.Add("onclick",
                       "return showPickList('spnReportedByShip3', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                       + ucVessel.SelectedValue + "&Date=" + txtDateOfIncident.Text + "', true); ");

                
                if (Filter.CurrentIncidentID != null)
                {
                    BindInspectionIncident();
                }
                else
                {
                    Reset();
                }
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if ((ViewState["OFFICEYN"].ToString() != "0"))
            {
                toolbar.AddButton("Raise Machinery Damage", "RAISEMACHINERY", ToolBarDirection.Right);
            }
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionIncident.AccessRights = this.ViewState;
            if (Filter.CurrentSelectedIncidentMenu == null)
                MenuInspectionIncident.MenuList = toolbar.Show();
            BindInvestigation();
            txtDescription.Resize = ResizeMode.Both;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindInspectionIncident()
    {
        DataSet ds;

        ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID.ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucVessel.ClearSelection();
            ucVessel.SelectedValue = dr["FLDVESSELID"].ToString();
            ViewState["OFFICEYN"] = dr["FLDVESSELID"].ToString();
            ucVessel.Enabled = false;
            ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();

            txtRefNo.Text = dr["FLDINCIDENTREFNO"].ToString();
            txtTitle.Text = dr["FLDINCIDENTTITLE"].ToString();
            txtDateOfIncident.Text = General.GetDateTimeToString(dr["FLDINCIDENTDATE"].ToString());
            if (dr["FLDINCIDENTDATETIME"].ToString() != "")
            {
                txtTimeOfIncident.SelectedDate = Convert.ToDateTime(dr["FLDINCIDENTDATETIME"].ToString());
            }
            if (dr["FLDINCIDENTDATETIME"].ToString() == null)
            {
                txtTimeOfIncident.SelectedDate = null;
            }
            txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            //txtWindCondition.Text = dr["FLDWINDCONDITION"].ToString();
            txtSeaCondition.Text = dr["FLDSEACONDITION"].ToString();
            ucWindCondionScale.SelectedQuick = dr["FLDWINDCONDITIONSCALE"].ToString();
            txtWindDirection.Text = dr["FLDWINDDIRECTION"].ToString();
            ucSwellLength.SelectedQuick = dr["FLDSWELLLENGTH"].ToString();
            ucSwellHeight.SelectedQuick = dr["FLDSWELLHEIGHT"].ToString();
            txtSwellDirection.Text = dr["FLDSWELLDIRECTION"].ToString();

            ucOnboardLocation.SelectedQuick = dr["FLDLOCATIONOFINCIDENT"].ToString();
            txtDateOfReport.Text = General.GetDateTimeToString(dr["FLDREPORTEDDATE"].ToString());
            ucConsequenceCategory.SelectedHard = dr["FLDINCIDENTCATEGORY"].ToString();
            ucPotentialCategory.SelectedHard = dr["FLDPOTENTIALCATEGORY"].ToString();
            ucActivity.SelectedHard = dr["FLDACTIVITYRELEVENT"].ToString();

            ucLatitude.Text = dr["FLDLATITUDE"].ToString();
            ucLongitude.Text = dr["FLDLONGITUDE"].ToString();
            txtComprehensiveDescription.Text = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();

            if (dr["FLDACTIVITYRELEVENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(1, 170, "OTH")))
            {
                txtOtherActivity.CssClass = "input_mandatory";
                txtOtherActivity.ReadOnly = false;
            }

            txtOtherActivity.Text = dr["FLDOTHERACTIVITY"].ToString();

            if (dr["FLDVESSELID"].ToString() == "0")
            {
                txtPersonInChargeOfficeId.Text = dr["FLDPERSONINCHARGE"].ToString();
                txtOfficePersonName.Text = dr["FLDPERSONINCHARGENAME"].ToString();
                txtOfficePersonDesignation.Text = dr["FLDPERSONINCHARGERANK"].ToString();
            }
            else
            {
                txtCrewId.Text = dr["FLDPERSONINCHARGE"].ToString();
                txtCrewName.Text = dr["FLDPERSONINCHARGENAME"].ToString();
                txtCrewRank.Text = dr["FLDPERSONINCHARGERANK"].ToString();
            }

            txtReportedByShipName.Text = dr["FLDREPORTEDBYNAME"].ToString();
            txtReportedbyDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();
            txtReportedByShipId.Text = dr["FLDREPORTEDBYID"].ToString();

            txtReportedbyDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();

            ucPort.SelectedSeaport = dr["FLDPORT"].ToString();
            ucQuickVesselActivity.SelectedQuick = dr["FLDVESSELACTIVITY"].ToString();

            txtCurrent.Text = dr["FLDCURRENTANDVISIBILITY"].ToString();
            txtVisibility.Text = dr["FLDVISIBILITY"].ToString();

            rblIncidentNearmiss.SelectedValue = dr["FLDISINCIDENTORNEARMISS"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();

            if (dr["FLDISINCIDENTORNEARMISS"].ToString() == "1")
            {
                ucCategory.CssClass = "input_mandatory";
                ucSubcategory.CssClass = "input_mandatory";
                ucConsequenceCategory.Enabled = true;
                ucPotentialCategory.Enabled = false;
                ucPotentialCategory.SelectedHard = "";
                txtConsequencePotential.Text = dr["FLDINCIDENTCATEGORYNAME"].ToString();
                lblConsequencePotential.Text = "Consequence Category";
            }
            else
            {
                ucPotentialCategory.Enabled = true;
                ucConsequenceCategory.Enabled = false;
                ucPotentialCategory.SelectedHard = "";
                txtConsequencePotential.Text = dr["FLDPOTENTIALCATEGORYNAME"].ToString();
                lblConsequencePotential.Text = "Potential Category";
            }
            if (dr["FLDVESSELID"].ToString() == "0")
            {
                ucCompany.CssClass = "input_mandatory";
                ucOnboardLocation.CssClass = "input";
                ucActivity.CssClass = "input";
            }
            else
            {
                ucCompany.CssClass = "input";
                ucOnboardLocation.CssClass = "dropdown_mandatory";
                ucActivity.CssClass = "dropdown_mandatory";
            }
            ucCategory.TypeId = dr["FLDISINCIDENTORNEARMISS"].ToString();
            ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(dr["FLDISINCIDENTORNEARMISS"].ToString()));
            ucCategory.DataBind();
            ucCategory.SelectedCategory = dr["FLDCATEGORY"].ToString();

            ucSubcategory.CategoryId = dr["FLDCATEGORY"].ToString();
            ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(dr["FLDCATEGORY"].ToString()));
            ucSubcategory.DataBind();
            ucSubcategory.SelectedSubCategory = dr["FLDSUBCATEGORY"].ToString();

            if (dr["FLDRAISEDFROM"].ToString() == "1")
                txtRemarks.Enabled = false;
            else
                txtRemarks.Enabled = true;
            txtReviewcategory.Text = dr["FLDREVIEWCATEGORYNAME"].ToString();

            txtCancelReason.Text = dr["FLDINCIDENTCANCELREASON"].ToString();
            ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
            txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
            txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
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

            txtReportedByOfficeByName1.Text = dr["FLDREPORTEDBYOFFICENAME1"].ToString();
            txtReportedByOfficeByName2.Text = dr["FLDREPORTEDBYOFFICENAME2"].ToString();
            txtReportedByOfficeByName3.Text = dr["FLDREPORTEDBYOFFICENAME3"].ToString();

            txtReportedByOfficeId1.Text = dr["FLDREPORTEDBYOFFICEID1"].ToString();
            txtReportedByOfficeId2.Text = dr["FLDREPORTEDBYOFFICEID2"].ToString();
            txtReportedByOfficeId3.Text = dr["FLDREPORTEDBYOFFICEID3"].ToString();

            txtReportedByOfficeByDesignation1.Text = dr["FLDREPORTEDBYOFFICEDESIGNATION1"].ToString();
            txtReportedByOfficeByDesignation2.Text = dr["FLDREPORTEDBYOFFICEDESIGNATION2"].ToString();
            txtReportedByOfficeByDesignation3.Text = dr["FLDREPORTEDBYOFFICEDESIGNATION3"].ToString();

            ucVentilation.SelectedQuick = dr["FLDVENTILATIONYN"].ToString();
            ucLightingCond.SelectedQuick = dr["FLDLIGHTINGCONDITION"].ToString();
            if (dr["FLDISCONTRACTORRELATEDINCIDENT"].ToString() == "1")
                chkContractorIncident.Checked = true;
            else
                chkContractorIncident.Checked = false;
            txtContractorDetails.Text = dr["FLDCONTRACTORDETAILS"].ToString();
            ChangeStatus();

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

            chkCriticalEquipmentYN.Checked = (dr["FLDCRITICALEQUIPMENTYN"].ToString() == "1" ? true : false);

            txtRANumber.Text = dr["FLDRAREFNO"].ToString();
            txtRA.Text = dr["FLDRISKASSESSMENT"].ToString();
            txtRAId.Text = dr["FLDRISKASSESSMENTID"].ToString();
            txtRaType.Text = dr["FLDRISKASSESSMENTTYPE"].ToString();
            ViewState["RISKASSESSMENTID"] = dr["FLDRISKASSESSMENTID"].ToString();
            ViewState["RISKASSESMENTTYPE"] = dr["FLDRISKASSESSMENTTYPE"].ToString();

            ViewState["NEWRISKASSESSMENTID"] = dr["FLDRISKASSESSMENTID"].ToString();
            ViewState["NEWRISKASSESMENTTYPE"] = dr["FLDRISKASSESSMENTTYPE"].ToString();

            txtNewRANumber.Text = dr["FLDNEWRAREFNO"].ToString();
            txtNewRA.Text = dr["FLDNEWRISKASSESSMENT"].ToString();
            txtNewRAId.Text = dr["FLDNEWRISKASSESSMENTID"].ToString();
            txtNewRaType.Text = dr["FLDNEWRISKASSESSMENTTYPE"].ToString();

            if (General.GetNullableGuid(dr["FLDRISKASSESSMENTID"].ToString()) != null)
            {
                cmdRA.Visible = true;
                SetReport();
            }
            else
            {
                cmdRA.Visible = false;
            }

            if (General.GetNullableGuid(dr["FLDNEWRISKASSESSMENTID"].ToString()) != null)
            {
                cmdNewRA.Visible = true;
                NewSetReport();
            }
            else
            {
                cmdNewRA.Visible = false;
            }

            if (dr["FLDCRITICALEQUIPMENTYN"].ToString() == "1")
            {
                txtRANumber.CssClass = "input_mandatory";
                txtRA.CssClass = "input_mandatory";
                txtNewRANumber.CssClass = "input_mandatory";
                txtNewRA.CssClass = "input_mandatory";
            }
            else
            {
                txtRANumber.CssClass = "input";
                txtRA.CssClass = "input";
                txtNewRANumber.CssClass = "input";
                txtNewRA.CssClass = "input";
            }
            imgShowRA.Attributes.Add("onclick",
                       "return showPickList('spnRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?OPT=G,N,M,C&vesselid="
                       + dr["FLDVESSELID"].ToString() + "', true); ");

            imgShowNewRA.Attributes.Add("onclick",
                       "return showPickList('spnNewRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRAExtn.aspx?OPT=G,N,M,C&vesselid="
                       + dr["FLDVESSELID"].ToString() + "', true); ");

            SetPickList(dr["FLDVESSELID"].ToString());
            txtReviewRemarks.Text = dr["FLDREVIEWREMARKS"].ToString();
            ucReviewedDate.Text = dr["FLDREVIEWEDDATE"].ToString();
            txtReviewedBy.Text = dr["FLDREVIEWEDBY"].ToString();
            txtmachinerynumber.Text = dr["FLDMACHINERYDAMAGEREFNUMBER"].ToString();
        }
    }

    private void SetPickList(string vslid)
    {
        if (General.GetNullableInteger(vslid) > 0)
        {
            spnReportedByShip1.Visible = true;
            spnReportedByShip2.Visible = true;
            spnReportedByShip3.Visible = true;

            spnReportedByOffice1.Visible = false;
            spnReportedByOffice2.Visible = false;
            spnReportedByOffice3.Visible = false;

            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
        else
        {
            spnReportedByShip1.Visible = false;
            spnReportedByShip2.Visible = false;
            spnReportedByShip3.Visible = false;

            spnReportedByOffice1.Visible = true;
            spnReportedByOffice2.Visible = true;
            spnReportedByOffice3.Visible = true;

            spnPersonInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
    }
    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            string reportedby = null;

            if (ucVessel.SelectedValue == "0")
            {
                reportedby = txtReportedByOfficeId1.Text.ToString() + "," + txtReportedByOfficeId2.Text.ToString() + "," + txtReportedByOfficeId3.Text.ToString() + ",";
            }
            else
            {
                reportedby = txtReportedByShipId1.Text.ToString() + "," + txtReportedByShipId2.Text.ToString() + "," + txtReportedByShipId3.Text.ToString() + ",";
            }

            if (reportedby.ToString().Contains(",,"))
                reportedby = reportedby.Trim().Replace(",,", ",");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionIncident())
                {
                    TimeSpan timeofincident = txtTimeOfIncident.SelectedTime.Value;

                    int? incidentyn = General.GetNullableInteger(rblIncidentNearmiss.SelectedValue);

                    string personincharge = "";

                    if (ucVessel.SelectedValue == "0")
                        personincharge = txtPersonInChargeOfficeId.Text;
                    else
                        personincharge = txtCrewId.Text;

                    PhoenixInspectionIncident.UpdateInspectionIncident(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(Filter.CurrentIncidentID),
                                General.GetNullableInteger(rblIncidentNearmiss.SelectedValue),
                                txtTitle.Text,
                                General.GetNullableDateTime(txtDateOfIncident.Text + " " + timeofincident),
                                General.GetNullableInteger(ucQuickVesselActivity.SelectedQuick),
                                General.GetNullableGuid(ucCategory.SelectedCategory),
                                General.GetNullableGuid(ucSubcategory.SelectedSubCategory),
                                General.GetNullableString(txtCurrent.Text),
                                General.GetNullableString(txtVisibility.Text),
                                ucLatitude.Text,
                                ucLongitude.Text,
                                General.GetNullableInteger(ucPort.SelectedSeaport),
                                General.GetNullableString(txtSeaCondition.Text),
                                General.GetNullableInteger(ucWindCondionScale.SelectedQuick),
                                txtWindDirection.Text.Trim(),
                                General.GetNullableInteger(ucSwellLength.SelectedQuick),
                                General.GetNullableInteger(ucSwellHeight.SelectedQuick),
                                txtSwellDirection.Text.Trim(),
                                General.GetNullableInteger(ucOnboardLocation.SelectedQuick),
                                General.GetNullableInteger(ucActivity.SelectedHard),
                                txtOtherActivity.Text,
                                General.GetNullableInteger(personincharge),
                                General.GetNullableString(txtDescription.Text),
                                General.GetNullableString(txtRemarks.Text),
                                General.GetNullableString(txtComprehensiveDescription.Text),
                                General.GetNullableString(txtImmediateActionTaken.Text),
                                General.GetNullableInteger(txtImmediateAssignedToId.Text),
                                reportedby,
                                General.GetNullableInteger(ucVentilation.SelectedQuick),
                                General.GetNullableInteger(ucLightingCond.SelectedQuick),
                                General.GetNullableInteger(chkContractorIncident.Checked.Equals(true) ? "1" : "0"),
                                General.GetNullableString(txtContractorDetails.Text),
                                General.GetNullableInteger(chkCriticalEquipmentYN.Checked == true ? "1" : "0"),
                                General.GetNullableGuid(txtRAId.Text),
                                General.GetNullableInteger(txtRaType.Text),
                                General.GetNullableDateTime(txtDateOfReport.Text),
                                General.GetNullableGuid(txtNewRAId.Text),
                                General.GetNullableInteger(txtNewRaType.Text)
                        );

                    ucStatus.Text = "Incident details updated.";
                    ucStatus.Visible = true;

                    Filter.CurrentIncidentTab = "INCIDENTDETAILS";

                    //String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
                    //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

            if (CommandName.ToUpper().Equals("RAISEMACHINERY"))
            {
                RadWindowManager1.RadConfirm("Do you want to raise an Machinery Damage / Failure.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Activity_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucActivity = (UserControlHard)sender;

            if (ucActivity.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 170, "OTH"))
            {
                txtOtherActivity.CssClass = "input_mandatory";
                txtOtherActivity.ReadOnly = false;
            }
            else
            {
                txtOtherActivity.CssClass = "readonlytextbox";
                txtOtherActivity.ReadOnly = true;
                txtOtherActivity.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInspectionIncident()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucVessel.SelectedValue) == null)
            ucError.ErrorMessage = "'Vessel' is required.";

        if (ucVessel.SelectedValue.ToUpper().Equals("-- OFFICE --"))
        {
            if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
                ucError.ErrorMessage = "'Company' is required";
        }

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "'Title' is required.";

        if (General.GetNullableDateTime(txtDateOfIncident.Text) == null)
            ucError.ErrorMessage = "'Date of Incident' is required.";

        if (txtTimeOfIncident.SelectedTime == null)
            ucError.ErrorMessage = "'Time of Incident' is required.";
        else
        {
            if (General.GetNullableDateTime(txtDateOfIncident.Text + " " + txtTimeOfIncident.SelectedTime) == null)
                ucError.ErrorMessage = "'Time of Incident' is not a valid time.";
        }

        if (General.GetNullableDateTime(txtDateOfIncident.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "'Date of Incident' should not be the future date.";

        if (General.GetNullableGuid(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "'Incident Category' is required.";

        if (General.GetNullableGuid(ucSubcategory.SelectedSubCategory) == null)
            ucError.ErrorMessage = "'Incident Subcategory' is required.";

        if (!string.IsNullOrEmpty(ucLatitude.TextDegree) && (Convert.ToInt32(ucLatitude.TextDegree) < 0 || Convert.ToInt32(ucLatitude.TextDegree) > 90))
            ucError.ErrorMessage = "Latitude Degree should not exceed 90&deg";

        if (!string.IsNullOrEmpty(ucLatitude.TextMinute) && (Convert.ToInt32(ucLatitude.TextMinute) < 0 || Convert.ToInt32(ucLatitude.TextMinute) > 59))
            ucError.ErrorMessage = "Latitude Minutes should not equal to or exceed 60 minutes";

        if (!string.IsNullOrEmpty(ucLatitude.TextSecond) && (Convert.ToInt32(ucLatitude.TextSecond) < 0 || Convert.ToInt32(ucLatitude.TextSecond) > 59))
            ucError.ErrorMessage = "Latitude Seconds should not equal to or exceed 60 seconds";

        if (!string.IsNullOrEmpty(ucLongitude.TextDegree) && (Convert.ToInt32(ucLongitude.TextDegree) < 0 || Convert.ToInt32(ucLongitude.TextDegree) > 180))
            ucError.ErrorMessage = "Longitude Degree should not exceed 180&deg";

        if (!string.IsNullOrEmpty(ucLongitude.TextMinute) && (Convert.ToInt32(ucLongitude.TextMinute) < 0 || Convert.ToInt32(ucLongitude.TextMinute) > 59))
            ucError.ErrorMessage = "Longitude Minutes should not equal to or exceed 60 minutes";

        if (!string.IsNullOrEmpty(ucLongitude.TextSecond) && (Convert.ToInt32(ucLongitude.TextSecond) < 0 || Convert.ToInt32(ucLongitude.TextSecond) > 59))
            ucError.ErrorMessage = "Longitude Seconds should not equal to or exceed 60 seconds";

        if (txtOtherActivity.CssClass.Equals("input_mandatory") && txtOtherActivity.Text.Trim().Equals(""))
            ucError.ErrorMessage = "'Other Activity relevent' is required.";

        if (!ucVessel.SelectedValue.ToUpper().Equals("-- OFFICE --"))
        {
            if (General.GetNullableInteger(ucOnboardLocation.SelectedQuick) == null)
                ucError.ErrorMessage = "'Onboard Location' is required.";

            if (General.GetNullableInteger(ucActivity.SelectedHard) == null)
                ucError.ErrorMessage = "'Activity relevent to the Event' is required.";
        }

        if (General.GetNullableString(txtDescription.Text.Trim()) == null)
            ucError.ErrorMessage = "'Brief Description' is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ucVessel.Enabled = true;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedValue = Filter.CurrentVesselConfiguration != null ? Filter.CurrentVesselConfiguration.ToString() : "";
        }
        ucCompany.SelectedCompany = "";
        txtRefNo.Text = "";
        txtTitle.Text = "";
        txtDateOfIncident.Text = "";
        txtTimeOfIncident.SelectedTime = null;
        txtReportedbyDesignation.Text = "";
        txtReportedByShipId.Text = "";
        txtReportedByShipName.Text = "";
        txtDescription.Text = "";
        //txtWindCondition.Text = "";
        txtSeaCondition.Text = "";
        //txtWeather.Text = "";
        ucOnboardLocation.SelectedQuick = "";
        ucWindCondionScale.SelectedQuick = "";
        txtWindDirection.Text = "";
        ucSwellLength.SelectedQuick = "";
        ucSwellHeight.SelectedQuick = "";
        txtSwellDirection.Text = "";
        txtDateOfReport.Text = "";
        ucConsequenceCategory.SelectedHard = "";
        ucPotentialCategory.SelectedHard = "";
        ucActivity.SelectedHard = "";
        txtOtherActivity.Text = "";
        txtCrewName.Text = "";
        txtCrewId.Text = "";
        txtCrewRank.Text = "";

        txtOtherActivity.CssClass = "readonlytextbox";
        txtOtherActivity.ReadOnly = true;

        ucPort.SelectedSeaport = "";
        ucLatitude.Clear();
        ucLongitude.Clear();
        ucLatitude.TextSecond = "00";
        ucLongitude.TextSecond = "00";
        ucQuickVesselActivity.SelectedQuick = "";
        txtCurrent.Text = "";
        txtCurrent.CssClass = "input";
        txtVisibility.Text = "";
        txtVisibility.CssClass = "input";
        rblIncidentNearmiss.SelectedValue = "1";
    }

    protected void ucConsequenceCategory_Changed(object sender, EventArgs e)
    {

    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {

    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {

    }

    protected void rblIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        ucCategory.TypeId = rblIncidentNearmiss.SelectedValue;
        ucCategory.SelectedCategory = "";
        ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(rblIncidentNearmiss.SelectedValue));
        ucCategory.DataBind();

        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SelectedSubCategory = "";
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();

        if (rblIncidentNearmiss.SelectedValue == "1")
        {
            ucCategory.CssClass = "input_mandatory";
            ucSubcategory.CssClass = "input_mandatory";
            ucConsequenceCategory.Enabled = true;
            if (General.GetNullableInteger(ucPotentialCategory.SelectedHard) != null)
                ucConsequenceCategory.SelectedHard = ucPotentialCategory.SelectedHard;
            ucPotentialCategory.Enabled = false;
            ucPotentialCategory.SelectedHard = "";
        }
        if (rblIncidentNearmiss.SelectedValue == "2" || rblIncidentNearmiss.SelectedValue == "3")
        {
            if (General.GetNullableInteger(ucConsequenceCategory.SelectedHard) != null)
                ucPotentialCategory.SelectedHard = ucConsequenceCategory.SelectedHard;
            ucConsequenceCategory.SelectedHard = "";
            ucConsequenceCategory.Enabled = false;
            ucPotentialCategory.Enabled = true;
            ucConsequenceCategory.SelectedHard = "";
        }
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SelectedSubCategory = "";
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
        BindInvestigation();
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
    private void BindData()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncident.IncidentFindingsSearch(
                      Filter.CurrentIncidentID != null ? General.GetNullableGuid(Filter.CurrentIncidentID.ToString()) : General.GetNullableGuid(null)
                    , sortexpression
                    , sortdirection);

        gvFindings.DataSource = ds;
    }

    protected void gvFindings_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidFindings(
                    ((RadTextBox)e.Item.FindControl("txtFindingsAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSNOAdd")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlContactTypeAdd")).SelectedValue))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionIncident.IncidentFindingsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    new Guid(Filter.CurrentIncidentID.ToString()),
                                                                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSNOAdd")).Text),
                                                                    ((RadTextBox)e.Item.FindControl("txtFindingsAdd")).Text,
                                                                    General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlContactTypeAdd")).SelectedValue)
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
    protected void gvFindings_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
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

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlContactTypeEdit = (RadComboBox)e.Item.FindControl("ddlContactTypeEdit");
            if (ddlContactTypeEdit != null)
            {
                ddlContactTypeEdit.DataSource = PhoenixInspectionContractType.ListContactType();
                ddlContactTypeEdit.DataTextField = "FLDCONTACTTYPENAME";
                ddlContactTypeEdit.DataValueField = "FLDCONTACTTYPEID";
                ddlContactTypeEdit.DataBind();
                ddlContactTypeEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlContactTypeEdit.SelectedValue = dr["FLDCONTACTTYPEID"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            RadComboBox ddlContactTypeAdd = (RadComboBox)e.Item.FindControl("ddlContactTypeAdd");
            if (ddlContactTypeAdd != null)
            {
                ddlContactTypeAdd.DataSource = PhoenixInspectionContractType.ListContactType();
                ddlContactTypeAdd.DataTextField = "FLDCONTACTTYPENAME";
                ddlContactTypeAdd.DataValueField = "FLDCONTACTTYPEID";
                ddlContactTypeAdd.DataBind();
                ddlContactTypeAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }

    private bool IsValidFindings(string strfindings, string serialnumber, string contacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (string.IsNullOrEmpty(serialnumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (strfindings.Trim().Equals(""))
            ucError.ErrorMessage = "Finding is required.";

        if (General.GetNullableGuid(contacttype) == null)
            ucError.ErrorMessage = "Contact Type is required.";

        return (!ucError.IsError);
    }

    private void BindInvestigation()
    {
        DataTable dt = PhoenixInspectionIncident.ListInvestigation(new Guid(Filter.CurrentIncidentID), int.Parse(rblIncidentNearmiss.SelectedValue), General.GetNullableGuid(ucCategory.SelectedValue));

        gvInvestigation.DataSource = dt;
        gvInvestigation.DataBind();
    }

    protected void gvInvestigation_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvInvestigation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadRadioButtonList rblAnswer = (RadRadioButtonList)e.Item.FindControl("rblAnswer");
            if (rblAnswer != null)
            {
                rblAnswer.DataSource = PhoenixInspectionIncident.ListInspectionAnswer(1, 36, 1, "YES,NO,NA");
                rblAnswer.DataBindings.DataTextField = "FLDHARDNAME";
                rblAnswer.DataBindings.DataValueField = "FLDHARDCODE";
                rblAnswer.DataBind();
                rblAnswer.SelectedValue = dr["FLDANSWER"].ToString();
            }
        }
    }
    private bool IsValidInvestigation(string answer)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(answer) == null)
            ucError.ErrorMessage = "Answer is required.";

        return (!ucError.IsError);
    }

    public void rblAnswer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadRadioButtonList rblAnswer = (RadRadioButtonList)sender;

            GridDataItem gvrow = (GridDataItem)rblAnswer.Parent.Parent;

            string lblInvestigationId = ((RadLabel)gvrow.FindControl("lblInvestigationId")).Text;

            string lblQuestionId = ((RadLabel)gvrow.FindControl("lblQuestionId")).Text;

            if (lblInvestigationId != null && lblInvestigationId != "")
            {
                PhoenixInspectionIncident.IncidentInvestigationUpdate(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    new Guid(lblInvestigationId),
                                                                    new Guid(Filter.CurrentIncidentID),
                                                                    new Guid(lblQuestionId),
                                                                    int.Parse(((RadRadioButtonList)gvrow.FindControl("rblAnswer")).SelectedValue)
                                                                    );
            }
            else
            {
                PhoenixInspectionIncident.IncidentInvestigationInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    new Guid(Filter.CurrentIncidentID),
                                                                    new Guid(lblQuestionId),
                                                                    int.Parse(((RadRadioButtonList)gvrow.FindControl("rblAnswer")).SelectedValue));
            }

            BindInvestigation();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindInvestigation();
        }
    }
    protected void chkCriticalEquipmentYN_Checked(object sender, EventArgs e)
    {
        if (chkCriticalEquipmentYN.Checked == true)
        {
            txtRANumber.CssClass = "input_mandatory";
            txtRA.CssClass = "input_mandatory";
        }
        else
        {
            txtRANumber.CssClass = "input";
            txtRA.CssClass = "input";
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string script = "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentRaiseMachineryDamage.aspx')\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
            BindInspectionIncident();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdRAClear_Click(object sender, EventArgs e)
    {
        cmdRA.Visible = false;

        ViewState["RISKASSESMENTID"] = "";
        ViewState["RISKASSESMENTTYPE"] = "";

        txtRANumber.Text = "";
        txtRA.Text = "";
        txtRAId.Text = "";
        txtRaType.Text = "";
    }

    protected void cmdNEWRAClear_Click(object sender, EventArgs e)
    {
        cmdNewRA.Visible = false;

        ViewState["NEWRISKASSESMENTID"] = "";
        ViewState["NEWRISKASSESMENTTYPE"] = "";

        txtNewRANumber.Text = "";
        txtNewRA.Text = "";
        txtNewRAId.Text = "";
        txtNewRaType.Text = "";
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindInspectionIncident();
        Rebind();
        BindInvestigation();
    }
    private void SetReport()
    {
        if (ViewState["RISKASSESMENTTYPE"].ToString() == "2")
        {
            if (cmdRA != null)
            {
                {
                    cmdRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + ViewState["RISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
        else if (ViewState["RISKASSESMENTTYPE"].ToString() == "3")
        {
            if (cmdRA != null)
            {
                {
                    cmdRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + ViewState["RISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
        else if (ViewState["RISKASSESMENTTYPE"].ToString() == "4")
        {
            if (cmdRA != null)
            {
                {
                    cmdRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + ViewState["RISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }

        else if (ViewState["RISKASSESMENTTYPE"].ToString() == "5")
        {
            if (cmdRA != null)
            {
                {
                    cmdRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + ViewState["RISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
    }

    private void NewSetReport()
    {
        if (ViewState["NEWRISKASSESMENTTYPE"].ToString() == "2")
        {
            if (cmdNewRA != null)
            {
                {
                    cmdNewRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERICNEW&genericid=" + ViewState["NEWRISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
        else if (ViewState["NEWRISKASSESMENTTYPE"].ToString() == "3")
        {
            if (cmdNewRA != null)
            {
                {
                    cmdNewRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + ViewState["NEWRISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
        else if (ViewState["NEWRISKASSESMENTTYPE"].ToString() == "4")
        {
            if (cmdNewRA != null)
            {
                {
                    cmdNewRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATIONNEW&navigationid=" + ViewState["NEWRISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }

        else if (ViewState["NEWRISKASSESMENTTYPE"].ToString() == "5")
        {
            if (cmdNewRA != null)
            {
                {
                    cmdNewRA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGONEW&genericid=" + ViewState["NEWRISKASSESSMENTID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                }
            }
        }
    }

    protected void gvFindings_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidFindings(
                    ((RadTextBox)e.Item.FindControl("txtFindingsEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSNOEdit")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlContactTypeEdit")).SelectedValue))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionIncident.IncidentFindingsUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                new Guid(((RadLabel)e.Item.FindControl("lblFindingsIdEdit")).Text),
                                                                new Guid(Filter.CurrentIncidentID),
                                                                int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSNOEdit")).Text),
                                                                ((RadTextBox)e.Item.FindControl("txtFindingsEdit")).Text,
                                                                General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlContactTypeEdit")).SelectedValue)
                                                                );
            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFindings_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixInspectionIncident.IncidentFindingsDelete(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(((RadLabel)e.Item.FindControl("lblFindingsId")).Text));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFindings_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFindings.CurrentPageIndex + 1;
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
        gvFindings.SelectedIndexes.Clear();
        gvFindings.EditIndexes.Clear();
        gvFindings.DataSource = null;
        gvFindings.Rebind();
    }
    protected void BindVessel()
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
       // ucVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ucVessel.DataSource = dt;
        ucVessel.DataBind();
        //DataColumn[] keyColumns = new DataColumn[1];
        //keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        //dt.Tables[0].PrimaryKey = keyColumns;


        //if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        //{
        //    if (dt.Tables[0].Rows.Contains(ViewState["VESSELID"].ToString()))
        //    {
        //        ucVessel.SelectedValue = ViewState["VESSELID"].ToString();
        //    }
        //}

    }
}