function toggleFullscreen(t, e) { t = t || document.documentElement, document.fullscreenElement || document.mozFullScreenElement || document.webkitFullscreenElement || document.msFullscreenElement ? ($(t).toggleClass("btnExpand").toggleClass("btnCollapse"), document.exitFullscreen ? document.exitFullscreen() : document.msExitFullscreen ? document.msExitFullscreen() : document.mozCancelFullScreen ? document.mozCancelFullScreen() : document.webkitExitFullscreen && document.webkitExitFullscreen(), $(t).find(e).removeClass("wrap3rDivFS"), $(t).find(e).addClass("wrap3rDivNS")) : ($(t).toggleClass("btnCollapse").toggleClass("btnExpand"), t.display = "block", t.requestFullscreen ? t.requestFullscreen() : t.msRequestFullscreen ? (t.msRequestFullscreen(), t.clientHeight = "1000px", alert(t.clientHeight)) : t.mozRequestFullScreen ? t.mozRequestFullScreen() : t.webkitRequestFullscreen && t.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT), $(t).find(e).removeClass("wrap3rDivNS"), $(t).find(e).addClass("wrap3rDivFS")) } function maxIE() { new ActiveXObject("Wscript.shell") }

function calcH(t)
{ var e = $(this).height() - 365; $(t).height(e + "px") }
function calcW(divVal) {
    var wideval = $(this).width() - 365;
    $(divVal).width(wideval + "px");
    $("#Col").width($(window).width() - ($(".icoBlkWrap").width() + $("#FCol").width() + 5));
}
function popupGenCall(t) { $("#gMain").html(t), $("#popupDiv").dialog({ modal: !0, width: 600, height: 400, dialogClass: "ui-widget-shadow" }) }
function popupInfo(t, e) { $("#popupDiv").html("<div id='gMain'></div>"), $("#gMain").css("margin", "10px"), $("#gMain").html(e), $("#popupDiv").dialog({ modal: !0, title: t, width: 600, height: 220, dialogClass: "ui-widget-shadow" }) }
function popupSetCall(t) { var e = "#" + t; $(e).dialog({ modal: !0, width: 800, height: 600, dialogClass: "ui-widget-shadow" }) }
function callDia(t, e, a, s, l, i) {
    $("#popupDiv").dialog({ modal: !0, width: 1e3, height: 600, dialogClass: "ui-widget-shadow", resize: function () { myChartExt.resize(); } });
    var n = { title: { x: "center", text: t, subtext: e }, tooltip: { trigger: "item" }, toolbox: { show: !0, feature: { restore: { show: !0, title: "Refresh" }, saveAsImage: { show: !0, title: "Save" } } }, calculable: !0, grid: { borderWidth: 1, x: 40, x2: 40, y: 50, y2: 130 }, xAxis: [{ type: "category", show: !0, axisLabel: { rotate: 90 }, data: s }], yAxis: [{ type: "value", show: !0 }], series: [{ name: a, type: "bar", itemStyle: { normal: { color: function (t) { var e = i; return e[t.dataIndex] }, label: { show: !0, position: "top", formatter: "{c}" } } }, data: l }] };
    showGraph(n, "popupDiv")
}
function callChartDialog(t, e, a, s, l, i, yAxisname,chartDiv) {
    //$("#popupDiv").dialog({ modal: !0, width: 1e3, height: 600, dialogClass: "ui-widget-shadow", resize: function () { myChartExt.resize(); } });
    var n = { title: { x: "center", text: t, subtext: e }, tooltip: { trigger: "item" }, toolbox: { show: !0, feature: { restore: { show: !0, title: "Refresh" }, saveAsImage: { show: !0, title: "Save" } } }, calculable: !0, grid: { borderWidth: 1, x: 40, x2: 40, y: 50, y2: 130 }, xAxis: [{ type: "category", show: !0, axisLabel: { rotate: 45 }, data: s }], yAxis: [{ name: yAxisname, type: "value", show: !0 }], series: [{ name: a, type: "bar", itemStyle: { normal: { color: function (t) { var e = i; return e[t.dataIndex] }, label: { show: !0, position: "top", formatter: "{c}" } } }, data: l }] };
    showGraph(n, chartDiv)
}
function calllineChart(t, e, a, s, l, i, yAxisname, chartDiv) {
    //$("#popupDiv").dialog({ modal: !0, width: 1e3, height: 600, dialogClass: "ui-widget-shadow", resize: function () { myChartExt.resize(); } });
    var n = { title: { x: "center", text: t, subtext: e }, tooltip: { trigger: "item" }, toolbox: { show: !0, feature: { restore: { show: !0, title: "Refresh" }, saveAsImage: { show: !0, title: "Save" } } }, calculable: !0, grid: { borderWidth: 1, x: 40, x2: 40, y: 50, y2: 130 }, xAxis: [{ type: "category", show: !0, axisLabel: { rotate: 45 }, data: s }], yAxis: [{ name: yAxisname, type: "value", show: !0 }], series: [{ name: a, type: "line", itemStyle: { normal: { color: function (t) { var e = i; return e[t.dataIndex] }, label: { show: !0, position: "top", formatter: "{c}" } } }, data: l }] };
    showGraph(n, chartDiv)  
    {
        { require.config({ paths: { echarts3: '../js/echartsAll3' } }), require(['echarts3'], function (a) { var s = a.init(document.getElementById(chartDiv)); s.setOption(n); myChartExt = s }) }
    }
}
var myChartExt = [];
function showGraph(t, e) { require.config({ paths: { echarts: "assets/js/echarts" } }), require(["echarts", "echarts/chart/bar"], function (a) { var s = a.init(document.getElementById(e)); s.setOption(t); myChartExt = s }) }

var iconDiv2, cpGraphState = {};
function resizeDashboard() {
    if ($("#pnl").attr("style") != null)
        $("#pnl").removeAttr("style");
    else
        $("#pnl").attr("style", "position:absolute;top:0px;height:100%;width:100%;background-color:white;z-index:99")
}


function loadGraphDiv(graphName, graphFile) {
    //$("#lwr3rDiv").html("<div class='otisTitleBar' onclick='toggleFullscreen(lwr3rDiv, otisIframe)'><div id='otisDivLbl' class='titleLabel'>" + graphName + "</div></div><iframe id='otisIframe' class='wrap3rDivNS' width='100%' src='" + graphFile + "' frameborder='0'></iframe>");
    //$("#lwr3rDiv").html("<iframe id='otisIframe' class='wrap3rDivNS' width='100%' height='100%' src='" + graphFile + "' frameborder='0'></iframe>");
}

function FreezeGridColumnScroll(gridId, colno) {
    // here clone our gridview first

    var tab = $("#"+gridId).clone(true);
    // clone again for freeze
    var tabFreeze = $("#"+gridId).clone(true);

    // set width (for scroll)
    var totalWidth = $("#" + gridId).outerWidth();
    var firstColWidth = 0;
    var expr = "td";
    for (var i = 1; i <= colno; i++) {
        firstColWidth = firstColWidth + $("#" + gridId + " th:nth-child(" + i + ")").outerWidth();
        tab.find("th:nth-child(1)").remove();
        tab.find("td:nth-child(1)").remove();
        expr = expr + ":not(:nth-child(" + i + "))";
    }

    tab.width(totalWidth - firstColWidth);

    // here make 2 table 1 for freeze column 2 for all remain column
    tabFreeze.find("th:gt(" + (colno - 1) + ")").remove();       
    tabFreeze.find(expr).remove();
    
    //var container = $('<table border="0" cellpadding="0" cellspacing="0"><tr><td valign="top"><div id="FCol"></div></td><td valign="top"><div id="Col" style="width:800px; overflow:auto"></div></td></tr></table)');
    var container = $('<table cellPadding="0" cellspacing="0" style="width:100%"><tr><td valign="top" ><div id="FCol"></div></td><td valign="top"><div id="Col" style="overflow:auto"></div></td></tr></table)');
    $("#FCol", container).html($(tabFreeze));
    $("#Col", container).html($(tab));

    // clear all html
    $("#dataMenu").html('');
    $("#dataMenu").append(container);
}





