using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewLicencePaymentRequestsApprove : PhoenixBasePage
{
    decimal sum = 0;
    string advancepaymentid = ",";
    decimal? depositamount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Proceed", "PROCEED");
            toolbarmain.AddButton("Cancel", "CANCEL");
            MenuLicenceStatus.AccessRights = this.ViewState;
            MenuLicenceStatus.MenuList = toolbarmain.Show();

            ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();
            //ViewState["AMOUNTSUM"] = Request.QueryString["AMOUNTSUM"].ToString();
            ViewState["CURRENCY"] = Request.QueryString["CURRENCY"].ToString();
            ViewState["PAYMENTONRECVINV"] = Request.QueryString["RECEIVEINVOICE"].ToString();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            ProcessEdit();
            
            if (!IsPostBack)
            {
                AddressEdit();
                ucTitle.Text = "You are approving " + ViewState["COUNT"].ToString() + " requests for " + ViewState["SUPPLIERNAME"].ToString();
            }

            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProcessEdit()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewLicencePaymentRequests.CrewLicenceRequestSearch(
                                                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , sortexpression
                                                          , sortdirection
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , General.ShowRecords(null)
                                                          , ref iRowCount
                                                          , ref iTotalPageCount
                                                          , General.GetNullableString(Filter.CurrentLicenceSelectedForPayment)
                                                          , General.GetNullableInteger(null)
                                                          , General.GetNullableInteger(null)
                                                          , General.GetNullableInteger(null)
                                                          );

        ViewState["COUNT"] = ds.Tables[0].Rows.Count;
        ViewState["SUPPLIERNAME"] = ds.Tables[0].Rows[0]["FLDSUPPLIERNAME"].ToString();
        ViewState["BILLTOCOMAPNYID"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();

        sum = 0;
        advancepaymentid = ",";
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sum = sum + decimal.Parse(ds.Tables[0].Rows[i]["FLDAMOUNT"].ToString());
            advancepaymentid = advancepaymentid + ds.Tables[0].Rows[i]["FLDADVANCEPAYMENTID"].ToString() + ",";
        }
        
    }

    protected void AddressEdit()
    {
        try
        {
            DataSet dsaddress = PhoenixCrewLicencePaymentRequests.ConsulateAmountEdit(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()));
                //PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);

            if (dsaddress.Tables[0].Rows.Count > 0)
            {
                ucCurrency.SelectedCurrency = dsaddress.Tables[0].Rows[0]["FLDDEPOSITCURRENCY"].ToString();
                //ucAmount.Text = dsaddress.Tables[0].Rows[0]["FLDDEPOSITAMOUNT"].ToString();
                //txtApplicationAmount.Text = dsaddress.Tables[0].Rows[0]["FLDDEPOSITAMOUNT"].ToString();
                txtDepositBalance.Text = dsaddress.Tables[0].Rows[0]["FLDDEPOSITAMOUNT"].ToString();
                txtApplicationAmount.Text = Convert.ToString(sum);
                ViewState["RECEIVEINVOICELATER"] = dsaddress.Tables[0].Rows[0]["FLDRECEIVINGINVOICE"].ToString();
                //ViewState["PAYMENTONRECVINV"] = dsaddress.Tables[0].Rows[0]["FLDPAYMENTONRECEIVEINVOICE"].ToString();
                if (ViewState["PAYMENTONRECVINV"].ToString() == "0")
                {
                    ddlUpfrontPaymentRequired.Items.Add(new ListItem("Yes", "1"));
                    ddlUpfrontPaymentRequired.SelectedValue = "1";
                    ddlUpfrontPaymentRequired.Enabled = false;
                    //ucPaymentTypes.Enabled = false;
                }
                else
                {
                    ddlUpfrontPaymentRequired.Items.Clear();
                    ddlUpfrontPaymentRequired.Items.Add(new ListItem("Yes","1"));
                    ddlUpfrontPaymentRequired.Items.Add(new ListItem("No", "0"));
                    ddlUpfrontPaymentRequired.SelectedValue = "1";
                }

                if (ddlUpfrontPaymentRequired.SelectedValue == "1")
                {
                    if (General.GetNullableDecimal(dsaddress.Tables[0].Rows[0]["FLDDEPOSITAMOUNT"].ToString()) != null)
                    {
                        depositamount = General.GetNullableDecimal(dsaddress.Tables[0].Rows[0]["FLDDEPOSITAMOUNT"].ToString());
                    }

                    if (depositamount > 0 && depositamount >= sum && depositamount != 0)
                        ucPaymentTypes.ShortNameFilter = "ODP,TAO";

                    if (depositamount > 0 && depositamount < sum && depositamount != 0)
                        ucPaymentTypes.ShortNameFilter = "TUD,TAO";

                    if (depositamount < 0)
                    {
                        ucPaymentTypes.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 209, "TUD");
                        ucAmount.Enabled = true;
                    }
                    if (depositamount == 0)
                        ucPaymentTypes.ShortNameFilter = "TUD,TAO";
                }

            }
            //ucPaymentTypes.Enabled = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLicenceStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToString().ToUpper().Equals("PROCEED"))
            {
                if (!IsValidBankCompany())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    int iRowCount = 0;
                    int iTotalPageCount = 0;
                    string paymentstatus = "0";

                    DataSet ds = PhoenixCrewLicencePaymentRequests.CrewLicenceRequestSearch(
                                                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , null
                                                          , null
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , General.ShowRecords(null)
                                                          , ref iRowCount
                                                          , ref iTotalPageCount
                                                          , Filter.CurrentLicenceSelectedForPayment
                                                          , General.GetNullableInteger(null)
                                                          , General.GetNullableInteger(null)
                                                          , General.GetNullableInteger(null)
                                                          );
                    DataTable dt = ds.Tables[0];

                    //DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(null
                    //                                                    , null
                    //                                                    , null, null
                    //                                                    , 1, General.ShowRecords(null)
                    //                                                    , ref iRowCount, ref iTotalPageCount
                    //                                                    , null
                    //                                                    , null
                    //                                                    , Filter.CurrentLicenceSelectedForPayment);

                    string SupplierCode = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                    //string Amount = dt.Rows[0]["FLDAMOUNT"].ToString();
                    string CurrencyId = dt.Rows[0]["FLDCURRENCY"].ToString();
                    string RefNo = dt.Rows[0]["FLDREFNUMBER"].ToString();
                    string ProcessId = dt.Rows[0]["FLDPROCESSID"].ToString();
                    string VesselId = dt.Rows[0]["FLDVESSELLIST"].ToString();
                    string BudgetCode = dt.Rows[0]["FLDBUDGETID"].ToString();
                    string BankId = dt.Rows[0]["FLDBANKID"].ToString();
                    string BillToCompanyId = dt.Rows[0]["FLDBILLTOCOMPANYID"].ToString();

                    if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TUD"))
                    {
                        //upfrontpaymenttype = top up deposit
                       
                        PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                int.Parse(SupplierCode), // supplier
                                decimal.Parse(ucAmount.Text), // amount
                                DateTime.Parse(DateTime.Now.ToString()), // pay date
                                int.Parse(CurrencyId), //currency
                                null, //remarks
                                null, // reference document
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "APD")), // payment status -- draft
                                General.GetNullableGuid(advancepaymentid), // order id -- licence process id
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "GAR")), // type - general advance request
                                General.GetNullableInteger(BillToCompanyId), // companyid
                                General.GetNullableInteger(BudgetCode), // budget code
                                General.GetNullableInteger(VesselId), // vessel id
                                General.GetNullableInteger(BankId) // bankid
                                );

                        PhoenixCrewLicencePaymentRequests.UpdateDepositAmount(int.Parse(SupplierCode), decimal.Parse(ucAmount.Text));

                        ProcessEdit();
                        AddressEdit();
                    }

                    else if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TAO") && ViewState["PAYMENTONRECVINV"].ToString() == "0")
                    {
                        PhoenixCrewLicencePaymentRequests.LicenceInvoicePost(General.GetNullableInteger(SupplierCode), General.GetNullableInteger(BillToCompanyId), Filter.CurrentLicenceSelectedForPayment);
                    }

                    else if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TAO") && ViewState["PAYMENTONRECVINV"].ToString() == "1")
                    {
                        //upfrontpaymenttype = this appln only && receive invoice later:yes 

                        PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                               int.Parse(SupplierCode), // supplier
                               decimal.Parse(ucAmount.Text), // amount
                               DateTime.Parse(DateTime.Now.ToString()), // pay date
                               int.Parse(CurrencyId), //currency
                               null, //remarks
                               null, // reference document
                               General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "APD")), // payment status -- draft
                               General.GetNullableGuid(advancepaymentid), // order id -- licence process id
                               General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "PAR")), // type - PO advance request
                               General.GetNullableInteger(BillToCompanyId), // companyid
                               General.GetNullableInteger(BudgetCode), // budget code
                               General.GetNullableInteger(VesselId), // vessel id
                               General.GetNullableInteger(BankId) // bankid
                               );

                        PhoenixCrewLicencePaymentRequests.UpdateDepositAmount(int.Parse(SupplierCode), decimal.Parse(ucAmount.Text));

                        ProcessEdit();
                        AddressEdit();
                    }

                    else if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "ODP"))
                    {

                    }

                    else if (ViewState["PAYMENTONRECVINV"].ToString() == "0")
                    {
                        PhoenixCrewLicencePaymentRequests.LicenceInvoicePost(General.GetNullableInteger(SupplierCode), General.GetNullableInteger(BillToCompanyId), Filter.CurrentLicenceSelectedForPayment);
                    }

                    PhoenixCrewLicencePaymentRequests.UpdatePaymentOnReceiveofInvoice(Filter.CurrentLicenceSelectedForPayment, General.GetNullableInteger(ViewState["PAYMENTONRECVINV"].ToString())); //if payment on receive of invoice

                    if (ViewState["PAYMENTONRECVINV"].ToString() == "1" && ddlUpfrontPaymentRequired.SelectedValue == "0")
                        paymentstatus = PhoenixCommonRegisters.GetHardCode(1, 212, "ROI");

                    else if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "ODP") || ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TUD"))
                        paymentstatus = PhoenixCommonRegisters.GetHardCode(1, 212, "DEP");

                    else if (ViewState["PAYMENTONRECVINV"].ToString() == "0" && ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TAO"))
                        paymentstatus = PhoenixCommonRegisters.GetHardCode(1, 212, "APM");

                    else if (ViewState["PAYMENTONRECVINV"].ToString() == "1" && ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 209, "TAO"))
                        paymentstatus = PhoenixCommonRegisters.GetHardCode(1, 212, "APM");

                    PhoenixAccountsAdvancePaymentVoucher.LicencePaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableInteger(ViewState["BILLTOCOMAPNYID"].ToString())
                                                                            , General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString())
                                                                            , General.GetNullableInteger(ViewState["CURRENCY"].ToString())
                                                                            , advancepaymentid
                                                                            , sum
                                                                            , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "DRF"))
                                                                            , General.GetNullableInteger(paymentstatus));

                    String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    //               "BookMarkScript", "CloseWindow('codehelp1');", true);
                }
            }
            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "CloseWindow('codehelp1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidBankCompany()
    {
        ucError.HeaderMessage = "Please provide the required information";

        if (ucCurrency.SelectedCurrency == "" || ucCurrency.SelectedCurrency.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Please Map Currency in the Address Register.";

        if ((ucAmount.Text == "" || ucAmount.Text == "0.00") && ucAmount.Enabled == true)
            ucError.ErrorMessage = "Amount is Required";

        return (!ucError.IsError);
    }

    protected void ucPayment_Changed(object sender, EventArgs e)
    {
        if (ucPaymentTypes.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 209, "TUD"))
        {
            ucAmount.Enabled = true;
        }
        else
        {
            ucAmount.Text = "";
            ucAmount.Enabled = false;
        }

    }

    protected void ddlUpfrontPaymentRequired_Changed(object sender, EventArgs e)
    {
        if (ddlUpfrontPaymentRequired.SelectedValue == "0")
        {
            ucPaymentTypes.Enabled = false;
        }

        if (ddlUpfrontPaymentRequired.SelectedValue == "1")
        {
            ucPaymentTypes.Enabled = true;
            AddressEdit();
        }
    }
}
