<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOperationalRiskHazardMapping.aspx.cs" Inherits="InspectionOperationalRiskHazardMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import Hazard - Impact</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align:top;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHazardAdd" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblaspect" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuMSCAT" runat="server" OnTabStripCommand="MenuMSCAT_TabStripCommand" Title="Import Hazard - Impact"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <table runat="server" id="tblaspect" width="100%">
                <tr>
                    <td valign="top" width="49%">
                        <b>
                            <telerik:RadLabel ID="lblaspect" runat="server" Text="Aspect"></telerik:RadLabel>
                        </b>
                        <telerik:RadTextBox ID="txtaspect" runat="server" TextMode="MultiLine" Resize="Both" Width="80%" Rows="4"></telerik:RadTextBox>
                    </td>
                    <td valign="top" width="49%">
                        <b>
                            <telerik:RadLabel ID="lblHazardRisks" runat="server" Text="Hazard / Risks"></telerik:RadLabel>
                        </b>
                        <telerik:RadTextBox ID="txtHazardRisks" runat="server" TextMode="MultiLine" Resize="Both" Width="80%" Rows="4"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="RadDockZone10" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock10" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblHealthandSafety" Text="Health and Safety" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <b>
                                                    <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkHazard" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="ChkHazard_SelectedIndexChanged" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>
                                                    <telerik:RadLabel ID="lblImpact" runat="server" Text="Hazard"></telerik:RadLabel>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkImpact" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock1" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblenvironmental" Text="Environmental" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <b>
                                                    <telerik:RadLabel ID="lblimpacttype" runat="server" Text="Impact Type"></telerik:RadLabel>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButtonList ID="rblimpacttype" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" OnSelectedIndexChanged="rblimpacttype_SelectedIndexChanged" CssClass="checkboxstyle">
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>
                                                    <telerik:RadLabel ID="lblEnvCategory" runat="server" Text="Category"></telerik:RadLabel>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkEnvHazard" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="ChkEnvHazard_SelectedIndexChanged" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEnvImpact" runat="server" Text="Hazard"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkEnvImpact" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock2" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblEconomic" Text="Economic" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEcoCategory" runat="server" Text="Category" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkEcoHazard" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="ChkEcoHazard_SelectedIndexChanged" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEcoImpact" runat="server" Text="Hazard" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkEcoImpact" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" CssClass="checkboxstyle">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
