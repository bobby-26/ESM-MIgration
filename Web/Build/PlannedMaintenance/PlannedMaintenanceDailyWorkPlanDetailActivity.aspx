<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanDetailActivity.aspx.cs"
    Inherits="PlannedMaintenanceDailyWorkPlanDetailActivity" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
             function resize() {
                 setTimeout(function () {
                     TelerikGridResize($find("<%= gvActivty.ClientID %>"), null, 50);
                }, 200);
             }
             window.onload = window.onresize = resize;
             function pageLoad() {
                 resize();
             }
             function onClientTabSelecting(sender, args) {
                 resize();
             }
             function CloseUrlModelWindow() {
                 if (typeof parent.CloseUrlModelWindow == 'function') {
                     parent.CloseUrlModelWindow('gvActivity');
                 }
                 if (typeof parent.refreshScheduler == 'function') {
                     parent.refreshScheduler();
                 }
                 if (typeof parent.refreshDashboard == 'function') {
                     parent.refreshDashboard();
                 }
                 if (typeof parent.refresh == 'function') {
                     parent.refresh();
                 }
             }             
         </script>
        <style type="text/css">
            .rbVerticalList {
                margin: 0 7px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" OnClientTabSelecting="onClientTabSelecting">
                <Tabs>
                    <telerik:RadTab Text="Activities" Width="200px"></telerik:RadTab>
                    <telerik:RadTab Text="Frequently Used" Width="200px"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                <telerik:RadPageView runat="server" ID="RadPageView1">                    
                    <table border="0" style="width: 100%" runat="server" id="tblActivityAdd">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblElement" runat="server" Text="Process"></telerik:RadLabel>
                            </td>
                            <td style="border-bottom: 1px solid black;">
                                <telerik:RadCheckBoxList runat="server" ID="cblElement" Columns="3" Layout="Flow" AutoPostBack="true"
                                    OnSelectedIndexChanged="cblElement_SelectedIndexChanged">
                                </telerik:RadCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBoxList runat="server" ID="cblActivity" Columns="3" Layout="Flow"
                                    DataBindings-DataTextField="FLDNAME" DataBindings-DataValueField="FLDACTIVITYID" AppendDataBoundItems="true">
                                </telerik:RadCheckBoxList>                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnCreate" Text="Add" runat="server" OnClick="btnCreate_Click"></telerik:RadButton>
                            </td>
                        </tr>                        
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView2">
                    <eluc:TabStrip ID="MenuActivity" runat="server" OnTabStripCommand="MenuActivity_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    <telerik:RadGrid ID="gvActivty" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AllowFilteringByColumn="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvActivty_NeedDataSource" AllowMultiRowSelection="true" OnItemCommand="gvActivty_ItemCommand"
                        EnableHeaderContextMenu="true" EnableLinqExpressions="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDDAILYPLANACTIVITYID" ClientDataKeyNames="FLDDAILYPLANACTIVITYID">
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true" ItemStyle-HorizontalAlign="Center">
                                </telerik:GridClientSelectColumn>
                                <telerik:GridTemplateColumn HeaderText="Process" UniqueName="FLDELEMENTNAME"
                                    FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                                    <ItemTemplate>                                    
                                        <%# DataBinder.Eval(Container,"DataItem.FLDELEMENTNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Activity" UniqueName="FLDACTIVITYNAME"
                                    FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Start Time" AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="End Time" AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of Time" AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDNOOFTIME") %>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                            ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
