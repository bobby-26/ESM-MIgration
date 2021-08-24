using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucherAdminFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        txtMakerId.Attributes.Add("style", "visibility:hidden");

        txtMakerCode.Attributes.Add("onkeydown", "return false;");
        txtMakerName.Attributes.Add("onkeydown", "return false;");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        txtVoucherNumberSearch.Focus();

        if (!IsPostBack)
        {          
           //DateTime now = DateTime.Now;

            //txtVoucherFromdateSearch.Text = now.Date.AddMonths(-1).ToShortDateString();
            //txtVoucherTodateSearch.Text = DateTime.Now.ToShortDateString();

            if (Request.QueryString["source"] != null)
                ViewState["Source"] = Request.QueryString["source"];

            if (ViewState["Source"].ToString() == "remittancegenerate")
            {
                ddlVoucherStatus.Enabled = false;
                ddlVoucherStatus.SelectedHard = "48";
            }

            if (ViewState["Source"].ToString() == "cashoutgenerate")
            {
                ddlVoucherStatus.Enabled = false;
                ddlVoucherStatus.SelectedHard = "48";
            }
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {

            if ((txtVoucherTodateSearch.Text != null && txtVoucherFromdateSearch.Text != null) && (txtVoucherNumberSearch.Text.Trim() == "" && txtMakerId.Text.Trim() == "" && ddlCurrencyCode.SelectedCurrency == "Dummy"
                        && chkShowRemittancenotGenerated.Checked == false && ddlVoucherStatus.SelectedHard == "Dummy" && txtInvoiceNumber.Text.Trim() == "" && txtPurchaseInvoiceVoucherNumber.Text.Trim() == "" && ddlSource.SelectedHard == "Dummy"
                        && ddlType.SelectedHard == "Dummy" && chkReportNotTaken.Checked == false))
            {
                TimeSpan t = (Convert.ToDateTime(txtVoucherTodateSearch.Text) - Convert.ToDateTime(txtVoucherFromdateSearch.Text));
                double Noofdays = t.Days;
                if (Noofdays > 365)
                {
                    ucError.ErrorMessage = "Selected date range should be within one year.";
                    ucError.Visible = true;
                    return;
                }
            }

            //TimeSpan t = (Convert.ToDateTime(txtVoucherTodateSearch.Text) - Convert.ToDateTime(txtVoucherFromdateSearch.Text));
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
            //else if (Noofdays > 100)
            //{
            //    ucError.ErrorMessage = "Selected date range should be within 100 days.";
            //    ucError.Visible = true;
            //    return;
            //}

            criteria.Clear();
            criteria.Add("txtVoucherNumberSearch", txtVoucherNumberSearch.Text.Trim());
            criteria.Add("txtMakerId", txtMakerId.Text);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtVoucherFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtVoucherTodateSearch.Text);
            if (chkShowRemittancenotGenerated.Checked == true)
            {
                criteria.Add("chkShowRemittancenotGenerated", "0");
            }
            else
            {
                criteria.Add("chkShowRemittancenotGenerated", "1");
            }
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ddlVoucherStatus", ddlVoucherStatus.SelectedHard);
            criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text.Trim());
            criteria.Add("txtPurchaseInvoiceVoucherNumber", txtPurchaseInvoiceVoucherNumber.Text.Trim());
            criteria.Add("ddlSource", ddlSource.SelectedHard);
            criteria.Add("ddlType", ddlType.SelectedHard);
            if (chkReportNotTaken.Checked == true)
            {
                criteria.Add("chkReportNotTaken", "1");
            }
            else
            {
                criteria.Add("chkReportNotTaken", "null");
            }

            Filter.CurrentInvoicePaymentVoucherAdminSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
        }

        if (ViewState["Source"] != null)
        {
            if (ViewState["Source"].ToString() == "paymentvoucher")
            {
                Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherAdminMaster.aspx", false);
            }
        }
    }

    //protected void ucMonthHard_Changed(object sender, EventArgs e)
    //{
    //    //int month = Convert.ToInt32(ucMonthHard.SelectedHard);
    //    DataTable dt = new DataTable();
    //    dt = PhoenixAccountsInvoice.GetMonthForInvoive(General.GetNullableInteger(ddlYearlist.SelectedQuick.ToString()),
    //                                                    General.GetNullableInteger(ucMonthHard.SelectedHard.ToString()));

    //    txtVoucherFromdateSearch.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
    //    txtVoucherTodateSearch.Text = dt.Rows[0]["FLDTODATE"].ToString();
    //    //DateTime.Now.Date(Convert.ToInt32((dt.Rows[0]["FLDSHORTNAME"].ToString()))).ToShortDateString();
    //}
}



