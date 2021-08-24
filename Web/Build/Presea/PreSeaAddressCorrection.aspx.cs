using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaAddressCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Update", "SAVE");
            MenuOfficeMain.AccessRights = this.ViewState;
            MenuOfficeMain.MenuList = toolbar.Show();
            MenuOfficeMain.SetTrigger(pnlAddressEntry);

            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            toolbarAddress.AddButton("Address", "ADDRESS");
            toolbarAddress.AddButton("Address Correction", "CORRECTION");

            MenuAddressMain.AccessRights = this.ViewState;
            MenuAddressMain.MenuList = toolbarAddress.Show();
            MenuAddressMain.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
               
                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();
                    DataSet dsaddress = PhoenixPreSeaAddress.EditAddress(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        long.Parse(Request.QueryString["ADDRESSCODE"].ToString()));

                    if (dsaddress.Tables.Count > 0)
                    {
                        DataRow draddress = dsaddress.Tables[0].Rows[0];
                        ViewState["FLDDTKEY"] = draddress["FLDDTKEY"].ToString();
                    }
                }
                if (ViewState["FLDDTKEY"] != null)
                {
                    VendorAddressEdit();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../PreSea/PreSeaOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"]);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OfficeMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)FindControl("txtPhone2");

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["FLDDTKEY"] != null)
                {
                    PhoenixPreSeaAddressCorrection.UpdateCorrectedAddress(
                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                 new Guid(ViewState["FLDDTKEY"].ToString()));

                    VendorAddressEdit();
                    ucStatusMessage.Text = "Address information updated";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidAddress()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtEmail1.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Email is required.";

        return (!ucError.IsError);
    }

    protected void VendorAddressEdit()
    {
        try
        {
            DataSet dsaddress = PhoenixPreSeaAddressCorrection.EditAddressForOfficeConfirmation(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                int.Parse(ViewState["ADDRESSCODE"].ToString()));

            if (dsaddress.Tables[0].Rows.Count > 0)
            {
                DataRow draddress = dsaddress.Tables[0].Rows[0];

                txtName.Text = draddress["FLDNAME"].ToString();
                txtAddress1.Text = draddress["FLDADDRESS1"].ToString();
                txtAddress2.Text = draddress["FLDADDRESS2"].ToString();
                txtAddress3.Text = draddress["FLDADDRESS3"].ToString();
                txtAddress4.Text = draddress["FLDADDRESS4"].ToString();
                ucCountry.SelectedCountry = draddress["FLDCOUNTRYID"].ToString();

                if (ucCountry.SelectedCountry != draddress["FLDCOUNTRYID"].ToString())
                {
                    ucCountry.CountryList = PhoenixRegistersCountry.ListCountry(1); //Active Country                
                }
                ucCountry.SelectedCountry = draddress["FLDCOUNTRYID"].ToString();
                if (draddress["FLDCOUNTRYID"].ToString() != ucState.Country)
                {
                    ucState.Country = draddress["FLDCOUNTRYID"].ToString();
                    ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(draddress["FLDCOUNTRYID"].ToString()));
                    ddlCity.Country = draddress["FLDCOUNTRYID"].ToString();
                    ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(draddress["FLDCOUNTRYID"].ToString()), null);
                }

                ucState.SelectedState = draddress["FLDSTATE"].ToString();
                if (draddress["FLDSTATE"].ToString() != ddlCity.State)
                {
                    ddlCity.State = draddress["FLDSTATE"].ToString();
                    ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(draddress["FLDSTATE"].ToString()));
                }

                ddlCity.SelectedCity = draddress["FLDCITY"].ToString();
                txtPostalCode.Text = draddress["FLDPOSTALCODE"].ToString();
                txtPhone1.Text = draddress["FLDPHONE1"].ToString();
                txtPhone2.Text = draddress["FLDPHONE2"].ToString();
                txtEmail1.Text = draddress["FLDEMAIL1"].ToString();
                txtEmail2.Text = draddress["FLDEMAIL2"].ToString();
                txtFax1.Text = draddress["FLDFAX1"].ToString();
                txtFax2.Text = draddress["FLDFAX2"].ToString();
                txtURL.Text = draddress["FLDURL"].ToString();
                txtAttention.Text = draddress["FLDATTENTION"].ToString();
                txtInCharge.Text = draddress["FLDINCHARGE"].ToString();
                txtaohTelephoneno.Text = draddress["FLDAOHTELEPHONENO"].ToString();
                txtaohMobileno.Text = draddress["FLDAOHMOBILENO"].ToString();

                ViewState["FLDDTKEY"] = draddress["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
