using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsAdvancePaymentFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtPaymentNumber.Focus();
            //ucPaymentStatus.HardTypeCode = "119";
            ViewState["SelectedVesselList"] = "";
            BindVesselList();
            if (Request.QueryString["status"].ToString() != "")
                ViewState["status"] = int.Parse(Request.QueryString["status"].ToString());

        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx', true); ");
        txtVendorId.Attributes.Add("style", "display:none");
    }
    private void BindVesselList()
    {        
        chkVesselList.Items.Clear();
        chkVesselList.DataSource = PhoenixRegistersVessel.ListAllVessel(1);                
        chkVesselList.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataBindings.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtPaymentNumber", txtPaymentNumber.Text.Trim());
            criteria.Add("txtSupplierReference", txtSupplierReferenceSearch.Text.Trim());
            criteria.Add("txtSupplierCode", txtVendorId.Text.Trim());
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());
            criteria.Add("FindType", "FIND");
            Filter.CurrentAdvancePaymentSelection = criteria;

            if (int.Parse(ViewState["status"].ToString()) > 0)
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx?type1", false);
            else
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            criteria.Add("FindType", "FIND");
            Filter.CurrentAdvancePaymentSelection = criteria;
            if (int.Parse(ViewState["status"].ToString()) > 0)
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx?type1", false);
            else
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx", false);
        }
       
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        foreach (ButtonListItem item in chkVesselList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }
}



