<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionToolTipUnsafeActsConditions.aspx.cs"
    Inherits="InspectionToolTipUnsafeActsConditions" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <table runat="server" width="100%" height="100%" >
        <tr valign="top">
            <td width="50%">
                <telerik:RadLabel ID="lblNumber" runat="server" Text="Number:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td width="50%">
                <telerik:RadLabel ID="lbltNumber" runat="server" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
        <tr valign="top">
            <td width="50%">
                <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Width="120px" Text="Corrective Action:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td width="50%">
                <telerik:RadLabel ID="lbltCorrectiveAction" runat="server" Width="150px" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
        <tr valign="top">
            <td width="50%">
                <telerik:RadLabel ID="lblRootCause" runat="server" Text="Root Cause:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td width="50%">
                <telerik:RadLabel ID="txtRootCause" runat="server" Width="150px" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
    </table>
</body>
</html>


