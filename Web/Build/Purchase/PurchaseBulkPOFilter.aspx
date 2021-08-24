<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOFilter.aspx.cs"
    Inherits="PurchaseBulkPOFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <asp:UpdatePanel runat="server" ID="pnlDiscussion">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand"></eluc:TabStrip>
                <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; border: none; width: 100%">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Bulk PO Reference Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReferenceNumber" runat="server" Width="240px" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Vendor" runat="server" Text="Vendor"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <telerik:RadTextBox ID="txtVendorNumber" runat="server" Width="60px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true);"
                                        Text=".." />
                                    <telerik:RadTextBox ID="txtVendorId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblInvoiceReferenceNumber" runat="server" Text="Invoice Reference Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtInvoiceReferenceNumber" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Currency ID="ucCurrency" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
