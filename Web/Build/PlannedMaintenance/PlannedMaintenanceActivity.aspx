<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceActivity.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceActivity" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function clientTimeSelected(sender, e) {
                document.getElementById("btnchangetimings").click();
            }
            function refresh() {
                top.closeTelerikWindow('activity', 'maint');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server">
             <asp:Button ID="btnchangetimings" runat="server" Text="cmdHiddenSubmit" OnClick="btnchangetimings_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtActivity" runat="server" Enabled="false" Width="200px"></telerik:RadTextBox>                       
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStart" runat="server" Text="Start Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="txtStartDate" runat="server" Enabled="false">
                        </telerik:RadDatePicker>                        
                    </td>    
                     <td >
                        <telerik:RadTimePicker ID="txtStartTime" runat="server" Enabled="false"  TimeView-OnClientTimeSelected="clientTimeSelected" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"  Width="63px">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompleted" runat="server" Text="End Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="txtCompletedDate" runat="server" Enabled="false" >
                        </telerik:RadDatePicker>                        
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="txtCompletedTime" runat="server" TimeView-OnClientTimeSelected="clientTimeSelected" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm" Width="63px">
                        </telerik:RadTimePicker>
                    </td>
                </tr>                
            </table>
            <b>Note: The Start and end times are pre-filled based on the est planned start and end times. User can change (edit) these timings as applicable prior saving.</b>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
