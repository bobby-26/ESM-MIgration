<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx.cs"
    Inherits="AccountsReportsSupplierSubsidairyLedgerBankPayment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="50%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSubsidiaryLedger" runat="server" OnTabStripCommand="MenuSubsidiaryLedger_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table style="height: 10px; width: 70%">
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
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierLedger" Height="48%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvSupplierLedger_ItemDataBound" OnNeedDataSource="gvSupplierLedger_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvSupplierLedger_ItemCommand">
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
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <HeaderStyle Width="7%" Height="10px" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="InVoice Reference Number">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO ") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phoenix Posted no">
                            <HeaderStyle Width="14%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhoenixPosted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Number">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Entry Number">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankEntry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prime Amount">
                            <HeaderStyle Width="13%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrimeAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="9%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
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
            <br />
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="25%">
                        <telerik:RadLabel Literal ID="lblAllocatedAmount" runat="server" Text="Total Allocated Amount"></telerik:RadLabel>
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
            <br />
            <b>Unaccounted Registered Invoice List</b>
            <eluc:TabStrip ID="MenuledgerUnaccount" runat="server" OnTabStripCommand="MenuledgerUnaccount_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvUnaccounted" Height="42%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvUnaccounted_NeedDataSource" OnItemCommand="gvUnaccounted_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Reference Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phoenix Invoice no">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhoenixPosted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
