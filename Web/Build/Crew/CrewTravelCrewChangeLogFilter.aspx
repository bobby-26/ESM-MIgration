<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelCrewChangeLogFilter.aspx.cs"
    Inherits="CrewTravelCrewChangeLogFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Request Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="travelrequestfilter" runat="server" OnTabStripCommand="travelrequestfilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucvessel" runat="server" AssignedVessels="true" Width="50%"
                            AppendDataBoundItems="true" Entitytype="VSL" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDatepfTravelBetween" runat="server" Text="Date of Travel Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" CssClass="input_mandatory" Width="30%" runat="server" />
                        &nbsp;<telerik:RadLabel ID="RadLabel1" runat="server" Text="-"></telerik:RadLabel>
                        &nbsp;
                            <eluc:Date ID="txtEndDate" CssClass="input_mandatory" Width="30%" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="uctravelstatus" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="130" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="Server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOfficeCrewChange" runat="server" Text="Office/Crew Change"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlofficetravelyn" runat="server" Width="80%"
                            AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="DUMMY" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="0" Text="Crew Change Travel" />
                                <telerik:RadComboBoxItem Value="1" Text="Office Travel" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShowCancelledEmployees" runat="server" Text="Show Cancelled Employees"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCanceledEmployees" runat="server" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text="Crew Change Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ucport" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestNumber" runat="server" Text="Request Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTravelRequestNo" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName" runat="server" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
