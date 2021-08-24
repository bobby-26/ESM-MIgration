<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceBatchInstructionList.aspx.cs"
    Inherits="AccountsAllotmentRemittanceBatchInstructionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvRemittence.ClientID %>"));
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .lblheader {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlRemittance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlRemittance" EnableAJAX="true">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" TabStrip="true" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="Menusub" runat="server" Title="Line Item" OnTabStripCommand="Menusub_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>Batch No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="99%"></telerik:RadTextBox>
                    </td>
                    <td>Account Code</td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="99%"></telerik:RadTextBox>
                    </td>
                    <td>Payment Mode</td>
                    <td>
                        <telerik:RadTextBox ID="txtPaymentMode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="99%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Payment Date</td>
                    <td>
                        <eluc:UserControlDate ID="txtpaydate" runat="server" CssClass="input_mandatory" />

                    </td>

                    <td>Tax & charges YN</td>
                    <td>
                        <telerik:RadCheckBox ID="ChkTaxchargesYN" runat="server" />
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid ID="gvRemittence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvRemittence_RowCommand" OnItemDataBound="gvRemittence_ItemDataBound"
                OnDeleteCommand="gvRemittence_RowDeleting" ShowFooter="false" OnNeedDataSource="gvRemittence_NeedDataSource"
                OnRowEditing="gvRemittence_RowEditing" EnableViewState="true" ShowHeader="true" AllowPaging="true" AllowCustomPaging="true"
                OnDetailTableDataBind="gvRemittence_DetailTableDataBind">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREMITTANCEIDLIST">
                    <HeaderStyle Width="102px" />
                    <DetailTables>
                        <telerik:GridTableView Width="100%" AutoGenerateColumns="false">
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Bank Details">
                                    <HeaderStyle Width="25%" />
                                    <ItemTemplate>
                                        <table style="width: 100%;">
                                            <tr valign="top">
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblBeneficiary" runat="server" Text="Beneficiary"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltBeneficiary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></asp:Label>
                                                </td>
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER") %>'></asp:Label>
                                                </td>
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblBank" runat="server" Text="Bank"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltBank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNAME") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr valign="top">

                                                <td class="lblheader">
                                                    <asp:Literal ID="lblSwiftCode" runat="server" Text="Swift Code"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltSwiftCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKSWIFTCODE") %>'></asp:Label>
                                                </td>
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblIFSCCode" runat="server" Text="IFSC Code"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltIFSCCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKIFSCCODE") %>'></asp:Label>
                                                </td>
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECURRENCY") %>'></asp:Label>
                                                </td>
                                            </tr>

                                            <tr valign="top">
                                                <td class="lblheader">
                                                    <asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal>
                                                </td>
                                                <td colspan="5">
                                                    <asp:Label ID="lbltAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKADDRESS") %>'></asp:Label>
                                                </td>
                                            </tr>

                                        </table>
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
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Voucher No.">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remittance No.">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblremittanceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEIDLIST")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRemittenceInstructionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEINSTRUCTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkRemittenceid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCENUMBERLIST")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAllotmentRemittanceBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTREMITTANCEBATCHID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Voucher No.">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblPaymentVoucherListHeader" runat="server" Text="Payment Voucher No's."></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentVoucherList" runat="server" ToolTip='<%#DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERLIST") %>' Text='<%#DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERLIST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemittanceamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Beneficiary">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBENEFICIARYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
