<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsEmployeeAllotmentRequestToolTip.aspx.cs"
    Inherits="AccountsEmployeeAllotmentRequestToolTip" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html>
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadLabel ID="lblsideletter" Visible="false" runat="server" Text="Side letter allotment to process. "></telerik:RadLabel>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <table style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1" cellspacing="2">
           <%-- <tr>
                <td colspan="5" style="align-content: center;"><b>
                    </td>
            </tr>--%>
           
            <%=BindData()%>
        </table>
    </telerik:RadCodeBlock>
</body>
</html>
