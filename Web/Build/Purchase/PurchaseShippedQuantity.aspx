<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseShippedQuantity.aspx.cs" Inherits="PurchaseShippedQuantity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shipped Quantity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table width="50%" border="0" cellpadding="1">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblShippeddate" runat="server" Text="Shipped Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="ucShippeddate" />
                </td>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblAttachment" runat="server" Text="Attachment"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="txtFileUpload" runat="server" Width="270px" />
                        <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                            Height="14px" ToolTip="Download File" Visible="false" ForeColor="Blue">
                        </asp:HyperLink>
                    </td>
                </tr>
            </tr>
        </table>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="79%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierlist" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvSupplierlist_ItemCommand" height="100%"  OnItemDataBound="gvSupplierlist_ItemDataBound" OnNeedDataSource="gvSupplierlist_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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
                        <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SERIALNO">
                            <HeaderStyle Width="40px" />
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMakerRef" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Drawing No" UniqueName="DRAWINGNO">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDrawingNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDRAWINGNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Stock Item Name" UniqueName="NAME">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorderlineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPartId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton><br />
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblIsFormNotes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTES") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsItemDetails" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILFLAGE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsContract" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTEXISTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Qty" UniqueName="ORDERQTY">
                            <HeaderStyle Width="90px" HorizontalAlign="Right" />
                            <ItemStyle Width="90px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderQty" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                        <eluc:Decimal ID="txtOrderQtyEdit" DecimalPlace="0" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>' Width="70px" />
                                    </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Shipped Qty" UniqueName="SHIPPEDQTY">
                            <HeaderStyle Width="90px" HorizontalAlign="Right" />
                            <ItemStyle Width="90px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblshippedQty" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSHIPPEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtshippedQtyEdit" DecimalPlace="0" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSHIPPEDQUANTITY","{0:n0}") %>' Width="70px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                        PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
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
