<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectBillingItemGeneral.aspx.cs" Inherits="AccountsProjectBillingItemGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="ProjectBilling" runat="server" OnTabStripCommand="ProjectBilling_TabStripCommand" TabStrip="false"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBillingName" runat="server" Text="Billing Name"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadTextBox ID="txtprojectBillingName" CssClass="input_mandatory" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBillingGroup" runat="server" Text="Billing Group"></telerik:RadLabel></td>
                    <td>
                        <eluc:Quick ID="ucProjectBillingGroups" Width="150px" runat="server" AppendDataBoundItems="true"
                            QuickTypeCode="98" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBillingUnit" runat="server" Text="Billing Unit"></telerik:RadLabel></td>
                    <td>
                        <eluc:UserControlUnit ID="ucBillingUnit" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDefaultPrice" runat="server" Text="Default Price"></telerik:RadLabel></td>
                    <td>
                        <eluc:UserControlCurrency ID="ucCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="false" />
                        <eluc:Number ID="ucDefaultPrice" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="4"
                            IsPositive="true" MaxLength="9" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselBudgetCode" runat="server" Text="Vessel Budget Code"></telerik:RadLabel></td>
                    <td>
                        <span id="spnPickListBudgetCode">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input_mandatory" MaxLength="20" Enabled="false"
                                Width="20%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetCodeDescription" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="50%"></telerik:RadTextBox>
                            <img runat="server" id="imgShowBudgetCode" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtBudgetCodeId" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreditAccount" runat="server" Text="Credit Account"></telerik:RadLabel></td>
                    <td>
                        <span id="spnPickListCreditAccount">
                            <telerik:RadTextBox ID="txtCreditAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="20" Width="20%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="50%"></telerik:RadTextBox>
                            <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInventoryAccount" runat="server" Text="Inventory Account"></telerik:RadLabel></td>
                    <td>
                        <span id="spnPickListInventoryAccount">
                            <telerik:RadTextBox ID="txtInventoryAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="20" Width="20%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInventoryAccountDescription" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="50%"></telerik:RadTextBox>
                            <img runat="server" id="img1" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtInventoryAccountId" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInventoryItem" runat="server" Text="Inventory Item"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickItem">
                            <telerik:RadTextBox ID="txtItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'
                                MaxLength="20" CssClass="input_mandatory" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtServiceNumber" runat="server" Width="0px" CssClass="input" Enabled="false"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowItem" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtStoreItemId" runat="server" Width="0px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBillingDescription" runat="server" Text="Billing Description"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadTextBox ID="txtBillingDescription" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="200" Rows="5"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
