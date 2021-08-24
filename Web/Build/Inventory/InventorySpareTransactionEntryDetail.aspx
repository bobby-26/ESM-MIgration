<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareTransactionEntryDetail.aspx.cs"
    Inherits="InventorySpareTransactionEntryDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item Transaction Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSpareInOut" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuInventoryStockInOut">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuInventoryStockInOut" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick"
                Visible="false" />
            <eluc:TabStrip ID="MenuInventoryStockInOut" runat="server" OnTabStripCommand="MenuInventoryStockInOut_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <br clear="all" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 12%">Item Number
                    </td>
                    <td colspan="2" style="width: 35%">
                        <span id="Span1">
                            <telerik:RadTextBox ID="txtItemNumber" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtItemName" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="200px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtItemId" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>Component
                    </td>
                    <td colspan="2">
                        <span id="spnPickListParentComponent">
                            <telerik:RadTextBox ID="txtComponentNumber" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="200px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentID" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input"
                                MaxLength="20" Width="10px">
                            </telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">Location
                    </td>
                    <td colspan="2">
                        <span id="Span2">
                            <telerik:RadTextBox ID="txtLocationCode" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtLocationName" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="200px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtLocationID" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <%-- Work Order--%>
                    </td>
                    <td colspan="2">
                        <eluc:UserControlHard ID="ddlWorkOrder" Visible="false" RenderMode="Lightweight" runat="server" CssClass="input " />
                    </td>
                    <td></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="6" style="width: 15%">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">Transaction Date
                    </td>
                    <td>
                        <eluc:Date ID="txtDispositionDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td style="width: 15%">Reported By
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReportedBy" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="input" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">Transaction Type
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlDispositionType" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="130px" />
                    </td>
                    <td style="width: 15%">Quantity
                    </td>
                    <td>
                        <eluc:Number ID="txtDispositionQuantity" RenderMode="Lightweight" ReadOnly="true"
                            runat="server" CssClass="input txtNumber" MaxLength="9" Width="120px" />
                    </td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <%--   Transaction Number--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTransactionNumber" RenderMode="Lightweight" Visible="false" runat="server" ReadOnly="true"
                            CssClass="input" MaxLength="10" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <%--    Reference--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDispositionReference" RenderMode="Lightweight" Visible="false" runat="server" CssClass="input"
                            MaxLength="10" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 20"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Order Number
                    </td>
                    <td>
                        <span id="spnPickListOrder">
                            <telerik:RadTextBox ID="txtOrderNumber" RenderMode="Lightweight" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="130px">
                            </telerik:RadTextBox>
                            <img id="imgOrder" runat="server" style="cursor: pointer; vertical-align: top; visibility: hidden"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListOrder', 'codehelp1', '', '../Common/CommonPickListOrder.aspx', true);" />
                            <telerik:RadTextBox ID="txtOrderName" RenderMode="Lightweight" runat="server" Visible="false" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="5px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOrderId" RenderMode="Lightweight" runat="server" CssClass="input" MaxLength="50" Width="10px"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td style="width: 15%">
                        <%-- Delivery Time(Days)--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDeliveryTime" Visible="false" runat="server" CssClass="input"
                            Width="60px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>&nbsp;
                    </td>
                    <td></td>
                    <td></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr style="width: 15%" valign="top">
                    <td colspan="6"></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuGridSpareInOut" runat="server" OnTabStripCommand="MenuGridSpareInOut_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSpareEntryDetail" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareEntryDetail" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvSpareEntryDetail_ItemCommand" OnSortCommand="gvSpareEntryDetail_SortCommand"
                OnNeedDataSource="gvSpareEntryDetail_NeedDataSource" AutoGenerateColumns="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMDISPOSITIONHEADERID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Transaction Date" AllowSorting="true" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpareItemDispositionHeaderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMDISPOSITIONHEADERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpareItemDispositionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMDISPOSITIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTransactionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transaction Type" AllowSorting="true" ShowSortIcon="true" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransactionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TRANSACTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purchase Price" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurchasePrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPRICE","{0:n2}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.WORKORDERTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported By" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.REPORTEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="30px" />
                            <ItemStyle Width="30px" Wrap="false" />
                            <ItemTemplate>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Transactions matching your search criteria"
                        PageSizeLabelText="Transactions per page:" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
