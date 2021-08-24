using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsDirectPOFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Direct PO";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
 
            BindVesselFleetList();
            BindVesselList();
            ddlAccountDetailsbind();
            ViewState["SelectedVesselList"] = "";

            //DateTime now = DateTime.Now;

            //txtReceivedFromdateSearch.Text = now.Date.AddMonths(-1).ToShortDateString();
            //txtReceivedTodateSearch.Text = DateTime.Now.ToShortDateString();
            
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132,135,183', true); ");

            imgShowBudgetCode.Attributes.Add("onclick", "return showPickList('spnPickListBudgetCode', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?framename=ifMoreInfo&budgetgroup=106&hardtypecode=30" + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

            //if (Request.QueryString["hardtypecode"] != null)
              ucBudgetGroup.HardTypeCode = "30";
            //if (Request.QueryString["budgetgroup"] != null)
            //ucBudgetGroup.SelectedHard = "106";

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
            StringBuilder strcompanylist = new StringBuilder();

            if ((txtReceivedTodateSearch.Text != null && txtReceivedFromdateSearch.Text != null) && (txtOrderNumber.Text.Trim() == "" && ddlCurrencyCode.SelectedCurrency == "Dummy" && txtVendorId.Text.Trim() == ""
                        && ddlStatus.SelectedValue == "Dummy" ))
            { 
                TimeSpan t = (Convert.ToDateTime(txtReceivedTodateSearch.Text) - Convert.ToDateTime(txtReceivedFromdateSearch.Text));
                double Noofdays = t.Days;
                if (Noofdays > 365)
                {
                    ucError.ErrorMessage = "Selected date range should be within one year.";
                    ucError.Visible = true;
                    return;
                }
            }

            //if (General.GetNullableInteger(ucMonthHard.SelectedHard) == null && General.GetNullableInteger(ddlYearlist.SelectedQuick) != null)
            //{
            //    ucError.ErrorMessage = "Please select month.";
            //    ucError.Visible = true;
            //    return;
            //}
            //else if (General.GetNullableInteger(ucMonthHard.SelectedHard) != null && General.GetNullableInteger(ddlYearlist.SelectedQuick) == null)
            //{
            //    ucError.ErrorMessage = "Please select year.";
            //    ucError.Visible = true;
            //    return;
            //}


            criteria.Clear();
            criteria.Add("ddlInvoiceType", ddlInvoiceType.SelectedHard);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());
            criteria.Add("txtReceivedFromdateSearch", txtReceivedFromdateSearch.Text);
            criteria.Add("txtReceivedTodateSearch", txtReceivedTodateSearch.Text);
            criteria.Add("txtOrderNumber", txtOrderNumber.Text.Trim());
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            criteria.Add("ucPortMulti", ucPortMulti.SelectedValue);
            criteria.Add("ucETAFrom", ucETAFrom.Text);
            criteria.Add("ucETATo", ucETATo.Text);
            criteria.Add("ucETDFrom", ucETDFrom.Text);
            criteria.Add("ucETDTo", ucETDTo.Text);
            criteria.Add("txtDescription", txtDescription.Text);
            criteria.Add("ddlAccountDetails", ddlAccountDetails.SelectedValue);
            criteria.Add("txtBudgetCodeId", txtBudgetCodeId.Text.Trim());
            criteria.Add("ucBudgetGroup", ucBudgetGroup.SelectedHard);

            Filter.CurrentDirectPOSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            criteria.Add("txtReceivedFromdateSearch", txtReceivedFromdateSearch.Text);
            criteria.Add("txtReceivedTodateSearch", txtReceivedTodateSearch.Text);
            Filter.CurrentDirectPOSelection = criteria;
        }

        Response.Redirect("../Accounts/AccountsInvoiceDirectPurchaseOrder.aspx", false);
        
    }
    private void ddlAccountDetailsbind()
    {
        DataSet ds = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null,1);
        //ddlAccountDetails.Items.Add("select");
        ddlAccountDetails.DataSource = ds;
        ddlAccountDetails.DataTextField = "FLDVESSELACCOUNTNAME";
        ddlAccountDetails.DataValueField = "FLDACCOUNTID";
        ddlAccountDetails.DataBind();
        ddlAccountDetails.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));

    }

    //protected void ucMonthHard_Changed(object sender, EventArgs e)
    //{
    //    //int month = Convert.ToInt32(ucMonthHard.SelectedHard);
    //    DataTable dt = new DataTable();
    //    dt = PhoenixAccountsInvoice.GetMonthForInvoive(General.GetNullableInteger(ddlYearlist.SelectedQuick.ToString()),
    //                                                    General.GetNullableInteger(ucMonthHard.SelectedHard.ToString()));

    //    txtReceivedFromdateSearch.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
    //    txtReceivedTodateSearch.Text = dt.Rows[0]["FLDTODATE"].ToString();
    //    //DateTime.Now.Date(Convert.ToInt32((dt.Rows[0]["FLDSHORTNAME"].ToString()))).ToShortDateString();
    //}

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel();
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
}
