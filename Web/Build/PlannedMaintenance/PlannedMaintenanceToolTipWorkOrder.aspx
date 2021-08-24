<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceToolTipWorkOrder.aspx.cs"
    Inherits="PlannedMaintenanceToolTipWorkOrder" %>

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
            <asp:literal id="lblStartedDate" runat="server" text="Started Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltStartedDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblLastDoneDate" runat="server" text="Last Done Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltLastDoneDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblCompletedDate" runat="server" text="Completed Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltCompletedDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblDueDate" runat="server" text="Due Date:"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltDueDate" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblWindowDays" runat="server" text="Window (Days):"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltWindowDays" runat="server"></asp:label>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:literal id="lblOverdueDays" runat="server" text="Overdue (Days):"></asp:literal>
        </td>
        <td>
            <asp:label id="lbltOverdueDays" runat="server"></asp:label>
        </td>
    </tr>
</table>
