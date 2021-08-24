<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitNote.aspx.cs"
    Inherits="AccountsCreditDebitNote" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Credit Note</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCreditNote" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="pnlCreditNote" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" Visible="false" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuCreditNote" runat="server" OnTabStripCommand="MenuCreditNote_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td colspan="5">
                        <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Possible Duplicate Credit Note exist for this Supplier. Click here to view the Credit Note List "
                            ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="False" Font-Size="Large"
                            Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreditNoteRegisterNo" runat="server" Text="Credit Note Register No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRegisterNo"  ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtStatus" 
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucReceivedDate" runat="server" CssClass="input_mandatory"  DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListSupplier">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="90px" CssClass="readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="200px"
                               >
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickSupplier" runat="server"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" ></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDate" runat="server" CssClass="input_mandatory"  ReadOnly="false" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVendorCreditNoteNo" runat="server" Text="Vendor Credit Note No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVendorCreditNoteNo" CssClass="input_mandatory" runat="server"  MaxLength="50"
                            Width="294px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Currency/ Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" ID="ucCurrency" CssClass="input_mandatory" AppendDataBoundItems="true" 
                            AutoPostBack="true" OnTextChangedEvent="CreditNote_SetExchangeRate" />
                        <eluc:Number ID="txtAmount" CssClass="input_mandatory" runat="server" 
                            DecimalPlace="2" IsPositive="true" />
                        <%--<telerik:RadTextBox runat="server" ID="txtAmount" CssClass="input_mandatory" Style="text-align: right;"></telerik:RadTextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtAmount"
                            Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender> --%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreditNoteVoucherNo" runat="server" Text="Credit Note Voucher No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCreditNoteVoucherNo" 
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlBillToCompany" CssClass="input_mandatory" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                             runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentVoucherNo" runat="server" Text="Payment Voucher No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPaymentVoucherNo" 
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="270px" Height="75px"
                            ></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblPostDate" runat="server" Text="Posting Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="UCPostDate" runat="server"  ReadOnly="false" DatePicker="true" />
                        <telerik:RadLabel ID="lblIsPOCancelled" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <%--<table cellpadding="1" cellspacing="1" style="width: 100%">
                    <tr>
                        <td>
                            Reference Number
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReferenceNumber" runat="server" CssClass="input_mandatory" MaxLength="50"
                                Width="240px"></telerik:RadTextBox>
                        </td>
                        <td>
                            Credit Note Reason 
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucCreditNoteReason" AppendDataBoundItems="true" QuickTypeCode="57"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Invoice Type</td>
                        <td>
                            
                            <eluc:Hard ID="ddlInvoiceType" runat="server" AppendDataBoundItems="true" 
                                AutoPostBack="TRUE" CssClass="dropdown_mandatory" 
                                HardList="<%# PhoenixRegistersHard.ListHard(1, 59) %>" HardTypeCode="59" 
                                OnTextChangedEvent="ddlInvoice_SelectedIndexChanged" Width="300px" />
                            
                        </td>
                        <td>
                            Received Date
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            Currency
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Exchange Rate
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtExchangeRate" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            Created Date
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtCreatedDate" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Supplier</td>
                        <td>
                            
                        </td>
                        <td>
                            PO
                            </td>
                        <td>
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox ID="txtPONumber" runat="server" Width="70px" CssClass="input_mandatory" ReadOnly="false"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtPOTitle" runat="server" Width="180px" CssClass="input_mandatory" ReadOnly="false"></telerik:RadTextBox>
                                <asp:ImageButton ID="ImgPOPickList" runat="server" 
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" 
                                onclick="ImgPOPickList_Click" />
                                <telerik:RadTextBox ID="txtOrderId" runat="server" Width="50px"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remarks
                        </td>
                        <td colspan="3">
                            
                        </td>
                    </tr>
                </table>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
