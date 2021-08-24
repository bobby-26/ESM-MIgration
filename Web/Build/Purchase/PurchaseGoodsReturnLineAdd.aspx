<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseGoodsReturnLineAdd.aspx.cs" Inherits="PurchaseGoodsReturnLineAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Goods Return Note </title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCommonItem" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <eluc:TabStrip ID="MenuItem" runat="server" OnTabStripCommand="MenuItem_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <div id="search">
                <table runat="server" style="margin-left: 20px">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblGRNNumber" runat="server" Text="GRN Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtGRNNumber" runat="server"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblform" runat="server" Text="Form No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtformno" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Form Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtOrder" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblvendor" runat="server" Text="Vendor"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtvedor" runat="server" Width="360px"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtvessel" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblstock" runat="server" Text="Stock Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtstocktype" runat="server"></telerik:RadLabel>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Invoice No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtInvoiceNo" runat="server"></telerik:RadLabel>
                        </td>

                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Invoice Supplier No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtSupplierReference" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Credit Note Register No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="txtCNRegisterNo" runat="server"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>

                    </tr>

                    <tr>

                        <td>
                            <telerik:RadLabel ID="lbComment" runat="server" Text="Comment"></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtComment" runat="server" Resize="Both" Width="360px" TextMode="MultiLine" Rows="3" CssClass="input"></telerik:RadTextBox>&nbsp;&nbsp;                 
                        </td>

                    </tr>


                </table>
            </div>
                <eluc:TabStrip ID="MenuReturn" runat="server" OnTabStripCommand="MenuReturn_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvItem" runat="server" Height="45%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" EnableViewState="false" GridLines="None" ShowHeader="true"
                OnNeedDataSource="gvItem_NeedDataSource" OnItemCommand="gvItem_ItemCommand" OnItemDataBound="gvItem_ItemDataBound">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Item Name">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblGRNLineId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRNLINEID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblGRNId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRNID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblStockItemName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Order Qty">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblOrderQty" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Received Qty">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblReceivedQty" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Return Qty">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblReturnQty" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRETURNEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtReturnQty" runat="server" DecimalPlace="2" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRETURNEDQUANTITY","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Accepted Qty">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAcceptedQty" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACCEPTEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtAcceptedQty" runat="server" DecimalPlace="2" Width="100%" IsPositive="true" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACCEPTEDQUANTITY","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                      <%--  <telerik:GridTemplateColumn HeaderText="Damaged Qty">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblDamagedQty" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDAMAGEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtDamagedQty" runat="server" DecimalPlace="2" Width="100%" IsPositive="true" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDAMAGEDQUANTITY","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                        <telerik:GridTemplateColumn HeaderText="Quoted Price">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblQuotedPrice" Text='<%#DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Discount (%)">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblDiscount" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDISCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Return Price">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblReturnPrice" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRETURNPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtReturnPrice" runat="server" DecimalPlace="2" Width="100%" IsPositive="true" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRETURNPRICE","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="center" Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblUnit" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUNITID","{0:n0}") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblUnitName" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Reason">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblReason" Text='<%#DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucReason" runat="server" CssClass="input" Width="100%" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKCODE") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="181" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Update" CommandName="UPDATE" ID="cmdSave" ToolTip="Update">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
