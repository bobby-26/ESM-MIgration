<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardV2GroupRankChart.aspx.cs" Inherits="DashboardV2GroupRankChart" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">    
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>    
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div id="popupDiv" class="dialDiv1">
            <div id="gMain"></div>

        </div>
    </form>
</body>
</html>
