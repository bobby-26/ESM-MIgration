<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentGeneral.aspx.cs"
    Inherits="AccountsAdvancePaymentGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuAdvancePayment" runat="server" OnTabStripCommand="MenuAdvancePayment_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAdvancePaymentNumber" runat="server" MaxLength="25" ReadOnly="true"
                        CssClass="input" Width="120px">
                    </telerik:RadTextBox>
                    <asp:HyperLink ID="imgAttachment" runat="server" ImageUrl="<%$ PhoenixTheme:images/attachment.png%>"
                        Visible="false" ToolTip="Attachments"></asp:HyperLink>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentStatus" runat="server" Text="Payment Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtStatus" runat="server" ReadOnly="true" Width="140px" CssClass="input"></telerik:RadTextBox>
                    <eluc:ToolTip ID="ucCreditNoteVoucherNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="80px" CssClass="input_mandatory"
                            ReadOnly="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px" CssClass="input_mandatory"
                            ReadOnly="false">
                        </telerik:RadTextBox>
                        <img runat="server" id="ImgSupplierPickList" onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,183', true);"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
                <td>
                    <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReferencedocument" runat="server" CssClass="input_mandatory"
                        ReadOnly="true" Width="120px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceNUmber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInvoiceStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    <eluc:ToolTip ID="ucToolTipINPVStatus" runat="server" Width="150PX" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAdvanceAmount" runat="server" Text="Advance Amount"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtAdvanceAmount" runat="server" CssClass="input_mandatory" ReadOnly="true" Mask="999,999,999.99" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Payment Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                        CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtPayDate" runat="server" CssClass="input_mandatory" ReadOnly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtType" runat="server" CssClass="input" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                </td>
                <td>&nbsp;
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Liability Company"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCompany ID="ddlLiabilitycompany" runat="server" AppendDataBoundItems="true"
                        CompanyList="<%# PhoenixRegistersCompany.ListCompany()%>" CssClass="input" Readonly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselsInvolved" runat="server" Text="Vessels Involved"></telerik:RadLabel>
                </td>
                <td class="style1">
                    <telerik:RadTextBox ID="txtVesselNameList" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Rows="3" TextMode="MultiLine" Width="240px">
                    </telerik:RadTextBox>
                </td>
                <td>&nbsp;
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupplierBankingDetails" runat="server" Text="Supplier Banking Details:"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListBank">
                        <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                            Width="80px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                            Width="140px">
                        </telerik:RadTextBox>
                        <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtBankID" runat="server" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRejectionRemarks" runat="server" Text="Rejection Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRejectionRemarks" runat="server" TextMode="MultiLine" Rows="3"
                        Width="240px" CssClass="input">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCheckedBy" runat="server" Text="Checked By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCheckedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    <eluc:UserControlDate runat="server" ID="ucCheckedDate" Enabled="false" CssClass="readonlytextbox"
                        ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblApplicabilityTDS" runat="server" Text="Applicabilitiy of TDS"></telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBox ID="chkTDS" runat="server" OnCheckedChanged="chkTDS_Changed" AutoPostBack="true" />
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTDSType" runat="server" Text="TDS Section Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlTDSType" runat="server" Enabled="false"
                        AutoPostBack="true" OnTextChanged="ddlTDSType_TextChanged">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblTDSOnWCT" Text="TDS on WCT" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlWCTType" Enabled="false"
                        OnTextChanged="ddlWCTType_TextChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTDSRate" runat="server" Text="TDS Rate"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtTDSRate" runat="server" CssClass="readonlytextbox" Mask="999.999"
                        ReadOnly="true" />
                    <telerik:RadLabel ID="lblTDSPercentage" runat="server" Text="%"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblWCTRate" Text="WCT Rate" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtWCTRate" runat="server" CssClass="readonlytextbox" Mask="999.999"
                        ReadOnly="true" />
                    <telerik:RadLabel ID="lblWCTPercentage" runat="server" Text="%"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblbankvoucher" Text="Bank Payment Voucher" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtbankpaymentvoucher" runat="server" CssClass="input" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblrowno" Text="Row No." runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtrowno" runat="server" CssClass="input" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
