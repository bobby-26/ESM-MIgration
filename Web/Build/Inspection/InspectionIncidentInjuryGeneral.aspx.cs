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
public partial class InspectionIncidentInjuryGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            ViewState["INJURYID"] = null;
            ViewState["INCIDENTVESSELID"] = null;
            ViewState["VESSELID"] = ""; 

            ucHealthSafetyCategory.HazardTypeList = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(int.Parse("1"), 0);
            BindHealthSafetySubCategory();
            
            if (Request.QueryString["INJURYID"] != null && Request.QueryString["INJURYID"].ToString() != "")
                ViewState["INJURYID"] = Request.QueryString["INJURYID"].ToString();
            if (Request.QueryString["MEDICALCASEID"] != null && Request.QueryString["MEDICALCASEID"].ToString() != "")
                ViewState["MEDICALCASEID"] = Request.QueryString["MEDICALCASEID"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
            else
                ViewState["VESSELID"] = "Dummy";
            BindInjured();
            BindVessel();
            BindWorkInjuryCategory();
            EditIncidentInjury();

            if (ViewState["MEDICALCASEID"] != null && !string.IsNullOrEmpty(ViewState["MEDICALCASEID"].ToString()))
            {
                lnkSicknessReport.Visible = true;
                lnkSicknessReport.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=SICKNESSREPORT&showexcel=no&pniid=" + ViewState["MEDICALCASEID"].ToString() + "');return true;");
            }
            else
                lnkSicknessReport.Visible = false;
        }
        imgShowCrewInCharge.Attributes.Add("onclick",
               "return showPickList('spnCrewInCharge', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&Date=" + General.GetNullableDateTime(ViewState["INCIDENTDATE"].ToString()) + "&framename=ifMoreInfo', true); ");

        imgPersonOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
        + PhoenixModule.QUALITY + "&framename=ifMoreInfo', true);");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuIncidentInjuryGeneral.AccessRights = this.ViewState;
        if (Filter.CurrentSelectedIncidentMenu == null)
            MenuIncidentInjuryGeneral.MenuList = toolbar.Show();
    }
    protected void SetPickList(string vslid)
    {
        if (vslid == "0")
        {
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnCrewInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
    }
    private void BindInjured()
    {
        ddlPartofTheBodyInjured.DataSource = PhoenixRegistersQuick.ListQuick(1, 68);
        ddlPartofTheBodyInjured.DataTextField = "FLDQUICKNAME";
        ddlPartofTheBodyInjured.DataValueField = "FLDQUICKCODE";
        ddlPartofTheBodyInjured.DataBind();
    }
    protected void BindHealthSafetySubCategory()
    {
        DataTable dt = PhoenixInspectionIncident.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHealthSafetyCategory.SelectedHazardType));
        ddlHealthSafetySubCategory.Items.Clear();
        ddlHealthSafetySubCategory.DataSource = dt;
        ddlHealthSafetySubCategory.DataTextField = "FLDNAME";
        ddlHealthSafetySubCategory.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSafetySubCategory.DataBind();
        ddlHealthSafetySubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlHealthSafetySubCategory.SelectedIndex = 0;
    }

    protected void ucHealthSafetyCategory_Changed(object sender, EventArgs e)
    {
        BindHealthSafetySubCategory();
    }

    protected void BindVessel()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["INCIDENTDATE"] = ds.Tables[0].Rows[0]["FLDDATEOFINCIDENT"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditIncidentInjury();
    }
    protected void MenuIncidentInjuryGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string PartofBodyInjured = GetCsvValue(ddlPartofTheBodyInjured);

                if (IsValidInspectionIncidentInjury(PartofBodyInjured))
                {
                    string personid = "";
                    if (ViewState["VESSELID"].ToString() == "0")
                        personid = txtPersonOfficeId.Text;
                    else
                        personid = txtCrewId.Text;

                    if (ViewState["INJURYID"] == null)
                    {
                        PhoenixInspectionIncidentInjury.InsertIncidentInjury(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , null
                            , new Guid(Filter.CurrentIncidentID)
                            , General.GetNullableInteger(personid)
                            , int.Parse(ucTypeOfInjury.SelectedQuick)
                            , General.GetNullableInteger(null)
                            , General.GetNullableString(PartofBodyInjured)
                            , null
                            , General.GetNullableDecimal(ucManHoursLost.Text)
                            , null
                            , General.GetNullableDecimal(ucExtimatedCost.Text)
                            , General.GetNullableInteger(chkThirdPartyInjury.Checked.Equals(true)? "1" : "0")
                            , General.GetNullableString(txtThirdPartyName.Text)
                            , General.GetNullableInteger(txtThirdPartyAge.Text)
                            , General.GetNullableString(txtDescription.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            , int.Parse(ucHealthSafetyCategory.SelectedHazardType)
                            , new Guid(ddlHealthSafetySubCategory.SelectedValue)
                            , General.GetNullableString(txtThirdPartyDesignation.Text)
                            , General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue)
                            );

                        ucStatus.Text = "Injury details are added.";
                        String script = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                        Filter.CurrentIncidentTab = "CONSEQUENCE";
                    }
                    else
                    {
                        PhoenixInspectionIncidentInjury.UpdateIncidentInjury(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["INJURYID"].ToString())
                            , new Guid(ViewState["INSPECTIONINCIDENTID"].ToString())
                            , General.GetNullableInteger(personid)
                            , int.Parse(ucTypeOfInjury.SelectedQuick)
                            , General.GetNullableInteger(null)
                            , General.GetNullableString(PartofBodyInjured)
                            , null
                            , General.GetNullableDecimal(ucManHoursLost.Text)
                            , null
                            , General.GetNullableDecimal(ucExtimatedCost.Text)
                            , General.GetNullableInteger(chkThirdPartyInjury.Checked.Equals(true) ? "1" : "0")
                            , General.GetNullableString(txtThirdPartyName.Text)
                            , General.GetNullableInteger(txtThirdPartyAge.Text)
                            , General.GetNullableString(txtDescription.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            , int.Parse(ucHealthSafetyCategory.SelectedHazardType)
                            , new Guid(ddlHealthSafetySubCategory.SelectedValue)
                            , General.GetNullableString(txtThirdPartyDesignation.Text)
                            , General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue)
                            );
                        ucStatus.Text = "Injury details are updated.";
                        String scriptu = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptu, true);
                        Filter.CurrentIncidentTab = "CONSEQUENCE";
                    }                    
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["INJURYID"] = null;
                Reset();
                EditIncidentInjury();                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void EditIncidentInjury()
    {
        DataSet ds1 = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = ds1.Tables[0].Rows[0];
            ViewState["VESSELID"] = dr1["FLDVESSELID"].ToString();
            Reset();
        }
        if (ViewState["INJURYID"] != null)
        {
            DataSet ds = PhoenixInspectionIncidentInjury.EditIncidentInjury(new Guid(ViewState["INJURYID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtCrewId.Text = dr["FLDINJUREDEMPLOYEEID"].ToString();
                txtAge.Text = dr["FLDAGE"].ToString();
                txtServiceYears.Text = dr["FLDSERVICEYEARS"].ToString();
                ucManHoursLost.Text = dr["FLDMANHOURSLOST"].ToString();
                SetCsvValue(ddlPartofTheBodyInjured, dr["FLDMULTIPARTOFTHEBODYINJURED"].ToString());
                ucTypeOfInjury.SelectedQuick = dr["FLDTYPEOFINJURY"].ToString();
                ViewState["INSPECTIONINCIDENTID"] = dr["FLDINSPECTIONINCIDENTID"].ToString();
                ViewState["INCIDENTVESSELID"] = dr["FLDVESSELID"].ToString();                

                if (ViewState["VESSELID"].ToString() != "0")
                {
                    txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                    txtCrewRank.Text = dr["FLDCREWRANK"].ToString();
                }
                else
                {
                    txtPersonOfficeId.Text = dr["FLDINJUREDEMPLOYEEID"].ToString();
                    txtOfficePersonName.Text = dr["FLDCREWNAME"].ToString();
                    txtOfficePersonDesignation.Text = dr["FLDCREWRANK"].ToString();
                }                

                lblDtkey.Text = dr["FLDDTKEY"].ToString();
                string typeofworkinjury = dr["FLDCATEGORYOFWORKINJURYSHORTNAME"].ToString();
                ucExtimatedCost.Text = dr["FLDESTIMATEDCOST"].ToString();
                txtServiceYearsAtSea.Text = dr["FLDSERVICEYEARSATSEA"].ToString();
                ucHealthSafetyCategory.Type = "1";
                ucHealthSafetyCategory.DataBind();
                ucHealthSafetyCategory.SelectedHazardType = dr["FLDHEALTHAFETYCATEGORY"].ToString();
                BindHealthSafetySubCategory();
                ddlHealthSafetySubCategory.SelectedValue = dr["FLDHEALTHSAFETYSUBCATEGORY"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                txtThirdPartyDesignation.Text = dr["FLDTHIRDPARTYDESIGNATION"].ToString();

                if (dr["FLDISTHIRDPARTYINJURY"].ToString() == "1")
                {
                    chkThirdPartyInjury.Checked = true;
                    txtCrewName.Text = "";
                    txtCrewRank.Text = "";
                    txtCrewId.Text = "";
                    spnCrewInCharge.Visible = false;
                    spnPersonInChargeOffice.Visible = false;
                    txtAge.Visible = false;

                    spnThirdParty.Visible = true;
                    txtThirdPartyAge.Visible = true;
                }
                else
                {
                    chkThirdPartyInjury.Checked = false;
                    txtThirdPartyName.Text = "";
                    txtThirdPartyDesignation.Text = "";
                    txtThirdPartyAge.Text = "";
                    spnThirdParty.Visible = false;
                    txtThirdPartyAge.Visible = false;
                    if (ViewState["VESSELID"].ToString() == "0")
                    {
                        spnCrewInCharge.Visible = false;
                        spnPersonInChargeOffice.Visible = true;
                    }
                    else
                    {
                        spnCrewInCharge.Visible = true;
                        spnPersonInChargeOffice.Visible = false;
                    }
                    txtAge.Visible = true;
                }
                txtThirdPartyName.Text = dr["FLDTHIRDPARTYNAME"].ToString();
                txtThirdPartyAge.Text = dr["FLDTHIRDPARTYAGE"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                if (dr["FLDWORKINJURYCATEGORYID"] != null && dr["FLDWORKINJURYCATEGORYID"].ToString() != string.Empty)
                    ddlWorkInjuryCategory.SelectedValue = dr["FLDWORKINJURYCATEGORYID"].ToString();
            }
        }
        else
        {
            Reset();
        }

    }
    private bool IsValidInspectionIncidentInjury(string PartofBodyInjured)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (chkThirdPartyInjury.Checked == false)
        {
            if (string.IsNullOrEmpty(txtCrewId.Text.Trim()) && string.IsNullOrEmpty(txtPersonOfficeId.Text.Trim()))
                ucError.ErrorMessage = "Injured's Name is Required.";
        }
        else if (chkThirdPartyInjury.Checked == true)
        {
            if (General.GetNullableString(txtThirdPartyName.Text) == null)
                ucError.ErrorMessage = " Injured's Name is required.";
            if (General.GetNullableString(txtThirdPartyDesignation.Text) == null)
                ucError.ErrorMessage = "Designation is required.";
            if (General.GetNullableInteger(txtThirdPartyAge.Text) == null)
                ucError.ErrorMessage = "Age is required.";
        }

        if (PartofBodyInjured == string.Empty)
            ucError.ErrorMessage = " Parts of Body Injured is required.";

        if (string.IsNullOrEmpty(ucTypeOfInjury.SelectedQuick) || ucTypeOfInjury.SelectedQuick.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = " Injury type is required.";

        if (General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category of work injury is required.";

        if (General.GetNullableInteger(ucHealthSafetyCategory.SelectedHazardType) == null)
            ucError.ErrorMessage = "Health and Safety Category is required.";

        if (General.GetNullableGuid(ddlHealthSafetySubCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Health and Safety Subcategory is required.";

        return (!ucError.IsError);
    }
    private void Reset()
    {
        txtCrewId.Text = txtCrewName.Text = txtAge.Text = txtServiceYears.Text = txtServiceYearsAtSea.Text = "";
        txtOfficePersonName.Text = txtOfficePersonDesignation.Text = txtPersonOfficeId.Text = txtAge.Text = txtServiceYears.Text = txtServiceYearsAtSea.Text = txtPersonOfficeEmail.Text = "";

        ddlPartofTheBodyInjured.ClearCheckedItems();
        ucTypeOfInjury.SelectedQuick = "";
        ucManHoursLost.Text = "";
        txtCrewRank.Text = "";
        ucExtimatedCost.Text = "";
        txtThirdPartyAge.Text = "";
        txtThirdPartyName.Text = "";
        txtDescription.Text = "";
        txtRemarks.Text = "";
        lnkSicknessReport.Visible = false;
        spnCrewInCharge.Visible = true;
        txtAge.Visible = true;
        spnThirdParty.Visible = false;
        txtThirdPartyAge.Visible = false;
        txtCategory.Text = "";
        ucHealthSafetyCategory.SelectedHazardType = "";
        ddlHealthSafetySubCategory.SelectedIndex = 0;
        txtThirdPartyDesignation.Text = "";
        chkThirdPartyInjury.Checked = false;
        ddlWorkInjuryCategory.SelectedIndex = 0;

        if (ViewState["VESSELID"].ToString() == "0")
        {
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnCrewInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;            
        }
    }
    protected void ThirdParty_Changed(object sender, EventArgs e)
    {
        if (chkThirdPartyInjury.Checked == true)
        {
            chkThirdPartyInjury.Checked = true;

            txtCrewName.Text = "";
            txtCrewRank.Text = "";
            txtCrewId.Text = "";
            txtServiceYears.Text = "";
            txtServiceYearsAtSea.Text = "";
            txtAge.Text = "";
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = false;
            txtAge.Visible = false;

            spnThirdParty.Visible = true;
            txtThirdPartyAge.Visible = true;
            txtThirdPartyName.Text = "";
            txtThirdPartyAge.Text = "";
            txtThirdPartyDesignation.Text = "";
        }
        else
        {
            txtThirdPartyName.Text = "";
            txtThirdPartyAge.Text = "";
            txtThirdPartyDesignation.Text = "";
            spnThirdParty.Visible = false;
            txtThirdPartyAge.Visible = false;

            if (ViewState["VESSELID"].ToString() == "0")
                spnPersonInChargeOffice.Visible = true;
            else
                spnCrewInCharge.Visible = true;

            txtAge.Visible = true;
            txtCrewName.Text = "";
            txtCrewRank.Text = "";
            txtCrewId.Text = "";
            txtServiceYears.Text = "";
            txtServiceYearsAtSea.Text = "";
            txtAge.Text = "";
        }
    }

    protected void BindWorkInjuryCategory()
    {
        ddlWorkInjuryCategory.DataSource = PhoenixInspectionRiskAssessmentWorkInjuryCategory.ListRiskAssessmentWorkInjuryCategory();
        ddlWorkInjuryCategory.DataTextField = "FLDNAME";
        ddlWorkInjuryCategory.DataValueField = "FLDWORKINJURYCATEGORYID";
        ddlWorkInjuryCategory.DataBind();
        ddlWorkInjuryCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }
}
