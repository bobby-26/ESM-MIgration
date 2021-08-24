<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelRequestListFilter.aspx.cs" Inherits="Crew_CrewHRTravelRequestListFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>My Travel Request Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="travelrequestfilter" runat="server" Title="My Travel Request Filter" OnTabStripCommand="travelrequestfilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <table cellpadding="2" cellspacing="2" width="100%">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTravelRequestNo" CssClass="input" runat="server" Width="240px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofTravelBetween" runat="server" Text=" Date of Travel Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" CssClass="input" Width="120px" runat="server" />
                        &nbsp;to&nbsp;
                                    <eluc:Date ID="txtEndDate" CssClass="input" Width="120px" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server" CssClass="input" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server" CssClass="input" Width="240px"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedYN" runat="server" Text="Approved"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlApproved" runat="server" CssClass="input" Width="240px" 
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                <telerik:RadComboBoxItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel Visible="false" ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="uctravelstatus" Visible="false" runat="server" CssClass="input" Width="240px"  AppendDataBoundItems="true"
                            HardTypeCode="130" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
