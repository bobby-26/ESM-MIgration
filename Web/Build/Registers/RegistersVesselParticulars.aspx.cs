using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselParticulars : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

            toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
            toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
            toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
            toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
            //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
            toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
            toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);


            MenuVesselList.AccessRights = this.ViewState;
            MenuVesselList.MenuList = toolbar.Show();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                MenuVesselList.SelectedMenuIndex = 5;
            else
                MenuVesselList.SelectedMenuIndex = 4;

            ucAddrPandIClub.AddressType = ((int)PhoenixAddressType.PANDICLUB).ToString();
            BindCrewingAgency();
            EditVesselParticulars(Int16.Parse(Filter.CurrentVesselMasterFilter));
        }

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if (ViewState["DTKEY"] != null && ViewState["DTKEY"].ToString() != string.Empty)
            toolbar1.AddLinkButton("javascript:openNewWindow('codehelp1','','"+Session["sitepath"] +"/Common/CommonPhoenixAuditChanges.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&shortcode=VSL-ADMIN" + "')", "Admin History", "HISTORY",ToolBarDirection.Right); ;
        MenuVesselParticulars.AccessRights = this.ViewState;
        MenuVesselParticulars.MenuList = toolbar1.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
       
    }

    protected void EditVesselParticulars(int vesselId)
    {
        DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(vesselId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();

            ucOfficerPool.SelectedPool = dr["FLDOFFICERPOOL"].ToString();
            ucRatingPool.SelectedPool = dr["FLDRATINGPOOL"].ToString();
            ucAddrPandIClub.SelectedAddress = dr["FLDPANDICLUB"].ToString();
            //Tech.Superindent
            RadMcUserSup.SelectedValue = dr["FLDSUPT"].ToString();
            RadMcUserSup.Text = dr["FLDSUPTNAME"].ToString() + (dr["FLDSUPTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDSUPTDESIGNATION"].ToString() : "") + (dr["FLDSUPTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDSUPTEMAIL"].ToString() : "");
            RadMcUserSup.SelectedEmail = dr["FLDSUPTEMAIL"].ToString();
            RadMcUserSup.ToolTip= dr["FLDSUPTNAME"].ToString() + (dr["FLDSUPTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDSUPTDESIGNATION"].ToString() : "") + (dr["FLDSUPTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDSUPTEMAIL"].ToString() : "");
            //Marine.Superindent
            MCMarineSupt.SelectedValue = dr["FLDMARINESUPT"].ToString();
            MCMarineSupt.Text = dr["FLDMARINESUPTNAME"].ToString() + (dr["FLDMARINESUPTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDMARINESUPTDESIGNATION"].ToString() : "") + (dr["FLDMARINESUPTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDMARINESUPTEMAIL"].ToString() : "");
            MCMarineSupt.SelectedEmail = dr["FLDMARINESUPTEMAIL"].ToString();
            MCMarineSupt.ToolTip = dr["FLDMARINESUPTNAME"].ToString() + (dr["FLDMARINESUPTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDMARINESUPTDESIGNATION"].ToString() : "") + (dr["FLDMARINESUPTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDMARINESUPTEMAIL"].ToString() : "");
            //Fleet Manager
            RadMcUserFM.SelectedValue = dr["FLDFLEETMANAGER"].ToString();
            RadMcUserFM.Text = dr["FLDFLEETMANAGERNAME"].ToString() + (dr["FLDFLEETMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFLEETMANAGERDESIGNATION"].ToString() : "") + (dr["FLDFLEETMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDFLEETMANAGEREMAIL"].ToString() : "");
            RadMcUserFM.SelectedEmail = dr["FLDFLEETMANAGEREMAIL"].ToString();
            RadMcUserFM.ToolTip = dr["FLDFLEETMANAGERNAME"].ToString() + (dr["FLDFLEETMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFLEETMANAGERDESIGNATION"].ToString() : "") + (dr["FLDFLEETMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDFLEETMANAGEREMAIL"].ToString() : "");      
            //Tech. Director
            RadMcUserTD.SelectedValue = dr["FLDTECHDIRECTOR"].ToString();
            RadMcUserTD.Text = dr["FLDTECHDIRECTORNAME"].ToString() + (dr["FLDTECHDIRECTORDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTECHDIRECTORDESIGNATION"].ToString() : "") + (dr["FLDTECHDIRECTOREMAIL"].ToString() != string.Empty ? " / " + dr["FLDTECHDIRECTOREMAIL"].ToString() : "");
            RadMcUserTD.SelectedEmail = dr["FLDTECHDIRECTOREMAIL"].ToString();
            RadMcUserTD.ToolTip = dr["FLDTECHDIRECTORNAME"].ToString() + (dr["FLDTECHDIRECTORDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTECHDIRECTORDESIGNATION"].ToString() : "") + (dr["FLDTECHDIRECTOREMAIL"].ToString() != string.Empty ? " / " + dr["FLDTECHDIRECTOREMAIL"].ToString() : "");
            //Personnel Officer
            RadMcUserPO.SelectedValue = dr["FLDPERSONALOFFICER"].ToString();
            RadMcUserPO.Text = dr["FLDPONAME"].ToString() + (dr["FLDPERSONALOFFICERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDPERSONALOFFICERDESIGNATION"].ToString() : "") + (dr["FLDPERSONALOFFICEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDPERSONALOFFICEREMAIL"].ToString() : "");
            RadMcUserPO.SelectedEmail = dr["FLDPERSONALOFFICEREMAIL"].ToString();
            RadMcUserPO.ToolTip = dr["FLDPONAME"].ToString() + (dr["FLDPERSONALOFFICERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDPERSONALOFFICERDESIGNATION"].ToString() : "") + (dr["FLDPERSONALOFFICEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDPERSONALOFFICEREMAIL"].ToString() : "");
            //Fleet Personnel Manager
            RadMcUserFPM.SelectedValue = dr["FLDFPM"].ToString();
            RadMcUserFPM.Text = dr["FLDFPMNAME"].ToString() + (dr["FLDFPMDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFPMDESIGNATION"].ToString() : "") + (dr["FLDFPMEMAIL"].ToString() != string.Empty ? " / " + dr["FLDFPMEMAIL"].ToString() : "");
            RadMcUserFPM.SelectedEmail = dr["FLDFPMEMAIL"].ToString();
            RadMcUserFPM.ToolTip = dr["FLDFPMNAME"].ToString() + (dr["FLDFPMDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFPMDESIGNATION"].ToString() : "") + (dr["FLDFPMEMAIL"].ToString() != string.Empty ? " / " + dr["FLDFPMEMAIL"].ToString() : "");
            //Fleet Personnel Superintendent
            RadMcUserFPS.SelectedValue = dr["FLDFPS"].ToString();
            RadMcUserFPS.Text = dr["FLDFPSNAME"].ToString() + (dr["FLDFPSDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFPSDESIGNATION"].ToString() : "") + (dr["FLDFPSEMAIL"].ToString() != string.Empty ? " / " + dr["FLDFPSEMAIL"].ToString() : "");
            RadMcUserFPS.SelectedEmail = dr["FLDFPSEMAIL"].ToString();
            RadMcUserFPS.ToolTip = dr["FLDFPSNAME"].ToString() + (dr["FLDFPSDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDFPSDESIGNATION"].ToString() : "") + (dr["FLDFPSEMAIL"].ToString() != string.Empty ? " / " + dr["FLDFPSEMAIL"].ToString() : "");
            //Vessel PIC (Crew, HO)
            RadMcUserVP.SelectedValue = dr["FLDCREWMANAGER"].ToString();
            RadMcUserVP.Text = dr["FLDCREWMANAGERNAME"].ToString() + (dr["FLDCREWMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDCREWMANAGERDESIGNATION"].ToString() : "") + (dr["FLDCREWMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDCREWMANAGEREMAIL"].ToString() : "");
            RadMcUserVP.SelectedEmail = dr["FLDCREWMANAGEREMAIL"].ToString();
            RadMcUserVP.ToolTip = dr["FLDCREWMANAGERNAME"].ToString() + (dr["FLDCREWMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDCREWMANAGERDESIGNATION"].ToString() : "") + (dr["FLDCREWMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDCREWMANAGEREMAIL"].ToString() : "");
            //Travel PIC
            RadMcUserTP.SelectedValue = dr["FLDTRAVELPIC"].ToString();
            RadMcUserTP.Text = dr["FLDTRAVELPICNAME"].ToString() + (dr["FLDTRAVELPICDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPICDESIGNATION"].ToString() : "") + (dr["FLDTRAVELPICEMAIL"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPICEMAIL"].ToString() : "");
            RadMcUserTP.SelectedEmail = dr["FLDTRAVELPICEMAIL"].ToString();
            RadMcUserTP.ToolTip = dr["FLDTRAVELPICNAME"].ToString() + (dr["FLDTRAVELPICDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPICDESIGNATION"].ToString() : "") + (dr["FLDTRAVELPICEMAIL"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPICEMAIL"].ToString() : "");
            //Account Administrator
            RadMcUserAA.SelectedValue = dr["FLDACCOUNTADMIN"].ToString();
            RadMcUserAA.Text = dr["FLDACCOUNTADMINNAME"].ToString() + (dr["FLDACCOUNTADMINDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTADMINDESIGNATION"].ToString() : "") + (dr["FLDACCOUNTADMINEMAIL"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTADMINEMAIL"].ToString() : "");
            RadMcUserAA.SelectedEmail = dr["FLDACCOUNTADMINEMAIL"].ToString();
            RadMcUserAA.ToolTip = dr["FLDACCOUNTADMINNAME"].ToString() + (dr["FLDACCOUNTADMINDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTADMINDESIGNATION"].ToString() : "") + (dr["FLDACCOUNTADMINEMAIL"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTADMINEMAIL"].ToString() : "");
            //Purchaser
            RadMcUserP.SelectedValue = dr["FLDSUPPLIER"].ToString();
            RadMcUserP.Text = dr["FLDSUPPLIERNAME"].ToString() + (dr["FLDSUPPLIERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDSUPPLIERDESIGNATION"].ToString() : "") + (dr["FLDSUPPLIEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDSUPPLIEREMAIL"].ToString() : "");
            RadMcUserP.SelectedEmail = dr["FLDSUPPLIEREMAIL"].ToString();
            RadMcUserP.ToolTip= dr["FLDSUPPLIERNAME"].ToString() + (dr["FLDSUPPLIERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDSUPPLIERDESIGNATION"].ToString() : "") + (dr["FLDSUPPLIEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDSUPPLIEREMAIL"].ToString() : "");
            //Quality PIC
            RadMcUserQP.SelectedValue = dr["FLDQUALITYINCHARGE"].ToString();
            RadMcUserQP.Text = dr["FLDQUALITYINCHARGENAME"].ToString() + (dr["FLDQUALITYINCHARGEDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQUALITYINCHARGEDESIGNATION"].ToString() : "") + (dr["FLDQUALITYINCHARGEEMAIL"].ToString() != string.Empty ? " / " + dr["FLDQUALITYINCHARGEEMAIL"].ToString() : "");
            RadMcUserQP.SelectedEmail = dr["FLDQUALITYINCHARGEEMAIL"].ToString();
            RadMcUserQP.ToolTip = dr["FLDQUALITYINCHARGENAME"].ToString() + (dr["FLDQUALITYINCHARGEDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQUALITYINCHARGEDESIGNATION"].ToString() : "") + (dr["FLDQUALITYINCHARGEEMAIL"].ToString() != string.Empty ? " / " + dr["FLDQUALITYINCHARGEEMAIL"].ToString() : "");
            //QA Manager
            RadMcUserQA.SelectedValue = dr["FLDQAMANAGER"].ToString(); ;
            RadMcUserQA.Text = dr["FLDQAMANAGERNAME"].ToString() + (dr["FLDQAMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQAMANAGERDESIGNATION"].ToString() : "") + (dr["FLDQAMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDQAMANAGEREMAIL"].ToString() : "");
            RadMcUserQA.SelectedEmail = dr["FLDQAMANAGEREMAIL"].ToString();
            RadMcUserQA.ToolTip= dr["FLDQAMANAGERNAME"].ToString() + (dr["FLDQAMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQAMANAGERDESIGNATION"].ToString() : "") + (dr["FLDQAMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDQAMANAGEREMAIL"].ToString() : "");
            //DPA
            RadMcUserDPA.SelectedValue = dr["FLDDPA"].ToString();
            RadMcUserDPA.Text = dr["FLDDPANAME"].ToString() + (dr["FLDDPADESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDDPADESIGNATION"].ToString() : "") + (dr["FLDDPAEMAIL"].ToString() != string.Empty ? " / " + dr["FLDDPAEMAIL"].ToString() : "");
            RadMcUserDPA.SelectedEmail = dr["FLDDPAEMAIL"].ToString();
            RadMcUserDPA.ToolTip = dr["FLDDPANAME"].ToString() + (dr["FLDDPADESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDDPADESIGNATION"].ToString() : "") + (dr["FLDDPAEMAIL"].ToString() != string.Empty ? " / " + dr["FLDDPAEMAIL"].ToString() : "");
            
            //txtDPAName.Text = dr["FLDDPANAME"].ToString();
            //txtDPADesignation.Text = dr["FLDDPADESIGNATION"].ToString();
            ////txtDPAEmail.Text = dr["FLDDPAEMAIL"].ToString();
            //txtDPA.Text = dr["FLDDPA"].ToString();

            //txtFleetManager.Text = dr["FLDFLEETMANAGER"].ToString();
            //txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            //txtSuptEmail.Text = dr["FLDSUPTEMAIL"].ToString();
            //txtSupt.Text = dr["FLDSUPT"].ToString();
            //txtFPMEmail.Text = dr["FLDFPMEMAIL"].ToString();
            //txtFPM.Text = dr["FLDFPM"].ToString();
            //txtPOEmail.Text = dr["FLDPERSONALOFFICEREMAIL"].ToString();
            //txtPersonalOfficer.Text = dr["FLDPERSONALOFFICER"].ToString();
            //txtFPSEmail.Text = dr["FLDFPSEMAIL"].ToString();
            //txtFPS.Text = dr["FLDFPS"].ToString();
            //txtTravelPICEmail.Text = dr["FLDTRAVELPICEMAIL"].ToString();
            //txtTravelPIC.Text = dr["FLDTRAVELPIC"].ToString();
            //txtAccountAdminEmail.Text = dr["FLDACCOUNTADMINEMAIL"].ToString();
            //txtAccountAdmin.Text = dr["FLDACCOUNTADMIN"].ToString();
            //txtTechDirectorEmail.Text = dr["FLDTECHDIRECTOREMAIL"].ToString();
            //txtTechDirector.Text = dr["FLDTECHDIRECTOR"].ToString();
            //txtSuptName.Text = dr["FLDSUPTNAME"].ToString();
            //txtSuptDesignation.Text = dr["FLDSUPTDESIGNATION"].ToString();
            //txtFleetManagerName.Text = dr["FLDFLEETMANAGERNAME"].ToString();
            //txtFleetManagerDesignation.Text = dr["FLDFLEETMANAGERDESIGNATION"].ToString();
            //txtFPMName.Text = dr["FLDFPMNAME"].ToString();
            //txtFPMDesignation.Text = dr["FLDFPMDESIGNATION"].ToString();
            //txtPOName.Text = dr["FLDPONAME"].ToString();
            //txtPODesignation.Text = dr["FLDPERSONALOFFICERDESIGNATION"].ToString();
            //txtFPSName.Text = dr["FLDFPSNAME"].ToString();
            //txtFPSDesignation.Text = dr["FLDFPSDESIGNATION"].ToString();
            //txtTravelPICName.Text = dr["FLDTRAVELPICNAME"].ToString();
            //txtTravelPICDesignation.Text = dr["FLDTRAVELPICDESIGNATION"].ToString();
            //txtTechDirectorName.Text = dr["FLDTECHDIRECTORNAME"].ToString();
            //txtTechDirectorDesignation.Text = dr["FLDTECHDIRECTORDESIGNATION"].ToString();
            //txtAccountAdminName.Text = dr["FLDACCOUNTADMINNAME"].ToString();
            //txtAccountAdminDesignation.Text = dr["FLDACCOUNTADMINDESIGNATION"].ToString();

            //txtPurchaserName.Text = dr["FLDSUPPLIERNAME"].ToString();
            //txtPurchaserDesignation.Text = dr["FLDSUPPLIERDESIGNATION"].ToString();
            ////txtPurchaserEmail.Text = dr["FLDSUPPLIEREMAIL"].ToString();
            //txtPurchaser.Text = dr["FLDSUPPLIER"].ToString();

            //txtQualityPICName.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
            //txtQualityPICDesignation.Text = dr["FLDQUALITYINCHARGEDESIGNATION"].ToString();
            ////txtQualityPICEmail.Text = dr["FLDQUALITYINCHARGEEMAIL"].ToString();
            //txtQualityPIC.Text = dr["FLDQUALITYINCHARGE"].ToString();

            //txtCrewManagerName.Text = dr["FLDCREWMANAGERNAME"].ToString();
            //txtCrewManagerDesignation.Text = dr["FLDCREWMANAGERDESIGNATION"].ToString();
            ////txtCrewManagerEmail.Text = dr["FLDCREWMANAGEREMAIL"].ToString();
            //txtCrewManager.Text = dr["FLDCREWMANAGER"].ToString();

            //txtQAManagerName.Text = dr["FLDQAMANAGERNAME"].ToString();
            //txtQAManagerDesignation.Text = dr["FLDQAMANAGERDESIGNATION"].ToString();
            ////txtQAManagerEmail.Text = dr["FLDQAMANAGEREMAIL"].ToString();
            //txtQAManager.Text = dr["FLDQAMANAGER"].ToString();

            txtESMStdWage.Text = dr["FLDSTANDARDWAGECODENAME"].ToString();
            txtOfficerWageScale.Text = dr["FLDWAGESCALENAME"].ToString() + " / " + dr["FLDOFFICEREXPIRYDATE"].ToString();
            txtRatingsWageScale.Text = dr["FLDWAGESCALERATINGNAME"].ToString() + " / " + dr["FLDRATINGEXPIRYDATE"].ToString();
            txtSeniorityWageScale.Text = dr["FLDSENIORITYSCALENAME"].ToString();

            SelectCrewingAgency(dr["FLDCREWINGAGENCY"].ToString());

            ucFleet.SelectedFleet = dr["FLDFLEET"].ToString();
            ucMedicals.SelectedHard = dr["FLDMEDICALREQUIRED"].ToString();
            txtDeductible.Text = dr["FLDDEDUCTIBLES"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDINSURANCECURRENCY"].ToString();
            ucTechFleet.SelectedFleet = dr["FLDTECHFLEET"].ToString();
            ucAcctFleet.SelectedFleet = dr["FLDACCOUNTFLEET"].ToString();

            if (dr["FLDISOFFICERATTACHMENT"].ToString() == "0")
                imgOfficerClip.ImageUrl = Session["images"] + "/no-attachment.png";
            if (dr["FLDISRATINGATTACHMENT"].ToString() == "0")
                imgRatingClip.ImageUrl = Session["images"] + "/no-attachment.png";

            if (dr["FLDOFFICERATTACHMENT"].ToString() != string.Empty)
                imgOfficerClip.Attributes["onclick"] = "javascript:parent.openNewWindow('UPLOAD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDOFFICERATTACHMENT"].ToString() + "&mod="
                    + PhoenixModule.REGISTERS + "&u=n'); return false;";
            else
                imgOfficerClip.Visible = false;

            if (dr["FLDRATINGATTACHMENT"].ToString() != string.Empty)
                imgRatingClip.Attributes["onclick"] = "javascript:parent.openNewWindow('UPLOAD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDRATINGATTACHMENT"].ToString() + "&mod="
                    + PhoenixModule.REGISTERS + "&u=n'); return false;";
            else
                imgRatingClip.Visible = false;

            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void VesselParticulars_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidVesselParticulars())
            {
                string selectedcrewingagency = SelectedCrewingAgency();

                PhoenixRegistersVessel.UpdateVesselCrewRelatedInformation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(Filter.CurrentVesselMasterFilter)
                    , selectedcrewingagency
                    , General.GetNullableInteger(ucOfficerPool.SelectedPool)
                    , General.GetNullableInteger(ucRatingPool.SelectedPool)
                    , General.GetNullableInteger(ucAddrPandIClub.SelectedAddress)
                    , General.GetNullableInteger(RadMcUserFM.SelectedValue)
                    , RadMcUserFM.SelectedEmail
                    , General.GetNullableInteger(RadMcUserSup.SelectedValue)
                    , General.GetNullableString(RadMcUserSup.SelectedEmail)
                    , General.GetNullableInteger(ucMedicals.SelectedHard)
                    , General.GetNullableDecimal(txtDeductible.Text)
                    , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                    , General.GetNullableInteger(ucFleet.SelectedFleet.ToString())
                    , General.GetNullableInteger(RadMcUserFPM.SelectedValue)
                    , RadMcUserFPM.SelectedEmail
                    , General.GetNullableInteger(RadMcUserPO.SelectedValue)
                    , RadMcUserPO.SelectedEmail
                    , General.GetNullableInteger(RadMcUserFPS.SelectedValue)
                    , RadMcUserFPS.SelectedEmail
                    , General.GetNullableInteger(RadMcUserTP.SelectedValue)
                    , RadMcUserTP.SelectedEmail
                    , General.GetNullableInteger(RadMcUserTD.SelectedValue)
                    , RadMcUserTD.SelectedEmail
                    , General.GetNullableInteger(RadMcUserAA.SelectedValue)
                    , RadMcUserAA.SelectedEmail
                    , General.GetNullableInteger(RadMcUserP.SelectedValue)
                    , RadMcUserP.SelectedEmail
                    , General.GetNullableInteger(ucTechFleet.SelectedFleet.ToString())
                    , General.GetNullableInteger(ucAcctFleet.SelectedFleet.ToString())
                    , General.GetNullableInteger(RadMcUserQP.SelectedValue)
                    , RadMcUserQP.SelectedEmail
                    , General.GetNullableInteger(RadMcUserQA.SelectedValue)
                    , RadMcUserQA.SelectedEmail
                    , General.GetNullableInteger(RadMcUserDPA.SelectedValue)
                    , RadMcUserDPA.SelectedEmail
                    , General.GetNullableInteger(RadMcUserVP.SelectedValue)
                    , RadMcUserVP.SelectedEmail
                    , General.GetNullableInteger(MCMarineSupt.SelectedValue)
                    , General.GetNullableString(MCMarineSupt.SelectedEmail));

                ucStatus.Text = "Admin details Updated.";
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }

    private void BindCrewingAgency()
    {
        DataSet dsCrewingAgency = PhoenixRegistersAddress.ListAddress("142");

        lbCrewingAgency.DataSource = dsCrewingAgency;
        lbCrewingAgency.DataTextField = dsCrewingAgency.Tables[0].Columns["FLDNAME"].ToString();
        lbCrewingAgency.DataValueField = dsCrewingAgency.Tables[0].Columns["FLDADDRESSCODE"].ToString();
        lbCrewingAgency.DataBind();
    }

    private string SelectedCrewingAgency()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lbCrewingAgency.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    private void SelectCrewingAgency(string crewingagency)
    {
        string[] deliminator = { "," };

        string[] strcrewingagency = crewingagency.Split(deliminator, 100, StringSplitOptions.RemoveEmptyEntries);

        foreach (string str in strcrewingagency)
        {
            if (str != "-1")
            {
                lbCrewingAgency.Items.FindByValue(str).Selected = true;
            }
        }
    }

    private bool IsValidVesselParticulars()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (txtFleetManagerEmail.Text.Trim() != "" && !General.IsvalidEmail(txtFleetManagerEmail.Text))
        //    ucError.ErrorMessage = "Fleet Manager Email is not a valid one";

        //if (txtSuptEmail.Text.Trim() != "" && !General.IsvalidEmail(txtSuptEmail.Text))
        //    ucError.ErrorMessage = "Supt Email is not a valid one";
        if (General.GetNullableInteger(ucMedicals.SelectedHard) == null)
            ucError.ErrorMessage = "Medical Type is Required.";

        return (!ucError.IsError);
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }
}
