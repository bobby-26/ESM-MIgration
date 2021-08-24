using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsAdvanceRemittanceRequest : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtSupplierId.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            if (!IsPostBack)
            {
                ViewState["Remittenceid"] = "";
                if ((Request.QueryString["REMITTENCEID"] != null) && (Request.QueryString["REMITTENCEID"] != ""))
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    ddlBankAccount.Enabled = false;
                    BindHeader(ViewState["Remittenceid"].ToString());
                    PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid);
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                    ddlBankAccount.Enabled = true;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindHeader(string remittanceid)
    {

        DataSet ds = PhoenixAccountsAdvanceRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
            Title1.Text = "Remittance      (" + dr["FLDREMITTANCENUMBER"].ToString() + ")     ";
        }
        else
        {
            Title1.Text = "Remittance";
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["Remittenceid"] == null)
                {
                    if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, txtSupplierId.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string iRemittenceNumber = "";
                    PhoenixAccountsAdvanceRemittance.InsertRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(txtAccountId.Text),
                                                                            General.GetNullableInteger(txtCurrencyId.Text),
                                                                            241,
                                                                            ref iRemittenceNumber,
                                                                            txtSubAccountCode.Text,
                                                                            int.Parse(ddlBankAccount.SelectedBankAccount),
                                                                            int.Parse(txtSupplierId.Text)
                                                                       );
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    void Reset()
    {
        txtRemittanceNumber.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtSupplierId.Text = "";
        ddlBankAccount.SelectedBankAccount = null;
        ddlBankAccount.Enabled = true;
        txtCurrencyCode.Text = "";
        ViewState["Remittenceid"] = null;
        Title1.Text = "Remittance";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemittance(string accountcode, string strSupplierId)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (strSupplierId.Trim().Equals(""))
        {
            if (int.TryParse(strSupplierId, out result) == false)
                ucError.ErrorMessage = "Please select Supplier.";
        }
        if (accountcode.Trim().Equals("Dummy"))
        {
            if (int.TryParse(accountcode, out result) == false)
                ucError.ErrorMessage = "Please select Account Code.";
        }
        return (!ucError.IsError);
    }

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
                txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
            }
        }
    }
}
