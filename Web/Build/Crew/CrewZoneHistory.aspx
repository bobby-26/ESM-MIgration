<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewZoneHistory.aspx.cs"
    Inherits="CrewZoneHistory" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Zone History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>

    <form id="frmZoneHistory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="CrewZoneChangeHistory" runat="server" OnTabStripCommand="CrewZoneChangeHistory_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <table id="table1" runat="server" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="input_mandatory" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTransferDate" runat="server" Text="Transfer Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTransferDate" runat="server" Width="150px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="ZoneHistoryList" runat="server" OnTabStripCommand="ZoneHistoryList_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvZoneHistory" runat="server" Height="70%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvZoneHistory_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvZoneHistory_ItemDataBound"
                OnItemCommand="gvZoneHistory_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone From" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZoneFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDZONE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone To" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZoneTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWZONE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transfer Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransferDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSFERDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Changed" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateChanged" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Changed By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChangedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
