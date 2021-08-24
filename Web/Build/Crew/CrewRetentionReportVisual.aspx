<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRetentionReportVisual.aspx.cs" Inherits="Crew_CrewRetentionReportVisual" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" href="../css/Theme1/sn-visual/sn-visual-v2.css" />
        <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
        <script src="../js/d3/d3.min.js"></script>
        <script src="../js/sn-visual/sn-visual-common-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-tooltip-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-about-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-theme-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-bar-vertical-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-bar-horizontal-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-pie-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-donut-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-bar-vertical-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-line-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-table-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-interaction-v2.js"></script>
        <script src="../js/sn-visual/sn-visual-bar-grouped-vertical-v2.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <script>
        function createChart(params, filterParams) {
            sn.visual.interaction.create(params, window.location.href, filterParams);
        }
    </script>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
        </telerik:RadAjaxPanel>
        <br />
        <br />
        <br />
        <div>
            <div id="crew-retention-avg-length-of-contract-rankgroup"></div>
            <div id="crew-retention-avg-length-of-contract-rank"></div>
        </div>
    </form>
</body>
</html>
