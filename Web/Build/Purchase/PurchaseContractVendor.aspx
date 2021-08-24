<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseContractVendor.aspx.cs"
    Inherits="Purchase_PurchaseContractVendor" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contract Vendor</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvLine");           
            grid._gridDataDiv.style.height = (browserHeight - 180) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseContractVendor" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPurchaseVendor" runat="server" OnTabStripCommand="MenuPurchaseVendor_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuPurchaseContractVendor" runat="server" OnTabStripCommand="PurchaseContractVendor_TabStripCommand">
                    </eluc:TabStrip>

        <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator"  runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnSortCommand="rgvLine_SortCommand" OnNeedDataSource="rgvLine_NeedDataSource"  
            OnItemDataBound="rgvLine_ItemDataBound" OnItemCommand="rgvLine_ItemCommand" ShowFooter="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDVENDORID,FLDCONTRACTVENDORID" TableLayout="Fixed">
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" FooterStyle-HorizontalAlign="Center">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                        <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Add" 
                                            CommandName="ADD" ID="cmdAdd"
                                            ToolTip="Add">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="Vendor">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVendorName" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <span id="spnPickListSupplierAdd">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSupplierCodeAdd" runat="server" Width="100px" Enabled="false"></telerik:RadTextBox>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSupplierNameEdit" runat="server" Width="200px" 
                                            Enabled="False"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnSupplierAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.ItemIndex%>" />
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSupplierIdAdd" runat="server" MaxLength="20" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    </span>
                        </FooterTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
