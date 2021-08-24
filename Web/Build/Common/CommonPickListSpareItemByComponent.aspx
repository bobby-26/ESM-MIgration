<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListSpareItemByComponent.aspx.cs"
    Inherits="CommonPickListSpareItemByComponent" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Item List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight -40);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 50);
            var grid = $find("gvStockItem");     
            //var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (browserHeight - 220) + "px";
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStockItem" UpdatePanelHeight="80%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvStockItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStockItem" UpdatePanelHeight="80%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="200">
                <eluc:TreeView runat="server" ID="tvwComponent" OnNodeClickEvent="tvwComponent_NodeClickEvent" ></eluc:TreeView>
                <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server">
                <table cellpadding="1" cellspacing="1" style="float: left; width: 100%;">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtNumberSearch" Width="90px" CssClass="input" MaxLength="13"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStockItemName" runat="server" Text="Stock Item Name"></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox runat="server" ID="txtStockItemNameSearch" Width="240px" CssClass="input" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMakerReference" runat="server" Text="Maker Reference"></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox runat="server" ID="txtMakerRef" CssClass="input" Width="90px" Text=""></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvStockItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="400px"
                                CellSpacing="0" GridLines="None" OnNeedDataSource="gvStockItem_NeedDataSource" OnItemCommand="gvStockItem_ItemCommand"
                                OnItemDataBound="gvStockItem_ItemDataBound" OnSortCommand="gvStockItem_SortCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID">
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="FLDNUMBER">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Stock Item Name" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDNAME">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblStockItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblIsInMarket" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISINMARKET") %>'></telerik:RadLabel>
                                                <u><asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="PICKLIST" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton></u>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Maker Reference" HeaderStyle-Width="50px" AllowSorting="true" SortExpression="FLDMAKERREFERENCE">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Drawing No/Position" HeaderStyle-Width="30px" AllowSorting="true" SortExpression="FLDDRAWINGNUMBERPOSITION">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblPosition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNUMBERPOSITION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Maker Name" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDMAKERNAME" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblMakerName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Quantity" AllowSorting="true" HeaderStyle-Width="45px" SortExpression="FLDQUANTITY" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                                        PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                                        <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red">Red Line item is not in Market. </telerik:RadLabel>
                        </td>
                    </tr>
                    </table>
                    
                            
                            <%--<table width="100%" border="0" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                            
                                    </td>
                                </tr>
                            </table>--%>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
