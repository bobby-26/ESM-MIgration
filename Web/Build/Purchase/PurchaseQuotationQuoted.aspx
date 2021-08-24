<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationQuoted.aspx.cs" Inherits="PurchaseQuotationQuoted" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Quoted</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvForm");           
            grid._gridDataDiv.style.height = (browserHeight - 165) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
               <%-- <telerik:AjaxSetting AjaxControlID="rgvForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <table width="100%" border="0" cellpadding="10">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtVessel" ReadOnly="true" runat="server" Visible="false"></telerik:RadTextBox>
                    <eluc:Vessel runat="server" ID="ucVessel" AutoPostBack="True" AppendDataBoundItems="true" VesselsOnly="true" AssignedVessels="true" OnTextChangedEvent="ucVessel_TextChangedEvent" />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFormNo" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormNo" runat="server" MaxLength="20"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlTitle" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" MaxLength="20"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="ltrlStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStockType" OnTextChanged="ddlStockType_TextChanged" AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                            <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFromDate" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlToDate" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuNewRequisition" runat="server" OnTabStripCommand="MenuNewRequisition_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator"  runat="server" DecorationZoneID="rgvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvForm" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvForm_NeedDataSource" OnItemDataBound="rgvForm_ItemDataBound" OnSortCommand="rgvForm_SortCommand" OnItemCommand="rgvForm_ItemCommand" ShowFooter="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDVESSELID,FLDDTKEY" TableLayout="Fixed">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Type" UniqueName="TYPE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPriorityFlag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.PRIORITYFLAG") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER" AllowSorting="true" SortExpression="FLDFORMNO">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE" AllowSorting="true" SortExpression="FLDTITLE">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormTitle" runat="server"
                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString()%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Budget Code" UniqueName="BUDGETCODE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="VENDOR">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVendorName" runat="server"
                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDVENDORNAME").ToString()%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Committed (USD)" UniqueName="COMMITTED">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCommittedUsd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="ACTION" HeaderText="Action">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Form List" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' ImageUrl="<%$ PhoenixTheme:images/text-detail.png %>"
                                ID="cmdFormList" ToolTip="Form details"></asp:ImageButton>
                            <asp:ImageButton runat="server" AlternateText="Line Items" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' ImageUrl="<%$ PhoenixTheme:images/showlist.png %>"
                                ID="cmdLineItems" ToolTip="Line Items"></asp:ImageButton>
                            <asp:ImageButton runat="server" AlternateText="Quotation" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' ImageUrl="<%$ PhoenixTheme:images/te_quote.png %>"
                                ID="cmdQuotation" ToolTip="Quotation"></asp:ImageButton>
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
