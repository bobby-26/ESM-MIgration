<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogELCheifEngineerSignatureAmend.aspx.cs" Inherits="ElectricLogELCheifEngineerSignatureAmend" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Duty Engineer Sign </title>
    <telerik:RadCodeBlock runat="server" ID="radcodeblock">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Scripts.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Status ID="ucStatus" runat="server" Visible="false"></eluc:Status>
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <table>
            <tr>
                <td colspan="2" style="text-align:center"><telerik:RadLabel runat="server" Text="Are you sure you want to sign in ?"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td colspan="2" style="width:200px;text-align:center;"><telerik:RadLabel runat="server" Text="Existing data will be amended."></telerik:RadLabel></td>
            </tr>
            <tr>
                <td style="width:200px"><telerik:RadLabel runat="server" Text="Password"></telerik:RadLabel></td>
                <td><telerik:RadTextBox runat="server" ID="txtPassword" Width="250px" TextMode="Password"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td style="text-align:center"><telerik:RadButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"></telerik:RadButton></td>
                <td style="text-align:center"><telerik:RadButton runat="server" ID="btnSubmit" Text="Yes" OnClick="btnSubmit_Click"></telerik:RadButton></td>
            </tr>
        </table>
    </form>
</body>
</html>
