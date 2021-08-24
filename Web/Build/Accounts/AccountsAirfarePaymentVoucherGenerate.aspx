<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfarePaymentVoucherGenerate.aspx.cs"
    Inherits="Accounts_AccountsAirfarePaymentVoucherGenerate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Generate Payment Voucher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPVGenerate" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Generate Payment Voucher" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuPVGenerate" runat="server" OnTabStripCommand="MenuPVGenerate_TabStripCommand" Title="Generate Payment Voucher"></eluc:TabStrip>
            <table id="tblCourse" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" OnTextChanged="txtSupplierId_TextChanged"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtToDate" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>--%>
                        <eluc:UserControlDate ID="txtToDate" runat="server" CssClass="input_mandatory" ReadOnly="false" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" OnTextChangedEvent="CreditNote_SetExchangeRate" />
                        </td>
                    </tr>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankDetails" runat="server" Text="Supplier Banking Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBank">
                            <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtBankID" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuList" runat="server" OnTabStripCommand="MenuList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPVGenerate" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Height="68%" Width="100%" CellPadding="3" OnItemCommand="gvPVGenerate_ItemCommand" OnItemDataBound="gvPVGenerate_ItemDataBound" ShowFooter="false" AllowPaging="true" AllowCustomPaging="true"
                ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvPVGenerate_NeedDataSource">
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
                        <telerik:GridTemplateColumn HeaderText="Invoice Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInvoiceNoHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdInoiceNo" OnClick="cmdSearch_Click" CommandName="FLDINVOICENUMBER"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lnkInoiceNoHeader" runat="server">Invoice Number&nbsp;</telerik:RadLabel>
                                <img id="FLDINVOICENUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInoiceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charged Amount">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGEABLEAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount Payable">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmountPayable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYABLEAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger Name">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassengerName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flight Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlightDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFLIGHTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sector">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel/Non Vessel Account">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code/Sub Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Paying Company">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPayingCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYINGCOMPANYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cancelled Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCancelledYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="lblRemarksYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblRemarksDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSDETAILS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREMARKSDETAILS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKSDETAILS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSDETAILS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Remove" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Remove"></asp:ImageButton>
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
