<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewInvoiceToolTip.aspx.cs"
    Inherits="DashboardCrewInvoiceToolTip" %>

<table>
    <tr valign="top">
        <td>
            <asp:literal id="lblInvNo" runat="server" text="Invoice Number:"></asp:literal>
        </td>
        <td>
            <asp:label id="lblInvNovalue" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblSupCode" runat="server" text="Supplier Code:"></asp:literal>
        </td>
        <td>
            <asp:label id="lblSupCodeValue" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblSupName" runat="server" text="Supplier Name:"></asp:literal>
        </td>
        <td>
            <asp:label id="lblSupNameValue" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblVendorInvno" runat="server" text="Vendor Invoice Number:"></asp:literal>
        </td>
        <td>
            <asp:label id="lblVendorInvNoValue" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblInvStatus" runat="server" text="Invoice Status:"></asp:literal>
        </td>
        <td>
            <asp:label id="lblInvStatusValue" runat="server"></asp:label>
        </td>
    </tr>

</table>
