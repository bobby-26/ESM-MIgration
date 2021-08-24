<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListOrder.aspx.cs"
    Inherits="CommonPickListOrder" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Forms</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvAddress");
            grid._gridDataDiv.style.height = (browserHeight - 170) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
         </script>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <br clear="all" />
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtOrderNumber" CssClass="input" runat="server" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" CssClass="input" ID="txtTitle" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" />
                        </td>
                    </tr>
                </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAddress" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvAddress_NeedDataSource" OnItemCommand="gvAddress_ItemCommand" >
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                         CommandItemDisplay="Top"  AutoGenerateColumns="false"  TableLayout="Fixed">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false" ShowPrintButton="false"  ShowRefreshButton="true" RefreshText="Search" />
                             <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblOrderNumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'  Visible="false"></telerik:RadLabel>
                                
                                <asp:LinkButton ID="lnkOrderNumber" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'
                                    Visible="true"></telerik:RadLabel>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                                PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                        </ClientSettings>
                    </telerik:RadGrid>
    </form>
</body>
</html>
