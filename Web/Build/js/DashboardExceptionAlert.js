//*****************Machinery Parameters Graph********************
//******************************************************************************

var tabNum = 1;
var cpGraphState1 = {};
//var cpGraphState1a = {};
var cpGraphState2 = {};
//var cpGraphState3 = {};
var cpGraphState3a = {};
var cpGraphState4 = {};
var cpGraphState5 = {};
var cpGraphState6 = {};
var cpGraphState7 = {};
var cpGraphState7a = {};
var cpGraphState8 = {};
var cpGraphState9 = {};
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



$(window).resize(function () {
    //cpGraphState.resize();
    resetGraph(tabNum);
});

function resetGraph(num) {

    switch (num) {
        case 1:
            cpGraphState1.resize();
            //cpGraphState1a.resize();
            break;

        case 2:
           cpGraphState2.resize();
            break;

        case 3:
            //cpGraphState3.resize();
            cpGraphState3a.resize();
            break;

        case 4:
            cpGraphState4.resize();
            break;

        case 5:
            cpGraphState5.resize();
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
// Auxiliary Engine Load
function mpchart01(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#FFDAB9', '#008000'];

    var seriesName = ['Total Capacity Available', 'Total Capacity Utilized', 'Total Hours'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
    var seriesDataMax1 = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
    //seriesDataMax = Math.ceil(seriesDataMax * 1.7);
    var maxhours = Math.ceil(Math.max(Math.max.apply(null, seriesData[23])));
   // maxhours = Math.ceil(maxhours * 1.7);

    option = {
        title: {
            text: "A/E Load at Sea",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }


                tip = '<table class="tooltipTable" width="600px" ><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  LightOrange">•</span>' + seriesName[0] + '</td><td> ' + insrt[0] + 'kWh</td><td></td></tr><tr><td><span class="tlTip tomatoOrange">•</span>' + seriesName[1] + '</td><td> ' + insrt[1] + ' kWh</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[2] + '</td><td> ' + insrt[23] + ' hrs</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>Load %</td><td> ' + insrt[2] + '%</td><td></td></tr></table><table class="tooltipTable"><tr><td></td><td><b>AE1</b></td><td><b>AE2</b></td><td><b>AE3</b></td></tr><tr><td><span class="tlTip oliveGreen">•</span>Max Load (kW)</td><td> ' + insrt[3] + '</td><td> ' + insrt[8] + '</td><td> ' + insrt[13] + '</td><td></td></tr><tr><td><span class="tlTip milkyEmerald">•</span>AE Load (kW)</td><td> ' + insrt[6] + '</td><td> ' + insrt[11] + '</td><td> ' + insrt[16] + '</td><td></td></tr><tr><td><span class="tlTip darkTeal">•</span>AE Hrs Run</td><td> ' + insrt[5] + '</td><td> ' + insrt[10] + '</td><td> ' + insrt[15] + '</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>AE Cap Available (kWh)</td><td> ' + insrt[4] + '</td><td> ' + insrt[9] + '</td><td> ' + insrt[14] + '</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>AE Load Utilized (kWh)</td><td> ' + insrt[7] + '</td><td> ' + insrt[12] + '</td><td> ' + insrt[17] + '</td><td></td></tr></table>';
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
                    top: 23
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
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: 'kwh',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxhours,
                
                min: 0,
                splitLine: {
                    show: false
                },
                name: 'hrs',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[1],
                show: false,
                type: 'bar',
                stack: true,
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[1]
            },
            {
                name: seriesName[0],
                type: 'bar',
                barGap: 0,
                stack: true,
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[24]
            },
            {
                name: seriesName[2],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[2],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[23]
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
//Dashboard Common Allert
function mpchart04(graphDiv, seriesdata, dateseries) {
    var app1 = {};

    var option = null;

    var colors = ['#E76322'];



    var seriesData = seriesdata;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var seriesDataMin = Math.floor(Math.min.apply(null, seriesData[0]));

    var measure = seriesData[3][0];
    var unit = seriesData[4][0];
    if (unit == "&mu;m")
        unit = "μm";

    if (unit == "degree")
        unit = "°C";

    if (seriesDataMin > 0)
        seriesDataMin = 0;
    option = {
        title: {
            text: measure,
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var valtag = insrt[1] == "1" ? '<td class="tooltpstyle"  align="center"><span style="background: red; color: white; padding: 0px 7px; border - radius: 4px;"> ' + insrt[0] + ' ' + unit + '</span></td>' : '<td class="tooltpstyle"  align="center"><span style="background: green; color: white; padding: 0px 7px; border - radius: 4px;"> ' + insrt[0] + ' ' + unit + '</span></td>';

                tip = '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td><td class="tooltpStyle"><span class="pad20">' + insrt[6] + '</span></td><td class="tooltpStyle"><span class="pad20">' + insrt[5] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  tomatoOrange">•</span>' + insrt[3] + '</td>' + valtag + '</tr></table>';
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
                    top: 23
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
        //legend: {
        //    data: seriesName,
        //    bottom: 0
        //},
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            }
        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: seriesDataMin,
            name: unit,
            index: 0,
            nameLocation: 'center',
            axisLine: { onZero: true },
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
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },     
        }
        ],
        series: [{
            name: measure,
            type: 'bar',
            barGap: 0,
            label: {
                normal: {
                    show: false,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            index: 0,
            data: seriesData[0]
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

//cpGraphState5a.legend.dispatch.on('legendClick', onLegendClick);

function onLegenedClick() {
    console.log('Clicked Legend');
}

// Fine Filter
function mpchart07(graphDiv, seriesData1, dateRange1) {

    //var app1 = {};

    var option = null;
    var colors = ['#8BB883', '#1D663A','#E8B33F']; //'#FFC565', '#2C9A57', '#B55D09'
    var dateRange = dateRange1;

    var seriesName = ['Counter', 'No. Of Operations/Day','Avg Operation/Day'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [40629, 40641, 40653, 40665, 40677, 40689, 40701], // M/E FO - Counter
    //  [12, 16, 14, 15, 12, 11, 12], // M/E FO - no. of operations perday
    //  ['At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status
    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition
    //  [14, 12, 16, 20, 14, 24, 20], //fullspeed info from DNR
    //  [0, 6, 8, 1, 4, 0, 2], // reduced Speed info from DNR
    //  [13,14,15,12,10,24,20] // Avg
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

    var avgcounter = Math.ceil(Math.max(Math.max.apply(null, seriesData[6])));


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
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr><table class="tooltiptable" style="margin - bottom: 20px; width: 320px;"><tr><td class="tooltpstyle" align="center" style="background:#555; color: #fff" colspan="2">M/E Fuel Oil Auto Filter</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">Counter</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: #fff; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">No. Of Backflush<br>Operation</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span></td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding:5px;">Average Operation/Day</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px">' + avgOpRate + '</td></tr></table>';

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
        },
        {
            name: seriesName[2],
            yAxisIndex: 1,
            type: 'line',
            Color: colors[2],
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
    var colors = ['#E8B33F', '#E87206', '#8BB883'];
    var dateRange = dateRange1;

    var seriesName = ['Counter', 'No. Of Operations/Day', 'Avg Operation/Day'];
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
                var avgOpRate = insrt[6];// Math.round((insrt[1] / totalTime) * 24);

                var test = insrt[4] + "-" + insrt[5] + "-" + totalTime + "-" + avgOpRate;

                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr><td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr><table class="tooltiptable" style="margin - bottom: 20px; width: 320px;"><tr><td class="tooltpstyle" align="center" style="background:#555; color: #fff" colspan="2">M/E LO Auto Filter</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">Counter</td><td class="tooltpstyle" align="center" style="background: #dedede;padding: 5px"><span style="color: #fff; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding: 5px;">No. Of Backflush<br>Operation</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span></td></tr><tr><td class="tooltpstyle" align="left" style="background: #fff; padding:5px;">Average Operation/Day</td><td class="tooltpstyle" align="center" style="background: #dedede; padding: 5px">' + avgOpRate + '</td></tr></table>';

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
        },
        {
            name: seriesName[2],
            yAxisIndex: 1,
            type: 'line',
            Color: colors[2],
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


//// ::::::::  Triger Point setting ::::::

//// Polpulate Serise Data with expected consumption and feed rate, using radio button values

//var pwr = 0;
//var consump = 0;
//var conRateLimit = trigPointForm.trigPoint.value; //----►   0.9 | 1.0 | 1.1
//var fullSpd = 0; //----►  Full speed time in hours from DNR


//// function for Cylinder Oil Consumption graph
//function popuplateLOdata(seriesData) {

//    var result = seriesData[4];

//    var vallll = trigPointForm.trigPoint.value; //value of the radio button
//    pwr = seriesData[3];
//    fullSpd = seriesData[2];
   
//    seriesData[4].forEach(function (item, index, arr) {
//        consump = Number(seriesData[4][index]) + Number(seriesData[6][index]);
//        seriesData[9].push(calcFR(pwr[index], consump)); //// feed rate 
//        seriesData[10].push(calcEC(pwr[index], trigPointVal, fullSpd[index])); /// expected consumption
//    });
//}

//// function for Crankcase Oil Feed Rate & EC graph
//function popuplateFeedRateLOdata(seriesData) {
//    pwr = seriesData[3];
//    fullSpd = seriesData[2];

//    seriesData[4].forEach(function (item, index, arr) {
//        seriesData[6].push(calcFR(pwr[index], item)); //// feed rate 
//        seriesData[7].push(calcEC(pwr[index], trigPointVal, fullSpd[index])); /// expected consumption
//    });
//}


//// Calculation of Feed Rate
//function calcFR(pwr, consump) {
//    var retval = 0;
//    if (Number(pwr) != 0 && Number(consump) != 0)
//        retval = ((Number(consump) * 950) / (24 * Number(pwr))).toFixed(2);
//    return retval;
//}


//// Calculation of Expected Consumption
//function calcEC(pwr, conRate, fullSpd) {
//    return ((Number(pwr) * Number(conRate) * Number(fullSpd)) / 950).toFixed(2);
//}



//// :::::::::::::::::::::::::::::::::::::

//// Lube Oil
//function mpchart09(graphDiv, seriesData1, dateRange1) {

//    var option = null;
//    // var colors = ['#08E880', '#13B8FF', '#E8403D', '#B897E8']; //'#0631FF', '#FF800D';
//    var colors = ['#FF6F01', '#F5E301', '#00649B', '#B897E8']; //'#399B45', '#0631FF', '#FF800D';
//    var dateRange = dateRange1;


//    var seriesName = ['M/E CO Consumption (Ltr/Day)', 'M/E CO Low TBN (Ltr/Day)', 'Recommended Consumption (Ltr/Day)', 'M/E CO Feed Rate (g/BHP h)'];
//    var seriesData = seriesData1;
//    //var seriesData = [

//    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (0)
//    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  (1)

//    //  [24, 23, 24, 24, 24, 23, 24], // Full Speed from DNR  (2)

//    //  [7092, 7333, 7351, 6835, 7094, 6796, 7197], // Power output from DNR  (3)

//    //  [210, 205, 100, 0, 130, 140, 158], // ME CO Consumption fron DNR (4)
//    //  [16950, 17100, 17250, 17410, 17450, 17400, 17500], // ME CO Consumption ROB (5)

//    //  [0, 0, 60, 100, 0, 0, 60], // ME CO Low TBN  (6)
//    //  [14660, 14660, 14660, 14660, 14660, 14660, 14660], // ME Cylinder Oil Low TBN - ROB  (7)

//    //  [0, 0, 0, 0, 0, 0, 0], // Consumption Rate Limit - use radio button to populate.   (8)

//    //  [], // Feed Rate - populate using calcFR  (9)
//    //  [], // Calculated Expected Consumption - populate using calcEC() (10)

//    //  [0, 0, 1, 1, 0, 0, 1], // ECA TRANSIT from DNR  (11)
//    //  [2.60, 2.70, 2.45, 2.83, 2.50, 2.30, 2.10], // Sulphur Content % from DNR  (12)

//    //  [16848, 16950, 17100, 17250, 17410, 17450, 17400], // CO Previous ROB  (13)
//    //  [14660, 14660, 14660, 14560, 14560, 14560, 14560] // Low TBN Previous ROB  (14)

//    //];


//    popuplateLOdata(seriesData);

//    // ::::::::::::::::::::::::::: End of calculations to populate values for FEED RATE and  EXPECTED CONSUMPTION :::::::::::::::


//    // setting the max value for the graph

//    var totComsump = Number(Math.max(Math.max.apply(null, seriesData[4]))) + Number(Math.max(Math.max.apply(null, seriesData[6])));
//    var hghConsumpt = Math.round(Math.max(Math.max.apply(null, seriesData[9]), Math.max.apply(null, seriesData[10])));
//    var hghConsump = Math.round(Math.max(totComsump, hghConsumpt));
//    // maximim between Actual and Low TBN values.
//    var maxConsump = Math.ceil(Number(hghConsump) + Number(hghConsump) * 0.1);

//    var frLimit = Math.max(...seriesData[9]);

//    var maxFRLimit = frLimit + frLimit * 0.1;

//    option = {
//        title: {
//            text: 'Cylinder Lubricating Oil',
//            textStyle: {
//                color: '#C2417C'
//            },
//            subtext: 'Consumption & Inventory (Ltr/Day)',
//            subtextStyle: {
//                fontSize: 15,
//                color: '#BF6D92'
//            },
//            left: 'center'

//        },
//        color: colors,
//        grid: [{
//            top: 80,
//            bottom: 100
//        }],
//        tooltip: {
//            trigger: 'axis',
//            axisPointer: {
//                type: 'shadow'
//            },
//            formatter: function (params) {

//                /*---------------------Code for ballast and air sea---------------------------------*/

//                var insrt = [];
//                var result = params[0].dataIndex;

//                for (var i in seriesData) {
//                    var obj = seriesData[i];
//                    insrt[i] = obj[result];
//                }
//                /*----------------------------------------------------------------------------------*/

//                var ecaTag = (Number(insrt[11])) ? '<td class="tooltpstyle"  align="center"><span style="background: red; color: white; padding: 0px 7px; border - radius: 4px;">ECA TRANSIT</span></td>' : '';

//                var tip = "";
//                tip = '<table class="tooltipTable" style="margin-bottom: 0px; width:100%"> <tr> <td class="tooltpstyle" align="center"> <b>' + insrt[0] + '</b> </td><td class="tooltpstyle" align="center"><b>' + insrt[1] + '</b></td>' + ecaTag + '</tr></table> <table class="tooltipTable" style="margin - bottom: 0px; text - align: center; width: 100%"> <tr> <td class="tooltpStyle">Engine Power</td><td>' + insrt[3] + ' BHP</td><td class="tooltpStyle" style="background: #ffefef; color: #777">Sulphur Content</td><td align="center" style="background:#ffdfdf; color: #555; font - weight: 700">' + insrt[12] + '% </td></tr></table> <table class="tooltipTable" style="margin - bottom:10px; text - align: center;"> <tr> <td class="tooltpStyle" style="background: #dedede; color: #777; text - align: left;">Oil Type</td><td class="tooltpStyle" style="background: #dedede; color: #777; text - align: center;">M/E <br>Cylinder Oil</td><td class="tooltpStyle" style="background: #dedede; color: #777; text - align: center;">M/E CO <br>Low TBN</td></tr><tr> <td class="tooltpStyle" align="left">Previous ROB (Ltr)</td><td>' + insrt[13] + '</td><td>' + insrt[14] + '</td></tr><tr> <td class="tooltpStyle" align="left">Consumption (Ltr/Day)</td><td> <span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[4] + '</span> </td><td> <span style="color: #333; background: ' + colors[1] + '; padding: 0 5px">' + insrt[6] + '</span> </td></tr><tr> <td class="tooltpStyle" align="left">ROB at noon (Ltr)</td><td>' + insrt[5] + '</td><td>' + insrt[7] + '</td></tr><tr> <td class="tooltpStyle" align="left">Recommended Consumption (Ltr/Day)</td><td colspan="2"><span style="color: white; background:' + colors[2] + '; padding: 0 5px">' + insrt[10] + '</span> </td></tr><tr> <td class="tooltpStyle" align="left">Calculated Feed Rate (g/BHP h)</td><td colspan="2"><span style="color: #333; background: ' + colors[3] + '; padding: 0 5px">' + insrt[9] + '</span></td></tr><tr> <td class="tooltpStyle" align="left">Recommended Feed Rate (g/BHP h)</td><td colspan="2">' + trigPointVal + '</td></tr></table>';

//                return tip;
//            },

//            backgroundColor: '#ecf0f1',
//            borderColor: 'black',
//            padding: 5,
//            backgroundColor: 'rgba(245, 245, 245, 0.9)',
//            borderWidth: 2,
//            borderColor: '#999',
//            textStyle: {
//                color: '#000'
//            },

//            position: function (pos, params, el, elRect, size) {
//                var obj = {
//                    top: 55
//                };
//                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
//                return obj;
//            },

//            shared: true,
//            extraCssText: 'width: auto; height: auto'
//        },
//        toolbox: {
//            feature: {
//                restore: {
//                    show: true,
//                    title: 'Refresh'
//                },
//                saveAsImage: {
//                    show: true,
//                    title: 'Save'
//                }
//            }
//        },
//        dataZoom: [{
//            bottom: 30,
//            type: 'slider',
//            show: 'true',
//            start: 0,
//            end: 100,
//            dataBackground: {
//                areaStyle: {
//                    color: '#7ECFF2'
//                }
//            },
//            fillerColor: 'rgba(78,119,166,0.2)',
//            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
//            handleSize: '70%',
//            handleStyle: {
//                color: '#3D7C7F'
//            }
//        }],

//        legend: {


//            left: 'center',
//            width: 1200,
//            bottom: 0,
//            itemGap: 30,
//            data: [{
//                name: seriesName[0]
//            },

//            {
//                name: seriesName[1]
//            },
//            {
//                name: seriesName[2]
//            },
//            {
//                name: seriesName[3]
//            }
//            ],
//            textStyle: {
//                fontWeight: 'bold'
//            }

//        },
//        xAxis: {
//            type: 'category',
//            data: dateRange,
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//        },
//        yAxis: [{
//            name: "M/E CO Consumption (Ltr/Day)",
//            max: maxConsump,
//            nameLocation: 'center',
//            nameRotate: 90,
//            nameTextStyle: {
//                padding: 30,
//                fontWeight: 700,
//                fontSize: 14
//            },
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//            type: 'value'
//        },
//        {
//            //name: 'Expected Consumption (Ltr/Day)',
//            max: maxConsump,
//            index: 1,
//            label: {
//                normal: {
//                    show: false
//                }
//            },
//            nameLocation: 'center',
//            nameRotate: 90,
//            nameTextStyle: {
//                padding: 30,
//                color: colors[2],
//                fontWeight: 700,
//                fontSize: 14
//            },
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//            type: 'value'
//        },
//        {
//            name: 'M/E CO Feed Rate (g/BHP h)',
//            nameLocation: 'center',
//            nameRotate: 90,
//            nameTextStyle: {
//                padding: 30,
//                color: '#9076B5',
//                fontWeight: 700,
//                fontSize: 14
//            },
//            max: maxFRLimit,
//            index: 2,
//            splitLine: {
//                show: false
//            },
//            axisLabel: {
//                show: true
//            },
//            axisTick: {
//                show: true

//            },
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//            axisLine: {
//                lineStyle: {
//                    color: '#9076B5'
//                }
//            },
//            type: 'value'
//        }
//        ],
//        series: [{
//            name: seriesName[0],
//            type: 'bar',
//            barGap: '0%',
//            stack: true,
//            label: {
//                normal: {
//                    show: false,
//                    distance: 15,
//                    color: '#000',
//                    align: 'center',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}'
//                }
//            },
//            data: seriesData[4]
//        },
//        {
//            name: seriesName[1],
//            type: 'bar',
//            bargap: '0%',
//            stack: true,
//            label: {
//                normal: {
//                    show: false,
//                    distance: 15,
//                    color: '#000',
//                    align: 'center',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}'
//                }
//            },
//            data: seriesData[6]
//        },
//        {
//            name: seriesName[2],
//            type: 'line',
//            lineStyle: {
//                type: 'dashed'
//            },
//            label: {
//                normal: {
//                    show: true,
//                    distance: 15,
//                    color: colors[2],
//                    align: 'center',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}'
//                }
//            },
//            data: seriesData[10]
//        },
//        {
//            name: seriesName[3],
//            type: 'bar',
//            //barWidth: 30,
//            bargap: '0%',
//            yAxisIndex: 2,
//            label: {
//                normal: {
//                   // show: true,
//                    distance: 15,
//                    rotate: 90,
//                    color: '#000',
//                    align: 'left',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}'
//                }
//            },
//            data: seriesData[9]
//        },
//        {
//            type: 'line',
//            itemStyle: {
//                normal: {
//                    opacity: 0
//                }
//            },
//            yAxisIndex: 2,
//            lineStyle: {
//                normal: {
//                    opacity: 0
//                }
//            },
//            markLine: {
//                data: [{
//                    name: 'Markline between two points',
//                    yAxis: trigPointVal
//                }],
//                label: {
//                    normal: {
//                        show: true,
//                        position: 'middle',
//                        formatter: '{c} g/BHP h Recommended Feed Rate\n'
//                    }
//                },
//                lineStyle: {
//                    normal: {
//                        color: 'blue'
//                    }
//                }
//            }
//        }
//        ]
//    };


//    require.config({
//        paths: {
//            echarts3: '../js/echartsAll3'
//        },
//    });

//    require(
//      ['echarts3'],
//      function (ec) {
//          var graphFilDivName = graphDiv + "Graph";
//          var graphFilDiv = document.getElementById(graphFilDivName);
//          var myChartPerf = ec.init(graphFilDiv);
//          myChartPerf.setOption(option);
//          cpGraphState9 = myChartPerf;
//      }
//    );
//}


//// Lube Oil 2nd - Crankcase Oil
//function mpchart09a(graphDiv, seriesData1, dateRange1) {

//    var option = null;
//    var colors = ['#FF605A', '#524FFF', '#E8403D']; //'#0631FF'];
//    var dateRange = dateRange1;


//    var seriesName = ['Crankcase Oil ROB (Ltr)', 'Expected Feed Rate (g/BHP h)'];
//    var seriesData = seriesData1;
//    //var seriesData = [

//    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (0)
//    //  ['Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition  (1)

//    //  [14600, 14600, 14600, 14600, 14600, 14600, 14600], // ME Crankcase Oil - Prev ROB  (2)
//    //  [600, 0, 0, 0, 0, 100, 0], // ME Crankcase Oil   (3)
//    //  [14000, 14000, 14000, 14000, 14000, 13900, 13900], // ME Crankcase Oil - ROB  (4)
//    //  [20, 20, 20, 20, 20, 20, 20], // ME Crankcase Oil - Capacity  (m3) (5) 
//    //];


//    // setting the max value for the graph

//    var minROB = Number(Math.min(Math.min.apply(null, seriesData[4]))) - Number(Math.min(Math.min.apply(null, seriesData[4]))) * 0.05;
//    var maxROB = Number(Math.max(Math.max.apply(null, seriesData[4]))) + Number(Math.max(Math.max.apply(null, seriesData[4]))) * 0.05;
//    var maxcapacity = Math.max(Math.max.apply(null, seriesData[5]));
//    if (maxROB < maxcapacity)
//        maxROB = maxcapacity;
//    if (minROB > maxcapacity)
//        minROB = maxcapacity;

//    option = {
//        title: {
//            text: 'M/E Crankcase Lubricating Oil',
//            textStyle: {
//                color: '#C2417C'
//            },
//            subtext: 'Consumption & Inventory (Ltr/Day)',
//            subtextStyle: {
//                fontSize: 16,
//                color: '#BF6D92'
//            },
//            left: 'center'

//        },
//        color: colors,
//        grid: [{
//            top: 80,
//            bottom: 100
//        }],
//        tooltip: {
//            trigger: 'axis',
//            axisPointer: {
//                type: 'shadow'
//            },
//            formatter: function (params) {

//                /*---------------------Code for ballast and air sea---------------------------------*/


//                var insrt = [];
//                var result = params[0].dataIndex;

//                for (var i in seriesData) {
//                    var obj = seriesData[i];
//                    insrt[i] = obj[result];
//                }
//                /*----------------------------------------------------------------------------------*/

//                var tip = "";
//                tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpstyle" align="center"><b>' + insrt[0] + '</b></td><td class="tooltpstyle" align="center"><b>' + insrt[1] + '</b></td></tr></table><table class="tooltipTable" style="margin - bottom:10px;"> <tr> <td class="tooltpStyle" align="center" colspan="2" style="background: #777; color: white">Crankcase Oil</td></tr><tr> <td class="tooltpStyle" align="left">Previous ROB</td><td align="right"> ' + insrt[2] + ' Ltr</td></tr><tr> <td class="tooltpStyle" align="left">Consumption</td><td align="right">' + insrt[3] + ' Ltr/Day</td></tr><tr> <td class="tooltpStyle" align="left">ROB</td><td align="right"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[4] + '</span> Ltr</td></tr></table>';

//                return tip;
//            },

//            backgroundColor: '#ecf0f1',
//            borderColor: 'black',
//            padding: 5,
//            backgroundColor: 'rgba(245, 245, 245, 0.9)',
//            borderWidth: 2,
//            borderColor: '#999',
//            textStyle: {
//                color: '#000'
//            },

//            position: function (pos, params, el, elRect, size) {
//                var obj = {
//                    top: 55
//                };
//                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
//                return obj;
//            },

//            shared: true,
//            extraCssText: 'width: auto; height: auto'
//        },
//        toolbox: {
//            feature: {
//                restore: {
//                    show: true,
//                    title: 'Refresh'
//                },
//                saveAsImage: {
//                    show: true,
//                    title: 'Save'
//                }
//            }
//        },
//        dataZoom: [{
//            bottom: 30,
//            type: 'slider',
//            show: 'true',
//            start: 0,
//            end: 100,
//            dataBackground: {
//                areaStyle: {
//                    color: '#7ECFF2'
//                }
//            },
//            fillerColor: 'rgba(78,119,166,0.2)',
//            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
//            handleSize: '70%',
//            handleStyle: {
//                color: '#3D7C7F'
//            }
//        }],

//        legend: {


//            left: 'center',
//            width: 1200,
//            bottom: 0,
//            itemGap: 30,
//            data: [{
//                name: seriesName[0]
//            },

//            {
//                name: seriesName[1]
//            }

//            ],
//            textStyle: {
//                fontWeight: 'bold'
//            }

//        },
//        xAxis: {
//            type: 'category',
//            data: dateRange,
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//        },
//        yAxis: [{
//            name: seriesName[0],
//            min: minROB,
//            max: maxROB,
//            nameLocation: 'center',
//            nameRotate: 90,
//            nameTextStyle: {
//                padding: 50,
//                fontWeight: 700,
//                fontSize: 14
//            },
//            axisPointer: {
//                show: true,
//                label: {
//                    show: true
//                }
//            },
//            type: 'value'
//        }],
//        series: [{
//            name: seriesName[0],
//            type: 'bar',
//            barGap: '0%',
//            label: {
//                normal: {
//                    //show: true,
//                    distance: 15,
//                    color: '#fff',
//                    align: 'center',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}'
//                }
//            },
//            data: seriesData[4]
//           }
//           ,
//        {
//            type: 'line',
//            itemStyle: {
//                normal: {
//                    opacity: 0
//                }
//            },
//            lineStyle: {
//                normal: {
//                    opacity: 0
//                }
//            },
//            markLine: {
//                data: [{
//                    name: 'Markline between two points',
//                    yAxis: maxcapacity
//                }],
//                label: {
//                    normal: {
//                        show: true,
//                        position: 'middle',
//                        formatter: '{c} M/E Oil Sump Tank Capacity'
//                    }
//                },
//                lineStyle: {
//                    normal: {
//                        color: 'blue'
//                    }
//                }
//            }
//        }
//        ]
//    };


//    require.config({
//        paths: {
//            echarts3: '../js/echartsAll3'
//        },
//    });

//    require(
//      ['echarts3'],
//      function (ec) {
//          var graphFilDivName = graphDiv + "Graph";
//          var graphFilDiv = document.getElementById(graphFilDivName);
//          var myChartPerf = ec.init(graphFilDiv);
//          myChartPerf.setOption(option);
//          cpGraphState9a = myChartPerf;
//      }
//    );
//}


// Auxiliary Boiler
//function mpchart11(graphDiv) {

//    var option = null;
//    var colors = ['#FFC565', '#FF800F', '#0F3B66'];
//    var dateRange = dateRangeOrg;

//    var seriesName = ['Boiler FO Consumption (MT/day)', 'Boiler Water Consumption (MT/day)', 'Ship Speed (Kts)'];
//    var seriesData = [
//      [1, 1.5, 1, 0, 0, 0.5, 0], // Boiler Fuel Oil Consumtion MT  (0)
//      [1, 1, 3, 0, 0, 2, 0], // Boiler Water Consumption  MT   (1)
//      [13, 12, 14, 13, 13, 12, 12], // Ship Speed Kts  - EM Log Speed fron DNR   (2)
//      ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status   (3)
//      ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition   (4)
//      [6, 10, 19, 20, 21, 20, 17], // Ship Full Seep hours from DNR, to be shown in the tooltip   (5)
//      [0.5, 0, 0, 0, 0, 0, 0], // Fuel Consumed for Cargo Heating   (6)
//      [0, 1, 0, 0, 0, 0, 0], // Fuel Consumed for tank Cleaning    (7)
//      [0, 0, 1, 0, 0, 0, 0] // Fuel consumed for inerting   (8)
//    ];


//    var spdMax = Math.round(Math.max(...seriesData[2]) + Math.max(...seriesData[2]) * 0.1);
//    var hghConsump = (Math.max(...seriesData[0]) > Math.max(...seriesData[1]) ? Math.max(...seriesData[0]) : Math.max(...seriesData[1]));
//    var maxConsump = (hghConsump + hghConsump * 0.1);


//    option = {
//        title: {
//            text: 'Auxiliary Boiler',
//            subtext: 'Fuel Consumption - Feed Water Consumption',
//            textStyle: {
//                color: '#C2417C'
//            },
//            subtextStyle: {
//                fontSize: 15,
//                color: '#BF6D92'
//            },
//            left: 'center'

//        },
//        color: colors,
//        grid: [{
//            top: 60,
//            bottom: 100
//        }],
//        tooltip: {
//            trigger: 'axis',
//            axisPointer: {
//                type: 'shadow'
//            },
//            formatter: function (params) {

//                /*---------------------Code for ballast and air sea---------------------------------*/


//                var insrt = [];
//                var result = params[0].dataIndex;

//                for (var i in seriesData) {
//                    var obj = seriesData[i];
//                    insrt[i] = obj[result];
//                }
//                /*----------------------------------------------------------------------------------*/

//                var tip = "";
//                tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 100%" ><tr><td class="tooltpstyle" align="center" width="40 %"> <b> ' + insrt[3] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[4] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"><tr> <td class="tooltpStyle" align="left" width="70 %"> Boiler FO Consumption </td><td align="left" width="30 %"> <span style="color: black; background:' + colors[0] + '; padding: 0 3px">' + insrt[0] + '</span> MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %"> Boiler Water Consumption </td><td align="left" width="30 %"> <span style="color: black; background:' + colors[1] + '; padding: 0 3px">' + insrt[1] + '</span> MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %"> Ship&apos;s Speed</td><td align="left" width="30 %"> <span style="color: white; background:' + colors[2] + '; padding: 0 3px">' + insrt[2] + '</span> Kts</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Full Speed Hours</td><td align="left" width="30 %">' + insrt[5] + ' Hrs</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Cargo Heating</td><td align="left" width="30 %">' + insrt[6] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Tank Cleaning</td><td align="left" width="30 %">' + insrt[7] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Inerting</td><td align="left" width="30 %">' + insrt[8] + ' MT/day</td></tr></table>';

//                return tip;
//            },

//            backgroundColor: '#ecf0f1',
//            borderColor: 'black',
//            padding: 5,
//            backgroundColor: 'rgba(245, 245, 245, 0.9)',
//            borderWidth: 2,
//            borderColor: '#999',
//            textStyle: {
//                color: '#000'
//            },

//            position: function (pos, params, el, elRect, size) {
//                var obj = {
//                    top: 50
//                };
//                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 160;
//                return obj;
//            },

//            shared: true,
//            extraCssText: 'width: auto; height: auto'
//        },
//        dataZoom: [{
//            bottom: 30,
//            type: 'slider',
//            show: 'true',
//            start: 1,
//            end: 100,
//            dataBackground: {
//                areaStyle: {
//                    color: '#7ECFF2'
//                }
//            },
//            fillerColor: 'rgba(78,119,166,0.2)',
//            handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
//            handleSize: '70%',
//            handleStyle: {
//                color: '#3D7C7F'
//            }
//        }],

//        legend: {

//            left: 'center',
//            width: 1200,
//            bottom: 0,
//            itemGap: 30,
//            data: seriesName,
//            //				[	{
//            //						name: seriesName[2],
//            //						icon: 'image://assets//img/mainEngine/mainEngine_mtd.svg'
//            //					},
//            //				],
//            textStyle: {
//                fontWeight: 'bold'
//            }

//        },
//        xAxis: {
//            type: 'category',
//            data: ['21-Jan-2018', '22-Jan-2018', '23-Jan-2018', '24-Jan-2018', '25-Jan-2018', '26-Jan-2018', '27-Jan-2018']
//        },
//        yAxis: [{
//            name: seriesName[0],
//            nameLocation: 'center',
//            nameRotate: 90,
//            max: maxConsump,
//            nameTextStyle: {
//                padding: 30,
//                fontWeight: 700,
//                fontSize: 14
//            },
//            type: 'value'
//        }, {
//            name: seriesName[2],
//            nameLocation: 'center',
//            nameRotate: 90,
//            min: 0,
//            max: spdMax,
//            nameTextStyle: {
//                padding: 30,
//                fontWeight: 700,
//                fontSize: 14
//            },
//            splitLine: {
//                show: false
//            },
//            type: 'value'
//        }],
//        series: [{
//            name: seriesName[0],
//            type: 'bar',
//            label: {
//                normal: {
//                    show: true,
//                    distance: 15,
//                    rotate: 90,
//                    color: '#333',
//                    align: 'left',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c} MT'
//                }
//            },
//            barGap: '0%',
//            data: seriesData[0]
//        }, {
//            name: seriesName[1],
//            type: 'bar',
//            label: {
//                normal: {
//                    show: true,
//                    distance: 15,
//                    rotate: 90,
//                    color: '#333',
//                    align: 'left',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c} MT'
//                }
//            },
//            barGap: '0%',
//            data: seriesData[1]
//        },
//        {
//            name: seriesName[2],
//            type: 'line',
//            yAxisIndex: 1,
//            label: {
//                normal: {
//                    show: true,
//                    distance: 15,
//                    color: '#000',
//                    align: 'center',
//                    verticalAlign: 'middle',
//                    position: 'insideBottom',
//                    formatter: '{c}Kts'
//                }
//            },
//            data: seriesData[2]
//        }
//        ]
//    };


//    require.config({
//        paths: {
//            echarts3: '../js/echartsAll3'
//        },
//    });

//    require(
//      ['echarts3'],
//      function (ec) {
//          var graphFilDivName = graphDiv + "Graph";
//          var graphFilDiv = document.getElementById(graphFilDivName);
//          var myChartPerf = ec.init(graphFilDiv);
//          myChartPerf.setOption(option);
//          cpGraphState11 = myChartPerf;
//      }
//    );
//}
function mpchart10(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#776B06', '#DE0D03', '#97A302', '#094549'];
    var dateRange = dateRange1;

    var seriesName = ['TC1 Exhaust Temp In', 'TC1 Exhaust Temp Out', 'TC2 Exhaust Temp In', 'TC2 Exhaust Temp Out'];
    var seriesData = seriesData1;


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
            text: 'Turbo Charger Exhaust Temperature Analysis °C',
            subtext: 'T/C In & Out Exhaust Temperature.',
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
               // tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Exhaust Temperature</td></tr><tr><td class="tooltpStyle" align="left">Max</td><td align="left"><span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">Min</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding:0 5px">' + insrt[1] + '</span>' + seriesUnit[1] + '</td></tr></table>';
                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">T/C Exh Temperature</td></tr><tr><td class="tooltpStyle" align="left" width="70 %">' + seriesName[0] + '</td><td align="left" width="30 %"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[1] + '</span>' + seriesUnit[1] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background:' + colors[2] + '; padding: 0 5px">' + insrt[4] + '</span>' + seriesUnit[2] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[3] + '</td><td align="left"><span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[5] + '</span>' + seriesUnit[3] + '</td></tr></table>';
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
            name: "Exhaust Temperature °C",
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
        },
        {
            name: "",
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
        }
        ],
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
            yAxisIndex: 0,
            data: seriesData[4]
        },
        {
            name: seriesName[3],
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[5]
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
          cpGraphState10 = myChartPerf;
      }
    );

}

function mpchart01(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#FFDAB9', '#008000'];

    var seriesName = ['Total Capacity Available', 'Total Capacity Utilized', 'Total Hours'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
    var seriesDataMax1 = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
    //seriesDataMax = Math.ceil(seriesDataMax * 1.7);
    var maxhours = Math.ceil(Math.max(Math.max.apply(null, seriesData[23])));
   // maxhours = Math.ceil(maxhours * 1.7);

    option = {
        title: {
            text: "A/E Load at Sea",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }


                tip = '<table class="tooltipTable" width="600px" ><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  LightOrange">•</span>' + seriesName[0] + '</td><td> ' + insrt[0] + 'kWh</td><td></td></tr><tr><td><span class="tlTip tomatoOrange">•</span>' + seriesName[1] + '</td><td> ' + insrt[1] + ' kWh</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[2] + '</td><td> ' + insrt[23] + ' hrs</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>Load %</td><td> ' + insrt[2] + '%</td><td></td></tr></table><table class="tooltipTable"><tr><td></td><td><b>AE1</b></td><td><b>AE2</b></td><td><b>AE3</b></td></tr><tr><td><span class="tlTip oliveGreen">•</span>Max Load (kW)</td><td> ' + insrt[3] + '</td><td> ' + insrt[8] + '</td><td> ' + insrt[13] + '</td><td></td></tr><tr><td><span class="tlTip milkyEmerald">•</span>AE Load (kW)</td><td> ' + insrt[6] + '</td><td> ' + insrt[11] + '</td><td> ' + insrt[16] + '</td><td></td></tr><tr><td><span class="tlTip darkTeal">•</span>AE Hrs Run</td><td> ' + insrt[5] + '</td><td> ' + insrt[10] + '</td><td> ' + insrt[15] + '</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>AE Cap Available (kWh)</td><td> ' + insrt[4] + '</td><td> ' + insrt[9] + '</td><td> ' + insrt[14] + '</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>AE Load Utilized (kWh)</td><td> ' + insrt[7] + '</td><td> ' + insrt[12] + '</td><td> ' + insrt[17] + '</td><td></td></tr></table>';
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
                    top: 23
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
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: 'kwh',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxhours,
                
                min: 0,
                splitLine: {
                    show: false
                },
                name: 'hrs',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[1],
                show: false,
                type: 'bar',
                stack: true,
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[1]
            },
            {
                name: seriesName[0],
                type: 'bar',
                barGap: 0,
                stack: true,
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[24]
            },
            {
                name: seriesName[2],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[2],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[23]
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
//Dashboard Common Allert
function mpchart04(graphDiv, seriesdata, dateseries) {
    var app1 = {};

    var option = null;

    var colors = ['#E76322'];



    var seriesData = seriesdata;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var seriesDataMin = Math.floor(Math.min.apply(null, seriesData[0]));

    var measure = seriesData[3][0];
    var unit = seriesData[4][0];
    if (unit == "&mu;m")
        unit = "μm";

    if (unit == "degree")
        unit = "°C";

    if (seriesDataMin > 0)
        seriesDataMin = 0;
    option = {
        title: {
            text: measure,
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var valtag = insrt[1] == "1" ? '<td class="tooltpstyle"  align="center"><span style="background: red; color: white; padding: 0px 7px; border - radius: 4px;"> ' + insrt[0] + ' ' + unit + '</span></td>' : '<td class="tooltpstyle"  align="center"><span style="background: green; color: white; padding: 0px 7px; border - radius: 4px;"> ' + insrt[0] + ' ' + unit + '</span></td>';

                tip = '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td><td class="tooltpStyle"><span class="pad20">' + insrt[6] + '</span></td><td class="tooltpStyle"><span class="pad20">' + insrt[5] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  tomatoOrange">•</span>' + insrt[3] + '</td>' + valtag + '</tr></table>';
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
                    top: 23
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
        //legend: {
        //    data: seriesName,
        //    bottom: 0
        //},
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            }
        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: seriesDataMin,
            name: unit,
            index: 0,
            nameLocation: 'center',
            axisLine: { onZero: true },
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
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },     
        }
        ],
        series: [{
            name: measure,
            type: 'bar',
            barGap: 0,
            label: {
                normal: {
                    show: false,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            index: 0,
            data: seriesData[0]
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

// AE FOC
function mpchart11(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#008000'];

    var seriesName = ['A/E FOC', 'Total Run Hours'];
    var seriesUnit = ['MT', 'Hrs'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxhours = Math.ceil(Math.max(Math.max.apply(null, seriesData[1])));


    option = {
        title: {
            text: "Auxiliary Engine Fuel Oil Consumption",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }


                //tip = '<table class="tooltipTable" width="600px" ><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  LightOrange">•</span>' + seriesName[0] + '</td><td> ' + insrt[0] + 'kWh</td><td></td></tr><tr><td><span class="tlTip tomatoOrange">•</span>' + seriesName[1] + '</td><td> ' + insrt[1] + ' kWh</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[2] + '</td><td> ' + insrt[23] + ' hrs</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>Load %</td><td> ' + insrt[2] + '%</td><td></td></tr></table><table class="tooltipTable"><tr><td></td><td><b>AE1</b></td><td><b>AE2</b></td><td><b>AE3</b></td></tr><tr><td><span class="tlTip oliveGreen">•</span>Max Load (kW)</td><td> ' + insrt[3] + '</td><td> ' + insrt[8] + '</td><td> ' + insrt[13] + '</td><td></td></tr><tr><td><span class="tlTip milkyEmerald">•</span>AE Load (kW)</td><td> ' + insrt[6] + '</td><td> ' + insrt[11] + '</td><td> ' + insrt[16] + '</td><td></td></tr><tr><td><span class="tlTip darkTeal">•</span>AE Hrs Run</td><td> ' + insrt[5] + '</td><td> ' + insrt[10] + '</td><td> ' + insrt[15] + '</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>AE Cap Available (kWh)</td><td> ' + insrt[4] + '</td><td> ' + insrt[9] + '</td><td> ' + insrt[14] + '</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>AE Load Utilized (kWh)</td><td> ' + insrt[7] + '</td><td> ' + insrt[12] + '</td><td> ' + insrt[17] + '</td><td></td></tr></table>';
                tip = '<table class="tooltipTable" style="margin-bottom: 1px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[2] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[3] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Auxiliary Engine</td></tr><tr><td class="tooltpStyle" align="left" width="70 %">' + seriesName[0] + '</td><td align="left" width="30 %"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[1] + '</span>' + seriesUnit[1] + '</td></tr></table>';
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
                    top: 23
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
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: 'MT',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxhours,

                min: 0,
                splitLine: {
                    show: false
                },
                name: 'hrs',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[0],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[0]
            },
            {
                name: seriesName[1],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
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
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphState11 = myChartPerf;
        }
    );
}

// ME FOC
function mpchart02(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#008000'];

    var seriesName = ['M/E FOC', 'M/E Load'];
    var seriesUnit = ['MT', 'kW'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxLoad = Math.ceil(Math.max(Math.max.apply(null, seriesData[7])));

    option = {
        title: {
            text: "Main Engine Fuel Oil Consumption",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                tip = '<table class="tooltipTable" style="margin-bottom: 1px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[5] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[6] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Main Engine</td></tr><tr><td class="tooltpStyle" align="left" width="50 %">' + seriesName[0] + '</td><td align="left" width="50%"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[7] + '</span>' + seriesUnit[1] + '</td></tr></table>';
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
                    top: 23
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
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: seriesUnit[0],
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxLoad,

                min: 0,
                splitLine: {
                    show: false
                },
                name: seriesUnit[1],
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[0],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[0]
            },
            {
                name: seriesName[1],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[7]
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
            cpGraphState2 = myChartPerf;
        }
    );
}

// ME FOC / 24 Hrs
function mpchart05(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#008000'];

    var seriesName = ['M/E FOC/Day', 'M/E Load'];
    var seriesUnit = ['MT/Day', 'kW'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxLoad = Math.ceil(Math.max(Math.max.apply(null, seriesData[7])));

    option = {
        title: {
            text: "Main Engine Fuel Oil Consumption",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                tip = '<table class="tooltipTable" style="margin-bottom: 1px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[5] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[6] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Main Engine</td></tr><tr><td class="tooltpStyle" align="left" width="50 %">' + seriesName[0] + '</td><td align="left" width="50%"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[7] + '</span>' + seriesUnit[1] + '</td></tr></table>';
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
                    top: 23
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
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: seriesUnit[0],
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxLoad,

                min: 0,
                splitLine: {
                    show: false
                },
                name: seriesUnit[1],
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[0],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[0]
            },
            {
                name: seriesName[1],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[7]
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

// Boiler FOC
function mpchart06(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#008000', '#F8043F'];

    var seriesName = ['BLR FOC', 'M/E Load', 'SW Temp'];
    var seriesUnit = ['MT', 'kW', '°C'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var maxLoad = Math.ceil(Math.max(Math.max.apply(null, seriesData[7])));
    var maxTemp = Math.ceil(Math.max(Math.max.apply(null, seriesData[8])));

    option = {
        title: {
            text: "Boiler Fuel Oil Consumption",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                tip = '<table class="tooltipTable" style="margin-bottom: 1px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[5] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[6] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Boiler FOC / ME Load</td></tr><tr><td class="tooltpStyle" align="left" width="50 %">' + seriesName[0] + '</td><td align="left" width="50%"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[7] + '</span>' + seriesUnit[1] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[2] + '</td><td align="left"><span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[8] + '</span>' + seriesUnit[2] + '</td></tr></table>';
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
                    top: 23
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
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: 0,
            name: seriesUnit[0],
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 10,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxLoad,

                min: 0,
                splitLine: {
                    show: false
                },
                name: seriesUnit[1],
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            },
            {
                type: 'value',
                max: maxTemp,

                min: 0,
                splitLine: {
                    show: false
                },
                name: seriesUnit[2],
                nameLocation: 'center',
                nameRotate: 90,
                position: 'left',
                offset: 50,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 2,
                axisPointer: {
                    //show: true,
                    label: {
                        show: true
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: colors[2]
                    }
                },
            }
        ],
        series: [
            {
                name: seriesName[0],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[0]
            },
            {
                name: seriesName[1],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[7]
            },
            {
                name: seriesName[2],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 2,
                data: seriesData[8]
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

// Slip(%)
function mpchart08(graphDiv, seriesdata1, dateseries) {

    var option = null;

    var colors = ['#FF6347', '#008000'];

    var seriesName = ['Slip(%)', 'M/E Load'];
    var seriesUnit = ['%', 'kW'];

    var seriesData = seriesdata1;
    var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0])));
    var seriesDataMin = Math.min(Math.min.apply(null, seriesData[0]));
    if (Number(seriesDataMin) < 0)
        seriesDataMin = Math.floor(seriesDataMin);
    else
        seriesDataMin = 0;

    var maxLoad = Math.ceil(Math.max(Math.max.apply(null, seriesData[9])));
    var minLoad = Math.ceil(Math.min(Math.min.apply(null, seriesData[9])));


    if (seriesDataMax > maxLoad)
        maxLoad = seriesDataMax;
    else
        seriesDataMax = maxLoad;

    if (seriesDataMin < minLoad)
        minLoad = seriesDataMin;
    else
        seriesDataMin = minLoad;

    option = {
        title: {
            text: "Slip(%)",
            textStyle: {
                color: '#C2417C'
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
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var insrtAmbi = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }
                tip = '<table class="tooltipTable" style="margin-bottom: 1px; width: 320px"><tr> <td class="tooltpstyle" align="center" width="40 %"><b>' + insrt[5] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[6] + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"> <tr><td colspan="2" class="tooltpStyle" align="center" style="background: #555; color: #fff" width="70 %">Slip(%) / ME Load</td></tr><tr><td class="tooltpStyle" align="left" width="50 %">' + seriesName[0] + '</td><td align="left" width="50%"><span style="color: white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span>' + seriesUnit[0] + '</td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[7] + '</span>' + seriesUnit[1] + '</td></tr></table>';
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
                    top: 23
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
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: [{
            type: 'category',
            data: dateseries,
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },


        }],
        yAxis: [{
            type: 'value',
            max: seriesDataMax,
            min: seriesDataMin,
            axisLine: { onZero: true },
            name: seriesUnit[0],
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            index: 0,
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

        },
            {
                type: 'value',
                max: maxLoad,
                min: seriesDataMin,
                axisLine: { onZero: true },
                splitLine: {
                    show: false
                },
                name: 'kW X 100', //seriesUnit[1],
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14
                },
                index: 1,
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
            }
        ],
        series: [
            {
                name: seriesName[0],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[0],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 0,
                data: seriesData[0]
            },
            {
                name: seriesName[1],
                show: true,
                type: 'line',
                label: {
                    normal: {
                        show: false,
                        rotate: 90,
                        distance: 15,
                        color: colors[1],
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[9]
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
            cpGraphState8 = myChartPerf;
        }
    );
}
var startDate = "";
var endDate = "";
var StartdateLabel = "";
var EnddateLabel = "";
function mpchart09(graphDiv, seriesData1, dateRange1) {

    var app1 = {};

    var option = null;

    var colors = ['#2980b9', '#E81651', '#2980b9', '#27ae60', '#2980b9', '#c0392b', '#2980b9', '#e67e22'];

    var seriesName = ['Condition', 'CP Wx', 'Actual Wx', 'CP Speed', 'SOG', 'CP HFO', 'Vsl HFO', 'CP DO', 'Vsl DO'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  ['ballast', 'ballast', 'ballast', 'laden', 'laden', 'laden', 'laden'],  // condition  0
    //  [4, 4, 4, 4, 4, 4, 4],  //  cp wind   1
    //  [2, 3, 5, 3, 3, 6, 3],  //  actual wind   2
    //  [12, 12, 12, 11, 11, 11, 11],  // cp sog  3
    //  [13, 12.5, 11, 12.5, 12.7, 11, 12.7],  // vessel sog  4
    //  [20, 20, 20, 24, 24, 24, 24], //  cp hfo   5
    //  [19, 18, 21, 23, 23.5, 25.5, 23],  // vessel hfo   6
    //  [4, 4, 4, 4, 4, 4, 4],   // cp do   7
    //  [3, 3.5, 3, 3.75, 3.5, 3, 3],  // vessel do   8
    //  // [102, 102, 102, 101, 101, 100, 100]  // voyage number  9
    //  [102, 102, 102, 102, 102, 102, 102],  // voyage number  9
    //  [],
    //  []
    //];
    //var dateRange = dateRangeOrg;
    var dateRange = dateRange1;
    var runingAvgSpeed = 0;
    var avgSOG = 0;
    var avgFO = 0;
    var maxWind = Math.ceil(Math.max(Math.max.apply(null, seriesData[1]), Math.max.apply(null, seriesData[2])));
    var maxSog = Math.ceil(Math.max(Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4])));
    maxSog += maxSog > 0 ? Math.ceil(maxSog * 0.2) : maxSog;
    var maxHfo = Math.ceil(Math.max(Math.max.apply(null, seriesData[5]), Math.max.apply(null, seriesData[6]), Math.max.apply(null, seriesData[7]), Math.max.apply(null, seriesData[8])));
    maxHfo += maxHfo > 0 ? Math.ceil(maxHfo * 0.2) : maxHfo;
    //var maxDo = Math.ceil(Math.max(Math.max.apply(null, seriesData[7]), Math.max.apply(null, seriesData[8])));

    // summation of speed and populating for running Avg speed
    seriesData[4].forEach(function (itm, idx, arr) {
        avgSOG += arr[idx];
        runingAvgSpeed += arr[idx];
        if (!idx)
            seriesData[10][0] = arr[idx];
        else
            seriesData[10][idx] = ((Number(seriesData[10][idx - 1]) + Number(arr[idx])) / (idx + 1)).toFixed(2);
    });

    runingAvgSpeed /= seriesData[10].length;
    runingAvgSpeed = runingAvgSpeed.toFixed(2);
    // calculating average SOG - suggestion is to calculate from distance travelled divided by time taken for the voyage.
    avgSOG /= seriesData[4].length;
    avgSOG = avgSOG.toFixed(2);

    // calculating average ME Fuel.
    seriesData[6].forEach(function (itm, idx, arr) {
        avgFO += arr[idx];
    });

    avgFO /= seriesData[6].length;
    avgFO = avgFO.toFixed(2);
    // 

    // console.log(runingAvgSpeed);
    option = {
        color: colors,
        title: {
            text: 'Charter Party Performance',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Speed & Fuel Consumption Vs Charter Party Requirements',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            x: 'center'
        },

        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];

                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var windDev = insrt[2] - cpBF;
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";
                
                var speedDev = insrt[4] - insrt[3];
                var speedStyle = (speedDev >= 0) ? "greenStatus" : "redStatus";
                var speedDevPercent = (((Math.abs(insrt[4] - insrt[3]) * 100) / insrt[4]).toFixed(2)).toString() + "%";

                var hfoDev = insrt[6] - insrt[5];
                var hfoStyle = (hfoDev <= 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[6] - insrt[5]) * 100) / insrt[6]).toFixed(2)).toString() + "%";

                var doDev = insrt[8] - insrt[7];
                var doStyle = (doDev <= 0) ? "greenStatus" : "redStatus";
                var doDevPercent = (insrt[8]) ? ((((Math.abs(insrt[8] - insrt[7]) * 100) / insrt[8]).toFixed(2)).toString() + "%") : "-";

                // console.log(insrt[10]);

                tip = '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[0] + '</span></td><td class="tooltpstyle" align="center" width="30%"><b>' + insrt[12] + '</b></td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[2] + '</span></td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Speed</td</tr><tr><td><span class="tlTip style2">•</span>' + seriesName[3] + '</td><td> ' + insrt[3] + ' Kts</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[4] + '</td><td>' + insrt[4] + ' Kts</td><td></td></tr><tr style="margin-bottom: 20px;"><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kts</span></td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Fuel</td</tr><tr><td><span class="tlTip style4">•</span>' + seriesName[5] + '</td><td width="80"> ' + insrt[5] + ' MT/d</td><td width="40"></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[6] + '</td><td> ' + insrt[6] + ' MT/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip style6">•</span>' + seriesName[7] + '</td><td> ' + insrt[7] + ' MT/d</td><td></td></tr><tr><td><span class="tlTip style5">•</span>' + seriesName[8] + '</td><td> ' + insrt[8] + ' MT/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + doStyle + '">' + doDevPercent + '</span></td><td><span class="' + doStyle + '">' + Math.abs(doDev.toFixed(2)) + ' MT/d</span></td></tr></table>';
                
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
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 60;
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

        axisPointer: {
            link: {
                xAxisIndex: 'all'
            }
        },

        grid: [{
            left: 70,
            right: 70,
            height: '35%'
        }, {
            left: 70,
            right: 70,
            top: '50%',
            height: '35%'
        }],

        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 0,
            end: 100,
            xAxisIndex: [0, 1, 2, 3],
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
            itemGap: 10,
            data: [

              {
                  name: seriesName[0]
              }, {
                  name: seriesName[1]
              }, {
                  name: seriesName[2]
              }, {
                  name: seriesName[3]
              },
              {
                  name: seriesName[4]
              },
              {
                  name: seriesName[5]
              },
              {
                  name: seriesName[6]
              },
              {
                  name: seriesName[7]
              },
              {
                  name: seriesName[8]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }
        },


        xAxis: [{
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: false
                }
            }
        }, {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: false
                }
            }
        }, {
            gridIndex: 1,
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: false
                }
            }
        }, {
            gridIndex: 1,
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange
        }],

        yAxis: [{
            type: 'value',
            name: 'SOG MarkLine',
            //max: 15,
            nameLocation: 'end',
            show: false,
            position: 'right',
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },
        {
            type: 'value',
            name: 'Actual Wx (BF)',
            max: 8,
            nameLocation: 'middle',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40
            },
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLabel: {
                formatter: '{value}'
            },
        },

        ///  Vessel Speed

        {
            type: 'value',
            name: 'CP Speed',
            show: false,
            nameLocation: 'end',
            max: maxSog,
            position: 'left',
            axisLine: {
                lineStyle: {
                    color: colors[2]
                }
            },
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },

        {
            type: 'value',
            name: 'SOG (Kts)',
            max: maxSog,
            nameLocation: 'middle',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40
            },
            position: 'left',
            axisLine: {
                lineStyle: {
                    color: colors[3]
                }
            },
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },


        //    HFO DO detail...

        {
            gridIndex: 1,
            type: 'value',
            name: 'CP HFO',
            nameLocation: 'end',
            max: maxHfo,
            show: false,
            position: 'right',
            axisPointer: {
                label: {
                    show: false
                }
            },
            axisLine: {
                lineStyle: {
                    color: colors[4]
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },
        {
            gridIndex: 1,
            type: 'value',
            name: 'FO Cons (MT/d)',
            max: maxHfo,
            nameLocation: 'middle',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40
            },
            position: 'left',
            axisLine: {
                lineStyle: {
                    color: colors[5]
                }
            },
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },
        {
            gridIndex: 1,
            type: 'value',
            name: 'CP DO',
            show: false,
            nameLocation: 'end',
            max: maxHfo,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[6]
                }
            },
            axisPointer: {
                label: {
                    show: false
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        },
        {
            gridIndex: 1,
            type: 'value',
            name: 'DO Cons (MT/d)',
            max: maxHfo,
            nameLocation: 'middle',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40
            },
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[7]
                }
            },
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        }
        ],
        series: [{
            name: seriesName[1],
            type: 'line',
            data: seriesData[1],
            symbolSize: 10,
            yAxisIndex: 1

        },
        {
            // name: 'Actual Wind',
            name: seriesName[2],
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[2],
            symbolSize: 10
        }, {
            name: 'CP Speed',
            type: 'line',
            xAxisIndex: 1,
            yAxisIndex: 2,
            data: seriesData[3],
            symbolSize: 10
        }, {
            name: 'SOG',
            type: 'line',
            xAxisIndex: 1,
            yAxisIndex: 3,
            data: seriesData[4],
            symbolSize: 10,
            markLine: {
                label: {
                    position: 'start'
                },
                data: [
                  {
                      yAxis: avgSOG,
                      name: 'Avg SOG'
                  }
                ]
            }
        }, {
            name: 'CP HFO',
            type: 'line',
            xAxisIndex: 2,
            yAxisIndex: 4,
            data: seriesData[5],
            symbolSize: 10
        }, {
            name: 'Vsl HFO',
            type: 'line',
            xAxisIndex: 2,
            yAxisIndex: 5,
            data: seriesData[6],
            symbolSize: 10,
            markLine: {
                data: [
                  {
                      yAxis: avgFO,
                      name: '平均值'
                  }
                ]
            }
        }, {
            name: 'CP DO',
            type: 'line',
            xAxisIndex: 3,
            yAxisIndex: 6,
            data: seriesData[7],
            symbolSize: 10
        }, {
            name: 'Vsl DO',
            type: 'line',
            xAxisIndex: 3,
            yAxisIndex: 7,
            data: seriesData[8],
            symbolSize: 10
        }]
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
          cpGraphState9 = myChartPerf;
      }
    );
}
