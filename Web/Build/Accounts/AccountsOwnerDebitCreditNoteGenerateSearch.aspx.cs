using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOwnerDebitCreditNoteGenerateSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        
        MenuFilterMain.AccessRights = this.ViewState;
        MenuFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ddlType.DataBind();
            ddlSubtype.DataBind();
            ddlBank.DataBind();
        }
    }

    protected void ddlType_DataBound(object sender, EventArgs e)
    {
        ddlType.Items.Insert(0, new RadComboBoxItem ("--Select--", ""));
    }
    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlSubtype_DataBound(object sender, EventArgs e)
    {
        ddlSubtype.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void ddlBillToCompany_Changed(object sender, EventArgs e)
    {
        ddlBank.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, General.GetNullableInteger(ddlBillToCompany.SelectedCompany));
        ddlBank.DataBind();
    }

    protected void MenuFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("txtReferenceNo", txtReferenceNo.Text);
            criteria.Add("ddlBillToCompany", ddlBillToCompany.SelectedCompany.Equals("Dummy") ? "" : ddlBillToCompany.SelectedCompany.ToString());
            criteria.Add("chkVesselList", ddlType.SelectedValue);
            criteria.Add("ddlBank", ddlBank.SelectedValue);
            criteria.Add("txtFromAmount", txtFromAmount.Text);
            criteria.Add("txtToAmount", txtToAmount.Text);
            criteria.Add("ddlOpenClose", ddlOpenClose.SelectedValue);
            criteria.Add("ddlType", ddlType.SelectedValue);
            criteria.Add("ddlSubType", ddlSubtype.SelectedValue);
            Filter.OwnerDebitCreditNote = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.OwnerDebitCreditNote = criteria;
        }
        Session["New"] = "Y";
        Response.Redirect("../Accounts/AccountsOwnerDebitCreditNoteGenerateRegister.aspx", false);

    }
}
