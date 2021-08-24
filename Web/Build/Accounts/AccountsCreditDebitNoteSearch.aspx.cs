using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class AccountsCreditDebitNoteSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
      //  if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

            MenuFilterMain.AccessRights = this.ViewState;
            MenuFilterMain.Title = "IT/Superintendent Visit";
            MenuFilterMain.MenuList = toolbar.Show();

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132,135,183', true); ");
        }
    }
    protected void MenuFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        NameValueCollection criteria = new NameValueCollection();

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtVendorId", txtVendorId.Text);
            criteria.Add("txtCreditRegisterNo", txtCreditRegisterNo.Text);
            criteria.Add("ddlStatus", ddlStatus.SelectedHard.ToString());
            criteria.Add("txtVendorCreditNoteNo", txtVendorCreditNoteNo.Text);
            criteria.Add("ucReceivedFromDate", ucReceivedFromDate.Text);
            criteria.Add("ucReceivedToDate", ucReceivedToDate.Text);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);
            Filter.CreditDebitNoteFilter = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CreditDebitNoteFilter = criteria;
        }
        Session["New"] = "Y";
        Response.Redirect("../Accounts/AccountsCreditDebitNoteMaster.aspx", false);
    }
}
