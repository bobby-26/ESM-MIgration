using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class VesselAccounts_VesselAccountEarningsDeductionsEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString();

            ViewState["Employeeid"] = null;
            if (Request.QueryString["empid"] != null)
                ViewState["Employeeid"] = Request.QueryString["empid"].ToString();
            if (Request.QueryString["VESSELID"] != null)
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            if (Request.QueryString["signonof"] != null)
                ViewState["Signonofid"] = Request.QueryString["signonof"].ToString();
            if (Request.QueryString["type"] != null)
                ViewState["type"] = Request.QueryString["type"].ToString();
            if (Request.QueryString["month"] != null)
                ViewState["Month"] = Request.QueryString["month"].ToString();
            if (Request.QueryString["year"] != null)
                ViewState["Year"] = Request.QueryString["year"].ToString();
            ViewState["CURRENCYCHANGE"] = "";
            BindData(0);

        }     /*binddetails();*/
    }
    private void BindData(int bowyn)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBoardEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                        , General.GetNullableInteger(ViewState["Employeeid"].ToString())
                                        , General.GetNullableInteger(ViewState["Signonofid"].ToString())
                                        , General.GetNullableDateTime(txtDate.Text)
                                        , General.GetNullableInteger(ViewState["type"].ToString()), int.Parse(ViewState["Month"].ToString())
                                        , int.Parse(ViewState["Year"].ToString()), bowyn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtname.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtfileno.Text = dr["FLDFILENO"].ToString();
                txtrank.Text = dr["FLDRANKCODE"].ToString();
                if (dr["FLDMONTH"].ToString() != string.Empty && dr["FLDYEAR"].ToString() != string.Empty)
                    txtMonthAndYear.Text = DateTime.Parse("01/" + dr["FLDMONTH"].ToString() + "/" + dr["FLDYEAR"].ToString()).ToString("MMM") + "-" + dr["FLDYEAR"].ToString();
                txtwage.Text = dr["FLDBALANCEWAGE"].ToString();
                txtcashonboard.Text = dr["FLDCASHBOARD"].ToString();
                ViewState["VESSELCURRENCY"] = dr["FLDVESSELCURRENCY"].ToString();
                ViewState["Confirm"] = dr["FLDCONFIRMEDYN"].ToString();
                ViewState["Eardedid"] = dr["FLDEARNINGDEDUCTIONID"].ToString();
                ViewState["REQUESTEDDATE"] = dr["FLDREQUESTEDDATE"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void binddetails()
    {

        DataSet dst = new DataSet();

        dst = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBoardEditlist(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                              , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                              , General.GetNullableInteger(ViewState["Employeeid"].ToString())
                              , General.GetNullableInteger(ViewState["Signonofid"].ToString())
                              , General.GetNullableInteger(ViewState["Month"].ToString())
                              , General.GetNullableInteger(ViewState["Year"].ToString())
                              , General.GetNullableInteger(ViewState["type"].ToString()));
        showtitle();
        if (ViewState["type"].ToString() == "3" || ViewState["type"].ToString() == "5")
        {
            ViewState["RECORDYN"] = "1";
            gvCrewSearch.DataSource = dst;
            gvCrewSearch.VirtualItemCount = dst.Tables[0].Rows.Count;
            gvAllotment.Visible = false;
        }
        else if (ViewState["type"].ToString() == "4" || ViewState["type"].ToString() == "7")
        {
            ViewState["RECORDYN"] = "1";
            gvAllotment.DataSource = dst;
            gvAllotment.VirtualItemCount = dst.Tables[0].Rows.Count;
            gvCrewSearch.Visible = false;
        }
        else
        {
            DataTable dt = dst.Tables[0];
            if (ViewState["type"].ToString() == "3" || ViewState["type"].ToString() == "5")
            {
                ViewState["RECORDYN"] = "0";
            }
            if (ViewState["type"].ToString() == "4" || ViewState["type"].ToString() == "7")
            {
                ViewState["RECORDYN"] = "0";
            }
        }

    }
    public void showtitle()
    {
        if (ViewState["type"].ToString() == "3")
        {
            MenuSave.Title = "Cash Advance";
        }
        if (ViewState["type"].ToString() == "5")
        {
            MenuSave.Title = "Radio Log";
        }
        if (ViewState["type"].ToString() == "4")
        {
            MenuSave.Title = "Allotment";
        }
        if (ViewState["type"].ToString() == "7")
        {
            MenuSave.Title = "Special Allotment";
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuSave.AccessRights = this.ViewState;
        MenuSave.MenuList = toolbar.Show();
    }
    private bool IsValidEarningDeduction(string amount, string purpose, string date)
    {
        string month = ViewState["Month"].ToString();
        string year = ViewState["Year"].ToString();
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1")) < 0)
        {
            ucError.ErrorMessage = "Date should be later than 1/" + month + "/" + year;
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1)) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than " + string.Format("{0:dd/MM/yyyy}", DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1));
        }
        if (string.IsNullOrEmpty(purpose))
        {
            ucError.ErrorMessage = "Purpose is required.";
        }
        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        if (General.GetNullableInteger(amount.ToString()) <= 0)
        {
            ucError.ErrorMessage = "Amount should be greater than 0";
        }

        return (!ucError.IsError);
    }
    private bool IsValidEarningDeductionValidation(string amount, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(purpose))
        {
            ucError.ErrorMessage = "Purpose is required.";
        }

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        if (General.GetNullableInteger(amount.ToString()) <= 0)
        {
            ucError.ErrorMessage = "Amount should be grater than 0.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidEarningsDeductions(string amount, string purpose, string bankacc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(purpose))
        {
            ucError.ErrorMessage = "Purpose is required.";
        }

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        else if (General.GetNullableDecimal(amount) <= 0)
        {
            ucError.ErrorMessage = "Amount shoud be grater than 0.";
        }

        if (ViewState["type"].ToString() == "4" || ViewState["type"].ToString() == "7")
        {
            if (General.GetNullableGuid(bankacc) == null)
                ucError.ErrorMessage = "Bank account is required.";
        }
        return (!ucError.IsError);
    }
    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                string type = ViewState["type"].ToString();
                string month = ViewState["Month"].ToString();
                string year = ViewState["Year"].ToString();
                Guid? id = General.GetNullableGuid(ViewState["Eardedid"].ToString());

                if (id != null)
                {
                    PhoenixVesselAccountsEarningDeduction.AllotmentConfirmation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , id
                        , int.Parse(month)
                        , int.Parse(year)
                        , int.Parse(type));
                    ucStatus.Text = "Allotment Confirmed";
                    MenuSave.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void RebindCrewSearch()
    {
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }

    private void RebindAllotment()
    {
        gvAllotment.EditIndexes.Clear();
        gvAllotment.SelectedIndexes.Clear();
        gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }
    protected void cmdCalculateWage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BindData(1);
            binddetails();
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
            GridItem row = (GridItem)ddl.Parent.Parent;

            UserControlVesselMappingCurrency Currency = row.FindControl(row is GridFooterItem ? "ddlCurrencyAdd" : "ddlCurrencyEdit") as UserControlVesselMappingCurrency;
            UserControlMaskNumber txtExchangeRate = row.FindControl(row is GridFooterItem ? "txtExchangeRateAdd" : "txtExchangeRateEdit") as UserControlMaskNumber;
            UserControlDate txtdate = row.FindControl(row is GridFooterItem ? "txtDateadd" : "txtDateedit") as UserControlDate;

            if (string.IsNullOrEmpty(txtdate.Text))
            {
                ucError.ErrorMessage = "Date is required.";
                ucError.Visible = true;
                return;
            }
            ViewState["CURRENCYCHANGE"] = Currency.SelectedCurrency;
            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(Currency.SelectedCurrency)
                                                                        , DateTime.Parse(txtdate.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        binddetails();
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
                if (de != null)
                {
                    de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                    de.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
                UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyEdit"));
                if (ddlCurrency != null)
                {

                    ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                    ddlCurrency.SelectedCurrency = drv["FLDBASECURRENCY"].ToString();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    UserControlMaskNumber txtExchangeRate = (UserControlMaskNumber)e.Item.FindControl("txtExchangeRateEdit");
                    if (txtExchangeRate != null)
                    {
                        txtExchangeRate.CssClass = "readonlytextbox txtNumber";
                        txtExchangeRate.ReadOnly = true;
                    }
                }
            }
            if (e.Item is GridFooterItem)
            {
                UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyAdd"));
                if (ddlCurrency != null)
                {

                    ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                    ddlCurrency.VesselId = ViewState["VESSELID"].ToString();
                    ddlCurrency.SelectedCurrency = ViewState["CURRENCYCHANGE"].ToString() != string.Empty ? ViewState["CURRENCYCHANGE"].ToString() : ViewState["VESSELCURRENCY"].ToString();
                }
                UserControlMaskNumber exchangerate = (UserControlMaskNumber)e.Item.FindControl("txtExchangeRateAdd");
                if (exchangerate != null)
                {
                    exchangerate.Text = "1";
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    {
                        exchangerate.CssClass = "readonlytextbox txtNumber";
                        exchangerate.ReadOnly = true;
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

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "ADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                string employee = ViewState["Employeeid"].ToString();
                string type = ViewState["type"].ToString();
                string purpose = ((RadTextBox)footer.FindControl("txtpurposeadd")).Text;
                string amount = ((UserControlMaskNumber)footer.FindControl("txtAmountadd")).Text;
                string date = ((UserControlDate)footer.FindControl("txtDateadd")).Text;
                string Currency = ((UserControlVesselMappingCurrency)footer.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string ExchangeRate = ((UserControlMaskNumber)footer.FindControl("txtExchangeRateAdd")).Text;
                string signonoffid = ViewState["Signonofid"].ToString();
                string month = ViewState["Month"].ToString();
                string year = ViewState["Year"].ToString();

                if (!IsValidEarningDeduction(amount, purpose, date))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonVesselAccounts.VesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                            byte.Parse(type),
                            int.Parse(employee),
                            null,
                            null,
                            byte.Parse(month),
                            int.Parse(year),
                            -1,
                            General.GetNullableDateTime(date),
                            decimal.Parse(amount),
                            purpose,
                            null,
                            int.Parse(signonoffid), decimal.Parse(ExchangeRate), int.Parse(ViewState["VESSELCURRENCY"].ToString()), int.Parse(Currency));

                ucStatus.Text = "Added successfully";
                ViewState["CURRENCYCHANGE"] = "";
                RebindCrewSearch();
            }

            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridEditableItem item = (GridEditableItem)e.Item;

                string employeeid = ViewState["Employeeid"].ToString();
                string type = ViewState["type"].ToString();
                string month = ViewState["Month"].ToString();
                string year = ViewState["Year"].ToString();
                string purpose = ((RadTextBox)item.FindControl("txtpurposeEdit")).Text;
                string amount = ((UserControlMaskNumber)item.FindControl("txtAmountEdit")).Text;
                string date = ((UserControlDate)item.FindControl("txtDateedit")).Text;
                string earningdedid = ((RadLabel)item.FindControl("lblEarDedEditId")).Text;
                string signonoffid = ViewState["Signonofid"].ToString();
                string Currency = ((UserControlVesselMappingCurrency)item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
                string ExchangeRate = ((UserControlMaskNumber)item.FindControl("txtExchangeRateEdit")).Text;

                if (!IsValidEarningDeduction(amount, purpose, date))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionUpdate(General.GetNullableGuid(earningdedid),
                                                         int.Parse(employeeid),
                                                         null,
                                                         int.Parse(month),
                                                         int.Parse(year),
                                                         General.GetNullableDateTime(date),
                                                         null,
                                                         -1,
                                                         decimal.Parse(amount),
                                                         purpose,
                                                         int.Parse(signonoffid), decimal.Parse(ExchangeRate), int.Parse(ViewState["VESSELCURRENCY"].ToString()), int.Parse(ViewState["CURRENCYCHANGE"].ToString() != string.Empty ? ViewState["CURRENCYCHANGE"].ToString() : Currency));

                ucStatus.Text = "Cash Advance Updated.";

                gvCrewSearch.EditIndexes.Clear();
                ViewState["CURRENCYCHANGE"] = "";
                RebindCrewSearch();

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = ((RadLabel)item.FindControl("lblEarDedId")).Text;
                if (id != "")
                    PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionDelete(new Guid(id));
                ucStatus.Text = "Deleted Successfully";
                RebindCrewSearch();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        binddetails();
    }

    protected void gvAllotment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            UserControlEmployeeBankAccount ddlBankAccount = (UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccountEdit");

            if (ddlBankAccount != null) ddlBankAccount.SelectedBankAccount = drv["FLDBANKACCOUNTID"].ToString();

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }

            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
            if (cmdAttachment != null)
            {
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdAttachment.Controls.Add(html);
                }

                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + " &cmdname=DCEUPLOAD'); return false;");
            }
            LinkButton confirm = (LinkButton)e.Item.FindControl("cmdConfirm");
            LinkButton edbtn = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton debtn = (LinkButton)e.Item.FindControl("cmdDelete");
            if (confirm != null && edbtn != null && debtn != null)
            {
                if (drv["FLDCONFIRMEDYN"].ToString() != "1")
                {
                    confirm.Visible = SessionUtil.CanAccess(this.ViewState, confirm.CommandName);
                    confirm.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm?')");
                    edbtn.Visible = SessionUtil.CanAccess(this.ViewState, edbtn.CommandName);
                    debtn.Visible = SessionUtil.CanAccess(this.ViewState, debtn.CommandName);
                }
                else
                {
                    confirm.Visible = false;
                    edbtn.Visible = false;
                    debtn.Visible = false;
                }

                if (General.GetNullableGuid(drv["FLDEARNINGDEDUCTIONID"].ToString()) == null)
                {
                    confirm.Visible = false;
                    debtn.Visible = false;
                    cmdAttachment.Visible = false;
                    edbtn.Visible = SessionUtil.CanAccess(this.ViewState, edbtn.CommandName);
                }
            }
            UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyEdit"));
            if (ddlCurrency != null)
            {

                ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                ddlCurrency.SelectedCurrency = drv["FLDBASECURRENCY"].ToString();
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                UserControlMaskNumber txtExchangeRate = (UserControlMaskNumber)e.Item.FindControl("txtExchangeRateEdit");
                if (txtExchangeRate != null)
                {
                    txtExchangeRate.CssClass = "readonlytextbox txtNumber";
                    txtExchangeRate.ReadOnly = true;
                }
            }

        }

        if (e.Item is GridFooterItem)
        {
            UserControlEmployeeBankAccount ddlBankAccount = (UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccount");

            ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(int.Parse(ViewState["VESSELID"].ToString())
                                                                                                              , General.GetNullableInteger(ViewState["Employeeid"].ToString()));
            ddlBankAccount.EmployeeId = ViewState["Employeeid"].ToString();

            UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrencyAdd"));
            if (ddlCurrency != null)
            {

                ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                ddlCurrency.VesselId = ViewState["VESSELID"].ToString();
                ddlCurrency.SelectedCurrency = ViewState["CURRENCYCHANGE"].ToString() != string.Empty ? ViewState["CURRENCYCHANGE"].ToString() : ViewState["VESSELCURRENCY"].ToString();
            }
            UserControlDate Date = (UserControlDate)e.Item.FindControl("txtDateadd");
            if (Date != null)
            {
                Date.Text = ViewState["REQUESTEDDATE"].ToString();
            }
            UserControlMaskNumber exchangerate = (UserControlMaskNumber)e.Item.FindControl("txtExchangeRateAdd");
            if (exchangerate != null)
            {
                exchangerate.Text = "1";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    exchangerate.CssClass = "readonlytextbox txtNumber";
                    exchangerate.ReadOnly = true;
                }
            }

        }
    }

    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "ADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;

                string employee = ViewState["Employeeid"].ToString();
                string type = ViewState["type"].ToString();
                string purpose = ((RadTextBox)footer.FindControl("txtpurposeadd")).Text;
                string amount = ((UserControlMaskNumber)footer.FindControl("txtAmountadd")).Text;
                string signonoffid = ViewState["Signonofid"].ToString();
                string bank = ((UserControlEmployeeBankAccount)footer.FindControl("ddlBankAccount")).SelectedBankAccount;
                string month = ViewState["Month"].ToString();
                string year = ViewState["Year"].ToString();
                string date = ((UserControlDate)footer.FindControl("txtDateadd")).Text;
                string Currency = ((UserControlVesselMappingCurrency)footer.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string ExchangeRate = ((UserControlMaskNumber)footer.FindControl("txtExchangeRateAdd")).Text;

                if (!IsValidEarningsDeductions(amount, purpose, bank))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonVesselAccounts.VesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                               , byte.Parse(type)
                               , int.Parse(employee)
                               , General.GetNullableGuid(bank)
                               , null
                               , byte.Parse(month)
                               , int.Parse(year)
                               , -1
                               , General.GetNullableDateTime(date)
                               , decimal.Parse(amount)
                               , purpose
                               , null
                               , int.Parse(signonoffid), decimal.Parse(ExchangeRate), int.Parse(ViewState["VESSELCURRENCY"].ToString()), int.Parse(Currency));

                if (ViewState["type"].ToString() == "4")
                    ucStatus.Text = "Allotment added successfully.";
                else
                    ucStatus.Text = "Special Allotment added successfully.";
                ViewState["CURRENCYCHANGE"] = "";
                RebindAllotment();
            }

            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                string employeeid = ViewState["Employeeid"].ToString();
                string type = ViewState["type"].ToString();
                string month = ViewState["Month"].ToString();
                string year = ViewState["Year"].ToString();
                string purpose = ((RadTextBox)item.FindControl("txtpurposeEdit")).Text;
                string amount = ((UserControlMaskNumber)item.FindControl("txtAmountEdit")).Text;
                string bank = ((UserControlEmployeeBankAccount)item.FindControl("ddlBankAccountEdit")).SelectedBankAccount;
                string earningdedid = ((RadLabel)item.FindControl("lblEarDedAllotmentEditId")).Text;
                string signonoffid = ViewState["Signonofid"].ToString();
                string date = ((UserControlDate)item.FindControl("txtDateedit")).Text;
                string Currency = ((UserControlVesselMappingCurrency)item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
                string ExchangeRate = ((UserControlMaskNumber)item.FindControl("txtExchangeRateEdit")).Text;

                if (!IsValidEarningsDeductions(amount, purpose, bank))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionUpdate(General.GetNullableGuid(earningdedid),
                                                  int.Parse(employeeid),
                                                  General.GetNullableGuid(bank),
                                                  int.Parse(month),
                                                  int.Parse(year),
                                                  General.GetNullableDateTime(date),
                                                  null,
                                                  -1,
                                                  decimal.Parse(amount),
                                                  purpose,
                                                  int.Parse(signonoffid), decimal.Parse(ExchangeRate), int.Parse(ViewState["VESSELCURRENCY"].ToString()), int.Parse(ViewState["CURRENCYCHANGE"].ToString() != string.Empty ? ViewState["CURRENCYCHANGE"].ToString() : Currency));

                if (ViewState["type"].ToString() == "4")
                    ucStatus.Text = "Allotment details Updated.";
                else
                    ucStatus.Text = "Special Allotment details Updated.";

                gvAllotment.EditIndexes.Clear();
                ViewState["CURRENCYCHANGE"] = "";
                RebindAllotment();
            }
            if (e.CommandName.ToUpper() == "DELETE")
            {
                PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionDelete(new Guid(ViewState["Eardedid"].ToString()));
                ucStatus.Text = "Deleted Successfully";
                RebindAllotment();

            }
            if (e.CommandName.ToUpper() == "CONFIRM")
            {
                GridEditableItem item = (GridEditableItem)e.Item;

                string type = Request.QueryString["type"].ToString();
                string month = Request.QueryString["month"].ToString();
                string year = Request.QueryString["year"].ToString();
                string id = ((RadLabel)item.FindControl("lblEarDedAllotmentId")).Text;

                if (id != null)
                    PhoenixVesselAccountsEarningDeduction.AllotmentConfirmation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableGuid(id)
                        , int.Parse(month)
                        , int.Parse(year)
                        , int.Parse(type));
                ucStatus.Text = "Allotment Confirmed";
                RebindAllotment();
            }
            if (e.CommandName.ToUpper() == "ATTACHMENT")
            {
                GridEditableItem item = (GridEditableItem)e.Item;

                string Dtkey = ((RadLabel)item.FindControl("lbldtkey")).Text; ;
                if (General.GetNullableGuid(Dtkey) == null)
                {
                    ucError.ErrorMessage = "Kindly update the amount before uploading the attachment.";
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}