using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class AccountsCtmRequestArrangementGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

            ViewState["PAIDCOMPANY"] = "";
            if (ddlPaidFromCompany.SelectedCompany != null)
                ViewState["PAIDCOMPANY"] = ddlPaidFromCompany.SelectedCompany;

            if (ViewState["PAIDCOMPANY"] != null)
                ddlPaidFromCompany.SelectedCompany = ViewState["PAIDCOMPANY"].ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("CTM Request", "BACK",ToolBarDirection.Right);
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["ACTIVEYN"] = null;
                ViewState["COMPANYID"] = "";
                ViewState["CTMID"] = Request.QueryString["CTMID"];

                if (ViewState["CTMID"] != null && General.GetNullableGuid(ViewState["CTMID"].ToString()) != null)
                {
                    EditCTM(new Guid(ViewState["CTMID"].ToString()));
                }
            }
            Menu();
            txtBudgetName.Attributes.Add("style", "visibility:hidden");
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetName.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtChargesBudgetName.Attributes.Add("style", "visibility:hidden");
            txtChargesBudgetId.Attributes.Add("style", "visibility:hidden");
            txtChargesBudgetGroupId.Attributes.Add("style", "visibility:hidden");
            txtChargesOwnerBudgetName.Attributes.Add("style", "visibility:hidden");
            txtChargesOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
            txtChargesOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Menu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["ACTIVEYN"].ToString() == "1")
        {
            toolbar.AddButton("Arrangement", "FINALIZE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();
        }
        else
        { MenuCTM.Visible = false; }

    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Accounts/AccountsCtmRequestArrangement.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
          
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=5&showexcel=no&showword=no&reportcode=CTM&vesselid=" + lblVesselId.Text + "&captaincashid=" + ViewState["CTMID"].ToString());
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCTM(txtAmountArranged.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string Arrangedvia = rblOfficeUser.SelectedValue == "3" ? PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "RME") : PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "HCY");
                PhoenixAccountsCtm.CtmUpdate(new Guid(ViewState["CTMID"].ToString())
                                                , int.Parse(lblVesselId.Text)
                                                , decimal.Parse(txtAmountArranged.Text)
                                                , General.GetNullableInteger(Arrangedvia), txtRemarks.Text
                                                , General.GetNullableDecimal(rblOfficeUser.SelectedValue == "3" ? txtRemittanceCharges.Text : "")
                                                , General.GetNullableDecimal(rblOfficeUser.SelectedValue == "3" ? txtRemittanceLumpsumCharges.Text : "")
                                                , General.GetNullableDecimal(txtRemittanceAmount.Text)
                                                , General.GetNullableInteger(ddlPaidFromCompany.SelectedCompany)
                                                , General.GetNullableInteger(rblOfficeUser.SelectedValue == "3" ? txtBankID.Text : "")
                                                , General.GetNullableDecimal(txtHandlingFees.Text)
                                                , General.GetNullableInteger(txtuserid.Text)
                                                , int.Parse(rblOfficeUser.SelectedValue.ToString()) // 1 - SEAFARER, 2 - OFFICE USER ,3 - AGENT
                                                , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                                , General.GetNullableInteger(txtBudgetId.Text)
                                                , General.GetNullableGuid(txtOwnerBudgetId.Text)
                                                , General.GetNullableInteger(txtChargesBudgetId.Text)
                                                , General.GetNullableGuid(txtChargesOwnerBudgetId.Text));


                EditCTM(new Guid(ViewState["CTMID"].ToString()));
                ucStatus.Text = "Updated Successfully.";
                //   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            if (CommandName.ToUpper().Equals("FINALIZE"))
            {
                if (!IsValidCTM(txtAmountArranged.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                //if (!IsFinalizeValidCTM(new Guid(ViewState["CTMID"].ToString())))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                PhoenixAccountsCtm.CtmFinalizeUpdate(new Guid(ViewState["CTMID"].ToString()));
                ucStatus.Text = "Finalized Successfully.";
                //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                EditCTM(new Guid(ViewState["CTMID"].ToString()));
                Menu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditCTM(Guid gCTMId)
    {
        DataTable dt = PhoenixAccountsCtm.CtmEdit(gCTMId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            //   txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            lblVesselId.Text = dr["FLDVESSELID"].ToString();
            txtDate.Text = dr["FLDDATE"].ToString();
            txtPort.Text = dr["FLDSEAPORTNAME"].ToString();
           // Title1.Text = "CTM Arrangement - " + dr["FLDVESSELNAME"].ToString() + " ( " + dr["FLDREQUESTNUMBER"].ToString() + " " + General.GetDateTimeToString(dr["FLDDATE"].ToString()) + " )";
            //  txtRequestNumber.Text = dr["FLDREQUESTNUMBER"].ToString(); 
            txtcurrencycode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
            txtAmount.ReadOnly = true;
            txtAmountArranged.Text = dr["FLDAMOUNTARRANGED"].ToString();
            txtAmountArranged.ReadOnly = false;
            txtAmountArranged.CssClass = "input";
            txtRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
            txtBankID.Text = dr["FLDREMITTEDTO"].ToString();
            txtBankName.Text = dr["FLDBANKNAME"].ToString();
            txtAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtPortAgentEmail.Text = dr["FLDEMAIL1"].ToString();
            txtDeliveredBy.Text = dr["FLDDELIVEREDBY"].ToString();
            txtCharges.Text = dr["FLDCHARGESAMT"].ToString();
            txtHandlingFees.Text = dr["FLDHANDLINGFEES"].ToString();
            ViewState["COMPANYID"] = dr["FLDCOMPANYID"].ToString();
            ddlPaidFromCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();


            txtuserid.Text = dr["FLDADDRESSCODE"].ToString();
            txtMentorName.Text = dr["FLDDELIVEREDBY"].ToString();
            rblOfficeUser.SelectedValue = dr["FLDCASHCARRY"].ToString();
            OfficeCarry();
            txtRemittanceCharges.Text = dr["FLDCHARGES"].ToString();
            txtRemittanceLumpsumCharges.Text = dr["FLDLUMPSUMCHARGES"].ToString();
            txtuserDesignation.Text = dr["FLDCODE"].ToString();
            txtLumpsumCharges.Text = dr["FLDLUMPSUMCHARGES"].ToString();
            txtTotalCharges.Text = dr["FLDTOTALCHARGES"].ToString();
            txtRemittanceAmount.Text = dr["FLDPAYABLEAMOUNT"].ToString();
            txtBalance.Text = dr["FLDBALANCE"].ToString();
            txtRemarks.ReadOnly = false;
            txtRemarks.CssClass = "input";
            ViewState["ACTIVEYN"] = dr["FLDACTIVEYN"].ToString();
            ddlAccountDetails.Visible = true;
            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
            ddlAccountDetails.DataBind();
            //ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));
            ddlAccountDetails.SelectedValue = dr["FLDACCOUNTID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETCODEDESCRIPTION"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();

            txtOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            txtOwnerBudgetgroupId.Text = dr["FLDOWNERBUDGETGROUPID"].ToString();
            txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETID"].ToString();

            txtChargesBudgetCode.Text = dr["FLDCHARGESBUDGETCODE"].ToString();
            txtChargesBudgetName.Text = dr["FLDCHARGESBUDGETDESCRIPTION"].ToString();
            txtChargesBudgetId.Text = dr["FLDCHARGESBUDGETID"].ToString();

            txtChargesOwnerBudget.Text = dr["FLDCHARGESOWNERBUDGETCODE"].ToString();
            txtChargesOwnerBudgetgroupId.Text = dr["FLDCHARGESOWNERBUDGETGROUPID"].ToString();
            txtChargesOwnerBudgetId.Text = dr["FLDCHARGESOWNERBUDGETID"].ToString();

            btnShowBudget.Attributes.Add("onclick", "return showPickList('spnBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");
            btnShowChargesBudget.Attributes.Add("onclick", "return showPickList('spnChargesBudgetCode', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");

            btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
            btnShowChargesOwnerBudget.Attributes.Add("onclick", "return showPickList('spnChargesOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtChargesBudgetId.Text + "', true); ");


        }
    }
    private bool IsValidCTM(string ArrangedAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(ArrangedAmount).HasValue)
        {
            ucError.ErrorMessage = "Arranged amount is required.";
        }
        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Account is required.";

        if (General.GetNullableInteger(txtBudgetId.Text) == null)
            ucError.ErrorMessage = "Budget Code is required.";

        if (General.GetNullableGuid(txtOwnerBudgetId.Text) == null)
            ucError.ErrorMessage = "Owner Budget Code is required.";

        if (General.GetNullableInteger(txtChargesBudgetId.Text) == null)
            ucError.ErrorMessage = "Charges Budget Code is required.";

        if (General.GetNullableGuid(txtChargesOwnerBudgetId.Text) == null)
            ucError.ErrorMessage = "Charges Owner Budget Code is required.";

        if (rblOfficeUser.SelectedIndex == -1)
        {
            ucError.ErrorMessage = "Arranged Via is required.";
        }
        else if (rblOfficeUser.SelectedIndex == 2)
        {
            if ((General.GetNullableInteger(txtRemittanceCharges.Text).HasValue || General.GetNullableInteger(txtRemittanceLumpsumCharges.Text).HasValue) && General.GetNullableInteger(txtHandlingFees.Text).HasValue)
            {
                ucError.ErrorMessage = "Please enter either charge or handling fees";
            }
        }

        if (General.GetNullableInteger(txtuserid.Text) == null)
        {
            ucError.ErrorMessage = lblEmployees.Text + " is required.";
        }
        if (ddlPaidFromCompany.SelectedCompany == "Dummy")
            ucError.ErrorMessage = " 	Paid From is required.";
        return (!ucError.IsError);
    }
    protected void chkOfficeUser_OnCheckedChanged(object sender, EventArgs e)
    {
        txtuserid.Text = "";
        txtMentorName.Text = "";
        txtuserDesignation.Text = "";
        OfficeCarry();
    }

    private void OfficeCarry()
    {
        if (rblOfficeUser.SelectedValue == "3")
        {
            lblEmployees.Text = "Agent";
            ddlPaidFromCompany.SelectedCompany = ViewState["COMPANYID"].ToString() == "" ? "12" : ViewState["COMPANYID"].ToString();
            imguser.Attributes.Add("onclick", "return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=', true); ");
            imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtuserid.Text + "', true);");
            if (hdnagentyn.Value == "1")
            {
                txtBankID.Text = "";
                txtBankName.Text = "";
                txtAccountNo.Text = "";
            }
        }
        if (rblOfficeUser.SelectedValue == "2")
        {
            txtBankID.Text = "";
            txtBankName.Text = "";
            txtAccountNo.Text = "";
            ddlPaidFromCompany.SelectedCompany = ViewState["COMPANYID"].ToString() == "" ? "16" : ViewState["COMPANYID"].ToString();
            lblEmployees.Text = "Office Employee";
            imguser.Attributes.Add("onclick", "return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListUserWithShortCode.aspx', true); ");
            imgBankPicklist.Attributes.Add("onclick", "javascript:void(0);");
        }
        else if (rblOfficeUser.SelectedValue == "1")
        {
            imgBankPicklist.Attributes.Add("onclick", "javascript:void(0);");
            txtBankID.Text = "";
            txtAccountNo.Text = "";
            txtBankName.Text = "";
            ddlPaidFromCompany.SelectedCompany = ViewState["COMPANYID"].ToString() == "" ? "16" : ViewState["COMPANYID"].ToString();
            imguser.Attributes.Add("onclick", "return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListVesselJoinerList.aspx?vesselid=" + lblVesselId.Text + "', true); ");
            lblEmployees.Text = "Joiner";
        }

        if (ViewState["PAIDCOMPANY"] != null)
            ddlPaidFromCompany.SelectedCompany = ViewState["PAIDCOMPANY"].ToString();

        hdnagentyn.Value = "";
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        OfficeCarry();
        btnShowBudget.Attributes.Add("onclick", "return showPickList('spnBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");
        btnShowChargesBudget.Attributes.Add("onclick", "return showPickList('spnChargesBudgetCode', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");

        btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
        btnShowChargesOwnerBudget.Attributes.Add("onclick", "return showPickList('spnChargesOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtChargesBudgetId.Text + "', true); ");

    }

}
