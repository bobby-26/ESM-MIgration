using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsAdvancePaymentAdvancedFilter : PhoenixBasePage
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
           // ucPaymentStatus.ShortNameFilter = "ADP;APP;CNL;PAD;WAPP;POC";
            ViewState["SelectedVesselList"] = "";

            //DateTime now = DateTime.Now;

            //txtPaymentFromdate.Text = now.Date.AddMonths(-1).ToShortDateString();
            //txtPaymentTodate.Text = DateTime.Now.ToShortDateString();

            BindVesselList();
            if (Request.QueryString["status"].ToString() != "")
                ViewState["status"] = int.Parse(Request.QueryString["status"].ToString());

            ucInvoiceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.INVOICESTATUS).ToString();
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx', true); ");
        txtVendorId.Attributes.Add("style", "display:none");
    }

    //protected void ucMonthHard_Changed(object sender, EventArgs e)
    //{
    //    //int month = Convert.ToInt32(ucMonthHard.SelectedHard);
    //    DataTable dt = new DataTable();
    //    dt = PhoenixAccountsInvoice.GetMonthForInvoive(General.GetNullableInteger(ddlYearlist.SelectedQuick.ToString()),
    //                                                    General.GetNullableInteger(ucMonthHard.SelectedHard.ToString()));

    //    txtPaymentFromdate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
    //    txtPaymentTodate.Text = dt.Rows[0]["FLDTODATE"].ToString();
    //    //DateTime.Now.Date(Convert.ToInt32((dt.Rows[0]["FLDSHORTNAME"].ToString()))).ToShortDateString();
    //}

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

            if ((txtPaymentTodate.Text != null && txtPaymentFromdate.Text != null) && (txtPaymentNumber.Text.Trim() == "" && txtSupplierReferenceSearch.Text.Trim() == "" && ddlCurrencyCode.SelectedCurrency == "Dummy" 
                        && ucPaymentStatus.SelectedHard == "Dummy" && ucType.SelectedHard == "Dummy" && txtVendorId.Text.Trim() == "" && ViewState["SelectedVesselList"].ToString() == "" && ucInvoiceStatus.SelectedHard == "Dummy"))
            {
                TimeSpan t = (Convert.ToDateTime(txtPaymentTodate.Text) - Convert.ToDateTime(txtPaymentFromdate.Text));
                double Noofdays = t.Days;
                if (Noofdays > 365)
                {
                    ucError.ErrorMessage = "Selected date range should be within one year.";
                    ucError.Visible = true;
                    return;
                }
            }

            //TimeSpan t = (Convert.ToDateTime(txtPaymentTodate.Text) - Convert.ToDateTime(txtPaymentFromdate.Text));
            //double Noofdays = t.Days;

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
            //if (Noofdays > 365)
            //{
            //    ucError.ErrorMessage = "Selected date range should be within one year.";
            //    ucError.Visible = true;
            //    return;
            //}
            criteria.Clear();
            criteria.Add("txtPaymentNumber", txtPaymentNumber.Text.Trim());
            criteria.Add("txtSupplierReference", txtSupplierReferenceSearch.Text.Trim());
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtPaymentFromdate", txtPaymentFromdate.Text);
            criteria.Add("txtPaymentTodate", txtPaymentTodate.Text);
            criteria.Add("ucPaymentStatus", ucPaymentStatus.SelectedHard);
            criteria.Add("ucType", ucType.SelectedHard);
            criteria.Add("txtSupplierCode", txtVendorId.Text.Trim());
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());
            criteria.Add("ucInvoiceStatus", ucInvoiceStatus.SelectedHard);
            criteria.Add("FindType", "ADVANCEDFIND");
            criteria.Add("chkPOCancelled", (chkPOCancelled.Checked == true ? "1" : "0"));
            Filter.CurrentAdvancePaymentSelection = criteria;

            if (int.Parse(ViewState["status"].ToString()) > 0)
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx?type1", false);
            else
                Response.Redirect("../Accounts/AccountsAdvancePayment.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            criteria.Add("FindType", "FINDADVANCED");
            criteria.Add("txtPaymentFromdate", txtPaymentFromdate.Text);
            criteria.Add("txtPaymentTodate", txtPaymentTodate.Text);
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



