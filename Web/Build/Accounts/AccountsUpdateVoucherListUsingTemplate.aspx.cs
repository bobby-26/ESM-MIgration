using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Data;
using System.IO;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Collections.Specialized;

public partial class Accounts_AccountsUpdateVoucherListUsingTemplate : System.Web.UI.Page
{
    public string Message;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Accounts/AccountsUpdateVoucherListUsingTemplate.aspx", "Bulk Update", "<i class=\"fas fa-database\"></i>", "BULKUPDATE");
        MenuExcelUpload.AccessRights = this.ViewState;
        MenuExcelUpload.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ddlvessellistbind();
        }

    }
    private void ddlvessellistbind()
    {
        DataSet ds = PhoenixAccountsVoucherList.ListVesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        //ddlAccountDetails.Items.Add("select");
        ddlaccount.DataSource = ds;
        ddlaccount.DataTextField = "FLDDESCRIPTION";
        ddlaccount.DataValueField = "FLDACCOUNTID";
        ddlaccount.DataBind();
        ddlaccount.Items.Insert(0, new DropDownListItem("--Select--", ""));

    }
    protected void MenuExcelUpload_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BULKUPDATE"))
            {

                string from = ucFromDate.Text;
                string To = ucToDate.Text;
                string principal = ucPrincipal.SelectedAddress;
                string vessel = ddlaccount.SelectedValue;

                if (!IsValidData(from, To, principal, vessel))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {


                    DataSet ds = new DataSet();
                    ds = PhoenixAccountsVoucherList.ExchangeRateUpdateNotBalancedVoucherList(General.GetNullableInteger(principal.ToString())
                                                                              , General.GetNullableInteger(ddlaccount.ToString())
                                                                              , General.GetNullableDateTime(ucFromDate.Text)
                                                                              , General.GetNullableDateTime(ucToDate.Text));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int n = ds.Tables[0].Rows.Count;
                        while (n > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[n - 1];
                            Message += dr["FLDVOUCHERNUMBER"].ToString() + "<br>";
                            n--;
                        }
                        RadWindowManager1.RadConfirm("The following Vouchers are Not Balanced.<br>" + Message + "Do you want to continue.?<br>" + " Note : Not balanced voucher will not be part of downloaded file.<br><br>", "confirm ", 420, 150, null, "Confirm Message");
                    }
                    //RadWindowManager1.RadConfirm("The following Vouchers are Not Balanced.<br>" + Message + "Do you want to continue.?<br>" + " Note : Not balanced voucher will not be part of downloaded file.<br><br>", "confirm ", 420, 150, null, "Confirm Message");
                    else
                    {
                        UpdateExcelDownload();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateExcelDownload();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidData(string fromdate, string todate, string principal, string vessel)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than from date";
        }
        if (string.IsNullOrEmpty(principal) && string.IsNullOrEmpty(vessel))
        {
            ucError.ErrorMessage = "Either Principal or vesselis required";
        }

        string fromInString = ucFromDate.Text;
        string toInString = ucToDate.Text;

        DateTime startDate = DateTime.Parse(fromInString);
        DateTime totDate = DateTime.Parse(toInString);
        DateTime maxDate = startDate.AddDays(62);

        if (totDate > maxDate)
            ucError.ErrorMessage = "From /To date cannotbe more than 62 days apart.";

        return (!ucError.IsError);
    }
    protected void UpdateExcelDownload()
    {
        try
        {
            PhoenixAccounts2XL.Export2XLExchangeRateUpdate(General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                                            , General.GetNullableInteger(ddlaccount.SelectedValue)
                                                            , General.GetNullableDateTime(ucFromDate.Text)
                                                            , General.GetNullableDateTime(ucToDate.Text));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        if (ucPrincipal.SelectedText == "" || ucPrincipal.SelectedText == "--Select--")
            ddlaccount.Enabled = true;
        else
            ddlaccount.Enabled = false;

    }

    protected void ddlaccount_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        if (ddlaccount.SelectedText == "--Select--")
            ucPrincipal.Enabled = true;
        else
            ucPrincipal.Enabled = false;

    }
}
