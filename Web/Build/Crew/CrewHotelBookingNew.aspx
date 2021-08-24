<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingNew.aspx.cs"
    Inherits="CrewHotelBookingNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HotelRoom" Src="~/UserControls/UserControlHotelRoom.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hotel Booking New</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="formHBNew" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
    <div>
        <div class="subHeader" style="position: relative">
            <eluc:title runat="server" id="Title1" text="New Booking" showmenu="false"></eluc:title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:tabstrip id="MenuHBNewGeneral" runat="server" ontabstripcommand="MenuHBNewGeneral_TabStripCommand">
            </eluc:tabstrip>
        </div>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
            border: none; width: 100%">
            <table cellpadding="1" cellspacing="4" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:vessel id="ucVessel" runat="server" cssclass="input_mandatory" vesselsonly="true"
                            appenddatabounditems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListAirportdestinationedit">
                            <asp:TextBox ID="txtcityname" runat="server" Width="200px" Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                            <asp:ImageButton ID="btnShowCityedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="Top" Text=".." OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                            <asp:TextBox ID="txtcityid" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblRoomType" runat="server" Text="Room Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:hotelroom id="ucHotelRoom" runat="server" cssclass="dropdown_mandatory" ontextchangedevent="ucHotelRoom_TextChangedEvent"
                            autopostback="true"></eluc:hotelroom>
                    </td>
                    <td>
                        <asp:Literal ID="lblNumberofBeds" runat="server" Text="Number of Beds"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOfBeds" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblExtraBeds" runat="server" Text="Extra Beds"></asp:Literal>
                    </td>
                    <td>
                        <eluc:number id="txtExtraBeds" runat="server" cssclass="input" ispositive="true"
                            isinteger="true" maxlength="1" defaultzero="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCheckinDate" runat="server" Text="Checkin Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:date id="txtCheckinDate" runat="server" cssclass="input_mandatory" datepicker="true">
                        </eluc:date>
                    </td>
                    <td>
                        <asp:Literal ID="lblCheckoutDate" runat="server" Text="Checkout Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:date id="txtCheckoutDate" runat="server" cssclass="input_mandatory" datepicker="true">
                        </eluc:date>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblNumberOfNights" runat="server" Text="Number Of Nights"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOfNights" runat="server" CssClass="input"></asp:TextBox>
                        <asp:ImageButton ID="imgNoOfNights" runat="server" OnClick="imgNoOfNights_Click"
                            ImageUrl="<%$ PhoenixTheme:images/te_calc.png %>" />
                    </td>
                    <td>
                        <asp:Literal ID="lblReason" runat="server" Text="Reason"></asp:Literal>
                    </td>
                    <td>
                        <eluc:travelreason id="ucCrewChangeReason" runat="server" appenddatabounditems="true"
                            cssclass="dropdown_mandatory" reasonfor="1" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
