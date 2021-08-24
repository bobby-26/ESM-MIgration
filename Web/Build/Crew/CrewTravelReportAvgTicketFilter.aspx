<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelReportAvgTicketFilter.aspx.cs" Inherits="CrewTravelReportAvgTicketFilter" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"></eluc:TabStrip>

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <telerik:RadLabel ForeColor="Black" ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <telerik:RadListBox ID="ddlYear" SelectionMode="Multiple" AppendDataBoundItems="true" Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                    </td>

                </tr>
                <tr>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <telerik:RadLabel ForeColor="Black" ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <eluc:City ID="ddlOrigin" runat="server" AppendDataBoundItems="true" Width="240px" />
                    </td>

                </tr>
                <tr>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <telerik:RadLabel ForeColor="Black" ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <eluc:City ID="ddlDestination" runat="server" AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
