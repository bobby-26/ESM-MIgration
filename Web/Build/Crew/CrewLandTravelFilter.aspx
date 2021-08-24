<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLandTravelFilter.aspx.cs" Inherits="CrewLandTravelFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Land Travel Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLandTravelFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuLandTravelFilter" runat="server" OnTabStripCommand="MenuLandTravelFilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table width="60%" cellspacing="1" cellpadding="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtReferenceNo" MaxLength="50" Width="40%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAirportdestinationedit">
                            <telerik:RadTextBox ID="txtcityname" runat="server" Enabled="False" Width="40%"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowCityedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="Top" Text=".." OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                            <telerik:RadTextBox ID="txtcityid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDateHeader" runat="server" Text="Travel From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToDateHeader" runat="server" Text="Travel To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReqFromDateHeader" runat="server" Text="Requested From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReqFromDate" runat="server" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReqToDateHeader" runat="server" Text="Requested To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReqToDate" runat="server" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTypeofDuty" runat="server" Text="Type of Duty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucTypeOfDuty" runat="server" AppendDataBoundItems="true" HardTypeCode="247" Width="40%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatusHeader" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="248" Width="40%"/>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
