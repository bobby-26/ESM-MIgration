using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewVesselMasterAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenPick.Attributes.Add("style", "display:none");

        txtFleetManager.Attributes.Add("style", "display:none");
        txtSupt.Attributes.Add("style", "display:none");
        txtFPM.Attributes.Add("style", "display:none");
        txtPersonalOfficer.Attributes.Add("style", "display:none");
        txtFPS.Attributes.Add("style", "display:none");
        txtTravelPIC.Attributes.Add("style", "display:none");
        txtTechDirector.Attributes.Add("style", "display:none");
        txtAccountAdmin.Attributes.Add("style", "display:none");
        txtPurchaser.Attributes.Add("style", "display:none");
        txtQualityPIC.Attributes.Add("style", "display:none");
        txtQAManager.Attributes.Add("style", "display:none");
        txtDPA.Attributes.Add("style", "display:none");
        txtCrewManager.Attributes.Add("style", "display:none");

        txtFleetManagerEmailHidden.Attributes.Add("style", "display:none");
        txtSuptEmailHidden.Attributes.Add("style", "display:none");
        txtFPMEmailHidden.Attributes.Add("style", "display:none");
        txtPOEmailHidden.Attributes.Add("style", "display:none");
        txtFPSEmailHidden.Attributes.Add("style", "display:none");
        txtTravelPICEmailHidden.Attributes.Add("style", "display:none");
        txtTechDirectorEmailHidden.Attributes.Add("style", "display:none");
        txtAccountAdminEmailHidden.Attributes.Add("style", "display:none");
        txtPurchaserEmailHidden.Attributes.Add("style", "display:none");
        txtQualityPICEmailHidden.Attributes.Add("style", "display:none");
        txtQAManagerEmailHidden.Attributes.Add("style", "display:none");
        txtDPAEmailHidden.Attributes.Add("style", "display:none");
        txtCrewManagerEmailHidden.Attributes.Add("style", "display:none");

        if (!IsPostBack)
        {
            ucAddrPandIClub.AddressType = ((int)PhoenixAddressType.PANDICLUB).ToString();
            BindCrewingAgency();

            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != "")
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
                EditVesselParticulars(Int16.Parse(ViewState["VESSELID"].ToString()));
            }
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuVesselAdmin.AccessRights = this.ViewState;
        MenuVesselAdmin.MenuList = toolbar.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtSuptEmailHidden")
            txtSuptEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtFleetManagerEmailHidden")
            txtFleetManagerEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtTechDirectorEmailHidden")
            txtTechDirectorEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtPOEmailHidden")
            txtPOEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtFPMEmailHidden")
            txtFPMEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtFPSEmailHidden")
            txtFPSEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtTravelPICEmailHidden")
            txtTravelPICEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtAccountAdminEmailHidden")
            txtAccountAdminEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtPurchaserEmailHidden")
            txtPurchaserEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtQualityPICEmailHidden")
            txtQualityPICEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtQAManagerEmailHidden")
            txtQAManagerEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtDPAEmailHidden")
            txtDPAEmail.Text = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[4].ToString() == "txtCrewManagerEmailHidden")
            txtCrewManagerEmail.Text = Filter.CurrentPickListSelection.Get(4);
    }

    protected void EditVesselParticulars(int vesselId)
    {
        DataSet ds = PhoenixCrewManagement.EditVesselCrewAdmin(vesselId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();

            ucOfficerPool.SelectedPool = dr["FLDOFFICERPOOL"].ToString();
            ucRatingPool.SelectedPool = dr["FLDRATINGPOOL"].ToString();
            ucAddrPandIClub.SelectedAddress = dr["FLDPANDICLUB"].ToString();
            txtFleetManager.Text = dr["FLDFLEETMANAGER"].ToString();
            txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            txtSuptEmail.Text = dr["FLDSUPTEMAIL"].ToString();
            txtSupt.Text = dr["FLDSUPT"].ToString();
            txtFPMEmail.Text = dr["FLDFPMEMAIL"].ToString();
            txtFPM.Text = dr["FLDFPM"].ToString();
            txtPOEmail.Text = dr["FLDPERSONALOFFICEREMAIL"].ToString();
            txtPersonalOfficer.Text = dr["FLDPERSONALOFFICER"].ToString();
            txtFPSEmail.Text = dr["FLDFPSEMAIL"].ToString();
            txtFPS.Text = dr["FLDFPS"].ToString();
            txtTravelPICEmail.Text = dr["FLDTRAVELPICEMAIL"].ToString();
            txtTravelPIC.Text = dr["FLDTRAVELPIC"].ToString();
            txtAccountAdminEmail.Text = dr["FLDACCOUNTADMINEMAIL"].ToString();
            txtAccountAdmin.Text = dr["FLDACCOUNTADMIN"].ToString();
            txtTechDirectorEmail.Text = dr["FLDTECHDIRECTOREMAIL"].ToString();
            txtTechDirector.Text = dr["FLDTECHDIRECTOR"].ToString();
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
            txtPurchaser.Text = dr["FLDSUPPLIER"].ToString();

            txtQualityPICName.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
            txtQualityPICDesignation.Text = dr["FLDQUALITYINCHARGEDESIGNATION"].ToString();
            txtQualityPICEmail.Text = dr["FLDQUALITYINCHARGEEMAIL"].ToString();
            txtQualityPIC.Text = dr["FLDQUALITYINCHARGE"].ToString();

            txtCrewManagerName.Text = dr["FLDCREWMANAGERNAME"].ToString();
            txtCrewManagerDesignation.Text = dr["FLDCREWMANAGERDESIGNATION"].ToString();
            txtCrewManagerEmail.Text = dr["FLDCREWMANAGEREMAIL"].ToString();
            txtCrewManager.Text = dr["FLDCREWMANAGER"].ToString();

            txtQAManagerName.Text = dr["FLDQAMANAGERNAME"].ToString();
            txtQAManagerDesignation.Text = dr["FLDQAMANAGERDESIGNATION"].ToString();
            txtQAManagerEmail.Text = dr["FLDQAMANAGEREMAIL"].ToString();
            txtQAManager.Text = dr["FLDQAMANAGER"].ToString();

            txtDPAName.Text = dr["FLDDPANAME"].ToString();
            txtDPADesignation.Text = dr["FLDDPADESIGNATION"].ToString();
            txtDPAEmail.Text = dr["FLDDPAEMAIL"].ToString();
            txtDPA.Text = dr["FLDDPA"].ToString();

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

    protected void MenuVesselAdmin_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["launched"] == "CrewList")
                {
                    Response.Redirect("../Crew/CrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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