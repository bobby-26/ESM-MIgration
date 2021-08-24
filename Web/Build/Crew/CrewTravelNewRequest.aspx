<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelNewRequest.aspx.cs"
    Inherits="CrewTravelNewRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewTraveladd" runat="server"  Title="New Travel Request" OnTabStripCommand="MenuCrewTraveladd_TabStripCommand"></eluc:TabStrip>
            <br />
            <table width="100%" cellpadding="1" cellspacing="2">
                <tr>
                    <td width="100px">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucvessel" CssClass="dropdown_mandatory" runat="server" AssignedVessels="true" Width="20%" AutoPostBack="true"
                            OnTextChangedEvent="ucVessel_OnTextChanged" Entitytype="VSL" ActiveVesselsOnly ="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" OnDataBound="ddlAccountDetails_DataBound"
                            Width="20%" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ReasonFor="2" Width="20%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurposeDesc" runat="server" Text="Purpose Desc"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPurposeDesc" runat="server" TextMode="MultiLine" Width="20%"
                            Height="10%" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengersFrom" runat="server" Text="Passengers from"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPassfrom" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="20%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="0" Text="Office" />
                                <telerik:RadComboBoxItem Value="1" Text="Crew" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>            
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
