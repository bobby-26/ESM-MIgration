using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;

public partial class OwnersVesselCommunicationDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            EditVesselCommunicationDetails(Int16.Parse(Filter.CurrentOwnerVesselMasterFilter));
        }
    }

    protected void EditVesselCommunicationDetails(int vesselId)
    {
        DataSet ds = PhoenixOwnersVessel.EditCommunicationDetails(vesselId);

        DataSet dsVessel = PhoenixOwnersVessel.EditVessel(vesselId);

        DataRow drVessel = dsVessel.Tables[0].Rows[0];

        txtVesselName.Text = drVessel["FLDVESSELNAME"].ToString();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtAPhone.Text = dr["FLDAPHONE"].ToString();
            txtAFax.Text = dr["FLDAFAX"].ToString();
            txtBPhone.Text = dr["FLDBPHONE"].ToString();
            txtBFax.Text = dr["FLDBFAX"].ToString();
            txtEmail.Text = dr["FLDEMAIL"].ToString();
            //txtAccAdministratorEmail.Text = dr["FLDACCOUNTADMINISTRATOREMAIL"].ToString();
            txtNotificationEmail.Text = dr["FLDNOTIFICATIONEMAIL"].ToString();
            //txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            txtAccInchargeEmail.Text = dr["FLDACCOUNTINCHARGEEMAIL"].ToString();

            txtPhone.Text = dr["FLDPHONE"].ToString();
            txtFax.Text = dr["FLDFAX"].ToString();
            txtMobileNumber.Text = dr["FLDMOBILENUMBER"].ToString();
            txtFPhone.Text = dr["FLDFPHONE"].ToString();
            txtFFax.Text = dr["FLDFFAX"].ToString();
            txtCTalex.Text = dr["FLDCTALEX"].ToString();
        }
    }     
}
