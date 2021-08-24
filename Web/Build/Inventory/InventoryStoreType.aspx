<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreType.aspx.cs"
    Inherits="InventoryStoreType" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvStoreType");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 100) + "px";
            var genPane = splitter.getPaneById("navigationPane");
            document.getElementById('ifMoreInfo').style.height = (genPane._contentElement.offsetHeight) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmStoreType" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStoreType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStoreType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuStoreType" runat="server" OnTabStripCommand="MenuStoreType_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 600px; width: 100%" frameborder="0"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                <eluc:TabStrip ID="MenuGridStoreType" runat="server" OnTabStripCommand="MenuGridStoreType_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreType" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreType_NeedDataSource" OnItemDataBound="gvStoreType_ItemDataBound" OnItemCommand="gvStoreType_ItemCommand" 
                    OnSortCommand="gvStoreType_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSTORETYPEID">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDSTORETYPENUMBER">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTORETYPENUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="35%" AllowSorting="true" SortExpression="FLDSTORETYPENAME">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStoreTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTORETYPEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkStoryTypeName" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTORETYPENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="UNITNAME" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.UNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Preferred Vendor" HeaderStyle-Width="35%" AllowSorting="true">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PREFERREDVENDOR") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="10%" />
                                <ItemStyle Width="10%" Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete"
                                        CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
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
