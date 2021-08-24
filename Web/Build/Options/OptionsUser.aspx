<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsUser.aspx.cs" Inherits="OptionsUser" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlMappedDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserType" Src="~/UserControls/UserControlUserType.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuUserAdmin" runat="server" OnTabStripCommand="UserAdmin_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuSecurityUser" runat="server" OnTabStripCommand="SecurityUser_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtUserName" MaxLength="100" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td rowspan="4">
                        <telerik:RadLabel runat="server" ID="lblMessage" Font-Bold="true" ForeColor="Red"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassword" runat="server" Text="Password"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNewPassword" MaxLength="100" Width="360px" TextMode="Password"
                            CssClass="input_mandatory">
                        </telerik:RadTextBox>
                    </td>
                  
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblConfirmPassword" runat="server" Text="Confirm Password"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtConfirmNewPassword" MaxLength="100" Width="360px"
                            TextMode="Password" CssClass="input_mandatory">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEMail" MaxLength="100" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" MaxLength="100" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td><td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" MaxLength="100" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td><td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtShortCode" MaxLength="5" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td><td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department runat="server" ID="ucDepartment" CssClass="input" AppendDataBoundItems="true"></eluc:Department>
                    </td><td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYesNo" runat="server" Text="Active Yes/No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucActiveYesNo" HardTypeCode="44" CssClass="input" />
                    </td><td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserType" runat="server" Text="User Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserType runat="server" ID="ucUserType" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td><td></td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
