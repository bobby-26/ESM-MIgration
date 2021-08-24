<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseStoreItemPriceVisual.aspx.cs" Inherits="Purchase_PurchaseStoreItemPriceVisual" %>

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
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
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
</head>
<body>
    <script>
        function createChart(params, filterParams) {
            sn.visual.interaction.create(params, window.location.href, filterParams);
        }
    </script>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlStoreItemPriceReportEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

        </telerik:RadAjaxPanel>
        <br />
        <br />
        <br />
        <div id="purchaseyearchart"></div>
        <div id="purchaseportchart"></div>
        <div id="purchasevesselchart"></div>
        <div id="purchaseportdetailchart"></div>
    </form>
</body>
</html>
