<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDeposit.aspx.cs"
    Inherits="Accounts_AccountsDeposit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Deposit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeposit" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager> 
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuDeposit" runat="server" Title="Deposit" OnTabStripCommand="MenuDeposit_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDepositNo" runat="server" Text="Deposit No"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDepositNo" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Deposit Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtStatus" Width="30%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier Name"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListSupplier">
                        <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:ImageButton ID="btnPickSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/audit_start.png %>" />
                        <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                    </span>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDepositType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDepositType" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Amount"></telerik:RadLabel>
                </td>
                <td>
                                <eluc:UserControlCurrency ID="ucCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnTextChangedEvent="Deposit_SetExchangeRate" />
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory txtNumber"
                                    DecimalPlace="2" IsPositive="true" Width="120px"></eluc:Number>
                                <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                    Visible="false" Wrap="False" Width="150px" ReadOnly="true"></eluc:Number>

<%--                    <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                        AutoPostBack="true" OnTextChangedEvent="Deposit_SetExchangeRate" />
                    <asp:TextBox runat="server" ID="txtAmount" Height="24px" CssClass="input_mandatory" Style="text-align: right;"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtAmount"
                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="ucDate" runat="server" CssClass="input_mandatory" ReadOnly="false"
                        DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Liability Company"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCompany ID="ddlLiabilityCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                        CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupplierBankingDetails" runat="server" Text="Supplier Banking Details"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListBank">
                        <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input" ReadOnly="false"
                            Width="80px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input" ReadOnly="false"
                            Width="100px"></telerik:RadTextBox>
                        <asp:ImageButton ID="btnPickBank" runat="server" ImageUrl="<%$ PhoenixTheme:images/audit_start.png %>" />
                        <telerik:RadTextBox ID="txtBankID" runat="server" Width="10px"></telerik:RadTextBox>
                        <%--                                <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" 
                                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />--%>

                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="270px" Height="75px"
                        CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCancellationRemarks" runat="server" Text="Cancellation Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCancellationRemarks" TextMode="MultiLine" Width="270px"
                        Height="75px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPaymentVoucherNo" runat="server" Text="Account Voucher No"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPaymentVoucherNo" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtApprovedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    <eluc:UserControlDate runat="server" ID="ucApprovedDate" Enabled="false" CssClass="readonlytextbox"
                        ReadOnly="true" />
                </td>
            </tr>
        </table>
        <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
