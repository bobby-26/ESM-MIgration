<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealOfficeInventory.aspx.cs" Inherits="InspectionSealOfficeInventory" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seal Office Inventory</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealInventory" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
            <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            </div>
            <div id="divFind" runat="server">
                <table id="tblFind" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucSealType" Width="200px" runat="server" AutoPostBack="true" CssClass="input" AppendDataBoundItems="true" QuickTypeCode="87" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSeal" runat="server" OnTabStripCommand="MenuSeal_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvSealInventory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSealInventory_RowDataBound" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSealInventory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
               
                    OnItemDataBound="gvSealInventory_ItemDataBound"
                    OnNeedDataSource="gvSealInventory_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="150px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblLocationHeader" runat="server"> Location</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblLocationid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSTOREITEMLOCATIONID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStoreItemId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSTOREITEMID"]%>'></telerik:RadLabel>
                                <%# ((DataRowView)Container.DataItem)["FLDLOCATIONNAME"]%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="80px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblNumberHeader" runat="server"> Number</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDNUMBER"]%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="150px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblNameHeader" runat="server">Name</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDNAME"]%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="80px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblPurchasedHeader" runat="server">Purchased</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%--<%# DataBinder.Eval(Container, "DataItem.FLDPURCHASEDQTY", "{0:n0}")%>--%>
                                <asp:LinkButton ID="lnkPurchasedQty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPURCHASEDQTY"]%>'></asp:LinkButton>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="80px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblIssuedHeader" runat="server">Issued</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <asp:LinkButton ID="lnkIssuedQty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDQTY"]%>'></asp:LinkButton>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left" width="80px"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblInStockHeader" runat="server">In Stock</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDQUANTITY", "{0:n0}")%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            
        </div>

    </form>
</body>
</html>
