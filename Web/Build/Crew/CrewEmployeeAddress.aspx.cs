using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewEmployeeAddress : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("New", "NEW");
        toolbarmain.AddButton("Save", "SAVE");
		MenuEmployeeAddress.AccessRights = this.ViewState;
        MenuEmployeeAddress.MenuList = toolbarmain.Show();
        MenuEmployeeAddress.SetTrigger(pnlEmployeeAddressEntry);
        txtEmployeeNo.Text = "1";
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["FLDEMPLOYEEADDRESSID"] = null;
            BindData();
        }
       
    }

    protected void MenuEmployeeAddress_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            ViewState["EMPLOYEEADDRESSID"] = null;
            Reset();

        }
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {

            if (!IsValidEmployeeAddress())
            {
                ucError.Visible = true;
                return;

            }

            if (ViewState["FLDEMPLOYEEADDRESSID"] == null)
            {
                PhoenixCrewEmloyeeAddress.InsertEmloyeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                              Convert.ToInt32(txtEmployeeNo.Text),
                                                              Convert.ToInt32(PhoenixCrewConstants.PERMANENTADDRESS),
                                                              txtAddressPermanent.Text,
                                                              txtCityPermanent.Text,
                                                              Convert.ToInt32(ucCountryPermanent.SelectedCountry),
                                                              Convert.ToInt32(txtStatePermanent.Text),
                                                              txtPinPermanent.Text,
                                                              General.GetNullableInteger(txtPhonenoPermanent.Text),
                                                              General.GetNullableInteger(txtMobilenoPermanent.Text),
                                                              General.GetNullableInteger(txtFaxNoPermanent.Text),
                                                              txtEmail.Text,
                                                              Convert.ToInt32(ucAirport.SelectedAirport),
                                                              Convert.ToInt32(ucInstitution.SelectedInstitution));

                 PhoenixCrewEmloyeeAddress.InsertEmloyeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                              Convert.ToInt32(txtEmployeeNo.Text),
                                                              Convert.ToInt32(PhoenixCrewConstants.LOCALADDRESS),
                                                              txtAddressLocal.Text,
                                                              txtCityLocal.Text,
                                                              Convert.ToInt32(ucCountryLocal.SelectedCountry),
                                                              Convert.ToInt32(txtStatePermanent.Text),
                                                              txtPinLocal.Text,
                                                              General.GetNullableInteger(txtPhonenoLocal.Text),
                                                              General.GetNullableInteger(txtMobilenoLocal.Text),
                                                              General.GetNullableInteger(txtFaxnoLocal.Text),
                                                              txtEmail.Text,
                                                              Convert.ToInt32(ucAirport.SelectedAirport),
                                                              Convert.ToInt32(ucInstitution.SelectedInstitution));

                 BindData();
            }
            else
            {
                PhoenixCrewEmloyeeAddress.UpdateEmloyeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                              Convert.ToInt32(ViewState["FLDEMPLOYEEADDRESSID"]),
                                                              Convert.ToInt32(txtEmployeeNo.Text),
                                                              Convert.ToInt32(PhoenixCrewConstants.PERMANENTADDRESS),
                                                              txtAddressPermanent.Text,
                                                              txtCityPermanent.Text,
                                                              Convert.ToInt32(ucCountryPermanent.SelectedCountry),
                                                              txtStatePermanent.Text,
                                                              txtPinPermanent.Text,
                                                              General.GetNullableInteger(txtPhonenoPermanent.Text),
                                                              General.GetNullableInteger(txtMobilenoPermanent.Text),
                                                              General.GetNullableInteger(txtFaxNoPermanent.Text),
                                                              txtEmail.Text,
                                                              Convert.ToInt32(ucAirport.SelectedAirport),
                                                              Convert.ToInt32(ucInstitution.SelectedInstitution),
                                                              Convert.ToInt32(PhoenixCrewConstants.LOCALADDRESS),
                                                              txtAddressLocal.Text,
                                                              txtCityLocal.Text,
                                                              Convert.ToInt32(ucCountryLocal.SelectedCountry),
                                                              txtStateLocal.Text,
                                                              txtPinLocal.Text,
                                                              General.GetNullableInteger(txtPhonenoLocal.Text),
                                                              General.GetNullableInteger(txtMobilenoLocal.Text),
                                                              General.GetNullableInteger(txtFaxnoLocal.Text));
                BindData();
            }       
        }
     
    }


    public bool IsValidEmployeeAddress()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtAddressPermanent.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Permanent Address is required.";

        if (txtAddressLocal.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Local Address is required.";

        if(ucCountryPermanent.SelectedCountry=="0")
            ucError.ErrorMessage = " Permanent Address Country is required.";

        if (ucCountryLocal.SelectedCountry == "0")
            ucError.ErrorMessage = "Local Address Country is required.";


        return (!ucError.IsError);
    }

   private void BindData()
    {

        DataSet dsaddress = PhoenixCrewEmloyeeAddress.EditEmloyeeAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                       Convert.ToInt32(txtEmployeeNo.Text),
                                                                       Convert.ToInt32(PhoenixCrewConstants.PERMANENTADDRESS),
                                                                       Convert.ToInt32(PhoenixCrewConstants.LOCALADDRESS));

        if (dsaddress.Tables[0].Rows.Count > 0)
        {
            DataRow draddress = dsaddress.Tables[0].Rows[0];
            lblEmployeeAddressid.Text = draddress["FLDEMPLOYEEADDRESSID"].ToString();
            txtAddressPermanent.Text = draddress["FLDADDRESSPERMANENT"].ToString();
            txtAddressLocal.Text = draddress["FLDADDRESSLOCAL"].ToString();
            txtCityPermanent.Text = draddress["FLDCITYPERMANENT"].ToString();
            txtCityLocal.Text = draddress["FLDCITYLOCAL"].ToString();
            txtPinPermanent.Text = draddress["FLDPINCODEPERMANENT"].ToString();
            txtPinLocal.Text = draddress["FLDPINCODELOCAL"].ToString();
            txtStatePermanent.Text = draddress["FLDSTATEPERMANENT"].ToString();
            txtStateLocal.Text = draddress["FLDSTATELOCAL"].ToString();
            txtPhonenoPermanent.Text = draddress["FLDPHONENUMBERPERMANENT"].ToString();
            txtPhonenoLocal.Text = draddress["FLDPHONENUMBERLOCAL"].ToString();
            txtMobilenoPermanent.Text = draddress["FLDMOBILENUMBERPERMANENT"].ToString();
            txtMobilenoLocal.Text = draddress["FLDMOBILENUMBERLOCAL"].ToString();
            txtFaxNoPermanent.Text = draddress["FLDFAXNUMBERPERMANENT"].ToString();
            txtFaxnoLocal.Text = draddress["FLDFAXNUMBERLOCAL"].ToString();
            ucCountryPermanent.SelectedCountry = draddress["FLDCOUNTRYPERMANENT"].ToString();
            ucCountryLocal.SelectedCountry = draddress["FLDCOUNTRYLOCAL"].ToString();
            txtEmail.Text = draddress["FLDEMAIL"].ToString();
            ucAirport.SelectedAirport = draddress["FLDNEARESTAIRPORT"].ToString();
            ucInstitution.SelectedInstitution = draddress["FLDPREFFEREDINSTITUTION"].ToString();
            ViewState["FLDEMPLOYEEADDRESSID"] = lblEmployeeAddressid.Text;
        }
        else
        {
            ViewState["FLDEMPLOYEEADDRESSID"] = null;
        }  
    }

   protected void CopyPermanentAddress(object sender, EventArgs e)
   {
       txtAddressLocal.Text = txtAddressPermanent.Text;
       txtCityLocal.Text = txtCityPermanent.Text;
       txtPinLocal.Text = txtPinPermanent.Text;
       txtStateLocal.Text = txtStatePermanent.Text;
       ucCountryLocal.SelectedCountry = ucCountryPermanent.SelectedCountry;
       txtPhonenoLocal.Text = txtPhonenoPermanent.Text;
       txtMobilenoLocal.Text = txtMobilenoPermanent.Text;
       txtFaxnoLocal.Text = txtFaxNoPermanent.Text;
   }

   protected void Reset()
   {
       txtAddressPermanent.Text = "";
       txtAddressLocal.Text = "";
       txtCityPermanent.Text = "";
       txtCityLocal.Text = "";
       txtPinPermanent.Text = "";
       txtPinLocal.Text = "";
       txtStatePermanent.Text = "";
       txtStateLocal.Text = "";
       txtPhonenoLocal.Text = "";
       txtPhonenoPermanent.Text = "";
       txtPhonenoLocal.Text = "";
       txtPhonenoPermanent.Text = "";
       txtMobilenoPermanent.Text = "";
       txtMobilenoLocal.Text = "";
       ucCountryPermanent.SelectedCountry = "0";
       ucCountryLocal.SelectedCountry = "0";
       ucAirport.SelectedAirport = "0";
       ucInstitution.SelectedInstitution= "0";
   }
}
