<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestMoreInfoList.aspx.cs"
    Inherits="Crew_CrewLicenceRequestMoreInfoList" %>

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
                <asp:Literal ID="ltCreatedBy" runat="server" Text="Created By"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblCreatedBy" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDBY")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltCreatedDate" runat="server" Text="Created Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltRequestSentBy" runat="server" Text="Request Sent By"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblRequestSentBy" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDREQUESTEDBYNAME")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltRequestSentDate" runat="server" Text="Request Sent Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblRequestSentDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltReceivedBy" runat="server" Text="Received By"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblReceivedBy" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDRECEIVEDBYNAME")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltReceivedDate" runat="server" Text="Received Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblReceivedDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltAccountStatus" runat="server" Text="Account Status"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblAccountStatus" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDACCOUNTSTATUS")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltRepresentative" runat="server" Text="Representative"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblRepresentative" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDAUTHORIZEDREP")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltDesignation" runat="server" Text="Designation"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblDesignation" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDDESIGNATION")%>'></asp:Label>
            </td>
        </tr>
    </table>
</body>
</html>
