using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsAdminEarningDeduction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                CreateMenu();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["BRF"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "BRF");
                ViewState["OTA"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "OTA");
                ViewState["OTD"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "OTD");
                ViewState["REM"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "REM");
                ViewState["CRR"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "CRR");
                ddlType.SelectedValue = "1";
                ddlYear.SelectedYear = DateTime.Now.Year;
                ddlMonth.SelectedMonth = DateTime.Now.Month.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                ViewState["CURRENCYSEELECTEDYN"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = new DataTable();
                if (ddlType.SelectedValue == "1")
                {
                    string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDHARDNAME", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDMONTHYEAR" };
                    string[] alCaptions = { "Employee Code", "Employee Name", "Rank", "Entry Type", "Purpose", "Currency", "Amount", "Date" };

                    dt = PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionOnboardSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1 //Onboard Earning/Deduction
                        , (short?)General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()), General.GetNullableInteger(ddlEmployee.SelectedEmployee)
                        , 1, sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
                    General.ShowExcel(ddlType.SelectedItem.Text, dt, alColumns, alCaptions, sortdirection, sortexpression);
                }
                else
                {
                    string[] alColumns = null;
                    string[] alCaptions = null;
                    if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
                    {
                        alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDDATE" };
                        alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Date" };
                    }
                    else
                    {
                        alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDACCOUNTNUMBER", "FLDDATE" };
                        alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Bank Account/Beneficiary Name", "Date" };
                    }
                    dt = PhoenixVesselAccountsEarningDeduction.AdminVesselEarningDeductionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(ddlType.SelectedValue)
                       , (short?)General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()), General.GetNullableInteger(ddlEmployee.SelectedEmployee)
                       , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
                    General.ShowExcel(ddlType.SelectedItem.Text, dt, alColumns, alCaptions, sortdirection, sortexpression);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int vesselcurrency = 0;
            decimal Exchagerate = 0;
            UserControlVesselMappingCurrency ddl = (UserControlVesselMappingCurrency)sender;

            GridDataItem row = (GridDataItem)ddl.Parent.Parent;
            UserControlVesselMappingCurrency Currency = row.FindControl("ddlCurrencyEdit") as UserControlVesselMappingCurrency;
            UserControlMaskNumber txtExchangeRate = row.FindControl("txtExchangeRateEdit") as UserControlMaskNumber;
            UserControlDate txtdate = row.FindControl("txtDate") as UserControlDate;

            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(Currency.SelectedCurrency)
                                                                        , DateTime.Parse(txtdate.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["CURRENCYSEELECTEDYN"] = Currency.SelectedCurrency;
                txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = new DataTable();
            if (ddlType.SelectedValue == "1")
            {
                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDHARDNAME", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDMONTHYEAR" };
                string[] alCaptions = { "Employee Code", "Employee Name", "Rank", "Entry Type", "Purpose", "Currency", "Amount", "Date" };

                dt = PhoenixVesselAccountsEarningDeduction.AdminVesselEarningDeductionOnboardSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1 //Onboard Earning/Deduction
                    , (short?)General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()), General.GetNullableInteger(ddlEmployee.SelectedEmployee)
                    , 1, sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize, ref iRowCount, ref iTotalPageCount);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions("gvCrewSearch", ddlType.SelectedItem.Text, alCaptions, alColumns, ds);
            }
            else
            {
                string[] alColumns = null;
                string[] alCaptions = null;

                if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
                {
                    alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDDATE" };
                    alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Date" };
                }
                else
                {
                    alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDACCOUNTNUMBER", "FLDMONTHYEAR" };
                    alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Bank Account/Beneficiary Name", "Date" };
                }

                dt = PhoenixVesselAccountsEarningDeduction.AdminVesselEarningDeductionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(ddlType.SelectedValue)
                   , (short?)General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()), General.GetNullableInteger(ddlEmployee.SelectedEmployee)
                   , sortexpression, sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , gvCrewSearch.PageSize, ref iRowCount, ref iTotalPageCount);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions("gvCrewSearch", ddlType.SelectedItem.Text, alCaptions, alColumns, ds);
            }
            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidEarningDeduction(string employeeid, string month, string year, string type, string amount, string purpose, string date, string wageheadid, string BankAccount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(employeeid).HasValue)
            ucError.ErrorMessage = "Employee is required.";
        if (!General.GetNullableInteger(month).HasValue)
            ucError.ErrorMessage = "Month is required.";
        if (!General.GetNullableInteger(year).HasValue)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableInteger(month).HasValue && General.GetNullableInteger(year).HasValue
            && DateTime.Compare(General.GetNullableDateTime(year + "/" + month + "/01").Value, General.GetNullableDateTime(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").Value) > 0)
            ucError.ErrorMessage = "Month and Year should be earlier than current Month and Year";
        string ota = "";
        string otd = "";
        ota = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "OTA");
        otd = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "OTD");

        if (((wageheadid.ToString().Equals(ota) || wageheadid.ToString().Equals(otd)) && (ddlType.SelectedValue.ToString().Equals("1"))) || (!ddlType.SelectedValue.ToString().Equals("1")))
        {
            if (string.IsNullOrEmpty(purpose))
                ucError.ErrorMessage = "Purpose is required.";
        }

        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";
        else if (decimal.Parse(amount) < 0)
            ucError.ErrorMessage = "Amount is positive integer field";

        if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
        {
            if (!General.GetNullableDateTime(date).HasValue)
                ucError.ErrorMessage = "Date is required.";
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
                ucError.ErrorMessage = "Date should be earlier than current date";
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1")) < 0)
                ucError.ErrorMessage = "Date should be later than 1/" + month + "/" + year;
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1)) > 0)
                ucError.ErrorMessage = "Date should be earlier than " + string.Format("{0:dd/MM/yyyy}", DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1));
        }
        if (ddlType.SelectedValue == "4" || ddlType.SelectedValue == "7" || ddlType.SelectedValue == "8")
        {
            if (!General.GetNullableGuid(BankAccount).HasValue)
                ucError.ErrorMessage = "Bank Account is required.";
        }
        return (!ucError.IsError);
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsAdminEarningDeduction.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();

    }

    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // Rebind();
            gvCrewSearch.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        //if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        //{
        //    NameValueCollection nvc = (NameValueCollection)ViewState["UPDATEVALUES"];

        //    string id = nvc["id"].ToString();
        //    string employee = nvc["employee"].ToString();
        //    string type = nvc["type"].ToString();
        //    string month = nvc["month"].ToString();
        //    string year = nvc["year"].ToString();
        //    string wageheadid = General.GetNullableString(nvc["wageheadid"]);
        //    string purpose = nvc["purpose"].ToString();
        //    string amount = nvc["amount"].ToString();
        //    string date = General.GetNullableString(nvc["date"]);
        //    string accit = General.GetNullableString(nvc["accit"]);
        //    string earningordeduction = nvc["earningordeduction"].ToString();
        //    string signonoffid = nvc["signonoffid"].ToString();

        //    UpdategvAmount(new Guid(id.ToString()), int.Parse(employee), General.GetNullableGuid(accit), General.GetNullableInteger(wageheadid)
        //        , byte.Parse(month), int.Parse(year), General.GetNullableDateTime(date), int.Parse(earningordeduction), decimal.Parse(amount), purpose, int.Parse(signonoffid));
        //    ViewState["UPDATEVALUES"] = null;
        //}
        //Rebind();
    }



    protected void gvCrewSearch_PreRender(object sender, EventArgs e)
    {


        if (ddlType.SelectedValue == "1")
        {
            gvCrewSearch.MasterTableView.GetColumn("Date").Display = false;
            gvCrewSearch.MasterTableView.GetColumn("Bank").Display = false;
        }
        if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
        {
            gvCrewSearch.MasterTableView.GetColumn("wage").Display = false;
            gvCrewSearch.MasterTableView.GetColumn("Bank").Display = false;
            gvCrewSearch.MasterTableView.GetColumn("MonthYear").Display = false;
        }
        else if (ddlType.SelectedValue == "4" || ddlType.SelectedValue == "7" || ddlType.SelectedValue == "8")
        {
            gvCrewSearch.MasterTableView.GetColumn("wage").Display = false;
            gvCrewSearch.MasterTableView.GetColumn("Date").Display = false;
        }
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            Guid id = new Guid(((RadLabel)e.Item.FindControl("lblEDid")).Text);
            string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text; ;
            string type = ddlType.SelectedValue;
            string month = ddlMonth.SelectedMonth;
            int year = ddlYear.SelectedYear;
            string wageheadid = ((RadLabel)e.Item.FindControl("lblWageHeadId")).Text;
            string lpurpose = ((RadLabel)e.Item.FindControl("lblPurpose")).Text;
            string lamount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;
            string lblvesselCurrencyid = ((RadLabel)e.Item.FindControl("lblvesselCurrencyid")).Text;
            string purpose = ((RadTextBox)e.Item.FindControl("txtPurposeEdit")).Text;
            string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
            string date = ((UserControlDate)e.Item.FindControl("txtDate")).Text;
            string accit = ((UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccount")).SelectedBankAccount;
            string signonoffid = ((RadLabel)e.Item.FindControl("lblsignonoffid")).Text;
            string earningordeduction = ((RadLabel)e.Item.FindControl("lblED")).Text;
            string Currency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
            string ExchangeRate = ((UserControlMaskNumber)e.Item.FindControl("txtExchangeRateEdit")).Text;
            if (!IsValidEarningDeduction(employeeid, month, year.ToString(), type, amount, purpose, date, wageheadid, accit))
            {
                ucError.Visible = true;
                return;
            }
            //  if (lamount != amount || lpurpose != purpose)
            {
                PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionUpdate(id, int.Parse(employeeid), General.GetNullableGuid(accit)
                  , byte.Parse(month), year, General.GetNullableDateTime(date), General.GetNullableInteger(wageheadid), int.Parse(earningordeduction)
                  , decimal.Parse(amount), purpose, int.Parse(signonoffid), decimal.Parse(ExchangeRate), int.Parse(lblvesselCurrencyid), int.Parse(Currency));

            }
            ViewState["CURRENCYSEELECTEDYN"] = null;
            Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            string id = ((RadLabel)e.Item.FindControl("lblEDid")).Text;
            PhoenixVesselAccountsCorrections.VesselAdminEarningDeductionDelete(new Guid(id));
            Rebind();
        }
        else if (e.CommandName.ToUpper() == "CONFIRM")
        {
            string id = ((RadLabel)e.Item.FindControl("lblEDid")).Text;
            string type = ddlType.SelectedValue;
            string month = ddlMonth.SelectedMonth;
            int year = ddlYear.SelectedYear;
            PhoenixVesselAccountsEarningDeduction.AllotmentConfirmation(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(id)
                , int.Parse(month), year, int.Parse(type));
            Rebind();
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblwageheadid = (RadLabel)e.Item.FindControl("lblWageHeadId");
            RadTextBox purpse = (RadTextBox)e.Item.FindControl("txtPurposeEdit");
            if (lblwageheadid != null && ddlType.SelectedValue.ToString().Equals("1") && !lblwageheadid.Text.Equals(ViewState["OTA"].ToString()) && !lblwageheadid.Text.Equals(ViewState["OTD"].ToString()))
            {

                if (purpse != null) purpse.CssClass = "gridinput";
            }
            if (lblwageheadid != null && ((ddlType.SelectedValue.ToString().Equals("1") && lblwageheadid.Text.Equals(ViewState["BRF"].ToString()) && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                || lblwageheadid.Text.Equals(ViewState["REM"].ToString()) || lblwageheadid.Text.Equals(ViewState["CRR"].ToString())))
            {

                if (purpse != null)
                {
                    purpse.CssClass = "gridinput readonlytextbox";
                    purpse.Enabled = false;
                    purpse.ReadOnly = true;
                }
                UserControlMaskNumber amt = (UserControlMaskNumber)e.Item.FindControl("txtAmountEdit");
                if (amt != null)
                {
                    amt.CssClass = "readonlytextbox txtNumber"; amt.Enabled = false;
                    amt.ReadOnly = true;
                }
            }
            if (lblwageheadid != null && ((ddlType.SelectedValue.ToString().Equals("1") && lblwageheadid.Text.Equals(ViewState["CRR"].ToString()))))
            {
                if (purpse != null)
                {
                    purpse.CssClass = "gridinput readonlytextbox"; purpse.Enabled = false;
                    purpse.ReadOnly = true;
                }
                UserControlMaskNumber amt = (UserControlMaskNumber)e.Item.FindControl("txtAmountEdit");
                if (amt != null)
                {
                    amt.CssClass = "readonlytextbox txtNumber"; amt.Enabled = false;
                    amt.ReadOnly = true;
                }
            }
            LinkButton confirm = (LinkButton)e.Item.FindControl("cmdConfirm");
            if (ddlType.SelectedValue == "4" || ddlType.SelectedValue == "7" || ddlType.SelectedValue == "8")
            {

                if (confirm != null)
                {
                    confirm.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm?')");
                    confirm.Visible = SessionUtil.CanAccess(this.ViewState, confirm.CommandName);
                    if (drv["FLDCONFIRMEDYN"].ToString() != "1")
                        confirm.Visible = true;
                    else
                        confirm.Visible = false;
                }
            }
            else
            {
                if (confirm != null)
                {
                    confirm.Visible = false;
                }

            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            UserControlMaskNumber txtamount = (UserControlMaskNumber)e.Item.FindControl("txtAmountEdit");
            UserControlEmployeeBankAccount ba = (UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccount");
            if (ba != null) ba.SelectedBankAccount = drv["FLDBANKACCOUNTID"].ToString();
            UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyEdit"));
            if (ddlCurrency != null)
            {
                // ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                ddlCurrency.SelectedCurrency = ViewState["CURRENCYSEELECTEDYN"] != null ? ViewState["CURRENCYSEELECTEDYN"].ToString() : drv["FLDBASECURRENCY"].ToString();
            }
        }
    }
}
