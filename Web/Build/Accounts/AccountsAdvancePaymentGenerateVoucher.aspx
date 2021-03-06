<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentGenerateVoucher.aspx.cs" Inherits="Accounts_AccountsAdvancePaymentGenerateVoucher" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Payment Voucher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuOrderFormMain" Title="Generate Payment Voucher" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvInvoice" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid ID="gvInvoice" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" Height="89%" CellPadding="3" AllowSorting="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true"
            OnSortCommand="gvInvoice_Sorting" OnNeedDataSource="gvInvoice_NeedDataSource" OnSelectedIndexChanging="gvInvoice_SelectedIndexChanging">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDADVANCEPAYMENTID">
                <HeaderStyle HorizontalAlign="Center" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Supplier Code" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supplier Name">
                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSupplierId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reference No.">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselFleet" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEET") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENT") %>' CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency">
                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="100px" HorizontalAlign="Right" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Invoice Date">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE") %>'></telerik:RadLabel>
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
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" EnablePostBackOnRowClick="false" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
