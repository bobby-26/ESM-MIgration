<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReceiptAdd.aspx.cs" Inherits="PurchaseReceiptAdd" %>

<!DOCTYPE html>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Receipt Add</title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvRLine.ClientID %>"));
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReceiptLineItem" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <eluc:TabStrip ID="MenuReceiptLineItemGeneral" runat="server" OnTabStripCommand="MenuReceiptLineItemGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" Width="150px" Enabled="false" CssClass="input_mandatory"/>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblReceiptNo" Text="Receipt No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtReceiptNo" runat="server" MaxLength="20" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblTitle" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" MaxLength="20" Width="180px"  CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="ltrlStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStockType" AutoPostBack="true" EnableLoadOnDemand="true" CssClass="input_mandatory">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                                <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                                <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:MultiPort ID="ucPort" runat="server" Width="300px" AppendDataBoundItems="true" CssClass="input_mandatory"/>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblReceiptDate" Text="Receipt Date" ></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReceiptDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" TextMode="MultiLine" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuReceiptLineItem" runat="server" OnTabStripCommand="MenuReceiptLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false"
                OnNeedDataSource="gvRLine_NeedDataSource" OnItemDataBound="gvRLine_ItemDataBound" OnItemCommand="gvRLine_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDLINEITEMID">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDORDERFORMNO" FieldAlias="Ref#" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDORDERFORMNO" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="PO.No." UniqueName="PURCHASEORDER">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPOnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumeber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="lbllineitemid" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>' Visible="false"></telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ordered" UniqueName="QTY" ColumnGroupName="Quantity">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblOQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received" UniqueName="RECVDQTY" ColumnGroupName="Quantity">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblRQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECIEVEDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balanced" UniqueName="BALQTY" ColumnGroupName="Quantity">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblBQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEQUANTITY") %>'></telerik:RadLabel>
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
