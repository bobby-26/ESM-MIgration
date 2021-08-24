using System;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;


public partial class UserControlAddress : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucQAGrading.QuickTypeCode = Convert.ToInt32(PhoenixQuickTypeCode.QAGRADING).ToString();
            ucStatus.HardTypeCode = "191";
        }
    }

    public bool IsValidAddress()
    {
        ucError.HeaderMessage = "Please provide the following required information";

       
     
        if (txtName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (txtPhone1.Text.Trim().Replace("~", "").Equals(""))
        {
            ucError.ErrorMessage = "Phone number  is required.";
        }
        if (txtEmail1.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Email is required.";


        return (!ucError.IsError);
    }


    public string ErrorMessage
    {
        get
        {
            return ucError.ErrorMessage;
        }
    }

    public string Name
    {
        get { return txtName.Text; }
        set { txtName.Text = value; }
    }

    public string code
    {
        get { return txtCode.Text; }
        set { txtCode.Text = value; }
    }

    public string Attention
    {
        get { return txtAttention.Text; }
        set { txtAttention.Text = value; }
    }

    public string Address1
    {
        get { return txtAddress1.Text; }
        set { txtAddress1.Text = value; }
    }

    public string Address2
    {
        get { return txtAddress2.Text; }
        set { txtAddress2.Text = value; }
    }

    public string Address3
    {
        get { return txtAddress3.Text; }
        set { txtAddress3.Text = value; }
    }
	public string Address4
	{
		get { return txtAddress4.Text; }
		set { txtAddress4.Text = value; }
	}
    public string City
    {
        get { return ddlCity.SelectedCity; }
        set { ddlCity.SelectedCity = value; }
    }
	public string State
	{
		get
		{
			if (ucState.SelectedState.ToUpper() == "DUMMY")
				return string.Empty;
			else
				return ucState.SelectedState;
		}
		set
		{
			ucState.SelectedState = value;
			if (value != ddlCity.State)
			{
				ddlCity.State = value;
				ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(value));
			}
		}
	}

	public string Country
	{
		get { return ucCountry.SelectedCountry; }
		set
		{
			if (ucCountry.SelectedCountry != value)
			{
				ucCountry.CountryList = PhoenixRegistersCountry.ListCountry(1); //Active Country                
			}
            //ucCountry.SelectedCountry = value;
			ucCountry.SelectedCountry = value;
			if (value != ucState.Country)
			{
				ucState.Country = value;
                ucState.SelectedState = "";
				ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(value));
				ddlCity.Country = value;
                ddlCity.SelectedCity = "";
				ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(value), null);
			}
		}
	}
    public string PostalCode
    {
        get { return txtPostalCode.Text; }
        set { txtPostalCode.Text = value; }
    }

    public string Phone1
    {
        get { return txtPhone1.Text; }
        set { txtPhone1.Text = value; }
    }

    public string Phone2
    {
        get { return txtPhone2.Text; }
        set { txtPhone2.Text = value; }
    }

    public string Fax1
    {
        get { return txtFax1.Text; }
        set { txtFax1.Text = value; }
    }

    public string Fax2
    {
        get { return txtFax2.Text; }
        set { txtFax2.Text = value; }
    }
    public string Email1
    {
        get {return txtEmail1.Text ;}
        set { txtEmail1.Text = value; }
    }

    public string Email2
    {
        get { return txtEmail2.Text;}
        set { txtEmail2.Text = value; }
    }

    public string Url
    {
        get { return txtURL.Text;}
        set { txtURL.Text = value; }
    }

    public string Status
    {
        get { return ucStatus.SelectedHard; }
        set { ucStatus.SelectedHard = value; }
    }

    public string InCharge
    {
        get { return txtInCharge.Text; }

        set { txtInCharge.Text = value; }
    }

    public string QAGrading
    {
        get { return ucQAGrading.SelectedQuick; }
        set { ucQAGrading.SelectedQuick = value; }
    }

    public string Telephoneno
    {
        get { return txtaohTelephoneno.Text; }
        set { txtaohTelephoneno.Text = value; }
    }

    public string Mobileno
    {
        get { return txtaohMobileno.Text; }
        set { txtaohMobileno.Text = value; }
    }

    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
    }
	protected void ddlState_TextChanged(object sender, EventArgs e)
	{
		ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucState.SelectedState));
	}
}
