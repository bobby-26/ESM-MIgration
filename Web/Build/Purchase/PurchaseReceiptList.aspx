<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReceiptList.aspx.cs" Inherits="PurchaseReceiptList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Receipt</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvReceipt.ClientID %>"));
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="panel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="80%" border="0">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" Width="300px"/>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblReceiptNo" Text="Receipt No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtReceiptNo" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblTitle" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucPort" runat="server" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblReceiptDate" Text="Receipt Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReceiptDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceiptStatus" runat="server" Text="Receipt Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlStatus" runat="server"
                            EmptyMessage="Type or select Status" MarkFirstMatch="true" AppendDataBoundItems="true" Width = "120px">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="Open" Selected="true" />
                            <telerik:RadComboBoxItem Value="1" Text="Completed" />
                            <telerik:RadComboBoxItem Value="2" Text="Canceled" />
                        </Items>
                    </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuReceipt" runat="server" OnTabStripCommand="MenuReceipt_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvReceipt" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true" OnDeleteCommand="gvReceipt_DeleteCommand"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvReceipt_NeedDataSource" OnItemDataBound="gvReceipt_ItemDataBound" OnSortCommand="gvReceipt_SortCommand" OnItemCommand="gvReceipt_ItemCommand" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRECEIPTID" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Receipt No." UniqueName="NUMBER" AllowSorting="true" SortExpression="FLDRECEIPTNO">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton RenderMode="Lightweight" ID="lblReceiptNo" runat="server" CommandArgument="RECEIPTNO" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTNO") %>'></asp:LinkButton>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblReceiptId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="STOCKTYPE" HeaderText="Stock Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblStockty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" UniqueName="FLDTITLE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Receipt Date" UniqueName="Date" AllowSorting="true" SortExpression="FLDRECEIPTDATE">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblReceiptDate" runat="server"
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIPTDATE").ToString())%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Port" UniqueName="FLDPORT">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPortId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTPORTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDRECEIPTSTATUS">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblConsignyName" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIPTSTATUSNAME").ToString()%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ACTION" HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTID") %>' ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Complete" CommandName="COMPLETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIPTID") %>' ImageUrl="<%$ PhoenixTheme:images/45.png %>"
                                    ID="cmdComplete" ToolTip="Complete"></asp:ImageButton>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="false" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
