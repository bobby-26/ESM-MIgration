<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="DashboardVesselPositionAlertFineFilter.aspx.cs"
    Inherits="DashboardVesselPositionAlertFineFilter" %>

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
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/DashboardExceptionAlert.js"></script>
 
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
      display:flex;
      text-align: center;
      align-items: center;
      justify-content: center;
      flex-direction: row;
      flex-wrap: wrap;
      flex-flow: row wrap;
      align-content: flex-end;

    }
 /**/
    .aeTitle>div {
      flex:auto;
      text-align: left;
     /* width:180px;*/
    }
    .aeTitle>div>span {
      margin-bottom: 3px;
      padding: 3px 5px;
    }

    .aeTitle>div>span:nth-child(1) {
      color: white;
      line-height: 22px;
      background: rgba(60, 60, 60, 1);
      border-top-left-radius: 8px;
      border-bottom-left-radius: 8px;
      float: left;
      width: 100px;
      text-align: right;
      font-size: 11px !important;
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
      font-size: 11px !important;
    }
    .aeTitle>div>span:nth-child(2) {
     
      line-height: 20px;
      background: white;
      border-top-right-radius: 8px;
      border-bottom-right-radius: 8px;
      float: left;
     font-size: 11px !important;
     border:0px;
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
    .LightOrange { background: #FFDAB9; color: #FFDAB9 }
    
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
            <div class="Test">
                 <span style="width: 50px;">Vessel </span>
                 <span style="width: 100px;">
                     <asp:TextBox  size="10" ID="txtVessel" runat="server"  Enabled="false"  Visible="true" Height="20px" Style="width: 90px;  border:none;"></asp:TextBox>
                 </span>
            </div>
            <div >
               <span style="width: 65px;">Condition </span>
                 <span style="width: 80px;">
                     <asp:DropDownList runat="server" ID="lstVslCondition" AutoPostBack="true" OnSelectedIndexChanged="lstVslCondition_SelectedIndexChanged">
                        <asp:ListItem Text="Overall" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Ballast" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Laden" Value="3"></asp:ListItem>
                     </asp:DropDownList>
                </span>
            </div>
          <div>
            <span style="width: 40px;">From </span>
            <span style="width: 75px;">
              <asp:TextBox  class="fromCPDatePick" size="10" ID="fromDate" runat="server" AutoPostBack="true" Visible="true" Height="20px" Style="width: 68px;  border:none; border-top-right-radius: 4px;border-bottom-right-radius: 4px;" OnTextChanged="fromDateInput_TextChanged"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="ajxfromDateInput" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="fromDate" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
            </span>
          </div>
          <div>
            <span style="width: 25px;">To </span>
            <span style="width: 75px;">
              <asp:TextBox class="fromCPDatePick" size="10" ID="ToDateInput" AutoPostBack="true" runat="server"  Visible="true" Height="20px" Style="width: 68px;  border:none; border-top-right-radius: 4px;border-bottom-right-radius: 4px;" OnTextChanged="ToDateInput_TextChanged"></asp:TextBox>
               <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="ToDateInput" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>
            </span>
          </div>
            <div>
                            <span style="width: 60px;">Weather </span>
                            <span style="width: 65px;">
                                <asp:DropDownList runat="server" ID="lstWeather" AutoPostBack="true" OnSelectedIndexChanged="lstWeather_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Good" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Bad" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
             </div>
            <div >
                    <span style="width: 100px; text-align: left; padding: 0px" class="radio">
                    <asp:RadioButton ID="b4" GroupName="BadweatherOption" runat="server" Text="BF4" AutoPostBack="true" Checked="true" OnCheckedChanged="b4_CheckedChanged" />
                    <asp:RadioButton ID="b5" GroupName="BadweatherOption" runat="server" Text="BF5" AutoPostBack="true" OnCheckedChanged="b5_CheckedChanged" />
                 </span>
                 <br>
            </div>
          <div>
            <span style="width: 50px;">Status </span>
             <span style="width: 95px;">
                                <asp:DropDownList runat="server" ID="lstVslStatus" AutoPostBack="true" OnSelectedIndexChanged="lstVslStatus_SelectedIndexChanged" CssClass="ddlnostyle">
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
          <div class="chartsDiv nizhal">
            <div class="mp01Chart currentMP" id="mp01Chart01" style="min-height: 500px;">
              <div id="mp01Chart01Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart02" style="min-height: 500px;">
                <div id="mp01Chart02Graph" style="height: 400px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>
            </div> 
            <div class="mp01Chart" id="mp01Chart03" style="min-height: 500px;">
                <div id="mp01Chart03aGraph" style="height: 400px; width: 100%; margin: 0 auto; padding-top: 15px;"></div>
            </div>   
              <div class="mp01Chart" id="mp01Chart04">
              <div id="mp01Chart04Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>         
            <div class="mp01Chart" id="mp01Chart05">
              <div id="mp01Chart05Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart06">
              <div id="mp01Chart06Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart07">
              <div id="mp01Chart07Graph" style="height: 380px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
              <div id="mp01Chart07aGraph" style="height: 380px; width: 100%; margin: 20px auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart08">
              <div id="mp01Chart08Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart09" style="height: 600px">
              <div id="mp01Chart09Graph" style="height: 600px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
            </div>
            <div class="mp01Chart" id="mp01Chart10">
              <div id="mp01Chart10Graph" style="height: 400px; width: 100%; margin: 0 auto; border-bottom: 1px solid #999;"></div>
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
<script type="text/javascript">
    var ChartData;
    var ChartData1;
    var dateseries;
    $(document).ready(function () {

        var path = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&')
        var measure;
        for (var i = 0; i < path.length; i++) {
            var urlparam = path[i].split('=');
            if (urlparam[0] == "measurename") {
                measure = replaceurl(urlparam[1]);
            }
        }
        //alert(measure);
        var tab_id;
        if (measure == "FLDMELOAUTOBACKWASHFILTERCOUNTER" || measure == "FLDMEFOAUTOBACKWASHFILTERCOUNTER") {
            $('.mp01Chart').removeClass('currentMP');
            $('#mp01Chart07').addClass('currentMP');

            var url = "Dashboard/DashboardMachinaryFineFilterAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart07("mp01Chart07", ChartData, dateseries);
            mpchart07a("mp01Chart07a", ChartData1, dateseries);

            tab_id = "mp01Chart07";

        }
        else if (measure == "FLDAUXENGINEHROFOPERATION" || measure == "FLDAECAPACITY" || measure == "FLDAEUTILIZATION") {
           $('.mp01Chart').removeClass('currentMP');
            $('#mp01Chart01').addClass('currentMP');

            var url = "Dashboard/dashboardvesselpositionalertauxillaryloaddetails.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });

            mpchart01("mp01Chart01", ChartData1, dateseries);
            //mpchart02("mp01Chart01a", ChartData1, dateseries);

            tab_id = "mp01Chart01";
        }
       else if (measure == "FLDMAXEXHTEMP" || measure == "FLDMINEXHTEMP")
        {
            $('.mp01Chart').removeClass('currentMP');
            $('#mp01Chart03').addClass('currentMP');

            var url = "Dashboard/DashboardMachinaryTemperatureAnalysis.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            //mpchart03("mp01Chart03", ChartData, dateseries);
            mpchart03a("mp01Chart03a", ChartData1, dateseries);


            var tab_id = "mp01Chart03";
       }
       else if (measure == "FLDEXHGASTEMPTCINBOARDBEFORE" || measure == "FLDEXHGASTEMPTCOUTBOARDBEFORE") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart10').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionTurbochargerAlert.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });

           mpchart10("mp01Chart10", ChartData, dateseries);


           var tab_id = "mp01Chart10";
       }
       else if (measure == "FLDAEFOC") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart11').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionAEFOCAlert.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });

           mpchart11("mp01Chart11", ChartData, dateseries);


           var tab_id = "mp01Chart11";
       }
       else if (measure == "FLDMEFOC")
       {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart02').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionAlertsMonthly.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });
           mpchart02("mp01Chart02", ChartData, dateseries);

           var tab_id = "mp01Chart02";
       }
       else if (measure == "FLDMEPRORATEDFOC") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart05').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionAlertsMonthly.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });
           mpchart05("mp01Chart05", ChartData, dateseries);

           var tab_id = "mp01Chart05";
       }
       else if (measure == "FLDBOILERFOC") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart06').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionAlertsMonthly.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });
           mpchart06("mp01Chart06", ChartData, dateseries);

           var tab_id = "mp01Chart06";
       }
       else if (measure == "FLDSLIP") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart08').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionAlertsMonthly.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });
           mpchart08("mp01Chart08", ChartData, dateseries);

           var tab_id = "mp01Chart08";
       }
       else if (measure == "FLDSPEED") {
           $('.mp01Chart').removeClass('currentMP');
           $('#mp01Chart09').addClass('currentMP');

           var url = "Dashboard/DashboardVesselPositionSpeedAlert.aspx";
           AjxGet(SitePath + url, 'divscript', false);
           $("#divscript").find("script").each(function () {
               eval($(this).text());
           });
           mpchart09("mp01Chart09", ChartData, dateseries);

           var tab_id = "mp01Chart09";
       }
       else {
            $('.mp01Chart').removeClass('currentMP');
            $('#mp01Chart04').addClass('currentMP');

            var url = "Dashboard/DashboardVesselPositionAlertsMonthly.aspx";
            AjxGet(SitePath + url, 'divscript', false);
            $("#divscript").find("script").each(function () {
                eval($(this).text());
            });
            mpchart04("mp01Chart04", ChartData, dateseries);

            var tab_id = "mp01Chart04";
        }
        
        tabNum = Number(tab_id.substr(-2, 2));
        resetGraph(tabNum);
    });

    function replaceurl(str)
    {
        str = str.replace(/\%20/g, " ");
        str = str.replace(/\+/g, " ");
        str = str.replace(/\%2f/g, "/");
        str = str.replace(/\%28/g, "(");
        str = str.replace(/\%29/g, ")");
        return str;
    }
    var cpBF = <%=this.BfValue%>;
    $(window).resize(function () {
        resetGraph(tabNum);
    });

</script>
        </telerik:RadCodeBlock>
 </body>   
</html>

