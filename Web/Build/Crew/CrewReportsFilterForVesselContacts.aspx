<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsFilterForVesselContacts.aspx.cs" Inherits="ReportsFilterForVesselContacts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagName="UserControlStatus" TagPrefix="eluc" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagName="ucVessel" TagPrefix="eluc" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagName="ucVesselType" TagPrefix="eluc" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagName="ucAddressType" TagPrefix="eluc" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Filter</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmReportsFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand" TabStrip="false"></eluc:TabStrip>

            <table width="100%" class="style4">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucAddressType runat="server" ID="ucPrincipal" AddressType="128" OnTextChangedEvent="PrincipalSelection" AppendDataBoundItems="true" Width="80%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVessel runat="server" ID="ucVessel" AppendDataBoundItems="true" OnTextChangedEvent="VesselSelection" Entitytype="VSL" AssignedVessels="true" VesselsOnly="true" ActiveVesselsOnly="true" />
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVesselType runat="server" ID="ucVesselType" AppendDataBoundItems="true" />
                    </td>
                </tr>

            </table>


            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 550px; width: 100%;"></iframe>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
