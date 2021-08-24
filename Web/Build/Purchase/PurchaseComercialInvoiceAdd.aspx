<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseComercialInvoiceAdd.aspx.cs" Inherits="PurchaseComercialInvoiceAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comercial Invoice</title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= rgvLine.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeliveryInstruction" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConsigny" runat="server" Text="Consignee"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiAddress runat="server" ID="ucAddrAgent"
                        Width="80%"  />
                    </td>
                </tr>
                <tr>
                    <td><telerik:RadLabel ID="lblInvoiceDate" runat="server" Text="Invoice Date"></telerik:RadLabel></td>
                    <td><eluc:Date ID="txtInvoicedate" runat="server" /></td>
                    <td><telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel></td>
                    <td><telerik:RadTextBox ID="txtInvoiceNumber" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
            </table>

                    
             <eluc:TabStrip ID="MenuPoAdd" runat="server" OnTabStripCommand="MenuPoAdd_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnItemCommand="rgvLine_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCOMRTIALINVOICEID" >
                        <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDPONUMBER" FieldAlias="Ref#" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDPONUMBER" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Description" UniqueName="NUMBER">
                                <ItemStyle Width="240px" />
                                <HeaderStyle Width="240px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="HS Code" UniqueName="NAME">
                                <ItemStyle Width="70px" />
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblHSCode" Text=""></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="QTY" UniqueName="QTY">
                                <ItemStyle Width="90px" HorizontalAlign="Right" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQTY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="UOM" UniqueName="UNIT">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUOM") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Unit Price (USD)" UniqueName="PRICE">
                                <ItemStyle Width="70px" HorizontalAlign="Right" />
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE","{0:n3}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Total Price (USD)" UniqueName="PRICEUSD">
                                <ItemStyle Width="60px" HorizontalAlign="Right" />
                                <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPRICE","{0:n3}") %>'></telerik:RadLabel>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
