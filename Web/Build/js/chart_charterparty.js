var cpGraphState = {};
function winResize()
{
    cpGraphState.resize();
}

function CharterpartyChart(seriesname, daterange)
{
    var app1 = {};

    var option = null;

    var colors = ['#2980b9', '#f1c40f', '#2980b9', '#27ae60', '#2980b9', '#c0392b', '#2980b9', '#e67e22'];

    var seriesName = ['Condition', 'CP Wind', 'Actual Wind', 'CP SOG (Avg)', 'Vsl SOG (Avg)', 'CP HFO', 'Vsl HFO', 'CP DO', 'Vsl DO'];

    var seriesData = seriesname;

    var dateRange = daterange;

    var SOGmaxval = Math.ceil(Math.max(Math.max.apply(null,seriesData[1]),Math.max.apply(null,seriesData[2]),Math.max.apply(null,seriesData[3]),Math.max.apply(null,seriesData[4])));
    var HFOmaxval = Math.ceil(Math.max(Math.max.apply(null,seriesData[5]),Math.max.apply(null,seriesData[6]),Math.max.apply(null,seriesData[7]),Math.max.apply(null,seriesData[8])));
    SOGmaxval = 20;
    HFOmaxval = 50;
    DOMax = 25;
    option = {
        color: colors,
        title: {
            text: 'Charter Party Performance Chart',
            x: 'center'
        },

        legend: {
            left: 'center',
            width:750 ,
            bottom:25,
            itemGap: 5,
            data: [
              { name: 'CP Wind', icon: 'image://../css/Theme1/images/icon-CpWind.svg' },
              { name: 'Actual Wind', icon: 'image://../css/Theme1/images/icon-CpActualWind.svg' },
              { name: 'CP SOG (Avg)', icon: 'image://../css/Theme1/images/icon-CpSOG.svg' },
              { name: 'Vsl SOG (Avg)', icon: 'image://../css/Theme1/images/icon-CpVslSOG.svg' },
              { name: 'CP HFO', icon: 'image://../css/Theme1/images/icon-CpHFO.svg' },
              { name: 'Vsl HFO', icon: 'image://../css/Theme1/images/icon-CpVslHFO.svg' },
              { name: 'CP DO', icon: 'image://../css/Theme1/images/icon-CpDO.svg' },
              { name: 'Vsl DO', icon: 'image://../css/Theme1/images/icon-CpVslDO.svg' }
            ],
            textStyle: { fontWeight: 'bold' }
        },

        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            },
            formatter: function(params) {
                var tip = "";
                var insrt = [];
                var result = params[0].dataIndex;
                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                var windDev = insrt[2] - insrt[1];            ////Actual wind speed difference (0 and < acceptable)
                var windStyle = (windDev <= 0) ? "greenSpeed" : "redSpeed"


                var speedDev = insrt[4] - insrt[3];           ///Speed diff between CP speed and Vsl speed (>=0 acceptable,In case of Bad weather vsl speed diff  >0 acceptable )
                var speedStyle = (speedDev >= 0) ? "greenStatus" : (windDev > 0 ) ? "greenStatus" :  "redStatus";
          
                var speedDevPercent = (insrt[4]<=0) ? "Error" : (((Math.abs(insrt[3] - insrt[4]) * 100) / insrt[3]).toFixed(1)).toString()+"%";

                var hfoDev = insrt[6] - insrt[5];            ///Fo consumption diff between CP and Vsl (<=0 acceptable,Bad weather >0 acceptable )
                var hfoStyle = (hfoDev <= 0) ? "greenStatus" : (windDev > 0 ) ? "greenStatus" :  "redStatus";

                var hfoDevPercent = (insrt[6]<=0) ? "Error" : (((Math.abs(insrt[5] - insrt[6]) * 100) / insrt[5]).toFixed(1)).toString()+"%";

                var doDev = insrt[8] - insrt[7];
                var doStyle = (doDev <= 0) ? "greenStatus" : "redStatus";

                var doDevPercent = (insrt[8]<=0) ? "Error" : (((Math.abs(insrt[7] - insrt[8]) * 100) / insrt[7]).toFixed(1)).toString()+"%";

                //insrt[9] - vessel status from noon report (at sea ,at port)
                tip = '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + insrt[9] + '</span></td><td>' + insrt[0] + '</td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Speed</td</tr><tr><td><span class="tlTip style2">•</span>' + seriesName[3] + '</td><td> ' + insrt[3] + 'Kn</td><td></td></tr><tr><td><span class="tlTip style3">•</span>' + seriesName[4] + '</td><td>' + insrt[4] + 'Kn</td><td></td></tr><tr style="margin-bottom: 20px;"><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + speedStyle + '">' + speedDevPercent + '</span></td><td><span class="' + speedStyle + '">' + Math.abs(speedDev.toFixed(2)) + 'kn</span></td></tr></table><table class="tooltipTable"><tr><td colspan="2" class="tooltipHeadStyle">Wind</td</tr><tr><td><span class="tlTip style0">•</span>' + seriesName[1] + '</td><td> Bf-' + insrt[1] + '</td></tr><tr><td><span class="tlTip style1">•</span>' + seriesName[2] + '</td><td><span class="' + windStyle + '"> Bf-' + insrt[2] + '</span></td></tr></table><table class="tooltipTable"><tr><td colspan="3" class="tooltipHeadStyle">Fuel</td</tr><tr><td width="150"><span class="tlTip style4">•</span>' + seriesName[5] + '</td><td width="80"> ' + insrt[5] + ' t/d</td><td width="40"></td></tr><tr><td><span class="tlTip style7">•</span>' + seriesName[6] + '</td><td> ' + insrt[6] + ' t/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + hfoStyle + '">' + hfoDevPercent + '</span></td><td><span class="' + hfoStyle + '">' + Math.abs(hfoDev.toFixed(2)) + ' t/d</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip style6">•</span>' + seriesName[7] + '</td><td> ' + insrt[7] + ' t/d</td><td></td></tr><tr><td><span class="tlTip style5">•</span>' + seriesName[8] + '</td><td> ' + insrt[8] + ' t/d</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="' + doStyle + '">' + doDevPercent + '</span></td><td><span class="' + doStyle + '">' + Math.abs(doDev.toFixed(2)) + ' t/d</span></td></tr></table>';

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
            position: function(pos, params, el, elRect, size) {
                var obj = {
                    top: 50
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
            left: 50,
            right: 50,
            height: '30%'
        }, {
            left: 50,
            right: 50,
            top: '55%',
            height: '30%'
        }],

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
            name: 'Actual Wind',
            nameLocation: 'end',
            max: SOGmaxval,
            show: false,
            position: 'right',
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
                formatter: 'Bf-{value}'
            }
        }, {
            type: 'value',
            name: 'Actual Wind',
            nameLocation: 'end',
            max: SOGmaxval,
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
                formatter: 'Bf-{value}'
            }
        }, {
            type: 'value',
            name: 'SOG',
            show: false,
            nameLocation: 'end',
            max: SOGmaxval,
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
            nameLocation: 'end',
            max: SOGmaxval,
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
        },
          //    HFO DO detail...

          {
              gridIndex: 1,
              type: 'value',
              name: 'CP HFO',
              nameLocation: 'end',
              max: HFOmaxval,
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
                  formatter: '{value} t/d'
              }
          }, {
              gridIndex: 1,
              type: 'value',
              name: 'Vsl HFO',
              nameLocation: 'end',
              max: HFOmaxval,
              position: 'left',
              axisLine: {
                  lineStyle: {
                      color: colors[5]
                  }
              },
              axisPointer: {
                  label: {
                      show: false
                  }
              },
              axisLabel: {
                  formatter: '{value} t/d'
              }
          }, {
              gridIndex: 1,
              type: 'value',
              name: 'CP DO',
              show: false,
              nameLocation: 'end',
              max: DOMax,
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
                  formatter: '{value} t/d'
              }
          }, {
              gridIndex: 1,
              type: 'value',
              name: 'Vsl DO',
              nameLocation: 'end',
              max: DOMax,
              position: 'right',
              axisLine: {
                  lineStyle: {
                      color: colors[7]
                  }
              },
              axisPointer: {
                  label: {
                      show: false
                  }
              },
              axisLabel: {
                  formatter: '{value} t/d'
              }
          }
        ],
        series: [{
            name: 'CP Wind',
            type: 'line',
            data: seriesData[1],
            symbolSize: 7
        }, {
            name: 'Actual Wind',
            type: 'line',
            yAxisIndex: 1,
            data: seriesData[2],
            symbolSize: 7
        }, {
            name: 'CP SOG (Avg)',
            type: 'line',
            xAxisIndex: 1,
            yAxisIndex: 2,
            data: seriesData[3],
            symbolSize: 7
        }, {
            name: 'Vsl SOG (Avg)',
            type: 'line',
            xAxisIndex: 1,
            yAxisIndex: 3,
            data: seriesData[4],
            symbolSize: 7
        }, {
            name: 'CP HFO',
            type: 'line',
            xAxisIndex: 2,
            yAxisIndex: 4,
            data: seriesData[5],
            symbolSize: 7
        }, {
            name: 'Vsl HFO',
            type: 'line',
            xAxisIndex: 2,
            yAxisIndex: 5,
            data: seriesData[6],
            symbolSize: 7
        }, {
            name: 'CP DO',
            type: 'line',
            xAxisIndex: 3,
            yAxisIndex: 6,
            data: seriesData[7],
            symbolSize: 7
        }, {
            name: 'Vsl DO',
            type: 'line',
            xAxisIndex: 3,
            yAxisIndex: 7,
            data: seriesData[8],
            symbolSize: 7
        }]
    };


    require.config({
        paths: {
            echarts2: '../js/echartsAll3'
        }
    });

    require(
      ['echarts2'],
      function(ec) {
          var graphFilDiv = document.getElementById("cpGraph");
          var myChartPerf = ec.init(graphFilDiv);
          myChartPerf.setOption(option);
          cpGraphState = myChartPerf;
      }
    );
}