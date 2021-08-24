using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAdvancePaymentGeneral : PhoenixBasePage
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtVendorCode.Attributes.Add("onkeydown", "return false;");
        txtVenderName.Attributes.Add("onkeydown", "return false;");
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        txtVendorId.Attributes.Add("style", "display:none");
        ImgSupplierPickList.Attributes.Add("style", "display:none");
        txtBankID.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("Bank", "BANK");
        toolbar.AddButton("Reject", "REJECT", ToolBarDirection.Right);
        toolbar.AddButton("Check","CHECK", ToolBarDirection.Right);
        toolbar.AddButton("Save", "UPDATE", ToolBarDirection.Right);
        //toolbar.AddButton("Approve", "APPROVE");

        //toolbar.AddButton("Create Payment Voucher", "CREATEPYMTVOUCHER");
        MenuAdvancePayment.AccessRights = this.ViewState;
        MenuAdvancePayment.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsAdvancePayment.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ADVANCEPAYMENTID"] = null;
            ViewState["ADDRESSCODE"] = null;
            ViewState["BANKID"] = null;
            ViewState["RECORDSTATUS"] = "ADD"; 
            txtPayDate.Text = DateTime.Now.ToString();

            BindTDS();
            BindWCT();
            if (Request.QueryString["ADVANCEPAYMENTID"] != null && Request.QueryString["ADVANCEPAYMENTID"] != string.Empty)
            {
                ViewState["ADVANCEPAYMENTID"] = Request.QueryString["ADVANCEPAYMENTID"].ToString();
               
                AdvancePaymentEdit();              
            }
        }       
      
    }

    private void Reset()
    {
        ddlCurrencyCode.SelectedCurrency = "";
        txtVendorId.Text = "";
        txtVendorCode.Text = "";
        txtVenderName.Text = "";
        txtAdvancePaymentNumber.Text = "";
        txtStatus.Text = "";
        //txtRemarks.Text = "";
        txtReferencedocument.Text = "";
        txtAdvanceAmount.Text = "";
        txtPayDate.Text =DateTime.Now.ToString();
        txtType.Text = "";
        imgAttachment.Visible = false;          
    }

  

    protected void MenuAdvancePayment_TabStripCommand(object sender, EventArgs e)
    {
    
        string strAdvancePaymentCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
            ViewState["RECORDSTATUS"] = "ADD";
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {

            if (!IsValidAdvancePayment())
            {
                ucError.Visible = true;
                return;
            } 
            if ( ViewState["RECORDSTATUS"].ToString()== "ADD")
            {
                try
                {
                    PhoenixAccountsAdvancePayment.AdvancePaymentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(txtVendorId.Text), decimal.Parse(txtAdvanceAmount.Text), DateTime.Parse(txtPayDate.Text),
                        int.Parse(ddlCurrencyCode.SelectedCurrency), null, txtReferencedocument.Text, null
                        , null, null, null, null, null, null);
                       
                    ucStatus.Text = "Voucher information added";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                               
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                
            }

            else
            {
                try
                {
                    PhoenixAccountsAdvancePayment.AdvancePaymentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ADVANCEPAYMENTID"].ToString())
                        , int.Parse(txtVendorId.Text), decimal.Parse(txtAdvanceAmount.Text), DateTime.Parse(txtPayDate.Text),
                        int.Parse(ddlCurrencyCode.SelectedCurrency), null, txtReferencedocument.Text
                        );
                    ViewState["RECORDSTATUS"] = "EDIT";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                
            }
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        //if (dce.CommandName.ToUpper().Equals("BANK"))
        //{
        //    string script = "parent.Openpopup('Bank','','../Accounts/AccountsSupplierBankInformation.aspx?addresscode=" + ViewState["ADDRESSCODE"].ToString() + " &advancepaymentid=" + ViewState["ADVANCEPAYMENTID"].ToString() + " &bankid=" + ViewState["BANKID"].ToString() + "');";
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
        //}

        if (CommandName.ToUpper().Equals("UPDATE"))
        {
            if (!IsValidAdvancePayment())
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixAccountsAdvancePayment.AdvancePaymentBankUpdate(new Guid(ViewState["ADVANCEPAYMENTID"].ToString()), int.Parse(txtBankID.Text),General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany)
                                            , General.GetNullableInteger(chkTDS.Checked == true ? "1" : "0")
                        , General.GetNullableGuid(ddlTDSType.SelectedValue)
                        , General.GetNullableDecimal(txtTDSRate.Text)
                        , General.GetNullableGuid(ddlWCTType.SelectedValue)
                        , General.GetNullableDecimal(txtWCTRate.Text));
            }
        }

        if (CommandName.ToUpper().Equals("APPROVE"))
        {
            if (ViewState["PAYMENTSTATUS"] != null)
            {
                //if (ViewState["PAYMENTSTATUS"].ToString() == "628")
                //{
                //    if (ViewState["ADVANCEPAYMENTID"] != null)
                        AdvancePaymentApproval();
                //}
                //else (ViewState["PAYMENTSTATUS"].ToString() == "629" || ViewState["PAYMENTSTATUS"].ToString() == "630")
                //{
                //    ucError.ErrorMessage = "Advance Payment is already approved";
                //    ucError.Visible = true;
                //    return;
                //}
            }

        }


        if (CommandName.ToUpper().Equals("CHECK"))
        {
            try
            {
                if (!IsValidAdvancePayment())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsAdvancePayment.AdvancePaymentBankUpdate(new Guid(ViewState["ADVANCEPAYMENTID"].ToString()), int.Parse(txtBankID.Text), General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany)
                        , General.GetNullableInteger(chkTDS.Checked == true ? "1" : "0")
                        , General.GetNullableGuid(ddlTDSType.SelectedValue)
                        , General.GetNullableDecimal(txtTDSRate.Text)
                        , General.GetNullableGuid(ddlWCTType.SelectedValue)
                        , General.GetNullableDecimal(txtWCTRate.Text));

                    PhoenixAccountsAdvancePayment.AdvancePaymentStatusUpdate(new Guid(ViewState["ADVANCEPAYMENTID"].ToString()), int.Parse(PhoenixCommonRegisters.GetHardCode(1, 127, "ARM")));
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("REJECT"))
        {
            if (ViewState["PAYMENTSTATUS"] != null)
            {
                if (ViewState["PAYMENTSTATUS"].ToString() != "630")
                {
                    if (txtRejectionRemarks.Text.Trim().Length != 0)
                    {
                        if (ViewState["ADVANCEPAYMENTID"] != null)
                            AdvancePaymentReject();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Unable to reject. Rejection remarks is mandatory. ";
                        ucError.Visible = true;
                        return;
                    }
                }
                else 
                {
                    ucError.ErrorMessage = " Unable to reject. Advance payment status is Operations manager approval.";
                    ucError.Visible = true;
                    return;
                }
            }

        }

        if (CommandName.ToUpper().Equals("CREATEPYMTVOUCHER"))
        {
            if (ViewState["PAYMENTSTATUS"].ToString() == "629")
            {
                if (ViewState["ADVANCEPAYMENTID"] != null)
                    AdvancePaymentVoucherInsert();
            }
            else if (ViewState["PAYMENTSTATUS"].ToString() == "630")
            {
                ucError.ErrorMessage = "Advance payment voucher is already created";
                ucError.Visible = true;
                return;
            }
            else
            {
                ucError.ErrorMessage = "Advance payment needs to be approved";
                ucError.Visible = true;
                return;
            }

        }
        String scriptudate = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptudate, true);
    }

    private void AdvancePaymentApproval()
    {
        try
        {
            PhoenixAccountsAdvancePayment.AdvancePaymentApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ADVANCEPAYMENTID"].ToString()));
            //PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(txtVendorId.Text)
            //    , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency), ViewState["ADVANCEPAYMENTID"].ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    private void AdvancePaymentReject()
    {
        try
        {
            PhoenixAccountsAdvancePayment.AdvancePaymentReject(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ADVANCEPAYMENTID"].ToString()), txtRejectionRemarks.Text);
            //PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(txtVendorId.Text)
            //    , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency), ViewState["ADVANCEPAYMENTID"].ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    private void AdvancePaymentVoucherInsert()
    {
        try
        {           
            PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                , ViewState["ADVANCEPAYMENTID"].ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    public bool IsValidAdvancePayment()
    {
        ucError.HeaderMessage = "Please provide the following required information";
      
        if (txtVendorId.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Supplier is required.";

        if (txtReferencedocument.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Payment reference is required";
       
        if (txtAdvanceAmount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (General.GetNullableDateTime(txtPayDate.Text)==null )
            ucError.ErrorMessage = "Date is required.";

        if (General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required.";

        if (string.IsNullOrEmpty(txtBankID.Text))
            ucError.ErrorMessage = "Bank is required.";

        if (General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Liability company is required.";

        return (!ucError.IsError);
    }


    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void AdvancePaymentEdit()
    {
        if (ViewState["ADVANCEPAYMENTID"] != null)
        {

            DataSet dsAdvancePayment = PhoenixAccountsAdvancePayment.AdvancePaymentEdit(new Guid(ViewState["ADVANCEPAYMENTID"].ToString()));
            if (dsAdvancePayment.Tables.Count > 0)
            {
                if (dsAdvancePayment.Tables[0].Rows.Count > 0)
                {
                    DataRow drAdvancePayment = dsAdvancePayment.Tables[0].Rows[0];
                    txtAdvancePaymentNumber.Text = drAdvancePayment["FLDADVANCEPAYMENTNUMBER"].ToString();
                    txtAdvanceAmount.Text = String.Format("{0:######.00}", drAdvancePayment["FLDAMOUNT"]);
                    txtPayDate.Text = General.GetDateTimeToString(drAdvancePayment["FLDPAYDATE"].ToString());
                    txtReferencedocument.Text = drAdvancePayment["FLDREFERENCEDOCUMENT"].ToString();
                    txtVendorId.Text = drAdvancePayment["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = drAdvancePayment["FLDCODE"].ToString();
                    txtVenderName.Text = drAdvancePayment["FLDNAME"].ToString();
                    ddlCurrencyCode.SelectedCurrency = drAdvancePayment["FLDCURRENCY"].ToString();
                    //txtRemarks.Text = drAdvancePayment["FLDREMARKS"].ToString();
                    txtStatus.Text = drAdvancePayment["FLDHARDNAME"].ToString();
                    txtDTKey.Text = drAdvancePayment["FLDDTKEY"].ToString();
                    txtType.Text = drAdvancePayment["FLDTYPEDESCRIPTION"].ToString();
                    ViewState["RECORDSTATUS"] = "EDIT";
                    ddlLiabilitycompany.SelectedCompany = drAdvancePayment["FLDCOMPANYID"].ToString();
                    txtVesselNameList.Text = drAdvancePayment["FLDVESSELNAME"].ToString();
                    txtBudgetCode.Text = drAdvancePayment["FLDBUDGETCODE"].ToString();
                    ViewState["PAYMENTSTATUS"] = drAdvancePayment["FLDPAYMENTSTATUS"].ToString();
                    ViewState["ADDRESSCODE"] = drAdvancePayment["FLDSUPPLIERCODE"].ToString();
                    ViewState["BANKID"] = drAdvancePayment["FLDBANKID"].ToString();
                    txtRejectionRemarks.Text = drAdvancePayment["FLDREJECTREMARKS"].ToString();
                    txtBankName.Text = drAdvancePayment["FLDBANKNAME"].ToString();
                    txtBankID.Text = drAdvancePayment["FLDBANKID"].ToString();
                    txtAccountNo.Text = drAdvancePayment["FLDACCOUNTNUMBER"].ToString();
                    txtCheckedBy.Text = drAdvancePayment["FLDCHECKEDBY"].ToString();
                    ucCheckedDate.Text = drAdvancePayment["FLDCHECKEDDATE"].ToString();
                    imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtVendorId.Text + "&currency=" + drAdvancePayment["FLDCURRENCY"] + "', true);");

                    chkTDS.Checked = drAdvancePayment["FLDTDSYN"].ToString() == "1" ? true : false;
                    ddlTDSType.SelectedValue = drAdvancePayment["FLDTDSTYPE"].ToString();
                    ddlTDSType.Enabled = drAdvancePayment["FLDTDSYN"].ToString() == "1" ? true : false;
                    txtTDSRate.Text = drAdvancePayment["FLDTDSRATE"].ToString();
                    ddlWCTType.SelectedValue = drAdvancePayment["FLDWCTTYPE"].ToString();
                    txtWCTRate.Text = drAdvancePayment["FLDWCTRATE"].ToString();
                    ddlWCTType.Enabled = ddlTDSType.SelectedItem.Text.ToUpper() == "194C" ? true : false;


                    txtInvoiceNumber.Text = drAdvancePayment["FLDINVOICENUMBER"].ToString();
                    txtInvoiceStatus.Text = drAdvancePayment["FLDINVOICESTATUSNAME"].ToString();

                    txtbankpaymentvoucher.Text = drAdvancePayment["FLDBANKPAYMENTVOUCHERNO"].ToString();
                    txtrowno.Text = drAdvancePayment["FLDROWNUMBER"].ToString();

                    ucToolTipINPVStatus.Text = drAdvancePayment["FLDINVOICEPAYMENTVOUCHERSTATUS"].ToString();
                    if (txtInvoiceStatus != null && (drAdvancePayment["FLDINVOICESTATUS"].ToString() == "372"))
                    {
                        txtInvoiceStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipINPVStatus.ToolTip + "', 'visible');");
                        txtInvoiceStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipINPVStatus.ToolTip + "', 'hidden');");
                    }

                    ucCreditNoteVoucherNo.Text = drAdvancePayment["FLDCREDITNOTEVOUCHERNO"].ToString();
                    if(txtStatus.Text != null && drAdvancePayment["FLDPAYMENTSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 127, "POC"))
                    {
                        //lblAdvancePaymentStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + ',' + uct1.ToolTip + "', 'visible');");
                        txtStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucCreditNoteVoucherNo.ToolTip + "', 'visible');");
                        //lblAdvancePaymentStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + ',' + uct1.ToolTip + "', 'hidden');");
                        txtStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucCreditNoteVoucherNo.ToolTip + "', 'hidden');");
                    }

                }
            }
        }
    }
   
   

    protected void AdvancePaymentClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMCODE"] = e.CommandArgument;
    }
    protected void BindTDS()
    {
        int irowcount = 0;
        int itotalpagecount = 0;

        ddlTDSType.DataTextField = "FLDSECTIONCODE";
        ddlTDSType.DataValueField = "FLDTDSPAYMENTID";

        ddlTDSType.DataSource = PhoenixAccountsTDSRegister.TDSRegisterList(1, 1000, ref irowcount, ref itotalpagecount);
        ddlTDSType.DataBind();
        ddlTDSType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindWCT()
    {
        int irowcount = 0;
        int itotalpagecount = 0;

        ddlWCTType.DataTextField = "FLDPAYMENTTYPE";
        ddlWCTType.DataValueField = "FLDWCTPAYMENTID";

        ddlWCTType.DataSource = PhoenixAccountsWCTRegister.WCTRegisterList(1, 1000, ref irowcount, ref itotalpagecount);
        ddlWCTType.DataBind();
        ddlWCTType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void chkTDS_Changed(object sender, EventArgs e)
    {
        if (chkTDS.Checked)
            ddlTDSType.Enabled = true;
        else
            ddlTDSType.Enabled = false;
        
        ddlTDSType.SelectedValue = "Dummy";
        txtTDSRate.Text = "";
        ddlWCTType.SelectedValue = "Dummy";
        txtWCTRate.Text = "";
        ddlWCTType.Enabled = false;
    }
    protected void ddlTDSType_TextChanged(object sender, EventArgs e)
    {

        if (ddlTDSType.SelectedItem.Text.ToUpper() == "194C")
            ddlWCTType.Enabled = true;
        else
            ddlWCTType.Enabled = false;

        DataTable dt = PhoenixAccountsTDSCalculations.GetTDSRate(General.GetNullableGuid(ddlTDSType.SelectedValue), General.GetNullableInteger(txtVendorId.Text));
        if (dt.Rows.Count > 0)
        {
            txtTDSRate.Text = dt.Rows[0]["FLDTDSRATE"].ToString();
        }
        else
            txtTDSRate.Text = "";

        ddlWCTType.SelectedValue = "Dummy";
        txtWCTRate.Text = "";
    }
    protected void ddlWCTType_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = PhoenixAccountsTDSCalculations.GetWCTRate(General.GetNullableGuid(ddlWCTType.SelectedValue), General.GetNullableInteger(txtVendorId.Text));
        if (dt.Rows.Count > 0)
        {
            txtWCTRate.Text = dt.Rows[0]["FLDWCTRATE"].ToString();
        }
        else
            txtWCTRate.Text = "";
    }

  
}
