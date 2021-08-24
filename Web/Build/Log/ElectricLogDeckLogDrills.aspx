<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogDeckLogDrills.aspx.cs" Inherits="Log_ElectricLogDeckLogDrills" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record of Drills, Training and Weekly Check of LSA / FFA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .hidden {
            display: none;
        }

        .center {
            text-align: center !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <%-- Splitter 1 --%>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Height="60%" Orientation="Vertical">
            <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">

                <telerik:RadGrid RenderMode="Lightweight" ID="gvContigancyDrills" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvContigancyDrills_NeedDataSource"
                    OnItemCommand="gvContigancyDrills_ItemCommand"
                    OnItemDataBound="gvContigancyDrills_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="A. Contingency Drills" Name="ContingencyDrills"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="ContingencyDrills">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="ContingencyDrills">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>

            <telerik:RadPane ID="listPane" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSOPEP" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvSOPEP_NeedDataSource"
                    OnItemCommand="gvSOPEP_ItemCommand"
                    OnItemDataBound="gvSOPEP_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="B. SOPEP / SMPEP Drills" Name="SOPEP"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="SOPEP">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="SOPEP">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Both"></telerik:RadSplitBar>

            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTrainingCarried" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvTrainingCarried_NeedDataSource"
                    OnItemCommand="gvTrainingCarried_ItemCommand"
                    OnItemDataBound="gvTrainingCarried_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="C. Training Carried out" Name="Training"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="Training">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="Training">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>


        </telerik:RadSplitter>
        <%-- Splitter 2 --%>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter2" runat="server" Width="100%" Height="60%" Orientation="Vertical">


            <telerik:RadPane ID="RadPane2" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvISPS" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvISPS_NeedDataSource"
                    OnItemCommand="gvISPS_ItemCommand"
                    OnItemDataBound="gvISPS_ItemDataBound" >

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="D. ISPS Drills" Name="ISPS"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="ISPS">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="ISPS">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar3" runat="server" CollapseMode="Both"></telerik:RadSplitBar>

            <telerik:RadPane ID="RadPane3" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvWeeklyCheckCondition" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvWeeklyCheckCondition_NeedDataSource"
                    OnItemCommand="gvWeeklyCheckCondition_ItemCommand"
                    OnItemDataBound="gvWeeklyCheckCondition_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="E. Weekly Check of Condition of LSA / FFA" Name="WeeklyConditon"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="WeeklyConditon">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="WeeklyConditon">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar4" runat="server" CollapseMode="Both"></telerik:RadSplitBar>


        </telerik:RadSplitter>
        <%-- Splitter 3 --%>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter3" runat="server" Width="100%" Height="60%" Orientation="Vertical">

            <telerik:RadPane ID="RadPane4" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvIBCIGC" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvIBCIGC_NeedDataSource"
                    OnItemCommand="gvIBCIGC_ItemCommand"
                    OnItemDataBound="gvIBCIGC_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="F. IBC / IGC Monthly Checks (For Chemical / Gas Tanker)" Name="IBC"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="IBC">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="IBC">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar5" runat="server" CollapseMode="Both"></telerik:RadSplitBar>

            <telerik:RadPane ID="RadPane5" runat="server" Scrolling="None">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvMonthlyCheck" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  Style="margin-bottom: 0px" EnableViewState="true"
                    OnNeedDataSource="gvMonthlyCheck_NeedDataSource"
                    OnItemCommand="gvMonthlyCheck_ItemCommand"
                    OnItemDataBound="gvMonthlyCheck_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderStyle-CssClass="center" HeaderText="G. Monthly Check For Condition of LSA / FFA" Name="MonthlyCheck"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" ColumnGroupName="MonthlyCheck">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Details" AllowSorting="true" ColumnGroupName="MonthlyCheck">
                                <HeaderStyle Width="300px" />
                                <ItemTemplate>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <telerik:RadSplitBar ID="RadSplitbar6" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            s

        </telerik:RadSplitter>

    </form>
</body>
</html>
