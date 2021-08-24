<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoicePaymentVoucherCreditNoteAdd.aspx.cs"
    Inherits="Accounts_AccountsInvoicePaymentVoucherCreditNoteAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Note Payment Vourcher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Credit Note Payment Vourcher" ShowMenu="false" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false"/>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                            Width="240px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text="Voucher Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherDate" runat="server" CssClass="readonlytextbox" MaxLength="50"
                            ReadOnly="true" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="240px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" Enabled="false" runat="server" CssClass="input readonlytextbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" MaxLength="50" ReadOnly="true" Width="120px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                        <%--       <ajaxtoolkit:maskededitextender id="MaskNumber" runat="server" targetcontrolid="txtAmount"
                                    mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                            </ajaxtoolkit:maskededitextender>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditNotes" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvCreditNotes_ItemCommand" OnItemDataBound="gvCreditNotes_ItemDataBound"
                            OnNeedDataSource="gvCreditNotes_NeedDataSource"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true">
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
                                    <telerik:GridTemplateColumn HeaderText="Cno Register No" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCreditNoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITNOTEID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCnoRegisterNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vendor Credit Note No" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="VendorCreditNote" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Credit Note Voucher No" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCreditVourcherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Original Amount" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Remaining Balance" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount To be used" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotalUtilized" runat="server" Text=''></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtAmountUtilized" runat="server" CssClass="input_mandatory" Width="99%"></telerik:RadTextBox>
                         <%--                   <ajaxtoolkit:maskededitextender id="MaskAmount" runat="server" targetcontrolid="txtAmountUtilized"
                                                mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                                            </ajaxtoolkit:maskededitextender>--%>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="8%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="SAVE" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                                ToolTip="Save"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
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
