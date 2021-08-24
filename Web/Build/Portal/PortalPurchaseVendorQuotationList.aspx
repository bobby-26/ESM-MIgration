<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalPurchaseVendorQuotationList.aspx.cs" Inherits="PortalPurchaseVendorQuotationList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forms</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    <script type="text/javascript">
             function PaneResized(sender, args) {
                 
                 var browserHeight = $telerik.$(window).height();
                 var grid = $find("rgvLine");
                 grid._gridDataDiv.style.height = (browserHeight - 150) + "px";
             }
            function pageLoad() {
                PaneResized();
            }
        </script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuMain">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuMain" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <table style="width:100%;">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:ucVessel ID="ucVessel" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="ucVessel_TextChanged" Width="100%"/>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormNo" runat="server" Text="Form Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormNO" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuFilter" runat="server" OnTabStripCommand="MenuFilter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemCommand="rgvLine_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" CommandItemDisplay="None" DataKeyNames="FLDORDERID" TableLayout="Fixed">
                
                <Columns>
                <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="VESSEL">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Form Number" UniqueName="NUMBER">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkFormNumberName" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE">
                    <ItemTemplate>
                        <telerik:RadLabel RenderMode="Lightweight" ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Stock Type" UniqueName="STOCKTYPE">
                    <ItemTemplate>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Quoted" UniqueName="QUOTED">
                    <ItemTemplate>
                        <telerik:RadLabel RenderMode="Lightweight" ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONSTATUS") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                    AllowActiveRowCycle="true" MoveDownKey="DownArrow" MoveUpKey="UpArrow"></KeyboardNavigationSettings>
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
