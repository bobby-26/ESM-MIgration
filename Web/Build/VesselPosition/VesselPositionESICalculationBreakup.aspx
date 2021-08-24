<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionESICalculationBreakup.aspx.cs"
    Inherits="VesselPositionESICalculationBreakup" MaintainScrollPositionOnPostback="true" %>

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
    <title>ESI Calculation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNoonReport" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuTab" TabStrip="false" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Voyage No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoyageNo" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageStartDate" runat="server" Text="Voyage Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoyageStartDate" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 50%;" valign="top">
                        <table>

                            <tr>
                                <td colspan="2">
                                    <telerik:RadDockZone ID="RadDockZone2" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock1" runat="server" Title="<b>CO2 Emission</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <telerik:RadGrid ID="gvOilConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                    Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                                                    OnItemDataBound="gvOilConsumption_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                                                    OnNeedDataSource="gvOilConsumption_NeedDataSource">
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
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
                                                            <telerik:GridTemplateColumn HeaderText="Name">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Cons Qty" HeaderStyle-Wrap="false">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblConsumptionQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALCONSUMPTIONQTY")%>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="CF">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCF" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCF")%>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <b><telerik:RadLabel ID="lblTotalFooter" runat="server" Text="Total:"></telerik:RadLabel></b>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="CO2 Emission">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCo2Emission" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCO2EMISSION")%>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                   <b> <telerik:RadLabel ID="lblCo2EmissionFooter" runat="server"></telerik:RadLabel></b>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </td>
                            </tr>
                            <tr>

                                <td colspan="2">
                                    <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock2" runat="server" Title="<b>Distance Observed</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <telerik:RadGrid ID="gvDistance" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                    Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" EnableViewState="false" ShowFooter="true"
                                                    OnItemDataBound="gvDistance_ItemDataBound" OnNeedDataSource="gvDistance_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
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
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblReporttype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTTYPE")%>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Date">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNOONREPORTDATE")) %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                   <b> <telerik:RadLabel ID="lblTotalFooter" runat="server" Text="Total:"></telerik:RadLabel></b>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Distance Observed">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblDistance" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDISTANCEOBSERVED")%>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                  <b>  <telerik:RadLabel ID="lblDistanceFooter" runat="server"></telerik:RadLabel></b>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblco2if" runat="server" Text="CO2 Index = (CO2 Emission / Distance) * 1000"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtco2i" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" DecimalPlace="3" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lbleeoif" runat="server" Text="EEOI = (CO2 Index / Cargo) * 1000"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txteeoi" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" DecimalPlace="3" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtco2e" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtDistanceObserved" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtCargo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>Base Line Sulphur</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                    <telerik:RadGrid ID="gvBaseLineSC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvBaseLineSC_NeedDataSource">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
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
                                            <telerik:GridTemplateColumn Visible="false" HeaderText="Tier">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTier" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTIER")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="High">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblbsout" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBSOUT") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Mid">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblbsin" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSIN")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Low">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblbsberth" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSBERTH")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                            </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                    </telerik:RadGrid>
                                                 </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
                                </td>
                                <td>
                                    <telerik:RadDockZone ID="RadDockZone4" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>NOx</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                    <telerik:RadGrid ID="gvNOX" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvNOX_NeedDataSource">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
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
                                            <telerik:GridTemplateColumn HeaderText="NOx" HeaderStyle-Wrap="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblnox" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNOX")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="OPS" HeaderStyle-Wrap="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblops" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPS") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="EEOI Reported" HeaderStyle-Width="50%" HeaderStyle-Wrap="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblbsin" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEEOIREPORTED")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                            </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                    </telerik:RadGrid>
                                                </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadDockZone ID="RadDockZone5" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock5" runat="server" Title="<b>Actual Sulphur Content</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                    <telerik:RadGrid ID="gvActualSC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" EnableViewState="false"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvActualSC_NeedDataSource">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
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
                                            <telerik:GridTemplateColumn HeaderText="Oil Name">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblOilName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Location">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLOCATION")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Actual Sulphur Content">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblActualSC" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTUALSC")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                            </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                    </telerik:RadGrid>
                                                </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblesisoxcal" runat="server" Text="ESI_SOx = ( ((BSout - ASout) / (3.00) * 30) ) + ( ((BSin - ASin) / (0.4) * 35) ) + ( ((BSberth - ASberth) / (0.1) * 35) )"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtesisox" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" DecimalPlace="3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lbloverallesical" runat="server" Text="Overall ESI = (2 * NOx + SOx + EEOIREPORTED + OPS) / 3.1"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtoverallesi" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" DecimalPlace="3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblNote" runat="server" Text="<u><b>Note:</b></u> If EEOI Reported is ''YES'' then EEOIREPORTED = 10 else EEOIREPORTED = 0 | If OPS  is ''YES'' then OPS = 35 else OPS = 0"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%;" valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadDockZone ID="RadDockZone6" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                                        <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock6" runat="server" Title="<b>Cargo</b>" EnableAnimation="true"
                                            EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                    <telerik:RadGrid ID="gvCargo" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" EnableViewState="false" ShowFooter="true"
                                        OnItemDataBound="gvCargo_ItemDataBound" OnNeedDataSource="gvCargo_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false">
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
                                                <telerik:GridTemplateColumn HeaderText="Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Port Name">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSeaPortName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Loaded">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblLoaded" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLOADED")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                       <b> <telerik:RadLabel ID="lblCFooter" runat="server" Text="Cargo:"></telerik:RadLabel></b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Discharged">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDischarged" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDISCHARGED")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <b><telerik:RadLabel ID="lblCargoFooter" runat="server"></telerik:RadLabel></b>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                                </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
