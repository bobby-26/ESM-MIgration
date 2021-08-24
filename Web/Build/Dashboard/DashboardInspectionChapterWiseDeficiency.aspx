<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardInspectionChapterWiseDeficiency.aspx.cs" Inherits="Dashboard_DashboardInspectionChapterWiseDeficiency" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">


    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script src="../js/jquery.min.js"></script>
    <link href="../css/Theme1/sn-visual/sn-visual.css" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">
    <script src="../js/d3/d3.js"></script>
    <script src="../js/sn-visual/sn-visual.js"></script>
    <script src="../js/sn-visual/sn-visual-theme.js"></script>


    <style>
        /* Checkbox List Styles Starts*/
        .ListControl input[type=checkbox], input[type=radio] {
            display: none;
        }

        .ListControl label {
            display: inline;
            float: left;
            color: #000;
            cursor: pointer;
            text-indent: 20px;
            white-space: nowrap;
        }

        .ListControl input[type=checkbox] + label {
            display: block;
            width: 8px;
            height: 1em;
            border: 0.0625em solid rgb(27, 26, 26);
            border-radius: 0.25em;
            vertical-align: middle;
            line-height: 1em;
            font-size: 8px;
        }

        .ListControl input[type=checkbox]:checked + label {
            background-image: none;
            background-color: black;
            opacity: 0.8;
            width: 8px;
        }

        .ListControl input[type=radio] + label {
            width: 8px;
            height: 1em;
            border-radius: 1em;
            display: block;
            border: 0.0625em solid rgb(27, 26, 26);
            vertical-align: middle;
            line-height: 1em;
            font-size: 8px;
        }

        .ListControl input[type=radio]:checked + label {
            background-image: none;
            background-color: black;
            opacity: 0.8;
            width: 8px;
        }

        /* Checkbox List Styles Stops*/

        /*Layout Style Starts*/
    .v-boundary {
        //border: 1px dashed slategray;
    }
    .v-ground {
        padding: 2px;
        display: flex;
    }
    .v-tray {
        margin: 2px;
        display: inline-block;
    }

/*Layout Style Ends*/
    </style>
</telerik:RadCodeBlock></head>

<body>
    <form id="form1" runat="server">
        <div class="container-wrapper">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <div>
            
                <div class="subHeader" style="position: relative">
                <eluc:title runat="server" id="Title1" text="" showmenu="true"></eluc:title>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MainMenu" runat="server" TabStrip="true" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuSearch" OnTabStripCommand="MenuSearch_TabStripCommand" runat="server"></eluc:TabStrip>
                    </span>
                </div>
                <br />
            </div>
                <div style="height: 100%; width: 100%; opacity: 0.8;">
                    <div style="width: 10%; height: 100%; float: left; vertical-align: top; font-size: xx-small; font-family: 'Segoe UI'; white-space: nowrap;">
                        <div style="margin: 5px; font-family: 'Segoe UI';margin-top: 25px;">
                            <div
                                style="float: left; vertical-align: text-bottom; font-size: 12px; font-weight: bold;">
                                <%--  OWNER--%>
                            PRINCIPAL
                            </div>
                            <div runat="server" class="ListControl" id="div1" style="border: 1px solid lightgray; overflow-x: hidden; overflow-y: auto; height: 347px; width: 100%; line-height: none;">
                                <asp:RadioButtonList ID="rdbAddressPrincipal" runat="server"></asp:RadioButtonList>
                            </div>
                        </div>
                        <div style="margin: 5px; font-family: 'Segoe UI';">
                            <span style="font-size: 12px; font-weight: bold;">YEAR  </span>

                            <div runat="server" class="ListControl" id="divYearList" style="border: 1px solid lightgray; padding-left: 5px; overflow-x: hidden; overflow-y: auto; height: 105px; width: 100%; line-height: none;">
                                <asp:CheckBoxList ID="chkyear" runat="server"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>


                    <div style="width: 89%; height: 100%; float: left; display: inline-block; vertical-align: top;">
                       <%-- <div class="navSelect" style="position: relative; clear: both; width: 15px">
                            <eluc:TabStrip ID="MenuSearch" OnTabStripCommand="MenuSearch_TabStripCommand" runat="server"></eluc:TabStrip>
                        </div>--%>

                               <div id="visual-ground" class="v-boundary v-ground row" style="float:left;"></div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="deficencyData" runat="server" />

            <%-- Inspection Vs Deficency Starts --%>
                

            <%-- Inspection Vs Deficency Ends --%>
            

        </div>
    </form>

    <script>

        (function () {

            var selectedValue = {};

            if (document.getElementById('deficencyData').value != '') {
                

                getDataGrouped = function (groupBy, sourceData) {
                    let result = [];
                    for (var index = 0; index < sourceData.length; index++) {
                        d = sourceData[index];
                        let gIndex = result.findIndex(f => f['keyProp'] == d[groupBy]);
                        if (gIndex < 0) {
                            let newItem = {};
                            newItem['keyProp'] = d[groupBy];
                            newItem['insp-by-def'] = [];
                            newItem['insp-by-def'].push({ key: 'Inspection', value: d['FLDINSPECTIONCOUNT'] })
                            newItem['insp-by-def'].push({ key: 'Deficiency', value: d['FLDDEFICIENCYCOUNT'] })
                            result.push(newItem);
                            gIndex = result.length - 1;
                        }
                        else {
                            let iIndex = result[gIndex]['insp-by-def'].findIndex(f => f['key'] == 'Inspection');
                            let dIndex = result[gIndex]['insp-by-def'].findIndex(f => f['key'] == 'Deficiency');
                            result[gIndex]['insp-by-def'][iIndex]['value'] += d['FLDINSPECTIONCOUNT'];
                            result[gIndex]['insp-by-def'][iIndex]['value'] += d['FLDDEFICIENCYCOUNT'];
                        }
                    }

                    return result;
                }


                var rawData_insp_def = JSON.parse(document.getElementById('deficencyData').value)['Table1'];
                var rawData_insp_def_barData = JSON.parse(document.getElementById('deficencyData').value)['Table'];
                ;

                let insp_vs_def_init = getDataGrouped('FLDMOU', rawData_insp_def_barData);
                
                ////new Function Implement
                function insVSDef(selected, propName) {
                    console.log('insVSDef');
                    var result = [];
                    selected.forEach(selectedProp => {
                        let filterdData = rawData_insp_def_barData.filter(obj => obj[propName] == selectedProp.key);

                        let sum = function (items, prop) {
                            return items.reduce(function (a, b) {
                                return a + b[prop];
                            }, 0);
                        };
                        var insp_vs_def_init = {};
                        insp_vs_def_init['keyProp'] = selectedProp.key;
                        insp_vs_def_init['insp-by-def'] = [];
                        insp_vs_def_init['insp-by-def'].push({ key: 'Inspection', value: sum(filterdData, 'FLDINSPECTIONCOUNT') })
                        insp_vs_def_init['insp-by-def'].push({ key: 'Deficiency', value: sum(filterdData, 'FLDDEFICIENCYCOUNT') })
                        result.push(insp_vs_def_init);
                    })
                    return result;
                }


                var visualList = [];

                let mou_data_init = d3.nest()
                .key(function (d) { return d['FLDMOU']; })
                .rollup(function (d) {
                    return d3.sum(d, function (item) { return item['FLDDEFICIENCYCOUNT']; })
                }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                let chapter_data_init = d3.nest()
                    .key(function (d) { return d['FLDCHAPTER']; })
                    .rollup(function (d) {
                        return d3.sum(d, function (item) { return item['FLDDEFICIENCYCOUNT']; })
                    }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                let country_data_init = d3.nest()
                    .key(function (d) { return d['FLDCOUNTRYNAME']; })
                    .rollup(function (d) {
                        return d3.sum(d, function (item) { return item['FLDDEFICIENCYCOUNT']; })
                    }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                let port_data_init = d3.nest()
                    .key(function (d) { return d['FLDPORTNAME']; })
                    .rollup(function (d) {
                        return d3.sum(d, function (item) { return item['FLDDEFICIENCYCOUNT']; })
                    }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');


                visual_params = [
         {
             _name: 'mou',
             holder: {
                 elementSelector: '#test-parent-5'
             },
             basic: {
                 title: 'Deficiency Count by MOU'
             },
             pattern: {
                 type: 'doughnut',
                 orientation: 'vertical',
                 dataKey: {
                     baseAxis: {
                         key: 'key',
                         title: 'M o U',
                     },
                     plotAxis: {
                         key: 'value',
                         title: 'MoU Def. Count',
                     }
                 },
                 legend: { position: 'ne' }
             },
             data: mou_data_init,
             listener: {
                 onVisualElementClick: function (context) {
                     selectedValue['FLDMOU'] = [];
                     let chapterAll = [];
                     // chapter - refresh
                     context.selected.forEach(function (sel) {
                         let filter = sel.key;
                         selectedValue['FLDMOU'].push(filter);
                         let chapter = d3.nest()
                             .key(function (d) { if (d['FLDMOU'] == filter) { return d['FLDCHAPTER']; } })
                             .rollup(function (d) {
                                 return d3.sum(d, function (item) {
                                     if (item['FLDMOU'] == filter) {
                                         return item['FLDDEFICIENCYCOUNT'];
                                     };
                                 })
                             }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         chapter.forEach(c => chapterAll.push(c));
                     });

                     chapterAll = d3.nest()
                       .key(function (d) { return d['key']; })
                       .rollup(function (d) {
                           return d3.sum(d, function (item) {
                               return item['value'];
                           })
                       }).entries(chapterAll).filter(obj => obj.key != 'undefined');

                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'chapter')[0]['visual'].id,
                         data: chapterAll
                     });

                     // country - refresh
                     let countryAll = [];

                     context.selected.forEach(function (sel) {
                         let filter = sel.key;

                         let country = d3.nest()
                         .key(function (d) { if (d['FLDMOU'] == filter) { return d['FLDCOUNTRYNAME']; } })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 if (item['FLDMOU'] == filter) {
                                     return item['FLDDEFICIENCYCOUNT'];
                                 };
                             })
                         }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         country.forEach(c => countryAll.push(c));

                     });

                     countryAll = d3.nest()
                           .key(function (d) { return d['key']; })
                           .rollup(function (d) {
                               return d3.sum(d, function (item) {
                                   return item['value'];
                               })
                           }).entries(countryAll).filter(obj => obj.key != 'undefined');

                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'country')[0]['visual'].id,
                         data: countryAll
                     });

                     // port - refresh
                     let portAll = [];

                     context.selected.forEach(function (sel) {
                         let filter = sel.key;
                         let port = d3.nest()
                         .key(function (d) { if (d['FLDMOU'] == filter) { return d['FLDPORTNAME']; } })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 if (item['FLDMOU'] == filter) {
                                     return item['FLDDEFICIENCYCOUNT'];
                                 };
                             })
                         }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         port.forEach(c => portAll.push(c));

                     })

                     portAll = d3.nest()
                          .key(function (d) { return d['key']; })
                          .rollup(function (d) {
                              return d3.sum(d, function (item) {
                                  return item['value'];
                              })
                          }).entries(portAll).filter(obj => obj.key != 'undefined');

                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'port')[0]['visual'].id,
                         data: portAll
                     });

                     //ins VS def - refresh
                     //console.log(filter);
                     let insp_vs_def_init = insVSDef(context.selected, 'FLDMOU');
                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'ins_vs_def')[0]['visual'].id,
                         data: insp_vs_def_init
                     });
                 }
             },
             scale: {
                 width: 150,
                 height: 110,
                 legend: {
                     width: 50
                 }
             },
             option: {
                 behaviour: {
                     interactionMode: 'manual'
                 },
                 suppress: {
                     randomColors: true,
                     toolBar: false,
                     tileHeader: false
                 }
             },
             decoration: {
                 textSize: 8
             }
         },
         {
             _name: 'country',
             holder: {
                 elementSelector: '#test-child-5-1'
             },
             basic: {
                 title: 'Deficiency Count by Country'
             },
             pattern: {
                 type: 'donut',
                 dataKey: {
                     baseAxis: {
                         key: 'key',
                         title: 'Country'
                     },
                     plotAxis: {
                         key: 'value',
                         title: 'Country-Wise Def. Count',
                     }
                 }
             },
             data: country_data_init,
             listener: {
                 onVisualElementClick: function (context) {
                     let filter = context.selected[0].key;
                     selectedValue['FLDCOUNTRYNAME'] = [];


                     // chapter - refresh
                     let chapterAll = [];

                     context.selected.forEach(function (sel) {
                         let filter = sel.key;
                         selectedValue['FLDCOUNTRYNAME'].push(filter);
                         let chapter = d3.nest()
                             .key(function (d) { if (d['FLDCOUNTRYNAME'] == filter) { return d['FLDCHAPTER']; } })
                             .rollup(function (d) {
                                 return d3.sum(d, function (item) {
                                     if (item['FLDCOUNTRYNAME'] == filter) {
                                         return item['FLDDEFICIENCYCOUNT'];
                                     };
                                 })
                             }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         chapter.forEach(c => chapterAll.push(c));

                     })


                     chapterAll = d3.nest()
                         .key(function (d) { return d['key']; })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 return item['value'];
                             })
                         }).entries(chapterAll).filter(obj => obj.key != 'undefined');


                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'chapter')[0]['visual'].id,
                         data: chapterAll
                     });

                     // port - refresh
                     console.log('port - refresh');
                     let portAll = [];
                     context.selected.forEach(function (sel) {
                         let filter = sel.key;
                         let port = d3.nest()
                         .key(function (d) { if (d['FLDCOUNTRYNAME'] == filter) { return d['FLDPORTNAME']; } })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 if (item['FLDCOUNTRYNAME'] == filter) {
                                     return item['FLDDEFICIENCYCOUNT'];
                                 };
                             })
                         }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         port.forEach(c => portAll.push(c))
                     });

                     portAll = d3.nest()
                         .key(function (d) { return d['key']; })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 return item['value'];
                             })
                         }).entries(portAll).filter(obj => obj.key != 'undefined');


                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'port')[0]['visual'].id,
                         data: portAll
                     });

                     //ins VS def - refresh
                     insVSdefKey = 'FLDCOUNTRYNAME';
                     let insp_vs_def_init = insVSDef(context.selected, 'FLDCOUNTRYNAME');
                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'ins_vs_def')[0]['visual'].id,
                         data: insp_vs_def_init
                     });
                 }
             },
             scale: {
                 width: 150,
                 height: 110,
                 legend: {
                     width: 50
                 }
             },
             option: {
                 behaviour: {
                     interactionMode: 'manual'
                 },
                 suppress: {
                     randomColors: true,
                     toolBar: false,
                     tileHeader: false,
                 }
             },
         },
         {
             _name: 'port',
             holder: {
                 elementSelector: '#test-child-5-1-1'
             },
             basic: {
                 title: 'Deficiency Count by Port'
             },
             pattern: {
                 type: 'donut',
                 dataKey: {
                     baseAxis: {
                         key: 'key',
                         title: 'Port'
                     },
                     plotAxis: {
                         key: 'value',
                         title: 'Port-Wise Def. Count'
                     }
                 }
             },
             data: port_data_init,
             listener: {
                 onVisualElementClick: function (context) {

                     //chapter- refresh
                     let chapterAll = [];
                     context.selected.forEach(function (sel) {
                         let filter = sel.key;
                         let chapter = d3.nest()
                             .key(function (d) { if (d['FLDPORTNAME'] == filter) { return d['FLDCHAPTER']; } })
                             .rollup(function (d) {
                                 return d3.sum(d, function (item) {
                                     if (item['FLDPORTNAME'] == filter) {
                                         return item['FLDDEFICIENCYCOUNT'];
                                     };
                                 })
                             }).entries(rawData_insp_def).filter(obj => obj.key != 'undefined');

                         chapter.forEach(c => chapterAll.push(c));
                     })

                     chapterAll = d3.nest()
                         .key(function (d) { return d['key']; })
                         .rollup(function (d) {
                             return d3.sum(d, function (item) {
                                 return item['value'];
                             })
                         }).entries(chapterAll).filter(obj => obj.key != 'undefined');

                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'chapter')[0]['visual'].id,
                         data: chapterAll
                     });

                     //ins VS def - refresh
                     //insVSdefKey = 'FLDPORTNAME';

                     let insp_vs_def_init = insVSDef(context.selected, 'FLDPORTNAME');
                     sn.visual.update({
                         id: visualList.filter(v => v['name'] == 'ins_vs_def')[0]['visual'].id,
                         data: insp_vs_def_init
                     });
                 }
             },
             scale: {
                 width: 150,
                 height: 110,
                 legend: {
                     width: 50
                 }
             },
             option: {
                 behaviour: {
                     interactionMode: 'manual'
                 },
                 suppress: {
                     randomColors: true,
                     toolBar: false,
                     tileHeader: false,
                 }
             }
         },
         {
             _name: 'chapter',
             holder: {
                 elementSelector: '#test-child-5-2'
             },
             basic: {
                 title: 'Deficiency Count by Chapter-Wise '
             },
             pattern: {
                 type: 'donut',
                 dataKey: {
                     baseAxis: {
                         key: 'key',
                         dataSourceKey: 'def-by-chapter-vessel',
                         title: 'Chapter'
                     },
                     plotAxis: {
                         key: 'value',
                         title: 'Chapter-Wise Def. Count',
                     }
                 }
             },
             data: chapter_data_init,
             scale: {
                 width: 150,
                 height: 110,
                 legend: {
                     width: 50
                 },
               
             },
             option: {
                 suppress: {
                     randomColors: true,
                     toolBar: false,
                     tileHeader: false
                 }
             }
         },
      
                ];


                $('.v-ground .v-tray').wrapAll('<div class="v-tray"></div>');

                _create_visuals = function () {
                    visual_params.forEach(function (param) {
                        visualList.push({
                            'name': param._name,
                            'visual': sn.visual.create(param)
                        });
                    });
                }


                $('#visual-ground').html(
                    // inspection visuals
                      '<div id="test-parent-5" class="v-boundary v-tray"></div>'
                    + '<div id="test-child-5-1" class="v-boundary v-tray"></div>'
                    + '<div id="test-child-5-1-1" class="v-boundary v-tray"></div>'

                    // chapter - deficiency visuals
                    + '<div id="test-child-5-2" class="v-boundary v-tray"></div>'
                    + '<div id="test-child-5-1-2" class="v-boundary v-tray"></div>'
                    + '<div id="test-child-5-1-1-1" class="v-boundary v-tray"></div>'

                    // mou - inspection vs deficiency
                    + '<div id="test-child-5-3" class="v-boundary v-tray"></div>'

                );

                sn.visual._model = [];
                visualList = [];
                _create_visuals();

                $('.v-ground .v-tray').wrapAll('<div class="v-tray"></div>');


            }
        }())



    </script>
</body>
</html>
