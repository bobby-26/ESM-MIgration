//*****************Auxiliary Engine Performance Graphs********************
//******************************************************************************

function popupPerfCall(graphDiv, seriesdata, seriesdata1, seriesdata2, dateseries, shoptestSfoc) {
    var app1 = {};

    var option = null;

    var colors = ['#FFC565', '#2C9A57'];

    var seriesName = ['Actual SFOC g/kWh', 'SFOC g/kWh (ISO)', 'Lower Calorific Value MJ/kg'];

    var seriesData = seriesdata;

    var seriesDataMax = (Math.max(seriesData[0]) > Math.max(seriesData[1])) ? Math.max(seriesData[0]) : Math.max(seriesData[1]);

    // shoptest SFOC value from Vessel master.
    var shoptestSFOC = shoptestSfoc;
    var shoptestSFOC6Per = shoptestSFOC + (shoptestSFOC * 0.06);
    // maximum is 16% 

    var overhaulData = seriesdata1;
    //var overhaulData = [
    //    //Piston (Hrs)
    //    [7186, 7522, 80, 771.2, 1143.2, 1323.2, 1881.2, 2067.2, 2607.2, 3165.2, 3705.2, 4263.2],
    //    //F.O.Injector(Hrs)
    //    [76, 412, 970, 661.2, 33.2, 213.2, 771.2, 957.2, 497.2, 55.2, 595.2, 153.2],
    //    //F.O.Injector Pump(Hrs)
    //    [7936, 272, 830, 1521.2, 1893.2, 2073.2, 2631.2, 2817.2, 3357.2, 3915.2, 4455.2, 5013.2],
    //    //Suc.Valve(Hrs)
    //    [3786, 122, 680, 1371.2, 1743.2, 1923.2, 2481.2, 2667.2, 3207.2, 3765.2, 4305.2, 4863.2],
    //    //Exh.Valve(Hrs)
    //    [3586, 3922, 480, 1171.2, 1543.2, 1723.2, 2281.2, 2467.2, 3007.2, 3565.2, 105.2, 663.2],
    //    //T / C Water washing(Hrs)
    //    [36, 22, 30, 21.2, 43.2, 23.2, 31.2, 17.2, 7.2, 15.2, 5.2, 13.2],
    //    //T / C Renew Bearing(Hrs)
    //    [11986, 322, 880, 1571.2, 1943.2, 2123.2, 2681.2, 2867.2, 3407.2, 3965.2, 4505.2, 5063.2]

    //];

    var overhaulRecomends = [16000, 200, 12000, 16000, 16000, 50, 5000];

    var overhaulNames = ['Piston (Hrs)', 'F.O. Injector (Hrs)', 'F. O. injection Pump (Hrs)', 'Suc. Valve (Hrs)', 'Exh. Valve (Hrs)', 'T/C Water Washing (Hrs)', 'T/C Renew Bearing (Hrs)'];

    // Ambient Data

    var ambiData = seriesdata2;
    //var ambiData = [
    //    // C/W Inlet to Air Clr. ºC
    //    [35, 37, 36, 36, 37, 35, 36, 35, 33, 31, 30, 30],
    //    // Baromatric Pressure (millibar)
    //    [1011, 1000, 1003, 1009, 1015, 1020, 1024, 1021, 1012, 1010, 1009, 1008],
    //    // Engine Room ºC
    //    [25, 25, 30, 31, 33, 35, 34, 34, 31, 26, 24, 25]
    //];

    var ambiNames = ["C/W Inlet to Air Clr. ºC ", "Baromatric Pressure (millibar)", "Engine Room ºC"];

    option = {
        title: {
            text: "Specific Fuel Oil Consumption: Actuals & Corrected to ISO",
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

                for (var i in overhaulData) {
                    var obj = overhaulData[i];
                    insrtH[i] = obj[result];
                }

                for (var i in ambiData) {
                    var obj = ambiData[i];
                    insrtAmbi[i] = obj[result];
                }

                tip = '<table class="tooltipTable" style="margin-bottom: 20px; width: 320px"><tr><td class="tooltpStyle" align="left" width="70%">' + seriesName[0] + '</td><td  align="left" width="30%"><span>' + insrt[0] + '</span></td></tr><tr><td  class="tooltpStyle" align="left">' + seriesName[1] + '</td><td align="left"><span>' + insrt[1] + '</span></td></tr><tr><td  class="tooltpStyle" align="left">' + seriesName[2] + '<br><span class="subTextLine">(LCV ISO - 42.7 MJ/kg) </span></td><td align="left"><span>' + insrt[2] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px; width:320px;"><tr class="mtboTitle"><td class="tooltpStyle" align="left" width="125">&nbsp;</td><td align="right"><span>Actuals</span></td><td align="left"><span>MTBO</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[0] + '</td><td align="right"><span>' + insrtH[0] + '</span></td><td align="left"><span>' + overhaulRecomends[0] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[1] + '</td><td align="right"><span>' + insrtH[1] + '</span></td><td align="left"><span>' + overhaulRecomends[1] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[2] + '</td><td align="right"><span>' + insrtH[2] + '</span></td><td align="left"><span>' + overhaulRecomends[2] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[3] + '</td><td align="right"><span>' + insrtH[3] + '</span></td><td align="left"><span>' + overhaulRecomends[3] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[4] + '</td><td align="right"><span>' + insrtH[4] + '</span></td><td align="left"><span>' + overhaulRecomends[4] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[5] + '</td><td align="right"><span>' + insrtH[5] + '</span></td><td align="left"><span>' + overhaulRecomends[5] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[6] + '</td><td align="right"><span>' + insrtH[6] + '</span></td><td align="left"><span>' + overhaulRecomends[6] + '</span></td></tr></table><table class="tooltipTable" style="margin-bottom: 10px; width:320px;"><tr class="ambiTitle"><td class="tooltpStyle" align="left" colspan="2">Ambient Conditions</td></tr><tr class="ambiRow"><td class="tooltpStyle" align="left">' + ambiNames[0] + '</td><td align="right"><span>' + insrtAmbi[0] + '</span></td></tr><tr class="ambiRow"><td class="tooltpStyle" align="left">' + ambiNames[1] + '</td><td align="right"><span>' + insrtAmbi[1] + '</span></td></tr><tr class="ambiRow"><td class="tooltpStyle" align="left">' + ambiNames[2] + '<br>(Air inlet to blower)</td><td align="right"><span>' + insrtAmbi[2] + '</span></td></tr></table>';

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
            data: seriesName,
            bottom: 0
        },
        xAxis: [{
            type: 'category',
            data: dateseries
        }],
        yAxis: [{
            type: 'value',
            max: 220,
            min: 150,
            name: 'SFOC g/kWh',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14
            }
        },
            {
                type: 'value',
                max: 220,
                min: 150,
                axisLabel: {
                    show: false
                },
                index: 1
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: 0,
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    distance: 15,
                    color: '#000',
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                show: false,
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        distance: 15,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        formatter: '{c}'
                    }
                },
                yAxisIndex: 1,
                data: seriesData[1]
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
                        name: 'SFOC 6%',
                        yAxis: shoptestSFOC6Per,
                        formatter: '{b}: {d}'
                    }],
                    label: {
                        normal: {
                            show: true
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: 'red'
                        }
                    }
                }
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
                        yAxis: shoptestSFOC
                    }],
                    label: {
                        normal: {
                            show: true,
                            formatter: '{c}\nShoptest\nSFOC(ISO)'
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
            cpGraphState = myChartPerf;
        }
    );
}

function focGraphCall(graphDiv, seriesdata, dateseries) {

    var app1 = {};

    var option = null;

    var colors = ['#c0392b', '#2980b9', '#2F4E57'];

    var seriesName = ['Fuel Consumption MT/day', 'Actual Load kW', 'Load %'];

    //var seriesData = [
    //	// FO Consumption MT / day
    //	[1.5, 3, 4.5, 6, 3, 1.5, 4.5, 1.5, 4.5, 4.5, 4.5, 4.5],
    //	// Load  (kW)
    //	[299, 613, 923, 1229, 613, 299, 923, 299, 923, 923, 923, 923],
    //	// Load %
    //	[25, 50, 75, 96, 50, 25, 75, 25, 75, 75, 75, 75]
    //];

    var seriesData = seriesdata;

    //var overhaulData = [
    //    //Piston (Hrs)
    //    [7186, 7522, 80, 771.2, 1143.2, 1323.2, 1881.2, 2067.2, 2607.2, 3165.2, 3705.2, 4263.2],
    //    //F.O.Injector(Hrs)
    //    [76, 412, 970, 661.2, 33.2, 213.2, 771.2, 957.2, 497.2, 55.2, 595.2, 153.2],
    //    //F.O.Injector Pump(Hrs)
    //    [7936, 272, 830, 1521.2, 1893.2, 2073.2, 2631.2, 2817.2, 3357.2, 3915.2, 4455.2, 5013.2],
    //    //Suc.Valve(Hrs)
    //    [3786, 122, 680, 1371.2, 1743.2, 1923.2, 2481.2, 2667.2, 3207.2, 3765.2, 4305.2, 4863.2],
    //    //Exh.Valve(Hrs)
    //    [3586, 3922, 480, 1171.2, 1543.2, 1723.2, 2281.2, 2467.2, 3007.2, 3565.2, 105.2, 663.2],
    //    //T / C Water washing(Hrs)
    //    [36, 22, 30, 21.2, 43.2, 23.2, 31.2, 17.2, 7.2, 15.2, 5.2, 13.2],
    //    //T / C Renew Bearing(Hrs)
    //    [11986, 322, 880, 1571.2, 1943.2, 2123.2, 2681.2, 2867.2, 3407.2, 3965.2, 4505.2, 5063.2]

    //];

    //var overhaulRecomends = [16000, 200, 12000, 16000, 16000, 50, 5000];

    //var overhaulNames = ['Piston (Hrs)', 'F.O. Injector (Hrs)', 'F. O. injection Pump (Hrs)', 'Suc. Valve (Hrs)', 'Exh. Valve (Hrs)', 'T/C Water Washing (Hrs)', 'T/C Renew Bearing (Hrs)'];


    option = {
        title: {
            text: "Fuel Oil Consumption MT/day, kW Load & Percentage Load",
            left: 'center',
            textStyle: {
                color: '#C2417C'
            }
        },
        color: colors,
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                //for (var i in overhaulData) {
                //    var obj = overhaulData[i];
                //    insrtH[i] = obj[result];
                //}

                tip = '<table class="tooltipTable" style="margin: 10px;" ><tr><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999; border-right: 1px solid #999">Actual Load</td><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999;border-right: 1px solid #999">Fuel Consumption</td><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999; border-right: 1px solid #999">Load</td></tr><tr><td align="center" style="border-right: 1px solid #999"><span style="color:white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + ' MT/day</span></td><td align="center" style="border-right: 1px solid #999"><span style="color:white; background:' + colors[1] + '; padding: 5px ; padding-bottom: 2px">' + insrt[1] + ' kW</span></td><td align="center" style="border-right: 1px solid #999"><span>' + insrt[2] + '%</span></td></tr></table>';

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
                    top: 93
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 120;
                return obj;
            },
            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        legend: {
            data: seriesName,
            bottom: 0
        },
        grid: [{
            top: 80,
            bottom: 100
        }],
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
        xAxis: [{
            type: 'category',
            data: dateseries
        }],
        yAxis: [{
            type: 'value',
            name: 'F.O. Consumption MT/day',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            }
        },
            {
                type: 'value',
                splitLine: {
                    show: false
                },
                name: 'Load kW',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[1]
                },
                index: 1
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: 0,
            label: {
                normal: {
                    show: true,
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideTop',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        distance: 15,
                        align: 'center',
                        verticalAlign: 'middle',
                        position: 'insideTop',
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
            var graphFilDivName = graphDiv + "aGraph";
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphStateA = myChartPerf;
        }
    );
}

function focGraphCall2(graphDiv, seriesdata, dateseries) {

    var app1 = {};

    var option = null;

    var colors = ['#2D542B', '#B5B232']; //#262014

    var seriesName = ['F.O. Inlet Temp. ºC', 'F.O. Inlet Pressure   bar'];

    var seriesData = seriesdata
    option = {
        title: {
            text: "Fuel Oil System",
            subtext: 'Fuel Oil Inlet Temperature & Pressure',
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            }
        },
        color: colors,
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                tip = '<table class="tooltipTable" style="margin: 10px;" width="100%"><tr><td align="center" style="background: #555; color: #fff">&nbsp;</td><td align="center" style="background: #555; color: #fff">Actuals</td><td align="center" style="background: #555; color: #fff">Normal Operating Data</td></tr><tr><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999; border-right: 1px solid #999">FO Inlet Temp.</td><td align="center" style="border-right: 1px solid #999"><span style="color:white; background:' + colors[0] + '; padding: 3px 5px">' + insrt[0] + ' ºC</span></td><td align="center">110ºC ~ 140ºC</td></tr><tr><td class="tooltpStyle" align="center" style="border-right: 1px solid #999">FO Inlet Pressure</td><td align="center" style="border-right: 1px solid #999"><span style="color:#000; background:' + colors[1] + '; padding: 3px 5px">' + insrt[1] + ' bar</span></td><td align="center">8 bar</td></tr></table>';

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
                    top: 93
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 190;
                return obj;
            },
            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        legend: {
            data: seriesName,
            bottom: 0
        },
        grid: [{
            top: 80,
            bottom: 100
        }],
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
        xAxis: [{
            type: 'category',
            data: dateseries
        }],
        yAxis: [{
            type: 'value',
            name: 'F.O. Inlet Temp. (ºC)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            }
        },
            {
                type: 'value',
                splitLine: {
                    show: false
                },
                name: 'F.O. Inlet Pressure  (bar)',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[1]
                },
                index: 1
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: 0,
            label: {
                normal: {
                    show: true,
                    distance: 20,
                    rotate: 90,
                    align: 'center',
                    verticalAlign: 'middle',
                    position: 'insideTop',
                    formatter: '{c}'
                }
            },
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        distance: 20,
                        align: 'center',
                        rotate: 90,
                        color: '#000',
                        verticalAlign: 'middle',
                        position: 'insideTop',
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
            var graphFilDivName = graphDiv + "bGraph";
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphStateB = myChartPerf;
        }
    );
}
//	Switchboard Graph 

function switchBoardGraphCall(graphDiv, seriesdata, dateseries) {

    var app1 = {};

    var option = null;

    var colors = ['#0E2431', '#FC3A52', '#F9B248', 'red', 'blue'];

    var seriesName = ['Load (kW)', 'Load (%)', 'Current (A)', 'RPM', 'Frequency Hz'];

    var seriesData = seriesdata;

    var dateRange = dateseries;

    option = {
        color: colors,
        title: {
            text: 'Switchboard Load, Current & Frequency (RPM)',
            // freq and RPM value from vessel master.
            subtext: "Frequency - 60Hz (720 RPM)",
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            }
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrt = [];
                var insrtH = [];
                var result = params[0].dataIndex;

                for (var i in seriesData) {
                    var obj = seriesData[i];
                    insrt[i] = obj[result];
                }

                tip = '<table class="tooltipTable" style="margin: 10px;" width="100%"><tr><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999; border-right: 1px solid #999">Actual Load</td><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999;border-right: 1px solid #999">Fuel Consumption</td><td class="tooltpStyle" align="center" style="border-bottom: 1px solid #999; border-right: 1px solid #999">Load</td></tr><tr><td align="center" style="border-right: 1px solid #999"><span style="color:white; background:' + colors[0] + '; padding: 0 5px">' + insrt[0] + ' kW</span></td><td align="center" style="border-right: 1px solid #999"><span style="color:white; background:' + colors[1] + '; padding: 0 5px">' + insrt[1] + '%</span></td><td align="center" style="border-right: 1px solid #999"><span style="color:black; background:' + colors[2] + '; padding: 0 5px">' + insrt[2] + ' A</span></td></tr></table>';
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
                    top: 80
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 220;
                return obj;
            },
            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        grid: [{
            top: 80,
            bottom: 100
        }],
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
        axisPointer: {
            show: true
        },
        legend: {
            bottom: 0,
            data: seriesName
        },
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            type: 'value',
            show: true,
            name: seriesName[0],
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            axisPointer: {
                show: true
            }
        },
            {
                type: 'value',
                show: false,
                min: 0,
                max: 100,
                name: seriesName[1],
                index: 1
            }, {
                type: 'value',
                axisLabel: {
                    show: true
                },
                splitLine: {
                    show: false
                },
                name: seriesName[2],
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[0]
                },
                axisPointer: {
                    show: true,
                    label: {
                        backgroundColor: colors[2],
                        textStyle: {
                            color: '#000'
                        }
                    }
                },
                index: 2
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            barGap: '0%',
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 15,
                    formatter: '{c} kW'
                }
            },
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: '{c}%   Load'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: seriesData[1]
            },
            {
                name: seriesName[2],
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        color: '#000',
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: '{c} A'
                    }
                },
                type: 'bar',
                yAxisIndex: 2,
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
            var graphFilDivName = graphDiv + "Graph";
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphState = myChartPerf;
        }
    );
}

function cylinderGraphCall(graphDiv, dateseries, pmaxData, pumpIndexData, exhaustTempData, cfwData) {

    var app1 = {};
    var option = null;

    var colors = ['#2980b9', '#f1c40f', '#2980b9', '#27ae60', '#2980b9', '#c0392b', '#2980b9', '#e67e22', '#2980b9'];

    var seriesName = ['Cyl 01', 'Cyl 02', 'Cyl 03', 'Cyl 04', 'Cyl 05', 'Cyl 06', 'Cyl 07', 'Cyl 08', 'Cyl 09'];

    var dateRange = dateseries;

    option = {
        color: ['#0E2431', '#FC3A52', '#F9B248', ' #CC0033'],
        title: {
            text: 'Power Balance',
            subtext: 'Pump Index (mm) &  Pmax (bar)',
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#A15C7B',
                fontSize: 15
            }
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 380px "><tr class="mtboTitle"><td class="tooltpStyle" align="center" >Cylinder<br>No.</td><td align="center" style="background-color: #428C31">Pmax<br><span>bar</span></td><td align="center" style="background-color: #382CA6 ">Pump Index<br><span>mm</span></td><td align="center">Exhaust Temp.<br><span>ºC</span></td><td align="center">CFW Outlet.<br><span>ºC</span></td></tr>';
                var insrtMax = [];
                var insrtIndex = [];
                var insrtAmbi = [];
                var meanMax = 0;
                var meanIndex = 0;
                var meanTemp = 0;
                var meanCFW = 0;
                var result = params[0].dataIndex;
                var numofCylinder = Number(pmaxData.length);

                for (var i in pmaxData) {
                    tip += '<tr class="mtboRow"><td class="tooltpStyle" align="left">' + (Number(i) + 1) + '</td><td style="color: #382CA6">' + pmaxData[i][result] + '</td><td style="color: #428C31">' + pumpIndexData[i][result] + '</td><td>' + exhaustTempData[i][result] + '</td><td>' + cfwData[i][result] + '</td></tr>';

                    meanMax = meanMax + Number(pmaxData[i][result]);
                    meanIndex = meanIndex + pumpIndexData[i][result];
                    meanTemp = meanTemp + exhaustTempData[i][result];
                    meanCFW = meanCFW + cfwData[i][result];
                }

                meanMax =  meanMax / numofCylinder;
                meanIndex = meanIndex / numofCylinder;
                meanTemp = meanTemp / numofCylinder;
                meanCFW = meanCFW / numofCylinder;

                meanMax = meanMax.toFixed(2);
                meanIndex = meanIndex.toFixed(2);
                meanTemp = meanTemp.toFixed(2);
                meanCFW = meanCFW.toFixed(2);

                tip += '<tr class="mtboRow" style="border: 1px solid #888"><td class="tooltpStyle" align="center">Mean</td><td style="color: #382CA6">' + meanMax + '</td><td style="color: #428C31">' + meanIndex + '</td><td>' + meanTemp + '</td><td>' + meanCFW + '</td></tr></table>';

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
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 1,
            end: 10,
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
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        grid: [{
            top: 80,
            bottom: 100
        }],
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            name: 'Pump Index  (mm)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40,
                fontWeight: 700,
                fontSize: 14,
                color: '#382CA6'
            },
            type: 'value',
            axisPointer: {
                show: true,
                label: {
                    backgroundColor: '#382CA6'
                }
            }
        },
            {
                name: 'Pmax  (bar)',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 30,
                    fontWeight: 700,
                    fontSize: 14,
                    color: '#5DC745'
                },
                type: 'value',
                index: 1,
                show: true,
                position: 'right',
                splitLine: {
                    show: false
                },
                axisPointer: {
                    show: true,
                    label: {
                        backgroundColor: '#5DC745'
                    }
                }
            }
        ],
        series: [{
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 15,
                    formatter: 'Cyl ' + 1 + ':  {c}'
                }
            },
            itemStyle: {
                normal: {
                    color: '#382CA6'
                }
            },
            type: 'bar',
            barGap: 0,
            data: pumpIndexData[0]
        },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[0]
            },

            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 2 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[1]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[1]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 3 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[2]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[2]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 4 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[3]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[3]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 5 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[4]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[4]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 6 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[5]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[5]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 7 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[6]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[6]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 8 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[7]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[7]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 15,
                        formatter: 'Cyl ' + 9 + ':  {c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#382CA6'
                    }
                },
                type: 'bar',
                data: pumpIndexData[8]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 25,
                        color: '#333',
                        formatter: '{c}'
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#5DC745'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: pmaxData[8]
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
            cpGraphState = myChartPerf;
        }
    );
}



function cylinderGraphCall2(graphDiv, dateseries, pmaxData, pumpIndexData, exhaustTempData, cfwData) {

    var app1 = {};
    var option = null;

    var colors = ['#FC3A52', '#FC3A52', '#FC3A52', '#FC3A52', '#FC3A52', '#FC3A52', '#FC3A52', '#FC3A52'];

    var seriesName = ['Cyl 01', 'Cyl 02', 'Cyl 03', 'Cyl 04', 'Cyl 05', 'Cyl 06', 'Cyl 07', 'Cyl 08', 'Cyl 09'];

    var dateRange = dateseries;

    option = {
        color: colors,
        title: {
            subtext: 'Exhaust Temperature ',
            left: 'center',
            subtextStyle: {
                color: '#A15C7B',
                fontSize: 15,

            }
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 380px"><tr class="mtboTitle"><td class="tooltpStyle" align="center" >Cylinder<br>No.</td><td align="center">Pmax<br><span>bar</span></td><td align="center">Pump Index<br><span>mm</span></td><td align="center" style="background-color: #FC3A52">Exhaust Temp.<br><span>ºC</span></td><td align="center">CFW Outlet.<br><span>ºC</span></td></tr>';
                
                var insrtMax = [];
                var insrtIndex = [];
                var insrtAmbi = [];
                var meanMax = 0;
                var meanIndex = 0;
                var meanTemp = 0;
                var meanCFW = 0;
                var result = params[0].dataIndex;
                var numofCylinder = Number(pmaxData.length);


                for (var i in pmaxData) {
                    tip += '<tr class="mtboRow"><td class="tooltpStyle" align="left">' + (Number(i) + 1) + '</td><td style="color: #234357">' + pmaxData[i][result] + '</td><td style="color: #234357">' + pumpIndexData[i][result] + '</td><td style="color: #FC3A52">' + exhaustTempData[i][result] + '</td><td>' + cfwData[i][result] + '</td></tr>';

                    meanMax = meanMax + Number(pmaxData[i][result]);
                    meanIndex = meanIndex + pumpIndexData[i][result];
                    meanTemp = meanTemp + exhaustTempData[i][result];
                    meanCFW = meanCFW + cfwData[i][result];
                }

                meanMax = meanMax / numofCylinder;
                meanIndex = meanIndex / numofCylinder;
                meanTemp = meanTemp / numofCylinder;
                meanCFW = meanCFW / numofCylinder;

                meanMax = meanMax.toFixed(2);
                meanIndex = meanIndex.toFixed(2);
                meanTemp = meanTemp.toFixed(2);
                meanCFW = meanCFW.toFixed(2);

                tip += '<tr class="mtboRow"><td class="tooltpStyle" align="left">Mean</td><td style="color: #234357">' + meanMax + '</td><td style="color: #234357">' + meanIndex + '</td><td style="color: #FC3A52">' + meanTemp + '</td><td>' + meanCFW + '</td></tr></table>';

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
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 1,
            end: 10,
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
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        grid: [{
            top: 50,
            bottom: 100
        }],
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            name: 'Exhaust Temperature (ºC)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            type: 'value',
            axisPointer: {
                show: true,
                label: {
                    backgroundColor: colors[0]
                }
            }
        },
            {
                type: 'value',
                index: 1,
                show: true,
                position: 'right'
            },
            {
                type: 'value',
                index: 2,
                show: false
            },
            {
                type: 'value',
                index: 3,
                show: false
            },
            {
                type: 'value',
                index: 4,
                show: false
            },
            {
                type: 'value',
                index: 5,
                show: false
            },
            {
                type: 'value',
                index: 6,
                show: false
            },
            {
                type: 'value',
                index: 7,
                show: false
            },
            {
                type: 'value',
                index: 8,
                show: false
            }
        ],
        series: [{
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 70,
                    formatter: 'Cyl ' + 1 + ':  {c}'
                }
            },
            type: 'bar',
            data: exhaustTempData[0]
        },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 2 + ':  {c}'

                    }
                },
                type: 'bar',
                data: exhaustTempData[1]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 3 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[2]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 4 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[3]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 5 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[4]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 6 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[5]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 7 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[6]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 8 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[7]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 9 + ':  {c}'
                    }
                },
                type: 'bar',
                data: exhaustTempData[8]
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
            var graphFilDivName = graphDiv + "aGraph";
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphStateA = myChartPerf;
        }
    );
}



function cylinderGraphCall3(graphDiv, dateseries, pmaxData, pumpIndexData, exhaustTempData, cfwData) {

    var app1 = {};
    var option = null;

    var colors = ['#CC5800', '#CC5800', '#CC5800', '#CC5800', '#CC5800', '#CC5800', '#CC5800', '#CC5800'];

    var seriesName = ['Cyl 01', 'Cyl 02', 'Cyl 03', 'Cyl 04', 'Cyl 05', 'Cyl 06', 'Cyl 07', 'Cyl 08', 'Cyl 09'];

    var dateRange = dateseries;

    option = {
        color: colors,
        title: {
            subtext: 'Cooling Fresh Water Outlet',
            left: 'center',
            subtextStyle: {
                color: '#A15C7B',
                fontSize: 15
            }
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = '<table class="tooltipTable" style="margin-bottom: 10px; width: 380px;"><tr class="mtboTitle"><td class="tooltpStyle" align="center" >Cylinder<br>No.</td><td align="center">Pmax<br><span>bar</span></td><td align="center">Pump Index<br><span>mm</span></td><td align="center">Exhaust Temp.<br><span>ºC</span></td><td align="center"style="background-color: #CC5800">CFW Outlet.<br><span>ºC</span></td></tr>';
                var insrtMax = [];
                var insrtIndex = [];
                var insrtAmbi = [];
                var meanMax = 0;
                var meanIndex = 0;
                var meanTemp = 0;
                var meanCFW = 0;
                var result = params[0].dataIndex;
                var numofCylinder = Number(pmaxData.length);


                for (var i in pmaxData) {
                    tip += '<tr class="mtboRow"><td class="tooltpStyle" align="left">' + (Number(i) + 1) + '</td><td style="color: #234357">' + pmaxData[i][result] + '</td><td style="color: #234357">' + pumpIndexData[i][result] + '</td><td style="color: #234357">' + exhaustTempData[i][result] + '</td><td style="color: #CC5800">' + cfwData[i][result] + '</td></tr>';

                    meanMax = meanMax + Number(pmaxData[i][result]);
                    meanIndex = meanIndex + pumpIndexData[i][result];
                    meanTemp = meanTemp + exhaustTempData[i][result];
                    meanCFW = meanCFW + cfwData[i][result];
                }

                meanMax = meanMax / numofCylinder;
                meanIndex = meanIndex / numofCylinder;
                meanTemp = meanTemp / numofCylinder;
                meanCFW = meanCFW / numofCylinder;

                meanMax = meanMax.toFixed(2);
                meanIndex = meanIndex.toFixed(2);
                meanTemp = meanTemp.toFixed(2);
                meanCFW = meanCFW.toFixed(2);

                tip += '<tr class="mtboRow"><td class="tooltpStyle" align="left">Mean</td><td style="color: #234357">' + meanMax + '</td><td style="color: #234357">' + meanIndex + '</td><td style="color: #234357">' + meanTemp + '</td><td style="color: #CC5800">' + meanCFW + '</td></tr></table>';

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
                    top: -100
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 120;
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
            end: 10,
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
            show: true,
            right: 0,
            top: 20,
            itemGap: 30,
            data: "Cylinder 1 - 8",
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        grid: [{
            top: 50,
            bottom: 100
        }],
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            name: 'CFW Outlet (ºC)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 40,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            type: 'value',
            axisPointer: {
                show: true,
                label: {
                    backgroundColor: colors[0]
                }
            }
        },
            {
                type: 'value',
                index: 1,
                show: false,
                position: 'right'
            },
            {
                type: 'value',
                index: 2,
                show: false
            },
            {
                type: 'value',
                index: 3,
                show: false
            },
            {
                type: 'value',
                index: 4,
                show: false
            },
            {
                type: 'value',
                index: 5,
                show: false
            },
            {
                type: 'value',
                index: 6,
                show: false
            },
            {
                type: 'value',
                index: 7,
                show: false
            },
            {
                type: 'value',
                index: 8,
                show: false
            }
        ],
        series: [{
            label: {
                normal: {
                    show: true,
                    rotate: 90,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 70,
                    formatter: 'Cyl ' + 1 + ':  {c}'
                }
            },
            type: 'bar',
            data: cfwData[0]
        },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 2 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: cfwData[1]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 3 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 2,
                data: cfwData[2]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 4 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 3,
                data: cfwData[3]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 5 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 4,
                data: cfwData[4]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 6 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 5,
                data: cfwData[5]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 7 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 6,
                data: cfwData[6]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 8 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 7,
                data: cfwData[7]
            },
            {
                label: {
                    normal: {
                        show: true,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        position: 'insideBottom',
                        distance: 70,
                        formatter: 'Cyl ' + 9 + ':  {c}'
                    }
                },
                type: 'bar',
                yAxisIndex: 8,
                data: cfwData[8]
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
            var graphFilDivName = graphDiv + "bGraph";
            var graphFilDiv = document.getElementById(graphFilDivName);
            var myChartPerf = ec.init(graphFilDiv);
            myChartPerf.setOption(option);
            cpGraphStateB = myChartPerf;
        }
    );
}

// turbo Charger

function tcGraphCall(graphDiv, dateseries, seriesData) {

    var app1 = {};
    var option = null;

    var colors = ['#B379CC', '#B09CD9', '#5851A6', '#CC3C67', '#CC154B'];

    var seriesName = ['Exh. Temp Before T/C ºC', 'Exh. Temp After T/C ºC', 'Charge Air A/E  ºC', 'Charge Air A/E (bar)', 'Air Clr. Diff. P. (mmAq)'];

    var seriesData = seriesData;
    //var seriesData = [
    //    //25	50	75	96	50	25	75	25	75	75	75	75

    //    //Exh. Temp Before T/C   ºC
    //    [475, 477, 478, 478, 485, 490, 492, 493, 450, 451, 451, 452],
    //    //Exh. Temp After T/C   ºC	
    //    [385, 388, 389, 389, 395, 400, 403, 404, 340, 345, 345, 346],
    //    //Air After Blower   ºC	
    //    //[50, 50, 50, 51, 51, 51, 52, 52, 51, 51, 52, 53],
    //    //Charge Air A / E ºC
    //    [36, 39, 39, 41, 39, 36, 39, 36, 39, 39, 39, 39],
    //    //Charge Air A/E  (bar)	
    //    [0.39, 1.1, 1.98, 2.8, 1.1, 0.39, 1.98, 1.1, 1.98, 1.98, 1.98, 1.98],
    //    //Air Clr. Diff. P.  (mmWc)
    //    [25, 50, 75, 96, 50, 25, 75, 25, 75, 75, 75, 75]

    //]

    var dateRange = dateseries;

    option = {
        color: colors,
        title: {
            text: 'Tubo Charger',
            subtext: 'Exhaust In/Out, Charge Air & Air Cooler',
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            }

        },
        tooltip: {
            show: false,
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        grid: {
            x: 70,
            x2: 175,
            y2: 90
        },
        dataZoom: {
            bottom: 30,
            start: 1,
            end: 30,
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
        },
        legend: {
            itemGap: 20,
            itemHeight: 12,
            bottom: 0,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        axisPointer: {
            show: true
        },
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            type: 'value',
            name: 'Exh. Temp Before & After T/C ºC',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 30,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
        },
            {
                type: 'value',
                name: seriesName[1],
                index: 1,
                show: false
            },
            {
                type: 'value',
                name: seriesName[2],
                index: 2,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[2]
                },
                axisLine: {
                    lineStyle: {
                        color: colors[2]
                    }
                },
            },
            {
                type: 'value',
                name: seriesName[3],
                index: 3,
                offset: 60,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[3]
                },
                axisLine: {
                    lineStyle: {
                        color: colors[3]
                    }
                },
                splitLine: {
                    show: false
                }
            },
            {
                type: 'value',
                name: 'Air Cooler Differential Pressure (mmAq)',
                index: 4,
                offset: 126,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[4]
                },
                axisLine: {
                    lineStyle: {
                        color: colors[4]
                    }
                },
            }
        ],
        series: [{
            name: seriesName[0],
            label: {
                normal: {
                    show: true,
                    position: 'insideBottom',
                    distance: 30,
                    rotate: 90,
                    align: 'center',
                    verticalAlign: 'middle',
                    formatter: '{c} °C'
                }
            },
            type: 'bar',
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                label: {
                    normal: {
                        show: true,
                        position: 'insideBottom',
                        distance: 30,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} °C'
                    }
                },
                type: 'bar',
                yAxisIndex: 1,
                data: seriesData[1]
            },
            {
                name: seriesName[2],
                label: {
                    normal: {
                        show: true,
                        position: 'insideBottom',
                        distance: 30,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} °C'
                    }
                },
                type: 'bar',
                yAxisIndex: 2,
                data: seriesData[2]
            }, {
                name: seriesName[3],
                label: {
                    normal: {
                        show: true,
                        position: 'insideBottom',
                        distance: 30,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} bar'
                    }
                },
                type: 'bar',
                barGap: '0%',
                yAxisIndex: 3,
                data: seriesData[3]
            }, {
                name: seriesName[4],
                label: {
                    normal: {
                        show: true,
                        position: 'insideBottom',
                        distance: 30,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} mmWc'
                    }
                },
                type: 'bar',
                barGap: '0%',
                yAxisIndex: 4,
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
            cpGraphState = myChartPerf;
        }
    );
}

// Cooling Water System

function cwsGraphCall(graphDiv, dateseries, seriesdata) {

    var app1 = {};

    var option = null;

    var colors = ['#2a7b9b', '#eddd53'];

    var seriesName = ['J.C.F.W. Inlet (bar)', 'J.C.F.W. Inlet (°C)'];
    var seriesData = seriesdata;
    //var seriesData = [
    //    // Cool F/W (bar)
    //    [3.4, 3.45, 3.5, 3.5, 3.45, 3.4, 3.5, 3.4, 3.5, 3.5, 3.5, 3.5],
    //    // J.C.F.W Inlet (°C)
    //    [25, 50, 75, 96, 50, 25, 75, 25, 75, 75, 75, 75]
    //]

    var dateRange = dateseries;

    option = {
        color: colors,
        title: {
            text: 'Cooling Water Inlet to Engine',
            subtext: '(Pressure & Temperature)',
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            }
        },
        tooltip: {
            trigger: 'none',
            axisPointer: {
                type: 'shadow'
            },
            showContent: false
        },
        legend: {
            left: 'center',
            width: 600,
            bottom: 0,
            itemGap: 30,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        axisPointer: {
            show: true
        },
        grid: {
            x: 70,
            x2: 175,
            y2: 90
        },
        dataZoom: {
            bottom: 30,
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
        },
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            type: 'value',
            name: 'Jacket Cooling Fresh Water (bar)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 10,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            axisPointer: {
                label: {
                    backgroundColor: colors[0]
                }
            }
        },
            {
                type: 'value',
                name: 'Jacket Cooling Fresh Water (°C)',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 20,
                    fontWeight: 700,
                    fontSize: 14,
                    color: '#777'
                },
                splitLine: {
                    show: false
                },
                axisPointer: {
                    label: {
                        backgroundColor: colors[1],
                        textStyle: {
                            color: 'black'
                        }
                    }
                },
                index: 1
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            label: {
                normal: {
                    show: true,
                    position: 'inside',
                    distance: 15,
                    rotate: 90,
                    align: 'left',
                    verticalAlign: 'middle',
                    formatter: '{c} bar'
                }
            },
            barGap: '0%',
            data: seriesData[0]
        },
            {
                name: seriesName[1],
                type: 'bar',
                yAxisIndex: 1,
                data: seriesData[1],
                label: {
                    normal: {
                        show: true,
                        position: 'inside',
                        color: '#000',
                        distance: 15,
                        rotate: 90,
                        align: 'left',
                        verticalAlign: 'middle',
                        formatter: '{c} °C'
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
            cpGraphState = myChartPerf;
        }
    );
}

// Lube Oil System

function losGraphCall(graphDiv, dateseries, seriesdata) {

    var app1 = {};

    var option = null;

    var colors = ['#661510', '#D9351A', '#F2C76F', '#BF9727', '#204C3F'];

    var seriesName = ['LO Inlet to Engine °C', 'L.O. Inlet to Engine (bar)', 'Dif. Press of L.O. Filter (bar)', 'L.O. Inlet to T/C (bar)', 'L.O. Consump L/Day'];
    var seriesData = seriesdata;
    //var seriesData = [
    //    //	A/E LO Inlet    °C		
    //    [65, 68, 66, 66, 68, 65, 66, 65, 66, 66, 65, 66],
    //    //	L.O. Inlet  (bar)		
    //    [5.2, 5.9, 5.9, 5.9, 5.9, 4.2, 5.9, 5.2, 5.9, 5.9, 5.9, 5.9],
    //    //	Differ Press of L.O. Filter  (bar)		
    //    [0.5, 0.5, 0.57, 0.6, 0.5, 0.5, 0.57, 0.5, 0.57, 0.57, 0.57, 0.57],
    //    //	L.O. Inlet of T/C (After Filter)  (bar)		
    //    [6, 6, 6, 6, 6, 3, 6, 5.2, 6, 6, 6, 6],
    //    //	L.O. Consump    L/Day		
    //    [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]
    //]

    //var dateRange = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var dateRange = dateseries;

    option = {
        title: {
            text: 'Lube Oil System',
            subtext: "Temperature, Pressure & Consumption",
            left: 'center',
            textStyle: {
                color: '#C2417C'
            },
            subtextStyle: {
                color: '#bf6d92',
                fontSize: 15
            }
        },
        color: colors,
        tooltip: {
            trigger: 'axis',
            show: false,
            axisPointer: {
                type: 'shadow'
            }
        },
        legend: {
            left: 'center',
            itemGap: 10,
            bottom: 0,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        calculable: true,
        axisPointer: {
            show: true
        },
        grid: {
            x: 120,
            x2: 180,
            y2: 90
        },
        dataZoom: [{
            bottom: 30,
            type: 'slider',
            show: 'true',
            start: 1,
            end: 30,
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
        xAxis: [{
            type: 'category',
            data: dateRange
        }],
        yAxis: [{
            type: 'value',
            offset: 60,
            name: 'Lube Oil Inlet to Engine   (°C)',
            nameLocation: 'center',
            nameRotate: 90,
            nameTextStyle: {
                padding: 20,
                fontWeight: 700,
                fontSize: 14,
                color: colors[0]
            },
            axisLine: {
                lineStyle: {
                    color: colors[0]
                }
            },
            splitLine: {
                show: false
            },
        },
            {
                type: 'value',
                max: 10,
                index: 1,
                position: 'left',
                name: 'Lube Oil Inlet to Engine   (bar)',
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[1]
                },
                axisLine: {
                    lineStyle: {
                        color: colors[1]
                    }
                },
                splitLine: {
                    show: false
                },
            },

            {
                type: 'value',
                name: 'Dif. Press of L.O. Filter (bar)',
                index: 2,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: '#CCA85E'
                },
                axisLine: {
                    lineStyle: {
                        color: '#CCA85E' //colors[2]
                    }
                },
                splitLine: {
                    show: false
                },
            },
            {
                type: 'value',
                name: 'L.O. Inlet to T/C (bar)',
                index: 3,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[3]
                },
                offset: 60,
                axisLine: {
                    lineStyle: {
                        color: colors[3]
                    }
                },
                splitLine: {
                    show: false
                },
            }, {
                type: 'value',
                name: 'L.O. Consump L/Day',
                index: 4,
                offset: 120,
                nameLocation: 'center',
                nameRotate: 90,
                nameTextStyle: {
                    padding: 10,
                    fontWeight: 700,
                    fontSize: 14,
                    color: colors[4]
                },
                axisLine: {
                    lineStyle: {
                        color: colors[4]
                    }
                }
            }
        ],
        series: [{
            name: seriesName[0],
            type: 'bar',
            data: seriesData[0],
            label: {
                normal: {
                    show: true,
                    position: 'insideTop',
                    distance: 25,
                    rotate: 90,
                    align: 'center',
                    verticalAlign: 'middle',
                    formatter: '{c} °C'
                }
            }
        },
            {
                name: seriesName[1],
                type: 'bar',
                barGap: '0%',
                yAxisIndex: 1,
                data: seriesData[1],
                label: {
                    normal: {
                        show: true,
                        position: 'insideTop',
                        distance: 25,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} bar'
                    }
                }
            },
            {
                name: seriesName[2],
                type: 'bar',
                yAxisIndex: 2,
                data: seriesData[2],
                label: {
                    normal: {
                        show: true,
                        position: 'insideTop',
                        distance: 30,
                        rotate: 90,
                        color: '#000',
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} bar'
                    }
                }
            },
            {
                name: seriesName[3],
                type: 'bar',
                yAxisIndex: 3,
                data: seriesData[3],
                label: {
                    normal: {
                        show: true,
                        position: 'insideTop',
                        distance: 25,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} bar'
                    }
                }
            }, {
                name: seriesName[4],
                type: 'bar',
                yAxisIndex: 4,
                data: seriesData[4],
                label: {
                    normal: {
                        show: true,
                        position: 'insideTop',
                        distance: 30,
                        rotate: 90,
                        align: 'center',
                        verticalAlign: 'middle',
                        formatter: '{c} L/Day'
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
            cpGraphState = myChartPerf;
        }
    );
}

// Last Overhaul

function overhaulGraphCall(graphDiv, dateseries, overhauldata, seriesdata) {

    var app1 = {};

    var option = null;

    var colors = ['#19A122', '#D9351A', '#F2C76F', '#BF9727', '#204C3F', '#382CA6', '#661510', '#47CC37'];

    var seriesName = ['Piston (Hrs)', 'F.O. Injector (Hrs)', 'F.O. injection Pump (Hrs)', 'Suc. Valve (Hrs)', 'Exh. Valve (Hrs)', 'T/C Water Washing (Hrs)', 'T/C Renew Bearing (Hrs)'];

    var seriesData = seriesdata;

    var overhaulData = overhauldata;
    //var overhaulData = [
    //    //Piston (Hrs)
    //    [7186, 7522, 80, 771.2, 1143.2, 1323.2, 1881.2, 2067.2, 2607.2, 3165.2, 3705.2, 4263.2],
    //    //F.O.Injector(Hrs)
    //    [76, 412, 970, 661.2, 33.2, 213.2, 771.2, 957.2, 497.2, 55.2, 595.2, 153.2],
    //    //F.O.Injector Pump(Hrs)
    //    [7936, 272, 830, 1521.2, 1893.2, 2073.2, 2631.2, 2817.2, 3357.2, 3915.2, 4455.2, 5013.2],
    //    //Suc.Valve(Hrs)
    //    [3786, 122, 680, 1371.2, 1743.2, 1923.2, 2481.2, 2667.2, 3207.2, 3765.2, 4305.2, 4863.2],
    //    //Exh.Valve(Hrs)
    //    [3586, 3922, 480, 1171.2, 1543.2, 1723.2, 2281.2, 2467.2, 3007.2, 3565.2, 105.2, 663.2],
    //    //T / C Water washing(Hrs)
    //    [36, 22, 30, 21.2, 43.2, 23.2, 31.2, 17.2, 7.2, 15.2, 5.2, 13.2],
    //    //T / C Renew Bearing(Hrs)
    //    [11986, 322, 880, 1571.2, 1943.2, 2123.2, 2681.2, 2867.2, 3407.2, 3965.2, 4505.2, 5063.2]

    //]


    var overhaulRecomends = [16000, 200, 12000, 16000, 16000, 50, 5000];

    var overhaulNames = ['Piston (Hrs)', 'F.O. Injector (Hrs)', 'F. O. injection Pump (Hrs)', 'Suc. Valve (Hrs)', 'Exh. Valve (Hrs)', 'T/C Water Washing (Hrs)', 'T/C Renew Bearing (Hrs)'];

    //var dateRange = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var dateRange = dateseries;

    option = {
        title: {
            text: 'Hours Since Last Overhaul',
            left: 'center',
            textStyle: {
                color: '#C2417C'
            }
        },
        color: colors,
        grid: {
            x: 90,
            y: -315,
            x2: 175,
            y2: 90
        },
        dataZoom: {
            bottom: 300,
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
        },
        tooltip: {
            trigger: 'item',
            show: true,
            axisPointer: {
                type: 'shadow'
            },
            formatter: function (params) {
                var tip = "";
                var insrtH = [];
                var result = params.dataIndex;

                for (var i in overhaulData) {
                    var obj = overhaulData[i];
                    insrtH[i] = obj[result];
                }

                tip = '</table><table class="tooltipTable" style="margin-bottom: 10px; width:380px;"><tr class="mtboTitle"><td class="tooltpStyle" align="left" width="125">' + dateRange[params.dataIndex] + '</td><td align="right"><span>Actuals</span></td><td align="left"><span>Typical<br> MTBO</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[0] + '</td><td align="right"><span>' + insrtH[0] + '</span></td><td align="left"><span>' + overhaulRecomends[0] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[1] + '</td><td align="right"><span>' + insrtH[1] + '</span></td><td align="left"><span>' + overhaulRecomends[1] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[2] + '</td><td align="right"><span>' + insrtH[2] + '</span></td><td align="left"><span>' + overhaulRecomends[2] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[3] + '</td><td align="right"><span>' + insrtH[3] + '</span></td><td align="left"><span>' + overhaulRecomends[3] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[4] + '</td><td align="right"><span>' + insrtH[4] + '</span></td><td align="left"><span>' + overhaulRecomends[4] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[5] + '</td><td align="right"><span>' + insrtH[5] + '</span></td><td align="left"><span>' + overhaulRecomends[5] + '</span></td></tr><tr class="mtboRow"><td class="tooltpStyle" align="left">' + overhaulNames[6] + '</td><td align="right"><span>' + insrtH[6] + '</span></td><td align="left"><span>' + overhaulRecomends[6] + '</span></td></tr></table>';

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
            //				position: function(pos, params, el, elRect, size) { // var obj = { // top: 23 // }; // obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]]=1 20; // return obj; // },
            shared: true,
            extraCssText: 'width: auto; height: auto'
        },
        legend: {
            left: 'center',
            bottom: 0,
            itemGap: 10,
            data: seriesName,
            textStyle: {
                fontWeight: 'bold'
            }
        },
        xAxis: [{
            type: 'category',
            data: dateRange,
            axisTick: {
                show: false
            },
            axisLabel: {
                show: false
            },

            axisLine: {
                show: false
            }
        }],
        yAxis: [{
            type: 'value',
            min: 9,
            max: 11,
            interval: 1,
            axisTick: {
                show: false
            },
            axisLabel: {
                show: false
            },

            axisLine: {
                show: false
            },
            splitLine: {
                show: false
            }
        }],
        series: [{
            type: 'line',
            data: seriesData,
            symbolSize: 20,
            label: {
                normal: {
                    show: true,
                    position: 'insideTop',
                    distance: 45,
                    rotate: 0,
                    color: '#000',
                    align: 'center',
                    verticalAlign: 'middle',
                    formatter: function (params) {
                        var tip = "";
                        var insrtH = [];
                        var result = params.dataIndex;

                        for (var i in dateRange) {
                            var obj = dateRange[i];
                            insrtH[i] = obj[result];
                        }

                        tip = dateRange[params.dataIndex];

                        return tip;
                    }
                }
            }
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
            cpGraphState = myChartPerf;
        }
    );

}

function AeChartResize()
{
    cpGraphState.resize();
    cpGraphStateA.resize();
    cpGraphStateB.resize();

}