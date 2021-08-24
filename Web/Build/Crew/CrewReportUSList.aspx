<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportUSList.aspx.cs" Inherits="CrewReportUSCrewList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Specific Crew List</title>
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RecruitmentTarget" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
       
                <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>
          
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                                CssClass="dropdown_mandatory" EntityType="VSL" AssignedVessels="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPortofArrivalDeparture" runat="server" Text="Port of Arrival/Departure"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort ID="ucSeaPort" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                        </td>
                   </tr>
                   <tr>   
                        <td>
                            <telerik:RadLabel ID="lblDateofArrivalDeparture" runat="server" Text="Date of Arrival/Departure"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFormats" runat="server" Text="Formats"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblFormats" runat="server" Layout="Flow" Columns="2"
                                OnSelectedIndexChanged="rblFormats_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                <telerik:ButtonListItem Text="Page #1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Page #2"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                   </tr>
                </table>
            
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 85%;
                    width: 100%;"></iframe>
        </telerik:RadAjaxPanel>
        </form>
    </body>
</html>
