using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsInvoiceFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Incoice";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["qcalfrom"] != null && Request.QueryString["qcalfrom"] != string.Empty)
                ViewState["CALLFROM"] = Request.QueryString["qcalfrom"];
            if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                ViewState["CALFROM"] = Request.QueryString["qcallfrom"];

            txtInvoiceNumberSearch.Focus();
            bind();
            BindVesselFleetList();
            BindVesselList();
            BindVesselPurchaserList();
            BindVesselPurchaseSuptList();
            BindPortList();
            ViewState["SelectedVesselList"] = "";
            ucInvoiceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.INVOICESTATUS).ToString();
            ucInvoiceStatus.ShortNameFilter = "ACH,AMC,APO,ARE,ASI,FPP,ICD,INP,OMA,PAU,PBB,RCK,RPO,RWA,PUS,NTP,INR";

            if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() != string.Empty)
            {
                //Additional Reconcilation statuses will be displayed for Invoice Register By PO and Reconcilation only.
                if (ViewState["CALLFROM"].ToString() == "ADJUSTMENT" || ViewState["CALLFROM"].ToString() == "INVOICEFORPURCHASE")
                    ucInvoiceStatus.ShortNameFilter = "ACH,AMC,APO,ARE,ASI,AVR,AWR,FPP,ICD,INP,OMA,PAU,PBB,RCK,RPO,RWA,PUS,NTP,INR";
            }
            if (ViewState["CALFROM"] != null && ViewState["CALFROM"].ToString() != string.Empty)
            {
                if (ViewState["CALFROM"].ToString() == "inv")
                    dvVesselType.Visible = true;
            }
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true); ");
        }
    }
    private void BindVesselPurchaserList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVesselPurchaser();
        ddlPurchaserList.Items.Add("select");
        ddlPurchaserList.DataSource = ds;
        ddlPurchaserList.DataTextField = "FLDUSERNAME";
        ddlPurchaserList.DataValueField = "FLDUSERCODE";
        ddlPurchaserList.DataBind();
        ddlPurchaserList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    private void BindVesselPurchaseSuptList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVesselPurchaseSupdt();
        ddlSuptList.Items.Add("select");
        ddlSuptList.DataSource = ds;
        ddlSuptList.DataTextField = "FLDUSERNAME";
        ddlSuptList.DataValueField = "FLDUSERCODE";
        ddlSuptList.DataBind();
        ddlSuptList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void bind()
    {
        DataSet ds = PhoenixRegistersCompany.ListCompany();
        chkCompanyList.Items.Add("select");
        chkCompanyList.DataSource = ds;
        chkCompanyList.DataTextField = "FLDSHORTCODE";
        chkCompanyList.DataValueField = "FLDCOMPANYID";
        //DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"               
        chkCompanyList.DataBind();
        chkCompanyList.Items.Insert(0, new ListItem("--Select--", "Dummy"));

    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel(1);
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        chkFleetList.Items.Add("select");
        chkFleetList.DataSource = ds;
        chkFleetList.DataTextField = "FLDFLEETDESCRIPTION";
        chkFleetList.DataValueField = "FLDFLEETID";
        chkFleetList.DataBind();
        chkFleetList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkFleetList_Changed(object sender, EventArgs e)
    {
        StringBuilder strfleetlist = new StringBuilder();

        foreach (ListItem item in chkFleetList.Items)
        {
            if (item.Selected == true)
            {
                strfleetlist.Append(item.Value.ToString());
                strfleetlist.Append(",");
            }
        }
        if (strfleetlist.Length > 1)
        {
            strfleetlist.Remove(strfleetlist.Length - 1, 1);
        }
        if (strfleetlist.ToString().Contains("Dummy"))
        {
            strfleetlist = new StringBuilder();
            strfleetlist.Append("0");
        }
        if (strfleetlist.ToString() == null || strfleetlist.ToString() == "")
            strfleetlist.Append("-1");

        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist.ToString() == "0" ? null : strfleetlist.ToString());

        ViewState["SelectedVesselList"] = "";
        foreach (ListItem item in chkVesselList.Items)
            item.Selected = false;

        if (ds.Tables[0].Rows.Count > 0)
        {
            string vesselid = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vesselid = dr["FLDVESSELID"].ToString();
                foreach (ListItem item in chkVesselList.Items)
                {
                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
                        break;
                    }
                }
            }
        }
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVesselList.Items)
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

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidSearch())
            {
                ucError.Visible = true;
                return;
            }


            criteria.Add("ddlPurchaserList", ddlPurchaserList.SelectedValue);
            criteria.Add("ddlSuptList", ddlSuptList.SelectedValue);
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());

            criteria.Add("ucInvoiceStatus", ucInvoiceStatus.SelectedHard);

            StringBuilder strcompanylist = new StringBuilder();
            StringBuilder strPortList = new StringBuilder();

            foreach (ListItem item in chkCompanyList.Items)
            {
                if (item.Selected == true)
                {
                    strcompanylist.Append(item.Value.ToString());
                    strcompanylist.Append(",");
                }
            }
            if (strcompanylist.Length > 1)
            {
                strcompanylist.Remove(strcompanylist.Length - 1, 1);
            }
            if (strcompanylist.ToString().Contains("Dummy"))
            {
                strcompanylist = new StringBuilder();
                strcompanylist.Append("Dummy");

            }

            foreach (ListItem item in chkPortList.Items)
            {
                if (item.Selected == true)
                {
                    strPortList.Append(item.Value.ToString());
                    strPortList.Append(",");
                }
            }
            if (strPortList.Length > 1)
            {
                strPortList.Remove(strPortList.Length - 1, 1);
            }

            criteria.Clear();
            criteria.Add("ddlInvoiceType", ddlInvoiceType.SelectedHard);
            criteria.Add("txtInvoiceNumberSearch", txtInvoiceNumberSearch.Text.Trim());
            criteria.Add("txtSupplierReferenceSearch", txtSupplierReferenceSearch.Text.Trim());
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());
            criteria.Add("txtInvoiceFromdateSearch", txtInvoiceFromdateSearch.Text);
            criteria.Add("txtReceivedFromdateSearch", txtReceivedFromdateSearch.Text);
            criteria.Add("txtReceivedTodateSearch", txtReceivedTodateSearch.Text);
            criteria.Add("txtOrderNumber", txtOrderNumber.Text.Trim());
            criteria.Add("ucInvoiceStatus", ucInvoiceStatus.SelectedHard);
            criteria.Add("companylist", strcompanylist.ToString());
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());
            criteria.Add("txtRemarks", txtRemarks.Text.Trim());
            criteria.Add("ddlPurchaserList", ddlPurchaserList.SelectedValue);
            criteria.Add("ddlSuptList", ddlSuptList.SelectedValue);
            criteria.Add("ucPIC", ucPIC.SelectedUser);
            criteria.Add("chkPortList", strPortList.ToString());
            if (chkPriorityInv.Checked == true)
            {
                criteria.Add("chkPriorityInv", "1");
            }
            else
            {
                criteria.Add("chkPriorityInv", "null");
            }

            if (General.GetNullableDateTime(txtInvoiceTodateSearch.Text) == null)
            {
                if (General.GetNullableInteger(ddlInvPending.SelectedValue) != null)
                {
                    double days = double.Parse(ddlInvPending.SelectedValue);
                    days = days * -1;

                    criteria.Add("txtInvoiceTodateSearch", DateTime.Today.AddDays(days).ToShortDateString());
                    criteria.Add("invoiceStatusList", ",241,242,632,");
                }
                else
                    criteria.Add("txtInvoiceTodateSearch", txtInvoiceTodateSearch.Text);
            }
            else
                criteria.Add("txtInvoiceTodateSearch", txtInvoiceTodateSearch.Text);

            Filter.CurrentInvoiceSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceSelection = criteria;
        }
        if (ViewState["CALLFROM"].ToString() == "POSTINVOICE")
            Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "INVOICE")
            Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "INVOICEFORPURCHASE")
            Response.Redirect("../Accounts/AccountsInvoiceMasterForPurchase.aspx", false);
        else if (ViewState["CALLFROM"].ToString() == "ADJUSTMENT")
            Response.Redirect("../Accounts/AccountsInvoiceAdjustmentMaster.aspx", false);
    }

    public bool IsValidSearch()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if ((ddlPurchaserList.SelectedValue != null && ddlPurchaserList.SelectedValue != "Dummy") && ucInvoiceStatus.SelectedHard == "Dummy")
        {
            ucError.ErrorMessage = "Invoice status is required.";
        }

        if ((ddlSuptList.SelectedValue != null && ddlSuptList.SelectedValue != "Dummy") && ucInvoiceStatus.SelectedHard == "Dummy")
        {
            ucError.ErrorMessage = "Invoice status is required.";
        }
        if (ViewState["CALLFROM"].ToString() != "INVOICE")
        {
            if ((ViewState["SelectedVesselList"] != null && ViewState["SelectedVesselList"].ToString() != "") && ucInvoiceStatus.SelectedHard == "Dummy")
            {
                ucError.ErrorMessage = "Invoice status is required.";
            }
        }
        return (!ucError.IsError);
    }

    private void BindPortList()
    {
        DataSet ds = PhoenixRegistersSeaport.ListSeaport();
        chkPortList.Items.Add("select");
        chkPortList.DataSource = ds;
        chkPortList.DataTextField = "FLDSEAPORTNAME";
        chkPortList.DataValueField = "FLDSEAPORTID";
        chkPortList.DataBind();
        chkPortList.Items.Insert(0, new ListItem("--Select--", "0"));
    }
}



