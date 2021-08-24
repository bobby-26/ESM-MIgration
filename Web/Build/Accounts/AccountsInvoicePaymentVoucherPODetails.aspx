<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoicePaymentVoucherPODetails.aspx.cs"
    Inherits="AccountsInvoicePaymentVoucherPODetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Payment VoucherLine ItemDetails</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Invoice Payment Voucher Details" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                            Width="265px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 380px"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="82px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="180px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td style="width: 380px"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" Enabled="false" runat="server" CssClass="input readonlytextbox" />
                    </td>
                    <td style="width: 380px"></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="vertical-align: top">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoicePo" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnPreRender="gvInvoicePo_PreRender" ShowHeader="true" OnNeedDataSource="gvInvoicePo_NeedDataSource"
                            EnableViewState="false" OnRowCommand="gvInvoice_RowCommand" AllowSorting="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
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
                                    <telerik:GridTemplateColumn HeaderStyle-Width="12%" HeaderText="  Invoice Number /<br />
                                                Voucher Number&nbsp;">
                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                            /
                                            <br />
                                            <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEINVOICEVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Invoice Reference" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'
                                                Visible="false">
                                            </telerik:RadLabel>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="PO Number" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPoNumber" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO")  %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblInvoicePoId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Committed amount">
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCommittedAmtHeader" runat="server">
                                                Committed amount (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCommittedAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNTINREPORTCURRENCY","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Invoice difference" HeaderStyle-Width="10%">
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInvoiceDiffHeader" runat="server">
                                                Invoice difference (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceDiff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNTINREPORTCURRENCY","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Exchange difference" HeaderStyle-Width="10%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblExDiffHeader" runat="server">
                                                Exchange difference (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExDiffAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEDIFFERENCE","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Charged amount" HeaderStyle-Width="10%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblChargedAmtHeader" runat="server">
                                                Charged amount (<%=strReportCurrencyCode %>) &nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblChargedAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGEDAMOUNTINREPORTCURRENCY" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel amount" HeaderStyle-Width="10%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVESSELAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Payable" HeaderStyle-Width="8%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPayableAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPAYABLEAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
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
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table>
                <tr>
                    <th>
                        <telerik:RadLabel ID="lblPOReceiptDetails" runat="server" Text="PO Receipt Details"></telerik:RadLabel>
                    </th>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="vertical-align: top">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPOReceipt" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvPOReceipt_NeedDataSource">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Received Quantity" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQTY","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Received Remarks" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReceivedRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTREMARKS")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Last Updated By" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCommittedAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDUSERNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Last Update Date/ Time" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceDiff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
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
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
