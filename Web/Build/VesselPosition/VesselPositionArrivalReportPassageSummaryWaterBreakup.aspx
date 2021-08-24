<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportPassageSummaryWaterBreakup.aspx.cs"
    Inherits="VesselPositionArrivalReportPassageSummaryWaterBreakup" MaintainScrollPositionOnPostback="true" %>

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
    <title>Noon Report</title>
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
                        <telerik:RadDockZone ID="RadDockZone2" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock1" runat="server" Title="<b>ROB on Prev Arr</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>

                                    <telerik:RadGrid ID="gvROBonPrevArr" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvROBonPrevArr_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBONARRIVAL")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock2" runat="server" Title="<b>Received</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvReceived" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvReceived_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Received">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReceived" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVED")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>Cons in Port</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvConsinPort" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                                        OnItemDataBound="gvConsinPort_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                                        OnNeedDataSource="gvConsinPort_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNOONREPORTDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblTotalFooter" runat="server" Text="Total:"></telerik:RadLabel></b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cons in Port">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblConsinPort" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONSUMPTIONQTY")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblConsinPortFooter" runat="server"></telerik:RadLabel>
                                                        </b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone4" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>ROB on Dep</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvROBonPrevDep" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvROBonPrevDep_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblOilNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblReportTypeHeader" runat="server" Text="Report Name"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblDateHeader" runat="server" Text="Date"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblROBHeader" runat="server" Text="ROB"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBONDEPARTURE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone5" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock5" runat="server" Title="<b>Produced</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvProduced" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                                        OnItemDataBound="gvProduced_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvProduced_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNOONREPORTDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblTotalFooter" runat="server" Text="Total:"></telerik:RadLabel></b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Produced">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblProduced" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRODUCED")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblProducedFooter" runat="server"></telerik:RadLabel>
                                                        </b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone6" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock6" runat="server" Title="<b>ROB on Arr</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvROBonArr" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvROBonArr_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblOilNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblReportTypeHeader" runat="server" Text="Report Name"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblDateHeader" runat="server" Text="Date"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblROBHeader" runat="server" Text="ROB"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBONARR")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone7" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock7" runat="server" Title="<b>Cons at Sea</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvConsatSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                                        OnItemDataBound="gvConsatSea_ItemDataBound" OnNeedDataSource="gvConsatSea_NeedDataSource"
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNOONREPORTDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblTotalFooter" runat="server" Text="Total:"></telerik:RadLabel></b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cons at Sea">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblConsatSea" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONSUMPTIONQTY")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b>
                                                            <telerik:RadLabel ID="lblConsatSeaFooter" runat="server"></telerik:RadLabel>
                                                        </b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone8" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock8" runat="server" Title="<b>Cons at Arr</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvConsatArr" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvConsatArr_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONSATARR")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td valign="top">
                        <telerik:RadDockZone ID="RadDockZone9" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock9" runat="server" Title="<b>Cons at Dep</b>" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvConsatDep" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvConsatDep_NeedDataSource">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Report Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONSATDEP")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
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
