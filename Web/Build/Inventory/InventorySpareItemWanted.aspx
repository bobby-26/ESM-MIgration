<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemWanted.aspx.cs"
    Inherits="InventorySpareItemWanted" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item Wanted</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight);
            splitter.set_width("100%");
            var grid = $find("gvStockItemWanted");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 140) + "px";
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
                <telerik:AjaxSetting AjaxControlID="gvStockItemWanted">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStockItemWanted" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
         <telerik:RadNotification ID="ucstatusnew" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
          <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 320px; width: 100%" frameborder="0"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="SpareItem_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStockItemWanted" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvStockItemWanted" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvStockItemWanted_NeedDataSource" OnUpdateCommand="gvStockItemWanted_UpdateCommand"
                        OnItemDataBound="gvStockItemWanted_ItemDataBound" OnItemCommand="gvStockItemWanted_ItemCommand" OnSortCommand="gvStockItemWanted_SortCommand"
                        Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" ImageUrl="<%$ PhoenixTheme:images/red.png%>" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="8%" ItemStyle-Width="8%" AllowSorting="true" SortExpression="FLDNUMBER">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="22%" ItemStyle-Width="22%" AllowSorting="true" SortExpression="FLDNAME">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lnkStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="20%" ItemStyle-Width="20%" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Maker" HeaderStyle-Width="20%" ItemStyle-Width="20%" AllowSorting="true" SortExpression="MAKER">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="6%" ItemStyle-Width="6%" AllowSorting="true" SortExpression="FLDQUANTITY">
                                    <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMinimumQuantity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREMINIMUM","{0:n0}") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCritical" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCRITICAL","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Wanted" HeaderStyle-Width="6%"  ItemStyle-Width="6%" AllowSorting="true" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWanted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWANTED","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtStockItemWantedQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWANTED","{0:n0}") %>'
                                            CssClass="gridinput" Width="60" MaxLength="9" IsPositive="true" IsInteger="true" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Unit" AllowSorting="true" HeaderStyle-Width="5%"  ItemStyle-Width="5%" SortExpression="FLDUNITNAME">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Stock Class" AllowSorting="true" HeaderStyle-Width="5%" ItemStyle-Width="5%" SortExpression="STOCKCLASSNAME">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.STOCKCLASSNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <table cellpadding="5" cellspacing="1">
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
        </div>
    </form>
</body>
</html>
