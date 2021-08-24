using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsCreateFundRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.Title = "Create Fund Request";
            MenuSave.MenuList = toolbarmain.Show();
            txtSupplierCode.Attributes.Add("onkeydown", "return false;");
            txtSupplierName.Attributes.Add("onkeydown", "return false;");
            txtSupplierId.Attributes.Add("onkeydown", "return false;");
            cmdHiddenSubmit.Attributes.Add("style", "Display:None;");
            txtSupplierId.Attributes.Add("style", "Display:None;");
            //txtBankID.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["CREDITDEBITNOTEID"] = "";
                if ((Request.QueryString["creditnoteid"] != null) && (Request.QueryString["creditnoteid"] != ""))
                {
                    ViewState["CREDITDEBITNOTEID"] = Request.QueryString["creditnoteid"].ToString();
                    BindHeader(ViewState["CREDITDEBITNOTEID"].ToString());
                    BindBankAccount();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindBankAccount()
    {
        DataSet dsBankAccount = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(lblbilltocompany.Text),0);
        ddlBank.DataSource = dsBankAccount;
        ddlBank.DataTextField = "FLDBANKACCOUNTNUMBER";
        ddlBank.DataValueField = "FLDSUBACCOUNTID";
        ddlBank.DataBind();
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    private void BindHeader(string CREDITDEBITNOTEID)
    {
        if (ViewState["CREDITDEBITNOTEID"] != null)
        {
            DataSet ds = PhoenixAccountsCreditDebitNote.EditCreditDebitNote(new Guid(ViewState["CREDITDEBITNOTEID"].ToString()));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    txtSupplierCode.Text = dr["FLDCODE"].ToString();
                    txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
                    txtcurrency.Text = dr["FLDCURRENCYNAME"].ToString();
                    txtDebitnoteNoReferenceno.Text = dr["FLDREFERENCENO"].ToString();
                    lblcurrencyid.Text = dr["FLDCURRENCYCODE"].ToString();
                    lblbilltocompany.Text = dr["FLDCOMPANYID"].ToString();
                }
            }
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsCreateFundRequest.CreateFundRequestInsert(
                    General.GetNullableGuid(Convert.ToString(ViewState["CREDITDEBITNOTEID"]))
                    , General.GetNullableInteger(txtSupplierId.Text)
                    , txtDebitnoteNoReferenceno.Text
                    , General.GetNullableInteger(ddlBank.SelectedValue)
                    , General.GetNullableDateTime(ucDate.Text)
                    , txtSubject.Text
                    , txtDescription.Text);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
}
