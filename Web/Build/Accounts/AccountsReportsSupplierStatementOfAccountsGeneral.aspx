<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsSupplierStatementOfAccountsGeneral.aspx.cs" Inherits="AccountsReportsSupplierStatementOfAccountsGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSubsidiaryLedger" runat="server" OnTabStripCommand="MenuSubsidiaryLedger_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"></eluc:TabStrip>
            <table style="width: 60%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
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
                        <telerik:RadLabel ID="lblDate" runat="server" Text="As On Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
                    <td>
                        <eluc:UserControlDate ID="ucDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
                    <td>
                        <eluc:Currency runat="server" Width="135px" ID="ddlCurrencyCode" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" Width="135px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="Menuledger" runat="server" OnTabStripCommand="Menuledger_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierLedger" Height="51%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvSupplierLedger_ItemCommand" OnNeedDataSource="gvSupplierLedger_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher Number">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'
                                    CommandName="EDIT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblVoucherType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERTYPEID ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Debit">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDebit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRPRIMEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Credit">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCredit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCRPRIMEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Net Balance" />
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="FLDPRIMEAMOUNT" Aggregate="Sum" DataFormatString="{0:n}" HeaderText="Balance" FooterText=" "></telerik:GridBoundColumn>
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


            <table cellpadding="2" cellspacing="1" style="width: 100%" border="1">
                <tr>
                    <th><=30 Days</th>
                    <th><=60 Days</th>
                    <th><=90 Days</th>
                    <th>>90 Days</th>
                    <th>Unallocated</th>
                    <th>Net Balance</th>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrlessThirty" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrlessSixty" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrlessNinety" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrmoreNinety" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrUnallocated" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblstrAmountTotal" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
