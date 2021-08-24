<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRequest.aspx.cs" Inherits="AccountsAllotmentRemittanceRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittence Line Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>
        <table style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                    <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlPaymentmode" runat="server" CssClass="dropdown_mandatory" HardTypeCode="132"
                        HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true" ShortNameFilter="ATT,NLT,ALT"
                        Width="180px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Bank Account"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlBankAccount ID="ddlBankAccount" AppendDataBoundItems="true" runat="server"
                        OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged" AutoPostBack="true" CssClass="input_mandatory" style="width: 240px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBankchargebasis" runat="server" Text="Bank charge basis"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlBankChargebasis" runat="server" CssClass="dropdown_mandatory" HardTypeCode="133"
                        HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                        Width="180px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDRCurrency" runat="server" Text="Remit in DR Currency"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkDRCurrency" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Payment Voucher Currency/Amount"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSubAccountCode" Visible="false" runat="server" Style="display: none"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtAccountId" Visible="false" runat="server" Style="display: none"> </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCurrencyId" Visible="false" runat="server" Style="display: none"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="readonlytextbox" Width="55px"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtPaymentVoucherAmount" runat="server" Style="text-align: right; width: 120px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    <asp:Image ImageUrl="<%$ PhoenixTheme:images/currency_mismatch.png %>" ID="imgCurrencyConverter" Visible="false" ToolTip="Currency Converter"
                        runat="server" />
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRemittanceCurrency" runat="server" Text="Remittance Currency/Amount"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemittanceCurrency" Visible="true" ReadOnly="true" runat="server" CssClass="readonlytextbox" Width="55px"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtRemittanceAmount" runat="server" Style="text-align: right; width: 120px" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td><b>
                    <telerik:RadLabel ID="lblBeneficiaryBankDetails" runat="server" Text="Beneficiary Bank Details"></telerik:RadLabel></b>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAccountNumber" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBankAddress" runat="server" Text="Bank Address"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBankAddress" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBeneficiaryBankName" runat="server" Text="Bank"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBeneficiaryBankName" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>

                <td>
                    <telerik:RadLabel ID="lblIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtIFSCCode" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBankCurrency" runat="server" Text="Bank Currency"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBankCurrency" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Batch/Voucher No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>

            </tr>
        </table>
    </form>
</body>
</html>
