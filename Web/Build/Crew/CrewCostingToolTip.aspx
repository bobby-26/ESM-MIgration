<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostingToolTip.aspx.cs"
    Inherits="Crew_CrewCostingToolTip" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head>
    <title></title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <table>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblPoNumber" runat="server" Text="Po Number:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltPoNumber" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblCrewChangeVessel" runat="server" Text="Crew Change Vessel:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltCrewChangeVessel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblCrewChangeDate" runat="server" Text="Crew Change Date:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltCrewChangeDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblPort" runat="server" Text="Port Name:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltPort" runat="server"></asp:Label>
            </td>
        </tr>
         <tr valign="top">
            <td>
                <asp:Literal ID="lblPortAgent" runat="server" Text="Port Agent:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltPortAgent" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblETA" runat="server" Text="ETA:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltETA" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblETD" runat="server" Text="ETD:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltETD" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblQuotation" runat="server" Text="Quotation No:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltQuotation" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblNumberOfOnSigners" runat="server" Text="Number Of On-Signers:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltNumberOfOnSigners" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblNumberOfOffSigners" runat="server" Text="Number Of Off-Signers:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltNumberOfOffSigners" runat="server"></asp:Label>
            </td>
        </tr>
         <tr valign="top">
            <td>
                <asp:Literal ID="lblTotal" runat="server" Text="Total Amount:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltTotal" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</body>
</html>
