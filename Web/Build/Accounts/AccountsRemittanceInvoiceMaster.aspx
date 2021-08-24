<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceInvoiceMaster.aspx.cs"
    Inherits="AccountsRemittanceInvoiceMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="frmTitle" Text="Remittance Invoice List" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" Title="Remittance Invoice List"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 100%; border-left-width: 1px; border-left-style: solid; border-right-width: 1px; border-right-style: solid; border-bottom-width: 1px; border-bottom-style: solid; border-top-width: 1px; border-top-style: solid; margin-left: 0px;"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="Both">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                    Width="100%" CellPadding="3" OnItemCommand="gvVoucherDetails_ItemCommand" OnItemDataBound="gvVoucherDetails_ItemDataBound"
                    EnableHeaderContextMenu="true" GroupingEnabled="false" AllowSorting="true" EnableViewState="false"
                    OnNeedDataSource="gvVoucherDetails_NeedDataSource" OnSelectedIndexChanging="gvVoucherDetails_SelectedIndexChanging">
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDINVOICECODE">
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
                            <telerik:GridTemplateColumn HeaderText="Invoice Number" AllowSorting="true" SortExpression="FLDINVOICENUMBER" ShowSortIcon="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkInvoice" runat="server" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblBankInfoPageNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEBANKINFOPAGENUMBER") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Invoice Number">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblinvoicereferenceno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERREFERENCE") %>'>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Payable Amount">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblinvoicepayableamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment Voucher Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <eluc:TabStrip ID="MenuAdvancePayments" runat="server" OnTabStripCommand="MenuAdvancePayments_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAdvancePayment" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="false" AllowCustomPaging="false"
                    Width="100%" CellPadding="3" OnItemCommand="gvAdvancePayment_ItemCommand" OnItemDataBound="gvAdvancePayment_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="true"
                    AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvAdvancePayment_NeedDataSource">
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
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
                            <telerik:GridTemplateColumn HeaderText="Reference No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReferenceNo" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER")  %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PV Source">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payable Amount">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment Voucher Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
