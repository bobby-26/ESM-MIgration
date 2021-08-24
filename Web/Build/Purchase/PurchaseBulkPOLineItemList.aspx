<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOLineItemList.aspx.cs" Inherits="PurchaseBulkPOLineItemList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Line items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvBulkPOLineItem");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
            var genPane = splitter.getPaneById("navigationPane");
            document.getElementById('ifMoreInfo').style.height = (genPane._contentElement.offsetHeight) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvBulkPOLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvBulkPOLineItem" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuBulkPO" runat="server" OnTabStripCommand="MenuBulkPO_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 600px; width: 100%" frameborder="0"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                <eluc:TabStrip ID="MenuBulkPurchase" runat="server" OnTabStripCommand="MenuBulkPurchase_TabStripCommand"></eluc:TabStrip>
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvBulkPOLineItem" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvBulkPOLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvBulkPOLineItem_NeedDataSource"
                    OnItemDataBound="gvBulkPOLineItem_ItemDataBound" OnItemCommand="gvBulkPOLineItem_ItemCommand" OnSortCommand="gvBulkPOLineItem_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDLINEITEMID">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="240px" AllowSorting="true" SortExpression="FLDLINEITEMNAME">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkItemName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="90px" AllowSorting="true" SortExpression="FLDLINEITEMNUMBER">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNUMBER") %>'></asp:Label>
                                    <asp:Label ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDBUDGETCODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="70px" AllowSorting="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Ordered Qty" HeaderStyle-Width="90px" AllowSorting="true"  ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Qty" AllowSorting="true" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Price" AllowSorting="true" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Amount" AllowSorting="true" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="40px" />
                                <ItemStyle Width="40px" Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
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
                    <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
