using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersVesselHistoryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuVesselHistoryList.AccessRights = this.ViewState;
            MenuVesselHistoryList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
                ucManager.AddressType = ((int)PhoenixAddressType.MANAGER).ToString();
                ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();
                ucOfficerWageScale.AddressType = ((int)PhoenixAddressType.UNION).ToString();
                ucRatingsWageScale.AddressType = ((int)PhoenixAddressType.UNION).ToString();           
                HideAll();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void VesselHistoryList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            //String scriptClosePopup = String.Format("javascript:openNewWindow('codehelp1', 'ifMoreInfo');");
            String scriptKeepPopupOpen = String.Format("javascript:parent.fnReloadList('codehelp1', null, 'yes');");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidHistory())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVessel.InsertVesselHistory(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(Filter.CurrentVesselMasterFilter)
                    , txtVesselName.Text
                    , General.GetNullableInteger(ucFlag.SelectedFlag)
                    , General.GetNullableInteger(ucManager.SelectedAddress)
                    , General.GetNullableInteger(ucOwner.SelectedAddress)
                    , General.GetNullableInteger(ucPrincipal.SelectedAddress)
                    , txtVesselCode.Text
                    , General.GetNullableInteger(ucOfficerWageScale.SelectedAddress)
                    , General.GetNullableInteger(ucRatingsWageScale.SelectedAddress)
                    , General.GetNullableInteger(ucSeniorityWageScale.SelectedSeniorityScale)
                    , General.GetNullableInteger(drpActiveYN.SelectedValue)
                    , DateTime.Parse(txtWEF.Text)
                    , int.Parse(ucPortRegistered.SelectedValue)
                    , int.Parse(ucHistoryModification.SelectedHard)
                    , txtRemarks.Text
                    , General.GetNullableDateTime(txtESMHandOverDate.Text)
                    , General.GetNullableInteger(ucESMStdWage.SelectedHard)
                    , General.GetNullableInteger(ucManagementType.SelectedHard));

                if (ucHistoryModification.SelectedHard == "492") // owner
                {
                    SendMailForOwnerChange();
                }
                ucStatus.Text = "History saved successfully.";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptKeepPopupOpen, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SendMailForOwnerChange()
    {
        string emailid = "";
        try
        {
            string selectedvendors = ",";

            if (selectedvendors.Length <= 1)
                selectedvendors = null;

            DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(
                int.Parse(Filter.CurrentVesselMasterFilter));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string emailbodytext = "";

                if (!dr["FLDPERSONALOFFICEREMAIL"].ToString().Equals(""))
                {
                    emailid = dr["FLDPERSONALOFFICEREMAIL"].ToString().Replace(";", ",");

                    if (!dr["FLDFPSEMAIL"].ToString().Equals(""))
                    {
                        emailid = emailid + "," + dr["FLDFPSEMAIL"].ToString().Replace(";", ",");
                    }
                }
                else if (!dr["FLDFPSEMAIL"].ToString().Equals(""))
                {
                    emailid = dr["FLDFPSEMAIL"].ToString().Replace(";", ",");
                }
                else
                {
                    ucError.ErrorMessage = "Personal officer and Fleet Personal Supdt. mail ids are not configured for this vessel.";
                    ucError.Visible = true;
                    return;
                }

                try
                {
                    emailbodytext = PrepareEmailBodyText(dr["FLDVESSELNAME"].ToString());

                    PhoenixCommoneProcessing.PrepareEmailMessage(
                        emailid, "OWNERCHANGE", new Guid(dr["FLDDTKEY"].ToString()),
                        "", "",
                        "", "Owner changed for the vessel " + dr["FLDVESSELNAME"].ToString(), emailbodytext, "", "");

                    ucStatus.Text = "Email sent to 'Personal Officer' and 'Fleet Personnel Superintendent' regarding the owner change.";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected string PrepareEmailBodyText(string vesselname)
    {
        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.AppendLine();
        sbemailbody.Append("Good day,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Please note, the registered owner of the vessel " + vesselname + " has been changed.");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("In view of the above, kindly change the contracts of the seafarers onboard to reflect the new registered Owner");
        sbemailbody.AppendLine();
        //sbemailbody.AppendLine();
        //sbemailbody.AppendLine("Thank you,");
        //sbemailbody.AppendLine();
        //sbemailbody.Append("username");

        return sbemailbody.ToString();
    }

    private void SelectWageScale()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Filter.CurrentVesselMasterFilter));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ucOfficerWageScale.SelectedAddress = ds.Tables[0].Rows[0]["FLDWAGESCALE"].ToString();
            ucRatingsWageScale.SelectedAddress = ds.Tables[0].Rows[0]["FLDWAGESCALERATING"].ToString();
            ucSeniorityWageScale.SelectedSeniorityScale = ds.Tables[0].Rows[0]["FLDSENIORITYSCALE"].ToString();
            ucESMStdWage.SelectedHard = ds.Tables[0].Rows[0]["FLDSTANDARDWAGECODE"].ToString();
        }
    }

    protected void drpActiveYN_Changed(object sender, EventArgs e)
    {
        if (drpActiveYN.SelectedValue == "0")
        {
            lblESMHandOverDate.Visible = true;
            txtESMHandOverDate.Visible = true;

            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Filter.CurrentVesselMasterFilter));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDESMHANDOVERDATE"].ToString()) != null)
                {
                    txtESMHandOverDate.Text = ds.Tables[0].Rows[0]["FLDESMHANDOVERDATE"].ToString();
                    txtESMHandOverDate.Enabled = false;
                }
            }
        }
        else
        {
            txtESMHandOverDate.Text = "";
            txtESMHandOverDate.Visible = false;
            lblESMHandOverDate.Visible = false;
        }
    }

    private void Reset()
    {
        txtVesselName.Text = "";
        txtVesselCode.Text = "";
        txtESMHandOverDate.Text = "";
        //txtRemarks.Text = "";
        //txtWEF.Text = "";
        ucOwner.SelectedAddress = "";
        ucManager.SelectedAddress = "";
        //ucPortRegistered.SelectedSeaport = "";
        ucPrincipal.SelectedAddress = "";
        ucFlag.SelectedFlag = ""; 
        drpActiveYN.SelectedValue = "";
        ucSeniorityWageScale.SelectedSeniorityScale = "";
        ucRatingsWageScale.SelectedAddress = "";
        ucOfficerWageScale.SelectedAddress = "";
        ucManagementType.SelectedHard = "";
        ucESMStdWage.SelectedHard = "";
    }

    private void HideAll()
    {
        txtVesselCode.Visible = false;
        txtVesselName.Visible = false;
        txtESMHandOverDate.Visible = false;
        lblESMHandOverDate.Visible = false;
        ucFlag.Visible = false;
        ucOwner.Visible = false;
        ucManager.Visible = false;
        ucManagementType.Visible = false;
        ucPrincipal.Visible = false;
        tblWageScale.Visible = false; 
        drpActiveYN.Visible = false;
        lblValue.Visible = false;
    }

    protected void History_Changed(object sender, EventArgs e)
    {
        try
        {            
            Reset();
            HideAll();

            lblValue.Visible = true;

            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "VNM"))
                txtVesselName.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "MNG"))
                ucManager.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "MNT"))
                ucManagementType.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "OWN"))
                ucOwner.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "PRN"))
                ucPrincipal.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "VCD"))
                txtVesselCode.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "FLG"))
                ucFlag.Visible = true;
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "WSL"))
            {
                tblWageScale.Visible = true;
                SelectWageScale();
            }
            if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "ACT"))
                drpActiveYN.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHistory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtRemarks.Text.Trim() == "")
            ucError.ErrorMessage = "Remarks is required.";

        if (General.GetNullableInteger(ucPortRegistered.SelectedValue) == null)
            ucError.ErrorMessage = "Port of Change is required.";

        if (General.GetNullableDateTime(txtWEF.Text) == null)
            ucError.ErrorMessage = "W.E.F is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "VNM"))
            if (txtVesselName.Text.Trim() == "")
                ucError.ErrorMessage = "Name is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "MNG"))
            if (General.GetNullableInteger(ucManager.SelectedAddress) == null)
                ucError.ErrorMessage = "Manager is required";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "MNT"))
            if (General.GetNullableInteger(ucManagementType.SelectedHard) == null)
                ucError.ErrorMessage = "Management Type is required";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "OWN"))
            if (General.GetNullableInteger(ucOwner.SelectedAddress) == null)
                ucError.ErrorMessage = "Owner is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "PRN"))
            if (General.GetNullableInteger(ucPrincipal.SelectedAddress) == null)
                ucError.ErrorMessage = "Principal is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "VCD"))
            if (txtVesselCode.Text.Trim() == "")
                ucError.ErrorMessage = "Short Code is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "FLG"))
            if (General.GetNullableInteger(ucFlag.SelectedFlag) == null)
                ucError.ErrorMessage = "Flag is required.";

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "WSL"))
        {
            if (General.GetNullableInteger(ucOfficerWageScale.SelectedAddress) == null)
                ucError.ErrorMessage = "Officer Wage Scale is required.";

            if (General.GetNullableInteger(ucRatingsWageScale.SelectedAddress) == null)
                ucError.ErrorMessage = "Ratings Wage Scale is required.";
        }

        if (ucHistoryModification.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 110, "ACT"))
            if (General.GetNullableInteger(drpActiveYN.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Active/In-Active is required.";
            }
            else
            {
                if (drpActiveYN.SelectedValue == "0" && General.GetNullableDateTime(txtESMHandOverDate.Text) == null)
                    ucError.ErrorMessage = "Hand Over Date is required.";
            }

        return (!ucError.IsError);
    }
}
