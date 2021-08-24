<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsChangePassword.aspx.cs"
    Inherits="OptionsChangePassword" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
   
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSecurityChangePassword" runat="server" OnTabStripCommand="SecurityChangePassword_TabStripCommand">
        </eluc:TabStrip>
        <eluc:Status runat="server" ID="ucStatus" />
        <table cellpadding="8">
            <tr>
                <td colspan="2">
                    <Telerik:RadLabel ID="lblEnteryourCurrentPasswordNewPasswordandConfirmNewPasswordtochangeyourpassword"
                        runat="server" Text="Enter your Current Password, New Password and Confirm New Password to change your password."></Telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <Telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></Telerik:RadLabel>
                </td>
                <td>
                    <Telerik:RadTextBox runat="server" ID="txtUserName" MaxLength="100" Width="360px" CssClass="input_mandatory"></Telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <Telerik:RadLabel ID="lblCurrentPassword" runat="server" Text="Current Password"></Telerik:RadLabel>
                </td>
                <td>
                    <Telerik:RadTextBox runat="server" ID="txtCurrentPassword" MaxLength="100" Width="360px"
                        TextMode="Password" CssClass="input"></Telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <Telerik:RadLabel ID="lblNewPassword" runat="server" Text="New Password"></Telerik:RadLabel>
                </td>
                <td>
                    <Telerik:RadTextBox runat="server" ID="txtNewPassword" MaxLength="100" Width="360px" TextMode="Password"
                        CssClass="input"></Telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <Telerik:RadLabel ID="lblConfirmNewPassword" runat="server" Text="Confirm New Password"></Telerik:RadLabel>
                </td>
                <td>
                    <Telerik:RadTextBox runat="server" ID="txtConfirmNewPassword" MaxLength="100" Width="360px"
                        TextMode="Password" CssClass="input"></Telerik:RadTextBox>
                </td>
            </tr>
        </table>
   
    </form>
</body>
</html>
