<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchMoreInfoList.aspx.cs" Inherits="Crew_CrewBatchMoreInfoList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head>
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
    <table>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="txtStartDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTARTDATE")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="lblEndDate" runat="server" Text="End Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="txtEndDate" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.ENDDATE")%>'></asp:Label>
            </td>
        </tr>
         <tr valign="top">
            <td>
                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="txtCourse" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>'></asp:Label>
            </td>
        </tr>
              
    </table>
</body>
</html>