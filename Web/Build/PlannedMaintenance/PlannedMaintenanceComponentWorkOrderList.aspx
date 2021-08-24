<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentWorkOrderList.aspx.cs" Inherits="PlannedMaintenanceComponentWorkOrderList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                }, 200);
           }
           window.onresize = window.onload = Resize;
            
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvWorkOrder_ItemCommand"
                OnNeedDataSource="gvWorkOrder_NeedDataSource" OnPreRender="gvWorkOrder_PreRender" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvWorkOrder_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Work Order No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER">
                            <HeaderStyle Width="115px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNAME">
                            <HeaderStyle Width="218px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDWorkOrderID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER">
                            <HeaderStyle Width="115px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="64px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline">
                            <HeaderStyle Width="126px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Started">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStarted" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTARTEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompleted" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKORDERCOMPLETEDDATE")) %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
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
