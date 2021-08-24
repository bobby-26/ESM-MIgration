<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelHopListFilter.aspx.cs"
    Inherits="CrewTravelHopListFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HopList Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHopListFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuMainHopList" runat="server" OnTabStripCommand="MenuMainHopList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input" VesselsOnly="true"
                            AppendDataBoundItems="true" AssignedVessels="true" Entitytype="VSL" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblArrivalCity" runat="server" Text="Arrival City"></asp:Literal>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtArrivalCity" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblArrivalDatefrom" runat="server" Text="Arrival Date From"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtArrivalDateFrom" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblArrivalDateTo" runat="server" Text="Arrival Date To"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtArrivalDateTo" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
