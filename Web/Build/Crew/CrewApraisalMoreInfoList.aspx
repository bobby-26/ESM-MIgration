<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewApraisalMoreInfoList.aspx.cs"
    Inherits="CrewApraisalMoreInfoList" %>

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
            <td nowrap>
                <asp:Literal ID="ltLastVessel" runat="server" Text="Last Vessel : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblLastVessel" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltSignOffDate" runat="server" Text="Sign Off : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblSignOffDate" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltPresentVessel" runat="server" Text="Present Vessel : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblPresentVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELNAME")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltSignOn" runat="server" Text="Sign On : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblSignOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE", "{0:dd/MM/yyyy}") %>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltNextVessel" runat="server" Text="Next Vessel : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblNextVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDNEXTVESSEL")%>'></asp:Label>
            </td>
        </tr>        
        <tr valign="top">
            <td>
                <asp:Literal ID="ltDOA" runat="server" Text="D.O.A : "></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblDOA" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>        
    </table>
</body>
</html>
