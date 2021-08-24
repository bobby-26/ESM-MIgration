<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMSCATMapping.aspx.cs" Inherits="InspectionMSCATMapping" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMSCATMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand"></eluc:TabStrip>
            <table width="100%" id="tblMapping">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Font-Bold="true"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadComboBox ID="ddlMapping" runat="server" Width="30%" AutoPostBack="true" OnTextChanged="ddlMapping_Changed">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSource" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <asp:CheckBoxList ID="cblSource" runat="server" RepeatDirection="Vertical" RepeatColumns="1"></asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
