<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardShipRiskProfile.aspx.cs" Inherits="Inspection_InspectionDashboardShipRiskProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <br />
<%--        <div class="panel panel-success" style="height: 250px">
            <table></table>
            <asp:Literal ID="lblVessel" runat="server" Text=""></asp:Literal>
            </div>--%>

        <table cellpadding="3" cellspacing="3" width="80%" id="shipprofile">
            <tr>
                <td>
                    <asp:Label ID="lblshiprisk" runat="server" Font-Size="Medium"  Text="Risk Profile "></asp:Label>
                </td>
                <td>
                    <asp:LinkButton ID="lblshipriskresult" Font-Size="Medium" Font-Underline="true" runat="server" Text="-"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblpriority" runat="server" Font-Size="Medium" Text="Priority  "></asp:Label>
                </td>  
                <td>
                    <asp:Label ID="txtpriority" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                </td>                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbllastinspected" runat="server" Font-Size="Medium" Text="Last Inspected "></asp:Label>
                </td>  
                <td>
                    <asp:LinkButton ID="lbllastinspdate" runat="server" Font-Underline="true" Font-Size="Medium" Text="-"></asp:LinkButton>
                </td>                
            </tr>
            <tr><td><br /></td></tr>            
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblnote" runat="server" Text=""></asp:Label>
                </td>                 
            </tr>
        </table>

    </form>
</body>
</html>

