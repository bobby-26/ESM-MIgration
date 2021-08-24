<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReceiptLineitemAdd.aspx.cs" Inherits="PurchaseReceiptLineitemAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PO Issued</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= rgvForm.ClientID %>"));
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
        <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <table style="width: 100%;">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AutoPostBack="True" AppendDataBoundItems="true" VesselsOnly="true" AssignedVessels="true" />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFormNo" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormNo" runat="server" MaxLength="20"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlTitle" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" MaxLength="20"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="ltrlStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStockType" AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                            <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFromDate" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlToDate" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" />
                </td>
            </tr>
        </table>
        <div></div>
        <eluc:TabStrip ID="MenuNewRequisition" runat="server" OnTabStripCommand="MenuNewRequisition_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator" runat="server" DecorationZoneID="rgvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvForm" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvForm_NeedDataSource" OnItemDataBound="rgvForm_ItemDataBound" OnItemCommand="rgvForm_ItemCommand" ShowFooter="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDVESSELID,FLDDTKEY,FLDORDERLINEID" TableLayout="Fixed">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Type" UniqueName="TYPE" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblEditOrderLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="P.O. NO" UniqueName="PO" AllowSorting="true" SortExpression="FLDFORMNO">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER" AllowSorting="true" SortExpression="FLDPARTNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME" AllowSorting="true" SortExpression="FLDNAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE" AllowSorting="true" SortExpression="FLDTITLE">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormTitle" runat="server"
                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString()%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF" AllowSorting="true" SortExpression="FLDMAKERREF">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblMakeRef" runat="server"
                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAKERREF").ToString()%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Ordered" UniqueName="ORDERED" ColumnGroupName="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="txtOrderedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Received" UniqueName="RECEIVED" ColumnGroupName="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="99px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECIEVEDQUANTITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtReceivedQty" runat="server" CssClass="input_mandatory" MaxLength="8"
                                Width="80px" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRECIEVEDQUANTITY")%>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Received" UniqueName="TOTALRECV" ColumnGroupName="Quantity" HeaderTooltip="Total Received across Receipt">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALRECEIVEDQTY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="txtTotalReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALRECEIVEDQTY") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Balance" UniqueName="BALANCE" ColumnGroupName="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBalanceQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEQUANTITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblEditBalanceQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEQUANTITY") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="ACTION" HeaderText="Action">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>' ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                            <%--<asp:ImageButton runat="server" AlternateText="Add" CommandName="ADD" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>' ImageUrl="<%$ PhoenixTheme:images/Add.png %>"
                                ID="cmdAdd" ToolTip="Add"></asp:ImageButton>--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave" ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
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
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>


