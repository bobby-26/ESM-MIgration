<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoardVoyageOverview.aspx.cs" 
    Inherits="Dashboard_DashBoardVoyageOverview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" /> 
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script  type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/bootstrap/js/bootstrap.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart_voyageoverview.js"></script>
        

    </div>
</telerik:RadCodeBlock></head>
<body style="background: rgba(230,245,254,1)">
<form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top;text-align:left; font-size:14px;background:#0C8AD2;color:white">
          <asp:Literal ID="lbltitle" runat="server" Text="Voyage Overview"></asp:Literal>
        </div>
    </div>
  <div style="height: 500px; width: 80%; margin: 0 auto; padding: 10px; background: white;">
    <div id="graphFilterDiv" style="padding: 5px; background: #eee; maring-bottom: 5px; font-weight: 700; color: #777;">
      <table width="100%">
         <td align="center">Condition:
            <asp:DropDownList runat="server" ID="conditionSelect" CssClass="CPDatePicker" OnSelectedIndexChanged="conditionSelect_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="Combined" Value="1"></asp:ListItem>
                <asp:ListItem Text="Ballast" Value="2"></asp:ListItem>
                <asp:ListItem Text="Laden" Value="3"></asp:ListItem>
            </asp:DropDownList>
        </td>
          <td align="center">
              Weather:
              <asp:DropDownList runat="server" ID="weatherSelect" CssClass="CPDatePicker" OnSelectedIndexChanged ="weatherSelect_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="A. All Weather" Value="1"></asp:ListItem>
                <asp:ListItem Text="B. Bad Weather" Value="4"></asp:ListItem>
                <asp:ListItem Text="C. Calm Weather" Value="5"></asp:ListItem>
            </asp:DropDownList>
          </td>
        <td>
          <asp:TextBox class="fromCPDatePick" size="10" id="fromDateInput" runat="server" OnTextChanged="fromDateInput_TextChanged1" AutoPostBack="true" Visible="false"></asp:TextBox>
           <ajaxToolkit:CalendarExtender ID="ajxfromDateInput" runat="server" Format="dd/MMM/yyyy"
               Enabled="True" TargetControlID="fromDateInput" PopupPosition="TopRight">
           </ajaxToolkit:CalendarExtender>
        </td>
        <td>
          <asp:TextBox class="fromCPDatePick" size="10" id="toDateInput" runat="server" OnTextChanged="toDateInput_TextChanged" AutoPostBack="true" Visible="false"></asp:TextBox>
           <ajaxToolkit:CalendarExtender ID="ajxtoDateInput" runat="server" Format="dd/MMM/yyyy"
               Enabled="True" TargetControlID="toDateInput" PopupPosition="TopRight">
           </ajaxToolkit:CalendarExtender>
        </td>
      </table>
    </div>

<%--    <div id="cpGraph" style="height: 100%; width: 100%; margin: 0 auto;"></div>--%>
    <div style="background: white"><div id="voyGraphA" style="box-sizing: border-box; height: 250px; width: 100%; margin: 0px auto; border: 1px solid grey; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648848"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 61px; top: 61px; width: auto; height: auto;"><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td class="tooltpStyle" width="125" align="center">Laden</td><td align="center"><span class="greenSpeed"> Bf-3</span></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td><span class="tlTip style2">•</span>SOG</td><td> 15.1Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>LOG</td><td>15.2Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>CP Speed</td><td>15Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="redStatus">0.67</span></td><td><span class="redStatus">0.1kn</span></td></tr><tr><td><span class="tlTip style3">•</span>Trim</td><td>0.5m</td><td></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkGreen">•</span>RPM</td><td>98</td></tr><tr><td><span class="tlTip steelBlue">•</span>Load</td><td><span class="greenSpeed">79%</span></td></tr><tr><td><span class="tlTip style8">•</span>SLIP</td><td>5%</td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkMagenda">•</span>FOC</td><td> 19.5 t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>DOC Rate</td><td> 20 t</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">2.50%</span></td><td><span class="greenStatus">0.5 t</span></td></tr></tbody></table></div></div></div>
    <div style="background: white"><div id="voyGraphB" style="height: 250px; width: 100%; margin: 20px auto 0px; border: 1px solid grey; box-sizing: border-box; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648847"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div style="position: absolute; display: none; border-style: solid; white-space: nowrap; z-index: 9999999; transition: left 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s, top 0.4s cubic-bezier(0.23, 1, 0.32, 1) 0s; background-color: rgba(245, 245, 245, 0.9); border-width: 2px; border-color: rgb(153, 153, 153); border-radius: 4px; color: rgb(0, 0, 0); font: normal normal normal normal 14px/21px Microsoft YaHei; padding: 5px; left: 61px; top: -159px; width: auto; height: auto;"><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td class="tooltpStyle" width="125" align="center">Laden</td><td align="center"><span class="redSpeed"> Bf-6</span></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td><span class="tlTip style2">•</span>SOG</td><td> 6Kn</td><td></td></tr><tr><td><span class="tlTip style7">•</span>LOG</td><td>6Kn</td><td></td></tr><tr><td><span class="tlTip style1">•</span>CP Speed</td><td>15Kn</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">60.00</span></td><td><span class="greenStatus">9kn</span></td></tr><tr><td><span class="tlTip style3">•</span>Trim</td><td>0.5m</td><td></td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkGreen">•</span>RPM</td><td>0</td></tr><tr><td><span class="tlTip steelBlue">•</span>Load</td><td><span class="greenSpeed">0%</span></td></tr><tr><td><span class="tlTip style8">•</span>SLIP</td><td>0%</td></tr></tbody></table><table class="tooltipTable" style="margin-bottom: 20px;"><tbody><tr><td width="125"><span class="tlTip darkMagenda">•</span>FOC</td><td> 0 t</td><td></td></tr><tr><td><span class="tlTip oceanGreen">•</span>DOC Rate</td><td> 20 t</td><td></td></tr><tr><td class="tooltpStyle"><span class="pad20">Deviation</span></td><td><span class="greenStatus">100.00%</span></td><td><span class="greenStatus">20 t</span></td></tr></tbody></table></div></div></div>
    <div style="background: white"><div id="voyGraphC" style="height: 250px; width: 100%; margin: 20px auto 0px; border: 1px solid grey; box-sizing: border-box; -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1516355648846"><div style="position: relative; overflow: hidden; width: 1064px; height: 248px; padding: 0px; margin: 0px; border-width: 0px;"><canvas style="position: absolute; left: 0px; top: 0px; width: 1064px; height: 248px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1064" height="248" data-zr-dom-id="zr_0"></canvas></div><div></div></div></div>
  </div>
</form>
</body>
</html>

<script type="text/javascript">

    $(document).ready(function() {
        var seriesData = <%=this.seriesdata%>;
        var dateRange = <%=this.dateList%>;
        VoyageOverViewChart(seriesData,dateRange);
    });

    //Resize window in zoom in/out
    $(window).resize(function () {
        winResize();
    });

</script>
