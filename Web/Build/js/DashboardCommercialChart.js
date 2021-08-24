//*****************Commercial Performance Graph********************
//******************************************************************************

function resetGraph(num) {
    switch (num) {
        case 1:
            cpGraphState1.resize();
            break;

        case 2:
            cpGraphState2.resize();
            break;

        case 3:
            cpGraphState3.resize();
            break;

        case 4:
            cpGraphState4.resize();
            break;

        case 5:
            cpGraphState5.resize();
            cpGraphState5a.resize();
            cpGraphState5b.resize();
            break;
    }
}

var startDate = "";
var endDate = "";
var StartdateLabel = "";
var EnddateLabel = "";
function mpchart01(graphDiv, seriesData1, dateRange1) {

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


                // var windDev = insrt[2] - insrt[1];
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

                voyageParam.forEach(function (itm, index, arr) {
                        startDate = arr[index][0];
                        endDate = arr[index][1];
                        StartdateLabel = arr[index][2];
                        EnddateLabel = arr[index][3];

                });


                // console.log(insrt[10]);

                tip = '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[0] + '</span></td><td class="tooltpstyle" align="center" width="30%"><b>' + insrt[12] + '</b></td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[2] + '</span></td></tr><tr><td class="tooltpStyle">' + StartdateLabel + '</td><td colspan="2" style="color:#1B56FF">' + startDate + '</td></tr><tr><td class="tooltpStyle">' + EnddateLabel + '</td><td  colspan="2" style="color:#1B56FF">' + endDate + '</td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Speed</td</tr><tr><td><span class="tlTip style2">•</span>' + seriesName[3] + '</td><td> ' + insrt[3] + ' Kts</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[4] + '</td><td>' + insrt[4] + ' Kts</td><td></td></tr><tr style="margin-bottom: 20px;"><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kts</span></td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Fuel</td</tr><tr><td><span class="tlTip style4">•</span>' + seriesName[5] + '</td><td width="80"> ' + insrt[5] + ' MT/d</td><td width="40"></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[6] + '</td><td> ' + insrt[6] + ' MT/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip style6">•</span>' + seriesName[7] + '</td><td> ' + insrt[7] + ' MT/d</td><td></td></tr><tr><td><span class="tlTip style5">•</span>' + seriesName[8] + '</td><td> ' + insrt[8] + ' MT/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + doStyle + '">' + doDevPercent + '</span></td><td><span class="' + doStyle + '">' + Math.abs(doDev.toFixed(2)) + ' MT/d</span></td></tr></table>';

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
            //markLine: {
            //    data: [{
            //        name: 'Markline between two points',
            //        yAxis: 11
            //    }],
            //    label: {
            //        normal: {
            //            show: true,
            //            formatter: 'Running Average'
            //        }
            //    },
            //    lineStyle: {
            //        normal: {
            //            color: 'blue'
            //        }
            //    }
            //}
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
            yAxisIndex:1

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
                label:{
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
          cpGraphState1 = myChartPerf;
      }
    );
}



// Fuel Oil Consumption
function mpchart02(graphDiv, seriesData1, dateRange1) {
    var option = null;
    var colors = ['#FF5967','#FEDD55', '#0A7F1E', '#3B5082', '#188FCC', '#F9290C', '#B55C4F', '#450069', '#00F'];  //'#5D8B41'];  // 00FF7C

    //var dateRange = dateRangeOrg;
    var dateRange = dateRange1;
    var seriesName = ['M/E FO', 'M/E DO','A/E', 'IGG', 'C/Engine', 'C/Heating', 'Tank Cleaning', 'Others','CP FOC'];

    var seriesData = seriesData1;
    var maxY = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1]), Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4]), Math.max.apply(null, seriesData[5]), Math.max.apply(null, seriesData[6])));
    //var seriesData = [
    //  [28, 35, 40, 32, 39, 28, 40], // M/E (MT/Day)  0
    //  [1, 1, 2, 1.4, 1.5, 2, 1], // A/E (MT/Day   1
    //  [1, 1, 0, 0, 1, 0, 1], // IGG (MT/Day)   2
    //  [0, 0, 1, 2, 0, 1, 1], // C/Engine (MT/Day)  3
    //  [1, 2, 1, 2, 1, 1, 2], // C/Heating (MT/Day)  4
    //  [0, 0, 1, 1, 0, 1, 0], // Tank Cleaning (MT/Day)  5
    //  [0, 0, 1, 0, 0, 1, 0], //  Others (MT/Day) 6
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status 7
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition 8
    //  [], // total values of ME FO (9)
    //  [], // total Values of AE (10)
    //  [], // total values of IGG (11)
    //  [], // total Values of Cargo Pump (12)
    //  [], // total Values of Cargo Heating (13)
    //  [], // total Values of Tank Cleaning (14)
    //  [], // total Values of other (15)
    //  [2, 3, 5, 3, 3, 6, 3], // actual wind  (16)      
    //  [102, 102, 102, 102, 102, 102, 102]  // Voyage Number  (17)
    //  [102, 102, 102, 102, 102, 102, 102]  // CP Foc  (18)
    //  [102, 102, 102, 102, 102, 102, 102]  // M/E MDO (19)

    //  [102, 102, 102, 102, 102, 102, 102]  // M/E FO actual (20)
    //  [102, 102, 102, 102, 102, 102, 102]  // A/e actual  (21)
    //  [102, 102, 102, 102, 102, 102, 102]  // IGG actual  (22)
    //  [102, 102, 102, 102, 102, 102, 102]  // C/engine actual  (23)
    //  [102, 102, 102, 102, 102, 102, 102]  // c/heating actual  (24)
    //  [102, 102, 102, 102, 102, 102, 102]  // tank cleaning actual (25)
    //  [102, 102, 102, 102, 102, 102, 102]  // other actual  (26)
    //  [102, 102, 102, 102, 102, 102, 102]  // actual runing hrs  (27)
    //  [], // total Values of ME MDO (28)
    //  [102, 102, 102, 102, 102, 102, 102]  // M/E DO actual (29)
    //  [], // total Run hours (30) 
    //  [], // Cumulative values of ME FO(31)
    //  [], // Cumulative Values of ME DO (32)
    //  [], // Cumulative Values of AE (33)
    //  [], // Cumulative values of IGG (34)
    //  [], // Cumulative Values of Cargo Pump (35)
    //  [], // Cumulative Values of Cargo Heating (36)
    //  [], // Cumulative Values of Tank Cleaning (37)
    //  [], // Cumulative Values of others (38)
    //  [], // Total prorate for ME (39)
    //  [102, 102, 102, 102, 102, 102, 102]  // A/E DO actual (40)
    //  [102, 102, 102, 102, 102, 102, 102]  // IGG DO actual (41)
    //  [102, 102, 102, 102, 102, 102, 102]  // Cargo Engine DO actual (42)
    //  [102, 102, 102, 102, 102, 102, 102]  // Cargo heating DO actual (43)
    //  [102, 102, 102, 102, 102, 102, 102]  // Tank Cleaning DO actual (44)
    //  [102, 102, 102, 102, 102, 102, 102]  // Other DO actual (45)
    //  [], // Total AE DO hours (46) 
    //  [], // total IGG Do (47) 
    //  [], // total Cargo Engine DO (48) 
    //  [], // total Cargo Heating DO (49) 
    //  [], // total Tank Cleaning DO (50) 
    //  [], // total Other DO (51) 

    //  [], // Total prorate for AE (52)
    //  [], // Total prorate for IGG (53)
    //  [], // Total prorate for C/E (54)
    //  [], // Total prorate for C/HTG (55)
    //  [], // Total prorate for Tank Clean (56)
    //  [], // Total prorate for OTHER (57)
    //];

    //function checkGen(g1, g2, g3, g4)

    var seriesUnit = ['kW', 'hr', 'kW', 'hr', 'kW', 'hr', 'kW', 'hr'];
    lastidx = 0;
    seriesData[0].forEach(function (itm, idx, arr)   {

        if (!idx) {
                seriesData[9][idx]  = seriesData[20][idx];
                seriesData[10][idx] = seriesData[21][idx];
                seriesData[11][idx] = seriesData[22][idx];
                seriesData[12][idx] = seriesData[23][idx];
                seriesData[13][idx] = seriesData[24][idx];
                seriesData[14][idx] = seriesData[25][idx];
                seriesData[15][idx] = seriesData[26][idx];
                seriesData[28][idx] = seriesData[29][idx];
                seriesData[30][idx] = seriesData[27][idx];

                seriesData[46][idx] = seriesData[40][idx];
                seriesData[47][idx] = seriesData[41][idx];
                seriesData[48][idx] = seriesData[42][idx];
                seriesData[49][idx] = seriesData[43][idx];
                seriesData[50][idx] = seriesData[44][idx];
                seriesData[51][idx] = seriesData[45][idx];
             }
        else {
                seriesData[9][idx]  = Number(Number(seriesData[9][idx - 1]) +  Number(seriesData[20][idx])).toFixed(2); // total values for ME FO (10)
                seriesData[10][idx] = Number(Number(seriesData[10][idx - 1]) + Number(seriesData[21][idx])).toFixed(2); // total values for AE (10)
                seriesData[11][idx] = Number(Number(seriesData[11][idx - 1]) + Number(seriesData[22][idx])).toFixed(2); // total values for IGG (10)
                seriesData[12][idx] = Number(Number(seriesData[12][idx - 1]) + Number(seriesData[23][idx])).toFixed(2); // total values for Cargo Pump (10)
                seriesData[13][idx] = Number(Number(seriesData[13][idx - 1]) + Number(seriesData[24][idx])).toFixed(2); // total values for Cargo Heating (10)
                seriesData[14][idx] = Number(Number(seriesData[14][idx - 1]) + Number(seriesData[25][idx])).toFixed(2); // total values for Tank Cleaning (10)
                seriesData[15][idx] = Number(Number(seriesData[15][idx - 1]) + Number(seriesData[26][idx])).toFixed(2); // total values for others(10)
                seriesData[28][idx] = Number(Number(seriesData[28][idx - 1]) + Number(seriesData[29][idx])).toFixed(2); // total values for ME DO (10)
                seriesData[30][idx] = Number(Number(seriesData[30][idx - 1]) + Number(seriesData[27][idx])).toFixed(2); // total values for running hours (10)

                seriesData[46][idx] = Number(Number(seriesData[46][idx - 1]) + Number(seriesData[40][idx])).toFixed(2); // total values for running hours (10)
                seriesData[47][idx] = Number(Number(seriesData[47][idx - 1]) + Number(seriesData[41][idx])).toFixed(2); // total values for running hours (10)
                seriesData[48][idx] = Number(Number(seriesData[48][idx - 1]) + Number(seriesData[42][idx])).toFixed(2); // total values for running hours (10)
                seriesData[49][idx] = Number(Number(seriesData[49][idx - 1]) + Number(seriesData[43][idx])).toFixed(2); // total values for running hours (10)
                seriesData[50][idx] = Number(Number(seriesData[50][idx - 1]) + Number(seriesData[44][idx])).toFixed(2); // total values for running hours (10)
                seriesData[51][idx] = Number(Number(seriesData[51][idx - 1]) + Number(seriesData[45][idx])).toFixed(2); // total values for running hours (10)

        }
        seriesData[39][idx] = Number(((Number(seriesData[9][idx]) + Number(seriesData[28][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);

        seriesData[52][idx] = Number(((Number(seriesData[10][idx]) + Number(seriesData[46][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);
        seriesData[53][idx] = Number(((Number(seriesData[11][idx]) + Number(seriesData[47][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);
        seriesData[54][idx] = Number(((Number(seriesData[12][idx]) + Number(seriesData[48][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);
        seriesData[55][idx] = Number(((Number(seriesData[13][idx]) + Number(seriesData[49][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);
        seriesData[56][idx] = Number(((Number(seriesData[14][idx]) + Number(seriesData[50][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);
        seriesData[57][idx] = Number(((Number(seriesData[15][idx]) + Number(seriesData[51][idx])) / Number(seriesData[30][idx])) * 24).toFixed(2);

        seriesData[31][idx] = Number(Number((Number(seriesData[9][idx])) / (Number(seriesData[9][idx]) + Number(seriesData[28][idx]))) * Number(seriesData[39][idx])).toFixed(2);
        seriesData[32][idx] = Number(Number((Number(seriesData[28][idx])) / (Number(seriesData[9][idx]) + Number(seriesData[28][idx]))) * Number(seriesData[39][idx])).toFixed(2);
        seriesData[33][idx] = Number(Number((Number(seriesData[10][idx])) / (Number(seriesData[10][idx]) + Number(seriesData[46][idx]))) * Number(seriesData[52][idx])).toFixed(2);
        seriesData[34][idx] = Number(Number((Number(seriesData[11][idx])) / (Number(seriesData[11][idx]) + Number(seriesData[47][idx]))) * Number(seriesData[53][idx])).toFixed(2);
        seriesData[35][idx] = Number(Number((Number(seriesData[12][idx])) / (Number(seriesData[12][idx]) + Number(seriesData[48][idx]))) * Number(seriesData[54][idx])).toFixed(2);
        seriesData[36][idx] = Number(Number((Number(seriesData[13][idx])) / (Number(seriesData[13][idx]) + Number(seriesData[49][idx]))) * Number(seriesData[55][idx])).toFixed(2);
        seriesData[37][idx] = Number(Number((Number(seriesData[14][idx])) / (Number(seriesData[14][idx]) + Number(seriesData[50][idx]))) * Number(seriesData[56][idx])).toFixed(2);
        seriesData[38][idx] = Number(Number((Number(seriesData[15][idx])) / (Number(seriesData[15][idx]) + Number(seriesData[51][idx]))) * Number(seriesData[57][idx])).toFixed(2);


        lastidx = lastidx + 1;
    });
    option = {

        title: {
            text: 'Fuel Oil Consumption (MT)',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'M/E - A/E - IGG - C/Engine - C/Heating - Tank Cleaning - Others',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            left: 'center'

        },
        legend: {
            data: [
            { name: seriesName[0] },
            { name: seriesName[1] },
            { name: seriesName[2] },
            { name: seriesName[3] },
            { name: seriesName[4] },
            { name: seriesName[5] },
            { name: seriesName[6] },
            { name: seriesName[7] },
            { name: seriesName[8] },
            ],
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

                var windDev = insrt[16] - cpBF;

                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });


                var tip = "";
                tip = '<table class="tooltipTable" style="width: 100%"><tr><td class="tooltpStyle" align="center">' + insrt[8] + '</td><td class="tooltpstyle" align="center" width="40%"><b>' + insrt[7] + '</b></td><td align="center"><span class="' + windStyle + '"> Bf-' + insrt[16] + '</span></td></tr><tr><td class="tooltpStyle">' + StartdateLabel + '</td><td  colspan="2" style="color:#1B56FF">' + startDate + '</td></tr><tr><td class="tooltpStyle">'+ EnddateLabel +'</td><td  colspan="2" style="color:#1B56FF">' + endDate + '</td></tr></table><table class="tooltipTable" style="margin - bottom:10px; width: 320px"><tr><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">Fuel Oil Consumption</td><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">On <br> <span id="onDate">' + dateRange[result] + '</span></td><td class="tooltpStyle" align="center" style="background: #888; color: #fff" width="">Cumulative <br>Consumption</td></tr><tr><td class="tooltpStyle" align="left">M/E FO</td><td class="tooltpStyle" align="right"> <span style="color: white; background: ' + colors[0] + '; padding: 0 5px">' + insrt[0] + '</span> MT</td><td align="right" style="background: #ffefef; ; color: #777">' + insrt[31] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">M/E DO</td><td class="tooltpStyle" align="right"> <span style="color: #000; background: ' + colors[1] + '; padding: 0 5px">' + insrt[19] + '</span> MT</td><td align="right" style="background: #ffefef; ; color: #777">' + insrt[32] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">Auxiliary Engine</td><td class="tooltpStyle" align="right"> <span style="color: #fff; background: ' + colors[2] + '; padding: 0 5px">' + insrt[1] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[33] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">IGG</td><td class="tooltpStyle" align="right"> <span style="color: #fff; background: ' + colors[3] + '; padding: 0 5px">' + insrt[2] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[34] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">Cargo Engine (Pump)</td><td class="tooltpStyle" align="right"> <span style="color: white; background: ' + colors[4] + '; padding: 0 5px">' + insrt[3] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[35] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">Cargo Heating</td><td class="tooltpStyle" align="right"> <span style="color: white;background: ' + colors[5] + '; padding: 0 5px">' + insrt[4] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[36] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">Tank Cleaning</td><td class="tooltpStyle" align="right"> <span style="color: white;background: ' + colors[6] + '; padding: 0 5px">' + insrt[5] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[37] + ' MT</td></tr><tr><td class="tooltpStyle" align="left">Others</td><td class="tooltpStyle" align="right"> <span style="color: #fff;background: ' + colors[7] + '; padding: 0 5px">' + insrt[6] + '</span> MT</td><td align="right" style="background: #ffefef; color: #777">' + insrt[38] + ' MT</td></tr></table>';

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
              axisPointer:{
                  show : true,
                  label:{
                      show:true
                }
              },
              splitLine: {
                  show: true,
                  lineStyle: {
                      type: 'dashed',
                      color: '#777'
                  }
              }
          }

        ],

        yAxis: [{
            name: 'Fuel Oil Consumption (MT)',
            nameLocation: 'center',
            min: 0,
            // max: kwMaxLimit,
            nameRotate: 90,
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
            axisLine: {
                lineStyle: {
                    color: '#000'//colors[0]
                }
            },
            type: 'value'
        },
        {
            // name: 'Fuel Oil Consumption (MT/Day)',
            nameLocation: 'center',
            min: 0,
            nameRotate: 90,
            nameTextStyle: {
                padding: 10,
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
        }
        ],


        series: [{
            name: seriesName[0],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 45,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            stack: 'FO-MDO',
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#333',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            stack: 'FO-MDO',
            data: seriesData[19]
        },
        {
            name: seriesName[2],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#333',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[1]
        },
        {
            name: seriesName[3],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[2]
        },
        {
            name: seriesName[4],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[3]
        },
        {
            name: seriesName[5],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[4]
        },
        {
            name: seriesName[6],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
           // yAxisIndex: 1,
            data: seriesData[5]
        },
        {
            name: seriesName[7],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    //color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[6]
        },
        {
            name: seriesName[8],
            type: 'line',
            label: {
                normal: {
                    show: true,
                    //rotate: 90,
                    distance: 15,
                    color:'#00F',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) {
                            return ''
                        }
                    }
                }
            },
            barGap: '5%',
            //yAxisIndex: 1,
            data: seriesData[18]
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

    var cumulativData = "";
    cumulativData = '<table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">';
    cumulativData = cumulativData + '<tbody>';
    cumulativData = cumulativData + '<tr>';
    cumulativData = cumulativData + '<td colspan="8" style="padding: 5px; color: white; background: #333;" align="center">Cumulative Consumption from COSP, as on ' + dateRange[lastidx - 1] + ' </td>';
    cumulativData = cumulativData + '</tr>';
    cumulativData = cumulativData + '<tr>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">M/E FO</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">M/E DO</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #ccc; padding: 5px 10px; font-weight: normal" align="center">Auxiliary Engine</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">IGG</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">Cargo Engine (Pump)</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #ccc; padding: 5px 10px; font-weight: normal" align="center">Cargo Heating</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">Tank Cleaning</td>';
    cumulativData = cumulativData + '<td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">Others</td>';
    cumulativData = cumulativData + '</tr>';
    cumulativData = cumulativData + '<tr>';
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: white; background: #2a7b9b; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalFOC01">' + Number(seriesData[31][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>'
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: white; background: #2a7b9b; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalDOC01">' + Number(seriesData[32][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>'
    cumulativData = cumulativData + '<td style="padding: 10px 0" align="center">';
    cumulativData = cumulativData + '<span style="color: white; background: #2a7b9b; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span>' + Number(seriesData[33][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '<td style="padding: 10px 0" align="center">';
    cumulativData = cumulativData + '<span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span>' + Number(seriesData[34][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalFO2">' + Number(seriesData[35][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalFO3">' + Number(seriesData[36][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalFOC4">' + Number(seriesData[37][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '<td align="center">';
    cumulativData = cumulativData + '<span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">';
    cumulativData = cumulativData + '<span id="totalFOC5">' + Number(seriesData[38][lastidx - 1]).toFixed(2) + '</span> MT</span>';
    cumulativData = cumulativData + '</td>';
    cumulativData = cumulativData + '</tr>';
    cumulativData = cumulativData + '</tbody>';
    cumulativData = cumulativData + '</table>';

    $("#divCumulative").html(cumulativData);
}



// Average FOC/Nm
function mpchart03(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#22F', '#FE8A00', '#7D5F03', '#DE5B49','#F00'];
    var dateRange = dateRange1;

    var seriesName = ['Vessel Speed (Kts)', 'FOC Rate (kg/Nm)', 'EEOI (g-CO2/T-nm)', 'Fuel Consumption (MT)', 'Cargo Carried (MT)'];
    var seriesData = seriesData1;
    //var seriesData = [

    //  [13.8, 13.4, 13, 14, 13.9, 14, 15],  /// Vsl Speed kts  (0)
    //  [31, 30, 32, 31.3, 31.8, 32, 31],   /// FOC kg/Nm  (1)
    //  [0, 0, 0, 1.5, 2.1, 0.3, 8.8, 2.1, 2, 1.5, 4.5, 2.5],  // EEOI  (2)
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status  (3)
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition   (4)        
    //  [2, 3, 5, 3, 3, 6, 3], // actual wind  (5)      
    //  [102, 102, 102, 102, 102, 102, 102],  // Voyage Number  (6)
    //  [20, 23, 16, 19, 20, 23, 23], // Distance observed  (7)
    //  [20, 23, 16, 19, 20, 23, 23],  // Fuel Consumed / nm  using formula (8)
    //  [], // Cumulative Full Speed hours (9)
    //  [], // Cumulative Values of distance  (10)
    //  [], // Cumulative Values of FOC (11)
    //  [],  //  cumulative value of /Nm  (12)
    //  [12, 14, 15, 20, 23, 22, 23] // Full Speed hours (13)
    //  [12, 14, 15, 20, 23, 22, 23] // Cargo Carried(14)
    //  [12, 14, 15, 20, 23, 22, 23] // Actual consumption(15)
    //  []total hrs //16
    //  []total distance //17
    //  []cumulative total hrs //18
    //  []cumulative total distance //19
    //  []cumulative sov //20
    //  []cumulative Actual FOC //21
    //  []cumulative FOC MT/d  //22 
    //  []cumulative FOC rate kg/nm  //23
    //];

    //seriesData[0].forEach(function (itm, idx, arr) {
    //    // Populating values of Kg/Nm
    //    //seriesData[8][idx] = (Number(seriesData[1][idx]) * 1000 / Number(seriesData[7][idx])).toFixed(2);

    //    // Populating: Cumulative values of FOC /Nm
    //    seriesData[12][idx] = (Number(seriesData[11][idx]) * 1000 / Number(seriesData[10][idx])).toFixed(2);
    //});
    var seriesUnit = ['°C', '°C', '°C', '°C', '°C', '°C'];

    var maxEEOI = Number(Math.max(...seriesData[2]));
    var Maxcargo = Math.ceil(Number(Math.max(...seriesData[14])));
    Maxcargo += Maxcargo > 0 ?  Math.ceil(Maxcargo * 0.2) : Maxcargo;
    var Mincargo = Math.ceil(Number(Math.min(...seriesData[14])));
    Mincargo -= Mincargo > 0 ? Math.ceil(Mincargo * 0.2) : Mincargo;
    var MaxSpeed = Math.ceil(Number(Math.max(...seriesData[0])));
    MaxSpeed += MaxSpeed > 0 ? Math.ceil(MaxSpeed * 0.2) : MaxSpeed;
    var MinSpeed = Math.ceil(Number(Math.min(...seriesData[0])));
    MinSpeed -= MinSpeed > 0 ? Math.ceil(MinSpeed * 0.2) : MinSpeed;
    var MaxFoc = Math.ceil(Number(Math.max(...seriesData[1])));
    MaxFoc += MaxFoc > 0 ? Math.ceil(MaxFoc * 0.2) : MaxFoc;
    var MinFoc = Math.ceil(Number(Math.min(...seriesData[1])));
    MinFoc -= MinFoc > 0 ? Math.ceil(MinFoc * 0.2) : MinFoc;
    var MaxFocRate = Math.ceil(Number(Math.max(...seriesData[8])));
    MaxFocRate += MaxFocRate > 0 ? Math.ceil(MaxFocRate * 0.2) : MaxFocRate;
    var MinFocRate = Math.ceil(Number(Math.min(...seriesData[8])));
    MinFocRate -= MinFocRate > 0 ? Math.ceil(MinFocRate * 0.2) : MinFocRate;
    console.log("max:" + Maxcargo + "min cargo:" + Mincargo + "max speed:" + MaxSpeed + "min speed:" + MinSpeed);
    maxEEOI = (maxEEOI + maxEEOI * 0.1).toFixed(2);

    option = {
        title: {
            text: 'Fuel Efficiency of the Vessel',
            subtext: 'kg/Nm & EEOI',
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
            left: 220,
            right: 150
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

                var windDev = insrt[5] - cpBF;

                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });
                seriesData[0].forEach(function (itm, idx, arr) {

                    if (!idx) {
                        seriesData[9][idx] = Number(seriesData[13][idx]);
                        seriesData[10][idx] = Number(seriesData[7][idx]);
                        seriesData[11][idx] = Number(seriesData[1][idx]);
                        //seriesData[12][idx] = Number(seriesData[8][idx]);
                        seriesData[18][idx] = Number(seriesData[16][idx]);
                        seriesData[19][idx] = Number(seriesData[17][idx]);
                        seriesData[21][idx] = Number(seriesData[15][idx]);
                    }
                    else {
                        seriesData[9][idx] = (Number(seriesData[9][idx - 1]) + Number(seriesData[13][idx])).toFixed(2); // Populating: Cumulative Full Speed hours 
                        seriesData[10][idx] = (Number(seriesData[10][idx - 1]) + Number(seriesData[7][idx])).toFixed(2); // Populating: Cumulative values of Distance
                        seriesData[11][idx] = (Number(seriesData[11][idx - 1]) + Number(seriesData[1][idx])); // Populating: Cumulative values of FOC
                        //seriesData[12][idx] = (Number(seriesData[12][idx - 1]) + Number(seriesData[8][idx])); // Populating: Cumulative values of FOC /Nm

                        seriesData[18][idx] = (Number(seriesData[18][idx - 1]) + Number(seriesData[16][idx])); // Populating: Cumulative values of total hrs
                        seriesData[19][idx] = (Number(seriesData[19][idx - 1]) + Number(seriesData[17][idx])); // Populating: Cumulative values of total dist nm
                        seriesData[21][idx] = (Number(seriesData[21][idx - 1]) + Number(seriesData[15][idx])).toFixed(2); // Populating: Cumulative values of actual FOC MT
                    }

                    // Populating: Cumulative values of Kg/Nm
                    //seriesData[8][idx] = (((Number(seriesData[1][idx]) * Number(seriesData[13][idx])) / Number(seriesData[7][idx])) * 1000);
                    //seriesData[8][idx] = (Number(seriesData[1][idx]) * 1000 / Number(seriesData[7][idx])).toFixed(2);
                    // Populating: Cumulative values of FOC /Nm
                    seriesData[12][idx] = (Number(seriesData[11][idx]) * 1000 / Number(seriesData[10][idx])).toFixed(2);
                    seriesData[20][idx] = (Number(seriesData[19][idx]) / Number(seriesData[18][idx])).toFixed(2);
                    seriesData[22][idx] = ((Number(seriesData[21][idx]) / Number(seriesData[18][idx])) * 24).toFixed(2);

                    seriesData[23][idx] = ((Number(seriesData[21][idx]) / Number(seriesData[19][idx])) * 1000).toFixed(2);
                });


                var tip = "";
                // tip = '<table class="tooltipTable" style="width: 100%"><tr><td class="tooltpStyle" align="center">' + insrt[4] + '</td><td class="tooltpstyle" align="center" width="40%"> <b>' + insrt[3] + '</b></td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[5] + '</span></td></tr><tr><td class="tooltpStyle">COSP</td><td colspan="2" align="center" style="color:#1B56FF">' + startDate + '</td></tr><tr><td class="tooltpStyle">EOSP</td><td colspan="2" align="center" style="color:#1B56FF">' + endDate + '</td></tr></table><table class="tooltipTable" style="margin - bottom:10px; width: 320px"><tr><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">Conditions</td><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">On <br> <span id="onDate">' + dateRange[result] + '</span></td><td class="tooltpStyle" align="center" style="background: #888; color: #fff" width="">Cumulative <br>Totals</td></tr><tr><td class="tooltpStyle" align="left">Full Speed Hours (Hrs)</td><td class="tooltpStyle" align="right"> <span style="padding: 0 5px">' + insrt[13] + '</span></td><td align="right" style="background: #ffefef; ; color: #777">' + insrt[9] + '</td></tr><tr><td class="tooltpStyle" align="left">Distance Observed at Full Speed (Nm)</td><td class="tooltpStyle" align="right">' + insrt[7] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[10] + '</td></tr><tr><td class="tooltpStyle" align="left" >General Average Speed (Kts)</td><td class="tooltpStyle" align="right"><span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[0] + '</span></td><td align="right" style="background: #ffefef; color: #777">' + insrt[20] + '</td></tr><tr><td class="tooltpStyle" align="left" >Actual Fuel Cons (MT)</td><td class="tooltpStyle" align="right">' + insrt[15] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[21] + '</td></tr><tr><td class="tooltpStyle" align="left">Fuel Consumption (MT/d)</td><td class="tooltpStyle" align="right"> <span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[1] + '</span></td><td align="right" style="background: #ffefef; color: #777">' + insrt[22] + '</td></tr><tr><td class="tooltpStyle" align="left">FOC Rate (kg/Nm)</td><td class="tooltpStyle" align="right"><span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[8] + '</span></td><td align="right" style="background: #ffefef; color: #777">' + insrt[23] + '</td></tr><tr><td class="tooltpStyle" align="left">EEOI (g-CO<sub>2</sub>/T-nm)</td><td class="tooltpStyle" align="right"> <span style="color: #fff;background: ' + colors[0] + '; padding: 0 5px">' + insrt[2] + '</span></td><td align="right" style="background: #ffefef; color: #777"></td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[4] + '</td><td class="tooltpStyle" align="right"> <span style="color: #fff;background: ' + colors[4] + '; padding: 0 5px">' + insrt[14] + '</span></td><td align="right" style="background: #ffefef; color: #777"></td></tr></table>';
                tip = '<table class="tooltipTable" style="width: 100%"><tr><td class="tooltpStyle" align="center">' + insrt[4] + '</td><td class="tooltpstyle" align="center" width="40%"> <b>' + insrt[3] + '</b></td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[5] + '</span></td></tr><tr><td class="tooltpStyle">'+ StartdateLabel +'</td><td colspan="2" align="center" style="color:#1B56FF">' + startDate + '</td></tr><tr><td class="tooltpStyle">'+ EnddateLabel +'</td><td colspan="2" align="center" style="color:#1B56FF">' + endDate + '</td></tr></table><table class="tooltipTable" style="width: 100%"><tr><td style="background: #777; color: white; font-weight: 700;text-align: center;">Vessel Speed</td><td style="background: #777; color: white; font-weight: 700;text-align: center;">General Average Speed</td></tr><tr> <td class="tooltpStyle" align="center"> <span style="color: white; background: ' + colors[1] + '; padding: 0 5px">' + insrt[0] + '</span> Kts</td><td align="center" style="background: #ffefef; color: #777">' + insrt[20] + ' Kts</td></tr></table><table class="tooltipTable" style="margin - bottom:10px; width: 320px"><tr><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">Conditions</td><td class="tooltpStyle" align="center" style="background: #555; color: #fff" width="">On <br> <span id="onDate">' + dateRange[result] + '</span></td><td class="tooltpStyle" align="center" style="background: #888; color: #fff" width="">Cumulative <br>Totals</td></tr><tr><td class="tooltpStyle" align="left">Full Speed Hours (Hrs)</td><td class="tooltpStyle" align="right"> <span style="padding: 0 5px">' + insrt[13] + '</span></td><td align="right" style="background: #ffefef; ; color: #777">' + insrt[9] + '</td></tr><tr><td class="tooltpStyle" align="left">Distance Observed at Full Speed (Nm)</td><td class="tooltpStyle" align="right">' + insrt[7] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[10] + '</td></tr><tr><td class="tooltpStyle" align="left">Actual Fuel Cons (MT)</td><td class="tooltpStyle" align="right">' + insrt[15] + '</td><td align="right" style="background: #ffefef; color: #777">' + insrt[21] + '</td></tr><tr><td class="tooltpStyle" align="left">Fuel Consumption (MT/d)</td><td class="tooltpStyle" align="right"> <span style="color: white; background: ' + colors[2] + '; padding: 0 5px">' + insrt[1] + '</span></td><td align="right" style="background: #ffefef; color: #777">' + insrt[22] + '</td></tr><tr><td class="tooltpStyle" align="left">FOC Rate (kg/Nm)</td><td class="tooltpStyle" align="right"> <span style="color: white; background: ' + colors[3] + '; padding: 0 5px">' + insrt[8] + '</span></td><td align="right" style="background: #ffefef; color: #777">' + insrt[23] + '</td></tr><tr><td class="tooltpStyle" align="left">EEOI (g-CO<sub>2</sub>/T-nm)</td><td class="tooltpStyle" align="right"> <span style="color: #fff;background: ' + colors[0] + '; padding: 0 5px">' + insrt[2] + '</span></td><td align="right" style="background: #ffefef; color: #777"></td></tr><tr><td class="tooltpStyle" align="left">' + seriesName[4] + '</td><td class="tooltpStyle" align="right"> <span style="color: #fff;background: ' + colors[4] + '; padding: 0 5px">' + insrt[14] + '</span></td><td align="right" style="background: #ffefef; color: #777"></td></tr></table>';
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
                  name: seriesName[2],
                  // icon: 'image://assets//img/mainEngine/mainEngine_etmax.svg'
              }, {
                  name: seriesName[0],
                  // icon: 'image://assets//img/mainEngine/mainEngine_etmin.svg'
              },
              {
                  name: seriesName[3],
                  // icon: 'image://assets//img/mainEngine/mainEngine_etmin.svg'
              },
              {
                  name: seriesName[1],
              },
              {
                  name: seriesName[4]
              }

            ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            axisPointer: {
                show: true,
                label: {
                    show: true
                }
            },
            data: dateRange
        },
        yAxis:
          [
            {
                name: 'Cargo Carried (MT)',
                nameLocation: 'center',
                index: 0,
                min: Mincargo,
                max: Maxcargo,
                nameRotate: 90,
                position: 'left',
                nameTextStyle: {
                    padding: 50,
                    fontWeight: 700,
                    fontSize: 14
                },
                offset: 140,
                axisPointer: {
                    show: true,
                    label: {
                        show: true
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: colors[4]
                    }
                },
                type: 'value',
                splitLine: {
                    show: false
                }
            },
            {
                name: 'EEOI (g-CO2/T-nm)',
                nameLocation: 'center',
                index: 1,
                offset: 70,
                nameRotate: 90,
                position: 'left',
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
                type: 'value',
                splitLine: {
                    show: false
                }
            },
            {
                name: 'Vessel Speed (Kts)',
                nameLocation: 'center',
                index: 2,
                max: MaxSpeed,
                min: MinSpeed,
                nameRotate: 90,
                position: 'left',
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
            },
            {
                name: 'Fuel Consumption (MT)',
                nameLocation: 'center',
                index: 3,
                min: MinFoc,
                max: MaxFoc,
                nameRotate: 90,
                position: 'right',
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
                        color: colors[2]
                    }
                },
                type: 'value',
                splitLine: {
                    show: true
                }
            },
            {
                name: 'FOC Rate (kg/Nm)',
                nameLocation: 'center',
                index: 4,
                min: MinFocRate,
                max: MaxFocRate,
                nameRotate: 90,
                position: 'right',
                nameTextStyle: {
                    padding: 20,
                    fontWeight: 700,
                    fontSize: 14
                },
                offset: 70,
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
            }

          ],

        series: [

          {
              name: seriesName[2],
              type: 'line',
              yAxisIndex: 1,
              data: seriesData[2]
          },
          {
              name: seriesName[0],
              type: 'line',
              yAxisIndex: 2,
              data: seriesData[0]
          },
          {
              name: seriesName[3],
              type: 'line',
              yAxisIndex: 3,
              data: seriesData[1]
          },
          {
              name: seriesName[1],
              type: 'line',
              yAxisIndex: 4,
              data: seriesData[8]
          },
          {
              name: seriesName[4],
              type: 'line',
              yAxisIndex: 0,
              data: seriesData[14]
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

// Auxiliary Boiler Fuel
function mpchart04(graphDiv, seriesData1, dateRange1) {

    var option = null;
    var colors = ['#0085B6', '#FF005D', '#0F3B66'];
    var dateRange = dateRange1;

    var seriesName = ['Boiler FO Consumption (MT/day)', 'Vessel Speed (Kts)'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  [1, 1.5, 1, 0, 0, 0.5, 0], // Boiler Fuel Oil Consumtion MT  (0)
    //  [1, 1, 3, 0, 0, 2, 0], // Boiler Water Consumption  MT   (1)
    //  [13, 12, 14, 13, 13, 12, 12], // Ship Speed Kts  - EM Log Speed fron DNR   (2)
    //  ['At Port', 'At Port', 'At Port', 'At Sea', 'At Sea', 'At Sea', 'At Sea'], // Vessel Status   (3)
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], // Vessel Condition   (4)
    //  [6, 10, 19, 20, 21, 20, 17], // Ship Full Seep hours from DNR, to be shown in the tooltip   (5)
    //  [0.5, 0, 0, 0, 0, 0, 0], // Fuel Consumed for Cargo Heating   (6)
    //  [0, 1, 0, 0, 0, 0, 0], // Fuel Consumed for tank Cleaning    (7)
    //  [0, 0, 1, 0, 0, 0, 0], // Fuel consumed for inerting   (8)
    //  [2, 1, 2, 1, 1, 2, 1] // Fuel consumed for inerting   (9)
    //];

    //var spdMax = Math.round(Math.max(...seriesData[2]) + Math.max(...seriesData[2]) * 0.1);
    // var hghConsump = (Math.max(...seriesData[0]) > Math.max(...seriesData[1]) ? Math.max(...seriesData[0]) : Math.max(...seriesData[1]));
    //var hghConsump = Math.max(...seriesData[0]);
    var spdMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[2])));

    //var maxConsump = (hghConsump + hghConsump * 0.1);
    var maxConsump = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));

    option = {
        title: {
            text: 'Auxiliary Boiler',
            subtext: 'Fuel Consumption',
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
            bottom: 100,
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
                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });
                var tip = "";
                tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 100%" ><tr><td class="tooltpstyle" align="center" width="40 %"> <b> ' + insrt[3] + '</b></td><td class="tooltpstyle" align="center" width="30 %">' + insrt[4] + '</td></tr><tr><td class="tooltpstyle" align="left" width="40 %"> <b>'+ StartdateLabel +'</b></td><td align="center" width="30 %" style="color:#1B56FF">' + startDate + '</td></tr><tr><td class="tooltpstyle" align="left" width="40 %"> <b>'+ EnddateLabel+'</b></td><td align="center" width="30 %" style="color:#1B56FF">' + endDate + '</td></tr></table> <table class="tooltipTable" style="margin - bottom: 20px; width: 320px"><tr> <td class="tooltpStyle" align="left" width="70 %"> Boiler FO Consumption </td><td align="left" width="30 %"> <span style="color: white; background:' + colors[0] + '; padding: 0 3px">' + insrt[0] + '</span> MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Vessel Speed</td><td align="left" width="30 %"> <span style="color: white; background:' + colors[1] + '; padding: 0 3px">' + insrt[2] + '</span> Kts</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Full Speed Hours</td><td align="left" width="30 %">' + insrt[5] + ' Hrs</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Cargo Heating</td><td align="left" width="30 %">' + insrt[6] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Tank Cleaning</td><td align="left" width="30 %">' + insrt[7] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Inerting</td><td align="left" width="30 %">' + insrt[8] + ' MT/day</td></tr><tr> <td class="tooltpStyle" align="left" width="70 %">Fuel Consumed for Cargo Pump</td><td align="left" width="30 %">' + insrt[9] + ' MT/day</td></tr></table>';

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
            data:
              [{
                  name: seriesName[0],
                  // icon: 'image://assets//img/mainEngine/mainEngine_mtd.svg'
              }, {
                  name: seriesName[1],
                  // icon: 'image://assets//img/mainEngine/mainEngine_mtd.svg'
              }
              ],
            textStyle: {
                fontWeight: 'bold'
            }

        },
        xAxis: {
            type: 'category',
            data: dateRange
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
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            //axisPointer: {
            //    show: true,
            //    label: {
            //        show: true
            //    }
            //},
            type: 'value'
        },

        {
            name: seriesName[1],
            nameLocation: 'center',
            nameRotate: 90,
            min: 0,
            max: spdMax,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            },
            axisLine: {
                lineStyle: {
                    color: colors[1]
                }
            },
            //axisPointer: {
            //    show: true,
            //    label: {
            //        show: true
            //    }
            //},
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
                    color: colors[0],
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: function (params) {
                        if (!params.value) { return '' } else { return params.value + '  MT' }
                    }
                }
            },
            barGap: '0%',
            data: seriesData[0]
        },
        {
            name: seriesName[1],
            type: 'line',
            yAxisIndex: 1,
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    color: colors[1],
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
          cpGraphState4 = myChartPerf;
      }
    );
}





// ::::::::::::  Resistance Limit Capture ::::::::::::::

let resLimitVal = 1;

function setResLimit() {
    resLimitVal = resLimitForm.resLimit.value;
    mpchart05a("mp01Chart05a");
}



// ::::::::::::      Resistance Limit ENDS       ::::::::::::::

// // Voyage Overview three graphs 5, 5a, 5b
var colors5 = ['#820333', '#c0392b', '#35478C', '#27ae60', '#e67e22', '#FF403C', '#FF982C', '#FFD706', '#7DE346', '#3AAEB0'];
var colors5a = ['#275936', '#FF403C', '#4682b4', '#5e005e', '#b40e93'];
var colors5b = ['#733250', '#0B877D'];
function mpchart05(graphDiv, seriesData1, dateRange1) {

    var option = null;

    var colors = colors5;

    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'M/E FOC', 'CP M/E FOC'];

    var seriesData = seriesData1;
    //var seriesData = [
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], //conditions    0
    //  [2, 2, 5, 3, 4, 6, 3], // Wind Force    1
    //  [15.3, 15.5, 12.5, 15.3, 14.9, 6, 15.1], // SOG Kn   2
    //  [15.1, 15.1, 12, 15.1, 15.1, 6, 15.2], //LOG   3
    //  [15, 15, 15, 15, 15, 15, 15],  // CP Speed   4
    //  [0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5],  // Trim    5
    //  [44.819, 44.819, 44.819, 44.819, 44.819, 44.819, 44.819],  //  Displacement    ------  End of graph A     6

    //  [82, 83, 60, 79, 80, 0, 79],  // ME Load Gov/Fuel Index   %     7
    //  [90, 90, 90, 90, 90, 90, 90],  // ME Load Limit (NCR)   %      8
    //  [5, 6, 10, 6, 5, 0, 5],  // SLIP   %     9
    //  [98, 99, 72, 98, 98, 0, 98],  // ME RPM  (rmp)   ------  End of graph B      10

    //  [19, 19, 10, 19.5, 19.5, 0, 19.5],  // FOC Rate  (t/d)   11
    //  [20, 20, 20, 20, 20, 20, 20],  // CP FOC Rate  (t/d)   ------  End of graph C    12
    //  [4, 4, 4, 4, 4, 4, 4]    //  CP Wind Speed    13
    //  [4, 4, 4, 4, 4, 4, 4]    //  vsl status   14
    //  [4, 4, 4, 4, 4, 4, 4]    //  min displacement    15
    //  [4, 4, 4, 4, 4, 4, 4]    //  max displacement    16
    //];

    var dateRange = dateRange1;
    //var maxDo = Math.ceil(Math.max(Math.max.apply(null, seriesData[7]), Math.max.apply(null, seriesData[8])));
    var maxSog = Math.ceil(Math.max(Math.max.apply(null, seriesData[2]), Math.max.apply(null, seriesData[3]), Math.max.apply(null, seriesData[4])));
    maxSog += maxSog > 0 ? Math.ceil(maxSog * 0.2) : maxSog;
    var minSog = Math.ceil(Math.min(Math.min.apply(null, seriesData[2]), Math.min.apply(null, seriesData[3]), Math.min.apply(null, seriesData[4])));
    minSog -= minSog > 0 ? Math.ceil(minSog * 0.2) : minSog;
    var maxTrim = Math.ceil(Math.max(Math.max.apply(null, seriesData[5])));
    maxTrim += maxTrim > 0 ? Math.ceil(maxTrim * 0.2) : maxTrim;
    var maxDisplace = Math.ceil(seriesData[16][0])
    maxDisplace = maxDisplace + maxDisplace * 0.2;
    var minDisplace = Math.ceil(seriesData[15][0])

    option = {
        color: colors,
        title: {
            text: 'Vessel Speed Performance',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Speed, Trim & Displacement Analysis',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            x: 'center'
        },

        legend: {
            left: 'center',
            width: 600,
            bottom: 3,
            itemGap: 30,
            data: [
              { name: 'SOG'},
              { name: 'LOG'},
              { name: 'CP Speed'},
              { name: 'Trim'},
              //          {name:'Displacement', icon:'roundRect'}
              { name: 'Displacement'}
            ],
            selected: {
                'Trim': false,
                'Displacement': false
            },
            textStyle: { fontWeight: 'bold' }
        },

        grid: [{
            top: 60,
            bottom: 100
        }],

        dataZoom: [{
            bottom: 25,
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

                // var windDev = insrt[1] - insrt[13];
                var windDev = insrt[1] - cpBF;
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[2] - insrt[4];
                //var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var speedStyle = (speedDev >= 0) ? "greenStatus" : "redStatus";
                var speedDevPercent = ((Math.abs(insrt[2] - insrt[4]) * 100) / insrt[2]).toFixed(2) + "%";

                var hfoDev = insrt[12] - insrt[11];
                //var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(2)).toString() + "%";

                var doStyle = (insrt[7] <= 80) ? "greenSpeed" : "redSpeed";

                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });

                tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr><tr><td class="tooltpStyle">'+ StartdateLabel +'</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">'+ startDate + '</td></tr><tr><td class="tooltpStyle">'+ EnddateLabel +'</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">'+ endDate + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle">' + seriesName[0] + '</td><td><span style="color: white;background: ' + colors5[0] + '; padding: 0 5px">' + insrt[2] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[1] + '</td><td><span style="color: white;background: ' + colors5[1] + '; padding: 0 5px">' + insrt[3] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[2] + '</td><td><span style="color: white;background: ' + colors5[2] + '; padding: 0 5px">' + insrt[4] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[4] + '</td><td><span style="color: white;background: ' + colors5[4] + '; padding: 0 5px">' + insrt[6] + '</span> *1000 MT</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td> <span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kn</span></td></tr><tr><td class="tooltpStyle">' + seriesName[3] + '</td><td><span style="color: white;background: ' + colors5[3] + '; padding: 0 5px">' + insrt[5] + '</span> m</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[8] + '</td><td><span style="color: white;background: ' + colors5a[2] + '; padding: 0 5px">' + insrt[10] + '</span></td></tr><tr><td class="tooltpStyle">' + seriesName[5] + '</td><td> <span style="color: white;background: ' + colors5a[3] + '; padding: 0 5px">' + insrt[7] + '</span>%</td></tr><tr><td class="tooltpStyle">' + seriesName[7] + '</td><td><span style="color: white;background: ' + colors5a[1] + '; padding: 0 5px">' + insrt[9] + '</span>%</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[9] + '</td><td><span style="color: white;background: ' + colors5b[0] + '; padding: 0 5px"> ' + insrt[11] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[10] + '</td><td><span style="color: white;background: ' + colors5b[1] + '; padding: 0 5px"> ' + insrt[12] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td> <span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table>';

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
            left: 70,
            right: 110,
            height: '160'
        },

        xAxis: {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: true,
            axisPointer: {
                label: {
                    show: true
                }
            }
        },

        yAxis: [{
            type: 'value',
            name: "Speed (Kn)",
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            max: maxSog,
            min: minSog,
            axisPointer: {
                label: {
                    show: true
                }
            },
            axisLine: {
                lineStyle: {
                    //color: colors[0]
                }
            },
            axisLabel: {
                formatter: '{value}'
            }
        }, {
            type: 'value',
            name: "LOG (Kn)",
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            offset: 60,
            show: false,
            max: maxSog,
            min: minSog,
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
            },
            splitLine: {
                show: false
            }
        }, {
            type: 'value',
            name: 'CP Speed',
            show: false,
            nameLocation: 'end',
            max: maxSog,
            min: minSog,
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
            name: 'Trim',
            show: true,
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            //max: 6,       //changes on 13/06/2018 req by Mr.Ashok
            //min: -2,
            beginAtZero:true,
            position: 'right',
            offset: 60,
            splitLine: {
                show: false
            },
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
        {
            type: 'value',
            name: 'Displacement * 1000 (MT)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 15,
                fontWeight: 700,
                fontSize: 14
            },
            max: maxDisplace,
            min:minDisplace,
            position: 'right',
            splitLine: {
                show: false
            },
            axisLine: {
                lineStyle: {
                    color: colors[4]
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
            splitLine: {
                show:false
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
            data: seriesData[5],
            symbolSize: 10,
        }, {
            name: 'Displacement',
            type: 'line',
            yAxisIndex: 4,
            data: seriesData[6],
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
          cpGraphState5 = myChartPerf;
      }
    );
}


function mpchart05a(graphDiv, seriesData1, dateRange1) {

    var option = null;

    var colors = colors5a;
    //  var colors = ['#2980b9', '#c0392b', '#f1c40f', '#27ae60', '#e67e22', '#FF403C', '#FF982C', '#FFD706', '#7DE346', '#3AAEB0'];

    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'M/E FOC', 'CP M/E FOC'];

    var seriesData = seriesData1;
    //var seriesData = [
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], //conditions    0
    //  [2, 2, 5, 3, 4, 6, 3], // Wind Force    1
    //  [15.3, 15.5, 12.5, 15.3, 14.9, 6, 15.1], // SOG Kn   2
    //  [15.1, 15.1, 12, 15.1, 15.1, 6, 15.2], //LOG   3
    //  [15, 15, 15, 15, 15, 15, 15],  // CP Speed   4
    //  [0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5],  // Trim    5
    //  [44.819, 44.819, 44.819, 44.819, 44.819, 44.819, 44.819],  //  Displacement    ------  End of graph A     6

    //  [82, 83, 60, 79, 80, 0, 79],  // ME Load Gov/Fuel Index   %     7
    //  [90, 90, 90, 90, 90, 90, 90],  // ME Load Limit (NCR)   %      8
    //  [5, 6, 10, 6, 5, 0, 5],  // SLIP   %     9
    //  [98, 99, 72, 98, 98, 0, 98],  // ME RPM  (rmp)   ------  End of graph B      10

    //  [19, 19, 10, 19.5, 19.5, 0, 19.5],  // FOC Rate  (t/d)   11
    //  [20, 20, 20, 20, 20, 20, 20],  // CP FOC Rate  (t/d)   ------  End of graph C    12
    //  [4, 4, 4, 4, 4, 4, 4]    //  CP Wind Speed
    //];
    var loadlenth = seriesData[7].length - 1;
    seriesData[7][loadlenth] = '-';

    var dateRange = dateRange1;
    var maxSog = Math.ceil(Math.max(Math.max.apply(null, seriesData[2])));
    maxSog += maxSog > 0 ? Math.ceil(maxSog * 0.2) : maxSog;
    var minSog = Math.ceil(Math.min(Math.min.apply(null, seriesData[2])));
    minSog -= minSog > 0 ? Math.ceil(minSog * 0.2) : minSog;
    var maxslip = Math.ceil(Math.max(Math.max.apply(null, seriesData[9]))); //not used
    maxslip += maxslip > 0 ? Math.ceil(maxslip * 0.2) : maxslip;
    var maxRpm = Math.ceil(Math.max(Math.max.apply(null, seriesData[10])));
    maxRpm += maxRpm > 0 ? Math.ceil(maxRpm * 0.2) : maxRpm;
    var minRpm = Math.ceil(Math.min(Math.min.apply(null, seriesData[10])));
    minRpm -= minRpm > 0 ? Math.ceil(minRpm * 0.2) : minRpm;
    var maxMeLoad = Math.ceil(Math.max(Math.max.apply(null, seriesData[7]), Math.max.apply(null, seriesData[8])));
    maxMeLoad += maxMeLoad > 0 ? Math.ceil(maxMeLoad * 0.2) : maxMeLoad;
    var minMeLoad = Math.ceil(Math.min(Math.min.apply(null, seriesData[7]), Math.min.apply(null, seriesData[8])));
    minMeLoad -= minMeLoad > 0 ? Math.ceil(minMeLoad * 0.2) : minMeLoad;
    //var maxMeLimit = Math.ceil(Math.max(Math.max.apply(null, seriesData[8])));
    
    option = {
        color: colors,
        title: {
            text: 'Engine Performance',
            textStyle: {
                color: '#C2417C'
            },
            subtext: 'Speed, SLIP, LOAD & RPM Analysis',
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            },
            x: 'center'
        },

        grid: {
            top: 60,
            bottom: 100,
            left: 170,
            right: 120,
            height: '160'
        },

        dataZoom: [{
            bottom: 25,
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
            width: 650,
            bottom: 3,
            itemGap: 30,
            data: [
            { name: 'SOG (Kn)' },
            { name: 'SLIP %' },
            { name: 'ME RPM' },
            { name: 'ME Load %' },
            { name: 'ME Load Limit (NCR)%' }
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

                // var windDev = insrt[1] - insrt[13];
                var windDev = insrt[1] - cpBF;
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[2] - insrt[4];
                //var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var speedStyle = (speedDev >= 0) ? "greenStatus" : "redStatus";
                var speedDevPercent = ((Math.abs(insrt[2] - insrt[4]) * 100) / insrt[2]).toFixed(2) + "%";

                var hfoDev = insrt[12] - insrt[11];
                //var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(2)).toString() + "%";

                var doStyle = (insrt[7] <= 80) ? "greenSpeed" : "redSpeed";

                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });

                //tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr><tr><td class="tooltpStyle" align="center">'+ StartdateLabel +'</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + startDate + '</td></tr><tr><td class="tooltpStyle" align="center">'+ EnddateLabel +'</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + endDate + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle">' + seriesName[0] + '</td><td><span style="color: white;background: ' + colors5[0] + '; padding: 0 5px">' + insrt[2] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[1] + '</td><td><span style="color: white;background: ' + colors5[1] + '; padding: 0 5px">' + insrt[3] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[2] + '</td><td><span style="color: white;background: ' + colors5[2] + '; padding: 0 5px">' + insrt[4] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[4] + '</td><td><span style="color: white;background: ' + colors5[4] + '; padding: 0 5px">' + insrt[6] + '</span> *1000 MT</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td> <span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kn</span></td></tr><tr><td class="tooltpStyle">' + seriesName[3] + '</td><td><span style="color: white;background: ' + colors5[3] + '; padding: 0 5px">' + insrt[5] + '</span> m</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[8] + '</td><td><span style="color: white;background: ' + colors5a[2] + '; padding: 0 5px">' + insrt[10] + '</span></td></tr><tr><td class="tooltpStyle">' + seriesName[5] + '</td><td> <span style="color: white;background: ' + colors5a[3] + '; padding: 0 5px">' + insrt[7] + '</span>%</td></tr><tr><td class="tooltpStyle">' + seriesName[7] + '</td><td><span style="color: white;background: ' + colors5a[1] + '; padding: 0 5px">' + insrt[9] + '</span>%</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[9] + '</td><td><span style="color: white;background: ' + colors5b[0] + '; padding: 0 5px"> ' + insrt[11] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[10] + '</td><td><span style="color: white;background: ' + colors5b[1] + '; padding: 0 5px"> ' + insrt[12] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td> <span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table>';
                tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr><tr><td class="tooltpStyle">' + StartdateLabel + '</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + startDate + '</td></tr><tr><td class="tooltpStyle">' + EnddateLabel + '</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + endDate + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle">' + seriesName[0] + '</td><td><span style="color: white;background: ' + colors5[0] + '; padding: 0 5px">' + insrt[2] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[1] + '</td><td><span style="color: white;background: ' + colors5[1] + '; padding: 0 5px">' + insrt[3] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[2] + '</td><td><span style="color: white;background: ' + colors5[2] + '; padding: 0 5px">' + insrt[4] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[4] + '</td><td><span style="color: white;background: ' + colors5[4] + '; padding: 0 5px">' + insrt[6] + '</span> *1000 MT</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td> <span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kn</span></td></tr><tr><td class="tooltpStyle">' + seriesName[3] + '</td><td><span style="color: white;background: ' + colors5[3] + '; padding: 0 5px">' + insrt[5] + '</span> m</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[8] + '</td><td><span style="color: white;background: ' + colors5a[2] + '; padding: 0 5px">' + insrt[10] + '</span></td></tr><tr><td class="tooltpStyle">' + seriesName[5] + '</td><td> <span style="color: white;background: ' + colors5a[3] + '; padding: 0 5px">' + insrt[7] + '</span>%</td></tr><tr><td class="tooltpStyle">' + seriesName[7] + '</td><td><span style="color: white;background: ' + colors5a[1] + '; padding: 0 5px">' + insrt[9] + '</span>%</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[9] + '</td><td><span style="color: white;background: ' + colors5b[0] + '; padding: 0 5px"> ' + insrt[11] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[10] + '</td><td><span style="color: white;background: ' + colors5b[1] + '; padding: 0 5px"> ' + insrt[12] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td> <span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table>';
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
            top: 60,
            bottom: 100,
            left: 170,
            right: 90,
            height: '160'
        },

        xAxis: {
            type: 'category',
            axisTick: {
                alignWithLabel: true
            },
            data: dateRange,
            show: true,
            axisPointer: {
                label: {
                    show: false
                }
            }
        },

        yAxis: [
            {
                type: 'value',
                name: 'SOG (Kn)',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 15,
                    fontWeight: 700,
                    fontSize: 14
                },
                show: true,
                splitLine: {
                    show: false
                },
                max: maxSog,
                min: minSog,
                position: 'left',
                offset: 120,
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
            },
            {
                type: 'value',
                name: 'Slip %',
                //min: -15,
                //max: 30,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 15,
                    fontWeight: 700,
                    fontSize: 14
                },
                show: true,
                position: 'left',
                offset: 60,
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
                splitLine: {
                    show: false
                }
            },
            {
                type: 'value',
                name: "M/E RPM",
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 15,
                    fontWeight: 700,
                    fontSize: 14
                },
                max: maxRpm,
                min: minRpm,
                position: 'left',
                axisPointer: {
                    label: {
                        show: true
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: colors[2]
                    }
                },
                axisLabel: {
                    formatter: '{value}'
                }
            },
            {
                type: 'value',
                name: "ME Load %",
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 20,
                    fontWeight: 700,
                    fontSize: 14
                },
                max: 110,
                show: true,
                position: 'right',
                axisLine: {
                    lineStyle: {
                        //color: colors[3]
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
                splitLine: {
                    show: false
                }
            },
            {
                type: 'value',
                name: 'ME Load (NCR) %',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 20,
                    fontWeight: 700,
                    fontSize: 14
                },
                show: false,
                max: 110,
                position: 'right',
                offset: 60,
                axisLine: {
                    lineStyle: {
                        color: colors[4]
                    }
                },
                axisPointer: {
                    label: {
                        show: false
                    }
                },
                axisLabel: {
                    formatter: '{value}'
                },
                splitLine: {
                    show: false
                }
            }

        ],
        series: [

            {
                name: 'SOG (Kn)',
                type: 'line',
                data: seriesData[2],
                symbolSize: 10
            },
            {
                name: 'SLIP %',
                type: 'line',
                yAxisIndex: 1,
                data: seriesData[9],
                symbolSize: 10
            },
            {
                name: 'ME RPM',
                type: 'line',
                yAxisIndex: 2,
                data: seriesData[10],
                symbolSize: 10
            },
            {
                name: 'ME Load %',
                type: 'line',
                yAxisIndex: 3,
                data: seriesData[7],
                symbolSize: 10
            },
            {
                name: 'ME Load Limit (NCR)%',
                type: 'line',
                yAxisIndex: 4,
                data: seriesData[8],
                symbolSize: 10,
                markLine: {
                    label: {
                        position: 'start'
                    },
                    data: [
                      {
                          yAxis: 100,
                          name: 'MCR %'
                      }
                    ],
                    label: {
                        normal: {
                            formatter: "\n\n MCR {c}%"
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


function mpchart05b(graphDiv, seriesData1, dateRange1) {

    var option = null;

    var colors = colors5b;
    //  var colors = ['#FF403C', '#2980b9'];

    var seriesName = ['SOG', 'LOG', 'CP Speed', 'Trim', 'Displacement', 'Load', 'Load Limit', 'SLIP', 'RPM', 'M/E FOC', 'CP M/E FOC'];
    var seriesData = seriesData1;
    //var seriesData = [
    //  ['Ballast', 'Ballast', 'Ballast', 'Laden', 'Laden', 'Laden', 'Laden'], //conditions    0
    //  [2, 2, 5, 3, 4, 6, 3], // Wind Force    1
    //  [15.3, 15.5, 12.5, 15.3, 14.9, 6, 15.1], // SOG Kn   2
    //  [15.1, 15.1, 12, 15.1, 15.1, 6, 15.2], //LOG   3
    //  [15, 15, 15, 15, 15, 15, 15],  // CP Speed   4
    //  [0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5],  // Trim    5
    //  [44.819, 44.819, 44.819, 44.819, 44.819, 44.819, 44.819],  //  Displacement    ------  End of graph A     6

    //  [82, 83, 60, 79, 80, 0, 79],  // ME Load power output(NR)   %     7
    //  [90, 90, 90, 90, 90, 90, 90],  // ME Load Limit (NCR)   %      8
    //  [5, 6, 10, 6, 5, 0, 5],  // SLIP   %     9
    //  [98, 99, 72, 98, 98, 0, 98],  // ME RPM  (rmp)   ------  End of graph B      10

    //  [19, 19, 10, 19.5, 19.5, 0, 19.5],  // FOC Rate  (t/d)   11
    //  [20, 20, 20, 20, 20, 20, 20],  // CP FOC Rate  (t/d)   ------  End of graph C    12
    //  [4, 4, 4, 4, 4, 4, 4]    //  CP Wind Speed
    //];

    var dateRange = dateRange1;
    var maxVal = Math.ceil(Math.max(Math.max.apply(null, seriesData[11]), Math.max.apply(null, seriesData[12])));
    maxVal += maxVal > 0 ? Math.ceil(maxVal * 0.2) : maxVal;
    var minVal = Math.ceil(Math.min(Math.min.apply(null, seriesData[11]), Math.min.apply(null, seriesData[12])));
    minVal -= minVal > 0 ? Math.ceil(minVal * 0.2) : minVal;
    option = {
        color: colors,
        title: {
            text: 'M/E FOC Analysis',
            textStyle: {
                color: '#C2417C'
            },
            x: 'center'
        },

        grid: [{
            top: 60,
            bottom: 100
        }],

        dataZoom: [{
            bottom: 25,
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
            bottom: 3,
            itemGap: 30,
            data: [
              { name: 'M/E FOC MT/d' },
              { name: 'CP M/E FOC MT/d' }
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

                // var windDev = insrt[1] - insrt[13];
                var windDev = insrt[1] - cpBF;
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed";

                var speedDev = insrt[2] - insrt[4];
                //var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var speedStyle = (speedDev >= 0) ? "greenStatus" : "redStatus";
                var speedDevPercent = ((Math.abs(insrt[2] - insrt[4]) * 100) / insrt[2]).toFixed(2) + "%";

                var hfoDev = insrt[12] - insrt[11];
                //var hfoStyle = (hfoDev >= 0) ? "greenStatus" : (windDev > 0) ? "greenStatus" : "redStatus";
                var hfoStyle = (hfoDev >= 0) ? "greenStatus" : "redStatus";
                var hfoDevPercent = (((Math.abs(insrt[12] - insrt[11]) * 100) / insrt[12]).toFixed(2)).toString() + "%";

                var doStyle = (insrt[7] <= 80) ? "greenSpeed" : "redSpeed";

                voyageParam.forEach(function (itm, index, arr) {
                    startDate = arr[index][0];
                    endDate = arr[index][1];
                    StartdateLabel = arr[index][2];
                    EnddateLabel = arr[index][3];

                });

                tip = '<table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle" align="center">' + insrt[14] + '</td><td class="tooltpStyle" align="center">' + insrt[0] + '</td><td align="center"> <span class="' + windStyle + '"> Bf-' + insrt[1] + '</span></td></tr><tr><td class="tooltpStyle">' + StartdateLabel + '</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + startDate + '</td></tr><tr><td class="tooltpStyle">' + EnddateLabel + '</td><td class="tooltpStyle" align="center" colspan="2" style="color:#1B56FF; font-weight: 700">' + endDate + '</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td class="tooltpStyle">' + seriesName[0] + '</td><td><span style="color: white;background: ' + colors5[0] + '; padding: 0 5px">' + insrt[2] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[1] + '</td><td><span style="color: white;background: ' + colors5[1] + '; padding: 0 5px">' + insrt[3] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[2] + '</td><td><span style="color: white;background: ' + colors5[2] + '; padding: 0 5px">' + insrt[4] + '</span> Kn</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[4] + '</td><td><span style="color: white;background: ' + colors5[4] + '; padding: 0 5px">' + insrt[6] + '</span> *1000 MT</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td> <span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + ' Kn</span></td></tr><tr><td class="tooltpStyle">' + seriesName[3] + '</td><td><span style="color: white;background: ' + colors5[3] + '; padding: 0 5px">' + insrt[5] + '</span> m</td><td></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[8] + '</td><td><span style="color: white;background: ' + colors5a[2] + '; padding: 0 5px">' + insrt[10] + '</span></td></tr><tr><td class="tooltpStyle">' + seriesName[5] + '</td><td> <span style="color: white;background: ' + colors5a[3] + '; padding: 0 5px">' + insrt[7] + '</span>%</td></tr><tr><td class="tooltpStyle">' + seriesName[7] + '</td><td><span style="color: white;background: ' + colors5a[1] + '; padding: 0 5px">' + insrt[9] + '</span>%</td></tr></table><table class="tooltipTable" style="margin-bottom: 10px;"><tr><td width="125" class="tooltpStyle">' + seriesName[9] + '</td><td><span style="color: white;background: ' + colors5b[0] + '; padding: 0 5px"> ' + insrt[11] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle">' + seriesName[10] + '</td><td><span style="color: white;background: ' + colors5b[1] + '; padding: 0 5px"> ' + insrt[12] + '</span> MT/d</td><td></td></tr><tr><td class="tooltpStyle"> <span class="pad20">Deviation</span></td><td> <span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td> <span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' MT/d</span></td></tr></table>';
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
            show: true,
            axisPointer: {
                label: {
                    show: false
                }
            }
        },

        yAxis: [
          {
              type: 'value',
              name: "M/E FOC MT/d",
              nameLocation: 'center',
              nameRotate: 90,
              nameTextStyle: {
                  padding: 20,
                  fontWeight: 700,
                  fontSize: 14
              },
              show: true,
              max: maxVal,             //changed on 13/06/2018 req by Mr.Ashok
              min: minVal,
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
              nameLocation: 'center',
              nameRotate: 90,
              nameTextStyle: {
                  padding: 20,
                  fontWeight: 700,
                  fontSize: 14
              },
              show: true,
              //max: 130,             //changed on 13/06/2018 req by Mr.Ashok
              //name: "CP M/E FOC MT/d",
              position: 'right',
              axisLine: {
                  lineStyle: {
                      //color: colors[1]
                  }
              },
              axisPointer: {
                  label: {
                      show: false
                  }
              },
              axisLabel: {
                  formatter: '{value}',
                  show: false
              },
              splitLine: {
                  show: false
              }
              
          }
        ],
        series: [{
            name: 'M/E FOC MT/d',
            type: 'line',
            barGap: '0%',
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#fff',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[11],
            symbolSize: 10
        }, {
            name: 'CP M/E FOC MT/d',
            type: 'line',
            label: {
                normal: {
                    show: false,
                    distance: 15,
                    color: '#fff',
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            //yAxisIndex: 1,
            data: seriesData[12],
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
          cpGraphState5b = myChartPerf;
      }
    );
}