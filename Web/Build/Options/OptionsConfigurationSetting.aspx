<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsConfigurationSetting.aspx.cs" Inherits="OptionsConfigurationSetting" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuConfigurationEdit" runat="server" OnTabStripCommand="MenuConfigurationEdit_TabStripCommand"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstallCode" runat="server" Text="Install Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstallCode" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSyncDBPath" runat="server" Text="Sync DB Path"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSyncPath" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachmentPath" runat="server" Text="Attachment Path"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAttachmentPath" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOfficeMail" runat="server" Text="Office Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMailPath" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSMTPHost" runat="server" Text="SMTP Host"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSmtpHost" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSMTPPort" runat="server" Text="SMTP Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSmtpPort" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPOP3Host" runat="server" Text="POP3 Host"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPop3Host" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPOP3Port" runat="server" Text="POP3 Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPop3Port" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMailExePath" runat="server" Text="POP3 Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMailExePath" runat="server" CssClass="input" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                 <tr>
                 
                    <td>
                        <telerik:RadTextBox ID="txtusername" runat="server" CssClass="input" Width="350px" Visible="false"></telerik:RadTextBox>
                         <telerik:RadTextBox ID="txtpassword" runat="server" CssClass="input" Width="350px" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                 
                    <td>
                       
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
