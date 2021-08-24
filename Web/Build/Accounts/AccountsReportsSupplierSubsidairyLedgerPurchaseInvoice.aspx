<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx.cs" Inherits="AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ledger General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLedgerGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSubsidiaryLedger" runat="server" OnTabStripCommand="MenuSubsidiaryLedger_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td id="spnPickListSupplier">

                        <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="90px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="input_mandatory">
                        </telerik:RadTextBox>
                        <asp:ImageButton ID="btnPickSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="Menuledger" runat="server" OnTabStripCommand="Menuledger_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierLedger" Height="50%" runat="server" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvSupplierLedger_ItemDataBound" OnNeedDataSource="gvSupplierLedger_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Reference Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Entry Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankEntry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prime Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrimeAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAllocatedAmount" runat="server" Text="Total Allocated Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAllocatedAmount" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnAllocatedAmount" runat="server" Text="Unallocated Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUnAllocatedAmount" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
