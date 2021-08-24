<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelOffSignersTravelRequest.aspx.cs"
    Inherits="CrewTravelOffSignersTravelRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirmation " Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew offsigners Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="fmCrewOffSignersTravelRequest" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="fmCrewOffSignersTravelRequest" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuCrewTravel" runat="server" OnTabStripCommand="MenuCrewTravel_TabStripCommand"></eluc:TabStrip>

            <table width="90%">
                <tr>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblFirstName" runat="Server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="Server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="File No."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlAccountDetails" runat="server" EnableLoadOnDemand="True"
                            CssClass="dropdown_mandatory" EmptyMessage="Type to select Account"
                            Width="240px" OnDataBound="ddlAccountDetails_DataBound" Filter="Contains" MarkFirstMatch="true" DataValueField="FLDACCOUNTID" DataTextField="FLDVESSELACCOUNTNAME">
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofTravel" runat="server" Text="Crew Change"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfCrewChange" runat="server" CssClass="input_mandatory" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ucport" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangereason" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucCrewChangeReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" ReasonFor="1" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server" CssClass="input_mandatory" Width="240px" />
                    </td>
                    <td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="Server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server" CssClass="input_mandatory" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="DepatureDate" runat="server" Text="Depature"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDepatureDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblArrivalDate" runat="server">Arrival</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtArrivalDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentMode" runat="server">Payment Mode</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" Width="240px" />
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
            <%--  <eluc:Confirmation ID="ucConfirm" runat="server" OnConfirmMesage="ApproveCrewTravelReq" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
