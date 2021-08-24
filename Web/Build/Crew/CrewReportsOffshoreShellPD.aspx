<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsOffshoreShellPD.aspx.cs"
    Inherits="CrewReportsOffshoreShellPD" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Offshore Shell PD</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()">
    <form id="frmReportCommittedCost" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
        TabStrip="false"></eluc:TabStrip>
    <table width="100%">
        <tr>
            <td>
                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                </telerik:RadLabel>
            </td>
            <td>
                <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                    CssClass="dropdown_mandatory" AssignedVessels="true" Entitytype="VSL" />
            </td>
            <td>
                <telerik:RadLabel ID="lblFileno" runat="server" Text="File No">
                </telerik:RadLabel>
            </td>
            <td>
                <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input_mandatory">
                </telerik:RadTextBox>
            </td>
        </tr>
    </table>
    <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
        width: 100%;"></iframe>
    </form>
</body>
</html>
