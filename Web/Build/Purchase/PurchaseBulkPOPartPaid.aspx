<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOPartPaid.aspx.cs" Inherits="PurchaseBulkPOPartPaid" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk PO Part Paid</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOrderPartPaid" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOrderNumber" runat="server" Text="Order Number :"></telerik:RadLabel>
                    &nbsp;&nbsp; &nbsp;
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtOrderNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvBulkPOPartPaid" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBulkPOPartPaid" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvBulkPOPartPaid_NeedDataSource" OnUpdateCommand="gvBulkPOPartPaid_UpdateCommand"
            OnItemDataBound="gvBulkPOPartPaid_ItemDataBound" OnItemCommand="gvBulkPOPartPaid_ItemCommand" OnSortCommand="gvBulkPOPartPaid_SortCommand"
            GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERID" ShowFooter="true">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderPartPaidId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERPARTPAIDID") %>'></asp:Label>
                            <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:Label>
                            <asp:LinkButton ID="lnkDescription" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblOrderPartPaidIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERPARTPAIDID") %>'></asp:Label>
                            <asp:Label ID="lblOrderIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                            <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' CssClass="input_mandatory" MaxLength="200">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="true" SortExpression="FLDAMOUNT">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Decimal ID="txtAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                Style="text-align: right" CssClass="input_mandatory" MaxLength="14"></eluc:Decimal>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Decimal ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="14"
                                Style="text-align: right"></eluc:Decimal>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Exchange Rate" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDEXCHANGERATE">
                        <ItemTemplate>
                            <asp:Label ID="lblExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher Number" HeaderStyle-Width="70px" AllowSorting="true">
                        <ItemTemplate>
                            <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher Date" HeaderStyle-Width="70px" AllowSorting="true">
                        <ItemTemplate>
                            <asp:Label ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="40px" />
                        <ItemStyle Width="40px" Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve">
                                <span class="icon"><i class="fas fa-award"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton runat="server" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                <span class="icon"><i class="fas fa-plus-square"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
