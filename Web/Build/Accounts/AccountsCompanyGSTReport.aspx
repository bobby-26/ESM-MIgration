<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCompanyGSTReport.aspx.cs" Inherits="Accounts_AccountsCompanyGSTReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GST Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLedgerGeneral" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" RenderMode="Lightweight" EnableShadow="true"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" />
            <eluc:TabStrip ID="MenuGST" runat="server" OnTabStripCommand="MenuGST_TabStripCommand"></eluc:TabStrip>
            <table style="width: 70%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <eluc:Date ID="txtFromDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <eluc:Date ID="txtToDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>

            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 524px; width: 99.5%;"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
