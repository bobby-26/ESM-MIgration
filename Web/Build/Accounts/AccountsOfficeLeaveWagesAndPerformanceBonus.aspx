<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficeLeaveWagesAndPerformanceBonus.aspx.cs"
    Inherits="AccountsOfficeLeaveWagesAndPerformanceBonus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leave Wages And Performance Bonus</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 55 + "px";
            }
        </script>

    </telerik:RadCodeBlock>

</head>
<body onload="resize();" onresize="resize();">
    <form id="frmLeaveWagesPerformanceBonus" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="Title1" Text="Leave Wages And Performance Bonus" ShowMenu="True" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuReportLeaveWagesPerformanceBonus" runat="server" OnTabStripCommand="MenuReportLeaveWagesPerformanceBonus_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 200px; width: 99.5%;"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
