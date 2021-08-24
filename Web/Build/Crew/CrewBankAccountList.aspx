<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBankAccountList.aspx.cs"
    Inherits="CrewBankAccountList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankRegiter" Src="~/UserControls/UserControlBankRegister.ascx" %>
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

                    <td align="left" style="width: 13%">
                        <telerik:RadLabel ID="lblSeafarerBank" runat="server" Text="Bank Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 37%">
                        <eluc:BankRegiter ID="ucBank" runat="server" Width="49%" AutoPostBack="true" CssClass="input_mandatory" AppendDataBoundItems="true" OnTextChangedEvent="UcBank_SelectedIndexChanged" />
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBank" CssClass="readonlytextbox" Enabled="false" Width="49%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td align="left" style="width: 13%">
                        <telerik:RadLabel ID="lblShortCodeSeafarerBank" runat="server" Text="Short Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 37%">
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankSortCode" Width="80%" MaxLength="10"
                            CssClass="readonlytextbox" Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountType" runat="server" Text="Account Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucAccountType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankCurrency" runat="server" Width="40%" Text="Bank Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucBankCurrency" runat="server" Width="40%" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountNumber" CssClass="input_mandatory" Width="80%"
                            MaxLength="50">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblDigits" runat="server" Text=""></telerik:RadLabel>


                    </td>

                    <td align="left">
                        <telerik:RadLabel ID="lblSwiftCodeSeafarerBank" runat="server" Text="Swift Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankSwiftCode" CssClass="input_mandatory" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblswiftcodedigits" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountName" CssClass="input_mandatory" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBankIFSCCode" runat="server" CssClass="input_mandatory" Width="80%"></telerik:RadTextBox>
                    </td>

                </tr>

                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblBranchSeafarerBank" runat="server" Text="Branch"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeafarerBankBranch" Width="80%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblaccountopenby" runat="server" Text="Opened By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" Width="40%" CssClass="input_mandatory" ID="ddlaccountopenby" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select record" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Company" />
                                <telerik:RadComboBoxItem Value="0" Text="Others" />
                            </Items>
                        </telerik:RadComboBox>
                        <%--<asp:DropDownList ID="ddlaccountopenby" runat="server" Width="40%" CssClass="input_mandatory">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                            <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="0"></asp:ListItem>
                        </asp:DropDownList>--%></td>

                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblPaymentPercentage" runat="server" Text="Payment Percentage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtPaymentPercentage" CssClass="txtNumber" Type="Percent" Width="60px"
                            MaxLength="9" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActive YN"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkInActiveYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Bank Address"></telerik:RadLabel>
                        </b></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <eluc:Address ID="ucAddress" runat="server" />
                    </td>
                    <td colspan="2" style="vertical-align: top; word-wrap: normal">
                        <table width="75%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDefaultAccountType" runat="server" Text="Default Account Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Quick runat="server" ID="ucDefaultAccountType" QuickTypeCode="182" AppendDataBoundItems="true" Width="180px" />
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    <telerik:RadLabel ID="lblDefaultCurrency" runat="server" Text="Default Allotment Currency"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Currency ID="ucDefaultAllotmentCurrency" runat="server" Width="180px" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAllotmentAmount" runat="server" Text="Default Allotment Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Decimal runat="server" ID="txtAllotmentAmount" Width="180px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblIsRemittingAgentReq" runat="server" Text="Is Remitting Agent Required"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkRemittingAgent" runat="server"></telerik:RadCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRemittingAgent" runat="server" Text="Remitting Agent"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:AddressType ID="ucRemittingAgent" runat="server" AddressType="135" Width="180px" EmptyMessage="Type or select Agent" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>
            </table>


            <table width="100%" cellpadding="1" cellspacing="1">
                <%--  <tr>
                    <td><b>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Bank Address"></telerik:RadLabel>
                    </b></td>
                </tr>--%>
            </table>


            <hr />
            <table width="100%" cellpadding="1" cellspacing="1">
                <b>
                    <telerik:RadLabel ID="lblIntermediateBank" runat="server" Text="Intermediate Bank"></telerik:RadLabel>
                </b>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBank" Width="60%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateBeneficiaryName" runat="server" Text="Beneficiary"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIbankAccountName" Width="60%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblIntermediateAccountNumber" runat="server" Text="Account No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIbankAccountNo" Width="60%" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblCountryIntBank" runat="server" Text="Country (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country runat="server" ID="ucIntermediateBankCountry" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblIBANNumberIntBank" runat="server" Text="IBAN Number (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIBANNumber" Width="60%" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblSwiftCodeIntBank" runat="server" Text="Swift Code (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBankSwiftCode" Width="60%"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIbankIFSCCode" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntermediateBankCurrency" runat="server" Text="Bank Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucIBankCurrency" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblTypeOfRemittance" runat="server" Text="Type Of Remittance"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucRemittanceType" AppendDataBoundItems="true" />
                    </td>
                    <td align="left">
                        <telerik:RadLabel ID="lblModeOfPayment" runat="server" Text="Mode Of Payment"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucModeOfPayment" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblAddressIntBank" runat="server" Text="Address (Int. Bank)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIntermediateBankAddres" CssClass="input" TextMode="MultiLine"
                            Width="130%" Height="60px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text="Verified Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVerifiedDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text="Verified By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
