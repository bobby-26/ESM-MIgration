<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBankAddress.aspx.cs"
    Inherits="RegistersBankAddress" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlCommonAddress.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bank Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="pnlBankAddressEntry" runat="server">
<%--                        <telerik:RadLabel ID="lblBankInformation" runat="server" Text="Bank Information"></telerik:RadLabel>--%>
                    <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand"></eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table id="tblCompanyBank" width="100%">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtBeneficiaryName" Width="40%" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank Name"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtBankName" Width="40%" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBankCode" runat="server" Text="Bank Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtBankCode" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <eluc:Address runat="server" ID="ucAddress" />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblBranchCode" runat="server" Text="Branch Code:"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox ID="txtBranchcode" runat="server" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSwiftCode" runat="server" Text="Swift Code:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSwiftCode" runat="server" Width="40%" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIBANnumber" runat="server" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtAccountNumber" CssClass="input" Width="40%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblIntermediaryBankName" runat="server" Text="Intermediary Bank Name"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtIntermediaryBankName" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblIntermediaryBankAdd1" runat="server" Text="Intermediary Bank Address 1"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtIntermediaryBankAdd1" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblIntermediaryBankAdd2" runat="server" Text="Intermediary Bank Address 2"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtIntermediaryBankAdd2" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblIntermediaryswiftcode" runat="server" Text="Intermediary Swift Code"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtIntermediarySwiftCode" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblIntermediaryAccountNumber" runat="server" Text="Intermediary Account Number"></telerik:RadLabel>
                        </td>
                        <td width="63%">
                            <telerik:RadTextBox runat="server" ID="txtIntermediaryAccountNumber" Width="40%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <%--<tr>
                    <td>
                         <telerik:RadLabel ID="lblVoucherPrefixCurrencyCode" runat="server" Text="Voucher Prefix Currency Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCurrencyCode" MaxLength="1" CssClass="input_mandatory" Width="20px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                         <telerik:RadLabel ID="lblVoucherPrefixAccountCode" runat="server" Text="Voucher Prefix Account Code"></telerik:RadLabel>
                    </td>
                    <td> 
                        <telerik:RadTextBox runat="server" ID="txtAccountCode" MaxLength="1" CssClass="input_mandatory" Width="20px" ></telerik:RadTextBox>
                    </td>
                </tr>--%>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBankChargesforTT" runat="server" Text="Bank Charges for T/T"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtBankChargesofTT" CssClass="input" Width="70px"></telerik:RadTextBox>
                         <%--   <ajaxtoolkit:maskededitextender id="MaskNumber" runat="server" targetcontrolid="txtBankChargesofTT"
                                mask="999,999.99" masktype="Number" inputdirection="RightToLeft">
                                </ajaxtoolkit:maskededitextender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBankChargesforACH" runat="server" Text="Bank Charges for ACH"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtBankChargesofACH" CssClass="input" Width="70px"></telerik:RadTextBox>
                          <%--  <ajaxtoolkit:maskededitextender id="MaskedEditExtender1" runat="server" targetcontrolid="txtBankChargesofACH"
                                mask="999,999.99" masktype="Number" inputdirection="RightToLeft">
                                </ajaxtoolkit:maskededitextender>--%>
                        </td>
                    </tr>
                </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
