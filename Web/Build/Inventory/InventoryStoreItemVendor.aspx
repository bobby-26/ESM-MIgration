<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemVendor.aspx.cs"
    Inherits="InventoryStoreItemVendor" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item Vendors</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvVendor");           
            grid._gridDataDiv.style.height = (browserHeight - 122) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuGridStoreItemVendor" runat="server" OnTabStripCommand="StoreItemVendor_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVendor" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnSortCommand="gvVendor_SortCommand"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvVendor_DeleteCommand" OnNeedDataSource="gvVendor_NeedDataSource" AutoGenerateColumns="false" Width="100%"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vendor Code" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDVENDORID">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemVendorId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMVENDORID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVendorCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="QA Grading">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQaGrading" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQAGRADING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Purchased Price" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkStockItemPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form No">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblContractId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Placed" ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlacedOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Ordered Date">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastOrderedDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received">
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemPriceExpires" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
