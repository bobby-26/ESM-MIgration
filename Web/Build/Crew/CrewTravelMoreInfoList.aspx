<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelMoreInfoList.aspx.cs"
    Inherits="Crew_CrewTravelMoreInfoList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
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
            <td nowrap>
                <asp:Literal ID="ltDOB" runat="server" Text="D.O.B"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblDOB" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltPPNo" runat="server" Text="PP No"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblPPNo" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltCDC" runat="server" Text="CDC No"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblCDC" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltZone" runat="server" Text="Zone"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltUSVISA" runat="server" Text="US VISA"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblUSVisa" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDUSVISANUMBER")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltOtherVisa" runat="server" Text="Other VISA"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblOtherVisa" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERVISADETAILS") %>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltAirport" runat="server" Text="Airport"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblAirport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTNAME") %>'></asp:Label>
            </td>
        </tr>
         <tr valign="top">
            <td>
                <asp:Literal ID="ltAirportCity" runat="server" Text="Airport City"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblAirportCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTCITY") %>'></asp:Label>
            </td>
        </tr>
    </table>
</body>
</html>
