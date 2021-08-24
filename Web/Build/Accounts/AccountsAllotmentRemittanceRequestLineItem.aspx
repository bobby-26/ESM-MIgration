<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRequestLineItem.aspx.cs" Inherits="AccountsAllotmentRemittanceRequestLineItem" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittence Line Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlRemittance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlOrderForm">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceNumber" runat="server" Text="Remittance No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemittanceNumber" ReadOnly="true" Width="180px" runat="server"
                            CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtEmployeeId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlBankAccount ID="ddlBankAccount"
                            AppendDataBoundItems="true" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged"
                            AutoPostBack="true" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubAccountCode" Visible="false" runat="server"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtAccountId" Visible="false" runat="server"> </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCurrencyId" Visible="FALSE" ReadOnly="true" runat="server" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="input" Width="60px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <b>
                <telerik:RadLabel ID="lblPaymentVoucherDetails" runat="server" Text="Payment Voucher Details"></telerik:RadLabel></b>

            <telerik:RadGrid ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvVoucherDetails_RowCommand" OnItemDataBound="gvVoucherDetails_ItemDataBound"
                OnDeleteCommand="gvVoucherDetails_RowDeleting" AllowSorting="true" EnableViewState="false"
                OnSortCommand="gvVoucherDetails_Sorting" OnNeedDataSource="gvVoucherDetails_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPAYMENTVOUCHERID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                <Columns>
                    <telerik:GridTemplateColumn>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActiveYNHeader" runat="server"> Payment Voucher No.</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'
                                MaxLength="10" Visible="false"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'
                                MaxLength="10" Visible="false"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Crew Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Crew Name"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency Code">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text="Currency"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrencyShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActionHeader" runat="server"> Action </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <b>
                <telerik:RadLabel ID="lblAdvancePaymentVoucherDetails" runat="server" Text="Allotment Request Details"></telerik:RadLabel>
            </b>

            <telerik:RadGrid ID="gvAdvanceVoucherDetails" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvAdvanceVoucherDetails_NeedDataSource"
                OnItemCommand="gvAdvanceVoucherDetails_RowCommand" OnDeleteCommand="gvAdvanceVoucherDetails_RowDeleting"
                OnItemDataBound="gvAdvanceVoucherDetails_ItemDataBound" OnSortCommand="gvVoucherDetails_Sorting"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPAYMENTVOUCHERID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                <Columns>
                    <telerik:GridTemplateColumn>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActiveYNHeader" runat="server"> Request No.</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllotmentRequestNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTREQUESTNO") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtAllotmentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>' Visible="false"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblVesselNameHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Allotment Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblAllotmentTypeHeader" runat="server" Text="AllotmentType"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllotmentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblMonthHeader" runat="server" Text="Month"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblCurrencyALHeader" runat="server" Text="Currency"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrencyAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActiveYNHeader" runat="server">Payment Voucher No.</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtVoucherAId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>' Visible="false"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

