<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanActivityReschedule" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">             
            function CloseModelWindow(gridid) {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow(gridid);
            }
            function refresh() {
                if (typeof parent.refreshScheduler === "function")
                    parent.refreshScheduler();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="divEdit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divEdit" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <div id="divEdit" runat="server">
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblPostpone">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPospone" runat="server" Text="Reschedule Date"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDatePicker ID="txtPostponeDate" runat="server" Width="120px">
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
