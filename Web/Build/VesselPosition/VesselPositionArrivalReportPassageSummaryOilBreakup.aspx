<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportPassageSummaryOilBreakup.aspx.cs"
    Inherits="VesselPositionArrivalReportPassageSummaryOilBreakup" MaintainScrollPositionOnPostback="true" %>

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
    <title>Fuel ROB & Consumption</title>
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
                    <td>
                        <u><b>
                            <telerik:RadLabel ID="lblROBCOSP" runat="server" Text="ROB @ COSP:"></telerik:RadLabel>
                        </b></u>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvROBCOSP" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnNeedDataSource="gvROBCOSP_NeedDataSource"
                            Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
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
                                            <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBCOSP")%>'></telerik:RadLabel>
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
                    <td>
                        <u><b>
                            <telerik:RadLabel ID="lblROBFWE" runat="server" Text="ROB @ EOSP:"></telerik:RadLabel>
                        </b></u>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvROBFWE" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnNeedDataSource="gvROBFWE_NeedDataSource"
                            Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
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
                                            <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBFWE")%>'></telerik:RadLabel>
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
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalCons" runat="server" Text="Total Cons = (ROB @ COSP - ROB @ FWE)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucTotalCons" runat="server" CssClass="readonlytextbox txtNumber" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAvgConsPerDay" runat="server" Text="Avg Cons/day = (Total Cons / (Full Speed (hrs) + Reduced Speed (hrs))) * 24"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucAvgConsPerDay" runat="server" CssClass="readonlytextbox txtNumber" Enabled="false" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
