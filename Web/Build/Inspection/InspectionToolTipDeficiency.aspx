<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionToolTipDeficiency.aspx.cs"
    Inherits="InspectionToolTipDeficiency" %>

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
    <table>
        <tr valign="top">
            <td>
                <telerik:RadLabel ID="lblNumber" runat="server" Text="Number:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lbltNumber" runat="server" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <telerik:RadLabel ID="lblDeficidenyDetails" runat="server" Text="Def. Details:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblText" runat="server" Width="150px" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action:" Font-Size="Small"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lbltCorrectiveAction" runat="server" Width="150px" Font-Size="Small"></telerik:RadLabel>
            </td>
        </tr>
    </table>
</body>
</html>
