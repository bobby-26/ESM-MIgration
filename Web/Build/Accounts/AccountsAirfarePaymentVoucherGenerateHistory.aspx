<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfarePaymentVoucherGenerateHistory.aspx.cs" Inherits="AccountsAirfarePaymentVoucherGenerateHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuHistoryMain" runat="server" OnTabStripCommand="MenuHistoryMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHistoryType" runat="server" Text="History Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblHistoryType" runat="server" Layout="Flow" Columns="6" Direction="Horizontal" AppendDataBoundItems="false"
                            AutoPostBack="true" OnSelectedIndexChanged="ReBindData" Enabled="true">
                            <Items>
                                <telerik:ButtonListItem Value="VESSEL CHARGEABLE" Text="Vessel Chargeable" Selected="true" />
                                <telerik:ButtonListItem Value="ACCOUNT" Text="Vessel Account" />
                                <telerik:ButtonListItem Value="BUDGET CODE" Text="Budget Code" />
                                <telerik:ButtonListItem Value="OWNER BUDGET CODE" Text="Owner Budget Code" />
                                <telerik:ButtonListItem Value="CHARGEABLE AMOUNT" Text="Chargeable Amount" />
                                <telerik:ButtonListItem Value="PAYABLE AMOUNT" Text="Payable Amount" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuHistory" runat="server" OnTabStripCommand="MenuHistory_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoiceHistory" Height="80%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvInvoiceHistory_ItemCommand" OnItemDataBound="gvInvoiceHistory_ItemDataBound1"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvInvoiceHistory_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date/Time of Change">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPDATEDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Change">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Name">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Field">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELD")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Old Value">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOldValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSVALUE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Value">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Procedure Used">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcedureUsed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURENAME") %>'></telerik:RadLabel>
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
