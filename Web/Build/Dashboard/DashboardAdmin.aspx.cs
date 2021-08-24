using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PhoenixToolbar toolbarmain = new PhoenixToolbar();

        //toolbarmain.AddButton("Particulars", "PARTICULARS");
        //toolbarmain.AddButton("Crew List", "CREWLIST");
        //toolbarmain.AddButton("Certificates", "CERTIFICATES");
        //toolbarmain.AddButton("Admin", "ADMIN");
        //toolbarmain.AddButton("Office Admin", "OFFICE");
        //toolbarmain.AddButton("Manning", "MANNING");
        //toolbarmain.AddButton("Travel", "TRAVEL");
        //toolbarmain.AddButton("Attachments","ATTACHMENTS");
        //toolbarmain.AddButton("Summary", "SUMMARY");
        //toolbarmain.AddButton("Sync Status", "OLD");
        //toolbarmain.AddButton("Alerts", "ALERTS");
        //MenuDdashboradVesselParticulrs.MenuList = toolbarmain.Show();
        //MenuDdashboradVesselParticulrs.SelectedMenuIndex = 3;

        BindToolBar();

        if (!IsPostBack)
        {
            EditVesselParticulars(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
    }

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            int index = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                toolbar.AddButton(dr["FLDURLDESCRIPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='ADMIN'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuDdashboradVesselParticulrs.MenuList = toolbar.Show();
            MenuDdashboradVesselParticulrs.SelectedMenuIndex = index;
        }
    }


    protected void EditVesselParticulars(int vesselId)
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();

            //ucOfficerPool.SelectedPool = dr["FLDOFFICERPOOL"].ToString();
            //ucRatingPool.SelectedPool = dr["FLDRATINGPOOL"].ToString();
            //ucAddrPandIClub.SelectedAddress = dr["FLDPANDICLUB"].ToString();

            txtCrewingFleet.Text = dr["FLDCREWINGFLEETNAME"].ToString();
            txtTechnicalFleet.Text = dr["FLDTECHFLEETNAME"].ToString();
            txtAccountsFleet.Text = dr["FLDACCOUNTSFLEETNAME"].ToString();
            txtOfficerPool.Text = dr["FLDOFFICERPOOLNAME"].ToString();
            txtRatingPool.Text = dr["FLDRATINGPOOLNAME"].ToString();
            txtPIClub.Text = dr["FLDPANDICLUBNAME"].ToString();
            txtMedicalType.Text = dr["FLDMEDICALTYPEREQUIRED"].ToString();
            txtInsuranceCurrency.Text = dr["FLDINSURANCECURRENCY"].ToString();
            txtDeductible.Text = dr["FLDDEDUCTIBLES"].ToString();
            txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            txtSuptEmail.Text = dr["FLDSUPTEMAIL"].ToString();
            
            txtFPMEmail.Text = dr["FLDFPMEMAIL"].ToString();
            
            txtPOEmail.Text = dr["FLDPERSONALOFFICEREMAIL"].ToString();
            
            txtFPSEmail.Text = dr["FLDFPSEMAIL"].ToString();
            
            txtTravelPICEmail.Text = dr["FLDTRAVELPICEMAIL"].ToString();
           
            txtAccountAdminEmail.Text = dr["FLDACCOUNTADMINEMAIL"].ToString();
            
            txtTechDirectorEmail.Text = dr["FLDTECHDIRECTOREMAIL"].ToString();
            
            txtESMStdWage.Text = dr["FLDSTANDARDWAGECODENAME"].ToString();

            txtSuptName.Text = dr["FLDSUPTNAME"].ToString();
            txtSuptDesignation.Text = dr["FLDSUPTDESIGNATION"].ToString();
            txtFleetManagerName.Text = dr["FLDFLEETMANAGERNAME"].ToString();
            txtFleetManagerDesignation.Text = dr["FLDFLEETMANAGERDESIGNATION"].ToString();
            txtFPMName.Text = dr["FLDFPMNAME"].ToString();
            txtFPMDesignation.Text = dr["FLDFPMDESIGNATION"].ToString();
            txtPOName.Text = dr["FLDPONAME"].ToString();
            txtPODesignation.Text = dr["FLDPERSONALOFFICERDESIGNATION"].ToString();
            txtFPSName.Text = dr["FLDFPSNAME"].ToString();
            txtFPSDesignation.Text = dr["FLDFPSDESIGNATION"].ToString();
            txtTravelPICName.Text = dr["FLDTRAVELPICNAME"].ToString();
            txtTravelPICDesignation.Text = dr["FLDTRAVELPICDESIGNATION"].ToString();
            txtTechDirectorName.Text = dr["FLDTECHDIRECTORNAME"].ToString();
            txtTechDirectorDesignation.Text = dr["FLDTECHDIRECTORDESIGNATION"].ToString();
            txtAccountAdminName.Text = dr["FLDACCOUNTADMINNAME"].ToString();
            txtAccountAdminDesignation.Text = dr["FLDACCOUNTADMINDESIGNATION"].ToString();

            txtPurchaserName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtPurchaserDesignation.Text = dr["FLDSUPPLIERDESIGNATION"].ToString();
            txtPurchaserEmail.Text = dr["FLDSUPPLIEREMAIL"].ToString();
            
            txtQualityPICName.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
            txtQualityPICDesignation.Text = dr["FLDQUALITYINCHARGEDESIGNATION"].ToString();
            txtQualityPICEmail.Text = dr["FLDQUALITYINCHARGEEMAIL"].ToString();
            

            txtQAManagerName.Text = dr["FLDQAMANAGERNAME"].ToString();
            txtQAManagerDesignation.Text = dr["FLDQAMANAGERDESIGNATION"].ToString();
            txtQAManagerEmail.Text = dr["FLDQAMANAGEREMAIL"].ToString();
            

            txtDPAName.Text = dr["FLDDPANAME"].ToString();
            txtDPADesignation.Text = dr["FLDDPADESIGNATION"].ToString();
            txtDPAEmail.Text = dr["FLDDPAEMAIL"].ToString();
            

            txtOfficerWageScale.Text = dr["FLDWAGESCALENAME"].ToString();
            txtRatingsWageScale.Text = dr["FLDWAGESCALERATINGNAME"].ToString();
            txtSeniorityWageScale.Text = dr["FLDSENIORITYSCALENAME"].ToString();

            //ucFleet.SelectedFleet = dr["FLDFLEET"].ToString();
            //ucMedicals.SelectedHard = dr["FLDMEDICALREQUIRED"].ToString();
            
            //ucCurrency.SelectedCurrency = dr["FLDINSURANCECURRENCY"].ToString();
            //ucTechFleet.SelectedFleet = dr["FLDTECHFLEET"].ToString();
            //ucAcctFleet.SelectedFleet = dr["FLDACCOUNTFLEET"].ToString();

            ViewState["FLDDTKEY"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void MenuDdashboradVesselParticulrs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("CREWLIST"))
        {
           Response.Redirect("../Dashboard/DashboardVesselCrewList.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
        {
            Response.Redirect("../Dashboard/DashboardVesselCertificates.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("ADMIN"))
        {
            Response.Redirect("../Dashboard/DashboardAdmin.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("OFFICE"))
        {
            Response.Redirect("../Dashboard/DashboardOfficeAdmin.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../Dashboard/DashboardManning.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Dashboard/DashboardAttachments.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS, true);
        }

        if (dce.CommandName.ToUpper().Equals("TRAVEL"))
        {
            Response.Redirect("../Dashboard/DashboardTravelSingOnOffList.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("SUMMARY"))
        {
            Response.Redirect("../Dashboard/DashboardSummary.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("OLD"))
        {
            Response.Redirect("../Dashboard/DashboardVesselSynchronizationStatus.aspx", false);
        }

        if (dce.CommandName.ToUpper().Equals("ALERTS"))
        {
            Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
        }

    }


}
