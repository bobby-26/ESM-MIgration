<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewToolTipFollowupRemarks.aspx.cs"
    Inherits="CrewToolTipFollowupRemarks" %>

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
                <asp:Literal ID="lblcontactdate" runat="server" Text="Last Contact Date:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblLastcontactdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE") %>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lbllastremarks" runat="server" Text="Last Remarks:"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTREMARKS") %>'></asp:Label>
            </td>
        </tr>
    </table>
</body>
</html>
