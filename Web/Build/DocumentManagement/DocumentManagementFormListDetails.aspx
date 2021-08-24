<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormListDetails.aspx.cs" Inherits="DocumentManagement_DocumentManagementFormListDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .lblheader {
            font-weight: bold;
        }
    </style>
</telerik:RadCodeBlock>
</head>
<body>
    <table style="width: 1000px;" align="left">
        <tr>
            <td><b>Form Details</b></td>
        </tr>
        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltFileNo" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblPrimaryOwnerShip" runat="server" Text="Primary Owner Ship"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltPrimaryOwnerShip" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblSecondaryOwnerShip" runat="server" Text="Secondary Owner Ship"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltSecondaryOwnerShip" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblOtherParticipants" runat="server" Text="Other Participants"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltOtherParticipants" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblPrimaryOwnerOffice" runat="server" Text="Primary Owner Office"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltPrimaryOwnerOffice" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblSecondaryOwnerOffice" runat="server" Text="Secondary Owner Office"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltSecondaryOwnerOffice" runat="server"></asp:Label>
            </td>
        </tr>

        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblShipDept" runat="server" Text="Ship Department"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltShipDept" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblOfficeDept" runat="server" Text="Office Department"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltOfficeDept" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblShipType" runat="server" Text="Ship Type"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltShipType" runat="server"></asp:Label>
            </td>
        </tr>

        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblcountry" runat="server" Text="Country/Port"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltcountry" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblEqMaker" runat="server" Text="Equipment Maker/Model"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltEqMaker" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblPMSComp" runat="server" Text="PMS Component/Work Order"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltPMSComp" runat="server"></asp:Label>
            </td>
        </tr>

        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblActivity" runat="server" Text="Activity/Operation"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltActivity" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblTimeInterval" runat="server" Text="Time Interval"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltTimeInterval" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblProcedure" runat="server" Text="Procedure"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltProcedure" runat="server"></asp:Label>
            </td>
        </tr>

        <tr valign="top">
            <td  class="lblheader">
                <asp:Literal ID="lblRA" runat="server" Text="RA"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbtRA" runat="server"></asp:Label>
            </td>
            <td  class="lblheader">
                <asp:Literal ID="lblJHA" runat="server" Text="JHA"></asp:Literal>
            </td>
            <td >
                <asp:Label ID="lbltJHA" runat="server"></asp:Label>
            </td>
            <td  class="lblheader"></td>
            <td ></td>
        </tr>

    </table>
</body>
</html>
