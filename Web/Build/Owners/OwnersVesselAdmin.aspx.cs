using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class OwnersVesselAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ucAddrPandIClub.AddressType = ((int)PhoenixAddressType.PANDICLUB).ToString();

            BindCrewingAgency();
            EditVesselParticulars(Int16.Parse(Filter.CurrentOwnerVesselMasterFilter));
        }
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

            SelectCrewingAgency(dr["FLDCREWINGAGENCY"].ToString());

            ucFleet.SelectedFleet = dr["FLDFLEET"].ToString();
            ucMedicals.SelectedHard = dr["FLDMEDICALREQUIRED"].ToString();
            txtDeductible.Text = dr["FLDDEDUCTIBLES"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDINSURANCECURRENCY"].ToString();
            ucTechFleet.SelectedFleet = dr["FLDTECHFLEET"].ToString();
            ucAcctFleet.SelectedFleet = dr["FLDACCOUNTFLEET"].ToString();
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
}
