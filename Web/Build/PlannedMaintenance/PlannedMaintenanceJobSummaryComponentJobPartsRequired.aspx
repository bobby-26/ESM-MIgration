<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobSummaryComponentJobPartsRequired.aspx.cs" Inherits="PlannedMaintenanceJobSummaryComponentJobPartsRequired" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Parts</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRequiredParts.ClientID %>"));
               }, 150);
           }
           window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmgvRequiredParts" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenugvRequiredParts" runat="server" OnTabStripCommand="gvRequiredParts_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRequiredParts" runat="server" ShowFooter="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
               CellSpacing="0" GridLines="None" OnNeedDataSource="gvRequiredParts_NeedDataSource">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Part No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentJobLineID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPJOBLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPartNumber" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPartName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocation" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity in Location">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONROB","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantity" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <ItemStyle Wrap="False"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnit" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME","{0:n0}") %>'></telerik:RadLabel>
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
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
