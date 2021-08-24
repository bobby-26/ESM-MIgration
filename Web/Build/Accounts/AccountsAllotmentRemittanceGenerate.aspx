<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceGenerate.aspx.cs" Inherits="AccountsAllotmentRemittanceGenerate" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html>
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlorderform" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlorderform" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuOrderFormHeader" runat="server" OnTabStripCommand="MenuOrderFormHeader_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuOrderFormMain" Title="Generate Allotment Remittance" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalSelectedAmount" Text="Selected Vouchers Total Amount(USD)" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTotalSelectedAmount" runat="server" CssClass="readonlytextbox" Width="150px" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemDataBound="gvVoucherDetails_ItemDataBound"
                Width="100%" Height="75%" CellPadding="3" AllowSorting="true" OnNeedDataSource="gvVoucherDetails_NeedDataSource" OnSortCommand="gvVoucherDetails_Sorting"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvVoucherDetails_ItemCommand">
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
                        <telerik:GridTemplateColumn UniqueName="Listcheckbox">
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher No." AllowSorting="true" SortExpression="FLDPAYMENTVOUCHERNUMBER" ShowSortIcon="true" DataField="FLDPAYMENTVOUCHERNUMBER" UniqueName="FLDPAYMENTVOUCHERNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentVoucherID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherNumber" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSuppCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Amount(USD)">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
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

