<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionVPRSChartAuxillarydetails.aspx.cs" 
    Inherits="VesselPositionVPRSChartAuxillarydetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/bootstrap/js/bootstrap.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

<%--        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart_voyageoverview.js"></script>--%>
        <script type="text/javascript">

    $(document).ready(function() {
        //var seriesData = [[12, 10, 11, 10], [10, 11, 20, 100]];
        //var dateRange = ['12-JAN-2018', '12-FEB-2018', '12-MAR-2018', '12-APR-2018']
        var seriesData = <%=this.seriesdata%>;
        var seriesData1 = <%=this.seriesdata1%>;
        var dateRange = <%=this.dateList%>;
        focAnalysis('voyGraphA',seriesData,dateRange);
        LoadAnalysis('voyGraphB',seriesData1,dateRange);
    });
    var cpGraphState = {};
    var cpGraphStateA = {};
    //Resize window in zoom in/out
    $(window).resize(function () {
        cpGraphState.resize();
        cpGraphStateA.resize();
    });

    function focAnalysis(graphDiv, seriesdata, dateseries) {
        var app1 = {};

        var option = null;

        var colors = ['#4682B4','#FF6347'];

        var seriesName = ['A/E FOC', 'BLR FOC'];

        var seriesData = seriesdata;
        var dateRange = dateseries;
        var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
        option = {
            title: {
                text: "A/E & BLR FOC at Sea",
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

                    tip =  '<table class="tooltipTable"><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  steelBlue">•</span>' + seriesName[0] + '</td><td> ' + insrt[0] + ' t</td><td></td></tr><tr><td><span class="tlTip tomatoOrange">•</span>' + seriesName[1] + '</td><td> ' + insrt[1] + ' t</td><td></td></tr></table>';
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
                max: seriesDataMax,
                min: 0,
                name: 'MT',
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
                    max: seriesDataMax,
                    min: 0,
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
                data: seriesData[0]
            },
                {
                    name: seriesName[1],
                    show: false,
                    type: 'bar',
                    label: {
                        normal: {
                            show: false,
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
                var graphFilDivName =  "voyGraphA";
                var graphFilDiv = document.getElementById(graphFilDivName);
                var myChartPerf = ec.init(graphFilDiv);
                myChartPerf.setOption(option);
                cpGraphState = myChartPerf;
            }
        );
    }
   
    function LoadAnalysis(graphDiv, seriesdata1, dateseries) {
        var app1 = {};

        var option = null;

        var colors = ['#4682B4','#FF6347'];

        var seriesName = ['Total Capacity Available', 'Total Capacity Utilized'];

        var seriesData = seriesdata1;
        var seriesDataMax = Math.ceil(Math.max(Math.max.apply(null, seriesData[0]), Math.max.apply(null, seriesData[1])));
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


                    tip =  '<table class="tooltipTable" width="600px" ><tr><td class="tooltpStyle"><span class="pad20">' + dateseries[result] + '</span></td></tr></table><table class="tooltipTable"><tr><td><span class="tlTip  steelBlue">•</span>' + seriesName[0] + '</td><td> ' + insrt[0] + 'kWh</td><td></td></tr><tr><td><span class="tlTip tomatoOrange">•</span>' + seriesName[1] + '</td><td> ' + insrt[1] + ' kWh</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>Load %</td><td> ' + insrt[2] + '%</td><td></td></tr></table><table class="tooltipTable"><tr><td></td><td><b>AE1</b></td><td><b>AE2</b></td><td><b>AE3</b></td></tr><tr><td><span class="tlTip oliveGreen">•</span>Max Load (kW)</td><td> ' + insrt[3] + '</td><td> ' + insrt[8] + '</td><td> ' + insrt[13] + '</td><td></td></tr><tr><td><span class="tlTip milkyEmerald">•</span>AE Load (kW)</td><td> ' + insrt[4] + '</td><td> ' + insrt[9] + '</td><td> ' + insrt[14] + '</td><td></td></tr><tr><td><span class="tlTip darkTeal">•</span>AE Hrs Run</td><td> ' + insrt[5] + '</td><td> ' + insrt[10] + '</td><td> ' + insrt[15] + '</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>AE Cap Available (kWh)</td><td> ' + insrt[7] + '</td><td> ' + insrt[12] + '</td><td> ' + insrt[17] + '</td><td></td></tr><tr><td><span class="tlTip darkMagenda">•</span>AE Load Utilized (kWh)</td><td> ' + insrt[6] + '</td><td> ' + insrt[11] + '</td><td> ' + insrt[16] +  '</td><td></td></tr></table>';
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
                max: seriesDataMax,
                min: 0,
                name: 'kwh',
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
                    max: seriesDataMax,
                    min: 0,
                    axisLabel: {
                        show: false
                    },
                    index: 1
                }
            ],
            series: [{
                name: seriesName[0],
                type: 'scatter',
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
                data: seriesData[0]
            },
                {
                    name: seriesName[1],
                    show: false,
                    type: 'bar',
                    label: {
                        normal: {
                            show: false,
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
                var graphFilDivName =  "voyGraphB";
                var graphFilDiv = document.getElementById(graphFilDivName);
                var myChartPerf = ec.init(graphFilDiv);
                myChartPerf.setOption(option);
                cpGraphStateA = myChartPerf;
            }
        );
    }

</script>
</telerik:RadCodeBlock>
</head>
<body style="background: rgba(230,245,254,1)">
<form id="form1" runat="server">
    <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />


    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

             <eluc:TabStrip ID="MenuChart" runat="server" OnTabStripCommand="MenuChart_TabStripCommand"
             TabStrip="true"></eluc:TabStrip>
    
  <div style="height: 500px; width: 80%; margin: 0 auto; padding: 10px; background: white;">
    <div id="graphFilterDiv" style="padding: 5px; background: #eee; maring-bottom: 5px; font-weight: 700; color: #777;">
      <table width="100%">
          <td><telerik:RadLabel runat="server" ID="lblFromDate" Text="From Date"></telerik:RadLabel> </td>
        <td>
<%--          <asp:TextBox class="fromCPDatePick" size="10" id="fromDateInput" runat="server" OnTextChanged="fromDateInput_TextChanged1" AutoPostBack="true" Visible ="true"></asp:TextBox>
           <ajaxToolkit:CalendarExtender ID="ajxfromDateInput" runat="server" Format="dd/MMM/yyyy"
               Enabled="True" TargetControlID="fromDateInput" PopupPosition="TopRight">
           </ajaxToolkit:CalendarExtender>--%>

            <telerik:RadDatePicker ID="fromDateInput" runat="server" Culture="English (United States)" AutoPostBack="true" OnSelectedDateChanged="fromDateInput_SelectedDateChanged">         
                            </telerik:RadDatePicker> 

        </td>
          <td><telerik:RadLabel runat="server" ID="lblodate" Text="To Date"></telerik:RadLabel> </td>
        <td>
<%--          <asp:TextBox class="fromCPDatePick" size="10" id="toDateInput" runat="server" OnTextChanged="toDateInput_TextChanged" AutoPostBack="true" Visible ="true"></asp:TextBox>
           <ajaxToolkit:CalendarExtender ID="ajxtoDateInput" runat="server" Format="dd/MMM/yyyy"
               Enabled="True" TargetControlID="toDateInput" PopupPosition="TopRight">
           </ajaxToolkit:CalendarExtender>--%>

            <telerik:RadDatePicker ID="toDateInput" runat="server" Culture="English (United States)" AutoPostBack="true" OnSelectedDateChanged="toDateInput_SelectedDateChanged">         
                            </telerik:RadDatePicker>
        </td>
      </table>
    </div>

<%--    <div id="cpGraph" style="height: 100%; width: 100%; margin: 0 auto;"></div>--%>
    <div style="background: white"><div id="voyGraphA" style="box-sizing: border-box; height: 350px; width: 100%; margin: 0px auto; border: 1px solid grey; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648848"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 61px; top: 61px; width: auto; height: auto;"><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td class="tooltpStyle" width="125" align="center">Laden</td><td align="center"><span class="greenSpeed"> Bf-3</span></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td><span class="tlTip style2">•</span>SOG</td><td> 15.1Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>LOG</td><td>15.2Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>CP Speed</td><td>15Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="redStatus">0.67</span></td><td><span class="redStatus">0.1kn</span></td></tr><tr><td><span class="tlTip style3">•</span>Trim</td><td>0.5m</td><td></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkGreen">•</span>RPM</td><td>98</td></tr><tr><td><span class="tlTip steelBlue">•</span>Load</td><td><span class="greenSpeed">79%</span></td></tr><tr><td><span class="tlTip style8">•</span>SLIP</td><td>5%</td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkMagenda">•</span>FOC</td><td> 19.5 t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>DOC Rate</td><td> 20 t</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">2.50%</span></td><td><span class="greenStatus">0.5 t</span></td></tr></tbody></table></div></div></div>
    <div style="background: white"><div id="voyGraphB" style="height: 350px; width: 100%; margin: 20px auto 0px; border: 1px solid grey; box-sizing: border-box; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648847"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 61px; top: -159px; width: auto; height: auto;"><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td class="tooltpStyle" width="125" align="center">Laden</td><td align="center"><span class="redSpeed"> Bf-6</span></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td><span class="tlTip style2">•</span>SOG</td><td> 6Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>LOG</td><td>6Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>CP Speed</td><td>15Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">60.00</span></td><td><span class="greenStatus">9kn</span></td></tr><tr><td><span class="tlTip style3">•</span>Trim</td><td>0.5m</td><td></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkGreen">•</span>RPM</td><td>0</td></tr><tr><td><span class="tlTip steelBlue">•</span>Load</td><td><span class="greenSpeed">0%</span></td></tr><tr><td><span class="tlTip style8">•</span>SLIP</td><td>0%</td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkMagenda">•</span>FOC</td><td> 0 t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>DOC Rate</td><td> 20 t</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">100.00%</span></td><td><span class="greenStatus">20 t</span></td></tr></tbody></table></div></div></div>
    <%--<div style="background: white"><div id="voyGraphC" style="height: 250px; width: 100%; margin: 20px auto 0px; border: 1px solid grey; box-sizing: border-box; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648846"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div></div></div></div>--%>
  </div>
</form>
</body>
</html>


