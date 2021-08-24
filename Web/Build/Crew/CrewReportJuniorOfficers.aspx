<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportJuniorOfficers.aspx.cs" Inherits="Crew_CrewReportsJuniorOfficersReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Junior Officers Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>                
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
  </telerik:RadCodeBlock>  
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewReportEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Junior Officers Report"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand" TabStrip ="false">
                </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                <div>
                <table width="100%" >
                <tr>
                <td>
                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems ="true" CssClass="input" VesselsOnly="true"  />                      
                </td>
                <td>
                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                </td>
                <td>
                <eluc:VesselType runat="server" ID="ucVesselType" AppendDataBoundItems = "true" CssClass="input"  />
                </td>
                <td>
                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                </td>
                <td>
                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" CssClass="input" />
                </td>
                </tr>
                <tr>
                <td >
                <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                
                </td>
                <td>
                <eluc:Hard id="ddlMonthlist" runat ="server" HardTypeCode="55" SortByShortName="true" AppendDataBoundItems = "true" CssClass="input_mandatory" />
                </td>
                <td>
                <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                
                </td>
                <td>
                <eluc:Quick ID="ddlYearlist" runat="server" QuickTypeCode="55" AppendDataBoundItems="true" CssClass="input_mandatory" />
                </td>
                </tr>
                </table>
                </div>
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                     <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                     </eluc:TabStrip>
                 </div>
                <div>
                <div id="divGrid" runat="server" style="position: relative; overflow:auto; z-index: 0" >
                <table width="100%">
                <td align="left">
                <asp:Literal runat="server" ID="ltGrid" Text=""></asp:Literal> 
                </td>
                </table>
                </div>  
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
