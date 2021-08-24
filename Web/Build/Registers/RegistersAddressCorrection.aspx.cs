using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAddressCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Update", "SAVE" , ToolBarDirection.Right);
                MenuOfficeMain.AccessRights = this.ViewState;
                MenuOfficeMain.MenuList = toolbar.Show();
                //MenuOfficeMain.SetTrigger(pnlAddressEntry);

                PhoenixToolbar toolbarAddress = new PhoenixToolbar();
                toolbarAddress.AddButton("Address", "ADDRESS", ToolBarDirection.Left);
                toolbarAddress.AddButton("Bank", "BANK", ToolBarDirection.Left);
                toolbarAddress.AddButton("Question", "QUESTION", ToolBarDirection.Left);
                toolbarAddress.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);
                toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT", ToolBarDirection.Left);
                toolbarAddress.AddButton("Address Correction", "CORRECTION", ToolBarDirection.Left);
                toolbarAddress.AddButton("Contacts", "CONTACTS", ToolBarDirection.Left);

                MenuAddressMain.AccessRights = this.ViewState;
                MenuAddressMain.MenuList = toolbarAddress.Show();
                MenuAddressMain.SelectedMenuIndex = 5;

                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();
                    DataSet dsaddress = PhoenixRegistersAddress.EditAddress(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        long.Parse(Request.QueryString["ADDRESSCODE"].ToString()));

                    if (dsaddress.Tables.Count > 0)
                    {
                        DataRow draddress = dsaddress.Tables[0].Rows[0];
                        ViewState["FLDDTKEY"] = draddress["FLDDTKEY"].ToString();
                    }
                }

                cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                    Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));

                cblProduct.DataTextField = "FLDQUICKNAME";
                cblProduct.DataValueField = "FLDQUICKCODE";
                cblProduct.DataBind();

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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../Registers/RegistersOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=");
            }
            if (CommandName.ToUpper().Equals("QUESTION"))
            {
                Response.Redirect("../Registers/RegistersAddressQuestion.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Registers/RegistersAddressAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("AGREEMENTSATTACHMENT"))
            {
                Response.Redirect("../Registers/RegistersAgreementsAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CORRECTION"))
            {
                Response.Redirect("../Registers/RegistersAddressCorrection.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersSupplierInvoiceApprovalUserMap.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CONTACTS"))
            {
                Response.Redirect("../Registers/RegistersAddressPurpose.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)FindControl("txtPhone2");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if (ViewState["FLDDTKEY"] == null)
                //{
                //    ucError.ErrorMessage = "You are not a valid user";
                //    ucError.Visible = true;
                //    return;
                //}

                if (ViewState["FLDDTKEY"] != null)
                {
                    PhoenixRegistersAddressCorrection.UpdateCorrectedAddress(
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
            DataSet dsaddress = PhoenixRegistersAddressCorrection.EditAddressForOfficeConfirmation(
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
                string[] producttype = draddress["FLDPRODUCTTYPE"].ToString().Split(',');

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        cblProduct.Items.FindByValue(item).Selected = true;
                    }
                }

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
