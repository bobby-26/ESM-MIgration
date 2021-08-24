<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelRequestGeneral.aspx.cs" Inherits="CrewHRTravelRequestGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Family Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHRTravelRequestGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucstatus" runat="server" Visible="false" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuHRTravelRequestGeneral" runat="server" OnTabStripCommand="HRTravelRequestGeneral_TabStripCommand"
                TabStrip="true" Title="My Travel Request"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuHRTravelRequestDetail" runat="server" TabStrip="false" OnTabStripCommand="MenuHRTravelRequestDetail_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepatureDate" runat="server" Text="Travel date"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtDepaturedate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        &nbsp;&nbsp;
                                <telerik:RadComboBox ID="ddlDepartureTime" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepatureCity" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:MUCCity ID="txtDepatureCityId" runat="server" CssClass="input_mandatory" />
                        <telerik:RadLabel ID="lblDepatureCityName" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestinationCity" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:MUCCity ID="txtDestinationCityId" runat="server" CssClass="input_mandatory" />
                        <telerik:RadLabel ID="lblDestinationCityName" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblAccountFrom" runat="server" Text="Travel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblAccountFrom" runat="server"  
                            OnSelectedIndexChanged="rblAccountFrom_SelectedIndexChanged" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Corporate" Value="0" Selected="true" />
                                <telerik:ButtonListItem Text="Vessel" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>

                        <eluc:Vessel runat="server" ID="ddlVessel" Enabled="false" AppendDataBoundItems="true" VesselsOnly="false" AssignedVessels="true"
                            CssClass="input" EntityType="VSL" ActiveVessels="true" />

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
