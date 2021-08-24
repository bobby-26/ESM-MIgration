using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class UserControlCommonAddress : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    public string Address1
    {
        get { return txtAddressLine1.Text; }
        set { txtAddressLine1.Text = value; }
    }

    public string Address2
    {
        get { return txtAddressLine2.Text; }
        set { txtAddressLine2.Text = value; }
    }

    public string Address3
    {
        get { return txtAddressLine3.Text; }
        set { txtAddressLine3.Text = value; }
    }
    public string Address4
    {
        get { return txtAddressLine4.Text; }
        set { txtAddressLine4.Text = value; }
    }
    public string City
    {
        get { return ddlCity.SelectedCity; }
        set { ddlCity.SelectedCity = value; }
    }

    public string State
    {
        get {
            if (ddlState.SelectedState.ToUpper() == "DUMMY")
                return string.Empty;
            else
            return ddlState.SelectedState; }
        set
        {
            ddlState.SelectedState = value;
            if (value != ddlCity.State)
            {
                ddlCity.State = value;
                ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(value));
            }
        }
    }

    public string Country
    {
        get { return ddlCountry.SelectedCountry; }
        set
        {
            if (ddlCountry.SelectedCountry != value)
            {
                ddlCountry.CountryList = PhoenixRegistersCountry.ListCountry(1); //Active Country                
            }
            ddlCountry.SelectedCountry = value;
            if (value != ddlState.Country)
            {
                ddlState.Country = value;
                ddlState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(value));
                ddlCity.Country = value;
                ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(value), null);
            }            
        }
    }

    public string PostalCode
    {
        get { return txtPinCode.Text; }
        set { txtPinCode.Text = value; }
    }

    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        ddlState.SelectedState = "";
        ddlCity.SelectedCity = "";
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableInteger(ddlCountry.SelectedCountry));
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), null);
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.SelectedCity = "";
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));        
    }
}
