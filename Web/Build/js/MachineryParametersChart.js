//*****************Machinery Parameters Graph********************
//******************************************************************************

var tabNum = 1;
var cpGraphState1 = {};
var cpGraphState1a = {};
// var cpGraphState2 = {};
var cpGraphState3 = {};
var cpGraphState3a = {};
var cpGraphState4 = {};
var cpGraphState5 = {};
var cpGraphState5a = {};
var cpGraphState6 = {};
var cpGraphState7 = {};
var cpGraphState7a = {};
var cpGraphState8 = {};
var cpGraphState9 = {};
var cpGraphState9a = {};
var cpGraphState10 = {};
var cpGraphState10a = {};
var cpGraphState11 = {};

var seriesDataOrg = [];
var seriesNameOrg = [];
var dateRangeOrg = [];

$(function () {
    $("#datepicker_from").datepicker();
    $("#datepicker_to").datepicker();
});


//$(document).ready(function () {
//    mpchart01("mp01Chart01");
//    mpchart02("mp01Chart01a");
//    $(".fromCPDatePicker").datepicker();

//    var d = new Date();

//    var month = d.getMonth() + 1;
//    var day1 = d.getDate() - 6;
//    var day2 = d.getDate();
//    var m_ydate = '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
//    var dateOutput1 = (day1 < 10 ? '0' : '') + day1 + m_ydate;
//    var dateOutput2 = (day2 < 10 ? '0' : '') + day2 + m_ydate;

//    $("#fromDateInput").val(dateOutput1);
//    $("#toDateInput").val(dateOutput2);
//    var trigPointVal;
//    setTrigPoint();
//});

$(window).resize(function () {
    //cpGraphState.resize();
    resetGraph(tabNum);
});

function resetGraph(num) {

    switch (num) {
        case 1:
            cpGraphState1.resize();
            cpGraphState1a.resize();
            break;

        case 2:
            //cpGraphState2.resize();
            break;

        case 3:
            cpGraphState3.resize();
            cpGraphState3a.resize();
            break;

        case 4:
            cpGraphState4.resize();
            break;

        case 5:
            cpGraphState5.resize();
            cpGraphState5a.resize();
            break;

        case 6:
            cpGraphState6.resize();
            break;

        case 7:
            cpGraphState7.resize();
            cpGraphState7a.resize();
            break;

        case 8:
            cpGraphState8.resize();
            break;

        case 9:
            cpGraphState9.resize();
            cpGraphState9a.resize();
            break;

        case 10:
            cpGraphState10.resize();
            //cpGraphState10a.resize();
            break;

        case 11:
            cpGraphState11.resize();
            break;
    }
}

// Main Engine Graph
function mpchart01(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    var colors = ['#A3827B', '#2C9A57', '#265984', '#F8043F'];
    //var dateRange = dateRangeOrg;
    var dateRange = dateRange1;
    
    

    var seriesName = ['Average - M/E RPM (%)', 'Gov Setting Or Fuel Rack', 'Power Output (BHP)', 'M/E FOC (MT/Day)'];
    var seriesData = seriesData1;

    var maxncrbhp = Math.ceil(Math.max.apply(null, seriesData[11]));

    var ncrME = maxncrbhp;//10960; // to be fetched once from Vessel Master.
    var ncrMEmax = Math.round(ncrME + (ncrME * 0.1));

    var maxRPM = Math.round(Math.max.apply(null, seriesData[0]) + (Math.max.apply(null, seriesData[0])) * 0.1);
    var minRPM = Math.floor(Math.min.apply(null, seriesData[0]));

    var maxGOVSet = Math.round(Math.max.apply(null, seriesData[1]) + (Math.max.apply(null, seriesData[1])) * 0.1);
    var minGOVSet = Math.floor(Math.min.apply(null, seriesData[1]));

    var powerMin = Math.round(Math.max.apply(null, seriesData[2]) + (Math.max.apply(null, seriesData[2])) * 0.1);
    var powerMax = Math.floor(Math.min.apply(null, seriesData[2]));

    var maxFOC = Math.round(Math.max.apply(null, seriesData[3]) + (Math.max.apply(null, seriesData[3])) * 0.1);
    var minFOC = Math.floor(Math.min.apply(null, seriesData[3]));

    //var seriesData = [
    //  [71, 71, 71, 71, 71, 71, 71], // Speed  0
    //  [63, 62, 63, 63, 65, 63, 63], //Gov Settings  1
    //  [8041, 8030, 8025, 8060, 8150, 8040, 8030], //Power Output  2
    //  [24.5, 24.5, 24.5, 24.5, 24.8, 24.5, 24.5], // Main Engine FOC  3
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  4
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  5
    //  [90, 90, 90, 90, 90, 90, 90], // RPM value from Vessle master  6
    //  [9600, 9600, 9600, 9600, 9600, 9600, 9600], // Power out value from Vessel Master  7
    //  [25, 25, 25, 25, 25, 25, 25], // FOC  8
    //  [3, 4, 4, 4, 5, 4, 3], // Wind speed BF info  9
    //  [11.04, 12, 13, 11, 13, 14, 14] // L O G data, speed information  10
    //];

    var seriesUnit = ['%', '', 'BHP', 'MT/d'];

    

    option = {
        title: {
            text: 'Main Engine',
            subtext: 'Average RPM - Gov Setting/Fuel Rack - Power (BHP) - Fuel Consumption',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 110,
            right: 140
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and at sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var wsColor1 = (insrt[9] > 4) ? "white" : "#333";
                var wsColor2 = (insrt[9] > 4) ? "red" : "";


                var tip = "";

                tip = '<table class="tooltipTable" style="margin: 10px 0px; width: 320px;"><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[4] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[5] + '</td><td class="tooltpstyle" align="center" width="30 %"><span style="color: ' + wsColor1 + '; background: ' + wsColor2 + '; padding:0 5px"> BF' + insrt[9] + '</span></td></tr></table><table class="tooltipTable" style="margin - bottom: 20px; width:320px"><tr><td colspan="2" class="tooltpStyle" align="left" width="70 %" style="background: #dedede;"></td><td align="left" width="30 %" style="background: #dedede; color: #777; text - align: center;">NCR</td></tr><tr><td class="tooltpStyle" align="left" width="70 %">' + seriesName[0] + '</td><td align="left" width="30 %"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span></td><td align="right" style="background: #ffefef;color: #777">' + insrt[6] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[1] + '</span></td><td align="left" style="background: #efefef; color: #777"> </td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[2] + '</span></td><td align="right" style="background: #dfdfff; color: #777">' + insrt[7] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[3] + '</td><td align="left"><span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[3] + '</span></td > <td align="right" style="background: #ffefef; color: #777">' + insrt[8] + '</td></tr > <tr><td class="tooltpStyle" align="left"> LOG (Kts)</td><td align="left">' + insrt[10] + '</td><td align="right" style="background: #ffefef; color: #777"> </td></tr></table > <table class="tooltipTable" style="margin: 10px 0px; width: 100%;"><tr><td class="tooltpstyle" align="center" colspan="2" style="background: #dedede;">Formula</td></tr><tr><td class="tooltpstyle" align="center"><b>Avg. RPM (%)</b></td><td class="tooltpstyle" align="center">(AVG RPM x 100)/MCR RPM</td></tr><tr><td class="tooltpstyle" align="center"><b>NCR RPM (%)</b></td><td class="tooltpstyle" align="center">(NCR RPM x 100)/MCR RPM</td></tr></table';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 50
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },

        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 800,
            bottom: 0,
            itemGap: 30,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }, {
                  name: seriesName[2]
              }, {
                  name: seriesName[3]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }
        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            nameRotate: 90,
            min: minRPM,
            max: maxRPM,
            index: 0,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },

            type: 'value',
            splitLine: {
                show: true
            }
        }, {
            name: seriesName[1],
            nameLocation: 'center',
            nameRotate: 90,
            min: minGOVSet,
            max: maxGOVSet,
            index: 1,
            position: 'left',
            offset: 60,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },

            type: 'value',
            splitLine: {
                show: false
            }
        }, {
            name: seriesName[2],
            nameLocation: 'center',
            nameRotate: 90,
            min: powerMin, // 
            max: powerMax,
            index: 2,
            nameTextStyle: {
                padding: 35,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[2]
                }
            },

            type: 'value',
            splitLine: {
                show: false
            }
        }, {
            name: seriesName[3],
            nameLocation: 'center',
            nameRotate: 90,
            min: minFOC,
            max: maxFOC,
            index: 3,
            position: 'right',
            offset: 80,
            nameTextStyle: {
                padding: 25,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[3]
                }
            },

            type: 'value',
            splitLine: {
                show: false
            }
        }],
        series: [{
            name: seriesName[0],
            type: 'line',
            yAxisIndex: 0,
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[1]
        },
        {
            name: seriesName[2],
            type: 'line',
            yAxisIndex: 2,
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: ncrME
                }],
                label: {
                    normal: {
                        show: true,
                        position: 'middle',
                        formatter: ' NCR {c} BHP'
                    }
                },
                lineStyle: {
                    normal: {
                        color: colors[2]
                    }
                }
            },
            data: seriesData[2]
        },
        {
            name: seriesName[3],
            type: 'line',
            yAxisIndex: 3,
            data: seriesData[3]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState1 = myChartPerf;
      }
    );
}

// RPM
function mpchart02(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    var colors = ['#A3827B', '#2C9A57', '#265984'];
    //var dateRange = dateRangeOrg;
    var dateRange = dateRange1;
    var seriesName = ['Main Engine', 'Turbocharger 1', 'Turbocharger 2'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [105.7, 106, 105.7, 105.7, 104.7, 105.7, 105.7], // Main Engine
    //  [11.2, 11.2, 11.2, 11.2, 11.2, 11.2, 11.2], // Turbocharger 1
    //  [11.3, 11.3, 11.3, 11.3, 11.3, 11.3, 11.3], // Turbocharger 2
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //];

    var maxme = Math.ceil(Math.max.apply(null, seriesData[0]));
    var minme = Math.floor(Math.min.apply(null, seriesData[0]));

    var maxtc = Math.ceil(Math.max(Math.max.apply(null, seriesData[1]), Math.max.apply(null, seriesData[2])));
    var mintc = Math.floor(Math.min(Math.min.apply(null, seriesData[1]), Math.min.apply(null, seriesData[2])));

    var seriesUnit = ['RPM', 'RPM', 'RPM'];


    option = {
        title: {
            text: 'RPM Analysis',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Main Engine - Turbocharger 1 - Turbocharger 2',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 70,
            right: 80
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/


                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 100%"><tr><td class="tooltpstyle" align="center"><b>' + insrt[3] + '</b></td><td class="tooltpstyle" align="center">' + insrt[4] + '</td></tr></table><table class="tooltipTable" style="margin - bottom: 20px; width: 320px"><tr><td class="tooltpStyle" align="left">' + seriesName[0] + '</td><td align="left"><span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span></td></tr><tr><td colspan="2" class="tooltpStyle" align="left" style="background: #555; color: #eaeaea">Turbocharger RPM &nbsp;&nbsp;▼</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background:' + colors[1] + '; padding: 0 5px">' + insrt[1] + '</span></td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[2] + '</span></td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 40
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 600,
            bottom: 0,
            itemGap: 30,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }, {
                  name: seriesName[2]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: 'Main Engine',
            nameLocation: 'center',
            nameRotate: 90,
            min: minme,
            max: maxme,
            index: 0,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisLabel: {
                //color: colors[0]
                color: '#000'
            },
            axisPointer: {
               show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }, {
            name: 'Turbocharger (x1000)',
            nameLocation: 'center',
            nameRotate: 90,
            min: mintc,
            max: maxtc,
            index: 1,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            splitLine: {
                show: false
            }
        }],
        series: [{
            name: 'Main Engine',
            type: 'line',
            yAxisIndex: 0,
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#333',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
        {
            name: 'Turbocharger 1',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[1]
        },
        {
            name: 'Turbocharger 2',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[2]
        }
        ]
    };
    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          console.log(graphDiv);
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          // cpGraphState2 = myChartPerf;
          cpGraphState1a = myChartPerf;
      }
    );

}

// Temperature
function mpchart03(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#DE0D03', '#776B06', '#094549', '#97A302'];
    var dateRange = dateRange1;

    var seriesName = ['Engine Room Temp', 'Sea Water Temp', 'Scavange Air Temp', 'Fuel Oil Inlet Temp'];
    var seriesData = seriesData1;

    //var seriesData = [
    //  [41, 41, 41, 41, 41, 41, 41],
    //  [31, 31, 31, 31, 31, 31, 31],
    //  [40, 40, 40, 40, 40, 40, 40],
    //  [125, 125, 125, 125, 125, 125, 125],
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //];

    var maxer = Math.ceil(Math.max.apply(null, seriesData[0]));
    var miner = Math.floor(Math.min.apply(null, seriesData[0]));

    var maxsw = Math.ceil(Math.max.apply(null, seriesData[1]));
    var minsw = Math.floor(Math.min.apply(null, seriesData[1]));

    var maxsca = Math.ceil(Math.max.apply(null, seriesData[2]));
    var minsca = Math.floor(Math.min.apply(null, seriesData[2]));

    var maxfoi = Math.ceil(Math.max.apply(null, seriesData[3]));
    var minfoi = Math.floor(Math.min.apply(null, seriesData[3]));

    var seriesUnit = ['°C', '°C', '°C', '°C', '°C', '°C'];

    option = {
        title: {
            text: 'Temperature Analysis °C',
            subtext: 'ER Temp - SW Temp - Scav Air Temp - Fuel Oil Inlet Temp',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 60,
            bottom: 100,
            left: 110,
            right: 120
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[4] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[5] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Temperature</td></tr><tr><td class="tooltpStyle" align="left" width="70 %">' + seriesName[0] + '</td><td align="left" width="30 %"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[1] + '</span>' + seriesUnit[1] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background:' + colors[2] + '; padding: 0 5px">' + insrt[2] + '</span>' + seriesUnit[2] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[3] + '</td><td align="left"><span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[3] + '</span>' + seriesUnit[3] + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 40
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }, {
                  name: seriesName[2]
              }, {
                  name: seriesName[3]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            index: 0,
            nameRotate: 90,
            min: miner,
            max: maxer,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            type: 'value'
        }, {
            name: seriesName[1],
            nameLocation: 'center',
            index: 1,
            nameRotate: 90,
            min: minsw,
            max: maxsw,
            position: 'left',
            offset: 60,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            type: 'value',
            splitLine: {
                show: false
            }
        }, {
            name: seriesName[2],
            nameLocation: 'center',
            index: 2,
            nameRotate: 90,
            min: minsca,
            max: maxsca,
            position: 'right',
            offset: 0,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[2]
                }
            },
            type: 'value',
            splitLine: {
                show: false
            }
        }, {
            name: seriesName[3],
            nameLocation: 'center',
            index: 3,
            nameRotate: 90,
            min: minfoi,
            max: maxfoi,
            position: 'right',
            offset: 60,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[3]
                }
            },
            type: 'value',
            splitLine: {
                show: false
            }
        }],
        series: [{
            name: seriesName[0],
            type: 'line',
            yAxisIndex: 0,
            data: seriesData[0]
        },

        {
            name: seriesName[1],
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[1]
        },

        {
            name: seriesName[2],
            type: 'line',
            yAxisIndex: 2,
            data: seriesData[2]
        },

        {
            name: seriesName[3],
            type: 'line',
            yAxisIndex: 3,
            data: seriesData[3]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState3 = myChartPerf;
      }
    );

}

// Temperature 2nd
function mpchart03a(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#FE8A00', '#7D5F03'];
    var dateRange = dateRange1;

    var seriesName = ['Exhaust Temp max', 'Exhaust Temp min'];
    var seriesData = seriesData1;
    //var seriesData = [

    //  [340, 340, 340, 340, 340, 340, 340],
    //  [330, 330, 330, 330, 330, 330, 330],
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //];

    var maxma = Math.ceil(Math.max.apply(null, seriesData[0]));
    var minma = Math.floor(Math.min.apply(null, seriesData[0]));

    var maxmi = Math.ceil(Math.max.apply(null, seriesData[1]));
    var minmi = Math.floor(Math.min.apply(null, seriesData[1]));

    if (minma > minmi)
        minma = minmi;
    if (maxma < maxmi)
        maxma = maxmi;

    var seriesUnit = ['°C', '°C', '°C', '°C', '°C', '°C'];

    option = {
        title: {
            text: 'Exhaust Temperature Analysis °C',
            subtext: 'M/E Maximum & Minimum Exhaust Temperature.',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 60,
            bottom: 100,
            left: 60,
            right: 50
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Exhaust Temperature</td></tr><tr><td class="tooltpStyle" align="left">Max</td><td align="left"><span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">Min</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span>' + seriesUnit[1] + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 40
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            index: 0,
            nameRotate: 90,
            max: maxma,
            min: minma,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            type: 'value'
        }, {
            name: "",
            nameLocation: 'center',
            index: 1,
            nameRotate: 90,
            max: maxma,
            min:minma,
            position: 'right',
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            type: 'value',
            splitLine: {
                show: false
            }
        }],
        series: [{
            name: seriesName[0],
            type: 'line',
            yAxisIndex: 0,
            data: seriesData[0]
        },

        {
            name: seriesName[1],
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[1]
        }
        ]
    };

    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          //alert(graphFilDivName);
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState3a = myChartPerf;
      }
    );

}

// Pressure
function mpchart04(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    var colors = ['#265984', '#FFC565', '#97A302']; //'#2C9A57',
   // var dateRange = dateRangeOrg;
    var dateRange = dateRange1;
    var seriesName = ['Sea Water Pressure (Bar)', 'Scavange Air Pressure (Bar)', 'Fuel oil Inlet Pressure (Bar)'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [1.4, 1.4, 1.4, 1.4, 1.4, 1.4, 1.4],
    //  [1.3, 1.3, 1.3, 1.3, 1.3, 1.3, 1.3],
    //  [24.5, 24.5, 24.5, 24.5, 24.8, 24.5, 24.5],
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //];

    var maxFuel = Math.ceil(Math.max(Math.max.apply(null, seriesData[2])));
    var maxwater = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));

    if (maxFuel < 14)
        maxFuel = 14

    var seriesUnit = ['Bars', 'Bars', 'Bars'];


    option = {
        title: {
            text: 'Pressure Analysis (Bar)',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Sea Water  -  Scavenge Air  -  Fuel Oil',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 60,
            right: 60
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/


                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[3] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[4] + '</td></tr></table><table class="tooltipTable" style="margin - bottom: 20px; width: 320px"><tr> <td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Pressure Analysis</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">' + seriesName[0] + '</td><td align="left" width="30 %"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span> ' + seriesUnit[0] + '</td></tr><tr> <td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: #333; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span> ' + seriesUnit[1] + '</td></tr><tr> <td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[2] + '</span> ' + seriesUnit[2] + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 40
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 800,
            bottom: 0,
            itemGap: 30,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }, {
                  name: seriesName[2]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
           // data: ['21-Jan-2018', '22-Jan-2018', '23-Jan-2018', '24-Jan-2018', '25-Jan-2018', '26-Jan-2018', '27-Jan-2018']
        },
        yAxis: [{
            name: 'Fuel Oil (Bar)',
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
           max: maxFuel,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }, {
            name: 'Sea Water, Scavange Air (Bar)',
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
            max: maxFuel,
            index: 1,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value',
            splitLine: {
                show: false
            }
        },
        {
            type: "value",
            min: 0,
            max: maxFuel,
           // name: seriesName[1],
            //nameLocation: "center",
            //nameRotate: 90,
            //nameTextStyle: {
            //    padding: 30,
            //    fontWeight: 700,
            //    fontSize: 14
            //},
            splitArea: {
                show: true,
                areaStyle: {
                    color: [
                     "rgba(0,0,0,0)",
                     "rgba(0,0,0,0)",
                     "rgba(0,150,0,0.2)",
                     "rgba(0,150,0,0.2)"
                    ]
                }
            }
        }
        ],
        series: [{
            name: seriesName[0],
            type: 'line',
            yAxisIndex: 1,
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            type: 'line',
            yAxisIndex: 1,
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[1]
        },
        {
            name: seriesName[2],
            type: 'line',
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[2]
        },
        {
            type: 'line',
            itemStyle: {
                normal: {
                    opacity: 0
                }
            },
            lineStyle: {
                normal: {
                    opacity: 0
                }
            },
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: 12
                }],
                label: {
                    normal: {
                        show: true,
                        formatter: 'Fuel Oil Pressure - Working Range (6-12Bars)',
                        position: 'middle'
                    }
                },
                lineStyle: {
                    normal: {
                        color: 'rgba(0,80,0,0.8)'
                    }
                }
            }
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState4 = myChartPerf;
      }
    );
}


// Electrical Plant
function mpchart05(graphDiv, seriesData1, dateRange1) {

    var option = null;
    //		var colors = ['#F9290C', '#B55C4F', '#00FF7C', '#5D8B41', '#E1F707', '#898B41', '#0A4DF2', '#41558B'];
    // var colors = ['#FF5967', 'rgba(0,0,0,0.11)', '#00FF7C', 'rgba(0,0,0,0.12)', '#E1F707', 'rgba(0,0,0,0.11)', '#188FCC', 'rgba(0,0,0,0.12)'];
    var colors = ['#FF5967', '#00FF7C', '#E1F707', '#188FCC'];
    var dateRange = dateRange1;

    //		var seriesName = ['A/E No 1(kW)', 'A/E No 1(hr)', 'A/E No 2(kW)', 'A/E No 2(hr)', 'A/E No 3(kW)', 'A/E No 3(hr)', 'A/E No 4(kW)', 'A/E No 4(hr)'];

    let maxLoad = 700; // Max load data from Vessel Master
    var seriesName = ['A/E No 1(kW)', 'A/E No 2(kW)', 'A/E No 3(kW)', 'A/E No 4(kW)'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [200, 350, 400, 300, 550, 200, 400], // A/E No 1(kW)  0
    //  [10, 10, 24, 24, 5, 5, 10], // A/E No 1(hr)   1
    //  [200, 350, 0, 300, 0, 0, 300], // A/E No 2(kW)   2
    //  [24, 24, 0, 24, 0, 0, 24], // A/E No 2(hr)  3
    //  [0, 0, 400, 300, 550, 200, 0], // A/E No 3(kW)  4
    //  [0, 0, 10, 24, 23, 23, 0], // A/E No 3(hr)  5
    //  [0, 0, 0, 0, 550, 300, 0], //  A/E No 4(kW)  6
    //  [0, 0, 0, 0, 20, 20, 0], //  A/E No 4(hr)  7
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status 8
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition 9
    //  [], // Max load data from Vessel Master - AE1 (10)
    //  [], // Max load data from Vessel Master - AE2 (11)
    //  [], // Max load data from Vessel Master - AE3 (12)
    //  [] // Max load data from Vessel Master - AE4 (13)
    //];

    //function checkGen(g1, g2, g3, g4)

    var seriesUnit = ['kW', 'hr', 'kW', 'hr', 'kW', 'hr', 'kW', 'hr'];

    //seriesData[0].forEach(function (itm, idx, arr) {
    //    seriesData[10][idx] = maxLoad - seriesData[0][idx];
    //    seriesData[11][idx] = maxLoad - seriesData[2][idx];
    //    seriesData[12][idx] = maxLoad - seriesData[4][idx];
    //    seriesData[13][idx] = maxLoad - seriesData[6][idx];
    //});
    maxLoad = Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[4]), Math.max.apply(null, seriesData[6]));

    let kwMaxLimit = maxLoad + (maxLoad * 0.1);


    option = {

        title: {
            text: 'Electrical Plant Load & Hours',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'A/E1 - A/E2 - A/E3 - A/E4',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        legend: {
            data: seriesName,
            bottom: 0
        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 70,
            right: 80
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var insrtSum = parseFloat(insrt[0]) + parseFloat(insrt[2]) + parseFloat(insrt[4]) + parseFloat(insrt[6]);

                var tip = "";
                tip = '<table class="tooltipTable" style="width: 320px"><tr><td class="tooltpstyle" align="center" width="40%"><b>' + insrt[8] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[9] + '</td></tr></table><table class="tooltipTable" style="margin - bottom:10px; width: 320px"><tr><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width=""></td><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">Hrs of <br>Operation</td><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">Actual<br>Load</td><td class="tooltpStyle" align="center" style="background: #888; color: #fff" width="">Maximum<br>Capacity</td></tr><tr><td class="tooltpStyle" align="left">A/E 1</td><td align="right">' + insrt[1] + ' ' + seriesUnit[1] + '</td><td class="tooltpStyle" align="right"><span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span> ' + seriesUnit[0] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[10] + ' kW</td></tr><tr><td class="tooltpStyle" align="left">A/E 2</td><td align="right">' + insrt[3] + ' ' + seriesUnit[3] + '</td><td class="tooltpStyle" align="right"><span style="color: black;background: ' + colors[1] + '; padding: 0 5px">' + insrt[2] + '</span> ' + seriesUnit[2] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[11] + ' kW</td></tr><tr><td class="tooltpStyle" align="left">A/E 3</td><td align="right">' + insrt[5] + ' ' + seriesUnit[5] + '</td><td class="tooltpStyle" align="right"><span style="color: #333;background: ' + colors[2] + '; padding: 0 5px">' + insrt[4] + '</span> ' + seriesUnit[4] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[12] + ' kW</td></tr><tr><td class="tooltpStyle" align="left">A/E 4</td><td align="right">' + insrt[7] + ' ' + seriesUnit[7] + '</td><td class="tooltpStyle" align="right"><span style="color: white;background: ' + colors[3] + '; padding: 0 5px">' + insrt[6] + '</span> ' + seriesUnit[6] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[13] + ' kW</td></tr><tr><td class="tooltpStyle" colspan="2" align="left" style="background: #bdbdbd">Total Load</td><td class="tooltpStyle" colspan="2" align="right" style="background: #ddd;padding - right: 100px;">' + insrtSum + ' kW</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 60
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],
        xAxis: [

          {
              type: 'category',
              data: dateRange,
              splitLine: {
                  show: true,
                  lineStyle: {
                      type: 'dashed',
                      color: '#777'
                  }
              },
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          }

        ],

        yAxis: [{
            name: 'Load (kW)',
            nameLocation: 'center',
            min: 0,
            max: kwMaxLimit,
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        },
        {
            min: 0,
            axisLabel: {
                show: false
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            max: kwMaxLimit,
            index: 1
        }
        ],
        series: [

          {
              name: seriesName[0],
              type: 'bar',
              label: {
                  normal: {
                      //show: true,
                      rotate: 90,
                      distance: 15,
                      color: '#000',
                      align: 'left',
                      verticalAlign: 'middle',
                      position: 'insideBottom',
                      formatter: '{c} kW'
                  }
              },
              // stack: seriesName[0],
              barGap: '0%',
              data: seriesData[0]
          },

          // {
          //   type: 'bar',
          //   silent: true,

          //   //stack: seriesName[0],
          //   data: [maxLoad, maxLoad, maxLoad, maxLoad, maxLoad, maxLoad, maxLoad]
          // },
          {
              name: seriesName[1],
              type: 'bar',
              label: {
                  normal: {
                      //show: true,
                      rotate: 90,
                      distance: 15,
                      color: '#333',
                      align: 'left',
                      verticalAlign: 'middle',
                      position: 'insideBottom',
                      formatter: '{c} kW'
                  }
              },
              stack: seriesName[1],
              barGap: '5%',
              data: seriesData[2]
          },
          // {
          //   type: 'bar',
          //   silent: true,
          //   stack: seriesName[1],
          //   barGap: '5%',
          //   data: seriesData[11]
          // },
          {
              name: seriesName[2],
              type: 'bar',
              label: {
                  normal: {
                      //show: true,
                      rotate: 90,
                      distance: 15,
                      color: '#000',
                      align: 'left',
                      verticalAlign: 'middle',
                      position: 'insideBottom',
                      formatter: '{c} kW'
                  }
              },
              stack: seriesName[2],
              barGap: '5%',
              data: seriesData[4]
          },
          // {
          //   type: 'bar',
          //   silent: true,
          //   stack: seriesName[2],
          //   barGap: '5%',
          //   data: seriesData[12]
          // },
          {
              name: seriesName[3],
              type: 'bar',
              label: {
                  normal: {
                      //show: true,
                      rotate: 90,
                      distance: 15,
                      color: '#000',
                      align: 'left',
                      verticalAlign: 'middle',
                      position: 'insideBottom',
                      formatter: '{c} kW'
                  }
              },
              stack: seriesName[3],
              barGap: '5%',
              data: seriesData[6]
          },
          // {
          //   type: 'bar',
          //   silent: true,
          //   stack: seriesName[3],
          //   barGap: '5%',
          //   data: seriesData[13]
          // },
          {
              type: 'line',
              itemStyle: {
                  normal: {
                      opacity: 0
                  }
              },
              lineStyle: {
                  normal: {
                      opacity: 0
                  }
              },
              markLine: {
                  data: [{
                      name: 'Markline between two points',
                      yAxis: maxLoad
                  }],
                  label: {
                      normal: {
                          show: true,
                          formatter: ' \n{c} kW\nMaximum\nCapacity\nof each\nGenerator'
                      }
                  },
                  lineStyle: {
                      normal: {
                          color: 'blue'
                      }
                  }
              }
          }
        ]
    };



    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState5 = myChartPerf;
      }
    );
}

// ::::::::::::  Resistance Limit Capture ::::::::::::::

// ::::::::::::      Resistance Limit ENDS       ::::::::::::::

// Electrical Plant 2nd
function mpchart05a(graphDiv, seriesData1, dateRange1) {

    var option = null;

    var colors = ['#5D8B41', '#0A4DF2', '#B55C4F'];
    var dateRange = dateRange1;

    var seriesName = ['440 Volts', '220/110 Volts'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //  [2.15, 2.30, 2.25, 2.15, 1.97, 1.90, 1.96], // new field in DNR R L1 (2)
    //  [1.96, 1.26, 0.86, 2.00, 1.90, 1.96, 1.93], // new field in DNR S L2 (3)
    //];

    let maxResLimit = Math.max(Math.max(...seriesData[2]), Math.max(...seriesData[3]));
    let maxResLimitData = maxResLimit + maxResLimit * 0.1;


    option = {

        title: {
            text: 'Main Switchboard 3Phase Earth Fault Monitor Readings',
            textStyle: {
                color: '#C2417C'
            },
            subtext: '440 Volts - 220/110 Volts',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        legend: {
            data: seriesName,
            bottom: 0
        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 80,
            right: 80
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var tip = "";
                tip = '<table class="tooltipTable" style="width: 100%"><tr><td class="tooltpstyle" align="center" width="40%"><b>' + insrt[0] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[1] + '</td></tr></table><table class="tooltipTable" style="margin - bottom:10px; width: 200px"><tr><td class="tooltpStyle" align="left" style="background: #555; color: #fff" width="">Earth Fault Monitor</td><td class="tooltpStyle" align="left" style="background: #555; color: #fff" width=""> Resistance </td></tr><tr><td class="tooltpStyle" align="left">440 Volts </td><td align="left"><span style="color: #fff; background: ' + colors[0] + '; padding: 0 5px">' + insrt[2] + '</span> MΩ</td></tr><tr><td class="tooltpStyle" align="left">220/110 Volts</td><td align="left"><span style="color: #fff; background:' + colors[1] + '; padding: 0 5px">' + insrt[3] + '</span> MΩ</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 60
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],
        xAxis: [

          {
              type: 'category',
              data: dateRange,
              splitLine: {
                  show: true,
                  lineStyle: {
                      type: 'dashed'
                  }
              },
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          }

        ],

        yAxis: [{
            name: 'Resistance  (MΩ)',
            nameLocation: 'center',
            min: 0,
            max: maxResLimitData,
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            type: 'bar',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                    // formatter: '{c} MΩ'
                }
            },
            barGap: '0%',
            data: seriesData[2]
        },
        {
            name: seriesName[1],
            type: 'bar',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            barGap: '0%',
            data: seriesData[3]
        },
        {
            type: 'line',
            itemStyle: {
                normal: {
                    opacity: 0
                }
            },
            lineStyle: {
                normal: {
                    opacity: 0
                }
            },
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: resLimitVal // resistance limit applied to the marker line.
                }],
                label: {
                    normal: {
                        show: true,
                        formatter: ' \n{c} MΩ\nResistance\nLimit'
                    }
                },
                lineStyle: {
                    normal: {
                        color: 'red'
                    }
                }
            }
        }
        ]
    };



    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState5a = myChartPerf;
      }
    );
}

//cpGraphState5a.legend.dispatch.on('legendClick', onLegendClick);

function onLegenedClick() {
    console.log('Clicked Legend');
}

// Purifier
function mpchart06(graphDiv, seriesData1, dateRange1) {

    var option = null;
    // var colors = ['#00cc7a', '#DBA700', '#265984', '#a02046', '#008fb3']; //cccc00
    var colors = ['#00cc7a', '#DBA700', '#265984', '#FF5967', '#008fb3']; //cccc00
    var dateRange = dateRange1;

    var seriesName = ['HFO Purifier 1 (Hrs)', 'HFO Purifier 2 (Hrs)', 'DO Purifier (Hrs)', 'M/E LO Purifier (Hrs)', 'A/E LO Purifier (Hrs)'];
    var seriesData = seriesData1;

    var max = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1]), Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4])));
    var min = Math.floor(Math.min(Math.min.apply(null, seriesData[0]), Math.min.apply(null, seriesData[1]), Math.min.apply(null, seriesData[2]), Math.min.apply(null, seriesData[3]), Math.min.apply(null, seriesData[4])));

    //var seriesData = [
    //  [24, 24, 24, 0, 0, 0, 0],
    //  [0, 0, 0, 24, 24, 24, 24],
    //  [0, 0, 0, 0, 0, 0, 0],
    //  [24, 24, 0, 0, 24, 24, 24],
    //  [0, 0, 0, 12, 24, 6, 0],
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //];

    option = {
        title: {
            text: 'Purifier Operation (Hrs)',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'HFO Purifier 1 - HFO Purifier 2 - DO Purifier - M/E LO Purifier - A/E LO Purifier',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 60,
            right: 50
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width:280px" ><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[5] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[6] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width:280px"> <tr><td class="tooltpstyle" align="center" style="background: #aeaeae;"></td><td class="tooltpstyle" align="center" style="background: #aeaeae; color: white;">Hours Of Operation</td></tr><tr><td class="tooltpStyle" align="left" >' + seriesName[0] + '</td><td align="center" ><span style="color: white;background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span> hr</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="center"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span> hr</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="center"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[2] + '</span> hr</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[3] + '</td><td align="center"><span style="color: white; background:' + colors[3] + '; padding: 0 5px">' + insrt[3] + '</span> hr</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[4] + '</td><td align="center"><span style="color: white; background: ' + colors[4] + '; padding: 0 5px">' + insrt[4] + '</span> hr</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 55
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            splitLine: {
                show: true,
                lineStyle: {
                    type: 'dashed',
                    color: '#0002FF'
                }
            }
        },
        yAxis: [{
            name: 'Hours',
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
           
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLabel: {
                show: true
            },
            type: 'value'
        }, {
            //name: 'Hours',
            nameLocation: 'center',
            index: 1,
            nameRotate: 90,
            nameTextStyle: {
                padding: 0,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                   // show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: 'HFO 1 - {c}'
                }
            },
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    //show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: 'HFO 2 - {c}'
                }
            },
            data: seriesData[1]
        },
        {
            name: seriesName[2],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    //show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: 'DO - {c}'
                }
            },
            data: seriesData[2]
        },
        {
            name: seriesName[3],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    //show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: 'M/E LO - {c}'
                }
            },
            data: seriesData[3]
        },
        {
            name: seriesName[4],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                   // show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: 'A/E LO - {c}'
                }
            },
            data: seriesData[4]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState6 = myChartPerf;
      }
    );
}


// Fine Filter
function mpchart07(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    var colors = ['#8BB883', '#1D663A']; //'#FFC565', '#2C9A57', '#B55D09'
    var dateRange = dateRange1;

    var seriesName = ['Counter', 'No. Of Operations/Day'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [40629, 40641, 40653, 40665, 40677, 40689, 40701], // M/E FO - Counter
    //  [12, 16, 14, 15, 12, 11, 12], // M/E FO - no. of operations perday
    //  ['At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //  [14, 12, 16, 20, 14, 24, 20], //fullspeed info from DNR
    //  [0, 6, 8, 1, 4, 0, 2] // reduced Speed info from DNR
    //];

   

    

    //var maxCounter = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxhour = Math.ceil(Math.max(Math.max.apply(null, seriesData[1])));

    var mincounter = 999999999;

    seriesData[0].forEach(function (itm, idx, arr) {
        if (seriesData[0][idx] != "_" && Number(seriesData[0][idx]) < mincounter && Number(seriesData[0][idx]) > 0)
            mincounter = Number(seriesData[0][idx]);
    });

    if (mincounter == 999999999)
        mincounter = 0;


    option = {
        title: {
            text: 'Auto-Back Wash Fine Filters Operation Per Day',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'M/E Fuel Oil Auto-Back Wash Filter',
            subtextStyle: {
                color: "BF6D92",
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 70,
            right: 80
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var totalTime = Number(insrt[4]) + Number(insrt[5]);
                var avgOpRate = insrt[6]; //Math.round((insrt[1] / totalTime) * 24);

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr><table class="tooltiptable" style="margin - bottom: 20px; width: 320px;"><tr><td class="tooltpstyle" align="center" style="background:#555; color: #fff" colspan="2">M/E Fuel Oil Auto Filter</td></tr><tr><td class="tooltpstyle" align="center" style="background: #aeaeae;"></td><td class="tooltpstyle" align="center" style="background: #aeaeae;">Hours Of Operation</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">Counter</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: #fff; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">No. Of Backflush<br>Operation</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span></td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding:5px;">Average Operation/Day</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px">' + avgOpRate + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 55
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            filterMode: 'empty',
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {
            data: seriesName,
            bottom: 0
        },
        xAxis: [

          {
              type: 'category',
              data: dateRange,
              splitLine: {
                  show: true,
                  lineStyle: {
                      type: 'dashed'
                  }
              },
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          }

        ],
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            nameRotate: 90,
            min: mincounter,
           // max: maxCounter,
            index: 0,
            nameTextStyle: {
                padding: 35,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            type: 'value'
        }, {
            name: seriesName[1],
            nameLocation: 'center',
            nameRotate: 90,
            min: 1,
            //max: maxhour,
            //scale:true,
            index: 1,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            splitLine: {
                show: false
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            yAxisIndex: 0,
            start:mincounter,
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
        //				   { // name: seriesName[1], // yAxisIndex: 0, // type: 'bar', // barGap: '0', // label: { // normal: { // show: true, // distance: 15, // color: '#000', // align: 'center', // verticalAlign: 'middle', // position: 'insideBottom', // formatter: '{c}' // } // }, // data: seriesData[1] // },
        {
            name: seriesName[1],
            yAxisIndex: 1,
            type: 'line',
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[1]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState7 = myChartPerf;
      }
    );
}

// Fine Filter 2nd
function mpchart07a(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    // var colors = ['#E8B33F', '#E87206', '#2C9A57', '#1D663A']; 
    var colors = ['#E8B33F', '#E87206', '#2C9A57', '#1D663A'];
    var dateRange = dateRange1;

    var seriesName = ['Counter', 'No. Of Operations/Day'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [3008, 3016, 3020, 3024, 3028, 3032, 3036], // M/E LO - Counter
    //  [8, 4, 4, 6, 7, 5, 4], // M/E LO - no. of backflush operations perday
    //  ['At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //  [24, 24, 24, 24, 24, 24, 20], //fullspeed info from DNR
    //  [0, 0, 0, 0, 0, 0, 4] // reduced Speed info from DNR
    //];

    //var counterMinVal = Math.round(Math.min(...seriesData[0]) - (Math.min(...seriesData[0]) * 0.1));
    //var counterMaxVal = Math.round(Math.max(...seriesData[0]) + (Math.max(...seriesData[0]) * 0.06));

    var maxCounter = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxhour = Math.ceil(Math.max(Math.max.apply(null, seriesData[1])));


    option = {
        title: {
            text: 'Auto-Back Wash Fine Filters Operation Per Day',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'M/E Lubricating Oil Auto-Back Wash Filter',
            subtextStyle: {
                color: "BF6D92",
                fontSize: 15
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100,
            left: 70,
            right: 80
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var totalTime = Number(insrt[4]) + Number(insrt[5]);
                var avgOpRate = insrt[6];//Math.round((insrt[1] / totalTime) * 24);

                var test = insrt[4] + "-" + insrt[5] + "-" + totalTime + "-" + avgOpRate;

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr><table class="tooltiptable" style="margin - bottom: 20px; width: 320px;"><tr><td class="tooltpstyle" align="center" style="background:#555; color: #fff" colspan="2">M/E LO Auto Filter</td></tr><tr><td class="tooltpstyle" align="center" style="background: #aeaeae;"></td><td class="tooltpstyle" align="center" style="background: #aeaeae;">Hours Of Operation</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">Counter</td><td class="tooltpstyle" align="center" style="background: #dedede;padding: 5px"><span style="color: #fff; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">No. Of Backflush<br>Operation</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span></td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding:5px;">Average Operation/Day</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px">' + avgOpRate + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 55
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 1,
            end: 100,
            realtime: true,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {
            data: seriesName,
            bottom: 0
        },
        xAxis: [

          {
              type: 'category',
              data: dateRange,
              splitLine: {
                  show: true,
                  lineStyle: {
                      type: 'dashed'
                  }
              },
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          }

        ],
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
            max: maxCounter,
            index: 0,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            type: 'value'
        }, {
            name: seriesName[1],
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
            max: maxhour,
            index: 1,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            splitLine: {
                show: false
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            yAxisIndex: 0,
            type: 'bar',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            yAxisIndex: 1,
            min: 0,
            max: 12,
            type: 'line',
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[1]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        }
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState7a = myChartPerf;
      }
    );
}


// Fresh Water Generator
function mpchart08(graphDiv, seriesData1, dateRange1) {

    var option = null;
    //var colors = ['#7413E8', '#0592E8', '#0A48FF', '#FF7E96'];
    var colors = ['#105187', '#C33325', '#F19722', '#2C8693'];
    var dateRange = dateRange1;

    var seriesName = ['Previous ROB (MT)', 'Total FW Produced (MT)', 'Total FW Consumed (MT)', 'Current ROB (MT)']; // 'Domestic Fresh Water', 'Drinking Water', 'Boiler Water', 'Tank Cleaning Water'];
    var seriesData = seriesData1;
    //var seriesData = [

    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  0
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  1

    //  [50, 70, 0, 0, 0, 0, 0], // Domestic Fresh Water - Previous ROB (MT)				2
    //  [50, 60, 40, 30, 40, 70, 60], // Domestic Fresh Water - Produced (MT) 	 	3
    //  [30, 40, 0, 0, 62, 0, 0], // Domestic Fresh Water - Consumption (MT)	 4
    //  [70, 90, 70, 70, 0, 62, 60], // Domestic Fresh Water - ROB (MT) 		 5

    //  [10, 0, 0, 0, 0, 0, 0], // Drinking Water - Previous ROB (MT)				6
    //  [10, 20, 10, 40, 20, 10, 10], // Drinking Water - Produced (MT) 		7
    //  [0, 0, 0, 0, 0, 0, 0], // Drinking Water - Consumption (MT) 	  		8
    //  [0, 0, 0, 0, 0, 0, 0], // Drinking Water - ROB (MT) 			  				9

    //  [50, 0, 0, 0, 0, 0, 0], // Boiler Water - Previous ROB (MT)					10
    //  [0, 0, 0, 0, 0, 0, 0], // Boiler Water - Produced (MT) 		  				11
    //  [0, 0, 0, 0, 0, 0, 0], // Boiler Water - Consumption (MT) 		  		12
    //  [0, 0, 0, 0, 0, 0, 0], // Boiler Water - ROB (MT) 			  					13

    //  [20, 20, 20, 10, 10, 20, 20], // Tank Cleaning Water - Previous ROB (MT)		14
    //  [0, 0, 0, 0, 30, 10, 0], // Tank Cleaning Water - Produced (MT) 	  	15
    //  [0, 0, 0, 10, 20, 0, 10], // Tank Cleaning Water - Consumption (MT)  	16
    //  [20, 20, 10, 10, 20, 20, 10], // Tank Cleaning Water - ROB (MT) 		  			17

    //  [0, 0, 0, 0, 0, 0, 0], // Total Previous ROB 		18
    //  [0, 0, 0, 0, 0, 0, 0], // Total Produced  			19
    //  [0, 0, 0, 0, 0, 0, 0], // Total Consumption  		20
    //  [0, 0, 0, 0, 0, 0, 0] // Total ROB   					  21
    //];

    seriesData[2].forEach(function (itm, indx, arr) {
        seriesData[18][indx] = Number(seriesData[2][indx]) + Number(seriesData[6][indx]) + Number(seriesData[10][indx]) + Number(seriesData[14][indx]);
        seriesData[19][indx] = Number(seriesData[3][indx]) + Number(seriesData[7][indx]) + Number(seriesData[11][indx]) + Number(seriesData[15][indx]);
        seriesData[20][indx] = Number(seriesData[4][indx]) + Number(seriesData[8][indx]) + Number(seriesData[12][indx]) + Number(seriesData[16][indx]);
        seriesData[21][indx] = Number(seriesData[5][indx]) + Number(seriesData[9][indx]) + Number(seriesData[13][indx]) + Number(seriesData[17][indx]);
    });

    var maxProd = 100;

    option = {
        title: {
            text: 'Fresh Water Generator',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Water Inventory (MT)',
            subtextStyle: {
                fontSize: 15,
                color: '#bf6d92'
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="font-size: 10px; margin-bottom: 10px; width: 100%"><tr><td class="tooltpstyle" align="center"><b>' + insrt[0] + '</b></td><td class="tooltpstyle" align="center">' + insrt[1] + '</td></tr></table> <table class="tooltipTable" style="font-size: 10px; margin-bottom: 10px; text-align: center"> <tr> <td align="left" rowspan="2" class="tooltpStyle" align="left" style="background: #ddd; border-bottom: 1px solid #aaa;">Tank</td><td align="center" colspan="4" style="background: #777; color: #fff">Water Inventory</td></tr><tr> <td align="center" style="background: #ddd;border-bottom: 1px solid #aaa;">Prev. ROB <br>(MT)</td><td align="center" style="background: #ddd; border-bottom:1px solid #aaa;">Produced <br>(MT)</td><td align="center" style="background: #ddd; border-bottom:1px solid #aaa;">Consumed <br>(MT)</td><td align="center" style="background: #ddd; border-bottom: 1px solid #aaa;">ROB <br>(MT)</td></tr><tr> <td class="tooltpStyle" align="left">Domestic FW</td><td>' + insrt[2] + '</td><td>' + insrt[3] + '</td><td>' + insrt[4] + '</td><td>' + insrt[5] + '</td></tr><tr> <td class="tooltpStyle" align="left">Drinking Water</td><td>' + insrt[6] + '</td><td>' + insrt[7] + '</td><td>' + insrt[8] + '</td><td>' + insrt[9] + '</td></tr><tr> <td class="tooltpStyle" align="left">Boiler Water</td><td>' + insrt[10] + '</td><td>' + insrt[11] + '</td><td>' + insrt[12] + '</td><td>' + insrt[13] + '</td></tr><tr> <td class="tooltpStyle" align="left">Tank Cleaning Water</td><td>' + insrt[14] + '</td><td>' + insrt[15] + '</td><td>' + insrt[16] + '</td><td>' + insrt[17] + '</td></tr><tr> <td class="tooltpStyle" align="left" style="background: #ccd; border-top: 1px solid #aab;">Total</td><td style="background:#ccd; border-top: 1px solid #aab;"><span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[18] + '</span></td><td style="background: #ccd; border-top: 1px solid #aab;"> <span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[19] + '</span> </td><td style="background: #ccd;border-top: 1px solid #aab;"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[20] + '</span></td><td style="background: #ccd; border-top: 1px solid #aab;"> <span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[21] + '</span> </td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 50
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {
            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: "Water Produced or Consumed (MT)",
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        },
        {
            //name: "Water Consumed (MT/day)",
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            type: 'bar',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            barGap: '1%',
            //barWidth: 35,
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: maxProd
                }],
                label: {
                    normal: {
                        show: true,
                        position: 'end',
                        formatter: 'FWG\nProduction\nCapacity\n{c} MT'
                    }
                },
                lineStyle: {
                    normal: {
                        color: "red"
                    }
                }
            },
            data: seriesData[18]
        },
        {
            name: seriesName[1],
            type: 'bar',
            //barWidth: 50,
            stack: true,
            label: {
                normal: {
                   // show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[19]
        },
        {
            name: seriesName[2],
            type: 'bar',
            //barWidth: 50,
            stack: true,
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[20]
        },
        {
            name: seriesName[3],
            type: 'bar',
            bargap: '1%',
            //barWidth: 35,
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[21]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        },
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState8 = myChartPerf;
      }
    );
}




// ::::::::  Triger Point setting ::::::

// Polpulate Serise Data with expected consumption and feed rate, using radio button values

var pwr = 0;
var consump = 0;
var conRateLimit = trigPointForm.trigPoint.value; //----►   0.9 | 1.0 | 1.1
var fullSpd = 0; //----►  Full speed time in hours from DNR


// function for Cylinder Oil Consumption graph
function popuplateLOdata(seriesData) {

    var result = seriesData[4];

    var vallll = trigPointForm.trigPoint.value; //value of the radio button
    pwr = seriesData[3];
    fullSpd = seriesData[2];
   
    seriesData[4].forEach(function (item, index, arr) {
        consump = Number(seriesData[4][index]) + Number(seriesData[6][index]);
        seriesData[9].push(calcFR(pwr[index], consump)); //// feed rate 
        seriesData[10].push(calcEC(pwr[index], trigPointVal, fullSpd[index])); /// expected consumption
    });
}

// function for Crankcase Oil Feed Rate & EC graph
function popuplateFeedRateLOdata(seriesData) {
    pwr = seriesData[3];
    fullSpd = seriesData[2];

    seriesData[4].forEach(function (item, index, arr) {
        seriesData[6].push(calcFR(pwr[index], item)); //// feed rate 
        seriesData[7].push(calcEC(pwr[index], trigPointVal, fullSpd[index])); /// expected consumption
    });
}


// Calculation of Feed Rate
function calcFR(pwr, consump) {
    var retval = 0;
    if (Number(pwr) != 0 && Number(consump) != 0)
        retval = ((Number(consump) * 950) / (24 * Number(pwr))).toFixed(2);
    return retval;
}


// Calculation of Expected Consumption
function calcEC(pwr, conRate, fullSpd) {
    return ((Number(pwr) * Number(conRate) * Number(fullSpd)) / 950).toFixed(2);
}



// :::::::::::::::::::::::::::::::::::::

// Lube Oil
function mpchart09(graphDiv, seriesData1, dateRange1) {

    var option = null;
    // var colors = ['#08E880', '#13B8FF', '#E8403D', '#B897E8']; //'#0631FF', '#FF800D';
    var colors = ['#FF6F01', '#F5E301', '#00649B', '#B897E8']; //'#399B45', '#0631FF', '#FF800D';
    var dateRange = dateRange1;


    var seriesName = ['M/E CO Consumption (Ltr/Day)', 'M/E CO Low TBN (Ltr/Day)', 'Recommended Consumption (Ltr/Day)', 'M/E CO Feed Rate (g/BHP h)'];
    var seriesData = seriesData1;
    //var seriesData = [

    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (0)
    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  (1)

    //  [24, 23, 24, 24, 24, 23, 24], // Full Speed from DNR  (2)

    //  [7092, 7333, 7351, 6835, 7094, 6796, 7197], // Power output from DNR  (3)

    //  [210, 205, 100, 0, 130, 140, 158], // ME CO Consumption fron DNR (4)
    //  [16950, 17100, 17250, 17410, 17450, 17400, 17500], // ME CO Consumption ROB (5)

    //  [0, 0, 60, 100, 0, 0, 60], // ME CO Low TBN  (6)
    //  [14660, 14660, 14660, 14660, 14660, 14660, 14660], // ME Cylinder Oil Low TBN - ROB  (7)

    //  [0, 0, 0, 0, 0, 0, 0], // Consumption Rate Limit - use radio button to populate.   (8)

    //  [], // Feed Rate - populate using calcFR  (9)
    //  [], // Calculated Expected Consumption - populate using calcEC() (10)

    //  [0, 0, 1, 1, 0, 0, 1], // ECA TRANSIT from DNR  (11)
    //  [2.60, 2.70, 2.45, 2.83, 2.50, 2.30, 2.10], // Sulphur Content % from DNR  (12)

    //  [16848, 16950, 17100, 17250, 17410, 17450, 17400], // CO Previous ROB  (13)
    //  [14660, 14660, 14660, 14560, 14560, 14560, 14560] // Low TBN Previous ROB  (14)

    //];


    popuplateLOdata(seriesData);

    // ::::::::::::::::::::::::::: End of calculations to populate values for FEED RATE and  EXPECTED CONSUMPTION :::::::::::::::


    // setting the max value for the graph

    var totComsump = Number(Math.max(Math.max.apply(null, seriesData[4]))) + Number(Math.max(Math.max.apply(null, seriesData[6])));
    var hghConsumpt = Math.round(Math.max(Math.max.apply(null, seriesData[9]), Math.max.apply(null, seriesData[10])));
    var hghConsump = Math.round(Math.max(totComsump, hghConsumpt));
    // maximim between Actual and Low TBN values.
    var maxConsump = Math.ceil(Number(hghConsump) + Number(hghConsump) * 0.1);

    var frLimit = Math.max(...seriesData[9]);

    if (frLimit < 1.1)
        frLimit = 1.1
    var maxFRLimit = frLimit + frLimit * 0.1;

    option = {
        title: {
            text: 'Cylinder Lubricating Oil',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Consumption & Inventory (Ltr/Day)',
            subtextStyle: {
                fontSize: 15,
                color: '#BF6D92'
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/

                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var ecaTag = (Number(insrt[11])) ? '<td class="tooltpstyle"  align="center"><span style="background: red; color: white; padding: 0px 7px; border - radius: 4px;">ECA TRANSIT</span></td>' : '';

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 0px; width:100%"> <tr> <td class="tooltpstyle" align="center"> <b>' + insrt[0] + '</b> </td><td class="tooltpstyle" align="center"><b>' + insrt[1] + '</b></td>' + ecaTag + '</tr></table> <table class="tooltipTable" style="margin - bottom: 0px; text - align: center; width: 100%"> <tr> <td class="tooltpStyle">Engine Power</td><td>' + insrt[3] + ' BHP</td><td class="tooltpStyle" style="background: #ffefef; color: #777">Sulphur Content</td><td align="center" style="background:#ffdfdf; color: #555; font - weight: 700">' + insrt[12] + '% </td></tr></table> <table class="tooltipTable" style="margin - bottom:10px; text - align: center;"> <tr> <td class="tooltpStyle" style="background: #dedede; color: #777; text - align: left;">Oil Type</td><td class="tooltpStyle" style="background: #dedede; color: #777; text - align: center;">M/E <br>Cylinder Oil</td><td class="tooltpStyle" style="background: #dedede; color: #777; text - align: center;">M/E CO <br>Low TBN</td></tr><tr> <td class="tooltpStyle" align="left">Previous ROB (Ltr)</td><td>' + insrt[13] + '</td><td>' + insrt[14] + '</td></tr><tr> <td class="tooltpStyle" align="left">Consumption (Ltr/Day)</td><td> <span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[4] + '</span> </td><td> <span style="color: #333; background: ' + colors[1] + '; padding: 0 5px">' + insrt[6] + '</span> </td></tr><tr> <td class="tooltpStyle" align="left">ROB at noon (Ltr)</td><td>' + insrt[5] + '</td><td>' + insrt[7] + '</td></tr><tr> <td class="tooltpStyle" align="left">Recommended Consumption (Ltr/Day)</td><td colspan="2"><span style="color: white; background:' + colors[2] + '; padding: 0 5px">' + insrt[10] + '</span> </td></tr><tr> <td class="tooltpStyle" align="left">Calculated Feed Rate (g/BHP h)</td><td colspan="2"><span style="color: #333; background: ' + colors[3] + '; padding: 0 5px">' + insrt[9] + '</span></td></tr><tr> <td class="tooltpStyle" align="left">Recommended Feed Rate (g/BHP h)</td><td colspan="2">' + trigPointVal + '</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 55
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: [{
                name: seriesName[0]
            },

            {
                name: seriesName[1]
            },
            {
                name: seriesName[2]
            },
            {
                name: seriesName[3]
            }
            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: "M/E CO Consumption (Ltr/Day)",
            max: maxConsump,
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        },
        {
            //name: 'Expected Consumption (Ltr/Day)',
            max: maxConsump,
            index: 1,
            label: {
                normal: {
                    show: false
                }
            },
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                color: colors[2],
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        },
        {
            name: 'M/E CO Feed Rate (g/BHP h)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                color: '#9076B5',
                fontWeight: 700,
                fontSize: 14
            },
            max: maxFRLimit,
            index: 2,
            splitLine: {
                show: false
            },
            axisLabel: {
                show: true
            },
            axisTick: {
                show: true

            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: '#9076B5'
                }
            },
            type: 'value'
        }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: '0%',
            stack: true,
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[4]
        },
        {
            name: seriesName[1],
            type: 'bar',
            bargap: '0%',
            stack: true,
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[6]
        },
        {
            name: seriesName[2],
            type: 'line',
            lineStyle: {
                type: 'dashed'
            },
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    color: colors[2],
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[10]
        },
        {
            name: seriesName[3],
            type: 'bar',
            //barWidth: 30,
            bargap: '0%',
            yAxisIndex: 2,
            label: {
                normal: {
                   // show: true,
                    distance: 15,
                    rotate: 90,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[9]
        },
        {
            type: 'line',
            itemStyle: {
                normal: {
                    opacity: 0
                }
            },
            yAxisIndex: 2,
            lineStyle: {
                normal: {
                    opacity: 0
                }
            },
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: trigPointVal
                }],
                label: {
                    normal: {
                        show: true,
                        position: 'middle',
                        formatter: '{c} g/BHP h Recommended Feed Rate\n'
                    }
                },
                lineStyle: {
                    normal: {
                        color: 'blue'
                    }
                }
            }
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        },
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState9 = myChartPerf;
      }
    );
}


// Lube Oil 2nd - Crankcase Oil
function mpchart09a(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#FF605A', '#524FFF', '#E8403D']; //'#0631FF'];
    var dateRange = dateRange1;


    var seriesName = ['Crankcase Oil ROB (Ltr)', 'Expected Feed Rate (g/BHP h)'];
    var seriesData = seriesData1;
    //var seriesData = [

    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (0)
    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  (1)

    //  [14600, 14600, 14600, 14600, 14600, 14600, 14600], // ME Crankcase Oil - Prev ROB  (2)
    //  [600, 0, 0, 0, 0, 100, 0], // ME Crankcase Oil   (3)
    //  [14000, 14000, 14000, 14000, 14000, 13900, 13900], // ME Crankcase Oil - ROB  (4)
    //  [20, 20, 20, 20, 20, 20, 20], // ME Crankcase Oil - Capacity  (m3) (5) 
    //];


    // setting the max value for the graph

    var minROB = Number(Math.min(Math.min.apply(null, seriesData[4]))) - Number(Math.min(Math.min.apply(null, seriesData[4]))) * 0.05;
    var maxROB = Number(Math.max(Math.max.apply(null, seriesData[4]))) + Number(Math.max(Math.max.apply(null, seriesData[4]))) * 0.05;
    var maxcapacity = Math.max(Math.max.apply(null, seriesData[5]));
    if (maxROB < maxcapacity)
        maxROB = maxcapacity;
    if (minROB > maxcapacity)
        minROB = maxcapacity;

    option = {
        title: {
            text: 'M/E Crankcase Lubricating Oil',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Consumption & Inventory (Ltr/Day)',
            subtextStyle: {
                fontSize: 16,
                color: '#BF6D92'
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 80,
            bottom: 100
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpstyle" align="center"><b>' + insrt[0] + '</b></td><td class="tooltpstyle" align="center"><b>' + insrt[1] + '</b></td></tr></table><table class="tooltipTable" style="margin - bottom:10px;"> <tr> <td class="tooltpStyle" align="center" colspan="2" style="background: #777; color: white">Crankcase Oil</td></tr><tr> <td class="tooltpStyle" align="left">Previous ROB</td><td align="right"> ' + insrt[2] + ' Ltr</td></tr><tr> <td class="tooltpStyle" align="left">Consumption</td><td align="right">' + insrt[3] + ' Ltr/Day</td></tr><tr> <td class="tooltpStyle" align="left">ROB</td><td align="right"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[4] + '</span> Ltr</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 55
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {


            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: [{
                name: seriesName[0]
            },

            {
                name: seriesName[1]
            }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
        },
        yAxis: [{
            name: seriesName[0],
            min: minROB,
            max: maxROB,
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 50,
                fontWeight: 700,
                fontSize: 14
            },
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    //show: true,
                    distance: 15,
                    color: '#fff',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[4]
           }
           ,
        {
            type: 'line',
            itemStyle: {
                normal: {
                    opacity: 0
                }
            },
            lineStyle: {
                normal: {
                    opacity: 0
                }
            },
            markLine: {
                data: [{
                    name: 'Markline between two points',
                    yAxis: maxcapacity
                }],
                label: {
                    normal: {
                        show: true,
                        position: 'middle',
                        formatter: '{c} M/E Oil Sump Tank Capacity'
                    }
                },
                lineStyle: {
                    normal: {
                        color: 'blue'
                    }
                }
            }
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        },
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState9a = myChartPerf;
      }
    );
}


//Bilge Graph
function mpchart10(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#FFC565', '#E86914'];
    var dateRange = dateRange1;

    var seriesName = ['Bilge Tank ROB (M3)', 'Sludge Tank ROB (M3)'];
    //var seriesData = [
    //  [15, 14, 13, 12, 15, 13, 14], // (0)
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (1)
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition   (2)
    //  [7, 7, 8, 9, 8, 7, 8], // Bilge Tank ROB cu.M   (3)
    //  [9, 8, 10, 7, 9, 8, 7] // Sludge  ROB cu.M    (4)
    //];

    var seriesData = seriesData1;

    var totBilgeCap = Math.max(Math.max.apply(null, seriesData[5]));  // value from components (if more than one available, then use sum of the capacity)
    var totSludgeCap = Math.max(Math.max.apply(null, seriesData[6]));  // value from components 

    var bilgeMax = Math.ceil(Math.max(totBilgeCap, Math.max.apply(null, seriesData[3])))+4;
    var sludgeMax = Math.ceil(Math.max(totSludgeCap, Math.max.apply(null, seriesData[4])))+4;



    option = {
        title: {
            text: 'Bilge & Sludge',
            textStyle: {
                color: '#C2417C'
            },
            left: 'center'

        },
        color: colors,
        grid:
          [
            {
                left: 90,
                right: 80,
                height: '33%'
            },
            {
                top: '50%',
                left: 90,
                right: 80,
                height: '33%',
                bottom: 100
            }
          ],
        axisPointer: {
            link: { xAxisIndex: 'all' }
        },

        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/

                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; "><tr><td class="tooltpstyle" align="center"><b>' + insrt[1] + '</b></td><td class="tooltpstyle" align="center" >' + insrt[2] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; "><tr><td class="tooltpStyle" align="left" width="70%">' + seriesName[0] + '</td><td align="right" width="30%"><span style="color: black; background: ' + colors[0] + '; padding: 0 5px">' + insrt[3] + '</span> M³</td></tr><tr><td class="tooltpStyle" align="left" width="70%">' + seriesName[1] + '</td><td align="right" width="40px"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[4] + '</span> M³</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 160
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 120;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        toolbox: {
            feature: {
                restore: {
                    show: true,
                    title: 'Refresh'
                },
                saveAsImage: {
                    show: true,
                    title: 'Save'
                }
            }
        },

        dataZoom:
          [
            {
                bottom: 30,
                type: 'slider',
                show: 'true',
                start: 0,
                end: 100,
                realtime: true,
                xAxisIndex: [0,1],
                dataBackground: {
                    areaStyle: {
                        color: '#7ECFF2'
                    }
                },
                fillerColor: 'rgba(78,119,166,0.2)',
                handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
                handleSize: '70%',
                handleStyle: {
                    color: '#3D7C7F'
                }
            }
          ],

        legend:
        {
            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        xAxis: [
          {
              type: 'category',
              data: dateRange,
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          },
          {
              type: 'category',
              gridIndex: 1,
              //position: 'top',
              data: dateRange,
              axisPointer: {
                  show: true,
                  label: {
                      show: true
                  }
              },
          }

        ],
        yAxis:
          [
            {
                name: seriesName[0],
                nameLocation: 'center',
                nameRotate: 90,
                max: bilgeMax,
                nameTextStyle:
                {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                axisPointer: {
                    show: true,
                    label: {
                        show: true
                    }
                },
                type: 'value'
            },
            {
                name: seriesName[1],
                nameLocation: 'center',
                gridIndex: 1,
                index: 1,
                nameRotate: 90,
                max: sludgeMax,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                axisPointer: {
                    show: true,
                    label: {
                        show: true
                    }
                },
                // inverse: true,
                type: 'value'
            }
          ],
        series:
          [
            {
                name: seriesName[0],
                type: 'bar',
                label: {
                    normal: {
                       // show: true,
                        distance: 15,
                        color: '#333',
                        align: 'center',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c} M3'
                    }
                },
                markLine: {
                    data: [{
                        name: ' Total Bilge Tank Capacity',
                        yAxis: totBilgeCap
                    }],
                    label: {
                        normal: {
                            show: true,
                            position: 'end',
                            formatter: ' \n{c} M3\nTotal Bilge\nCapacity'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: "blue"
                        }
                    }
                },
                data: seriesData[3]
            },
            {
                name: seriesName[1],
                type: 'bar',
                xAxisIndex: 1,
                yAxisIndex: 1,
                label: {
                    normal: {
                        //show: true,
                        distance: 15,
                        color: '#fff',
                        align: 'center',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c} M3'
                        // formatter: '{c} M³'
                    }
                },
                markLine: {
                    data: [{
                        name: totSludgeCap+' Markline between two points',
                        yAxis: totSludgeCap
                    }],
                    label: {
                        normal: {
                            show: true,
                            position: 'end',
                            formatter: ' \n{c} M3\nTotal Sludge\nCapacity'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: "blue"
                        }
                    }
                },
                data: seriesData[4]
            }
          ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        },
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState10 = myChartPerf;
      }
    );
}


// Auxiliary Boiler
function mpchart11(graphDiv) {

    var option = null;
    var colors = ['#FFC565', '#FF800F', '#0F3B66'];
    var dateRange = dateRangeOrg;

    var seriesName = ['Boiler FO Consumption (MT/day)', 'Boiler Water Consumption (MT/day)', 'Ship Speed (Kts)'];
    var seriesData = [
      [1, 1.5, 1, 0, 0, 0.5, 0], // Boiler Fuel Oil Consumtion MT  (0)
      [1, 1, 3, 0, 0, 2, 0], // Boiler Water Consumption  MT   (1)
      [13, 12, 14, 13, 13, 12, 12], // Ship Speed Kts  - EM Log Speed fron DNR   (2)
      ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status   (3)
      ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition   (4)
      [6, 10, 19, 20, 21, 20, 17], // Ship Full Seep hours from DNR, to be shown in the tooltip   (5)
      [0.5, 0, 0, 0, 0, 0, 0], // Fuel Consumed for Cargo Heating   (6)
      [0, 1, 0, 0, 0, 0, 0], // Fuel Consumed for tank Cleaning    (7)
      [0, 0, 1, 0, 0, 0, 0] // Fuel consumed for inerting   (8)
    ];


    var spdMax = Math.round(Math.max(...seriesData[2]) + Math.max(...seriesData[2]) * 0.1);
    var hghConsump = (Math.max(...seriesData[0]) > Math.max(...seriesData[1]) ? Math.max(...seriesData[0]) : Math.max(...seriesData[1]));
    var maxConsump = (hghConsump + hghConsump * 0.1);


    option = {
        title: {
            text: 'Auxiliary Boiler',
            subtext: 'Fuel Consumption - Feed Water Consumption',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                fontSize: 15,
                color: '#BF6D92'
            },
            left: 'center'

        },
        color: colors,
        grid: [{
            top: 60,
            bottom: 100
        }],
        tooltip: {
            trigger: 'axis',
            triggerOn: "click",
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {

                /*---------------------Code for ballast and air sea---------------------------------*/


                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                /*----------------------------------------------------------------------------------*/

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 100%" ><tr><td class="tooltpstyle" align="center" width="40 %"> <b> ' + insrt[3] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[4] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"><tr> <td class="tooltpStyle" align="left" width="70 %"> Boiler FO Consumption </td><td align="left" width="30 %"> <span style="color: black; background:' + colors[0] + '; padding: 0 3px">' + insrt[0] + '</span> MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %"> Boiler Water Consumption </td><td align="left" width="30 %"> <span style="color: black; background:' + colors[1] + '; padding: 0 3px">' + insrt[1] + '</span> MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %"> Ship&apos;s Speed</td><td align="left" width="30 %"> <span style="color: white; background:' + colors[2] + '; padding: 0 3px">' + insrt[2] + '</span> Kts</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Full Speed Hours</td><td align="left" width="30 %">' + insrt[5] + ' Hrs</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Cargo Heating</td><td align="left" width="30 %">' + insrt[6] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Tank Cleaning</td><td align="left" width="30 %">' + insrt[7] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Inerting</td><td align="left" width="30 %">' + insrt[8] + ' MT/day</td></tr></table>';

                return tip;
            },

            backgroundColor: '#ecf0f1',
            borderColor: 'black',
            padding: 5,
            backgroundColor: 'rgba(245, 245, 245, 0.9)',
            borderWidth: 2,
            borderColor: '#999',
            textStyle: {
                color: '#000'
            },

            position: function (pos, params, el, elRect, size) {
                var obj = {
                    top: 50
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
                return obj;
            },

            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 1,
            end: 100,
            dataBackground: {
                areaStyle: {
                    color: '#7ECFF2'
                }
            },
            fillerColor: 'rgba(78,119,166,0.2)',
            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
            handleSize: '70%',
            handleStyle: {
                color: '#3D7C7F'
            }
        }],

        legend: {

            left: 'center',
            width: 1200,
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            //				[	{
            //						name: seriesName[2],
            //						icon: 'image://assets//img/mainEngine/mainEngine_mtd.svg'
            //					},
            //				],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: ['21-Jan-2018', '22-Jan-2018', '23-Jan-2018', '24-Jan-2018', '25-Jan-2018', '26-Jan-2018', '27-Jan-2018']
        },
        yAxis: [{
            name: seriesName[0],
            nameLocation: 'center',
            nameRotate: 90,
            max: maxConsump,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            type: 'value'
        }, {
            name: seriesName[2],
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
            max: spdMax,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            splitLine: {
                show: false
            },
            type: 'value'
        }],
        series: [{
            name: seriesName[0],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    rotate: 90,
                    color: '#333',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c} MT'
                }
            },
            barGap: '0%',
            data: seriesData[0]
        }, {
            name: seriesName[1],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    rotate: 90,
                    color: '#333',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c} MT'
                }
            },
            barGap: '0%',
            data: seriesData[1]
        },
        {
            name: seriesName[2],
            type: 'line',
            yAxisIndex: 1,
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}Kts'
                }
            },
            data: seriesData[2]
        }
        ]
    };


    require.config({
        paths: {
            echarts3: '../js/echartsAll3'
        },
    });

    require(
      ['echarts3'],
      function (ec) {
          var graphFilDivName = graphDiv + "Graph";
          var graphFilDiv = document.getElementById(graphFilDivName);
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState11 = myChartPerf;
      }
    );
}