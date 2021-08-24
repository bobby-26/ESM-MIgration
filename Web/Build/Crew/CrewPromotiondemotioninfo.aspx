<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPromotiondemotioninfo.aspx.cs" Inherits="CrewPromotiondemotioninfo" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head runat="server">
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
    <table id="tableContent" runat="server" style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1" cellspacing="2">
        <tr><td colspan="4"><b>PROMOTION/ DEMOTION</b></td></tr>
        <tr>
            <td>Rank From            
            </td>
            <td>Rank To</td>
            <td>Date</td>
            <td>Status</td>            
        </tr>      
    </table>

</body>
</html>
