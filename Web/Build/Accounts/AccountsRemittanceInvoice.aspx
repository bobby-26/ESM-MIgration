<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceInvoice.aspx.cs"
    Inherits="AccountsRemittanceInvoice" EnableEventValidation="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Payment Voucher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoicePaymentVoucher" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ttlInvoice" Text="Invoice" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="Voucher_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblSupplierCodeName" runat="server" Text="Supplier Code / Name:"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <span id="spnPickListSupplier">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                                CssClass="readonlytextbox">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input" Visible="false"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentmode" runat="server" CssClass="dropdown_mandatory" HardTypeCode="132"
                            Enabled="false" HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                </tr>
                <%-- <tr>
            <td>
                Payment Currency
            </td>
            <td>
                <telerik:RadTextBox ID="txtSubAccountCode" Visible="false" runat="server"></telerik:RadTextBox>
                <telerik:RadTextBox ID="txtAccountId" Visible="false" runat="server"> </telerik:RadTextBox>
                <telerik:RadTextBox ID="txtCurrencyId" Visible="FALSE" ReadOnly="true" runat="server" CssClass="input"></telerik:RadTextBox>
                <telerik:RadTextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
            </td>
            <td>
                Account Code
            </td>
            <td>
                <eluc:UserControlBankAccount ID="ddlBankAccount" Enabled="false" AppendDataBoundItems="true"
                    runat="server" CssClass="input_mandatory" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td>
                Amount
            </td>
            <td>
                <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
            </td>
            <td>
                Bank charge basis
            </td>
            <td>
                <eluc:Hard ID="ddlBankChargebasis" runat="server" CssClass="dropdown_mandatory" HardTypeCode="133"
                    Enabled="false" HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                    Width="300px" />
            </td>
        </tr>--%>
            </table>
            <br />
            <b>&nbsp;<telerik:RadLabel ID="lblRegisteredSupplierBankingDetails" runat="server" Text="Registered Supplier Banking Details"></telerik:RadLabel>
            </b>
            <br />
            <table>
                <tr>
                    <td width="12%">
                        <telerik:RadLabel ID="lblBeneficiaryBankName" runat="server" Text="Beneficiary Bank Name"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <telerik:RadTextBox ID="txtBeneficiaryBankName" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel ID="lblBeneficiaryBankSWIFTCode" runat="server" Text="Beneficiary Bank SWIFT Code"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <telerik:RadTextBox ID="txtBenficiaryBankSWIFTCode" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel ID="lblIntermediaryBankSWIFTCode" runat="server" Text="Intermediary Bank SWIFT Code"></telerik:RadLabel>
                    </td>
                    <td width="10%">
                        <telerik:RadTextBox ID="txtIntermediaryBankSWIFTCode" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryBankCode" runat="server" Text="Beneficiary Bank Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBenficiaryBankCode" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediaryBankName" runat="server" Text="Intermediary Bank Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIntermediaryBankName" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountNumber" runat="server" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryBranchCode" runat="server" Text="Beneficiary Branch Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBenficiaryBranchCode" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediaryBankAccountNumber" runat="server" Text="Intermediary Bank Account Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIntermediaryBankAccountNumber" runat="server" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIBANNumber" runat="server" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmailAddress" runat="server" Text="Email Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmailAddress" runat="server" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 100%"></iframe>
         
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
