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

public partial class InspectionIncidentInjuryMedicalCasaeGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgShowCrewInCharge.Attributes.Add("onclick",
                "return showPickList('spnCrewInCharge', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                + ViewState["VESSELID"] + "', true); ");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PNIID"] = null;
            txtCrewId.Attributes.Add("style", "visibility:hidden");
            txtPersonalOfficer.Attributes.Add("style", "visibility:hidden");
            txtPOEmail.Attributes.Add("style", "visibility:hidden");
            txtPic.Attributes.Add("style", "visibility:hidden");
            txtPICEmailHidden.Attributes.Add("style", "visibility:hidden");
            imgShowCrewInCharge.Visible = false;
            imgShowPO.Visible = false;
            imgShowPic.Visible = false;
            ucTypeofcase.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 174, "INJ");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Details", "INJURY");
            toolbar.AddButton("Doctor's Visit", "DOCTOR");
            toolbar.AddButton("Medical Case", "MEDICALCASE");
            MenuIncidentInjurySearch.MenuList = toolbar.Show();
            MenuIncidentInjurySearch.SelectedMenuIndex = 2;

            if (Request.QueryString["INJURYID"] != null && Request.QueryString["INJURYID"].ToString() != "")
                ViewState["INJURYID"] = Request.QueryString["INJURYID"].ToString();
            if (Request.QueryString["MEDICALCASEID"] != null && Request.QueryString["MEDICALCASEID"].ToString() != "")
                ViewState["MEDICALCASEID"] = Request.QueryString["MEDICALCASEID"].ToString();

            EditOperation();
        }
    }

    protected void MenuIncidentInjurySearch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("INJURY"))
        {           
            Response.Redirect("../Inspection/InspectionIncidentInjuryGeneral.aspx?INJURYID=" + ViewState["INJURYID"], true);            
        }
        else if (dce.CommandName.ToUpper().Equals("DOCTOR"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDoctorVisit.aspx?INJURYID=" + ViewState["INJURYID"] + "&MEDICALCASEID=" + ViewState["MEDICALCASEID"], true);            
        }
    }

    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;       
    }
    private void EditOperation()
    {
        if (ViewState["MEDICALCASEID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["MEDICALCASEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                //ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                //ucVessel.Enabled = false;
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtcaseNo.Text = dr["FLDCASENUMBER"].ToString();
                ucInjuryDate.Text = dr["FLDILLNESSINJURYDATE"].ToString();
                txtCrewId.Text = dr["FLDEMPLOYEEID"].ToString();
                txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                txtCrewRank.Text = dr["FLDRANKNAME"].ToString();
                txtDescription.Text = dr["FLDILLNESSINJURYDESCRIPTION"].ToString();
                chkHospital.Checked = dr["FLDHOSPITALIZEDYN"].ToString() == "1" ? true : false;
                txtPersonalOfficer.Text = dr["FLDSPAFORVESSELPO"].ToString();
                txtponame.Text = dr["FLDPONAME"].ToString();
                txtpoDesignation.Text = dr["FLDPODESIGNATIONNAME"].ToString();
                txtPic.Text = dr["FLDSIGAPOREPIC"].ToString();
                txtSigaporeName.Text = dr["FLDPICNAME"].ToString();
                txtSigaporeDesignation.Text = dr["FLDPICDESIGNATIONNAME"].ToString();
                ucTypeofcase.SelectedHard = dr["FLDTYPEOFCASE"].ToString();
                txtDaysLost.Text = dr["FLDDAYSLOST"].ToString();
                txtServiceYears.Text = dr["FLDSERVICEYEARS"].ToString();
                ucPartsinjured.SelectedQuick = dr["FLDINJUREDPART"].ToString();
                ucInjuryType.SelectedQuick = dr["FLDINJURYTYPE"].ToString();
                ucInjuryCategory.SelectedQuick = dr["FLDWORKINJURYCATEGORY"].ToString();
            }
        }
        else
        {
            Reset();
        }

    }
    private bool IsValidInspectionPNIOperation()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (string.IsNullOrEmpty(txtcaseNo.Text.Trim()))
            ucError.ErrorMessage = "Case No is Required.";

        if (General.GetNullableDateTime(ucInjuryDate.Text) == null)
            ucError.ErrorMessage = "Injury Date is required.";
        else if (DateTime.TryParse(ucInjuryDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "Injury Date Should not be a Future Date";        

        if (General.GetNullableInteger(txtCrewId.Text) == null)
            ucError.ErrorMessage = "Crew Name is required.";

        if (string.IsNullOrEmpty(ucTypeofcase.SelectedHard) || ucTypeofcase.SelectedHard.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Type of Case is required.";

        if (ucTypeofcase.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 174, "INJ"))
        {
            if (string.IsNullOrEmpty(ucPartsinjured.SelectedQuick) || ucPartsinjured.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = " Parts of Body Injured is required.";

            if (string.IsNullOrEmpty(ucInjuryType.SelectedQuick) || ucInjuryType.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = "Injury Type is required.";

            if (string.IsNullOrEmpty(ucInjuryCategory.SelectedQuick) || ucInjuryCategory.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = "Injury Category is required.";

        }

        return (!ucError.IsError);
    }
    private void Reset()
    {       
        txtcaseNo.Text = "";
        ucInjuryDate.Text = DateTime.Now.ToLongDateString();
        txtCrewId.Text = "";
        txtCrewName.Text = "";
        txtDescription.Text = "";
        chkHospital.Checked = false;
        txtPersonalOfficer.Text = "";
        txtponame.Text = "";
        txtpoDesignation.Text = "";
        txtPic.Text = "";
        txtSigaporeName.Text = "";
        txtSigaporeDesignation.Text = "";
        ucTypeofcase.SelectedHard = "";
        txtDaysLost.Text = "";
        txtServiceYears.Text = "";
        ucPartsinjured.SelectedQuick = "";
        ucInjuryType.SelectedQuick = "";
        ucInjuryCategory.SelectedQuick = "";
    }
    protected void ucTypeofcase_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucCaseType = (UserControlHard)sender;

            if (ucCaseType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 174, "INJ"))
            {
                ucInjuryType.Enabled = true;
                ucInjuryCategory.Enabled = true;
                ucPartsinjured.Enabled = true;
            }
            else
            {
                ucInjuryType.Enabled = false;
                ucInjuryCategory.Enabled = false;
                ucPartsinjured.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
