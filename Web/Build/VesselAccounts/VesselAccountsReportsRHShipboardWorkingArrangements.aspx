<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportsRHShipboardWorkingArrangements.aspx.cs" Inherits="VesselAccounts_VesselAccountsReportsRHShipboardWorkingArrangements" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule of Shipboard Working arrangements</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:TabStrip ID="MenuShipboardWorking" runat="server" OnTabStripCommand="MenuShipboardWorking_TabStripCommand"></eluc:TabStrip>--%>
            <table width="100%">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel runat="server" ID="lblVessel" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="95%">
                        <telerik:RadTextBox runat="server" ID="txtVessel" Width="20%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 570px; width: 100%" frameborder="0"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
