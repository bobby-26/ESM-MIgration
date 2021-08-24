<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestFilter.aspx.cs"
    Inherits="CrewTravelRequestFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Request Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:TabStrip ID="travelrequestfilter" runat="server" OnTabStripCommand="travelrequestfilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVessel ID="ucVessel" runat="server"  AppendDataBoundItems="true"
                            AssignedVessels="true" Entitytype="VSL" Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true" 
                            AutoPostBack="true" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFileNo"  MaxLength="10" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTravelRequestNo"  runat="server" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName"  runat="server" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server"  Width="250px" AppendDataBoundItems="true"></eluc:Zone>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="uctravelstatus" runat="server"  Width="250px" AppendDataBoundItems="true"
                            HardTypeCode="130" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofTravelBetween" runat="server" Text=" Date of Travel Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate"  Width="120px" runat="server" />
                        -
                        <eluc:Date ID="txtEndDate"  Width="120px" runat="server" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server"  Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOfficeCrewChanges" runat="server" Text="Office/Crew Change" Width="250px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlofficetravelyn" runat="server"  Filter="Contains" MarkFirstMatch="true"
                            EnableLoadOnDemand="True" EmptyMessage="Type to select" Width="250px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                <telerik:RadComboBoxItem Text="Crew Change Travel" Value="0" />
                                <telerik:RadComboBoxItem Text="Office Travel" Value="1" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server"  Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text="Crew Change Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server"  Width="250px" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
