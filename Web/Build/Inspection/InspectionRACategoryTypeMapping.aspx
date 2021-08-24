<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACategoryTypeMapping.aspx.cs" Inherits="Inspection_InspectionRACategoryTypeMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Title runat="server" ID="ucTitle" Text="Type Mapping" ShowMenu="false" visible="false"> </eluc:Title>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Type Mapping">
            </eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>&nbsp;&nbsp<telerik:RadLabel ID="lblCategory" runat="server" Font-Bold="true" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" ReadOnly="true" Width="420px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">&nbsp;&nbsp<telerik:RadLabel ID="lblType" runat="server" Font-Bold="true" Text="Process"></telerik:RadLabel>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <asp:CheckBoxList ID="cblType" runat="server" RepeatDirection="Vertical" RepeatColumns="1">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
