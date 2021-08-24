<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsEmailMenu.aspx.cs"
    Inherits="OptionsEmailMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOptionEmailMenu" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <img runat="server" src="<%$ PhoenixTheme:images/eloglogo.png %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink runat="server" NavigateUrl="OptionsEmail.aspx"
                        Text="Write Mail"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink runat="server" NavigateUrl="OptionsEmailDraft.aspx"
                        Text="Draft Mail"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink runat="server" NavigateUrl="OptionsEmailSent.aspx"
                        Text="Sent Mail"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="OptionsEmailDelete.aspx"
                        Text="Delete Mail"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
