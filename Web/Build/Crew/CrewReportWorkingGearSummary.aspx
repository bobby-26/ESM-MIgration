<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportWorkingGearSummary.aspx.cs"
    Inherits="CrewReportWorkingGearSummary" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuSummary" runat="server" TabStrip="true" OnTabStripCommand="MenuSummary_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="4">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuSummaryItems" runat="server" TabStrip="false" OnTabStripCommand="MenuSummaryItems_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkingGearSummary" runat="server" Height="80%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvWorkingGearSummary_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemCommand="gvWorkingGearSummary_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                        <telerik:GridTemplateColumn HeaderText="Working Gear Item" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkingGearitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Opening Stock" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOpeningBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGBALANCE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Receipts" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRecepts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUED") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closing Stock" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosingSlock" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSINGSTOCK") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
