<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommercialPerformanceChart.aspx.cs"
    Inherits="Dashboard_DashboardCommercialPerformanceChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<html lang="en">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="df" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/js/bootstrap.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/DashboardCommercialChart.js"></script>
        <script type="text/javascript">
            function showFn(chk) {
                document.getElementById("redDiv").style.display = (chk) ? "none" : "block";
            }
        </script>
 
        <style>
            body {
                font-family: Tahoma;
                font-size: 12px;
                margin: 0;
                padding: 0;
            }



            .chartContainer {
                box-sizing: border-box;
                width: 100%;
                height: 100%;
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
                background: #27455E;
                /* For browsers that do not support gradients */
                background: -webkit-linear-gradient(left top, #27455E, #537895);
                /* For Safari 5.1 to 6.0 */
                background: -o-linear-gradient(bottom right, #27455E, #537895);
                /* For Opera 11.1 to 12.0 */
                background: -moz-linear-gradient(bottom right, #27455E, #537895);
                /* For Firefox 3.6 to 15 */
                background: linear-gradient(to bottom right, #27455E, #537895);
                /*background: #FFC300;*/
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


            .chartsBlock {
                width: 100%;
                padding: 0px;
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
                -moz-box-shadow: 0px -2px 4px rgba(0, 0, 0, 0.2);
                -webkit-box-shadow: 0px -2px 4px rgba(0, 0, 0, 0.2);
                box-shadow: 0px -2px 4px rgba(0, 0, 0, 0.2);
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
                background: rgba(224, 223, 223, 1);
                color: #657C79;
                padding: 5px 10px;
                cursor: pointer;
                transition: 0.4s;
                border-top-left-radius: 4px;
                border-top-right-radius: 4px;
                margin-right: 2px;
                -moz-box-shadow: inset 0px -2px 2px rgba(0, 0, 0, 0.2);
                -webkit-box-shadow: inset 0px -2px 2px rgba(0, 0, 0, 0.2);
                box-shadow: inset 0px -2px 2px rgba(0, 0, 0, 0.2);
                border: none;
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
                -moz-box-shadow: none;
                -webkit-box-shadow: none;
                box-shadow: none;
            }

            .chartsDiv {
                padding: 10px;
                background: rgba(255, 255, 255, 1);
                /*height: 600px;*/
            }

            .chartDashDiv {
                padding: 10px;
                background: rgba(255, 255, 255, 1);
                height: 500px;
            }

            .mp01Chart,
            .mp02Chart,
            .mp03Chart,
            .mp04Chart {
                height: 500px;
                display: none;
            }

            #mp01Chart05,
            #mp01Chart07 {
                height: 800px;
            }

            .currentMP {
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

            .pasSumVsl {
                width: 100%;
                margin: 0 auto;
                position: relative;
            }

            .pasSumVslTitle {
                position: absolute;
                color: #5bb1bd;
                font-size: 0.6em;
                padding-right: 20px;
                top: 50%;
                right: 0;
                transform: translateY(-50%);
            }

            .pasSumVslName {
                padding: 10px 20px;
                background: #018498;
                color: white;
                font-family: 'Fjalla One', sans-serif;
                font-size: 2.8em;
                text-transform: uppercase;
            }

            .companyName {
                padding: 10px 10px;
                color: #018498;
                font-family: 'Fjalla One', sans-serif;
                font-size: 2em;
                text-transform: uppercase;
            }

            .voyNum {
                position: absolute;
                /* color: #820333; */
                color: #C94150;
                font-size: 0.6em;
                padding-right: 20px;
                top: 50%;
                right: 0;
                transform: translateY(-50%);
            }

            .voyNumVal {
                color: #C9283E;
                font-size: 1.8em;
                font-weight: 700;
            }

            .pasSumTable {
                width: 100%;
                margin: 10px auto;
                border: 1px solid #999;
                font-size: 14px;
                box-shadow: 2px 4px 8px rgba(0, 0, 0, 0.3);
            }

            .tdHeadTitle {
                padding: 10px;
                color: white;
                background: #333;
                font-size: 1.4em;
            }

            .tdHead {
                /* background: #c3c3c3; */
                background: #d4d4d4;
                padding: 10px;
                text-align: left;
                font-weight: 700;
                width: 150px;
            }

            .subHeadTitle {
                background: #c3c3c3;
                color: #333;
                font-weight: 700;
                padding: 5px 10px;
            }

            .subSideTR > td {
                padding: 10px;
                border-bottom: 1px solid #ddd;
            }

                .subSideTR > td:nth-child(2),
                .subSideTR > td:nth-child(3) {
                    border-right: 1px solid #ddd;
                }

            .lastTable > td {
                border-bottom: 1px solid #9acffb;
            }

                .lastTable > td:nth-child(1) {
                    border-right: 1px solid #9acffb;
                }

                .lastTable > td:nth-child(4),
                .lastTable > td:nth-child(5),
                .lastTable > td:nth-child(6),
                .lastTable > td:nth-child(7),
                .lastTable > td:nth-child(8) {
                    border-right: 1px solid #ddd;
                }

            .subSideTitle {
                background: #ddd;
                color: #333;
                font-weight: 700;
                padding: 5px 10px;
                text-align: left;
                width: 300px;
            }

            .tdValue {
                text-align: left;
                font-weight: 700;
                color: #27455e;
                padding: 10px;
                border-bottom: 1px solid #c3c3c3;
            }

            .gainVal {
                background: #2A6629;
                color: white;
                padding: 3px 5px;
            }

            .lossVal {
                background: #F01511;
                color: white;
                padding: 3px 5px;
            }

                .lossVal:after {
                    content: ")";
                }

                .lossVal:before {
                    content: "(";
                }

            #showDivLabel {
                position: absolute;
                top: 0;
                left: 0;
                margin-left: 30px;
                margin-top: 15px;
                width: 50px;
                z-index: 190;
            }

            #lstPassage {
                width: 300px;
            }

            #redDiv {
                background: rgba(255, 255, 255, 0.4);
                width: 100%;
                height: 100%;
                position: absolute;
                top: 0;
                left: 0;
                z-index: 90;
            }

            #chkPassage {
                position: absolute;
                top: 0;
                left: 0;
                margin-top: 17px;
                z-index: 190;
            }
            .export2XL {
            background: #018498;
            padding: 6px 9px;
            margin: 5px auto;
            border-radius: 4px;
            color: white;
            font-weight: 700;
            cursor: pointer;
            box-shadow: 0 8px 6px -6px rgba(0, 0, 0, 0.6);
        }
        .export2XL:hover {
            margin-top: 3px;
            box-shadow: 0 8px 6px -6px rgba(0, 0, 0, 0.8);
        }
        </style>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCommercialChart" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="chartContainer" style="min-width: 1020px;">
            <div class="newBGgrad">
                <div class="chartsBlock" style="background: #F3F3F3">

                    <!-- Filter strip -->
                    <div class="aeTitle" style="max-width: 100%; margin: 0 auto; margin-bottom: 10px;">
                        <div class="titleA">
                            <span style="width: 80px;">Condition </span>
                            <span style="width: 80px;">
                                <asp:DropDownList runat="server" ID="lstVslCondition" AutoPostBack="true" OnSelectedIndexChanged="lstVslCondition_SelectedIndexChanged">
                                    <asp:ListItem Text="Overall" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Ballast" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Laden" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                            <br>
                        </div>
                        <div class="titleA">
                            <span style="width: 50px;">From </span>
                            <span style="width: 120px;">
                                <asp:TextBox size="10" ID="fromDateInput" runat="server" AutoPostBack="true" Visible="true" Style="width: 110px;" OnTextChanged="fromDateInput_TextChanged"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ajxfromDateInput" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="fromDateInput" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
                            </span>
                            <br>
                        </div>
                        <div class="titleA">
                            <span style="width: 40px;">To </span>
                            <span style="width: 120px;">
                                <asp:TextBox class="fromCPDatePick" size="10" ID="ToDateInput" runat="server" AutoPostBack="true" Visible="true" Height="20px" Style="width: 110px;" OnTextChanged="ToDateInput_TextChanged"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ajxToDateInput" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="ToDateInput" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
                            </span>
                            <br>
                        </div>
                        <div class="titleA">
                            <span style="width: 80px;">Weather </span>
                            <span style="width: 80px;">
                                <asp:DropDownList runat="server" ID="lstWeather" AutoPostBack="true" OnSelectedIndexChanged="lstWeather_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Good" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Bad" Value="4"></asp:ListItem> 
                                </asp:DropDownList>
                            </span>
                            <br>
                        </div>
                        <div class="titleA">
                            <span style="width: 100px; text-align: left; padding: 0px">
                                <asp:RadioButton ID="b4" GroupName="BadweatherOption" runat="server" Text="BF4" AutoPostBack="true" Checked="true" OnCheckedChanged="b4_CheckedChanged" />
                                <asp:RadioButton ID="b5" GroupName="BadweatherOption" runat="server" Text="BF5" AutoPostBack="true" OnCheckedChanged="b5_CheckedChanged" />
                            </span>
                            <br>
                        </div>
                        <div class="titleA">
                            <span style="width: 100px;">Vessel Status </span>
                            <span style="width: 100px;">
                                <asp:DropDownList runat="server" ID="lstVslStatus" AutoPostBack="true" OnSelectedIndexChanged="lstVslStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="At Sea" Value="ATSEA"></asp:ListItem>
                                    <asp:ListItem Text="In Port" Value="INPORT"></asp:ListItem>
                                    <asp:ListItem Text="At Anchor" Value="ATANCHOR"></asp:ListItem>
                                    <asp:ListItem Text="Drifting" Value="DRIFTING"></asp:ListItem>
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                            <br>
                        </div>


                    </div>

                    <!-- sub filter fileds -->
                    <div style="background: #a2b1b0; width: 98%; margin: 10px auto 20px;">
                        <table style="font-weight: 700; font-size: 12px;position:relative;" cellpadding="10" align="center">
                            <tbody>
                                <tr>
                                    <td rowspan="2" style="min-width: 60px">
                                        <span">
                                            <asp:CheckBox ID="chkPassage" runat="server" OnCheckedChanged="chkPassage_CheckedChanged" AutoPostBack="true" />
                                            <span id="showDivLabel">Display Voyage Data</span>
                                            <div id="redDiv"></div>
                                        </span>
                                    </td>
                                    <td align="right">Vessel</td>
                                    <td>
                                        <asp:TextBox ID="lblvesselname" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right">Voyage No.</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="lstVoyageNo" AutoPostBack="true" DataTextField="FLDVOYAGENO" DataValueField="FLDVOYAGEID"
                                            OnSelectedIndexChanged="lstVoyageNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">Commenced Port</td>
                                    <td>
                                        <asp:TextBox ID="txtcommencedPort" runat="server" Style="padding-left: 5px"></asp:TextBox>
                                    </td>
                                    <td align="right">COSP</td>
                                    <td>
                                        <asp:TextBox ID="txtCospDate" runat="server" Style="padding-left: 5px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Passage</td>
                                    <td colspan="3">
                                        <asp:DropDownList runat="server" ID="lstPassage" AutoPostBack="true" DataTextField="FLDARRIVALPORTNAME" DataValueField="FLDARRIVALREPORTID"
                                            OnSelectedIndexChanged="lstPassage_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>

                                    <td align="right">Completed Port</td>
                                    <td>
                                        <asp:TextBox ID="txtCompletedPort" runat="server" Style="padding-left: 5px"></asp:TextBox>
                                    </td>

                                    <td align="right">EOSP</td>
                                    <td>
                                        <asp:TextBox ID="txtEospDate" runat="server" Style="padding-left: 5px"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!-- tabs -->
                    <div id=" MP01ChartBlock" class="chartab currentMP">

                        <div>
                            <div id="tabs01Nav" runat="server">
                                <input type="button" data-tab="mp01Chart01" class="tabs01Btn tabsBtnActive" value="Charter Party" />
                                <input type="button" data-tab="mp01Chart02" class="tabs01Btn" value="Fuel Consumption" />
                                <input type="button" data-tab="mp01Chart03" class="tabs01Btn" value="Vessel Fuel Efficiency" />
                                <input type="button" data-tab="mp01Chart04" class="tabs01Btn" value="Auxiliary Boiler Fuel" />
                                <input type="button" data-tab="mp01Chart05" class="tabs01Btn" value="Passage Overview" />
                                <input type="button" data-tab="mp01Chart06" class="tabs01Btn" value="Passage Summary" />
                                <input type="button" data-tab="mp01Chart07" class="tabs01Btn" value="Voyage Summary" />
                            </div>
                        </div>

                        <!-- charts -->
                        <div class="chartsDiv nizhal">

                            <!-- Charter Party Tab -->
                            <div class="mp01Chart currentMP" id="mp01Chart01">
                                <div id="mp01Chart01Graph" style="height: 600px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1526453946634">
                                    <div style="position: relative; overflow: hidden; width: 1302px; height: 599px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                                        <canvas style="position: absolute; left: 0px; top: 0px; width: 1302px; height: 599px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1302" height="599" data-zr-dom-id="zr_0"></canvas>
                                    </div>

                                </div>

                            </div>

                            <!-- Fuel Consumption tab -->
                            <div class="mp01Chart" id="mp01Chart02" style="min-height: 792px;">
                                <div id="mp01Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>

                                <div id="divCumulative"></div>
                            </div>


                            <!-- Average Fuel Consumption Per Mile -->

                            <div class="mp01Chart" id="mp01Chart03" style="height: 840px">
                                <div id="mp01Chart03Graph" style="height: 400px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>

                            </div>

                            <!-- Auxiliary Boiler tab -->
                            <div class="mp01Chart" id="mp01Chart04">
                                <div id="mp01Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
                            </div>
                            <!-- Voyage overview -->
                            <div class="mp01Chart" id="mp01Chart05">
                                <div id="mp01Chart05Graph" style="height: 300px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
                                <div id="mp01Chart05aGraph" style="height: 300px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
                                <div id="mp01Chart05bGraph" style="height: 300px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
                            </div>
                            <div class="mp01Chart" id="mp01Chart06" style="height: 1390px;" runat="server">
                                <div style="text-align:right;width:90%;line-height:44px">
                                    <asp:Button ID="lnkPassagePdf" runat="server" Text="Export" OnClick="lnkPassageExcel" CssClass="export2XL" ToolTip="Export to Excel"></asp:Button>
                                </div>
                                <div id="mp01Chart06Graph" style="width: 80%; margin: 0 auto;" runat="server">
                                </div>
                            </div>
                            <div class="mp01Chart" id="mp01Chart07" style="height: 1390px;" runat="server">
                                <div style="text-align:right;width:90%;line-height:44px">
                                    <asp:Button ID="lnkVoyagePdf" runat="server" Text="Export" OnClick="lnkVoyageExcel" CssClass="export2XL" ToolTip="Export to Excel"></asp:Button>
                                </div>
                                <div id="mp01Chart07Graph" style="width: 80%; margin: 0 auto;" runat="server">
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
    <script>
        var tabNum = 1;
        var cpGraphState1 = {};
        var cpGraphState2 = {};
        var cpGraphState3 = {};
        var cpGraphState4 = {};
        var cpGraphState5 = {};
        var cpGraphState5a = {};
        var cpGraphState5b = {};

        var seriesDataOrg = [];
        var seriesNameOrg = [];
        var ChartData;
        var ChartData1;
        var ChartData2;
        var dateseries;

        $(document).ready(function () {
            var url = "Dashboard/DashboardCommercialCharterPartyPerformance.aspx"
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart01('mp01Chart01', ChartData, dateseries);
            var trigPointVal;
            // setBFVal();
        });



        $('.tabs01Btn').click(function () {
            var tab_id = $(this).attr('data-tab');

            $('.tabs01Btn').removeClass('tabsBtnActive');

            $('.mp01Chart').removeClass('currentMP');

            $(this).addClass('tabsBtnActive');

            var toDiv = "#" + tab_id;

            $(toDiv).addClass('currentMP');

            if (tab_id == "mp01Chart01") {
                var url = "Dashboard/DashboardCommercialCharterPartyPerformance.aspx"
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });
                mpchart01(tab_id, ChartData, dateseries);
            }

            if (tab_id == "mp01Chart02") {
                var url = "Dashboard/DashboardCommercialFuelConsumption.aspx"
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });
                mpchart02(tab_id, ChartData, dateseries);
            }

            if (tab_id == "mp01Chart03") {
                var url = "Dashboard/DashboadCommercialFuelEfficiency.aspx"
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });

                mpchart03(tab_id, ChartData, dateseries);
            }

            if (tab_id == "mp01Chart04") {
                var url = "Dashboard/DashboardCommercialAuxiliaryBoiler.aspx"
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });

                mpchart04(tab_id, ChartData, dateseries);
            }

            if (tab_id == "mp01Chart05") {

                var url = "Dashboard/DashboardCommercialVoyageOverview.aspx"
                AjxGet(SitePath + url, 'divscript', false);
                $("#divscript").find("script").each(function () {
                    eval($(this).text());
                });

                mpchart05(tab_id, ChartData, dateseries)
                mpchart05a(tab_id + 'a', ChartData, dateseries);
                mpchart05b(tab_id + 'b', ChartData, dateseries);
            }

            tabNum = Number(tab_id.substr(-2, 2));

            resetGraph(tabNum);
        });

        var voyageParam = <%=this.voyageData%>;
 
        var cpBF = <%=this.BfValue%>;


        $(window).resize(function () {
            //cpGraphState.resize();
            resetGraph(tabNum);
        });
   

    </script>
        </telerik:RadCodeBlock>
    <div id="ui-datepicker-div" class="ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all"></div>
</body>
</html>

