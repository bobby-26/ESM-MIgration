using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsInvoiceDirectPurchaseOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuDirectPO.AccessRights = this.ViewState;
        MenuDirectPO.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");

            ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
            ViewState["ORDERID"] = Request.QueryString["orderid"];
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
            {
                imgAddAdvanceAmount.Attributes.Add("onclick", "return showPickList('spnPicPartPaid', 'codehelp1', '', '../Accounts/AccountsDirectPOAdvancePayment.aspx?orderid=" + ViewState["ORDERID"].ToString() + "', true); ");
            }
            ddlVessel.bind();

            EditOrder();

            ucProjectcode.bind(General.GetNullableInteger(ddlAccountDetails.SelectedValue), General.GetNullableInteger(txtBudgetCodeId.Text));
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?framename=ifMoreInfo', true); ");

        imgShowBudgetCode.Attributes.Add("onclick", "return showPickList('spnPickListBudgetCode', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?framename=ifMoreInfo&budgetgroup=106&hardtypecode=30&vesselid=" + ddlVessel.SelectedVessel + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

        txtBudgetCodeId.Attributes.Add("Style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("Style", "visibility:hidden");
        if (ViewState["Ownerid"] != null && txtBudgetCodeId.Text != null)
        {
            //imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + ddlVessel.SelectedVessel + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetCodeId.Text + "', true); ");
            imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?framename=ifMoreInfo&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetCodeId.Text + "', true); ");
        }

        txtOwnerBudgetName.Attributes.Add("style", "visibility:hidden;");
        txtOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
        txtOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");
    }

    protected void MenuDirectPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string vessel = ddlVessel.SelectedVessel;
                string ponumber = txtPONumber.Text;
                string vendor = txtVendorId.Text;
                string currency = ddlCurrency.SelectedCurrency;
                string podate = txtReceivedDate.Text;
                string Description = txtDescription.Text;
                string ETA = ucETA.Text;
                string ETD = ucETD.Text;
                string Port = ucPortMulti.SelectedValue;
                string invoicetype = ddlInvoiceType.SelectedHard;
                string ispni = ddlispni.SelectedValue.ToString();
                if (!IsValidInvoice(vessel, ponumber, vendor, currency, ETA, ETD, Port, invoicetype, Description, ispni))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] == null)
                {
                    PhoenixAccountsInvoice.InvoiceDirectPOAmosInsert(
                        null, int.Parse(vessel), ponumber, int.Parse(vendor), int.Parse(currency)
                        , General.GetNullableDateTime(podate), General.GetNullableDecimal(string.Empty), byte.Parse(chkGSTOffset.Checked ? "1" : "0"),
                        General.GetNullableDecimal(txtAdvanceAmount.Text), 0, txtDescription.Text,
                        General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                        General.GetNullableDecimal(txtDiscount.Text),
                        General.GetNullableInteger(txtBudgetCodeId.Text),
                        General.GetNullableDateTime(ucETA.Text),
                        General.GetNullableDateTime(ucETD.Text),
                        General.GetNullableInteger(ucPortMulti.SelectedValue), General.GetNullableInteger(ddlCompany.SelectedCompany),
                        General.GetNullableGuid(txtOwnerBudgetId.Text),
                        General.GetNullableInteger(ddlInvoiceType.SelectedHard)
                        , General.GetNullableInteger(ispni)
                        ,General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                        );
                }
                else
                {
                    PhoenixAccountsInvoice.InvoiceDirectPOUpdate(
                        new Guid(ViewState["ORDERID"].ToString()), int.Parse(vessel), ponumber, int.Parse(vendor), int.Parse(currency)
                        , General.GetNullableDateTime(podate), General.GetNullableDecimal(string.Empty), byte.Parse(chkGSTOffset.Checked ? "1" : "0"),
                        General.GetNullableDecimal(txtAdvanceAmount.Text), 0, txtDescription.Text,
                        General.GetNullableDecimal(txtDiscount.Text),
                        General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                        General.GetNullableInteger(txtBudgetCodeId.Text),
                        General.GetNullableDateTime(ucETA.Text),
                        General.GetNullableDateTime(ucETD.Text),
                        General.GetNullableInteger(ucPortMulti.SelectedValue), General.GetNullableInteger(ddlCompany.SelectedCompany),
                        General.GetNullableGuid(txtOwnerBudgetId.Text),
                        General.GetNullableInteger(ddlInvoiceType.SelectedHard)
                        , General.GetNullableInteger(ispni)
                        , General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                        );
                }
                ViewState["ISPNI"] = ddlispni.SelectedValue.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                Session["New"] = "Y";
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {

                ViewState["ORDERID"] = null;
                ddlVessel.SelectedVessel = string.Empty;
                txtPONumber.Text = string.Empty;
                //txtPoReceivedDate.Text = string.Empty;
                ddlCurrency.SelectedCurrency = string.Empty;
                txtVendorId.Text = string.Empty;
                txtVenderName.Text = string.Empty;
                txtVendorCode.Text = string.Empty;
                //txtVesselDiscount.Text = string.Empty;
                chkGSTOffset.Checked = false;
                txtAdvanceAmount.Text = string.Empty;
                txtStatus.Text = string.Empty;
                txtReferenceNo.Text = string.Empty;
                txtEstimateAmount.Text = string.Empty;
                txtDescription.Text = string.Empty;
                ddlAccountDetails.SelectedValue = string.Empty;
                txtDiscount.Text = string.Empty;

                txtBudgetCode.Text = string.Empty;
                txtBudgetCodeDescription.Text = string.Empty;
                txtBudgetCodeId.Text = string.Empty;
                txtBudgetgroupId.Text = string.Empty;

                txtOwnerBudgetCode.Text = string.Empty;
                txtOwnerBudgetName.Text = string.Empty;
                txtOwnerBudgetId.Text = string.Empty;
                txtOwnerBudgetgroupId.Text = string.Empty;

                ucPortMulti.SelectedValue = string.Empty;
                ucETA.Text = string.Empty;
                ucETD.Text = string.Empty;
                ucPortMulti.Text = string.Empty;

                ddlCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
                ddlInvoiceType.SelectedHard = string.Empty;
                ddlispni.SelectedIndex = 0;

                txtApprovedBy.Text = string.Empty;
                txtApprovedOn.Text = string.Empty;

                txtReceivedDate.Text = string.Empty;

                ucProjectcode.SelectedProjectCode = string.Empty;
                ViewState["Ownerid"] = null;



            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                string attachmentflag;
                DataTable dt = PhoenixAccountsInvoice.AdavanceDirectPOCancelValidation(General.GetNullableGuid(ViewState["ORDERID"].ToString()), 1);
                attachmentflag = dt.Rows[0]["FLDFLAG"].ToString();
                if (attachmentflag == "1")
                {
                    if (lblApprovedStatus.Text != "2")
                    {
                        String scriptpopup = String.Format(
                                "javascript:parent.Openpopup('codehelp1', '', 'AccountsInvoiceDirectPurchaseOrderCancelRemarks.aspx?directpo=" + ViewState["ORDERID"].ToString() + "');");
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                    }
                    //else
                    //{
                    //    ucError.ErrorMessage = "Already cancelled";
                    //    ucError.Visible = true;
                    //    return;
                    //}
                }
                else
                {
                    dt = PhoenixAccountsInvoice.AdavanceDirectPOCancelValidation(General.GetNullableGuid(ViewState["ORDERID"].ToString()), 0);
                    if (dt.Rows.Count > 0)
                    {
                        attachmentflag = dt.Rows[0]["FLDFLAG"].ToString();
                        if (attachmentflag == "1")
                        {
                            ucError.ErrorMessage = "Unable to Cancel PO,check with Accounts if the advance has been made";
                            ucError.Visible = true;
                            return;
                        }
                        if (attachmentflag == "0")
                        {
                            if (lblApprovedStatus.Text != "2")
                            {
                                ucConfirmMsg.Visible = true;
                                ucConfirmMsg.Text = "Advance Payment already been made,are you sure this PO is to be cancelled?";
                                return;
                            }
                        }
                        if (attachmentflag == "2")
                        {
                            if (lblApprovedStatus.Text != "2")
                            {
                                String scriptpopup = String.Format(
                                        "javascript:parent.Openpopup('codehelp1', '', 'AccountsInvoiceDirectPurchaseOrderCancelRemarks.aspx?directpo=" + ViewState["ORDERID"].ToString() + "');");
                                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                            }
                        }
                    }
                }

            }
            EditOrder();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditOrder()
    {
        if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();

                ddlAccountDetails.Visible = true;
                ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
                ddlAccountDetails.DataBind();

                ddlAccountDetails.SelectedValue = dr["FLDACCOUNTID"].ToString();

                txtPONumber.Text = dr["FLDFORMNO"].ToString();
                //txtPoReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
                ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                txtVendorId.Text = dr["FLDVENDORID"].ToString();
                txtVenderName.Text = dr["FLDNAME"].ToString();
                txtVendorCode.Text = dr["FLDCODE"].ToString();
                //txtVesselDiscount.Text = dr["FLDVESSELDISCOUNT"].ToString();
                if (dr["FLDGSTOFFSET"].ToString() == "1")
                    chkGSTOffset.Checked = true;
                //if (dr["FLDISGSTAPPLICABLE"].ToString() == "1")
                //    chkGSTOffset.Enabled = false;
                //if (dr["FLDPOHAVINGISSUES"].ToString() == "1")
                //    chkIssues.Checked = true;
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtAdvanceAmount.Text = dr["FLDPURCHASEADVANCEAMOUNT"].ToString();
                txtDiscount.Text = dr["FLDDEFAULTDISCOUNT"].ToString();
                txtReferenceNo.Text = dr["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtStatus.Text = dr["FLDINVOICESTATUS"].ToString();
                txtEstimateAmount.Text = dr["FLDESTIMATEAMOUNT"].ToString();

                txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
                txtBudgetCodeDescription.Text = dr["FLDBUDGETDESCRIPTION"].ToString();
                txtBudgetCodeId.Text = dr["FLDBUDGETCODE"].ToString();
                txtBudgetgroupId.Text = dr["FLDBUDGETGROUP"].ToString();

                txtOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODENAME"].ToString();
                txtOwnerBudgetName.Text = dr["FLDOWNERBUDGETCODE"].ToString();
                txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETID"].ToString();

                ucPortMulti.SelectedValue = dr["FLDSEAPORT"].ToString();
                ucPortMulti.Text = dr["FLDSEAPORTNAME"].ToString();
                ucETA.Text = dr["FLDETA"].ToString();
                ucETD.Text = dr["FLDETD"].ToString();

                txtApprovedBy.Text = dr["FLDAPPROVEDBY"].ToString();
                txtApprovedOn.Text = dr["FLDAPPROVEDDATE"].ToString();
                txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
                lblApprovedStatus.Text = dr["FLDAPPROVALSTATUS"].ToString();
                ucToolTip.Text = dr["FLDCANCELLEDREMARKS"].ToString();

                ddlispni.SelectedValue = dr["FLDPNIYN"].ToString();

                ddlCompany.SelectedCompany = dr["FLDLIABILITYCOMPANY"].ToString();

                if (lblApprovedStatus.Text == "2")
                {
                    txtPONumber.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'visible');");
                    txtPONumber.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'hidden');");
                }

                lblBillToCompanyName.Text = dr["FLDLIABILITYCOMPANYNAME"].ToString();
                lblVesselAccountName.Text = dr["FLDVESSELACCOUNTNAME"].ToString();
                ddlInvoiceType.SelectedHard = dr["FLDINVOICETYPE"].ToString();
                ucProjectcode.SelectedProjectCode = dr["FLDPROJECTID"].ToString();

                //DataTable dt = PhoenixPurchaseApprovedVesselAccount.VesselAccountSearch(
                //    new Guid(ViewState["quotationid"].ToString()),
                //    new Guid(ViewState["orderid"].ToString()));

                //if (dt.Rows.Count > 0)
                //    ddlAccountDetails.SelectedValue = dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
                //else
                //    ddlAccountDetails.SelectedValue = "";

                if (ddlAccountDetails.SelectedIndex > 0)
                {
                    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["Ownerid"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                        }
                    }
                }

            }
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
        txtDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
        lblPercentage.Visible = (showcreditnotedisc == 1) ? true : false;

    }
    private bool IsValidInvoice(string vessel, string ponumber, string vendor, string currency, string ETA, string ETD, string Port, string invoicetype, string Description, string ispni)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (invoicetype.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Invoice type required.";

        if (!General.GetNullableInteger(vessel).HasValue)
            ucError.ErrorMessage = "Vessel is required";

        //if (ponumber.Trim() == string.Empty)
        //    ucError.ErrorMessage = "PO Number is required.";

        if (!General.GetNullableInteger(vendor).HasValue)
            ucError.ErrorMessage = "Vendor is required.";

        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Currency is required.";

        if (Description.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";

        if (invoicetype == "1340")
        {
            if (!General.GetNullableInteger(Port).HasValue)
                ucError.ErrorMessage = "Port is required";
            if (!General.GetNullableDateTime(ETA).HasValue)
                ucError.ErrorMessage = "ETA is required";
            if (!General.GetNullableDateTime(ETD).HasValue)
                ucError.ErrorMessage = "ETD is required";
        }

        if (ispni.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Is this P&I.";


        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlInvoiceType.SelectedHard.ToUpper() != "DUMMY")
        //{


        //    if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "SPI"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=183', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else if (ddlInvoiceType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "BNP"))
        //    {
        //        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListBondAndProvisionAddress.aspx', true); ");
        //        txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";

        //    }
        //    else
        //    {
        //        ImgSupplierPickList.Attributes["onclick"] = null;
        //    }

        //}
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ddlVessel.SelectedVessel), 1);
        ddlAccountDetails.DataBind();

        //Bind default company for Direct PO
        DataTable dt = PhoenixAccountsPoTypeCompanyMapping.GetBillToCompanyforDirectPO(General.GetNullableInteger(ddlVessel.SelectedVessel));
        if (dt.Rows.Count > 0)
        {
            ddlCompany.SelectedCompany = dt.Rows[0]["FLDBILLTOCOMPANY"].ToString();
        }
        else
        {
            ddlCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
        }

    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (ViewState["Ownerid"] != null && txtBudgetCodeId.Text != null)
        {
            //imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + ddlVessel.SelectedVessel + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetCodeId.Text + "', true); ");
            imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?framename=ifMoreInfo&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetCodeId.Text + "', true); ");
        }

        int? VesselAccountId = General.GetNullableInteger(ddlAccountDetails.SelectedValue);

        if (imgShowBudgetCode != null)
        {
            int? bulkBudget = General.GetNullableInteger(txtBudgetCodeId.Text);
            ucProjectcode.bind(VesselAccountId, bulkBudget);
        }

    }
    protected void CheckMapping_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) != null)
                {
                    PhoenixAccountsInvoice.AdavanceDirectPOCreditNoteInsert(General.GetNullableGuid(ViewState["ORDERID"].ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlAccountDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccountDetails.SelectedIndex > 0)
        {
            DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Ownerid"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                }
            }
        }

        ucProjectcode.bind(General.GetNullableInteger(ddlAccountDetails.SelectedValue), General.GetNullableInteger(txtBudgetCodeId.Text));
    }
}
