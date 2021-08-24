<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelRequestApprovalFilter.aspx.cs" Inherits="Crew_CrewHRTravelRequestApprovalFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="travelrequestfilter" runat="server" OnTabStripCommand="travelrequestfilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table cellpadding="2" cellspacing="2" width="60%">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTravelRequestNo"  runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofTravelBetween" runat="server" Text=" Date of Travel Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate"   runat="server" />
                        &nbsp;to&nbsp;
                        <eluc:Date ID="txtEndDate"   runat="server" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server"  Width="60%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server"  Width="60%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedYN" runat="server" Text="Approved Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkApprovedYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel Visible="false" ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="uctravelstatus" Visible="false" runat="server"  Width="124" AppendDataBoundItems="true"
                            HardTypeCode="130" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
