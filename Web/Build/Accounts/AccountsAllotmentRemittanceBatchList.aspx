<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceBatchList.aspx.cs"
    Inherits="AccountsAllotmentRemittanceBatchList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlRemittance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlRemittance" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvRemittenceBatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="93%" CellPadding="3" OnItemCommand="gvRemittenceBatch_RowCommand" OnItemDataBound="gvRemittenceBatch_ItemDataBound"
                 OnDeleteCommand="gvRemittenceBatch_RowDeleting" OnNeedDataSource="gvRemittenceBatch_NeedDataSource"
                AllowSorting="true" EnableViewState="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnSortCommand="gvRemittenceBatch_Sorting">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBATCHID">
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
                    <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Batch No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDBATCHNUMBER" DataField="FLDBATCHNUMBER" UniqueName="FLDBATCHNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkBatchId" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem%>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNUMBER")  %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Payment Date" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:UserControlDate ID="txtPaymentDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE","{0:dd/MMM/yyyy}") %>'
                                CssClass="input_mandatory" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Payment Mode" HeaderStyle-Width="15%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Account Code">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNT") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBankchargeYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXANDCHARGESYN") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBankchargeVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKCHARGESVOUCHERID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="LINEITEM" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            <telerik:RadLabel ID="lblFileName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'> </telerik:RadLabel>
                            <telerik:RadLabel ID="lblVoucherid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblIsmodified" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISBATCHMODIFIED") %>'></telerik:RadLabel>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="APPROVE" ID="cmdPost" ToolTip="Post">
                                <span class="icon"><i class="fas fa-check-circle-completed"></i></span>
                                </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Bank Upload" CommandName="BANKUPLOAD" ID="imgUpload" ToolTip="Bank Upload">
                                <span class="icon"><i class="fas fa-upload"></i></span>
                                </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Bank Charges" CommandName="TAXANDCHARGES" ID="cmdBankCharges" ToolTip="Tax and Charges" Visible="false">
                                <span class="icon"><i class="fas fa-refund-letter"></i></span>
                                </asp:LinkButton>
                            <asp:HyperLink ID="lnkfilename" Target="_blank" Text="Bank download" runat="server"
                                Width="90" Visible="false" Height="14px" ToolTip="Download File">
                                <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH") %>'></telerik:RadLabel>
                            </asp:HyperLink>

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
