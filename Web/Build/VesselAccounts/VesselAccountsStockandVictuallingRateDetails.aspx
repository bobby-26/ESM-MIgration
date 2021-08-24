<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsStockandVictuallingRateDetails.aspx.cs"
    Inherits="VesselAccountsStockandVictuallingRateDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Stock and Victualling Rate</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmRegistersRank" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                            AssignedVessels="true" Entitytype="VSL" ActiveVessels="true" Width="180px" />
                    </td>
                    <td>
                        <asp:Literal ID="lblFromDate" runat="server" Text="From"></asp:Literal></td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <asp:Literal ID="lblToDate" runat="server" Text="To"></asp:Literal></td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <asp:Literal ID="lblReportType" runat="server" Text="Report Type"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlType" runat="server" EnableLoadOnDemand="True" Width="240px"
                            EmptyMessage="Type to select Type" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="typechange">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Bond Closing Stock" />
                                <telerik:RadComboBoxItem Value="2" Text="Provision Closing Stock" />
                                <telerik:RadComboBoxItem Value="3" Text="Victualling Rate" />
                                <telerik:RadComboBoxItem Value="4" Text="Victualling Rate – Detailed Report" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuStock" runat="server" OnTabStripCommand="MenuStock_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvStock" runat="server" AutoGenerateColumns="False" Width="100%" Height="92%" ShowHeader="true"
                AllowCustomPaging="false" EnableViewState="false" CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true"
                EnableHeaderContextMenu="true" ShowFooter="false" OnNeedDataSource="gvStock_NeedDataSource" MasterTableView-AutoGenerateColumns="true"
                OnColumnCreated="gvStock_ColumnCreated" OnInfrastructureExporting="gvStock_InfrastructureExporting"
                OnExportCellFormatting="gvStock_ExportCellFormatting" OnGridExporting="gvStock_GridExporting">
                <ExportSettings ExportOnlyData="true" Excel-Format="Biff" UseItemStyles="true" IgnorePaging="true" OpenInNewWindow="true">
                </ExportSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100%"
                    GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <ColumnGroups>

                        <telerik:GridColumnGroup HeaderText="" Name="1" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="2" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="3" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="" Name="4" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="5" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="6" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="" Name="7" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="8" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="9" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="" Name="10" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="11" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="12" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="1" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
