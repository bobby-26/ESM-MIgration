using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
public partial class CrewTravelPassengerInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {          
            if (Request.QueryString["Requestid"] != null)
            {
                ListTravelPassengerInformation(Request.QueryString["Requestid"].ToString());                
            }
        }    
    }
    public void ListTravelPassengerInformation(string requestid)
    {
        try
        {
            DataTable dt = PhoenixCrewTravelRequest.EditTravelRequest(General.GetNullableGuid(requestid));
            if (dt.Rows.Count > 0)
            {
                txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                txtcdcno.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
  
                txtOrigin.Text = dt.Rows[0]["FLDORIGINNAME"].ToString();
                txtDestination.Text = dt.Rows[0]["FLDDESTINATIONNAME"].ToString();
                ucDateOfArrival.Text = dt.Rows[0]["FLDARRIVALDATE"].ToString();
                ucDateOftravel.Text = dt.Rows[0]["FLDTRAVELDATE"].ToString();

                txtpdateofissue.Text = String.Format("{0:MM/dd/yy}", dt.Rows[0]["FLDPASSPORTDATEOFISSUE"].ToString());
                txtpplaceodissue.Text = dt.Rows[0]["FLDPASSPORTPLACEOFISSUE"].ToString();
                txtpdateofexpiry.Text = String.Format("{0:MM/dd/yy}", dt.Rows[0]["FLDPASSPORTEXPIRYDATE"].ToString());

                txtcdcdateofissue.Text = String.Format("{0:MM/dd/yy}", dt.Rows[0]["FLDCDCDATEOFISSUE"].ToString());
                txtcdcplaceofissue.Text = dt.Rows[0]["FLDCDCPLACEOFISSUE"].ToString();
                txtcdcdateofexpiry.Text = String.Format("{0:MM/dd/yy}", dt.Rows[0]["FLDCDCEXPIRYDATE"].ToString());
                ucnationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();

                txtusvisa.Text = dt.Rows[0]["FLDUSVISANUMBER"].ToString();
                txtothervisa.Text = dt.Rows[0]["FLDOTHERVISADETAILS"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
