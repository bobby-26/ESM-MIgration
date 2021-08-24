<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPNIInfoList.aspx.cs" Inherits="Crew_CrewTravelPNIInfoList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>
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
                <asp:Literal ID="ltmeicalcaseid" runat="server" Text="Medical Case ID: "></asp:Literal>
            </td>
            <td> <asp:Label ID="lblmeicalcaseid" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREFERENCENO")%>'></asp:Label>
            </td>
        </tr>
         <tr valign="top">
            <td nowrap>
                <asp:Literal ID="Literal1" runat="server" Text="Employee Name: "></asp:Literal>
            </td>
            <td> <asp:Label ID="lblempname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIRSTNAME")%>'></asp:Label>
            </td>
        </tr>
      
              
    </table>
</body>
</html>
