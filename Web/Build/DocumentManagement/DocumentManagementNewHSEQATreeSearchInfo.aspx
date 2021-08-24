<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementNewHSEQATreeSearchInfo.aspx.cs" Inherits="DocumentManagementNewHSEQATreeSearchInfo" %>

<!DOCTYPE html>

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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <table width="99%">
            <tr>
                <td width="7%" valign="Top">
                    <img runat="server" src="<%$ PhoenixTheme:images/search.png %>" alt="Search" height="16" width="16" />
                </td>
                <td width="93%">
                    <telerik:RadLabel ID="lblInfo" runat="server" Font-Bold="true" Font-Size="Larger"></telerik:RadLabel>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
