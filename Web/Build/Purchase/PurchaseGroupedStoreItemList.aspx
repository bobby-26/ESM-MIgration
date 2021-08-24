<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseGroupedStoreItemList.aspx.cs"
    Inherits="PurchaseGroupedStoreItemList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Grouped Stote PO Create </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmQuotationLineBulkSave" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStoreItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStoreItem" UpdatePanelHeight="70%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="menuSaveDetails" runat="server" OnTabStripCommand="menuSaveDetails_TabStripCommand" TabStrip="true"></eluc:TabStrip>


        <table style="color: blue">
            <tr>
                <td>Note:</td>
            </tr>
            <tr>
                <td>
                    <%--<telerik:RadLabel ID="lblClickHeader" runat="server" Text="1.Click '"></telerik:RadLabel>
                    <span class="icon"><i class="fas fa-edit"></i></span>
                    <telerik:RadLabel ID="lbltoaddnextdate" runat="server" Text="' to update the quantity."></telerik:RadLabel>--%>
                    1. Click '<span class="icon"><i class="fas fa-edit"></i></span>' to update the quantity.
                </td>
            </tr>
            <tr>
                <td>2.Click 'Send' for sending approval request.</td>
            </tr>
            <tr>
                <td>3.When you click 'Send', approval request email will be sent to the Tech Supnt for all type of stores (Deck,Engine,Galley,Safety).</td>
            </tr>
        </table>
        <asp:Panel ID="pnlSearchFilters" runat="server" GroupingText="Search">
            <table width="100%">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblItemNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" Width="90px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblItemName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="210px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselname" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlvessel" AutoPostBack="true"
                            CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlvessel_SelectedIndexChanged" />
                    </td>

                </tr>
            </table>
        </asp:Panel>
        <eluc:TabStrip ID="MenuStoreItemControl" runat="server" OnTabStripCommand="MenuStoreItemControl_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStoreItem" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreItem_NeedDataSource" Height="100%"
            OnItemDataBound="gvStoreItem_ItemDataBound"
            OnItemCommand="gvStoreItem_ItemCommand"
            OnUpdateCommand="gvStoreItem_UpdateCommand"
            OnSortCommand="gvStoreItem_SortCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDGROUPEDSTOREITEMID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDROWNUMBER" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="12%" AllowSorting="true" SortExpression="FLDNUMBER" ItemStyle-Width="12%">
                        <ItemTemplate>
                            <asp:Label ID="lblGroupedstoreId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPEDSTOREITEMID") %>'></asp:Label>
                            <asp:Label ID="lblStoreItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></asp:Label>
                            <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                            <asp:Label ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="38%" AllowSorting="true" SortExpression="FLDNAME" ItemStyle-Width="38%">
                        <ItemTemplate>
                            <asp:Label ID="lnkStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label><br />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="10%" AllowSorting="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtQuantityEdit" runat="server" Width="90px" CssClass="gridinput_mandatory"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' Mask="99999" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDUNITNAME" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblunitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label><br />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle Width="10%" Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
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

                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
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
