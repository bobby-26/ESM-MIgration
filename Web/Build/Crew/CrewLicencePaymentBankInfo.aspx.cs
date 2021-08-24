using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewLicencePaymentBankInfo : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["ADDRESSCODE"] = string.Empty;
                ViewState["FLAGID"] = string.Empty;
                ViewState["BANKID"] = string.Empty;

                if (Request.QueryString["Addresscode"] != null && Request.QueryString["Flagid"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["Addresscode"];
                    ViewState["FLAGID"] = Request.QueryString["Flagid"];
                    ViewState["BANKID"] = Request.QueryString["Bankid"];
                    
                }

                DisableAddress();
                BankBind();
                AddressEdit();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void DisableAddress()
    {
        foreach (Control c in ucAddress.Controls)
        {
            if (c is TextBox)
            {
                TextBox txt = c as TextBox;
                txt.ReadOnly = true;
                txt.CssClass = "readonlytextbox";
            }
            if (c.GetType().FullName.Contains("usercontrols"))
            {
                foreach (Control cs in c.Controls)
                {
                    if (cs is DropDownList)
                    {
                        DropDownList ddl = cs as DropDownList;
                        ddl.Enabled = false;
                        ddl.CssClass = "readonlytextbox";
                    }
                }
            }
        }
    }
    protected void BankBind()
    {
        try
        {
            DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), General.GetNullableInteger(ViewState["BANKID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBank.Text = ds.Tables[0].Rows[0]["FLDBANKNAME"].ToString();
                txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                txtBeneficiaryName.Text = ds.Tables[0].Rows[0]["FLDBENEFICIARYNAME"].ToString();
                txtBankAccount.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNUMBER"].ToString();
            }
        }
           catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
   
    protected void AddressEdit()
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["ADDRESSCODE"].ToString()) && ViewState["ADDRESSCODE"].ToString() != "Dummy")
            {
                Int64 addresscode = Convert.ToInt64(ViewState["ADDRESSCODE"]);
                DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);
                if (dsaddress.Tables.Count > 0)
                {
                    DataRow draddress = dsaddress.Tables[0].Rows[0];
                    ucAddress.Name = draddress["FLDNAME"].ToString();
                    txtConsulate.Text = draddress["FLDNAME"].ToString();
                    ucAddress.Address1 = draddress["FLDADDRESS1"].ToString();
                    ucAddress.Address2 = draddress["FLDADDRESS2"].ToString();
                    ucAddress.Address3 = draddress["FLDADDRESS3"].ToString();
                    ucAddress.Country = draddress["FLDCOUNTRYID"].ToString();
                    ucAddress.QAGrading = draddress["FLDQAGRADING"].ToString();
                    ucAddress.State = draddress["FLDSTATE"].ToString();
                    ucAddress.City = draddress["FLDCITY"].ToString();
                    ucAddress.PostalCode = draddress["FLDPOSTALCODE"].ToString();
                    ucAddress.Phone1 = draddress["FLDPHONE1"].ToString();
                    ucAddress.Phone2 = draddress["FLDPHONE2"].ToString();
                    ucAddress.Email1 = draddress["FLDEMAIL1"].ToString();
                    ucAddress.Email2 = draddress["FLDEMAIL2"].ToString();
                    ucAddress.Fax1 = draddress["FLDFAX1"].ToString();
                    ucAddress.Fax2 = draddress["FLDFAX2"].ToString();
                    ucAddress.Url = draddress["FLDURL"].ToString();
                    ucAddress.code = draddress["FLDCODE"].ToString();
                    ucAddress.Attention = draddress["FLDATTENTION"].ToString();
                    ucAddress.InCharge = draddress["FLDINCHARGE"].ToString();
                    ucAddress.Status = draddress["FLDSTATUS"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
