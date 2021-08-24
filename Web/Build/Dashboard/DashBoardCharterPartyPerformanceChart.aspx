<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoardCharterPartyPerformanceChart.aspx.cs" 
    Inherits="Dashboard_DashBoardCharterPartyPerformanceChart" %>

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
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart_charterparty.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body style="background: rgba(230,245,254,1)">
<form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top;text-align:left; font-size:14px;background:#0C8AD2;color:white">
          <asp:Literal ID="lbltitle" runat="server" Text="Charter Party Performance"></asp:Literal>
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

    <div id="cpGraph" style="height: 100%; width: 100%; margin: 0 auto;"></div>
  </div>
</form>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function () {
        var seriesData = <%=this.seriesdata%>;
        var dateRange = <%=this.dateList%>

        CharterpartyChart(seriesData,dateRange);

  });

    //Resize window in zoom in/out
    $(window).resize(function () {
        winResize();
    });

</script>
