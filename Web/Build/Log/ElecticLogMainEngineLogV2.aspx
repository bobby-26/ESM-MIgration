<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElecticLogMainEngineLogV2.aspx.cs" Inherits="Log_ElecticLogMainEngineLogV2" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <style>
            body {
                background-color: white;
				overflow: hidden;
            }

            .center {
                text-align: center;
            }

            .controls {
                width: 150px !important;
            }

            .controls, .controls-btn {
                margin-top: 2%;
            }

            .header {
                width: 100%;
                margin-left: 4%;
            }

                .header td a {
                    font-size: 13px;
                    display: block;
                    margin: 2%;
                }

                .header h4, .header h5 {
                    margin: 0px;
                    font-size: 13px;
                    font-weight: bold;
                }

            .label-text {
                color: white;
            }

            .label-value {
                color: yellow !important;
            }

            .day-indicator {
                padding-left:2%;
            }

            .indicator {
                display: inline-block;
                width: 10px;
                border-radius: 5px;
                height: 10px;
            }

            .indicator-failed {
                background-color: red;
            }

            .indicator-amber {
                background-color: #ffc000;
            }

            .indicator-success {
                background-color: green;
            }

            .full-width {
                width: 100%;
            }

            .underline {
                text-decoration: underline;
            }

            .links {
                color: black;
                text-decoration: underline;
            }

                .links:hover {
                    color: black;
                    cursor: pointer;
                }

            .cursor {
                cursor: pointer;
            }

            .export-window {
                margin-top: 30%;
                font-size: 1em;
                display: none;
            }

            .fixed_header tbody {
                display: block;
                overflow: auto;
                height: 50px;
                width: 100%;
            }

            .fixed_header thead tr {
                display: block;
            }

            #log-list {
                height: 85px;
            }

                #log-list thead tr {
                    background-color: #1c84c6;
                    color: white;
                    text-align: center !important;
                }

                #log-list tbody td {
                    text-align: center !important;
                }

                #log-list tbody input[type=checkbox] {
                    display: block;
                    width: 100%;
                }

                #log-list thead th {
                    text-align: center !important;
                }

            .custom-btn {
                width: 60px;
                margin-left: 10px;
            }

            #navigationpane {
                overflow-x: hidden;
            }


            .contentpane {
                overflow: hidden !important;
            }

            .page-heading {
                padding-bottom: 0px;
                overflow-x: hidden;
                margin-left: 0%;
                position: fixed;
                z-index: 100;
            }

                .btn-container button {
                    padding: 0.2%;
                }

            .rssActiveCell {
                border-color: red !important;
            }

            .section {
                margin-top: 5%;
            }

            .rotate-text {
                display: block;
                transform: rotate(-90deg);
                -webkit-transform: rotate(-90deg) !important;
                -moz-transform: rotate(-90deg) !important;
                -ms-transform: rotate(-90deg) !important;
                -o-transform: rotate(-90deg) !important;
                filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3) !important;
            }

            /* Desktops and laptops ----------- */
            @media only screen and (min-width : 1224px) {
                .section-container {
                    margin-top: 5%;
                }

                .fixed_header tbody {
                    height: 75px;
                }
            }

            /* Large screens ----------- */
            @media only screen and (min-width : 1824px) {
                .section-container {
                    margin-bottom: 100%;
                    margin-top: 5%;
                }

                .fixed_header tbody {
                    height: 150px;
                }
            }

            .RadSpreadsheet, .rssPane {
                border: 0px !important;
            }
            .RadSpreadsheet .rssScroller {
                overflow: hidden !important;
            }
            .rssToolbarWrapper,
            .rssFormulaBar,
            .rssSheetsbar,
            .rssRowHeader,
            .rssColumnHeader,
            .rssTopCorner {
                display: none;
            }
            .overlay {
                position: absolute;
                display: none;
                width: 100%;
                height: 100%;
                /*  background-color: rgba(0,0,0,0.5);*/
                z-index: 9;
                cursor: pointer;
                opacity: .1;
            }
.RadSpreadsheet .k-spreadsheet-clipboard-paste,
.RadSpreadsheet .k-spreadsheet-clipboard {
    position: fixed;
}
      .k-state-disabled a {
                pointer-events: auto !important;
            }
        </style>
        <script type="text/javascript">
            var spreadsheets;
            var validationState = [];
            var intervalState = {};
            var currentsheet;
            var headyn;

            var unitConfiguration;
            var allowentry = "0";
            var allowwatchsign = "0";
            var allowdaysign = "0";
            var mainenginecount = 1;
            document.addEventListener("DOMContentLoaded", function() {
                pageOnLoad();
                sheetScroll();
            });                           
            //document.addEventListener("resize", function () {
            //    //pageOnLoad();
            //    //PaneResized();
            //});

            window.addEventListener('unload', function (event) {
                pageOnUnload();
            });
            
            function pageOnUnload() {
                for (var key in intervalState) {
                    clearInterval(intervalState[key]);
                }
            }
            function sheetScroll(){
                var c = $telerik.$("#sheetcontainer");
                var s = $telerik.$("#sheetscroll");
                c.on("wheel", function () {
                    s.show();
                    clearTimeout($telerik.$.data(this, 'timer'));
                    $telerik.$.data(this, 'timer', setTimeout(function () {
                        s.hide();
                    }, 250));
                    spreadShettCellRotate(spreadsheets);
                });
                setTimeout(function () {
                    document.getElementById("meparam").scrollIntoView();
                    $telerik.$('.RadSpreadsheet .rssScroller').on("scroll", function (e) {
                        //console.log(e);
                        e.preventDefault();

                        if (e.target.scrollTop > 0) {
                            e.target.scrollTop = 0;
                        }
                        $telerik.$(e.target).prev().find(".rssData").scrollTop(0);
                        //console.log(e.target.scrollTop, $telerik.$(e.target).prev().find(".rssData").scrollTop());
                    })
                }, 2000);
                let anchorlinks = document.querySelectorAll('a[href^="#"]')

                for (let item of anchorlinks) { // relitere 
                    item.addEventListener('click', (e) => {
                        let hashval = item.getAttribute('href')
                        let target = document.querySelector(hashval)
                        target.scrollIntoView({
                            behavior: 'smooth'
                        })
                        history.pushState(null, null, hashval)
                        e.preventDefault()
                    })
                }
            }
            function PaneResized() {
                var sender = $find('RadSplitter1');
                if (sender) {
                    var browserHeight = $telerik.$(window).height();
                    sender.set_height(browserHeight);  
					var sh = $telerik.$("#spreadsheet");                  
                    var c = $telerik.$("#sheetcontainer");
                    var s = $telerik.$("#sheetscroll");
                    c.height(browserHeight - 18);
                    //sh.height(browserHeight - 18);
                    s.height(c.prop("scrollHeight"));
                    s.width(c.prop("scrollWidth"));
                    onSpreadSheetLoad();
                    //hideToolBarOnSpreadSheet();
                }
                
            }
			window.onresize = PaneResized;
            function pageOnLoad() {

                setTimeout(() => {
                    unitConfiguration = JSON.parse(document.getElementById('hdnmachinery').value);
                    if (unitConfiguration == null || unitConfiguration == undefined)
                        mainenginecount = [];
                    allowentry = document.getElementById('hdnallowentry').value;
                    allowwatchsign = document.getElementById('hdnallowWatchsign').value;
                    allowdaysign = document.getElementById('hdnallowdaysign').value;
                    
                    headyn = '0';

                    validationState = [];
                    spreadsheet = [];
                    
                    var sheetConfig = unitConfiguration.find(x => x.field == 'MAE');
                    if (sheetConfig != null && sheetConfig != undefined)
                        mainenginecount = sheetConfig.Value;
                    
                    if (mainenginecount == "2") {
                        spreadsheets = [
                            { 'sheetName': 'meparam', 'sheetobj': $find("<%=RadSpreadsheet1.ClientID %>"), 'id': 'RadSpreadsheet1' },
                            { 'sheetName': 'metemp', 'sheetobj': $find("<%=RadSpreadsheet2.ClientID %>"), 'id': 'RadSpreadsheet2' },
                            { 'sheetName': 'turbocharger', 'sheetobj': $find("<%=RadSpreadsheet3.ClientID %>"), 'id': 'RadSpreadsheet3' },

                            { 'sheetName': 'meparam2', 'sheetobj': $find("<%=RadSpreadsheet9.ClientID %>"), 'id': 'RadSpreadsheet9' },
                            { 'sheetName': 'metemp2', 'sheetobj': $find("<%=RadSpreadsheet10.ClientID %>"), 'id': 'RadSpreadsheet10' },
                            { 'sheetName': 'turbocharger2', 'sheetobj': $find("<%=RadSpreadsheet11.ClientID %>"), 'id': 'RadSpreadsheet11' },

                            { 'sheetName': 'generator', 'sheetobj': $find("<%=RadSpreadsheet4.ClientID %>"), 'id': 'RadSpreadsheet4' },
                            { 'sheetName': 'misc', 'sheetobj': $find("<%=RadSpreadsheet5.ClientID %>"), 'id': 'RadSpreadsheet5' },
                            { 'sheetName': 'aircond', 'sheetobj': $find("<%=RadSpreadsheet6.ClientID %>"), 'id': 'RadSpreadsheet6' },
                            { 'sheetName': 'noonreport', 'sheetobj': $find("<%=RadSpreadsheet7.ClientID %>"), 'id': 'RadSpreadsheet7' },
                            { 'sheetName': 'remarks', 'sheetobj': $find("<%=RadSpreadsheet8.ClientID %>"), 'id': 'RadSpreadsheet8' },
                        ];

                    } else
                    {
                         spreadsheets = [
                            { 'sheetName': 'meparam', 'sheetobj': $find("<%=RadSpreadsheet1.ClientID %>"), 'id': 'RadSpreadsheet1' },
                            { 'sheetName': 'metemp', 'sheetobj': $find("<%=RadSpreadsheet2.ClientID %>"), 'id': 'RadSpreadsheet2' },
                            { 'sheetName': 'turbocharger', 'sheetobj': $find("<%=RadSpreadsheet3.ClientID %>"), 'id': 'RadSpreadsheet3' },
                            { 'sheetName': 'generator', 'sheetobj': $find("<%=RadSpreadsheet4.ClientID %>"), 'id': 'RadSpreadsheet4' },
                            { 'sheetName': 'misc', 'sheetobj': $find("<%=RadSpreadsheet5.ClientID %>"), 'id': 'RadSpreadsheet5' },
                            { 'sheetName': 'aircond', 'sheetobj': $find("<%=RadSpreadsheet6.ClientID %>"), 'id': 'RadSpreadsheet6' },
                            { 'sheetName': 'noonreport', 'sheetobj': $find("<%=RadSpreadsheet7.ClientID %>"), 'id': 'RadSpreadsheet7' },
                            { 'sheetName': 'remarks', 'sheetobj': $find("<%=RadSpreadsheet8.ClientID %>"), 'id': 'RadSpreadsheet8' },
                        ];
                    }

                    PaneResized();
                    createValidationState();
                    setDefaultWorkSheet(spreadsheets);  // radsplitter and radspreadsheet not working on page load we need to define the active sheet to make spreadsheet works
					//disableSpreasheetScroll(spreadsheets);
                    onSpreadSheetScroll(spreadsheets);
                    spreadShettCellRotate(spreadsheets);
                    openSpreadSheetPopup(spreadsheets);
                    cellLock(spreadsheets);

                    showHideTurbo('turbocharger');
                    showHideGenerator();
                    if (mainenginecount == '2')
                        showHideTurbo('turbocharger2');
                    showHideRow('metemp');
                    if (mainenginecount == '2')
                        showHideRow('metemp2');
     
                    save(spreadsheets);                    
                    checkCheifEngineerValidation(spreadsheets);
                    dayValidation(spreadsheets);
                    activateWatchOnCurrentTime(spreadsheets);
					//onSectionClick('meparam');
                    checkCheifEngineerValidationSchedular(spreadsheets);
                    dayValidationSchedular(spreadsheets);
					hideToolBarOnSpreadSheet();
                    enablewatch(spreadsheets);
                    addimage();
                    saveSchedular(spreadsheets);
                }, 2000);
             }
            function addimage() {
                setTimeout(() => {
                    for(var spreadsheet of spreadsheets)
                    {
                        var kendoSpreadsheet = spreadsheet.sheetobj.get_kendoWidget();
                        var sheet = kendoSpreadsheet.sheetByName(spreadsheet.sheetName);
                        var islock = document.getElementById('hdnislock').value;
                        var imgurl = "../css/Theme1/images/lock_2.png";

                        var result = validationState.find(state => state.cheifengineerSigned == false);
                        if ((result == undefined || result == null) && islock == "1") {
                            imgurl = "../css/Theme1/images/unlock_3.png";
                        }
                        if (spreadsheet.sheetName == "remarks") {
                            var drawing = kendo.spreadsheet.Drawing.fromJSON({
                                topLeftCell: "D35",
                                offsetX: 151,
                                offsetY: 15,
                                width: 16,
                                height: 16,
                                image: kendoSpreadsheet.addImage(imgurl)
                            });
                            sheet.addDrawing(drawing);
                        }
                    }
                },2000);
            }
            function enablewatch(preadsheets)
            {
                var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase().trim() == 'remarks')[0];
                var sheet = spreadsheet.sheetobj.get_activeSheet();
                var startwatch1 = sheet.get_range('A3').get_value();
                var startwatch2 = sheet.get_range('A8').get_value();
                var startwatch3 = sheet.get_range('A13').get_value();
                var startwatch4 = sheet.get_range('A18').get_value();
                var startwatch5 = sheet.get_range('A23').get_value();
                var startwatch6 = sheet.get_range('A28').get_value();

                var endwatch1 = sheet.get_range('A5').get_value();
                var endwatch2 = sheet.get_range('A10').get_value();
                var endwatch3 = sheet.get_range('A15').get_value();
                var endwatch4 = sheet.get_range('A20').get_value();
                var endwatch5 = sheet.get_range('A25').get_value();
                var endwatch6 = sheet.get_range('A30').get_value();

                if (startwatch1 && (!endwatch1 || !isDutyEngineerSigned(startwatch1 + '-' + endwatch1)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A5:A7', true);
                if (startwatch2 && (!endwatch2 || !isDutyEngineerSigned(startwatch2 + '-' + endwatch2)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A10:A12', true);
                if (startwatch3 && (!endwatch3 || !isDutyEngineerSigned(startwatch3 + '-' + endwatch3)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A15:A17', true);
                if (startwatch4 && (!endwatch4 || !isDutyEngineerSigned(startwatch4 + '-' + endwatch4)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A20:A22', true);
                if (startwatch5 && (!endwatch5 || !isDutyEngineerSigned(startwatch5 + '-' + endwatch5)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A25:A27', true);
                if (startwatch6 && (!endwatch6 || !isDutyEngineerSigned(startwatch6 + '-' + endwatch6)) && allowentry == '1')
                    cellRangeEnable(spreadsheet.sheetobj, 'A30:A32', true);
            }
            function OnRemarksClientChange(sender, eventArgs) {

                var cellref = eventArgs.get_range()._range._sheet._viewSelection.originalActiveCell;

                //if (cellref.toString() == 'C3' || cellref.toString() == 'C8' || cellref.toString() == 'C13' || cellref.toString() == 'C18' || cellref.toString() == 'C23' || cellref.toString() == 'C28' || cellref.toString() == 'C35')
                //{
                //    eventArgs.set_cancel(true);
                //}
                //console.log(cellref.toString());
                var cellAddress = cellref.toString().replace(/\d+/g, '');
                if (cellAddress != 'A')
                    return;
                var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase().trim() == 'remarks')[0];
                var sheet = spreadsheet.sheetobj.get_activeSheet();
                var startwatch1 = sheet.get_range('A3').get_value();
                var startwatch2 = sheet.get_range('A8').get_value();
                var startwatch3 = sheet.get_range('A13').get_value();
                var startwatch4 = sheet.get_range('A18').get_value();
                var startwatch5 = sheet.get_range('A23').get_value();
                var startwatch6 = sheet.get_range('A28').get_value();

                var endwatch1 = sheet.get_range('A5').get_value();
                var endwatch2 = sheet.get_range('A10').get_value();
                var endwatch3 = sheet.get_range('A15').get_value();
                var endwatch4 = sheet.get_range('A20').get_value();
                var endwatch5 = sheet.get_range('A25').get_value();
                var endwatch6 = sheet.get_range('A30').get_value();

                var spreadsheetremark = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'remarks')[0];
                var sheetremark = spreadsheetremark.sheetobj.get_activeSheet();
                

                var watchAddress = getLogWatchAddress();

                var watchval = eventArgs.get_range().get_value();

                if (cellref == 'A5') {
                    var rangeremark = sheetremark.get_range('A8');
                    if (watchval == rangeremark.get_value())
                        return;

                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '1')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2') {
                                    range.set_value(startwatch1 + ' - ' + watchval);
									}
                                else
                                    range.set_value(startwatch1 + ' ' + watchval);
                            });
                        }
                    }
                    rangeremark.set_value(watchval);
                    enablewatch(spreadsheets);
                }

                if (cellref == 'A10') {
                    var rangeremark = sheetremark.get_range('A13');
                    if (watchval == rangeremark.get_value())
                        return;
                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '2')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2')
                                    range.set_value(startwatch2 + ' - ' + watchval);
                                else
                                    range.set_value(startwatch2 + ' ' + watchval);
                            });
                        }
                    }
                    rangeremark.set_value(watchval);
                    enablewatch(spreadsheets);
                }
               
                if (cellref == 'A15') {
                    var rangeremark = sheetremark.get_range('A18');
                    if (watchval == rangeremark.get_value())
                        return;
                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '3')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2')
                                    range.set_value(startwatch3 + ' - ' + watchval);
                                else
                                    range.set_value(startwatch3 + ' ' + watchval);
                            });
                        }
                    }
                    rangeremark.set_value(watchval);
                    enablewatch(spreadsheets);
                }

                if (cellref == 'A20') {
                    var rangeremark = sheetremark.get_range('A23');
                    if (watchval == rangeremark.get_value())
                        return;
                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '4')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2')
                                    range.set_value(startwatch4 + ' - ' + watchval);
                                else
                                    range.set_value(startwatch4 + ' ' + watchval);
                            });
                        }
                    }
                    rangeremark.set_value(watchval);
                    enablewatch(spreadsheets);
                }

                if (cellref == 'A25') {
                    var rangeremark = sheetremark.get_range('A28');
                    if (watchval == rangeremark.get_value())
                        return;
                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '5')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2')
                                    range.set_value(startwatch5 + ' - ' + watchval);
                                else
                                    range.set_value(startwatch5 + ' ' + watchval);
                            });
                        }
                    }
                    rangeremark.set_value(watchval);
                    enablewatch(spreadsheets);
                }
                if (cellref == 'A30') {
                    for (var spreadsheet of spreadsheets) {
                        if (spreadsheet && spreadsheet.sheetName != 'remarks' && spreadsheet.sheetName != 'noonreport') {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase() && w.watchref == '6')[0]
                            logAddress.watchadd.forEach(w => {
                                var range = sheet.get_range(w);
                                headyn = '1';
                                if (spreadsheet.sheetName == 'meparam' || spreadsheet.sheetName == 'metemp' || spreadsheet.sheetName == 'meparam2' || spreadsheet.sheetName == 'metemp2')
                                    range.set_value(startwatch6 + ' - ' + watchval);
                                else
                                    range.set_value(startwatch6 + ' ' + watchval);
                            });
                        }
                    }
                }
                activateWatchOnCurrentTime(spreadsheets);
            }
            function OnClientChange(sender, eventArgs) {
                //returns the new value of the cell
                if (headyn == '1') {
                    headyn = '0';
                    return;
                }
                
                var paramname = "";
                var newval = eventArgs.get_range().get_value();
                currentsheet = eventArgs.get_range()._range._sheet.name();

                var cellref = eventArgs.get_range()._range._sheet._viewSelection.originalActiveCell;

                var validateaddress = getValidateAddress();
                var logs = validateaddress.filter(a=> a.logname.toLowerCase() == currentsheet.toLowerCase() )
                if (logs != null && logs != undefined) {
                    for(var log of logs)
                    {
                        log.address.forEach(a=> {
                            if(a==cellref.toString())
                                paramname = log.field;
                        });
                    } 
                }

                if (newval && paramname != null && paramname != undefined) {
                    var value = document.getElementById('hdnValidateStatus').value;
                    if (value == null || value == undefined)
                        return;
                    var validation = JSON.parse(value).filter(s => s.field.toLowerCase() == paramname.toLowerCase() && s.field.toLowerCase() != 'ME FO ME' && s.field.toLowerCase() != 'Control Location');
                    if (validation.length <= 0)
                        return;

                    if (isNaN(newval)) {
                        eventArgs.get_range().set_value('');
                        //eventArgs.set_cancel(true);
                        return;
                    }
                    
                    if (validation.length > 0) {
                        if (validation[0].max >0 && newval > validation[0].max ) {
                            alert('Given value is greater than the configured value (' + validation[0].max.toString()+').')
                            eventArgs.get_range().set_value('');
                        }
                        if (validation[0].min > 0 && newval < validation[0].min) {
                            alert('Given value is less than the configured value (' + validation[0].min.toString() + ').')
                            eventArgs.get_range().set_value('');
                        }
                        if ((validation[0].maxalert >0 && newval > validation[0].maxalert) || (validation[0].minalert>0 && newval < validation[0].minalert)) {
                            eventArgs.get_range().set_color('#ffa64d');
                        } else {
                            eventArgs.get_range().set_color('blue');
                        }
                    }
                }
            }

            function getValidateAddress() {
                var validateaddress = [
                    //{ 'logname': 'meparam', 'field': 'FLDMEAVGRPM', 'address': ['C9', 'D9', 'E9', 'F9', 'G9', 'H9'] },
                    { 'logname': 'meparam', 'field': 'FLDMEFOC', 'address': ['C15', 'D15', 'E15', 'F15', 'G15', 'H15'] },
                    //{ 'logname': 'meparam', 'field': 'FLDMAXEXHTEMP', 'address': ['C9', 'D9', 'E9', 'F9', 'G9', 'H9'] },
                    //{ 'logname': 'meparam', 'field': 'FLDMINEXHTEMP', 'address': ['C9', 'D9', 'E9', 'F9', 'G9', 'H9'] },
                    
                    { 'logname': 'meparam', 'field': 'FLDHOURS', 'address': ['C5', 'D5', 'E5', 'F5', 'G5', 'H5'] },
                    { 'logname': 'meparam', 'field': 'FLDMINS', 'address': ['C6', 'D6', 'E6', 'F6', 'G6', 'H6'] },
                    { 'logname': 'meparam', 'field': 'FLDMECOUNTER', 'address': ['C8', 'D8', 'E8', 'F8', 'G8', 'H8'] },
                    { 'logname': 'meparam', 'field': 'FLDMEGOVERNORCONTROL', 'address': ['C12', 'D12', 'E12', 'F12', 'G12', 'H12'] },
                    { 'logname': 'meparam', 'field': 'FLDMEFOVISCOSITY', 'address': ['C9', 'D9', 'E9', 'F9', 'G9', 'H9'] },
                    { 'logname': 'meparam', 'field': 'FLDMEFOFLOWMETER', 'address': ['C14', 'D14', 'E14', 'F14', 'G14', 'H14'] },
                    { 'logname': 'meparam', 'field': 'FLDCYLOILTANKLEVEL', 'address': ['C17', 'D17', 'E17', 'F17', 'G17', 'H17'] },
                    { 'logname': 'meparam', 'field': 'FLDMECYLOILCONS', 'address': ['C18', 'D18', 'E18', 'F18', 'G18', 'H18'] },
                    { 'logname': 'meparam', 'field': 'FLDMESUMP', 'address': ['C19', 'D19', 'E19', 'F19', 'G19', 'H19'] },
                    { 'logname': 'metemp', 'field': 'FLDMEJACKETCOOLTEMPIN', 'address': ['D5', 'G5', 'J5', 'M5', 'P5', 'S5'] },
                    {
                        'logname': 'metemp', 'field': 'FLDMEJACKETCOOLTEMPOUT', 'address': ['D6', 'D7', 'D8', 'D9', 'D10', 'D11', 'D12', 'D13', 'D14', 'D15', 'D16', 'D17', 'D18', 'D19', 'D20', 'D21',
                                                                                            'G6', 'G7', 'G8', 'G9', 'G10', 'G11', 'G12', 'G13', 'G14', 'G15', 'G16', 'G17', 'G18', 'G19', 'G20', 'G21',
                                                                                            'J6', 'J7', 'J8', 'J9', 'J10', 'J11', 'J12', 'J13', 'J14', 'J15', 'J16', 'J17', 'J18', 'J19', 'J20', 'J21',
                                                                                            'M6', 'M7', 'M8', 'M9', 'M10', 'M11', 'M12', 'M13', 'M14', 'M15', 'M16', 'M17', 'M18', 'M19', 'M20', 'M21',
                                                                                            'P6', 'P7', 'P8', 'P9', 'P10', 'P11', 'P12', 'P13', 'P14', 'P15', 'P16', 'P17', 'P18', 'P19', 'P20', 'P21',
                                                                                            'S6', 'S7', 'S8', 'S9', 'S10', 'S11', 'S12', 'S13', 'S14', 'S15', 'S16', 'S17', 'S18', 'S19', 'S20', 'S21']
                    },
                    { 'logname': 'metemp', 'field': 'FLDMEPISTONCOOLTEMPIN', 'address': ['E5', 'H5', 'K5', 'N5', 'Q5', 'T5'] },
                    {
                        'logname': 'metemp', 'field': 'FLDMEPISTONCOOLTEMOUT', 'address': ['E6', 'E7', 'E8', 'E9', 'E10', 'E11', 'E12', 'E13', 'E14', 'E15', 'E16', 'E17', 'E18', 'E19', 'E20', 'E21',
                                                                                              'H6', 'H7', 'H8', 'H9', 'H10', 'H11', 'H12', 'H13', 'H14', 'H15', 'H16', 'H17', 'H18', 'H19', 'H20', 'H21',
                                                                                              'K6', 'K7', 'K8', 'K9', 'K10', 'K11', 'K12', 'K13', 'K14', 'K15', 'K16', 'K17', 'K18', 'K19', 'K20', 'K21',
                                                                                              'N6', 'N7', 'N8', 'N9', 'N10', 'N11', 'N12', 'N13', 'N14', 'N15', 'N16', 'N17', 'N18', 'N19', 'N20', 'N21',
                                                                                              'Q6', 'Q7', 'Q8', 'Q9', 'Q10', 'Q11', 'Q12', 'Q13', 'Q14', 'Q15', 'Q16', 'Q17', 'Q18', 'Q19', 'Q20', 'Q21',
                                                                                              'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12', 'T13', 'T14', 'T15', 'T16', 'T17', 'T18', 'T19', 'T20', 'T21']
                    },
                    { 'logname': 'turbocharger', 'field': 'FLDEXHGASTEMPTCINBOARDBEFORE', 'address': ['D6', 'E6', 'F6', 'G6', 'H6', 'I6', 'J6', 'K6', 'L6', 'M6', 'N6', 'O6', 'P6', 'Q6', 'R6', 'S6', 'T6', 'U6', 'V6', 'W6', 'X6', 'Y6', 'Z6', 'AA6'] },
                    { 'logname': 'turbocharger', 'field': 'FLDEXHGASTEMPTCOUTBOARDBEFORE', 'address': ['D7', 'E7', 'F7', 'G7', 'H7', 'I7', 'J7', 'K7', 'L7', 'M7', 'N7', 'O7', 'P7', 'Q7', 'R7', 'S7', 'T7', 'U7', 'V7', 'W7', 'X7', 'Y7', 'Z7', 'AA7'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCAIRCLRAIRTEMPIN', 'address': ['D8', 'E8', 'F8', 'G8', 'H8', 'I8', 'J8', 'K8', 'L8', 'M8', 'N8', 'O8', 'P8', 'Q8', 'R8', 'S8', 'T8', 'U8', 'V8', 'W8', 'X8', 'Y8', 'Z8', 'AA8'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCAIRCLRAIRTEMOUT', 'address': ['D9', 'E9', 'F9', 'G9', 'H9', 'I9', 'J9', 'K9', 'L9', 'M9', 'N9', 'O9', 'P9', 'Q9', 'R9', 'S9', 'T9', 'U9', 'V9', 'W9', 'X9', 'Y9', 'Z9', 'AA9'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCCOOLWTRTEMPIN', 'address': ['D10', 'E10', 'F10', 'G10', 'H10', 'I10', 'J10', 'K10', 'L10', 'M10', 'N10', 'O10', 'P10', 'Q10', 'R10', 'S10', 'T10', 'U10', 'V10', 'W10', 'X10', 'Y10', 'Z10', 'AA10'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCCOOLWTRTEMPOUT', 'address': ['D11', 'E11', 'F11', 'G11', 'H11', 'I11', 'J11', 'K11', 'L11', 'M11', 'N11', 'O11', 'P11', 'Q11', 'R11', 'S11', 'T11', 'U11', 'V11', 'W11', 'X11', 'Y11', 'Z11', 'AA11'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCLUBEOILTEMP', 'address': ['D12', 'E12', 'F12', 'G12', 'H12', 'I12', 'J12', 'K12', 'L12', 'M12', 'N12', 'O12', 'P12', 'Q12', 'R12', 'S12', 'T12', 'U12', 'V12', 'W12', 'X12', 'Y12', 'Z12', 'AA12'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCSCAVAIRPRES', 'address': ['D14', 'E14', 'F14', 'G14', 'H14', 'I14', 'J14', 'K14', 'L14', 'M14', 'N14', 'O14', 'P14', 'Q14', 'R14', 'S14', 'T14', 'U14', 'V14', 'W14', 'X14', 'Y14', 'Z14', 'AA14'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCAIRCLRPRESDROP', 'address': ['D16', 'E16', 'F16', 'G16', 'H16', 'I16', 'J16', 'K16', 'L16', 'M16', 'N16', 'O16', 'P16', 'Q16', 'R16', 'S16', 'T16', 'U16', 'V16', 'W16', 'X16', 'Y16', 'Z16', 'AA16'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCAIRSUCFILPRESDRP', 'address': ['D17', 'E17', 'F17', 'G17', 'H17', 'I17', 'J17', 'K17', 'L17', 'M17', 'N17', 'O17', 'P17', 'Q17', 'R17', 'S17', 'T17', 'U17', 'V17', 'W17', 'X17', 'Y17', 'Z17', 'AA17'] },
                    { 'logname': 'turbocharger', 'field': 'FLDMETCTURBINEPRESSDROP', 'address': ['D18', 'E18', 'F18', 'G18', 'H18', 'I18', 'J18', 'K18', 'L18', 'M18', 'N18', 'O18', 'P18', 'Q18', 'R18', 'S18', 'T18', 'U18', 'V18', 'W18', 'X18', 'Y18', 'Z18', 'AA18'] },
                    { 'logname': 'generator', 'field': 'FLDAERUNNINGHRS', 'address': ['D4', 'E4', 'F4', 'G4', 'H4', 'I4', 'J4', 'K4', 'L4', 'M4', 'N4', 'O4', 'P4', 'Q4', 'R4', 'S4', 'T4', 'U4', 'V4', 'W4', 'X4', 'Y4', 'Z4', 'AA4'] },
                    { 'logname': 'generator', 'field': 'FLDAEEXHGASTEMPMIN', 'address': ['D6', 'E6', 'F6', 'G6', 'H6', 'I6', 'J6', 'K6', 'L6', 'M6', 'N6', 'O6', 'P6', 'Q6', 'R6', 'S6', 'T6', 'U6', 'V6', 'W6', 'X6', 'Y6', 'Z6', 'AA6'] },
                    { 'logname': 'generator', 'field': 'FLDAEEXHGASTEMPMAX', 'address': ['D7', 'E7', 'F7', 'G7', 'H7', 'I7', 'J7', 'K7', 'L7', 'M7', 'N7', 'O7', 'P7', 'Q7', 'R7', 'S7', 'T7', 'U7', 'V7', 'W7', 'X7', 'Y7', 'Z7', 'AA7'] },
                    { 'logname': 'generator', 'field': 'FLDAEBOOSTAIRTEMP', 'address': ['D12', 'E12', 'F12', 'G12', 'H12', 'I12', 'J12', 'K12', 'L12', 'M12', 'N12', 'O12', 'P12', 'Q12', 'R12', 'S12', 'T12', 'U12', 'V12', 'W12', 'X12', 'Y12', 'Z12', 'AA12'] },
                    { 'logname': 'generator', 'field': 'FLDAEPEDESTALBEARINGTEMP', 'address': ['D13', 'E13', 'F13', 'G13', 'H13', 'I13', 'J13', 'K13', 'L13', 'M13', 'N13', 'O13', 'P13', 'Q13', 'R13', 'S13', 'T13', 'U13', 'V13', 'W13', 'X13', 'Y13', 'Z13', 'AA13'] },
                    { 'logname': 'generator', 'field': 'FLDAELUBEOILPRESS', 'address': ['D15', 'E15', 'F15', 'G15', 'H15', 'I15', 'J15', 'K15', 'L15', 'M15', 'N15', 'O15', 'P15', 'Q15', 'R15', 'S15', 'T15', 'U15', 'V15', 'W15', 'X15', 'Y15', 'Z15', 'AA15'] },
                    { 'logname': 'generator', 'field': 'FLDAECOOLINGWATERPRESS', 'address': ['D16', 'E16', 'F16', 'G16', 'H16', 'I16', 'J16', 'K16', 'L16', 'M16', 'N16', 'O16', 'P16', 'Q16', 'R16', 'S16', 'T16', 'U16', 'V16', 'W16', 'X16', 'Y16', 'Z16', 'AA16'] },
                    { 'logname': 'generator', 'field': 'FLDAEBOOSTAIRPRESS', 'address': ['D17', 'E17', 'F17', 'G17', 'H17', 'I17', 'J17', 'K17', 'L17', 'M17', 'N17', 'O17', 'P17', 'Q17', 'R17', 'S17', 'T17', 'U17', 'V17', 'W17', 'X17', 'Y17', 'Z17', 'AA17'] },
                    { 'logname': 'generator', 'field': 'FLDAEFUELOILPRESS', 'address': ['D18', 'E18', 'F18', 'G18', 'H18', 'I18', 'J18', 'K18', 'L18', 'M18', 'N18', 'O18', 'P18', 'Q18', 'R18', 'S18', 'T18', 'U18', 'V18', 'W18', 'X18', 'Y18', 'Z18', 'AA18'] },
                    { 'logname': 'generator', 'field': 'FLDAECURRENT', 'address': ['D20', 'E20', 'F20', 'G20', 'H20', 'I20', 'J20', 'K20', 'L20', 'M20', 'N20', 'O20', 'P20', 'Q20', 'R20', 'S20', 'T20', 'U20', 'V20', 'W20', 'X20', 'Y20', 'Z20', 'AA20'] },
                    { 'logname': 'generator', 'field': 'FLDAELOAD', 'address': ['D21', 'E21', 'F21', 'G21', 'H21', 'I21', 'J21', 'K21', 'L21', 'M21', 'N21', 'O21', 'P21', 'Q21', 'R21', 'S21', 'T21', 'U21', 'V21', 'W21', 'X21', 'Y21', 'Z21', 'AA21'] },
                    { 'logname': 'generator', 'field': 'FLDDOSERVICETANKLEVEL', 'address': ['D22', 'E22', 'F22', 'G22', 'H22', 'I22', 'J22', 'K22', 'L22', 'M22', 'N22', 'O22', 'P22', 'Q22', 'R22', 'S22', 'T22', 'U22', 'V22', 'W22', 'X22', 'Y22', 'Z22', 'AA22'] },
                    { 'logname': 'noonreport', 'field': 'FLDMEAVGRPM', 'address': ['D11'] },
                    { 'logname': 'noonreport', 'field': 'FLDMAXEXHTEMP', 'address': ['I17'] },
                    { 'logname': 'noonreport', 'field': 'FLDMINEXHTEMP', 'address': ['I18'] },
                    { 'logname': 'noonreport', 'field': 'FLDFOCATFINES', 'address': ['I13'] },
                    { 'logname': 'noonreport', 'field': 'FLDMEFOAUTOBACKWASHFILTERCOUNTER', 'address': ['D21'] },
                    { 'logname': 'noonreport', 'field': 'FLDMELOAUTOBACKWASHFILTERCOUNTER', 'address': ['I21'] },
                    { 'logname': 'noonreport', 'field': 'FLDMEFOVISCOSITY', 'address': ['I8'] },
                    { 'logname': 'noonreport', 'field': 'FLDBOILERCHLORIDES', 'address': ['O23'] },
                    { 'logname': 'noonreport', 'field': 'FLDSPEED', 'address': ['D6'] },
                ];
                return validateaddress;
            }

             function onSectionClick(id, event) {
                 //var a = document.createElement('a')
                 //var body = document.getElementsByTagName('body')[0];
                 //a.style.visibility = "hidden";
                 //a.href = '#' + id;
                 //a.id = 'dynamicanchor';
                 //body.appendChild(a);
                 //a.click();
                 //body.removeChild(a);
                 //currentsheet = id;
                 //return false;
             }

             function disableSpreasheetScroll(spreadsheets) {
                 //for (var spreadsheet of spreadsheets) {
                 //    if (spreadsheet) {
                 //        var scrollarea = $(spreadsheet.sheetobj.get_element()).find('.rssScroller');
                 //        scrollarea.css('overflow', 'hidden');
                 //    }
                 //}
             }

             function onSpreadSheetScroll(spreadsheets) {
                 //for (var spreadsheet of spreadsheets) {
                 //    if (spreadsheet) {
                 //        $(spreadsheet.sheetobj.get_element()).find(".rssScroller").on("scroll", function (ev) {
                 //            hideToolBarOnSpreadSheet();
                 //            if ($(this).scrollTop() > 0) {
                 //                this.scrollTop = 0;
                 //            }

                 //        });
                 //    }
                 //}
             }
             function onSpreadSheetLoad() { // pending need to complete it
                <%-- var height = window.innerHeight;
                 var width = window.innerHeight;
                $("<%= RadSpreadsheet1.ClientID %>").css({ 'height': height});
                $("<%= RadSpreadsheet1.ClientID %>").css({ 'width': width});
                 $("<%= RadSpreadsheet1.ClientID %>").resize();

                 $("<%= RadSpreadsheet7.ClientID %>").css({ 'height': height});
                $("<%= RadSpreadsheet7.ClientID %>").css({ 'width': width});
                $("<%= RadSpreadsheet7.ClientID %>").resize();--%>
                 
            }
            function hideToolBarOnSpreadSheet() {
            //    $telerik.$(".rssToolbarWrapper").hide();
            //    $telerik.$(".rssFormulaBar").hide();
            //    $telerik.$(".rssRowHeader").hide();
            //    $telerik.$(".rssColumnHeader").hide();
            //    $telerik.$(".rssTopCorner").hide();
            //    $telerik.$(".rssSheetsbar").hide();
            }
            function setDefaultWorkSheet(spreadsheets) {
                for (var spreadsheet of spreadsheets) {
                    if (spreadsheet) {
                        var kendoSpreadsheet = spreadsheet.sheetobj.get_kendoWidget();
                        var sheet = kendoSpreadsheet.sheetByName(spreadsheet.sheetName);
                        if (sheet) {
                            kendoSpreadsheet.activeSheet(sheet);
                        }
                        //hideToolBarOnSpreadSheet();

                        if (spreadsheet.sheetName == "remarks") {
                            enablesignlink();
                        }
						if (spreadsheet.sheetName == "meparam") {
                            enableWatchLink();
                        }

                    }
                }

            }
            function enablesignlink()
            {
                var sh = $find("<%=RadSpreadsheet8.ClientID %>").get_activeSheet();
                sh.get_range("C3").set_link("#watch1");
                sh.get_range("C8").set_link("#watch2");
                sh.get_range("C13").set_link("#watch3");
                sh.get_range("C18").set_link("#watch4");
                sh.get_range("C23").set_link("#watch5");
                sh.get_range("C28").set_link("#watch6");

                sh.get_range("C35").set_link("#cheifengineer");

                var shn = $find("<%=RadSpreadsheet7.ClientID %>").get_activeSheet();
                shn.get_range("B3:B6").set_link("#rotate");
                shn.get_range("B8:B16").set_link("#rotate");
                shn.get_range("B17:B19").set_link("#rotate");
                shn.get_range("E3:E6").set_link("#rotate");
                shn.get_range("E8:E13").set_link("#rotate");
                shn.get_range("E14:E16").set_link("#rotate");
                shn.get_range("E17:E19").set_link("#rotate");
                shn.get_range("E27:E28").set_link("#rotate");
                shn.get_range("L3:L5").set_link("#rotate");
                shn.get_range("L6:L8").set_link("#rotate");
                shn.get_range("P3:P5").set_link("#rotate");
                shn.get_range("P6:P8").set_link("#rotate");
                shn.get_range("V27:V28").set_link("#rotate");
                shn.get_range("W27:W28").set_link("#rotate");
                shn.get_range("X27:X28").set_link("#rotate");
                shn.get_range("Y27:Y28").set_link("#rotate");
                shn.get_range("F28").set_link("#rotate");
                shn.get_range("G28").set_link("#rotate");
                shn.get_range("H28").set_link("#rotate");
                shn.get_range("I28").set_link("#rotate");
                shn.get_range("J28").set_link("#rotate");
                shn.get_range("K28").set_link("#rotate");
                shn.get_range("L28").set_link("#rotate");
                shn.get_range("M28").set_link("#rotate");
                shn.get_range("N28").set_link("#rotate");
                shn.get_range("O28").set_link("#rotate");
                shn.get_range("P28").set_link("#rotate");
                shn.get_range("Q28").set_link("#rotate");
                shn.get_range("R28").set_link("#rotate");
                shn.get_range("S28").set_link("#rotate");
                shn.get_range("T28").set_link("#rotate");
                shn.get_range("U28").set_link("#rotate");
            }
			function enableWatchLink() {

                var sh = $find("<%=RadSpreadsheet1.ClientID %>").get_activeSheet();

			    var obj = sh.get_range("C3");
                if (obj.get_value() == null || obj.get_value() == "0000 - ") {
                    obj.set_link("#remarks");                    
                }
                obj.set_enabled(false);
                obj = sh.get_range("D3");
                if (obj.get_value() == null || obj.get_value() == "Create Watch") {
                    obj.set_link("#remarks");
                    obj.set_enabled(false);
                }
                obj = sh.get_range("E3");
                if (obj.get_value() == null || obj.get_value() == "Create Watch") {
                   // obj.set_value("Create Watch");
                    obj.set_link("#remarks");
                }
                obj = sh.get_range("F3");
                if (obj.get_value() == null || obj.get_value() == "Create Watch") {
                    //obj.set_value("Create Watch");
                    obj.set_link("#remarks");
                }
                obj = sh.get_range("G3");
                if (obj.get_value() == null || obj.get_value() == "Create Watch") {
                   // obj.set_value("Create Watch");
                    obj.set_link("#remarks");
                    obj.set_enabled(false);
                }
                obj = sh.get_range("H3");
                if (obj.get_value() == null || obj.get_value() == "Create Watch") {
                   // obj.set_value("Create Watch");
                    obj.set_link("#remarks");
                    obj.set_enabled(false);
                }

                $telerik.$("#RadSpreadsheet1 .rssData a").each(function (index) {
                    $telerik.$(this).removeAttr("target");
                    $telerik.$(this).attr("href", "#remarks");
                    $telerik.$(this).attr("style", "text-decoration: none; color: blue;");                    
                });
                
            }


            function closeExport() {
                $('.export-window').css('display', 'none')
            }

            function openExport() {
                $('.export-window').css('display', 'block')
            }

            function selectAll() {
                $("#checkall").click(function () {
                    $(".checkbox").prop('checked', $(this).prop('checked'));
                });
            }

            function openSpreadSheetPopup(spreadsheets) {
                if (spreadsheets) {
                    
                    var spreadsheet = spreadsheets.filter(s => s.sheetName == 'remarks')[0];
                    $telerik.$(spreadsheet.sheetobj.get_element()).on('click', '.k-spreadsheet-cell a', function (ev) {
                        ev.preventDefault();
                        
                        var meta = getMetaFromHref($telerik.$(this).attr('href'));
                         var selectedDate = $find("<%= txtdate.ClientID %>");
                          var seldate = selectedDate.get_selectedDate();
                         var date = `${seldate.getFullYear()}-${seldate.getMonth() + 1}-${seldate.getDate()} ${seldate.getHours()}:${seldate.getMinutes()}:${seldate.getSeconds()} `;
                         var spreadsheetwatch = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'meparam');
                         var sheetwatch = spreadsheetwatch[0].sheetobj.get_activeSheet();
                         var watch = '';
                         if (meta["watch"] == 'watch1') watch = sheetwatch.get_range('C3').get_value();
                         if (meta["watch"] == 'watch2') watch = sheetwatch.get_range('D3').get_value();
                         if (meta["watch"] == 'watch3') watch = sheetwatch.get_range('E3').get_value();
                         if (meta["watch"] == 'watch4') watch = sheetwatch.get_range('F3').get_value();
                         if (meta["watch"] == 'watch5') watch = sheetwatch.get_range('G3').get_value();
                         if (meta["watch"] == 'watch6') watch = sheetwatch.get_range('H3').get_value();
                         var error = canPopupOpen($telerik.$(this).attr('href'), watch);

                        //validation before popup
                        //if (canPopupOpen($(this).attr('href'), meta["watch"]) == false) {
                        //    return;
                        //}

                         if (!error)
                             return;
                         
                        save(spreadsheets); // save the spreadsheet before 
                        if (this.href.indexOf('cheifengineer') > 0) {
                            javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogELCheifEngineerSignature.aspx?watch=${watch}&date=${date}&error=${error}&watchno=${meta['watch']}`, 'false', '405', '180', null, null, { 'disableMinMax': true })
                        } else {
                            javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogELOfficerSignature.aspx?watch=${watch}&date=${date}&error=${error}&watchno=${meta['watch']}`, 'false', '405', '180', null, null, { 'disableMinMax': true })
                        }
                      });
                }

                $telerik.$(spreadsheet.sheetobj.get_element()).on('click', '.k-spreadsheet-drawing-image', function (ev) {
                    ev.preventDefault();
                    var watch = '';
                    var islock = document.getElementById('hdnislock').value;
                    
                    islock =  islock.toString() == "0" ? "1" : "0";
                    var selectedDate = $find("<%= txtdate.ClientID %>");
                          var seldate = selectedDate.get_selectedDate();
                          var date = `${seldate.getFullYear()}-${seldate.getMonth() + 1}-${seldate.getDate()} ${seldate.getHours()}:${seldate.getMinutes()}:${seldate.getSeconds()} `;
                          if (isCheifEngineerSigned() && allowdaysign == "1")
                             javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogELCheifEngineerSignature.aspx?watch=${watch}&date=${date}&error=true&lock=${islock}`, 'false', '405', '180', null, null, { 'disableMinMax': true });
                });


            }

            function onCancelPopup() {
                
            }

            function canPopupOpen(context, watch) {
                if (context.indexOf('cheifengineer') == -1 && isDutyEngineerSigned(watch)) {
                    //alert('selected watch is already signed');
                    return false;
                }
                if (context.indexOf('cheifengineer') == -1 && allowwatchsign == "0") {
                    //alert('selected watch is already signed');
                    return false;
                }

                if (context.indexOf('cheifengineer') == -1 && isEveryWatchSigned(watch) == false) {
                    //alert('Please complete the current watch related log and try again');
                    return false;
                }

                if (context.indexOf('cheifengineer') >= 0 && isAllLogCompleted() == false) {
                    //alert('Please complete the current watch related log and try again');
                    return false;
                }

                if (context.indexOf('cheifengineer') >= 0 && isCheifEngineerSigned()) {
                    //alert('Cheif Engineer already signed');
                    return false;
                }
                if (context.indexOf('cheifengineer') >= 0 && allowdaysign == "0") {
                    //alert('Cheif Engineer already signed');
                    return false;
                }

                return true;
            }

            function isDutyEngineerSigned(watch) {
                if (watch) {
                    var states = validationState.filter(state => state.watch.toLowerCase() == watch.toLowerCase());
                    var result = states.find(state => state.dutyengineerSigned == false);
                    if (result == undefined || result == null) {
                        return true;
                    }
                }
                return false;
            }

            function isEveryWatchSigned(watch) {
                if (watch) {
                    
                    var states = validationState.filter(state => state.watch.toLowerCase() == watch.toLowerCase() & state.logname != 'noonreport');
                    var result = states.find(state => state.validation == false);
                    
                    if (result == undefined || result == null) {
                        return true;
                    }
                }
                return false;
            }

            function isAllLogCompleted() {

                var states = validationState.filter(state => state.logname == 'noonreport');
                var result = states.find(state => state.validation == false);
                if (result != undefined && result != null) {
                    return false;
                }

                    var result = validationState.find(state => state.dutyengineerSigned == false);
                    if (result == undefined || result == null) {
                        return true;
                    }
                    
                return false;
            }

            function isCheifEngineerSigned() {
                var result = validationState.find(state => state.cheifengineerSigned == false);
                if (result == undefined || result == null) {
                    return true;
                }
                return false;
            }

            function openHistory(date) {
                javascript: parent.openNewWindow('engineerhistory', '', 'Log/ElectricLogEngineLogHistroy.aspx?date='+ date, 'true', null, null, null, null, { 'disableMinMax': true })
            }
            function openHistory() {
                var selectedDate = $find("<%= txtdate.ClientID %>");
                var seldate = new Date(selectedDate.get_selectedDate());
                var curr_date = seldate.getDate();
                var curr_month = seldate.getMonth() + 1; //Months are zero based
                var curr_year = seldate.getFullYear();
                var date = curr_date.toString() + '/' + curr_month.toString() + '/' + curr_year.toString();
                javascript: parent.openNewWindow('engineerhistory', '', 'Log/ElectricLogEngineLogHistroy.aspx?date=' + date, 'true', null, null, null, null, { 'disableMinMax': true })
            }
            function openConfig() {
                javascript: parent.openNewWindow('Configuration', '', 'Log/ElectricLogEngineAttributes.aspx', 'true', null, null, null, null, { 'disableMinMax': true })
            }

            function getMetaFromHref(hrefValue) {
                var result = {};
                if (hrefValue) {
                    var value = hrefValue.split('#');
                    result['watch'] = value[1];
                    result['nameCellAddress'] = '';
                    result['rankCellAddress'] = '';
                    result['dateCellAddress'] = '';
                }
                return result;
            }

            function cellLock(spreadsheets) {
                if (mainenginecount == "2")
                {
                    var cellRange = {
                        'meparam': ['A1:Z100'],
                        'metemp': ['A1:Z100'],
                        'turbocharger': ['A1:AA100'],
                        'meparam2': ['A1:Z100'],
                        'metemp2': ['A1:Z100'],
                        'turbocharger2': ['A1:AA100'],
                        'misc': ['A1:Z100'],
                        'generator': ['A1:AA100'],
                        'aircond': ['A1:Z100'],
                        'noonreport': ['A1:Z100'],
                        'remarks': ['A1:Z100']
                    };
                }
                else
                {
                    var cellRange = {
                        'meparam': ['A1:Z100'],
                        'metemp': ['A1:Z100'],
                        'turbocharger': ['A1:AA100'],
                        'misc': ['A1:Z100'],
                        'generator': ['A1:AA100'],
                        'aircond': ['A1:Z100'],
                        'noonreport': ['A1:Z100'],
                        'remarks': ['A1:Z100']
                    };
                }

                for (var spreadsheet of spreadsheets) {
                    if (spreadsheet) {
                        var sheetName = spreadsheet.sheetobj.get_kendoWidget().activeSheet().name();
                        var cellRanges = cellRange[sheetName] || [];
                        cellRanges.forEach(r => {
                            var sheet = spreadsheet.sheetobj.get_activeSheet();
                            var range = sheet.get_range(r);
                            range.set_enabled(false);
                            
                        });
                    }
                }
            }

            function cellRangeEnable(spreadsheetObj, cellRange, enable) {
                var sheet = spreadsheetObj.get_activeSheet();
                var range = sheet.get_range(cellRange);
                range.set_enabled(enable);
            }

            function spreadShettCellRotate(spreadsheets) {
                if (spreadsheets) {

                    var spreadsheet = spreadsheets.filter(s => s.sheetName == 'metemp')[0];

                    for (var o of $telerik.$(spreadsheet.sheetobj.get_element()).find('.k-spreadsheet-cell a')) {
                        $telerik.$(o).addClass('rotate-text');
                    }

                    if (mainenginecount == "2") {
                        var spreadsheettemp = spreadsheets.filter(s => s.sheetName == 'metemp2')[0];
                        for (var o2 of $telerik.$(spreadsheettemp.sheetobj.get_element()).find('.k-spreadsheet-cell a')) {
                            $telerik.$(o2).addClass('rotate-text');
                        }
                    }

                    var spreadsheetnoonreport = spreadsheets.filter(s => s.sheetName == 'noonreport')[0];
                    for (var o3 of $telerik.$(spreadsheetnoonreport.sheetobj.get_element()).find('.k-spreadsheet-cell a')) {
                        $telerik.$(o3).addClass('rotate-text');
                    }
                }
            }

            function CellContextMenuItemClicked(sender, args) {
                if (args.get_item().get_value() == "cmdAmend") {
                    var spreadsheet = spreadsheets.filter(s => s.id.toLowerCase() == sender._element.parentNode.id.toLowerCase())[0];
                    var sheet = spreadsheet.sheetobj.get_activeSheet();
                    currentsheet = sheet._sheet._sheetName;
                    amend();
                }              
            }
            function amend() {
                //var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                for (var spreadsheet of spreadsheets) {
                    if (spreadsheet) {
                        var sheet = spreadsheet.sheetobj.get_activeSheet();
                        var sheetname = sheet._sheet._sheetName;
                        if (currentsheet == sheetname) {
                            var value = sheet.get_selection().get_value();
                            var cellref = sheet.get_selection()._range._sheet._viewSelection.originalActiveCell;
                            var cellrow = sheet.get_selection()._range._sheet._viewSelection.originalActiveCell.row;
                            var cellcol = sheet.get_selection()._range._sheet._viewSelection.originalActiveCell.col;
                            var cellAddress = cellref.toString().replace(/\d+/g, '');
                            var complete = 0;
                            var v = validationState.filter(v => v.logname == sheetname)[0];
                            if (v) {
                                if (v.validation && v.dutyengineerSigned && v.cheifengineerSigned && v.isLogCompleted) {
                                    complete = 1;
                                } 
                            }
                            var islock = document.getElementById('hdnislock').value;

                            if (complete != 1 || allowentry == "0" || islock == "0")
                                return;

                            var watchAddress = getLogWatchAddress();
                            var logAddress = watchAddress.filter(w => w.logname.toLowerCase() == sheetname.toLowerCase())
                            var currentWatch = null;
                            var watchno = "";
                            logAddress.forEach(watch => {
                                if (watch.address.join().indexOf(cellAddress) > -1) {
                                    currentWatch = watch;
                                    watchno = 'watch'+watch.watchref
                                }
                            });
                            
                            var rangeadd ;
                            var range ;
                            var parameter;
                            var param = '';
                        
                            if (sheetname == 'misc' || sheetname == 'aircond')
                            {
                                
                                if (cellcol > 9) {
                                    
                                    rangeadd = currentWatch.tbl2rowhead.toString() + (cellrow + 1).toString();
                                    range = sheet.get_range(rangeadd);
                                    parameter = range.get_value();

                                    if (parameter == null) {
                                        var rangeaddrecal = currentWatch.tbl2rowhead.toString() + (cellrow).toString();
                                        var rangerecal = sheet.get_range(rangeaddrecal);
                                        var parameter = rangerecal.get_value();
                                    }
                                    if (parameter == null) {
                                        var rangeaddrecal = currentWatch.tbl2rowhead.toString() + (cellrow-1).toString();
                                        var rangerecal = sheet.get_range(rangeaddrecal);
                                        var parameter = rangerecal.get_value();
                                    }

                                    var rangeadd = currentWatch.tbl2rowsubhead + (cellrow + 1).toString();
                                    var range = sheet.get_range(rangeadd);
                                    if (range)
                                        param = range.get_value();
                                }else
                                {
                                    rangeadd = currentWatch.rowhead.toString() + (cellrow + 1).toString();
                                    range = sheet.get_range(rangeadd);
                                    parameter = range.get_value();
                                    
                                    if (parameter == null) {
                                        var rangeaddrecal = currentWatch.rowhead.toString() + (cellrow).toString();
                                        var rangerecal = sheet.get_range(rangeaddrecal);
                                        var parameter = rangerecal.get_value();
                                    }

                                    var rangeadd = currentWatch.rowsubhead + (cellrow + 1).toString();
                                    var range = sheet.get_range(rangeadd);
                                    if (range)
                                        param = range.get_value();
                                }
                            }
                            else
                            {
                                rangeadd = currentWatch.rowhead.toString() + (cellrow + 1).toString();
                                range = sheet.get_range(rangeadd);
                                parameter = range.get_value();
                            }
                            if (sheetname == 'metemp' || sheetname == 'metemp2')
                            {
                                var metemprangeadd = cellAddress + '4';
                                var metemprange = sheet.get_range(metemprangeadd);
                                if (metemprange)
                                    param = metemprange.get_value();
                            }
                            if (sheetname == 'turbocharger' || sheetname == 'turbocharger2' || sheetname == 'generator') {
                                var rangeadd = currentWatch.rowsubhead + (cellrow + 1).toString();
                                var range = sheet.get_range(rangeadd);
                                if (range)
                                    param = range.get_value();
                                
                                if(parameter==null)
                                {
                                    var rangeaddrecal = currentWatch.rowhead.toString() + (cellrow).toString();
                                    var rangerecal = sheet.get_range(rangeaddrecal);
                                    var parameter = rangerecal.get_value();
                                }
                            }
                            if (param != null)
                                var parameter = parameter + ' ' + param;



                            var selectedDate = $find("<%= txtdate.ClientID %>");
                            var seldate = selectedDate.get_selectedDate();
                            var date = `${seldate.getFullYear()}-${seldate.getMonth() + 1}-${seldate.getDate()} ${seldate.getHours()}:${seldate.getMinutes()}:${seldate.getSeconds()} `;


                            //if (value) {
                                javascript: parent.openNewWindow('EngineLogAmend', '', 'Log/ElectricLogEngineLogAmendment.aspx?value=' + value + '&parameter=' + parameter + '&watch=' + currentWatch.watch + '&date=' + date + '&address=' + cellref + '&sheetname=' + sheetname + '&watchno=' + watchno + '&row=' + cellrow +'&col='+cellcol, null, '345', '300', null, null, { 'disableMinMax': true });
                            //}
                            //else {
                            //    javascript: parent.openNewWindow('EngineLogAmend', '', 'Log/ElectricLogEngineLogAmendment.aspx', null, '345', '300', null, null, { 'disableMinMax': true });
                            //}
                        }
                    }
                }
            }

            function Amended(address, value, sheet) {

                setTimeout(() => {
                    if (spreadsheets) {
                        var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == sheet);
                        var sheetobj = spreadsheet[0].sheetobj.get_activeSheet();
                        setRangeValue(sheetobj, address, value);
                        var rangeobj = sheetobj.get_range(address);
                        spreadsheet[0].sheetobj.save();
                        rangeobj.set_color('red');
                        
                    }
                }, 7000);
            }

            function showHideTurbo(sheetName) {
               // var sheetName = "turbocharger"; // from configuration
                var sheet = spreadsheets.find(x => x.sheetName == sheetName);
                var activeSheet = sheet.sheetobj.get_activeSheet();
                var sheetConfig = unitConfiguration.find(x => x.field == 'TUME');
                if (sheetConfig == null || sheetConfig == undefined)
                    return;
                var rowIndex = 3; // need to store the value 
                var endIndex = 27;
                var set = 6;
                
                var startIndex = rowIndex + (parseInt(sheetConfig.Value) * set);
                for (startIndex; startIndex < endIndex + 1; startIndex++) {
                    activeSheet.hideColumn(startIndex);
                }
            }

            function showHideGenerator() {
                 sheetName = "generator"; // from configuration
                var sheet = spreadsheets.find(x => x.sheetName == sheetName);
                var activeSheet = sheet.sheetobj.get_activeSheet();
                
                var sheetConfig = unitConfiguration.find(x => x.field == 'AUEN');
                if (sheetConfig == null || sheetConfig == undefined)
                    return;
                var rowIndex = 3; // need to store the value 
                var endIndex = 27;
                var set = 6;
                var startIndex = rowIndex + (parseInt(sheetConfig.Value) * set);
               
                for (startIndex; startIndex < endIndex + 1; startIndex++) {
                    activeSheet.hideColumn(startIndex);
                }
            }




            function showHideRow(sheetName) {
                //var sheet = spreadsheets[1].sheetobj.get_activeSheet();
                //console.log(spreadsheets[1]);
                //sheet.hideRow(11);

                //var sheetName = "metemp"; // from configuration
                var sheet = spreadsheets.find(x => x.sheetName == sheetName);
                var activeSheet = sheet.sheetobj.get_activeSheet();
                var unitcount = '16';
                
                
                var sheetConfig = unitConfiguration.find(x => x.field == 'UPME');
                if (sheetConfig != null || sheetConfig != undefined) {
                    unitcount = sheetConfig.Value;
                }
               
                var rowIndex = 5; // need to store the value 
                var endIndex = 21;
                var startIndex = rowIndex + parseInt(unitcount);
                for (startIndex; startIndex < endIndex + 1; startIndex++) {
                    
                    activeSheet.hideRow(startIndex);
                }
            }

            function saveSchedular(spreadsheets) {
                intervalState['saveIntervalId'] = setInterval(() => { //every 2 mintues it will save the value
                    save(spreadsheets);
                }, 120000);
            }

            function save(spreadsheets) {
                for (var spreadsheet of spreadsheets) {
                        if (spreadsheet) {
                            var sheetObj = spreadsheet.sheetobj
                            sheetObj.save();
                        }
                    }
            }

            function saveSheetByName(spreadsheets, sheetname) {
                if (spreadsheets && sheetname) {
                    var spreadsheet = spreadsheets.find(sheet => sheet.sheetName.toLowerCase() == sheetname.toLowerCase());
                    var sheetObj = spreadsheet.sheetobj;
                    sheetObj.save();
                }
            }

            function instantSave() {
                for (var spreadsheet of spreadsheets) {
                    if (spreadsheet) {
                        var sheetObj = spreadsheet.sheetobj
                        sheetObj.save();
                    }
                }
            }

            function getLogWatchAddress() {
                var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'meparam');
                var sheet = spreadsheet[0].sheetobj.get_activeSheet();
                var watch1 = sheet.get_range('C3').get_value() == '' || sheet.get_range('C3').get_value() == null || sheet.get_range('C3').get_value() == undefined ? '-' : sheet.get_range('C3').get_value();
                var watch2 = sheet.get_range('D3').get_value() == '' || sheet.get_range('D3').get_value() == null || sheet.get_range('D3').get_value() == undefined ? '-' : sheet.get_range('D3').get_value();
                var watch3 = sheet.get_range('E3').get_value() == '' || sheet.get_range('E3').get_value() == null || sheet.get_range('E3').get_value() == undefined ? '-' : sheet.get_range('E3').get_value();
                var watch4 = sheet.get_range('F3').get_value() == '' || sheet.get_range('F3').get_value() == null || sheet.get_range('F3').get_value() == undefined ? '-' : sheet.get_range('F3').get_value();
                var watch5 = sheet.get_range('G3').get_value() == '' || sheet.get_range('G3').get_value() == null || sheet.get_range('G3').get_value() == undefined ? '-' : sheet.get_range('G3').get_value();
                var watch6 = sheet.get_range('H3').get_value() == '' || sheet.get_range('H3').get_value() == null || sheet.get_range('H3').get_value() == undefined ? '-' : sheet.get_range('H3').get_value();



                var config41 = ['D4', 'D6:D12', 'D14', 'D16:D18', 'J4', 'J6:J12', 'J14', 'J16:J18', 'P4', 'P6:P12', 'P14', 'P16:P18', 'V4', 'V6:V12', 'V14', 'V16:V18'];
                var config42 = ['E4', 'E6:E12', 'E14', 'E16:E18', 'K4', 'K6:K12', 'K14', 'K16:K18', 'Q4', 'Q6:Q12', 'Q14', 'Q16:Q18', 'W4', 'W6:W12', 'W14', 'W16:W18'];
                var config43 = ['F4', 'F6:F12', 'F14', 'F16:F18', 'L4', 'L6:L12', 'L14', 'L16:L18', 'R4', 'R6:R12', 'R14', 'R16:R18', 'X4', 'X6:X12', 'X14', 'X16:X18'];
                var config44 = ['G4', 'G6:G12', 'G14', 'G16:G18', 'M4', 'M6:M12', 'M14', 'M16:M18', 'S4', 'S6:S12', 'S14', 'S16:S18', 'Y4', 'Y6:Y12', 'Y14', 'Y16:Y18'];
                var config45 = ['H4', 'H6:H12', 'H14', 'H16:H18', 'N4', 'N6:N12', 'N14', 'N16:N18', 'T4', 'T6:T12', 'T14', 'T16:T18', 'Z4', 'Z6:Z12', 'Z14', 'Z16:Z18'];
                var config46 = ['I4', 'I6:I12', 'I14', 'I16:I18', 'O4', 'O6:O12', 'O14', 'O16:O18', 'U4', 'U6:U12', 'U14', 'U16:U18', 'AA4', 'AA6:AA12', 'AA14', 'AA16:AA18'];

                var config31 = ['D4', 'D6:D12', 'D14', 'D16:D18', 'J4', 'J6:J12', 'J14', 'J16:J18', 'P4', 'P6:P12', 'P14', 'P16:P18'];
                var config32 = ['E4', 'E6:E12', 'E14', 'E16:E18', 'K4', 'K6:K12', 'K14', 'K16:K18', 'Q4', 'Q6:Q12', 'Q14', 'Q16:Q18'];
                var config33 = ['F4', 'F6:F12', 'F14', 'F16:F18', 'L4', 'L6:L12', 'L14', 'L16:L18', 'R4', 'R6:R12', 'R14', 'R16:R18'];
                var config34 = ['G4', 'G6:G12', 'G14', 'G16:G18', 'M4', 'M6:M12', 'M14', 'M16:M18', 'S4', 'S6:S12', 'S14', 'S16:S18'];
                var config35 = ['H4', 'H6:H12', 'H14', 'H16:H18', 'N4', 'N6:N12', 'N14', 'N16:N18', 'T4', 'T6:T12', 'T14', 'T16:T18'];
                var config36 = ['I4', 'I6:I12', 'I14', 'I16:I18', 'O4', 'O6:O12', 'O14', 'O16:O18', 'U4', 'U6:U12', 'U14', 'U16:U18'];

                var config21 = ['D4', 'D6:D12', 'D14', 'D16:D18', 'J4', 'J6:J12', 'J14', 'J16:J18'];
                var config22 = ['E4', 'E6:E12', 'E14', 'E16:E18', 'K4', 'K6:K12', 'K14', 'K16:K18'];
                var config23 = ['F4', 'F6:F12', 'F14', 'F16:F18', 'L4', 'L6:L12', 'L14', 'L16:L18'];
                var config24 = ['G4', 'G6:G12', 'G14', 'G16:G18', 'M4', 'M6:M12', 'M14', 'M16:M18'];
                var config25 = ['H4', 'H6:H12', 'H14', 'H16:H18', 'N4', 'N6:N12', 'N14', 'N16:N18'];
                var config26 = ['I4', 'I6:I12', 'I14', 'I16:I18', 'O4', 'O6:O12', 'O14', 'O16:O18'];

                var config11 = ['D4', 'D6:D12', 'D14', 'D16:D18'];
                var config12 = ['E4', 'E6:E12', 'E14', 'E16:E18'];
                var config13 = ['F4', 'F6:F12', 'F14', 'F16:F18'];
                var config14 = ['G4', 'G6:G12', 'G14', 'G16:G18'];
                var config15 = ['H4', 'H6:H12', 'H14', 'H16:H18'];
                var config16 = ['I4', 'I6:I12', 'I14', 'I16:I18'];

                var sheetConfig = unitConfiguration.find(x => x.field == 'TUME');
                var unitno = '4';
                if (sheetConfig != null && sheetConfig != undefined)
                    unitno = sheetConfig.Value;
                var address1 = unitno == '4' ? config41 : (unitno == '3' ? config31 : (unitno == '2' ? config21 : (unitno == '1' ? config11 : config41)))
                var address2 = unitno == '4' ? config42 : (unitno == '3' ? config32 : (unitno == '2' ? config22 : (unitno == '1' ? config12 : config42)))
                var address3 = unitno == '4' ? config43 : (unitno == '3' ? config33 : (unitno == '2' ? config23 : (unitno == '1' ? config13 : config43)))
                var address4 = unitno == '4' ? config44 : (unitno == '3' ? config34 : (unitno == '2' ? config24 : (unitno == '1' ? config14 : config44)))
                var address5 = unitno == '4' ? config45 : (unitno == '3' ? config35 : (unitno == '2' ? config25 : (unitno == '1' ? config15 : config45)))
                var address6 = unitno == '4' ? config46 : (unitno == '3' ? config36 : (unitno == '2' ? config26 : (unitno == '1' ? config16 : config46)))

                var configgen41 = ['D4', 'D6:D13', 'D15:D18', 'D20:D22', 'J4', 'J6:J13', 'J15:J18', 'J20:J22', 'P4', 'P6:P13', 'P15:P18', 'P20:P22', 'V4', 'V6:V13', 'V15:V18', 'V20:V22'];
                var configgen42 = ['E4', 'E6:E13', 'E15:E18', 'E20:E22', 'K4', 'K6:K13', 'K15:K18', 'K20:K22', 'Q4', 'Q6:Q13', 'Q15:Q18', 'Q20:Q22', 'W4', 'W6:W13', 'W15:W18', 'W20:W22'];
                var configgen43 = ['F4', 'F6:F13', 'F15:F18', 'F20:F22', 'L4', 'L6:L13', 'L15:L18', 'L20:L22', 'R4', 'R6:R13', 'R15:R18', 'R20:R22', 'X4', 'X6:X13', 'X15:X18', 'X20:X22'];
                var configgen44 = ['G4', 'G6:G13', 'G15:G18', 'G20:G22', 'M4', 'M6:M13', 'M15:M18', 'M20:M22', 'S4', 'S6:S13', 'S15:S18', 'S20:S22', 'Y4', 'Y6:Y13', 'Y15:Y18', 'Y20:Y22'];
                var configgen45 = ['H4', 'H6:H13', 'H15:H18', 'H20:H22', 'N4', 'N6:N13', 'N15:N18', 'N20:N22', 'T4', 'T6:T13', 'T15:T18', 'T20:T22', 'Z4', 'Z6:Z13', 'Z15:Z18', 'Z20:Z22'];
                var configgen46 = ['I4', 'I6:I13', 'I15:I18', 'I20:I22', 'O4', 'O6:O13', 'O15:O18', 'O20:O22', 'U4', 'U6:U13', 'U15:U18', 'U20:U22', 'AA4', 'AA6:AA13', 'AA15:AA18', 'AA20:AA22'];

                var configgen31 = ['D4', 'D6:D13', 'D15:D18', 'D20:D22', 'J4', 'J6:J13', 'J15:J18', 'J20:J22', 'P4', 'P6:P13', 'P15:P18', 'P20:P22'];
                var configgen32 = ['E4', 'E6:E13', 'E15:E18', 'E20:E22', 'K4', 'K6:K13', 'K15:K18', 'K20:K22', 'Q4', 'Q6:Q13', 'Q15:Q18', 'Q20:Q22'];
                var configgen33 = ['F4', 'F6:F13', 'F15:F18', 'F20:F22', 'L4', 'L6:L13', 'L15:L18', 'L20:L22', 'R4', 'R6:R13', 'R15:R18', 'R20:R22'];
                var configgen34 = ['G4', 'G6:G13', 'G15:G18', 'G20:G22', 'M4', 'M6:M13', 'M15:M18', 'M20:M22', 'S4', 'S6:S13', 'S15:S18', 'S20:S22'];
                var configgen35 = ['H4', 'H6:H13', 'H15:H18', 'H20:H22', 'N4', 'N6:N13', 'N15:N18', 'N20:N22', 'T4', 'T6:T13', 'T15:T18', 'T20:T22'];
                var configgen36 = ['I4', 'I6:I13', 'I15:I18', 'I20:I22', 'O4', 'O6:O13', 'O15:O18', 'O20:O22', 'U4', 'U6:U13', 'U15:U18', 'U20:U22'];

                var configgen21 = ['D4', 'D6:D13', 'D15:D18', 'D20:D22', 'J4', 'J6:J13', 'J15:J18', 'J20:J22'];
                var configgen22 = ['E4', 'E6:E13', 'E15:E18', 'E20:E22', 'K4', 'K6:K13', 'K15:K18', 'K20:K22'];
                var configgen23 = ['F4', 'F6:F13', 'F15:F18', 'F20:F22', 'L4', 'L6:L13', 'L15:L18', 'L20:L22'];
                var configgen24 = ['G4', 'G6:G13', 'G15:G18', 'G20:G22', 'M4', 'M6:M13', 'M15:M18', 'M20:M22'];
                var configgen25 = ['H4', 'H6:H13', 'H15:H18', 'H20:H22', 'N4', 'N6:N13', 'N15:N18', 'N20:N22'];
                var configgen26 = ['I4', 'I6:I13', 'I15:I18', 'I20:I22', 'O4', 'O6:O13', 'O15:O18', 'O20:O22'];

                var configgen11 = ['D4', 'D6:D13', 'D15:D18', 'D20:D22'];
                var configgen12 = ['E4', 'E6:E13', 'E15:E18', 'E20:E22'];
                var configgen13 = ['F4', 'F6:F13', 'F15:F18', 'F20:F22'];
                var configgen14 = ['G4', 'G6:G13', 'G15:G18', 'G20:G22'];
                var configgen15 = ['H4', 'H6:H13', 'H15:H18', 'H20:H22'];
                var configgen16 = ['I4', 'I6:I13', 'I15:I18', 'I20:I22'];

                var sheetGenConfig = unitConfiguration.find(x => x.field == 'AUEN');
                var unitgenno = '4';
                if (sheetGenConfig != null && sheetGenConfig != undefined)
                    unitgenno = sheetGenConfig.Value;

                var addressgen1 = unitgenno == '4' ? configgen41 : (unitgenno == '3' ? configgen31 : (unitgenno == '2' ? configgen21 : (unitgenno == '1' ? configgen11 : configgen41)))
                var addressgen2 = unitgenno == '4' ? configgen42 : (unitgenno == '3' ? configgen32 : (unitgenno == '2' ? configgen22 : (unitgenno == '1' ? configgen12 : configgen42)))
                var addressgen3 = unitgenno == '4' ? configgen43 : (unitgenno == '3' ? configgen33 : (unitgenno == '2' ? configgen23 : (unitgenno == '1' ? configgen13 : configgen43)))
                var addressgen4 = unitgenno == '4' ? configgen44 : (unitgenno == '3' ? configgen34 : (unitgenno == '2' ? configgen24 : (unitgenno == '1' ? configgen14 : configgen44)))
                var addressgen5 = unitgenno == '4' ? configgen45 : (unitgenno == '3' ? configgen35 : (unitgenno == '2' ? configgen25 : (unitgenno == '1' ? configgen15 : configgen45)))
                var addressgen6 = unitgenno == '4' ? configgen46 : (unitgenno == '3' ? configgen36 : (unitgenno == '2' ? configgen26 : (unitgenno == '1' ? configgen16 : configgen46)))

                var sheettEMPConfig = unitConfiguration.find(x => x.field == 'UPME');
                var unittemno = '16';
                if (sheettEMPConfig != null && sheettEMPConfig != undefined)
                    unittemno = sheettEMPConfig.Value;
                var addresstemp1 = ['D5', 'E5', 'C6:C' + (parseInt(unittemno) + 5).toString(), 'D6:D' + (parseInt(unittemno) + 5).toString(), 'E6:E' + (parseInt(unittemno) + 5).toString()];
                var addresstemp2 = ['G5', 'H5', 'F6:F' + (parseInt(unittemno) + 5).toString(), 'G6:G' + (parseInt(unittemno) + 5).toString(), 'H6:H' + (parseInt(unittemno) + 5).toString()];
                var addresstemp3 = ['J5', 'K5', 'I6:I' + (parseInt(unittemno) + 5).toString(), 'J6:J' + (parseInt(unittemno) + 5).toString(), 'K6:K' + (parseInt(unittemno) + 5).toString()];
                var addresstemp4 = ['M5', 'N5', 'L6:L' + (parseInt(unittemno) + 5).toString(), 'M6:M' + (parseInt(unittemno) + 5).toString(), 'N6:N' + (parseInt(unittemno) + 5).toString()];
                var addresstemp5 = ['P5', 'Q5', 'O6:O' + (parseInt(unittemno) + 5).toString(), 'P6:P' + (parseInt(unittemno) + 5).toString(), 'Q6:Q' + (parseInt(unittemno) + 5).toString()];
                var addresstemp6 = ['S5', 'T5', 'R6:R' + (parseInt(unittemno) + 5).toString(), 'S6:S' + (parseInt(unittemno) + 5).toString(), 'T6:T' + (parseInt(unittemno) + 5).toString()];

                var fopurifier = 0;
                var dopurifier = 0;
                var lopurifier = 0;

                var sheetfopurifier = unitConfiguration.find(x => x.field == 'FOPU');
                if (sheetfopurifier != null && sheetfopurifier != undefined) {
                    fopurifier = sheetfopurifier.Value;
                }

                var sheetdopurifier = unitConfiguration.find(x => x.field == 'DOPU');
                if (sheetdopurifier != null && sheetdopurifier != undefined) {
                    dopurifier = sheetdopurifier.Value;
                }
                var sheetlopurifier = unitConfiguration.find(x => x.field == 'LOPU');
                if (sheetlopurifier != null && sheetlopurifier != undefined) {
                    lopurifier = sheetlopurifier.Value;
                }
                var watchAddress = [
                    { 'logname': 'meparam', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3'], 'address': ['C5:C6', 'C8:C9', 'C11:C12', 'C14:C15', 'C17:C19'], 'rowhead': 'B' },
                    { 'logname': 'meparam', 'watch': watch2, 'watchref': '2', 'watchadd': ['D3'], 'address': ['D5:D6', 'D8:D9', 'D11:D12', 'D14:D15', 'D17:D19'], 'rowhead': 'B' },
                    { 'logname': 'meparam', 'watch': watch3, 'watchref': '3', 'watchadd': ['E3'], 'address': ['E5:E6', 'E8:E9', 'E11:E12', 'E14:E15', 'E17:E19'], 'rowhead': 'B' },
                    { 'logname': 'meparam', 'watch': watch4, 'watchref': '4', 'watchadd': ['F3'], 'address': ['F5:F6', 'F8:F9', 'F11:F12', 'F14:F15', 'F17:F19'], 'rowhead': 'B' },
                    { 'logname': 'meparam', 'watch': watch5, 'watchref': '5', 'watchadd': ['G3'], 'address': ['G5:G6', 'G8:G9', 'G11:G12', 'G14:G15', 'G17:G19'], 'rowhead': 'B' },
                    { 'logname': 'meparam', 'watch': watch6, 'watchref': '6', 'watchadd': ['H3'], 'address': ['H5:H6', 'H8:H9', 'H11:H12', 'H14:H15', 'H17:H19'], 'rowhead': 'B' },

                    //{ 'logname': 'metemp', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3'], 'address': ['D5:E5', 'C6:E12'], 'rowhead': 'B' },
                    //{ 'logname': 'metemp', 'watch': watch2, 'watchref': '2', 'watchadd': ['F3'], 'address': ['G5:H5', 'F6:H12'], 'rowhead': 'B' },
                    //{ 'logname': 'metemp', 'watch': watch3, 'watchref': '3', 'watchadd': ['I3'], 'address': ['J5:K5', 'I6:K12'], 'rowhead': 'B' },
                    //{ 'logname': 'metemp', 'watch': watch4, 'watchref': '4', 'watchadd': ['L3'], 'address': ['M5:N5', 'L6:N12'], 'rowhead': 'B' },
                    //{ 'logname': 'metemp', 'watch': watch5, 'watchref': '5', 'watchadd': ['O3'], 'address': ['P5:Q5', 'O6:Q12'], 'rowhead': 'B' },
                    //{ 'logname': 'metemp', 'watch': watch6, 'watchref': '6', 'watchadd': ['R3'], 'address': ['S5:T5', 'R6:T12'], 'rowhead': 'B' },

                    { 'logname': 'metemp', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3'], 'address': addresstemp1, 'rowhead': 'B' },
                    { 'logname': 'metemp', 'watch': watch2, 'watchref': '2', 'watchadd': ['F3'], 'address': addresstemp2, 'rowhead': 'B' },
                    { 'logname': 'metemp', 'watch': watch3, 'watchref': '3', 'watchadd': ['I3'], 'address': addresstemp3, 'rowhead': 'B' },
                    { 'logname': 'metemp', 'watch': watch4, 'watchref': '4', 'watchadd': ['L3'], 'address': addresstemp4, 'rowhead': 'B' },
                    { 'logname': 'metemp', 'watch': watch5, 'watchref': '5', 'watchadd': ['O3'], 'address': addresstemp5, 'rowhead': 'B' },
                    { 'logname': 'metemp', 'watch': watch6, 'watchref': '6', 'watchadd': ['R3'], 'address': addresstemp6, 'rowhead': 'B' },

                    //{ 'logname': 'turbocharger', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'J3', 'P3', 'V3'], 'address': ['D4', 'D6:D12', 'D14', 'D16:D18', 'J4', 'J6:J12', 'J14', 'J16:J18', 'P4', 'P6:P12', 'P14', 'P16:P18', 'V4', 'V6:V12', 'V14', 'V16:V18'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'turbocharger', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'K3', 'Q3', 'W3'], 'address': ['E4', 'E6:E12', 'E14', 'E16:E18', 'K4', 'K6:K12', 'K14', 'K16:K18', 'Q4', 'Q6:Q12', 'Q14', 'Q16:Q18', 'W4', 'W6:W12', 'W14', 'W16:W18'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'turbocharger', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'L3', 'R3', 'X3'], 'address': ['F4', 'F6:F12', 'F14', 'F16:F18', 'L4', 'L6:L12', 'L14', 'L16:L18', 'R4', 'R6:R12', 'R14', 'R16:R18', 'X4', 'X6:X12', 'X14', 'X16:X18'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'turbocharger', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'M3', 'S3', 'Y3'], 'address': ['G4', 'G6:G12', 'G14', 'G16:G18', 'M4', 'M6:M12', 'M14', 'M16:M18', 'S4', 'S6:S12', 'S14', 'S16:S18', 'Y4', 'Y6:Y12', 'Y14', 'Y16:Y18'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'turbocharger', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'N3', 'T3', 'Z3'], 'address': ['H4', 'H6:H12', 'H14', 'H16:H18', 'N4', 'N6:N12', 'N14', 'N16:N18', 'T4', 'T6:T12', 'T14', 'T16:T18', 'Z4', 'Z6:Z12', 'Z14', 'Z16:Z18'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'turbocharger', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'O3', 'U3', 'AA3'], 'address': ['I4', 'I6:I12', 'I14', 'I16:I18', 'O4', 'O6:O12', 'O14', 'O16:O18', 'U4', 'U6:U12', 'U14', 'U16:U18', 'AA4', 'AA6:AA12', 'AA14', 'AA16:AA18'], 'rowhead': 'B', 'rowsubhead': 'C' },

                    { 'logname': 'turbocharger', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'J3', 'P3', 'V3'], 'address': address1, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'K3', 'Q3', 'W3'], 'address': address2, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'L3', 'R3', 'X3'], 'address': address3, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'M3', 'S3', 'Y3'], 'address': address4, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'N3', 'T3', 'Z3'], 'address': address5, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'O3', 'U3', 'AA3'], 'address': address6, 'rowhead': 'B', 'rowsubhead': 'C' },


                    { 'logname': 'meparam2', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3'], 'address': ['C5:C6', 'C8:C9', 'C11:C12', 'C14:C15', 'C17:C19'], 'rowhead': 'B' },
                    { 'logname': 'meparam2', 'watch': watch2, 'watchref': '2', 'watchadd': ['D3'], 'address': ['D5:D6', 'D8:D9', 'D11:D12', 'D14:D15', 'D17:D19'], 'rowhead': 'B' },
                    { 'logname': 'meparam2', 'watch': watch3, 'watchref': '3', 'watchadd': ['E3'], 'address': ['E5:E6', 'E8:E9', 'E11:E12', 'E14:E15', 'E17:E19'], 'rowhead': 'B' },
                    { 'logname': 'meparam2', 'watch': watch4, 'watchref': '4', 'watchadd': ['F3'], 'address': ['F5:F6', 'F8:F9', 'F11:F12', 'F14:F15', 'F17:F19'], 'rowhead': 'B' },
                    { 'logname': 'meparam2', 'watch': watch5, 'watchref': '5', 'watchadd': ['G3'], 'address': ['G5:G6', 'G8:G9', 'G11:G12', 'G14:G15', 'G17:G19'], 'rowhead': 'B' },
                    { 'logname': 'meparam2', 'watch': watch6, 'watchref': '6', 'watchadd': ['H3'], 'address': ['H5:H6', 'H8:H9', 'H11:H12', 'H14:H15', 'H17:H19'], 'rowhead': 'B' },

                    { 'logname': 'metemp2', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3'], 'address': addresstemp1, 'rowhead': 'B' },
                    { 'logname': 'metemp2', 'watch': watch2, 'watchref': '2', 'watchadd': ['F3'], 'address': addresstemp2, 'rowhead': 'B' },
                    { 'logname': 'metemp2', 'watch': watch3, 'watchref': '3', 'watchadd': ['I3'], 'address': addresstemp3, 'rowhead': 'B' },
                    { 'logname': 'metemp2', 'watch': watch4, 'watchref': '4', 'watchadd': ['L3'], 'address': addresstemp4, 'rowhead': 'B' },
                    { 'logname': 'metemp2', 'watch': watch5, 'watchref': '5', 'watchadd': ['O3'], 'address': addresstemp5, 'rowhead': 'B' },
                    { 'logname': 'metemp2', 'watch': watch6, 'watchref': '6', 'watchadd': ['R3'], 'address': addresstemp6, 'rowhead': 'B' },

                    { 'logname': 'turbocharger2', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'J3', 'P3', 'V3'], 'address': address1, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger2', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'K3', 'Q3', 'W3'], 'address': address2, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger2', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'L3', 'R3', 'X3'], 'address': address3, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger2', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'M3', 'S3', 'Y3'], 'address': address4, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger2', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'N3', 'T3', 'Z3'], 'address': address5, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'turbocharger2', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'O3', 'U3', 'AA3'], 'address': address6, 'rowhead': 'B', 'rowsubhead': 'C' },


                    //{ 'logname': 'generator', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'J3', 'P3', 'V3'], 'address': ['D4', 'D6:D13', 'D15:D18', 'D20:D22', 'J4', 'J6:J13', 'J15:J18', 'J20:J22', 'P4', 'P6:P13', 'P15:P18', 'P20:P22', 'V4', 'V6:V13', 'V15:V18', 'V20:V22'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'generator', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'K3', 'Q3', 'W3'], 'address': ['E4', 'E6:E13', 'E15:E18', 'E20:E22', 'K4', 'K6:K13', 'K15:K18', 'K20:K22', 'Q4', 'Q6:Q13', 'Q15:Q18', 'Q20:Q22', 'W4', 'W6:W13', 'W15:W18', 'W20:W22'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'generator', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'L3', 'R3', 'X3'], 'address': ['F4', 'F6:F13', 'F15:F18', 'F20:F22', 'L4', 'L6:L13', 'L15:L18', 'L20:L22', 'R4', 'R6:R13', 'R15:R18', 'R20:R22', 'X4', 'X6:X13', 'X15:X18', 'X20:X22'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'generator', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'M3', 'S3', 'Y3'], 'address': ['G4', 'G6:G13', 'G15:G18', 'G20:G22', 'M4', 'M6:M13', 'M15:M18', 'M20:M22', 'S4', 'S6:S13', 'S15:S18', 'S20:S22', 'Y4', 'Y6:Y13', 'Y15:Y18', 'Y20:Y22'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'generator', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'N3', 'T3', 'Z3'], 'address': ['H4', 'H6:H13', 'H15:H18', 'H20:H22', 'N4', 'N6:N13', 'N15:N18', 'N20:N22', 'T4', 'T6:T13', 'T15:T18', 'T20:T22', 'Z4', 'Z6:Z13', 'Z15:Z18', 'Z20:Z22'], 'rowhead': 'B', 'rowsubhead': 'C' },
                    //{ 'logname': 'generator', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'O3', 'U3', 'AA3'], 'address': ['I4', 'I6:I13', 'I15:I18', 'I20:I22', 'O4', 'O6:O13', 'O15:O18', 'O20:O22', 'U4', 'U6:U13', 'U15:U18', 'U20:U22', 'AA4', 'AA6:AA13', 'AA15:AA18', 'AA20:AA22'], 'rowhead': 'B', 'rowsubhead': 'C' },

                    { 'logname': 'generator', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'J3', 'P3', 'V3'], 'address': addressgen1, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'generator', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'K3', 'Q3', 'W3'], 'address': addressgen2, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'generator', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'L3', 'R3', 'X3'], 'address': addressgen3, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'generator', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'M3', 'S3', 'Y3'], 'address': addressgen4, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'generator', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'N3', 'T3', 'Z3'], 'address': addressgen5, 'rowhead': 'B', 'rowsubhead': 'C' },
                    { 'logname': 'generator', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'O3', 'U3', 'AA3'], 'address': addressgen6, 'rowhead': 'B', 'rowsubhead': 'C' },

                    { 'logname': 'misc', 'watch': watch1, 'watchref': '1', 'watchadd': ['C3', 'M3'], 'address': ['C5:C10', 'C12:C13', 'C15:C16', fopurifier == 1 ? 'C18' : (fopurifier == 2 ? 'C18:C19' : 'C15'), dopurifier == 1 ? 'C20' : (dopurifier == 2 ? 'C20:C21' : 'C15'), lopurifier == 1 ? 'C22' : (lopurifier == 2 ? 'C22:C23' : 'C15'), 'M5:M21', 'M23:M33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },
                    { 'logname': 'misc', 'watch': watch2, 'watchref': '2', 'watchadd': ['D3', 'N3'], 'address': ['D5:D10', 'D12:D13', 'D15:D16', fopurifier == 1 ? 'D18' : (fopurifier == 2 ? 'D18:D19' : 'D15'), dopurifier == 1 ? 'D20' : (dopurifier == 2 ? 'D20:D21' : 'D15'), lopurifier == 1 ? 'D22' : (lopurifier == 2 ? 'D22:D23' : 'D15'), 'N5:N21', 'N23:N33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },
                    { 'logname': 'misc', 'watch': watch3, 'watchref': '3', 'watchadd': ['E3', 'O3'], 'address': ['E5:E10', 'E12:E13', 'E15:E16', fopurifier == 1 ? 'E18' : (fopurifier == 2 ? 'E18:E19' : 'E15'), dopurifier == 1 ? 'E20' : (dopurifier == 2 ? 'E20:E21' : 'E15'), lopurifier == 1 ? 'E22' : (lopurifier == 2 ? 'E22:E23' : 'E15'), 'O5:O21', 'O23:O33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },
                    { 'logname': 'misc', 'watch': watch4, 'watchref': '4', 'watchadd': ['F3', 'P3'], 'address': ['F5:F10', 'F12:F13', 'F15:F16', fopurifier == 1 ? 'F18' : (fopurifier == 2 ? 'F18:F19' : 'F15'), dopurifier == 1 ? 'F20' : (dopurifier == 2 ? 'F20:F21' : 'F15'), lopurifier == 1 ? 'F22' : (lopurifier == 2 ? 'F22:F23' : 'F15'), 'P5:P21', 'P23:P33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },
                    { 'logname': 'misc', 'watch': watch5, 'watchref': '5', 'watchadd': ['G3', 'Q3'], 'address': ['G5:G10', 'G12:G13', 'G15:G16', fopurifier == 1 ? 'G18' : (fopurifier == 2 ? 'G18:G19' : 'G15'), dopurifier == 1 ? 'G20' : (dopurifier == 2 ? 'G20:G21' : 'G15'), lopurifier == 1 ? 'G22' : (lopurifier == 2 ? 'G22:G23' : 'G15'), 'Q5:Q21', 'Q23:Q33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },
                    { 'logname': 'misc', 'watch': watch6, 'watchref': '6', 'watchadd': ['H3', 'R3'], 'address': ['H5:H10', 'H12:H13', 'H15:H16', fopurifier == 1 ? 'H18' : (fopurifier == 2 ? 'H18:H19' : 'H15'), dopurifier == 1 ? 'H20' : (dopurifier == 2 ? 'H20:H21' : 'H15'), lopurifier == 1 ? 'H22' : (lopurifier == 2 ? 'H22:H23' : 'H15'), 'R5:R21', 'R23:R33'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'K', 'tbl2rowsubhead': 'L' },

                    { 'logname': 'aircond', 'watch': watch1, 'watchref': '1', 'watchadd': ['D3', 'D16', 'N3', 'N16', 'N22'], 'address': ['D5:D8', 'D10:D13', 'D17', 'D19:D23', 'D25:D28', 'N5:N8', 'N10:N12', 'N17:N19', 'N23:N27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },
                    { 'logname': 'aircond', 'watch': watch2, 'watchref': '2', 'watchadd': ['E3', 'E16', 'O3', 'O16', 'O22'], 'address': ['E5:E8', 'E10:E13', 'E17', 'E19:E23', 'E25:E28', 'O5:O8', 'O10:O12', 'O17:O19', 'O23:O27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },
                    { 'logname': 'aircond', 'watch': watch3, 'watchref': '3', 'watchadd': ['F3', 'F16', 'P3', 'P16', 'P22'], 'address': ['F5:F8', 'F10:F13', 'F17', 'F19:F23', 'F25:F28', 'P5:P8', 'P10:P12', 'P17:P19', 'P23:P27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },
                    { 'logname': 'aircond', 'watch': watch4, 'watchref': '4', 'watchadd': ['G3', 'G16', 'Q3', 'Q16', 'Q22'], 'address': ['G5:G8', 'G10:G13', 'G17', 'G19:G23', 'G25:G28', 'Q5:Q8', 'Q10:Q12', 'Q17:Q19', 'Q23:Q27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },
                    { 'logname': 'aircond', 'watch': watch5, 'watchref': '5', 'watchadd': ['H3', 'H16', 'R3', 'R16', 'R22'], 'address': ['H5:H8', 'H10:H13', 'H17', 'H19:H23', 'H25:H28', 'R5:R8', 'R10:R12', 'R17:R19', 'R23:R27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },
                    { 'logname': 'aircond', 'watch': watch6, 'watchref': '6', 'watchadd': ['I3', 'I16', 'S3', 'S16', 'S22'], 'address': ['I5:I8', 'I10:I13', 'I17', 'I19:I23', 'I25:I28', 'S5:S8', 'S10:S12', 'S17:S19', 'S23:S27'], 'rowhead': 'B', 'rowsubhead': 'C', 'tbl2rowhead': 'L', 'tbl2rowsubhead': 'M' },

                    //{ 'logname': 'noonreport', 'watch': '0000-0400', 'address': ['C21', 'D2:D5', 'D7:D21', 'D23:D24', 'E28:E35', 'F28:F35', 'G28:G35', 'H23:H24', 'H28:H35', 'I2:I5', , 'I7:I21', 'I23:I24', 'I28:I35', 'J28:J35', 'K28:K35', 'L28:L35', 'M28:M35', 'N28:N35', 'O2:O7', 'O10:O15', 'O18:O22', 'O28:O35', 'P10:P15', 'P18:P21', 'P28:P35', 'Q7', 'Q28:Q35', 'R10:R15', 'R18:R21', 'R28:R35', 'S2:S7', 'S10:S15', 'S18:S21', 'S28:S35', 'T28:T35', 'U28:U35', 'V28:V35', 'W28:W35', 'X28:X35', 'Y28:Y35'] },
                    //{ 'logname': 'noonreport', 'watch': '0400-0800', 'address': ['C21', 'D2:D5', 'D7:D21', 'D23:D24', 'E28:E35', 'F28:F35', 'G28:G35', 'H23:H24', 'H28:H35', 'I2:I5', , 'I7:I21', 'I23:I24', 'I28:I35', 'J28:J35', 'K28:K35', 'L28:L35', 'M28:M35', 'N28:N35', 'O2:O7', 'O10:O15', 'O18:O22', 'O28:O35', 'P10:P15', 'P18:P21', 'P28:P35', 'Q7', 'Q28:Q35', 'R10:R15', 'R18:R21', 'R28:R35', 'S2:S7', 'S10:S15', 'S18:S21', 'S28:S35', 'T28:T35', 'U28:U35', 'V28:V35', 'W28:W35', 'X28:X35', 'Y28:Y35'] },
                    //{ 'logname': 'noonreport', 'watch': '0800-1200', 'address': ['C21', 'D2:D5', 'D7:D21', 'D23:D24', 'E28:E35', 'F28:F35', 'G28:G35', 'H23:H24', 'H28:H35', 'I2:I5', , 'I7:I21', 'I23:I24', 'I28:I35', 'J28:J35', 'K28:K35', 'L28:L35', 'M28:M35', 'N28:N35', 'O2:O7', 'O10:O15', 'O18:O22', 'O28:O35', 'P10:P15', 'P18:P21', 'P28:P35', 'Q7', 'Q28:Q35', 'R10:R15', 'R18:R21', 'R28:R35', 'S2:S7', 'S10:S15', 'S18:S21', 'S28:S35', 'T28:T35', 'U28:U35', 'V28:V35', 'W28:W35', 'X28:X35', 'Y28:Y35'] },
                    { 'logname': 'noonreport', 'watch': watch6, 'address': ['D3:D6', 'D8:D15', 'D17:D19', 'D21:D22', 'E29:E36', 'F29:F36', 'G29:G36', 'H29:H36', 'I3:I6', , 'I8:I19', 'I21:I22', 'I29:I36', 'J29:J36', 'K29:K36', 'L29:L36', 'M29:M36', 'N29:N36', 'O3:O8', 'O11:O16', 'O19:O23', 'O29:O36', 'P11:P16', 'P19:P22', 'P29:P36', 'Q11:Q16', 'Q19:Q22', 'Q29:Q36', 'R11:R16', 'R19:R22', 'R29:R36', 'S3:S7', 'S11:S16', 'S19:S22', 'S29:S36', 'T29:T36', 'U29:U36', 'V29:V36', 'W29:W36', 'X29:X36', 'Y29:Y36'] },
                    //{ 'logname': 'noonreport', 'watch': '1600-2000', 'address': ['C21', 'D2:D5', 'D7:D21', 'D23:D24', 'E28:E35', 'F28:F35', 'G28:G35', 'H23:H24', 'H28:H35', 'I2:I5', , 'I7:I21', 'I23:I24', 'I28:I35', 'J28:J35', 'K28:K35', 'L28:L35', 'M28:M35', 'N28:N35', 'O2:O7', 'O10:O15', 'O18:O22', 'O28:O35', 'P10:P15', 'P18:P21', 'P28:P35', 'Q7', 'Q28:Q35', 'R10:R15', 'R18:R21', 'R28:R35', 'S2:S7', 'S10:S15', 'S18:S21', 'S28:S35', 'T28:T35', 'U28:U35', 'V28:V35', 'W28:W35', 'X28:X35', 'Y28:Y35'] },
                    //{ 'logname': 'noonreport', 'watch': '2000-2400', 'address': ['C21', 'D2:D5', 'D7:D21', 'D23:D24', 'E28:E35', 'F28:F35', 'G28:G35', 'H23:H24', 'H28:H35', 'I2:I5', , 'I7:I21', 'I23:I24', 'I28:I35', 'J28:J35', 'K28:K35', 'L28:L35', 'M28:M35', 'N28:N35', 'O2:O7', 'O10:O15', 'O18:O22', 'O28:O35', 'P10:P15', 'P18:P21', 'P28:P35', 'Q7', 'Q28:Q35', 'R10:R15', 'R18:R21', 'R28:R35', 'S2:S7', 'S10:S15', 'S18:S21', 'S28:S35', 'T28:T35', 'U28:U35', 'V28:V35', 'W28:W35', 'X28:X35', 'Y28:Y35'] },

                    { 'logname': 'remarks', 'watch': watch1, 'watchref': '1', 'address': ['B3:B7'], 'name': 'D5', 'rank': 'D6', 'date': 'D7', 'signature': 'C3:D4' },
                    { 'logname': 'remarks', 'watch': watch2, 'watchref': '2', 'address': ['B8:B12'], 'name': 'D10', 'rank': 'D11', 'date': 'D12', 'signature': 'C8:D9' },
                    { 'logname': 'remarks', 'watch': watch3, 'watchref': '3', 'address': ['B13:B17'], 'name': 'D15', 'rank': 'D16', 'date': 'D17', 'signature': 'C13:D14' },
                    { 'logname': 'remarks', 'watch': watch4, 'watchref': '4', 'address': ['B18:B22'], 'name': 'D20', 'rank': 'D21', 'date': 'D22', 'signature': 'C18:D19' },
                    { 'logname': 'remarks', 'watch': watch5, 'watchref': '5', 'address': ['B23:B27'], 'name': 'D25', 'rank': 'D26', 'date': 'D27', 'signature': 'C23:D24' },
                    { 'logname': 'remarks', 'watch': watch6, 'watchref': '6', 'address': ['B28:B32'], 'name': 'D30', 'rank': 'D31', 'date': 'D32', 'signature': 'C28:D29' },

                    { 'logname': 'cheifengineer', 'watch': watch6, 'address': ['B35:B38'],'signature':'C35:D36', 'name': 'D37', 'date': 'D38' },
                ];
                
                return watchAddress;
            }

            function getWatchTime(year, month, date, currentTimeStamp, watchhour) {
                var watchTime;
                var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'meparam');
                var sheet = spreadsheet[0].sheetobj.get_activeSheet();
                var watch1 = sheet.get_range('C3').get_value() == '' || sheet.get_range('C3').get_value() == null || sheet.get_range('C3').get_value() == undefined ? '-' : sheet.get_range('C3').get_value();
                var watch2 = sheet.get_range('D3').get_value() == '' || sheet.get_range('D3').get_value() == null || sheet.get_range('D3').get_value() == undefined ? '-' : sheet.get_range('D3').get_value();
                var watch3 = sheet.get_range('E3').get_value() == '' || sheet.get_range('E3').get_value() == null || sheet.get_range('E3').get_value() == undefined ? '-' : sheet.get_range('E3').get_value();
                var watch4 = sheet.get_range('F3').get_value() == '' || sheet.get_range('F3').get_value() == null || sheet.get_range('F3').get_value() == undefined ? '-' : sheet.get_range('F3').get_value();
                var watch5 = sheet.get_range('G3').get_value() == '' || sheet.get_range('G3').get_value() == null || sheet.get_range('G3').get_value() == undefined ? '-' : sheet.get_range('G3').get_value();
                var watch6 = sheet.get_range('H3').get_value() == '' || sheet.get_range('H3').get_value() == null || sheet.get_range('H3').get_value() == undefined ? '-' : sheet.get_range('H3').get_value();

                if (watch1 != '-' && parseInt(watchhour) >= parseInt(watch1.split("-")[0]) && parseInt(watchhour) <= parseInt(watch1.split("-")[1]))
                    watchTime = watch1;
                if (watch2 != '-' && parseInt(watchhour) >= parseInt(watch2.split("-")[0]) && parseInt(watchhour) <= parseInt(watch2.split("-")[1]))
                    watchTime = watch2;
                if (watch3 != '-' && parseInt(watchhour) >= parseInt(watch3.split("-")[0]) && parseInt(watchhour) <= parseInt(watch3.split("-")[1]))
                    watchTime = watch3;
                if (watch4 != '-' && parseInt(watchhour) >= parseInt(watch4.split("-")[0]) && parseInt(watchhour) <= parseInt(watch4.split("-")[1]))
                    watchTime = watch4;
                if (watch5 != '-' && parseInt(watchhour) >= parseInt(watch5.split("-")[0]) && parseInt(watchhour) <= parseInt(watch5.split("-")[1]))
                    watchTime = watch5;
                if (watch6 != '-' && parseInt(watchhour) >= parseInt(watch6.split("-")[0]) && parseInt(watchhour) <= parseInt(watch6.split("-")[1]))
                    watchTime = watch6;

                if (watchTime == null)
                {
                    if (watch1 != '-') watchTime = watch1;
                    if (watch2 != '-') watchTime = watch2;
                    if (watch3 != '-') watchTime = watch3;
                    if (watch4 != '-') watchTime = watch4;
                    if (watch5 != '-') watchTime = watch5;
                    if (watch6 != '-') watchTime = watch6;
                }
                //if (currentTimeStamp >= new Date(year, month, date, 12, 0, 0) && new Date(year, month, date, 15, 59, 59) >= currentTimeStamp) {
                //    watchTime = '1200-1600';
                //}
                //else if (currentTimeStamp >= new Date(year, month, date, 16, 0, 0) && new Date(year, month, date, 19, 59, 59) >= currentTimeStamp) {
                //    watchTime = '1600-2000';
                //}
                //else if (currentTimeStamp >= new Date(year, month, date, 20, 0, 0) && new Date(year, month, date, 23, 59, 59) >= currentTimeStamp) {
                //    watchTime = '2000-2400';
                //}
                //    // future watch starts
                //else if (currentTimeStamp >= new Date(year, month, date , 0, 0, 0) && new Date(year, month, date , 3, 59, 59) >= currentTimeStamp) {
                //    watchTime = '0000-0400';
                //}
                //else if (currentTimeStamp >= new Date(year, month, date , 4, 0, 0) && new Date(year, month, date, 7, 59, 59) >= currentTimeStamp) {
                //    watchTime = '0400-0800';
                //}
                //else if (currentTimeStamp >= new Date(year, month, date , 8, 0, 0) && new Date(year, month, date, 11, 59, 59) >= currentTimeStamp) {
                //    watchTime = '0800-1200';
                //}
                return watchTime;
            }
            function disableconfiguration() {

                var fopurifier = null;
                var dopurifier = null;
                var lopurifier = null;

                var sheetfopurifier = unitConfiguration.find(x => x.field == 'FOPU');
                if (sheetfopurifier != null && sheetfopurifier != undefined) {
                    fopurifier = sheetfopurifier.Value;
                }

                var sheetdopurifier = unitConfiguration.find(x => x.field == 'DOPU');
                if (sheetdopurifier != null && sheetdopurifier != undefined) {
                    dopurifier = sheetdopurifier.Value;
                }
                var sheetlopurifier = unitConfiguration.find(x => x.field == 'LOPU');
                if (sheetlopurifier != null && sheetlopurifier != undefined) {
                    lopurifier = sheetlopurifier.Value;
                }
                var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == "misc");
                var sheet = spreadsheet[0].sheetobj.get_activeSheet();
                if (fopurifier != null && fopurifier == 0) { var range = sheet.get_range('C18:H18'); range.set_background('gray'); } else { var range = sheet.get_range('C18:H18'); range.set_background('white'); }
                if (fopurifier != null && (fopurifier == 0 || fopurifier == 1)) { var range = sheet.get_range('C19:H19'); range.set_background('gray'); } else { var range = sheet.get_range('C19:H19'); range.set_background('white'); }

                if (dopurifier != null && dopurifier == 0) { var range = sheet.get_range('C20:H20'); range.set_background('gray'); } else { var range = sheet.get_range('C20:H20'); range.set_background('white'); }
                if (dopurifier != null && (dopurifier == 0 || dopurifier == 1)) { var range = sheet.get_range('C21:H21'); range.set_background('gray');; } else { var range = sheet.get_range('C21:H21'); range.set_background('white'); }

                if (lopurifier != null && lopurifier == 0) { var range = sheet.get_range('C22:H22'); range.set_background('gray'); } else { var range = sheet.get_range('C22:H22'); range.set_background('white'); }
                if (lopurifier != null && (lopurifier == 0 || lopurifier == 1)) { var range = sheet.get_range('C23:H23'); range.set_background('gray'); } else { var range = sheet.get_range('C23:H23'); range.set_background('white'); }

            }

            function activateWatchOnCurrentTime(spreadsheets) {
                var currentDate = new Date();
                currentDate.setHours(0, 0, 0, 0);
                var watchaddress = getLogWatchAddress();
                var log = watchaddress.filter(w=> w.logname.toLowerCase() == 'meparam')
                log.forEach(w=> {

                    var currentWatchTime = w.watch;
                    //var currentWatchTime = getCurrentWatchTime();
                    //console.log(allowentry);
                    var proceed = 1;
                    if (currentWatchTime == undefined || currentWatchTime == null || currentWatchTime == '-' || currentWatchTime == '0000 - ' || allowentry == '0') {
                        //return; // if watch nots meet the critier
                        proceed = 0;
                    }

                    var selectedDate = $find("<%= txtdate.ClientID %>");

                    var currentWatchTimeState = validationState.filter(v => v.watch.toLowerCase() == currentWatchTime);
                    var flags = currentWatchTimeState.map(state => state.dutyengineerSigned);

                    if (flags.includes(true)) {
                        //return;
                        proceed = 0;
                    }

                   

                    //if (selectedDate && selectedDate.get_selectedDate().getTime() != currentDate.getTime()) {
                    //    return;
                    //}
                    if (proceed == 1) {
                        enableDisableSpreadSheetWatches(spreadsheets, currentWatchTime, true);
                        disableconfiguration();
                    }

                });

                var result = validationState.find(state => state.dutyengineerSigned == false);
                if (result == undefined || result == null) {
                    var logsce = getLogWatchAddress().filter(w => w.logname == 'cheifengineer');
                    var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'remarks');
                    logsce.forEach(log => {
                        log.address.forEach(address => {
                            cellRangeEnable(spreadsheet[0].sheetobj, address, true);
                        });
                    });
                }

               // enablesignature();
            }

            //function enablesignature()
            //{
            //    var logsce = getLogWatchAddress().filter(w => w.logname == 'cheifengineer');
            //    var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == 'remarks');
            //    logsce.forEach(log => {
            //        cellRangeEnable(spreadsheet[0].sheetobj, log.signature, true);
            //    });

            //    var logsde = getLogWatchAddress().filter(w => w.logname.toLowerCase() == 'remarks');
            //    logsde.forEach(log => {
            //        cellRangeEnable(spreadsheet[0].sheetobj, log.signature, true);
            //    });
                
            //}
            function enableDisableSpreadSheetWatches(spreadsheets, watch, enable) {
                if (mainenginecount == "2")
                    var watchAddress = getLogWatchAddress().filter(w => w.logname != 'cheifengineer');
                else
                    var watchAddress = getLogWatchAddress().filter(w => w.logname != 'cheifengineer' & w.logname != 'meparam2' & w.logname != 'metemp2' & w.logname != 'turbocharger2');
                var logs = watchAddress.filter(w => w.watch.toLowerCase() == watch);
                logs.forEach(log => {
                    var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == log.logname.toLowerCase());
                    
                    log.address.forEach(address => {
                        cellRangeEnable(spreadsheet[0].sheetobj, address, enable);
                    });
                });
            }

            function dutyEngineerSigned(watch, name, rank, date) {
                setTimeout(() => {
                    if (spreadsheets) {
                        //console.log('duty engineer signed for watch : ' + watch + ' date : ' + date);
                        enableDisableSpreadSheetWatches(spreadsheets, watch, false);
                        var watchAddress = getLogWatchAddress();
                        var currentWatchAddress;
                        if (mainenginecount == "2")
                            currentWatchAddress = getLogWatchAddress().filter(w => w.watch.toLowerCase() == watch.toLowerCase());
                        else
                            currentWatchAddress = getLogWatchAddress().filter(w => w.watch.toLowerCase() == watch.toLowerCase() & w.logname != 'meparam2' & w.logname != 'metemp2' & w.logname != 'turbocharger2');
                        //var logs = watchAddress.filter(w => w.watch.toLowerCase() == watch);

                        var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == "remarks");
                        var cellRange = currentWatchAddress.filter(cw => cw.logname == "remarks");
                        
                        var sheet = spreadsheet[0].sheetobj.get_activeSheet();

                        setRangeValue(sheet, cellRange[0].name, name);
                        setRangeValue(sheet, cellRange[0].rank, rank);
                        setRangeValue(sheet, cellRange[0].date, date);
                        
                        // temp code for color check
                        
                        ChangeColor(spreadsheets, currentWatchAddress);
                        spreadsheet[0].sheetobj.save();
                    }

                    PaneResized();
                    sheetScroll();
                }, 7000);
            }

            function ChangeColor(spreadsheets, currentWatchAddress) {
                var color = 'black';
                spreadsheets.forEach(spreadsheet => {
                    var sheet = spreadsheet.sheetobj.get_activeSheet();
                    var currentSheetAddress = currentWatchAddress.filter(cw => cw.logname == spreadsheet.sheetName);
                    if (currentSheetAddress != null && currentSheetAddress != undefined && currentSheetAddress.length>0) {
                        currentSheetAddress[0].address.forEach(add => {
                            var range = sheet.get_range(add);
                            range.set_color(color);
                        });
                    }
                });
            }


            function cheifEngineerSigned(watch, name, date) {
                setTimeout(() => {
                    if (spreadsheets) {
                        //console.log('cheif engineer signed');
                        enableDisableSpreadSheetWatches(spreadsheets, watch, false);
                        var watchAddress = getLogWatchAddress();
                        var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == "remarks");
                        var cellRange = watchAddress.filter(wa => {
                            if (wa.logname == "cheifengineer") {
                                return true;
                            }
                            return false;
                        });
                        var sheet = spreadsheet[0].sheetobj.get_activeSheet();
                        setRangeValue(sheet, cellRange[0].name, name);
                        setRangeValue(sheet, cellRange[0].date, date);
                        spreadsheet[0].sheetobj.save();
                    }
                    addimage();
                }, 7000);
            }

            function setRangeValue(sheetObj, address, value) {
                if (sheetObj && address && value) {
                    var range = sheetObj.get_range(address);
                    range.set_value(value);
                }
            }

            function dayValidationSchedular(spreadsheets) {
                intervalState['dayValidationIntervalId'] = setInterval(() => {
                    dayValidation(spreadsheets);
                }, 4000);
            };

            function dayValidation(spreadsheets) {
                    var watchAddress = getLogWatchAddress();
                    var currentWatchTime = getCurrentWatchTime();
                    //console.log('current watch Time - ' + currentWatchTime);
                    currentWatchTime = currentWatchTime ? currentWatchTime : '1200-1600'; // default validation for past days
                     for (var spreadsheet of spreadsheets) {
                        if (spreadsheet) {
                            var logAddress = watchAddress.filter(w => w.logname == spreadsheet.sheetName);
                    // get the value for each cell 
                   

                            var spreadsheetObj = spreadsheet.sheetobj;
                            var activeSheet = spreadsheetObj.get_activeSheet();
                            var states = validationState.filter(v =>  v.logname.toLowerCase() == spreadsheet.sheetName.toLowerCase());
                            if (spreadsheetObj && logAddress && logAddress.length > 0) {
                                logAddress.forEach(logAddres => {
                                    var flag = [];
                                    logAddres.address.forEach(address => {
                                        //alert(spreadsheet.sheetName + '     ' + address);
                                        var range = activeSheet.get_range(address);

                                        // here we need to attach validation functionality here
                                        if (spreadsheet.sheetName == 'remarks') {
                                            checkDutyEngineerValidation(spreadsheetObj, logAddres, currentWatchTime);
                                        }
                                        
                                        var values = range.get_values();
                                        //if (spreadsheet.sheetName.toLowerCase() == 'metemp' )
                                        //{
                                            

                                        //}
                                        var proced = 1;
                                        if (spreadsheet.sheetName.toLowerCase() == 'noonreport' && (address == 'Q19:Q22' || address == 'Q11:Q16'))
                                        {
                                            proced = 0;
                                        }
                                           
                                        if (values && values.length > 0 && proced==1) {
                                            var rangeValues = range.get_values().flat();
                                            if (rangeValues.includes(null) || rangeValues.includes(undefined)) {
                                                flag.push(false);
                                            } else {
                                                flag.push(true);
                                            }
                                        }
                                    });
                                    //updateValidationState(spreadsheet.sheetName, logAddres.watch, !flag.includes(false));
                                    updateValidationState(states, logAddres.watch, !flag.includes(false));
                                });
                                // update the completed flag
                                updateIsCompletedFlag(spreadsheet.sheetName, validationState);
                            }
                        }
                    }

                    changeIndicator(validationState, currentWatchTime);
                    changeDayIndicator(validationState, spreadsheets);
            }

            function updateValidationState(validStates, watch, validation) {
                if (validStates && watch) {
                    var state = validStates.find(s => s.watch.toLowerCase() == watch.toLowerCase());
                    if (state) {
                        state.validation = validation;
                        return true;
                    }
                }
                return false;
            }

            function checkCheifEngineerValidationSchedular(spreadsheets) {
                intervalState['cheifEngineerValidationIntervalId'] = setInterval(() => {
                    checkCheifEngineerValidation(spreadsheets)
                }, 3000);
            }

            function checkCheifEngineerValidation(spreadsheets) {
                    var ceAddress = getLogWatchAddress().find(w => w.logname == 'cheifengineer');
                    var spreadsheet = spreadsheets.filter(s => s.sheetName.toLowerCase() == "remarks");
                    var values = [];
                    var flag = false;
                    var activeSheet = spreadsheet[0].sheetobj.get_activeSheet();
                    var namerange = activeSheet.get_range(ceAddress.name);
                    var daterange = activeSheet.get_range(ceAddress.date);
                    values.push(namerange.get_value());
                    values.push(daterange.get_value());
                    if (values.includes(null) || values.includes(undefined)) {
                        flag = false;
                    } else {
                        flag = true;
                    }
                    validationState.forEach(v => v.cheifengineerSigned = flag);
            }

            function updateIsCompletedFlag(sheetName, validationState) {
                if (sheetName && validationState) {
                    var states = validationState.filter(v => v.logname.toLowerCase() == sheetName.toLowerCase());
                    var flag = [];
                    // to many loops need to rewrite the logic
                    states.forEach(state => {
                        flag.push(state.validation);
                    });
                    states.map(state => state.isLogCompleted = !flag.includes(false));
                }
            }

            function checkDutyEngineerValidation(spreadsheetObj, addressObj, watch) {
                if (validationState && spreadsheetObj && addressObj && watch) {

                    var activeSheet = spreadsheetObj.get_activeSheet();
                    var dutyEngineerCheck = ['name', 'rank', 'date'];
                    var flag = [];
                    for (var check of dutyEngineerCheck) {
                        var range = activeSheet.get_range(addressObj[check]);
                        // here we need to attach validation functionality here
                        var values = range.get_values();
                        if (values && values.length > 0) {
                            var rangeValues = range.get_values().map(m => m[0]);
                            if (rangeValues.includes(null) || rangeValues.includes(undefined)) {
                                flag.push(false);
                            } else {
                                flag.push(true);
                            }
                        }
                    }
                    validationState.filter(v => v.watch.toLowerCase() == addressObj.watch.toLowerCase()).forEach(state => {
                            state.dutyengineerSigned = !flag.includes(false);
                    });
                }
            }

            function changeIndicator(validationState, watch) {
                
                if (validationState && watch) {
                    var states = validationState.filter(state => state.watch.toLowerCase() == watch.toLowerCase());
                   // console.log(states);
                    states.forEach(v => {
                        if (v.validation && v.dutyengineerSigned) {
                            $telerik.$(`#${v.watchIndicatorId}`).attr('class', 'indicator indicator-success');
                        } else if (v.validation && v.dutyengineerSigned == false) {
                            $telerik.$(`#${v.watchIndicatorId}`).attr('class', 'indicator indicator-amber');
                        } else {
                            $telerik.$(`#${v.watchIndicatorId}`).attr('class', 'indicator indicator-failed');
                        }
                    });
                }
            }

            function changeDayIndicator(validationState, spreadsheets) {
               
                if (validationState && spreadsheets) {
                    var spreadsheetsName = spreadsheets.map(s => s.sheetName);
                    spreadsheetsName.forEach(sheetName => {
                        var v = validationState.filter(v => v.logname == sheetName)[0];
                        if (v) {
                            if (v.validation && v.dutyengineerSigned && v.cheifengineerSigned && v.isLogCompleted) {
                                $telerik.$(`#${v.dayIndicatorId}`).attr('class', 'indicator indicator-success');
                            } else if (v.validation && v.isLogCompleted) {
                                $telerik.$(`#${v.dayIndicatorId}`).attr('class', 'indicator indicator-amber');
                            } else {
                                $telerik.$(`#${v.dayIndicatorId}`).attr('class', 'indicator indicator-failed');
                            }
                        }
                    });
                }
            }
            
            function changeTextColorOnValidation(rangeObj, validStates) {
                if (rangeObj && validStates) {
                    rangeObj.set_color('red');
                }
            }


            function getCurrentWatchTime() {
                var currentTime = new Date();
                var watchhour = ('0' + currentTime.getHours()).slice(-2) + "" + ('0' + currentTime.getMinutes()).slice(-2);
                var selectedDt = $find("<%= txtdate.ClientID %>").get_selectedDate();
                var currentWatchTime = getWatchTime(selectedDt.getFullYear(), selectedDt.getMonth(), selectedDt.getDate(), currentTime.getTime(), watchhour)
                return currentWatchTime;
            }

            function validateState(watch, logname) {
                this.cheifengineerSigned = false;
                this.dayIndicatorId = `${logname}` + 'DayIndicator';
                this.dutyengineerSigned = false;
                this.logname = logname;
                this.watch = watch;
                this.isLogCompleted = false;
                this.watchIndicatorId = `${logname}` + 'WatchIndicator';
                this.validation = false;
            }

            function createValidationState() {
                if (mainenginecount == "2")
                    var watchAddress = getLogWatchAddress().filter(w => w.logname != 'cheifengineer');
                else
                    var watchAddress = getLogWatchAddress().filter(w => w.logname != 'cheifengineer' & w.logname != 'meparam2' & w.logname != 'metemp2' & w.logname != 'turbocharger2');
                
                var watches = watchAddress.filter(w => w.logname != 'cheifengineer').map(w => {
                    return { 'logname': w.logname, 'watch': w.watch }
                });
                watches.forEach(w => {
                    validationState.push(new validateState(w.watch,w.logname));
                }) 
            }
           
            
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>



       <%-- <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="meparam">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="meparam" />
                    </UpdatedControls>
                </telerik:AjaxSetting>                
            </AjaxSettings>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="metemp">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="metemp" />
                    </UpdatedControls>
                </telerik:AjaxSetting>                
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
                <telerik:RadAjaxPanel runat="server" Height="100%" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="pageOnLoad" ClientEvents-OnResponseEnd="pageOnUnload">


        <eluc:Status ID="ucStatus" runat="server" Visible="false"></eluc:Status>
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>

            <asp:HiddenField runat="server" ID="hdnValidateStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
            

            <asp:HiddenField runat="server" ID="hdnmachinery" />
            <asp:HiddenField ID="hdnallowentry" runat="server" />
            <asp:HiddenField ID="hdnallowamend" runat="server" />
            <asp:HiddenField ID="hdnallowWatchsign" runat="server" />
            <asp:HiddenField ID="hdnallowdaysign" runat="server" />  
            <asp:HiddenField ID="hdnislock" runat="server" />
            <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="100%" Width="100%">
                                <telerik:RadPane ID="navigationPane" runat="server" Width="300" Scrolling="None" OnClientResized="PaneResized">

                    <div class="section-container">

                        <div class="log-selection-container">

                            <table class="header">
                                <tr style="display:none;">
                                    <td>
                                        <h5 class="title">Select Log</h5>
                                    </td>
                                    <td>
                                                     <telerik:RadComboBox ID="ddlLogSelect" runat="server" CssClass="controls" OnSelectedIndexChanged="ddlLogSelect_SelectedIndexChanged" AutoPostBack="true"
                                            DropDownPosition="Static" EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                        </telerik:RadComboBox>&nbsp;&nbsp;

                                    </td>
                                </tr>
                                <tr>
									<td>
                                    <table>
                                            <tr>
                                                <td colspan="3">
                                                    <div style="display: flex;padding: .2em">
                                                        <asp:LinkButton runat="server" ID="lnkPdfall" OnClick="lnkPdf_Click">



                                                            <img src="../css/Theme1/images/pdf.png" style="margin: 1%;" title="Export PDF" />
                                                        </asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="LinkButton2" OnClientClick="openHistory();return false;">
                                                            <img src="../css/Theme1/images/task-list.png" style="margin: 1%;" title="History" class="cursor"  />
                                                    </asp:LinkButton>


                                                        <asp:LinkButton runat="server" ID="lnkconfiguration" OnClientClick="openConfig();return false;">
                                                            <img src="../css/Theme1/images/part.png" style="margin: 1%;" title="Configuration" class="cursor"  />
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 class="title">Date</h5>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker runat="server" ID="txtdate" CssClass="controls" AutoPostBack="true" OnSelectedDateChanged="txtdate_SelectedDateChanged"></telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <button runat="server" id="btnPrev" class="controls-btn" onserverclick="btnPrev_Click">◄</button></td>
                                                <td>
                                                    <button runat="server" id="btnNext" class="controls-btn" onserverclick="btnNxt_Click">►</button></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div class="section">

                            <table class="header" style="width: 100%;" runat="server" id="section">
                                <thead>
                                    <tr>
                                        <td style="width: 220px;">
                                            <h3>Sections</h3>
                                        </td>
                                        <td class="links center" style="width: 35px; text-align: center;">Watch</td>
                                        <td class="links day-indicator">Day</td>
                                    </tr>
                                </thead>
                                <tr id="trme1head" runat="server"><td><h3><telerik:RadLabel ID="lblme1Head" runat="server" Text="Main Engine 1"></telerik:RadLabel></h3></td></tr>
                                <tr>
                                    <td>
                                        <a id="lnkmeparam" class="links" href="#meparam">Main Engine Parameters</a>
                                    </td>
                                    <td class="center">
                                        <span id="meparamWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="meparamDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#metemp">ME Temperatures</a>
                                    </td>
                                    <td class="center">
                                        <span id="metempWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="metempDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#turbocharger">ME Turbocharger(s)</a>
                                    </td>
                                    <td class="center">
                                        <span id="turbochargerWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="turbochargerDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>

                                <tr id="trme2head" runat="server"><td><h3><telerik:RadLabel ID="lblme2" runat="server" Text="Main Engine 2"></telerik:RadLabel></h3></td></tr>
                                <tr id="trme2param" runat="server">
                                    <td>
                                        <a class="links" href="#meparam2">Main Engine Parameters</a>
                                    </td>
                                    <td class="center">
                                        <span id="meparam2WatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="meparam2DayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr id="trme2temp" runat="server">
                                    <td>
                                        <a class="links" href="#metemp2">ME Temperatures</a>
                                    </td>
                                    <td class="center">
                                        <span id="metemp2WatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="metemp2DayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr id="trme2turbocharger" runat="server">
                                    <td>
                                        <a class="links" href="#turbocharger2">ME Turbocharger(s)</a>
                                    </td>
                                    <td class="center">
                                        <span id="turbocharger2WatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="turbocharger2DayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>

                                <tr id="trempty" runat="server"><td colspan="3">&nbsp;</td></tr>

                                <tr>
                                    <td>
                                        <a class="links" href="#generator">Generator Engine(s)</a>
                                    </td>
                                    <td class="center">
                                        <span id="generatorWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="generatorDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#misc">F.O. / L.O / Misc / Heat Exchangers</a>
                                    </td>
                                    <td class="center">
                                        <span id="miscWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="miscDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#aircond">Air Cond / Refrig / FW Gen / Boiler</a>
                                    </td>
                                    <td class="center">
                                        <span id="aircondWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="aircondDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#remarks">Remarks / D.E. Sign/ C.E. Sign</a>
                                    </td>
                                    <td class="center">
                                        <span id="remarksWatchIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="remarksDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a class="links" href="#noonreport">Noon Report, Daily Cons & Hours</a>
                                    </td>
                                    <td class="center">
                                        <%--<asp:Literal runat="server" ID="noonreportWatchIndicator"></asp:Literal>--%>
                                    </td>
                                    <td class="day-indicator">
                                        <span id="noonreportDayIndicator" class="indicator indicator-failed"></span>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>

                    <div class="export-window">
                        <table class="full-width">
                            <tr>
                                <td class="underline">List of Daily Engine Log
                                    <asp:LinkButton runat="server" ID="lnkPdf" OnClick="lnkPdf_Click">
                                            <img src="../css/Theme1/images/pdf.png" style="margin: 1%;" title="Export PDF" />
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkHistory" OnClick="lnkHistory_Click">
                                            <img src="../css/Theme1/images/task-list.png" style="margin: 1%;" title="History" class="cursor"  />
                                    </asp:LinkButton>
                                    
                                    <%--<img src="../css/Theme1/images/task-list.png" style="margin: 1%;" title="History" class="cursor" onclick="openHistory()" />--%>
                                </td>
                                <td onclick="closeExport()" class="underline cursor"><a title="Close"></a>Close</td>
                            </tr>
                        </table>

                        <asp:Repeater ID="gvLogListRepeater" runat="server" OnItemDataBound="gvLogListRepeater_ItemDataBound">
                            <HeaderTemplate>
                                <table id="log-list" class="full-width fixed_header">
                                    <thead>
                                        <tr>
                                            <th style="width: 50px"></th>
                                            <th style="width: 75px">Date</th>
                                            <th style="width: 50px">Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 50px">
                                        <telerik:RadCheckBox runat="server" ID="chkDate" AutoPostBack="false" ></telerik:RadCheckBox>
                                    </td>
                                    <td style="width: 75px">
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("FLDDATE", "{0:dd-MM-yyyy}") %>' />
                                    </td>
                                    <td style="width: 75px">
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("FLDSTATUS") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                 <tfoot>
                                     <tr>
                                         <td colspan="4" style="text-align: right;"><b><telerik:RadLabel runat="server" ID="lblRecords"></telerik:RadLabel></b> Records Found</td>
                                     </tr>
                                 </tfoot>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </div>
                </telerik:RadPane>

                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>

                <telerik:RadPane ID="contentPane" runat="server" CssClass="contentpane">
                    <div class="container-fluid" style="position: fixed">
                        <div class="row bg-success">
                        <div class="col-lg-8 col-md-8">
                            <table id="vesselDetails">
                                <tr>
                                    <td style="width: 50px" class="label-text">MV/MT: </td>
                                    <td style="width: 300px">
                                        <telerik:RadLabel ID="lblvesselname" runat="server" CssClass="label-value"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 300px" class="label-text">Voyage number:</td>
                                    <td style="width: 250px">
                                        <telerik:RadLabel ID="lblvoyageno" runat="server" CssClass="label-value"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 250px" class="label-text">From</td>
                                    <td style="width: 250px">
                                        <telerik:RadLabel ID="lblfrom" runat="server" CssClass="label-value"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 250px" class="label-text">To</td>
                                    <td style="width: 250px">
                                        <telerik:RadLabel ID="lblto" runat="server" CssClass="label-value"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 250px" class="label-text">Date</td>
                                    <td style="width: 250px">
                                        <telerik:RadLabel ID="lbldate" runat="server" CssClass="label-value"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>


                        <div class="btn-container col-lg-4 col-md-4">

                           <%-- <button class="btn btn-primary pull-right custom-btn" type="button" title="Click here to show the edit page for engine log book" runat="server" id="btnList" onclick="openExport()">
                                List
                            </button>--%>

                            <%--<button class="btn btn-primary pull-right custom-btn" type="button" onclick="amend()">
                                Amend
                            </button>

                            <button class="btn btn-primary pull-right custom-btn" type="button" onclick="instantSave()">
                                Save
                            </button>--%>
							</div>
                        </div>
                    <div class="row">
                    <div id="sheetcontainer" style="position:relative; overflow: auto">
                        <div id="sheetscroll" class="overlay"></div>
                        <div id="spreadsheet">

                            <div id="meparam">                                
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Height="580px" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="metemp">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet2" Height="675px" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="turbocharger">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet3" Height="535px" Width="180%" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="meparam2" runat="server">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet9" Height="580px" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="metemp2" runat="server">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet10" Height="675px" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="turbocharger2" runat="server">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet11" Height="535px"  Width="180%" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="generator">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet4" Height="590px" Width="180%" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="misc">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet5" Height="820px" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>

                            <div id="aircond">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet6" Height="780px" OnClientChange="OnClientChange">                                    
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>
                            <div id="remarks">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet8" Height="960px" OnClientChange="OnRemarksClientChange">
                                </telerik:RadSpreadsheet>
                            </div>
                            <div id="noonreport">
                                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet7" Height="930px" Width="180%" OnClientChange="OnClientChange">
                                    <ContextMenus>
                                        <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                            <Items>
                                                <telerik:RadMenuItem Text="Amend" Value="cmdAmend"></telerik:RadMenuItem>
                                            </Items>
                                        </CellContextMenu>
                                    </ContextMenus>
                                </telerik:RadSpreadsheet>
                            </div>
                        </div>
                    </div>
				</div>
            </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>

    </form>
</body>

</html>
