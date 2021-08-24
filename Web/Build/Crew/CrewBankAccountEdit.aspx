<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBankAccountEdit.aspx.cs"
    Inherits="CrewBankAccountEdit" %>

<!DOCTYPE html >
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankRegiter" Src="~/UserControls/UserControlBankRegister.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Bank Account List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewBankAccountList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewBankAccountList" runat="server" OnTabStripCommand="CrewBankAccountList_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4"><b>Seafarer Bank
                    </b></td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblSeafarerBank" runat="server" Text="Bank Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%;">

                        <telerik:RadTextBox runat="server" ID="txtSeafarerBank" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblShortCodeSeafarerBank" runat="server" Text="Short Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%;">
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankSortCode" Width="80%" MaxLength="10"
                            Enabled="False" CssClass="input" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountType" runat="server" Text="Account Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtType" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Text=""></telerik:RadTextBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankCurrency" runat="server" Text="Bank Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Text=""></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Account No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountNumber" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="50">
                        </telerik:RadTextBox>

                        <%--  <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtAccountNumber"
                                        MaskType="Number" Filtered="0123456789" Mask="9999999999"
                                        InputDirection="RightToLeft" ClearMaskOnLostFocus="false">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                    </td>

                    <td align="left">
                        <telerik:RadLabel ID="lblSwiftCodeSeafarerBank" runat="server" Text="Swift Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankSwiftCode" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountName" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBankIFSCCode" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblBranchSeafarerBank" runat="server" Text="Branch"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankBranch" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblaccountopenby" runat="server" Text="Opened By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtaccountopenby" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Text=""></telerik:RadTextBox>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblPaymentPercentage" runat="server" Text="Payment Percentage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPaymentPercentage" Width="80%" MaxLength="50"
                            Enabled="False" CssClass="input" ReadOnly="true">
                        </telerik:RadTextBox>
                        <%--  <ajaxToolkit:MaskedEditExtender ID="mexPaymentPercentage" runat="server" TargetControlID="txtPaymentPercentage"
                            Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>--%>
                    </td>

                </tr>
                <tr>
                    <td>Address1
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressLine1" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="200">
                        </telerik:RadTextBox>
                    </td>
                    <td>Address2
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressLine2" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Address3
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressLine3" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>Address4
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressLine4" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Country
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCountry" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>

                    </td>
                    <td>State
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtstate" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>City
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCity" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="200"></telerik:RadTextBox>

                    </td>
                    <td>Postal Code
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPinCode" runat="server" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="10" Style="text-align: right;"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"><b>Intermediate Bank
                    </b></td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBank" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateBeneficiaryName" runat="server" Text="Beneficiary"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIbankAccountName" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblIntermediateAccountNumber" runat="server" Text="Account No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIbankAccountNo" Enabled="False" CssClass="input" ReadOnly="true" Width="80%" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblCountryIntBank" runat="server" Text="Country (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBankCountry" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                        <%--<eluc:Country runat="server" ID="ucIntermediateBankCountry" CssClass="input" AppendDataBoundItems="true" />--%>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblIBANNumberIntBank" runat="server" Text="IBAN Number (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIBANNumber" Width="80%" MaxLength="50" Enabled="False" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblSwiftCodeIntBank" runat="server" Text="Swift Code (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBankSwiftCode" Enabled="False" CssClass="input" ReadOnly="true" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIbankIFSCCode" runat="server" Enabled="False" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateBankCurrency" runat="server" Text="Bank Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIBankCurrency" runat="server" Enabled="False" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblTypeOfRemittance" runat="server" Text="Type Of Remittance"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemittanceType" runat="server" Enabled="False" CssClass="input" ReadOnly="true"></telerik:RadTextBox>

                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblModeOfPayment" runat="server" Text="Mode Of Payment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtModeOfPayment" runat="server" Enabled="False" CssClass="input" ReadOnly="true"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblAddressIntBank" runat="server" Text="Address (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBankAddres" Enabled="False" CssClass="input" ReadOnly="true"
                            TextMode="MultiLine" Width="80%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActive YN"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkInActiveYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDefaultAccountType" runat="server" Text="Default Account Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucDefaultAccountType" QuickTypeCode="182" AppendDataBoundItems="true" Width="80%" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDefaultAllotmentCurrency" runat="server" Text="Default Allotment Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucDefaultAllotmentCurrency" runat="server" Width="80%" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAllotmentAmount" runat="server" Text="Default Allotment Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtAllotmentAmount" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIsRemittingAgentReq" runat="server" Text="Is Remitting Agent Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRemittingAgent" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittingAgent" runat="server" Text="Remitting Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType ID="ucRemittingAgent" runat="server" AddressType="135" Width="80%" EmptyMessage="Type or select Agent" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text="Verified By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox><b>On</b>
                        <eluc:Date ID="txtVerifiedDate" runat="server" CssClass="input_mandatory" />
                    </td>

                    <%--                </tr>
                <tr>--%>
                    <td>
                        <telerik:RadLabel ID="lblInActiveRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtInActiveRemarks" TextMode="MultiLine" Width="270px" Height="75px"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
