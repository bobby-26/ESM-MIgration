<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingGeneral.aspx.cs"
    Inherits="CrewHotelBookingGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HotelRoom" Src="~/UserControls/UserControlHotelRoom.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hotel Booking General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="formHBGeneral" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
    </eluc:Error>
    <div>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Booking" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuHBGeneral" runat="server" OnTabStripCommand="MenuHBGeneral_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
            border: none; width: 100%">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="lblReferenceNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="4" width="100%">
                <tr>
                    <%--  <td>
                        City
                    </td>
                    <td>
                        <asp:TextBox ID="txtCityName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>--%>
                    <td>
                        <asp:Literal ID="lblRoomType" runat="server" Text="Room Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:HotelRoom ID="ucHotelRoom" runat="server" CssClass="dropdown_mandatory" OnTextChangedEvent="ucHotelRoom_TextChangedEvent"
                            AutoPostBack="true">
                        </eluc:HotelRoom>
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
                        <eluc:Number ID="txtExtraBeds" runat="server" CssClass="input" ispositive="true"
                            isinteger="true" MaxLength="1" defaultzero="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblNoofRooms" runat="server" Text="No.of Rooms"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOfRooms" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCheckinDate" runat="server" Text="Checkin Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckinDate" runat="server" CssClass="input_mandatory" DatePicker="true">
                        </eluc:Date>
                        <asp:TextBox ID="txtTimeOfCheckIn" runat="server" CssClass="input_mandatory" Width="50px"
                            Visible="false" />
                        <ajaxToolkit:MaskedEditExtender ID="txtCheckInTimeMask" runat="server" AcceptAMPM="false"
                            ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                            TargetControlID="txtTimeOfCheckIn" UserTimeFormat="TwentyFourHour" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCheckoutDate" runat="server" Text="Checkout Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckoutDate" runat="server" CssClass="input_mandatory" DatePicker="true">
                        </eluc:Date>
                        <asp:TextBox ID="txtTimeOfCheckOut" runat="server" CssClass="input_mandatory" Width="50px"
                            Visible="false" />
                        <ajaxToolkit:MaskedEditExtender ID="txtCheckOutTimeMask" runat="server" AcceptAMPM="false"
                            ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                            TargetControlID="txtTimeOfCheckOut" UserTimeFormat="TwentyFourHour" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblNumberOfNights" runat="server" Text="Number Of Nights"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOfNights" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        <asp:ImageButton ID="imgNoOfNights" runat="server" OnClick="imgNoOfNights_Click"
                            ImageUrl="<%$ PhoenixTheme:images/te_calc.png %>" ToolTip="Calculate" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCrewChangeReason" runat="server" Text="Crew Change Reason"></asp:Literal>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucCrewChangeReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" ReasonFor="1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblPayableBy" runat="server" Text="Payable By"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185"
                            OnTextChangedEvent="ucPaymentmode_TextChangedEvent"/>
                    </td>
                    <td>
                        <asp:Literal ID="lblPurpose" runat="server" Text="Purpose"></asp:Literal>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucPurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ReasonFor="1" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="pnlComanyPayableCharges" GroupingText="Comany Payable Charges">
                            <asp:CheckBoxList ID="cblComanyPayableCharges" runat="server" DataValueField="FLDHOTELCHARGESID"
                                DataTextField="FLDCHARGINGNAME" RepeatDirection="Horizontal" RepeatColumns="2">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server">
            </eluc:Status>
        </div>
    </form>
</body>
</html>
