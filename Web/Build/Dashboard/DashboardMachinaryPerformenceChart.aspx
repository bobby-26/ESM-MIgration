<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="DashboardMachinaryPerformenceChart.aspx.cs"
    Inherits="DashboardMachinaryPerformenceChart" %>

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
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/MachineryParametersChart.js"></script>
 
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

    .menuAEStyle>em {
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

    .activeAE>span {
      color: #FF5733;
    }

    .activeAE>em {
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
      flex-wrap: nowrap;
    }

    .aeTitle>div {
      flex: 1;
      text-align: left;
     /* width:180px;*/
    }

    .aeTitle>div>span {
      margin-bottom: 3px;
      padding: 3px 5px;
    }

    .aeTitle>div>span:nth-child(1),
    .aeTitle>div>span:nth-child(4) {
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

    .radio{
        color: white;
      line-height: 22px;
      background: rgba(60, 60, 60, 1);
      border-top-left-radius: 8px;
      border-bottom-left-radius: 8px;
       border-top-right-radius: 8px;
      border-bottom-right-radius: 8px;
      float: left;
      width: 100px;
      text-align: right;
      font-size: 12px !important;
    }
    .aeTitle>div>span:nth-child(2),
    .aeTitle>div>span:nth-child(5) {
      color: black;
      line-height: 20px;
      background: white;
      border-top-right-radius: 8px;
      border-bottom-right-radius: 8px;
      width: 200px;
      float: left;
      border: 1px solid #bbb;
    }

    .aeTitle>div>br {
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
        margin-top: 15px
      }
      to {
        margin-top: 5;
      }
    }

    /* Standard syntax */

    @keyframes blockAnim {
      from {
        margin-top: 15px
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
    #tabs04Nav,
      {
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

    .mtboTitle>td,
    .ambiTitle>td {
      background: #2F4E57;
      color: #fff;
      border-bottom: 1px solid #bbb;
      font-weight: 200;
    }

    .ambiTitle>span {
      color: #416B78;
    }

    .mtboRow>td:nth-child(2) {
      font-weight: 700;
      color: #FF55A3;
    }

    .mtboRow>td:nth-child(3) {
      color: #855CCC;
    }

    .ambiRow>td:nth-child(2) {
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
    </div>
</telerik:RadCodeBlock></head>
<body>
     <form id="frmCommercialChart" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
  <div class="chartContainer">
    <div class="newBGgrad">

      <div class="chartsBlock" style="background:#F3F3F3">
        <div class="aeTitle" style="max-width: 100%; margin: 0 auto; margin-bottom: 10px;">
            <div class="titleA">
                 <span style="width: 60px;">Vessel </span>
                 <span style="width: 100px;">
                     <asp:TextBox  size="10" ID="txtVessel" runat="server"  Enabled="false"  Visible="true" Height="20px" Style="width: 90px;"></asp:TextBox>
                 </span>
            </div>
            <div class="titleA">
               <span style="width: 80px;">Condition </span>
                 <span style="width: 80px;">
                     <asp:DropDownList runat="server" ID="lstVslCondition" AutoPostBack="true" OnSelectedIndexChanged="lstVslCondition_SelectedIndexChanged">
                        <asp:ListItem Text="Overall" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Ballast" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Laden" Value="3"></asp:ListItem>
                     </asp:DropDownList>
                </span>
            </div>
          <div class="titleA">
            <span style="width: 50px;">From </span>
            <span style="width: 100px;">
              <asp:TextBox  class="fromCPDatePick" size="10" ID="fromDate" runat="server" AutoPostBack="true" Visible="true" Height="20px" Style="width: 90px;" OnTextChanged="fromDateInput_TextChanged"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="ajxfromDateInput" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="fromDate" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
            </span>
          </div>
          <div class="titleA">
            <span style="width: 40px;">To </span>
            <span style="width: 100px;">
              <asp:TextBox class="fromCPDatePick" size="10" ID="ToDateInput" AutoPostBack="true" runat="server"  Visible="true" Height="20px" Style="width: 90px;" OnTextChanged="ToDateInput_TextChanged"></asp:TextBox>
               <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="ToDateInput" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
            </span>
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
             </div>
            <div class="titleA" >
                    <span style="width: 100px; text-align: left; padding: 0px" class="radio">
                    <asp:RadioButton ID="b4" GroupName="BadweatherOption" runat="server" Text="BF4" AutoPostBack="true" Checked="true" OnCheckedChanged="b4_CheckedChanged" />
                    <asp:RadioButton ID="b5" GroupName="BadweatherOption" runat="server" Text="BF5" AutoPostBack="true" OnCheckedChanged="b5_CheckedChanged" />
                 </span>
                 <br>
            </div>
          <div class="titleA">
            <span style="width: 50px;">Status </span>
             <span style="width: 100px;">
                                <asp:DropDownList runat="server" ID="lstVslStatus" AutoPostBack="true" OnSelectedIndexChanged="lstVslStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="At Sea" Value="ATSEA"></asp:ListItem>
                                    <asp:ListItem Text="In Port" Value="INPORT"></asp:ListItem>
                                    <asp:ListItem Text="At Anchor" Value="ATANCHOR"></asp:ListItem>
                                    <asp:ListItem Text="Drifting" Value="DRIFTING"></asp:ListItem>
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </span>
          </div>
        </div>

        <div id=" MP01ChartBlock" class="chartab currentMP">

          <div>
            <div id="tabs01Nav" runat="server">
              <input type="button" data-tab="mp01Chart01" class="tabs01Btn tabsBtnActive" value="Main Engine"/>
              <input type="button" data-tab="mp01Chart03" class="tabs01Btn" value="Temperature"/>
              <input type="button" data-tab="mp01Chart04" class="tabs01Btn" value="Pressure"/>
              <input type="button" data-tab="mp01Chart09" class="tabs01Btn" value="Lub. Oil"/>
              <input type="button" data-tab="mp01Chart06" class="tabs01Btn" value="Purifier"/>
              <input type="button" data-tab="mp01Chart07" class="tabs01Btn" value="Fine Filters"/>
              <input type="button" data-tab="mp01Chart08" class="tabs01Btn" value="FWG"/>
              <input type="button" data-tab="mp01Chart10" class="tabs01Btn" value="Bilge & Sludge">
              <input type="button" data-tab="mp01Chart05" class="tabs01Btn" value="Electrical Plant"/>
            </div>
          </div>

          <div class="chartsDiv nizhal">
            <div class="mp01Chart currentMP" id="mp01Chart01" style="min-height: 820px;">
              <div id="mp01Chart01Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <!-- </div>
						<div class="mp01Chart" id="mp01Chart02"> -->
              <!-- <div id="mp01Chart02Graph" style="height: 510px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div> -->
              <div id="mp01Chart01aGraph" style="height: 400px; width: 100%; margin: 20px auto 0; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart03" style="min-height: 792px;">
              <div id="mp01Chart03Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <div id="mp01Chart03aGraph" style="height: 400px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart04">
              <div id="mp01Chart04Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart05">
              <div id="mp01Chart05Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <!-- limits for insulation resistance  -->
              <div style="margin-top: 10px; padding: 5px 10px; background: #27455e; color: white; border-radius: 10px; width:
						 370px;display: -webkit-flex; display: -ms-flexbox; display: flex; align-items: center; box-sizing: border-box;">
                Insulation Resistance Limit
                <form id="resLimitForm" style="margin: 0 10px; padding: 3px 15px; background:white; color: #27455e; border-radius:
						 10px; box-sizing: border-box; display: table-cell; vertical-align: middle;">
                  <span style="margin: 0 10px; padding: 3px 15px; background:white; color: #27455e; border-radius:
						 10px; box-sizing: border-box; display: table-cell; vertical-align: middle;"">
                    <input type="radio" name="resLimit" value="0.5" style="margin: 0; vertical-align: middle" onclick="setResLimit(0.5)">0.5&nbsp;&nbsp;
                    <input type="radio" name="resLimit" value="1.0" style="margin: 0; vertical-align: middle" checked onclick="setResLimit(1.0)">1.0&nbsp;&nbsp;
                    <input type="radio" name="resLimit" value="1.5" style="margin: 0; vertical-align: middle" onclick="setResLimit(1.5)">1.5</span>
                </form> M&ohm;
              </div>
              <div id="mp01Chart05aGraph" style="height: 350px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart06">
              <div id="mp01Chart06Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart07">
              <div id="mp01Chart07Graph" style="height: 380px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <div id="mp01Chart07aGraph" style="height: 380px; width: 100%; margin: 20px auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart08">
              <div id="mp01Chart08Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart09" style="height: 840px">
              <div style="padding: 5px 10px; background: #27455e; color: white; border-radius: 10px; width: 475px;display: -webkit-flex;
						 display: -ms-flexbox; display: flex; align-items: center; box-sizing: border-box;">
                Recommended Rate
                <form id="trigPointForm" style="margin: 0 10px; padding: 3px 15px; background:white; color: #27455e; border-radius:
						 10px; box-sizing: border-box; display: table-cell; vertical-align: middle;">
                  <span>
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="0.6"  onclick="setTrigPoint(0.6)">0.6&nbsp;&nbsp;
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="0.7"  onclick="setTrigPoint(0.7)">0.7&nbsp;&nbsp;
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="0.8"  onclick="setTrigPoint(0.8)">0.8&nbsp;&nbsp;
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="0.9" onclick="setTrigPoint(0.9)"> 0.9&nbsp;&nbsp;
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="1.0" checked onclick="setTrigPoint(1.0)">1.0&nbsp;&nbsp;
                    <input type="radio" name="trigPoint" style="margin: 0; vertical-align: middle" value="1.1" onclick="setTrigPoint(1.1)">1.1</span>
                </form> g/BHp Hr
              </div>
              <div id="mp01Chart09Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <div id="mp01Chart09aGraph" style="height: 400px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>

            </div>
            <div class="mp01Chart" id="mp01Chart10">
              <div id="mp01Chart10Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart11">
              <div id="mp01Chart11Graph" style="height: 500px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>

          </div>

        </div>

      </div>

    </div>
  </div>
         <div id="divscript"></div>
</form>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script>
    var ChartData;
    var ChartData1;
    var dateseries;

    $(document).ready(function () {

        var url = "Dashboard/DashboardMachinaryMainEngineAnalysis.aspx";
        AjxGet(SitePath + url, 'divscript', false);
        $("#divscript").find("script").each(function () {
            eval($(this).text());
        });

        mpchart01("mp01Chart01", ChartData, dateseries);
        mpchart02("mp01Chart01a", ChartData1, dateseries);

        var tab_id = "mp01Chart01";
        tabNum = Number(tab_id.substr(-2, 2));
        resetGraph(tabNum);
        setTrigPoint(0.8)


    });

    let resLimitVal = 1;

    function setResLimit(reval) {
        resLimitVal = reval;
        mpchart05a("mp01Chart05a",ChartData1, dateseries);
    }
    let trigPointVal = 1;
    function setTrigPoint(trgval) {
        trigPointVal = trgval;
        mpchart09("mp01Chart09", ChartData, dateseries);
        mpchart09a("mp01Chart09a", ChartData1, dateseries);

    }

    $('.tabs01Btn').click(function () {
        var tab_id = $(this).attr('data-tab');
        $('.tabs01Btn').removeClass('tabsBtnActive');
        $('.mp01Chart').removeClass('currentMP');
        $(this).addClass('tabsBtnActive');
        var toDiv = "#" + tab_id;
        $(toDiv).addClass('currentMP');

        if (tab_id == "mp01Chart01") {

            var url = "Dashboard/DashboardMachinaryMainEngineAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });

            mpchart01("mp01Chart01", ChartData, dateseries);
            mpchart02("mp01Chart01a", ChartData1, dateseries);
        }
        // if (tab_id == "mp01Chart02") mpchart02(tab_id);
        if (tab_id == "mp01Chart03") {
            var url = "Dashboard/DashboardMachinaryTemperatureAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart03("mp01Chart03", ChartData, dateseries);
            mpchart03a("mp01Chart03a", ChartData1, dateseries);
        }
        if (tab_id == "mp01Chart04") {
            var url = "Dashboard/DashboardMachinaryPresureAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart04("mp01Chart04", ChartData, dateseries);
        }
        if (tab_id == "mp01Chart05") {
            var url = "Dashboard/DashboardMachinaryEarthFaultAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart05("mp01Chart05", ChartData, dateseries);
            mpchart05a("mp01Chart05a", ChartData1, dateseries);
        }
        if (tab_id == "mp01Chart06") {
            var url = "Dashboard/DashboardMachinaryPurifierAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart06("mp01Chart06", ChartData, dateseries);
        }
        if (tab_id == "mp01Chart07") {
            var url = "Dashboard/DashboardMachinaryFineFilterAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart07("mp01Chart07", ChartData, dateseries);
            mpchart07a("mp01Chart07a", ChartData1, dateseries);
        }
        if (tab_id == "mp01Chart08") {

            var url = "Dashboard/DashboardMachinaryFreshWaterAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });

            mpchart08("mp01Chart08", ChartData, dateseries);
        }
        if (tab_id == "mp01Chart09") {
            var url = "Dashboard/DashboardMachinaryLubOilAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });

            mpchart09("mp01Chart09", ChartData, dateseries);
            mpchart09a("mp01Chart09a", ChartData1, dateseries);
        }
        if (tab_id == "mp01Chart10") {
            var url = "Dashboard/DashboardMachinaryBilgeSludgeWaterAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart10("mp01Chart10", ChartData, dateseries);
        }
        if (tab_id == "mp01Chart11") mpchart11(tab_id);
        tabNum = Number(tab_id.substr(-2, 2));
        resetGraph(tabNum);
    });

    $(window).resize(function () {
        resetGraph(tabNum);
    });

</script>
        </telerik:RadCodeBlock>
 </body>   
</html>

