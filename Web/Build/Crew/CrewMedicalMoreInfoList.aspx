<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedicalMoreInfoList.aspx.cs" Inherits="Crew_CrewMedicalMoreInfoList" %>

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
                <asp:Literal ID="ltAppointmentDate" runat="server" Text="Appointment Date"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblAppointmentDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPOINTMENTDATE")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltPaymentType" runat="server" Text="Payment Type"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblPaymentType" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDPAYMENTTYPE")%>'></asp:Label>
            </td>
        </tr>
              
    </table>
</body>
</html>