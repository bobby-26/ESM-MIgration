var seriesDataOrg;
var dateRangeOrg;

var voyGraphStateA = {};
var voyGraphStateB = {};
var voyGraphStateC = {};

function VoyageOverViewChart(seriesdata, daterange)
{

     seriesDataOrg = seriesdata;
     dateRangeOrg = daterange;

     graphA(seriesdata, daterange);
     graphB(seriesdata, daterange);
     graphC(seriesdata, daterange);

}


function winResize()
{

    voyGraphStateA.resize();
    voyGraphStateB.resize();
    voyGraphStateC.resize();
}

function graphA(seriesDataOrg, dateRangeOrg) {

    var app1 = {};

    var option = null;

    var colors = ['#2980b9', '#c0392b', '#f1c40f', '#27ae60', '#e67e22', '#FF403C', '#FF982C', '#FFD706', '#7DE346', '#3AAEB0'];

    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load % MCR', 'Load Limit', 'SLIP', 'RPM', 'FOC', 'DOC', 'Vessel Status', 'NCR', 'Power Output', 'Gov/Fuel Index'];
    //var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'FOC Rate', 'DOC Rate','Vessel Status'];
    var seriesData = seriesDataOrg;

    var dateRange = dateRangeOrg;
    var maxval = Math.ceil(Math.max(Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4]), Math.max.apply(null, seriesData[5]), Math.max.apply(null, seriesData[6])));
    var maxSog = Math.ceil(Math.max(Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4])));
    option = {
        color: colors,
        title: {
            text: 'Speed, Trim & Displacement Analysis',
            x: 'center'
        },

        legend: {
            left: 'center',
            width: 600,
            bottom: -3,
            itemGap: 30,
            data: [
              { name: 'SOG', icon: 'image://../css/Theme1/images/icon-SOG.svg' },
              { name: 'LOG', icon: 'image://../css/Theme1/images/icon-LOG.svg' },
              { name: 'CP Speed', icon: 'image://../css/Theme1/images/icon-CP-Speed.svg' },
              { name: 'Trim', icon: 'image://../css/Theme1/images/icon-Trim.svg' },
    //          {name:'Displacement', icon:'roundRect'}
              { name: 'Displacement', icon: 'image://../css/Theme1/images/icon-Displacement.svg' },
              { name: 'SLIP %', icon: 'image://../css/Theme1/images/icon-Slip.svg' }
            ],
            textStyle: { fontWeight: 'bold' }
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

                var windDev = insrt[1] - insrt[13];
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[4] - insrt[2];
                var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";

                var speedDevPercent = ((Math.abs(insrt[4] - insrt[2]) * 100) / insrt[4]).toFixed(1);

                var hfoDev = insrt[12] - insrt[11];
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(1)).toString() + "%";

                var doStyle = (insrt[8] <= 80) ? "greenSpeed" : "redSpeed";

                var trimDev = insrt[5];
                var trimStyle = (trimDev < 0) ? "redSpeed" : "greenSpeed";

                //insrt[14] vessel status 
                //insrt[15] doc rate 
                // tip = '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center" width="125">' + insrt[9] + '</td><td class="tooltpStyle" align="center" width="125">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>' + seriesName[2] + '</td><td>' + insrt[4] + 'Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + 'kn</span></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td>' + insrt[5] + 'm</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[7] + '%</span></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t/d</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' t/d</span></td></tr></table>';
                tip = (insrt[14] == "At Sea") ? '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td><span class="' + trimStyle + '">' + insrt[5] + 'm</span></td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td width="125"><span class="tlTip oceanGreen">•</span>' + seriesName[13] + '</td><td>' + insrt[16] + 'bhp</td></tr><tr><td><span class="tlTip darkGreen">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[8] + '%</span></td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[14] + '</td><td>' + insrt[7] + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + 't</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + 't</td><td></td></tr></table>' : '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[14] + '</span></td><td class="tooltpStyle" align="center">' + insrt[0] + '</td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t</td><td></td></tr></table>';
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

        grid: {
            left: 50,
            right: 50,
            height: '160'
        },

        xAxis: {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: true
                }
            }
        },

        yAxis: [{
            type: 'value',
            nameLocation: 'end',
            max: maxSog,
            name: "Kn",
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
        }, {
            type: 'value',
            nameLocation: 'end',
            show: false,
            max: maxSog,
            position: 'left',
            axisLine: {
                lineStyle: {
                    color: colors[1]
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
        }, {
            type: 'value',
            name: 'SOG',
            show: false,
            nameLocation: 'end',
            max: maxval,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[2]
                }
            },
            axisPointer: {
                label: {
                    show: false
                }
            },
            axisLabel: {
                formatter: '{value}Kn'
            }
        }, {
            type: 'value',
            name: 'VSL SOG',
            show: false,
            nameLocation: 'end',
            max: maxval,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[3]
                }
            },
            axisPointer: {
                label: {
                    show: false
                }
            },
            axisLabel: {
                formatter: '{value}Kn'
            }
        }, {
            type: 'value',
            name: '*1000t',
            nameLocation: 'end',
            max: maxval,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: colors[0]
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
            name: 'SOG',
            type: 'line',
            data: seriesData[2],
            symbolSize: 10
        }, {
            name: 'LOG',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[3],
            symbolSize: 10
        }, {
            name: 'CP Speed',
            type: 'line',
            yAxisIndex: 2,
            data: seriesData[4],
            symbolSize: 10
        }, {
            name: 'Trim',
            type: 'line',
            yAxisIndex: 3,
            label: { show: true },
            data: seriesData[5],
            symbolSize: 10
        }, {
            name: 'Displacement',
            type: 'line',
            yAxisIndex: 4,
            data: seriesData[6],
            symbolSize: 10
        }, {
            name: 'SLIP %',
            type: 'line',
            yAxisIndex: 4,
            data: seriesData[9],
            symbolSize: 10
        }]
    };


    require.config({
        paths: {
            echarts2: '../js/echartsAll3'
        }
    });

    require(
      ['echarts2'],
      function (ec) {
          var graphFilDiv = document.getElementById("voyGraphA");
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          voyGraphStateA = myChartPerf;
      }
    );
}
function graphB(seriesDataOrg, dateRangeOrg) {

    var app1 = {};

    var option = null;

    var colors = ['#275936', '#4682b4', '#3AAEB0', '#FF403C'];
    //  var colors = ['#2980b9', '#c0392b', '#f1c40f', '#27ae60', '#e67e22', '#FF403C', '#FF982C', '#FFD706', '#7DE346', '#3AAEB0'];

    //var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'FOC Rate', 'CP FOC Rate'];
    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load % MCR', 'Load Limit', 'SLIP', 'RPM', 'FOC', 'DOC', 'Vessel Status', 'NCR', 'Power Output', 'Gov/Fuel Index'];
    var seriesData = seriesDataOrg;

    var dateRange = dateRangeOrg;

    var maxval = Math.ceil(Math.max(Math.max.apply(null, seriesData[7]), Math.max.apply(null, seriesData[8]), Math.max.apply(null, seriesData[9]), Math.max.apply(null, seriesData[10])));
    var BhpMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[15]), Math.max.apply(null, seriesData[16])));
    option = {
        color: colors,
        title: {
            text: 'LOAD & RPM Analysis',
            x: 'center'
        },

        legend: {
            left: 'center',
            width: 650,
            bottom: -3,
            itemGap: 30,
            data: [
              { name: 'ME RPM', icon: 'image://../css/Theme1/images/icon-ME-RPM.svg' },
              { name: 'ME GOV/Fuel Index', icon: 'image://../css/Theme1/images/icon-Load.svg' },
              { name: 'Power Output', icon: 'image://../css/Theme1/images/icon-Load-Limit.svg' },
              { name: 'NCR', icon: 'image://../css/Theme1/images/icon-Slip.svg' }
            ],
            textStyle: { fontWeight: 'bold' }
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

                var windDev = insrt[1] - insrt[13];
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[4] - insrt[2];
                var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";

                var speedDevPercent = ((Math.abs(insrt[4] - insrt[2]) * 100) / insrt[4]).toFixed(2);

                var hfoDev = insrt[12] - insrt[11];
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(2)).toString() + "%";

                var doStyle = (insrt[8] <= 80) ? "greenSpeed" : "redSpeed";

                var trimDev = insrt[5];
                var trimStyle = (trimDev < 0) ? "redSpeed" : "greenSpeed";

                //tip = '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center" width="125">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>' + seriesName[2] + '</td><td>' + insrt[4] + 'Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + 'kn</span></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td>' + insrt[5] + 'm</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[7] + '%</span></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t/d</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' t/d</span></td></tr></table>';
                tip = (insrt[14] == "At Sea") ? '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td><span class="' + trimStyle + '">' + insrt[5] + 'm</span></td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td width="125"><span class="tlTip oceanGreen">•</span>' + seriesName[13] + '</td><td>' + insrt[16] + 'bhp</td></tr><tr><td><span class="tlTip darkGreen">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[8] + '%</span></td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[14] + '</td><td>' + insrt[7] + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + 't</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + 't</td><td></td></tr></table>' : '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[14] + '</span></td><td class="tooltpStyle" align="center">' + insrt[0] + '</td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t</td><td></td></tr></table>';
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
                    top: -160
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

        grid: {
            left: 50,
            right: 50,
            height: '160'
        },

        xAxis: {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: true
                }
            }
        },

        yAxis: [
          {
              type: 'value',
              name: "RPM",
              nameLocation: 'end',
              max: maxval,
              axisPointer: {
                  label: {
                      show: false
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
            nameLocation: 'end',
            max: maxval,
            name: '%',
            show: false,
            position: 'left',
            axisLine: {
                lineStyle: {
                    color: colors[1]
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
            type: 'value',
            name: 'SOG',
            show: false,
            nameLocation: 'end',
            max: BhpMax,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: '#FF982C'
                }
            },
            axisPointer: {
                label: {
                    show: false
                }
            },
            axisLabel: {
                formatter: '{value}Kn'
            }
        }, {
            type: 'value',
            name: 'bhp',
            show: true,
            nameLocation: 'end',
            max: BhpMax,
            position: 'right',
            axisLine: {
                lineStyle: {
                    color: '#3AAEB0'
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
            name: 'ME RPM',
            type: 'line',
            data: seriesData[10],
            symbolSize: 10
        }, {
            name: 'ME GOV/Fuel Index',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[7],
            symbolSize: 10
        }, {
            name: 'Power Output',
            type: 'line',
            yAxisIndex: 2,
            data: seriesData[16],
            symbolSize: 10
        }, {
            name: 'NCR',
            type: 'line',
            yAxisIndex: 3,
            data: seriesData[15],
            symbolSize: 10
        }]
    };


    require.config({
        paths: {
            echarts2: '../js/echartsAll3'
        }
    });

    require(
      ['echarts2'],
      function (ec) {
          var graphFilDiv = document.getElementById("voyGraphB");
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          voyGraphStateB = myChartPerf;
      }
    );
}
function graphC(seriesDataOrg, dateRangeOrg) {

    var app1 = {};

    var option = null;

    var colors = ['#733250', '#0B877D'];
    //  var colors = ['#FF403C', '#2980b9'];

    //var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'FOC Rate', 'CP FOC Rate'];
    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load % MCR', 'Load Limit', 'SLIP', 'RPM', 'FOC', 'DOC', 'Vessel Status', 'NCR', 'Power Output', 'Gov/Fuel Index'];
    var seriesData = seriesDataOrg;

    var dateRange = dateRangeOrg;

    var maxvalfo = Math.ceil(Math.max.apply(null, seriesData[11]));
    var maxvaldo = Math.ceil(Math.max.apply(null, seriesData[12]))
    option = {
        color: colors,
        title: {
            text: 'FOC Analysis',
            x: 'center'
        },

        legend: {
            left: 'center',
            width: 600,
            bottom: 0,
            itemGap: 30,
            data: [
              { name: 'FOC t', icon: 'image://../css/Theme1/images/icon-FOC.svg' },
              { name: 'DOC t', icon: 'image://../css/Theme1/images/icon-FOC-CP.svg' }
            ],
            textStyle: { fontWeight: 'bold' }
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

                var windDev = insrt[1] - insrt[13];
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[4] - insrt[2];
                var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";

                var speedDevPercent = ((Math.abs(insrt[4] - insrt[2]) * 100) / insrt[4]).toFixed(2);

                var hfoDev = insrt[12] - insrt[11];
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(2)).toString() + "%";

                var doStyle = (insrt[8] <= 80) ? "greenSpeed" : "redSpeed";

                var trimDev = insrt[5];
                var trimStyle = (trimDev < 0) ? "redSpeed" : "greenSpeed";

                //tip = '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center" width="125">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>' + seriesName[2] + '</td><td>' + insrt[4] + 'Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + 'kn</span></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td>' + insrt[5] + 'm</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[7] + '%</span></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t/d</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' t/d</span></td></tr></table>';
                tip = (insrt[14] == "At Sea") ? '<table class="tooltipTable" style="margin-bottom: 20px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td><span class="tlTip style2">•</span>' + seriesName[0] + '</td><td> ' + insrt[2] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[1] + '</td><td>' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style8">•</span>' + seriesName[7] + '</td><td>' + insrt[9] + '%</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[3] + '</td><td><span class="' + trimStyle + '">' + insrt[5] + 'm</span></td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkGreen">•</span>' + seriesName[8] + '</td><td>' + insrt[10] + '</td></tr><tr><td width="125"><span class="tlTip oceanGreen">•</span>' + seriesName[13] + '</td><td>' + insrt[16] + 'bhp</td></tr><tr><td><span class="tlTip darkGreen">•</span>' + seriesName[5] + '</td><td><span class="' + doStyle + '">' + insrt[8] + '%</span></td></tr><tr><td><span class="tlTip steelBlue">•</span>' + seriesName[14] + '</td><td>' + insrt[7] + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 20px;"><tr><td width="125"><span class="tlTip darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + 't</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + 't</td><td></td></tr></table>' : '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[14] + '</span></td><td class="tooltpStyle" align="center">' + insrt[0] + '</td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  darkMagenda">•</span>' + seriesName[9] + '</td><td> ' + insrt[11] + ' t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>' + seriesName[10] + '</td><td> ' + insrt[12] + ' t</td><td></td></tr></table>';
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
                    top: -160
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

        grid: {
            left: 50,
            right: 50,
            height: '160'
        },

        xAxis: {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: false,
            axisPointer: {
                label: {
                    show: true
                }
            }
        },

        yAxis: [
          {
              type: 'value',
              name: "FOC t",
              nameLocation: 'end',
              max: maxvalfo,
              axisPointer: {
                  label: {
                      show: false
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
              nameLocation: 'end',
              max: maxvaldo,
              name: "DOC t",
              position: 'right',
              axisLine: {
                  lineStyle: {
                      color: colors[1]
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
          }
        ],
        series: [{
            name: 'FOC t',
            type: 'line',
            data: seriesData[11],
            symbolSize: 10
        }, {
            name: 'DOC t',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[12],
            symbolSize: 10
        }]
    };


    require.config({
        paths: {
            echarts2: '../js/echartsAll3'
        }
    });

    require(
      ['echarts2'],
      function (ec) {
          var graphFilDiv = document.getElementById("voyGraphC");
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          voyGraphStateC = myChartPerf;
      }
    );
}
