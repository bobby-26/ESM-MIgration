<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFollowUpFilter.aspx.cs"
    Inherits="CrewFollowUpFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="../UserControls/UserControlVesselTypeList.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Follow Up Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewFollowUpFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCrewFollowUp" runat="server" OnTabStripCommand="CrewFollowUp_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFollowUp" runat="server" Text="Follow up by"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:User ID="ddlUser" runat="server" AppendDataBoundItems="true" 
                            ActiveYN="172" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ddlVesselType" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Last Contact Between"
                            Width="100%">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblContactFrom" runat="server" Text="Last Contact From"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucLastContactFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblContactTo" runat="server" Text="Last Contact To"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucLastContactToDate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlPeriodoftest" runat="server" GroupingText="Follow up period"
                            Width="100%">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucToDate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
