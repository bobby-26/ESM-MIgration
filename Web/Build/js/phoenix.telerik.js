var TO;
$telerik.$(window).resize(function () {
	clear()
});

function clear() {
	clearTimeout(TO);
	TO = setTimeout(sizeWindowInPercentage, 100);
}

function sizeWindowInPercentage() {
	var browserWidth = $telerik.$(window).width();
	var browserHeight = $telerik.$(window).height();
    $("#fraPhoenixApplication").attr("style", "height:" + (browserHeight - 38) + "px");		
    $('#radMenuTree').height(browserHeight - 120);
    //$('#radMenuTree .rpSlide, #radMenuTree .rpGroup').css("height", "auto");
}

function setMenuCode(sender, eventArgs) {	
	var menucode = eventArgs.get_item().get_value();
	var currPage = eventArgs.get_item().get_linkElement().href;
	if (menucode != null && menucode != "") {
	    AjxPost("functionname=SetMenuCode|menucode=" + menucode + "|page=" + (currPage != null ? currPage : ""), SitePath + "PhoenixWebFunctions.aspx", null, false);
	}
	var title = "\xa0\xa0" + eventArgs.get_item().get_text();
	pageTitle.value = title;
	$(".home-btn span.text").html(title);
	currModule.value = getRootMenuValue(eventArgs.get_item());
	setContext(currModule.value);
}
function getRootMenuValue(item) {    
    while (item.get_level() > 0) {        
        item = item.get_parent();
    }    
    return item.get_value();
}
function setContext(menucode) {
    var menu = ["PUR", "QUA", "CSY", "INV", "VAC", "BGT", "VPS", "DRD", "PMS", "OFS", "DMR"];
    var cpyMenu = ["ACC", "QUA"];
    var cpynode = $find("mainNavigation").findNodeByText("Company");
    var vslnode = cpynode.get_previousNode();
    var $vsl = $('div.RadComboBox[id*="_ddlVessel"]');
    var vsl = $find($vsl.attr("id"));
    var $cpy = $('div.RadComboBox[id*="_ddlCompany"]');
    var cpy = $find($cpy.attr("id"));
    if (vsl != null) {
        if (menu.indexOf(menucode) > -1) {
            vsl.set_enabled(true);
            $($vsl).find('table.rcbDisabled').removeClass();
            vslnode.set_enabled(true);
        }
        else {
            vsl.set_enabled(false);
            vslnode.set_enabled(false);
        }
    }
    if (cpy != null) {
        if (cpyMenu.indexOf(menucode) > -1) {
            cpy.set_enabled(true);            
            $($cpy).find('table.rcbDisabled').removeClass();
            cpynode.set_enabled(true);
        }
        else {
            cpy.set_enabled(false);
            cpynode.set_enabled(false);
        }
    }
}
(function () {
    var desktop = window.desktop = {};

    desktop.initialize = function (nav) {
        if (nav === "desktop") {

        }
    };
    window.onNavigationClicked = function (sender, args) {
        var idx = args.get_node().get_index();
        var ele = args.get_node().get_element();
        var txt = args.get_node().get_text();
        var dmslink = ele.querySelector("a");
        var isdmslink = false;
        if (dmslink != null && dmslink.href != null & dmslink.href.toLowerCase().includes("documentmanagement/"))
            isdmslink = true;
        if (isdmslink) {
            $find("mainNavigation").get_firstNode()._element.querySelector("span[id*='lblTitle']").innerHTML = txt;
        }
        else {
            if (idx != null && idx == 0) {
                $('form').toggleClass('expandedSlider');
            } else if (idx != null && idx > 0) {
                args.get_node().set_selected(false);
            }
        }
        if (args.get_node().get_level() > 0) {
            sender._collapseAll();
        }
    };
    window.onNavigationMouseEnter = function (sender, args) {
        var ele = args.get_node().get_element();
        var idx = args.get_node().get_index();
        if (idx != null && idx > 0) {
            args.get_node()._onMouseEnter = function () { return false; }
        }
    };
})();
