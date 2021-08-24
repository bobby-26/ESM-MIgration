<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseToolTipOrderForm.aspx.cs"
    Inherits="PurchaseToolTipOrderForm" %>

<table>
    <tr valign="top">
        <td>
            <asp:literal id="lblNumber" runat="server" text="Number:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltNumber" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblFormTitle" runat="server" text="Form Title:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltFormTitle" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblVendor" runat="server" text="Vendor:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltVendor" runat="server"></asp:label>
        </td>
    </tr>
   <%-- <tr valign="top">
        <td>
            <asp:literal id="lblFormType" runat="server" text="Form Type:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltFormType" runat="server"></asp:label>
        </td>
    </tr>--%>
    <tr valign="top">
        <td>
            <asp:literal id="lblFromStatus" runat="server" text="From Status"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltFromStatus" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblBudgetCode" runat="server" text="Budget Code:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltBudgetCode" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblApprovedDate" runat="server" text="Approved Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltApprovedDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblOrderedDate" runat="server" text="Ordered Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltOrderedDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblReceivedDate" runat="server" text="Imported Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltReceivedDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblType" runat="server" text="Type:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltType" runat="server"></asp:label>
        </td>
    </tr>
     <tr valign="top">
        <td>
            <asp:literal id="lblVesselReceivedDate" runat="server" text="Vessel Received Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltVesselReceivedDate" runat="server"></asp:label>
        </td>
    </tr>
</table>
