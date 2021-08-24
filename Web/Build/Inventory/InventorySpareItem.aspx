<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItem.aspx.cs" Inherits="InventorySpareItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item</title>
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
            var grid = $find("gvStockItem");
            var contentPane = splitter.getPaneById("contentPane");
            if (grid._gridDataDiv != null)
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
            var genPane = splitter.getPaneById("navigationPane");
            document.getElementById('ifMoreInfo').style.height = (genPane._contentElement.offsetHeight) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStockItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStockItem" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuInventoryStockItem" runat="server" OnTabStripCommand="InventoryStockItem_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 600px; width: 100%" frameborder="0"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="StockItem_TabStripCommand"></eluc:TabStrip>
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStockItem" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvStockItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnDeleteCommand="gvStockItem_DeleteCommand" OnNeedDataSource="gvStockItem_NeedDataSource"
                    OnItemDataBound="gvStockItem_ItemDataBound" OnItemCommand="gvStockItem_ItemCommand" OnSortCommand="gvStockItem_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" ImageUrl="<%$ PhoenixTheme:images/red.png%>" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="8%" AllowSorting="true" SortExpression="FLDNUMBER">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="20%" AllowSorting="true" SortExpression="FLDNAME">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStockItemCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMinQtyFlage" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.MINQTYFLAGE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSpareClass" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPARECLASS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lnkStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Maker" HeaderStyle-Width="20%" AllowSorting="true" SortExpression="MAKER">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Maker Reference" HeaderStyle-Width="20%" AllowSorting="true">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCEFULLDETAILS") %>'></telerik:RadLabel>
                                    <%--<telerik:RadLabel ID="lblMarkerReferencFullDetails" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCEFULLDETAILS") %>'></telerik:RadLabel>--%>
                                    <%--<eluc:ToolTip ID="ucToolTipMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCEFULLDETAILS")%>'/>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Preferred Vendor" HeaderStyle-Width="20%" AllowSorting="true">
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PREFERREDVENDOR") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ROB" AllowSorting="true" HeaderStyle-Width="5%" SortExpression="FLDROB" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Wanted" AllowSorting="true" HeaderStyle-Width="5%" SortExpression="FLDWANTED" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWanted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWANTED","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="7%" />
                                <ItemStyle Width="7%" Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="cmdLocation" CommandName="LOCATION" runat="server" Width="20px" Height="20px"><span class="icon"><i class="fas fa-map-marker-alt"></i></span></asp:LinkButton>
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
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblROBislessthanMinimumLevel" runat="server" Text="* ROB is less than Minimum Level"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
