<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceBatchTaxandChargesList.aspx.cs"
    Inherits="AccountsAllotmentRemittanceBatchTaxandChargesList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
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
    <title>Tax and Charges</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>

<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlRemittance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlRemittance">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" TabStrip="true" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="Menusub" runat="server" OnTabStripCommand="Menusub_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>Batch No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Account Code</td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>Payment Mode</td>
                    <td>
                        <telerik:RadTextBox ID="txtPaymentMode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Payment Date</td>
                    <td>
                        <telerik:RadTextBox ID="txtpaydate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>No. of Remittance</td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfRemittance" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Remittance Amount(USD)</td>
                    <td>
                        <telerik:RadTextBox ID="txtAmountinUSD" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Exchange rate</td>
                    <td>
                        <eluc:Number ID="txtBankExchangerate" runat="server" DecimalPlace="18" Width="200px" IsPositive="true"
                            MaxLength="26" CssClass="input_mandatory" Text="" />
                    </td>
                    <td>Remittance Amount (<telerik:RadLabel ID="lblBankcurrency" runat="server" Text=""> </telerik:RadLabel>
                        )</td>
                    <td>
                        <telerik:RadTextBox ID="txtAmountinbankCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="4"><b>Bank Charges</b>
                    </td>
                </tr>
                <tr>
                    <td>Per Remittance</td>
                    <td>
                        <telerik:RadTextBox ID="txtPerRemittance" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Total Amount(USD)</td>
                    <td>
                        <telerik:RadTextBox ID="txtBankTotalchargesUSD" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Voucher No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherno" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Total Amount(<telerik:RadLabel ID="lblcurrency" runat="server" Text=""> </telerik:RadLabel>
                        )</td>
                    <td>
                        <telerik:RadTextBox ID="txtbanktotalinbankcur" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"><b>Total Charges</b>
                    </td>
                </tr>
                <tr>
                    <td>Total Amount(USD)</td>
                    <td>
                        <telerik:RadTextBox ID="txtTotalAmountinUSD" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>Total Amount(<telerik:RadLabel ID="lblBankcurrency2" runat="server" Text=""> </telerik:RadLabel>
                        )</td>
                    <td>
                        <telerik:RadTextBox ID="txtTotalAmountCurr" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid ID="gvTaxandCharges" CssClass="HeaderFreez" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false" AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvTaxandCharges_NeedDataSource">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHVESSELBANKCHARGEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No of Remittance">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoofremittance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFREMITANCE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Charges in USD">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankChargesinusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALBANKCHARGESINUSD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Account">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
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
