
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDashboardMenu.ascx.cs"
    Inherits="UserControlDashboardMenu" %>       
 <%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript">
        function DashboardRefresh() {
            AjxPost("functionname=DASHBOARDREFRESH", SitePath + "PhoenixWebFunctions.aspx", null, false);
            // fnReloadList('codehelp1', 'true', 'true');
                $("#dialog").dialog({
                    modal: true,
                    title: "Dashboard Refresh",
                    width: 300,
                    height: 150,
                    open: function (event, ui) {
                        setTimeout(function () {
                            $("#dialog").dialog("close");
                        }, 2000);
                    }
                });

        }
    </script>
<div class="chartTitleBar">
<span id="cmdFilter" runat="server" data-toggle="tooltip" title="Filter Vessels" class="filterDiv glyphicon glyphicon-filter"></span>
        <div class="titleLabel">       
        <span id="spnApplication" runat="server" class="titleLabel"></span>
        <span id="spnModule" runat="server" class="titleLabel"></span>
    </div>
</div>

 <div id="dialog" style="display: none">
    Dashboard will be updated in few minutes..
</div>

<eluc:Status ID="ucStatus" runat="server" />

