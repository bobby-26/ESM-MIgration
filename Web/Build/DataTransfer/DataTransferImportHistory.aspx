<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferImportHistory.aspx.cs"
    Inherits="Registers_DataTransferImportHistory" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Synchronizer - Import History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
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
        <eluc:Title runat="server" ID="ucTitle" Text="Data Synchronizer - Import History"
            Visible="false" />
        <eluc:TabStrip ID="MenuExportVesselList" runat="server" OnTabStripCommand="ExportVesselList_TabStripCommand"
            Visible="true" TabStrip="true"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselImportList" Height="88%" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnNeedDataSource="gvVesselImportList_NeedDataSource" OnItemDataBound="gvVesselImportList_ItemDataBound"
            OnItemCommand="gvVesselImportList_RowCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
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
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"
            Visible="false" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
