using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOfficeDebitCreditNoteGenerateSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();      
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuFilterMain.AccessRights = this.ViewState;
        MenuFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["SelectedStatus"] = "";
            ddlType.DataBind();
            BindStatus();
        }
    }

    protected void ddlType_DataBound(object sender, EventArgs e)
    {
        ddlType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
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
            criteria.Add("txtSubject", txtSubject.Text);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("txtReferenceNo", txtReferenceNo.Text);
            criteria.Add("ddlBillToCompany", ddlBillToCompany.SelectedCompany.Equals("Dummy") ? "" : ddlBillToCompany.SelectedCompany.ToString());
            criteria.Add("chkVesselList", ddlType.SelectedValue);
            criteria.Add("ddlBank", ddlBank.SelectedValue);
            criteria.Add("txtFromAmount", txtFromAmount.Text);
            criteria.Add("txtToAmount", txtToAmount.Text);
            criteria.Add("ddlOpenClose", ddlOpenClose.SelectedValue);
            criteria.Add("ucCurrencyCode", ucCurrencyCode.SelectedCurrency);
            criteria.Add("txtReceivedStatus", ViewState["SelectedStatus"].ToString());
            Filter.OfficeDebitCreditNote = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.OfficeDebitCreditNote = criteria;
        }
        Session["New"] = "Y";
        Response.Redirect("../Accounts/AccountsOfficeDebitCreditNoteGenerateRegister.aspx", false);
    }

    private void BindStatus()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 141);
        chkStatus.Items.Add("select");
        chkStatus.DataSource = ds;
        chkStatus.DataTextField = "FLDQUICKNAME";
        chkStatus.DataValueField = "FLDQUICKCODE";
        chkStatus.DataBind();
    }

    protected void chkStatus_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true && !ViewState["SelectedStatus"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedStatus"] = ViewState["SelectedStatus"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedStatus"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedStatus"] = ViewState["SelectedStatus"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }
}
