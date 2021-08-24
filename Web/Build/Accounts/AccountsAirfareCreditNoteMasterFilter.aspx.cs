using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsAirfareCreditNoteMasterFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ucStatus.HardTypeCode = ((int)PhoenixHard.PRIORITY).ToString();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();

            criteria.Add("txtVendorId", txtVendorId.Text);
            criteria.Add("txtCreditNoteNumber", txtCreditNoteNumber.Text == null ? string.Empty : txtCreditNoteNumber.Text);
            criteria.Add("txtReferenceNumber", txtReferenceNumber.Text == null ? string.Empty : txtReferenceNumber.Text);
            criteria.Add("ddlBillToCompany", ddlBillToCompany.SelectedCompany == null ? string.Empty : ddlBillToCompany.SelectedCompany);
            criteria.Add("ucDocFromDate", ucDocFromDate.Text == null ? string.Empty : ucDocFromDate.Text);
            criteria.Add("ucDocToDate", ucDocFromDate.Text == null ? string.Empty : ucDocToDate.Text);
            criteria.Add("ucReceivedFromDate", ucReceivedFromDate.Text == null ? string.Empty : ucReceivedFromDate.Text);
            criteria.Add("ucReceivedDate", ucReceivedDate == null ? string.Empty : ucReceivedDate.Text);
            criteria.Add("txtVoucherNo", txtVoucherNo == null ? string.Empty : txtVoucherNo.Text);
            criteria.Add("txtPaymentVoucher", txtPaymentVoucher == null ? string.Empty : txtPaymentVoucher.Text);
            criteria.Add("chkshowzeroamount",Convert.ToString(chkshowzeroamount.Checked==true?1:0));
            criteria.Add("ucCashStatus", ucStatus.SelectedHard);
            Filter.CurrentAirfareCreditNoteRegisterFilter = criteria;

        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {

        }
        Response.Redirect("../Accounts/AccountsAirfareCreditNoteMaster.aspx", false);
    }
}
