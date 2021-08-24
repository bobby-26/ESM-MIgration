<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelieveeFilter.aspx.cs"
    Inherits="CrewPlanRelieveeFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register Src="../UserControls/UserControlNationalityList.ascx" TagName="UserControlNationalityList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register Src="../UserControls/UserControlZoneList.ascx" TagName="UserControlZoneList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlPoolList.ascx" TagName="UserControlPoolList" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relief Plan Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="PlanRelieveeFilterMain" runat="server" OnTabStripCommand="PlanRelieveeFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" />
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                        <eluc:Date ID="txtFromTo" runat="server" AutoPostBack="true" OnTextChangedEvent="CalulateDays" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblRelefDuePlanRelief" runat="server" Text="Relief Due / Plan Relief"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReliefDue" runat="server" CssClass="input txtNumber" MaxLength="3" IsInteger="true" AutoPostBack="false" OnTextChangedEvent="CalculateDatetime" />
                        <telerik:RadLabel ID="lblDays" runat="server" Text="(Days)"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="100%" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128"
                            AutoPostBack="true" OnTextChangedEvent="ucPrincipal_TextChangedEvent" AppendDataBoundItems="true"
                            Width="100%" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVesselTypeList ID="lstVesselType" runat="server" AutoPostBack="true" AppendDataBoundItems="false"
                            OnTextChangedEvent="ucPrincipal_TextChangedEvent" Width="100%" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVesselList ID="ucVessel" runat="server" Entitytype="VSL" AssignedVessels="true" VesselsOnly="true" Width="100%" ActiveVesselsOnly="true" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlZoneList ID="ucZone" AppendDataBoundItems="true" runat="server" Width="100%" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="ucRank" runat="server" Width="100%" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlPoolList ID="ucPool" AppendDataBoundItems="true" runat="server" Width="100%" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblGroupRank" runat="server" Text="Group Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlGroupRank" runat="server" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID"
                            EmptyMessage="Type to select group rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNotPlanned" runat="server" Text="Not Planned Only"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkNotPlanned" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
