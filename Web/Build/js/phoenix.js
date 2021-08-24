var o;
var WinPrint;
var _startX = 0;
var _startY = 0;
var _offsetX = 0;
var _offsetY = 0;
var mouseOffset = null;
var eleminscroll;

var _changeFlag = 0;
document.onmouseup = null;
window.history.forward(1);

function checkChangeFlag(e) {
    if (!e) e = window.event;
    var elem;
    if (e.target) elem = e.target;
    if (e.srcElement) elem = e.srcElement;
    if (elem.nodeType == 1)
        clearChangeFlag();
}

function setChangeFlag(e) {
    if (!e) e = window.event;
    var keyCode = (e.keyCode) ? e.keyCode : e.which;
    if (_changeFlag == 0) {
        var elem;
        if (e.target) elem = e.target;
        if (e.srcElement) elem = e.srcElement;
        if (elem.nodeType == 1)
            _changeFlag = 1;
    }
}

function clearChangeFlag() {
    if (_changeFlag == 1) {
        if (!confirm("You have not saved the changes. Click on 'No' to cancel and save the changes"))
            return;
    }
    _changeFlag = 0;
}

function resizeWindow() {
    try {
        var windowHeight = getWindowHeight();
        var windowWidth = document.documentElement.clientWidth;
        document.getElementById("navigation").style.height = windowHeight + "px";
        document.getElementById("navigation").style.width = windowWidth + "px";
    }
    catch (e) { }
}
function getWindowHeight() {
    var windowHeight = 0;
    if (typeof (window.innerHeight) == 'number') {
        windowHeight = window.innerHeight;
    }
    else {
        if (document.documentElement && document.documentElement.clientHeight) {
            windowHeight = document.documentElement.clientHeight;
        }
        else {
            if (document.body && document.body.clientHeight) {
                windowHeight = document.body.clientHeight;
            }
        }
    }
    return windowHeight;
}
function refreshMenu() {
    OpenTaskPane();
}
function expandMenu() {
    if (document.getElementById('content').cols == '0%,*')
        document.getElementById('content').cols = '15%,*'
}
function collapseMenu() {
    if (document.getElementById('content').cols != '0%,*')
        document.getElementById('content').cols = '0%,*'
}
function hideMenu() {
    if (document.getElementById('content').cols == '0%,*')
        document.getElementById('content').cols = '15%,*'
    else
        document.getElementById('content').cols = '0%,*';

    refreshMenu();
}
function ResizeMenu(o) {
    try {
        parent.hideMenu();
    }
    catch (e) {
        parent.parent.hideMenu();
    }
}
function Openpopup(name, title, url, wsize) {
    if (wsize == null)
        top.OpenPopupWindow(name, title, url, 'codehelp', 'xdata');
    else
        top.OpenPopupWindow(name, title, url, 'codehelp', wsize);
    return false;
}
function NavigateTo(name, title, url, callback, wsize) {
    if (wsize == null)
        parent.parent.OpenPopupWindow(name, title, url, 'codehelp', 'xdata', null, null, null, callback);
    else
        parent.parent.OpenPopupWindow(name, title, url, 'codehelp', wsize, null, null, null, callback);
}
function CloseWindow(name) {
    CloseCodeHelpWindow(name);
}
function ShowFind() {
    if (document.getElementById("divFind").style.display == "none") {
        document.getElementById("divFind").style.display = "block";
        document.getElementById("divFind").style.height = "";
    }
    else {
        document.getElementById("divFind").style.display = "none";
        document.getElementById("divFind").style.height = "0px";
    }
}
function txtkeypress(e, o, p, d, n) {
    var keyCode = (e.keyCode) ? e.keyCode : e.which;
    var v = o.value;

    if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39 || keyCode == 190 || keyCode == 189 || keyCode == 46 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105) || (keyCode >= 109 && keyCode <= 110)) {
        if (keyCode == 190 || keyCode == 110) {
            if (p) return false;
            if (v.lastIndexOf('.') >= 0)
                return false;
        }
        var startPos = 0;
        var endPos = 0;
        if (document.selection) {
            o.focus();

            var oSel = document.selection.createRange();

            oSel.moveStart('character', -o.value.length);

            var startPos = oSel.text.length;
            v = v.substring(0, startPos)
                            + (keyCode == 190 || keyCode == 110 ? '.' : ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) ? '0' : '')
                            + v.substring(startPos, v.length);
        }
        else if (o.selectionStart || o.selectionStart == '0') {
            startPos = o.selectionStart;
            endPos = o.selectionEnd;
            v = v.substring(0, startPos)
                            + (keyCode == 190 || keyCode == 110 ? '.' : ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) ? '0' : '')
                            + v.substring(endPos, v.length);
        }
        if (v.lastIndexOf('.') >= 0) {
            if (v.substring(v.lastIndexOf('.') + 1).length > d)
                if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))
                    return false;
        }
        if ((keyCode == 109 || keyCode == 189) && n) {
            return false;
        }
        if ((keyCode == 109 || keyCode == 189) && (v.lastIndexOf('-') >= 0 || startPos > 1)) {
            return false;
        }
        return true;
    }
    e.returnValue = false;
    if (e.which)
        e.preventDefault() ? e.preventDefault() : e.returnValue;
    else
        return false;
}
function txtonnumericblur(o, prefix) {
    var inval;
    inval = o.value;

    if (isNaN(inval.substr(1)))
        o.value = '';

    var v = o.value;
    if (v.lastIndexOf(prefix) >= 0) {
        if (v.length == 1)
            o.value = prefix + '0.00';
        return;
    }
    else {
        if (v.length == 0)
            o.value = prefix + '0.00';
        else
            o.value = prefix + v;
    }
}

function fnConfirmDelete(e, msg) {
    var result;

    if (msg == null)
        result = confirm("Are you sure you want to delete this record?");
    else
        result = confirm(msg);

    if (result == true) {
        return window.returnValue = true;
    }
    else {
        if (e.which)
            e.stopPropagation();
        else
            window.event.cancelBubble = true;

        return false;
    }
}
function fnRejectDelete() {
    alert('You cannot delete this record.');
    return false;
}
function refreshApplicationTitle() {
    document.getElementById('applicationtitle').src = "PhoenixApplicationTitle.aspx";
}
function OpenTaskPane(querystring) {
    if (querystring == null || querystring == '')
        document.getElementById('taskpane').src = "PhoenixTaskPane.aspx";
    else
        parent.document.getElementById('taskpane').src = "PhoenixTaskPane.aspx?option=" + querystring;
}

function fnReload() {
    location.reload();
    return true;
}
function fnReloadList(name, refreshiframe, keeppopupopen) {
    var o = top.document.getElementById("fraPhoenixApplication");

    if (keeppopupopen == null) {
        try {
            closeTelerikWindow();
            if (name == null) {
                top.CloseWindow('Filter');
            }
            else {
                top.CloseWindow(name);
            }
        } catch (e) { }
    }

    if (o.contentWindow.document.getElementById(refreshiframe) == null)
        refreshiframe = null;

    if (refreshiframe == null)
        o.contentDocument.getElementById('cmdHiddenSubmit').click();
    else
        o.contentWindow.document.getElementById(refreshiframe).contentDocument.getElementById('cmdHiddenSubmit').click();
}

function fnClosePickList(name, refreshiframe, ignoreiframe) {
    var o = top.document.getElementById("fraPhoenixApplication");

    closeTelerikWindow();

    if (o.contentWindow.document.getElementById(refreshiframe) == null)
        refreshiframe = null;

    if (refreshiframe == null || ignoreiframe == true)
        o.contentWindow.populatePick();
    else
        o.contentWindow.document.getElementById(refreshiframe).contentWindow.populatePick();
}

function CallPrint(gridview, issystemsecuritycontext) {
    if (issystemsecuritycontext)
        WinPrint = window.open('../PhoenixPrintGrid.aspx?gv=' + gridview + '&issystemsecuritycontext=' + issystemsecuritycontext, 'Print', 'letf=0,top=0,width=1024px,height=768px,toolbar=0,scrollbars=0,status=0');
    else
        WinPrint = window.open('../PhoenixPrintGrid.aspx?gv=' + gridview, 'Print', 'letf=0,top=0,width=1024px,height=768px,toolbar=0,scrollbars=0,status=0');

    WinPrint.focus();
}

function CallPrintback(strid) {
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
}

function showPickList(pickspan, name, caption, url, isiframe) {
    var divPick = document.getElementById(pickspan);
    if (divPick != null) {
        var elem = divPick.querySelectorAll("input[type='text']");
        var args = "function=PickListSet";
        for (var i = 0; i < elem.length; i++) {
            if (elem[i].type == 'text') {
                args += "|";
                args += elem[i].id;
                args += "=";
                args += elem[i].value;
            }
        }
        AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
    }
    if (url.indexOf('../') > -1) {
        top.openNewWindow(name, caption, SitePath + url.replace('../', '/'));
    } else {
        top.openNewWindow(name, caption, url);
    }
}

function showAccountPickList(pickspan, name, caption, url, isiframe) {
    if (document.getElementById(pickspan) != null) {
        var elem = document.getElementById(pickspan).querySelectorAll("input[type='text']");
        var args = "function=PickListAccountSet";
        for (var i = 0; i < elem.length; i++) {
            if (elem[i].type == 'text') {
                args += "|";
                args += elem[i].id;
                args += "=";
                args += elem[i].value;
            }
        }

        args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);

        var arrValue = args.split('|');
        var dt = arrValue[1].split('=');

        if (dt[1] == '') {
            if (url.indexOf('../') > -1) {
                top.openNewWindow(name, caption, SitePath + url.replace('../', '/'));
            } else {
                top.openNewWindow(name, caption, url);
            }
            return;
        }
        else {
            for (var i = 0; i < arrValue.length; i++) {
                var data = arrValue[i].split('=');

                if (document.getElementById(data[0]) != null)
                    document.getElementById(data[0]).value = data[1];
            }

            if (document.getElementById('cmdHiddenPick') != null)
                document.getElementById('cmdHiddenPick').click();
        }
    }
}

function showSubAccountPickList(pickspan, name, caption, url, isiframe) {
    if (document.getElementById(pickspan) != null) {
        var elem = document.getElementById(pickspan).querySelectorAll("input[type='text']");
        var args = "function=PickListSubAccountSet";
        for (var i = 0; i < elem.length; i++) {
            if (elem[i].type == 'text') {
                args += "|";
                args += elem[i].id;
                args += "=";
                args += elem[i].value;
            }
        }

        args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);

        var arrValue = args.split('|');
        var dt = arrValue[1].split('=');

        if (dt[1] == '') {
            if (url.indexOf('../') > -1) {
                top.openNewWindow(name, caption, SitePath + url.replace('../', '/'));
            } else {
                top.openNewWindow(name, caption, url);
            }
            return;
        }
        else {
            for (var i = 0; i < arrValue.length; i++) {
                var data = arrValue[i].split('=');

                if (document.getElementById(data[0]) != null)
                    document.getElementById(data[0]).value = data[1];
            }

            if (document.getElementById('cmdHiddenPick') != null)
                document.getElementById('cmdHiddenPick').click();
        }
    }
}

function populatePick() {
    var args = "function=PickListGet";
    args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
    var arrValue = args.split('|');
    for (var i = 0; i < arrValue.length; i++) {
        var data = arrValue[i].split('=');
        if (document.getElementById(data[0]) != null)
            document.getElementById(data[0]).setAttribute('value', data[1]);
    }

    if (document.getElementById('cmdHiddenPick') != null)
        document.getElementById('cmdHiddenPick').click();
}
function hideContextMenu() {
    if (document.getElementById('divContextMenu') != null) {
        if (document.getElementById('divContextMenu').style.display == "block")
            document.getElementById('divContextMenu').style.display = "none";
    }
}
//function showVessel() {
//    //parent.document.getElementById('fraPhoenixApplication').contentDocument.getElementById('lblVessel').innerHTML = AjxPost("function=GetCurrentVessel", SitePath + "PhoenixWebFunctions.aspx", null, false);
//    parent.document.getElementById('fraPhoenixApplication').src=SitePath + "Dashboard/Dashboard.aspx";
//    //parent.refreshMenu();
//}


function refreshApplicationtitle() {
    parent.document.getElementById('applicationtitle').contentDocument.location.reload();
    OPENSEARCHPAGE(SitePath + "Dashboard/Dashboard.aspx", "");
    parent.refreshMenu();
}

function showSwitch(flag) {
    if (flag == false) {
        document.getElementById('applicationtitle').contentDocument.getElementById('aSwitch').style.display = "none";
        document.getElementById('applicationtitle').contentDocument.getElementById('lblVessel').style.display = "none";
    }
    else {
        document.getElementById('applicationtitle').contentDocument.getElementById('aSwitch').style.display = "";
        document.getElementById('applicationtitle').contentDocument.getElementById('lblVessel').style.display = "";
    }
}

function showQualityCompanySwitch(flag) {
    if (flag == false) {
        document.getElementById('applicationtitle').contentDocument.getElementById('aCompanySwitch').style.display = "none";
    }
    else {
        document.getElementById('applicationtitle').contentDocument.getElementById('aCompanySwitch').style.display = "";
    }
}

function showCompany() {
    var sReturn = AjxPost("function=GetCurrentCompany", SitePath + "PhoenixWebFunctions.aspx", null, false);
    sReturn = sReturn.split(',');
    parent.document.getElementById('applicationtitle').contentDocument.getElementById('lblVessel').innerHTML = sReturn[0];
}

window.onload = function () {
    prepareMouseClick();
}

function prepareMouseClick() {
    if (document.getElementById('divContextMenu') != null) {
        if (document.all) {
            document.body.oncontextmenu = function () {
                return false;
            }
            document.body.oncontextmenu();
        }
    }
    fnEnableClick();
}

function fnEnableClick() {
    document.onmousedown = click;
}

function click(ev) {
    e = ev || window.event

    if (document.all) {
        e.returnValue = false;
        e.cancelBubble = true;
    }

    if (e.button == 1 || e.which == 1)
        ShowMoreLinks(e);

    if (e.button > 1 || e.which > 1) {
        showContextMenu(e);
        ShowMoreLinks(e);
    }
    else {
        o = e.target || e.srcElement;
        if (o.id == null)
            return;

        if (o.id.indexOf('divIframe') >= 0 || o.id.indexOf('divSplit') >= 0) {
            var dgSplit = (o && o.previousSibling.nodeType == 3 ? o.previousSibling.previousSibling : o.previousSibling);
            if (dgSplit.id.indexOf('dragSplit') >= 0) {
                dgSplit.style.display = "block";
            }
            document.onmouseup = release;
            engage(e);
        }
        else if (o.id.indexOf('divGridColumn') >= 0) {
            document.onmouseup = release;
            engageGridColumn(e);
        }
        else {
            document.onmouseup = null;
            return showContextMenu(e);
        }
    }
}

function ShowMoreLinks(e) {
    if (document.getElementById('divMoreLinks') == null)
        return true;

    var o = e.target || e.srcElement;
    var name = o.innerHTML;

    if (name == null || name.indexOf('More Links') < 0) {
        return;
    }
    if (o.id.indexOf("dlstTabs") < 0)
        return;

    if (document.getElementById('divMoreLinks').style.display == "block") {
        document.getElementById('divMoreLinks').style.display = "none";
        return;
    }

    if (navigator.appName == 'Netscape' && e.which == 1) {
        mouseOffset = getMouseOffset(o, e);

        xMousePos = e.clientX - mouseOffset.x;
        yMousePos = document.getElementById(o.id).clientHeight;

        if (document.getElementById('divMoreLinks') != null) {
            document.getElementById('divMoreLinks').style.display = "block";
            document.getElementById('divMoreLinks').style.left = xMousePos + 'px';
            document.getElementById('divMoreLinks').style.top = yMousePos + 'px';
        }
        e.returnValue = false;
        e.stopPropagation();
        e.preventDefault();

        return false;
    }

    if (navigator.appName == 'Microsoft Internet Explorer' && e.button == 1) {

        xMousePos = e.clientX - e.offsetX;
        yMousePos = document.getElementById(o.id).clientHeight;

        if (document.getElementById('divMoreLinks') != null) {
            document.getElementById('divMoreLinks').style.display = "block";
            document.getElementById('divMoreLinks').style.left = xMousePos + 'px';
            document.getElementById('divMoreLinks').style.top = yMousePos + 'px';
        }
        e.returnValue = false;
        e.cancelBubble = true;
        return false;
    }

}
function showContextMenu(e) {
    if (document.getElementById('divContextMenu') == null)
        return true;

    var o = e.target || e.srcElement;

    if (navigator.appName == 'Netscape' && e.which == 3) {
        xMousePos = e.pageX;
        yMousePos = e.pageY;

        if (document.getElementById('divContextMenu') != null) {
            document.getElementById('divContextMenu').style.display = "block";
            document.getElementById('divContextMenu').style.left = xMousePos + 'px';
            document.getElementById('divContextMenu').style.top = yMousePos + 'px';
        }
        e.returnValue = false;
        e.preventDefault();
        e.stopPropagation();

        return false;
    }

    if (navigator.appName == 'Microsoft Internet Explorer' && e.button == 2) {
        xMousePos = e.x;
        yMousePos = e.y;

        if (document.getElementById('divContextMenu') != null) {
            document.getElementById('divContextMenu').style.display = "block";
            document.getElementById('divContextMenu').style.left = xMousePos + 'px';
            document.getElementById('divContextMenu').style.top = yMousePos + 'px';
        }
        e.returnValue = false;
        e.cancelBubble = true;
        return false;
    }

    if (e.which == 1 || e.button == 1) {
        if (o.id.indexOf("dlstMenu") < 0)
            hideContextMenu();
    }
}

function showMoreInformation(ev, pagemoreinfo) {
    var e = ev || window.event;
    var obj = e.target || e.srcElement;


    var xMousePos;
    var yMousePos;

    if (window.event) {
        xMousePos = e.x + 10;
        yMousePos = e.y + 70;
    }
    else {
        mouseOffset = getMouseOffset(obj, e);
        xMousePos = e.pageX - mouseOffset.x + 20;
        yMousePos = e.pageY - mouseOffset.y;
    }
    OpenPopupWindow('MoreInfo', '', pagemoreinfo, 'codehelp', 'large', null, xMousePos, yMousePos);
}

function closeMoreInformation() {
    CloseCodeHelpWindow('MoreInfo');
}

function authenticateAccess(listpage, menucode) {
    var strPage;
    strPage = AjxPost("functionname=AuthenticateAccess|listpage=" + listpage + "|menucode=" + menucode, SitePath + "PhoenixWebFunctions.aspx", null, false)
    return strPage;
}

function selectTab(tabid, tabtitle) {
    var elem;
    elem = document.documentElement.getElementsByTagName("a");


    for (var i = 0; i < elem.length; i++) {
        if (elem[i].id.indexOf(tabid) >= 0) {
            if (elem[i].title.indexOf(tabtitle) >= 0) {
                if (document.all)
                    elem[i].click();
                else
                    document.location = elem[i].getAttribute('href');
            }
        }
    }
}

function open_greybox() {
    var onclickHandler = document.getElementById('grey_link').getAttribute('onclick')
    if (onclick == null) document.location = document.getElementById('grey_link').getAttribute('href');
    else eval(onclickHandler);
}

function animateFade(lastTick, eid) {
    var curTick = new Date().getTime();
    var elapsedTicks = curTick - lastTick;

    var element = document.getElementById(eid);

    if (element.FadeTimeLeft <= elapsedTicks) {
        element.style.opacity = element.FadeState == 1 ? '1' : '0';
        element.style.filter = 'alpha(opacity = '
            + (element.FadeState == 1 ? '100' : '0') + ')';
        element.FadeState = element.FadeState == 1 ? 2 : -2;
        element.style.zIndex = -1;
        return;
    }

    element.FadeTimeLeft -= elapsedTicks;
    var newOpVal = element.FadeTimeLeft / TimeToFade;
    if (element.FadeState == 1)
        newOpVal = 1 - newOpVal;

    element.style.opacity = newOpVal;
    element.style.filter = 'alpha(opacity = ' + (newOpVal * 100) + ')';

    setTimeout("animateFade(" + curTick + ",'" + eid + "')", 33);
}

var TimeToFade = 3000.0;

function fade(eid) {
    var element = document.getElementById(eid);
    if (element == null)
        return;

    element.style.zIndex = 99;
    if (element.FadeState == null) {
        if (element.style.opacity == null
            || element.style.opacity == ''
            || element.style.opacity == '1') {
            element.FadeState = 2;
        }
        else {
            element.FadeState = -2;
        }
    }

    if (element.FadeState == 1 || element.FadeState == -1) {
        element.FadeState = element.FadeState == 1 ? -1 : 1;
        element.FadeTimeLeft = TimeToFade - element.FadeTimeLeft;
    }
    else {
        element.FadeState = element.FadeState == 2 ? -1 : 1;
        element.FadeTimeLeft = TimeToFade;
        setTimeout("animateFade(" + new Date().getTime() + ",'" + eid + "')", 33);
    }
}
function pageLoad() {
    var mb = document.getElementById("isouterpage");
    if (mb == null) {
        if (top.frames.length == 0)
            top.location = "../PhoenixLogout.aspx";
    }
    fade('statusmessage');
}
function fnStopPropagation(e, lnkID) {
    if (document.all)
        document.getElementById(lnkID).click();
}

function SelectSibling(e) {
    var e = e ? e : window.event;
    var KeyCode = e.which ? e.which : e.keyCode;
    if (e.target) eleminscroll = e.target;
    if (e.srcElement) eleminscroll = e.srcElement;

    if (KeyCode == 40 || KeyCode == 38)
        try { eleminscroll.select(); } catch (e) { return true; }
    return KeyCode;
}

function fnPostToWebFunction(methodname, spnInput, spnOutput) {
    var args = "function=" + methodname;

    var elem = document.getElementById(spnInput).childNodes;
    for (var i = 0; i < elem.length; i++) {
        if (elem[i].type == 'text') {
            args += "|";
            args += elem[i].id;
            args += "=";
            args += elem[i].value;
        }
    }

    var elemOut = document.getElementById(spnOutput).childNodes;
    for (var j = 0; j < elemOut.length; j++) {
        if (elemOut[j].type == 'text') {
            args += "|";
            args += elemOut[j].id;
            args += "=";
            args += "OUT";
        }
    }

    var argsr = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
    var arrValue = argsr.split('|');

    for (var i = 0; i < arrValue.length - 1; i++) {
        var data = arrValue[i].split('=');
        if (document.getElementById(data[0]) != null)
            document.getElementById(data[0]).value = data[1];
    }
}

function txtonnumberblur(o) {
    var v = o.value;
    if (v.length == 0)
        o.value = '0.00';
    if (v.lastIndexOf('.') < 0)
        o.value = v + '.00';
}
function showTooltip(ev, divId, opt) {
    var e = ev || window.event

    var yMousePos = e.clientY; // + document.getElementById(divId).clientHeight;


    document.getElementById(divId).style.top = yMousePos + 'px';
    document.getElementById(divId).style.visibility = opt;

}
function fnScrollToTree(tvwObj) {
    if (tvwObj != null) {
        var name = tvwObj.selectedNodeID.value;
        var selectedNode = document.getElementById(name);
        if (selectedNode) {
            selectedNode.scrollIntoView(false);
        }
        window.scrollBy(0, -80);
    }
}
function scrollToVal(divId, hdnId) {
    var divObj = document.getElementById(divId);
    var hdnScroll = document.getElementById(hdnId);
    if (divObj && hdnScroll)
        divObj.scrollTop = hdnScroll.value;
}
function setScroll(divId, hdnId) {
    var bigDiv = document.getElementById(divId);
    var hdnScroll = document.getElementById(hdnId);
    if (bigDiv && hdnScroll)
        hdnScroll.value = bigDiv.scrollTop;
    document.getElementById(divId.replace("divMain", "divHeader")).scrollLeft = bigDiv.scrollLeft;
}
function FreezeGridViewHeader(gridId, height, isFooter, saveCmd) {
    var grid = document.getElementById(gridId);
    if (grid) {
        var DivRT = grid.parentNode.appendChild(document.createElement("div"));
        DivRT.setAttribute("id", "divRoot" + gridId);
        DivRT.setAttribute("align", "left");

        var DivHR = DivRT.appendChild(document.createElement("div"));
        DivHR.setAttribute("id", "divHeader" + gridId);
        DivHR.setAttribute("style", "overflow: hidden;");

        var DivMC = DivRT.appendChild(document.createElement("div"));
        DivMC.setAttribute("id", "divMain" + gridId);
        DivMC.setAttribute("style", "overflow: auto;");
        DivMC.setAttribute("onscroll", "javascript:setScroll('divMain" + gridId + "', 'hdnScroll" + gridId + "');");
        DivMC.appendChild(grid);

        var DivFR = DivRT.appendChild(document.createElement("div"));
        DivFR.setAttribute("id", "divFooter" + gridId);
        DivFR.setAttribute("style", "overflow: hidden;");

        var width = grid.offsetWidth

        var headerHeight = grid.getElementsByTagName("TR")[0].offsetHeight;
        DivRT.style.height = height + 'px';

        //*** Set divheaderRow Properties ****
        DivHR.style.height = headerHeight + 'px';
        //DivHR.style.width = "98.5%"
        DivHR.style.width = (parseInt(width) - 16) + 'px';
        DivHR.style.position = 'relative';
        DivHR.style.top = '0px';
        DivHR.style.zIndex = '10';
        DivHR.style.verticalAlign = 'top';

        //*** Set divMainContent Properties ****
        //DivMC.style.width = "100%"//width + 'px';
        DivMC.style.width = width + 'px';
        DivMC.style.height = height + 'px';
        DivMC.style.position = 'relative';
        DivMC.style.top = -headerHeight + 'px';
        DivMC.style.zIndex = '1';

        //*** Set divFooterRow Properties ****
        //DivFR.style.width = "99%"
        DivFR.style.width = (parseInt(width) - 16) + 'px';
        DivFR.style.position = 'relative';
        DivFR.style.top = -headerHeight + 'px';
        DivFR.style.verticalAlign = 'top';
        DivFR.style.paddingtop = '2px';

        if (isFooter) {
            var tblfr = grid.cloneNode(true);
            tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
            var tblBody = document.createElement('tbody');
            tblfr.style.width = '100%';
            tblfr.cellSpacing = "0";
            tblfr.border = "0px";
            tblfr.rules = "none";
            //*****In the case of Footer Row *******
            tblBody.appendChild(grid.rows[grid.rows.length - 1]);
            tblfr.appendChild(tblBody);
            DivFR.appendChild(tblfr);
        }
        //****Copy Header in divHeaderRow****
        var cloneObj = grid.cloneNode(true);
        if (saveCmd != null || saveCmd != "") {
            var editIndex = -1;
            var tags = cloneObj.getElementsByTagName("INPUT");
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].type.toLowerCase() != "image") continue;
                if (tags[i].id.indexOf(saveCmd) > -1) {
                    editIndex = tags[i].parentNode.parentNode.rowIndex; break;
                }
            }
            if (parseInt(editIndex) > -1) {
                cloneObj.deleteRow(editIndex);
            }
        }
        DivHR.appendChild(cloneObj);
        scrollToVal('divMain' + gridId, 'hdnScroll' + gridId);
        //IE takes time to move the object, to overcome this time out method is used.
        setTimeout(function () { ResizeGridViewHeader(gridId) }, 50);
    }
}

function ResizeGridViewHeader(gridId) {
    var grid = document.getElementById(gridId);
    var nav = document.getElementById("navigation");
    if (nav == null) return;
    // to find grid root element to ignore the height calcuation
    var compareObj = null;
    if (grid != null) {
        compareObj = grid;
        while (true) {
            if (compareObj.parentNode.id == "navigation") {

                break;
            }
            compareObj = compareObj.parentNode;
        }
    }
    var height = 0;
    for (var i = 0; i < nav.childNodes.length; i++) {
        var ele = nav.childNodes[i];
        if ((ele.className == "navSelect" && ele.style.position == "absolute")
                    || (ele.nodeName == "INPUT" || ele.type == "hidden")
                    || ele.nodeName == "#text"
                    || ele == compareObj) continue;
        height = height + ele.offsetHeight;
    }
    var winht = (document.all ? (document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight) : window.innerHeight) - 10;
    var winwd = (document.all ? (document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth) : window.innerWidth) - 10;

    var root = document.getElementById("divRoot" + gridId);
    var header = document.getElementById("divHeader" + gridId);
    var content = document.getElementById("divMain" + gridId);
    var footer = document.getElementById("divFooter" + gridId);
    if (root != null) {
        if (winht - height > 100) {
            root.style.height = winht - height - 5 + "px";
            content.style.height = winht - height - 5 + "px";
        }
        header.style.width = parseInt(winwd) - 16 + "px";
        content.style.width = parseInt(winwd) + "px";
        if (footer.childNodes.length > 0)
            content.style.height = parseInt(content.style.height) - 25 + "px";
        footer.style.width = parseInt(winwd) - 16 + "px";

        var headerheight = grid.getElementsByTagName("TR")[0].offsetHeight;

        header.style.height = headerheight + 'px';
        content.style.top = -headerheight + "px";
        footer.style.top = -headerheight + "px";
    }
}

function txtkeypressdecimal(e, o, p, d, n, m) {
    var keyCode = (e.keyCode) ? e.keyCode : e.which;
    var v = o.value;

    var digits = (d > 0) ? m - (d + 1) : m;

    if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39 || keyCode == 190 || keyCode == 189 || keyCode == 46 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105) || (keyCode >= 109 && keyCode <= 110)) {
        if (keyCode == 190 || keyCode == 110) {
            if (p) return false;
            if (v.lastIndexOf('.') >= 0)
                return false;
        }
        var startPos = 0;
        var endPos = 0;
        if (document.selection) {
            o.focus();

            var oSel = document.selection.createRange();

            oSel.moveStart('character', -o.value.length);

            var startPos = oSel.text.length;
            v = v.substring(0, startPos)
                            + (keyCode == 190 || keyCode == 110 ? '.' : ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) ? '0' : '')
                            + v.substring(startPos, v.length);
        }
        else if (o.selectionStart || o.selectionStart == '0') {
            startPos = o.selectionStart;
            endPos = o.selectionEnd;
            v = v.substring(0, startPos)
                            + (keyCode == 190 || keyCode == 110 ? '.' : ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) ? '0' : '')
                            + v.substring(endPos, v.length);
        }
        if (v.lastIndexOf('.') >= 0) {
            if (v.substring(v.lastIndexOf('.') + 1).length > d)
                if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))
                    return false;
        }
        if (v.lastIndexOf('.') >= 0) {
            if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))
                if (v.substring(0, v.lastIndexOf('.')).length > digits && startPos < v.lastIndexOf('.'))
                    return false;
        }
        else {
            if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))
                if (v.length > digits)
                    return false;
        }
        if ((keyCode == 109 || keyCode == 189) && n) {
            return false;
        }
        if ((keyCode == 109 || keyCode == 189) && (v.lastIndexOf('-') >= 0 || startPos > 1)) {
            return false;
        }
        return true;
    }
    e.returnValue = false;
    if (e.which)
        e.preventDefault() ? e.preventDefault() : e.returnValue;
    else
        return false;
}

function checkUnchekAll(obj) {
    var lstchk = obj.parentElement.parentElement.parentElement.getElementsByTagName("input");
    if (lstchk.length > 0) {
        for (var i = 0; i < lstchk.length; i++) {
            if (lstchk[i].type = "checkbox") {
                lstchk[i].checked = obj.checked;
            }
        }
    }
}
(function (global, undefined) {
    var telerik = global.telerik = {};
    var treeSearchTimer = null;
    telerik.OnClientClicked = function (sender, eventArgs) {
        ResizeMenu(null);
    };
    telerik.clientTreeSearch = function (sender) {
        $telerik.$(".riTextBox", sender.get_element().parentNode).bind("keydown", telerik.valueChanging);
    };
    telerik.valueChanging = function (sender, eventArgs) {
        if (treeSearchTimer) {
            clearTimeout(treeSearchTimer);
        }

        treeSearchTimer = setTimeout(function () {
            var tree = $find(sender.target.parentElement.parentElement.nextElementSibling.querySelectorAll(".RadTreeView")[0].id);
            var textbox = $find(sender.target.id);
            var searchString = textbox.get_element().value;

            for (var i = 0; i < tree.get_nodes().get_count() ; i++) {
                telerik.findNodes(tree.get_nodes().getNode(i), searchString);
            }
        }, 200);
    };
    telerik.findNodes = function (node, searchString) {
        node.set_expanded(true);

        var hasFoundChildren = false;
        for (var i = 0; i < node.get_nodes().get_count() ; i++) {
            hasFoundChildren = telerik.findNodes(node.get_nodes().getNode(i), searchString) || hasFoundChildren;
        }

        if (hasFoundChildren || node.get_text().toLowerCase().indexOf(searchString.toLowerCase()) != -1) {
            node.set_visible(true);
            return true;
        }
        else {
            node.set_visible(false);
            return false;
        }
    };
    telerik.OnClientDropDownOpenedHandler = function (sender, eventArgs) {
        var tree = sender.get_items().getItem(0).findControl("DropDownTreeView");
        var selectedNode = tree.get_selectedNode();
        tree._element.style.height = (sender._animatedElement.offsetHeight - 31) + "px";
        if (selectedNode) {
            selectedNode.scrollIntoView();
        }
    };
    telerik.nodeClicking = function (sender, args) {

        var id = sender._clientStateFieldID;
        var position = id.indexOf("_", id.indexOf("_") + 1);
        var comboBox = $find(id.substring(0, position));
        var node = args.get_node();

        comboBox.set_text(node.get_text());

        comboBox.trackChanges();
        comboBox.get_items().getItem(0).set_text(node.get_text());
        comboBox.get_items().getItem(0).set_value(node.get_value());
        comboBox.commitChanges();
        comboBox.hideDropDown();

    };
})(window);

var rzWnd;
function openNewWindow(name, title, url, maximize, width, height, isStandAlone, pickListSpanId, options) {
    var oWnd;
    if (isStandAlone != null) {
        oWnd = window.radopen(url, name);
    }
    else {
        oWnd = top.radopen(url, name);
    }
    if (title != null && title != "") {
        oWnd.SetTitle(title);
    }
    if (options != null && options.icon != null) {
        oWnd._titleIconElement.className = "rwIcon rwIconDisable";
        oWnd._titleIconElement.innerHTML = options.icon;
    }
    if (options != null && options.disableMinMax != null && options.disableMinMax == true) {
        oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Resize + Telerik.Web.UI.WindowBehaviors.Close);
    }
    if (options != null && options.onClose  != null) {
        oWnd.add_close(options.onClose);
    }
    if (options != null && options.helpWinURL != null) {
        oWnd._helpWindowURL = options.helpWinURL;
    }
    sizePopupWindow(oWnd, width, height);
    $telerik.$(window).resize(function () {
        clearTimeout(rzWnd);
        rzWnd = setTimeout(sizePopupWindow(oWnd, width, height), 100);
    });
    var model;
    if (isStandAlone != null) {
        model = $telerik.$(".TelerikModalOverlay")
    }
    else {
        model = top.$telerik.$(".TelerikModalOverlay")
    }
    var ismodel = false;
    if (options != null && options.model != null & options.model) {
        var ismodel = true;
    }
    if (!ismodel) {
        model.click(function () {
            oWnd.close();
        });
    }
    if (maximize != null && maximize.toString().toLowerCase() == 'true') {
        oWnd.maximize();
    }
    if (pickListSpanId) {
        var divPick = document.getElementById(pickListSpanId);
        if (divPick != null) {
            var elem = divPick.querySelectorAll("input[type='text']");
            var args = "function=PickListSet";
            for (var i = 0; i < elem.length; i++) {
                if (elem[i].type == 'text') {
                    args += "|";
                    args += elem[i].id;
                    args += "=";
                    args += elem[i].value;
                }
            }
            AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
        }
    }
    setTimeout(function () { oWnd.setActive(true); setRadWindowZIndex(oWnd); }, 100);
}
function sizePopupWindow(oWnd, width, height) {
    try {
        var browserWidth = $telerik.$(window).width();
        var browserHeight = $telerik.$(window).height();
        if (width != null && height != null) {
            oWnd.setSize(width, height);
        }
        else {
            oWnd.setSize(Math.ceil(browserWidth * 80 / 100), Math.ceil(browserHeight * 80 / 100));
        }
        oWnd.center();
    } catch(e) { }
}
function closeTelerikWindow(closeWindowName, refreshWindow, refreshTop) {
    var $top = top;
    if (closeWindowName == '')
        closeWindowName = null;
    if (refreshWindow == '')
        refreshWindow = null;
    var windowManager = top.$find('phoenixPopup');
    var windows = windowManager.get_windows();
    for (var i = 0; i < windows.length; i++) {
        var oWnd = windows[i];
        if (closeWindowName != null) {
            if (oWnd._name == closeWindowName) {
                oWnd['close']();
            }
        }
        if (refreshWindow != null) {
            if (refreshWindow.indexOf(',') > -1) {
                if (oWnd._name == refreshWindow.split(',')[0]) {
                    var button = oWnd._iframe.contentWindow.$find(refreshWindow.split(',')[1])._iframe.contentDocument.getElementById('cmdHiddenSubmit');
                    if (button != null) {
                        button.click();
                    }
                }
            } else if (refreshWindow.indexOf('~') > -1) {
                if (oWnd._name == refreshWindow.split('~')[0]) {
                    var button = oWnd._iframe.contentWindow.document.getElementById(refreshWindow.split('~')[1]).contentDocument.getElementById('cmdHiddenSubmit');
                    if (button != null) {
                        button.click();
                    }
                }
            } else {
                if (oWnd._name == refreshWindow) {
                    var button = oWnd._iframe.contentDocument.getElementById('cmdHiddenSubmit');
                    if (button != null) {
                        button.click();
                    }
                }
            }
        }
        if (refreshWindow == null && closeWindowName == null) {
            oWnd['close']();
        }
    }
    if (refreshTop) {
        var button = $top.document.getElementById("fraPhoenixApplication").contentDocument.getElementById('cmdHiddenSubmit');
        if (button != null) {
            button.click();
        }
    }
}
function setRadWindowZIndex(oWnd) {
    var zIndex = 0;
    var windowManager = top.$find('phoenixPopup');
    var windows = windowManager.get_windows();
    for (var i = 0; i < windows.length; i++) {
        windows[i]._popupElement.style.zIndex = 10012 + i;
        top.$telerik.$(".TelerikModalOverlay")[i].style.zIndex = 10012 + (i - 1);
    }
    if (oWnd)
        oWnd._popupElement.style.zIndex = 10012 + windows.length;
}
function getRadWindow(name) {
    var radWindow = null;
    var windowManager = top.$find('phoenixPopup');
    var windows = windowManager.get_windows();
    for (var i = 0; i < windows.length; i++) {
        var oWnd = windows[i];
        if (oWnd._name == name) {
            radWindow = oWnd;
            break
        }
    }
    return oWnd;
}
function fnConfirmDeleteTelerik(sender, msg) {
    var callBackFn = function (shouldSubmit, e) {
        if (shouldSubmit) {
            eval(sender.target.parentElement.parentElement.href);
        }
        else {
            if (e != null && e.which)
                e.stopPropagation();
            else
                window.event.cancelBubble = true;
            return false;
        }
    }
    var confirm;

    if (msg == null)
        confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
    else
        confirm = radconfirm(msg, callBackFn);

    return false;
}
(function (global, undefined) {
    var popupWnd = {};

    function showDialog(title, maximizeWin) {
        var wnd = getModalWindow();
        wnd.set_title(title);
        if (popupWnd.modalWindowUrl != null && popupWnd.modalWindowUrl != "") {
            wnd.setUrl(popupWnd.modalWindowUrl);
        }
        wnd.show();
        if (maximizeWin != null && maximizeWin) {
            wnd.maximize();
        } 
    }

    function getModalWindow() { return $find(popupWnd.modalWindowID); }

    global.$modalWindow = popupWnd;
    global.showDialog = showDialog;
})(window);
Date.prototype.toShortFormat = function () {

    var month_names = ["Jan", "Feb", "Mar",
        "Apr", "May", "Jun",
        "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec"];

    var day = this.getDate();
    var month_index = this.getMonth();
    var year = this.getFullYear();

    return "" + day + "-" + month_names[month_index] + "-" + year;
}
function TelerikGridResize(sender, isParent, adjHeight, args) {
    var $ = $telerik.$;
    if (adjHeight == null)
        adjHeight = 2;
    var parentHeight = (isParent == null || isParent == false ? $(window).height() : $(window.parent).height()); // make grid fit the Window height
    //var parentHeight = sender.get_element().parentElement.offsetHeight; // make grid fit its parent container height
    var scrollArea = sender.GridDataDiv;
    var gridHeaderHeight = (sender.GridHeaderDiv) ? sender.GridHeaderDiv.offsetHeight : 0;
    var gridTopPagerHeight = (sender.TopPagerControl) ? sender.TopPagerControl.offsetHeight : 0;
    var gridDataHeight = sender.get_masterTableView().get_element().offsetHeight;
    var gridFooterHeight = (sender.GridFooterDiv) ? sender.GridFooterDiv.offsetHeight : 0;
    var gridPagerHeight = (sender.PagerControl) ? sender.PagerControl.offsetHeight : 0;

    // Do nothing if scrolling is not enabled
    if (!scrollArea) {
        return;
    }
    //if (gridDataHeight < 350 || parentHeight > (gridDataHeight + gridHeaderHeight + gridPagerHeight + gridTopPagerHeight + gridFooterHeight)) {
    //  scrollArea.style.height = (getVisibleHeight('<%= gvDebriefing.ClientID %>') - gridHeaderHeight - gridPagerHeight - gridTopPagerHeight - gridFooterHeight - 2) + "px";
    //} else {
    //    scrollArea.style.height = (parentHeight - gridHeaderHeight - gridPagerHeight - gridTopPagerHeight - gridFooterHeight - 2) + "px"
    //}
    var gridRoot = sender.Control;
    while (true) {
        if (gridRoot.parentNode.tagName.toUpperCase() == "FORM") {
            break;
        }
        gridRoot = gridRoot.parentNode;
    }

    var height = 0;
    var ele = null;
    var gridParent = sender.Control.parentNode;
    for (var i = 0; i < gridParent.children.length; i++) {
        var ele = gridParent.children[i];
        if ($(ele).is(":hidden") || ele.className.toUpperCase() == "ASPNETHIDDEN"
            || (ele.nodeName == "INPUT" && ele.type == "hidden")
            || ele.nodeName.toUpperCase() == "#TEXT" || ele.nodeName.toUpperCase() == "SCRIPT"
            || ele == sender.Control) continue;
        height = height + ele.offsetHeight;
    }
    if (gridParent.tagName.toUpperCase() != "FORM") {
        for (var i = 0; i < document.forms[0].children.length; i++) {
            var ele = document.forms[0].children[i];
            if ($(ele).is(":hidden") || ele.className.toUpperCase() == "ASPNETHIDDEN"
                || (ele.nodeName == "INPUT" && ele.type == "hidden")
                || ele.nodeName.toUpperCase() == "#TEXT" || ele.nodeName.toUpperCase() == "SCRIPT"
                || ele == gridRoot || ele.className.toUpperCase() == "TELERIKMODALOVERLAY" || ele.className.toUpperCase().includes("RADWINDOW")) continue;
            height = height + ele.offsetHeight;
        }
    }
    var gridHeight = (parentHeight - height - gridHeaderHeight - gridPagerHeight - gridTopPagerHeight - gridFooterHeight - adjHeight);
    scrollArea.style.height = (gridHeight < 0 ? 200 : gridHeight) + "px";
}
document.addEventListener("click", function (e) {
    if (parent.name != null && parent.name.indexOf("tab_") > -1) {
        var tooltip = parent.$find("UserProfile");
        if (tooltip != null)
            tooltip.hide();
    }
});
var toolbarButtonDisable;
function removeToolbardDisabled(btn, lnkName) {
    setTimeout(function () {
        var btns = document.querySelectorAll("span[title='" + lnkName + "']");
        for (var i = 0; i < btns.length; i++) {
            btns[i].parentElement.classList.remove('rtbDisabled');
        }
    }, 300);
}
function populateTelerikPick(windowname, framename, closewindow) {
    var o = top.document.getElementById("fraPhoenixApplication");
    var $top = top;
    var windowManager = top.$find('phoenixPopup');

    var args = "function=PickListGet";
    args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
    var arrValue = args.split('|');
    if (framename != null && o.contentDocument.getElementById(framename) != null) {
        var t = o.contentDocument.getElementById(framename);
        for (var i = 0; i < arrValue.length; i++) {
            var data = arrValue[i].split('=');
            if (t.contentDocument.getElementById(data[0]) != null)
                t.contentDocument.getElementById(data[0]).setAttribute('value', data[1]);
        }
        if (t.contentDocument.getElementById('cmdHiddenPick') != null)
            t.contentDocument.getElementById('cmdHiddenPick').click();
        var windows = windowManager.get_windows();
        for (var i = 0; i < windows.length; i++) {
            var oWnd = windows[i];
            if (closewindow != null) {
                if (oWnd._name == closewindow) {
                    oWnd['close']();
                }
            }
        }
    } else {
        var windows = windowManager.get_windows();
        for (var i = 0; i < windows.length; i++) {
            var oWnd = windows[i];
            if (closewindow != null) {
                if (oWnd._name == closewindow) {
                    oWnd['close']();
                }
            }
            if (windowname != null) {
                if (oWnd._name == windowname) {
                    if (framename != null) {
                        var t = oWnd._iframe.contentDocument.getElementById(framename);
                        for (var j = 0; j < arrValue.length; j++) {
                            var data = arrValue[j].split('=');
                            if (t.contentDocument.getElementById(data[0]) != null)
                                t.contentDocument.getElementById(data[0]).setAttribute('value', data[1]);
                        }
                        if (t.contentDocument.getElementById('cmdHiddenPick') != null)
                            t.contentDocument.getElementById('cmdHiddenPick').click();

                    } else {
                        for (var k = 0; k < arrValue.length; k++) {
                            var data = arrValue[k].split('=');
                            if (oWnd.document.getElementById(data[0]) != null)
                                oWnd.document.getElementById(data[0]).setAttribute('value', data[1]);
                        }


                    }
                }

            }


        }
    }
}
function OnHeaderMenuShowing(sender, args) {
    var menu = args.get_menu(); //  Gets the RadContextMenu object
    var radMenuItem = menu.findItemByText("Columns");   //  Gets the Columns RadMenuItem Object
    var radMenuItemCollection = radMenuItem.get_items();    //  Gets the Collection of Items in the Columns Object
    //  Iterate the Collection
    for (var i = 0; i < radMenuItemCollection.get_count() ; i++) {
        $telerik.$(radMenuItemCollection.getItem(i)._element).find("input").on("click", function (e) { saveGridHeader(sender, args); });
    }
}
function saveGridHeader(sender, args) {
    var menu = args.get_menu(); //  Gets the RadContextMenu object
    var radMenuItem = menu.findItemByText("Columns");   //  Gets the Columns RadMenuItem Object
    var radMenuItemCollection = radMenuItem.get_items();
    var hiddenCol = "";

    for (var i = 0; i < radMenuItemCollection.get_count() ; i++) {
        var checked = $telerik.$(radMenuItemCollection.getItem(i)._element).find("input").prop("checked");
        if (!checked) {
            hiddenCol += radMenuItemCollection.getItem(i)._getData().value.split('|')[1] + ",";
        }
    }
    AjxPost("functionname=saveGridFilter|gridid=" + sender.ClientID + "|page=" + location.pathname.split('/').splice(2).join('/') + "|cols=" + hiddenCol, SitePath + "PhoenixWebFunctions.aspx", null, false);
}
