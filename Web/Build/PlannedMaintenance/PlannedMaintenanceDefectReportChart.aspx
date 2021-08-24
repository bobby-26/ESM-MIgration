<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectReportChart.aspx.cs"
    Inherits="PlannedMaintenanceDefectReportChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html lang="en">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
        <script type="text/javascript">
            function Resize() {
                $("#popupDiv").width($(window).width);
                $("element").width($(window).width);
                myChartExt.resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="Resize();">
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div style="width: 100%; border: 1px; background-color: #1c84c6;">
            <table style="width: 50%; color: #ffffff; font: 11px Helvetica, Arial, sans-serif; font-size: 11px; font-weight: bold">
                <tr>
                    <td>
                        <asp:Label ID="lblVesselName" runat="server" Text="Vessel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox id="txtVesselName" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" Visible="true" Style="width: 110px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="ajxFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                            PopupPosition="Right" Enabled="true">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true" Visible="true" Style="width: 110px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="ajxToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate"
                            PopupPosition="Right" Enabled="true">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div id="ChartDiv" style="height: 600px; width: 100%; margin: 0px auto; border-bottom: 1px solid rgb(153, 153, 153); -moz-user-select: none; position: relative; background: transparent none repeat scroll 0% 0%;" _echarts_instance_="ec_1526453946634">
            <div style="position: relative; overflow: hidden; width: 1302px; height: 599px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;">
                <canvas style="position: absolute; left: 0px; top: 0px; width: 1302px; height: 599px; -moz-user-select: none; padding: 0px; margin: 0px; border-width: 0px;" width="1302" height="599" data-zr-dom-id="zr_0"></canvas>
            </div>

        </div>
    </form>
</body>
</html>
