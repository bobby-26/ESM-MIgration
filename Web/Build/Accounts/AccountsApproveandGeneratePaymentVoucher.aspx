<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsApproveandGeneratePaymentVoucher.aspx.cs" Inherits="AccountsApproveandGeneratePaymentVoucher" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AgentBankAccount" Src="~/UserControls/UserControlAgentBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetCode" Src="~/UserControls/UserControlOwnerBudgetCode.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve and Generate Payment Voucher</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="paymentvoucher" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfiguremovement" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="MenuVoucher_TabStripCommand"></eluc:TabStrip>
            <br />
            <table id="payment">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoofTransaction" runat="server" Text="No.of Transaction" Enabled="false"></telerik:RadLabel>

                        <telerik:RadTextBox ID="txtNoofTransaction" runat="server" Enabled="false" Width="50%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Total Amount" Enabled="false"></telerik:RadLabel>

                        <telerik:RadTextBox ID="txtTotalAmount" runat="server" Enabled="false" Width="20%">
                        </telerik:RadTextBox>
                        <%--  </td>
                    <td>--%>
                        <telerik:RadLabel ID="lblPVCompany" runat="server" Text="PV Company" Enabled="false"></telerik:RadLabel>

                        <telerik:RadTextBox ID="txtCompany" runat="server" Enabled="false" Width="20%">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPVRemarks" runat="server" Text="PV Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPaymentvoucherremarks" TextMode="MultiLine" Rows="3" runat="server"
                            EmptyMessage="Payment Voucher Remarks" Width="72%">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittingAgent" runat="server" Text="Remitting Agent">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ddlUnion_Changed" AddressType="135" Width="180px" />
                    </td>
                    <td></td>
                    <td></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittingagentbankaccount" runat="server" Text="Remitting Agent Bank Account">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AgentBankAccount ID="ddlAgentBankAccount" runat="server" Width="180px" OnTextChangedEvent="ddlAgentBankAccount_TextChangedEvent"
                            AppendDataBoundItems="true" AddressCode="3355" CssClass="input_mandatory" />

                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAgentCharges" runat="server" Text="Agent Charges">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ddlCurrency" runat="server" AppendDataBoundItems="true"
                            CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="180px" AutoPostBack="true" CssClass="input_mandatory" />
                        <%--   </td>

                    <td>--%>
                        <eluc:Number ID="txtAmount" runat="server" DecimalPlace="2" Width="120px" EmptyMessage="Amount" CssClass="input_mandatory" />
                        <%-- <telerik:RadTextBox ID="txtAmount" runat="server" InputType="Number" EmptyMessage="Amount" Width="100%"></telerik:RadTextBox>--%>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OwnerBudgetCode ID="ddlBudgetGroup" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" Width="180px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>

                    <td rowspan="3">
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="SourceUsage" runat="server" Text="Source/Usage"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>

                    </td>
                    <td rowspan="3">

                        <span id="spnPickListExpenseAccount">

                            <telerik:RadTextBox ID="txtAccountCode" runat="server" ReadOnly="false" InputType="Text" CssClass="input_mandatory"
                                MaxLength="20" Width="30%" OnTextChanged="txtAccountCode_changed">
                            </telerik:RadTextBox>
                            &nbsp;
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" InputType="Text" CssClass="input_mandatory"
                                ReadOnly="false" MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowAccount" Style="cursor: pointer; vertical-align: top"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx?iframename=true',true); " />
                            <telerik:RadTextBox ID="txtAccountId" runat="server" InputType="Text" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountSource" InputType="Text" CssClass="readonlytextbox" runat="server" Width="30%"></telerik:RadTextBox>
                            /
                                    <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" Width="17%" runat="server"></telerik:RadTextBox>
                            <br />
                            <telerik:RadTextBox ID="txtBudgetCode" InputType="Text" runat="server" Width="30%" CssClass="input"></telerik:RadTextBox>&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtBudgetName" InputType="Text" runat="server" Width="40%" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx',true); " />

                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
