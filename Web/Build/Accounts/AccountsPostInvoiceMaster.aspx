<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPostInvoiceMaster.aspx.cs"
    Inherits="AccountsPostInvoiceMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
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
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%-- <asp:Button ID="cmdHiddenSubmit" runat="server" Visible="false" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" /> --%>
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 100%; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoice" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvInvoice_ItemCommand" OnItemDataBound="gvInvoice_ItemDataBound" OnNeedDataSource="gvInvoice_NeedDataSource"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                            <telerik:GridTemplateColumn HeaderText="State" AllowSorting="true">
                                <HeaderStyle Width="51px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrioritySupplier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITYSUPPLIER") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:ImageButton runat="server" AlternateText="SUPPLIERMISMATCH" ImageUrl="<%$ PhoenixTheme:images/suppliercode_mismatch.png %>"
                                        CommandName="SUPPLIERMISMATCH" CommandArgument="<%# Container.DataSetIndex %>"
                                        ID="imbSupplierMismatch" Enabled="false" ToolTip="Supplier Mismatch" Visible="false"></asp:ImageButton>
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="imbCurrencyMismatch" runat="server" AlternateText="CURRENCYMISMATCH"
                                        CommandArgument="<%# Container.DataSetIndex %>" CommandName="CURRENCYMISMATCH"
                                        ImageUrl="<%$ PhoenixTheme:images/currency_mismatch.png%>" Enabled="false" ToolTip="Currency Mismatch"
                                        Visible="false" />
                                    <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="imbMultiplePOs" runat="server" AlternateText="MULTIPLEPOS" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="MULTIPLEPOS" ImageUrl="<%$ PhoenixTheme:images/multiple_po.png%>"
                                        Enabled="false" ToolTip="Multiple PO's" Visible="false" />
                                    <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="imbPriorityInvoice" runat="server" AlternateText="PRIORITYINVOICE"
                                        CommandArgument="<%# Container.DataSetIndex %>" CommandName="PRIORITYINVOICE"
                                        ImageUrl="<%$ PhoenixTheme:images/priority_invoice.png%>" Enabled="false" ToolTip="Priority Invoice"
                                        Visible="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Code" AllowSorting="true">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBackToBack" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name" AllowSorting="true">
                                <HeaderStyle Width="210px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vendor Invoice Number" AllowSorting="true" SortExpression="FLDSTATENAME">
                                <HeaderStyle Width="120px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkInvoiceRef" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Date" AllowSorting="true">
                                <HeaderStyle Width="83px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICERECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Amount" AllowSorting="true">
                                <HeaderStyle Width="67px" HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText=" Currency Code" AllowSorting="true">
                                <HeaderStyle Width="71px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Status" AllowSorting="true">
                                <HeaderStyle Width="117px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSCHANGEINFO") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSCHANGEINFO")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Register Number" UniqueName="InvoiceCode" AllowSorting="true">
                                <HeaderStyle Width="87px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkInvoice" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></asp:LinkButton>
                                    <telerik:RadTextBox ID="txtInvoiceCode" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                        Visible="false">
                                    </telerik:RadTextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Date" AllowSorting="true">
                                <HeaderStyle Width="63px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice PIC" AllowSorting="true">
                                <HeaderStyle Width="62px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="108%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                        CommandName="INFO" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdMoreInfo"
                                        ToolTip="More Info"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="imgAttachment" runat="server" AlternateText="Attachment" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png%>"
                                        ToolTip="Attachment" />
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
