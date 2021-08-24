<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferErrorHandler.aspx.cs"
    Inherits="DataTransfer_DataTransferErrorHandler" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Synchornizer - Error Description</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <style type="text/css">
            .divFloatLeft
            {
                height: 53px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDataTransferVesselList" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatus" />
    <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
        DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divForm">
    </telerik:RadFormDecorator>
    <div id="divForm" runat="server" style="position: relative; z-index: 0;">
        <telerik:RadLabel ID="lblvesseldetails" runat="server" Style="text-align: left; margin: 0px;
            font-size: 09pt; font-weight: bold;">
        </telerik:RadLabel>
        <br />
        <br />
        <asp:Label ID="lbltitle" runat="server" Style="text-align: left; margin: 0px; font-size: 09pt;
            font-weight: bold;"></asp:Label>
        <asp:TextBox ID="lblsteps" runat="server" Text="" BorderStyle="None" BorderWidth="0px"
            Height="50px" ReadOnly="true" Rows="3" TextMode="MultiLine" Width="100%" Font-Bold="true"></asp:TextBox>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvImportFolder" Height="50px" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnNeedDataSource="gvImportFolder_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Folder Name">
                        <HeaderStyle Width="170px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFolderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLDERNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="false" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <asp:TextBox ID="lblFileList" runat="server" Text="" BorderStyle="None" BorderWidth="0px"
            Height="30px" ReadOnly="true" Rows="1" TextMode="MultiLine" Width="100%" Font-Bold="true"></asp:TextBox>
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvDeleteFileList" Height="50px" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnNeedDataSource="gvDeleteFileList_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="File Name">
                        <HeaderStyle Width="170px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFilename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNWANTEDFILES") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="false" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <asp:TextBox ID="txtMissingSeq" runat="server" Text="" BorderStyle="None" BorderWidth="0px"
            Height="30px" ReadOnly="true" Rows="1" TextMode="MultiLine" Width="100%" Font-Bold="true"></asp:TextBox>
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvMissingSeq" Height="50px" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnNeedDataSource="gvMissingSeq_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Missing Sequence No">
                        <HeaderStyle Width="170px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFilename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTMISSING") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="false" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <asp:TextBox ID="lbllastimport" runat="server" Text="" BorderStyle="None" BorderWidth="0px"
            ReadOnly="true" Rows="1" TextMode="MultiLine" Width="100%" Font-Bold="true"></asp:TextBox>
    </div>
    </form>
</body>
</html>
