using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceReturn : PhoenixBasePage
{
    public string advanceStatus = "";
    public string claimStatus = "";
    public int claimSubmitted;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                if (Request.QueryString["visitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;

                if (Request.QueryString["TravelAdvanceId"] != "")
                    ViewState["TravelAdvanceId"] = Request.QueryString["TravelAdvanceId"];
                else
                    ViewState["TravelAdvanceId"] = null;
                ViewState["CashReceiptVoucher"] = null;

                TravelReturnEdit();

                int tab = 0;
                PhoenixToolbar toolbar = new PhoenixToolbar();
                if (claimStatus != "CPD")
                {
                    toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
                    tab = 1;
                }
                if (advanceStatus == "APP")
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbar.AddButton("Reject", "REJECT", ToolBarDirection.Right);
                    tab = 1;
                }

                MenuTravelAdvance.AccessRights = this.ViewState;
                if (tab == 1)
                    MenuTravelAdvance.MenuList = toolbar.Show();
                tab = 0;
                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                if (advanceStatus == "PEN" && claimSubmitted == 0)
                {
                    toolbarsub.AddButton("Salary Adjustment", "SALARY", ToolBarDirection.Right);
                    tab = 1;
                }

                if ((advanceStatus == "PEN" || advanceStatus == "PAID") && Convert.ToDecimal(txtReturnAmount.Text) == 0)
                {
                    toolbarsub.AddButton("Return", "RETURN", ToolBarDirection.Right);
                    tab = 1;
                }
                if (ViewState["CashReceiptVoucher"] != null && ViewState["CashReceiptVoucher"].ToString() != "")
                {
                    toolbarsub.AddButton("Return-Repost", "REPOST", ToolBarDirection.Right);
                    tab = 1;
                }

                if (ViewState["Salaryadjvoucher"] != null && ViewState["Salaryadjvoucher"].ToString() != "")
                {
                    toolbarsub.AddButton("Sal Adj-Repost", "SALADJREPOST", ToolBarDirection.Right);
                    tab = 1;
                }

                if (ViewState["Vesseltakencashvoucher"] != null && ViewState["Vesseltakencashvoucher"].ToString() != "")
                {
                    toolbarsub.AddButton("Vessel Cash-Repost", "VESSELCASHREPOST", ToolBarDirection.Right);
                    tab = 1;
                }

                if (tab == 1)
                {
                    MenuTravelAdvanceSub.AccessRights = this.ViewState;
                    MenuTravelAdvanceSub.MenuList = toolbarsub.Show();
                }
                TravelReturnEdit();
            }
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format(
                    "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelAdvanceReturnAdd.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(txtPaymentAmount.Text, ddlLiabilitycompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnUpdate(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                 , General.GetNullableDecimal(txtPaymentAmount.Text)
                                                                                 , int.Parse(ddlLiabilitycompany.SelectedCompany)
                                                                                 , txtRemarks.Text
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Updated successfully";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("REJECT"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestCancel(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                    , new Guid(ViewState["VisitId"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode, "Accounts");
                ucStatus.Text = "Travel Advance status changed as Requested ";
            }
            TravelReturnEdit();
            //String script = String.Format("javascript:parent.fnReloadList('code1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SALARY"))
            {
                if (!IsValidData(txtPaymentAmount.Text, ddlLiabilitycompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceSalaryAdjustmentPost(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Salary adjustment posted";
            }
            if (CommandName.ToUpper().Equals("RETURN"))
            {
                String scriptpopup = String.Format(
                    "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelAdvanceReturnAmount.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("REPOST"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestRepost(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Travel Advance Return reposted";
                TravelReturnEdit();
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("SALADJREPOST"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceSalaryAdjRepost(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Salary adjustment reposted";

                TravelReturnEdit();
            }

            if (CommandName.ToUpper().Equals("VESSELCASHREPOST"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceVesselTakenRepost(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Vessel taken amount reposted";
            }

            TravelReturnEdit();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void TravelReturnEdit()
    {
        try
        {
            if (ViewState["TravelAdvanceId"] != null)
            {
                DataSet ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnEdit(new Guid(ViewState["TravelAdvanceId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                txtTravelAdvanceNo.Text = dr["FLDTRAVELADVANCENUMBER"].ToString();
                txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                txtEmployee.Text = dr["FLDEMPLOYEEID"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                txtPort.Text = dr["FLDSEAPORTNAME"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtPaymentAmount.Text = dr["FLDPAYMENTAMOUNT"].ToString();
                txtApprovedAmount.Text = dr["FLDAPPROVEDAMOUNT"].ToString();
                txtPaymentDate.Text = dr["FLDPAYMENTDATE"].ToString();
                txtRequestDate.Text = General.GetDateTimeToString(dr["FLDREQUESTEDDATE"].ToString());
                txtReturnAmount.Text = dr["FLDRETURNAMOUNT"].ToString();
                if (dr["FLDLIABILITYCOMPANY"].ToString() == "")
                {
                    if (dr["FLDVISITTYPE"].ToString() == "3")
                        ddlLiabilitycompany.SelectedCompany = "12";
                    else
                        ddlLiabilitycompany.SelectedCompany = "16";
                }
                else
                    ddlLiabilitycompany.SelectedCompany = dr["FLDLIABILITYCOMPANY"].ToString();
                txtReturnDate.Text = dr["FLDRETURNDATE"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtBalance.Text = dr["FLDBALANCE"].ToString();
                txtVoucherNumber.Text = dr["FLDVESSELCASHVOUCHERNUMBER"].ToString() + "," + dr["FLDVOUCHERNUMBER"].ToString() + " , " + dr["FLDPAYMENTVOUCHERNUMBER"].ToString() + " , " + dr["FLDCASHPAYMENTVOUCHERNUMBER"].ToString();
                txtSalAdjVoucherNumber.Text = dr["FLDSALARYADJVOUCHERNUMBER"].ToString();
                advanceStatus = dr["FLDADVANCESTATUS"].ToString();
                claimStatus = dr["FLDCLAIMSTATUS"].ToString();
                claimSubmitted = int.Parse(dr["FLDCLAIMSUBMITTED"].ToString());
                ViewState["CashReceiptVoucher"] = dr["FLDVOUCHERID"].ToString();
                ViewState["Salaryadjvoucher"] = dr["FLDSALARYADJUSTMENTVOUCHERID"].ToString();
                ViewState["Vesseltakencashvoucher"] = dr["FLDJOURNALVOUCHERID"].ToString();
            }
            else
            {
                txtTravelAdvanceNo.Text = string.Empty;
                txtPurpose.Text = string.Empty;
                txtEmployee.Text = string.Empty;
                txtPort.Text = string.Empty;
                txtCurrency.Text = string.Empty;
                txtPaymentAmount.Text = string.Empty;
                txtApprovedAmount.Text = string.Empty;
                txtPaymentDate.Text = string.Empty;
                txtRequestDate.Text = string.Empty;
                txtReturnAmount.Text = string.Empty;
                ddlLiabilitycompany.SelectedCompany = string.Empty;
                txtReturnDate.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtBalance.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string approvedAmount, string paymentAmount, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (company.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Liability company is required.";
        if (paymentAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Payment Amount cannot be blank. If not payable, please update Payment Amount as 0.00.";
        if (Convert.ToDecimal(approvedAmount) < Convert.ToDecimal(paymentAmount))
            ucError.ErrorMessage = "Payment amount should be less than or equal to approved amount.";

        return (!ucError.IsError);

    }
    private bool IsValidData(string paymentAmount, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (company.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Liability company is required.";
        if (paymentAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Payment Amount cannot be blank. If not payable, please update Payment Amount as 0.00.";

        return (!ucError.IsError);

    }
}
