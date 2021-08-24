<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformance.aspx.cs"
    Inherits="Dashboard_DashboardTechnicalAuxiliaryPerformance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

<%--    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" />--%>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />--%>
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/DashboardAePerformanceGraph.js"></script>
    
    <style>
        body {
            font-family: Tahoma;
            font-size: 12px;
            margin: 0;
            padding: 0;
        }

        table{
            font-family: Tahoma;
            font-size: 12px;
        }

        .newBGgrad {
            /*weight: 200px;*/
            background: #263744;
            /* For browsers that do not support gradients */
            background: -webkit-linear-gradient(left top, #263744, #7c9a98);
            /* For Safari 5.1 to 6.0 */
            background: -o-linear-gradient(bottom right, #263744, #7c9a98);
            /* For Opera 11.1 to 12.0 */
            background: -moz-linear-gradient(bottom right, #263744, #7c9a98);
            /* For Firefox 3.6 to 15 */
            background: linear-gradient(to bottom right, #263744, #7c9a98);
            /* Standard syntax */
            display: flex;
        }

        .sideVertical {
            color: #6b7d8e;
            /*#7b9795;*/
            padding: auto;
            padding-top: 30px;
            padding-bottom: 30px;
            background: #263744;
            background: -webkit-linear-gradient(#2c3b46, #30434e);
            background: -o-linear-gradient(#263744, #7c9a98);
            background: -moz-linear-gradient(#263744, #7c9a98);
            background: linear-gradient(#2c3b46, #30434e);
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.4);
            box-sizing: border-box;
        }

        .menuAEStyle {
            width: 80px;
            height: 50px;
            font-weight: 700;
            background-color: none;
            border-radius: 4px;
            border: 1px solid rgba(0, 0, 0, 0);
            margin: 5px 10px;
            text-align: center;
            vertical-align: middle;
            display: table;
            line-height: 50px;
        }

            .menuAEStyle > em {
                color: white;
                position: absolute;
                left: 87px;
                font-size: 20px;
                text-shadow: 0 4px 2px 0 rgba(0, 0, 0, 0.8);
                display: none;
            }

            .menuAEStyle:hover {
                cursor: pointer;
                color: rgba(255, 255, 255, 1);
                background: rgba(0, 0, 0, 0.26);
                border: 1px solid rgba(255, 255, 255, 0.03);
                top: 4px;
                box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            }

            .menuAEStyle:focus {
                outline: none;
            }

        .activeAE,
        .activeAE:hover {
            color: #263744;
            background: rgba(255, 255, 255, 1);
            border: 1px solid rgba(255, 255, 255, 0.03);
            top: 4px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

            .activeAE > span {
                color: #FF5733;
            }

            .activeAE > em {
                display: inline;
            }

        .aeTitle {
            background: #FFC300;
            color: #333;
            font-weight: 700;
            font-size: 12px;
            padding: 10px 20px;
            margin-bottom: 20px;
            text-align: center;
            display: flex;
        }

            .aeTitle > div {
                flex: 1;
                text-align: left;
            }

                .aeTitle > div > span {
                    margin-bottom: 3px;
                    padding: 3px 5px;
                }

                    .aeTitle > div > span:nth-child(1),
                    .aeTitle > div > span:nth-child(4) {
                        color: white;
                        line-height: 22px;
                        background: rgba(60, 60, 60, 1);
                        border-top-left-radius: 8px;
                        border-bottom-left-radius: 8px;
                        float: left;
                        width: 100px;
                        text-align: right;
                        font-size: 12px !important;
                    }

                    .aeTitle > div > span:nth-child(2),
                    .aeTitle > div > span:nth-child(5) {
                        color: black;
                        line-height: 20px;
                        background: white;
                        border-top-right-radius: 8px;
                        border-bottom-right-radius: 8px;
                        width: 200px;
                        float: left;
                        border: 1px solid #bbb;
                    }

                .aeTitle > div > br {
                    clear: both;
                }


        .chartContainer {
            box-sizing: border-box;
            width: 100%;
            height: 100%;
        }

        .chartsBlock {
            width: 100%;
            padding: 10px;
        }

        .currentChart {
            display: block;
        }

        .chartab {
            max-width: 98%;
            position: relative;
            box-sizing: border-box;
            /*margin: 10 auto;*/
            padding: 0;
            margin: 5px auto;
            /*box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);*/
            display: none;
            -webkit-animation-name: blockAnim;
            /* Safari 4.0 - 8.0 */
            -webkit-animation-duration: 0.8s;
            /* Safari 4.0 - 8.0 */
            animation-name: blockAnim;
            animation-duration: 0.8s;
        }

        /* Safari 4.0 - 8.0 */

        @-webkit-keyframes blockAnim {
            from {
                margin-top: 15px;
            }

            to {
                margin-top: 5;
            }
        }

        /* Standard syntax */

        @keyframes blockAnim {
            from {
                margin-top: 15px;
            }

            to {
                margin-top: 5;
            }
        }

        .chart1 {
            width: 200px;
            height: 200px;
            background: white;
        }

        .nizhal {
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        #tabs01Nav,
        #tabs02Nav,
        #tabs03Nav,
        #tabs04Nav {
            display: flex;
            overflow: hidden;
        }

        .tabs01Btn,
        .tabs02Btn,
        .tabs03Btn,
        .tabs04Btn {
            border-color: transparent;
            font-size: 14px;
            font-weight: 700;
            background: rgba(255, 255, 255, 0.5);
            color: #263744;
            padding: 5px 10px;
            cursor: pointer;
            transition: 0.4s;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            margin-right: 2px;
        }

            .tabs01Btn:focus,
            .tabs02Btn:focus,
            .tabs03Btn:focus,
            .tabs04Btn:focus {
                outline: none;
            }

            .tabs01Btn:hover,
            .tabs02Btn:hover,
            .tabs03Btn:hover,
            .tabs04Btn:hover {
                background: rgba(255, 255, 255, 0.75);
            }

            .tabs01Btn:last-child,
            .tabs02Btn:last-child,
            .tabs03Btn:last-child,
            .tabs04Btn:last-child {
                margin-right: 0px;
            }

        .tabsBtnActive,
        .tabsBtnActive:hover,
        .tabsBtnActive:focus {
            background: rgba(255, 255, 255, 1);
            font-weight: 700;
            color: #FF5733;
            outline: none;
        }

        .chartsDiv {
            padding: 10px;
            background: rgba(255, 255, 255, 1);
            height: 600px;
        }

        .chartDashDiv {
            padding: 10px;
            background: rgba(255, 255, 255, 1);
            height: 500px;
        }

        .ae01Chart,
        .ae02Chart,
        .ae03Chart,
        .ae04Chart {
            height: 400px;
            display: none;
        }

        .currentAE {
            display: block;
        }

        .mtboTitle > td,
        .ambiTitle > td {
            background: #2F4E57;
            color: #fff;
            border-bottom: 1px solid #bbb;
            font-weight: 200;
        }

        .ambiTitle > span {
            color: #416B78;
        }

        .mtboRow > td:nth-child(2) {
            font-weight: 700;
            color: #FF55A3;
        }

        .mtboRow > td:nth-child(3) {
            color: #855CCC;
        }

        .ambiRow > td:nth-child(2) {
            font-weight: 700;
            color: #234357;
        }

        .mtboABRRV {
            background: #333;
            color: white;
            padding: 5px 10px;
            text-align: center;
        }

        .subTextLine {
            font-size: 12px;
            color: #C2596D;
            line-height: 12px;
        }

    </style>
    
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAuxiliary" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="chartContainer">
            <div class="newBGgrad">
                <div class="sideVertical">
                    <div class="menuAEStyle activeAE" data-tab="AE01ChartBlock" id="1">A/E <span>#1</span><em>►</em></div>
                    <div class="menuAEStyle" data-tab="AE02ChartBlock" id="2">A/E <span>#2</span><em>►</em></div>
                    <div class="menuAEStyle" data-tab="AE03ChartBlock" id="3">A/E <span>#3</span><em>►</em></div>
                    <div class="menuAEStyle" data-tab="AE04ChartBlock" id="4">A/E <span>#4</span><em>►</em></div>
                </div>
                <div class="chartsBlock">
                    <div class="aeTitle" style="max-width: 98%; margin: 0 auto; margin-bottom: 10px;" id="divHeader">
                    </div>
                    <div id="AE00ChartBlock" class="chartab">
                        <div class="nizhal">
                            <div class="chartDashDiv">
                                <div class="ae00DashDiv" id="ae00Dash01">
                                    <!--								Total Working Hours<br/> Mean SFOC Consumption<br/> Switchboard Load Chart-->

                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="AE01ChartBlock" class="chartab currentAE">
                        <div class="nizhal">
                            <div id="tabs01Nav">
                                <input type="button" data-tab="ae01Chart01" class="tabs01Btn tabsBtnActive" value="SFOC" />
                                <input type="button" data-tab="ae01Chart02" class="tabs01Btn" value="Switchboard Load" />
                                <input type="button" data-tab="ae01Chart03" class="tabs01Btn" value="Power Balance" />
                                <input type="button" data-tab="ae01Chart04" class="tabs01Btn" value="Turbo Charger" />
                                <input type="button" data-tab="ae01Chart05" class="tabs01Btn" value="Cooling Water System" />
                                <input type="button" data-tab="ae01Chart06" class="tabs01Btn" value="Lube Oil System" />
                                <input type="button" data-tab="ae01Chart07" class="tabs01Btn" value="Last Overhaul" />
                            </div>

                            <div class="chartsDiv" style="min-height: 1400px;">
                                <div class="ae01Chart currentAE" id="ae01Chart01">
                                    <div id="ae01Chart01Graph" style="height: 500px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979493">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 499px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 100%; height: 499px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="499" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: block; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 734px; top: 23px; width: auto; height: auto;">
                                        </div>
                                    </div>
                                    <div id="ae01Chart01aGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979492">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 399px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 399px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="399" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 745px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Actual Load</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Fuel Consumption</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Load</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #c0392b; padding: 0 5px">1.5 MT/day</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2980b9; padding: 0 5px">299 kW</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span>25%</span></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="ae01Chart01bGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979491">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 400px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 400px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="400" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 582px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="background: #555; color: #fff" align="center">&nbsp;</td>
                                                        <td style="background: #555; color: #fff" align="center">Actuals</td>
                                                        <td style="background: #555; color: #fff" align="center">Normal Operating Data</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">FO Inlet Temp.</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2D542B; padding: 3px 5px">62.5 ºC</span></td>
                                                        <td align="center">110ºC ~ 140ºC</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-right: 1px solid #999" align="center">FO Inlet Pressure</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: #000; background: #B5B232; padding: 3px 5px">2.85 bar</span></td>
                                                        <td align="center">8 bar</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="ae01Chart" id="ae01Chart02">
                                    <div id="ae01Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae01Chart" id="ae01Chart03">
                                    <div id="ae01Chart03Graph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae01Chart03aGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae01Chart03bGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae01Chart" id="ae01Chart04">

                                    <div id="ae01Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 30px;"></div>
                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp Before T/C</td>
                                                <td style="padding: 10px 0" align="left"><span style="color: white; background: #B379CC; padding: 2px 10px; margin: 10px">450°C ~ 550°C</span></td>
                                                <td style="background: #c3c3c3; padding: 5px 10px; font-weight: normal; border-left: 1px solid #aaa" align="center">Air Cooler Diff. Pressure</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp After T/C</td>
                                                <td align="left"><span style="color: black; background: #B09CD9; padding: 2px 10px; margin: 10px">250ºC ~ 380ºC</span></td>
                                                <td rowspan="3" style="border-left: 1px solid #aaa; padding-left: 5px" align="left"><b style="color: #1E2054"><b>Normal:</b> 10~15 milibar<br>
                                                    <b>Cleaning: </b>30 milibar
                                                    <br>
                                                    <br>
                                                    <b>1 milibar = 10 mmAq</b></td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c6c6c6; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Temp.</td>
                                                <td align="left"><span style="color: white; background: #5851A6; padding: 2px 10px; margin: 10px">40ºC</span></td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Pressure</td>
                                                <td align="left"><span style="color: white; background: #CC3C67; padding: 2px 10px; margin: 10px">2.0 bar</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="ae01Chart" id="ae01Chart05">
                                    <div id="ae01Chart05Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>

                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="2" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #ccc; padding: 5px 10px; font-weight: normal" align="center">J.C.F.W. Inlet - Temperature</td>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">J.C.F.W. Inlet - Presure</td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 10px 0" align="center"><span style="color: white; background: #2a7b9b; padding: 2px 10px; margin: 10px">75 ~ 85°C</span></td>
                                                <td align="center"><span style="color: #333; background: #eddd53; padding: 2px 10px; margin: 10px">2.5~4.5 bar</span></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                                <div class="ae01Chart" id="ae01Chart06" style="width: 100%">
                                    <div id="ae01Chart06Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>

                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="5" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">LO Inlet - Temp.</td>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">L.O. Inlet - Presure</td>
                                                <td class="tooltpStyle" style="background: #c6c6c6; padding: 5px 10px; font-weight: normal" align="center">Dif. Press of L.O. Filter</td>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">L.O. Inlet to T/C</td>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="center">L.O. Consumption/Day</td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 10px 0" align="center"><span style="color: white; background: #661510; padding: 2px 10px; margin: 10px">60°C ~ 70°C</span></td>
                                                <td align="center"><span style="color: white; background: #D9351A; padding: 2px 10px; margin: 10px">4.0 bar</span></td>
                                                <td align="center"><span style="color: black; background: #F2C76F; padding: 2px 10px; margin: 10px">0.1 ~ 1.0 bar</span></td>
                                                <td align="center"><span style="color: black; background: #BF9727; padding: 2px 10px; margin: 10px">2.0 bar</span></td>
                                                <td align="center"><span style="color: white; background: #204C3F; padding: 2px 10px; margin: 10px">6 L/Day</span>
                                                    <!--  0.2/1000/0.9*1200*24   -->
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                                <div class="ae01Chart" id="ae01Chart07" style="width: 100%">
                                    <div id="ae01Chart07Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="AE02ChartBlock" class="chartab">
                        <div class="nizhal">
                            <div id="tabs02Nav">
                                <input type="button" data-tab="ae02Chart01" class="tabs02Btn tabsBtnActive" value="SFOC" />
                                <input type="button" data-tab="ae02Chart02" class="tabs02Btn" value="Switchboard Load" />
                                <input type="button" data-tab="ae02Chart03" class="tabs02Btn" value="Power Balance" />
                                <input type="button" data-tab="ae02Chart04" class="tabs02Btn" value="Turbo Charger" />
                                <input type="button" data-tab="ae02Chart05" class="tabs02Btn" value="Cooling Water System" />
                                <input type="button" data-tab="ae02Chart06" class="tabs02Btn" value="Lube Oil System" />
                                <input type="button" data-tab="ae02Chart07" class="tabs02Btn" value="Last Overhaul" />
                            </div>
                            <div class="chartsDiv" style="min-height: 1400px;">
                                <div class="ae02Chart currentAE" id="ae02Chart01">
                                    <div id="ae02Chart01Graph" style="height: 500px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979493">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 499px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 100%; height: 499px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="499" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: block; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 734px; top: 23px; width: auto; height: auto;">
                                        </div>
                                    </div>
                                    <div id="ae02Chart01aGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979492">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 399px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 399px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="399" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 745px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Actual Load</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Fuel Consumption</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Load</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #c0392b; padding: 0 5px">1.5 MT/day</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2980b9; padding: 0 5px">299 kW</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span>25%</span></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="ae02Chart01bGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979491">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 400px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 400px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="400" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 582px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="background: #555; color: #fff" align="center">&nbsp;</td>
                                                        <td style="background: #555; color: #fff" align="center">Actuals</td>
                                                        <td style="background: #555; color: #fff" align="center">Normal Operating Data</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">FO Inlet Temp.</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2D542B; padding: 3px 5px">62.5 ºC</span></td>
                                                        <td align="center">110ºC ~ 140ºC</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-right: 1px solid #999" align="center">FO Inlet Pressure</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: #000; background: #B5B232; padding: 3px 5px">2.85 bar</span></td>
                                                        <td align="center">8 bar</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="ae02Chart" id="ae02Chart02">
                                    <div id="ae02Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae02Chart" id="ae02Chart03">
                                    <div id="ae02Chart03Graph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae02Chart03aGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae02Chart03bGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae02Chart" id="ae02Chart04">

                                    <div id="ae02Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 30px;"></div>

                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp Before T/C</td>
                                                <td style="padding: 10px 0" align="left"><span style="color: white; background: #B379CC; padding: 2px 10px; margin: 10px">60°C ~ 70°C</span></td>
                                                <td style="background: #c3c3c3; padding: 5px 10px; font-weight: normal; border-left: 1px solid #aaa" align="center">Air Cooler Diff. Pressure</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp After T/C</td>
                                                <td align="left"><span style="color: black; background: #B09CD9; padding: 2px 10px; margin: 10px">4.0 ºC</span></td>
                                                <td rowspan="3" style="border-left: 1px solid #aaa; padding-left: 5px" align="left"><b style="color: #1E2054">1 milibar = 10 mmAq</b><br>
                                                    <br>
                                                    <b>Normal:</b> 10~15 milibar<br>
                                                    <b>Cleaning: </b>30 milibar</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c6c6c6; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Temp.</td>
                                                <td align="left"><span style="color: white; background: #5851A6; padding: 2px 10px; margin: 10px">0.1 ~ 1.0 bar</span></td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Pressure</td>
                                                <td align="left"><span style="color: white; background: #CC3C67; padding: 2px 10px; margin: 10px">2.0 bar</span></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                                <div class="ae02Chart" id="ae02Chart05">
                                    <div id="ae02Chart05Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae02Chart" id="ae02Chart06">
                                    <div id="ae02Chart06Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae02Chart" id="ae02Chart07" style="width: 100%">
                                    <div id="ae02Chart07Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="AE03ChartBlock" class="chartab">
                        <div class="nizhal">
                            <div id="tabs03Nav">
                                <input type="button" data-tab="ae03Chart01" class="tabs03Btn tabsBtnActive" value="SFOC" />
                                <input type="button" data-tab="ae03Chart02" class="tabs03Btn" value="Switchboard Load" />
                                <input type="button" data-tab="ae03Chart03" class="tabs03Btn" value="Power Balance" />
                                <input type="button" data-tab="ae03Chart04" class="tabs03Btn" value="Turbo Charger" />
                                <input type="button" data-tab="ae03Chart05" class="tabs03Btn" value="Cooling Water System" />
                                <input type="button" data-tab="ae03Chart06" class="tabs03Btn" value="Lube Oil System" />
                                <input type="button" data-tab="ae03Chart07" class="tabs03Btn" value="Last Overhaul" />
                            </div>
                            <div class="chartsDiv" style="min-height: 1400px;">
                                <div class="ae03Chart currentAE" id="ae03Chart01">
                                    <div id="ae03Chart01Graph" style="height: 500px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979493">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 499px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 100%; height: 499px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="499" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: block; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 734px; top: 23px; width: auto; height: auto;">
                                        </div>
                                    </div>
                                    <div id="ae03Chart01aGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979492">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 399px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 399px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="399" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 745px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Actual Load</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Fuel Consumption</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Load</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #c0392b; padding: 0 5px">1.5 MT/day</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2980b9; padding: 0 5px">299 kW</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span>25%</span></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="ae03Chart01bGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979491">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 400px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 400px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="400" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 582px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="background: #555; color: #fff" align="center">&nbsp;</td>
                                                        <td style="background: #555; color: #fff" align="center">Actuals</td>
                                                        <td style="background: #555; color: #fff" align="center">Normal Operating Data</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">FO Inlet Temp.</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2D542B; padding: 3px 5px">62.5 ºC</span></td>
                                                        <td align="center">110ºC ~ 140ºC</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-right: 1px solid #999" align="center">FO Inlet Pressure</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: #000; background: #B5B232; padding: 3px 5px">2.85 bar</span></td>
                                                        <td align="center">8 bar</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="ae03Chart" id="ae03Chart02">
                                    <div id="ae03Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae03Chart" id="ae03Chart03">
                                    <div id="ae03Chart03Graph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae03Chart03aGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae03Chart03bGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae01Chart" id="ae03Chart04">

                                    <div id="ae03Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 30px;"></div>

                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp Before T/C</td>
                                                <td style="padding: 10px 0" align="left"><span style="color: white; background: #B379CC; padding: 2px 10px; margin: 10px">60°C ~ 70°C</span></td>
                                                <td style="background: #c3c3c3; padding: 5px 10px; font-weight: normal; border-left: 1px solid #aaa" align="center">Air Cooler Diff. Pressure</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp After T/C</td>
                                                <td align="left"><span style="color: black; background: #B09CD9; padding: 2px 10px; margin: 10px">4.0 ºC</span></td>
                                                <td rowspan="3" style="border-left: 1px solid #aaa; padding-left: 5px" align="left"><b style="color: #1E2054">1 milibar = 10 mmAq</b><br>
                                                    <br>
                                                    <b>Normal:</b> 10~15 milibar<br>
                                                    <b>Cleaning: </b>30 milibar</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c6c6c6; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Temp.</td>
                                                <td align="left"><span style="color: white; background: #5851A6; padding: 2px 10px; margin: 10px">0.1 ~ 1.0 bar</span></td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Pressure</td>
                                                <td align="left"><span style="color: white; background: #CC3C67; padding: 2px 10px; margin: 10px">2.0 bar</span></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                                <div class="ae03Chart" id="ae03Chart05">
                                    <div id="ae03Chart05Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae03Chart" id="ae03Chart06">
                                    <div id="ae03Chart06Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae03Chart" id="ae03Chart07" style="width: 100%">
                                    <div id="ae03Chart07Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="AE04ChartBlock" class="chartab">
                        <div class="nizhal">
                            <div id="tabs04Nav">
                                <input type="button" data-tab="ae04Chart01" class="tabs04Btn tabsBtnActive" value="SFOC" />
                                <input type="button" data-tab="ae04Chart02" class="tabs04Btn" value="Switchboard Load" />
                                <input type="button" data-tab="ae04Chart03" class="tabs04Btn" value="Power Balance" />
                                <input type="button" data-tab="ae04Chart04" class="tabs04Btn" value="Turbo Charger" />
                                <input type="button" data-tab="ae04Chart05" class="tabs04Btn" value="Cooling Water System" />
                                <input type="button" data-tab="ae04Chart06" class="tabs04Btn" value="Lube Oil System" />
                                <input type="button" data-tab="ae04Chart07" class="tabs04Btn" value="Last Overhaul" />
                            </div>
                            <div class="chartsDiv" style="min-height: 1400px;">
                                <div class="ae04Chart currentAE" id="ae04Chart01">
                                    <div id="ae04Chart01Graph" style="height: 500px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979493">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 499px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 100%; height: 499px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="499" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: block; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 734px; top: 23px; width: auto; height: auto;">
                                        </div>
                                    </div>
                                    <div id="ae04Chart01aGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979492">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 399px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 399px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="399" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 745px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Actual Load</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Fuel Consumption</td>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">Load</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #c0392b; padding: 0 5px">1.5 MT/day</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2980b9; padding: 0 5px">299 kW</span></td>
                                                        <td style="border-right: 1px solid #999" align="center"><span>25%</span></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="ae04Chart01bGraph" style="height: 400px; width: 100%; margin: 30px auto 0px; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516429979491">
                                        <div style="position: relative; overflow: hidden; width: 1184px; height: 400px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                            <canvas style="position: absolute; left: 0px; top: 0px; width: 1184px; height: 400px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1184" height="400" data-zr-dom-id="zr_0"></canvas>
                                        </div>
                                        <div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 582px; top: 93px; width: auto; height: auto;">
                                            <table class="tooltipTable" style="margin: 10px;" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="background: #555; color: #fff" align="center">&nbsp;</td>
                                                        <td style="background: #555; color: #fff" align="center">Actuals</td>
                                                        <td style="background: #555; color: #fff" align="center">Normal Operating Data</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-bottom: 1px solid #999; border-right: 1px solid #999" align="center">FO Inlet Temp.</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: white; background: #2D542B; padding: 3px 5px">62.5 ºC</span></td>
                                                        <td align="center">110ºC ~ 140ºC</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tooltpStyle" style="border-right: 1px solid #999" align="center">FO Inlet Pressure</td>
                                                        <td style="border-right: 1px solid #999" align="center"><span style="color: #000; background: #B5B232; padding: 3px 5px">2.85 bar</span></td>
                                                        <td align="center">8 bar</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="ae04Chart" id="ae04Chart02">
                                    <div id="ae04Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae04Chart" id="ae04Chart03">
                                    <div id="ae04Chart03Graph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae04Chart03aGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                    <div id="ae04Chart03bGraph" style="height: 300px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae04Chart" id="ae04Chart04">

                                    <div id="ae04Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 30px;"></div>

                                    <table style="margin: 40px auto; border: 1px solid #999; box-shadow: 2px 4px 8px rgba(0,0,0,0.3)">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" style="padding: 5px; color: white; background: #333;" align="center">Normal Operating Data</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp Before T/C</td>
                                                <td style="padding: 10px 0" align="left"><span style="color: white; background: #B379CC; padding: 2px 10px; margin: 10px">60°C ~ 70°C</span></td>
                                                <td style="background: #c3c3c3; padding: 5px 10px; font-weight: normal; border-left: 1px solid #aaa" align="center">Air Cooler Diff. Pressure</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Exh. Temp After T/C</td>
                                                <td align="left"><span style="color: black; background: #B09CD9; padding: 2px 10px; margin: 10px">4.0 ºC</span></td>
                                                <td rowspan="3" style="border-left: 1px solid #aaa; padding-left: 5px" align="left"><b style="color: #1E2054">1 milibar = 10 mmAq</b><br>
                                                    <br>
                                                    <b>Normal:</b> 10~15 milibar<br>
                                                    <b>Cleaning: </b>30 milibar</td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c6c6c6; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Temp.</td>
                                                <td align="left"><span style="color: white; background: #5851A6; padding: 2px 10px; margin: 10px">0.1 ~ 1.0 bar</span></td>
                                            </tr>
                                            <tr>
                                                <td class="tooltpStyle" style="background: #c3c3c3; padding: 5px 10px; font-weight: normal" align="left">Charge Air A/E Pressure</td>
                                                <td align="left"><span style="color: white; background: #CC3C67; padding: 2px 10px; margin: 10px">2.0 bar</span></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>
                                <div class="ae04Chart" id="ae04Chart05">
                                    <div id="ae04Chart05Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae04Chart" id="ae04Chart06">
                                    <div id="ae04Chart06Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                                <div class="ae01Chart" id="ae04Chart07" style="width: 100%">
                                    <div id="ae04Chart07Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div id="divscript">
        </div>

    </form>

    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">
        var vesselid;
        var AeNo = 1;

        var seriesData;
        var seriesData2;
        var seriesData3;
        var seriesData4;
        var seriesData5;
        var dateseries;
        var shoptestSfoc;

        var pmaxData ;
        var pumpIndexData ;
        var exhaustTempData ;
        var cfwData ;

        var cpGraphState = {};
        var cpGraphStateA = {};
        var cpGraphStateB = {};


        $(document).ready(function () {

            vesselid = <%=this.vesselid%>;
            //var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString()
            //AjxGet(SitePath + url, 'divCharts', false);
            var url = "Dashboard/DashboardTecnicalAEPerformanceHeader.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
            AjxGet(SitePath + url, 'divHeader', false);

            var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            }); 
            popupPerfCall('ae01Chart01',seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
            focGraphCall('ae01Chart01',seriesData2,dateseries);
            focGraphCall2('ae01Chart01',seriesData3,dateseries);
            
        });

        $(window).resize(function () {
            cpGraphState.resize();
            cpGraphStateA.resize();
            cpGraphStateB.resize();
            AeChartResize();
        });

        var cVal = 1;
        var wVal = 1;

        function tabActivator(num) {

        }

        //AE 1
        $('.tabs01Btn').click(function () {
            vesselid = <%=this.vesselid%>;
            var tab_id = $(this).attr('data-tab');
            $('.tabs01Btn').removeClass('tabsBtnActive');
            $('.ae01Chart').removeClass('currentAE');
            $(this).addClass('tabsBtnActive');
            var toDiv = "#" + tab_id;
            $(toDiv).addClass('currentAE');
            if (tab_id == "ae01Chart01") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall(tab_id,seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall(tab_id ,seriesData2,dateseries);
                focGraphCall2(tab_id ,seriesData3,dateseries);
            }
            if (tab_id == "ae01Chart02") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSwitchboard.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                switchBoardGraphCall(tab_id ,seriesData,dateseries);

            }

            if (tab_id == "ae01Chart03") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformancePowerBalance.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  

                cylinderGraphCall(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall2(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall3(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
            }

            if (tab_id == "ae01Chart04") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceTurboCharger.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                tcGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae01Chart05") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceCoolingWaterSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                cwsGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae01Chart06") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLubeOilSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                losGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae01Chart07") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLastOverhaul.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                overhaulGraphCall(tab_id,dateseries,seriesData,seriesData2);
            }

 
        });
        
        //AE 2
        $('.tabs02Btn').click(function() {
            var tab_id = $(this).attr('data-tab');
            $('.tabs02Btn').removeClass('tabsBtnActive');
            $('.ae02Chart').removeClass('currentAE');
            $(this).addClass('tabsBtnActive');
            var toDiv = "#" + tab_id;
            $(toDiv).addClass('currentAE');
            if (tab_id == "ae02Chart01") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall(tab_id,seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall(tab_id ,seriesData2,dateseries);
                focGraphCall2(tab_id ,seriesData3,dateseries);
            }
            if (tab_id == "ae02Chart02") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSwitchboard.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                switchBoardGraphCall(tab_id ,seriesData,dateseries);

            }

            if (tab_id == "ae02Chart03") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformancePowerBalance.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  

                cylinderGraphCall(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall2(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall3(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
            }

            if (tab_id == "ae02Chart04") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceTurboCharger.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                tcGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae02Chart05") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceCoolingWaterSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                cwsGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae02Chart06") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLubeOilSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                losGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae02Chart07") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLastOverhaul.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                overhaulGraphCall(tab_id,dateseries,seriesData,seriesData2);
            }

        });


        //AE 3
        $('.tabs03Btn').click(function() {
            var tab_id = $(this).attr('data-tab');
            $('.tabs03Btn').removeClass('tabsBtnActive');
            $('.ae03Chart').removeClass('currentAE');
            $(this).addClass('tabsBtnActive');
            var toDiv = "#" + tab_id;
            $(toDiv).addClass('currentAE');
            if (tab_id == "ae03Chart01") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall(tab_id,seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
            }
            if (tab_id == "ae03Chart02") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSwitchboard.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                switchBoardGraphCall(tab_id,seriesData,dateseries);
            }

            if (tab_id == "ae03Chart03") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformancePowerBalance.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  

                cylinderGraphCall(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall2(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall3(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
            }

            if (tab_id == "ae03Chart04") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceTurboCharger.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                tcGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae03Chart05")
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceCoolingWaterSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                cwsGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae03Chart06") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLubeOilSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                losGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae03Chart07") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLastOverhaul.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                overhaulGraphCall(tab_id,dateseries,seriesData,seriesData2);
            }

        });

        //AE 4
        $('.tabs04Btn').click(function() {
            var tab_id = $(this).attr('data-tab');
            $('.tabs04Btn').removeClass('tabsBtnActive');
            $('.ae04Chart').removeClass('currentAE');
            $(this).addClass('tabsBtnActive');
            var toDiv = "#" + tab_id;
            $(toDiv).addClass('currentAE');
            if (tab_id == "ae04Chart01") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall(tab_id,seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
            }
            if (tab_id == "ae04Chart02") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSwitchboard.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                switchBoardGraphCall(tab_id,seriesData,dateseries);
            }

            if (tab_id == "ae04Chart03") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformancePowerBalance.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  

                cylinderGraphCall(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall2(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
                cylinderGraphCall3(tab_id,dateseries,pmaxData,pumpIndexData,exhaustTempData,cfwData);
            }

            if (tab_id == "ae04Chart04") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceTurboCharger.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                tcGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae04Chart05")
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceCoolingWaterSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });  
                cwsGraphCall(tab_id,dateseries,seriesData);
            }

            if (tab_id == "ae04Chart06") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLubeOilSystem.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                losGraphCall(tab_id,dateseries,seriesData);
            }
            if (tab_id == "ae04Chart07") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceLastOverhaul.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                
                overhaulGraphCall(tab_id,dateseries,seriesData,seriesData2);
            }

        });
        
        //Left side button AE no 1,2,3,4
        $('.menuAEStyle').click(function () {
            var tab_id = $(this).attr('data-tab');
            var chartInitTab;
            $('.menuAEStyle').removeClass('activeAE');
            $('.chartab').removeClass('currentAE');

            $(this).addClass('activeAE');
            var toDiv = "#" + tab_id;
            $(toDiv).addClass('currentAE');
            AeNo = $(this).attr('id');
            var url = "Dashboard/DashboardTecnicalAEPerformanceHeader.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
            AjxGet(SitePath + url, 'divHeader', false);

            if (tab_id == "AE01ChartBlock") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall('ae01Chart01',seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall('ae01Chart01',seriesData2,dateseries);
                focGraphCall2('ae01Chart01',seriesData3,dateseries);
            }
            if (tab_id == "AE02ChartBlock") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall('ae02Chart01',seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall('ae02Chart01',seriesData2,dateseries);
                focGraphCall2('ae02Chart01',seriesData3,dateseries);
            }
            if (tab_id == "AE03ChartBlock") 
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall('ae03Chart01',seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall('ae03Chart01',seriesData2,dateseries);
                focGraphCall2('ae03Chart01',seriesData3,dateseries);
            }
            if (tab_id == "AE04ChartBlock")
            {
                var url = "Dashboard/DashboardTechnicalAuxiliaryPerformanceSFOC.aspx?vesselid="+vesselid.toString()+"&AeNo="+AeNo.toString();
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                }); 
                popupPerfCall('ae04Chart01',seriesData,seriesData4,seriesData5,dateseries,shoptestSfoc);
                focGraphCall('ae04Chart01',seriesData2,dateseries);
                focGraphCall2('ae04Chart01',seriesData3,dateseries);
            }

        });
    </script>
        </telerik:RadCodeBlock>
</body>



