<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPoolHistory.aspx.cs"
    Inherits="CrewPoolHistory" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pool History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>            
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPoolHistory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager> 
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPoolHistory,table1" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="CrewPoolChangeHistory" runat="server" OnTabStripCommand="CrewPoolChangeHistory_TabStripCommand"></eluc:TabStrip>
            <table id="table1" runat="server" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
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
            <eluc:TabStrip ID="PoolHistoryList" runat="server" OnTabStripCommand="PoolHistoryList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvPoolHistory" runat="server" Height="50%" EnableViewState ="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvPoolHistory_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvPoolHistory_ItemDataBound"
                OnItemCommand="gvPoolHistory_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />            
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pool From" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoolFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPOOLNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pool To" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoolTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPOOLNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transfer Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransferDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Changed By" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChangedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></telerik:RadLabel>
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
