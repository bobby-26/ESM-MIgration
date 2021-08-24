<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceZeroPaymentVoucherMaster.aspx.cs"
    Inherits="AccountsInvoiceZeroPaymentVoucherMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 40);
                splitter.set_width("100%");
                var grid = $find("gvFormDetails");
                var contentPane = splitter.getPaneById("contentPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 200px; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVoucherDetails" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvVoucherDetails_ItemCommand" OnItemDataBound="gvVoucherDetails_ItemDataBound" EnableViewState="false"
                    OnSelectedCellChanging="gvVoucherDetails_SelectedIndexChanging" OnNeedDataSource="gvVoucherDetails_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDPAYMENTVOUCHERID">
                        <NoRecordsTemplate>
                            <table runat="server" width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                <HeaderStyle Width="165px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="Label1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSuppCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkVoucherId" runat="server" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER")  %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSubType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherTypes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherSource" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name">
                                <HeaderStyle Width="364px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency">
                                <HeaderStyle Width="133px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount">
                                <HeaderStyle Width="154px" HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                <HeaderStyle Width="181px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblJournalVoucherNumberItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOURNALVOUCHERNUMBER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblrepost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPOST") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="156px"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Allocate" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="ALLOCATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAllocate" ToolTip="Allocate"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Allocate" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                                        CommandName="REPOST" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdrepost" ToolTip="Repost"></asp:ImageButton>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
