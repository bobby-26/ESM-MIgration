<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSpareItemSearchAcrossVessel.aspx.cs"
    Inherits="Purchase_PurchaseSpareItemSearchAcrossVessel" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item Search By Reference Number </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseSpareItemSearch" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSpareItemList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemList" UpdatePanelHeight="95%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:TabStrip ID="MenuSpareItemList" runat="server" TabStrip="false" OnTabStripCommand="MenuSpareItemList_TabStripCommand">
                            </eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareItemList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareItemList_NeedDataSource" OnItemDataBound="gvSpareItemList_ItemDataBound1">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDPARTID,FLDORDERID,FLDVESSELID,FLDADDRESSCODE" TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Form No">
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Title">
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Item Number">
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vendor">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVendorName" runat="server" CommandArgument='<%# Bind("FLDADDRESSCODE") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Requested Qty">
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblReqQty" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")%>'></telerik:RadLabel>
                                    </ItemTemplate>
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

    </form>
</body>
</html>
