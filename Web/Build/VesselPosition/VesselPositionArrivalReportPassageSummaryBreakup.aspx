<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportPassageSummaryBreakup.aspx.cs"
    Inherits="VesselPositionArrivalReportPassageSummaryBreakup" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Break Up</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNoonReport" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuTab" TabStrip="false" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvPassageSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                            OnItemDataBound="gvPassageSummary_ItemDataBound" OnNeedDataSource="gvPassageSummary_NeedDataSource"
                            EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Report Name">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNOONREPORTDATE"))%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotal" Font-Bold="true" runat="server" Text="Total: "></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Full Speed (hrs)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFullSpeedTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFULLSPEED")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFullSpeedTotalFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Reduced Speed (hrs)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReducedSpeedTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREDUCEDSPEED")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblReducedSpeedFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Stopped (hrs)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStopped" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTOPPED")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblStoppedFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Distance Observed (nm)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotalDistanceObserved" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDISTANCEOBSERVED")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotalDistanceObservedFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Engine Distance (nm)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotalEngineDistance" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGINEDISTANCE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotalEngineDistanceFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="RPM">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="12.5%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRPM" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMERPM")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvManoeuvering" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                            OnItemDataBound="gvManoeuvering_ItemDataBound" OnNeedDataSource="gvManoeuvering_NeedDataSource">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Report Name">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATE"))%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotal" Font-Bold="true" runat="server" Text="Total: "></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Manoeuvering (hrs)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblManoeuvering" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANOEVERING")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblManoeuveringFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Manoeuvering Distance (nm)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblManoeuveringDistance" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANOEVERINGDIST")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblManoeuveringDistFooter" Font-Bold="true" runat="server"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAvgSpeed" runat="server" Text="Avg Speed = (Distance Observed / (Full Speed (hrs) + Reduced Speed (hrs)))"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucAvgSpeed" runat="server" CssClass="readonlytextbox txtNumber" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAvgSlip" runat="server" Text="Avg Slip = ((Engine Distance - Distance Observed) / Engine Distance) * 100"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucAvgSlip" runat="server" CssClass="readonlytextbox txtNumber" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAvgRPM" runat="server" Text="Avg RPM"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucAvgRPM" runat="server" CssClass="readonlytextbox txtNumber" Enabled="false" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
