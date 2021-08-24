<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWRHNCTypeCountbyNOWVisual.aspx.cs" Inherits="Inspection_InspectionWRHNCTypeCountbyNOWVisual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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

</telerik:RadCodeBlock></head>
<body>

    <script>
        function createChart(params, filterParams) {
            sn.visual.interaction.create(params, window.location.href, filterParams);
        }
    </script>
    <form id="form1" runat="server">
        <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
        </ajaxtoolkit:toolkitscriptmanager>
        <asp:UpdatePanel runat="server" ID="pnlCrewReportEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; height: 0;">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="NC Type Count by Nature Of Work"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />
        <br />
        <br />
        <div id="nctypecountnowbyyear-1"></div>
        <div id="nctypecountnowbyquarter-2"></div>
        <div id="nctypecountnowbymonth-3"></div>
        <div id="nctypecountnowbyfleet-4"></div>
        <div id="nctypecountnowbyvessel-5"></div>
        <div id="nctypecountbynow-6"></div>
        
    </form>

</body>
</html>



