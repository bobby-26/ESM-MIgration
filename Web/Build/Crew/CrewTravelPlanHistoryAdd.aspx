<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPlanHistoryAdd.aspx.cs" Inherits="CrewTravelPlanHistoryAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMedical" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuHistory" runat="server" OnTabStripCommand="MenuHistory_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" CssClass="dropdown_mandatory" runat="server" AssignedVessels="true" AutoPostBack="true"
                            OnTextChangedEvent="ucVessel_OnTextChanged" Width="50%" Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
              
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                            DataValueField="FLDACCOUNTID" Width="50%"
                            EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOrigin" Text="Origin" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MUCCity ID="ucOriginCity" runat="server" CssClass="dropdown_mandatory" Width="50%" />
                </td>
            
                <td>
                    <telerik:RadLabel ID="lblDestination" Text="Destination" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MUCCity ID="ucDestinationCity" runat="server" CssClass="dropdown_mandatory" Width="50%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDepDate" runat="server" Width="50%" CssClass="input_mandatory" />
                </td>
            
                <td>
                    <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucArrDate" runat="server" Width="50%" CssClass="input_mandatory" />
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrency" Text="Currency" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCurrency ID="ucCurrency" runat="server" AppendDataBoundItems="true" Width="50%"
                        CssClass="input_mandatory"></eluc:UserControlCurrency>
                </td>
          
                <td>
                    <telerik:RadLabel ID="lblTicketNo" Text="Ticket No" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTicketNo" runat="server" Width="50%" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblClass" Text="Class" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucClass" runat="server" AppendDataBoundItems="true"
                        HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" Width="50%" />
                </td>
          
                <td>
                    <telerik:RadLabel ID="lblAirlineCode" Text="AirLine Code" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAirlineCode" runat="server" Width="50%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAmount" Text="Amount" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtAmount" runat="server" Width="50%" />
                </td>
           
                <td>
                    <telerik:RadLabel ID="lblTax" Text="Tax" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtTax" runat="server" Width="50%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPNR" Text="PNR No." runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPNR" runat="server" Width="50%"></telerik:RadTextBox>
                </td>

            </tr>


        </table>
    </form>
</body>
</html>
