<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewCrewList.aspx.cs"
    Inherits="DashboardCrewCrewList" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
        <script type="text/javascript">
            function pageLoad() {
                var code = document.querySelector('[title="Code"]');
                if (code != null) {
                    code.parentElement.style.display = "none";
                }

                var headerRow = document.querySelectorAll(".rpgRowHeaderZoneDiv table tbody tr");
                var dataRow = document.querySelectorAll(".rpgContentZoneDiv table tbody tr");
                for (var i = 0; i < headerRow.length; i++) {
                    var row = headerRow[i]
                    var data = dataRow[i];
                    row.style.height = row.offsetHeight + "px";
                    data.style.height = row.offsetHeight + "px";
                }
            }
            function BindMeasureResult(measureid, rankid, args) {
                document.getElementById("hdnMeasureid").value = measureid;
                document.getElementById("hdnRankid").value = rankid;
                document.getElementById("args").value = args
                __doPostBack("<%=hdnButton.UniqueID %>", "");
            }
            function setSize() {
                var $ = $telerik.$;
                var wh = $(window).height();
                var top = document.querySelector("#dashWrap");
                var pnl = document.querySelector("#pnl");
                if (pnl.style.position == "")
                    pnl.style.height = (wh - top.offsetHeight) + "px";

                var rH = pnl.children[0].offsetHeight + pnl.children[1].offsetHeight;
                //console.log(pnl.offsetHeight, rH);
                var sender = $find("<%= gvMeasureResult.ClientID %>");
                 var scrollArea = sender.GridDataDiv;
                 var gridHeaderHeight = (sender.GridHeaderDiv) ? sender.GridHeaderDiv.offsetHeight : 0;
                 var gridTopPagerHeight = (sender.TopPagerControl) ? sender.TopPagerControl.offsetHeight : 0;
                 var gridFooterHeight = (sender.GridFooterDiv) ? sender.GridFooterDiv.offsetHeight : 0;
                 var gridPagerHeight = (sender.PagerControl) ? sender.PagerControl.offsetHeight : 0;
                 var gridHeight = (pnl.offsetHeight - rH - gridHeaderHeight - gridPagerHeight - gridTopPagerHeight - gridFooterHeight - 10);
                 scrollArea.style.height = (gridHeight < 0 ? 200 : gridHeight) + "px";
                //console.log(gridHeight);
             }
             window.onresize = window.onload = resize;
             function toggle() {
                 var pnl = document.querySelector("#pnl");
                 console.log(pnl.toggle);
                 if (pnl.toggle == null || pnl.toggle == "1") {
                     pnl.toggle = "0";
                     pnl.style.height = "100%";
                     pnl.style = "position:absolute;top:0px;height:100%;width:100%;background-color:white;z-index:99";
                 }
                 else {
                     pnl.toggle = "1";
                     pnl.style = "";
                 }
             }
             function resize() {
                 setTimeout(function () { setSize(); }, 200);
             }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .mlabel {
            display: inline;
            padding: .2em .6em .3em;
            font-size: 82%;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: .25em;
            cursor: pointer;
        }

        html .t-container-fluid,
        html .t-row-wrap > .t-row {
            max-width: none;
        }

        /*.rgMasterTable {
            table-layout: auto !important;
        }*/
    </style>
</head>
<body>
     <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:HiddenField ID="hdnMeasureid" runat="server" />
        <asp:HiddenField ID="hdnRankid" runat="server" />
        <asp:HiddenField ID="args" runat="server" />
        <asp:Button ID="hdnButton" runat="server" CssClass="hidden" OnClick="hdnButton_Click" />

        <div class="db_container">
            <div id="dashWrap">
                <div class="icoBlkWrap">
                    <div id="icoTabDiv">
                        <%--<div id="sideExpander" onclick="javascript:ResizeMenu(this);calcW('dataMenu');" style="width: 100px;"><span>◄</span>►</div>--%>
                        <div id="techtab" class="icoTabItem" data-tab="techIcons">Technical</div>
                        <div id="crwid" class="icoTabItem icoTabActive" data-tab="crewIcons" runat="server">Crew</div>
                    </div>
                    <div id="crewIcons" class="icoBlkDiv" runat="server">
                    </div>
                </div>
                <div id="popupDiv" class="dialDiv1">
                    <div id="gMain"></div>

                </div>
                <div id="dataVizWrap">
                    <div id="ucmenu" runat="server">
                        <div id="dataMenu">
                            <telerik:RadPivotGrid ID="GvCrew" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="GvCrew_NeedDataSource"
                                AllowPaging="false" AllowFiltering="true" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="GvCrew_CellDataBound"
                                ShowColumnHeaderZone="true" ShowDataHeaderZone="true" ShowRowHeaderZone="true" Height="100%" TotalsSettings-RowsSubTotalsPosition="None"
                                OnItemCommand="GvCrew_ItemCommand" EnableViewState="true">
                                <ClientSettings>
                                    <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                                    <Scrolling SaveScrollPosition="true" AllowVerticalScroll="true" />
                                </ClientSettings>
                                <Fields>
                                    <telerik:PivotGridColumnField DataField="FLDRANKNAME" Caption="Rank" SortOrder="None">
                                        <CellTemplate>
                                            <asp:Label ID="Label1" runat="server" CssClass="rotate" ToolTip='<%# Container.DataItem %>'>
                                                 <%# Container.DataItem %>
                                            </asp:Label>
                                        </CellTemplate>
                                    </telerik:PivotGridColumnField>
                                    <telerik:PivotGridRowField DataField="FLDMEASURENAME" Caption="Measure" CellStyle-Width="250px">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridRowField DataField="FLDMEASUREID" Caption="Code" CellStyle-Width="1px" CellStyle-CssClass="hidden">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridAggregateField DataField="FLDMEASURE" Caption="Count" Aggregate="Sum" IgnoreNullValues="true" CellStyle-Width="50px"></telerik:PivotGridAggregateField>
                                </Fields>
                            </telerik:RadPivotGrid>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnl">
                <div id="otisTitleBar" class='otisTitleBar' onclick='toggle();setSize();' runat="server">
                    <div id='otisDivLbl' class='titleLabel'>
                        <asp:Literal ID="lblResult" runat="server" Text="Result"></asp:Literal>
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                        <b>
                            <asp:Label ID="lblLastScheduleDate" runat="server" Text="Last update on:"></asp:Label></b>
                        <b>
                            <asp:Label ID="lblModifiedDate" runat="server"></asp:Label></b>
                    </div>
                    <div id='chartDivBtn'>
                        <span class='btnExpand'>
                            <svg width='16' height='16' viewbox='0 0 1792 1792' xmlns='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg>
                        </span>
                    </div>

                </div>
                <div id="lwr3rDiv" runat="server">
                    <div id="DataDiv" style="width: 100%">
                        <eluc:TabStrip ID="gvExport" runat="server" OnTabStripCommand="gvExport_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid ID="gvMeasureResult" runat="server" RenderMode="Lightweight" OnNeedDataSource="gvMeasureResult_NeedDataSource" OnItemDataBound="gvMeasureResult_ItemDataBound"
                            OnItemCommand="gvMeasureResult_ItemCommand" AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnPreRender="gvMeasureResult_PreRender">
                            <MasterTableView TableLayout="Fixed">
                                <Columns>
                                </Columns>
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Requisitions per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                               <%-- <Resizing AllowColumnResize="true" />--%>
                            </ClientSettings>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
           <script type="text/javascript">
            var svgLnk = "http://www.w3.org/2000/svg";
            var $ = $telerik.$;
            $(document).ready(function () {
                calcH('lwr3rDiv');
                //var otisHdr = "<div class='otisTitleBar' onclick='toggleFullscreen(lwr3rDiv, otisIframe)'><div id='otisDivLbl' class='titleLabel'>OTIS INFORMATION</div><div id='chartDivBtn'><span class='btnExpand'><svg width='16' height='16' viewBox='0 0 1792 1792' xmlns ='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg></span></div></div>";
            });

            $("#idOTIS").click(function () {
                $("#ucmenu").html("<div></div>")
                $(".otisTitleBar").html("<div id='otisDivLbl' class='titleLabel'>OTIS INFORMATION</div><div id='chartDivBtn'><span class='btnExpand'><svg width='16' height='16' viewBox='0 0 1792 1792' xmlns ='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg></span></div>");
                $("#lwr3rDiv").html("<iframe id='otisIframe' width='100%' height='600px' src='https://system.stratumfive.com/otis/index.html' frameborder='0'></iframe>");
            });

            // Dashboard tab change click 
            $("#techtab").click(function () {
                location.href = "../Dashboard/DashboardCommon.aspx?ModuleType=" + "1";
            });

            //Resize window in zoom in/out
            $(window).resize(function () {
                calcW('dataMenu');
            });
</script>
    </form>
</body>
</html>

