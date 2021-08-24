using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewLicencePaymentDatailsEdit : PhoenixBasePage
{
    Guid g = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            //toolbarmain.AddButton("Cancel", "CANCEL");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (Request.QueryString["ADVANCEPAYMENTID"].ToString() != null)
            {
                ViewState["ADVANCEPAYMENTID"] = Request.QueryString["ADVANCEPAYMENTID"];
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                EditDetails();
                ProcessEdit();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditDetails()
    {

        DataSet ds = new DataSet();

        ds = PhoenixAccountsAdvancePayment.AdvancePaymentEdit(new Guid(ViewState["ADVANCEPAYMENTID"].ToString()));

        ViewState["ADDRESSCODE"] = ds.Tables[0].Rows[0]["FLDSUPPLIERCODE"].ToString();
        g = new Guid(ds.Tables[0].Rows[0]["FLDORDERID"].ToString());
        BankBind();

        ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        ddlBank.SelectedValue = ds.Tables[0].Rows[0]["FLDBANKID"].ToString();

        if (ds.Tables[0].Rows[0]["FLDPAYMENTSTATUS"].ToString() != "629")
        {
            ucCompany.Enabled = false;
            ddlBank.Enabled = false;
        }
    }

    protected void BankBind()
    {
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBank.DataSource = ds;
            ddlBank.DataTextField = "FLDBANKNAME";
            ddlBank.DataValueField = "FLDBANKID";
            ddlBank.DataBind();
        }
        else
        {
            ddlBank.Items.Clear();
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            txtCurrency.Text = "";
        }
    }

    protected void BankCurrency(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()),
            General.GetNullableInteger(ddlBank.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows.Count == 1)
            {

                txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                txtBeneficiaryName.Text = ds.Tables[0].Rows[0]["FLDBENEFICIARYNAME"].ToString();
                txtBankAccount.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNUMBER"].ToString();
            }
            else
            {
                txtCurrency.Text = "";
            }
        }

    }

    private void ProcessEdit()
    {
        DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceProcess(g);
        if (dt.Rows.Count > 0)
        {
            ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();
            if (dt.Rows[0]["FLDBANKID"].ToString() != "")
            {
                ddlBank.SelectedValue = dt.Rows[0]["FLDBANKID"].ToString();
                txtCurrency.Text = dt.Rows[0]["FLDCURRENCYCODE"].ToString();
                txtBeneficiaryName.Text = dt.Rows[0]["FLDBENEFICIARYNAME"].ToString();
                txtBankAccount.Text = dt.Rows[0]["FLDACCOUNTNUMBER"].ToString();
            }
            else
            {
                txtCurrency.Text = "";
                txtBeneficiaryName.Text = "";
            }
        }
    }

    public void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBankCompany())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsAdvancePayment.LicenceAdvancePaymentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(ViewState["ADVANCEPAYMENTID"].ToString())
                                                                            , General.GetNullableInteger(ddlBank.SelectedValue)
                                                                            , General.GetNullableInteger(ucCompany.SelectedCompany));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidBankCompany()
    {
        if (ddlBank.SelectedValue == "" || ddlBank.SelectedValue.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Please Select Bank";

        if (ucCompany.SelectedCompany == "" || ucCompany.SelectedCompany.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Company is required";
                
        return (!ucError.IsError);
    }
}
