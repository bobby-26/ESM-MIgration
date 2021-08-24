<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPDFilter.aspx.cs" Inherits="CrewPDFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlHard.ascx" TagName="Hard" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVesselCommon.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Approval Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuPD" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <Telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></Telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true"  Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <Telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></Telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" 
                            AutoPostBack="true" OnTextChangedEvent="ucPrincipal_TextChangedEvent" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <Telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></Telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                             Width="240px" AssignedVessels="true" Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <Telerik:RadLabel ID="lblPDStatus" runat="server" Text="PD Status"></Telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Hard ID="ddlPDStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="99" Width="240px"
                             />
                    </td>
                </tr>
                <tr>
                    <td>
                        <Telerik:RadLabel ID="lblProposedBy" runat="server" Text="Proposed By"></Telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:User ID="ddlUser" runat="server" AppendDataBoundItems="true"  ActiveYN="172" Width="240px" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
