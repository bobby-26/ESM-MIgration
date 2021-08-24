<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectBillingItemFilter.aspx.cs"
    Inherits="AccountsProjectBillingItemFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProjectBillingName" runat="server" Text="Project Billing Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtProjectBillingName" MaxLength="200" CssClass="input" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblProjectBillingGroup" runat="server" Text="Project Billing Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucProjectBillingGroups" Width="150px" runat="server" AppendDataBoundItems="true"
                            QuickTypeCode="98" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBillingUnit" runat="server" Text="Billing Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlUnit ID="ucBillingUnit" runat="server" CssClass="input"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ucCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="input" runat="server" AppendDataBoundItems="true" AutoPostBack="false" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
