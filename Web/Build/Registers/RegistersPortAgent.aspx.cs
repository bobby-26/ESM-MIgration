using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersPortAgent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            {
                ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVesselName.Enabled = false;
            }
            else
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVesselName.Enabled = false;
                }
            }

            if (Request.QueryString["AddressCode"] != null)
            {
                toolbar.AddButton("Save", "SAVE");
                ViewState["AddressCode"] = Request.QueryString["AddressCode"].ToString();

                AddressEdit(new Guid(Request.QueryString["AddressCode"].ToString()));
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
            }
            MenuCompanyList.AccessRights = this.ViewState;
            MenuCompanyList.MenuList = toolbar.Show();           
        }
    }

    private bool IsValidAddress()
    {
        if ((txtAgentName.Text == null) || (txtAgentName.Text == ""))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(ucVesselName.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel name is required.";

        if (General.GetNullableInteger(ddlAddressType.SelectedHard) == null)
            ucError.ErrorMessage = "Address Type is required.";

        if ((txtAddress1.Text == null) || (txtAddress1.Text == ""))
            ucError.ErrorMessage = "Address1 is required.";

        if (General.GetNullableInteger(ucCountry.SelectedCountry) == null)
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }

    private bool IsvalidEmail(string emailid, string message)
    {
        if (!General.IsvalidEmail(emailid))
        {
            ucError.ErrorMessage = message;
            ucError.Visible = true;
        }
        return (!ucError.IsError);
    }

    protected void AgentPortList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidAddress())
                {
                    ucError.Visible = true;
                    return;
                }

                if (txtEmail1.Text != "")
                {
                    if (!IsvalidEmail(txtEmail1.Text, "Please enter valid emailid-1"))
                        return;
                }               
                if (txtEmail2.Text != "")
                {
                    if (!IsvalidEmail(txtEmail2.Text, "Please enter valid emailid-2"))
                        return;
                }               

                if (ViewState["AddressCode"] != null)
                {
                    PhoenixRegistersPortAgent.UpdatePortAgent(new Guid((ViewState["AddressCode"].ToString())),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            txtAgentName.Text,
                                                            txtAddress1.Text,
                                                            txtAddress2.Text,
                                                            txtAddress3.Text,
                                                            txtFullAddress.Text,
                                                            General.GetNullableInteger((ucCountry.SelectedCountry).ToString()),
                                                            txtState.Text,
                                                            txtCity.Text,
                                                            txtPostalCode.Text,
                                                            txtTelephoneNo1.Text,
                                                            txtTelephoneNo2.Text,
                                                            txtFaxNo1.Text,
                                                            txtFaxNo2.Text,
                                                            txtEmail1.Text,
                                                            txtEmail2.Text,
                                                            ddlAddressType.SelectedHard,
                                                            General.GetNullableInteger(ucVesselName.SelectedVessel.ToString())
                                                            );
                }
                else
                {
                    PhoenixRegistersPortAgent.InsertPortAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                             txtAgentName.Text,
                                                             txtAddress1.Text,
                                                             txtAddress2.Text,
                                                             txtAddress3.Text,
                                                             txtFullAddress.Text,
                                                             General.GetNullableInteger((ucCountry.SelectedCountry).ToString()),
                                                             txtState.Text,
                                                             txtCity.Text,
                                                             txtPostalCode.Text,
                                                             txtTelephoneNo1.Text,
                                                             txtTelephoneNo2.Text,
                                                             txtFaxNo1.Text,
                                                             txtFaxNo2.Text,
                                                             txtEmail1.Text,
                                                             txtEmail2.Text,
                                                             ddlAddressType.SelectedHard,
                                                             General.GetNullableInteger(ucVesselName.SelectedVessel.ToString())
                                                             );
                }                
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }
   
    private void AddressEdit(Guid addresscode)
    {       
        DataSet ds = PhoenixRegistersPortAgent.EditPortAgent(addresscode);

        if (ds.Tables[0].Rows.Count > 0)
        {           
            DataRow dr = ds.Tables[0].Rows[0];
            txtAgentName.Text=dr["FLDNAME"].ToString();
            txtPostalCode.Text = dr["FLDPOSTALCODE"].ToString();
            ddlAddressType.SelectedHard = dr["FLDADDRESSTYPE"].ToString();
            txtAddress1.Text = dr["FLDADDRESS1"].ToString();
            txtAddress2.Text = dr["FLDADDRESS2"].ToString();
            txtAddress3.Text = dr["FLDADDRESS3"].ToString();
            txtFullAddress.Text = dr["FLDFULLADDRESS"].ToString();
            ucCountry.SelectedCountry = dr["FLDCOUNTRYID"].ToString();
            txtState.Text = dr["FLDSTATE"].ToString();
            txtCity.Text = dr["FLDCITY"].ToString();
            txtTelephoneNo1.Text = dr["FLDPHONE1"].ToString();
            txtTelephoneNo2.Text = dr["FLDPHONE2"].ToString();
            txtFaxNo1.Text = dr["FLDFAX1"].ToString();
            txtFaxNo2.Text = dr["FLDFAX2"].ToString();
            txtEmail1.Text = dr["FLDEMAIL1"].ToString();
            txtEmail2.Text = dr["FLDEMAIL2"].ToString();
            ucVesselName.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucVesselName.Enabled = false;
        }
    }
}
