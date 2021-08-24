<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountEarningDeductionToolTip.aspx.cs" Inherits="VesselAccounts_VesselAccountEarningDeductionToolTip" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html>
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <table style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1" cellspacing="2">
            <%=BindData()%>
        </table>
    </telerik:RadCodeBlock>
</body>
</html>
