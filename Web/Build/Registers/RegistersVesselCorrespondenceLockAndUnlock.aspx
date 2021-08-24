<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselCorrespondenceLockAndUnlock.aspx.cs" Inherits="RegistersVesselCorrespondenceLockAndUnlock" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Lock and UnLock</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCorrespondenceLockAndUnlock" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="CorrespondenceLock" runat="server" OnTabStripCommand="CorrespondenceLock_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="30%">
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblEnterPasswordtoLockandUnlock" runat="server" Text="Enter Password to Lock and Unlock"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEnterPassword" runat="server" Text="Enter Password"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPassword" runat="server" CssClass="input_mandatory" TextMode="Password" MaxLength="20"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
