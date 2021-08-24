<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreWageTrackerTotal.aspx.cs"
    Inherits="CrewOffshoreWageTrackerTotal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Wage Tracker</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWageTracker" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <%-- <asp:GridView ID="gvWageTracker" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3"
                ShowHeader="true" ShowFooter="false" EnableViewState="false" OnRowDataBound="gvWageTracker_RowDataBound">
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWageTracker" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvWageTracker_NeedDataSource"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Revision No" Name="Revision" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="From">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfromdateitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="To">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltodateitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manning" ColumnGroupName="Revision" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmanigrevnoitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDMANNINGSCALEREVISIONNUMBER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget" ColumnGroupName="Revision" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblbudgetrevnoitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDBUDGETREVISIONNUMBER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="Rank">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrankitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Scale" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblownerscaleitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDOWNERSCALE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budgeted Wage" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblbudgetteditem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDBUDGETEDWAGE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="(Owner Scale * Budgeted Wage)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltotalitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDCALCULATED")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Overlap Wage" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                           <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverlapWageitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDOVERLAPWAGE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tank Clean Allowance" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                          <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltankcleanitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDTANKCLEANALLOWANCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP Allowance" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                           <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPAllowanceitem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDDPALLOWANCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Other Allowance" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                               <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOterAllowanceItem" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDOTHERALLOWANCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="310px" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
