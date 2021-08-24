<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterFleetFilter.aspx.cs"
    Inherits="Registers_RegisterFleetFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fleet Search</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="fleetfilter" runat="server" OnTabStripCommand="fleetfilter_TabStripCommand"></eluc:TabStrip>
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <table id="tblConfigureFleet" cellpadding="7" cellspacing="2">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFleetCode" runat="server" MaxLength="6" Width="150%" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" Width="150%" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFleetType" runat="server" Text="Fleet Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlFleetType" runat="server" Width="150%" CssClass="dropdown_mandatory">
                        <Items>
                            <telerik:DropDownListItem Text="--Select--" Value="" />
                            <telerik:DropDownListItem Text="Crew" Value="1" />
                            <telerik:DropDownListItem Text="Tech" Value="2" />
                            <telerik:DropDownListItem Text="Accounts" Value="3" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" Width="150%" VesselsOnly="true" AppendDataBoundItems="true"
                        AssignedVessels="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselType ID="ucVesselType" runat="server" Width="150%" AppendDataBoundItems="true"
                        CssClass="input" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input"
                        AppendDataBoundItems="true" Width="150%" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Flag ID="ucFlag" runat="server" Width="150%" AppendDataBoundItems="true" CssClass="input"
                        AutoPostBack="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
