<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListType.aspx.cs"
    Inherits="CommonPickListType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Stock Class</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvStockClass.ClientID %>"));
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <br />
        <telerik:RadGrid ID="gvStockClass" runat="server" AutoGenerateColumns="False" CellPadding="3" AllowCustomPaging="true" AllowPaging="true"
            Font-Size="11px" ShowHeader="true" Width="100%" OnNeedDataSource="gvStockClass_NeedDataSource" OnSortCommand="gvStockClass_SortCommand" 
            OnItemCommand="gvStockClass_ItemCommand" OnItemDataBound="gvStockClass_ItemDataBound" AllowSorting="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Stock Class" AllowSorting="true" SortExpression="FLDHARDNAME">
                        <HeaderStyle Width="50%" />
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblStockClassId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'
                                Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkStockClass" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Short Name" AllowSorting="true" SortExpression="FLDSHORTNAME">
                        <HeaderStyle Width="50%" />
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblShortNameName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'
                                Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel runat="server" ID="lblShortName" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
