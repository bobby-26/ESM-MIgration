<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRemarksPopup.aspx.cs"
    Inherits="PlannedMaintenanceRemarksPopup" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">              
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function refresh() {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrderReschedule" runat="server" OnTabStripCommand="MenuWorkOrderReschedule_TabStripCommand"></eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblremarks" runat="server" Text='Remarks'></telerik:RadLabel>
                </td>
                 <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>          
        </table>     
    </form>
</body>
</html>
