<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceHistoryTemplateReports.aspx.cs"
    Inherits="PlannedMaintenanceHistoryTemplateReports" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Form Reports</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="formPMHistoryTemplateReports" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuHistoryTemplateReports" runat="server" OnTabStripCommand="MenuHistoryTemplateReports_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuGrid" runat="server" OnTabStripCommand="MenuGrid_TabStripCommand" />
<%--        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S. No.">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>
                                <telerik:RadLabel ID="lblReportId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormtype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcomponentno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENT")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="uclblcomponentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Report Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREPORTDATE">
                            <HeaderStyle Width="110px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblReportDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTDATE"))%>'> </asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported By">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChiefEngineerNameItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCHIEFENGINEERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                            <ItemTemplate>
                            <telerik:RadImageButton runat="server" AlternateText="Save" Image-Url="<%$ PhoenixTheme:images/download_1.png %>"
                                CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItem %>' ID="cmdDownload"
                                ToolTip="DownLoad">
                            </telerik:RadImageButton>
                                <asp:LinkButton runat="server" AlternateText="Excel" ID="cmdExcel"
                                    CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel">
                                    <span class="icon"><i class="far fa-file-excel"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Visible="false"
                                    CommandName="MAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="CmdMapping"
                                    ToolTip="Map To Work Order" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-map-marker"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server"
                                    CommandName="MAPPEDREPORT" CommandArgument='<%# Container.DataSetIndex %>' ID="CmdMapped"
                                    ToolTip="Mapped Work Order" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-check-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="false" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
<%--        </telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
