<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareInvoiceAdminAttachments.aspx.cs"
    Inherits="AccountsAirfareInvoiceAdminAttachments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Title runat="server" ID="ttlInvoice" Text="Invoice" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachmentType" runat="server" Text="Attachment Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblAttachmentType" runat="server" AppendDataBoundItems="false"
                            RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="SetValue"
                            CssClass="readonlytextbox" Enabled="true">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; height: 500px; width: 99.6%"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
