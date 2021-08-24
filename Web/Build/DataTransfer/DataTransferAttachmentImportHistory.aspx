<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferAttachmentImportHistory.aspx.cs"
    Inherits="DataTransferAttachmentImportHistory" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attachment Synchronizer - Import History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDataTransferImportHistory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuExportVesselList" runat="server" OnTabStripCommand="ExportVesselList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <%-- <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselImportList" Height="88%" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnNeedDataSource="gvVesselImportList_NeedDataSource" OnItemDataBound="gvVesselImportList_ItemDataBound"
            OnItemCommand="gvVesselImportList_RowCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />--%>
            <telerik:RadGrid ID="gvVesselImportList" RenderMode="Lightweight" runat="server" Height="92%" OnItemDataBound="gvVesselImportList_ItemDataBound"
                OnNeedDataSource="gvVesselImportList_NeedDataSource" OnItemCommand="gvVesselImportList_RowCommand"
                EnableViewState="false" ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File Name" AllowSorting="true" SortExpression="FLDFOLDERNAME">
                            <HeaderStyle Width="45%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lblFolderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLDERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Import Date">
                            <HeaderStyle Width="130px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lblImportDate" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTDATE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Files">
                            <HeaderStyle Width="85px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalFiles" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALFILES") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transfer Code">
                            <HeaderStyle Width="278px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransferCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSFERCODE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Action" AllowSorting="true"
                            SortExpression="">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Import" ID="cmdImport" ToolTip="Import"
                                    CommandName="IMPORT">
                                    <span class="icon"><i class="fas fa-file-download"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
