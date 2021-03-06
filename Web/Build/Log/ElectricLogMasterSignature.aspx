<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogMasterSignature.aspx.cs" Inherits="Log_ElectricLogMasterSignature" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log Book Master Signature</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>
    <style>
        .displayNone {
            display:none;
        }
        .container {
            padding: 20px 20px; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <telerik:radwindowmanager rendermode="Lightweight" id="RadWindowManager1" runat="server" enableshadow="true">
        </telerik:radwindowmanager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="displayNone" />

        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>


        <div class="container">
            <h3>
                <telerik:RadLabel Text="Enter master username and password to sign the current page" ID="lblheading" runat="server"></telerik:RadLabel>
            </h3>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUsername" runat="server" Text="Username"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUsername" runat="server" CssClass="input_mandatory" autocomplete="off"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="txtUserCode" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassword" runat="server" Text="Password"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="input_mandatory" autocomplete="off"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadButton ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Ok"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMasterFirstName" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblMasterLastName" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblLoggedUser" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
