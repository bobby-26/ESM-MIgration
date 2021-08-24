using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class PurchaseVendorAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuOfficeMain.AccessRights = this.ViewState;
                MenuOfficeMain.MenuList = toolbar.Show();
                

                ViewState["REGKEY"] = null;

                ViewState["SESSIONID"] = Request.QueryString["sessionid"];

                cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
                cblProduct.DataBindings.DataTextField = "FLDQUICKNAME";
                cblProduct.DataBindings.DataValueField = "FLDQUICKCODE";
                cblProduct.DataBind();

                if (ViewState["SESSIONID"] != null)
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
    protected void OfficeMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)FindControl("txtPhone2");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["REGKEY"] == null)
                {
                    ucError.ErrorMessage = "You are not a valid user";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidAddress())
                {
                    ucError.ErrorMessage = ErrorMessage;
                    ucError.Visible = true;
                    return;
                }
                if (!General.IsvalidEmail(txtEmail1.Text))
                {
                    ucError.ErrorMessage = "Please enter valid e-mail1";
                    ucError.Visible = true;
                    return;
                }
                if (!phoneno.IsValidPhoneNumber())
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    StringBuilder strproducttype = new StringBuilder();
                    foreach (ButtonListItem item in cblProduct.Items)
                    {
                        if (item.Selected == true)
                        {
                            strproducttype.Append(item.Value.ToString());
                            strproducttype.Append(",");
                        }
                    }
                    if (strproducttype.Length > 1)
                    {
                        strproducttype.Remove(strproducttype.Length - 1, 1);
                    }

                    if (ViewState["REGKEY"] != null)
                    {
                        PhoenixRegistersAddressCorrection.UpdateAddress(
                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                     new Guid(ViewState["REGKEY"].ToString()),
                                     txtName.Text,
                                     General.GetNullableString(txtAddress1.Text),
                                     General.GetNullableString(txtAddress2.Text),
                                     General.GetNullableString(txtAddress3.Text),
                                     General.GetNullableString(txtAddress4.Text),
                                     General.GetNullableInteger(ucCountry.SelectedCountry),
                                     General.GetNullableInteger(ucState.SelectedState),
                                     General.GetNullableInteger(ddlCity.SelectedCity),
                                     General.GetNullableString(txtPostalCode.Text),
                                     General.GetNullableString(txtPhone1.Text),
                                     General.GetNullableString(txtPhone2.Text),
                                     General.GetNullableString(txtFax1.Text),
                                     General.GetNullableString(txtFax2.Text),
                                     txtEmail1.Text,
                                     General.GetNullableString(txtEmail2.Text),
                                     General.GetNullableString(txtURL.Text),
                                     General.GetNullableString(txtAttention.Text),
                                     General.GetNullableString(txtInCharge.Text),
                                     General.GetNullableString(strproducttype.ToString()),
                                     General.GetNullableString(txtaohTelephoneno.Text),
                                     General.GetNullableString(txtaohMobileno.Text));

                        VendorAddressEdit();
                        ucStatusMessage.Text = "Address information updated";
                    }
                    else
                    {
                        ucError.ErrorMessage = "You are not a valid user";
                        ucError.Visible = true;
                    }
                }
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            ucCountry.SelectedCountry = value;
            if (value != ucState.Country)
            {
                ucState.Country = value;
                ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(value));
                ddlCity.Country = value;
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
        get { return txtEmail1.Text; }
        set { txtEmail1.Text = value; }
    }

    public string Email2
    {
        get { return txtEmail2.Text; }
        set { txtEmail2.Text = value; }
    }

    public string Url
    {
        get { return txtURL.Text; }
        set { txtURL.Text = value; }
    }

    public string InCharge
    {
        get { return txtInCharge.Text; }

        set { txtInCharge.Text = value; }
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

    protected void VendorAddressEdit()
    {
        try
        {
            DataSet dsaddress = PhoenixRegistersAddressCorrection.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SESSIONID"].ToString()));

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
                string[] producttype = draddress["FLDPRODUCTTYPE"].ToString().Split(',');

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        //cblProduct.Items.FindByValue(item).Selected = true;
                        cblProduct.Items[0].Selected = true;
                    }
                }

                ViewState["REGKEY"] = draddress["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
