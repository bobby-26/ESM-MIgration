using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;

public partial class AccountsRemitanceEmpBankDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        { 
            if (Request.QueryString["remittanceid"] != null && Request.QueryString["remittanceid"] != "")
                ViewState["remittanceid"] = Request.QueryString["remittanceid"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {


        string remittanceid;

        if (ViewState["remittanceid"] != null)
        {
            remittanceid = ViewState["remittanceid"].ToString();
        }
        else
        {
            remittanceid = null;
        }

        DataSet ds = PhoenixAccountsAllotmentRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            lbltBeneficiary.Text = dr["FLDACCOUNTNAME"].ToString();
            lbltBank.Text = dr["FLDBANKNAME"].ToString();
            lbltIFSCCode.Text = dr["FLDBANKIFSCCODE"].ToString();
            lbltSwiftCode.Text = dr["FLDBANKSWIFTCODE"].ToString();
            lbltCurrency.Text = dr["FLDEMPLOYEECURRENCY"].ToString();
            lbltAddress.Text = dr["FLDBANKADDRESS"].ToString();

        }
    }
}
