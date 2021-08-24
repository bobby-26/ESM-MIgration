<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InjuryAnalysisDataVisual.aspx.cs" Inherits="Inspection_InjuryAnalysisDataVisual" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>

        <br />
        <br />
        <br />
        <div id="injuryanalysis-1"></div>
        <div id="injuryanalysis-2"></div>
        <div id="injuryanalysis-3"></div>
        <div id="injuryanalysis-4"></div>
        <div id="injuryanalysis-5"></div>
        <div id="injuryanalysis-6"></div>
        <div id="injuryanalysis-7"></div>
        <div id="injuryanalysis-8"></div>
        <div id="injuryanalysis-9"></div>
        <div id="injuryanalysis-10"></div>
        <div id="injuryanalysis-11"></div>
        <div id="injuryanalysis-12"></div>

    </form>

</body>
</html>
