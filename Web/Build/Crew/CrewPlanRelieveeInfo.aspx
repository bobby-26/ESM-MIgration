<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelieveeInfo.aspx.cs" Inherits="CrewPlanRelieveeInfo" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html>
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>        
    </telerik:RadCodeBlock>
</head>
<body>
    <table style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1" cellspacing="2">

        <tr>
            <td>Off Signer</td>
            <td>
                <asp:Label ID="txtoffsignername" runat="server" Text="" />
            </td>
            <td>Vessel </td>
            <td>
                <asp:Label ID="txtVessel" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td>Contract Expired On</td>
            <td>
                <asp:Label ID="txtcontractExpired" runat="server" Text="" />
            </td>
            <td>Rank</td>
            <td>
                <asp:Label ID="txtoffsignerRank" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td>On Signer Name</td>
            <td>
                <asp:Label ID="txtonsignername" runat="server" Text="" />
            </td>
            <td>Planned Relief</td>
            <td>
                <asp:Label ID="txtPlannedReliefDate" runat="server" Text="" />

            </td>
        </tr>
        <tr>
            <td>Planned Port</td>
            <td>
                <asp:Label ID="ucport" runat="server" Text="" />
            </td>
            <td>Date of Readiness</td>
            <td>
                <asp:Label ID="txtDateofReadiness" runat="server" Text="" />

            </td>
        </tr>
        <tr>
        </tr>
        <tr>

            <td>Remarks</td>
            <td>
                <asp:Label ID="txtcrewchangeremarks" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</body>
</html>
