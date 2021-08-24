<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingDetailsFilter.aspx.cs"
    Inherits="CrewHotelBookingDetailsFilter" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HotelRoom" Src="~/UserControls/UserControlHotelRoom.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hotel Booking Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHotelBookingFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHotelBookingFilter" runat="server" OnTabStripCommand="MenuHotelBookingFilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table width="60%" cellspacing="1" cellpadding="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtReferenceNo" MaxLength="50" Width="40%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="40%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"
                            AssignedVessels="true" Entitytype="VSL" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAirportdestinationedit">
                            <asp:TextBox ID="txtcityname" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="btnShowCityedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="Top" Text=".." OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                            <asp:TextBox ID="txtcityid" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucPurpose" runat="server" AppendDataBoundItems="true" Width="40%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBookingStatus" runat="server" Text="Booking Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlBookingStatus" runat="server" HardTypeCode="223" Width="40%"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCheckinDateBW" runat="server" Text="Checkin Date B/W"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckinDateFrom" runat="server" DatePicker="true" />
                        <eluc:Date ID="txtCheckinDateTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCheckOutDateBW" runat="server" Text="CheckOut Date B/W"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckOutDateFrom" runat="server" DatePicker="true" />
                        <eluc:Date ID="txtCheckOutDateTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGuestStatus" runat="server" Text="Guest Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblGuestStatus" runat="server" Layout="Flow" Columns="2" Direction="Horizontal" >
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Open" />
                                <telerik:ButtonListItem Value="0" Text="Cancelled" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
